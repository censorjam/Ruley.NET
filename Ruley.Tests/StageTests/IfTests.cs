using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Ruley.NET;

namespace Ruley.Tests.StageTests
{
    public class StageTest
    {
        private Context ctx = new Context(new DynamicDictionary());
        public Stage Pipeline { get; private set; }
        public List<Event> Output { get; set; }

        public StageTest(Stage stage)
        {
            Output = new List<Event>();
            Pipeline = stage;
            Pipeline.Subscribe(s => { Output.Add(s); });
            Pipeline.Start();
        }

        public void Next(Action<Event> e)
        {
            var ev = ctx.GetNext();
            e(ev);
            Pipeline.OnNext(ev);
        }
    }

    public class MockTimeProvider : ITimeProvider
    {
        public DateTime Next { get; set; }

        public DateTime GetNow()
        {
            return Next;
        }
    }

    [TestFixture]
    class DebounceTests
    {
        private MockTimeProvider _timeProvider = new MockTimeProvider();

        [OneTimeSetUp]
        public void Setup()
        {
            IoC.Register<ITimeProvider>(_timeProvider);
        }

        [Test]
        public void AllowSingleValue()
        {
            _timeProvider.Next = new DateTime(2000, 1, 1);

            var stage = IoC.Resolve<DebounceStage>();
            stage.Period = TimeSpan.FromSeconds(10);
            var test = new StageTest(stage);

            var x = 0;
            test.Next(e => { e["a"] = x++; }); //0
            _timeProvider.Next += TimeSpan.FromSeconds(5);
            test.Next(e => { e["a"] = x++; }); //1
            _timeProvider.Next += TimeSpan.FromSeconds(10);
            test.Next(e => { e["a"] = x++; }); //2
            _timeProvider.Next += TimeSpan.FromSeconds(5);
            test.Next(e => { e["a"] = x++; }); //3
            _timeProvider.Next += TimeSpan.FromSeconds(5);

            Assert.AreEqual(0, test.Output[0]["a"]);
            Assert.AreEqual(2, test.Output[1]["a"]);
            Assert.AreEqual(2, test.Output.Count);
        }

        [Test]
        public void AllowMultipleValues()
        {
            _timeProvider.Next = new DateTime(2000, 1, 1);

            var stage = IoC.Resolve<DebounceStage>();
            stage.Period = TimeSpan.FromSeconds(10);
            stage.Allow = 2;
            var test = new StageTest(stage);

            var x = 0;
            test.Next(e => { e["a"] = x++; }); //0
            _timeProvider.Next += TimeSpan.FromSeconds(5);
            test.Next(e => { e["a"] = x++; }); //1
            _timeProvider.Next += TimeSpan.FromSeconds(2);
            test.Next(e => { e["a"] = x++; }); //2
            _timeProvider.Next += TimeSpan.FromSeconds(2);
            test.Next(e => { e["a"] = x++; }); //3
            _timeProvider.Next += TimeSpan.FromSeconds(5);

            Assert.AreEqual(0, test.Output[0]["a"]);
            Assert.AreEqual(1, test.Output[1]["a"]);
            Assert.AreEqual(3, test.Output[2]["a"]);
            Assert.AreEqual(3, test.Output.Count);
        }


        [Test]
        public void StartsFromFirst()
        {
            _timeProvider.Next = new DateTime(2000, 1, 1);

            var stage = IoC.Resolve<DebounceStage>();
            stage.Period = TimeSpan.FromSeconds(10);
            var test = new StageTest(stage);

            var x = 0;
            _timeProvider.Next += TimeSpan.FromSeconds(12);
            test.Next(e => { e["a"] = x++; });
            Assert.AreEqual(1, test.Output.Count);
        }
    }

    [TestFixture]
    class TemplateTests
    {
        [Test]
        public void A()
        {
            var stage = new TemplateStage()
            {
                Template = "{@event.x}",
                Field = "template"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual("abc", test.Output[0]["template"]);
        }
    }

    [TestFixture]
    class CountTests
    {
        [Test]
        public void SimpleCount()
        {
            var stage = new CountStage()
            {
                Field = "count",
            };
            var test = new StageTest(stage);

            test.Next(e => { });
            Assert.AreEqual(1, test.Output.Last()["count"]);
            test.Next(e => { });
            Assert.AreEqual(2, test.Output.Last()["count"]);
            test.Next(e => { });
            Assert.AreEqual(3, test.Output.Last()["count"]);
        }
    }

    [TestFixture]
    class DistinctTests
    {
        [Test]
        public void SimpleDistinct()
        {
            var stage = new DistinctStage()
            {
                Value = "{@event.x}"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual(1, test.Output.Count);
            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual(1, test.Output.Count);
            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual(1, test.Output.Count);
            test.Next(e => { e["x"] = "xyz"; });
            Assert.AreEqual(2, test.Output.Count);
            test.Next(e => { e["x"] = "xyz"; });
            Assert.AreEqual(2, test.Output.Count);
        }

        [Test]
        public void PassesNullValue()
        {
            var stage = new DistinctStage()
            {
                Value = "{@event.x}"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual(1, test.Output.Count);
            test.Next(e => { e["x"] = null; });
            Assert.AreEqual(2, test.Output.Count);
            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual(3, test.Output.Count);
        }

        [Test]
        public void FiltersDuplicateNullValue()
        {
            var stage = new DistinctStage()
            {
                Value = "{@event.x}"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = null; });
            Assert.AreEqual(1, test.Output.Count);
            test.Next(e => { e["x"] = null; });
            Assert.AreEqual(1, test.Output.Count);
        }
    }

    [TestFixture]
    class ScriptTests
    {
        [Test]
        public void Assign()
        {
            var stage = new ScriptStage()
            {
                Value = "@event.y = @event.x"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = "abc"; });
            Assert.AreEqual("abc", test.Output[0]["y"]);
        }

        [Test]
        public void MathRound()
        {
            var stage = new ScriptStage()
            {
                Value = "@event.y = Math.Round(@event.x, 2)"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = Math.PI; });
            Assert.AreEqual(3.14, test.Output[0]["y"]);
        }
    }

    [TestFixture]
    class PrevTests
    {
        [Test]
        public void FirstIsNull()
        {
            var stage = new PrevStage()
            {
                Field = "prev"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = 1; });
            Assert.IsNull(test.Output.Last()["prev"]);
        }

        [Test]
        public void HasPrev()
        {
            var stage = new PrevStage()
            {
                Field = "prev"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["x"] = 1; });
            test.Next(e => { e["x"] = 2; });
            dynamic prev = test.Output.Last()["prev"];
            Assert.AreEqual(1, prev.x);
        }
    }


    [TestFixture]
    class GroupByTests
    {
        [Test]
        public void WithSimpleAction()
        {
            var action = new CountStage()
            {
                Field = "count"
            };

            var stage = new GroupByStage()
            {
                Key = "{@event.b}"
            };
            stage.Action = action;

            var test = new StageTest(stage);

            test.Next(e => { e["b"] = "abc"; });
            Assert.AreEqual(1, test.Output.Last()["count"]);
            test.Next(e => { e["b"] = "abc"; });
            Assert.AreEqual(2, test.Output.Last()["count"]);
            test.Next(e => { e["b"] = "xyz"; });
            Assert.AreEqual(1, test.Output.Last()["count"]);
            test.Next(e => { e["b"] = "abc"; });
            Assert.AreEqual(3, test.Output.Last()["count"]);
        }
    }

    [TestFixture]
    class IfTests
    {
        [Test]
        public void BooleanValue()
        {
            var stage = new IfStage()
            {
                Value = "=> @event.b"
            };
            var test = new StageTest(stage);

            test.Next(e => { e["b"] = false; });
            test.Next(e => { e["b"] = true; });

            Assert.AreEqual(1, test.Output.Count);
        }

        [Test]
        public void Branching()
        {
            var stage = new IfStage()
            {
                Value = "=> @event.b"
            };

            stage.Then = new TimestampStage() { Field = "A" };
            stage.Else = new TimestampStage() { Field = "B" };

            var test = new StageTest(stage);

            test.Next(e => { e["b"] = false; });
            Assert.IsTrue(test.Output.Last().ContainsKey("B"));
            Assert.IsFalse(test.Output.Last().ContainsKey("A"));

            test.Next(e => { e["b"] = true; });
            Assert.IsTrue(test.Output.Last().ContainsKey("A"));
            Assert.IsFalse(test.Output.Last().ContainsKey("B"));
        }
    }
}
