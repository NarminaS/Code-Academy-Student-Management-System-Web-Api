using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Attributes
{
    public class ImageAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                IFormFile photo = (IFormFile)value;
                if (photo.ContentType.Contains("image"))
                {
                    return true;
                }
                else
                    ErrorMessage = "The file is not in valid format";
                return false;
            }
            return true;
        }
    }
}
