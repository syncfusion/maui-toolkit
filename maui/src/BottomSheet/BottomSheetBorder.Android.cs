
namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	internal partial class BottomSheetBorder
	{
		/// <summary>
		/// Forwards the specified pointer action and touch point to the associated <see cref="SfBottomSheet"/> for gesture handling.
		/// </summary>
		/// <param name="action">The pointer action to deliver.</param>
		/// <param name="point">The touch point in device-independent pixels.</param>
		internal void ForwardToSheet(Internals.PointerActions action, Point point)
		{
			if (_bottomSheetRef?.TryGetTarget(out var bottomSheet) == true)
			{
				bottomSheet.OnHandleTouch(action, point);
			}
		}
	}
}
