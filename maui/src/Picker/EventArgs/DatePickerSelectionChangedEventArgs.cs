namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Date picker selection changed event arguments.
    /// </summary>
    public class DatePickerSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new selected date value.
        /// </summary>
        public DateTime? NewValue { get; internal set; }

        /// <summary>
        /// Gets the previous selected date value.
        /// </summary>
        public DateTime? OldValue { get; internal set; }
    }
}