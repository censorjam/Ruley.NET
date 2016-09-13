using System;
using System.Net.Mail;

namespace Ruley.NET.External
{
    public interface IEmailClient
    {
        void Send(MailMessage message);
        string Host { get; set; }
        int Port { get; set; }
    }
}
