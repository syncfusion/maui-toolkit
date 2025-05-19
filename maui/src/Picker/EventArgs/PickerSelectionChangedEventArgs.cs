namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Picker selection changed event arguments.
    /// </summary>
    public class PickerSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new selected index.
        /// </summary>
        public int NewValue { get; internal set; }

        /// <summary>
        /// Gets the previous selected index.
        /// </summary>
        public int OldValue { get; internal set; }

        /// <summary>
        /// Gets the selected column index.
        /// </summary>
        public int ColumnIndex { get; internal set; }
    }
}