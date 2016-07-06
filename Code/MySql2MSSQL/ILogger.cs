using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public interface ILogger
    {
        void Log(string message);
        void LogLine(string message);
    }
}
