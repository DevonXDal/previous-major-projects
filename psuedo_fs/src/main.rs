pub mod super_block;
pub mod block;
pub mod directory;
pub mod disk_driver;
pub mod disk;
pub mod file_system;
pub mod xml_device_driver;
pub mod inode;
pub mod json_device_driver;
pub mod disk_driver_kit;

extern crate chrono;
extern crate quick_xml;

use std::{process::{Child, Stdio, Command}, io::{stdin, stdout, Write}, path::Path, env, collections::HashMap, str::SplitWhitespace, convert::TryFrom, fs};
use block::Blocks::Block;
use directory::Directory;
use disk::Disk;
use disk_driver_kit::driver_disk_kit::{CurrentDriverSelected, DriverDiskKit};
use inode::inode_content::Inode;
use json_device_driver::JsonDriver::{JsonDiskDriver, self};
use disk_driver::DeviceDriver::{self, DiskDriver};
use super_block::SuperBlock;
use file_system::FileSystem;
use regex::Regex;
use xml_device_driver::XmlDriver::XmlDiskDriver;


fn main(){
    let mut variables_used = HashMap::<String, String>::new();
    let mut mounted_filesystem: FileSystem = FileSystem::new(); // The filesystem needs to be mounted before usage
    let dir = JsonDiskDriver::deserialize_directory("{\"contents\":{\".\":0,\"..\":0}}".to_string());

    loop {
        print!("DevShell# ");
        stdout().flush(); // Clean out the output, input can get stuck otherwise and not get printed correctly.

        let mut input = String::new();
        let mut is_variable_declaration = false;

        // Adds a newline at the end that must be removed.
        stdin().read_line(&mut input).unwrap(); 
        input = input.trim().to_string(); // Trim the string in advance so it is not done repeatedly

        if input.is_empty() { // If no input is provided, just ignore it and accept another command.
            continue;
        }

        {
            // https://stackoverflow.com/questions/19587328/extracting-key-value-pairs-with-regex-dot-group-not-cooperating-with
            let re = Regex::new(r#"(\w+)=([^\s]+)"#).unwrap();

            let possible_variable_declaration = input.to_string(); // Done for readability purposes and borrow errors that are easier to solve this way

            if re.is_match(&possible_variable_declaration) /*possible_variable_declaration.matches("=").count() == 1*/ {
                is_variable_declaration = true;
            }


        }


        if !is_variable_declaration { // It is a command or printed variable
            //Use any variables
            {
                let mut skip_command_section = false;
                let re = Regex::new(r#"\$(\w+)"#).unwrap();

                let input_copy = input.to_string();
                let mut space_seperated_parts = input_copy.split(" ").clone();

                let num_counted = space_seperated_parts.clone().count() as usize;
                let iteration = 1..=num_counted;
                for _ in iteration {
                    if let Some(part) = space_seperated_parts.next() { 
                        if re.is_match(&part.trim()) && !part.is_empty() {
                            let mut key = &part.trim().replace("$", "");

                            if variables_used.contains_key(&key.to_string()) {
                                input = input.clone().replace(part, variables_used.get(&key.to_string()).unwrap());
                            }

                            if num_counted == 1 {
                                println!("{}", input);
                                skip_command_section = true;
                            }
                        }
                     }

                }

                if skip_command_section {
                    continue;
                }
            }



            // It is set to be peekable so that it is known when the last command is reached.
            // The input is trimmed to remove extra space, particularly the new line created by read line.
            let mut commands = input.split(" | ").peekable();
            let mut previous_command = None;

            while let Some(command) = commands.next()  { // NOTE: Interesting way to do a while loop

                let mut parts = command.trim().split_whitespace(); // Arguments of the commands
                let command = parts.next().unwrap(); // Just the command
                let mut args = parts; // Each argument provided

                match command {
                    "cd" => { // Does not create new directories from cding into one that does not exist, need to change.
                        let new_dir = args.peekable().peek()
                            .map_or("/", |x| *x); // Sets / as a default directory if no argument is provided
                        let root = Path::new(new_dir);
                        if let Err(e) = env::set_current_dir(&root) {
                            eprintln!("{}", e);
                        }

                        previous_command = None;
                    },
                    "cat" => { 
                        cat_command(args.next(), &mut args, &mut mounted_filesystem);
                    },
                    "copyin" => { // Create a file at the specified location with the specified size 'create {location} {size}'
                        copyin_command(args.next(), args.next(), &mut args, &mut mounted_filesystem);
                    },
                    "copyout" => { // Create a file at the specified location with the specified size 'create {location} {size}'
                        copyout_command(args.next(), args.next(), &mut args, &mut mounted_filesystem);
                    },
                    "create" => { // Create a file at the specified location with the specified size 'create {location} {size}'
                        create_command(args.next(), args.next(), args.next(), &mut args);
                    },
                    "debug" => { 
                        debug_command(&mut mounted_filesystem);
                    },
                    "delete" => { 
                        delete_command(args.next(), &mut args, &mut mounted_filesystem);
                    },
                    "format" => { 
                        format_command(args.next(), &mut args);
                    },
                    "help" => { 
                        help();
                    },
                    "ls" => { 
                        ls_command(&mut mounted_filesystem);
                    },
                    "mount" => { 
                        mount_command(args.next(), &mut args, &mut mounted_filesystem);
                    },
                    "unmount" => { 
                        unmount_command(&mut mounted_filesystem);
                    },
                    "exit" => return, 
                        command => {
                            let stdin = previous_command
                                .map_or(
                                    Stdio::inherit(),
                                    |output: Child| Stdio::from(output.stdout.unwrap())
                                );

                            let stdout = if commands.peek().is_some() {
                                // Send output to the next command since it is being piped.
                                Stdio::piped()
                            } else {
                                // It is not being piped to another command.
                                Stdio::inherit()
                            };

                            let output = Command::new(command)
                                .args(args)
                                .stdin(stdin)
                                .stdout(stdout)
                                .spawn();

                            match output {
                                Ok(output) => { previous_command = Some(output); },
                                Err(e) => {
                                    previous_command = None;
                                    eprintln!("{}", e);
                                },
                            };
                        }
                }

                
            }

            if let Some(mut final_command) = previous_command {
                // block until the final command has finished
                final_command.wait();
            }
        } else {
            let mut variable_key_and_value = input.split("=").peekable();

            let mut key = variable_key_and_value.next().unwrap();
            let mut value = variable_key_and_value.next().unwrap();

            variables_used.insert(key.to_string(), value.to_string());

        }
        

        println!(""); // Add extra space between input and output

    }
}

// ---- Commands ----

fn cat_command(possible_location: Option<&str>, extra_arguments: &mut SplitWhitespace, filesystem: &mut FileSystem) {
    match possible_location { // Argument one check
        Some(location) => {            
            if extra_arguments.count() > 0 {
                cat_help(extra_arguments.count() as u8 + 2);
            } else {
                match filesystem.read_file(location.to_string()) {
                    Some(file_string) => println!("{}", file_string),
                    None => println!("Could not successfully load the file to print it."),
                }
            }
        },
        None => {
            cat_help(0);
        }
    };
}

fn copyin_command(possible_location: Option<&str>, possible_filename: Option<&str>, extra_arguments: &mut SplitWhitespace, filesystem: &mut FileSystem) {
    match possible_location {
        Some(location) => {
            match possible_filename {
                Some(filename) => {
                    if extra_arguments.count() > 0 {
                        create_help((extra_arguments.count() + 2) as u8);
                    } else {
                        let response = FileSystem::try_read_all_files(location.to_string());

                        if !response.0 {
                            println!("Aborting due to file reading issues");
                        }
                        let re = Regex::new(r#"[A-Za-z0-9-_]+\.?[A-Za-z0-9]{0,5}"#).unwrap();

                        if !re.is_match(filename.clone()) {
                            println!("This cannot be used as a filename..Aborting");
                        }

                        if filesystem.write_file(filename.to_string(), response.1) {
                            println!("File successfully written to the system")
                        }
                    }
                },
                None => copyin_help(1),
            }
        },
        None => copyin_help(0),
    }
}

fn copyout_command(possible_filename: Option<&str>, possible_location: Option<&str>, extra_arguments: &mut SplitWhitespace, filesystem: &mut FileSystem) {
    match possible_filename {
        Some(filename) => {
            match possible_location {
                Some(location) => {
            
                    if extra_arguments.count() > 0 {
                        copyout_help((extra_arguments.count() + 2) as u8);
                    } else {
                        let response = filesystem.read_file(filename.to_string());

                        match response {
                            Some(file_string) => {
                                let write_result = fs::write(location, file_string.to_string());

                                match write_result {
                                    Ok(_) => println!("File copied back to host system successfully"),
                                    Err(__) => println!("Could not write file on host system"),
                                }
                            },
                            None => return,
                        }
                    }
                },
                None => copyout_help(1),
            }
        },
        None => copyout_help(0),
    }
}

fn create_command(possible_location: Option<&str>, possible_size: Option<&str>, possible_file_type: Option<&str>, extra_arguments: &mut SplitWhitespace) {
     match possible_location { // Argument one check
         Some(location) => { 
            match possible_size { // Argument two check
                Some(size) => {
                    // https://stackoverflow.com/questions/27043268/convert-a-string-to-int 
                    let possible_value = str::parse::<u16>(size);

                    match possible_value { // Attempted parse check
                        Ok(value) => { 
                            let mut selected_driver = CurrentDriverSelected::JSON;

                            match possible_file_type {
                                Some(file_type) => {
                                    if file_type.contains("xml") {
                                        selected_driver = CurrentDriverSelected::XML;
                                    } else if file_type.contains("json") {
                                        // Nothing to do here
                                    } else {
                                        create_help(0)
                                    }

                                    let mut num_of_extra_args: u8 = 0;
                                    loop {
                                        match extra_arguments.next() {
                                            Some(_) => num_of_extra_args += 1,
                                            None => break,
                                        }
                                    }

                                    if num_of_extra_args > 0 {
                                        create_help(num_of_extra_args + 2);
                                    } else if value < 100 || value > 1000 {
                                        create_help(0);
                                    } else {
                                        FileSystem::create(location, value, selected_driver);
                                    }
                                },
                                None => {
                                    create_help(2);
                                }
                            }
                        },
                        Err(e) => {
                            create_help(0);
                        }
                    }
                },
                None => {
                    create_help(1);
                }
            }
         },
         None => {
            create_help(0);
         }
     };
}

fn debug_command(filesystem: &mut FileSystem) {
    filesystem.diagnostics();
}

fn delete_command(possible_location: Option<&str>, extra_arguments: &mut SplitWhitespace, filesystem: &mut FileSystem) {
    match possible_location { // Argument one check
        Some(location) => {            
            if extra_arguments.count() > 0 {
                delete_help(extra_arguments.count() as u8 + 2);
            } else {
                match filesystem.delete_file(location.to_string()) {
                    true => {},
                    false => println!("Failed to delete the file")
                }
            }
        },
        None => {
            delete_help(0);
        }
    };
}

fn format_command(possible_location: Option<&str>, extra_arguments: &mut SplitWhitespace) {
    match possible_location { // Argument one check
        Some(location) => {            
            if extra_arguments.count() > 0 {
                format_help(extra_arguments.count() as u8 + 2);
            } else {
                FileSystem::format(location.to_string(),);
            }
        },
        None => {
            format_help(0);
        }
    };
}

fn ls_command(filesystem: &mut FileSystem) {
    const EMPTY_STRING: &str = "";
    let possible_files = filesystem.list();

    let files_available = match possible_files {
        Some(files) => files,
        None => {
            [EMPTY_STRING; 255]
        },
    };

    if files_available[0].is_empty() {
        return;
    }

    for i in 0..files_available.len() {
        if files_available[i].is_empty() {
            continue;
        }
        println!("{}", files_available[i]);
    }
}

fn mount_command(possible_location: Option<&str>, extra_arguments: &mut SplitWhitespace, filesystem: &mut FileSystem) {
    match possible_location { // Argument one check
        Some(location) => {            
            if extra_arguments.count() > 0 {
                mount_help(extra_arguments.count() as u8 + 2);
            } else {
                 if !filesystem.mount(location.to_string()) {
                     println!("The filesystem on the disk image is missing or corrupted.");
                 } else {
                     println!("The filesystem is now mounted and ready for use.");
                 }
            }
        },
        None => {
            mount_help(0);
        }
    };
}

fn unmount_command(filesystem: &mut FileSystem) {
    filesystem.unmount();
}

// ---- Help Functions ----

fn cat_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The cat command takes a file on the psuedo-fs system and prints its contents to the screen.\n");
        println!("Expected Usage: 'cat [filename on psuedo-fs]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 1, Found {}", num_of_arguments);
    }
}

fn copyin_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The copyin command copies a file from the host machine and stores it on the psuedo-fs system.\n");
        println!("Expected Usage: 'copyin [file path and filename on host machine] [a valid filename with no directory pathing]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 2, Found {}", num_of_arguments);
    }
}

fn copyout_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The copyout command copies a file from the psuedo-fs system and stores it on the host machine.\n");
        println!("Expected Usage: 'copyout [filename on psuedo-fs] [file path and filename on host machine] '");
    } else {
        println!("Unexpected Number of Arguments: Expected 2, Found {}", num_of_arguments);
    }
}

fn create_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The create command creates a new disk image file on the host filesystem to store a possible filesystem.");
        println!("The create command needs a location to store the disk image file on the host system and can accept file sizes from 100-1000 KB");
        println!("The create command also needs a file type, either 'xml' or 'json' to be specified. This will choose the serization used\n");
        println!("Expected Usage: 'create [file path to store disk file] [size of disk image file from 100-1000 (KB)] [xml OR json]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 3, Found {}", num_of_arguments);
    }
}

fn delete_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The delete command softly removes the file.\n");
        println!("Expected Usage: 'delete [filename on psuedo-fs]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 1, Found {}", num_of_arguments);
    }
}

fn format_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The format command takes a file created by the 'create' command on the host machine and formats it with a filesystem.\n");
        println!("Expected Usage: 'format [file path and filename to format with psuedo-fs]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 1, Found {}", num_of_arguments);
    }
}

fn mount_help(num_of_arguments: u8) {
    if num_of_arguments == 0 {
        println!("The mount command takes a file formatted by the 'format' command on the host machine and mounts the files so they can be accessed.\n");
        println!("Expected Usage: 'mount [file path and filename to mount to the psuedo-fs system]'");
    } else {
        println!("Unexpected Number of Arguments: Expected 1, Found {}", num_of_arguments);
    }
}

fn help() {
    println!("The DevShell features both commands and variables. It is rather simplistic but gets simple jobs done fast.\n");
    
    println!("Variables:");
    println!("To assign a runtime variable use: '[variable name]=[sequence of characters to store as the variable's value]'");
    println!("To display a variable's value use: '$[variable name]'");
    println!("When using a command the value will be translated before the command runs, allowing similar commands to be done more quickly.\n");

    println!("Commands:");
    println!("The structure of commands is '[command] [argument] [argument] [more arguments...]");
    println!("Typing just the command for most will provide a description of what they do\n");

    println!("Available commands:");
    println!("cat: (Has help menu) - Prints a file's contents to the screen.");
    println!("copyin: (Has help menu) - Copies a file from the host system to psuedo-fs");
    println!("copyout: (Has help menu) - Copies a file from psuedo-fs to the host machine.");
    println!("create: (Has help menu) - Creates a disk image on the host system.");
    println!("debug: Prints diagnostics of the mounted filesystem");
    println!("delete: (Has help menu) - Deletes a file from the psuedo-fs");
    println!("exit: Will shutdown the DevShell and PsuedoFS. Warning all unsaved changes will be lost, use unmount to save changes.");
    println!("format: (Has help menu) - Formats a disk image file with the filesystem so it can be mounted.");
    println!("help: Provides help with using the system.");
    println!("ls: Lists the files in the main directory (/)");
    println!("mount: (Has help menu) - Mounts the disk image with the filesystem so it can be accessed.");
    println!("unmount: Saves and unmounts the psuedo-fs formatted filesystem.")
    
}

// https://stackoverflow.com/questions/50072055/converting-unix-timestamp-to-readable-time-string-in-rust - For later use