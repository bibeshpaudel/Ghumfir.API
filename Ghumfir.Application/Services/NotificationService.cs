using Ghumfir.Application.Contracts;

namespace Ghumfir.Application.Services;

internal class NotificationService : INotificationService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Implement email sending logic here
        return Task.CompletedTask;
    }

    public Task SendSmsAsync(string to, string message)
    {
        // Implement SMS sending logic here
        return Task.CompletedTask;
    }
}