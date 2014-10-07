using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MVVM.UI.Extensions
{
    public static class UITreeHelper
    {
        #region Public static methods

        public static T GetRootVisualParent<T>(this DependencyObject node)
            where T : DependencyObject
        {
            var parent = GetVisualParent<T>(node);
            if (parent != null)
            {
                return GetRootVisualParentBase(parent);
            }
            return null;
        }

        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            var toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                var children = toDo.Dequeue();
                foreach (var child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        public static IEnumerable<T> GetVisualDescendents<T>(this FrameworkElement root, bool allAtSameLevel = false)
            where T : FrameworkElement
        {
            var found = false;
            foreach (var e in root.GetVisualDescendents())
            {
                if (e is T)
                {
                    found = true;
                    yield return e as T;
                }
                else
                {
                    if (found && allAtSameLevel)
                    {
                        yield break;
                    }
                }
            }
        }

        public static T GetVisualParent<T>(this DependencyObject node)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(node);
            var parentT = VisualTreeHelper.GetParent(node) as T;
            if (parent == null)
            {
                return null;
            }
            if (parentT != null)
            {
                return parentT;
            }
            return GetVisualParent<T>(parent);
        }

        public static T GetVisualParent<T>(this FrameworkElement node, string name)
            where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(node);
            var parentT = VisualTreeHelper.GetParent(node) as T;
            if (parent == null)
            {
                return null;
            }
            if (parentT != null && parentT.Name == name)
            {
                return parentT;
            }
            return GetVisualParent<T>(parent as FrameworkElement, name);
        }

        #endregion
        #region Private methods

        private static T GetRootVisualParentBase<T>(this T node)
            where T : DependencyObject
        {
            var parent = GetVisualParent<T>(node);
            if (parent != null)
            {
                return GetRootVisualParentBase(parent);
            }
            return node;
        }

        #endregion
    }
}
