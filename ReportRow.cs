using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Report
{
    public class ReportRow
    {
        public int id { get; set; }
        public string content { get; set; }
        public bool is_test { get; set; }
        public string mobi { get; set; }
        public string email { get; set; }
        public string info { get; set; }
        public DateTime last_update { get; set; }

        public ReportRow(IDataRecord record) {
            id = (int)record[0];
            content = (string)record[1];
            is_test = (bool)record[2];
            mobi = (string)(record.IsDBNull(3) ? "" : record.GetValue(3));
            email = (string)(record.IsDBNull(4) ? "" : record.GetValue(4));
            info = (string)(record.IsDBNull(5) ? "" : record.GetValue(5));
            last_update = (DateTime)(record.IsDBNull(6) ? default(DateTime) : record.GetValue(6));
        }
    }
}
