namespace Syncfusion.Maui.Toolkit.TabView
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="SfTabView.TabItemTapped"/> event.
    /// </summary>
    public class TabItemTappedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the selected tab item.
        /// </summary>
        /// <value>
        /// Returns the selected tab item.
        /// </value>
        public SfTabItem? TabItem { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        /// <value>
        /// A boolean value indicating if the event should be canceled.
        /// </value>
        public bool Cancel { get; set; }

        #endregion
    }
}
