using System;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Ruley.NET
{


    public class GroupByStage : Stage
    {
        [Primary]
        public Property<object> Key { get; set; }
        public Stage Action { get; set; }

        private ConcurrentDictionary<string, Stage> _groups = new ConcurrentDictionary<string, Stage>();

        private string _jsonClone;

        public override void OnFirst(Event e)
        {
            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            _jsonClone = JsonConvert.SerializeObject(new StageWrapper() { Stage = Action }, settings);
        }

        protected override void Process(Event msg)
        {
            var key = Key.Get(msg).ToString();

            var stage = _groups.GetOrAdd(key, k =>
            {
                var source = (Pipeline) Action;
                var s = new Pipeline(); 
                foreach (var sourceStage in source.Stages)
                {
                    s.Stages.Add(Clone(sourceStage));
                }

                s.Start();
                s.Subscribe((x) =>
                {
                    PushNext(x);
                });
                return s;
            });

            stage.OnNext(msg);
        }


        Stage Clone(Stage stage)
        {
            var type = stage.GetType();
            var newStage = (Stage)IoC.Resolve(type);

            newStage.Context = stage.Context;

            foreach (var prop in type.GetProperties())
            {
                if (prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Property<>))
                {
                    //todo this should probably clone, not sure if safe!
                    prop.SetValue(newStage, prop.GetValue(stage));
                }
            }

            return newStage;
        }

        class StageWrapper
        {
            public Stage Stage { get; set; }
        }
    }
}
