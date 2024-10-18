using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// Class that has various common methods useful for PullToRefresh control.
    /// </summary>
    internal static class PullToRefreshHelpers
    {
        #region Fields

        static bool _shouldHandle = false;
        static IView? _pullToRefreshElement;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method that iterates the children inside the pullable content and returns whether the child is of type IPullToRefresh and whether the child should handle the gesture or not.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="pullToRefreshView">The pullToRefreshView.</param>
        /// <param name="isPullToRefresh">Gets the <see cref="bool"/> value to PullToRefresh.</param>
        /// <param name="isAction">Gets the <see cref="bool"/> value to handle or not.</param>
        internal static void CheckChildren(IView view, object pullToRefreshView, out bool isPullToRefresh, out bool isAction)
        {
            isPullToRefresh = CheckChildrenIsIPullToRefresh(view, pullToRefreshView);
            isAction = _shouldHandle;
        }

        /// <summary>
        /// Method that iterates the children inside the pullable content and returns whether the Element is of type IPullToRefresh.
        /// </summary>
        /// <param name="view">The PullToRefreshElement.</param>
        /// <param name="pullToRefreshView">The pullToRefreshView.</param>
        /// <returns>Returns whether is of type IPullToRefresh.</returns>
        internal static IView? GetIPullToRefreshElement(IView view, object pullToRefreshView)
        {
            CheckChildrenIsIPullToRefresh(view, pullToRefreshView);
            return _pullToRefreshElement;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method that iterates the children inside the pullable content and returns whether the child is of type IPullToRefresh and whether the child should handle the gesture or not.
        /// </summary>
        static bool CheckChildrenIsIPullToRefresh(IView view, object pullToRefresh)
        {
            if (view == null)
            {
                return false;
            }

            if (view is IPullToRefresh pulltoRefreshChild)
            {
                return SetPullToRefreshElement(pulltoRefreshChild, pullToRefresh);
            }

            _shouldHandle = false;

            if (view is Layout layout)
            {
                return CheckLayoutChildren(layout, pullToRefresh);
            }
            else if (view is Microsoft.Maui.Controls.Compatibility.Layout compatibilityLayout)
            {
                return CheckCompatibilityLayoutChildren(compatibilityLayout, pullToRefresh);
            }

            return false;
        }

        static bool SetPullToRefreshElement(IPullToRefresh pulltoRefreshChild, object pullToRefresh)
        {
            _pullToRefreshElement = pulltoRefreshChild as IView;
            _shouldHandle = pulltoRefreshChild.CanHandleGesture(pullToRefresh);
            return _shouldHandle;
        }

        static bool CheckLayoutChildren(Layout layout, object pullToRefresh)
        {
            foreach (var child in layout.Children)
            {
                if (child is IView view && CheckChildrenIsIPullToRefresh(view, pullToRefresh))
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckCompatibilityLayoutChildren(Microsoft.Maui.Controls.Compatibility.Layout compatibilityLayout, object pullToRefresh)
        {
            foreach (var child in compatibilityLayout.Children)
            {
                if (child is IView view && CheckChildrenIsIPullToRefresh(view, pullToRefresh))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
