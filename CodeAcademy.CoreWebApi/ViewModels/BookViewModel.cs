using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class BookViewModel:PostViewModel
    {
        public BookViewModel(Book book)
        {
            this.Id = book.Id;
            this.Name = book.Name;
            this.Pages = book.Pages;
            this.Year = book.Year;
            this.IsApproved = book.IsApproved;
            this.BookPath = book.File.Url;
            this.CoverPath = book.Photo.Url;
            this.Language = book.Language.Name;
            this.UserId = book.AppIdentityUser.Id;
            this.UserName = book.AppIdentityUser.Name;
            this.UserSurname = book.AppIdentityUser.Surname;
            this.UserPhoto = book.AppIdentityUser.Photo.Url;
            this.UserType = book.AppIdentityUser.UserType;
            this.Tags = book.PostTags.Select(x => new TagModel { Id = x.TagId, Name = x.Tag.Name }).ToList();
            this.LikeCount = book.Likes.Count;
            this.DateAdded = book.DateAdded;
            this.FacultyId = book.FacultyId;
            this.PostType = book.PostType;
        }

        public string Name { get; set; }

        public int Pages { get; set; }

        public int Year { get; set; }

        public bool IsApproved { get; set; }

        public string BookPath { get; set; }

        public string CoverPath { get; set; }

        public string Language { get; set; }
    }
}
