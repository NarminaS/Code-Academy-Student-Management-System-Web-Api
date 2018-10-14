using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using CodeAcademy.CoreWebApi.DataTransferObject.ToView;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using CodeAcademy.CoreWebApi.Helpers.NotifyHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Edu
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private Logger _logger;
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public HomeController(IAppRepository context,
                                      IOptions<CloudinarySettings> cloudinaryConfig,
                                            IAuthRepository auth, Logger logger)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
            this._logger = logger;
        }

        [HttpGet]
        [Route("getallposts")]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<PostViewModel> postViewModels = new List<PostViewModel>();

                    List<Article> articles = await _context.GetAllArticles();
                    List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
                    foreach (var ar in articles)
                    {
                        ArticleViewModel viewModel = new ArticleViewModel(ar);
                        if (ar.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(ar.AppIdentityUserId);
                        }
                        articleViewModels.Add(viewModel);
                    }

                    List<Question> questions = await _context.GetAllQuestions();
                    List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
                    foreach (var q in questions)
                    {
                        QuestionViewModel viewModel = new QuestionViewModel(q);
                        if (q.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(q.AppIdentityUserId);
                        }
                        if (q.Photo != null)
                        {
                            viewModel.Photo = q.Photo.Url;
                        }
                        questionViewModels.Add(viewModel);
                    }


                    List<Link> links = await _context.GetAllLinks();
                    List<LinkViewModel> linkViewModels = new List<LinkViewModel>();
                    foreach (var l in links)
                    {
                        LinkViewModel viewModel = new LinkViewModel(l);
                        if (l.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(l.AppIdentityUserId);
                        }
                        linkViewModels.Add(viewModel);
                    }

                    List<Book> books = await _context.GetAllBooks();
                    List<BookViewModel> bookViewModels = new List<BookViewModel>();
                    foreach (Book book in books)
                    {
                        BookViewModel viewModel = new BookViewModel(book);
                        if (book.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(book.AppIdentityUserId);
                        }
                        bookViewModels.Add(viewModel);
                    }

                    postViewModels.AddRange(articleViewModels);
                    postViewModels.AddRange(bookViewModels);
                    postViewModels.AddRange(questionViewModels);
                    postViewModels.AddRange(linkViewModels);

                    int _facultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int);
                    if (_facultyId != default(int))
                    {
                        List<PostViewModel> filtered = postViewModels.Where(x => x.FacultyId == _facultyId).ToList();
                        List<PostViewModel> orderedByDatef = filtered.OrderByDescending(x => x.DateAdded).ToList();

                        return Ok(orderedByDatef);
                    }

                    List<PostViewModel> orderedByDate = postViewModels.OrderByDescending(x => x.DateAdded).ToList();
                    return Ok(orderedByDate);
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

        [HttpPost]
        [Route("addarticle")]
        public async Task<IActionResult> AddArticle([FromForm] ArticleModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
                            AppIdentityUser = this.GetLoggedUser(_auth, _context),
                            FacultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
                        };

                        List<PostTag> artTags = new List<PostTag>();
                        foreach (var tag in articleTags)
                        {
                            artTags.Add(new PostTag() { Post = item, Tag = tag });
                        }

                        item.PostTags = artTags;
                        await _context.Add(item);
                        bool saved = await _context.SaveAll();

                        item.AppIdentityUser.Point += 20;
                        await _auth.UpdateUser(item.AppIdentityUser);

                        if (saved == true)
                        {
                            await _context.Add(new Notifier(_context, _auth).SharePost(item, 20));
                            await _context.SaveAll();
                            ArticleViewModel viewModel = new ArticleViewModel(item);
                            if (item.AppIdentityUser.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }
                        return BadRequest("Error adding new article");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("updatearticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                    if (model.UserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
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
                            bool saved = await _context.SaveAll();
                            if (saved == true)
                            {
                                ArticleViewModel viewModel = new ArticleViewModel(item);
                                if (item.AppIdentityUser.UserType == "Student")
                                {
                                    viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                                }
                                return Ok(viewModel); ;
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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpPost]
        [Route("deletearticle")]
        public async Task<IActionResult> DeleteArticle([FromBody] PostDeleteModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                    if (ModelState.IsValid)
                    {
                        if (model.PostUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                        {
                            Article item = await _context.GetByIdAsync<Article>(x => x.Id == model.PostId);
                            if (item != null)
                            {
                                _context.Delete(item);
                                bool result = await _context.SaveAll();

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
                        return BadRequest($"{currentuser.Name}, you don't have a permission");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("approvearticle")]
        public async Task<IActionResult> ApproveArticle([FromBody] PostApproveModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Teacher" }))
                {
                    Teacher current = this.GetLoggedUser(_auth, _context) as Teacher;
                    AppIdentityUser author = await _auth.FindUserById(model.PostAuthorId);
                    if (ModelState.IsValid)
                    {
                        Article article = await _context.GetByIdAsync<Article>(x => x.Id == model.PostId);
                        if (article != null && article.IsApproved == false)
                        {
                            article.IsApproved = true;
                            _context.Update(article);

                            author.Point += 20;
                            await _auth.UpdateUser(author);

                            await _context.Add(new Notifier(_context, _auth).Approved(article, current));
                            if (await _context.SaveAll())
                            {
                                _logger.LogApproveArticle(article, current.Email, Request.Path);
                                return Ok(new SuccesApproveModel(current));
                            }
                        }
                        return NotFound("Article not found or is already approved");
                    }
                    return BadRequest("Model is not valid");
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
        [Route("getallarticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Article> articles = await _context.GetAllArticles();
                    List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
                    foreach (var ar in articles)
                    {
                        ArticleViewModel viewModel = new ArticleViewModel(ar);
                        if (ar.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(ar.AppIdentityUserId);
                        }
                        articleViewModels.Add(viewModel);
                    }

                    if (articleViewModels.Count > 0)
                        return Ok(articleViewModels);
                    else
                        return NotFound("No articles in database");
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
        [Route("getarticle")]
        public async Task<IActionResult> GetArticle([FromBody]GetByIdModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        Article article = await _context.GetArticle(model.Id.Value);
                        if (article != null)
                        {
                            ArticleViewModel viewModel = new ArticleViewModel(article);
                            if (article.AppIdentityUser.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(article.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }
                        return NotFound("Article not found");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("addlink")]
        public async Task<IActionResult> AddLink([FromForm] LinkModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
                            AppIdentityUser = this.GetLoggedUser(_auth, _context),
                            FacultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
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
                        bool saved = await _context.SaveAll();

                        item.AppIdentityUser.Point += 5;
                        await _auth.UpdateUser(item.AppIdentityUser);

                        if (saved == true)
                        {
                            await _context.Add(new Notifier(_context, _auth).SharePost(item, 5));
                            await _context.SaveAll();
                            LinkViewModel viewModel = new LinkViewModel(item);
                            if (item.AppIdentityUser.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }

                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("updatelink")]
        public async Task<IActionResult> UpdateLink([FromForm] LinkModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
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
                            bool saved = await _context.SaveAll();
                            if (saved == true)
                            {
                                LinkViewModel viewModel = new LinkViewModel(item);
                                if (item.AppIdentityUser.UserType == "Student")
                                {
                                    viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                                }
                                return Ok(viewModel);
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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpPost]
        [Route("deletelink")]
        public async Task<IActionResult> DeleteLink([FromBody] PostDeleteModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);

                    if (model.PostUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                    {
                        if (ModelState.IsValid)
                        {
                            Link item = await _context.GetByIdAsync<Link>(x => x.Id == model.PostId);
                            if (item != null)
                            {
                                _context.Delete(item);
                                bool result = await _context.SaveAll();

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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }

        [HttpGet]
        [Route("getalllinks")]
        public async Task<IActionResult> GetAllLinks()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Link> links = await _context.GetAllLinks();
                    List<LinkViewModel> linkViewModels = new List<LinkViewModel>();
                    foreach (var link in links)
                    {
                        LinkViewModel viewModel = new LinkViewModel(link);
                        if (link.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(link.AppIdentityUserId);
                        }
                        linkViewModels.Add(viewModel);
                    }

                    if (linkViewModels.Count > 0)
                        return Ok(linkViewModels);
                    else
                        return NotFound("No links in database");
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
        [Route("getlink")]
        public async Task<IActionResult> GetLink([FromBody]GetByIdModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        Link link = await _context.GetLink(model.Id.Value);
                        if (link != null)
                        {
                            LinkViewModel viewModel = new LinkViewModel(link);
                            if (link.AppIdentityUser.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(link.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }
                        return NotFound("Link not found");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("addquestion")]
        public async Task<IActionResult> AddQuestion([FromForm] QuestionModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
                            AppIdentityUser = this.GetLoggedUser(_auth, _context),
                            FacultyId = this.GetLoggedUser(_auth, _context).FacultyId ?? default(int),
                            HeadText = model.HeadText,
                            Text = model.Text
                        };

                        if (model.Photo != null)
                        {
                            PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                            Photo photo = upload.Upload(model.Photo);
                            await _context.Add(photo);
                            if (await _context.SaveAll())
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
                        bool saved = await _context.SaveAll();

                        item.AppIdentityUser.Point += 10;
                        await _auth.UpdateUser(item.AppIdentityUser);

                        if (saved == true)
                        {
                            await _context.Add(new Notifier(_context, _auth).SharePost(item, 10));
                            await _context.SaveAll();

                            QuestionViewModel viewModel = new QuestionViewModel(item);
                            if (item.Photo != null)
                                viewModel.Photo = item.Photo.Url;
                            if (item.AppIdentityUser.UserType == "Student")
                                viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                            return Ok(viewModel);
                        }
                    }
                    return Ok();
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

        [HttpPost]
        [Route("updatequestion")]
        public async Task<IActionResult> UpdateQuestion([FromForm] QuestionModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                    if (model.UserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
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

                            Question item = await _context.GetQuestion(model.Id);

                            item.HeadText = model.HeadText;
                            item.Text = model.Text;

                            if (model.Photo != null)
                            {
                                PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                                Photo photo = upload.Upload(model.Photo);
                                await _context.Add(photo);
                                if (await _context.SaveAll())
                                {
                                    item.Photo = photo;
                                    item.PhotoId = photo.Id;
                                }
                            }

                            if (model.Photo == null && item.Photo != null)
                            {
                                item.Photo = null;
                            }

                            List<PostTag> newTagPosts = new List<PostTag>();
                            foreach (var tag in questionTags)
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
                            bool saved = await _context.SaveAll();
                            if (saved == true)
                            {
                                QuestionViewModel viewModel = new QuestionViewModel(item);
                                if (item.Photo != null)
                                    viewModel.Photo = item.Photo.Url;
                                if (item.AppIdentityUser.UserType == "Student")
                                    viewModel.GroupName = _context.GetUserGroup(item.AppIdentityUserId);
                                return Ok(viewModel);
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
            catch (Exception ex)
            {
                var arguments = this.GetBaseData(_context, _auth);
                _logger.LogException(ex, arguments.Email, arguments.Path);
                return BadRequest($"{ex.GetType().Name} was thrown.");
            }
        }


        [HttpPost]
        [Route("deletequestion")]
        public async Task<IActionResult> DeleteQuestion([FromBody] PostDeleteModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                    if (ModelState.IsValid)
                    {
                        if (model.PostUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                        {
                            Question item = await _context.GetByIdAsync<Question>(x => x.Id == model.PostId);
                            if (item != null)
                            {
                                _context.Delete(item);
                                bool result = await _context.SaveAll();

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
                        return BadRequest($"{currentuser.Name}, you don't have a permission");
                    }
                    return BadRequest("Model is not valid");
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
        [Route("getallquestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    List<Question> questions = await _context.GetAllQuestions();
                    List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
                    foreach (var q in questions)
                    {
                        QuestionViewModel viewModel = new QuestionViewModel(q);
                        if (q.Photo != null)
                        {
                            viewModel.Photo = q.Photo.Url;
                        }
                        if (q.AppIdentityUser.UserType == "Student")
                        {
                            viewModel.GroupName = _context.GetUserGroup(q.AppIdentityUserId);
                        }
                        questionViewModels.Add(viewModel);
                    }

                    if (questionViewModels.Count > 0)
                        return Ok(questionViewModels);
                    else
                        return NotFound("No articles in database");
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
        [Route("getquestion")]
        public async Task<IActionResult> GetQuestion([FromBody]GetByIdModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        Question question = await _context.GetQuestion(model.Id.Value);
                        if (question != null)
                        {
                            QuestionViewModel viewModel = new QuestionViewModel(question);
                            if (question.AppIdentityUser.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(question.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }
                        return NotFound("Question not found");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("likepost")]
        public async Task<IActionResult> LikePost([FromBody] LikeModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser current = this.GetLoggedUser(_auth, _context);
                        Post postToLike = await _context.GetPost(model.PostId);
                        AppIdentityUser postAuthor = await _auth.FindUserById(model.PostUserId);

                        if (model.IsLiked == true)
                        {
                            if (postAuthor.Id == current.Id)
                            {
                                return BadRequest("You can't like your own post");
                            }

                            if (postToLike != null)
                            {
                                Like likeToAdd = new Like
                                {
                                    AppIdentityUser = current,
                                    Post = postToLike
                                };

                                await _context.Add(likeToAdd);

                                if (await _auth.CheckUserRole(current, "Teacher"))
                                {
                                    postAuthor.Point += 10;
                                    await _auth.UpdateUser(postAuthor);
                                    await _context.Add(new Notifier(_context, _auth).Like(likeToAdd, 10));
                                }
                                else
                                {
                                    postAuthor.Point += 1;
                                    await _auth.UpdateUser(postAuthor);
                                    await _context.Add(new Notifier(_context, _auth).Like(likeToAdd, 1));
                                }

                                bool saved = await _context.SaveAll();
                                if (saved == true)
                                    return Ok(new LikeViewModel(postToLike.Likes.Count, false));
                                else
                                    return BadRequest("Error adding like to post");
                            }
                            return NotFound("Post not found");
                        }
                        else
                        {
                            Like likeToDelete = await _context.GetCommentLike(model.PostId, current.Id);
                            if (likeToDelete != null)
                            {
                                _context.Delete(likeToDelete);

                                if (await _auth.CheckUserRole(current, "Teacher"))
                                {
                                    postAuthor.Point -= 10;
                                    await _auth.UpdateUser(postAuthor);
                                    await _context.Add(new Notifier(_context, _auth).Dislike(likeToDelete, 10));
                                }
                                else
                                {
                                    postAuthor.Point -= 1;
                                    await _auth.UpdateUser(postAuthor);
                                    await _context.Add(new Notifier(_context, _auth).Dislike(likeToDelete, 1));
                                }

                                bool saved = await _context.SaveAll();
                                if (saved == true)
                                    return Ok(new LikeViewModel(postToLike.Likes.Count, false));
                                else
                                    return BadRequest("Error disliking the post");
                            }
                            return NotFound("Like not found");
                        }
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("addcomment")]
        public async Task<IActionResult> AddComment([FromForm] CommentModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser current = this.GetLoggedUser(_auth, _context);
                        Question question = await _context.GetQuestion(model.PostId);
                        if (question != null)
                        {
                            Comment item = new Comment
                            {
                                Post = question,
                                Text = model.Text,
                                User = current
                            };

                            await _context.Add(item);
                            bool saved = await _context.SaveAll();

                            current.Point += 5;
                            await _auth.UpdateUser(item.User);

                            if (saved == true)
                            {
                                await _context.Add(new Notifier(_context, _auth).Comment(item));
                                await _context.SaveAll();

                                CommentViewModel viewModel = new CommentViewModel(item);
                                if (current.UserType == "Student")
                                {
                                    viewModel.GroupName = _context.GetUserGroup(current.Id);
                                }
                                return Ok(viewModel);
                            }
                            return BadRequest("Error adding comment");
                        }
                        return NotFound("Question not found!");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("updatecomment")]
        public async Task<IActionResult> UpdateComment([FromForm] CommentModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                        AppIdentityUser author = await _auth.FindUserById(model.CommentUserId);
                        if (model.CommentUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                        {
                            Comment item = await _context.GetByIdAsync<Comment>(x => x.Id == model.Id);
                            if (item != null)
                            {
                                item.Text = model.Text;
                                _context.Update(item);
                                bool saved = await _context.SaveAll();
                                if (saved == true)
                                {
                                    CommentViewModel viewModel = new CommentViewModel(item);
                                    if (author.UserType == "Student")
                                    {
                                        viewModel.GroupName = _context.GetUserGroup(author.Id);
                                    }
                                    return Ok(viewModel);
                                }
                                else
                                {
                                    return BadRequest("Item cannot be updated");
                                }
                            }
                            return NotFound("Comment not found");
                        }
                        return BadRequest($"{currentuser.Name}, you don't have a permission");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("deletecomment")]
        public async Task<IActionResult> DeleteComment([FromBody]PostDeleteModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                    if (ModelState.IsValid)
                    {
                        if (model.PostUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                        {
                            Comment item = await _context.GetByIdAsync<Comment>(x => x.Id == model.PostId);
                            if (item != null)
                            {
                                _context.Delete(item);
                                bool result = await _context.SaveAll();

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
                    }
                    return BadRequest("Model not found");
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

        [HttpPost]
        [Route("getcomments")]
        public async Task<IActionResult> LoadComments([FromBody]LoadCommentsModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        if (await _context.GetByIdAsync<Question>(x => x.Id == model.PostId) != null)
                        {
                            List<Comment> comments = await _context.GetComments(model.PostId);
                            if (comments.Count > 0)
                            {
                                return Ok(comments.Select(x => new CommentViewModel(x)));
                            }
                            return Ok("Question has no comments");
                        }
                        return NotFound("Question not found");
                    }
                    return BadRequest("Model is not valid");
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
        [Route("getcomment")]
        public async Task<IActionResult> GetComment([FromBody] GetByIdModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        Comment comment = await _context.GetComment(model.Id.Value);
                        if (comment != null)
                        {
                            CommentViewModel viewModel = new CommentViewModel(comment);
                            if (comment.User.UserType == "Student")
                            {
                                viewModel.GroupName = _context.GetUserGroup(comment.AppIdentityUserId);
                            }
                            return Ok(viewModel);
                        }
                        return NotFound("Comment not found");
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("likecomment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser current = this.GetLoggedUser(_auth, _context);
                        Comment commentToLike = await _context.GetComment(model.PostId);
                        AppIdentityUser commentAuthor = await _auth.FindUserById(model.PostUserId);

                        if (model.IsLiked == true)
                        {
                            if (commentAuthor.Id == current.Id)
                            {
                                return BadRequest("You can't like your own comment");
                            }

                            if (commentToLike != null)
                            {
                                Like likeToAdd = new Like
                                {
                                    AppIdentityUser = current,
                                    Comment = commentToLike
                                };

                                await _context.Add(likeToAdd);

                                if (await _auth.CheckUserRole(current, "Teacher"))
                                {
                                    commentToLike.IsApproved = true;
                                    _context.Update(commentToLike);

                                    commentAuthor.Point += 10;
                                    await _auth.UpdateUser(commentAuthor);

                                    await _context.Add(new Notifier(_context, _auth).Like(likeToAdd, 10));
                                }
                                else
                                {
                                    commentAuthor.Point += 1;
                                    await _auth.UpdateUser(commentAuthor);

                                    await _context.Add(new Notifier(_context, _auth).Like(likeToAdd, 1));
                                }


                                bool saved = await _context.SaveAll();
                                if (saved == true)
                                    return Ok(new LikeViewModel(commentToLike.Likes.Count, false));
                                else
                                    return BadRequest("Error adding like to comment");
                            }
                            return NotFound("Comment not found");
                        }
                        else
                        {
                            Like likeToDelete = await _context.GetCommentLike(model.PostId, current.Id);
                            if (likeToDelete != null)
                            {
                                _context.Delete(likeToDelete);

                                if (await _auth.CheckUserRole(current, "Teacher"))
                                {
                                    commentToLike.IsApproved = true;
                                    _context.Update(commentToLike);

                                    commentAuthor.Point -= 10;
                                    await _auth.UpdateUser(commentAuthor);

                                    await _context.Add(new Notifier(_context, _auth).Dislike(likeToDelete, 10));
                                }
                                else
                                {
                                    commentAuthor.Point -= 1;
                                    await _auth.UpdateUser(commentAuthor);

                                    await _context.Add(new Notifier(_context, _auth).Dislike(likeToDelete, 1));
                                }

                                bool saved = await _context.SaveAll();
                                if (saved == true)
                                    return Ok(new LikeViewModel(commentToLike.Likes.Count, false));
                                else
                                    return BadRequest("Error disliking the comment");
                            }
                            return NotFound("Like not found");
                        }
                    }
                    return BadRequest("Model is not valid");
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

        [HttpPost]
        [Route("reply")]
        public async Task<IActionResult> ReplyComment([FromForm] CommentModel model)
        {
            try
            {
                if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
                {
                    if (ModelState.IsValid)
                    {
                        AppIdentityUser current = this.GetLoggedUser(_auth, _context);
                        Question question = await _context.GetQuestion(model.PostId);
                        Comment parent = await _context.GetComment(model.ParentId);

                        if (question != null)
                        {
                            Comment item = new Comment
                            {
                                Post = question,
                                Text = model.Text,
                                User = current,
                                ParentId = parent.Id
                            };

                            await _context.Add(item);
                            bool saved = await _context.SaveAll();
                            current.Point += 5;
                            await _auth.UpdateUser(item.User);

                            if (saved == true)
                            {
                                await _context.Add(new Notifier(_context, _auth).Comment(item));
                                await _context.SaveAll();

                                CommentViewModel viewModel = new CommentViewModel(item);
                                if (current.UserType == "Student")
                                {
                                    viewModel.GroupName = _context.GetUserGroup(current.Id);
                                }
                                return Ok(viewModel);
                            }
                            return BadRequest("Error adding comment");

                        }
                        return NotFound("Question not found!");
                    }
                    return BadRequest("Model is not valid");
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