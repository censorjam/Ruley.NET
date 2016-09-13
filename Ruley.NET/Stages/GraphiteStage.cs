using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;

namespace Ruley.NET
{
    public class GraphiteStage : Stage
    {
        [Primary]
        [Mandatory]
        public Property<string> Query { get; set; }

        [Mandatory]
        public Property<string> Url { get; set; }

        private Dictionary<string, long> _lastSent = new Dictionary<string, long>();

        protected override void Process(Event msg)
        {
            using (var client = new HttpClient())
            {
                bool sent = false;
                client.BaseAddress = new Uri(Url.Get(msg));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var query = "render?target=" + HttpUtility.UrlEncode(Query.Get(msg)) + "&format=json&from=-45sec&to=now";
                var response = client.GetAsync(query).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic payload = new DynamicDictionary("{ 'graphitedata': " + data + " }");

                    if (payload.graphitedata.Count == 0)
                    {
                        Logger.Debug("No new datapoints found");
                        return;
                    }

                    foreach (var item in payload.graphitedata)
                    {
                        
                        List<object> p = item.datapoints;
                        foreach (IList<object> o in p)
                        {
                            if (!_lastSent.ContainsKey(item.target))
                                _lastSent[item.target] = 0;

                            if ((long)o[1] > _lastSent[item.target] && o[0] != null)
                            {
                                _lastSent[item.target] = (long)o[1];
                                dynamic e = Context.GetNext();
                                e.value = o[0];
                                e.key = item.target;
                                e.timestamp = new DateTime(1970, 1, 1).AddSeconds((long)o[1]).ToLocalTime();
                                e.graphiteJsonQuery = query;
                                e.graphitePngQuery = query.Replace("&format=json", "&format=png");

                                msg.Merge(e);

                                Logger.Debug(e);
                                PushNext(e);
                                sent = true;
                            }
                        }
                    }
                }
                if (!sent)
                    Logger.Debug("No new datapoints found");
            }
        }
    }
}
