using System;
using System.Collections.Generic;
using awsmanagerLib.Models;
using awsmanagerLib.Configuration;
using Amazon;
using System.Configuration;
using Amazon.CloudWatch;

namespace awsmanagerLib.Repositories
{
    public class LambdaMetricsRepository
    {
        public List<Metric> MetricsRepository { get; set; }
        private readonly ServiceSection ConfigSection;
        private static RegionEndpoint Region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
        private static string access = ConfigurationManager.AppSettings["accessKey"].ToString();
        private static string secret = ConfigurationManager.AppSettings["secretKey"].ToString();
        private readonly IAmazonCloudWatch cloudWatchClient = new AmazonCloudWatchClient(access,secret, RegionEndpoint.USEast1);
        private List<string> statisticList = new List<string>() { "Average", "Maximum", "Minimum", "SampleCount", "Sum" };
        private readonly string ServiceType = "Lambda";

        public LambdaMetricsRepository(string functionName)
        {
            var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\ServiceConfiguration.config"));
            ConfigSection = config.GetSection("serviceSection") as ServiceSection;
            MetricsRepository = GetMetricRepository(functionName);
            config.Save(ConfigurationSaveMode.Modified);
        }

        private List<Metric> GetMetricRepository(string functionName)
        {
            List<Metric> repositoryOfMetrics = new List<Metric>();
            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.Invocations.ToString(),
                    Value = GetCountOfInvocations(functionName).ToString()
                });
            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.Errors.ToString(),
                    Value = GetCountOfErrors(functionName).ToString()
                });
            repositoryOfMetrics.Add(
                 new Metric
                 {
                     Title = Metrics.Duration.ToString(),
                     Value = GetDuration(functionName).ToString()
                 });
            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.Throttles.ToString(),
                    Value = GetCountOfThrottles(functionName).ToString()
                });
            /* repositoryOfMetrics.Add(
                 new Metric
                 {
                     Title = Metrics.DeadLetterErrors.ToString(),
                     Value = GetCountOfDeadLetterErrors(functionName).ToString()
                 });

            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.IteratorAge.ToString(),
                    Value = GetIteratorAge(functionName).ToString()
                });*/
            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.ConcurrentExecutions.ToString(),
                    Value = GetCountOfConcurrentExecutions(functionName).ToString()
                });
            /*
            repositoryOfMetrics.Add(
                new Metric
                {
                    Title = Metrics.UnreservedConcurrentExecutions.ToString(),
                    Value = GetCountOfUnreservedConcurrentExecutions(functionName).ToString()
                });*/
            return repositoryOfMetrics;
        }

        private double GetCountOfInvocations(string functionName)
        {
            double countOfInvocation = 0;
            var InvocationResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {

                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }

                    },
                    EndTime = DateTime.Today,
                    MetricName = "Invocations",
                    Namespace = "AWS/Lambda",
                    // Get statistics by day.
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    // Get statistics for the past month.
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in InvocationResponse.Result.Datapoints)
            {
                countOfInvocation += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.Invocations.ToString(),
                Value = countOfInvocation.ToString()
            });
            return countOfInvocation;
        }

        private double GetCountOfErrors(string functionName)
        {
            double countOfErrors = 0;
            var ErrorResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {
                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }
                    },
                    EndTime = DateTime.Today,
                    MetricName = "Errors",
                    Namespace = "AWS/Lambda",
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in ErrorResponse.Result.Datapoints)
            {
                countOfErrors += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.Errors.ToString(),
                Value = countOfErrors.ToString()
            });

            return countOfErrors;
        }

        private double GetCountOfDeadLetterErrors(string functionName)
        {
            double countOfDeadLetterErrors = 0;
            var DeadLettrerResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {
                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }
                    },
                    EndTime = DateTime.Today,
                    MetricName = "DeadLetterErrors",
                    Namespace = "AWS/Lambda",
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in DeadLettrerResponse.Result.Datapoints)
            {
                countOfDeadLetterErrors += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.DeadLetterErrors.ToString(),
                Value = countOfDeadLetterErrors.ToString()
            });
            return countOfDeadLetterErrors;
        }

        private double GetDuration(string functionName)
        {
            double duration = 0;
            var DurationResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {

                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }

                    },
                    EndTime = DateTime.Today,
                    MetricName = "Duration",
                    Namespace = "AWS/Lambda",
                    // Get statistics by day.
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    // Get statistics for the past month.
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Milliseconds
                });
            foreach (var point in DurationResponse.Result.Datapoints)
            {
                duration += point.Average;
            }

            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.Duration.ToString(),
                Value = duration.ToString()
            });
            return duration;
        }

        private double GetCountOfThrottles(string functionName)
        {
            double countOfThrottles = 0;
            var ThrottlesResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {
                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }
                    },
                    EndTime = DateTime.Today,
                    MetricName = "Throttles",
                    Namespace = "AWS/Lambda",
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in ThrottlesResponse.Result.Datapoints)
            {
                countOfThrottles += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.Throttles.ToString(),
                Value = countOfThrottles.ToString()
            });
            return countOfThrottles;
        }

        private double GetIteratorAge(string functionName)
        {
            double iteratorAge = 0;
            var IteratorAgeResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {

                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }

                    },
                    EndTime = DateTime.Today,
                    MetricName = "IteratorAge",
                    Namespace = "AWS/Lambda",
                    // Get statistics by day.
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    // Get statistics for the past month.
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Milliseconds
                });
            foreach (var point in IteratorAgeResponse.Result.Datapoints)
            {
                iteratorAge = point.Maximum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.IteratorAge.ToString(),
                Value = iteratorAge.ToString()
            });
            return iteratorAge;
        }

        private double GetCountOfConcurrentExecutions(string functionName)
        {
            double countOfConcurrentExecution = 0;
            var ConcurrentExecutionsResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {
                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }
                    },
                    EndTime = DateTime.Today,
                    MetricName = "ConcurrentExecutions",
                    Namespace = "AWS/Lambda",
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in ConcurrentExecutionsResponse.Result.Datapoints)
            {
                countOfConcurrentExecution += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.ConcurrentExecutions.ToString(),
                Value = countOfConcurrentExecution.ToString()
            });

            return countOfConcurrentExecution;
        }

        private double GetCountOfUnreservedConcurrentExecutions(string functionName)
        {
            double countOfUnreservedConcurrentExecution = 0;
            var UnreservedConcurrentExecutionsResponse = cloudWatchClient.GetMetricStatisticsAsync(
                new Amazon.CloudWatch.Model.GetMetricStatisticsRequest
                {
                    Dimensions = new List<Amazon.CloudWatch.Model.Dimension>()
                    {
                        new Amazon.CloudWatch.Model.Dimension
                        {
                            Name = "FunctionName",
                            Value = functionName
                        }
                    },
                    EndTime = DateTime.Today,
                    MetricName = "UnreservedConcurrentExecutions",
                    Namespace = "AWS/Lambda",
                    Period = (int)TimeSpan.FromDays(1).TotalSeconds,
                    StartTime = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
                    Statistics = statisticList,
                    Unit = StandardUnit.Count
                });
            foreach (var point in UnreservedConcurrentExecutionsResponse.Result.Datapoints)
            {
                countOfUnreservedConcurrentExecution += point.Sum;
            }
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = functionName,
                Metric = Metrics.UnreservedConcurrentExecutions.ToString(),
                Value = countOfUnreservedConcurrentExecution.ToString()
            });
            return countOfUnreservedConcurrentExecution;
        }
    }
}
