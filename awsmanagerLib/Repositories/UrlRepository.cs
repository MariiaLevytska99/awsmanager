using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using awsmanagerLib.Configuration;
using awsmanagerLib.Models;

namespace awsmanagerLib.Repositories
{
    public class UrlRepository
    {
        public List<ApiGatewayUrl> urlrepository { get; set; }
        private ServiceSection ConfigSection;

        public Code Code { get; private set; }

        private readonly string ServiceType = "ApiGateWay";

        public UrlRepository()
        {

            urlrepository = new List<ApiGatewayUrl>();
        }

        public void addNewUrl(string url)
        {

            ApiGatewayUrl apiUrl = new ApiGatewayUrl();
            apiUrl = apiUrl.CheckUrl(url);
            urlrepository.Add(apiUrl);


        }



    }

    }

