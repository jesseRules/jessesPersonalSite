using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooglePhotosController : ControllerBase
    {
        private GooglePhotosService googlePhotosSvc;

        private async Task Login()
        {
            //new-up logging
            var logger = new LoggerFactory().CreateLogger<GooglePhotosService>();

            //new-up configuration options
            var options = new GooglePhotosOptions
            {
                User = RestAPICore.Services.CredentialService.googlePhotoUser,
                ClientId = RestAPICore.Services.CredentialService.googlePhotoClientId,
                ClientSecret = RestAPICore.Services.CredentialService.googlePhotoClientSecret,
                Scopes = new[] { GooglePhotosScope.ReadOnly },
            };

            //new-up a single HttpClient
            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            var client = new HttpClient(handler) { BaseAddress = new Uri(options.BaseAddress) };

            //new-up the GooglePhotosService and pass in the logger, options and HttpClient
            this.googlePhotosSvc = new GooglePhotosService(logger, Options.Create(options), client);

            //attempt to log-in
            if (!await googlePhotosSvc.LoginAsync())
                throw new Exception($"login failed!");
        }

        /// <summary>
        /// Returns a list of libraries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("libraries")]
        public List<Album> getLibraries()
        {
            this.Login().GetAwaiter().GetResult();
            //get and list all albums
            List<Album> albums = googlePhotosSvc.GetAlbumsAsync().GetAwaiter().GetResult();
            return albums;
        }

        /// <summary>
        /// Returns a library of photos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("libraries/{id}")]
        public Album getLibraryItems(string id)
        {
            this.Login().GetAwaiter().GetResult();
            Album album = googlePhotosSvc.GetAlbumAsync(id).GetAwaiter().GetResult();
            return album;
        }
    }
}