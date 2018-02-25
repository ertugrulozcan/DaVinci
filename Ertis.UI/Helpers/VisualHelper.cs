using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ertis.Shared.Helpers
{
    public static class VisualHelper
    {
        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            var child = default(T);
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static T GetVisualFramworkElement<T>(DependencyObject parent) where T : FrameworkElement
        {
            var child = default(T);
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < numVisuals; i++)
            {
                var v = (FrameworkElement)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualFramworkElement<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }


        public static IEnumerable<T> GetVisualChildren<T>(Visual parent) where T : Visual
        {
            var frameworkElement = parent as FrameworkElement;
            if (frameworkElement != null)
                frameworkElement.ApplyTemplate();

            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);

                var child = v as T;
                if (child == null)
                {
                    foreach (var c in GetVisualChildren<T>(v))
                    {
                        yield return c;
                    }
                }
                else
                {
                    yield return child;
                }
            }
        }

        public static T FindVisualChildNamedForTextBox<T>(DependencyObject obj, string controlName) where T : DependencyObject
        {
            try
            {
                for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);
                    var block = child as T;
                    if (block != null && ((TextBlock)child).Name == controlName)
                    {
                        return block;
                    }
                    var childOfChild = FindVisualChildNamedForTextBox<T>(child, controlName);
                    if (childOfChild == null) continue;
                    return childOfChild;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
                return null;
            if (element.GetType() == type)
                return element;

            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);

                if (foundElement != null)
                    break;
            }

            return foundElement;
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                    return (childItem)child;

                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                    return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null)
                    return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child’s name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child’s name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
