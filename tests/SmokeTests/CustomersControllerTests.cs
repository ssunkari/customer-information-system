using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api;
using Api.Models;
using FluentAssertions;
using Newtonsoft.Json;
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

        private static readonly (string, string, string, HttpStatusCode)[] _authTestMappings =
        {
            ("valid credentials", "administrator", "password", HttpStatusCode.OK),
            ("valid credentials", "test", "password", HttpStatusCode.OK),
            ("in valid credentials", "test123", "password", HttpStatusCode.Unauthorized)
        };

        [TestCaseSource(nameof(_authTestMappings))]

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

        private static readonly (string, CustomersApiRequestModel, HttpStatusCode)[] _requestModelMappings =
        {
            ("bad request", new CustomersApiRequestModel(), HttpStatusCode.BadRequest),
            ("bad request", new CustomersApiRequestModel {FirstName = ""}, HttpStatusCode.BadRequest),
            ("bad request", new CustomersApiRequestModel {FirstName = "", Surname = ""}, HttpStatusCode.BadRequest),
            ("bad request", new CustomersApiRequestModel {FirstName = "", Surname = "", Email = ""},
                HttpStatusCode.BadRequest),
            ("string <min chars", new CustomersApiRequestModel {FirstName = "", Surname = "", Email = "", Password = ""},
                HttpStatusCode.BadRequest),
            ("valid model", new CustomersApiRequestModel {FirstName = "tester", Surname = "test", Email = "test@tester.com", Password = "tester123"},
                HttpStatusCode.OK)
        };


       [TestCaseSource(nameof(_requestModelMappings))]
        public async Task CreateCustomerEndpointValidationTests((string, CustomersApiRequestModel, HttpStatusCode) testData)
        {
            var (_, inputRequest, expectedHttpStatusCode) = testData;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"test:password")));

            var response = await _client.PostAsync("/api/customers",
                new StringContent(JsonConvert.SerializeObject(inputRequest),Encoding.UTF8,"application/json"));

            response.StatusCode.Should().Be(expectedHttpStatusCode);
        }

    }
}
