using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers
{
    public class PdfUploadCloudinary
    {
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PdfUploadCloudinary(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.Cloudname,
                                          _cloudinaryConfig.Value.APIKey,
                                          _cloudinaryConfig.Value.APISecret);
            _cloudinary = new Cloudinary(account);
        }

        public File Upload(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            string url = uploadResult.Uri.ToString();
            string publicId = uploadResult.PublicId;

            File pdf = new File
            {
                Url = url,
                DateAdded = DateTime.Now,
                PublicId = publicId
            };
            return pdf;
        }

    }
}
