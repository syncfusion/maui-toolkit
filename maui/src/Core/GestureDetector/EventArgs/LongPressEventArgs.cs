using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents the event data for a long press action on a view.
    /// </summary>
    public class LongPressEventArgs
    {
        #region Fields

        private readonly Point _touchPoint;
        Func<IElement?, Point?>? _getPosition;
#if IOS || MACCATALYST
        private readonly GestureStatus _status;
#endif
        #endregion

        #region Properties

        /// <summary>
        /// Gets the actual touch point on long press.
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> representing the touch point where the long press occurred.
        /// </value>
        public Point TouchPoint
        {
            get
            {
                return _touchPoint;
            }
        }

#if IOS || MACCATALYST
        internal GestureStatus Status { get { return _status; } }
#endif

        #endregion

        #region Constructor

#if IOS || MACCATALYST
        /// <summary>
        /// Initializes an instance of <see cref="LongPressEventArgs"/>.
        /// </summary>
        /// <param name="touchPoint">The touch point where the long press occurred.</param>
        /// <param name="status">The status of the gesture (only for iOS or MacCatalyst).</param>
        public LongPressEventArgs(Point touchPoint, GestureStatus status)
        {
            _touchPoint = touchPoint;
            _status = status;
        }
#else
        /// <summary>
        /// Initializes an instance of <see cref="LongPressEventArgs"/>.
        /// </summary>
        /// <param name="touchPoint">The touch point where the long press occurred.</param>
        public LongPressEventArgs(Point touchPoint)
        {
            _touchPoint = touchPoint;
        }
#endif

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the touch position relative to the specified element.
        /// </summary>
        /// <param name="relativeTo">The element to which the touch position should be relative</param>
        /// <returns>A <see cref="Point"/> representing the relative touch position.</returns>
        public Point? GetPosition(Element? relativeTo) => _getPosition?.Invoke(relativeTo);

#if IOS || MACCATALYST
        internal LongPressEventArgs(Func<IElement?, Point?>? position, Point touchPoint, GestureStatus status) : this(touchPoint, status)
        {
            _getPosition = position;
        }
#else
        internal LongPressEventArgs(Func<IElement?, Point?>? position, Point touchPoint) : this(touchPoint)
        {
            _getPosition = position;
        }
#endif

        #endregion

    }
}
