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

		#region Interface Implementation

		/// <summary>
		/// Method to invoke swiping in bottom sheet.
		/// </summary>
		/// <param name="e">e.</param>
		public void OnTouch(Toolkit.Internals.PointerEventArgs e)
		{
#if IOS || MACCATALYST || ANDROID
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