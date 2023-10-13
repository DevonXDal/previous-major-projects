/**
 * BusStaffTable instances hold the data representing a table with data that the bus staff needs to know.
 */
export interface BusStaffTable {
    id: number,
    hasPaid: boolean,
    isNeedingCleaned: boolean,
    lastUpdate: Date
}