using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private static readonly (string, string, string, HttpStatusCode)[] _authMappings =
        {
            ("valid credentials", "administrator", "password", HttpStatusCode.OK),
            ("valid credentials", "test", "password", HttpStatusCode.OK),
            ("in valid credentials", "test123", "password", HttpStatusCode.Unauthorized)
        };

        [TestCaseSource(nameof(_authMappings))]

        public async Task BasicAuthTests((string, string, string, HttpStatusCode) testData)
        {
            var (_, username, password, expectedHttpStatusCode) = testData;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"{username}:{password}")));

            var response = await _client.GetAsync("api/customers");
            response.StatusCode.Should().BeEquivalentTo(expectedHttpStatusCode);
 
        }

        [Test]
        public async Task GetAllCustomersShouldReturnOkResponseWithDefaultSetup()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"test:password")));

            var response = await _client.GetAsync("api/customers");

            response.EnsureSuccessStatusCode();
            var responseStrong = await response.Content.ReadAsStringAsync();
            responseStrong.Should().Be("[]");
        }


    }
}
