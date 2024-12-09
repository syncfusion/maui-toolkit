namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// This interface provides properties for handling  gesture recognition.
	/// </summary>
	public interface IGestureListener
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating whether touches should be passed to the parent or child view.
		/// </summary>
		bool IsTouchHandled
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the single-tap gesture recognizer should be restricted when a double-tap gesture is triggered.
		/// </summary>
		internal bool IsRequiredSingleTapGestureRecognizerToFail
		{
			get { return true; }
		}

		#endregion
	}
}
