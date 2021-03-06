﻿using System;
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
            _helper = new SqlHelper(Args);
        }
        
        private DateTime _startedOn;
        private SqlHelper _helper;
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
                        Log.LogProgress(totalCount, count, _startedOn);
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

        private void ProcessRow(IDataReader reader, StreamWriter stream)
        {
            if (Args.Format == "csv") ProcessCsv(reader, stream); else ProcessSql(reader, stream);
        }

        private void ProcessCsv(IDataReader reader, StreamWriter stream)
        {
            stream.WriteLine(ToCsv(reader));
        }

        private string ToCsv(IDataReader reader)
        {
            var values = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                values.Add(string.Format(@"""{0}""", Parse(reader.GetValue(i))));
            }
            return string.Join(",", values);
        }

        private void ProcessSql(IDataReader reader, StreamWriter stream)
        {
            stream.WriteLine(_helper.GetCommand(ToCsv(reader)));
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
