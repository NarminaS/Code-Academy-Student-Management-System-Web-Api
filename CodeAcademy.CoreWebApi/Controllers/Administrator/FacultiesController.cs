using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        private IAppRepository _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public FacultiesController(IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.GetFacultiesAsync();
            return Ok(result);
        }


        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] FacultyModel model)
        {
            bool saved;
            if (ModelState.IsValid)
            {
                PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                Photo photo = upload.Upload(model.Photo);

                Faculty item = new Faculty
                {
                    Name = model.Name,
                    LessonHour = model.LessonHour,
                    Photo = photo
                };

                await _context.Add(photo);
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

            Faculty item = await _context.GetByIdAsync<Faculty>(x => x.Id == id);
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
        public async Task<IActionResult> Update([FromForm] FacultyModel model)
        {
            bool saved;
            if (ModelState.IsValid)
            {
                Faculty item = await _context.GetByIdAsync<Faculty>(x => x.Id == model.Id);
                PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                Photo photo = upload.Upload(model.Photo);

                item.Name = model.Name;
                item.LessonHour = model.LessonHour;
                item.Photo = photo;

                await _context.Add(photo);
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