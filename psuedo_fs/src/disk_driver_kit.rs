pub mod driver_disk_kit {
    use crate::{xml_device_driver::XmlDriver::XmlDiskDriver, json_device_driver::JsonDriver::JsonDiskDriver};

    pub struct DriverDiskKit {
        pub xml_device_driver: XmlDiskDriver,
        pub json_device_driver: JsonDiskDriver,
        pub currently_selected_driver: CurrentDriverSelected
    }

    pub enum CurrentDriverSelected {
        XML, JSON
    }
}