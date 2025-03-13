using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.Internals
{
	internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, WindowOverlayStack>
	{
		internal OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
		{
		}

		protected override WindowOverlayStack CreatePlatformView()
		{
			return new WindowOverlayStack();
		}

		/// <summary>
		/// ConnectHandler the handler.
		/// </summary>
		/// <param name="platformView">Instance of platformView.</param>
		protected override void ConnectHandler(WindowOverlayStack platformView)
		{
			WireEvents(platformView);
			base.ConnectHandler(platformView);
		}

		/// <summary>
		/// Disconnects the handler.
		/// </summary>
		/// <param name="platformView">Instance of platformView.</param>
		protected override void DisconnectHandler(WindowOverlayStack platformView)
		{
			UnWireEvents(platformView);
			base.DisconnectHandler(platformView);
		}

		#region Private Methods

		/// <summary>
		/// Wires all the the events needed for processing manipulations.
		/// </summary>
		void WireEvents(WindowOverlayStack platformView)
		{
			platformView.PointerPressed += OnPointerPressed;
		}

		/// <summary>
		/// UnWires all the the events wired for processing manipulations.
		/// </summary>
		void UnWireEvents(WindowOverlayStack platformView)
		{
			platformView.PointerPressed -= OnPointerPressed;
		}

		/// <summary>
		/// Handles the PointerPressed event when the user interacts with the overlay container.
		/// This method processes the touch interaction and passes the coordinates to the VirtualView for further processing.
		/// </summary>
		/// <param name="sender">The source of the event, which is typically the <see cref="Microsoft.UI.Xaml.UIElement"/> that received the pointer press.</param>
		/// <param name="e">The event data associated with the pointer press, including information about the pointer's position.</param>
		void OnPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var position = e.GetCurrentPoint(sender as UIElement);
			VirtualView?.ProcessTouchInteraction((float)position.Position.X, (float)position.Position.Y);
		}

		#endregion
	}
}