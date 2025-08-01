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
    internal class BottomSheetBorder : SfBorder, ITouchListener
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
		/// Indicates whether pressed occurs inside a scrollable view.
		/// </summary>
		bool _isPressed = false;

		/// <summary>
		/// Determines whether the touch should be processed.
		/// </summary>
		bool _canProcessTouch = true;

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
						_canProcessTouch = false; // Only disable bottom sheet swipe if touch is inside scrollable view
						_isPressed = true;
						return;
					}
					else
					{
						_canProcessTouch = true; // Allow bottom sheet swipe
					}
				}

			}
			else if (e.Action == PointerActions.Moved)
			{
				// When moved is called multiple times, this flag helps us prevent the bottom sheet scrolling	
				if(_isPressed)
				{
					return;
				}
			}
			else if (e.Action == PointerActions.Released || e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
			{
				_canProcessTouch = true;

				// Early return to avoid bottom sheet position update after the scroll occured in a scrollable view
				if(_isPressed)
				{
					_isPressed = false;
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