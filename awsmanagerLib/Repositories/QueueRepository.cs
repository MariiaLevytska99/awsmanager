using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using awsmanagerLib.Configuration;
using awsmanagerLib.Models;
using System.Configuration;

namespace awsmanagerLib.Repositories
{
    public class QueueRepository
    {
        private static RegionEndpoint Region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
        private static string access = ConfigurationManager.AppSettings["accessKey"].ToString();
        private static string secret = ConfigurationManager.AppSettings["secretKey"].ToString();
        readonly IAmazonSQS sqsClient = new AmazonSQSClient(access, secret, RegionEndpoint.USEast1);
        private readonly ServiceSection ConfigSection;
        private readonly string ServiceType = "SQS";
        public List<Queue> queueRepository { get; set; }

        public QueueRepository()
        {
           var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\ServiceConfiguration.config"));
           ConfigSection = config.GetSection("serviceSection") as ServiceSection;
           queueRepository = GetQueueRepository();
           config.Save(ConfigurationSaveMode.Modified);
        }

        public List<Queue> GetQueueRepository()
        {
            List<Queue> listOfQueues = new List<Queue>();
            var queueRequest = new ListQueuesRequest();
            var queueResponse = sqsClient.ListQueuesAsync(queueRequest);
            foreach (var queue in queueResponse.Result.QueueUrls)
            {
                var a = sqsClient.GetQueueAttributesAsync(new GetQueueAttributesRequest
                {
                    QueueUrl = queue,
                    AttributeNames = new List<string>() { "All" }
                });

                listOfQueues.Add(new Queue
                {
                    QueueName = queue.Substring(queue.LastIndexOf('/') + 1),
                    CountOfAvailableMessages = GetCountOfMessages(queue),
                    CountOfMessagesInFlight = GetCountOfMessagesInFlight(queue),
                    CreatedDate = GetQueueCreatedDate(queue),
                    messages = new MessageRepository(queue).messageRepository


                }
                );

            }
            return listOfQueues;
        }

        public int GetCountOfMessages(string queueURL)
        {
            int countOfMessages = 0;
            var getNumberOfMessagesRequest = sqsClient.GetQueueAttributesAsync(
               new GetQueueAttributesRequest
               {
                   QueueUrl = queueURL,
                   AttributeNames = new List<string>() { "All" }
               }
               );
            countOfMessages = getNumberOfMessagesRequest.Result.ApproximateNumberOfMessages;
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = queueURL.Substring(queueURL.LastIndexOf('/') + 1),
                Metric = "CountOfMessages",
                Value = countOfMessages.ToString()
            });

            return countOfMessages;

        }

        public int GetCountOfMessagesInFlight(string queueURL)
        {
            int countOfMessagesInFlight = 0;
            var getNumberOfMessagesInFlightRequest = sqsClient.GetQueueAttributesAsync(
                new GetQueueAttributesRequest
                {
                    QueueUrl = queueURL,
                    AttributeNames = new List<string>() { "All" }
                });
            countOfMessagesInFlight = getNumberOfMessagesInFlightRequest.Result.ApproximateNumberOfMessagesNotVisible;
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = queueURL.Substring(queueURL.LastIndexOf('/') + 1),
                Metric = "CountOfMessagesInFlight",
                Value = countOfMessagesInFlight.ToString()
            });

            return countOfMessagesInFlight;
        }

        public DateTime GetQueueCreatedDate(string queueURL)
        {
            DateTime createdDate = DateTime.Today;
            var getCreatedDateResponse = sqsClient.GetQueueAttributesAsync(
                new GetQueueAttributesRequest
                {
                    QueueUrl = queueURL,
                    AttributeNames = new List<string>() { "All" }
                }
                );
            createdDate = getCreatedDateResponse.Result.CreatedTimestamp;
            ConfigSection.Add(new Service
            {
                ServiceType = ServiceType,
                ServiceName = queueURL.Substring(queueURL.LastIndexOf('/') + 1),
                Metric = "CreatedDate",
                Value = createdDate.ToString()
            });
            return createdDate;
        }
    }
}
