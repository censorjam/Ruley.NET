using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    public class GraphiteFilter : InlineFilter
    {
        public Property<string> From { get; set; }
        public Property<string> To { get; set; }
        public Property<string> Query { get; set; }

        private long _lastSent;

        public override Event Apply(Event msg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://lapgrp001:8080");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var query = "render?target=" + Query.Get(msg) + "&format=json&from=" + From.Get(msg) + "&to=" + To.Get(msg);
                var response = client.GetAsync(query).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic payload = DynamicDictionary.Create("{ 'graphitedata': " + data + " }");
                    List<object> p = payload.graphitedata[0].datapoints;// ["graphitedata"];
                    p.Reverse();

                    foreach (IList<object> o in p)
                    {
                        if ((long)o[1] > _lastSent && o[0] != null)
                        {
                            _lastSent = (long)o[1];
                            dynamic e = new DynamicDictionary();
                            e.Value = o[0];
                            e.Timestamp = new DateTime(1970,1,1).AddSeconds((long)o[1]).ToLocalTime();
                            e.Query = query;

                            msg.Data.Merge(e);
                            return msg;
                        }
                    }
                }
                return null;
                //throw new Exception("graphite call failed");
            }
        }
    }
}
