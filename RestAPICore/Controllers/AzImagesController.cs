using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestAPICore.Common.Extensions;
using RestAPICore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    public class AzImagesController : ControllerBase
    {
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private readonly AzureStorageConfig storageConfig = null;

        public AzImagesController(IOptions<AzureStorageConfig> config)
        {
            storageConfig = config.Value;
        }

        // POST /api/images/upload
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            bool isUploaded = false;

            if (files == null)
            {
                return BadRequest("No files received from the upload");
            }

            try
            {
                if (files.Count == 0)
                    return BadRequest("No files received from the upload");

                if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)
                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                if (storageConfig.ImageContainer == string.Empty)
                    return BadRequest("Please provide a name for your image container in the azure blob storage");

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using (Stream stream = formFile.OpenReadStream())
                            {
                                isUploaded = await StorageHelper.UploadFileToStorage(stream, formFile.FileName, storageConfig);
                            }
                        }
                    }
                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }

                if (isUploaded)
                {
                    if (storageConfig.ThumbnailContainer != string.Empty)
                        return new AcceptedAtActionResult("GetThumbNails", "Images", null, null);
                    else
                        return new AcceptedResult();
                }
                else
                    return BadRequest("Look like the image couldnt upload to the storage");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/images/thumbnails
        [HttpGet("thumbnails")]
        public async Task<IActionResult> GetThumbNails()
        {
            try
            {
                if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)
                    return BadRequest("Sorry, can't retrieve your Azure storage details from appsettings.js, make sure that you add Azure storage details there.");

                if (storageConfig.ImageContainer == string.Empty)
                    return BadRequest("Please provide a name for your image container in Azure blob storage.");

                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(storageConfig);
                return new ObjectResult(thumbnailUrls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}