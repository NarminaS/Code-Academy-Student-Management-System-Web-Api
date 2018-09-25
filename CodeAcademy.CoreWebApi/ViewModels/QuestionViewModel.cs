using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class QuestionViewModel:PostViewModel
    {
        public QuestionViewModel(Question question)
        {
            this.Id = question.Id;
            this.Tags = question.PostTags.Select(x => new TagModel { Id = x.TagId, Name = x.Tag.Name }).ToList();
            this.UserId = question.AppIdentityUser.Id;
            this.UserName = question.AppIdentityUser.Name;
            this.UserSurname = question.AppIdentityUser.Surname;
            this.UserPhoto = question.AppIdentityUser.Photo.Url;
            this.LikeCount = question.Likes.Count;
            this.HeadText = question.HeadText;
            this.Text = question.Text;
            this.Photo = question.Photo.Url ?? null;
            this.FacultyId = question.FacultyId;
            this.PostType = question.PostType;
            this.DateAdded = question.DateAdded;
        }

        public string UserPhoto { get; set; }

        public int Id { get; set; }

        public string HeadText { get; set; }

        public string Text { get; set; }

        public string Photo { get; set; }
    }
}
