using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ruley.Core.Filters
{
    public class EmailFilter : InlineFilter
    {
        public Property<string> Smtp { get; set; }
        public Property<string> From { get; set; }
        public Property<string> To { get; set; }
        public Property<string> Subject { get; set; }
        public Property<string> Body { get; set; }

        public override Event Apply(Event msg)
        {
            using (var client = new SmtpClient(Smtp.Get(msg)))
            {
                var message = new MailMessage();

                foreach (var address in To.Get(msg).Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }

                message.From = new MailAddress(From.Get(msg));
                message.Body = Body.Get(msg);
                message.Subject = Subject.Get(msg);
                message.IsBodyHtml = true;

                client.Send(message);
            }
            return msg;
        }
    }
}
