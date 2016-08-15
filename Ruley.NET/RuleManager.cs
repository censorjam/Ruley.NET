//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Ruley.Core
//{
//    public class RuleManager
//    {
//        public static MessageBus MessageBus = new MessageBus();
//        private readonly Dictionary<string, RuleSet> _rules = new Dictionary<string, RuleSet>();
//        private readonly RuleProvider _ruleProvider;
//        private readonly Preprocessor _preprocessor;
//        private List<string> _ignoreFile = new List<string>() { "global" };

//        public void Add(RuleSet rule)
//        {
//            _rules.Add("test123", rule);
//        }

//        public List<string> GetRuleNames()
//        {
//            return _rules.Keys.ToList();
//        }

//        public RuleManager()
//        {
//            //todo: autogenerate
//            _preprocessor = new Preprocessor();
//            _preprocessor.Alias("@interval", "Ruley.Core.Inputs.IntervalInput, Ruley.NET");
//            _preprocessor.Alias("@slack.out", "Ruley.Core.Outputs.SlackOutput, Ruley.NET");
//            _preprocessor.Alias("@map", "Ruley.Core.Filters.MapFilter, Ruley.NET");
//            _preprocessor.Alias("@throttle", "Ruley.Core.Filters.ThrottleFilter, Ruley.NET");
//            _preprocessor.Alias("@concat", "Ruley.Core.Filters.ConcatFilter, Ruley.NET");
//            _preprocessor.Alias("@conditional", "Ruley.Core.Filters.ConditionalFilter, Ruley.NET");
//            _preprocessor.Alias("@merge", "Ruley.Core.Filters.MergeFilter, Ruley.NET");
//            _preprocessor.Alias("@chain", "Ruley.Core.Filters.ChainFilter, Ruley.NET");
//            _preprocessor.Alias("@groupby", "Ruley.Core.Filters.GroupByFilter, Ruley.NET");
//            _preprocessor.Alias("@calc", "Ruley.Core.Filters.CalcFilter, Ruley.NET");
//            _preprocessor.Alias("@template", "Ruley.Core.Filters.TemplateFilter, Ruley.NET");
//            _preprocessor.Alias("@prev", "Ruley.Core.Filters.PrevFilter, Ruley.NET");
//            _preprocessor.Alias("@count", "Ruley.Core.Filters.CountFilter, Ruley.NET");
//            _preprocessor.Alias("@slack", "Ruley.Core.Filters.SlackFilter, Ruley.NET");
//            _preprocessor.Alias("@script", "Ruley.Core.Filters.ScriptFilter, Ruley.NET");
//            _preprocessor.Alias("@passthrough", "Ruley.Core.Filters.PassThroughFilter, Ruley.NET");
//            _preprocessor.Alias("@graphite", "Ruley.Core.Filters.GraphiteFilter, Ruley.NET");

//            _preprocessor.Alias("@bus.out", "Ruley.Core.Filters.MessageBusPublisherFilter, Ruley.NET");
//            _preprocessor.Alias("@bus.in", "Ruley.Core.Filters.MessageBusSubscriberFilter, Ruley.NET");

//            _preprocessor.Alias("@if", "Ruley.Core.Filters.BranchFilter, Ruley.NET");
//            _preprocessor.Alias("@replayField", "Ruley.Core.Filters.ReplayFieldFilter, Ruley.NET");
//            _preprocessor.Alias("@debounce", "Ruley.NET.Filters.DebounceFilter, Ruley.NET");
//            _preprocessor.Alias("@regex", "Ruley.Core.Filters.RegexFilter, Ruley.NET");

//            _preprocessor.Alias("@uppercase", "Ruley.Core.Filters.UpperCaseFilter, Ruley.NET");
//            _preprocessor.Alias("@lowercase", "Ruley.Core.Filters.LowerCaseFilter, Ruley.NET");
//            _preprocessor.Alias("@maxlength", "Ruley.Core.Filters.MaxLengthFilter, Ruley.NET");

//            _preprocessor.Alias("@email", "Ruley.Core.Filters.EmailFilter, Ruley.NET");

//            _preprocessor.Alias("@distinct", "Ruley.Core.Filters.DistinctFilter, Ruley.NET");
//            _preprocessor.Alias("@skip", "Ruley.Core.Filters.SkipFilter, Ruley.NET");
//            _preprocessor.Alias("@http", "Ruley.Core.Filters.HttpFilter, Ruley.NET");

//            _preprocessor.Alias("@redis.ping", "Ruley.Redis.RedisPingFilter, Ruley.Redis");
//            _preprocessor.Alias("@redis.info", "Ruley.Redis.RedisInfoInput, Ruley.Redis");

//            _preprocessor.Alias("@rabbitmq.in", "Ruley.RabbitMq.RabbitMqInput, Ruley.RabbitMq");
//            _preprocessor.Alias("@consolef", "Ruley.Core.Filters.ConsoleFilter, Ruley.NET");
//            _preprocessor.Alias("@graphite.out", "Ruley.NET.Filters.GraphiteOutputFilter, Ruley.NET");
//            _preprocessor.Alias("@console", "Ruley.Core.Outputs.ConsoleOutput, Ruley.NET");

//            _preprocessor.Alias("@slack_url", "https://hooks.slack.com/services/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");

//            _ruleProvider = new RuleProvider(_preprocessor);
//        }

//        public void StartFileWatcher()
//        {
//            var watcher = new FileSystemWatcher("rules");
//            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
//                                   NotifyFilters.DirectoryName | NotifyFilters.CreationTime;

//            watcher.Filter = "*.json";

//            var w = new ObservableFileWatcher(watcher);

//            w.Changed.Subscribe(fn =>
//            {
//                try
//                {
//                    lock (_rules)
//                    {
//                        if (_rules.ContainsKey(fn))
//                        {
//                            Console.WriteLine("File change detected ({0})", fn);
//                            _rules[fn].Dispose();

//                            var rule = _ruleProvider.Create(fn);
//                            _rules[fn] = rule;
//                            _rules[fn].Start();
//                        }
//                        else
//                        {
//                            Console.WriteLine("Couldn't change rule {0}", fn);
//                        }
//                    }
//                }
//                catch (Exception e)
//                {

//                }
//            });

//            w.Created.Subscribe(fn =>
//            {
//                lock (_rules)
//                {
//                    if (!_rules.ContainsKey(fn))
//                    {
//                        Console.WriteLine("File added detected ({0})", fn);
//                        _rules[fn] = _ruleProvider.Create(fn);
//                        _rules[fn].Start();
//                    }
//                }
//            });

//            w.Deleted.Subscribe(fn =>
//            {
//                lock (_rules)
//                {
//                    if (_rules.ContainsKey(fn))
//                    {
//                        Console.WriteLine("File deleted detected ({0})", fn);
//                        _rules[fn].Dispose();
//                        Console.WriteLine("Removing rule ({0})", fn);
//                        _rules.Remove(fn);
//                    }
//                }
//            });
//        }

//        public void Start()
//        {
//            lock (_rules)
//            {
//                foreach (var fn in Directory.GetFiles("rules"))
//                {
//                    try
//                    {
//                        if (_ignoreFile.Contains(fn))
//                            continue;

//                        _rules[fn] = _ruleProvider.Create(fn);
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(String.Format("Failed to load rule {0}", e.Message));
//                    }
//                }

//                foreach (var rulePair in _rules)
//                {
//                    rulePair.Value.Start();
//                }
//            }
//        }
//    }
//}