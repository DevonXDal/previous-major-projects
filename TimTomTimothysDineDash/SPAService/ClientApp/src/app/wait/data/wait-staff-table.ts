export interface WaitStaffTable {
    id: number;
    seatsUsed: number;
    orderGuid?: string;
    orderDescription?: string;
    isOrderFinished?: boolean;
    lastUpdate?: Date;
}