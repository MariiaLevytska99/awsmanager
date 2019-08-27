using System;
using System.Collections.Generic;
using awsmanagerLib.Models;
using awsmanagerLib.Configuration;
using Amazon;
using System.Configuration;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using System.Reflection;

namespace awsmanagerLib.Repositories
{
    public class LambdaRepository
    {
        private static RegionEndpoint Region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
        private static string access = ConfigurationManager.AppSettings["accessKey"].ToString();
        private static string secret = ConfigurationManager.AppSettings["secretKey"].ToString();
        readonly IAmazonLambda lambdaClient = new AmazonLambdaClient(access, secret, RegionEndpoint.USEast1);
        public List<Lambda> lambdaRepository { get; set; } = new List<Lambda>();

        public LambdaRepository()
        {
            lambdaRepository = GetLambdaRepository();
        }

        private List<Lambda> GetLambdaRepository()
        {
            var lambdaResponse = lambdaClient.ListFunctionsAsync(
                new ListFunctionsRequest());
            foreach (var function in lambdaResponse.Result.Functions)
            {
                
                var metrics = new LambdaMetricsRepository(function.FunctionName).MetricsRepository;
                LambdaStatus status;
                string value = metrics.Find(x => x.Title.Equals("ConcurrentExecutions")).Value;
                if (value.Equals('0'))
                {
                    status = LambdaStatus.DenyExecution;

                }
                else status = LambdaStatus.AllowExecution;
                lambdaRepository.Add(
                    new Lambda
                    {
                        FunctionName = function.FunctionName,
                        Description = function.Description,
                        Size = function.CodeSize,
                        Status = status,
                        Metrics = metrics

                    });
            }


            return lambdaRepository;
        }

        
        public void StopLambda(string functionName)
        {

            var res = lambdaClient.PutFunctionConcurrencyAsync(
                new PutFunctionConcurrencyRequest
                {
                    FunctionName = functionName,
                    ReservedConcurrentExecutions = 0
                });
            var lambda = lambdaRepository.Find(x => x.FunctionName.Equals(functionName));
            lambda.Status = LambdaStatus.DenyExecution;


        }

        public void ResumeLambda(string functionName)
        {
            var res = lambdaClient.DeleteFunctionConcurrencyAsync(
                new DeleteFunctionConcurrencyRequest
                {
                    FunctionName = functionName
                });
            var lambda = lambdaRepository.Find(x => x.FunctionName.Equals(functionName));
            lambda.Status = LambdaStatus.AllowExecution;
        }
    }
}
