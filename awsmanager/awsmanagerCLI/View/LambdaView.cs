using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerCLI.Controller;

namespace awsmanagerCLI.View
{
    public class LambdaView
    {
        private LambdaController controller { get; set; }

        public LambdaView()
        {
            controller = new LambdaController();
            ShowLambdaList();
        }

        private void ShowLambdaList()
        {
            int number = 1;
            var functions = controller.getListOfLambda();
            Console.WriteLine("Function Name{0,50}\t|Size\t|Status","|Description");
            foreach(var func in functions)
            {
                var firstdel = 63 - func.FunctionName.Length;
                Console.Write("{0}. {1}", number, func.FunctionName);
                var a = 48 - func.FunctionName.Length;
                for (int j = 0; j < a; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("|{0}\t\t|{1}\t|{2}", func.Description, func.Size, func.Status);
                Console.Write("\n");
                number++;
            }
            int numberOfFuncion = 0;
            do
            {
                
                Console.WriteLine("\nEnter number of lambda function to watch details(metrics) or 0 to go to prevoius page");

                if (Int32.TryParse(Console.ReadLine(), out numberOfFuncion) == false)
                    numberOfFuncion = -1;
            } while (numberOfFuncion > controller.getListOfLambda().Count || numberOfFuncion < 0);
            if (numberOfFuncion == 0)
            {

                new MainView();
                return;
            }
            Console.WriteLine("\nEnter 'S' to stop or 'R' to resume selected lambda function or press 'enter' to skip this step");
            var action = Console.ReadLine();
            if(action == "S" || action == "s")
            {
                controller.StopFunction(numberOfFuncion);
                ShowFunctionMetrics(numberOfFuncion);
            }
            else if(action == "R" || action == "r")
            {
                controller.ResumeFunction(numberOfFuncion);
                ShowFunctionMetrics(numberOfFuncion);
            }
            else
                ShowFunctionMetrics(numberOfFuncion);


        }
        private void ShowFunctionMetrics(int numberOfFunction)
        {

            var metrics = controller.getListOfLambda()[numberOfFunction-1].Metrics;
            Console.WriteLine("\nMetric Name{0,19}","|Value");
            foreach(var metric in metrics)
            {
                Console.Write("{0}", metric.Title);
                for (int i = 0; i < 24 - metric.Title.Length;i++)
                    Console.Write(" ");
                Console.Write("|{0}\n", metric.Value);
            }
            Console.WriteLine("\nPress 'enter' to go to previuos page");
            Console.ReadLine();
            ShowLambdaList();
        }

    }
}
