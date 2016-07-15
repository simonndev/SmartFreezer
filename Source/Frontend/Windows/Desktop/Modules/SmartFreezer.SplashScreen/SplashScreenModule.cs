using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using SmartFreezer.Infrastructure.Events;
using SmartFreezer.Infrastructure.UI;
using SmartFreezer.SplashScreen.Views;
using System;
using System.Threading;
using System.Windows.Threading;

namespace SmartFreezer.SplashScreen
{
    [Module(ModuleName = "SplashScreen")]
    public class SplashScreenModule : IModule
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IEventAggregator _eventAggregator;
        private readonly IShell _shell;

        private AutoResetEvent _shellLoadedEvent;

        public SplashScreenModule(IUnityContainer container, IShell shell, IEventAggregator eventAggregator)
        {
            this._unityContainer = container;
            this._shell = shell;
            this._eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
                (Action)(() =>
                {
                    this._shell.Show();
                    this._eventAggregator
                    .GetEvent<CloseSplashScreenEvent>()
                    .Publish(new CloseSplashScreenEvent());
                }));

            this._shellLoadedEvent = new AutoResetEvent(false);

            ThreadStart showSplashScreenThread =
                () =>
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(
                        (Action)(() =>
                        {
                            this._unityContainer.RegisterType<SplashScreenView, SplashScreenView>();

                            var splashscreen = ServiceLocator.Current.GetInstance<SplashScreenView>();

                            this._eventAggregator
                            .GetEvent<CloseSplashScreenEvent>()
                            .Subscribe(e => splashscreen.Dispatcher.BeginInvoke(
                                (Action)splashscreen.Close),
                                ThreadOption.PublisherThread,
                                true);

                            splashscreen.Show();

                            this._shellLoadedEvent.Set();
                        }));

                    Dispatcher.Run();
                };

            var thread = new Thread(showSplashScreenThread)
            {
                Name = "ShowSplashScreenThread",
                IsBackground = true
            };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            this._shellLoadedEvent.WaitOne();
        }
    }
}
