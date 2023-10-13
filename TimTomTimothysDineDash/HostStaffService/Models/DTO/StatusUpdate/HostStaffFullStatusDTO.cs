namespace HostStaffService.Models.DTO.StatusUpdate
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
