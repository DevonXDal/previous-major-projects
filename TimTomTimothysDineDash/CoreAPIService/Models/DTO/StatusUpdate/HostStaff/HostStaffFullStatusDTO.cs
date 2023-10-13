using BarebonesRabbitMQImplementationLibrary.Models;

namespace CoreAPIService.Models.DTO.StatusUpdate.HostStaff
{
    /// <summary>
    /// This holds all the information that is being sent as a status update relating to the host staff
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class HostStaffFullStatusDTO
    {
        public ICollection<HostStaffQueueStatusDTO> QueueParties { get; set; }

        public ICollection<HostStaffTableStatusDTO> Tables { get; set; }
    }
}
