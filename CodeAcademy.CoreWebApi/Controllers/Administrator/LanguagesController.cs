using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
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
    public class LanguagesController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public LanguagesController(IAppRepository context, IAuthRepository auth,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                var result = await _context.GetLanguagesAsync();
                return Ok(result);
            }
            return Forbid();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] LanguageModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    Language item = new Language
                    {
                        Name = model.Name,
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

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                Language item = await _context.GetByIdAsync<Language>(x => x.Id == id);
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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] LanguageModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    Language item = await _context.GetByIdAsync<Language>(x => x.Id == model.Id);
                    item.Name = model.Name;
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
    }
}