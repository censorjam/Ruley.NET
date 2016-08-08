using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ruley.Core.Outputs
{
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon_emoji")]
        public string Emoji { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }
}