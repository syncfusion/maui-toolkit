namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to hold the tap interaction event details and it occurs when the user clicks or touch on the calendar elements.
    /// </summary>
    /// <remarks>
    /// The interacted date and element details when the tap action performed on the calendar element available in the <see cref="CalendarTappedEventArgs"/>.
    /// </remarks>
    public class CalendarTappedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the interacted element date value.
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Gets the interacted element value.
        /// </summary>
        public CalendarElement Element { get; internal set; }

        /// <summary>
        /// Gets the interacted element week number value.
        /// </summary>
        /// <remarks>This is applicable for the month view</remarks>
        public int WeekNumber { get; internal set; }

        #endregion
    }

    /// <summary>
    /// Represents a class which is used to hold the double tap interaction event details and it occurs when the user double tap inside the calendar elements.
    /// </summary>
    /// <remarks>
    /// The interacted date and element details when the double tap action performed on the calendar element available in the <see cref="CalendarDoubleTappedEventArgs"/>.
    /// </remarks>
    public class CalendarDoubleTappedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the interacted element date value.
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Gets the interacted element value.
        /// </summary>
        public CalendarElement Element { get; internal set; }

        /// <summary>
        /// Gets the interacted element week number value.
        /// </summary>
        /// <remarks>This is applicable for the month view</remarks>
        public int WeekNumber { get; internal set; }

        #endregion
    }

    /// <summary>
    /// Represents a class which is used to hold the long press interaction event details and it occurs when the user long press inside the calendar elements.
    /// </summary>
    /// <remarks>
    /// The interacted date and element details when the long press action performed on the calendar element available in the <see cref="CalendarLongPressedEventArgs"/>.
    /// </remarks>
    public class CalendarLongPressedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the interacted element date value.
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Gets the interacted element value.
        /// </summary>
        public CalendarElement Element { get; internal set; }

        /// <summary>
        /// Gets the interacted element week number value.
        /// </summary>
        /// <remarks>This is applicable for the month view</remarks>
        public int WeekNumber { get; internal set; }

        #endregion
    }
}