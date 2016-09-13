using System;
using System.Net.Mail;
using Ruley.NET.External;
using TinyIoC;

namespace Ruley.NET
{
    class EmailStage : InlineStage
    {
        [Primary]
        public Property<string> Body { get; set; }

        public Property<string> Smtp { get; set; }
        public Property<string> From { get; set; }
        public Property<string> To { get; set; }
        public Property<string> Subject { get; set; }

        public override Event Apply(Event e)
        {
            var client = new EmailClient();
            client.Host = Smtp.Get(e);
            var message = new MailMessage();

            foreach (var address in To.Get(e).Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address);
            }

            message.From = new MailAddress(From.Get(e));
            message.Body = Body.Get(e);
            message.Subject = Subject?.Get(e);
            message.IsBodyHtml = true;

            client.Send(message);
            return e;
        }
    }
}
