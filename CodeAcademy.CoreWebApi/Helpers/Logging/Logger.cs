using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Logging
{
    public class Logger
    {
        private Serilog.ILogger _logger;
        public Logger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void LogInInfo(string username, string infotype, string requestUrl)
        {
            _logger.ForContext("Username", username)
                   .ForContext("Infotype", infotype)
                   .ForContext("RequestUrl", requestUrl)
                   .Information("User {@username} logged in", username);
        }

        public void LogExeption(Exception ex, string username, string requestUrl)
        {
            _logger.ForContext("Username" , username)
                   .ForContext("RequestUrl", requestUrl)
                   .Error(@"Exeption : {ex} \r\n Stack trace {st}",ex.Message,ex.StackTrace);
        }
    }
}
