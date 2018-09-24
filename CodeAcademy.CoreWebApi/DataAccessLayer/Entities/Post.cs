using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Post:IEntity
    {
        public int Id { get; set; }
        public string PostType { get; set; }
        public DateTime DateAdded { get; set; }


        public Post()
        {
            DateAdded = DateTime.Now;
            Likes = new List<Like>();
            PostTags = new List<PostTag>();
        }
        public List<Like> Likes { get; set; }
        public List<PostTag> PostTags { get; set; }

        public string AppIdentityUserId { get; set; }
        public AppIdentityUser AppIdentityUser { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

    }
}
