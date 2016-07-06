using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class ConsoleLogger : ILogger
    {

        int _expected = 0;

        public void Log(string message)
        {
            Console.Write(message);
        }

        public void LogLine(string message)
        {
            Console.WriteLine(message);
        }

        public void LogProgress(double total, double count, DateTime processStartTime)
        {
            const int barSize = 30;
            var progress = (double)((count / total) * 100);
            var progressStr = progress.ToString("N2");
            var barProgress = (int)((progress * barSize) / 100);
            if (((progress % 5) == 0) || count == 1)
            {
                var now = DateTime.Now;
                var duration = now.Subtract(processStartTime).TotalSeconds;
                _expected = (int)((total * duration) / count);
            }
            var bar = string.Format("[{0}] {1}% ETA: {2}", (new string('=', barProgress)).PadRight(barSize), progressStr, processStartTime.AddSeconds(_expected).ToString("ddd, HH:mm:ss"));
            Log(string.Format("\r{0}", bar));
        }
    }
}
