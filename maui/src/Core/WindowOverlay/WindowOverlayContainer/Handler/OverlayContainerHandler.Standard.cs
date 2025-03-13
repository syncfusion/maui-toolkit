using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, object>
    {
        public OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }

        protected override object CreatePlatformView()
        {
            return new object();
        }
    }
}
