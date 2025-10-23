using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;

#if IOS || MACCATALYST
using UIKit;
using CoreGraphics;
#endif

namespace Syncfusion.Maui.Toolkit.BottomSheet
{
    /// <summary>
    /// Represents the <see cref="BottomSheetBorder"/> that defines the layout of bottom sheet.
    /// </summary>
    internal partial class BottomSheetBorder : SfBorder, ITouchListener
    {
		#region Fields

		// To store the weak reference of bottom sheet instance.
        readonly WeakReference<SfBottomSheet>? _bottomSheetRef;

#if IOS || MACCATALYST

		/// <summary>
		/// Represents a scrollable UIView.
		/// </summary>
		UIView? _scrollableView;

		/// <summary>
		/// Determines whether the touch should be processed.
		/// </summary>
		bool _canProcessTouch = true;

		/// <summary>
		/// Represents a scrollable view under the finger (UIScrollView/UICollectionView).
		/// </summary>
		UIScrollView? _iosScrollView;

		/// <summary>
		/// Determines whether the touch started in a scrollable view.
		/// </summary>
		bool _iosInsideScrollable;

		/// <summary>
		/// Determines whether we have handed off touch to the bottom sheet.
		/// </summary>
		bool _iosHandoff;

		/// <summary>
		/// Stores the last Y position.
		/// </summary>
		double _iosLastY;

		/// <summary>
		/// A small epsilon value to avoid jitter at scroll edges.
		/// </summary>
		nfloat _epsilon = 1.0f;

#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BottomSheetBorder"/> class.
		/// </summary>
		/// <param name="bottomSheet">The SfBottomSheet instance.</param>
		/// <exception cref="ArgumentNullException">Thrown if bottomSheet is null.</exception>
		public BottomSheetBorder(SfBottomSheet bottomSheet)
        {
			if (bottomSheet is not null)
			{
				_bottomSheetRef = new WeakReference<SfBottomSheet>(bottomSheet);
				this.AddTouchListener(this);
			}
        }

		#endregion

		#region Private Methods

#if IOS || MACCATALYST

		/// <summary>
		/// Determines if the specified UIView or any of its parent views is a UIScrollView or UICollectionView.
		/// </summary>
		/// <param name="view">The UIView to check.</param>
		/// <returns>True if a scrollable view is found, otherwise false.</returns>
		bool IsScrollableView(UIView? view)
		{
			if (view == null)
			{
				return false;
			}

			if (view is UIScrollView || view is UICollectionView)
			{
				_scrollableView = view;
				return true;
			}

			UIView? parent = view.Superview;
			while (parent != null)
			{
				if (parent is UIScrollView || parent is UICollectionView)
				{
					_scrollableView = parent;
					return true;
				}

				parent = parent.Superview;
			}

			return false;
		}

		/// <summary>
		/// Determines if the inner UIScrollView can scroll in the direction of the finger movement.
		/// </summary>
		/// <param name="sv">The scrollable view to check.</param>
		/// <param name="dy">The scroll direction.</param>
		/// <returns>True if inner view is scrollable, otherwise false.</returns>
		bool CanInnerScroll(UIScrollView sv, double dy)
		{
			if (sv is not null)
			{
				// Insets (AdjustedContentInset is correct with safe area / content inset)
				nfloat topInset = sv.AdjustedContentInset.Top;
				nfloat bottomInset = sv.AdjustedContentInset.Bottom;

				// Effective scrollable range
				nfloat visibleHeight = sv.Bounds.Height;
				nfloat contentHeight = sv.ContentSize.Height;
				// If content is shorter than the viewport, there's nothing to scroll.
				if (contentHeight <= visibleHeight - (topInset + bottomInset))
				{
					return false;
				}

				nfloat minOffsetY = -topInset;
				nfloat maxOffsetY = (nfloat)Math.Max(0, contentHeight - visibleHeight + bottomInset);
				nfloat y = sv.ContentOffset.Y;

				if (dy < 0) // finger up => scroll down
				{
					return y < (maxOffsetY - _epsilon);
				}

				if (dy > 0) // finger down => scroll up
				{
					return y > (minOffsetY + _epsilon);
				}
			}

			return false;
		}

#endif

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method to invoke swiping in bottom sheet.
		/// </summary>
		/// <param name="e">e.</param>
		public void OnTouch(Internals.PointerEventArgs e)
		{
#if IOS || MACCATALYST || ANDROID

#if IOS || MACCATALYST
			if (!_canProcessTouch)
			{
				_canProcessTouch = true;
				return;
			}

			// Track touch points for translation calculation
			if (e.Action == PointerActions.Pressed)
			{				
				if (Handler?.PlatformView is UIView platformView)
				{
					var nativePoint = new CGPoint(e.TouchPoint.X, e.TouchPoint.Y);
					UIView? hitView = platformView.HitTest(nativePoint, null);

					if (IsScrollableView(hitView))
					{
						_iosScrollView = _scrollableView as UIScrollView;
						_iosInsideScrollable = _iosScrollView is not null;
						_iosHandoff = false;
						_iosLastY = e.TouchPoint.Y;

						if (_iosInsideScrollable)
						{
							// Start inside a scrollable: let it consume initially.
							// DO NOT forward Pressed to the sheet yet.
							return;
						}
					}
				}
			}
			else if (e.Action == PointerActions.Moved)
			{
				if (_iosInsideScrollable && _iosScrollView is not null)
                {
                    double dy = e.TouchPoint.Y - _iosLastY;

                    // While inner can scroll in this direction, don't route to sheet.
                    if (CanInnerScroll(_iosScrollView, dy))
                    {
                        _iosLastY = e.TouchPoint.Y;
                        return; // list keeps consuming
                    }

                    // Edge reached => hand off to sheet once
                    if (!_iosHandoff && _bottomSheetRef?.TryGetTarget(out var bottomSheetPressed) == true)
                    {
						bottomSheetPressed.OnHandleTouch(PointerActions.Pressed, e.TouchPoint);
                        _iosHandoff = true;

						// Route this first move to the sheet as well
						bottomSheetPressed.OnHandleTouch(PointerActions.Moved, e.TouchPoint);
                        _iosLastY = e.TouchPoint.Y;
                        return;
                    }

                    // Already handed off => keep sending moves to sheet here; skip common forward
                    if (_iosHandoff && _bottomSheetRef?.TryGetTarget(out var bottomSheetMoved) == true)
                    {
						bottomSheetMoved.OnHandleTouch(PointerActions.Moved, e.TouchPoint);
                        _iosLastY = e.TouchPoint.Y;
                        return;
                    }

                    _iosLastY = e.TouchPoint.Y;
                    return;
                }
			}
			else if (e.Action == PointerActions.Released || e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
			{
				// If we started in a scrollable and never handed off, do not forward release to sheet
                if (_iosInsideScrollable && !_iosHandoff)
                {
                    _iosInsideScrollable = false;
                    _iosScrollView = null;
                    _iosHandoff = false;
                    return;
                }

                // If we did hand off, forward release here and reset; skip common forward
                if (_iosHandoff && _bottomSheetRef?.TryGetTarget(out var bottomSheetReleased) == true)
                {
					bottomSheetReleased.OnHandleTouch(PointerActions.Released, e.TouchPoint);
                    _iosInsideScrollable = false;
                    _iosScrollView = null;
                    _iosHandoff = false;
                    return;
                }
			}
#endif

			if (e is not null && _bottomSheetRef is not null)
			{
				if (_bottomSheetRef.TryGetTarget(out var bottomSheet))
				{
					bottomSheet.OnHandleTouch(e.Action, e.TouchPoint);
				}
			}

#endif
		}

		#endregion

	}
}