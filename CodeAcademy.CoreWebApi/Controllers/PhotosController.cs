using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IAppRepository appRepository, 
                                IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _appRepository = appRepository;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.Cloudname,
                                          _cloudinaryConfig.Value.APIKey, 
                                          _cloudinaryConfig.Value.APISecret);
            _cloudinary = new Cloudinary(account);
        }


    //    [HttpPost]
    //    public async Task<IActionResult> AddPhoto(int userId,[FromBody] PhotoAddModel model)
    //    {
    //        var file = model.File;
    //        var uploadResult = new ImageUploadResult();
    //        if (file.Length > 0)
    //        {
    //            using(var stream = file.OpenReadStream())
    //            {
    //                var uploadParams = new ImageUploadParams
    //                {
    //                    File = new FileDescription(file.Name,stream)
    //                };
    //                uploadResult = _cloudinary.Upload(uploadParams);
    //            }
    //        }

    //        model.Url = uploadResult.Uri.ToString();
    //        model.PublicId = uploadResult.PublicId;
    //    }
    }
}