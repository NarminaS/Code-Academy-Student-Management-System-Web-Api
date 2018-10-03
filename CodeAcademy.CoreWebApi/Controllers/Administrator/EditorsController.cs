using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EditorsController : ControllerBase
    {
        private Logger _logger;
        private IAuthRepository _auth;
        private IAppRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public EditorsController(IAuthRepository context, IAppRepository app, Logger logger,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._auth = context;
            this._context = app;
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
                    var result = await _auth.GetUsersByRole("Editor");
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
        public async Task<IActionResult> Add([FromForm] EditorModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    bool saved;
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser editor = new AppIdentityUser
                        {
                            Name = model.Name,
                            Surname = model.Surname,
                            BirthDate = model.BirthDate,
                            Email = model.Email,
                            GenderId = model.GenderId,
                            PhotoId = 1,
                            UserName = model.Email
                        };
                        if (await _auth.FindUserByEmail(model.Email) == null)
                        {
                            await _auth.Register(editor, model.Password);
                            saved = await _auth.AddUserToRole(editor, "Editor") != null;
                            if (saved == true)
                            {
                                var urlHelper = HttpContext.RequestServices.GetRequiredService<IUrlHelper>();
                                await this.SendConfirmaitionMail(editor, _auth, urlHelper);
                                return Ok(editor);
                            }
                        }
                        else
                        {
                            return BadRequest($"User with {model.Email} email already exists");
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
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    AppIdentityUser item = await _auth.FindUserById(id);
                    if (item != null)
                    {
                        bool result = await _auth.DeleteUser(item) != null;

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
        public async Task<IActionResult> Update([FromForm] EditorModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    bool saved;
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser item = await _auth.FindUserById(model.Id);

                        item.Name = model.Name;
                        item.Surname = model.Surname;
                        item.BirthDate = model.BirthDate;

                        saved = await _auth.UpdateUser(item) != null;
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