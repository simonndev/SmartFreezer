using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using SmartFreezer.Infrastructure.UI;
using System.Windows;

namespace SmartFreezer
{
    public class Bootstrapper : UnityBootstrapper
    {
        public bool IsAuthenticated { get; set; }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            #region ** OLD CODE - if you want to use ModuleCatalog.xaml file **
            //string catelogUrl = "/SmartFreezer;component/ModuleCatalog.xaml";

            //return Prism.Modularity.ModuleCatalog.CreateFromXaml(new Uri(catelogUrl, UriKind.Relative));
            #endregion

            // Loads modules from App.config file.
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType(typeof(IShell), typeof(Shell), "Shell", new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            var shell = this.Container.TryResolve<IShell>();

            if (this.IsAuthenticated)
            {

            }

            return shell as DependencyObject;
        }
    }
}
