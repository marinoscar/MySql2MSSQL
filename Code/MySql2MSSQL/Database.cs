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
        private ConnectionManager _manager;

        public Database(Arguments args)
        {
            _manager = new ConnectionManager(args);
            _factory = DbProviderFactories.GetFactory(_manager.GetConnection());
        }

        public Arguments Args { get; private set; }

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
            WithConnection((conn) => {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.CommandTimeout = Args.Timeout;
                    withCommand(cmd);
                }
            });
        }

        public void WithConnection(Action<IDbConnection> withConnection)
        {
            using(var conn = _factory.CreateConnection())
            {
                conn.ConnectionString = _manager.GetConnectionString();
                conn.Open();
                withConnection(conn);
                conn.Close();
            }
        }

    }
}
