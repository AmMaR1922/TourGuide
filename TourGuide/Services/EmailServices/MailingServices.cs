using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TourGuide.Services.EmailServices
{
    public class MailingServices(IOptions<MailSettings> options) : IMailingService
    {
        private readonly MailSettings settings = options.Value;
        public async Task SendEmailAsync(string MailTo, string Subject, string Body, List<IFormFile> Attachments = null)
        {
            var email = new MimeMessage() { 
              Sender = MailboxAddress.Parse(settings.Email),
              Subject = Subject,
            };
            email.To.Add(MailboxAddress.Parse(MailTo));

            var Builder = new BodyBuilder();

            if(Attachments is not null)
            {
                byte[] FileBytes;
                foreach(var attachment in Attachments)
                {
                    if (attachment.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            attachment.CopyTo(stream);
                            FileBytes = stream.ToArray();
                            Builder.Attachments.Add(attachment.FileName, FileBytes, ContentType.Parse(attachment.ContentType));


                        }


                    }   }
            }
            Builder.HtmlBody = Body;
            email.Body = Builder.ToMessageBody();
            email.From.Add(new MailboxAddress(settings.DisplayName,settings.Email));

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(settings.Host, settings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(settings.Email, settings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }




        }
    }
}
