using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Domain.Log
{
    public class AppLog
    {
        public long LogId { get; set; }
        public string? Action { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public int StatusCode { get; set; }
        public int? UserId { get; set; }
        public string? Method { get; set; }
        public string? Path { get; set; }
        public long DurationMs { get; set; }
    }
}
