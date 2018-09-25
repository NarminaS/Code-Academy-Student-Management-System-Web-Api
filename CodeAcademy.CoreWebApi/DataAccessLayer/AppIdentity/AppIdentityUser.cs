using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity
{
    public class AppIdentityUser:IdentityUser
    {
        //properties
        public string LoginToken { get; set; }
        public string Name { get; set; }
        public string  Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserType { get; set; }
        public bool IsBlocked { get; set; }

        public AppIdentityUser()
        {
            DateAdded = DateTime.Now;
            //many to one 
            Posts = new List<Post>();
            Likes = new List<Like>();

        }
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }

        //one to many
        public byte GenderId { get; set; }
        public Gender Gender { get; set; }

        public int? FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

        public int Point { get; set; }  
    }
}
