using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ruley.Core.Filters;
using Ruley.Core.Inputs;
using Ruley.Core.Outputs;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using System.Reflection;

namespace Ruley.Core
{
    public class RuleSet : IDisposable
    {
        internal string FileName { get; set; }
        public bool Debug { get; set; }
        public List<JObject> Params { get; set; }
        public Rule Definition { get; set; }

        private List<Rule> _rules = new List<Rule>();

        internal void Validate()
        {
            
        }

        public void Start()
        {
            if (Params == null)
                Params = new List<JObject>() { new JObject() };

            foreach (var obj in Params)
            {
                var rule = JsonConvert.DeserializeObject<Rule>(JsonConvert.SerializeObject(Definition, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                _rules.Add(rule);
                rule.Parameters = DynamicDictionary.Create(obj);
                rule.Name = FileName;
                rule.Start();
            }
        }

        public void Dispose()
        {
            
        }
    }

    public class Pipeline : Stage
    {
        public List<Stage> Stages { get; set; }

        public Pipeline()
        {
            Stages = new List<Stage>();
        }

        public override Stage NextStage
        {
            get { return Stages[Stages.Count - 1].NextStage; }
            set { Stages[Stages.Count - 1].NextStage = value; }
        }

        public static Pipeline FromDynamic(dynamic def)
        {
            var chain = new Pipeline();

            Stage prev = null;
            foreach (var f in def)
            {
                var stage = StageBuilder.Resolve(f);
                chain.Stages.Add(stage);

                if (prev != null)
                {
                    prev.NextStage = stage;
                }

                prev = stage;
            }
            return chain;
        }

        public override void Next(Event e)
        {
            Stages[0].Next(e);
        }

        public override void Start()
        {
            //reverse order?
            foreach (var stage in Stages)
            {
                stage.Start();
            }
        }
    }


    public class Rule : IDisposable
    {
        public bool Debug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Input Input { get; set; }
        public List<Output> Outputs { get; set; }
        public List<Filter> Filters { get; set; }
        public event Action<Exception> OnError;
        public DynamicDictionary Parameters { get; set; } //todo make this readonly

        internal void Validate()
        {
            if (Input == null)
                throw new Exception("Rule requires an input");

            if (Outputs == null || Outputs.Count == 0)
                throw new Exception("Rules require one or more outputs");

            GetComponents().ForEach(f => f.ValidateComposition());
        }

        public Event GetNext()
        {
            return new Event() { Parameters = Parameters };
        }

        private List<Component> GetComponents()
        {
            var list = new List<Component>();
            list.Add(Input);
            list.AddRange(Filters);
            list.AddRange(Outputs);
            return list;
        }

        public void Start()
        {
            Console.WriteLine("Starting rule '{0} with params {1}", Name, JsonConvert.SerializeObject(Parameters));
            if (Filters == null)
                Filters = new List<Filter>();

            Filters.Add(new TimestampFilter() { Destination = "$processedUtc" });

            var logger = new Logger { IsDebugEnabled = Debug };
            
            GetComponents().ForEach(c =>
            {
                c.Logger = new Logger {IsDebugEnabled = Debug || c.Debug};
                //c.Context = this; //todo need to propogate this to children
            });

            var stream = Input.Source;
            stream.Subscribe(o => { /*noop*/ }, e =>
            {
                if (OnError != null) OnError(e);
            });

            foreach (var filter in Filters)
            {
                if (filter.Enabled)
                {
                    var f = filter;
                    stream = stream.Do(m => logger.Debug("Applying filter {0}", f.GetType()));
                    stream = f.Extend(stream);
                }
            }

            foreach (var output in Outputs)
            {
                var o = output;
                if (o.Enabled)
                {
                    stream.Subscribe(m =>
                    {
                        //force outputs to execute sequentially in case they modify the payload
                        lock (m)
                        {
                            o.Do(m);
                        }
                    });
                }
            }

            Input.Start();
        }

        public void Dispose()
        {
            Console.WriteLine("Stopping rule '{0}'", Name);
            Input.Dispose();
        }
    }
}