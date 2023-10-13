export interface KitchenStaffTable {
    id: number;
    orderGuid?: string;
    orderDescription?: string;
    isOrderStarted?: boolean;
    isOrderFinished?: boolean;
    lastUpdate?: Date;
}