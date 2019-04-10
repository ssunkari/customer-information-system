using System;
using System.Threading.Tasks;
using Api.Couchbase;
using Couchbase;
using Dao.Helpers;
using Dao.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Api.AcceptanceTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        public Mock<ICouchbaseOperations> CouchbaseOperations = new Mock<ICouchbaseOperations>(MockBehavior.Strict);
        public Mock<IOperationResult<object>> OperationResult = new Mock<IOperationResult<object>>();

        public CustomWebApplicationFactory()
        {
            CouchbaseOperations.Setup(x => x.Upsert(It.IsAny<Document<dynamic>>())).Returns(Task.CompletedTask);
            CouchbaseOperations.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(OperationResult.Object);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //ConfigureTestServices which will run after Startup.ConfigureServices.
            builder.ConfigureAppConfiguration(b => b.AddJsonFile("appSettings.json", optional: true))
                .ConfigureTestServices(s =>
                {
                    ServiceCollectionServiceExtensions.AddSingleton<ICouchbaseStartup>(s, p => new Mock<ICouchbaseStartup>().Object);
                    ServiceCollectionServiceExtensions.AddSingleton<ICouchbaseOperations>(s, p => CouchbaseOperations.Object);
                });
        }
    }
}