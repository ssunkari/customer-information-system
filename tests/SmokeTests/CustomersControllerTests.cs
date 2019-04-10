using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using NUnit.Framework;

namespace SmokeTests
{
    public class CustomersControllerTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            var factory = new CustomWebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }


        [Test]
        public async Task GetCustomersShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("api/customers");

            response.EnsureSuccessStatusCode();
        }
    }
}
