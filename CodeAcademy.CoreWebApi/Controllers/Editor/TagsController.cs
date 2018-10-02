using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Editor
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        public TagsController(IAppRepository context, IAuthRepository auth)
        {
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Editor" }))
            {
                var result = await _context.GetTagsAsync();
                return Ok(result);
            }
            return Forbid();
        }



        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] TagModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Editor" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    if (await IsUniqueTag(model.Name, model.FacultyId))
                    {
                        Tag item = new Tag
                        {
                            Name = model.Name,
                            FacultyId = model.FacultyId
                        };
                        await _context.Add(item);
                        saved = await _context.SaveAll();
                        if (saved == true)
                        {
                            return Ok(item);
                        }
                    }
                    else
                    {
                        return BadRequest("This tag already exists in database");
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
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Editor" }))
            {
                Tag item = await _context.GetByIdAsync<Tag>(x => x.Id == id);
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
        public async Task<IActionResult> Update([FromForm] TagModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Editor" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    Tag item = await _context.GetByIdAsync<Tag>(x => x.Id == model.Id);

                    item.Name = model.Name;
                    item.FacultyId = model.FacultyId;

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

        private async Task<bool> IsUniqueTag(string name, int facultyId)
        {
            var tags = await _context.GetTagsAsync();
            var found = tags.Where(x => x.Name.ToLower() == name.ToLower() && x.FacultyId == facultyId).FirstOrDefault();
            if (found != null)
                return false;
            else
                return true;
        }
    }
}