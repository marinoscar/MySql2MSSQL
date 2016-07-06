using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class ConnectionManager
    {
        public ConnectionManager(Arguments args)
        {
            Args = args;
        }

        public Arguments Args { get; private set; }

        public string GetConnectionString()
        {
            const string mysqlTemplate = "Server={server};Database={db};Uid={user};Pwd={password};";
            const string mssqlTemplate = "Server={server};Database={db};User Id={user};Password={password};";
            return IsMysql() ? DoReplace(mysqlTemplate) : DoReplace(mssqlTemplate);
        }

        public DbConnection GetConnection()
        {
            DbConnection result = null;
            var conStr = GetConnectionString();
            if (IsMysql())
                result = new MySqlConnection(conStr);
            else
                result = new SqlConnection(conStr);
            return result;
        }


        private string DoReplace(string template)
        {
            return template
                .Replace("{server}", Args.Host)
                .Replace("{db}", Args.Database)
                .Replace("{user}", Args.User)
                .Replace("{password}", Args.Password);
        }

        private bool IsMysql()
        {
            return Args.Target == "mysql";
        }
    }
}
