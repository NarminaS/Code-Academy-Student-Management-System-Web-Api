using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.ViewModels
{
    public class LibraryTagViewModel
    {
        public LibraryTagViewModel(Tag tag)
        {
            this.TagId = tag.Id;
            this.TagName = tag.Name;
            this.PostCount = tag.PostTags.Count;
        }
        public int TagId { get; set; }
        public string TagName { get; set; } 
        public int PostCount { get; set; }
    }
}
