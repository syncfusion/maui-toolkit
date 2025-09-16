using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	public partial class SfBottomSheet
	{
		#region Fields

		/// <summary>
		/// The native platform view of the bottom sheet, used for handling touch and manipulation events.
		/// </summary>
		FrameworkElement? _bottomSheetNativeView;

		/// <summary>
		/// A flag indicating whether a manipulation gesture (e.g., drag) has started on the bottom sheet.
		/// </summary>
		bool _isManipulationStarted;

		/// <summary>
		/// A flag that tracks whether the touch event has been handled, specifically for touch devices.
		/// </summary>
		bool _isTouchHandled;

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

		/// <summary>
		/// Configures touch events based on the availability of the platform-specific view.
		/// </summary>
		void ConfigureTouch()
		{
			if (Handler is not null && Handler.PlatformView is not null)
			{
				WireEvents();
			}
			else
			{
				UnWireEvents();
			}
		}

		/// <summary>
		/// Attaches necessary touch and manipulation event handlers to the platform-specific view.
		/// </summary>
		void WireEvents()
		{
			if (Handler is not null && Handler.PlatformView is not null && Handler.PlatformView is FrameworkElement)
			{
				_bottomSheetNativeView = Handler.PlatformView as FrameworkElement;
				if (_bottomSheetNativeView is not null)
				{
					_bottomSheetNativeView.ManipulationMode = ManipulationModes.All;
					_bottomSheetNativeView.ManipulationStarted += OnManipulationStarted;
					_bottomSheetNativeView.PointerPressed += OnPointerPressed;
					_bottomSheetNativeView.ManipulationDelta += OnManipulationDelta;
					_bottomSheetNativeView.ManipulationCompleted += OnManipulationCompleted;
					_bottomSheetNativeView.PointerReleased += OnPointerReleased;
					_bottomSheetNativeView.PointerMoved += OnPointerMoved;
				}
			}
		}

		/// <summary>
		/// Detaches previously attached touch and manipulation event handlers from the platform-specific view.
		/// </summary>
		void UnWireEvents()
		{
			if (Handler is null && _bottomSheetNativeView is not null)
			{
				_bottomSheetNativeView.ManipulationMode = ManipulationModes.All;
				_bottomSheetNativeView.ManipulationStarted -= OnManipulationStarted;
				_bottomSheetNativeView.PointerPressed -= OnPointerPressed;
				_bottomSheetNativeView.ManipulationDelta -= OnManipulationDelta;
				_bottomSheetNativeView.ManipulationCompleted -= OnManipulationCompleted;
				_bottomSheetNativeView.PointerReleased -= OnPointerReleased;
				_bottomSheetNativeView.PointerMoved -= OnPointerMoved;
			}
		}

		/// <summary>
		/// Handles the start of a manipulation gesture on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">The manipulation started event arguments containing position details.</param>
		void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			if (!_isPointerPressed && _bottomSheet is not null)
			{
				if (e.Position.Y < _bottomSheet.TranslationY)
				{
					return;
				}

				// Handle the start of the manipulation
				OnHandleTouch(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
				_isManipulationStarted = true;
			}
		}

		/// <summary>
		/// Handles the pointer press action on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Pointer routed event arguments containing pointer details.</param>
		void OnPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if (_bottomSheetNativeView is not null && _bottomSheet is not null)
			{
				// Get the pointer position relative to the view
				var point = e.GetCurrentPoint(_bottomSheetNativeView).Position;
				if (point.Y < _bottomSheet.TranslationY)
				{
					return;
				}

				// Handle the pointer press action
				OnHandleTouch(PointerActions.Pressed, new Point(point.X, point.Y));
			}

			if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Touch)
			{
				_isTouchHandled = true;
			}
			else
			{
				_isTouchHandled = false;
			}
		}

		/// <summary>
		/// Handles the pointer move action on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Pointer routed event arguments containing pointer details.</param>
		void OnPointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse && _isTouchHandled)
			{
				return;
			}

			if (_isPointerPressed && _bottomSheetNativeView is not null)
			{
				// Get the pointer position relative to the view
				var point = e.GetCurrentPoint(_bottomSheetNativeView).Position;
				// Handle the pointer move action
				OnHandleTouch(PointerActions.Moved, new Point(point.X, point.Y));
			}
		}

		/// <summary>
		/// Handles the pointer release action on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Pointer routed event arguments containing pointer details.</param>
		void OnPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (_bottomSheetNativeView is not null)
			{
				var point = e.GetCurrentPoint(_bottomSheetNativeView).Position;
				OnHandleTouch(PointerActions.Released, new Point(point.X, point.Y));
			}

			_isManipulationStarted = false;
			_isTouchHandled = false;
		}

		/// <summary>
		/// Handles the completion of a manipulation gesture on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Manipulation completed event arguments containing details about the gesture.</param>
		void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (_isPointerPressed && _isManipulationStarted)
			{
				// Handle the release action at the end of manipulation.
				OnHandleTouch(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
				// Reset the manipulation state
				_isManipulationStarted = false;
			}
		}

		/// <summary>
		/// Handles the ongoing manipulation action (such as drag) as the user moves their pointer.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Manipulation delta event arguments containing details about the ongoing manipulation.</param>
		void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			if (_isPointerPressed && _isManipulationStarted)
			{
				// Handle the intermediate manipulation action
				OnHandleTouch(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));

				// Check for boundary conditions
				if (e.Position.Y < 0 || e.Position.Y > Height)
				{
					// Handle the release action if outside of content width
					OnHandleTouch(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
				}
			}
		}

		#endregion
	}
}
