using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class TagModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be between 1 to int max length")]
        [Required]
        public int FacultyId { get; set; }
    }
}
