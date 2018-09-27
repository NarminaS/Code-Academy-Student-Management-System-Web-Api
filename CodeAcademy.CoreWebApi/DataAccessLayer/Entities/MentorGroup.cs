using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class MentorGroup
    {
        public int Id { get; set; }

        public Student Mentor { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
