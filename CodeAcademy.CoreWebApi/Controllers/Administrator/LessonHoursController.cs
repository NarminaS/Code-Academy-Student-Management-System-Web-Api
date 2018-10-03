using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LessonHoursController : ControllerBase
    {
        private Logger _logger;
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public LessonHoursController(IAppRepository context, 
                                        IAuthRepository auth, Logger logger,
                                         IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
            this._logger = logger;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    var result = await _context.GetLessonHoursAsync();
                    return Ok(result);
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
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] LessonHourModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    bool saved;
                    if (ModelState.IsValid)
                    {
                        LessonHour item = new LessonHour
                        {
                            Name = model.Name,
                            BeginHour = model.BeginHour,
                            BeginMinute = model.BeginMinute,
                            EndHour = model.EndHour,
                            EndMinute = model.EndMinute,
                            Friday = model.Friday,
                            Monday = model.Monday,
                            Saturday = model.Saturday,
                            Sunday = model.Sunday,
                            Thursday = model.Thursday,
                            Tuesday = model.Tuesday,
                            Wednesday = model.Wednesday
                        };

                        await _context.Add(item);
                        saved = await _context.SaveAll();
                        if (saved == true)
                        {
                            return Ok(item);
                        }
                    }
                    return BadRequest("Model is not valid");
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
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    LessonHour item = await _context.GetByIdAsync<LessonHour>(x => x.Id == id);
                    if (item != null)
                    {
                        _context.Delete(item);
                        bool result = await _context.SaveAll();

                        if (result == true)
                            return Ok(item);
                        else
                            return BadRequest("Model cannot be  deleted");
                    }
                    else
                    {
                        return NotFound("Model not found");
                    }
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
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] LessonHourModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    bool saved;
                    if (ModelState.IsValid)
                    {
                        LessonHour item = await _context.GetByIdAsync<LessonHour>(x => x.Id == model.Id);

                        item.Name = model.Name;
                        item.Wednesday = model.Wednesday;
                        item.Monday = model.Monday;
                        item.Saturday = model.Saturday;
                        item.Sunday = model.Sunday;
                        item.Thursday = model.Thursday;
                        item.Tuesday = model.Tuesday;
                        item.Friday = model.Friday;
                        item.BeginMinute = model.BeginMinute;
                        item.BeginHour = model.BeginHour;
                        item.EndHour = model.EndHour;
                        item.EndMinute = model.EndMinute;

                        _context.Update(item);
                        saved = await _context.SaveAll();
                        if (saved == true)
                        {
                            return Ok(item);
                        }
                        else
                        {
                            return BadRequest("Item cannot be updated");
                        }
                    }
                    return BadRequest("Model is not valid");
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