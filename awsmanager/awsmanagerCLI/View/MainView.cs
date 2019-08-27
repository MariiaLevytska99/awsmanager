using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerCLI.Controller;

namespace awsmanagerCLI.View
{

    public class MainView
    {
        MainController controller = new MainController();
        public MainView()
        {
            CallService(AskService());
        }
        private int AskService()
        {
            int numberOfService = 0;
            bool flag = true; ;
            do
            {
                Console.WriteLine("\nPlease choose an aws service or press '0' to exit:");
                Console.WriteLine("1 - SQS\n2 - Lambda\n3 - ApiGateway");
                if(Int32.TryParse(Console.ReadLine(),out numberOfService))
                {
                    if (numberOfService == 1 || numberOfService == 2 || numberOfService == 3 || numberOfService == 0)
                        flag = false;
                    else flag = true;
                }
                else flag = true;


            } while (flag == true);

            return numberOfService;
        }
        private void CallService(int numberOfService)
        {
            switch(numberOfService)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    new SQSView();
                    break;
                case 2:
                    new LambdaView();
                    break;
                case 3:
                    new ApiView();
                    break;
                default:
                    break;

            }

        }

    }
}
