using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Language:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language()
        {
            Books = new List<Book>();
        }
        public List<Book> Books { get; set; }
    }
}
