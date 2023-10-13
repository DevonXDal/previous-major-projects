import { HostStaffQueueParty } from "./host-staff-queue-party";
import { HostStaffTable } from "./host-staff-table";

export interface HostStaffStatus {
    Tables: HostStaffTable[];
    QueueParties: HostStaffQueueParty[];
}