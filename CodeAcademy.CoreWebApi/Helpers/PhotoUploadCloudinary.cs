using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers
{
    public class PhotoUploadCloudinary
    {
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoUploadCloudinary(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.Cloudname,
                                          _cloudinaryConfig.Value.APIKey,
                                          _cloudinaryConfig.Value.APISecret);
            _cloudinary = new Cloudinary(account);
        }

        public Photo Upload(IFormFile File)
        {
            var uploadResult = new ImageUploadResult();
            
            if (File.Length > 0)
            {
                using (var stream = File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(File.Name, stream)
                    };
                    uploadResult =  _cloudinary.Upload(uploadParams);
                }
            }
            string url = uploadResult.Uri.ToString();
            string publicId = uploadResult.PublicId;

            Photo photo = new Photo
            {
                Url = url,
                IsMain = true,
                PublicId = publicId
            };
            return photo;
        }
    }
}
