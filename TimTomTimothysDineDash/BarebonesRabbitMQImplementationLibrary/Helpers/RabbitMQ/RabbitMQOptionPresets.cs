using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ
{
    public class RabbitMQOptionPresets
    {
        public readonly static Dictionary<string, object> SHOULD_EXPIRE_AFTER_20_MINUTES = new Dictionary<string, object>() 
        {
            { "x-expires", 1200000 } // Is millis 
        };

        public readonly static Dictionary<string, object> SHOULD_EXPIRE_AFTER_2_MINUTES = new Dictionary<string, object>()
        {
            { "x-expires", 120000 } // Is millis 
        };
    }
}
