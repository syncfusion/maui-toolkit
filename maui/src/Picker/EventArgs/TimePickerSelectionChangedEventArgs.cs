namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Time picker selection changed event arguments.
    /// </summary>
    public class TimePickerSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new selected time value.
        /// </summary>
        public TimeSpan? NewValue { get; internal set; }

        /// <summary>
        /// Gets the previous selected time value.
        /// </summary>
        public TimeSpan? OldValue { get; internal set; }
    }
}