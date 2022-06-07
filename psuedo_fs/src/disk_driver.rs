

pub mod DeviceDriver {
    use crate::{block::Blocks::Block, inode::inode_content::Inode, directory::Directory};

    /// Provides the two main types of functions necessary for a disk driver, read and write.
    pub trait DiskDriver {

        fn serialize_inode(inode_to_serialize: Inode) -> String;
        
        fn deserialize_inode(inode_string: String) -> Inode;

        fn serialize_directory(directory: Directory) -> String;

        fn deserialize_directory(directory_string: String) -> Directory;

        fn serialize_block(block_to_serialize: Block) -> String;

        fn deserialize_block(block_string: String) -> Block;
    }
}

