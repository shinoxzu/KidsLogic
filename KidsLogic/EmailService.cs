using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace KidsLogic;

public class EmailService
{
    private readonly string _email;
    private readonly string _password;

    public EmailService(string email, string password)
    {
        _email = email;
        _password = password;
    }

    public async Task SendRegistrationPasswordAsync(string email, string name, string password)
    {
        MimeMessage emailMessage = new();

        emailMessage.From.Add(new MailboxAddress("KidsLogic", _email));
        emailMessage.To.Add(new MailboxAddress(name, email));
        emailMessage.Subject = "Регистрация аккаунта";
        emailMessage.Body = new TextPart(TextFormat.Plain)
        {
            Text = $"Ваш пароль для авторизации на сайте: {password}. Если Вы ничего не делали, то " +
                   "просто проигнорируйте это письмо."
        };

        using SmtpClient client = new();
        await client.ConnectAsync("smtp.mail.ru", 465, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_email, _password);
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }
}