

pub mod JsonDriver {
    use crate::{block::Blocks::Block, disk_driver::DeviceDriver::DiskDriver, inode::inode_content::Inode, directory::Directory};
    use std::fs;

    /// Implementation of the disk driver that serializes and deserializes blocks, inodes, etc. using JSON
    #[derive(Clone)]
    pub struct JsonDiskDriver {
        //pub disk_block_serialized_references: [Option<String>; u8::MAX as usize], // Holds the serialized string in an array
    }
    
    impl DiskDriver for JsonDiskDriver {
        fn serialize_inode(inode_to_serialize: Inode) -> String {
            serde_json::to_string(&inode_to_serialize).unwrap()
        }

        fn deserialize_inode(inode_string: String) -> Inode {
            serde_json::from_str(inode_string.as_str()).unwrap()
        }

        fn serialize_directory(directory: Directory) -> String {
            serde_json::to_string(&directory).unwrap()
        }

        fn deserialize_directory(directory_string: String) -> Directory {
            serde_json::from_str(directory_string.as_str()).unwrap()
        }

        fn serialize_block(block_to_serialize: Block) -> String {
            serde_json::to_string(&block_to_serialize).unwrap()
        }

        fn deserialize_block(block_string: String) -> Block {
            serde_json::from_str(block_string.as_str()).unwrap()
        }
    }

}
