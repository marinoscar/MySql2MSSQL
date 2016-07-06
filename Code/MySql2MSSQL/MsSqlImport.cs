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
        public MsSqlImport(Arguments args)
        {
            var cnxMgr = new ConnectionManager(args);
            Args = args;
            Db = new Database(Args);
        }

        private List<int> _numberIndex;

        public Arguments Args { get; private set; }
        public Database Db { get; private set; }

        public void DoImport()
        {
            using (var stream = new StreamReader(Args.FileName))
            {
                while (!stream.EndOfStream)
                {

                }
                stream.Close();
            }
        }

        private string GetCommand(string line)
        {
            var items = line.Split(",".ToCharArray()).Select(i => i.Remove(0,1)).Select(i => i.Remove(i.Length - 1, 1)).ToArray();
            var sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} VALUES ({1});", Args.TableName);
            return null;
        }

        private string GetValues(List<string> values)
        {
            if(_numberIndex == null)
            {

            }
            return null;
        }

        private void InitNumberIndex(List<int> values)
        {
            
        }


    }
}
