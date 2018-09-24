using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
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

        Task<List<T>> GetPostsByType<T>() where T : Post;
        //sigle methods
        //..
        Photo GetPhoto(int photoId);
        Task<List<Book>> GetAllBooks();
        Task<List<LeftNavItem>> GetLeftNavItems();
        Task<List<Faculty>> GetFacultiesAsync();
        Task<List<Tag>> GetTagsAsync();
        Task<List<PostTag>> GetPostTags(Post post);
        Task<List<Language>> GetLanguagesAsync();
        Task<List<Room>> GetAllRoomsAsync();
        Task<List<LessonStatus>> GetLessonStatusesAsync();
        Task<List<LessonHour>> GetLessonHoursAsync();
    }
}
