using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestAPICore.Common.Extensions;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    public class AzImagesController : ControllerBase
    {
        private readonly AzPhotoService _azPhotoService;

        // The Web API will only accept tokens 1) for users, and
        // 2) having the access_as_user scope for this API
        private static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public AzImagesController(AzPhotoService azPhotoService)
        {
            _azPhotoService = azPhotoService;
        }

        // POST /api/images/upload
        [Authorize]
        [HttpPost("[action]")]
        public IActionResult Upload(ICollection<IFormFile> files)
        {
            return _azPhotoService.Upload(files).GetAwaiter().GetResult();

        }

        // POST /api/images/upload
        [HttpPost("UploadToPhotoSesssion")]
        public IActionResult UploadToPhotoSesssion(ICollection<IFormFile> files, string photoSession)
        {
            return _azPhotoService.UploadToPhotoSession(files, photoSession).GetAwaiter().GetResult();

        }

        // GET /api/images/thumbnails
        [HttpGet("thumbnails")]
        public List<string> GetThumbNails()
        {
            return _azPhotoService.GetThumbNails();
        }

        // GET /api/images/thumbnails
        [HttpGet("images")]
        public List<string> GetImages()
        {
            return _azPhotoService.GetImages();
        }
    }
}