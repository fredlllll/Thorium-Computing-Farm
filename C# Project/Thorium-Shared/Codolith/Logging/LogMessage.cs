using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Codolith.Logging
{
    public class LogMessage
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string DateTimeFormat { get; set; } = Logger.METRIC_DATETIME_FORMAT;
        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public List<string> Tags { get; set; } = new List<string>();
        public string Message { get; set; } = "";


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.ToString(DateTimeFormat, CultureInfo));
            sb.Append("]");
            foreach(var tag in Tags)
            {
                sb.Append("[");
                sb.Append(tag);
                sb.Append("]");
            }
            sb.Append(": ");
            sb.Append(Message);
            return sb.ToString();
        }
    }
}
