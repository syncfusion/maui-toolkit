namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    using System.ComponentModel;

    /// <summary>
    /// Provides data for drawer opening and closing events that can be canceled.
    /// </summary>
    public sealed class DrawerCancelEventArgs(bool isPrimary) : CancelEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the event originated from the primary drawer.
        /// </summary>
        public bool IsPrimary { get; } = isPrimary;
    }
}