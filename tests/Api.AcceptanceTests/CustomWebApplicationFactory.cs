using Api.Couchbase;
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
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //ConfigureTestServices which will run after Startup.ConfigureServices.
            builder.ConfigureAppConfiguration(b => b.AddJsonFile("appSettings.json"))
                .ConfigureTestServices(s =>
                {
                    ServiceCollectionServiceExtensions.AddSingleton<ICouchbaseStartup>(s, p => new Mock<ICouchbaseStartup>().Object);
                    ServiceCollectionServiceExtensions.AddSingleton<ICouchbaseOperations>(s, p => new Mock<ICouchbaseOperations>().Object);
                });
        }
    }
}