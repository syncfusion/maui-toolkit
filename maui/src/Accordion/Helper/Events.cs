using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Accordion
{
	/// <summary>
	/// Provides data for the <see cref="SfAccordion.Expanding"/> and <see cref="SfAccordion.Collapsing"/> events of the <see cref="SfAccordion"/> control.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="ExpandingAndCollapsingEventArgs"/> class and it provides the respectable event data.
	/// </remarks>
	/// <param name="index">The currently expanding or collapsing accordion item index.</param>
	public class ExpandingAndCollapsingEventArgs(int index) : CancelEventArgs
	{
		#region Properties

		/// <summary>
		/// Gets the index of the <see cref="AccordionItem"/> that is being expanded or collapsed.
		/// </summary>
		public int Index { get; internal set; } = index;

		#endregion
	}

	/// <summary>
	/// Provides data for the <see cref="SfAccordion.Expanded"/> and <see cref="SfAccordion.Collapsed"/> events of the <see cref="SfAccordion"/> control.
	/// These events occur after an accordion item has been expanded or collapsed.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="ExpandedAndCollapsedEventArgs"/> class and it provides the respectable event data.
	/// </remarks>
	/// <param name="index">The currently expanded or collapsed accordion item index.</param>
	public class ExpandedAndCollapsedEventArgs(int index) : EventArgs
    {
		#region Properties

		/// <summary>
		/// Gets the index of the expanded or collapsed <see cref="AccordionItem"/> in the <see cref="SfAccordion"/>.
		/// </summary>
		public int Index { get; internal set; } = index;

		#endregion
	}
}
