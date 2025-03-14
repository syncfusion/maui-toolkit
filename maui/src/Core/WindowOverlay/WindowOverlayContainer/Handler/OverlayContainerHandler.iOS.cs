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
            return new WindowOverlayStack();
        }

        protected override void ConnectHandler(WindowOverlayStack platformView)
        {
            PlatformView.Connect(VirtualView);
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(WindowOverlayStack platformView)
        {
            PlatformView.DisConnect();
            base.DisconnectHandler(platformView);
        }
    }
}
