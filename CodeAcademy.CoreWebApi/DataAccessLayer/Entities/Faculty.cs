using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Faculty:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LessonHour { get; set; }

        public DateTime DateAdded { get; set; }

        public Faculty()
        {
            DateAdded = DateTime.Now;
            Users = new List<AppIdentityUser>();
            Posts = new List<Post>();
            Groups = new List<Group>();
        }

        public List<AppIdentityUser> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<Group> Groups { get; set; }


        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
