using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SQS;
using awsmanagerLib.Models;
using System.Configuration;
using Amazon.SQS.Model;
using regionr = Amazon.RegionEndpoint;

namespace awsmanagerLib.Repositories
{
    public class MessageRepository
    {
        private static RegionEndpoint Region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
        private static string access = ConfigurationManager.AppSettings["accessKey"].ToString();
        private static string secret = ConfigurationManager.AppSettings["secretKey"].ToString();
        readonly AmazonSQSClient sqsClient = new AmazonSQSClient(access, secret,regionr.USEast1);
        public List<QueueMessage> messageRepository { get; set; }

        public MessageRepository(string queueURL)
        {
            messageRepository = GetMessageRepository(queueURL);
        }

        private List<QueueMessage> GetMessageRepository(string queueURL)
        {
            List<QueueMessage> listOfMessage = new List<QueueMessage>();
            int countOfMessages = 0;
            var getNumberOfMessagesRequest = sqsClient.GetQueueAttributesAsync(
               new GetQueueAttributesRequest
               {
                   QueueUrl = queueURL,
                   AttributeNames = new List<string>() { "ApproximateNumberOfMessages" }
               }
               );
            countOfMessages = getNumberOfMessagesRequest.Result.ApproximateNumberOfMessages;
            for (int i = 0; i < countOfMessages; i++)
            {
                ReceiveMessageRequest receiveMessageRequest =new ReceiveMessageRequest();
                receiveMessageRequest.QueueUrl = queueURL;
                receiveMessageRequest.MaxNumberOfMessages = 10;
                receiveMessageRequest.AttributeNames = new List<string>() { "ApproximateReceiveCount" };

        
                var receiveMessageResponse = sqsClient.ReceiveMessageAsync(receiveMessageRequest); 

                foreach (var message in receiveMessageResponse.Result.Messages)
                {
                    string ReceiveCount_;
                    message.Attributes.TryGetValue("ApproximateReceiveCount", out ReceiveCount_);
                    listOfMessage.Add(
                         new QueueMessage
                         {
                             Body = message.Body,
                             Id = message.MessageId,
                             ReceiveCount = Int32.Parse(ReceiveCount_),
                             Size = message.Body.Length


                         });
                }
            }
            return listOfMessage;
        }
    }
}
