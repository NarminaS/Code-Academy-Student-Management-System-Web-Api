using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class ArticleModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        public string HeadText { get; set; }

        [Required]
        [MinLength(50)]
        public string Text { get; set; }

        [Required]
        public string Tags { get; set; }

        public bool IsApproved { get; set; }
    }
}
