using System;
using System.Threading;

namespace NMultiTool.TestConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            var runMode = GetRunModeFromCommandLineArguments(args);
            switch (runMode)
            {
                case RunMode.WriteErrorToStdErrAndReturnOne:
                    return WriteErrorToStdErrAndReturnOne();
                case RunMode.WriteVersionToStdOut:
                    return WriteVersionToStdOut();
                case RunMode.Wait5SecondsAndExit:
                    return Wait5SecondsAndExit();
                case RunMode.AskForPassword:
                    return AskForPassword();
                case RunMode.AskForPasswordInALoop:
                    return AskForPasswordInALoop();
                case RunMode.AskForPasswordInALoopUntilEscape:
                    return AskForPasswordInALoopUntilEscape();
                
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }

        private static int WriteVersionToStdOut()
        {
            System.Console.WriteLine("Version 1.0.0.1");
            return 0;
        }

        private static int WriteErrorToStdErrAndReturnOne()
        {
            System.Console.Error.WriteLine("ERROR: Simulated error 1!");
            System.Console.Error.WriteLine("ERROR: Simulated error 2!");
            return 1;
        }

        private static int AskForPasswordInALoopUntilEscape()
        {
            do
            {
                const string secretPassword = "secret";    
                System.Console.Error.WriteLine("Writing to stderr... (stderr)");
                System.Console.WriteLine("Writing to stdout...(stdout)");
                string password;
                do
                {
                    System.Console.WriteLine("Write '{0}' password: (stdout)", secretPassword);
                    password = System.Console.ReadLine();
                    if (password != secretPassword)
                    {
                        System.Console.Error.WriteLine("Password is not correct (stderr)");
                    }
                } while (password != secretPassword);
                System.Console.Error.WriteLine("Press ESC to terminate or ENTER to continue");
            } while ((System.Console.ReadKey(true).Key != System.ConsoleKey.Escape));
            return 0;
        }

        private static int AskForPasswordInALoop()
        {
            const string secretPassword = "secret";
            string password;
            do
            {
                System.Console.WriteLine("Write '{0}' password: (stdout)", secretPassword);
                password = System.Console.ReadLine();
                if (password != secretPassword)
                {
                    System.Console.Error.WriteLine("Password is not correct (stderr)");
                }
            } while (password != secretPassword);
            return 0;
        }

        private static int AskForPassword()
        {
            const string secretPassword = "secret";
            System.Console.WriteLine("Write '{0}' password: (stdout)", secretPassword);
            System.Console.Write("Password: " );
            var password = System.Console.ReadLine();
            if (password != secretPassword)
            {
                System.Console.WriteLine();
                System.Console.Error.WriteLine("Password is not correct (stderr): " + password);
                return 2;
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Password is correct (stdout)");
            return 0;
        }

        private static int Wait5SecondsAndExit()
        {
            Console.Write("Waiting");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
            Console.WriteLine("Done waiting!");
            return 0;
        }

        static RunMode GetRunModeFromCommandLineArguments(string[] args)
        {
            if (args.Length == 1)
            {
                var runModeString = args[0];
                RunMode runMode;
                var isParsed = Enum.TryParse<RunMode>(runModeString, out runMode);
                if (!isParsed)
                {
                    throw new ArgumentException("");
                }
                return runMode;
            }
            return RunMode.Wait5SecondsAndExit;            
        }
    }

    internal enum RunMode
    {        
        WriteVersionToStdOut,
        WriteErrorToStdErrAndReturnOne,
        Wait5SecondsAndExit,
        AskForPassword,
        AskForPasswordInALoop,
        AskForPasswordInALoopUntilEscape,
    }
}
