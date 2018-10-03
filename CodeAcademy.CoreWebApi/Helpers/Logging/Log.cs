using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Logging
{
    public class Log
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string InfoType { get; set; }

        public string Exception { get; set; }

        public string RequestUrl { get; set; }  
    }
}
