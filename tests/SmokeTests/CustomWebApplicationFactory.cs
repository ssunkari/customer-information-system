using System.Threading.Tasks;
using Api;
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

namespace SmokeTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        public Mock<IOperationResult<object>> OperationResult;

        public CustomWebApplicationFactory()
        {
            OperationResult = new Mock<IOperationResult<object>>();
            OperationResult.SetupGet(x => x.Success).Returns(false);

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //ConfigureTestServices which will run after Startup.ConfigureServices.
            builder.ConfigureAppConfiguration(b => b.AddJsonFile("appSettings.json", optional: true))
                .ConfigureTestServices(s =>
                {
                    s.AddSingleton<ICouchbaseStartup>(p => new Mock<ICouchbaseStartup>().Object);
                    s.AddSingleton<ICouchbaseOperations>(p =>
                    {
                        var couchbaseOperations = new Mock<ICouchbaseOperations>(MockBehavior.Strict);
                        couchbaseOperations.Setup(x => x.Upsert(It.IsAny<Document<dynamic>>())).Returns(Task.CompletedTask);
                        couchbaseOperations.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(OperationResult.Object);
                        return couchbaseOperations.Object;
                    });
                });
        }
    }
}