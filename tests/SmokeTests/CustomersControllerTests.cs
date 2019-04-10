using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api;
using Api.Models;
using Domain.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SmokeTests
{
    public class CustomersControllerTests
    {
        private HttpClient _client;

        private CustomWebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        private static readonly (string, string, string, string, HttpStatusCode)[] _authTestMappings =
        {
            ("valid credentials", "api/customers","administrator", "password", HttpStatusCode.OK),
            ("valid credentials", "api/customers", "test", "password", HttpStatusCode.OK),
            ("in valid credentials", "api/customers", "test123", "password", HttpStatusCode.Unauthorized),
            ("in valid credentials", "api/customers/123", "test123", "password", HttpStatusCode.Unauthorized)
        };

        [TestCaseSource(nameof(_authTestMappings))]

        public async Task BasicAuthTests((string, string, string, string, HttpStatusCode) testData)
        {
            var (_, relativePath,username, password, expectedHttpStatusCode) = testData;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"{username}:{password}")));

            var response = await _client.GetAsync(relativePath);
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

        [TestCaseSource(nameof(_requestModelMappings))]
        public async Task UpdateCustomerEndpointValidationTests((string, CustomersApiRequestModel, HttpStatusCode) testData)
        {
            _factory.OperationResult.SetupGet(x => x.Success).Returns(true);
            var (_, inputRequest, expectedHttpStatusCode) = testData;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"test:password")));

            var response = await _client.PutAsync("/api/customers",
                new StringContent(JsonConvert.SerializeObject(inputRequest), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(expectedHttpStatusCode);
        }

        [Test]
        public async Task GetCusomterByIdEndpointTests()
        {
            _factory.OperationResult.SetupGet(x => x.Success).Returns(true);
            _factory.OperationResult.SetupGet(x => x.Value).Returns(JsonConvert.SerializeObject(new Customer("","","","")));
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"test:password")));
            var id = "123";

            var response = await _client.GetAsync($"/api/customers/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
