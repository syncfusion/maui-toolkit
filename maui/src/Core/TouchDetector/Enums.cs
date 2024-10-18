
namespace Syncfusion.Maui.Toolkit.Internals
{
    #region Pointer Actions

    /// <summary>
    /// Represents the various actions that can be performed by a pointer device.
    /// </summary>
    public enum PointerActions
    {
        /// <summary>
        /// Indicates that the pointer is pressed.
        /// </summary>
        Pressed,
        /// <summary>
        /// Indicates that the pointer has moved.
        /// </summary>
        Moved,
        /// <summary>
        /// Indicates that the pointer has entered a specific area.
        /// </summary>
        Entered,
        /// <summary>
        /// Indicates that the pointer is released.
        /// </summary>
        Released,
        /// <summary>
        /// Indicates that the pointer action is cancelled.
        /// </summary>
        Cancelled,
        /// <summary>
        /// Indicates that the pointer has exited a specific area.
        /// </summary>
        Exited
    }

    #endregion

    #region Pointer Device Type

    /// <summary>
    /// Specifies the type of pointer device used for input.
    /// </summary>
    public enum PointerDeviceType
    {
        /// <summary>
        /// Represents a touch-based input device.
        /// </summary>
        Touch,
        /// <summary>
        /// Represents a mouse input device.
        /// </summary>
        Mouse,
        /// <summary>
        /// Represents a stylus input device, typically used with drawing tablets or certain touchscreens.
        /// </summary>
        Stylus
    }

    #endregion
}