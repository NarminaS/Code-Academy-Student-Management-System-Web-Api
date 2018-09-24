using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Tag:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public Tag()
        {
            PostTags = new List<PostTag>();
        }
        public List<PostTag> PostTags { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
