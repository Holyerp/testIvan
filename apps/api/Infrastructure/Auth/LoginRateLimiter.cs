using System.Collections.Concurrent;

namespace Pinoles.Api.Infrastructure.Auth;

public class LoginRateLimiter
{
    private readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _attempts = new();
    private const int MaxAttempts = 5;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(15);

    public bool IsRateLimited(string ip)
    {
        var now = DateTime.UtcNow;
        if (!_attempts.TryGetValue(ip, out var entry))
            return false;

        if (now - entry.WindowStart > Window)
        {
            _attempts[ip] = (0, now);
            return false;
        }
        return entry.Count >= MaxAttempts;
    }

    public void RecordFailure(string ip)
    {
        var now = DateTime.UtcNow;
        _attempts.AddOrUpdate(
            ip,
            _ => (1, now),
            (_, existing) => now - existing.WindowStart > Window
                ? (1, now)
                : (existing.Count + 1, existing.WindowStart));
    }

    public void Reset(string ip)
    {
        _attempts.TryRemove(ip, out _);
    }
}
