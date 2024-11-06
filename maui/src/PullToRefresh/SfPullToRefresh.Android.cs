using System;
using System.Linq;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using ListView = Android.Widget.ListView;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using ScrollView = Android.Widget.ScrollView;
using View = Android.Views.View;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// <see cref="SfPullToRefresh"/> enables interaction to refresh the loaded view. This control allows users to trigger a refresh action by performing the pull-to-refresh gesture.
    /// </summary>
    public partial class SfPullToRefresh
    {
        #region Fields

        LayoutViewGroupExt? _nativeView;
        float _density = 0;
        bool _isChildScrolledVertically = false;

        #endregion

        #region Private Methods

        Point GetCurrentTouchPoint(MotionEvent ev, int actionIndex)
        {
            Point screenPoint = new Point(ev.GetX(actionIndex), ev.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            return new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
        }

        bool HandleActionDown(Point currenTouchPoint)
        {
            HandleTouchInteraction(PointerActions.Pressed, currenTouchPoint);
            if (_nativeView != null && _nativeView.Parent != null && _nativeView.Parent.GetType().Name == "ScrollViewContainer")
            {
                // PullToRefresh is not working inside scrollView, this code passes touch to the parent without interfering any of the parent
                _nativeView.Parent.RequestDisallowInterceptTouchEvent(true);
            }

            return false;
        }

        bool HandleActionMove(MotionEvent ev, Point currenTouchPoint)
        {
            // Pulling is not happen when minimum amount of downY, break has been fixed by minus with the touch range of RawY.
            if (Math.Abs(currenTouchPoint.X - _downX) >= Math.Abs(currenTouchPoint.Y - _downY)
                || (Math.Abs(_downY - currenTouchPoint.Y) < 10 && Math.Abs((_downY * _density) - ev.RawY) < 10))
            {
                return false;
            }

            int sdk = (int)global::Android.OS.Build.VERSION.SdkInt;
            if (sdk >= 16 && _nativeView != null && _nativeView.IsScrollContainer)
            {
                return base.OnInterceptTouchEvent(ev);
            }

            if (PullableContent != null)
            {
                _childLoopCount = 0;
                _isChildScrolledVertically = IsChildElementScrolled(PullableContent.GetVisualTreeDescendants().FirstOrDefault(), new Point(ev.RawX / _density, ev.RawY / _density));
            }

            if (_downY < currenTouchPoint.Y && !_isChildScrolledVertically)
            {
                return true;
            }
            else
            {
                return base.OnInterceptTouchEvent(ev);
            }
        }

        /// <summary>
        /// Gets the X and Y coordinates of the specified child based on the screen.
        /// </summary>
        /// <param name="native">The current child for which coordinates are requested.</param>
        Point ChildLocationToScreen(object native)
        {
            var point = new int[2];
            if (native is View view)
            {
                view.GetLocationInWindow(point);
                return new Point(point[0] / _density, point[1] / _density);
            }
            return Point.Zero;
        }

        /// <summary>
        /// Gets the scroll offset of the specified child within a scroll view.
        /// </summary>
        /// <param name="view">The current child for which the scroll offset is requested.</param>
        /// <returns>Returns the current scroll offset of the specifies child.</returns>
        double GetChildScrollOffset(object view)
        {
            var childPanel = view as ViewGroup;
            if (childPanel != null)
            {
                if (view is SwipeRefreshLayout swipeRefreshLayout)
                {
                    return swipeRefreshLayout.ScrollY;
                }
                else if (view is ListView list && list.ChildCount > 0)
                {
                    View? firstChild = list.GetChildAt(0);
                    if (firstChild != null)
                    {
                        return (list.FirstVisiblePosition * firstChild.Height) - firstChild.Top;
                    }
                }
                else if (view is Android.Webkit.WebView || view is ScrollView || view is NestedScrollView)
                {
                    return (view is View childView) ? childView.ScrollY : 0;
                }
                else if (view is RecyclerView nativeCollectionView)
                {
                    return nativeCollectionView.ComputeVerticalScrollOffset();
                }
            }

            return 0;
        }

        /// <summary>
        /// Method configures all the touch related works.
        /// </summary>
        void ConfigTouch()
        {
            if (Handler != null && Handler.PlatformView != null)
            {
                this.AddTouchListener(this);
                _nativeView = Handler.PlatformView as LayoutViewGroupExt;

                if (_nativeView != null && _nativeView.Resources != null && _nativeView.Resources.DisplayMetrics != null)
                {
                    _density = _nativeView.Resources.DisplayMetrics.Density;
                }
            }
            else
            {
                _nativeView = null;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// This method will helps to intercept touch of <see cref="LayoutViewGroupExt"/>.
        /// </summary>
        /// <param name="ev">MotionEvent arguments.</param>
        /// <returns>Returns true, if <see cref="LayoutViewGroupExt"/> will handle the touch.</returns>
        internal override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (_nativeView != null && ev != null)
            {
                int actionIndex = ev.ActionIndex;
                Point currenTouchPoint = GetCurrentTouchPoint(ev, actionIndex);
                switch (ev.Action)
                {
                    case MotionEventActions.Down:
                        if (!ActualIsRefreshing)
                        {
                            return HandleActionDown(currenTouchPoint);
                        }

                        break;
                    case MotionEventActions.Move:
                        return HandleActionMove(ev, currenTouchPoint);
                    case MotionEventActions.Up:
                    case MotionEventActions.Pointer1Up:
                        HandleTouchInteraction(PointerActions.Released, new Point(0, 0));
                        return false;
                    case MotionEventActions.Cancel:
                        HandleTouchInteraction(PointerActions.Cancelled, new Point(0, 0));
                        return false;
                    default:
                        return base.OnInterceptTouchEvent(ev);
                }
            }

            return base.OnInterceptTouchEvent(ev);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method triggers on any touch interaction on <see cref="SfPullToRefresh"/>.
        /// </summary>
        /// <param name="e">Event args.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            if (e.Action != PointerActions.Pressed)
            {
                HandleTouchInteraction(e.Action, e.TouchPoint);
            }

            if (e.Action == PointerActions.Released || e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
            {
                _isChildScrolledVertically = false;
            }
        }

        #endregion
    }
}
