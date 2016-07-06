using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class FriendlyException : Exception
    {
        public FriendlyException(string message) : base(message)
        {

        }

        public FriendlyException(string message, Exception inner): base(message, inner)
        {

        }

        public FriendlyException()
        {

        }
    }
}
