using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Entities.InterfaceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Group:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LessonStartDate { get; set; }
        public DateTime LessonEndDate { get; set; }
        public DateTime DateAdded { get; set; }

        public Group()
        {
            DateAdded = DateTime.Now;
            Students = new List<Student>();
            TeacherGroups = new List<TeacherGroup>();
            MentorGroups = new List<MentorGroup>();
        }
        public List<TeacherGroup> TeacherGroups { get; set; }
        public List<MentorGroup> MentorGroups { get; set; }
        public List<Student> Students { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int LessonStatusId { get; set; }
        public LessonStatus LessonStatus { get; set; }

        public int LessonHourId { get; set; }
        public LessonHour LessonHour { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
