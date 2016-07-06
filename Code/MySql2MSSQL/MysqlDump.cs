using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class MysqlDump
    {
        public MysqlDump(Arguments args)
        {
            Args = args;
        }

        public Arguments Args { get; private set; }


    }
}
