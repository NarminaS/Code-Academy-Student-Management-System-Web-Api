using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Student:AppIdentityUser
    {
        public bool IsMentor { get; set; }

        public Student()
        {
            MentorGroups = new List<MentorGroup>();
        }

        public List<MentorGroup> MentorGroups { get; set; }

        public int LessonStatusId { get; set; }
        public LessonStatus LessonStatus { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int? CertificateId { get; set; }
        public Certificate Certificate { get; set; }
    }
}
