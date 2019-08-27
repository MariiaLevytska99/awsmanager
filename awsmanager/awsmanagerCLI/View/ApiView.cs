using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerCLI.Controller;
using awsmanagerLib.Models;

namespace awsmanagerCLI.View
{
   public class ApiView
    {
       private ApiController controller { get; set; }
        public ApiView()
        {
            controller = new ApiController();

            CheckUrl();
        }

        private void CheckUrl()
        {
            Console.WriteLine("\nEnter URL to check");
            string url_text = Console.ReadLine();
            ApiGatewayUrl url = controller.CheckURL(url_text);
             Console.WriteLine("{0}\t{1}\t{2}", url.Url, url.Code.CodeNumber, url.Code.Description);
            Console.WriteLine("\nPress '0' to go the previous page or 'enter to check another URL");
            var key = Console.ReadLine();
            if (key == "0")
                new MainView();
            else CheckUrl();
        }

    }
}
