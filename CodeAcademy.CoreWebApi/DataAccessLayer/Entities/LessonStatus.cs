using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class LessonStatus:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public LessonStatus()
        {
            Students = new List<Student>();
        }
        public List<Student> Students { get; set; }
    }
}
