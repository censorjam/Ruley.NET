//using System;
//using NUnit.Framework;
//using Ruley;

//namespace Ruley.Core.Filters
//{
//    [TestFixture]
//    public class DistinctTests
//    {
//	    [Test]
//	    public void Compare1()
//	    {
//		    dynamic d1 = new DynamicDictionary();
//		    d1.Test = "a";

//			dynamic d2 = new DynamicDictionary();
//			d2.Test = "a";

//			Assert.IsTrue(d1.Equals(d2));
//		}

//		[Test]
//		public void Compare2()
//		{
//			dynamic d1 = new DynamicDictionary();
//			d1.Test = "a";

//			dynamic d2 = new DynamicDictionary();
//			d2.Test = "b";

//			Assert.IsFalse(d1.Equals(d2));
//		}

//		[Test]
//		public void Compare3()
//		{
//			dynamic d1 = new DynamicDictionary();
//			d1.Test = 1;

//			dynamic d2 = new DynamicDictionary();
//			d2.Test = 1;

//			Assert.IsTrue(d1.Equals(d2));
//		}
		
//	    [Test]
//	    public void Compare4()
//	    {
//		    dynamic d1 = new DynamicDictionary();
//		    d1.Test = TimeSpan.FromSeconds(5);

//		    dynamic d2 = new DynamicDictionary();
//		    d2.Test = TimeSpan.FromSeconds(5);

//		    Assert.IsTrue(d1.Equals(d2));
//	    }

//	    [Test]
//		public void Compare5()
//		{
//			dynamic d1 = new DynamicDictionary();
//			d1.Test = new DynamicDictionary();
//		    d1["Test"]["Test2"] = "hello";

//			dynamic d2 = new DynamicDictionary();
//			d2.Test = new DynamicDictionary();
//			d2["Test"]["Test2"] = "hello";

//			Assert.IsTrue(d1.Equals(d2));
//		}

//		[Test]
//        public void String_Field_Test()
//        {
//            var distinct = new DistinctFilter();
//            distinct.Value = new Property<string>("[prop1]");

//            var ev = new Event();
//            ev.Data["prop1"] = "abc";

//            var ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = "abc";

//            ev2 = distinct.Apply(ev);
//            Assert.AreEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = "def";

//            ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);
//        }

//        [Test]
//        public void Templated_Value_Test()
//        {
//            var distinct = new DistinctFilter();
//            distinct.Value = new Property<string>("{prop1}{prop2}");

//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            ev.Data["prop2"] = 123;

//            var ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = "abc";
//            ev.Data["prop2"] = 123;

//            ev2 = distinct.Apply(ev);
//            Assert.AreEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = "abc";
//            ev.Data["prop2"] = null;

//            ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);
//        }

//        [Test]
//        public void Null_Field_Test()
//        {
//            var distinct = new DistinctFilter();
//            distinct.Value = new Property<string>("[prop1]");

//            var ev = new Event();
//            ev.Data["prop1"] = null;

//            var ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = null;

//            ev2 = distinct.Apply(ev);
//            Assert.AreEqual(null, ev2);
//        }

//        [Test]
//        public void Null_String_Field_Test()
//        {
//            var distinct = new DistinctFilter();
//            distinct.Value = new Property<string>("[prop1]");

//            var ev = new Event();
//            ev.Data["prop1"] = null;

//            var ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);

//            ev = new Event();
//            ev.Data["prop1"] = "null";

//            ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);
//        }

//        [Test]
//        public void Nested_Field_Test()
//        {
//            var distinct = new DistinctFilter();
//            distinct.Value = new Property<string>("[prop1.prop2]");

//            var ev = new Event();
//            dynamic data = ev.Data;
//            data.prop1 = new DynamicDictionary();
//            data.prop1.prop2 = "abc";

//            var ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);

//            ev = new Event();
//            data = ev.Data;
//            data.prop1 = new DynamicDictionary();
//            data.prop1.prop2 = "abc";

//            ev2 = distinct.Apply(ev);
//            Assert.AreEqual(null, ev2);

//            ev = new Event();
//            data = ev.Data;
//            data.prop1 = new DynamicDictionary();
//            data.prop1.prop2 = "def";

//            ev2 = distinct.Apply(ev);
//            Assert.AreNotEqual(null, ev2);
//        }
//    }
//}