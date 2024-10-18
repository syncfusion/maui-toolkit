namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// This interface defines a listener for panning actions on a view.
    /// </summary>
    public interface IPanGestureListener : IGestureListener
    {
        #region Methods

        /// <summary>
        /// Called when a panning interaction occurs on the associated view.
        /// </summary>
        /// <param name="e">The event arguments containing panning details.</param>
        void OnPan(PanEventArgs e);

        #endregion
    }
}
