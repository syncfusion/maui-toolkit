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
            return new WindowOverlayStack(Context);
        }
    }
}
