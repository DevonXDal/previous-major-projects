using BarebonesRabbitMQImplementationLibrary.Models;

namespace BarebonesRabbitMQImplementationLibrary.Models.DTO
{
    /// <summary>
    /// This SingleStringDTO class adds tracking information along with a string value.
    /// This string value could be serialized data, an id (perhaps needing parsed), etc.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class SingleStringDTO : DataWithCorrelation
    {
        /// <summary>
        /// This holds a single piece of information contained in a string
        /// 
        /// This could be a serialized string, and id to be parsed, etc.
        /// This prevents the need to create a unique DTO for every event.
        /// </summary>
        public string SingleStringValue { get; set; }

        public SingleStringDTO() : base() { }
    }
}
