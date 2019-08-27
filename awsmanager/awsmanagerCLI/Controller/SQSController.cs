using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerLib.Repositories;
using awsmanagerLib.Models;

namespace awsmanagerCLI.Controller
{
   public class SQSController
    {
        private QueueRepository QueueRepository { get; set; }
        private List<Queue> Queues { get; set; }
        public SQSController()
        {
            Console.WriteLine("Please wait ...");
            QueueRepository = new QueueRepository();
            Queues = QueueRepository.queueRepository;
        }

        public List<Queue> getListOfQueues()
        {
            return Queues;
        }
    }
}
