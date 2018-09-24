using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Teacher:AppIdentityUser
    {
        public Teacher()
        {
            TeacherGroups = new List<TeacherGroup>();
        }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}
