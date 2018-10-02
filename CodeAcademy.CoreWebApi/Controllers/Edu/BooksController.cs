using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.NotifyHelpers;
using CodeAcademy.CoreWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Edu
{
    [Authorize]
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
            List<BookViewModel> viewModels = new List<BookViewModel>();
            foreach (Book book in result)
            {
                BookViewModel viewModel = new BookViewModel(book);
                if (book.AppIdentityUser.UserType == "Student")
                {
                    viewModel.GroupName = _context.GetUserGroup(book.AppIdentityUserId);
                }
                viewModels.Add(viewModel);
            }
            return Ok(viewModels);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get([FromBody] int id)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                Book book = await _context.GetBook(id);
                if (book != null)
                {
                    if (book.AppIdentityUser.UserType == "Student")
                    {
                        BookViewModel viewModel = new BookViewModel(book);
                        viewModel.GroupName = _context.GetUserGroup(book.AppIdentityUserId);
                        return Ok(viewModel);
                    }
                    return Ok(new BookViewModel(book));
                }
                return NotFound("Book not found");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] BookModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher" }))
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
                        AppIdentityUser = this.GetLoggedUser(_auth, _context),
                        Photo = photo,
                        File = file,
                        Author = model.Author,
                        Language = await _context.GetByIdAsync<Language>(x => x.Id == model.LanguageId),
                        Year = model.Year,
                        Pages = model.Pages,
                        Faculty = await _context.GetByIdAsync<Faculty>(x => x.Id == model.FacultyId)
                    };

                    List<PostTag> tagPosts = new List<PostTag>();
                    foreach (var tag in bookTags)
                    {
                        tagPosts.Add(new PostTag() { Post = item, Tag = tag });
                    }

                    item.PostTags = tagPosts;

                    await _context.Add(photo);
                    await _context.Add(item);

                    await _context.SaveAll();
                    var broadcast = new Notifier(_context, _auth);
                    await _context.AddRange(await broadcast.NewBook(item));

                    saved = await _context.SaveAll();
                    if (saved == true)
                    {
                        if (item.AppIdentityUser.UserType == "Student")
                        {
                            BookViewModel viewModel = new BookViewModel(item);
                            viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                            return Ok(viewModel);
                        }
                        return Ok(new BookViewModel(item));
                    }
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] PostDeleteModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
            {
                AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                if (ModelState.IsValid)
                {
                    if (model.PostUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                    {
                        Book item = await _context.GetByIdAsync<Book>(x => x.Id == model.PostId);
                        if (item != null)
                        {
                            _context.Delete(item);
                            bool result = await _context.SaveAll();

                            if (result == true)
                                return Ok("Success");
                            else
                                return BadRequest("Model cannot be  deleted");
                        }
                        else
                        {
                            return NotFound("Model not found");
                        }
                    }
                    return BadRequest($"{currentuser.Name}, you don't have a permission");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] BookModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
            {
                AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                if (model.UserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                {
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
                        bool saved = await _context.SaveAll();
                        if (saved == true)
                        {
                            if (item.AppIdentityUser.UserType == "Student")
                            {
                                BookViewModel viewModel = new BookViewModel(item);
                                viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                                return Ok(viewModel);
                            }
                            return Ok(new BookViewModel(item));
                        }
                        else
                        {
                            return BadRequest("Item cannot be updated");
                        }
                    }
                    return BadRequest("Model is not valid");
                }
                return BadRequest($"{currentuser.Name}, you don't have a permission");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("getbylanguage")]
        public async Task<IActionResult> FilterBooksByLanguage([FromForm] int languageId)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                List<Book> all = await _context.GetAllBooks();
                List<Book> filtered = all.Where(x => x.LanguageId == languageId).ToList();

                List<BookViewModel> model = filtered.Select(x => new BookViewModel(x)).ToList();
                if (filtered.Count > 0)
                    return Ok(model);
                else
                    return NotFound("No books were found");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("getbyfaculty")]
        public async Task<IActionResult> FilterBooksByFaculty([FromForm] int facultyId)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                List<Book> all = await _context.GetAllBooks();
                List<Book> filtered = all.Where(x => x.FacultyId == facultyId).ToList();

                List<BookViewModel> model = filtered.Select(x => new BookViewModel(x)).ToList();
                if (filtered.Count > 0)
                    return Ok(model);
                else
                    return NotFound("No books were found");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("getbyname")]
        public async Task<IActionResult> FindByName([FromForm] BookSearchModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                List<Book> foundx = await _context.GetAllBooks();
                var found = foundx.Where(x => x.Name.ToLower().Contains(model.Name.ToLower())).ToList();
                List<BookViewModel> result = found.Select(x => new BookViewModel(x)).ToList();
                if (result.Count > 0)
                    return Ok(result);
                else
                    return NotFound("No book was found");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("getbytag")]
        public async Task<IActionResult> FilterBooksByTag([FromBody] TagFilterModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                if (ModelState.IsValid)
                {
                    List<Book> books = await _context.GetAllBooks();
                    List<Book> filtered = new List<Book>();
                    foreach (var book in books)
                    {
                        foreach (var pt in await _context.GetPostTags(book))
                        {
                            if (pt.PostId == book.Id && pt.TagId == model.TagId)
                            {
                                filtered.Add(book);
                            }
                        }
                    }
                    if (filtered.Count > 0)
                    {
                        return Ok(filtered.Select(x => new BookViewModel(x)));
                    }
                    return NotFound("No books with this tag were found");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("approve")]
        public async Task<IActionResult> ApproveBook([FromBody] PostApproveModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Teacher" }))
            {
                Teacher current = this.GetLoggedUser(_auth, _context) as Teacher;
                AppIdentityUser author = await _auth.FindUserById(model.PostAuthorId);
                if (ModelState.IsValid)
                {
                    Book book = await _context.GetBook(model.PostId);
                    if (book != null && book.IsApproved == false)
                    {
                        book.IsApproved = true;
                        _context.Update(book);

                        await _context.Add(new Notifier(_context, _auth).Approved(book, current));

                        if (await _context.SaveAll())
                        {
                            author.Point += 15;
                            await _auth.UpdateUser(author);
                            return Ok(new SuccesApproveModel(current));
                        }
                        return BadRequest("Error approving book");
                    }
                    return NotFound("Book not found or is already approved");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("disapprove")]
        public async Task<IActionResult> DisapproveBook([FromBody]PostApproveModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Teacher" }))
            {
                Teacher current = this.GetLoggedUser(_auth, _context) as Teacher;
                AppIdentityUser author = await _auth.FindUserById(model.PostAuthorId);
                if (ModelState.IsValid)
                {
                    Book book = await _context.GetByIdAsync<Book>(x => x.Id == model.PostId);
                    if (book != null && book.IsApproved == false)
                    {
                        _context.Delete(book);

                        await _context.Add(new Notifier(_context, _auth).Disapproved(book, current, model.Reason));

                        if (await _context.SaveAll())
                        {
                            return Ok($"The book has been deleted by {current.Name} {current.Surname}");
                        }
                        return BadRequest("Error disapproving book");
                    }
                    return NotFound("Book not found or is already approved");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }
    }
}
