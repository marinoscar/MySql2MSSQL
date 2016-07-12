using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql2MSSQL
{
    public class SqlHelper
    {
        private Dictionary<int, bool> _numberIndex;

        public SqlHelper(Arguments args)
        {
            Args = args;
        }
        public Arguments Args { get; private set; }

        public string GetCommand(string line)
        {
            var items = line.Split(",".ToCharArray()).Select(i => i.Remove(0, 1)).Select(i => i.Remove(i.Length - 1, 1)).ToList();
            var sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} VALUES ({1});", Args.TableName, GetValues(items));
            return sb.ToString();
        }

        private string GetValues(List<string> values)
        {
            var items = new List<string>();
            if (_numberIndex == null)
                InitNumberIndex(values);
            for (var i = 0; i < values.Count; i++)
            {
                if (_numberIndex[i])
                    items.Add(values[i]);
                else
                {
                    var val = values[i];
                    if (val == null || val == string.Empty)
                        items.Add("NULL");
                    else
                        items.Add(string.Format("'{0}'", val.Replace("'", "''")));
                }
            }
            return string.Join(",", items);
        }

        private void InitNumberIndex(List<string> values)
        {
            _numberIndex = new Dictionary<int, bool>();
            double d;
            long l;
            for (int i = 0; i < values.Count; i++)
            {
                _numberIndex[i] = (double.TryParse(values[i], out d) || long.TryParse(values[i], out l));
            }
        }
    }
}
