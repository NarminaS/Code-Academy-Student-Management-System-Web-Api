using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Attributes
{
    public class BookPdfAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                IFormFile book = (IFormFile)value;
                if (book.ContentType == "application/pdf")
                {
                    return true;
                }
                return false;
            }

            ErrorMessage = "Please upload a book";
            return false;
        }
    }
}
