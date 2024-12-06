namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// Class that has various common methods useful for PullToRefresh control.
    /// </summary>
    internal static class PullToRefreshHelpers
    {
        #region Fields

        static bool ShouldHandle = false;
        static IView? PullToRefreshElement;

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
            isAction = ShouldHandle;
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
            return PullToRefreshElement;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method that iterates the children inside the pullable content and returns whether the child is of type IPullToRefresh and whether the child should handle the gesture or not.
        /// </summary>
        static bool CheckChildrenIsIPullToRefresh(IView view, object pullToRefresh)
        {
            if (view is null)
            {
                return false;
            }

            if (view is IPullToRefresh pulltoRefreshChild)
            {
                return SetPullToRefreshElement(pulltoRefreshChild, pullToRefresh);
            }

            ShouldHandle = false;

            if (view is Layout layout)
            {
                return CheckLayoutChildren(layout, pullToRefresh);
            }
#pragma warning disable CS0618

            // TODO:We cannot remove this warning because, still framework didn't changed the ScrollView and ContentView from Compatibility.Layout. We can remove this only when framework changes this.
            else if (view is Microsoft.Maui.Controls.Compatibility.Layout compatibilityLayout)
            {
                return CheckCompatibilityLayoutChildren(compatibilityLayout, pullToRefresh);
            }
#pragma warning disable CS0618
            return false;
        }

        static bool SetPullToRefreshElement(IPullToRefresh pulltoRefreshChild, object pullToRefresh)
        {
            PullToRefreshElement = pulltoRefreshChild as IView;
            ShouldHandle = pulltoRefreshChild.CanHandleGesture(pullToRefresh);
            return ShouldHandle;
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
#pragma warning disable CS0618
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
#pragma warning disable CS0618
        #endregion
    }
}
