namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Enables a MAUI view to recognize gestures.
	/// </summary>
	public partial class GestureDetector
	{
		#region Internal Methods

#pragma warning disable IDE0060 // Remove unused parameter
		internal void SubscribeNativeGestureEvents(View? mauiView)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			// The method is left empty as the platform - specific implementations are provided in separate files.
		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			// The method is left empty as the platform - specific implementations are provided in separate files. 
		}

		internal void CreateNativeListener()
		{
			// The method is left empty as the platform - specific implementations are provided in separate files.
		}

		#endregion
	}
}
