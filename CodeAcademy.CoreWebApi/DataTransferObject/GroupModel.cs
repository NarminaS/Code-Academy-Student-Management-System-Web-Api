using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class GroupModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime LessonStartDate { get; set; }

        [Required]
        public DateTime LessonEndDate { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int LessonHourId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        public int LessonStatusId { get; set; } 

        [Required]
        public string TeacherId { get; set; }

        public string MentorId { get; set; }    

        [Required]
        public IFormFile Photo { get; set; }
    }   
}
