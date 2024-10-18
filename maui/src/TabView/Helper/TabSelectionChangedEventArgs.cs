namespace Syncfusion.Maui.Toolkit.TabView
{
    using System;

    #region Properties

    /// <summary>
    /// Provides data for the <see cref="SfTabView.SelectionChanged"/> event.
    /// </summary>
    public class TabSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of previously selected tab item.
        /// </summary>
        /// <value>
        /// Returns the index of previously selected tab item.
        /// </value>
        public int OldIndex { get; internal set; }

        /// <summary>
        /// Gets the index of currently selected tab item.
        /// </summary>
        /// <value>
        /// Returns the index of currently selected tab item.
        /// </value>
        public int NewIndex { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTabView.SelectionChanged"/> event is handled.
        /// </summary>
        /// <value>
        /// Returns true if the <see cref="SfTabView.SelectionChanged"/> event has been handled, otherwise false.
        /// </value>
        public bool Handled { get; set; }
    }

    #endregion
}