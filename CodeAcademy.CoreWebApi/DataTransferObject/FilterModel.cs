using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class FilterModel
    {
        public int? FacultyId { get; set; }
        public int? LanguageId { get; set; }
        public int? TagId { get; set; }
        public string PostType { get; set; }
    }
}
