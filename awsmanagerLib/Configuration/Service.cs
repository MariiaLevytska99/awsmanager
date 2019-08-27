using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace awsmanagerLib.Configuration
{
    public class Service : ConfigurationElement
    {
        [ConfigurationProperty("serviceType", DefaultValue = "", IsRequired = true)]
        public string ServiceType
        {
            get
            {
                return this["serviceType"] as string;
            }
            set
            {
                this["serviceType"] = value;
            }
        }

        [ConfigurationProperty("serviceName", DefaultValue = "", IsRequired = true)]
        public string ServiceName
        {
            get
            {
                return this["serviceName"] as string;
            }
            set
            {
                this["serviceName"] = value;
            }
        }

        [ConfigurationProperty("metric", DefaultValue = "", IsRequired = true)]
        public string Metric
        {
            get
            {
                return this["metric"] as string;
            }
            set
            {
                this["metric"] = value;
            }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
