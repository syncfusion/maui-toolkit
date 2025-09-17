namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Provides data for the event when tapped on a segment item in the segmented control.
	/// </summary>
	public class SegmentTappedEventArgs : EventArgs
	{
#nullable disable
		/// <summary>
		/// Gets the current tapped segment item.
		/// </summary>
		public SfSegmentItem SegmentItem { get; internal set; }
#nullable enable
	}
}
