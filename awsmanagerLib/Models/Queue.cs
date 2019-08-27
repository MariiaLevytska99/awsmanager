using System;
using System.Collections.Generic;
using System.Text;

namespace awsmanagerLib.Models
{
    public class Queue
    {
        public string QueueName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CountOfAvailableMessages { get; set; }
        public int CountOfMessagesInFlight { get; set; }
        public List<QueueMessage> messages { get; set; }
    }
}
