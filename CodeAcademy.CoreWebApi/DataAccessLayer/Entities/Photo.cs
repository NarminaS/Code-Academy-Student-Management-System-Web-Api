using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Entities
{
    public class Photo:IEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        public Photo()
        {
            Faculties = new List<Faculty>();
            Books = new List<Book>();
            DateAdded = DateTime.Now;
        }
        public List<Faculty> Faculties { get; set; }
        public List<Book> Books { get; set; }
    }
}
