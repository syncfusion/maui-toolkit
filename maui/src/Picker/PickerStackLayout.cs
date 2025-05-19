namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents the picker stack layout class.
    /// </summary>
    internal class PickerStackLayout : SfView
    {
        #region Fields

        /// <summary>
        /// The picker info.
        /// </summary>
        readonly IPicker _pickerInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerStackLayout"/> class.
        /// </summary>
        /// <param name="pickerInfo">The picker info.</param>
        internal PickerStackLayout(IPicker pickerInfo)
        {
            _pickerInfo = pickerInfo;
            //// TODO: In windows, child layouts get the parent flow direction hence while arranging child elements the framework automatically reverses the direction.
            //// In other platforms, child elements' flow direction is not set and always has left flow direction so we have to manually arrange child elements.
            //// In the Windows platform, the draw view is still needed to configure manually and not take the parent direction.
            //// Due to this inconsistent behavior in windows, set flow direction to LTR for the inner layout of the calendar, so we manually arrange and draw child elements for all the platforms as common.
            //// In the Windows platform, the draw view does not arrange based on the flow direction. https://github.com/dotnet/maui/issues/6978
            FlowDirection = FlowDirection.LeftToRight;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Trigger to invalidate the view. it was triggered while changing the header or footer view height.
        /// </summary>
        internal void InvalidateView()
        {
            InvalidateMeasure();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get the valid height of header or footer view.
        /// </summary>
        /// <param name="height">The height of the footer or header view.</param>
        /// <returns>Returns the height.</returns>
        static double GetValidHeight(double height)
        {
            if (height > 0)
            {
                return height;
            }

            return 0;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>Returns the maximum size of the layout.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(widthConstraint) ? widthConstraint : 300;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 300;
            double headerHeight = GetValidHeight(_pickerInfo.HeaderView.Height);
            double footerHeight = GetValidHeight(_pickerInfo.FooterView.Height);
            foreach (var child in Children)
            {
                if (child is HeaderLayout)
                {
                    child.Measure(width, headerHeight);
                }
                else if (child is PickerContainer)
                {
                    child.Measure(width, height - footerHeight - headerHeight);
                }
                else if (child is FooterLayout)
                {
                    child.Measure(width, footerHeight);
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>Returns the layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            double width = bounds.Width;
            double topPosition = bounds.Top;
            double headerHeight = GetValidHeight(_pickerInfo.HeaderView.Height);
            double footerHeight = GetValidHeight(_pickerInfo.FooterView.Height);
            foreach (var child in Children)
            {
                if (child is HeaderLayout)
                {
                    child.Arrange(new Rect(bounds.Left, topPosition, width, headerHeight));
                }
                else if (child is PickerContainer)
                {
                    child.Arrange(new Rect(bounds.Left, topPosition + headerHeight, width, bounds.Height - headerHeight - footerHeight));
                }
                else if (child is FooterLayout)
                {
                    child.Arrange(new Rect(bounds.Left, topPosition + bounds.Height - footerHeight, width, footerHeight));
                }
            }

            return bounds.Size;
        }

        #endregion
    }
}