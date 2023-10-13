namespace CoreAPIService.Models.DTO.StatusUpdate.WaitStaff
{
    /// <summary>
    /// Represents the data that the wait staff currently has on a table
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class WaitStaffTableStatusDTO
    {
        public int Id { get; set; }

        public int SeatsUsed { get; set; }

        public string? OrderGuid { get; set; }

        public string? OrderDescription { get; set; }

        public bool? IsOrderFinished { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
