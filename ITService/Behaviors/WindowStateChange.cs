using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace ITService.Behaviors
{
    class WindowStateChange : Behavior<Button>
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
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;

            switch (window?.WindowState)
            {
                case WindowState.Normal: 
                    window.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;
            }
        }
    }
}
