using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ruley.NET;

namespace Ruley.Tests.StageTests
{
    [TestFixture]
    class IfTests
    {
        [Test]
        public void SimpleConditionWithNoIfOrElse()
        {
            var ctx = new Context(new DynamicDictionary());
            var stage = new IfStage();
            stage.Value = new Property<bool>(ctx, "=> @event.x > 4 && @event.y < 3");

            var output = new List<Event>();
            stage.Subscribe(next => output.Add(next));

            var e = ctx.GetNext().Set("x", 5).Set("y", 2);
            stage.OnNext(e);

            e = ctx.GetNext().Set("x", 5).Set("y", 4);
            stage.OnNext(e);

            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(2, output[0]["y"]);
        }
    }
}
