using Android.Views;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
namespace Syncfusion.Maui.Toolkit.TabView
{
	internal partial class SfHorizontalContent
	{
		#region Fields

		// Initial touch point
		Point _initialPoint = new(0, 0);

		// Touch down coordinates
		double _downX;
		double _downY;

		// Move coordinates during touch interaction
		double _moveX;
		double _moveY;

		// Constants for touch movement thresholds

#pragma warning disable CS8602
		double _density = Android.App.Application.Context.Resources.DisplayMetrics.Density;
#pragma warning restore CS8602

		double _swipeThreshold => 5 * _density;


		#endregion

		#region Override Methods

		/// <summary>
		/// On the intercept touch event.
		/// </summary>
		/// <returns><c>true</c>, if intercept touch event was enabled, <c>false</c> otherwise.</returns>
		/// <param name="motionEvent">Event values.</param>
		internal override bool OnInterceptTouchEvent(MotionEvent? motionEvent)
		{
			if (motionEvent != null && _tabView != null && _tabView.EnableSwiping)
			{
				int actionIndex = motionEvent.ActionIndex;
				Point screenPoint = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
				Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
				Point currenTouchPoint = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
				switch (motionEvent.Action)
				{
					case MotionEventActions.Down:
						{
							_downX = motionEvent.GetX();
							_downY = motionEvent.GetY();
							_initialPoint = currenTouchPoint;
							return false;
						}
					case MotionEventActions.Up:
						{
							_initialPoint = new Point(0, 0);
							break;
						}
					case MotionEventActions.Move:
						{
							_moveX = motionEvent.GetX();
							_moveY = motionEvent.GetY();

							// Check for vertical scrolling threshold
							if (Math.Abs(_downY - _moveY) > _swipeThreshold && Math.Abs(_downX - _moveX) < _swipeThreshold)
							{
								return false;
							}

							// Handle initial touch interaction
							if (!_isPressed && Math.Abs(_downY - _moveY) > _swipeThreshold && Math.Abs(_downX - _moveX) > _swipeThreshold)
							{
								OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
								return true;
							}
						}
						break;
				}
			}

			return base.OnInterceptTouchEvent(motionEvent);
		}

		/// <summary>
		/// This method triggers on any touch interaction on <see cref="SfHorizontalContent"/>.
		/// </summary>
		/// <param name="e">Pointer event arguments containing touch action and point.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
			switch (e.Action)
			{
				case PointerActions.Pressed:
					{
						// Handle the press action
						OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
						break;
					}

				case PointerActions.Moved:
					{
						// Handle the move action
						OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
						break;
					}

				case PointerActions.Released:
					{
						// Handle the release action
						OnHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
						break;
					}
			}
		}
		#endregion
	}
}