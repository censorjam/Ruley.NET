using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ruley.NET
{
    public class SlackFilter : InlineFilter
    {
        private SlackClient _slackClient;
        public Property<string> WebHookUrl { get; set; }
        public Property<string> Username { get; set; }
        public Property<string> Message { get; set; }
        public Property<string> AttachmentMessage { get; set; }
        public Property<string> AttachmentFooter { get; set; }
        public Property<string> Channel { get; set; }
        public Property<string> Emoji { get; set; }

        public override Event Apply(Event @event)
        {
            if (_slackClient == null)
                _slackClient = new SlackClient(WebHookUrl.Get(@event));

            var payload = new Payload()
            {
                Text = Message.Get(@event),
                Username = Username.Get(@event),
                Emoji = Emoji.Get(@event)
            };

            if (Channel != null)
                payload.Channel = Channel.Get(@event);


            if (AttachmentMessage != null)
            {
                if (AttachmentFooter != null)
                {
                    payload.Attachments = new List<object>();
                    payload.Attachments.Add(new
                    {
                        image_url =
                            "https://www.google.co.uk/imgres?imgurl=http%3A%2F%2Ffindicons.com%2Ffiles%2Ficons%2F1689%2Fsplashy%2F16%2Ferror.png&imgrefurl=http%3A%2F%2Ffindicons.com%2Ficon%2F118890%2Fexclamation_red&docid=IcyuDOH455WIrM&tbnid=utmzfUIRH2NV1M%3A&w=16&h=16&bih=1115&biw=1920&ved=0ahUKEwjA37jzlevNAhVkCcAKHUJ6A1MQMwgiKAMwAw&iact=mrc&uact=8",
                        color = "#c02f37",
                        fallback = "fallback",
                        title = "Click for latest errors in Kibana",
                        title_link = AttachmentFooter.Get(@event),
                        text = AttachmentMessage.Get(@event)
                    });
                }
                else
                {
                    payload.Attachments = new List<object>();
                    payload.Attachments.Add(new
                    {
                        image_url = "https://www.google.co.uk/imgres?imgurl=http%3A%2F%2Ffindicons.com%2Ffiles%2Ficons%2F1689%2Fsplashy%2F16%2Ferror.png&imgrefurl=http%3A%2F%2Ffindicons.com%2Ficon%2F118890%2Fexclamation_red&docid=IcyuDOH455WIrM&tbnid=utmzfUIRH2NV1M%3A&w=16&h=16&bih=1115&biw=1920&ved=0ahUKEwjA37jzlevNAhVkCcAKHUJ6A1MQMwgiKAMwAw&iact=mrc&uact=8",
                        color = "#c02f37",
                        fallback = "fallback",
                        text = AttachmentMessage.Get(@event)
                    });
                }
            }

            try
            {
                _slackClient.PostMessage(payload);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            Logger.Debug(JsonConvert.SerializeObject(payload));
            return @event;
        }
    }
}