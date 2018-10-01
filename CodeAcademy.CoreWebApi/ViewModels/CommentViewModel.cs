using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class CommentViewModel:PostViewModel
    {
        public CommentViewModel(Comment comment)
        {
            this.DateAdded = comment.DateAdded;
            this.FacultyId = comment.User.FacultyId ?? default(int);
            this.Id = comment.Id;
            this.ParentId = comment.ParentId;
            this.Text = comment.Text;
            this.Tags = new List<TagModel>();
            this.UserId = comment.User.Id;
            this.UserName = comment.User.Name;
            this.UserSurname = comment.User.Surname;
            this.UserPhoto = comment.User.Photo.Url;
            this.UserType = comment.User.UserType;
            this.LikeCount = comment.Likes.Count;
            this.PostType = "Comment";
        }

        public string Text { get; set; }

        public int? ParentId { get; set; }   
    }
}
