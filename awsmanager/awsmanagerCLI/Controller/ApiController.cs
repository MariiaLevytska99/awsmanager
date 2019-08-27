using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerLib.Repositories;
using awsmanagerLib.Models;
using System.Configuration;
using awsmanagerLib.Configuration;

namespace awsmanagerCLI.Controller
{
  public  class ApiController
    {
        private UrlRepository UrlRepository { get; set; }
        private List<ApiGatewayUrl> urls { get; set; }
        public ServiceSection ConfigSection { get; private set; }

        public ApiController()
        {
            Console.WriteLine("Please wait ...");
            UrlRepository = new UrlRepository();
            urls = UrlRepository.urlrepository;
        }
        public ApiGatewayUrl CheckURL(string url)
        {
            var Url = new ApiGatewayUrl();
            Url.CheckUrl(url);
            urls.Add(Url);
            UrlRepository.addNewUrl(url);
           
            var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\..\ServiceConfiguration.config"));
            ConfigSection = config.GetSection("serviceSection") as ServiceSection;

            ConfigSection.Add(new Service
            {
                ServiceType = "ApiGateWay",
                ServiceName = url,
                Metric = "url",
                Value = Url.Code.Description
            });
            config.Save(ConfigurationSaveMode.Modified);
            return Url;
        }
        public List<ApiGatewayUrl> getCheckHistory()
        {
            return urls;
        }
    }
}
