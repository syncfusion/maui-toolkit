using MauiView = Microsoft.Maui.Controls.View;
using GestureStatus = Microsoft.Maui.GestureStatus;
using UIKit;
using CoreGraphics;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Represents a gesture detector for the iOS platform that handles various gestures.
	/// </summary>
	public partial class GestureDetector
	{
		#region Fields
#if MACCATALYST
		UIRightTapGestureExt? _rightTapGesture;
#endif
		UIPinchGestureExt? _pinchGesture;
		UIPanGestureExt? _panGesture;
		UITapGestureExt? _tapGesture;
		UILongPressGestureExt? _longPressGesture;
		#endregion

		#region Properties

		internal UITapGestureExt? DoubleTapGesture { get; set; }

		#endregion

		#region Internal Methods

		/// <summary>
		/// Subscribes to native gesture events for the specified MAUI view.
		/// </summary>
		internal void SubscribeNativeGestureEvents(MauiView? mauiView)
		{
			if (mauiView != null)
			{
				var handler = mauiView.Handler;
				if (handler?.PlatformView is UIView nativeView)
				{
					SubscribePinchGesture(nativeView);
					SubscribePanGesture(nativeView);
					SubscribeDoubleTapGesture(nativeView);
					SubscribeTapGesture(nativeView);
#if MACCATALYST
					SubscribeRightTapGesture(nativeView);
#endif
					SubscribeLongPressGesture(nativeView);
				}
			}
		}

		void SubscribePinchGesture(UIView nativeView)
		{
			if (_pinchGestureListeners?.Count > 0)
			{
				_pinchGesture = new UIPinchGestureExt(this);
				nativeView.AddGestureRecognizer(_pinchGesture);
			}
		}

		void SubscribePanGesture(UIView nativeView)
		{
			if (_panGestureListeners?.Count > 0)
			{
				_panGesture = new UIPanGestureExt(this);
				nativeView.AddGestureRecognizer(_panGesture);
			}
		}

		void SubscribeDoubleTapGesture(UIView nativeView)
		{
			if (_doubleTapGestureListeners?.Count > 0)
			{
				DoubleTapGesture = new UITapGestureExt(this, 2);
				nativeView.AddGestureRecognizer(DoubleTapGesture);
			}
		}

		void SubscribeTapGesture(UIView nativeView)
		{
			if (_tapGestureListeners?.Count > 0)
			{
				_tapGesture = new UITapGestureExt(this, 1);
				nativeView.AddGestureRecognizer(_tapGesture);
			}
		}

#if MACCATALYST
		void SubscribeRightTapGesture(UIView nativeView)
		{
			if (_rightTapGestureListeners?.Count > 0)
			{
				_rightTapGesture = new UIRightTapGestureExt(this);
				nativeView.AddGestureRecognizer(_rightTapGesture);
			}
		}
#endif

		void SubscribeLongPressGesture(UIView nativeView)
		{
			if (_longPressGestureListeners?.Count > 0)
			{
				_longPressGesture = new UILongPressGestureExt(this);
				nativeView.AddGestureRecognizer(_longPressGesture);
			}
		}

		/// <summary>
		/// Creates and adds native gesture recognizers to the view.
		/// </summary>
		internal void CreateNativeListener()
		{
			if (MauiView != null)
			{
				var handler = MauiView.Handler;
				if (handler?.PlatformView is UIView nativeView)
				{
					if (_pinchGesture == null && _pinchGestureListeners?.Count > 0)
					{
						_pinchGesture = new UIPinchGestureExt(this);
						nativeView.AddGestureRecognizer(_pinchGesture);
					}

					if (_panGesture == null && _panGestureListeners?.Count > 0)
					{
						_panGesture = new UIPanGestureExt(this);
						nativeView.AddGestureRecognizer(_panGesture);
					}

					if (DoubleTapGesture == null && _doubleTapGestureListeners?.Count > 0)
					{
						DoubleTapGesture = new UITapGestureExt(this, 2);
						nativeView.AddGestureRecognizer(DoubleTapGesture);
					}

					if (_tapGesture == null && _tapGestureListeners?.Count > 0)
					{
						_tapGesture = new UITapGestureExt(this, 1);
						nativeView.AddGestureRecognizer(_tapGesture);
					}

#if MACCATALYST
					if (_rightTapGesture == null && _rightTapGestureListeners?.Count > 0)
					{
						_rightTapGesture = new UIRightTapGestureExt(this);
						nativeView.AddGestureRecognizer(_rightTapGesture);
					}
#endif

					if (_longPressGesture == null && _longPressGestureListeners?.Count > 0)
					{
						_longPressGesture = new UILongPressGestureExt(this);
						nativeView.AddGestureRecognizer(_longPressGesture);
					}
				}
			}
		}

		/// <summary>
		/// Unsubscribes from native gesture events for the specified element handler.
		/// </summary>
		internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
		{
			if (handler != null)
			{
				if (handler.PlatformView is UIView nativeView)
				{
					var gestures = nativeView.GestureRecognizers;

					if (gestures != null)
					{
						if (_pinchGesture != null)
						{
							nativeView.RemoveGestureRecognizer(_pinchGesture);
						}
						if (_panGesture != null)
						{
							nativeView.RemoveGestureRecognizer(_panGesture);
						}
						if (_tapGesture != null)
						{
							nativeView.RemoveGestureRecognizer(_tapGesture);
						}
#if MACCATALYST
						if (_rightTapGesture != null)
						{
							nativeView.RemoveGestureRecognizer(_rightTapGesture);
						}
#endif
						if (DoubleTapGesture != null)
						{
							nativeView.RemoveGestureRecognizer(DoubleTapGesture);
						}
						if (_longPressGesture != null)
						{
							nativeView.RemoveGestureRecognizer(_longPressGesture);
						}
					}
				}
			}
		}

		#endregion

		#region Gesture Recognizers

		#region Pan Gesture
		/// <summary>
		/// Extends UIPanGestureRecognizer to handle pan gestures for the GestureDetector.
		/// </summary>
		internal class UIPanGestureExt : UIPanGestureRecognizer
		{
			#region Fields

			WeakReference<IGestureListener>? _gestureListener;
			WeakReference<GestureDetector>? _gestureDetector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the UIPanGestureExt class.
			/// </summary>
			public UIPanGestureExt(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;

				if (gestureDetector.MauiView is IGestureListener _gestureListener)
				{
					GestureListener = _gestureListener;
				}

				AddTarget(OnScroll);

				ShouldRecognizeSimultaneously += GestureRecognizer;
			}

			#endregion


			#region Properties

			IGestureListener? GestureListener
			{
				get => _gestureListener != null && _gestureListener.TryGetTarget(out var v) ? v : null;
				set => _gestureListener = value == null ? null : new(value);
			}

			GestureDetector? GestureDetector
			{
				get => _gestureDetector != null && _gestureDetector.TryGetTarget(out var v) ? v : null;
				set => _gestureDetector = value == null ? null : new(value);
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Determines whether this gesture recognizer should be allowed to recognize simultaneously with another gesture recognizer.
			/// </summary>
			bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
			{
				if (otherGestureRecognizer is UITouchRecognizerExt || otherGestureRecognizer is UIScrollRecognizerExt || GestureListener == null)
				{
					return true;
				}

				return !GestureListener.IsTouchHandled;
			}

			/// <summary>
			/// Handles the scroll event.
			/// </summary>
			void OnScroll()
			{
				if (GestureDetector == null || !GestureDetector.MauiView.IsEnabled || GestureDetector.MauiView.InputTransparent)
				{
					return;
				}

				var locationInView = LocationInView(View);
				var translateLocation = TranslationInView(View);
				var state = GestureStatus.Completed;

				switch (State)
				{
					case UIGestureRecognizerState.Began:
						state = GestureStatus.Started;
						break;
					case UIGestureRecognizerState.Changed:
						state = GestureStatus.Running;
						break;
					case UIGestureRecognizerState.Cancelled:
					case UIGestureRecognizerState.Failed:
						state = GestureStatus.Canceled;
						break;
					case UIGestureRecognizerState.Ended:
						state = GestureStatus.Completed;
						break;
				}

				Point velocity = Point.Zero;
				if (state == GestureStatus.Completed || state == GestureStatus.Canceled)
				{
					var nativeVelocity = VelocityInView(View);
					velocity = new Point(nativeVelocity.X, nativeVelocity.Y);
				}

				GestureDetector.OnScroll((relativeTo) => TouchDetector.CalculatePosition(relativeTo, GestureDetector.MauiView, this), state, new Point(locationInView.X, locationInView.Y), new Point(translateLocation.X, translateLocation.Y), velocity);
				SetTranslation(CGPoint.Empty, View);
			}

			#endregion
		}

		#endregion

		#region Pinch Gesture

		/// <summary>
		/// Extends UIPinchGestureRecognizer to handle pinch gestures for the GestureDetector.
		/// </summary>
		class UIPinchGestureExt : UIPinchGestureRecognizer
		{
			#region Fields

			WeakReference<IGestureListener>? _gestureListener;
			WeakReference<GestureDetector>? _gestureDetector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the UIPinchGestureExt class.
			/// </summary>
			public UIPinchGestureExt(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;

				if (gestureDetector.MauiView is IGestureListener _gestureListener)
				{
					GestureListener = _gestureListener;
				}

				AddTarget(OnPinch);

				ShouldRecognizeSimultaneously = (g, o) => GestureListener == null || !GestureListener.IsTouchHandled;
			}

			#endregion


			#region Properties

			IGestureListener? GestureListener
			{
				get => _gestureListener != null && _gestureListener.TryGetTarget(out var v) ? v : null;
				set => _gestureListener = value == null ? null : new(value);
			}

			GestureDetector? GestureDetector
			{
				get => _gestureDetector != null && _gestureDetector.TryGetTarget(out var v) ? v : null;
				set => _gestureDetector = value == null ? null : new(value);
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Handles the pinch event.
			/// </summary>
			void OnPinch()
			{
				if (GestureDetector == null || !GestureDetector.MauiView.IsEnabled || GestureDetector.MauiView.InputTransparent)
				{
					return;
				}

				var locationInView = LocationInView(View);
				var state = GestureStatus.Completed;
				double angle = double.NaN;
				if (NumberOfTouches == 2)
				{
					CGPoint touchStart = LocationOfTouch(0, View);
					CGPoint touchEnd = LocationOfTouch(1, View);
					angle = MathUtils.GetAngle((float)touchStart.X, (float)touchEnd.X, (float)touchStart.Y, (float)touchEnd.Y);
				}

				switch (State)
				{
					case UIGestureRecognizerState.Began:
						state = GestureStatus.Started;
						break;
					case UIGestureRecognizerState.Changed:
						state = GestureStatus.Running;
						break;
					case UIGestureRecognizerState.Cancelled:
					case UIGestureRecognizerState.Failed:
						state = GestureStatus.Canceled;
						break;
					case UIGestureRecognizerState.Ended:
						state = GestureStatus.Completed;
						break;
				}

				GestureDetector.OnPinch((relativeTo) => TouchDetector.CalculatePosition(relativeTo, GestureDetector.MauiView, this), state, new Point(locationInView.X, locationInView.Y), angle, (float)Scale);
				Scale = 1; // Resetting the previous scale value.
			}

			#endregion
		}

		#endregion

		#region Tap Gesture

		/// <summary>
		/// Extends <see cref="UITapGestureRecognizer"/> to handle tap gestures for the GestureDetector.
		/// </summary>
		internal class UITapGestureExt : UITapGestureRecognizer
		{
			#region Fields

			WeakReference<IGestureListener>? _gestureListener;
			WeakReference<GestureDetector>? _gestureDetector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="UITapGestureExt"/> class.
			/// </summary>
			public UITapGestureExt(GestureDetector gestureDetector, nuint tapsCount)
			{
				GestureDetector = gestureDetector;
				NumberOfTapsRequired = tapsCount;

				if (gestureDetector.MauiView is IGestureListener _gestureListener)
				{
					GestureListener = _gestureListener;
				}

				if (tapsCount == 1 && gestureDetector.DoubleTapGesture != null && gestureDetector._doubleTapGestureListeners != null && gestureDetector._doubleTapGestureListeners.Count > 0 && gestureDetector._doubleTapGestureListeners[0].IsRequiredSingleTapGestureRecognizerToFail)
				{
					RequireGestureRecognizerToFail(gestureDetector.DoubleTapGesture);
				}

				AddTarget(OnTap);

				ShouldRecognizeSimultaneously += GestureRecognizer;

				ShouldReceiveTouch += HandleTouchGesture;
			}

			#endregion

			#region Properties

			IGestureListener? GestureListener
			{
				get => _gestureListener != null && _gestureListener.TryGetTarget(out var v) ? v : null;
				set => _gestureListener = value == null ? null : new(value);
			}

			GestureDetector? GestureDetector
			{
				get => _gestureDetector != null && _gestureDetector.TryGetTarget(out var v) ? v : null;
				set => _gestureDetector = value == null ? null : new(value);
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Determines whether the touch should be handled by this gesture recognizer.
			/// </summary>
			bool HandleTouchGesture(UIGestureRecognizer recognizer, UITouch touch)
			{
				if (GestureListener is ITapGestureListener tapGestureListener)
				{
					tapGestureListener.ShouldHandleTap(touch);
				}

				return true;
			}

			/// <summary>
			/// Determines whether this gesture recognizer should be allowed to recognize simultaneously with another gesture recognizer.
			/// </summary>
			bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
			{
				if (otherGestureRecognizer is UILongPressGestureExt)
				{
					return false;
				}

				if (GestureListener != null && !GestureListener.IsRequiredSingleTapGestureRecognizerToFail && Equals(gestureRecognizer.View, otherGestureRecognizer.View) && gestureRecognizer is UITapGestureExt gestureExt && otherGestureRecognizer is UITapGestureExt otherGestureExt)
				{
					if (gestureExt.NumberOfTapsRequired != otherGestureExt.NumberOfTapsRequired)
					{
						return false;
					}
				}

				return GestureListener == null || !GestureListener.IsTouchHandled;
			}

			/// <summary>
			/// Handles the tap event.
			/// </summary>
			void OnTap()
			{
				if (GestureDetector == null || !GestureDetector.MauiView.IsEnabled || GestureDetector.MauiView.InputTransparent)
				{
					return;
				}

				var locationInView = LocationInView(View);
				GestureDetector.OnTapped(new Point(locationInView.X, locationInView.Y), (int)NumberOfTapsRequired);
			}

			#endregion
		}

		#endregion

		#region Right Tap Gesture

#if MACCATALYST

/* Unmerged change from project 'Syncfusion.Maui.Toolkit (net8.0-ios)'
Before:
            #region Fields
After:
			#region Fields
*/
		/// <summary>
		/// Extends <see cref="UITapGestureRecognizer"/> to handle right-click (secondary button) taps for MacCatalyst.
		/// </summary>
		internal class UIRightTapGestureExt : UITapGestureRecognizer
		{
			#region Fields

			WeakReference<IGestureListener>? _gestureListener;
			WeakReference<GestureDetector>? _gestureDetector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="UIRightTapGestureExt"/> class.
			/// </summary>
			public UIRightTapGestureExt(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;
				ButtonMaskRequired = UIEventButtonMask.Secondary;

				if (gestureDetector.MauiView is IGestureListener _gestureListener)
				{
					GestureListener = _gestureListener;
				}

				AddTarget(OnRightTap);

				ShouldRecognizeSimultaneously = (g, o) => GestureListener == null || !GestureListener.IsTouchHandled;
			}

			#endregion

			#region Properties

			IGestureListener? GestureListener
			{
				get => _gestureListener != null && _gestureListener.TryGetTarget(out var v) ? v : null;
				set => _gestureListener = value == null ? null : new(value);
			}

			GestureDetector? GestureDetector
			{
				get => _gestureDetector != null && _gestureDetector.TryGetTarget(out var v) ? v : null;
				set => _gestureDetector = value == null ? null : new(value);
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Handles the right-tap event.
			/// </summary>
			void OnRightTap()
			{
				if (GestureDetector == null || !GestureDetector.MauiView.IsEnabled || GestureDetector.MauiView.InputTransparent)
				{
					return;
				}

				var locationInView = LocationInView(View);
				GestureDetector.OnRightTapped(new Point(locationInView.X, locationInView.Y), PointerDeviceType.Mouse);
			}


/* Unmerged change from project 'Syncfusion.Maui.Toolkit (net8.0-ios)'
Before:
            #endregion
        }
After:
			#endregion
        }
*/
			#endregion
		}
#endif

		#endregion

		#region Long Press Gesture

		/// <summary>
		/// Extends UILongPressGestureRecognizer to handle long press gestures for the GestureDetector.
		/// </summary>
		class UILongPressGestureExt : UILongPressGestureRecognizer
		{
			#region Fields

			WeakReference<IGestureListener>? _gestureListener;
			WeakReference<GestureDetector>? _gestureDetector;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="UILongPressGestureExt"/> class.
			/// </summary>
			public UILongPressGestureExt(GestureDetector gestureDetector)
			{
				GestureDetector = gestureDetector;

				if (gestureDetector.MauiView is IGestureListener _gestureListener)
				{
					GestureListener = _gestureListener;
				}

				AddTarget(OnLongPress);

				ShouldRecognizeSimultaneously += GestureRecognizer;
			}

			#endregion

			#region Properties

			IGestureListener? GestureListener
			{
				get => _gestureListener != null && _gestureListener.TryGetTarget(out var v) ? v : null;
				set => _gestureListener = value == null ? null : new(value);
			}

			GestureDetector? GestureDetector
			{
				get => _gestureDetector != null && _gestureDetector.TryGetTarget(out var v) ? v : null;
				set => _gestureDetector = value == null ? null : new(value);
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Determines whether this gesture recognizer should be allowed to recognize simultaneously with another gesture recognizer.
			/// </summary>
			bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
			{
				if (otherGestureRecognizer is UITapGestureExt)
				{
					return false;
				}

				return GestureListener == null || !GestureListener.IsTouchHandled;
			}

			/// <summary>
			/// Handles the long press event.
			/// </summary>
			void OnLongPress()
			{
				if (GestureDetector == null || !GestureDetector.MauiView.IsEnabled || GestureDetector.MauiView.InputTransparent)
				{
					return;
				}

				var state = GestureStatus.Completed;
				var locationInView = LocationInView(View);
				switch (State)
				{
					case UIGestureRecognizerState.Began:
						state = GestureStatus.Started;
						break;
					case UIGestureRecognizerState.Changed:
						state = GestureStatus.Running;
						break;
					case UIGestureRecognizerState.Cancelled:
					case UIGestureRecognizerState.Failed:
						state = GestureStatus.Canceled;
						break;
					case UIGestureRecognizerState.Ended:
						state = GestureStatus.Completed;
						break;
				}

				if (State != UIGestureRecognizerState.Possible)
				{
					GestureDetector.OnLongPress((relativeTo) => TouchDetector.CalculatePosition(relativeTo, GestureDetector.MauiView, this), new Point(locationInView.X, locationInView.Y), state);
				}
			}

			#endregion
		}

		#endregion

		#endregion
	}
}

