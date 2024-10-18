namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Defines methods and properties for handling touch events in a view.
    /// </summary>
    public interface ITouchListener
    {
        #region Properties

        /// <summary>
        /// Gets the boolean value indicating to pass the touches on either the parent or child view.
        /// </summary>
        bool IsTouchHandled
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles touch events on the view.
        /// </summary>
        /// <param name="e">The event arguments containing information about the touch event.</param>
        void OnTouch(PointerEventArgs e);

        /// <summary>
        /// Serves event for the mouse wheel action on the view.
        /// </summary>
        /// <param name="e">The event arguments containing information about the scroll event.</param>
        void OnScrollWheel(ScrollEventArgs e)
        {
            // The empty implementation in the interface allows for optional implementation in derived classes.
        }

        #endregion
    }
}
