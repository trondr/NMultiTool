using System;
using Common.Logging;
using NCmdLiner;
using NCmdLiner.Exceptions;
using NMultiTool.Infrastructure;
using NMultiTool.Library.Infrastructure;

namespace NMultiTool
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            var returnValue = 0;
            try
            {
                var logger = LogManager.GetLogger<Program>();
                var applicationInfo = BootStrapper.Container.Resolve<IApplicationInfo>();
                try
                {
                    applicationInfo.Authors = @"trondr@outlook.com";
                    // ReSharper disable once CoVariantArrayConversion
                    object[] commandTargets = BootStrapper.Container.ResolveAll<CommandDefinition>();
                    logger.InfoFormat("Start: {0}.{1}. Command line: {2}", applicationInfo.Name, applicationInfo.Version, Environment.CommandLine);                    
                    returnValue = CmdLinery.Run(commandTargets, args, applicationInfo, BootStrapper.Container.Resolve<IMessenger>());
                    return returnValue;
                }
                catch (MissingCommandException ex)
                {
                    logger.ErrorFormat("Missing command. {0}", ex.Message);
                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error when exeuting command. {0}", ex.ToString());
                    returnValue = 2;
                }
                finally
                {
                    logger.InfoFormat("Stop: {0}.{1}. Return value: {2}", applicationInfo.Name, applicationInfo.Version, returnValue);
#if DEBUG
                    System.Console.WriteLine("Press any key...");
                    Console.ReadLine();
#endif
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Fatal error when wiring up the application.{0}{1}", Environment.NewLine, ex);
                returnValue = 3;
            }
            return returnValue;
        }
    }
}
