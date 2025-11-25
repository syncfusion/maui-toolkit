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
		PullToRefreshProxy? _proxy;

		#endregion

		#region Internal properties

		/// <summary>
		/// List to store gesture recognizers attached to pull to refresh control descendants.
		/// </summary>
		internal List<UIGestureRecognizer>? ChildGestureRecognizers
		{
			get { return _childGestureRecognizers; }
			set { _childGestureRecognizers = value; }
		}

		/// <summary>
		/// Indicates whether the child elements of pullToRefresh is Scrolled Vertically or not.
		/// </summary>
		internal bool IsChildScrolledVertically
		{
			get { return _isChildScrolledVertically; }
			set { _isChildScrolledVertically = value; }
		}

		/// <summary>
		/// Indicates the native view associated with pullToRefresh.
		/// </summary>
		internal LayoutViewExt? NativeView
		{
			get { return _nativeView; }
			set { _nativeView = value; }
		}

		/// <summary>
		/// Gets or sets the panGesture for the pullToRefresh.
		/// </summary>
		internal UIPanGestureRecognizer? PanGesture
		{
			get { return _panGesture; }
			set { _panGesture = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// This method initializes all the gesture related works.
		/// </summary>
		void InitializeGesture()
        {
			Dispose();
			if (Handler is not null && Handler.PlatformView is LayoutViewExt layoutView)
            {
                NativeView = layoutView;

                if (NativeView.GestureRecognizers is not null && NativeView.GestureRecognizers.Length > 0)
                {
                    var panGestureRecognizer = NativeView.GestureRecognizers.OfType<UIPanGestureRecognizer>().FirstOrDefault();
					if (panGestureRecognizer is not null)
					{
						PanGesture = panGestureRecognizer;
						PanGesture.CancelsTouchesInView = false;
						ChildGestureRecognizers = [];
						_proxy = new PullToRefreshProxy(this);
						PanGesture.ShouldRecognizeSimultaneously = null;
						PanGesture.ShouldRecognizeSimultaneously += _proxy.ShouldRecognizeSimultaneousGesture;
						PanGesture.ShouldBegin += _proxy.GestureShouldBegin;
					}
				}
            }
        }

		/// <summary>
		/// Unwires wired events and disposes used objects.
		/// </summary>
		void Dispose()
		{
			if (_proxy is not null)
			{
				if (PanGesture is not null)
				{
					PanGesture.ShouldRecognizeSimultaneously -= _proxy.ShouldRecognizeSimultaneousGesture;
					PanGesture.ShouldBegin -= _proxy.GestureShouldBegin;
					PanGesture = null;
				}

				_proxy = null;
			}

			NativeView = null;
			ChildGestureRecognizers = null;
		}

        /// <summary>
        /// Gets the X and Y coordinates of the specified element based on the screen.
        /// </summary>
        /// <param name="child">The current element for which coordinates are requested.</param>
        Microsoft.Maui.Graphics.Point ChildLocationToScreen(object child)
        {
            if (child is UIView view)
            {
                var point = view.ConvertPointToView(view.Bounds.Location, NativeView);
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
			// When the handler is changed without creating a new virtual view, adding a touch listener will skip attaching touch wiring to the native view.
			// This is because the TouchDetector will still remain in the VirtualView and will be wired with the old handler and old native view.
			this.RemoveTouchListener(this);
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
            if (args.OldHandler is not null)
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
            IsChildScrolledVertically = false;
            if (e.Action == PointerActions.Pressed || e.Action == PointerActions.Moved)
            {
                _childLoopCount = 0;
                var firstDescendant = PullableContent.GetVisualTreeDescendants().FirstOrDefault();
                if (firstDescendant is not null)
                {
                    IsChildScrolledVertically = IsChildElementScrolled(firstDescendant, e.TouchPoint);
                }

                if (IsChildScrolledVertically)
                {
                    return;
                }
            }

            HandleTouchInteraction(e.Action, e.TouchPoint);

            if (e.Action == PointerActions.Released || e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
            {
                IsIPullToRefresh = false;
                if (ChildGestureRecognizers is not null)
                {
                    foreach (var recognizer in ChildGestureRecognizers)
                    {
                        recognizer.Enabled = true;
                    }
                }
            }
        }

        #endregion
    }

	/// <summary>
	///  Manages the native gesture events.
	/// </summary>
	internal class PullToRefreshProxy
	{
		#region Fields

		WeakReference<SfPullToRefresh> _sfPullToRefresh;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PullToRefreshProxy"/> class.
		/// </summary>
		/// <param name="pullToRefresh">pullToRefresh instance.</param>
		internal PullToRefreshProxy(SfPullToRefresh pullToRefresh)
		{
			_sfPullToRefresh = new(pullToRefresh);
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets the instance of <see cref="SfPullToRefresh"/> class.
		/// </summary>
		internal SfPullToRefresh? SfPullToRefresh
		{
			get => _sfPullToRefresh is not null && _sfPullToRefresh.TryGetTarget(out var v) ? v : null;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Raises when pan gesture begins.
		/// </summary>
		/// <param name="uIGestureRecognizer">Instance of pan gesture.</param>
		/// <returns>True, If the gesture should begin, else false.</returns>
		internal bool GestureShouldBegin(UIGestureRecognizer uIGestureRecognizer)
		{
			if (SfPullToRefresh is null)
			{
				return false;
			}

			if (SfPullToRefresh.ChildGestureRecognizers is not null)
			{
				foreach (var recognizer in SfPullToRefresh.ChildGestureRecognizers)
				{
					recognizer.Enabled = true;
				}
			}

			return !(SfPullToRefresh.IsIPullToRefresh && SfPullToRefresh.IsChildScrolledVertically);
		}

		/// <summary>
		/// Raises when another gesture happens during <see cref="SfPullToRefresh.PanGesture"/>.
		/// </summary>
		/// <param name="gesture1">Instance of <see cref="SfPullToRefresh.PanGesture"/>.</param>
		/// <param name="gesture2">Instance of other gesture.</param>
		/// <returns>Return true, if we want to handle gesture, else false.</returns>
		internal bool ShouldRecognizeSimultaneousGesture(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
		{
			if (SfPullToRefresh is null || SfPullToRefresh.PanGesture is null || !(gesture2 is UIPanGestureRecognizer))
			{
				return false;
			}

			if (gesture2.View is UIScrollView uIScrollView)
			{
				HandleScrollBars(gesture2);
			}

			if (Math.Abs(SfPullToRefresh.PanGesture.VelocityInView(SfPullToRefresh.NativeView).X) > Math.Abs(SfPullToRefresh.PanGesture.VelocityInView(SfPullToRefresh.NativeView).Y))
			{
				return HandleHorizontalSwipe(gesture1, gesture2);
			}
			else if (SfPullToRefresh.PanGesture.VelocityInView(SfPullToRefresh.NativeView).Y <= 0)
			{
				gesture2.Enabled = true;
				return false;
			}
			else
			{
				return HandleCustomPanGesture(gesture1, gesture2);
			}
		}

		#endregion

		#region Private Methods
		void HandleScrollBars(UIGestureRecognizer gesture2)
		{
			if (SfPullToRefresh is null)
			{
				return;
			}

			var scrollBars = gesture2.View.Subviews.Where(x => x.Class.Name == "_UIScrollerImpContainerView");
			foreach (var scroll in scrollBars)
			{
				var touchPoint = gesture2.LocationInView(scroll);
				if (scroll.Bounds.Contains(touchPoint))
				{
					SfPullToRefresh.CancelPulling();
				}

				break;
			}
		}

		bool HandleHorizontalSwipe(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
		{
			if (SfPullToRefresh is null)
			{
				return false;
			}

			if (SfPullToRefresh.PanGesture is not null && gesture2.State == UIGestureRecognizerState.Began)
			{
				SfPullToRefresh.PanGesture.Enabled = false;
			}

			gesture2.Enabled = true;
			gesture1.Enabled = true;

			UIGestureRecognizer.Token? gestureToken = null;

			gestureToken ??= gesture2.AddTarget(() =>
				{
					if (gesture2.State == UIGestureRecognizerState.Failed ||
						gesture2.State == UIGestureRecognizerState.Ended ||
						gesture2.State == UIGestureRecognizerState.Cancelled)
					{
						if (SfPullToRefresh !=  null && SfPullToRefresh.PanGesture != null)
						{
							SfPullToRefresh.PanGesture.Enabled = true;
						}

						if (gestureToken is not null)
						{
							gesture2.RemoveTarget(gestureToken);
							gestureToken = null;
						}
					}
				});

			return true;
		}

		bool HandleCustomPanGesture(UIGestureRecognizer gesture1, UIGestureRecognizer gesture2)
		{
			if (SfPullToRefresh is not null && gesture1 is not null && gesture2 is not null &&
				gesture1 is UIPanGestureRecognizer &&
				gesture1.Class is not null &&
				gesture1.Class.Name is not null &&
				!gesture1.Class.Name.Equals("UIScrollViewPanGestureRecognizer", StringComparison.Ordinal))
			{
				if (SfPullToRefresh.IsChildScrolledVertically)
				{
					gesture2.Enabled = true;
				}
				else
				{
					if (SfPullToRefresh.ChildGestureRecognizers is not null && !SfPullToRefresh.ChildGestureRecognizers.Contains(gesture2))
					{
						SfPullToRefresh.ChildGestureRecognizers.Add(gesture2);
					}

					gesture2.Enabled = false;
				}
			}

#if NET10_0_OR_GREATER && IOS
			if (gesture2 is UIPanGestureRecognizer pan2 && pan2.Class != null && pan2.Class.Name != null && pan2.Class.Name.Equals("UIScrollViewPanGestureRecognizer", StringComparison.Ordinal))
			{
				return true;
			}
#endif

			return false;
		}

		#endregion
	}
}
