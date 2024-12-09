using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Semantics
{
	internal interface ISemanticsProvider
	{
		/// <summary>
		/// Return the semantics nodes for the view.
		/// </summary>
		/// <param name="width">The view width.</param>
		/// <param name="height">The view height.</param>
		/// <returns>The semantics nodes of the view.</returns>
		List<SemanticsNode>? GetSemanticsNodes(double width, double height);

		/// <summary>
		/// Used to scroll the view based on the node position while the view inside the scroll view.
		/// </summary>
		/// <param name="node">Current navigated semantics node.</param>
		void ScrollTo(SemanticsNode node);
	}
}