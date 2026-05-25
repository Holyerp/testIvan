using Pinoles.Api.Infrastructure.Auth;
using Xunit;

namespace Pinoles.Api.Tests.Application.Auth;

public class LoginRateLimiterTests
{
    [Fact]
    public void IsRateLimited_Initially_ReturnsFalse()
    {
        var limiter = new LoginRateLimiter();
        Assert.False(limiter.IsRateLimited("1.2.3.4"));
    }

    [Fact]
    public void IsRateLimited_After5Failures_ReturnsTrue()
    {
        var limiter = new LoginRateLimiter();
        for (int i = 0; i < 5; i++) limiter.RecordFailure("1.2.3.4");
        Assert.True(limiter.IsRateLimited("1.2.3.4"));
    }

    [Fact]
    public void IsRateLimited_After4Failures_ReturnsFalse()
    {
        var limiter = new LoginRateLimiter();
        for (int i = 0; i < 4; i++) limiter.RecordFailure("1.2.3.4");
        Assert.False(limiter.IsRateLimited("1.2.3.4"));
    }

    [Fact]
    public void Reset_ClearsRateLimit()
    {
        var limiter = new LoginRateLimiter();
        for (int i = 0; i < 5; i++) limiter.RecordFailure("1.2.3.4");
        limiter.Reset("1.2.3.4");
        Assert.False(limiter.IsRateLimited("1.2.3.4"));
    }

    [Fact]
    public void DifferentIPs_IndependentLimits()
    {
        var limiter = new LoginRateLimiter();
        for (int i = 0; i < 5; i++) limiter.RecordFailure("1.2.3.4");
        Assert.True(limiter.IsRateLimited("1.2.3.4"));
        Assert.False(limiter.IsRateLimited("5.6.7.8"));
    }
}
