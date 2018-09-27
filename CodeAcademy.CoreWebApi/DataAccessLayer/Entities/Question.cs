using CodeAcademy.CoreWebApi.Entities;
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

        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }

        public Question()
        {
            Comments = new List<Comment>();
        }
        public List<Comment> Comments { get; set; }

        public static implicit operator List<object>(Question v)
        {
            throw new NotImplementedException();
        }
    }
}
