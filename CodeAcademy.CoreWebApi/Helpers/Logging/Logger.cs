using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;

namespace CodeAcademy.CoreWebApi.Helpers.Logging
{
    public class Logger
    {
        private Serilog.ILogger _logger;
        private IConfiguration _configuration;

        public Logger(Serilog.ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void LogInInfo(string username, string infotype, string requestUrl)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("Infotype", infotype)
                       .ForContext("RequestUrl", requestUrl)
                       .Information("User {username} logged in", username);
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
            }
        }

        public void LogOutInfo(string username, string infotype, string requestUrl)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("Infotype", infotype)
                       .ForContext("RequestUrl", requestUrl)
                       .Information("User {username} logged out", username);
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
            }
        }

        public void LogException(Exception ex, string username, string requestUrl)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("RequestUrl", requestUrl)
                       .ForContext("Infotype", "Exception")
                       .Error(ex, "Exeption : {ex} \r\n Stack trace {st}", ex.Message, ex.StackTrace);
            }
            catch (Exception exp)
            {
                LogException(exp, username, requestUrl);
            }
        }

        public void LogApproveBook(Book book, string username, string requestUrl)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("Infotype", "BookApprove")
                       .ForContext("RequestUrl", requestUrl)
                       .Information("{teacher} approved book \"{book}\" ", username, book.Name);
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
            }
        }

        public void LogApproveArticle(Article article, string username, string requestUrl)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("Infotype", "ArticleApprove")
                       .ForContext("RequestUrl", requestUrl)
                       .Information("{teacher} approved article.Id : {id} ", username, article.Id);
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
            }
        }

        public void LogDisapproveBook(Book book, string username, string requestUrl, string reason)
        {
            try
            {
                _logger.ForContext("Username", username)
                       .ForContext("Infotype", "Disapprove")
                       .ForContext("RequestUrl", requestUrl)
                       .Information("{teacher} disapproved book \"{book}\" Reason - {reason} ", username, book.Name, reason);
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
            }
        }

        public async Task<List<Log>> GetLogs(string username, string requestUrl)
        {
            try
            {
                List<Log> logs = new List<Log>();
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Con1")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Logs", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    logs.Add(new Log
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = DateTime.Parse(reader.GetValue(4).ToString()),
                                        Exception = reader.GetValue(5).ToString(),
                                        InfoType = reader.GetValue(8).ToString(),
                                        LogLevel = reader.GetValue(3).ToString(),
                                        Message = reader.GetValue(1).ToString(),
                                        RequestUrl = reader.GetValue(9).ToString(),
                                        Username = reader.GetValue(7).ToString()
                                    });
                                }
                            }
                        }
                    }
                }
                return logs;
            }
            catch (Exception ex)
            {
                LogException(ex, username, requestUrl);
                return new List<Log>();
            }
        }
    }
}
