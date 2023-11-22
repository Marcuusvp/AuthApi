using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Aplicacao.Interface
{
    public interface IEmailService
    {
         public bool Send(
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Equipe Full-Stack",
            string fromEmail = "marcusvpdev@gmail.com");
    }
}