using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents event data for panning actions on a view.
    /// </summary>
    public class PanEventArgs
    {
        #region Fields

        private readonly Point _touchPoint;
        private readonly Point _translatePoint;
        private readonly GestureStatus _status;
        private readonly Point _velocity;
        Func<IElement?, Point?>? _getPosition;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes an instance of <see cref="PanEventArgs"/>
        /// </summary>
        /// <param name="status">The gesture status of the pan action.</param>
        /// <param name="touchPoint">The touch point where the pan action occurred.</param>
        /// <param name="translatePoint">The translate distance point of the pan action.</param>
        /// <param name="velocity">The velocity of the pan action.</param>
        public PanEventArgs(GestureStatus status, Point touchPoint, Point translatePoint, Point velocity)
        {
            _status = status;
            _touchPoint = touchPoint;
            _translatePoint = translatePoint;
            _velocity = velocity;
        }

        internal PanEventArgs(Func<IElement?, Point?>? position, GestureStatus status, Point touchPoint, Point translatePoint, Point velocity) : this(status, touchPoint, translatePoint, velocity)
        {
            _getPosition = position;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the gesture status of the pan action.
        /// </summary>
        public GestureStatus Status
        {
            get
            {
                return _status;
            }
        }

        /// <summary>
        /// Gets the translate distance point of the pan action.
        /// </summary>
        public Point TranslatePoint
        {
            get
            {
                return
                    _translatePoint;
            }
        }

        /// <summary>
        /// Gets the actual touch point of the pan action.
        /// </summary>
        public Point TouchPoint
        {
            get
            {
                return _touchPoint;
            }
        }

        /// <summary>
        /// Gets the velocity of the pan action.
        /// </summary>
        /// <remarks>
        /// Velocity values start from 0, 1000, 2000, etc., based on X and Y directions.
        /// While panning with less friction, the velocity values are in the range of 0 to 1000 based on X and Y directions.
        /// </remarks>
        public Point Velocity
        {
            get
            {
                return _velocity;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the touch position relative to the specified element.
        /// </summary>
        /// <param name="relativeTo">The element to which the position is relative.</param>
        /// <returns>A <see cref="Point"/> representing the relative position.</returns>
        public Point? GetPosition(Element? relativeTo) => _getPosition?.Invoke(relativeTo);

        #endregion
    }
}
