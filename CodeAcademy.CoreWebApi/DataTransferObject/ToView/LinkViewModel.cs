using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.ToView
{
    public class LinkViewModel:PostViewModel
    {
        public LinkViewModel(Link link)
        {
            this.Id = link.Id;
            this.DateAdded = link.DateAdded;
            this.LikeCount = link.Likes.Count;
            this.Tags = this.Tags = link.PostTags.Select(x => new TagModel { Id = x.TagId, Name = x.Tag.Name }).ToList();
            this.UserId = link.AppIdentityUser.Id;
            this.UserName = link.AppIdentityUser.Name;
            this.UserSurname = link.AppIdentityUser.Surname;
            this.UserPhoto = link.AppIdentityUser.Photo.Url;
            this.UserType = link.AppIdentityUser.UserType;
            this.HeadText = link.HeadText;
            this.LinkUrl = link.LinkUrl;
            this.FacultyId = link.FacultyId;
            this.PostType = link.PostType;
        }

        public string HeadText { get; set; }

        public string LinkUrl { get; set; }
    }
}
