﻿namespace Ghumfir.Application.Contracts.Notification;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendSmsAsync(string to, string message);
}
