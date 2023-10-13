namespace CoreAPIService.Models.DTO.StatusUpdate.HostStaff
{
    /// <summary>
    /// Represents the data that the host staff currently has on a table
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class HostStaffTableStatusDTO
    {
        public int Id { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
