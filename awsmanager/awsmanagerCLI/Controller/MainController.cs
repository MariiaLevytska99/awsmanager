using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using awsmanagerLib.Configuration;


namespace awsmanagerCLI.Controller
{
    public class MainController
    {
        private readonly ServiceSection ConfigSection;
        public MainController()
        {
            var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\..\ServiceConfiguration.config"));
            ConfigSection = config.GetSection("serviceSection") as ServiceSection;
            ConfigSection.Services.ClearAll();
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
