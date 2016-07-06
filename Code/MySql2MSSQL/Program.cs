using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            if(args.Length <= 0 || (arguments.ContainsSwtich("--help") || arguments.ContainsSwtich("-h") || arguments.ContainsSwtich("/help")))
            {
                arguments.PrintHelp();
                return;
            }
            try
            {
                ArgValidator.DoBasicValidation(arguments);
                if(arguments.Target == "mysql")
                {
                    var dump = new Dump(arguments, new ConsoleLogger());
                    dump.DoDump();
                }
            }
            catch (FriendlyException fex)
            {
                Console.WriteLine();
                Console.WriteLine(fex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
