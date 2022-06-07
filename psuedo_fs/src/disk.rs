use core::panic;
use std::fs;

use crate::{disk_driver_kit::driver_disk_kit::{DriverDiskKit, CurrentDriverSelected}, file_system::FileSystem, block::Blocks::Block, inode::inode_content::Inode};
use crate::xml_device_driver::XmlDriver::XmlDiskDriver;
use crate::json_device_driver::JsonDriver::JsonDiskDriver;
use crate::disk_driver::DeviceDriver::DiskDriver;


/// Represents the drivers and simulated disk. It tracks reads and writes while mounted and handles reading and writing to persistent storage.
pub struct Disk {
    pub driver: DriverDiskKit, // The driver to handle XML or JSON serialization of data
    pub contents: [Option<String>; 1024], // The disk cache that will be read to and from a disk file.
    pub mounted: bool, // Whether the disk is currently mounted
    pub reads_since_mount: u32, // The number of reads since the disk was mounted
    pub writes_since_mount: u32, // The number of writes since the disk was mounted
    pub disk_image_location: String, // The disk image location on the host machine to save to
    
}

impl Disk {
    /// Opens up either XML or JSON files acting as a disk image. 
    /// It reads this into its contents array. 
    /// It also mounts the disk if it is currently unmounted.
    /// param filename: The name of file and its location on the system.
    /// return: A boolean response indicating if the file was successfully opened and read into the disk contents array.
    pub fn open(&mut self, filename: String) -> bool {
        if self.mounted {
            return false;
        }

        let file_data = FileSystem::try_read_file(filename.clone());

        if !file_data.0 {
            return false;
        } 

        let file_string = file_data.1;
        self.driver.currently_selected_driver = file_data.2;

        let mut lines_of_file = file_string.split("\r\n");

        let mut current_block_id = 0;
        loop {
            match lines_of_file.next() {
                Some(serialized_block) => {
                    if serialized_block.is_empty() {
                        continue;
                    }
                    self.contents[current_block_id] = Some(serialized_block.to_string());
                    
                    current_block_id += 1;
                }, 
                None => { // No blocks left
                    break;
                }
            }
        }

        self.mounted = true;
        self.reads_since_mount = 0; // Should be 0 aleady, but it does not hurt to be sure.
        self.writes_since_mount = 0; 
        self.disk_image_location = filename;



        true // Endpoint if the file loads successfully
    }

    /// Closes the disk to unmount the psuedo-fs system. Saves changes to persistent storage.
    /// param all_of_the_inodes: The inode table from the superblock should be used here
    /// param inode_amount: The amount of inodes the system has available. This can also be found in the superblock
    /// return: Did the saving and closing of the psuedo-fs mount work?
    pub fn close(&mut self, all_of_the_inodes: &[Option<Inode>; 5001], inode_amount: usize) -> bool {
        let mut inode_index: usize = 0;
        for batch_i in 1..=(inode_amount / 50) { // This for loop starts the process of adding the blocks with inodes in them
            let mut inode_table_block_string = String::new();
            let mut inode_string = String::new();
            let mut inode_block = self.read(batch_i as u32).expect("Inode block not read for saving correctly");

            if batch_i == (inode_amount / 50) {
                println!("");
            }

            for inode_i in 0..50 {

                match self.driver.currently_selected_driver {
                    CurrentDriverSelected::JSON => { 
                        inode_string = JsonDiskDriver::serialize_inode(all_of_the_inodes[inode_index].clone().expect("Could not save inode table block"));
                    },
                    CurrentDriverSelected::XML => {
                        inode_string = XmlDiskDriver::serialize_inode(all_of_the_inodes[inode_index].clone().expect("Could not save inode table block"));
                    }
                }

                if inode_i == 0 {
                    inode_table_block_string = format!("{}{}", inode_table_block_string, inode_string); 
                } else {
                    inode_table_block_string = format!("{}-{}", inode_table_block_string, inode_string); // A dash is used to seperate entries
                }
                
                inode_index += 1;
                
                
            }

            inode_block.payload = inode_table_block_string;
            self.write(inode_block);
        }

        let mut file_string = String::new();
        let mut cache_index: usize = 0;
        loop {
            match self.contents[cache_index].clone() {
                Some(block_string) => file_string = format!("{}{}\r\n", file_string, block_string),
                None => break,
            }

            cache_index += 1;
        }

        let response = fs::write(self.disk_image_location.clone(), file_string);

        match response {
            Ok(_) => {
                println!("Successfully unmounted and saved changes to the disk image.");
                true
            },
            Err(e) => {
                println!("Could not unmount successfully");
                false
            },
        }

        
    }

    /// Reads a block from the cache to an in-memory object.
    /// param block_id: The block_id that corrosponds to the array the cached strings (the index)
    /// return: Returns a block if read successfully
    pub fn read(&mut self, block_id: u32) -> Option<Block> {
        self.reads_since_mount += 1;

        let serialized_block = match &self.contents[block_id as usize] {
            Some(serialized_block) => {
                if serialized_block.is_empty() {return None} else {serialized_block.clone()}
            },
            None => return None,
        };

        match self.driver.currently_selected_driver {
            CurrentDriverSelected::XML => {
                Some(XmlDiskDriver::deserialize_block(serialized_block.to_string()))
            },
            CurrentDriverSelected::JSON => {
                Some(JsonDiskDriver::deserialize_block(serialized_block.to_string()))
            },
        }
    }

    /// Does not write to the persistent storage but instead updates the cache. Takes a block and uses its id to update that field.
    /// param block_to_update: The block to update in the cache and mark for saving later on.
    pub fn write(&mut self, block_to_update: Block) {
        let id_of_block = block_to_update.block_id as usize;
        self.writes_since_mount += 1;

        match self.driver.currently_selected_driver {
            CurrentDriverSelected::XML => {
                self.contents[id_of_block] = Some(XmlDiskDriver::serialize_block(block_to_update));
            },
            CurrentDriverSelected::JSON => {
                self.contents[id_of_block] = Some(JsonDiskDriver::serialize_block(block_to_update));
            },
        }
    }
}