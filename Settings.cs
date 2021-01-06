using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report
{
    public class Settings
    {
        public string SQLConn { get; set; }
        public string SMSApiURL { get; set; }
        public string SMSApiUsername { get; set; }
        public string SMSApiPass { get; set; }
        public string SMSApiFrom { get; set; }
    }
}