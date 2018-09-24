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

        public async Task<List<T>> GetPostsByType<T>() where T : Post
        {
            var all = await _context.Set<T>()
                                            .Include(x => x.PostTags).ThenInclude(x => x.Tag)
                                            .Include(x => x.Photo)
                                            .Include(x => x.Likes)
                                            .Include(x => x.AppIdentityUser).ThenInclude(x => x.Photo)
                                            .ToListAsync();
            return all;
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
    }
}
