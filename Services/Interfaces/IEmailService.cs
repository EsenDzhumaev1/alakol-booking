namespace Alakol.Services.Interfaces;

public interface IEmailService
{
    Task SendBookingNotificationAsync(string name, string email, string phone);
}
