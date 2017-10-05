using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Seed.Api.Tests.Infrastructure;
using Seed.Common.Tests;
using Xunit;

namespace FxOnline.Api.Tests
{
    public class StatusControllerTests
    {
        [Fact]
        public async Task Should_return_200_OK_and_a_status_message()
        {
            var userId = "1234";

            using (var webServer = new TestWebServer())
            using (var client = webServer.GetClient(userId))
            {
                var response = await client.GetAsync($"/api/status");
                var content = await response.GetApiResponseDataAsync();

                response.StatusCode.ShouldBe(HttpStatusCode.OK);

                content.ShouldContain("Status is 'Up' at");
            }
        }
    }
}