using System.Windows;
using System.Windows.Media;

namespace System.Windows
{
    static class DependencyObjectExtentions
    {
        public static DependencyObject FindVisualRoot(this DependencyObject element)
        {
            do
            {
                var parent = VisualTreeHelper.GetParent(element);
                if (parent is null) return element;
                element = parent;
            }
            while (true);
        }

        public static DependencyObject FindLogicalRoot(this DependencyObject obj)
        {
            do
            {
                var parent = LogicalTreeHelper.GetParent(obj);
                if (parent is null) return obj;
                obj = parent;
            }
            while (true);
        }
    }
}
