using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class MsSqlImport
    {

        private SqlHelper _helper;

        public MsSqlImport(Arguments args, ILogger log)
        {
            var cnxMgr = new ConnectionManager(args);
            Args = args;
            Db = new Database(Args);
            _helper = new SqlHelper(Args);
            Log = log;
        }

        public ILogger Log { get; private set; }

        public Arguments Args { get; private set; }
        public Database Db { get; private set; }

        public void DoImport()
        {
            using (var stream = new StreamReader(Args.FileName))
            {
                var count = 0L;
                var sb = new StringBuilder();
                while (!stream.EndOfStream)
                {
                    sb.AppendLine(_helper.GetCommand(stream.ReadLine()));
                    count++;
                    if(count > Args.BatchSize)
                    {
                        Db.ExecuteNonQuery(sb.ToString());
                        sb.Clear();
                        count = 0;
                    }
                }
                stream.Close();
            }
        }

        


    }
}
