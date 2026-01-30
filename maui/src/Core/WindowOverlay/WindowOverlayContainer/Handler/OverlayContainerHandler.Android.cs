using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, WindowOverlayStack>
    {
        internal OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }
      
        protected override WindowOverlayStack CreatePlatformView()
        {
			// Creates the platform WindowOverlayStack that hosts overlay elements.
			// Assigns the cross platform VirtualView so platform events can be routed back.
			var windowOverlayStack = new WindowOverlayStack(Context);
			windowOverlayStack.OverlayContanier = VirtualView as WindowOverlayContainer;
			return windowOverlayStack;
		}
    }
}
