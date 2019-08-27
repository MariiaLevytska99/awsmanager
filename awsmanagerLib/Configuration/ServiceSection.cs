using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace awsmanagerLib.Configuration
{
    public class ServiceSection : ConfigurationSection
    {
        public static ServiceSection GetConfig()
        {
            return (ServiceSection)ConfigurationManager.GetSection("Services") ?? new ServiceSection();
        }

        [ConfigurationProperty("services")]
        [ConfigurationCollection(typeof(Services), AddItemName = "Service")]
        public Services Services
        {
            get
            {
                object o = this["services"];
                return o as Services;
            }
            set
            {
                this["services"] = value;
            }
        }

        public void Add(Service service)
        {
            Services.Add(service);
        }
    }
}
