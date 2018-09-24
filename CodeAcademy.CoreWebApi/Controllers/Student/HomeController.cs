using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
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
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            List<PostViewModel> postViewModels = new List<PostViewModel>();
            try
            {
                List<Article> articles = await _context.GetPostsByType<Article>();
                List<ArticleViewModel> articleViewModels = articles.Select(x => new ArticleViewModel(x)).ToList();

                List<Book> books = await _context.GetAllBooks();
                List<BookViewModel> bookViewModels = books.Select(x => new BookViewModel(x)).ToList();

                postViewModels.AddRange(articleViewModels);
                postViewModels.AddRange(bookViewModels);
                postViewModels.OrderByDescending(x => x.DateAdded).ToList();

            }
            catch (Exception ex)
            {
                //
            }
            return Ok(postViewModels);
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
                try
                {
                    if (saved == true)
                    {
                        return Ok(new ArticleViewModel(item));
                    }
                }
                catch (Exception ex)
                {
                    //...
                }
            }
            return BadRequest("Model is not valid");
        }
    }
}