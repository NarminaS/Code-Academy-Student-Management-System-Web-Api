using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class ArticleViewModel:PostViewModel
    {
        public ArticleViewModel(Article article)
        {
            this.Id = article.Id;
            this.DateAdded = article.DateAdded;
            this.HeadText = article.HeadText;
            this.Text = article.Text;
            this.IsApproved = article.IsApproved;
            this.Tags = article.PostTags.Select(x => new TagModel { Id = x.TagId, Name = x.Tag.Name }).ToList();
            this.UserId = article.AppIdentityUser.Id;
            this.UserName = article.AppIdentityUser.Name;
            this.UserSurname = article.AppIdentityUser.Surname;
            this.UserPhoto = article.AppIdentityUser.Photo.Url;
            this.LikeCount = article.Likes.Count;
            this.FacultyId = article.FacultyId;
            this.PostType = article.PostType;
        }

        public string HeadText { get; set; }

        public string Text { get; set; }

        public bool IsApproved { get; set; }

        public string UserPhoto { get; set; }

        public int Id { get; set; } 
    }
}
