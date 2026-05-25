using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Infrastructure.Email;
using Xunit;

namespace Pinoles.Api.Tests.Infrastructure.Email;

public class LoggingEmailServiceTests
{
    [Fact]
    public async Task SendAsync_DoesNotThrow()
    {
        var service = new LoggingEmailService(NullLogger<LoggingEmailService>.Instance);

        // The dev no-op implementation logs and returns — it must complete without throwing.
        await service.SendAsync("user@example.com", "Password reset", "https://example.com/reset?token=secret");
    }

    [Fact]
    public async Task SendAsync_CompletesSynchronously()
    {
        var service = new LoggingEmailService(NullLogger<LoggingEmailService>.Instance);

        var task = service.SendAsync("user@example.com", "Subject", "body");
        Assert.True(task.IsCompletedSuccessfully);
        await task;
    }
}
