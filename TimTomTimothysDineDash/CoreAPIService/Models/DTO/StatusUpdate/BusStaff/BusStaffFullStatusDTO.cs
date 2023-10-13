using CoreAPIService.Models.DTO.StatusUpdate.BusStaff;

namespace CoreAPIService.Models.DTO.StatusUpdate.HostStaff
{
    /// <summary>
    /// This holds all the information that is being sent as a status update relating to the host staff
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class BusStaffFullStatusDTO
    {
        public ICollection<BusStaffTableStatusDTO> Tables { get; set; }
    }
}
