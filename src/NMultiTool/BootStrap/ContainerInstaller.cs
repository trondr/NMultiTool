using System;
using System.IO;
using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Logging;
using NCmdLiner;
using NMultiTool.Library.BootStrap;
using NMultiTool.Library.Common;
using NMultiTool.Wireup;

namespace NMultiTool.BootStrap
{
    public class ContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());

            //Configure logging
            ILoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(loggingConfiguration.LogDirectoryPath, loggingConfiguration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            var applicationRootNameSpace = typeof (Program).Namespace;

            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().ConfiguredExternally());
            container.Kernel.Register(Component.For<ILog>().Instance(LogManager.GetLogger(applicationRootNameSpace))); //Default logger
            container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver()); //Enable injection of class specific loggers

            //Manual registrations

            //Factory registrations example:

            //container.Register(Component.For<ITeamProviderFactory>().AsFactory());
            //container.Register(
            //    Component.For<ITeamProvider>()
            //        .ImplementedBy<CsvTeamProvider>()
            //        .Named("CsvTeamProvider")
            //        .LifeStyle.Transient);
            //container.Register(
            //    Component.For<ITeamProvider>()
            //        .ImplementedBy<SqlTeamProvider>()
            //        .Named("SqlTeamProvider")
            //        .LifeStyle.Transient);

            //Automatic registrations

            container.Register(Classes.FromAssemblyInThisApplication().BasedOn<CommandsBase>().WithServiceBase());

            var libraryRootNameSpace = applicationRootNameSpace + ".Library";

            container.Register(Classes.FromAssemblyInThisApplication()
                .InNamespace(libraryRootNameSpace, true)
                .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                .WithService.DefaultInterfaces().LifestyleSingleton());

            container.Register(Classes.FromAssemblyInThisApplication()
                .InNamespace(libraryRootNameSpace, true)
                .WithService.DefaultInterfaces().LifestyleTransient());

            IApplicationInfo applicationInfo = new ApplicationInfo();
            container.Register(Component.For<IApplicationInfo>().Instance(applicationInfo).LifestyleSingleton());
        }
    }
}
