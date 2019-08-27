using System;
using System.Collections.Generic;
using System.Text;

namespace awsmanagerLib.Models
{
    public class QueueMessage
    {
        public string Body { get; set; }
        public int Size { get; set; }
        public int ReceiveCount { get; set; }
        public string Id { get; set; }
    }
}
