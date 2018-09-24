using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class File:IEntity
    {
        public File()
        {
            DateAdded = DateTime.Now;
        }
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }    
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }

    }
}
