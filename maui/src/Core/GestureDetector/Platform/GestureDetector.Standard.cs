using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Enables a MAUI view to recognize gestures.
    /// </summary>
    public partial class GestureDetector
    {
        #region Internal Methods

        internal void SubscribeNativeGestureEvents(View? mauiView)
        {
            // The method is left empty as the platform - specific implementations are provided in separate files.
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            // The method is left empty as the platform - specific implementations are provided in separate files. 
        }

        internal void CreateNativeListener()
        {
            // The method is left empty as the platform - specific implementations are provided in separate files.
        }

        #endregion
    }
}
