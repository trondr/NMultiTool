using System;
using Castle.DynamicProxy.Contributors;
using NMultiTool.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;
using NUnit.Framework;

namespace NMultiTool.Tests.Module.Common.Process
{
    [TestFixture]
    public class ProcessTests
    {
        [Test]
        public void ExecuteStdOutTest()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "WriteVersionToStdOut";
                target.Execute(exeFile, arguments, true, null);
                var expected = "Version 1.0.0.1" + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ExecuteWait5SecondsAndExitTest()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "Wait5SecondsAndExit";
                target.Execute(exeFile, arguments, true, null);
                var expected = "Waiting....."+ Environment.NewLine + "Done waiting!" + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual,"StandardOutput not expected.");
            }
        }
        
        [Test]
        public void ExecuteStdErrTest()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "WriteErrorToStdErrAndReturnOne";
                target.Execute(exeFile, arguments, true, null);
                var expected = "ERROR: Simulated error 1!" + Environment.NewLine + "ERROR: Simulated error 2!" + Environment.NewLine;
                var actual = target.StandardError;
                Assert.AreEqual(expected, actual, "StandardError not expected.");
                int actualExitCode = target.ExitCode;
                int expectedExitCode = 1;
                Assert.AreEqual(expectedExitCode, actualExitCode, "Exit code not expected.");
            }
        }

        [Test]
        public void ExecuteStdInputAskForPassowordGiveCorrectPassword()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "AskForPassword";
                target.WriteToStandardInput("secret");
                target.Execute(exeFile, arguments, true, null);
                var expected = "Write 'secret' password: (stdout)" + Environment.NewLine + "Password: " + Environment.NewLine + "Password is correct (stdout)" + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual, "StandardOutput not expected while asking for password.");
                int actualExitCode = target.ExitCode;
                int expectedExitCode = 0;
                Assert.AreEqual(expectedExitCode, actualExitCode, "Exit code not expected.");
            }
        }

        [Test]
        public void ExecuteStdInputAskForPassowordGiveInCorrectPassword()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "AskForPassword";
                target.WriteToStandardInput("wrongsecret");
                target.Execute(exeFile, arguments, true, null);
                var expected = "Write 'secret' password: (stdout)" + Environment.NewLine + "Password: " + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual, "StandardOutput not expected while asking for password.");
                expected = "Password is not correct (stderr): wrongsecret" + Environment.NewLine;
                actual = target.StandardError;
                Assert.AreEqual(expected, actual, "StandardError not expected while asking for password.");
                int actualExitCode = target.ExitCode;
                int expectedExitCode = 2;
                Assert.AreEqual(expectedExitCode, actualExitCode, "Exit code not expected.");
            }
        }


        [Test]
        public void ExecuteStartTwoProcessesWithSameInstanceWithoutReset_ThrowException()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "WriteVersionToStdOut";
                target.Execute(exeFile, arguments, true, null);
                Assert.Throws<NMultiToolException>(() =>
                {
                    target.Execute(exeFile, arguments, true, null);
                });
                var expected = "Version 1.0.0.1" + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual);                
            }
        }

        [Test]
        public void ExecuteStartTwoProcessesWithSameInstanceWithReset()
        {
            using (var testConsoleExe = new TestConsoleExe())
            {
                var exeFile = testConsoleExe.TestConsoleExeFile;
                var target = new Library.Module.Common.Process.Process();
                var arguments = "WriteVersionToStdOut";
                target.Execute(exeFile, arguments, true, null);
                var expected = "Version 1.0.0.1" + Environment.NewLine;
                var actual = target.StandardOutput;
                Assert.AreEqual(expected, actual);                
                target.Reset();
                target.Execute(exeFile, arguments, true, null);
                expected = "Version 1.0.0.1" + Environment.NewLine;
                actual = target.StandardOutput;
                Assert.AreEqual(expected, actual);                
            }
        }
    }
}