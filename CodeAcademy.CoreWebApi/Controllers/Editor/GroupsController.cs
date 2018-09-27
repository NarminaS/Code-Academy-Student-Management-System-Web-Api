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
    public class GroupsController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public GroupsController(IAppRepository context, IAuthRepository auth, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _context = context;
            _auth = auth;
            _cloudinaryConfig = cloudinaryConfig;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllTeachers()
        {
            List<Group> groups = await _context.GetAllGroups();
            return Ok(groups);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddGroup([FromForm]GroupModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Editor" }))
            {
                if (ModelState.IsValid)
                {
                    PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);    
                    Photo photo = upload.Upload(model.Photo);

                    Group item = new Group
                    {
                        Name = model.Name,
                        FacultyId = model.FacultyId,
                        LessonEndDate = model.LessonEndDate,
                        LessonStartDate = model.LessonStartDate,
                        LessonHourId = model.LessonHourId,
                        LessonStatusId = 1,
                        Photo = photo,
                        RoomId = model.RoomId
                    };

                    Teacher teacher = await _context.GetByIdAsync<Teacher>(x => x.Id == model.TeacherId);
                    TeacherGroup teacherGroup = new TeacherGroup { Group = item, Teacher = teacher };
                    await _context.Add(item);
                    await _context.Add(teacherGroup);
                    if (_context.SaveAll())
                    {
                        return Ok(item);
                    }
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }
    }
}