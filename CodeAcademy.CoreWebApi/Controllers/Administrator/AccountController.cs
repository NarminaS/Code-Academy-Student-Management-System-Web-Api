using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAuthRepository _auth;
        private IAppRepository _context;
        private Logger _logger;
        public AccountController(IAuthRepository auth, IAppRepository context, Logger logger)
        {
            _auth = auth;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return BadRequest("Data is null");
                }

                var user = await _auth.FindUserById(userId);
                if (user != null)
                {
                    bool confirmed = await _auth.ConfirmEmail(user, code) != null;
                    if (confirmed)
                        return Ok($"{user.Email} has been confirmed");
                    else
                        return BadRequest("Confirmation error");
                }
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpPost]
        [Route("changeavatar")]
        public async Task<IActionResult> ChangeProfilePhoto()
        {
            return Ok();
        }
    }
}