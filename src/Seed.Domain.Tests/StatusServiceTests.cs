using System.Threading.Tasks;
using FakeItEasy;
using Shouldly;
using Seed.Common.Logging;
using Seed.Domain.Services;
using Xunit;

namespace Seed.Domain.Tests
{
    public class StatusServiceTests
    {
        [Fact]
        public async Task Should_return_a_message_containing_the_system_date_time()
        {
            var dummyLoggingService = A.Dummy<ILoggingService>();

            var statusService = new StatusService(dummyLoggingService);
            var status = await statusService.GetStatusAsync();

            status.ShouldStartWith($"Status is 'Up' at");
        }
    }
}