
namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Helper class to override native methods.
	/// </summary>
	public abstract class PullToRefreshBase : SfView
	{
		#region Android

#if ANDROID
		/// <summary>
		/// This method will helps to intercept touch of <see cref="PullToRefreshBase"/>.
		/// </summary>
		/// <param name="ev">MotionEvent arguments.</param>
		/// <returns>Returns true, if <see cref="PullToRefreshBase"/> will handle the touch.</returns>
		internal virtual bool OnInterceptTouchEvent(Android.Views.MotionEvent? ev)
		{
			return false;
		}
#endif

		#endregion
	}
}