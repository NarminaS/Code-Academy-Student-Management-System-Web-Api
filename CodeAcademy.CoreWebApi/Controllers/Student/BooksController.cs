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
using CodeAcademy.CoreWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public BooksController(IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig,
                                            IAuthRepository auth)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.GetAllBooks();
            return Ok(result.Select(x=>new BookViewModel(x)));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] BookModel model)
        {
            bool saved;
                if (ModelState.IsValid)
                {
                string[] tags = model.Tags.Split(',');
                List<Tag> bookTags = new List<Tag>();
                foreach (var tag in tags)
                    {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    bookTags.Add(t);
                    }

                    PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                    Photo photo = upload.Upload(model.Cover);

                    PdfUploadCloudinary pdfUpload = new PdfUploadCloudinary(_cloudinaryConfig);
                    File file = pdfUpload.Upload(model.Book);

                Book item = new Book
                {
                    Name = model.Name,
                    AppIdentityUserId = "9f5732e2-ca9c-4b25-b7b7-23f3fb7c7e87",
                    //User = await this.GetLoggedUser(_auth, _context),
                    Photo = photo,
                    File = file,
                    Author = model.Author,
                    LanguageId = model.LanguageId,
                    Year = model.Year,
                    Pages = model.Pages,
                    Description = model.Description,
                    FacultyId = model.FacultyId
                };

                List<PostTag> tagPosts = new List<PostTag>();
                foreach (var tag in bookTags)
                  {
                    tagPosts.Add(new PostTag() { Post = item, Tag = tag });
                  }

                item.PostTags = tagPosts;

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

            Book item = await _context.GetByIdAsync<Book>(x => x.Id == id);
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
        public async Task<IActionResult> Update([FromForm] BookModel model)
        {
            bool saved;
            if (ModelState.IsValid)
            {
                string[] tags = model.Tags.Split(',');
                List<Tag> bookTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    bookTags.Add(t);
                }

                Book item = await _context.GetByIdAsync<Book>(x => x.Id == model.Id);

                PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                Photo photo = upload.Upload(model.Book);

                PdfUploadCloudinary pdfUpload = new PdfUploadCloudinary(_cloudinaryConfig);
                File file = pdfUpload.Upload(model.Book);

                item.Name = model.Name;
                item.Author = model.Author;
                item.Description = model.Description;
                item.Year = model.Year;
                item.File = file;
                item.Photo = photo;
                item.LanguageId = model.LanguageId;
                item.Pages = model.Pages;
                item.FacultyId = model.FacultyId;
                item.Photo = photo;

                List<PostTag> newTagPosts = new List<PostTag>();
                foreach (var tag in bookTags)
                {
                    newTagPosts.Add(new PostTag() { Post = item, Tag = tag });
                }

                List<PostTag> oldTagPosts = await _context.GetPostTags(item);

                foreach (var tp in oldTagPosts)
                {
                    _context.Delete(tp);
                }

                item.PostTags = newTagPosts;

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

        [HttpPost]
        [Route("getbylanguage")]
        public async Task<IActionResult> FilterBooksByLanguage([FromForm] int languageId)
        {
            List<Book> all = await _context.GetAllBooks();
            List<Book> filtered = all.Where(x => x.LanguageId == languageId).ToList();

            List<BookViewModel> model = filtered.Select(x => new BookViewModel(x)).ToList();
            if (filtered.Count > 0)
                return Ok(model);
            else
                return NotFound("No books were found");
        }

        [HttpPost]
        [Route("getbyfaculty")]
        public async Task<IActionResult> FilterBooksByFaculty([FromForm] int facultyId)
        {
            List<Book> all = await _context.GetAllBooks();
            List<Book> filtered = all.Where(x => x.FacultyId == facultyId).ToList();
            
            List<BookViewModel> model = filtered.Select(x => new BookViewModel(x)).ToList();
            if (filtered.Count > 0)
                return Ok(model);
            else
                return NotFound("No books were found");
        }

        [HttpPost]
        [Route("getbyname")]
        public async Task<IActionResult> FindByName([FromForm] BookSearchModel model)
        {
            List<Book> foundx = await _context.GetAllBooks();
            var found = foundx.Where(x => x.Name.ToLower().Contains(model.Name.ToLower())).ToList();
            List<BookViewModel> result = found.Select(x => new BookViewModel(x)).ToList();
            if (found != null)
                return Ok(result);
            else
                return NotFound("No book was found");
        }

    }
}