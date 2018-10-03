using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject.ToView;
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
    public class NotificationsController : ControllerBase
    {
        private Logger _logger;
        private IAppRepository _context;
        private IAuthRepository _auth;

        public NotificationsController(IAppRepository context, IAuthRepository auth, Logger logger)
        {
            _logger = logger;
            _auth = auth;
            _context = context;
        }

        [HttpGet]
        [Route("getunread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Notification> unread = await _context.GetUnread(this.GetLoggedUser(_auth, _context).Id);
                    return Ok(unread.Select(x => new NotificationViewModel(x)));
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Notification> all = await _context.GetAllNotifications(this.GetLoggedUser(_auth, _context).Id);
                    return Ok(all.Select(x => new NotificationViewModel(x)));
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> ReadNotification([FromForm]int id)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    Notification notification = await _context.GetByIdAsync<Notification>(x => x.Id == id);
                    if (notification != null)
                    {
                        notification.IsVisited = true;
                        _context.Update(notification);
                        if (await _context.SaveAll())
                        {
                            return Ok(notification.IsVisited);
                        }
                        return BadRequest("Error updation notification");
                    }
                    return NotFound("Notification not found");
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }
    }
}