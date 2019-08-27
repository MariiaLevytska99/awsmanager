using System;
using System.Collections.Generic;
using System.Text;

namespace awsmanagerLib.Models
{
    public class Code
    {
        public string CodeNumber { get; set; }
        public string Description { get; set; }

        public Code(string codeNumber, string description)
        {
            CodeNumber = codeNumber;
            Description = description;
        }
    }
}
