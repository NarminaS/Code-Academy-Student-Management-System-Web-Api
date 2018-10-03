using CodeAcademy.CoreWebApi.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class BookModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }  

        [Required]
        public string Name { get; set; }

        [Required]
        [PersonFullName]
        public string Author { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int FacultyId { get; set; }

        public bool IsApproved { get; set; }
        
        [BookPdf]
        [Required]
        public IFormFile Book { get; set; }

        [Required]
        public IFormFile Cover { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        public string Tags { get; set; }

    }
}
