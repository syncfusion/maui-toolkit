using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Syncfusion.Maui.Toolkit.Internals;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

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

		/// <summary>
		/// The ScrollViewer currently under the pointer, if any, used to detect scrollability and edge handoff.
		/// </summary>
		ScrollViewer? _activeScrollViewer;

		/// <summary>
		/// Indicates whether the pointer is within a scrollable region (i.e., over a ScrollViewer).
		/// </summary>
		bool _isPointerInsideScrollable;

		/// <summary>
		/// Becomes true once the inner ScrollViewer hits an edge and control is handed off to the bottom sheet.
		/// </summary>
		bool _handoffToSheet;

		/// <summary>
		/// The last observed pointer Y position, used to compute movement deltas during drag.
		/// </summary>
		double _lastPointerY;

		/// <summary>
		/// Indicates whether the bottom sheet has been dragged during the current gesture.
		/// </summary>
		bool _sheetWasDragged;

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

				// Only arm the sheet manipulation immediately if not over a scrollable
				if (!_isPointerInsideScrollable)
				{
					OnHandleTouch(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
					_isManipulationStarted = true;
				}
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
				var point = e.GetCurrentPoint(_bottomSheetNativeView).Position;

				// Identify if press is within a ScrollViewer
				var source = e.OriginalSource as DependencyObject;
				_activeScrollViewer = TryGetAncestorScrollViewer(source);
				_isPointerInsideScrollable = _activeScrollViewer is not null;
				_handoffToSheet = false;
				_lastPointerY = point.Y;

				// If pressed above the sheet, ignore (existing check)
				if (point.Y < _bottomSheet.TranslationY)
				{
					return;
				}

				// If NOT inside a scrollable, start sheet gesture now
				if (!_isPointerInsideScrollable)
				{
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

			if (_bottomSheetNativeView is not null)
			{

				var pp = e.GetCurrentPoint(_bottomSheetNativeView);
				var props = pp.Properties;

				bool isPressedOrInContact = e.Pointer.PointerDeviceType switch
				{
					Microsoft.UI.Input.PointerDeviceType.Mouse => props.IsLeftButtonPressed,
					Microsoft.UI.Input.PointerDeviceType.Touch => pp.IsInContact,
					_ => false
				};

				if (!isPressedOrInContact)
				{
					return;
				}

				var point = e.GetCurrentPoint(_bottomSheetNativeView).Position;
				double dy = point.Y - _lastPointerY;

				if (_isPointerInsideScrollable && _activeScrollViewer is not null)
				{
					// While inner can scroll in this direction, do NOT route to sheet
					if (CanInnerScroll(_activeScrollViewer, dy))
					{
						_lastPointerY = point.Y;
						return; // inner ScrollViewer consumes it naturally
					}

					// Inner cannot scroll further -> handoff to sheet
					if (!_handoffToSheet)
					{
						// Synthesize a 'Pressed' at the current pointer location before we send Moved
						OnHandleTouch(PointerActions.Pressed, new Point(point.X, point.Y));
						_handoffToSheet = true;

						// Optional: capture pointer so we keep getting move events even if the finger drifts
						_bottomSheetNativeView.CapturePointer(e.Pointer);
					}

					e.Handled = true; // we are now routing to the bottom sheet
				}

				// Route movement to the sheet if:
				//  - we are not inside a scrollable, or
				//  - a handoff has happened.
				if (!_isPointerInsideScrollable || _handoffToSheet)
				{
					OnHandleTouch(PointerActions.Moved, new Point(point.X, point.Y));
					_sheetWasDragged = true;
				}

				_lastPointerY = point.Y;
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

				if (!_isPointerInsideScrollable || _handoffToSheet)
				{
					OnHandleTouch(PointerActions.Released, new Point(point.X, point.Y));
				}

				try
				{ 
					_bottomSheetNativeView.ReleasePointerCaptures();
				}
				catch { }
			}

			_isManipulationStarted = false;
			_isTouchHandled = false;
			_activeScrollViewer = null;
			_isPointerInsideScrollable = false;
			_handoffToSheet = false;
			_sheetWasDragged = false;
		}

		/// <summary>
		/// Handles the completion of a manipulation gesture on the platform-specific view.
		/// </summary>
		/// <param name="sender">The source of the event, typically the platform view.</param>
		/// <param name="e">Manipulation completed event arguments containing details about the gesture.</param>
		void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if ((_isPointerPressed && _isManipulationStarted) || _sheetWasDragged)
			{
				// Only release if the sheet gesture was active
				if (!_isPointerInsideScrollable || _handoffToSheet)
				{
					OnHandleTouch(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
					_isManipulationStarted = false;
					_sheetWasDragged = false;
				}
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
				// Only forward deltas if we are not over a scrollable OR we have handed off
				if (!_isPointerInsideScrollable || _handoffToSheet)
				{
					OnHandleTouch(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));
					_sheetWasDragged = true;

					// Boundary check remains as-is
					if (e.Position.Y < 0 || e.Position.Y > Height)
					{
						OnHandleTouch(PointerActions.Released, new Point(e.Position.X, e.Position.Y));
					}
				}
			}
		}

		/// <summary>
		/// Finds the closest ancestor <see cref="ScrollViewer"/> in the visual tree starting from the specified element.
		/// </summary>
		/// <param name="start">The element from which to begin the ancestor search.</param>
		/// <returns>The nearest <see cref="ScrollViewer"/> ancestor if found; otherwise, null.</returns>
		ScrollViewer? TryGetAncestorScrollViewer(DependencyObject? start)
		{
			var current = start;
			while (current is not null)
			{
				if (current is ScrollViewer sv)
					return sv;
				current = VisualTreeHelper.GetParent(current);
			}
			return null;
		}

		/// <summary>
		/// Determines whether the specified <see cref="ScrollViewer"/> can continue scrolling vertically
		/// in the direction indicated by the pointer's Y delta.
		/// </summary>
		/// <param name="sv">The scrollable view to check.</param>
		/// <param name="dy">The scroll direction.</param>
		/// <returns>True if scroll can occur further in that direction; otherwise, false.</returns>
		bool CanInnerScroll(ScrollViewer sv, double dy)
		{
			// can go downwards
			if (dy < 0)
			{
				return sv.VerticalOffset < sv.ScrollableHeight - 0.5;
			}

			// can go upwards
			if (dy > 0)
			{
				return sv.VerticalOffset > 0.5;
			}

			return false;
		}

		#endregion
	}
}
