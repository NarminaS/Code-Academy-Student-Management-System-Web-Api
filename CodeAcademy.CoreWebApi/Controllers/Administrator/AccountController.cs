using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAuthRepository _context;
        public AccountController(IAuthRepository context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("data is null");
            }

            var user = await _context.FindUserById(userId);
            if (user != null)
            {
                bool confirmed = await _context.ConfirmEmail(user, code) != null;
                if (confirmed)
                    return Ok(user);
                else
                    return BadRequest();
            }
            return BadRequest();
        }
    }
}