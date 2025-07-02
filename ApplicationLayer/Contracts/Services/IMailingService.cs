using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IMailingService
    {

        Task SendEmailAsync(string MailTo, string Subject, string Body, List<IFormFile> Attachments = null); 

    }
}
