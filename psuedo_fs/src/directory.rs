use std::collections::HashMap;

use serde::{Deserialize, Serialize};
use serde_with::serde_as;

/// Represents a directory of files on a filesystem.
#[serde_as]
#[derive(Deserialize, Serialize, Clone, Debug, PartialEq)]
pub struct Directory {
    //#[serde_as(as = "Vec<(_, _)>")]
    pub contents: HashMap<String, u32> // File names to the inode of the file inside
}

impl Directory {
    /// Creates a new directory and handles the setting up of its contents.
    /// param inode_of_directory: The id of the inode for the directory being created.
    /// param inode_of_parent: The id of the inode for the parent directory of the directory being created
    /// return: The newly created directory
    pub fn new(inode_of_directory: u32, inode_of_parent: u32) -> Directory {
        let mut new_directory = Directory { contents: HashMap::<String, u32>::with_capacity(255) };

        new_directory.contents.insert(String::from("."), inode_of_directory);
        new_directory.contents.insert(String::from(".."), inode_of_parent);

        new_directory
    }

    /// Validates and adds the inode number association with a file name given that the filename is not currently in use.
    /// Will also return false if the directory reaches its maximum size of 255 files. The limit only applies to files directly inside it, not in child folders.
    /// param inode_number: The inode's identifier number that is to be associated with the provided filename.
    /// param filename: The name of the file to be associated with the provided inode number.
    /// return: Was the inode/filename association successfully added to the directory?
    pub fn add(&mut self, inode_number: u32, filename: String) -> bool {
        if self.contents.contains_key(&filename) {
            false
        } else {
            self.contents.insert(filename, inode_number);
            true
        }
    }

    /// Returns the associated inode number to the filename provided
    /// If the filename is not found in the directory, returns -1
    /// Returns a 64 bit integer in order to make use of all u32 values
    /// param filename: The filename of the file without its path
    /// return: The inode number the file is attached to or -1 if no file exists with the given filename
    pub fn get_inode_number(&self, filename: String) -> i64 {
        let possible_inode_number = self.contents.get(&filename);

        match possible_inode_number {
            Some(inode_number) => inode_number.clone() as i64,
            None => -1
        }
    }

    /// Gets the list of filenames directly inside the directory. Excludes the filenames held in subfolders.
    /// Holds up to 255 filenames but does not always have 255 filenames. Done this way to meet the size requirement from Rust.
    /// If a filename comes up as an empty string, it and all further file spots in the directory are currently unused.
    /// return: The names of files stored directly inside this directory.
    pub fn get_contents(&self) -> [&str; 255] {
        const empty_string: &str = "";
        let mut keys =[empty_string; 255];

        let mut current_key_index = 0;
        for key in self.contents.keys() {
            keys[current_key_index] = key.as_str().clone();
            current_key_index += 1;
        }

        keys
    }

    /// Removes the file from the directory if the file exists.
    /// Empties the contents of the file and frees its inodes and blocks for reuse if it has no more links.
    /// Will not delete the root directory.
    /// param filename: The name of the file to remove from the system.
    /// return: Was the file successfully removed from the directory
    pub fn remove(&mut self, filename: String) -> bool {
        self.contents.remove(&filename);
        true
    }
}