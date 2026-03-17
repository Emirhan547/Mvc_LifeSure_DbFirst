using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
