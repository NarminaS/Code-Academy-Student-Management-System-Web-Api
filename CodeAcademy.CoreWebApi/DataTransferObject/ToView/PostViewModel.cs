using System;
using System.Collections.Generic;
using CodeAcademy.CoreWebApi.DataTransferObject;
using System.Linq;
using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.DataTransferObject.FromView;

namespace CodeAcademy.CoreWebApi.DataTransferObject.ToView
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public string GroupName { get; set; }

        public string UserPhoto { get; set; }

        public string UserType { get; set; }    

        public List<TagModel> Tags { get; set; }

        public int LikeCount { get; set; }

        public int FacultyId { get; set; }

        public string PostType { get; set; }    
    }
}
