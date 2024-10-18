using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// <see cref="SfPullToRefresh"/> enables interaction to refresh the loaded view. This control allows users to trigger a refresh action by performing the pull-to-refresh gesture.
    /// </summary>
    public partial class SfPullToRefresh
    {
        #region Fields

        PointerEventHandler? _pointerPressedEventHandler;
        LayoutPanelExt? _nativeView;
        FrameworkElement? _pullableContentNativeView;
        bool _isChildScrolledVertically = false;
        int _maxLoopCount = 0;

        #endregion

        #region Private Methods

        /// <summary>
        /// Method configures all the touch related works.
        /// </summary>
        void ConfigTouch()
        {
            if (Handler != null && Handler.PlatformView != null)
            {
                _nativeView = Handler.PlatformView as LayoutPanelExt;
                WireEvents();
            }
            else
            {
                UnWireEvents();
                _nativeView = null;
            }
        }

        /// <summary>
        /// Wires touch related events.
        /// </summary>
        void WireEvents()
        {
            if (_nativeView == null || PullableContent == null || PullableContent.Handler == null || !(PullableContent.Handler.PlatformView is FrameworkElement pullElement))
            {
                return;
            }

            _pullableContentNativeView = pullElement;
            _pullableContentNativeView.ManipulationMode = ManipulationModes.All;
            _pullableContentNativeView.ManipulationDelta += PullableContentManipulationDelta;
            _pullableContentNativeView.ManipulationCompleted += PullableContentManipulationCompleted;
            _pullableContentNativeView.PointerCanceled += PullableContentCanceled;
            _pullableContentNativeView.PointerReleased += PullableContentPointerReleased;
            _pointerPressedEventHandler = new PointerEventHandler(PullableContentPointerPressed);
            _pullableContentNativeView.AddHandler(UIElement.PointerPressedEvent, _pointerPressedEventHandler, true);
        }

        /// <summary>
        /// Unwires touch related events.
        /// </summary>
        void UnWireEvents()
        {
            if (_pullableContentNativeView != null)
            {
                _pullableContentNativeView.ManipulationDelta -= PullableContentManipulationDelta;
                _pullableContentNativeView.ManipulationCompleted -= PullableContentManipulationCompleted;
                _pullableContentNativeView.PointerCanceled -= PullableContentCanceled;
                _pullableContentNativeView.PointerReleased -= PullableContentPointerReleased;
                _pullableContentNativeView.RemoveHandler(UIElement.PointerPressedEvent, _pointerPressedEventHandler);
                _pullableContentNativeView = null;
                _pointerPressedEventHandler = null;
            }
        }

        /// <summary>
        /// This method used to get the ScrollViewer.
        /// </summary>
        /// <param name="dependencyObject">The dependencyObject.</param>
        /// <returns>Returns the ScrollViewer.</returns>
        ScrollViewer? GetScrollViewer(DependencyObject dependencyObject)
        {
            var scrollViewer = dependencyObject as ScrollViewer;
            if (scrollViewer != null)
            {
                return scrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                scrollViewer = GetScrollViewer(child);
                if (scrollViewer != null)
                {
                    return scrollViewer;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the X and Y coordinates of the specified element based on the screen.
        /// </summary>
        /// <param name="native">The current element for which coordinates are requested.</param>
        Microsoft.Maui.Graphics.Point ChildLocationToScreen(object native)
        {
            var transformPoints = new Windows.Foundation.Point(0, 0);
            if (_nativeView != null)
            {
                var transform = _nativeView.TransformToVisual(native as UIElement);

                if (transform != null)
                {
                    transformPoints = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
                }
            }

            return new Microsoft.Maui.Graphics.Point(Math.Abs(transformPoints.X), Math.Abs(transformPoints.Y));
        }

        /// <summary>
        /// Gets the scroll offset of the specified child within a scroll view.
        /// </summary>
        /// <param name="scrollView">The current child for which the scroll offset is requested.</param>
        double GetChildScrollOffset(object scrollView)
        {
            double scrollOffset = 0;

            if (scrollView is ScrollViewer scroller)
            {
                scrollOffset = scroller.VerticalOffset;
            }
            else if (scrollView.GetType().Name == "FormsListView" || scrollView is Microsoft.UI.Xaml.Controls.ListView)
            {
                if (scrollView is UIElement uiElement)
                {
                    var control = GetScrollViewer(uiElement);
                    scrollOffset = control != null ? control.VerticalOffset : 0;
                }
                else
                {
                    scrollOffset = 0;
                }
            }

            return scrollOffset;
        }

        /// <summary>
        /// This method is triggered when a pointer is pressed in the <see cref="_pullableContentNativeView"/> control.
        /// </summary>
        /// <param name="sender">An instance of the <see cref="_pullableContentNativeView"/> control.</param>
        /// <param name="e">Event arguments for the pointer pressed event.</param>
        void PullableContentPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_nativeView != null)
            {
                // If we didn't pressed ScrollBar we didn't want to goes to continues loop. So here set maximum loop count.
                // When we pressed Thumb stick, we'll get ScrollBar as parent with maximum of 4 loops and for top and bottom arrow icon, we'll get ScrollBar as parent with maximum of 8 loops.
                const int thumbMaxLoopCount = 4;
                const int arrowMaxLoopCount = 8;

                // If we pressed any other than both, we set maxLoopCount as 0. So that we didn't want unnecessary loop.
                // If we pressed ScrollBar, then we didn't want to initiate pulling.
                _maxLoopCount = ((e.OriginalSource is Microsoft.UI.Xaml.Controls.Grid grid && grid.Name == "Root") || (e.OriginalSource is Rectangle rect && rect.Name == "ThumbVisual")) ? thumbMaxLoopCount : (e.OriginalSource is TextBlock ? arrowMaxLoopCount : 0);
                var isTouchInScrollBar = (e.OriginalSource is DependencyObject dependencyObject) && IsSourceScrollBar(dependencyObject);
                if (!isTouchInScrollBar)
                {
                    _childLoopCount = 0;
                    var touchPoint = e.GetCurrentPoint(_nativeView);
                    var point = new Microsoft.Maui.Graphics.Point(touchPoint.Position.X, touchPoint.Position.Y);
                    _nativeView.Children[0].ManipulationMode = ManipulationModes.All;
                    _isChildScrolledVertically = IsChildElementScrolled(PullableContent.GetVisualTreeDescendants().FirstOrDefault(), point);
                    HandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(0, 0));
                }
            }
        }

        /// <summary>
        /// Determines whether the current element is a ScrollBar.
        /// </summary>
        /// <param name="view">The view we were touching.</param>
        /// <returns>True if the ScrollBar is pressed; otherwise, false.</returns>
        bool IsSourceScrollBar(DependencyObject view)
        {
            if (_maxLoopCount <= 0)
            {
                return view is ScrollBar;
            }

            _maxLoopCount--;
            var parent = VisualTreeHelper.GetParent(view);
            if (parent != null && (parent is DependencyObject dependencyObject))
            {
                // Since we can't able to get the ScrollBar instance from tapping thumbstick, so we need to loop its parent until its parent was ScrollBar.
                // If parent is ScrollBar, then return true and no need to continue looping.
                return dependencyObject is ScrollBar ? true : IsSourceScrollBar(dependencyObject);
            }
            else
            {
                return false;
            }
        }

		#endregion

		#region Override Methods

		/// <summary>
		/// This method called upon adding a child to <see cref="SfPullToRefresh"/>.
		/// <exclude/>
		/// </summary>
		/// <param name="child">Child to be added.</param>
		protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (PullableContent != null && PullableContent == child && Handler != null)
            {
                WireEvents();
            }
        }

		/// <summary>
		/// This method called upon removing a child to <see cref="SfPullToRefresh"/>.
		/// <exclude/>
		/// </summary>
		/// <param name="child">Child to be removed.</param>
		/// <param name="oldLogicalIndex">Index of child.</param>
		protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);
            if (_progressCircleView != child)
            {
                UnWireEvents();
            }
        }

		/// <summary>
		/// Raises on handler changing.
		/// <exclude/>
		/// </summary>
		/// <param name="args">Relevant <see cref="HandlerChangingEventArgs"/>.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            if (args.OldHandler != null)
            {
                UnWireEvents();
                _nativeView = null;
            }

            base.OnHandlerChanging(args);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method triggers on any touch interaction on <see cref="SfPullToRefresh"/>.
        /// </summary>
        /// <param name="e">Event args.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// This method is triggered upon completion of touch manipulation in <see cref="_pullableContentNativeView"/> control.
        /// </summary>
        /// <param name="sender">An instance of the <see cref="_pullableContentNativeView"/> control.</param>
        /// <param name="e">Event arguments for the manipulation completed event.</param>
        void PullableContentManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            HandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(0, 0));
        }

        /// <summary>
        /// This method is triggered during a touch manipulation in <see cref="_pullableContentNativeView"/> control.
        /// </summary>
        /// <param name="sender">An instance of the <see cref="_pullableContentNativeView"/> control.</param>
        /// <param name="e">Event arguments for the manipulation delta event.</param>
        void PullableContentManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!_isIPullToRefresh && !_isChildScrolledVertically)
            {
                var pointerPoint = e.Cumulative.Translation;
                HandleTouchInteraction(PointerActions.Moved, new Microsoft.Maui.Graphics.Point(pointerPoint.X, pointerPoint.Y));
            }
        }

        /// <summary>
        /// This method is triggered when a pointer is released in the <see cref="_pullableContentNativeView"/> control.
        /// </summary>
        /// <param name="sender">An instance of the <see cref="_pullableContentNativeView"/> control.</param>
        /// <param name="e">Event arguments for the pointer released event.</param>
        void PullableContentPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            HandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(0, 0));
        }

        /// <summary>
        /// This method is triggered when a pointer is canceled in the <see cref="_pullableContentNativeView"/> control.
        /// </summary>
        /// <param name="sender">An instance of the <see cref="_pullableContentNativeView"/> control.</param>
        /// <param name="e">Event arguments for the pointer canceled event.</param>
        void PullableContentCanceled(object sender, PointerRoutedEventArgs e)
        {
            HandleTouchInteraction(PointerActions.Cancelled, new Microsoft.Maui.Graphics.Point(0, 0));
        }

        #endregion
    }
}
