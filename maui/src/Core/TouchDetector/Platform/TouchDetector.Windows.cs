using MauiView = Microsoft.Maui.Controls.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Provides functionality for detecting and handling touch events in the Windows platform.
	/// </summary>
	public partial class TouchDetector
	{
		#region Fields

		UIElement? _nativeView;

		#endregion

		#region Internal Methods

		/// <summary>
		/// Subscribes to native touch events for the given MauiView.
		/// </summary>
		internal void SubscribeNativeTouchEvents(MauiView? mauiView)
		{
			if (mauiView != null)
			{
				var handler = mauiView.Handler;
				_nativeView = handler?.PlatformView as UIElement;
				if (_nativeView != null)
				{
					_nativeView.PointerPressed += PlatformView_PointerPressed;
					_nativeView.PointerMoved += PlatformView_PointerMoved;
					_nativeView.PointerReleased += PlatformView_PointerReleased;
					_nativeView.PointerCanceled += PlatformView_PointerCanceled;
					_nativeView.PointerWheelChanged += PlatformView_PointerWheelChanged;
					_nativeView.PointerEntered += PlatformView_PointerEntered;
					_nativeView.PointerExited += PlatformView_PointerExited;
				}
			}
		}

		/// <summary>
		/// Gets the position of a routed event relative to a specified element.
		/// </summary>
		internal static Point? GetPosition(IElement? relativeTo, RoutedEventArgs e)
		{
			var result = GetPositionRelativeToElement(e, relativeTo);

			if (result is null)
			{
				return null;
			}

			return result;
		}

		/// <summary>
		/// Gets the position of a routed event relative to a specified element.
		/// </summary>
		static Point? GetPositionRelativeToElement(RoutedEventArgs e, IElement? relativeTo)
		{
			if (relativeTo == null)
			{
				return GetPositionRelativeToPlatformElement(e, null);
			}

			if (relativeTo?.Handler?.PlatformView is UIElement element)
			{
				return GetPositionRelativeToPlatformElement(e, element);
			}

			return null;
		}

		/// <summary>
		/// Unsubscribes from native touch events for the given element handler.
		/// </summary>
		internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
		{
			if (handler != null)
			{
				if (_nativeView != null)
				{
					_nativeView.PointerPressed -= PlatformView_PointerPressed;
					_nativeView.PointerMoved -= PlatformView_PointerMoved;
					_nativeView.PointerReleased -= PlatformView_PointerReleased;
					_nativeView.PointerCanceled -= PlatformView_PointerCanceled;
					_nativeView.PointerWheelChanged -= PlatformView_PointerWheelChanged;
					_nativeView.PointerEntered -= PlatformView_PointerEntered;
					_nativeView.PointerExited -= PlatformView_PointerExited;
					_nativeView = null;
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles the pointer exited event for the platform view.
		/// </summary>
		void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}

			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				var pointerPoint = e.GetCurrentPoint(nativeView);
				var property = pointerPoint.Properties;
				PointerEventArgs eventArgs = new PointerEventArgs((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Exited, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
				{
					IsLeftButtonPressed = property.IsLeftButtonPressed,
					IsRightButtonPressed = property.IsRightButtonPressed,
				};

				OnTouchAction(eventArgs);
			}
		}

		/// <summary>
		/// Handles the pointer entered event for the platform view.
		/// </summary>
		void PlatformView_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}

			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				var pointerPoint = e.GetCurrentPoint(nativeView);
				var property = pointerPoint.Properties;
				PointerEventArgs eventArgs = new PointerEventArgs((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Entered, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
				{
					IsLeftButtonPressed = property.IsLeftButtonPressed,
					IsRightButtonPressed = property.IsRightButtonPressed,
				};

				OnTouchAction(eventArgs);
			}
		}

		/// <summary>
		/// Handles the pointer wheel changed event for the platform view.
		/// </summary>
		void PlatformView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}

			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				var pointerPoint = e.GetCurrentPoint(nativeView);
				e.Handled = OnScrollAction(pointerPoint.PointerId, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y), pointerPoint.Properties.MouseWheelDelta, e.Handled);
			}
		}

		/// <summary>
		/// Handles the pointer pressed event for the platform view.
		/// </summary>
		void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}

			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				nativeView.CapturePointer(e.Pointer);
				var pointerPoint = e.GetCurrentPoint(nativeView);
				var property = pointerPoint.Properties;

				PointerEventArgs eventArgs = new PointerEventArgs((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Pressed, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
				{
					IsLeftButtonPressed = property.IsLeftButtonPressed,
					IsRightButtonPressed = property.IsRightButtonPressed,
				};

				OnTouchAction(eventArgs);

				if (_touchListeners[0].IsTouchHandled)
				{
					nativeView.ManipulationMode = ManipulationModes.None;
				}
			}
		}

		/// <summary>
		/// Gets the position of a pointer event relative to a specified UIElement.
		/// </summary>
		static Point? GetPositionRelativeToPlatformElement(RoutedEventArgs e, UIElement? relativeTo)
		{
			if (e is PointerRoutedEventArgs p)
			{
				var point = p.GetCurrentPoint(relativeTo);
				return new Point(point.Position.X, point.Position.Y);
			}

			return null;
		}

		/// <summary>
		/// Handles the pointer moved event for the platform view.
		/// </summary>
		void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}
			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				var pointerPoint = e.GetCurrentPoint(nativeView);
				OnTouchAction((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Moved, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));
			}
		}

		/// <summary>
		/// Handles the pointer canceled event for the platform view.
		/// </summary>
		void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}
			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				nativeView.ReleasePointerCapture(e.Pointer);
				var pointerPoint = e.GetCurrentPoint(nativeView);
				OnTouchAction((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Cancelled, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

				if (nativeView.ManipulationMode == ManipulationModes.None)
				{
					nativeView.ManipulationMode = ManipulationModes.System;
				}
			}
		}

		/// <summary>
		/// Handles the pointer released event for the platform view.
		/// </summary>
		void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (!MauiView.IsEnabled || MauiView.InputTransparent)
			{
				return;
			}
			var nativeView = sender as UIElement;
			if (nativeView != null)
			{
				nativeView.ReleasePointerCapture(e.Pointer);
				var pointerPoint = e.GetCurrentPoint(nativeView);
				OnTouchAction((relativeTo) => GetPosition(relativeTo, e), pointerPoint.PointerId, PointerActions.Released, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

				if (nativeView.ManipulationMode == ManipulationModes.None)
				{
					nativeView.ManipulationMode = ManipulationModes.System;
				}
			}
		}

		/// <summary>
		/// Converts a Microsoft.UI.Input.PointerDeviceType to the corresponding PointerDeviceType.
		/// </summary>
		static PointerDeviceType GetDeviceType(Microsoft.UI.Input.PointerDeviceType deviceType)
		{
			if (deviceType == Microsoft.UI.Input.PointerDeviceType.Pen)
			{
				return PointerDeviceType.Stylus;
			}
			else
			{
				return deviceType == Microsoft.UI.Input.PointerDeviceType.Mouse ? PointerDeviceType.Mouse : PointerDeviceType.Touch;
			}
		}

		#endregion
	}
}
