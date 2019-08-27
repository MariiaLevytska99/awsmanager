using System;
using System.Collections.Generic;
using System.Text;

namespace awsmanagerLib.Models
{
    public enum LambdaStatus
    {
        DenyExecution = 0,
        AllowExecution = 1
    }
    public class Lambda
    {
        public string FunctionName { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public LambdaStatus Status { get; set; }
        public List<Metric> Metrics { get; set; }
    }
}
