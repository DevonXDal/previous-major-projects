namespace CoreAPIService.Models.DTO.StatusUpdate.KitchenStaff
{
    /// <summary>
    /// Represents the data that the kitchen staff currently has on a table
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class KitchenStaffTableStatusDTO
    {
        public int Id { get; set; }

        public string? OrderGuid { get; set; }

        public string? OrderDescription { get; set; }

        public bool? IsOrderStarted { get; set; }

        public bool? IsOrderFinished { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
