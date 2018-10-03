using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeAcademy.CoreWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private Logger _logger;
        private IAppRepository _context;
        private IAuthRepository _auth;

        public LogsController(Logger logger, IAppRepository context, IAuthRepository auth)
        {
            this._logger = logger;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> ReadAllLogs()
        {
            var arguments = this.GetBaseData(_context, _auth); 
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Log> allLogs = await _logger.GetLogs(arguments.Email, arguments.Path);
                    if (allLogs.Count > 0)
                        return Ok(allLogs);
                    else
                        return NotFound("No logs");
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }
    }
}