using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides event data for right-tap actions on a view.
    /// </summary>
    public class RightTapEventArgs
    {
        #region Fields

        private readonly Point _tapPoint;
        private readonly PointerDeviceType _pointerDeviceType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the point where the right-tap occurred.
        /// </summary>
        /// <value>A <see cref="Point"/> representing the location of the right-tap.</value>
        public Point TapPoint
        {
            get
            {
                return _tapPoint;
            }
        }

        /// <summary>
        /// Gets the type of pointer device that initiated the right-tap.
        /// </summary>
        /// <value>A <see cref="PointerDeviceType"/> indicating the type of pointer device.</value>
        public PointerDeviceType PointerDeviceType
        {
            get
            {
                return _pointerDeviceType;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RightTapEventArgs"/> class.
        /// </summary>
        /// <param name="tapPoint">The point where the right-tap occurred.</param>
        /// <param name="pointerDeviceType">The type of pointer device that initiated the right-tap.</param>
        public RightTapEventArgs(Point tapPoint, PointerDeviceType pointerDeviceType)
        {
            _tapPoint = tapPoint;
            _pointerDeviceType = pointerDeviceType;
        }

        #endregion

    }
}
