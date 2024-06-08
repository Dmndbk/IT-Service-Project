using System.Windows;
using System.Windows.Controls;
using ITService.UI.Extensions;
using Microsoft.Xaml.Behaviors;

namespace ITService.UI.Behaviors
{
    class CloseWindow : Behavior<Button>
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
            (AssociatedObject.FindVisualRoot() as Window)?.Close();
        }
    }
}
