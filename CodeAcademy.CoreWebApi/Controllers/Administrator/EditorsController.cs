using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
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
        private IAuthRepository _context;
        private IAppRepository _app;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public EditorsController(IAuthRepository context,IAppRepository app,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._app = app;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            if (this.ValidRoleForAction(_app, _context, new string[] { "Admin" }))
            {
                var result = await _context.GetUsersByRole("Editor");
                return Ok(result);
            }
            return Forbid();
        }



        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] EditorModel model)
        {
            if (this.ValidRoleForAction(_app, _context, new string[] { "Admin" }))
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
                    if (await _context.FindUserByEmail(model.Email) == null)
                    {
                        await _context.Register(editor, model.Password);
                        saved = await _context.AddUserToRole(editor, "Editor") != null;
                        if (saved == true)
                        {
                            var urlHelper = HttpContext.RequestServices.GetRequiredService<IUrlHelper>();
                            await this.SendConfirmaitionMail(editor, _context, urlHelper);
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


        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            if (this.ValidRoleForAction(_app, _context, new string[] { "Admin" }))
            {
                AppIdentityUser item = await _context.FindUserById(id);
                if (item != null)
                {
                    bool result = await _context.DeleteUser(item) != null;

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


        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] EditorModel model)
        {
            if (this.ValidRoleForAction(_app, _context, new string[] { "Admin" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    AppIdentityUser item = await _context.FindUserById(model.Id);

                    item.Name = model.Name;
                    item.Surname = model.Surname;
                    item.BirthDate = model.BirthDate;

                    saved = await _context.UpdateUser(item) != null;
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
    }
}