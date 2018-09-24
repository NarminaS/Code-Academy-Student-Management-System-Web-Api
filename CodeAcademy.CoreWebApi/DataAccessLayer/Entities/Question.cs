using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Question:Post
    {
        public string HeadText { get; set; }
        public string Text { get; set; }


        public Question()
        {
            Comments = new List<Comment>();
        }
        public List<Comment> Comments { get; set; }

    }
}
