namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to hold the selection changed event details
    /// </summary>
    public class CalendarSelectionChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the new selected date/dates/range value.
        /// </summary>
        public object? NewValue { get; internal set; }

        /// <summary>
        ///  Gets the previous selected  date/dates/range value.
        /// </summary>
        public object? OldValue { get; internal set; }

        #endregion
    }
}
