using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;

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

#if IOS

		// To store the child count of bottom sheet
		double _childLoopCount;

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

#if IOS
		/// <summary>
		/// Gets the X and Y coordinates of the specified element based on the screen.
		/// </summary>
		/// <param name="element">The current element for which coordinates are requested.</param>
		/// <param name="touchPoint">The current element for which coordinates are requested.</param>
		bool IsChildElementScrolled(IVisualTreeElement? element, Point touchPoint)
		{
			if (element is null)
			{
				return false;
			}

			var view = element as View;
			if (view is null || view.Handler is null || view.Handler.PlatformView is null)
			{
				return false;
			}

			if (view is ScrollView || view is ListView || view is CollectionView)
			{
				return true;
			}

			foreach (var childView in element.GetVisualChildren().OfType<View>())
			{
				if (childView is null || childView.Handler is null || childView.Handler.PlatformView is null)
				{
					return false;
				}

				var childNativeView = childView.Handler.PlatformView;

				// Here items X and Y position converts based on screen.
				Point locationOnScreen = ChildLocationToScreen(childNativeView);
				var bottom = locationOnScreen.Y + childView.Bounds.Height;
				var right = locationOnScreen.X + childView.Bounds.Width;

				// We loop through child only 10 times.
				if (touchPoint.Y >= locationOnScreen.Y && touchPoint.Y <= bottom && touchPoint.X >= locationOnScreen.X && touchPoint.X <= right && _childLoopCount <= 10)
				{
					_childLoopCount++;
					return IsChildElementScrolled(childView, touchPoint);
				}
			}

			_childLoopCount = 0;
			return false;
		}

		Point ChildLocationToScreen(object child)
		{
			if (child is UIKit.UIView view && this.Handler is not null)
			{
				var point = view.ConvertPointToView(view.Bounds.Location, Handler.PlatformView as UIKit.UIView);
				return new Microsoft.Maui.Graphics.Point(point.X, point.Y);
			}

			return new Microsoft.Maui.Graphics.Point(0, 0);
		}
#endif

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method to invoke swiping in bottom sheet.
		/// </summary>
		/// <param name="e">e.</param>
		public void OnTouch(Toolkit.Internals.PointerEventArgs e)
		{
#if IOS || MACCATALYST || ANDROID

#if IOS
			if (Content is null)
			{
				return;
			}

			var firstDescendant = Content.GetVisualTreeDescendants().FirstOrDefault();
			if (firstDescendant is not null && IsChildElementScrolled(firstDescendant, e.TouchPoint))
			{
				return;
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