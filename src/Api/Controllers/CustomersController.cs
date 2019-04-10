using System.Collections.Generic;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(new List<CustomersApiResponseModel>());
        }

        [HttpPost]
        public ActionResult Create(CustomersApiRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
