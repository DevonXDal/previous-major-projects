using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarebonesRabbitMQImplementationLibrary.Models
{
    /// <summary>
    /// This DataWithCorrelation class provides information to help trace a request that has been made across microservices.
    /// This can be extended by anything that is sent over the queues in order to tack on tracking capabilities.
    /// The main use case is for logging across microservices.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class DataWithCorrelation
    {
        /// <summary>
        /// This guid should be unique for every request made to be processed across microservices for a client.
        /// </summary>
        public string CorrelationGuid { get; set; }

        /// <summary>
        /// This identifies the specific client of this system that made the request. 
        /// If this is provided, it can be used to identify the client's environment if it is believed that it
        /// is the reason the issue occured.
        /// </summary>
        public string? ClientUUID { get; set; }

        /// <summary>
        /// When this data with tracking information was originally created (provides a timestamp for logging).
        /// This should use UTC time.
        /// </summary>
        public DateTime InitialCreation { get; set; }

        /// <summary>
        /// Creates the object with any of the data provided and fills in the blanks where possible.
        /// </summary>
        public DataWithCorrelation(string? correlationGuid, string? clientUUID, DateTime initiallyCreated) 
        { 
            
            CorrelationGuid = correlationGuid ?? Guid.NewGuid().ToString();
            InitialCreation = initiallyCreated;

            if (clientUUID != null) ClientUUID = clientUUID;
        }

        /// <summary>
        /// Creates the object without a client UUID but fills in the blank for the other tracking information
        /// </summary>
        public DataWithCorrelation() 
        {
            InitialCreation = DateTime.UtcNow;
            CorrelationGuid = Guid.NewGuid().ToString();
        }


    }
}
