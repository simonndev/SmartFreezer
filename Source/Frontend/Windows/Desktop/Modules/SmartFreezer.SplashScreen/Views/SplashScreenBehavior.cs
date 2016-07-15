using System.Windows;

namespace SmartFreezer.SplashScreen.Views
{
    public class SplashScreenBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(SplashScreenBehavior), new PropertyMetadata(OnEnabledChanged));

        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var splash = d as Window;
            if (splash != null && e.NewValue is bool && (bool)e.NewValue)
            {
                splash.Closed += delegate
                {
                    splash.DataContext = null;
                    splash.Dispatcher.InvokeShutdown();
                };

                splash.MouseDoubleClick += delegate { splash.Close(); };
                splash.MouseLeftButtonDown += delegate { splash.DragMove(); };
            }
        }
    }
}
