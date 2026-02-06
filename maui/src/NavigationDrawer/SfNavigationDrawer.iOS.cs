using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using CoreGraphics;
using UIKit;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	public partial class SfNavigationDrawer
	{
		#region Fields

		/// <summary>
		/// Determines whether the touch should be processed.
		/// </summary>
		internal bool _canProcessTouch = true;

		/// <summary>
		/// Indicates whether the tap gesture has been removed.
		/// </summary>
		bool _isTapGestureRemoved;

		/// <summary>
		/// The pan gesture recognizer used for handling touch events.
		/// </summary>
		UIPanGestureRecognizer? _panGesture;

		/// <summary>
		/// The native view associated with the navigation drawer.
		/// </summary>
		LayoutViewExt? _nativeView;

#if !MACCATALYST

        /// <summary>
        /// Represents a scrollable UIView.
        /// </summary>
        internal UIView? ScrollableView;

        /// <summary>
        /// Indicates whether scrolling is currently active or not.
        /// </summary>
        internal bool isScroll = false;
#endif

		#endregion


		#region Private Methods

		/// <summary>
		/// This method used for handle the tap.
		/// </summary>
		/// <param name="e">The TapEventArgs parameter.</param>
		void ITapGestureListener.OnTap(TapEventArgs e)
		{
			if (!_canProcessTouch)
			{
				_canProcessTouch = true;
				return;
			}
		}

		/// <summary>
		/// Determines whether the tap gesture should be handled for the specified view.
		/// </summary>
		/// <param name="view">The view to check.</param>
		void ITapGestureListener.ShouldHandleTap(object view)
		{
			UIKit.UIView? touchView = (view as UIKit.UITouch)?.View;
			_canProcessTouch = true;
			// Provide the touch to the list view(framework) by removing the TapGesture.
			if (touchView is not Syncfusion.Maui.Toolkit.Platform.LayoutViewExt &&
				touchView is not Syncfusion.Maui.Toolkit.Platform.NativePlatformGraphicsView)
			{
				if (view is UIKit.UITouch uiTouch)
				{
					if (uiTouch.GestureRecognizers != null)
					{
						foreach (var gesture in uiTouch.GestureRecognizers)
						{
							if (gesture is UILongPressGestureRecognizer || gesture is UIPanGestureRecognizer)
							{
								this.RemoveGestureListener(this);
								_isTapGestureRemoved = true;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Initializes the gesture recognizer for handling touch events.
		/// </summary>
		void InitializeGesture()
		{
			if (Handler != null && Handler.PlatformView != null)
			{
				_nativeView = Handler.PlatformView as LayoutViewExt;
				if (_nativeView != null && _nativeView.GestureRecognizers != null && _nativeView.GestureRecognizers.Length > 0)
				{
					_panGesture = (UIPanGestureRecognizer)_nativeView.GestureRecognizers.FirstOrDefault(x => x is UIPanGestureRecognizer)!;
					_panGesture.ShouldBegin += _proxy.GestureShouldBegin;
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
				_panGesture.ShouldBegin -= _proxy.GestureShouldBegin;
			}

			_nativeView = null;
			_panGesture = null;
		}

		/// <summary>
		/// Handles touch input events when a pointer enters the control.
		/// </summary>
		/// <param name="e">Pointer event arguments containing information about the touch event.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
			if (!_canProcessTouch)
			{
				return;
			}

#if !MACCATALYST
            if (isScroll)
            {
                isScroll = false;
                return;
            }
#endif

			switch (e.Action)
			{
				case PointerActions.Pressed:
					OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
					break;

				case PointerActions.Moved:
					OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
					break;

				case PointerActions.Released:
					if (_isTapGestureRemoved)
					{
						this.AddGestureListener(this);
						_isTapGestureRemoved = false;
					}
					OnHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
					break;

				case PointerActions.Cancelled:
				case PointerActions.Entered:
					break;
			}
		}

		/// <summary>
		/// Method configures all the touch related works.
		/// </summary>
		void ConfigureTouch()
		{
			InitializeGesture();
		}

#if !MACCATALYST

        /// <summary>
        /// Determines if the specified UIView or any of its parent views is a UIScrollView or UICollectionView.
        /// </summary>
        /// <param name="view">The UIView to check.</param>
        /// <returns>True if a scrollable view is found, otherwise false.</returns>
        internal bool IsScrollableView(UIView? view)
        {
            if (view == null)
                return false;

            if (view is UIScrollView || view is UICollectionView)
            {
                ScrollableView = view;
                return true;
            }

            UIView? parent = view.Superview;
            while (parent != null)
            {
                if (parent is UIScrollView || parent is UICollectionView)
                {
                    ScrollableView = parent;
                    return true;
                }

                parent = parent.Superview;
            }

            return false;
        }

#endif
		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing.
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
	}
#if IOS || MACCATALYST

	internal class SfNavigationDrawerProxy(SfNavigationDrawer navigationDrawer)
	{
		readonly WeakReference<SfNavigationDrawer> _view = new(navigationDrawer);

		internal bool GestureShouldBegin(UIGestureRecognizer uIGestureRecognizer)
		{
			_view.TryGetTarget(out var view);
			if (view != null)
			{
				bool? isPressed = view?._canProcessTouch;
				if (isPressed == false)
				{
					// Return false if the CanProcessTouch value is false, preventing the control's gesture from starting.
					return false;
				}
			}

#if !MACCATALYST

            // Use the improved gesture detection logic
            if (view is not null && view.IsOpen)
            {
                if(view.DrawerSettings.Position == Position.Bottom || view.DrawerSettings.Position == Position.Top)
                {
                    if (uIGestureRecognizer is UIPanGestureRecognizer panGesture)
                    {
                        var translation = panGesture.TranslationInView(panGesture.View);
                        var location = panGesture.LocationInView(panGesture.View);
                        UIView? hitView = panGesture.View?.HitTest(location, null);
                        bool isScrollableContent = view?.IsScrollableView(hitView) ?? false;
                        bool isHorizontalGesture = Math.Abs(translation.X) > Math.Abs(translation.Y);
                        if (isScrollableContent && !isHorizontalGesture && view is not null)
                        {
                            view.isScroll = true;
                            if (view.ScrollableView is UIScrollView scrollView)
                            {
                                bool atTop = scrollView.ContentOffset.Y <= 0;
                                bool atBottom = scrollView.ContentOffset.Y >= scrollView.ContentSize.Height - scrollView.Bounds.Height;
                                if(ShouldProcessDrawerScroll(atTop, atBottom, view.DrawerSettings.Position,translation))
                                {
                                    view.isScroll =false;
                                    return true;
                                }

                                return false;
                            }

                            if(view.ScrollableView is UITableView collectionView)
                            {
                                bool atTop = collectionView.ContentOffset.Y <= 0;
                                bool atBottom = collectionView.ContentOffset.Y >= collectionView.ContentSize.Height - collectionView.Bounds.Height;
                                if (ShouldProcessDrawerScroll(atTop, atBottom, view.DrawerSettings.Position, translation))
                                {
                                    view.isScroll = false;
                                    return true;
                                }

                                return false;
                            }

                            return false;
                        }
                    }
                }
            }
#endif
			return true;
		}

#if !MACCATALYST
        /// <summary>
        /// Determines whether a scroll gesture should be handled by the drawer.
        /// </summary>
        /// <param name="atTop">Whether the scrollable content is at the top position</param>
        /// <param name="atBottom">Whether the scrollable content is at the bottom position</param>
        /// <param name="position">The position of the drawer (Top or Bottom)</param>
        /// <param name="translation">The translation point of the gesture</param>
        /// <returns>True if the drawer should handle the gesture; otherwise, false</returns>
        bool ShouldProcessDrawerScroll(bool atTop, bool  atBottom,Position position,CGPoint translation)
        {
            _view.TryGetTarget(out var view);
            if (view is not null)
            {
                if (position == Position.Bottom)
                {
                    if (atTop && translation.Y > 0)
                    {
                        return true;
                    }
                }
                else if (atBottom && translation.Y < 0)
                {
                    return true;
                }
            }

            return false;
        }
#endif

	}

#endif
}