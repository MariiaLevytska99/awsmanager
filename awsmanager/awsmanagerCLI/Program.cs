using System;
using System.Configuration;
using awsmanagerLib;
using awsmanagerLib.Configuration;
using awsmanagerCLI.View;

namespace awsmanagerCLI
{
    class Program
    {
        public static ServiceSection ConfigSection { get; private set; }

        static void Main(string[] args)
        {

            var view = new MainView();
        }
    }
}
