namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Interface that holds the properties used to render calendar header.
    /// </summary>
    internal interface IHeader : IHeaderCommon, IInteractionInfo
    {
        /// <summary>
        /// Gets the settings for the calendar header.
        /// </summary>
        CalendarHeaderView HeaderView { get; }

        /// <summary>
        /// Gets the number of week in calendar month view.
        /// </summary>
        int NumberOfVisibleWeeks { get; }

        /// <summary>
        /// Gets the current visible dates of the calendar.
        /// </summary>
        List<DateTime> VisibleDates { get; }

        /// <summary>
        /// Gets a header template.
        /// </summary>
        DataTemplate HeaderTemplate { get; }

        /// <summary>
        /// Gets the first day of the week for the calendar view.
        /// </summary>
        DayOfWeek FirstDayOfWeek { get; }

        /// <summary>
        /// Triggers the animation to move next view of the calendar view.
        /// </summary>
        void AnimateMoveToNextView();

        /// <summary>
        /// Triggers the animation to move previous view of the calendar view.
        /// </summary>
        void AnimateMoveToPreviousView();
    }
}