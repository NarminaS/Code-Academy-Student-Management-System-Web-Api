using CodeAcademy.CoreWebApi.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class StudentModel
    {
        public string Id { get; set; }

        [Required]
        [PersonFullName]
        public string Name { get; set; }

        [Required]
        [PersonFullName]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int GenderId { get; set; }

    }
}
