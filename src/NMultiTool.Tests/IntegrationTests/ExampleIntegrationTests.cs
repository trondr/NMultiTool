﻿using Common.Logging;
using Common.Logging.Simple;
using NUnit.Framework;

namespace NMultiTool.Tests.IntegrationTests
{
    [TestFixture(Category = "IntegrationTests")]
    public class ExampleIntegrationTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ExampleTest1()
        {
            //var target = new ExampleServive(_logger);
            //var expected = "SomeExpectedValue";
            //var actual = target.GetSomeValue()
            //Assert.AreEqual(expected,actual);
        }


    }
}