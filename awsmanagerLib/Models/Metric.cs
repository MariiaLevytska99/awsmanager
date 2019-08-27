using System;
using System.Collections.Generic;
using System.Text;

namespace awsmanagerLib.Models
{
    enum Metrics
    {
        Invocations,
        Errors,
        DeadLetterErrors,
        Duration,
        Throttles,
        IteratorAge,
        ConcurrentExecutions,
        UnreservedConcurrentExecutions
    }
    public class Metric
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
