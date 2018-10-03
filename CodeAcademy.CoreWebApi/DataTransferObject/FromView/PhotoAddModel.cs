using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject.FromView
{
    public class PhotoAddModel
    {
        public PhotoAddModel()
        {
            DateAdded = DateTime.Now;
        }

        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Desctiption { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

    }
}
