using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.ViewModels;
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
        private IAppRepository _context;
        private IAuthRepository _auth;

        public NotificationsController(IAppRepository context, IAuthRepository auth)
        {
            _auth = auth;
            _context = context;
        }

        [HttpGet]
        [Route("getunread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            List<Notification> unread = await _context.GetUnread(this.GetLoggedUser(_auth, _context).Id);
            return Ok(unread.Select(x=>new NotificationViewModel(x)));
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllNotifications()
        {
            List<Notification> all = await _context.GetAllNotifications(this.GetLoggedUser(_auth, _context).Id);
            return Ok(all.Select(x => new NotificationViewModel(x)));
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> ReadNotification([FromForm]int id)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                Notification notification = await _context.GetByIdAsync<Notification>(x => x.Id == id);
                if (notification != null)
                {
                    notification.IsVisited = true;
                    _context.Update(notification);
                    if (_context.SaveAll())
                    {
                        return Ok(notification.IsVisited);
                    }
                    return BadRequest("Error updation notification");
                }
                return NotFound("Notification not found");
            }
            return Forbid();
        }
    }
}