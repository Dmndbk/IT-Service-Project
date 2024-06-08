using System.Windows;
using System.Windows.Media;

namespace ITService.UI.Extensions
{
    static class DependencyObjectExtensions
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

        public static T? FindVisualParent<T>(this DependencyObject? obj) where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;

            do
            {
                target = VisualTreeHelper.GetParent(target);
            } 
            while (target != null && target is not T);
            
            return target as T;
        }

        public static T? FindLogicalParent<T>(this DependencyObject? obj) where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;

            do
            {
                target = LogicalTreeHelper.GetParent(target);   
            } 
            while (target != null && target is not T);
            
            return target as T;
        }
    }


}
