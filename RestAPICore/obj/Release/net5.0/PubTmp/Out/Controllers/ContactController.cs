using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Gets a Contact Entry by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetContact")]
        public ActionResult<ContactModel> Get(string id)
        {
            var contact = _contactService.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        /// <summary>
        /// Add a Conact Entry (set id to null)
        /// </summary>
        /// <remarks>Uses a MongoDB hosted on Azure</remarks>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContactModel> Create(ContactModel contact)
        {
            _contactService.Create(contact);

            return CreatedAtRoute("GetContact", new { id = contact.Id.ToString() }, contact);
        }


    }
}
