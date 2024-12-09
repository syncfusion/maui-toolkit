namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Represents event data for pinch gestures on a view.
	/// </summary>
	public class PinchEventArgs
	{
		#region Fields

		private readonly Point _touchPoint;
		private readonly double _angle;
		private readonly float _scale;
		private readonly GestureStatus _status;
		readonly Func<IElement?, Point?>? _getPosition;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the status of the pinch gesture.
		/// </summary>
		public GestureStatus Status
		{
			get
			{
				return _status;
			}
		}

		/// <summary>
		/// Gets the scale value of the pinch gesture.
		/// </summary>
		public float Scale
		{
			get
			{
				return _scale;
			}
		}

		/// <summary>
		/// Gets the touch point of the pinch gesture.
		/// </summary>
		public Point TouchPoint
		{
			get
			{
				return _touchPoint;
			}
		}

		/// <summary>
		/// Gets the angle of the pinch gesture.
		/// </summary>
		public double Angle
		{
			get
			{
				return _angle;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PinchEventArgs"/> class.
		/// </summary>
		/// <param name="status">The status of the gesture.</param>
		/// <param name="touchPoint">The point where the touch occurred.</param>
		/// <param name="angle">The angle of the pinch gesture.</param>
		/// <param name="scale">The scale factor of the pinch gesture.</param>
		public PinchEventArgs(GestureStatus status, Point touchPoint, double angle, float scale)
		{
			_status = status;
			_touchPoint = touchPoint;
			_scale = scale;
			_angle = angle;
		}

		internal PinchEventArgs(Func<IElement?, Point?>? position, GestureStatus status, Point touchPoint, double angle, float scale) : this(status, touchPoint, angle, scale)
		{
			_getPosition = position;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Obtains the touch position relative to the given element.
		/// </summary>
		/// <param name="relativeTo">The element to which the touch position is relative.</param>
		/// <returns>A <see cref="Point"/> representing the relative touch position.</returns>
		public Point? GetPosition(Element? relativeTo) => _getPosition?.Invoke(relativeTo);

		#endregion

	}
}