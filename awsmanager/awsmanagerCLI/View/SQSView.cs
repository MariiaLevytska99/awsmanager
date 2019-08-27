using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerCLI.Controller;

namespace awsmanagerCLI.View
{
   public class SQSView
    {
        private SQSController controller { get; set; } 
        public SQSView()
        {
            controller = new SQSController();
            ShowListOfQueue();
           
        }
        private void ShowListOfQueue()
        {
            var queues = controller.getListOfQueues();
            int number = 1;
            Console.WriteLine("\nQueueName\t|AvailableMessages\t|MessagesInFlight\t|Created date");
            foreach(var queue in queues)
            {
                Console.WriteLine("{0}. {1}\t|{2}\t\t\t|{3}\t\t\t|{4}", number,queue.QueueName, queue.CountOfAvailableMessages, queue.CountOfMessagesInFlight, queue.CreatedDate);
                number++;
            }
            QueueDetail();

        }
        private void QueueDetail()
        {
            int numberOfQueue = 0;

            do
            {
                Console.WriteLine("\nEnter number of queue to watch list of messages or press '0' to go to main menu");
                if (Int32.TryParse(Console.ReadLine(), out numberOfQueue) == false)
                    numberOfQueue = -1;
            } while (numberOfQueue > controller.getListOfQueues().Count || numberOfQueue < 0);
            if(numberOfQueue == 0)
            {
                
                new MainView();
                return;
            }

            var messageList = controller.getListOfQueues()[numberOfQueue-1].messages;
            Console.WriteLine("\nBody{0,50}{1}","|Size", "\t|Receive Count");
            foreach(var message in messageList)

            {
                int count = 0;
                int len = message.Body.Length;
                int printLen = 0;
                Console.WriteLine("{0,49}|{1}\t|{2}","", message.Size, message.ReceiveCount);
                do
                {
                    
                    if (len < 45)
                        printLen = message.Body.Length;
                    else  printLen = 45;
                    len -= 45;
                    for (int i = count; i < printLen; i++)
                    {
                        Console.Write(message.Body[i]);
                   
                    }
                    var a =49-( printLen - count);
                   for (int j = 0; j < a; j++)
                    {
                        Console.Write(" ");
                    }
                    

                    Console.Write("|\t|\n");
                    count += printLen;

                } while (count < message.Body.Length);      
            }
            Console.WriteLine("\nPress 'enter' to go to previuos page");
            Console.ReadLine();
            ShowListOfQueue();





        }

    }
}
