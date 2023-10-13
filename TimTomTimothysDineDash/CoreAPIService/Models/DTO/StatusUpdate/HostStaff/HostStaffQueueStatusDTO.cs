namespace CoreAPIService.Models.DTO.StatusUpdate.HostStaff
{
    /// <summary>
    /// Represents the data that the host staff currently has on the queue. 
    /// This only shows those currently in line
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class HostStaffQueueStatusDTO
    {
        public int Id { get; set; }

        public int NumToSeat { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
