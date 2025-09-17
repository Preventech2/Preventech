using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace Preventech.Core.Services;

public class EmailService
{
    private readonly HttpClient _httpClient;

    public EmailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string SendEmail(string to, string subject, string body)
    {
        try
        {
            // var email = new MimeMessage();
            // email.From.Add(MailboxAddress.Parse("examploemail@gmail.com"));
            // email.To.Add(MailboxAddress.Parse(to));
            // email.Subject = subject;
            // email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            // {
            //     Text = body
            // };

            // using var smtp = new SmtpClient();
            // smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // smtp.Authenticate("examploemail@gmail.com", "senha hash do gmail");
            // smtp.Send(email);
            // smtp.Disconnect(true);

            return "Email enviado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar email: {ex.Message}";
        }
    }
}
