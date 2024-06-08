using System.Windows;
using System.Windows.Input;
using ITService.UI.Extensions;
using Microsoft.Xaml.Behaviors;

namespace ITService.UI.Behaviors
{
    class WindowTitleBarBehavior : Behavior<UIElement>
    {
        private Window? _window;
        protected override void OnAttached()
        {
            _window = AssociatedObject as Window ?? AssociatedObject.FindLogicalParent<Window>();
            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
        }

        protected override void OnDetaching() 
        {
            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
            _window = null;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1: 
                    DragMove();
                    break;
                default:
                    Maximize();
                    break;
            }
        }

        private void DragMove()
        {
            if (AssociatedObject.FindVisualRoot() is not Window window) return;
            window?.DragMove();
        }

        private void Maximize()
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
