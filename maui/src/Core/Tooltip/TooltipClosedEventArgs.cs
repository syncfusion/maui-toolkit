
namespace Syncfusion.Maui.Toolkit
{
	/// <summary>
	/// Represents the event argument class for tooltip dismiss event.
	/// </summary>
	public class TooltipClosedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the value of tooltip duration is whether completed or terminated.
		/// </summary>
		public bool IsCompleted { get; internal set; }

		/// <summary>
		/// Intializes the new instance for <see cref="TooltipClosedEventArgs"/> class.
		/// </summary>
		public TooltipClosedEventArgs()
		{

		}
	}
}
