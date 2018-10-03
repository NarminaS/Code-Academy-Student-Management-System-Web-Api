using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class LinkModel
    {
        public int Id { get; set; }

        [Required]
        public string HeadText { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string LinkUrl { get; set; }

        [Required]
        public string Tags { get; set; }

        public string UserId { get; set; }
    }
}
