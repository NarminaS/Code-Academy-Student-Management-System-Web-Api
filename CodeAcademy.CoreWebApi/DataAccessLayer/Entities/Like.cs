using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Like:IEntity
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }

        public Like()
        {
            DateAdded = DateTime.Now;
        }

        public string AppIdentityUserId { get; set; }
        public AppIdentityUser UserId { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
