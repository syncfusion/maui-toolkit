namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Defines methods for handling tap gestures.
    /// </summary>
    public interface ITapGestureListener : IGestureListener
    {
        #region Methods

        /// <summary>
        /// Called when a tap gesture is detected.
        /// </summary>
        /// <param name="e">The event arguments containing tap gesture information.</param>
        void OnTap(TapEventArgs e);

        /// <summary>
        /// Called when a tap gesture is detected, providing the sender object.
        /// </summary>
        /// <param name="sender">The object that raised the tap event.</param>
        /// <param name="e">The event arguments containing tap gesture information.</param>
        void OnTap(object sender, TapEventArgs e) { }

        /// <summary>
        /// Determines whether the tap should be handled for the given view.
        /// </summary>
        /// <param name="view">The view on which the tap occurred.</param>
        void ShouldHandleTap(object view)
        {
            // The empty implementation in the interface allows for optional implementation in derived classes.
        }

        #endregion
    }
}
