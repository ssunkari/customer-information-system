using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Couchbase;
using Dao.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace Api.AcceptanceTests
{
    [Binding]
    public sealed class StepBindings
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _context;
        private readonly HttpClient _client;
        private readonly Mock<ICouchbaseOperations> _couchbaseClient;
        private Mock<IOperationResult<object>> _operationResult;

        public StepBindings(ScenarioContext injectedContext)
        {
            _context = injectedContext;
            var factory = new CustomWebApplicationFactory<Startup>();
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"test:password")));
            _couchbaseClient = factory.CouchbaseOperations;
            _operationResult = factory.OperationResult;
        }

        [Given(@"I have no customer records")]
        public void GivenIHaveNoCustomerRecords()
        {
            _operationResult.SetupGet(x => x.Success).Returns(false);
        }

        [Given(@"I database server is unavailable")]
        public void GivenIDatabaseServerIsUnavailable()
        {
            _couchbaseClient.Setup(x=>x.Upsert(It.IsAny<Document<dynamic>>())).ThrowsAsync(new Exception("server is unavailable"));
        }

        [Given(@"I have existing customer record")]
        public void GivenIHaveExistingCustomerRecord()
        {
            _operationResult.SetupGet(x => x.Success).Returns(true);
            _operationResult.SetupGet(x => x.Value).Returns(JsonConvert.SerializeObject(new Customer("","","","")));
            _couchbaseClient.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(_operationResult.Object);
        }

        [When(@"I submit a customer record")]
        public async Task WhenISubmitACustomerRecord()
        {
            var response = await _client.PostAsync("/api/customers",
                new StringContent(JsonConvert.SerializeObject(new CustomersApiRequestModel { FirstName = "tester", Surname = "test", Email = "test@tester.com", Password = "tester123" }), Encoding.UTF8, "application/json"));
            _context.Set(response);
        }

        [When(@"I request a customer record")]
        public async Task WhenIRequestACustomerRecord()
        {
            var response = await _client.GetAsync($"/api/customers/123");
            _context.Set(response);
        }


        [When(@"I update a customer record")]
        public async Task WhenIUpdateACustomerRecord()
        {
            var response = await _client.PutAsync("/api/customers",
                new StringContent(JsonConvert.SerializeObject(new CustomersApiRequestModel { FirstName = "tester", Surname = "test", Email = "test@tester.com", Password = "tester123" }), Encoding.UTF8, "application/json"));
            _context.Set(response);
        }


        [Then(@"I should get http (.*) response")]
        public void ThenShouldReturnHttpOkResponse(HttpStatusCode statusCode)
        {
            var response = _context.Get<HttpResponseMessage>();
            response.StatusCode.Should().Be(statusCode);
        }
    }
}
