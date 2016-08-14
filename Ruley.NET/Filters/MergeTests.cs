//using System.Runtime.InteropServices.WindowsRuntime;
//using NUnit.Framework;

//namespace Ruley.Core.Filters
//{
//    [TestFixture]
//    public class MergeTests
//    {
//        [Test]
//        public void Empty_Merge()
//        {
//            var merge = new MergeFilter {Value = new DynamicDictionary()};
//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            merge.Apply(ev);
            
//            Assert.IsTrue(ev.Data.HasProperty("prop1"));
//            Assert.AreEqual(1, ev.Data.Count);
//            Assert.AreEqual("abc", ev.Data["prop1"]);
//        }

//        [Test]
//        public void String_Merge()
//        {
//            var merge = new MergeFilter { Value = new DynamicDictionary() };
//            merge.Value["prop2"] = 123;
//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            merge.Apply(ev);
            
//            Assert.IsTrue(ev.Data.HasProperty("prop1"));
//            Assert.IsTrue(ev.Data.HasProperty("prop2"));
//            Assert.AreEqual(2, ev.Data.Count);
//            Assert.AreEqual("abc", ev.Data["prop1"]); 
//            Assert.AreEqual(123, ev.Data["prop2"]);
//        }

//        [Test]
//        public void Null_Value_Merge()
//        {
//            var merge = new MergeFilter { Value = new DynamicDictionary() };
//            merge.Value["prop1"] = null;
//            var ev = new Event();
//            merge.Apply(ev);

//            Assert.IsTrue(ev.Data.HasProperty("prop1"));
//            Assert.AreEqual(null, ev.Data["prop1"]);
//        }

//        [Test]
//        public void Clashing_Merge()
//        {
//            var merge = new MergeFilter { Value = new DynamicDictionary() };
//            merge.Value["prop1"] = 123;
//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            merge.Apply(ev);

//            Assert.IsTrue(ev.Data.HasProperty("prop1"));
//            Assert.AreEqual(1, ev.Data.Count);
//            Assert.AreEqual(123, ev.Data["prop1"]);
//        }

//        [Test]
//        public void Simple_Templated_Merge()
//        {
//            var merge = new MergeFilter { Value = new DynamicDictionary() };
//            merge.Value["prop2"] = "temp({prop1}-123)";
//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            merge.Apply(ev);

//            Assert.IsTrue(ev.Data.HasProperty("prop2"));
//            Assert.AreEqual("abc-123", ev.Data["prop2"]);
//        }

//        //todo should apply templating recursively
//        [Test]
//        public void Nested_Source_Merge()
//        {
//            var merge = new MergeFilter();
            
//            dynamic mergeData = new DynamicDictionary();
//            mergeData.prop2 = new DynamicDictionary();
//            mergeData.prop2.prop3 = 101;
//            merge.Value = mergeData;

//            var ev = new Event();
//            merge.Apply(ev);

//            dynamic data = ev.Data;

//            Assert.IsTrue(data.prop2 != null);
//            Assert.AreEqual(101, data.prop2.prop3);
//        }

//        [Test]
//        public void Nested_Dest_Merge()
//        {
//            var merge = new MergeFilter();

//            dynamic mergeData = new DynamicDictionary();
//            mergeData.prop2 = new DynamicDictionary();
//            mergeData.prop2.prop3 = "field(p1.p2)";
//            merge.Value = mergeData;

//            var ev = new Event();
//            dynamic d = ev.Data;
//            d.p1 = new DynamicDictionary();
//            d.p1.p2 = "test";
//            merge.Apply(ev);

//            dynamic data = ev.Data;

//            Assert.IsTrue(data.prop2 != null);
//            Assert.AreEqual("test", data.prop2.prop3);
//        }

//        [Test]
//        public void Nested_Clashing_Merge()
//        {
//            var merge = new MergeFilter();

//            dynamic mergeData = new DynamicDictionary();
//            mergeData.p1 = new DynamicDictionary();
//            mergeData.p1.p3 = "abc";
//            merge.Value = mergeData;

//            var ev = new Event();
//            dynamic d = ev.Data;
//            d.p1 = new DynamicDictionary();
//            d.p1.p2 = "def";
//            merge.Apply(ev);

//            dynamic data = ev.Data;

//            Assert.AreEqual("def", data.p1.p2);
//            Assert.AreEqual("abc", data.p1.p3);
//        }

//        //arrays?

//        //public DynamicDictionary Parse(DynamicDictionary sourceData, DynamicDictionary newData)
//        //{
//        //    foreach (var VARIABLE in newData)
//        //    {
                
//        //    }
//        //}

//        [Test]
//        public void Nested_Templated_Merge()
//        {
//            var merge = new MergeFilter();

//            dynamic mergeData = new DynamicDictionary();
//            mergeData.prop2 = new DynamicDictionary();
//            mergeData.prop2.prop3 = "{prop1}-123";
//            merge.Value = mergeData;

//            var ev = new Event();
//            ev.Data["prop1"] = "abc";
//            merge.Apply(ev);

//            dynamic data = ev.Data;

//            Assert.AreEqual("abc-123", data.prop2.prop3);
//        }
//    }
//}