namespace CoreAPIService.Models.DTO.StatusUpdate.MessageFeed
{
    /// <summary>
    /// This MessageDTO class holds the relevant information on incoming messages on the feed needed by client apps.
    /// This provides the means for which users can watch for updates from other staff.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class MessageDTO
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
    }
}
