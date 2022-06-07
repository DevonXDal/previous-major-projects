use std::collections::HashMap;
use std::fs;
use std::path::{Path, PathBuf};
use std::time::{SystemTime, UNIX_EPOCH};

use crate::block::Blocks::Block;
use crate::disk::{Disk, self};
use crate::disk_driver::DeviceDriver::DiskDriver;
use crate::disk_driver_kit::driver_disk_kit::{CurrentDriverSelected, DriverDiskKit};
use crate::inode;
use crate::inode::inode_content::{Inode, FileType};
use crate::directory::Directory;
use crate::json_device_driver::JsonDriver::JsonDiskDriver;
use crate::super_block::SuperBlock;
use crate::xml_device_driver::XmlDriver::XmlDiskDriver;


/// The main struct that everything else can be accessed through.
pub struct FileSystem {
    // https://users.rust-lang.org/t/using-any-struct-that-implements-a-trait-in-another-struct/13474 - Generics for traits
    pub disk: Disk, // The disk this filesystem is associated with  
    pub superblock: SuperBlock, // The file systems block 0 
    pub root: Directory, // The root directory of this filesystem
    pub mounted: bool, // Whether the filesystem is mounted
}

impl FileSystem {

    /// Constructs a new file system object and returns it for the shell to use. Uses default values everywhere possible. 
    pub fn new() -> FileSystem {
        let default_inode = Inode {
            id: u16::MAX,
            file_type: inode::inode_content::FileType::Free,
            start_block: u32::MAX,
            size: 0,
            c_time: 0,
        };
    
        FileSystem {
            disk: Disk {
                driver: DriverDiskKit {
                    xml_device_driver: XmlDiskDriver {},
                    json_device_driver: JsonDiskDriver {},
                    currently_selected_driver: CurrentDriverSelected::JSON,
                },
                contents: array_init::array_init(|_| None),
                mounted: false,
                reads_since_mount: 0,
                writes_since_mount: 0,
                disk_image_location: String::new(),
            },
            superblock: SuperBlock {
                magic_number: 0,
                block_count: 0,
                inode_count: 0,
                free_blocks: Vec::<Block>::new(),
                free_inodes: Vec::<Inode>::new(),
                inode_table: array_init::array_init(|_| Some(default_inode.clone())),
            },
            root: Directory {
                contents: HashMap::<String, u32>::new(),
            },
            mounted: false,
        }
    }

    /// Writes the file to the psuedo filesystem.
    pub fn write_file(&mut self, file_name: String, file_contents: String) -> bool {
        if !self.mounted {
            println!("No filesystem has been mounted");
            return false;
        }

        if self.root.get_contents().contains(&file_name.as_str()) {
            println!("Filename already in use..aborting");
            return false;
        }

        let mut storable_contents = FileSystem::sub_strings(file_contents.as_str(), 1024);

        let blocks_needed = storable_contents.len();
        let mut blocks_remaining = blocks_needed.clone();
        let mut first_block_id = u32::MAX;
        let mut current_block = Block {
            block_id: u32::MAX,
            next_block: -1,
            payload: String::new(),
        };
        let mut next_block = (Block {
            block_id: u32::MAX,
            next_block: -1,
            payload: String::new(),
        }, false);
        let mut list_of_blocks = Vec::<Block>::new();
        let mut first_block_collected = false;

        loop { // Get blocks filled with the data
            if blocks_remaining > 0 {
                next_block = self.superblock.get_free_block();
                if !next_block.1 {
                    self.superblock.free_blocks = self.next_free_blocks();
                }

                blocks_remaining -= 1;
            } else if next_block.0.block_id == current_block.block_id {
                current_block.payload = storable_contents.remove(0).to_string();
                current_block.next_block = -1;
                list_of_blocks.push(current_block.clone());
                break;
            }

            if first_block_collected {
                current_block.payload = storable_contents.remove(0).to_string();
                current_block.next_block = next_block.0.block_id as i64;
                list_of_blocks.push(current_block.clone());
            } else {
                first_block_id = next_block.0.block_id;
                first_block_collected = true;
            }

            current_block = next_block.0.clone();
        }

        let mut inode_result = self.superblock.get_free_inode();
        if !inode_result.1 {
            self.superblock.free_inodes = self.next_free_inodes();
        }

        let inode_id = inode_result.0.id;
        inode_result.0.file_type = FileType::File;
        inode_result.0.c_time = SystemTime::now().duration_since(UNIX_EPOCH).expect("Failed to get Unix time").as_secs();
        inode_result.0.start_block = first_block_id;
        inode_result.0.size = list_of_blocks.len() as u16;
        self.superblock.inode_table[(inode_id as usize)] = Some(inode_result.0);

        self.root.add(inode_id as u32, file_name);
        let serialized_directory = match self.disk.driver.currently_selected_driver {
            CurrentDriverSelected::JSON => JsonDiskDriver::serialize_directory(self.root.clone()),
            CurrentDriverSelected::XML => XmlDiskDriver::serialize_directory(self.root.clone()),
        };
        let root_folder_block_id = (self.superblock.inode_count / 50) + 1;

        let mut root_block = self.disk.read(root_folder_block_id).expect("Root directory could not be updated in cache");
        root_block.payload = serialized_directory;
        self.disk.write(root_block);

        loop {
            if list_of_blocks.len() == 0 {
                break;
            }

            self.disk.write(list_of_blocks.remove(0));
        }

        true
    }

    /// Reads a file from the system as a string and returns that string if the file was found
    pub fn read_file(&mut self, filename: String) -> Option<String> {
        if !self.mounted {
            println!("Cannot read file from an unmounted filesystem.");
            return None;
        }

        match self.get_filename_inode(filename) {
            Some(inode) => {
                let mut current_block_id: i64 = inode.start_block as i64;
                let mut current_file_string = String::new();

                loop {
                    let mut current_block = self.disk.read(current_block_id as u32).expect("Reading a block for an inode failed");

                    current_file_string = format!("{}{}", current_file_string, current_block.payload);
                    current_block_id = current_block.next_block;

                    if current_block_id == -1 {
                        break;
                    }
                }

                Some(current_file_string)
            },
            None => None,
        }
    }

    /// Returns the corrosponding inode if the filename is found in the directory
    /// The inode is read-only
    pub fn get_filename_inode(&self, filename: String) -> &Option<Inode> {
        let inode_num = self.root.get_inode_number(filename);
        if inode_num == -1 {
            return &None;
        }

        &self.superblock.inode_table[inode_num as usize]
    }

    /// Creates a seemingly blank file with a specified number of empty lines for proper formatting. Does not format the file.
    /// param path_of_new_file: Just the path, not the name of the new file to be created, will overwrite any files at ./disk-image.json when created
    /// param file_size: The size of the file to use as a disk image in KB (specify 100-1000 inclusive)
    /// return: Did the file get created successfully?
    pub fn create(path_of_new_file: &str, file_size: u16, file_type: CurrentDriverSelected) -> bool {
        // https://users.rust-lang.org/t/what-is-right-ways-to-concat-strings/3780 - Proper string concatenation
        let mut new_file_lines = String::new();
        let new_line = String::from("\r\n");

        for _ in 1..=file_size {
            new_file_lines = format!("{}{}", new_file_lines, new_line);
        }
        let full_file_path: PathBuf = match file_type {
            CurrentDriverSelected::JSON => {
                Path::new(path_of_new_file).join("disk-image.json")
            },
            CurrentDriverSelected::XML => {
                Path::new(path_of_new_file).join("disk-image.xml")
            }
        } ;

        let mut final_path_version = "";

        match full_file_path.to_str() {
            Some(path) => {
                final_path_version = path
            },
            None => {
                println!("A pathing issue has occured when trying to create the file")
            }
        }
        
        println!("{}", final_path_version);

        let did_it_work = fs::write(final_path_version, new_file_lines);

        match did_it_work {
            Ok(_) => { 
                true
            },
            Err(e) => {
                println!("Exception occured when creating disk image: {}", e);
                false
            }
        }
    }

    /// Deletes the file from the disk (soft delete, it does not wipe the data, just that it is used)
    pub fn delete_file(&mut self, filename: String) -> bool {
        if !self.mounted {
            println!("Cannot delete a file from an unmounted filesystem.");
            return false;
        }

        if filename.ends_with("..") || filename.ends_with(".") {
            println!("Cannot delete the root directory.");
            return false;
        }

        match &self.get_filename_inode(filename.to_string()).clone() {
            Some(inode) => {
                let inode_to_clear = inode.id.clone();
                let mut inode_from_table = self.superblock.inode_table[inode_to_clear.clone() as usize].clone().unwrap();
                inode_from_table.file_type = FileType::Free;
                inode_from_table.size = 0;
                inode_from_table.start_block = u32::MAX;

                self.superblock.update_inode(inode_from_table);

                let mut next_block_id_temp = inode.start_block as i64;

                loop {
                    let mut current_block = self.disk.read(next_block_id_temp as u32).expect("Reading a block for an inode failed");
                    next_block_id_temp = current_block.next_block;

                    current_block.next_block = -2;
                    self.disk.write(current_block);

                    if next_block_id_temp == -1 {
                        break;
                    }
                }

                self.root.remove(filename.to_string());
                true
            },
            None => false,
        }
    }

    /// Formats the previously created disk-image file with filesystem data so it can be mounted.
    pub fn format(path_and_filename: String) -> bool {
        let file_data = FileSystem::try_read_file(path_and_filename.clone());

        if !file_data.0 {
            return false;
        } 

        let file_string = file_data.1;
        let driver_selected = file_data.2;


        let total_file_size = file_string.matches("\r\n").count();
        let mut size_of_file_left = total_file_size.clone();

        let inode_block_count = (total_file_size as f64 * 0.1) as usize;
        size_of_file_left = size_of_file_left - (inode_block_count + 2); // The extra two is for the / directory and superblock
        let total_inodes = inode_block_count * 50;
        let total_blocks = size_of_file_left + 1;

        let mut superblock_block = Block { 
            block_id: 0, //It is the first block on the system
            next_block: -1, // Superblock does not reference any other blocks
            payload: format!("0-{}-{}", total_inodes, total_blocks), // Will be split on - when read, size_of_file_left +1 adds in the root folder
        };

        let mut root_directory_inode = Inode::create_new(0, FileType::Directory, (inode_block_count + 1) as u32, 1);

        let mut current_inode = 1;
        let mut current_block =  1;
        let mut root_directory_inserted = false;
        let mut blocks_finished = Vec::<Block>::new();

        blocks_finished.push(superblock_block);
        
        for block_i in 0..inode_block_count { // This for loop starts the process of adding the blocks with inodes in them
            let mut current_inode_table_block_string = String::new();
            let mut has_first_inode = false;

            for __ in 0..=50 {
                let mut new_inode = Inode::create_new(current_inode, FileType::Free, u32::MAX, 0);
                let mut new_serialized_inode = String::new();
                
                if !root_directory_inserted {
                    new_inode = root_directory_inode.clone();
                    root_directory_inserted = true;
                }

                match driver_selected {
                    CurrentDriverSelected::JSON => { 
                        new_serialized_inode = JsonDiskDriver::serialize_inode(new_inode);
                    },
                    CurrentDriverSelected::XML => {
                        new_serialized_inode = XmlDiskDriver::serialize_inode(new_inode);
                    }
                }

                if !has_first_inode {
                    has_first_inode = true;
                    current_inode_table_block_string = format!("{}{}", current_inode_table_block_string, new_serialized_inode);
                } else {
                    current_inode_table_block_string = format!("{}-{}", current_inode_table_block_string, new_serialized_inode); // A dash is used to seperate entries
                    current_inode += 1;
                }
                
            }

            let mut block = Block { 
                block_id: current_block,
                next_block: if block_i + 1 == inode_block_count { -1 } else { current_block as i64 + 1}, 
                payload: current_inode_table_block_string, 
            };

            current_block += 1;

            blocks_finished.push(block);
        }

        root_directory_inserted = false;
        for _ in 0..size_of_file_left {
            if root_directory_inserted {
                let mut data_block = Block { 
                    block_id: current_block,
                    next_block: -2, 
                    payload: String::new(), 
                };

                blocks_finished.push(data_block);
            } else {
                root_directory_inserted = true;
                let mut serialized_directory = String::new();

                let root_directory = Directory::new(0, 0);

                match driver_selected {
                    CurrentDriverSelected::JSON => { 
                        serialized_directory = JsonDiskDriver::serialize_directory(root_directory);
                    },
                    CurrentDriverSelected::XML => {
                        serialized_directory = XmlDiskDriver::serialize_directory(root_directory);
                    }
                }

                let root_directory_block = Block { 
                    block_id: current_block,
                    next_block: -1, 
                    payload: serialized_directory, 
                };

                blocks_finished.push(root_directory_block);
            }

            current_block += 1;
        }

        let total_number_of_blocks_to_process = blocks_finished.len();
        let mut formatted_file = String::new();
        for _ in 0..total_number_of_blocks_to_process {
            match driver_selected {
                CurrentDriverSelected::JSON => { 
                    formatted_file = format!("{}{}\r\n", formatted_file, JsonDiskDriver::serialize_block(blocks_finished.remove(0)));
                },
                CurrentDriverSelected::XML => {
                    formatted_file = format!("{}{}\r\n", formatted_file, XmlDiskDriver::serialize_block(blocks_finished.remove(0)));
                }
            } 
        }

        let mut did_it_write_the_file = fs::write(path_and_filename.clone(), formatted_file);
        match did_it_write_the_file {
            Ok(_) => { 
                println!("Formatting was completed successfully");
                true
            },
            Err(e) => {
                println!("Failed to write formatted file: {}", e);
                false
            }
        }
    }

    /// Mounts the specified disk image file and sets up the filesystem for access.
    pub fn mount(&mut self, path_and_filename: String) -> bool {
        if !self.disk.open(path_and_filename) {
            return false;
        }

        // This all assumes that since the disk image was just read that unwrapping these blocks is safe
        let mut superblock_block = self.disk.read(0).unwrap(); // Superblock resides on block 0
        let mut superblock_bts = superblock_block.payload.split("-");
        let superblock_big_three: (u32, u32, u32) = (superblock_bts.next().expect("Invalid magic number").parse::<u32>().unwrap(), 
                                                    superblock_bts.next().expect("Invalid inode amount").parse::<u32>().unwrap(), 
                                                    superblock_bts.next().expect("Invalid block amount").parse::<u32>().unwrap());
        
        let mut inode_table_blocks = Vec::<Block>::new();

        let mut current_block = 1;
        let mut last_inode_block = 0;
        loop { // Get the inode tables
            let mut possible_inode_block = self.disk.read(current_block).expect("Data loss when mounting inode blocks");
            
            let mut last_block = false;
            if possible_inode_block.next_block == -1 { // If no inode block comes next
                last_block = true;
                last_inode_block = possible_inode_block.block_id;
            }

            inode_table_blocks.push(possible_inode_block);
            current_block += 1; 

            if last_block {
                break;
            }
        }

        let mut inodes = Vec::<Inode>::new(); // Inodes for the inode table
        let mut free_inodes = Vec::<Inode>::new(); // 50 max inodes that are free for use
        let mut free_blocks = Vec::<Block>::new(); // 50 max blocks that are free for use
        loop { // Get the inode table built
            let mut inode_block = inode_table_blocks.remove(0);
            let mut serialized_inode_batch = inode_block.payload.split("-"); // - was used as a divider

            loop { //Handle individual inodes
                match serialized_inode_batch.next() {
                    Some(serialized_inode) => {
                        let mut inode_from_batch = match self.disk.driver.currently_selected_driver {
                            CurrentDriverSelected::JSON => JsonDiskDriver::deserialize_inode(serialized_inode.to_string()),
                            CurrentDriverSelected::XML => XmlDiskDriver::deserialize_inode(serialized_inode.to_string()),
                        };

                        if free_inodes.len() < 50 {
                            match inode_from_batch.file_type {
                                FileType::Free => free_inodes.push(inode_from_batch.clone()),
                                FileType::File => {},
                                FileType::Directory => {},
                            }
                        }

                        inodes.push(inode_from_batch);
                        
                    },
                    None => break,
                }
            }
            

            if last_inode_block == inode_block.block_id {
                break;
            }
        }

        let mut built_root_directory = false;
        loop { // Collect 50 free blocks if possible and get the root directory (exits if blocks available to scan run out)
            let possible_block = self.disk.read(current_block);
            match possible_block {
                Some(block) => {
                    if !built_root_directory { // Naive, assumes root directory is the first

                        let reformatted_directory_serialization = block.payload.to_string().replace(r#"\""#, "\"");
                        let root_directory = match self.disk.driver.currently_selected_driver {
                            CurrentDriverSelected::JSON => JsonDiskDriver::deserialize_directory(reformatted_directory_serialization),
                            CurrentDriverSelected::XML => XmlDiskDriver::deserialize_directory(reformatted_directory_serialization),
                        };

                        self.root = root_directory; // Root directory is mounted here
                        built_root_directory = true;
                    } else if block.next_block == -2 { // Block is free
                        free_blocks.push(block);
                    }
                },
                None => break, // Stop if no more blocks exist to read
            }

            current_block += 1;
            if free_blocks.len() >= 50 {
                break;
            }
        }

        self.superblock.magic_number = superblock_big_three.0; // Superblock assignments
        self.superblock.inode_count = superblock_big_three.1;
        self.superblock.block_count = superblock_big_three.2;
        self.superblock.free_blocks = free_blocks;
        self.superblock.free_inodes = free_inodes;
        
        loop {
            if inodes.len() == 0 {
                break;
            }

            let this_inode = inodes.remove(0);
            self.superblock.inode_table[this_inode.id as usize] = Some(this_inode.clone());
        }

        self.mounted = true; // Filesystem assignments
        

        true
    }

    /// Attempts to unmount the system and save changes to the disk image file.
    pub fn unmount(&mut self) -> bool {
        if !self.mounted {
            println!("System is not mounted already");
        }

        if self.disk.close(&self.superblock.inode_table, self.superblock.inode_count.clone() as usize) {
            self.disk.mounted = false;
            self.mounted = false;

            true
        } else {
            false
        }
    }

    /// Returns if it was successful, a string with file contents, and the driver to use. Only use the provided bool if unsuccessful.
    pub fn try_read_file(path_and_filename: String) -> (bool, String, CurrentDriverSelected) {
        println!("Reading file at: {}", path_and_filename);

        let mut driver_selected = CurrentDriverSelected::JSON;
        if !(path_and_filename.ends_with(".xml") || path_and_filename.ends_with(".json")) {
            println!("Invalid file format");
            ()
        } else if path_and_filename.ends_with(".xml") {
            driver_selected = CurrentDriverSelected::XML;
        }

        let mut possible_file = fs::read_to_string(path_and_filename.clone());
        let mut file_string = String::new();

        match possible_file {
            Ok(file_as_string) => { 
                (true, file_as_string, driver_selected)
            },
            Err(e) => {
                println!("File could not be read successfully");
                (false, String::new(), driver_selected)
            }
        }
    }

    /// Attempts to read any file it can and return it as a string. 
    pub fn try_read_all_files(path_and_filename: String) -> (bool, String) {
        println!("Reading file at: {}", path_and_filename);

        let mut possible_file = fs::read_to_string(path_and_filename.clone());
        let mut file_string = String::new();

        match possible_file {
            Ok(file_as_string) => { 
                (true, file_as_string)
            },
            Err(e) => {
                println!("File could not be read successfully");
                (false, String::new())
            }
        }
    }

    // Prints diagnostics about the filesystem
    pub fn diagnostics(&mut self) {
        if !self.mounted {
            println!("Cannot receive diagnostics from an unmounted filesystem.");
            return;
        }

        let blocks = self.count_blocks(); // Free, Used

        println!("The Magic Number Validity: {}", if self.superblock.magic_number == 0 {"Valid"} else {"Invalid"}); 
        println!("Disk Reads: {}, Disk Writes: {}", self.disk.reads_since_mount, self.disk.writes_since_mount);
        println!("Total Inodes: {}, Used Inodes: {}, Free Inodes {}", self.superblock.inode_count, self.root.get_contents().len() + 1, self.superblock.inode_count - (self.root.get_contents().len() + 1) as u32);
        println!("Total Blocks (All Blocks): {}, Used Blocks: {}, Free Blocks: {}", (blocks.1 + blocks.0), blocks.1, blocks.0);
    }

    /// Grabs a list of files in the current directory
    pub fn list(&mut self) -> Option<[&str; 255]> {
        if !self.mounted {
            println!("Cannot list files from an unmounted filesystem.");
            return None;
        }
        Some(self.root.get_contents())
    }

    /// Counts the number of (free, used) data blocks
    pub fn count_blocks(&mut self) -> (u32, u32) {
        let mut free_blocks: u32 = 0;
        let mut used_blocks: u32 = 0;
        let mut current_block =  1;
        loop { // Collect 50 free blocks if possible and get the root directory (exits if blocks available to scan run out)
            let possible_block = self.disk.read(current_block);
            match possible_block {
                Some(block) => {
                    if block.next_block == -2 { // Block is free
                        free_blocks += 1;
                    } else {
                        used_blocks += 1;
                    }
                },
                None => break, // Stop if no more blocks exist to read
            }

            current_block += 1;
        }

        (free_blocks, used_blocks)
    }

    /// Gathers the next 50 free blocks
    pub fn next_free_blocks(&mut self) -> Vec<Block> {
        let mut next_blocks = Vec::<Block>::new();
        let mut current_block =  1;
        loop { // Collect 50 free blocks if possible and get the root directory (exits if blocks available to scan run out)
            let possible_block = self.disk.read(current_block);
            match possible_block {
                Some(block) => {
                    if block.next_block == -2 { // Block is free
                        next_blocks.push(block);
                    }
                },
                None => break, // Stop if no more blocks exist to read
            }

            current_block += 1;
            if next_blocks.len() >= 50 {
                break;
            }
        }

        next_blocks
    }

    /// Gathers the next 50 free inodes
    pub fn next_free_inodes(&mut self) -> Vec<Inode> {
        let mut inode_table_blocks = Vec::<Block>::new();
        let mut next_inodes = Vec::<Inode>::new();
        let mut current_block =  1;
        let mut last_inode_block = 0;

        loop { // Get the inode tables
            let mut possible_inode_block = self.disk.read(current_block).expect("Data loss when mounting inode blocks");
            
            let mut last_block = false;
            if possible_inode_block.next_block == -1 { // If no inode block comes next
                last_block = true;
                last_inode_block = possible_inode_block.block_id;
            }

            inode_table_blocks.push(possible_inode_block);
            current_block += 1; 

            if last_block {
                break;
            }
        }

        loop { // Get the inode table built
            let mut inode_block = inode_table_blocks.remove(0);
            let mut serialized_inode_batch = inode_block.payload.split("-"); // - was used as a divider

            loop { //Handle individual inodes
                match serialized_inode_batch.next() {
                    Some(serialized_inode) => {
                        let mut inode_from_batch = match self.disk.driver.currently_selected_driver {
                            CurrentDriverSelected::JSON => JsonDiskDriver::deserialize_inode(serialized_inode.to_string()),
                            CurrentDriverSelected::XML => XmlDiskDriver::deserialize_inode(serialized_inode.to_string()),
                        };

                        if next_inodes.len() < 50 {
                            match inode_from_batch.file_type {
                                FileType::Free => next_inodes.push(inode_from_batch.clone()),
                                FileType::File => {},
                                FileType::Directory => {},
                            }
                        }           
                    },
                    None => break,
                }
            }
            

            if last_inode_block == inode_block.block_id {
                break;
            }
        }

        next_inodes
    }
    
    /// This is NOT mine. This was from juggle-tux at: https://users.rust-lang.org/t/solved-how-to-split-string-into-multiple-sub-strings-with-given-length/10542/8
    /// Used this because I did not know how to deal with unicode characters properly
    fn sub_strings(string: &str, sub_len: usize) -> Vec<&str> {
        let mut subs = Vec::with_capacity(string.len() / sub_len);
        let mut iter = string.chars();
        let mut pos = 0;
    
        while pos < string.len() {
            let mut len = 0;
            for ch in iter.by_ref().take(sub_len) {
                len += ch.len_utf8();
            }
            subs.push(&string[pos..pos + len]);
            pos += len;
        }
        subs
    }
}