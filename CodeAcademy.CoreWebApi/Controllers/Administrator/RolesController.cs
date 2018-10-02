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
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IAuthRepository _auth;
        private IAppRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public RolesController(IAuthRepository auth, IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                var result = _auth.GetRoles();
                return Ok(result);
            }
            return Forbid();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] RoleModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                AppIdentityRole saved;
                if (ModelState.IsValid)
                {
                    saved = await _auth.CreateRole(model.Name);

                    if (saved != null)
                    {
                        return Ok(saved);
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
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                AppIdentityRole item = await _auth.GetRoleByIdAsync(id);
                if (item != null)
                {
                    var result = await _auth.DeleteRole(item);

                    if (result != null)
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
        public async Task<IActionResult> Update([FromForm] RoleModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                if (ModelState.IsValid)
                {
                    AppIdentityRole item = await _auth.GetRoleByIdAsync(model.Id);
                    item.Name = model.Name;
                    var saved = await _auth.UpdateRole(item);
                    if (saved != null)
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