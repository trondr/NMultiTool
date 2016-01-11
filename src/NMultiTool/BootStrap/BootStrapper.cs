using Castle.Windsor;
using Castle.Windsor.Installer;

namespace NMultiTool.Wireup
{
    public static class BootStrapper
    {
        public static object Synch = new object();
        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    lock (Synch)
                    {
                        if (_container == null)
                        {
                             _container = new WindsorContainer();
                            _container.Install(FromAssembly.InThisApplication());
                        }
                    }
                }
                return _container;                
            }
        }
        private static IWindsorContainer _container;
    }
}
