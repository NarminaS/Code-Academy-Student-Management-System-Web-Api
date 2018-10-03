using CodeAcademy.CoreWebApi.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class LeftNavItemModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value must be between 1 to int max length")]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string IconClassname { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string RouterLink { get; set; }
        [Required]
        [PhotoPng(ErrorMessage ="Only .png files")]
        public IFormFile Photo { get; set; }
    }
}
