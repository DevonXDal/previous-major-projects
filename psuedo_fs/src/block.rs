pub mod Blocks {
    use serde::Serialize;
    use serde::Deserialize;
    
    /// Represents a block of serialized data for a file or folder
    #[derive(Deserialize, Serialize, PartialEq, Debug, Clone)]
    pub struct Block {
        pub block_id: u32, // The current block (for the later file system exercises it will be used as an array for the filesystem)
        pub next_block: i64, // The reference id to the next block on the disk drive (-1 if it is the last block in a file or folder, -2 is free for allocation)
        pub payload: String // Serialized payload of data to store (limited to 1024 characters)
    }
}
