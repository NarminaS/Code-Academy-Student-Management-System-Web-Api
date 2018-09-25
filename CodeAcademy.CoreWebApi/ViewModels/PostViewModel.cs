using System;
using System.Collections.Generic;
using CodeAcademy.CoreWebApi.DataTransferObject;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class PostViewModel
    {
        public DateTime DateAdded { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public List<TagModel> Tags { get; set; }

        public int LikeCount { get; set; }

        public int FacultyId { get; set; }

        public string PostType { get; set; }    
    }
}
