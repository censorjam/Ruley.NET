using System;
using NUnit.Framework;
using System.Reflection;
using System.IO;
using Moq;
using Ruley.NET;
using Ruley.NET.External;
using System.Collections.Generic;

namespace Ruley.Tests
{
    public class PipelineTest
    {
        public Pipeline Pipeline { get; private set; }
        public List<Event> Output { get; set; }

        private string LoadFromResource(string yamlFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Ruley.Tests.IntegrationTests." + yamlFileName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public PipelineTest(string yamlFile)
        {
            Output = new List<Event>();
            var yaml = LoadFromResource(yamlFile);
            Pipeline = YamlParser.Load(yaml)[0];
            Pipeline.Subscribe(s => { Output.Add(s); });
            Pipeline.Start();
        }

        public void Next(Event e)
        {
            Pipeline.OnNext(e);
        }
    }

    [TestFixture]
    public class YamlParsingTests
    {
        private Mock<IConsoleOutput> _consoleMock;

        [SetUp]
        public void Setup()
        {
            _consoleMock = new Mock<IConsoleOutput>();
            IoC.Register<IConsoleOutput>(_consoleMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            IoC.Reset();
        }

        private string LoadFromResource(string yamlFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Ruley.Tests.IntegrationTests." + yamlFileName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        [Test]
        public void Test1()
        {
            var yaml = LoadFromResource("Map1.yaml");
            var x = YamlParser.Load(yaml);

            Event last = null;
            x[0].Subscribe(s => { last = s; });
            x[0].Start();

            dynamic next = new Event(new DynamicDictionary());
            next.a = "abc";
            x[0].OnNext(next);

            Assert.AreEqual("XYZ", last["newfield"]);
        }

        [Test]
        public void Test2()
        {
            var test = new PipelineTest("Test1.yaml");

            dynamic next = new Event();
            next.inputField = "abc";

            test.Next(next);

            //_consoleMock.Verify(ms => ms.WriteLine(
            //    It.Is<string>(mo => mo == "hello world")
            //), Times.Once());

            Assert.AreEqual(1, test.Output.Count);
            Assert.IsTrue(test.Output[0].ContainsKey("count"));
        }
    }
}

