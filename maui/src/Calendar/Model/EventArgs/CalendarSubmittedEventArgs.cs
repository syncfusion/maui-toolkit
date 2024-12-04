namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to hold the submit button event details.
    /// </summary>
    public class CalendarSubmittedEventArgs : EventArgs
    {
        #region Property

        /// <summary>
        /// Gets the new selected date/dates/range value.
        /// </summary>
        public object? Value { get; internal set; }

        #endregion
    }
}
