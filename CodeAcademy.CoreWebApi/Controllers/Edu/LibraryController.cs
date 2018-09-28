using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.ViewModels;
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
        private IAppRepository _context;
        private IAuthRepository _auth;
        public LibraryController(IAppRepository context, IAuthRepository auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromForm] FilterModel model)
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
                List<PostViewModel> result = GenerateViewModel(posts);
                return Ok(result.OrderByDescending(x => x.LikeCount).ToList());
            }
            return Forbid();
        }


        [HttpGet]
        [Route("tagcloud")]
        public async Task<IActionResult> ShowTagCloud()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                int facultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int);
                var tags = await _context.GetTagsByFaculty(facultyId);
                return Ok(tags.Select(x => new LibraryTagViewModel(x)));
            }
            return Forbid();
        }

        private List<PostViewModel> GenerateViewModel(List<Post> posts)
        {
            List<PostViewModel> postViewModels = new List<PostViewModel>();

            List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            List<LinkViewModel> linkViewModels = new List<LinkViewModel>();
            List<BookViewModel> bookViewModels = new List<BookViewModel>();

            foreach (var post in posts)
            {
                switch (post.PostType)
                {
                    case "Article":
                        articleViewModels.Add(new ArticleViewModel(post as Article));
                        break;
                    case "Question":
                        Question q = post as Question;
                        QuestionViewModel viewModel = new QuestionViewModel(q);
                        if (q.Photo != null)
                        {
                            viewModel.Photo = q.Photo.Url;
                        }
                        questionViewModels.Add(viewModel);
                        break;
                    case "Link":
                        linkViewModels.Add(new LinkViewModel(post as Link));
                        break;
                    case "Book":
                        bookViewModels.Add(new BookViewModel(post as Book));
                        break;
                    default:
                        break;
                }
            }

            postViewModels.AddRange(articleViewModels);
            postViewModels.AddRange(bookViewModels);
            postViewModels.AddRange(questionViewModels);
            postViewModels.AddRange(linkViewModels);

            foreach (var vm in postViewModels)
            {
                if (vm.UserType == "Student")
                {
                    vm.GroupName = _context.GetUserGroup(vm.UserId);
                }
            }

            return postViewModels;
        }
    }
}