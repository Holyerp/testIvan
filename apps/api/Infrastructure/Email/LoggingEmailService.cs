using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Infrastructure.Email;

/// <summary>
/// Dev/mock <see cref="IEmailService"/>: logs that an email would be sent and returns —
/// it never contacts an SMTP server. Production wires a real MailKit SMTP implementation
/// via configuration; this no-op exists so consumers (e.g. US-021 password reset) can
/// depend on the interface during development without an outbound mail dependency.
/// Per .claude/rules/error-handling-and-logging.md only the recipient + subject are
/// logged — the body may contain PII or secrets (e.g. reset links) and is NOT logged.
/// </summary>
public class LoggingEmailService : IEmailService
{
    private readonly ILogger<LoggingEmailService> _logger;

    public LoggingEmailService(ILogger<LoggingEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // Body intentionally omitted from the log line (may contain PII / secrets).
        _logger.LogInformation(
            "[DEV email — not sent] to={Recipient} subject={Subject} bodyLength={BodyLength}",
            to, subject, body?.Length ?? 0);
        return Task.CompletedTask;
    }
}
