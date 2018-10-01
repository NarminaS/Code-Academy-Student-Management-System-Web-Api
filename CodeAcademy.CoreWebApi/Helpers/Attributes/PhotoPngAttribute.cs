using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Attributes
{
    public class PhotoPngAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                IFormFile photo = (IFormFile)value;
                if (photo.ContentType == "image/png")
                {
                    return true;
                }

                ErrorMessage = "Image must be only png";
                return false;
            }

            ErrorMessage = "Please upload a photo";
            return false;
        }
    }
}
