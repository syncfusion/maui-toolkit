using Android.Views;
using MauiView = Microsoft.Maui.Controls.View;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
using View = Android.Views.View;
namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Handles touch detection for Android platform.
	/// </summary>
	public partial class TouchDetector
	{
		#region Internal Methods

		/// <summary>
		/// Calculates the position of a touch event relative to different elements.
		/// </summary>
		internal static Point? CalculatePosition(MotionEvent? e, IElement? sourceElement, IElement? relativeElement)
		{
			var context = sourceElement?.Handler?.MauiContext?.Context;

			if (context == null)
			{
				return null;
			}

			if (e == null)
			{
				return null;
			}

			if (relativeElement == null)
			{
				return new Point(context.FromPixels(e.RawX), context.FromPixels(e.RawY));
			}

			if (relativeElement == sourceElement)
			{
				return new Point(context.FromPixels(e.GetX()), context.FromPixels(e.GetY()));
			}

			if (relativeElement?.Handler?.PlatformView is AView aView)
			{
				var location = GetLocationOnScreenPx(aView);

				var x = e.RawX - location.X;
				var y = e.RawY - location.Y;

				return new Point(context.FromPixels(x), context.FromPixels(y));
			}

			return null;
		}

		/// <summary>
		/// Gets the location of a view on the screen.
		/// </summary>
		internal static Point GetLocationOnScreenPx(View view)
		{
			int[] location = new int[2];
			view.GetLocationOnScreen(location);
			return new Point(location[0], location[1]);
		}

		/// <summary>
		/// Subscribes to native touch events.
		/// </summary>
		internal void SubscribeNativeTouchEvents(MauiView? mauiView)
		{
			if (mauiView != null)
			{
				var handler = mauiView.Handler;

				if (handler?.PlatformView is View nativeView)
				{
					nativeView.Touch += OnTouch;
				}
			}
		}

		/// <summary>
		/// Unsubscribes from native touch events for the given element handler.
		/// </summary>
		internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
		{
			if (handler != null)
			{
				if (handler.PlatformView is View nativeView)
				{
					nativeView.Touch -= OnTouch;
				}
			}
		}

		/// <summary>
		/// Handles touch events and translates them into appropriate actions.
		/// </summary>
		internal void OnTouch(object? sender, View.TouchEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}

			MotionEvent? motionEvent = e.Event;

			if (sender is not View nativeView || motionEvent == null)
			{
				return;
			}

			int actionIndex = motionEvent.ActionIndex;
			int pointerId = motionEvent.GetPointerId(actionIndex);
			Point screenPoint = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
			Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
			Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
			bool isHandled = _touchListeners[0].IsTouchHandled;

			var handled = isHandled || motionEvent.Action == MotionEventActions.Pointer2Down || motionEvent.PointerCount > 1;

			nativeView.Parent?.RequestDisallowInterceptTouchEvent(handled);

			HandleTouchAction(motionEvent, pointerId, point);
		}

		#endregion

		#region Private Methods

		private void HandleTouchAction(MotionEvent motionEvent, int pointerId, Point point)
		{
			PointerDeviceType pointerDeviceType = PointerDeviceType.Touch; // Default to Touch

			// Check if the first pointer is a stylus
			if (motionEvent.PointerCount > 0 && motionEvent.GetToolType(0) == MotionEventToolType.Stylus)
			{
				pointerDeviceType = PointerDeviceType.Stylus;
			}

			switch (motionEvent.ActionMasked)
			{
				case MotionEventActions.Down:
				case MotionEventActions.PointerDown:
					OnTouchAction((relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Pressed, pointerDeviceType, point);
					break;
				case MotionEventActions.Move:
					OnTouchAction((relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Moved, pointerDeviceType, point);
					break;
				case MotionEventActions.Up:
				case MotionEventActions.Pointer1Up:
					OnTouchAction((relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Released, pointerDeviceType, point);
					break;
				case MotionEventActions.Cancel:
					OnTouchAction((relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Cancelled, pointerDeviceType, point);
					break;
			}
		}

		#endregion

	}
}