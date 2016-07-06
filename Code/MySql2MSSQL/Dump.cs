using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class Dump
    {
        public Dump(Arguments args, ILogger log)
        {
            var connMgr = new ConnectionManager(args);
            Args = args;
            Log = log;
            Db = new Database(Args);
        }
        private int _expected;
        private DateTime _startedOn;
        public Database Db { get; private set; }
        public Arguments Args { get; private set; }
        public ILogger Log { get; private set; }

        public void DoDump()
        {
            double count = 0;
            long offset = Args.Offset;
            LogLine(string.Format("Starting the dump process on, {0}", Args.TableName));
            var totalCount = Convert.ToDouble(Db.ExecuteScalar(string.Format("SELECT COUNT(*) FROM {0}", Args.TableName)));
            LogLine(string.Format("{0} records to process in {1}", totalCount, Args.TableName));
            LogLine("");
            using (var stream = new StreamWriter(Args.FileName))
            {
                _startedOn = DateTime.Now;
                while (offset < totalCount)
                {
                    var cmd = GetSelect(offset);
                    Db.WhileReading(cmd, (r) =>
                    {
                        ProcessRow(r, stream);
                        count++;
                        ProgressBar(totalCount, count);
                    });
                    offset += Args.BatchSize;
                }
                stream.Close();
            }
        }

        private string GetSelect(long offset)
        {
            return string.Format("SELECT * FROM {0} LIMIT {1},{2}", Args.TableName, offset, Args.BatchSize);
        }

        private void ProgressBar(double total, double count)
        {
            const int barSize = 30;
            var progress = (double)((count / total) * 100);
            var progressStr = progress.ToString("N2");
            var barProgress = (int)((progress * barSize) / 100);
            if (((progress % 5) == 0) || count == 1)
            {
                var now = DateTime.Now;
                var duration = now.Subtract(_startedOn).TotalSeconds;
                _expected = (int)((total * duration) / count);
            }
            var bar = string.Format("[{0}] {1}% ETA: {2}", (new string('=', barProgress)).PadRight(barSize), progressStr, _startedOn.AddSeconds(_expected).ToString("ddd, HH:mm:ss"));
            Log.Log(string.Format("\r{0}", bar));
        }

        private void ProcessRow(IDataReader reader, StreamWriter stream)
        {
            if (Args.Format == "csv") ProcessCsv(reader, stream); else ProcessSql(reader, stream);
        }

        private void ProcessCsv(IDataReader reader, StreamWriter stream)
        {
            var values = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                values.Add(string.Format(@"""{0}""", Parse(reader.GetValue(i))));
            }
            stream.WriteLine(string.Join(",", values));
        }

        private void ProcessSql(IDataReader reader, StreamWriter stream)
        {
            throw new FriendlyException("Sql Server file not implemented yet");
        }

        private string Parse(object val)
        {
            if (val.GetType() == typeof(DateTime)) return ((DateTime)val).ToString("o");
            return Convert.ToString(val);
        }

        private void LogLine(string message)
        {
            Log.LogLine(string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), message));
        }


    }
}
