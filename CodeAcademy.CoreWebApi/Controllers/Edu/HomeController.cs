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
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
            return Forbid();
        }

        [HttpPost]
        [Route("addarticle")]
        public async Task<IActionResult> AddArticle([FromForm] ArticleModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher" }))
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

                    item.AppIdentityUser.Point += 20;
                    await _auth.UpdateUser(item.AppIdentityUser);

                    await _context.Add(item);

                    bool saved = _context.SaveAll();

                    if (saved == true)
                    {
                        await _context.Add(new Notifier(_context, _auth).SharePost(item, 20));
                        _context.SaveAll();

                        return Ok(new ArticleViewModel(item));
                    }
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("updatearticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                return BadRequest($"{currentuser.Name}, you don't have a permission");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("deletearticle")]
        public async Task<IActionResult> DeleteArticle([FromBody] PostDeleteModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                    return BadRequest($"{currentuser.Name}, you don't have a permission");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("approvearticle")]
        public async Task<IActionResult> ApproveArticle([FromBody] PostApproveModel model)
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
                        if (_context.SaveAll())
                        {
                            return Ok(new SuccesApproveModel(current));
                        }
                    }
                    return NotFound("Article not found or is already approved");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpGet]
        [Route("getallarticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
            return Forbid();
        }


        [HttpPost]
        [Route("addlink")]
        public async Task<IActionResult> AddLink([FromForm] LinkModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher" }))
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

                    item.AppIdentityUser.Point += 5;
                    await _auth.UpdateUser(item.AppIdentityUser);

                    await _context.Add(item);

                    bool saved = _context.SaveAll();

                    if (saved == true)
                    {
                        await _context.Add(new Notifier(_context, _auth).SharePost(item, 5));
                        _context.SaveAll();
                        return Ok(new LinkViewModel(item));
                    }

                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }


        [HttpPost]
        [Route("updatelink")]
        public async Task<IActionResult> UpdateLink([FromForm] LinkModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                return BadRequest($"{currentuser.Name}, you don't have a permission");
            }
            return Forbid();
        }


        [HttpPost]
        [Route("deletelink")]
        public async Task<IActionResult> DeleteLink([FromBody] PostDeleteModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
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
            return Forbid();
        }


        [HttpPost]
        [Route("addquestion")]
        public async Task<IActionResult> AddQuestion([FromForm] QuestionModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher" }))
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
                        if (_context.SaveAll())
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

                    item.AppIdentityUser.Point += 10;
                    await _auth.UpdateUser(item.AppIdentityUser);

                    await _context.Add(item);

                    bool saved = _context.SaveAll();
                    if (saved == true)
                    {
                        await _context.Add(new Notifier(_context, _auth).SharePost(item, 10));
                        _context.SaveAll();

                        QuestionViewModel viewModel = new QuestionViewModel(item);
                        if (item.Photo != null)
                            viewModel.Photo = item.Photo.Url;
                        return Ok(viewModel);
                    }
                }
                return Ok();
            }
            return Forbid();
        }

        [HttpPost]
        [Route("updatequestion")]
        public async Task<IActionResult> UpdateQuestion([FromForm] QuestionModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                            if (_context.SaveAll())
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
                        bool saved = _context.SaveAll();
                        if (saved == true)
                        {
                            QuestionViewModel viewModel = new QuestionViewModel(item);
                            if (item.Photo != null)
                                viewModel.Photo = item.Photo.Url;
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


        [HttpPost]
        [Route("deletequestion")]
        public async Task<IActionResult> DeleteQuestion([FromBody] PostDeleteModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                    return BadRequest($"{currentuser.Name}, you don't have a permission");
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }

        [HttpGet]
        [Route("getallquestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor", "Admin" }))
            {
                List<Question> questions = await _context.GetAllQuestions();
                List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
                foreach (var q in questions)
                {
                    if (q.Photo != null)
                    {
                        QuestionViewModel viewModel1 = new QuestionViewModel(q) { Photo = q.Photo.Url };
                        questionViewModels.Add(viewModel1);
                    }
                    QuestionViewModel viewModel = new QuestionViewModel(q);
                    questionViewModels.Add(viewModel);
                }

                //Filter by faculty
                //List<ArticleViewModel> filtered = articleViewModels.Where(x => x.FacultyId == this.GetLoggedUser(_auth, _context).FacultyId).ToList();

                if (questionViewModels.Count > 0)
                    return Ok(questionViewModels);
                else
                    return NotFound("No articles in database");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("likepost")]
        public async Task<IActionResult> LikePost([FromBody] LikeModel model)
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

                            bool saved = _context.SaveAll();
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

                            bool saved = _context.SaveAll();
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

        [HttpPost]
        [Route("addcomment")]
        public async Task<IActionResult> AddComment([FromForm] CommentModel model)
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

                        current.Point += 5;
                        await _auth.UpdateUser(item.User);

                        bool saved = _context.SaveAll();
                        if (saved == true)
                        {
                            await _context.Add(new Notifier(_context, _auth).Comment(item));
                            _context.SaveAll();

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

        [HttpPost]
        [Route("updatecomment")]
        public async Task<IActionResult> UpdateComment([FromForm] CommentModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
            {
                AppIdentityUser currentuser = this.GetLoggedUser(_auth, _context);
                if (ModelState.IsValid)
                {
                    if (model.CommentUserId == currentuser.Id || await _auth.CheckUserRole(currentuser, "Editor"))
                    {
                        Comment item = await _context.GetByIdAsync<Comment>(x => x.Id == model.Id);
                        if (item != null)
                        {
                            item.Text = model.Text;
                            _context.Update(item);
                            bool saved = _context.SaveAll();
                            if (saved == true)
                            {
                                return Ok(new CommentViewModel(item));
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

        [HttpPost]
        [Route("deletecomment")]
        public async Task<IActionResult> DeleteComment([FromBody]PostDeleteModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Student", "Teacher", "Editor" }))
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
                }
                return BadRequest("Model not found");
            }
            return Forbid();
        }

        [HttpPost]
        [Route("getcomments")]
        public async Task<IActionResult> LoadComments([FromBody]LoadCommentsModel model)
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

        [HttpPost]
        [Route("likecomment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeModel model)
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


                            bool saved = _context.SaveAll();
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

                            bool saved = _context.SaveAll();
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

        [HttpPost]
        [Route("reply")]
        public async Task<IActionResult> ReplyComment([FromForm] CommentModel model)
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

                        current.Point += 5;
                        await _auth.UpdateUser(item.User);

                        bool saved = _context.SaveAll();
                        if (saved == true)
                        {
                            await _context.Add(new Notifier(_context, _auth).Comment(item));
                            _context.SaveAll();

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
    }
}