pub mod XmlDriver {
    extern crate quick_xml;

    use crate::{block::Blocks::Block, disk_driver::DeviceDriver::DiskDriver, directory::Directory, inode::inode_content::Inode};
    use std::fs;
    use quick_xml::{de::{from_str, DeError, self}, se::to_string};


    /// Implementation of the disk driver that serializes and deserializes blocks using JSON
    #[derive(Clone)]
    pub struct XmlDiskDriver {
        //pub disk_block_serialized_references: [Option<String>; u8::MAX as usize], // Holds the serialized string in an array
    }
    
    impl DiskDriver for XmlDiskDriver {
        fn serialize_inode(inode_to_serialize: Inode) -> String {
            to_string(&inode_to_serialize).unwrap()
        }

        fn deserialize_inode(inode_string: String) -> Inode {
            from_str(inode_string.as_str()).unwrap()
        }

        fn serialize_directory(directory: Directory) -> String {
            to_string(&directory).unwrap()
        }

        fn deserialize_directory(directory_string: String) -> Directory {
            from_str(directory_string.as_str()).unwrap()
        }

        fn serialize_block(block_to_serialize: Block) -> String {
            to_string(&block_to_serialize).unwrap()
        }

        fn deserialize_block(block_string: String) -> Block {
            from_str(block_string.as_str()).unwrap()
        }
    }

}
