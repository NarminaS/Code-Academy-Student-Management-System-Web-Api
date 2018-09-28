using CodeAcademy.CoreWebApi.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class QuestionModel
    {
        public int Id { get; set; }

        [Required]
        public string HeadText { get; set; }

        [Required]
        public string Text { get; set; }

        [Image]
        public IFormFile Photo { get; set; }

        [Required]
        public string Tags { get; set; }

        public string UserId { get; set; }  
    }
}
