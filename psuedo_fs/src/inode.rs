pub mod inode_content {
    use std::time::{SystemTime, UNIX_EPOCH, Duration};

    use chrono::prelude::*;
    use serde::{Deserialize, Serialize};

    #[derive(Deserialize, Serialize, Clone, Debug, PartialEq)]
    pub struct Inode {
        pub id: u16, // The unique address of the inode (relates to the array index of the inodes array in the superblock)
        pub file_type: FileType, //
        pub start_block: u32, // The first block's id
        pub size: u16, // The number of blocks associated with the inode's file
        pub c_time: u64, // The date and time the file was created in Unix EPOCH
        
    }
    
    impl Inode {
        pub fn create_new(inode_number: u16, file_type: FileType, start_block: u32, size: u16) -> Inode { 
            let mut new_inode = Inode {
                id: inode_number, 
                file_type, 
                start_block,
                size,
                c_time: SystemTime::now().duration_since(UNIX_EPOCH).expect("Failed to get Unix time").as_secs(), // https://doc.rust-lang.org/std/time/constant.UNIX_EPOCH.html
            };
            
            new_inode
        }
    }
    
    #[derive(Serialize, Deserialize, Clone, Debug, PartialEq)]
    pub enum FileType {
        Free, File, Directory
    }
}
