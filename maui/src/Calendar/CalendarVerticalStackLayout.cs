namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the information for calendar elements like header, view header and looping panel in vertical stack layout and arranged in Calendar.
    /// </summary>
    internal class CalendarVerticalStackLayout : CalendarLayout
    {
        #region Fields

        /// <summary>
        /// Holds the calendar header height details and its value updated while header height property changed.
        /// </summary>
        double _headerHeight;

        /// <summary>
        /// Holds the calendar footer height details and its value updated while footer height property changed.
        /// </summary>
        double _footerHeight;

        /// <summary>
        /// The month view header height. Only for month view.
        /// </summary>
        double _monthViewHeaderHeight;

        /// <summary>
        /// Holds the show action button layout in calendar. Action button layout shows action buttons and today button.
        /// </summary>
        bool _showFooterLayout;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarVerticalStackLayout"/> class.
        /// </summary>
        /// <param name="headerHeight">The header height.</param>
        /// <param name="showFooterLayout">The show action layout value.</param>
        /// <param name="footerHeight">The footer height.</param>
        internal CalendarVerticalStackLayout(double headerHeight, bool showFooterLayout, double footerHeight)
        {
            _monthViewHeaderHeight = 0;
            _headerHeight = headerHeight;
            _footerHeight = footerHeight;
            _showFooterLayout = showFooterLayout;
            //// TODO: Child layouts get the parent flow direction hence while arranging child elements the framework automatically reverses the direction.
            //// In other platforms, child elements' flow direction is not set and always has left flow direction so we have to manually arrange child elements.
            //// The draw view is still needed to configure manually and not take the parent direction.
            //// Due to this inconsistent behavior in windows, set flow direction to LTR for the inner layout of the calendar, so we manually arrange and draw child elements for all the platforms as common.
            //// The draw view does not arrange based on the flow direction. https://github.com/dotnet/maui/issues/6978
            FlowDirection = Microsoft.Maui.FlowDirection.LeftToRight;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Update the header height value when the calendar header height changed.
        /// </summary>
        /// <param name="headerHeight">Updated calendar header height value.</param>
        internal void UpdateHeaderHeight(double headerHeight)
        {
            _headerHeight = headerHeight;

            //// In android platform some time's the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly. Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Update the footer height value when the calendar footer height changed.
        /// </summary>
        /// <param name="footerHeight">The footer height.</param>
        internal void UpdateFooterHeight(double footerHeight)
        {
            _footerHeight = footerHeight;
#if ANDROID
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Update show action button value when the calendar show action button property changed.
        /// </summary>
        /// <param name="isActionButtonLayout">The show action buttons value.</param>
        internal void UpdateActionButtonHeight(bool isActionButtonLayout)
        {
            if (_showFooterLayout == isActionButtonLayout)
            {
                return;
            }

            _showFooterLayout = isActionButtonLayout;
            //// In android platform some time's the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly. Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update the month view header height.
        /// </summary>
        /// <param name="viewHeaderHeight">The view header height.</param>
        internal void UpdateViewHeaderHeight(double viewHeaderHeight)
        {
            _monthViewHeaderHeight = viewHeaderHeight;

            //// In android platform some time's the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly. Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Updates the flow direction.
        /// </summary>
        internal void UpdateFlowDirection()
        {
            //// In android platform some time's InvalidateMeasure does not trigger while the calendar identifier is changed at run time. So the header view doesn't render properly.
            //// Hence calling measure and arrange directly without InvalidateMeasure.
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            double width = bounds.Width;
            double height = bounds.Height;
            //// bounds.Top and bounds.Left specified on arrange because iOS header icon not drawn correctly when set left and top as 0.
            double topPosition = bounds.Top;
            double footerButtonHeight = _showFooterLayout ? _footerHeight : 0;
            foreach (var child in Children)
            {
                if (child is HeaderLayout)
                {
                    // main header layout which contains date and navigation arrows
                    child.Arrange(new Rect(bounds.Left, topPosition, width, _headerHeight));
                }
                else if (child is MonthViewHeader)
                {
                    child.Arrange(new Rect(bounds.Left, topPosition + _headerHeight, width, _monthViewHeaderHeight));
                }
                else if (child is CustomSnapLayout)
                {
                    //// Swiping panel which holds view header and calendar views.
                    child.Arrange(new Rect(bounds.Left, topPosition + _headerHeight + _monthViewHeaderHeight, width, height - _headerHeight - _monthViewHeaderHeight - footerButtonHeight));
                }
                else if (child is FooterLayout)
                {
                    child.Arrange(new Rect(bounds.Left, topPosition + height - footerButtonHeight, width, footerButtonHeight));
                }
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            double footerButtonHeight = _showFooterLayout ? _footerHeight : 0;
            foreach (var child in Children)
            {
                if (child is HeaderLayout)
                {
                    // main header layout which contains date and navigation arrows.
                    child.Measure(widthConstraint, _headerHeight);
                }
                else if (child is MonthViewHeader)
                {
                    //// Month view header which holds month view header(Day of Week).
                    child.Measure(widthConstraint, _monthViewHeaderHeight);
                }
                else if (child is CustomSnapLayout)
                {
                    //// Swiping panel which holds view header and calendar views.
                    child.Measure(widthConstraint, heightConstraint - _headerHeight - _monthViewHeaderHeight - footerButtonHeight);
                }
                else if (child is FooterLayout)
                {
                    child.Measure(widthConstraint, footerButtonHeight);
                }
            }

            return new Size(widthConstraint, heightConstraint);
        }

        #endregion
    }
}