namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds common properties that are used to in calendar views of month, year and header views.
    /// </summary>
    internal interface IHeaderCommon
    {
        /// <summary>
        /// Gets or sets the view type for the calendar.
        /// </summary>
        CalendarView View { get; set; }

        /// <summary>
        /// Gets a value indicating whether the view is in RTL flow direction or not.
        /// </summary>
        bool IsRTLLayout { get; }

        /// <summary>
        /// Gets the minimum visible date for the calendar.
        /// </summary>
        DateTime MinimumDate { get; }

        /// <summary>
        /// Gets the maximum visible date for the calendar.
        /// </summary>
        DateTime MaximumDate { get; }

        /// <summary>
        /// Gets the navigation direction for the calendar view.
        /// </summary>
        CalendarNavigationDirection NavigationDirection { get; }

        /// <summary>
        /// Gets a value indicating whether the navigation on year, decade and century cell interaction.
        /// </summary>
        bool AllowViewNavigation { get; }

        /// <summary>
        /// Gets the today highlight color.
        /// </summary>
        Brush TodayHighlightBrush { get; }

        /// <summary>
        /// Gets the calendar identifier.
        /// </summary>
        CalendarIdentifier Identifier { get; }

        /// <summary>
        /// Gets the button text color.
        /// </summary>
        Brush ButtonTextColor { get; }

        /// <summary>
        /// Gets the header hover color.
        /// </summary>
        Brush HeaderHoverColor { get; }
    }
}