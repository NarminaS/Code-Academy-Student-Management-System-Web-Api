using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Attributes;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CodeAcademy.CoreWebApi.Controllers.Student
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public HomeController(IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig,
                                            IAuthRepository auth)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getallposts")]  
        public async Task<IActionResult> GetAllPosts()
        {
            List<PostViewModel> postViewModels = new List<PostViewModel>();

            List<Article> articles = await _context.GetAllArticles();
            List<ArticleViewModel> articleViewModels = articles.Select(x => new ArticleViewModel(x)).ToList();

            List<Question> questions = await _context.GetAllQuestions();
            List<QuestionViewModel> questionViewModels = questions.Select(x => new QuestionViewModel(x)).ToList();


            List<Link> links = await _context.GetAllLinks();
            List<LinkViewModel> linkViewModels = links.Select(x => new LinkViewModel(x)).ToList(); 

            List<Book> books = await _context.GetAllBooks();
            List<BookViewModel> bookViewModels = books.Select(x => new BookViewModel(x)).ToList();

            postViewModels.AddRange(articleViewModels);
            postViewModels.AddRange(bookViewModels);
            postViewModels.AddRange(questionViewModels);
            postViewModels.AddRange(linkViewModels);

            //List<PostViewModel> filtered = postViewModels.Where(x => x.FacultyId == this.GetLoggedUser(_auth, _context).FacultyId).ToList();

            List<PostViewModel> orderedByDate = postViewModels.OrderByDescending(x => x.DateAdded).ToList();
            return Ok(orderedByDate);
        }

        [HttpPost]
        [Route("addarticle")]
        public async Task<IActionResult> AddArticle([FromForm] ArticleModel model)
        {
            if (ModelState.IsValid)
            {
                string[] tags = model.Tags.Split(',');
                List<Tag> articleTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    articleTags.Add(t);
                }

                Article item = new Article
                {
                    HeadText = model.HeadText,
                    Text = model.Text,
                    AppIdentityUser = await _auth.FindUserById("fff5ec56-f16a-4bd8-a01e-2dbd8ccba678"),
                    FacultyId = 1,
                    //AppIdentityUser =  this.GetLoggedUser(_auth, _context),
                    //FacultyId =  this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
                };

                List<PostTag> artTags = new List<PostTag>();
                foreach (var tag in articleTags)
                {
                    artTags.Add(new PostTag() { Post = item, Tag = tag });
                }

                item.PostTags = artTags;

                await _context.Add(item);

                bool saved = _context.SaveAll();

                if (saved == true)
                {
                    return Ok(new ArticleViewModel(item));
                }
            }
            return BadRequest("Model is not valid");
        }

        [HttpPost]
        [Route("updatearticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleModel model)
        {
            if (ModelState.IsValid)
            {
                string[] tags = model.Tags.Split(',');
                List<Tag> articleTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    articleTags.Add(t);
                }

                Article item = await _context.GetByIdAsync<Article>(x => x.Id == model.Id);

                item.HeadText = model.Text;
                item.Text = model.Text;

                List<PostTag> newTagPosts = new List<PostTag>();
                foreach (var tag in articleTags)
                {
                    newTagPosts.Add(new PostTag() { Post = item, Tag = tag });
                }

                List<PostTag> oldTagPosts = await _context.GetPostTags(item);

                foreach (var tp in oldTagPosts)
                {
                    _context.Delete(tp);
                }

                item.PostTags = newTagPosts;

                _context.Update(item);
                bool saved = _context.SaveAll();
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
        [Route("deletearticle")]
        public async Task<IActionResult> DeleteArticle([FromForm] ArticleModel model)
        {
            AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
            if (currentuser != null)
            {
                if (ModelState.IsValid)
                {
                    Article item = await _context.GetByIdAsync<Article>(x => x.Id == model.Id);
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
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpGet]
        [Route("getallarticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            List<Article> articles = await _context.GetAllArticles();
            List<ArticleViewModel> articleViewModels = articles.Select(x => new ArticleViewModel(x)).ToList();
            //Filter by faculty
            //List<ArticleViewModel> filtered = articleViewModels.Where(x => x.FacultyId == this.GetLoggedUser(_auth, _context).FacultyId).ToList();

            if (articleViewModels.Count > 0)
                return Ok(articleViewModels);
            else
                return NotFound("No articles in database");
        }

        [HttpPost]
        [Route("addlink")]
        public async Task<IActionResult> AddLink([FromForm] LinkModel model)
        {
            if (ModelState.IsValid)
            {
                string[] tags = model.Tags.Split(',');
                List<Tag> linkTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    linkTags.Add(t);
                }

                Link item = new Link
                {
                    AppIdentityUser = await _auth.FindUserById("fff5ec56-f16a-4bd8-a01e-2dbd8ccba678"),
                    FacultyId = 1,
                    //AppIdentityUser =  this.GetLoggedUser(_auth, _context),
                    //FacultyId =  this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
                    HeadText = model.HeadText,
                    LinkUrl = model.LinkUrl
                };

                List<PostTag> lTags = new List<PostTag>();
                foreach (var tag in linkTags)
                {
                    lTags.Add(new PostTag() { Post = item, Tag = tag });
                }

                item.PostTags = lTags;
                await _context.Add(item);

                bool saved = _context.SaveAll();

                if (saved == true)
                {
                    return Ok(new LinkViewModel(item));
                }

            }
            return BadRequest("Model is not valid");
        }

        [Authorize]
        [HttpPost]
        [Route("updatelink")]
        public async Task<IActionResult> UpdateLink([FromForm] LinkModel model)
        {
            AppIdentityUser currentuser = this.GetLoggedUser( _auth, _context);
            if (currentuser != null)
            {
                if (model.UserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                {
                    if (ModelState.IsValid)
                    {
                        string[] tags = model.Tags.Split(',');
                        List<Tag> linkTags = new List<Tag>();
                        foreach (var tag in tags)
                        {
                            Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                            linkTags.Add(t);
                        }

                        Link item = await _context.GetByIdAsync<Link>(x => x.Id == model.Id);

                        item.HeadText = model.HeadText;
                        item.LinkUrl = model.LinkUrl;

                        List<PostTag> newTagPosts = new List<PostTag>();
                        foreach (var tag in linkTags)
                        {
                            newTagPosts.Add(new PostTag() { Post = item, Tag = tag });
                        }

                        List<PostTag> oldTagPosts = await _context.GetPostTags(item);

                        foreach (var tp in oldTagPosts)
                        {
                            _context.Delete(tp);
                        }

                        item.PostTags = newTagPosts;

                        _context.Update(item);
                        bool saved = _context.SaveAll();
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
            return Unauthorized();
        }

        [Authorize]
        [HttpPost]
        [Route("deletelink")]
        public async Task<IActionResult> DeleteLink([FromForm] LinkModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
            {   
                AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);

                if (model.UserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                {
                    if (ModelState.IsValid)
                    {
                        Link item = await _context.GetByIdAsync<Link>(x => x.Id == model.Id);
                        if (item != null)
                        {
                            _context.Delete(item);
                            bool result = _context.SaveAll();

                            if (result == true)
                                return Ok(item);
                            else
                                return BadRequest("Model cannot be deleted");
                        }
                        else
                        {
                            return NotFound("Model not found");
                        }
                    }
                    return BadRequest("Model is not valid");
                }
                return BadRequest($"{currentuser.Name}, you don't have a permission");
            }
            return Forbid();
        }

        [HttpGet]
        [Route("getalllinks")]
        public async Task<IActionResult> GetAllLinks()
        {
            List<Link> links = await _context.GetAllLinks();
            List<LinkViewModel> linkViewModels = links.Select(x => new LinkViewModel(x)).ToList();

            //Filter by faculty
            //List<ArticleViewModel> filtered = articleViewModels.Where(x => x.FacultyId == this.GetLoggedUser(_auth, _context).FacultyId).ToList();

            if (linkViewModels.Count > 0)
                return Ok(linkViewModels);
            else
                return NotFound("No articles in database");
        }

        [HttpPost]
        [Route("addquestion")]
        public async Task<IActionResult> AddQuestion([FromForm] QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                string[] tags = model.Tags.Split(',');
                List<Tag> questionTags = new List<Tag>();
                foreach (var tag in tags)
                {
                    Tag t = await _context.GetByNameAsync<Tag>(x => x.Name.ToLower() == tag.Trim().ToLower());
                    questionTags.Add(t);
                }

                Question item = new Question
                {
                    AppIdentityUser = await _auth.FindUserById("fff5ec56-f16a-4bd8-a01e-2dbd8ccba678"),
                    FacultyId = 1,
                    //AppIdentityUser =  this.GetLoggedUser(_auth, _context),
                    //FacultyId =  this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
                    HeadText = model.HeadText,
                    Text = model.Text
                };

                if (model.Photo != null)
                {
                    PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                    Photo photo = upload.Upload(model.Photo);
                    await _context.Add(photo);
                    if(_context.SaveAll())
                    {
                        item.Photo = photo;
                        item.PhotoId = photo.Id;
                    }                    
                }

                List<PostTag> tagPosts = new List<PostTag>();
                foreach (var tag in questionTags)
                {
                    tagPosts.Add(new PostTag() { Post = item, Tag = tag });
                }

                item.PostTags = tagPosts;

                await _context.Add(item);

                bool saved = _context.SaveAll();
                try
                {
                    if (saved == true)
                    {
                        return Ok(new QuestionViewModel(item));
                    }
                }
                catch (Exception ex)
                {
                   //...
                }
            }
            return Ok();
        }
    }
}