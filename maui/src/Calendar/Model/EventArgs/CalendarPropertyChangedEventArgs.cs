using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which contains events arguments for calendar property change including old value of the property.
    /// </summary>
    internal class CalendarPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        internal CalendarPropertyChangedEventArgs(string? propertyName)
            : base(propertyName)
        {
        }

        #endregion

        #region Internal Property

        /// <summary>
        /// Gets or sets the old vale of the property which is used to unwire events for nested class properties.
        /// </summary>
        internal object? OldValue { get; set; }

        #endregion
    }
}
