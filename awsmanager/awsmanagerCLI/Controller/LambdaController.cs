using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerLib.Repositories;
using awsmanagerLib.Models;

namespace awsmanagerCLI.Controller
{
   public class LambdaController
    {
        private List<Lambda> LambdaList { get; set; }
        private LambdaRepository LambdaRepository { get; set; }

        public LambdaController()
        {
            Console.WriteLine("Please wait ...");
            LambdaRepository = new LambdaRepository();
            LambdaList = LambdaRepository.lambdaRepository;
        }

        public List<Lambda> getListOfLambda()
        {
            return LambdaList;
        }
        public void StopFunction(int number)
        {
            string funcName = LambdaList[number - 1].FunctionName;
            LambdaRepository.StopLambda(funcName);
        }
        public void ResumeFunction(int number)
        {
            string funcName = LambdaList[number - 1].FunctionName;
            LambdaRepository.ResumeLambda(funcName);
        }
    }
}
