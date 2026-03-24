using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class MailService : IMailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public MailService()
        {
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            _smtpPass = ConfigurationManager.AppSettings["SmtpPass"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("LifeSure Sigorta", _smtpUser));
                email.To.Add(new MailboxAddress("", to));
                email.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #0d6efd; color: white; padding: 20px; text-align: center; }}
                            .content {{ padding: 20px; background-color: #f8f9fa; }}
                            .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #6c757d; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>LifeSure Sigorta</h2>
                            </div>
                            <div class='content'>
                                {body.Replace("\n", "<br>")}
                            </div>
                            <div class='footer'>
                                <p>Bu otomatik bir yanıttır. Lütfen bu maili yanıtlamayın.</p>
                                <p>&copy; {DateTime.Now.Year} LifeSure Sigorta. Tüm hakları saklıdır.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                email.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
            
                throw;
            }
        }
    }
}