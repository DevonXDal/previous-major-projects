using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStaffService
{
    /// <summary>
    /// This Env class serves to provide access to the configuration settings pertaining this service along with a general accessor
    /// </summary>
    public class Env
    {
        private IConfiguration _config;

        private IConfigurationSection _serviceSpecific 
        { get
            {
                return _config.GetRequiredSection("BusStaffService");
            }
        }

        /// <summary>
        /// Points to the database connection string.
        /// </summary>
        public string MainDb => _serviceSpecific["MainDb"];

        public Env(IConfiguration config)
        {
            _config = config;
        }
    }
}
