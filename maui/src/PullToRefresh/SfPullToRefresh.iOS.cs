using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using UIKit;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// <see cref="SfPullToRefresh"/> enables interaction to refresh the loaded view. This control allows users to trigger a refresh action by performing the pull-to-refresh gesture.
    /// </summary>
    public partial class SfPullToRefresh
    {
        #region Fields

        UIPanGestureRecognizer? _panGesture;
        LayoutViewExt? _nativeView;
        List<UIGestureRecognizer>? _childGestureRecognizers;
        bool _isChildScrolledVertically = false;
        #endregion

        #region Private Methods

        /// <summary>
        /// This method initializes all the gesture related works.
        /// </summary>
        void InitializeGesture()
        {
            if (Handler != null && Handler.PlatformView is LayoutViewExt layoutView)
            {
                _nativeView = layoutView;

                if (_nativeView.GestureRecognizers != null && _nativeView.GestureRecognizers.Length > 0)
                {
                    var panGestureRecognizer = _nativeView.GestureRecognizers.OfType<UIPanGestureRecognizer>().FirstOrDefault();

                    if (panGestureRecognizer != null)
                    {
                        _panGesture = panGestureRecognizer;
                        _panGesture.CancelsTouchesInView = false;
                        _childGestureRecognizers = new List<UIGestureRecognizer>();
                        _panGesture.ShouldRecognizeSimultaneously = null;
                        _panGesture.ShouldRecognizeSimultaneously += ShouldRecognizeSimultaneousGesture;
                        _panGesture.ShouldBegin += GestureShouldBegin;
                    }
                }
            }
            else
            {
                Dispose();
            }
        }

        /// <summary>
        /// Unwires wired events and disposes used objects.
        /// </summary>
        void Dispose()
        {
            if (_panGesture != null)
            {
                _panGesture.ShouldRecognizeSimultaneously -= ShouldRecognizeSimultaneousGesture;
                _panGesture.ShouldBegin -= GestureShouldBegin;
            }

            _nativeView = null;
            _childGestureRecognizers = null;
            _panGesture = null;
        }

        /// <summary>
        /// Gets the X and Y coordinates of the specified element based on the screen.
        /// </summary>
        /// <param name="child">The current element for which coordinates are requested.</param>
        Microsoft.Maui.Graphics.Point ChildLocationToScreen(object child)
        {
            if (child is UIView view)
            {
                var point = view.ConvertPointToView(view.Bounds.Location, _nativeView);
                return new Microsoft.Maui.Graphics.Point(point.X, point.Y);
            }

            return new Microsoft.Maui.Graphics.Point(0, 0);
        }

        /// <summary>
        /// Gets the scroll offset of the specified child within a scroll view.
        /// </summary>
        /// <param name="scrollView">The current child for which the scroll offset is requested.</param>
        double GetChildScrollOffset(object scrollView)
        {
            if (scrollView is UIScrollView uIScroll)
            {
                return uIScroll.ContentOffset.Y;
            }

            if (scrollView is UIView scrollViewUIView && scrollViewUIView.ToString().Contains("UICollectionViewControllerWrapperView", StringComparison.Ordinal))
            {
                foreach (var subview in scrollViewUIView.Subviews.OfType<UIScrollView>())
                {
                    return subview.ContentOffset.Y;
                }
            }

            return 0;
        }

        /// <summary>
        /// Method configures all the touch related works.
        /// </summary>
        void ConfigTouch()
        {
            this.AddTouchListener(this);
            InitializeGesture();
        }

		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing.
		/// <exclude/>
		/// </summary>
		/// <param name="args">Relevant <see cref="HandlerChangingEventArgs"/>.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            if (args.OldHandler != null)
            {
                Dispose();
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
            _isChildScrolledVertically = false;
            if (e.Action == PointerActions.Pressed || e.Action == PointerActions.Moved)
            {
                _childLoopCount = 0;
                var firstDescendant = PullableContent.GetVisualTreeDescendants().FirstOrDefault();
                if (firstDescendant != null)
                {
                    _isChildScrolledVertically = IsChildElementScrolled(firstDescendant, e.TouchPoint);
                }

                if (_isChildScrolledVertically)
                {
                    return;
                }
            }

            HandleTouchInteraction(e.Action, e.TouchPoint);

            if (e.Action == PointerActions.Released || e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
            {
                _isIPullToRefresh = false;
                if (_childGestureRecognizers != null)
                {
                    foreach (var recognizer in _childGestureRecognizers)
                    {
                        recognizer.Enabled = true;
                    }
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises when <see cref="_panGesture"/> begins.
        /// </summary>
        /// <param name="uIGestureRecognizer">Instance of <see cref="_panGesture"/>.</param>
        /// <returns>True, If the gesture should begin, else false.</returns>
        bool GestureShouldBegin(UIGestureRecognizer uIGestureRecognizer)
        {
            if (_childGestureRecognizers != null)
            {
                foreach (var recognizer in _childGestureRecognizers)
                {
                    recognizer.Enabled = true;
                }
            }

            return !(_isIPullToRefresh && _isChildScrolledVertically);
        }

        /// <summary>
        /// Raises when another gesture happens during <see cref="_panGesture"/>.
        /// </summary>
        /// <param name="gesture1">Instance of <see cref="_panGesture"/>.</param>
        /// <param name="gesture2">Instance of other gesture.</param>
        /// <returns>Return true, if we want to handle gesture, else false.</returns>
        bool ShouldRecognizeSimultaneousGesture(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
        {
            if (_panGesture == null || !(gesture2 is UIPanGestureRecognizer))
            {
                return false;
            }

            if (gesture2.View is UIScrollView uIScrollView)
            {
                HandleScrollBars(gesture2);
            }

            if (Math.Abs(_panGesture.VelocityInView(_nativeView).X) > Math.Abs(_panGesture.VelocityInView(_nativeView).Y))
            {
                return HandleHorizontalSwipe(gesture1, gesture2);
            }
            else if (_panGesture.VelocityInView(_nativeView).Y <= 0)
            {
                gesture2.Enabled = true;
                return false;
            }
            else
            {
                return HandleCustomPanGesture(gesture1, gesture2);
            }
        }

        void HandleScrollBars(UIGestureRecognizer gesture2)
        {
            var scrollBars = gesture2.View.Subviews.Where(x => x.Class.Name == "_UIScrollerImpContainerView");
            foreach (var scroll in scrollBars)
            {
                var touchPoint = gesture2.LocationInView(scroll);
                if (scroll.Bounds.Contains(touchPoint))
                {
                    CancelPulling();
                }

                break;
            }
        }

        bool HandleHorizontalSwipe(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
        {
            if (_panGesture != null && gesture2.State == UIGestureRecognizerState.Began)
            {
                _panGesture.Enabled = false;
            }

            gesture2.Enabled = true;
            gesture1.Enabled = true;

            UIGestureRecognizer.Token? gestureToken = null;

            if (gestureToken == null)
            {
                gestureToken = gesture2.AddTarget(() =>
                {
                    if (gesture2.State == UIGestureRecognizerState.Failed ||
                        gesture2.State == UIGestureRecognizerState.Ended ||
                        gesture2.State == UIGestureRecognizerState.Cancelled)
                    {
                        if (_panGesture != null)
                        {
                            _panGesture.Enabled = true;
                        }

                        if (gestureToken != null)
                        {
                            gesture2.RemoveTarget(gestureToken);
                            gestureToken = null;
                        }
                    }
                });
            }

            return true;
        }

        bool HandleCustomPanGesture(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
        {
            if (gesture1 != null && gesture2 != null &&
                gesture1 is UIPanGestureRecognizer &&
                gesture1.Class != null &&
                gesture1.Class.Name != null &&
                !gesture1.Class.Name.Equals("UIScrollViewPanGestureRecognizer", StringComparison.Ordinal))
            {
                if (_isChildScrolledVertically)
                {
                    gesture2.Enabled = true;
                }
                else
                {
                    if (_childGestureRecognizers != null && !_childGestureRecognizers.Contains(gesture2))
                    {
                        _childGestureRecognizers.Add(gesture2);
                    }

                    gesture2.Enabled = false;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        #endregion
    }
}
