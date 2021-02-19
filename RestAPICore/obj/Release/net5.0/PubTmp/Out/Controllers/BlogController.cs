using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        // The Web API will only accept tokens 1) for users, and 
        // 2) having the access_as_user scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Gets entire list of Blog Entries
        /// </summary>
        /// <remarks>Uses a MongoDB hosted on Azure</remarks>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<BlogModel>> Get() =>
            _blogService.Get();


        /// <summary>
        /// Gets a Blog Lists
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list", Name = "GetBlogList")]
        public ActionResult<List<BlogModel>> GetBlogList(Int32 count, Int32 offset)
        {

            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _blogService.GetWithWait();

        }

        /// <summary>
        /// Gets a Blog Entry by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetBlog")]
        public ActionResult<BlogModel> Get(string id)
        {
            var blog = _blogService.Get(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        /// <summary>
        /// Add a Blog Entry (set id to null)
        /// </summary>
        /// <remarks>Uses a MongoDB hosted on Azure</remarks>
        /// <param name="blog"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult<BlogModel> Create(BlogModel blog)
        {
            _blogService.Create(blog);

            return CreatedAtRoute("GetBlog", new { id = blog.Id.ToString() }, blog);
        }

        /// <summary>
        /// Update Blog Entry by ID
        /// </summary>
        /// <remarks>Uses a MongoDB hosted on Azure</remarks>
        /// <param name="id"></param>
        /// <param name="blogIn"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, BlogModel blogIn)
        {
            var blog = _blogService.Get(id);

            if (blog == null)
            {
                return NotFound();
            }

            _blogService.Update(id, blogIn);

            return NoContent();
        }

        /// <summary>
        /// Delete a Blog Entry by ID
        /// </summary>
        /// <remarks>Uses a MongoDB hosted on Azure</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var blog = _blogService.Get(id);

            if (blog == null)
            {
                return NotFound();
            }

            _blogService.Remove(blog.Id);

            return NoContent();
        }
    }
}
