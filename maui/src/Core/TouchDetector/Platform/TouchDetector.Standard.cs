using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides touch detection functionality. 
    /// </summary>
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(View? mauiView)
        {
            // The method is left empty as the platform - specific implementations are provided in separate files.
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            // The methd is left empty as the platform-specific implementations are provided in separate files.
        }
    }
}
