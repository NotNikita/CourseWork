using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comics.Services.Abstract
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string message);

        void SendEmail(string email, string subject, string message);
    }
}
