namespace Syncfusion.Maui.Toolkit.Accordion
{
	#region Enums

	/// <summary>
	/// Specifies the auto-scroll positions of the expanded <see cref="AccordionItem"/> in the <see cref="SfAccordion"/>.
	/// </summary>
	public enum AccordionAutoScrollPosition
    {
		/// <summary>
		/// Scrolls to ensure the accordion item is visible within the view. 
		/// If the item is already in view, no scrolling occurs. Otherwise, the item is scrolled into view entirely.
		/// </summary>
		MakeVisible,

		/// <summary>
		/// Scrolls to place the accordion item at the top of the view.
		/// </summary>
		Top,

		/// <summary>
		/// The accordion item does not scroll and remains in its current position.
		/// </summary>
		None,
    }

	/// <summary>
	/// Specifies the expand mode for the items in the <see cref="SfAccordion"/> control.
	/// </summary>
	public enum AccordionExpandMode
    {
		/// <summary>
		/// Allows only one item to be expanded at a time. 
		/// Expanding a new item collapses the previously expanded one. 
		/// At least one item must remain expanded, as collapsing all items is not allowed.
		/// </summary>
		Single,

		/// <summary>
		/// Allows only one item to be expanded at a time. 
		/// Allows for collapsing the currently expanded item.
		/// leaving all items in a collapsed state.
		/// </summary>
		SingleOrNone,

		/// <summary>
		/// Allows multiple items to be expanded simultaneously. 
		/// At least one item must be expanded at all times.
		/// </summary>
		Multiple,

		/// <summary>
		/// Allows multiple items to be expanded simultaneously, 
		/// and permits collapsing all items, leaving them in a collapsed state.
		/// </summary>
		MultipleOrNone,
    }

	#endregion
}
