using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class LessonHour:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public byte BeginHour { get; set; }
        public byte BeginMinute{ get; set; }
        public byte EndHour { get; set; }
        public byte EndMinute { get; set; }


        public LessonHour()
        {
            Groups = new List<Group>();
        }

        public List<Group> Groups { get; set; }
    }
}
