using System.Collections.Generic;
using Api.Models;
using Api.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IApplicationDirector _applicationDirector;

        public CustomersController(IApplicationDirector applicationDirector)
        {
            _applicationDirector = applicationDirector;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(new List<CustomersApiResponseModel>());
        }

        [HttpPost]
        public ActionResult Create([FromBody]CustomersApiRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = CustomerBuilder.Create()
                .WithFirstName(model.FirstName)
                .WithSurname(model.Surname)
                .WithEmail(model.Email)
                .WithPassword(model.Password)
                .Build();

            _applicationDirector.Create(customer);

            return Ok();
        }

        [HttpPut]
        public ActionResult Update([FromBody]CustomersApiRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(new CustomersApiRequestModel());
        }
    }
}
