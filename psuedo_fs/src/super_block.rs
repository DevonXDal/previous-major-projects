use crate::block::Blocks::Block;

use crate::inode::inode_content::{Inode, FileType};

/// The first block on the filesystem. It manages the blocks and inodes that can be assigned.
pub struct SuperBlock {
    pub magic_number: u32, // Need to know more and redocument
    pub inode_count: u32, // 0 if unmounted or else the number of inodes on the disk
    pub block_count: u32, // 0 if unmounted or else the number of blocks on the disk
    pub free_blocks: Vec<Block>, // All of the free blocks when mounted
    pub free_inodes: Vec<Inode>, // All of the free inodes when mounted
    pub inode_table: [Option<Inode>; 5001] // All of the inodes on the filesystem
}

impl SuperBlock {
    //pub fn create_new()

    /// Replaces a inode in the inode table with the new one specified. The id of the inode that is passed as an argument to this function
    /// is used to determine which inode to replace.
    pub fn update_inode(&mut self, new_inode: Inode) {
        let new_id = new_inode.id.clone();

        if new_id > 0 && new_id < self.inode_table.len() as u16 {
            self.inode_table[new_id as usize] = Some(new_inode);
        }
    }

    /// Takes an inode that is no longer needed and reinserts it to the list of free inodes
    /// param unneeded_inode: The inode that is no longer needed and can be freed for future use
    pub fn put_free_inode(&mut self, mut unneeded_inode: Inode) { 

        unneeded_inode.file_type = FileType::Free;

        self.free_inodes.push(unneeded_inode);
    }

    /// Gets an inode that is available for use and returns it
    /// return: An unused inode that can be used for a file
    pub fn get_free_inode(&mut self) -> (Inode, bool) {
        let more_left = self.free_inodes.len() > 1;
        (self.free_inodes.remove(0), more_left)
    }

    /// Takes a block that is no longer needed and reinserts it to the list of free blocks
    /// param unneeded_block: The block that is no longer needed and can be freed for future use
    pub fn put_free_block(&mut self, mut unneeded_block: Block) {
        self.free_blocks.push(unneeded_block);
    }

    /// Gets a block that is available for use and returns it
    /// return: An unused block that can be used for a file
    pub fn get_free_block(&mut self) -> (Block, bool) {
        let more_left = self.free_blocks.len() > 1;
        (self.free_blocks.remove(0), more_left)
    }


}