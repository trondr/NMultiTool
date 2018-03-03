using System;
using System.IO;
using Castle.Core.Internal;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Logging;
using NCmdLiner;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;
using NMultiTool.Library.Module.Commands.Folder2Wxs;
using NMultiTool.Library.Module.Commands.InstallUtil;
using NMultiTool.Library.Module.Commands.SplitIco;
using NMultiTool.Library.Module.ViewModels;
using NMultiTool.Library.Module.Views;
using SingletonAttribute = NMultiTool.Library.Infrastructure.SingletonAttribute;

namespace NMultiTool.Infrastructure
{
    public class ContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());
            container.Register(Component.For<IMessenger>().ImplementedBy<NotepadMessenger>());
            
            //Configure logging
            ILoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(loggingConfiguration.LogDirectoryPath, loggingConfiguration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            var applicationRootNameSpace = typeof (Program).Namespace;
            container.Kernel.Register(Component.For<ILog>().Instance(LogManager.GetLogger(applicationRootNameSpace))); //Default logger
            container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver()); //Enable injection of class specific loggers
            
            //Manual registrations
            container.Register(Component.For<MainWindow>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainView>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainViewModel>().Activator<StrictComponentActivator>());

            //Factory registrations example:

            container.Register(Component.For<IFolder2WxsCommandProviderFactory>().AsFactory());
            container.Register(
                Component.For<IFolder2WxsCommandProvider>()
                    .ImplementedBy<Folder2WxsCommandProvider>()
                    .Named(typeof(Folder2WxsCommandProvider).Name)
                    .LifeStyle.Transient);

            container.Register(Component.For<IInstallUtilCommandProviderFactory>().AsFactory());
            container.Register(
                Component.For<IInstallUtilCommandProvider>()
                    .ImplementedBy<InstallUtilCommandProvider>()
                    .Named(typeof(InstallUtilCommandProvider).Name)
                    .LifeStyle.Transient);

            container.Register(Component.For<ISplitIcoCommandProviderFactory>().AsFactory());
            container.Register(
                Component.For<ISplitIcoCommandProvider>()
                    .ImplementedBy<SplitIcoCommandProvider>()
                    .Named(typeof(SplitIcoCommandProvider).Name)
                    .LifeStyle.Transient);

            container.Register(Component.For<IConvertSvgToIcoCommandProviderFactory>().AsFactory());
            container.Register(
                Component.For<IConvertSvgToIcoCommandProvider>()
                    .ImplementedBy<ConvertSvgToIcoCommandProvider>()
                    .Named(typeof(ConvertSvgToIcoCommandProvider).Name)
                    .LifeStyle.Transient);

            container.Register(Component.For<IConvertAllSvgToIcoCommandProviderFactory>().AsFactory());
            container.Register(
                Component.For<IConvertAllSvgToIcoCommandProvider>()
                    .ImplementedBy<ConvertAllSvgToIcoCommandProvider>()
                    .Named(typeof(ConvertAllSvgToIcoCommandProvider).Name)
                    .LifeStyle.Transient);
            
            //container.Register(
            //    Component.For<ITeamProvider>()
            //        .ImplementedBy<SqlTeamProvider>()
            //        .Named("SqlTeamProvider")
            //        .LifeStyle.Transient);

            container.Register(Component.For<IInvocationLogStringBuilder>().ImplementedBy<InvocationLogStringBuilder>().LifestyleSingleton());
            container.Register(Component.For<ILogFactory>().ImplementedBy<LogFactory>().LifestyleSingleton());
            ///////////////////////////////////////////////////////////////////
            //Automatic registrations
            ///////////////////////////////////////////////////////////////////
            //
            //   Register all interceptors
            //
            container.Register(Classes.FromAssemblyInThisApplication()
                .Pick().If(type => type.Name.EndsWith("Aspect")).LifestyleSingleton());
            //
            //   Register all command providers and attach logging interceptor
            //
            const string libraryRootNameSpace = "NMultiTool.Library";
            container.Register(Classes.FromAssemblyContaining<CommandProvider>()
                .InNamespace(libraryRootNameSpace, true)
                .If(type => type.Is<CommandProvider>())
                .Configure(registration => registration.Interceptors(new[] { typeof(InfoLogAspect) }))
                .WithService.DefaultInterfaces().LifestyleTransient()                
            );
            //
            //   Register all command definitions
            //
            container.Register(
                Classes.FromAssemblyInThisApplication()
                .BasedOn<CommandDefinition>()
                .WithServiceBase()
                );
            //
            //   Register all singletons found in the library
            //
            container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                .InNamespace(libraryRootNameSpace, true)
                .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                .WithService.FirstInterface().LifestyleSingleton());
            //
            //   Register all transients found in the library
            //
            container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                .InNamespace(libraryRootNameSpace, true)
                .WithService.FirstInterface().LifestyleTransient());
            
            IApplicationInfo applicationInfo = new ApplicationInfo();
            container.Register(Component.For<IApplicationInfo>().Instance(applicationInfo).LifestyleSingleton());
        }
    }
}
