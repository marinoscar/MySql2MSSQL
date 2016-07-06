using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.Write(message);
        }

        public void LogLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
