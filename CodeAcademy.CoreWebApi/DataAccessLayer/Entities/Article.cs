using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Article:Post
    {
        public string HeadText { get; set; }
        public string Text { get; set; }
        public bool IsApproved { get; set; }
    }
}
