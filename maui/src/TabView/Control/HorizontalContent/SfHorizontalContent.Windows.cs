using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.TabView
{
	internal partial class SfHorizontalContent
	{
		#region Fields

		FrameworkElement? _horizontalContentNativeView;
		bool _isManipulationStarted;

		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing.
		/// </summary>
		/// <param name="args">Handler changing event arguments.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			base.OnHandlerChanging(args);
		}

		#endregion

		#region Private Methods

		void ConfigureTouch()
		{
			if (Handler != null && Handler.PlatformView != null)
			{
				WireEvents();
			}
			else
			{
				UnWireEvents();
			}
		}

		void WireEvents()
		{
			if (Handler != null && Handler.PlatformView != null && Handler.PlatformView is FrameworkElement)
			{
				_horizontalContentNativeView = Handler.PlatformView as FrameworkElement;
				if (_horizontalContentNativeView != null)
				{
					_horizontalContentNativeView.ManipulationMode = ManipulationModes.All;
					_horizontalContentNativeView.ManipulationStarted += OnManipulationStarted;
					_horizontalContentNativeView.PointerPressed += OnPointerPressed;
					_horizontalContentNativeView.ManipulationDelta += OnManipulationDelta;
					_horizontalContentNativeView.ManipulationCompleted += OnManipulationCompleted;
					_horizontalContentNativeView.PointerReleased += OnPointerReleased;
					_horizontalContentNativeView.PointerMoved += OnPointerMoved;
					_horizontalContentNativeView.PointerCaptureLost += OnPointerCaptureLost;
				}
			}
		}

		void UnWireEvents()
		{
			if (Handler == null && _horizontalContentNativeView != null)
			{
				_horizontalContentNativeView.ManipulationMode = ManipulationModes.All;
				_horizontalContentNativeView.ManipulationStarted -= OnManipulationStarted;
				_horizontalContentNativeView.PointerPressed -= OnPointerPressed;
				_horizontalContentNativeView.ManipulationDelta -= OnManipulationDelta;
				_horizontalContentNativeView.ManipulationCompleted -= OnManipulationCompleted;
				_horizontalContentNativeView.PointerReleased -= OnPointerReleased;
				_horizontalContentNativeView.PointerMoved -= OnPointerMoved;
				_horizontalContentNativeView.PointerCaptureLost -= OnPointerCaptureLost;
			}
		}

		void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			if (!_isPressed)
			{
				// Handle the start of the manipulation
				OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
				_isManipulationStarted = true;
			}
		}

		bool _isTouchHandled;
		void OnPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if (_horizontalContentNativeView != null)
			{
				// Get the pointer position relative to the view
				var point = e.GetCurrentPoint(_horizontalContentNativeView).Position;
				// Handle the pointer press action
				OnHandleTouchInteraction(PointerActions.Pressed, new Point(point.X, point.Y));
			}

			if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Touch)
			{
				_isTouchHandled = true;
			}
			else
			{
				_isTouchHandled = false;
			}
			_horizontalContentNativeView?.ReleasePointerCapture(e.Pointer);
		}

		void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			if (_horizontalContentNativeView != null)
			{
				var point = e.GetCurrentPoint(_horizontalContentNativeView).Position;
				OnHandleTouchInteraction(PointerActions.Released, new Point(point.X, point.Y));
			}
			_isManipulationStarted = false;
			_isTouchHandled = false;
		}

		void OnPointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse && _isTouchHandled)
			{
				return;
			}

			if (_isPressed && _horizontalContentNativeView != null)
			{
				// Get the pointer position relative to the view
				var point = e.GetCurrentPoint(_horizontalContentNativeView).Position;
				// Handle the pointer move action
				OnHandleTouchInteraction(PointerActions.Moved, new Point(point.X, point.Y));
			}
		}

		void OnPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (_horizontalContentNativeView != null)
			{
				var point = e.GetCurrentPoint(_horizontalContentNativeView).Position;
				OnHandleTouchInteraction(PointerActions.Released, new Point(point.X, point.Y));
			}

			_isManipulationStarted = false;
			_isTouchHandled = false;
		}

		void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (_isPressed && _isManipulationStarted)
			{
				// Handle the release action at the end of manipulation.
				OnHandleTouchInteraction(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
				// Reset the manipulation state
				_isManipulationStarted = false;
			}
		}

		void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			if (_isPressed && _isManipulationStarted)
			{
				// Handle the intermediate manipulation action
				OnHandleTouchInteraction(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));

				// Check for boundary conditions
				if (e.Position.X < 0 || e.Position.X > ContentWidth)
				{
					// Handle the release action if outside of content width
					OnHandleTouchInteraction(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
				}
			}
		}

		#endregion
	}
}