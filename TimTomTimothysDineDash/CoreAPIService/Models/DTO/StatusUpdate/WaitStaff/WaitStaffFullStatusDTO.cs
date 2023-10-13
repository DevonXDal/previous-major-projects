using CoreAPIService.Models.DTO.StatusUpdate.WaitStaff;

namespace CoreAPIService.Models.DTO.StatusUpdate.HostStaff
{
    /// <summary>
    /// This holds all the information that is being sent as a status update relating to the host staff
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class WaitStaffFullStatusDTO
    {
        public ICollection<WaitStaffTableStatusDTO> Tables { get; set; }
    }
}
