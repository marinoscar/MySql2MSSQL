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

        public MsSqlImport(Arguments args)
        {
            var cnxMgr = new ConnectionManager(args);
            Args = args;
            Db = new Database(Args);
            _helper = new SqlHelper(Args);
        }


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

        


    }
}
