using System.Runtime.Versioning;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents the arguments for a pointer event.
    /// </summary>
    public class PointerEventArgs
    {
        #region Fields

        readonly Func<IElement?, Point?>? _getPosition;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the pointer event.</param>
        /// <param name="action">The action performed in the pointer event.</param>
        /// <param name="touchPoint">The point where the touch occurred.</param>
        public PointerEventArgs(long id, PointerActions action, Point touchPoint)
        {
            Id = id;
            Action = action;
            TouchPoint = touchPoint;
        }

        internal PointerEventArgs(Func<IElement?, Point?>? position, long id, PointerActions action, Point touchPoint) : this(id, action, touchPoint)
        {
            _getPosition = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the pointer event.</param>
        /// <param name="deviceType">The device type of the pointer.</param>
        /// <param name="action">The action performed in the pointer event.</param>
        /// <param name="touchPoint">The point where the touch occurred.</param>
        public PointerEventArgs(long id, PointerActions action, PointerDeviceType deviceType, Point touchPoint)
        {
            Id = id;
            Action = action;
            TouchPoint = touchPoint;
            PointerDeviceType = deviceType;
        }

        internal PointerEventArgs(Func<IElement?, Point?>? position, long id, PointerActions action, PointerDeviceType deviceType, Point touchPoint) : this(id, action, deviceType, touchPoint)
        {
            _getPosition = position;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for this pointer event. 
        /// </summary>
        public long Id { private set; get; }

        /// <summary>
        /// Gets the action performed in this pointer event.
        /// </summary>
        public PointerActions Action { private set; get; }

        /// <summary>
        /// Gets the actual touch point of this pointer event.
        /// </summary>
        public Point TouchPoint { private set; get; }

        /// <summary>
        /// Gets the device type of the pointer.
        /// </summary> 
        public PointerDeviceType PointerDeviceType { internal set; get; }
#if MACCATALYST
            = PointerDeviceType.Mouse;
#else
            = PointerDeviceType.Touch;
#endif

        /// <summary>
        /// Gets a value indicating whether the left mouse button is pressed.
        /// </summary>
        public bool IsLeftButtonPressed { internal set; get; } = false;

        /// <summary>
        /// Gets a value indicating whether the right mouse button is pressed.
        /// </summary>
        /// <remarks>
        /// This property is not supported on Mac Catalyst.
        /// </remarks>
        [UnsupportedOSPlatform("MACCATALYST")]
        public bool IsRightButtonPressed { internal set; get; } = false;

        #endregion

        #region Methods

        /// <summary>
        /// Obtains the touch position relative to the given element.
        /// </summary>
        /// <param name="relativeTo">The element to which the position should be relative</param>
        /// <returns>A <see cref="Point"/> representing the touch position relative to the given element.
        /// </returns>
        public Point? GetPosition(Element? relativeTo) => _getPosition?.Invoke(relativeTo);

        #endregion
    }
}
