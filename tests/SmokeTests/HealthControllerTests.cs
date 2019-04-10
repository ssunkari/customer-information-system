using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace SmokeTests
{
    public class HealthControllerTests 
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            var factory = new CustomWebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }
    

        [Test]
        public async Task HealthControllerHappyPath()
        {
            var response = await _client.GetAsync("api/health");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            responseStrong.Should().Be("{\"environment\":\"Development\"}");
        }
    }
}
