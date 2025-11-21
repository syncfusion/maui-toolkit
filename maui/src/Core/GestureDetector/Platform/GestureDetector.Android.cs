using Android.Views;
using AGestureDetector = Android.Views.GestureDetector;
using GestureStatus = Microsoft.Maui.GestureStatus;
using Microsoft.Maui.Platform;
using MauiView = Microsoft.Maui.Controls.View;
using View = Android.Views.View;
using Android.Runtime;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Handles gesture detection for Android platform.
	/// </summary>
	public partial class GestureDetector
	{
		#region Fields

		ScaleListener? _scaleListener;
		ScaleGestureDetector? _scaleGestureDetector;
		ScrollListener? _scrollListener;
		AGestureDetector? _scrollGestureDetector;
		double _pinchAngle = double.NaN;

		#endregion

		#region Internal Methods

		/// <summary>
		/// Unsubscribes from native gesture events and disposes of related resources.
		/// </summary>
		internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
		{
			if (handler != null)
			{
				if (handler.PlatformView is View nativeView)
				{
					nativeView.Touch -= PlatformView_Touch;
					_scaleListener?.Dispose();
					_scaleGestureDetector?.Dispose();

					_scrollListener?.Dispose();
					_scrollGestureDetector?.Dispose();
				}
			}
		}

		/// <summary>
		/// Subscribes to native gesture events for the given MAUI view.
		/// </summary>
		internal void SubscribeNativeGestureEvents(MauiView? mauiView)
		{
			if (mauiView != null)
			{
				var handler = mauiView.Handler;

				if (handler?.PlatformView is View nativeView)
				{
					CreateScaleGestureDetectorIfNeeded(nativeView);
					CreateScrollGestureDetectorIfNeeded(nativeView);

					nativeView.Touch += PlatformView_Touch;
				}
			}
		}

		/// <summary>
		/// Creates native listeners for scale and scroll gestures.
		/// </summary>
		internal void CreateNativeListener()
		{
			if (MauiView != null)
			{
				var handler = MauiView.Handler;
				View? nativeView = handler?.PlatformView as View;

				CreateScaleListenerIfNeeded(nativeView);
				CreateScrollListenerIfNeeded(nativeView);
			}
		}

		#endregion

		#region Private Methods

		void CreateScaleGestureDetectorIfNeeded(View nativeView)
		{
			if (_pinchGestureListeners?.Count > 0 && nativeView.Context != null)
			{
				_scaleListener = new ScaleListener(this);
				_scaleGestureDetector = new ScaleGestureDetector(nativeView.Context, _scaleListener);
			}
		}

		void CreateScrollGestureDetectorIfNeeded(View nativeView)
		{
			if (_panGestureListeners?.Count > 0 ||
				_longPressGestureListeners?.Count > 0 ||
				_tapGestureListeners?.Count > 0 ||
				_doubleTapGestureListeners?.Count > 0)
			{
				_scrollListener = new ScrollListener(this);
				_scrollGestureDetector = new AGestureDetector(nativeView.Context, _scrollListener);

				if (_longPressGestureListeners == null || _longPressGestureListeners.Count <= 0)
				{
					_scrollGestureDetector.IsLongpressEnabled = false;
				}
			}
		}

		void CreateScaleListenerIfNeeded(View? nativeView)
		{
			if (nativeView != null && nativeView.Context != null)
			{
				if (_scaleListener == null && _pinchGestureListeners?.Count > 0)
				{
					_scaleListener = new ScaleListener(this);
					_scaleGestureDetector = new ScaleGestureDetector(nativeView.Context, _scaleListener);
				}
			}
		}

		void CreateScrollListenerIfNeeded(View? nativeView)
		{
			if (nativeView != null && nativeView.Context != null)
			{
				if (_scrollListener == null &&
				(_panGestureListeners?.Count > 0 ||
				_longPressGestureListeners?.Count > 0 ||
				_tapGestureListeners?.Count > 0 ||
				_doubleTapGestureListeners?.Count > 0))
				{
					_scrollListener = new ScrollListener(this);
					_scrollGestureDetector = new AGestureDetector(nativeView.Context, _scrollListener);

					if (_longPressGestureListeners == null || _longPressGestureListeners.Count <= 0)
					{
						_scrollGestureDetector.IsLongpressEnabled = false;
					}
				}
			}
		}

		/// <summary>
		/// Handles touch events for the platform view.
		/// </summary>
		void PlatformView_Touch(object? sender, View.TouchEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent || e.Event == null || sender is not View nativeView)
			{
				return;
			}
			var motionEvent = e.Event;
			HandleMotionEvent(nativeView, motionEvent);
		}

		void HandleMotionEvent(View nativeView, MotionEvent motionEvent)
		{
			int pointer1Index = motionEvent.FindPointerIndex(0);
			int pointer2Index = motionEvent.FindPointerIndex(1);

			Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
			Point point1 = Point.Zero;

			// Handle single touch.
			if (pointer1Index != -1)
			{
				point1 = new Point(motionEvent.GetX(pointer1Index), motionEvent.GetY(pointer1Index));
				point1 = new Point(fromPixels(point1.X), fromPixels(point1.Y));
				bool isHandled = false;
				if (_tapGestureListeners?.Count > 0 &&
					_tapGestureListeners[0].IsTouchHandled ||
					_doubleTapGestureListeners?.Count > 0 &&
					_doubleTapGestureListeners[0].IsTouchHandled ||
					_longPressGestureListeners?.Count > 0 &&
					_longPressGestureListeners[0].IsTouchHandled ||
					_pinchGestureListeners?.Count > 0 &&
					_pinchGestureListeners[0].IsTouchHandled ||
					_panGestureListeners?.Count > 0 &&
					_panGestureListeners[0].IsTouchHandled)
				{
					isHandled = true;
				}

				var handled = isHandled ||
					motionEvent.Action == MotionEventActions.Pointer2Down ||
					motionEvent.PointerCount > 1;

				nativeView.ParentForAccessibility?.RequestDisallowInterceptTouchEvent(handled);
			}

			// Handle multi-touch.
			if (pointer1Index != -1 && pointer2Index != -1)
			{
				HandleMultiTouch(motionEvent, pointer1Index, pointer2Index, fromPixels, point1);
			}

			HandleScrollEnd(motionEvent, point1);
			HandleScaleEnd(motionEvent);
		}

#pragma warning disable IDE0060 // Remove unused parameter
		void HandleMultiTouch(MotionEvent motionEvent, int pointer1Index, int pointer2Index, Func<double, double> fromPixels, Point point1)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			Point point2 = new Point(motionEvent.GetX(pointer2Index), motionEvent.GetY(pointer2Index));
			point2 = new Point(fromPixels(point2.X), fromPixels(point2.Y));
			_pinchAngle = MathUtils.GetAngle(point1.X, point2.X, point1.Y, point2.Y);
			if (_scaleListener != null)
			{
				_scaleListener._motionEvent = motionEvent;
			}
			_scaleGestureDetector?.OnTouchEvent(motionEvent);
		}

		void HandleScrollEnd(MotionEvent motionEvent, Point point1)
		{
			_scrollGestureDetector?.OnTouchEvent(motionEvent);
			if (_scrollListener != null && motionEvent?.Action == MotionEventActions.Up)
			{
				_scrollListener.EndScrolling(motionEvent, point1);
			}
		}

		void HandleScaleEnd(MotionEvent? motionEvent)
		{
			if (_scaleListener != null &&
				(motionEvent?.Action == MotionEventActions.Up ||
				motionEvent?.Action == MotionEventActions.Cancel))
			{
				_scaleListener._motionEvent = null;
			}
		}

		/// <summary>
		/// Gets the current pinch angle.
		/// </summary>
		double GetPinchAngle()
		{
			return _pinchAngle;
		}
		#endregion

		#region Scale Listener

		/// <summary>
		/// Listens for scale gestures and notifies the associated GestureDetector.
		/// </summary>
		class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
		{
			#region Fields

			WeakReference<GestureDetector>? _detector;
			internal MotionEvent? _motionEvent;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="ScaleListener"/> class.
			/// </summary>
			/// <param name="gestureDetector">The GestureDetector to notify of scale events.</param>
			public ScaleListener(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;
			}

			#endregion

			#region Properties

			GestureDetector? GestureDetector
			{
				get => _detector != null && _detector.TryGetTarget(out var v) ? v : null;
				set => _detector = value == null ? null : new(value);
			}

			#endregion

			#region Override Methods

			/// <summary>
			/// Called when a scale gesture is in progress.
			/// </summary>
			/// <param name="detector">The detector reporting the event.</param>
			public override bool OnScale(ScaleGestureDetector? detector)
			{
				if (detector != null)
				{
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					GestureDetector?.OnPinch((relativeTo) => TouchDetector.CalculatePosition(_motionEvent, GestureDetector.MauiView, relativeTo), GestureStatus.Running, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), GestureDetector.GetPinchAngle(), detector.ScaleFactor);
				}

				return true;
			}

			/// <summary>
			/// Called when a scale gesture begins.
			/// </summary>
			/// <param name="detector">The detector reporting the event.</param>
			public override bool OnScaleBegin(ScaleGestureDetector? detector)
			{
				if (detector != null)
				{
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					GestureDetector?.OnPinch((relativeTo) => TouchDetector.CalculatePosition(_motionEvent, GestureDetector.MauiView, relativeTo), GestureStatus.Started, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), GestureDetector.GetPinchAngle(), detector.ScaleFactor);
				}

				return true;
			}

			/// <summary>
			/// Called when a scale gesture ends.
			/// </summary>
			/// <param name="detector">The detector reporting the event.</param>
			public override void OnScaleEnd(ScaleGestureDetector? detector)
			{
				if (detector != null)
				{
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					GestureDetector?.OnPinch((relativeTo) => TouchDetector.CalculatePosition(_motionEvent, GestureDetector.MauiView, relativeTo), GestureStatus.Completed, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), GestureDetector.GetPinchAngle(), detector.ScaleFactor);
				}
			}

			/// <summary>
			/// Releases the unmanaged resources used by the ScaleListener and optionally releases the managed resources.
			/// </summary>
			protected override void Dispose(bool disposing)
			{
				_motionEvent = null;
				base.Dispose(disposing);
			}

			#endregion
		}

		#endregion

		#region Scroll Listener

		/// <summary>
		/// Listens for scroll and tap gestures and notifies the associated GestureDetector.
		/// </summary>
		class ScrollListener : AGestureDetector.SimpleOnGestureListener
		{
			#region Fields

			bool _isScrolling;
			Point _scrollVelocity;
			WeakReference<GestureDetector>? _detector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="ScrollListener"/> class.
			/// </summary>
			/// <param name="gestureDetector">The GestureDetector to notify of scroll and tap events.</param>
			public ScrollListener(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;
			}

			/// <summary>
			/// Initializes a new <see cref="ScrollListener"/> bound to an existing JNI peer,
			/// specifying how the native handle ownership is managed.
			/// </summary>
			/// <param name="handle">A pointer to the native JNI object.</param>
			/// <param name="transfer">
			/// The ownership semantics for <paramref name="handle"/> (e.g., transfer or retain).
			/// </param>
			protected ScrollListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
			{
			}

			#endregion

			#region Properties

			GestureDetector? GestureDetector
			{
				get => _detector != null && _detector.TryGetTarget(out var v) ? v : null;
				set => _detector = value == null ? null : new(value);
			}

			#endregion

			#region Override Methods

			/// <summary>
			/// Called when a double-tap occurs.
			/// </summary>
			public override bool OnDoubleTap(MotionEvent? e)
			{
				if (e != null)
				{
					Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					point = new Point(fromPixels(point.X), fromPixels(point.Y));

					GestureDetector?.OnTapped(point, 2);
					return true;
				}

				return false;
			}

			/// <summary>
			/// Called when a long press occurs.
			/// </summary>
			public override void OnLongPress(MotionEvent? e)
			{
				if (e != null)
				{
					Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					point = new Point(fromPixels(point.X), fromPixels(point.Y));

					GestureDetector?.OnLongPress((relativeTo) => TouchDetector.CalculatePosition(e, GestureDetector.MauiView, relativeTo), point);
					base.OnLongPress(e);
				}
			}

			/// <summary>
			/// Called when a single-tap has been confirmed.
			/// </summary>
			public override bool OnSingleTapConfirmed(MotionEvent? e)
			{
				if (e != null)
				{
					if (GestureDetector != null && GestureDetector._doubleTapGestureListeners != null &&
						GestureDetector._doubleTapGestureListeners.Count > 0 &&
						!GestureDetector._doubleTapGestureListeners[0].IsRequiredSingleTapGestureRecognizerToFail)
					{
						Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
						Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
						point = new Point(fromPixels(point.X), fromPixels(point.Y));
						GestureDetector?.OnTapped(point, 1);
						return true;
					}
				}
				return false;
			}

			/// <summary>
			/// Called when a single-tap up occurs.
			/// </summary>
			public override bool OnSingleTapUp(MotionEvent? e)
			{
				if (e != null)
				{
					if (GestureDetector != null && GestureDetector._doubleTapGestureListeners != null &&
						GestureDetector._doubleTapGestureListeners.Count > 0 &&
						!GestureDetector._doubleTapGestureListeners[0].IsRequiredSingleTapGestureRecognizerToFail)
					{
						return false;
					}

					Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					point = new Point(fromPixels(point.X), fromPixels(point.Y));
					GestureDetector?.OnTapped(point, 1);
					return true;
				}

				return false;
			}

			/// <summary>
			/// Called when a scroll occurs.
			/// </summary>
			public override bool OnScroll(MotionEvent? e1, MotionEvent? e2, float distanceX, float distanceY)
			{
				if (e1 != null && e2 != null && e2.PointerCount == 1)
				{
					Point point = new Point(e2.GetX(e2.ActionIndex), e2.GetY(e2.ActionIndex));
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					point = new Point(fromPixels(point.X), fromPixels(point.Y));
					if (!_isScrolling)
					{
						GestureDetector?.OnScroll((relativeTo) => TouchDetector.CalculatePosition(e2, GestureDetector.MauiView, relativeTo), GestureStatus.Started, point, Point.Zero, Point.Zero);
						_isScrolling = true;
					}

					GestureDetector?.OnScroll((relativeTo) => TouchDetector.CalculatePosition(e2, GestureDetector.MauiView, relativeTo), GestureStatus.Running, point, new Point(fromPixels(distanceX), fromPixels(distanceY)), Point.Zero);
				}
				return true;
			}

			/// <summary>
			/// Called when a fling occurs.
			/// </summary>
			public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
			{
				if (e1 != null && e2 != null &&
					e2.PointerCount == 1 &&
					GestureDetector != null &&
					GestureDetector._panGestureListeners != null &&
					GestureDetector._panGestureListeners.Count > 0)
				{
					Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
					_scrollVelocity = new Point(fromPixels(velocityX), fromPixels(velocityY));
				}

				return true;
			}

			#endregion

			#region Internal Methods

			/// <summary>
			/// Ends the scrolling gesture.
			/// </summary>
			internal void EndScrolling(MotionEvent e, Point point)
			{
				GestureDetector?.OnScroll((relativeTo) => TouchDetector.CalculatePosition(e, GestureDetector.MauiView, relativeTo), GestureStatus.Completed, point, Point.Zero, _scrollVelocity);
				_isScrolling = false;
				_scrollVelocity = Point.Zero;
			}

			#endregion
		}

		#endregion
	}
}