using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.Entities
{
    public class Link:Post
    {
        public string HeadText { get; set; }
        public string LinkUrl { get; set; }
    }
}
