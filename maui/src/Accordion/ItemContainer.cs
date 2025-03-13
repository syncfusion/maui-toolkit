namespace Syncfusion.Maui.Toolkit.Accordion
{
	/// <summary>
	/// Represents a container for items in the <see cref="SfAccordion"/> control.
	/// </summary>
	internal partial class ItemContainer : SfView
    {
        #region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemContainer"/> class.
		/// </summary>
		/// <param name="accordion">The instance of <see cref="SfAccordion"/> to be associated with this container.</param>
		internal ItemContainer(SfAccordion accordion)
        {
            _accordion = accordion;
        }

		#endregion

		#region Private Properties

		/// <summary>
		/// Gets or sets the instance of <see cref="SfAccordion"/>.
		/// </summary>
		SfAccordion _accordion { get; set; }

		#endregion

		#region Override Methods

		/// <summary>
		/// Measures the dimensions required for the item container to display its children.
		/// </summary>
		/// <param name="widthConstraint">The maximum width the container can occupy.</param>
		/// <param name="heightConstraint">The maximum height the container can occupy.</param>
		/// <returns>A <see cref="Size"/> representing the desired size of the container.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			double measuredHeight = 0;
			if (Children.Count > 0)
			{
				foreach (var accordionItem in Children)
				{
					var measure = (accordionItem as IView).Measure(widthConstraint, double.PositiveInfinity);
					measuredHeight += measure.Height;
				}
			}

			return new Size(widthConstraint, measuredHeight);
		}

		/// <summary>
		/// Arranges the children elements within the item container according to the provided bounds.
		/// </summary>
		/// <param name="bounds">A <see cref="Rect"/> that represents the area available for arranging children.</param>
		/// <returns>A <see cref="Size"/> representing the arranged size of the container.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			double width = bounds.Width;
			double height = 0;
			if (Children.Count > 0)
			{
				foreach (var accordionItem in Children)
				{
					var arrange = (accordionItem as IView).Arrange(new Rect(0, height, width, accordionItem.DesiredSize.Height));
					height += arrange.Height;
				}
			}

			return new Size(width, height);
		}

		/// <summary>
		/// Overrides the base OnSizeAllocated method to handle additional actions when the size is allocated.
		/// </summary>
		/// <param name="width">The allocated width for the container.</param>
		/// <param name="height">The allocated height for the container.</param>
		protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // To update accordion, scrollview height when loaded accordion inside StackLayout, ScrollView,etc.
            if (height > 0)
            {
                _accordion.InvalidateForceLayout();
            }
        }

        #endregion
    }
}
