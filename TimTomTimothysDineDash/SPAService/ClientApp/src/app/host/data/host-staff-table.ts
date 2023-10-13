/**
 * HostStaffTable instances hold the id, availability, and previous modification date-time of each of the tables.
 * This is helpful for host staff to identify which tables that they can seat the next party in queue at.
 */
export interface HostStaffTable {
    id: number;
    isAvailable: boolean;
    lastUpdate: Date;
}