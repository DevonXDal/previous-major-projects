namespace CoreAPIService.Models.DTO.StatusUpdate.BusStaff
{
    /// <summary>
    /// Represents the data that the bus staff currently has on a table
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class BusStaffTableStatusDTO
    {
        public int Id { get; set; }

        public bool IsNeedingCleaned { get; set; }

        public bool HasPaid { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
