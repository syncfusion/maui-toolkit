namespace Syncfusion.Maui.Toolkit.OtpInput
{
    /// <summary>
    /// Provides event data for the <see cref="SfOtpInput.ValueChanged"/> event.
    /// </summary>
    public class OtpInputValueChangedEventArgs
    {
        /// <summary>
        /// Represents OtpInput’s value changed event arguments.
        /// </summary>
        /// <remarks>
        /// It contains information like old value, and new value.
        /// </remarks>
        internal OtpInputValueChangedEventArgs(string? newValue, string? oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// Gets or sets a new value when the OtpInput’s value is changed.
        /// </summary>
        public string? NewValue { get; }

        /// <summary>
        /// Gets or sets an old value when the OtpInput’s value is changed.
        /// </summary>
        public string? OldValue { get; }
    }

}
