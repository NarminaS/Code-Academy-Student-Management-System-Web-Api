using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Book:Post
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; } 
        public int Year { get; set; }
        public bool IsApproved { get; set; }

        public int FileId { get; set; }
        public File File { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
