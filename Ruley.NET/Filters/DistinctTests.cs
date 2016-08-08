using NUnit.Framework;
using Ruley.Dynamic;

namespace Ruley.Core.Filters
{
    [TestFixture]
    public class DistinctTests
    {
        [Test]
        public void String_Field_Test()
        {
            var distinct = new DistinctFilter();
            distinct.Value = new Property<string>("[prop1]");

            var ev = new Event();
            ev.Data["prop1"] = "abc";

            var ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = "abc";

            ev2 = distinct.Apply(ev);
            Assert.AreEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = "def";

            ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);
        }

        [Test]
        public void Templated_Value_Test()
        {
            var distinct = new DistinctFilter();
            distinct.Value = new Property<string>("{prop1}{prop2}");

            var ev = new Event();
            ev.Data["prop1"] = "abc";
            ev.Data["prop2"] = 123;

            var ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = "abc";
            ev.Data["prop2"] = 123;

            ev2 = distinct.Apply(ev);
            Assert.AreEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = "abc";
            ev.Data["prop2"] = null;

            ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);
        }

        [Test]
        public void Null_Field_Test()
        {
            var distinct = new DistinctFilter();
            distinct.Value = new Property<string>("[prop1]");

            var ev = new Event();
            ev.Data["prop1"] = null;

            var ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = null;

            ev2 = distinct.Apply(ev);
            Assert.AreEqual(null, ev2);
        }

        [Test]
        public void Null_String_Field_Test()
        {
            var distinct = new DistinctFilter();
            distinct.Value = new Property<string>("[prop1]");

            var ev = new Event();
            ev.Data["prop1"] = null;

            var ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);

            ev = new Event();
            ev.Data["prop1"] = "null";

            ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);
        }

        [Test]
        public void Nested_Field_Test()
        {
            var distinct = new DistinctFilter();
            distinct.Value = new Property<string>("[prop1.prop2]");

            var ev = new Event();
            dynamic data = ev.Data;
            data.prop1 = new DynamicDictionary();
            data.prop1.prop2 = "abc";

            var ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);

            ev = new Event();
            data = ev.Data;
            data.prop1 = new DynamicDictionary();
            data.prop1.prop2 = "abc";

            ev2 = distinct.Apply(ev);
            Assert.AreEqual(null, ev2);

            ev = new Event();
            data = ev.Data;
            data.prop1 = new DynamicDictionary();
            data.prop1.prop2 = "def";

            ev2 = distinct.Apply(ev);
            Assert.AreNotEqual(null, ev2);
        }
    }
}