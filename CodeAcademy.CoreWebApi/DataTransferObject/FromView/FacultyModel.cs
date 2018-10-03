using CodeAcademy.CoreWebApi.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class FacultyModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(1,3000,ErrorMessage ="Value must be between 1 to 3000")]
        public int LessonHour { get; set; }

        [Required]
        [PhotoPng(ErrorMessage ="Only .png files")]
        public IFormFile Photo { get; set; }
    }
}
