using Common.Logging;
using Common.Logging.Simple;
using NMultiTool.Library.Commands.Folder2Wxs;
using NUnit.Framework;
using Rhino.Mocks;

namespace NMultiTool.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class IdFromNameGeneratorTests
    {
        private ILog _logger;
        private IRandomNumberGenerator _stubRandomNumberGenerator;
        private int _randomNumberExpected;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger("IdFromNameGeneratorTests", LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _stubRandomNumberGenerator = MockRepository.GenerateStub<IRandomNumberGenerator>();
            _randomNumberExpected = 1234567890;
            _stubRandomNumberGenerator.Stub(generator => generator.GetRandomNumber()).Return(_randomNumberExpected);
        }
        
        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void IdFromNameGeneratorTestNameIsShortReturnIdShorterThan72()
        {
            const string name = "Microsoft.Practices.dll";
            const string postfix = "WiXComponent";
            const string expected = "Id_Microsoft_Practices_dll_1234567890_WiXComponent";
            var target = new IdFromNameGenerator(_stubRandomNumberGenerator,_logger);
            var actual = target.GetId(name, postfix);
            Assert.AreEqual(expected,actual);
            Assert.IsTrue(actual.Length <= 72, "Id length is longer than 72");
            Assert.IsTrue(actual.Length < 72, "Id length is not shorter than 72");
        }

        [Test]
        public void IdFromNameGeneratorTestNameIsLongReturnIdShorterThan72()
        {
            const string name = "Castle.Services.Logging.Log4netIntegration.dll";
            const string postfix = "WiXComponent";
            const string expected = "Id_Castle_Services_Logging_Log4netIntegrati_1234567890_WiXComponent";
            var target = new IdFromNameGenerator(_stubRandomNumberGenerator, _logger);
            var actual = target.GetId(name, postfix);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual.Length <= 72,"Id length is longer than 72: " + actual + " " + actual.Length);
        }

        [Test]
        public void IdFromNameGeneratorTestNameIsContainingParantecesThatShouldBeReplacedByNothing()
        {
            const string name = "Castle.Services.(null).Logging.Log4netIntegration.dll";
            const string postfix = "WiXComponent";
            const string expected = "Id_Castle_Services_null_Logging_Log4netInte_1234567890_WiXComponent";
            var target = new IdFromNameGenerator(_stubRandomNumberGenerator, _logger);
            var actual = target.GetId(name, postfix);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual.Length <= 72, "Id length is longer than 72: " + actual + " " + actual.Length);
        }

        [Test]
        public void IdFromNameGeneratorTestNameIsContainingPlusesThatShouldBeReplacedByUnderscore()
        {
            const string name = "portable-net40+sl5+wp80+win8+wpa81.dll";
            const string postfix = "WiXComponent";
            const string expected = "Id_portable_net40_sl5_wp80_win8_wpa81_dll_1234567890_WiXComponent";
            var target = new IdFromNameGenerator(_stubRandomNumberGenerator, _logger);
            var actual = target.GetId(name, postfix);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual.Length <= 72, "Id length is longer than 72: " + actual + " " + actual.Length);
        }

        [Test]
        public void IdFromNameGeneratorTestNameIsContainingDashThatShouldBeReplacedByUnderscore()
        {
            const string name = "Downloads · loresoft-msbuildtasks.dll";
            const string postfix = "WiXComponent";
            const string expected = "Id_Downloads_loresoft_msbuildtasks_dll_1234567890_WiXComponent";
            var target = new IdFromNameGenerator(_stubRandomNumberGenerator, _logger);
            var actual = target.GetId(name, postfix);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual.Length <= 72, "Id length is longer than 72: " + actual + " " + actual.Length);
        }        
    }
}
