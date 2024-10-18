using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// This class serves as an event data for the mouse wheel action on the view.
    /// </summary>
    public class ScrollEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for the pointer (mouse or touch) that initiated the scroll event.
        /// </summary>
        public long PointerID { private set; get; }

        /// <summary>
        /// Gets the delta value of the scroll action.
        /// </summary>
        public double ScrollDelta { private set; get; }

        /// <summary>
        /// Gets the coordinates of the touch pointer at the time of the scroll event.
        /// </summary>
        public Point TouchPoint { private set; get; }

        /// <summary>
        /// Gets or sets a value that marks the routed event as handled, and prevents most handlers along the event route from handling the same event again. 
        /// </summary>
        /// <remarks>
        /// It is applicable only for routed events which are supported in Windows.
        /// </remarks>
        internal bool Handled { set; get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollEventArgs"/> class.
        /// </summary>
        public ScrollEventArgs(long id, Point origin, double direction)
        {
            PointerID = id;
            TouchPoint = origin;
            ScrollDelta = direction;
        }

        #endregion
    }
}
