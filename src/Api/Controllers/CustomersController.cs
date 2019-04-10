using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Create([FromBody]CustomersApiRequestModel model)
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

           return (await _applicationDirector.Create(customer)).Match<ActionResult>(success=>Ok(), error => BadRequest());
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody]CustomersApiRequestModel model)
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

            return (await _applicationDirector.Update(customer)).Match<ActionResult>(success => Ok(), error => BadRequest());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return (await _applicationDirector.Get(id)).Match<ActionResult>(success => Ok(success), none => BadRequest());
        }
    }
}
