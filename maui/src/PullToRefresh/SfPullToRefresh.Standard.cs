using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
	/// <summary>
	/// <see cref="SfPullToRefresh"/> enables interaction to refresh the loaded view. This control allows users to trigger a refresh action by performing the pull-to-refresh gesture.
	/// </summary>
	public partial class SfPullToRefresh
	{
		/// <summary>
		/// This method triggers on any touch interaction on <see cref="SfPullToRefresh"/>.
		/// </summary>
		/// <param name="e">Event args.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
		}

		/// <summary>
		/// Method configures all the touch related works.
		/// </summary>
		static void ConfigTouch()
		{
		}

		/// <summary>
		/// Gets the X and Y coordinates of the specified element based on the screen.
		/// </summary>
		/// <param name="native">The current element for which coordinates are requested.</param>
		static Point ChildLocationToScreen(object native)
		{
			return Point.Zero;
		}

		/// <summary>
		/// Gets the scroll offset of the specified child within a scroll view.
		/// </summary>
		/// <param name="scrollView">The current child for which the scroll offset is requested.</param>
		static double GetChildScrollOffset(object scrollView)
		{
			return 0;
		}
	}
}
