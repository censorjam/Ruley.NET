using System;
using System.Net.Mail;

namespace Ruley.NET.External
{
    interface IEmailClient : IDisposable
    {
        void Send(MailMessage message);
        string Host { get; set; }
        int Port { get; set; }
    }
}
