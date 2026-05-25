namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Outbound email abstraction. Consumers (e.g. US-021 password reset) depend on this
/// interface only. The dev/mock implementation logs and does NOT send; production wires
/// a real MailKit SMTP implementation via configuration (see
/// <see cref="Infrastructure.Email.LoggingEmailService"/>).
/// </summary>
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
}
