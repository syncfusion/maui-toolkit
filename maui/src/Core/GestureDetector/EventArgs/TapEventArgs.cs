namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides event data for the tap action on the view.
    /// </summary>
    public class TapEventArgs
    {
        #region Fields

        private readonly int _noOfTapCounts;
        private readonly Point _tapPoint;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the point where the tap occurred.
        /// </summary>
        public Point TapPoint
        {
            get
            {
                return _tapPoint;
            }
        }

        /// <summary>
        /// Gets the number of taps that occurred.
        /// </summary>
        public int TapCount
        {
            get
            {
                return _noOfTapCounts;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TapEventArgs"/> class.
        /// </summary>
        /// <param name="touchPoint">The point where the tap occurred.</param>
        /// <param name="tapCount">The number of taps that occurred.</param>
        public TapEventArgs(Point touchPoint, int tapCount)
        {
            _tapPoint = touchPoint;
            _noOfTapCounts = tapCount;
        }

        #endregion

    }
}
