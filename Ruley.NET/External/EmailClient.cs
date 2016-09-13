using System;
using System.Net.Mail;

namespace Ruley.NET.External
{
    public class EmailClient : IEmailClient
    {
        public EmailClient()
        {
        }

        public string Host { get; set; }
        public int Port { get; set; }

        public void Send(MailMessage message)
        {
            using (var client = new SmtpClient(Host))
            {
                client.Send(message);
            }
        }
    }
}
