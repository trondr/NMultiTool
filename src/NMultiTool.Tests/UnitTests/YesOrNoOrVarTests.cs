using NMultiTool.Library.Module.Commands.ConvertSvgToIco;
using NMultiTool.Library.Module.Commands.Folder2Wxs;
using NUnit.Framework;

namespace NMultiTool.Tests.UnitTests
{
    public class YesOrNoOrVarTests
    {

        [Test]
        [TestCase("yes",true)]
        [TestCase("no", true)]
        [TestCase("var.Win64", true)]
        [TestCase("var.Win65", false)]
        [TestCase("varWin64", false)]
        public void YesOrNoOrVarTest(string value, bool expectedSuccess)
        {
            var actual = YesOrNoOrVar.Create(value);
            Assert.AreEqual(expectedSuccess,actual.IsSuccess);
        }


        [Test]
        [TestCase("varWin64", false)]
        public void YesOrNoOrVarTest_Throw_Exception(string value, bool expectedSuccess)
        {
            Assert.Throws<NMultiToolException>(() =>
            {
                YesOrNoOrVar actual = value;
            });
        }

        [Test]
        [TestCase("var.Win64", false)]
        public void YesOrNoOrVarTest_Var(string value, bool expectedSuccess)
        {
            YesOrNoOrVar actual = value;
            Assert.AreEqual($"$({value})",actual.Value);
        }
    }
}
