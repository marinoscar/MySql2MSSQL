using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public static class ArgValidator
    {
        public static void Required(string name, object arg)
        {
            if (arg != null) return;
            throw new FriendlyException(string.Format("Argument {0} is required", name));
        }

        public static void DoBasicValidation(Arguments args)
        {
            Required(Arguments.FileSwitch, args.FileName);
            Required(Arguments.HostSwitch, args.Host);
            Required(Arguments.DatabaseSwitch, args.Database);
            Required(Arguments.UserSwitch, args.User);
            Required(Arguments.PasswordSwitch, args.Password);
            Required(Arguments.FormatSwitch, args.Format);
            Required(Arguments.TableSwitch, args.TableName);
            Required(Arguments.TargetSwitch, args.Target);

        }
    }
}
