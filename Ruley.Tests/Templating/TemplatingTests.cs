using NUnit.Framework;
using Ruley.NET;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Ruley.Tests.Templating
{
    [TestFixture]
    class TemplatingTests
    {
        public class TestObject1
        {
            public static TestObject1 StaticMember { get; set; }
            public TestObject2 TypedObjectMember { get; set; }
            public ExpandoObject ExpandoMember { get; set; }
            public Dictionary<string, string> DictionaryStringMember { get; set; }

            public int IntMember { get; set; }
            public double DoubleMember { get; set; }
            public string StringMember { get; set; }
            public string FieldMember;

            static TestObject1()
            {
                StaticMember = new TestObject1();
            }

            public TestObject1()
            {
                ExpandoMember = new ExpandoObject();

                var dict = ExpandoMember as IDictionary<string, object>;
                dict["DynamicSubMember"] = "Text";

                DictionaryStringMember = new Dictionary<string, string>();
                DictionaryStringMember["Key1"] = "Value1";
                IntMember = 10;
                DoubleMember = Math.PI;
                TypedObjectMember = new TestObject2();
                FieldMember = "Field";
            }
        }

        public class TestObject2
        {
            public DateTime DateTimeMember { get; set; }
            public string StringMember { get; set; }

            public TestObject2()
            {
                DateTimeMember = DateTime.MinValue;
            }
        }

        [Test]
        public void Test1()
        {
            var to = new TestObject1();

            var v = new ValueGetter("StaticMember.IntMember");
            Assert.AreEqual(10, v.Get(to));

            v = new ValueGetter("StaticMember.TypedObjectMember.DateTimeMember");
            Assert.AreEqual(DateTime.MinValue, v.Get(to));

            v = new ValueGetter("StaticMember.ExpandoMember.DynamicSubMember");
            Assert.AreEqual("Text", v.Get(to));
        }

        [Test]
        public void TemplateTest()
        {
            var to = new TestObject1();

            var templater = new Templater();
            templater.Compile("Value is {StaticMember.IntMember}");
            var result = templater.Apply(to);

            Assert.AreEqual("Value is 10", result);
        }

        [Test]
        public void FormttingTests()
        {
            var to = new TestObject1();

            var x = string.Format("{0:G3}", 4.03034d);

            var templater = new Templater();
            templater.Compile("{DoubleMember:G3}");
            var result = templater.Apply(to);
            Assert.AreEqual("3.14", result);
        }

        [Test]
        public void Handle_Invalid_Template()
        {
            var to = new TestObject1();
            to.StringMember = "TestString";

            var templater = new Templater();
            templater.Compile("{Invalid:G3}{StringMember}{StaticMember.NonexistantMember:G1}");
            var result = templater.Apply(to);
            Assert.AreEqual("TestString", result);
        }

        [Test]
        public void Handle_Invalid_Template2()
        {
            var to = new TestObject1();
            to.StringMember = "TestString";

            var templater = new Templater();
            templater.Compile("{Invalid:G3}{StringMember}{UnclosedToken");
            var result = templater.Apply(to);
            Assert.AreEqual("TestString", result);
        }

        [Test]
        public void Field_Members()
        {
            var to = new TestObject1();

            var templater = new Templater();
            templater.Compile("{FieldMember}");
            var result = templater.Apply(to);
            Assert.AreEqual("Field", result);
        }

        [Test]
        public void DynamicObject()
        {
            dynamic to = new ExpandoObject();
            to.Dynamic1 = "d1";
            to.Dynamic2 = 2;

            var templater = new Templater();
            templater.Compile("{Dynamic1}-d{Dynamic2}");
            var result = templater.Apply(to);
            Assert.AreEqual("d1-d2", result);
        }

        [Test]
        public void Props()
        {
            var to = new TestObject1();
            to.StringMember = "TestString";

            var templater = new Templater();
            templater.Compile("{DictionaryStringMember.Count}");
            var result = templater.Apply(to);
            Assert.AreEqual("1", result);
        }
    }
}
