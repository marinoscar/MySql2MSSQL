using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class Database
    {

        private DbProviderFactory _factory;
        private IDbConnection _conn;

        public Database(DbConnection conn)
        {
            _factory = DbProviderFactories.GetFactory(conn);
            _conn = conn;
        }


        public int ExecuteNonQuery(string command)
        {
            int result = 0;
            WithCommand(command, (cmd) => {
                result = cmd.ExecuteNonQuery();
            });
            return result;
        }

        public object ExecuteScalar(string command)
        {
            object result = null;
            WithCommand(command, (cmd) => {
                result = cmd.ExecuteScalar();
            });
            return result;
        }

        public void WhileReading(string command, Action<IDataReader> whileReading)
        {
            WithCommand(command, (cmd) => {
                using(var reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        whileReading(reader);
                    }
                }
            });
        }

        public void WithCommand(string command, Action<IDbCommand> withCommand)
        {
            using (var conn = _factory.CreateConnection())
            {
                conn.ConnectionString = _conn.ConnectionString;
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.CommandTimeout = _conn.ConnectionTimeout;
                    withCommand(cmd);
                }
                conn.Close();
            }
        }

    }
}
