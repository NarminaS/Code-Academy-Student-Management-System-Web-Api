using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class CommentModel
    {
        public int Id { get; set; }

        public int ParentId { get; set; }   

        [Required]
        public int PostId { get; set; }

        public string CommentUserId { get; set; }   

        [Required]
        public string Text { get; set; }
    }
}
