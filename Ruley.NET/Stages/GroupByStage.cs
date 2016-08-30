using System;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Ruley.NET
{
    public class StageWrapper
    {
        public Stage Stage { get; set; }
    }

    public class GroupByStage : Stage
    {
        [Primary]
        public Property<object> Key { get; set; }
        public Stage Action { get; set; }

        private ConcurrentDictionary<string, Stage> _groups = new ConcurrentDictionary<string, Stage>();
        
        protected override void Process(Event msg)
        {
            var key = Key.Get(msg).ToString();

            var stage = _groups.GetOrAdd(key, k =>
            {
                var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                var clone = JsonConvert.SerializeObject(new StageWrapper() { Stage = Action }, settings);
                var s = JsonConvert.DeserializeObject<StageWrapper>(clone, settings).Stage;
                s.Start();
                s.Subscribe((x) =>
                {
                    PushNext(x);
                });
                return s;
            });

            stage.OnNext(msg);
        }
    }
}
