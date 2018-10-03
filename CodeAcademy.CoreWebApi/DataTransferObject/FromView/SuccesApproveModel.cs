using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class SuccesApproveModel
    {
        public SuccesApproveModel(Teacher teacher)
        {
            this.Id = teacher.Id;
            this.Name = teacher.Name;
            this.Surname = teacher.Surname;
            this.Email = teacher.Email;
        }
        public string Id { get; set; }  

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; } 
    }
}
