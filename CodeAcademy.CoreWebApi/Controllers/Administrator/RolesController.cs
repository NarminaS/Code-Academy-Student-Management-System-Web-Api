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
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private Logger _logger;
        private IAuthRepository _auth;
        private IAppRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public RolesController(IAuthRepository auth,
                                IAppRepository context, Logger logger,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
            this._logger = logger;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
                {
                    var result = _auth.GetRoles();
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
        public async Task<IActionResult> Add([FromForm] RoleModel model)
        {
            try
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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] RoleModel model)
        {
            try
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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }
    }
}