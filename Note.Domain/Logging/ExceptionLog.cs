using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Domain.Log
{
    public class ExceptionLog
    {
        public string Action { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Request { get; set; }
        public int? UserId { get; set; }
        public string TraceId { get; set; }
    }
}
