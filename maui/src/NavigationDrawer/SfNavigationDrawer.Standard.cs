using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	public partial class SfNavigationDrawer
	{
		#region Private Methods

		/// <summary>
		/// This method triggers on any touch interaction on <see cref="SfNavigationDrawer"/>.
		/// </summary>
		/// <param name="e">Event args.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
		}

		#endregion
	}
}
