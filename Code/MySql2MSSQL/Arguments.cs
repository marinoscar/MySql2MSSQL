using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class Arguments
    {
        private List<string> _args;

        public Arguments(IEnumerable<string> args)
        {
            _args = new List<string>(args);
        }

        public bool ContainsSwtich(string name)
        {
            return _args.Contains(name);
        }

        private string GetSwitchValue(string name)
        {
            if (!ContainsSwtich(name)) return null;
            var item = _args.First(i => i == name);
            var itemIndex = _args.IndexOf(item);
            if ((itemIndex + 1) > _args.Count) return null;
            return _args[itemIndex + 1];
        }

        public void PrintHelp()
        {
            const string help = @"
--help: prints this help guide
--host: the name or ip of the database host to connect to 
--db: the name of the database
--user: database user name
--password: database password 
--table: the name of the table to work on
--file: the name of the file to work on
--format: the format to work on. csv or sql
--target: the target engine either mysql or mssql
--timeout: the connection time out, defaults to 0
--offset: the record start count
--batchSize: the size of the batchs to get from the server
";
            Console.WriteLine(help);
        }

        public const string HostSwitch = "--host";
        public string Host { get { return GetSwitchValue(HostSwitch); } }
        public const string DatabaseSwitch = "--db";
        public string Database { get { return GetSwitchValue(DatabaseSwitch); } }
        public const string UserSwitch = "--user";
        public string User { get { return GetSwitchValue(UserSwitch); } }
        public const string PasswordSwitch = "--password";
        public string Password { get { return GetSwitchValue(PasswordSwitch); } }
        public const string TableSwitch = "--table";
        public string TableName { get { return GetSwitchValue(TableSwitch); } }
        public const string FileSwitch = "--file";
        public string FileName { get { return GetSwitchValue(FileSwitch); } }
        public const string FormatSwitch = "--format";
        public string Format { get { return GetSwitchValue(FormatSwitch); } }
        public const string TargetSwitch = "--target";
        public string Target
        {
            get
            {
                if (!ContainsSwtich(TargetSwitch)) return "mysql";
                return GetSwitchValue(TargetSwitch);
            }
        }
        public const string TimeoutSwitch = "--timeout";
        public int Timeout
        {
            get
            {
                if (!ContainsSwtich(TimeoutSwitch)) return 7200;
                return Convert.ToInt32(GetSwitchValue(TimeoutSwitch));
            }
        }
        public const string OffsetSwitch = "--offset";
        public long Offset
        {
            get
            {
                if (!ContainsSwtich(OffsetSwitch)) return 0;
                return Convert.ToInt32(GetSwitchValue(OffsetSwitch));
            }
        }

        public const string BatchSizeSwitch = "--batchSize";
        public long BatchSize
        {
            get
            {
                if (!ContainsSwtich(BatchSizeSwitch)) return 10000;
                return Convert.ToInt32(GetSwitchValue(BatchSizeSwitch));
            }
        }

    }
}
