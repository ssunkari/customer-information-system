using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using FluentAssertions;
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
        }

        [Given(@"I have no customer records")]
        public void GivenIHaveNoCustomerRecords()
        {
        }

        [When(@"I submit a customer record")]
        public async Task WhenISubmitACustomerRecord()
        {
            var response = await _client.PostAsync("/api/customers",
                new StringContent(JsonConvert.SerializeObject(new CustomersApiRequestModel { FirstName = "tester", Surname = "test", Email = "test@tester.com", Password = "tester123" }), Encoding.UTF8, "application/json"));
            _context.Set(response);
        }

        [Then(@"I should get http ok response")]
        public void ThenShouldReturnHttpOkResponse()
        {
            var response = _context.Get<HttpResponseMessage>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
