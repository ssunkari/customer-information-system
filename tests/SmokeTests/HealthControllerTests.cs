using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SmokeTests
{

    public class HealthControllerTests
    {
        private TestServerFixture _fixture;

        [OneTimeSetUp]
        public void BeforeTest()
        {
            _fixture = new TestServerFixture();
        }

        [TearDown]
        public void CleanUp()
        {
            _fixture.Dispose();
        }

        [Test]
        public async Task HealthControllerHappyPath()
        {
            var response = await _fixture.Client.GetAsync("api/health");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            responseStrong.Should().Be("{\"environment\":\"Development\"}");
        }
    }
}
