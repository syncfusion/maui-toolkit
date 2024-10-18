using System;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Chips
{
    /// <summary>
    /// Represents chip group selection changed event arguments.
    /// </summary>
    /// <remarks>
    /// It contains information about selected chip and unselected chip.
    /// </remarks>
    public class SelectionChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the value of the currently selected chip.
        /// </summary>
        public object? AddedItem { get; internal set; }

        /// <summary>
        /// Gets the value of the currently unselected chip.
        /// </summary>
        public object? RemovedItem { get; internal set; }

        #endregion
    }

    /// <summary>
    /// Represents ChipGroup <see cref=" SelectionChangingEventArgs"/>  arguments.
    /// </summary>
    /// <remarks>
    /// It contains information about selected chip and unselected chip.
    /// </remarks>
    public class SelectionChangingEventArgs : CancelEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the value of the currently added item.
        /// </summary>
        public object? AddedItem { get; internal set; }

        /// <summary>
        /// Gets the value of the currently removed item.
        /// </summary>
        public object? RemovedItem { get; internal set; }

        #endregion
    }
}