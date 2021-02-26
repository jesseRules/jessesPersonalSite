using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestAPICore.Common.Extensions;
using RestAPICore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestAPICore.Services
{
    public class AzPhotoService
    {
        public IAzureStorageConfig storageConfig;

        public AzPhotoService(IAzureStorageConfig config)
        {
            storageConfig = config;
        }

        public List<string> GetThumbNails()
        {
            List<string> thumbnailUrls = StorageHelper.GetThumbNailUrls(storageConfig).GetAwaiter().GetResult();
            return thumbnailUrls;
        }

        public List<string> GetImages()
        {
            List<string> thumbnailUrls = StorageHelper.GetImagesUrls(storageConfig).GetAwaiter().GetResult();
            return thumbnailUrls;
        }

        public List<string> GetPhotoShoot(string photoShoot)
        {
            List<string> thumbnailUrls = StorageHelper.GetPhotoSessionImagesUrls(storageConfig, photoShoot).GetAwaiter().GetResult();
            return thumbnailUrls;
        }

        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using (Stream stream = formFile.OpenReadStream())
                            {
                                await StorageHelper.UploadFileToStorage(stream, formFile.FileName, storageConfig);
                            }
                        }
                    }
                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }
            if (storageConfig.ThumbnailContainer != string.Empty)
                return new AcceptedAtActionResult("GetImages", "Images", null, null);
            else
                return new AcceptedResult();

        }

        public async Task<IActionResult> UploadToPhotoSession(ICollection<IFormFile> files, string photoSession)
        {

            foreach (var formFile in files)
            {
                if (StorageHelper.IsImage(formFile))
                {
                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        {
                            await StorageHelper.UploadFileToPhotoStorage(stream, formFile.FileName, photoSession, storageConfig);
                        }
                    }
                }
                else
                {
                    return new UnsupportedMediaTypeResult();
                }
            }
            if (storageConfig.ThumbnailContainer != string.Empty)
                return new AcceptedAtActionResult("GetImages", "Images", null, null);
            else
                return new AcceptedResult();

        }
    }
}