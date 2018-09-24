using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonStatusesController : ControllerBase
    {
        private IAppRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public LessonStatusesController(IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.GetLessonStatusesAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] LessonStatusModel model)
        {
            bool saved;
            if (ModelState.IsValid)
            {
                LessonStatus item = new LessonStatus
                {
                    Name = model.Name,
                };

                await _context.Add(item);
                saved = _context.SaveAll();
                if (saved == true)
                {
                    return Ok(item);
                }
            }
            return BadRequest("Model is not valid");
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {

            LessonStatus item = await _context.GetByIdAsync<LessonStatus>(x => x.Id == id);
            if (item != null)
            {
                _context.Delete(item);
                bool result = _context.SaveAll();

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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] LessonStatusModel model)
        {
            bool saved;
            if (ModelState.IsValid)
            {
                LessonStatus item = await _context.GetByIdAsync<LessonStatus>(x => x.Id == model.Id);
                item.Name = model.Name;
                _context.Update(item);
                saved = _context.SaveAll();
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
    }
}