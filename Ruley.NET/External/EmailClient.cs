using System;
using System.Net.Mail;

namespace Ruley.NET.External
{
    public class EmailClient : IEmailClient
    {
        private SmtpClient _smtpClient = new SmtpClient();

        public EmailClient()
        {
            _smtpClient = new SmtpClient();
        }

        public string Host
        {
            get
            {
                return _smtpClient.Host;
            }
            set
            {
                _smtpClient.Host = value;
            }
        }

        public int Port
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Send(MailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
