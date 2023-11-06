using System.Net.Mail;
using System.Net;
using Serilog;
using StocksMarketEntitiesLibrary.EmailEntities;
using Serilog.Core;

namespace SharedServices.EmailServices
{
    public class EmailService:IEmailService
    {
       
        public EmailService()
        {
        }
        public async Task SendEmailsAsync(Emailer emailer)
        {
            SmtpClient smtpClient = CreateSmtpClient(emailer.smtpServer, emailer.smtpPort, emailer.username, emailer.password, emailer.enableSsl);

            // Her bir alıcıya e-posta gönderin (asenkron olarak)
            foreach (string to in emailer.toUserEmails)
            {
                MailMessage mail = CreateMail(emailer.from, to, emailer.subject, emailer.body);
                try
                {
                    await smtpClient.SendMailAsync(mail);
                    Log.Information($"E-posta {to} adresine başarıyla gönderildi.");
                }
                catch (Exception ex)
                {
                    Log.Error($"E-posta {to} adresine gönderilirken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    mail.Dispose();
                }
            }

            smtpClient.Dispose();
        }
        private SmtpClient CreateSmtpClient(string smtpServer, int smtpPort, string username, string password, bool enableSsl)
        {
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

            smtpClient.Credentials = new NetworkCredential(username, password);

            smtpClient.EnableSsl = enableSsl; // E-postayı SSL ile güvenli bir şekilde göndermek için.
            Log.Information("SmtpClient oluşturuldu");
            return smtpClient;
        }
        private MailMessage CreateMail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            Log.Information("MailMessage oluşturuldu");
            return mail;            
        }
    }
}
