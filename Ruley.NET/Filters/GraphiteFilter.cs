using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Ruley.Dynamic;
using System.Net.Http;

namespace Ruley.Core.Filters
{
    public class GraphiteFilter : InlineFilter
    {
        public Property<string> Query { get; set; }

        public Property<string> Url { get; set; }

        private Dictionary<string, long> _lastSent = new Dictionary<string, long>();

        public override Event Apply(Event msg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url.Get(msg));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var query = "render?target=" + Query.Get(msg) + "&format=json&from=-45sec&to=now";
                var response = client.GetAsync(query).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic payload = DynamicDictionary.Create("{ 'graphitedata': " + data + " }");

                    if (payload.graphitedata.Count == 0)
                    {
                        Console.WriteLine("Query returned 0 datapoints");
                        return null;
                    }

                    foreach (var item in payload.graphitedata)
                    {
                        List<object> p = item.datapoints;// ["graphitedata"];
                        p.Reverse();

                        foreach (IList<object> o in p)
                        {
                            if (!_lastSent.ContainsKey(item.target))
                                _lastSent[item.target] = 0;

                            if ((long)o[1] > _lastSent[item.target] && o[0] != null)
                            {
                                _lastSent[item.target] = (long)o[1];
                                dynamic e = new DynamicDictionary();
                                e.value = o[0];
                                e.key = item.target;
                                e.timestamp = new DateTime(1970, 1, 1).AddSeconds((long)o[1]).ToLocalTime();
                                e.graphiteJsonQuery = query;
                                e.graphitePngQuery = query.Replace("&format=json", "&format=png");

                                msg.Data.Merge(e);
                                return msg;
                            }
                        }
                    }
                }
                return null;
            }
        }
    }
}
