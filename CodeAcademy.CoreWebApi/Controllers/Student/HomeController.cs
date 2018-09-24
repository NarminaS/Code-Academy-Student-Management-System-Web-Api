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
            return Ok();
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
                    AppIdentityUserId = "9f5732e2-ca9c-4b25-b7b7-23f3fb7c7e87",
                    FacultyId = 1,
                    Photo = null
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
                try
                {
                    bool saved = _context.SaveAll();
                    if (saved == true)
                    {
                        return Ok(new ArticleViewModel(item));
                    }
                }
                catch (Exception ex)
                {
                    //
                }
            }
            else
            {
                return BadRequest("Model is not valid");
            }
            return Ok();
        }






    }
}