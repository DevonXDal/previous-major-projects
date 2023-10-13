using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarebonesRabbitMQImplementationLibrary.Models
{
    /// <summary>
    /// This EventResponse class provides a common way to indicate whether a request was handled successfully.
    /// It is not guarenteed that there will be a response message, but this will always indicate failure or success.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class EventResponse : DataWithCorrelation
    {
        /// <summary>
        /// This indicates whether the request was successful.
        /// 0 always indicates a standard success.
        /// Negative numbers indicate the request was not successful.
        /// Positive numbers indicates a successful request (perhaps an alternative success).
        /// 
        /// This can be used to indicate the type of failure or success occured, however,
        /// what these codes mean past that can differ from application to application in addition or request to request.
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// What message, if any, helps to indicate why this particular response was provided.
        /// A status code of 0 is not expected to have a message provided.
        /// </summary>
        public string? Message { get; set; }

        public EventResponse() : base() { }
    }
}
