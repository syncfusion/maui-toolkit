using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Picker property changed event arguments.
    /// </summary>
    internal class PickerPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PickerPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        internal PickerPropertyChangedEventArgs(string? propertyName)
            : base(propertyName)
        {
        }

        /// <summary>
        /// Gets or sets the old vale of the property which is used to unwire events for nested class properties.
        /// </summary>
        internal object? OldValue { get; set; }
    }
}