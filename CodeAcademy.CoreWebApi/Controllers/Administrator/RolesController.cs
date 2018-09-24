using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IAuthRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public RolesController(IAuthRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            var result = _context.GetRoles();
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] RoleModel model)
        {
            AppIdentityRole saved;
            if (ModelState.IsValid)
            {
                saved = await _context.CreateRole(model.Name);

                if (saved != null)
                {
                    return Ok(saved);
                }
            }
            return BadRequest("Model is not valid");
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            AppIdentityRole item = await _context.GetRoleByIdAsync(id);
            if (item != null)
            {
               var result = await _context.DeleteRole(item);

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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] RoleModel model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityRole item = await _context.GetRoleByIdAsync(model.Id);
                item.Name = model.Name;
                var saved = await _context.UpdateRole(item);
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
    }
}