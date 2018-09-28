using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Context;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.BusinessLogicLayer.Concrete
{
    public class AppRepository : IAppRepository
    {
        private AppIdentityDbContext _context;
        public AppRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task Add<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
             _context.Remove(entity);
        }

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> books = await _context.Books.Include(x=>x.PostTags).ThenInclude(x=>x.Tag)
                                                    .Include(x=>x.Photo)
                                                    .Include(x=>x.Language)
                                                    .Include(x=>x.Likes)
                                                    .Include(x=>x.AppIdentityUser)
                                                    .Include(x=>x.File).ToListAsync();
            return books;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            List<Room> rooms = await _context.Rooms.ToListAsync();
            return rooms;
        }

        public async Task<T> GetByIdAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            T result = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            return result;
        }

        public async Task<T> GetByNameAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            T result = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            return result;
        }

        public async Task<List<Faculty>> GetFacultiesAsync()
        {
            List<Faculty> faculties = await _context.Faculties.Include(x => x.Photo).ToListAsync();
            return faculties;
        }

        public async Task<List<Language>> GetLanguagesAsync()
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            return languages;
        }

        public async Task<List<LeftNavItem>> GetLeftNavItems()
        {
            List<LeftNavItem> leftNavItems = await _context.LeftNavItems.Include(x=>x.Photo).ToListAsync();
            return leftNavItems;
        }

        public async Task<List<LessonHour>> GetLessonHoursAsync()
        {
            List<LessonHour> lessonHours = await _context.LessonHours.ToListAsync();
            return lessonHours;
        }

        public async Task<List<LessonStatus>> GetLessonStatusesAsync()
        {
            List<LessonStatus> lessonStatuses = await _context.LessonStatuses.ToListAsync();
            return lessonStatuses;
        }

        public Photo GetPhoto(int photoId)
        {
            return _context.Photos.Where(photo => photo.Id == photoId).SingleOrDefault();
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            return tags;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }

        public void Update<T>(T entity) where T : class
        {
             _context.Update(entity);
        }

        public async Task<List<PostTag>> GetPostTags(Post post)
        {
            var postTags = await _context.PostTags.Where(x => x.PostId == post.Id).ToListAsync();
            return postTags;
        }

        public async Task<List<Article>> GetAllArticles()
        {
            List<Article> articles = await _context.Articles
                                        .Include(x => x.PostTags).ThenInclude(x => x.Tag)
                                        .Include(x => x.Likes)
                                        .Include(x => x.AppIdentityUser).ThenInclude(x => x.Photo)
                                        .ToListAsync();
            return articles;
        }

        public async Task<List<Link>> GetAllLinks()
        {
            List<Link> links = await _context.Links
                             .Include(x => x.PostTags).ThenInclude(x => x.Tag)
                             .Include(x => x.Likes)
                             .Include(x => x.AppIdentityUser).ThenInclude(x => x.Photo)
                             .ToListAsync();
            return links;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            List<Question> questions = await _context.Questions
                                         .Include(x => x.PostTags).ThenInclude(x => x.Tag)
                                         .Include(x => x.Photo)
                                         .Include(x => x.Likes)
                                         .Include(x => x.AppIdentityUser).ThenInclude(x=>x.Photo)
                                         .ToListAsync();
            return questions;
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            Question question = await _context.Questions.Include(x=>x.Photo)
                                                        .Include(x=>x.AppIdentityUser).ThenInclude(x=>x.Photo)
                                                        .Include(x=>x.Comments)
                                                            .FirstOrDefaultAsync(x => x.Id == questionId);
            return question;
        }

        public async Task<Post> GetPost(int postId)
        {
            Post post = await _context.Posts.Include(l => l.Likes)
                                                    .Include(u => u.AppIdentityUser)
                                                    .Include(l=>l.Likes)
                                                    .FirstOrDefaultAsync(x => x.Id == postId);
            return post;
        }

        public async Task<Like> GetPostLike(int postId, string userId)
        {
            Like like = await _context.Likes.FirstOrDefaultAsync(x => x.PostId == postId && x.AppIdentityUserId == userId);
            return like;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            List<Group> groups = await _context.Groups.ToListAsync();
            return groups;
        }

        public async Task<PostTag> GetPostTag(int postId, int tagId)
        {
            PostTag postTag = await _context.PostTags.FirstOrDefaultAsync(x => x.PostId == postId && x.TagId == tagId);
            return postTag;
        }

        public async Task<List<Post>> FilterPosts(int facultyId, int tagId, string postType)
        {
            List<Post> posts = new List<Post>();

            posts.AddRange(await GetAllArticles());
            posts.AddRange(await GetAllBooks());
            posts.AddRange(await GetAllQuestions());
            posts.AddRange(await GetAllLinks());

            if (facultyId != default(int))
            {
                posts = posts.Where(x => x.FacultyId == facultyId).ToList();
            }
         
            if (tagId != default(int))
            {
                List<Post> filteredByTag = new List<Post>();
                foreach (var post in posts)
                {
                    foreach (var pt in await GetPostTags(post))
                    {
                        if (pt.PostId == post.Id && pt.TagId == tagId)
                        {
                            filteredByTag.Add(post);
                        }
                    }
                }
                posts = filteredByTag;
            }

            if (postType != String.Empty)
            {
                posts = posts.Where(x => x.PostType == postType).ToList();
            }

            return posts;
        }

        public async Task<List<Book>> FilterBooks(int facultyId, int languageId, int tagId)    
        {
            List<Book> books = await GetAllBooks();

            if (facultyId != default(int))
            {
                books = books.Where(x => x.FacultyId == facultyId).ToList();
            }

            if (tagId != default(int))
            {
                List<Book> filteredByTag = new List<Book>();
                foreach (var book in books)
                {
                    foreach (var pt in await GetPostTags(book))
                    {
                        if (pt.PostId == book.Id && pt.TagId == tagId)
                        {
                            filteredByTag.Add(book);
                        }
                    }
                }
                books = filteredByTag;
            }

            if (languageId != default(int))
            {
                books = books.Where(x => x.LanguageId == languageId).ToList();
            }

            return books;
        }

        public string GetUserGroup(string id)
        {
            string groupName = _context.Students.Include(x => x.Group).FirstOrDefault(x => x.Id == id).Group.Name;
            return groupName;
        }

        public async Task<List<Tag>> GetTagsByFaculty(int facultyId)
        {
            if (facultyId != default(int))
            {
                List<Tag> tags = await _context.Tags.Include(x => x.PostTags).Where(x => x.FacultyId == facultyId).ToListAsync();
                return tags;
            }
            return await _context.Tags.Include(x => x.PostTags).ToListAsync();
        }

        public async Task<List<Comment>> GetComments(int postId)
        {
            List<Comment> comments = await _context.Comments
                                                 .Include(x => x.User).ThenInclude(x => x.Photo)
                                                 .Include(x => x.Likes)
                                                                     .Where(x => x.PostId == postId).ToListAsync();
            return comments;
        }

        public async Task<Comment> GetComment(int id)
        {
            Comment comment = await _context.Comments
                                                    .Include(x => x.Likes)
                                                            .FirstOrDefaultAsync(x => x.Id == id);
            return comment;
        }

        public async Task<Like> GetCommentLike(int commentId, string userId)
        {
            Like like = await _context.Likes.FirstOrDefaultAsync(x => x.CommentId == commentId && x.AppIdentityUserId == userId);
            return like;
        }
    }
}
