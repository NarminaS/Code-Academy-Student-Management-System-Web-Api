using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using CodeAcademy.CoreWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract
{
    public interface IAppRepository
    {
        //Gereric methods
        Task Add<T>(T entity) where T: class;
        Task<T> GetByIdAsync<T>(Expression<Func<T, bool>> predicate) where T: class;
        Task<T> GetByNameAsync<T>(Expression<Func<T, bool>> predicate) where T : class; 

        void Update<T> (T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool SaveAll();

        //sigle methods
        //..
        Photo GetPhoto(int photoId);
        Task<Question> GetQuestion(int questionId);
        Task<Post> GetPost(int postId);
        Task<Like> GetLike(int postId, string userId);
        Task<List<Group>> GetAllGroups();
        string GetUserGroup(string id);
        Task<List<Book>> GetAllBooks();
        Task<List<Article>> GetAllArticles();
        Task<List<Link>> GetAllLinks();
        Task<List<Question>> GetAllQuestions();
        Task<List<LeftNavItem>> GetLeftNavItems();
        Task<List<Faculty>> GetFacultiesAsync();
        Task<List<Tag>> GetTagsAsync();
        Task<List<Tag>> GetTagsByFaculty(int facultyId);
        Task<List<PostTag>> GetPostTags(Post post);
        Task<PostTag> GetPostTag(int postId, int tagId);
        Task<List<Language>> GetLanguagesAsync();
        Task<List<Room>> GetAllRoomsAsync();
        Task<List<LessonStatus>> GetLessonStatusesAsync();
        Task<List<LessonHour>> GetLessonHoursAsync();
        Task<List<Post>> FilterPosts(int facultyId, int tagId, string postType);
        Task<List<Book>> FilterBooks(int facultyId, int laguageId, int tagId);
    }
}
