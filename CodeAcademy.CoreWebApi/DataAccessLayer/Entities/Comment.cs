using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Comment:IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsApproved { get; set; }

        public Comment()
        {
            DateAdded = DateTime.Now;
            Likes = new List<Like>();
        }
        public List<Like> Likes { get; set; }

        public string AppIdentityUserId { get; set; }
        public AppIdentityUser User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int? ParentId { get; set; }      
    }
}
