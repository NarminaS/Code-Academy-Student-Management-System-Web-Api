using System.Threading.Tasks;
using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers;
using CodeAcademy.CoreWebApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeAcademy.CoreWebApi.Controllers.Administrator
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeftNavItemsController : ControllerBase
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public LeftNavItemsController(IAppRepository context, IAuthRepository auth,
                                      IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._context = context;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                var result = await _context.GetLeftNavItems();
                return Ok(result);
            }
            return Forbid();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromForm] LeftNavItemModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                    Photo photo = upload.Upload(model.Photo);

                    LeftNavItem item = new LeftNavItem
                    {
                        Name = model.Name,
                        RouterLink = model.RouterLink,
                        IconClassname = model.IconClassname,
                        IsVisible = model.IsVisible,
                        Photo = photo
                    };

                    await _context.Add(photo);
                    await _context.Add(item);
                    saved = await _context.SaveAll();
                    if (saved == true)
                    {
                        return Ok(item);
                    }
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }


        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                LeftNavItem item = await _context.GetByIdAsync<LeftNavItem>(x => x.Id == id);
                if (item != null)
                {
                    _context.Delete(item);
                    bool result = await _context.SaveAll();

                    if (result == true)
                        return Ok(item);
                    else
                        return BadRequest("Model cannot be  deleted");
                }
                else
                {
                    return NotFound("Model not found");
                }
            }
            return Forbid();
        }


        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] LeftNavItemModel model)
        {
            if (this.ValidRoleForAction(_context, _auth, new string[] { "Admin" }))
            {
                bool saved;
                if (ModelState.IsValid)
                {
                    LeftNavItem item = await _context.GetByIdAsync<LeftNavItem>(x => x.Id == model.Id);
                    PhotoUploadCloudinary upload = new PhotoUploadCloudinary(_cloudinaryConfig);
                    Photo photo = upload.Upload(model.Photo);

                    item.Name = model.Name;
                    item.RouterLink = model.RouterLink;
                    item.IconClassname = model.IconClassname;
                    item.IsVisible = model.IsVisible;
                    item.Photo = photo;

                    await _context.Add(photo);
                    _context.Update(item);
                    saved = await _context.SaveAll();
                    if (saved == true)
                    {
                        return Ok(item);
                    }
                    else
                    {
                        return BadRequest("Item cannot be updated");
                    }
                }
                return BadRequest("Model is not valid");
            }
            return Forbid();
        }
    }
}