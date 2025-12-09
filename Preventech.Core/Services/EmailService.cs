using System;
using MailKit.Net.Smtp;
using MimeKit;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class EmailService(HttpClient httpClient)
{
    private readonly string emailAddress = "ploudosmingote@gmail.com";
    private readonly string emailAppPassword = "lxor abxl awlt jzsl";

    private readonly HttpClient _httpClient = httpClient;

    public string EnviarEmail(NotificacaoEmail notificacao)
    {
        try
        {
            notificacao.Titulo = "[Preventech] " + notificacao.Titulo;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailAddress));
            email.To.Add(MailboxAddress.Parse(notificacao.Destinatario));
            email.Subject = notificacao.Titulo;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = notificacao.Mensagem
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailAppPassword);
            smtp.Send(email);
            smtp.Disconnect(true);

            return "Email enviado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar email: {ex.Message}";
        }
    }

    public string EnviarEmailComArquivo(NotificacaoEmail notificacao, byte[] attachmentData, string attachmentFileName)
    {
        try
        {
            notificacao.Titulo = "[Preventech] " + notificacao.Titulo;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailAddress));
            email.To.Add(MailboxAddress.Parse(notificacao.Destinatario));
            email.Subject = notificacao.Titulo;

            var bodyBuilder = new BodyBuilder { HtmlBody = notificacao.Mensagem };

            if (attachmentData != null && attachmentData.Length > 0)
            {
                bodyBuilder.Attachments.Add(
                    attachmentFileName,
                    attachmentData,
                    ContentType.Parse(MimeTypes.GetMimeType(attachmentFileName))
                );
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailAppPassword);
            smtp.Send(email);
            smtp.Disconnect(true);

            return "Email enviado com sucesso (com anexos)!";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar email com anexos: {ex.Message}";
        }
    }

    public string SendEmail(string to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailAddress));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailAppPassword);
            smtp.Send(email);
            smtp.Disconnect(true);

            return "Email enviado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar email: {ex.Message}";
        }
    }

    public async Task<string> SendEmailAttach(string to, string subject, string body, byte[] attachmentData, string attachmentFileName)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailAddress));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };

            if (attachmentData != null && attachmentData.Length > 0)
            {
                bodyBuilder.Attachments.Add(
                    attachmentFileName,
                    attachmentData,
                    ContentType.Parse(MimeTypes.GetMimeType(attachmentFileName))
                );
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailAddress, emailAppPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return "Email enviado com sucesso (com anexos)!";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar email com anexos: {ex.Message}";
        }
    }
}
