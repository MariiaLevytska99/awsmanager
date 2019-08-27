using awsmanagerLib.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace awsmanager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       private readonly ServiceSection ConfigSection;
        App()
        {
            var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\ServiceConfiguration.config"));
            ConfigSection = config.GetSection("serviceSection") as ServiceSection;
            ConfigSection.Services.ClearAll();
            config.Save(ConfigurationSaveMode.Modified);

        }
    
    }
}

