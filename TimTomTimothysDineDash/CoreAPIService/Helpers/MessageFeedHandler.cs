using BarebonesEFCoreRepositoryPatternStarter.Data;
using CoreAPIService.Database.Data;
using CoreAPIService.Database.Models;

namespace CoreAPIService.Helpers
{
    /// <summary>
    /// This MessageFeedHandler class handles inserting a message and retrieving recent messages from the message feed.
    /// This provides an almost real-time updating of what events have transpired.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class MessageFeedHandler
    {
        private readonly RepositoryBase<ApplicationDbContext, MessageFeed> _messageRepo;

        public MessageFeedHandler(RepositoryBase<ApplicationDbContext, MessageFeed> messageRepo) 
        { 
            _messageRepo = messageRepo; 
        }

        /// <summary>
        /// Tracks a new event into the message feed, so that staff can be updated on what's happening
        /// on the system.
        /// </summary>
        /// <param name="relatedService">The service or area that this event message is related to</param>
        /// <param name="description">What occured, in a end-user readable way</param>
        public void TrackNewEvent(string relatedService, string description)
        {
            _messageRepo.Insert(new()
            {
                ServiceRelation = relatedService,
                Description = description
            });
        }

        public ICollection<MessageFeed> GetRecentMessages() 
        {
            var messages = _messageRepo.Get(m => m.Created > DateTime.UtcNow.AddMinutes(-30)); // Grabs only ones from the last 30 minutes
            if (messages.Count() <= 5) return messages.ToList(); // Not enough to worry about

            return _messageRepo.Get().OrderByDescending(m => m.Created).Take(5).ToList();
        }
    }
}
