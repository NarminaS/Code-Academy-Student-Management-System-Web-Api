using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using CodeAcademy.CoreWebApi.DataTransferObject.ToView;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeAcademy.CoreWebApi.Controllers.Edu
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private Logger _logger;
        private IAppRepository _context;
        private IAuthRepository _auth;
        public LibraryController(IAppRepository context, IAuthRepository auth, Logger logger)
        {
            _context = context;
            _auth = auth;
            _logger = logger;
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromForm] FilterModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    int _facultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int);

                    string PostType = model.PostType ?? String.Empty;
                    int LanguageId = model.LanguageId ?? default(int);
                    int TagId = model.TagId ?? default(int);
                    int FacultyId = model.FacultyId ?? _facultyId;

                    if (model.LanguageId != null)
                    {
                        List<Book> books = await _context.FilterBooks(FacultyId, LanguageId, TagId);
                        return Ok(books.Select(x => new BookViewModel(x)));
                    }

                    List<Post> posts = await _context.FilterPosts(FacultyId, TagId, PostType);
                    List<PostViewModel> result = new ViewModelGenerator(_context, _auth).CreatePostListViewModel(posts);
                    return Ok(result.OrderByDescending(x => x.LikeCount).ToList());
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


        [HttpGet]
        [Route("tagcloud")]
        public async Task<IActionResult> ShowTagCloud()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    int facultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int);
                    var tags = await _context.GetTagsByFaculty(facultyId);
                    return Ok(tags.Select(x => new LibraryTagViewModel(x)));
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