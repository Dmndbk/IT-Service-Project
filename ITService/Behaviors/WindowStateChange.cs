using System.Windows;
using System.Windows.Controls;
using ITService.UI.Extensions;
using Microsoft.Xaml.Behaviors;

namespace ITService.UI.Behaviors
{
    internal class WindowStateChange : Behavior<Button>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnButtonClick;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.FindVisualRoot() is not Window window) return;

            window.WindowState = window.WindowState switch
            {
                WindowState.Normal => WindowState.Maximized,
                WindowState.Maximized => WindowState.Normal,
                _ => window.WindowState
            };
        }
    }
}
