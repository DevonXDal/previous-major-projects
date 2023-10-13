namespace KitchenStaffService.Models.DTO.StatusUpdate
{
    /// <summary>
    /// This holds all the information that is being sent as a status update relating to the host staff
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class KitchenStaffFullStatusDTO
    {
        public ICollection<KitchenStaffTableStatusDTO> Tables { get; set; }
    }
}
