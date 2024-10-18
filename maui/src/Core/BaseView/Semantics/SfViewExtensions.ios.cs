namespace Syncfusion.Maui.Toolkit
{
    using Foundation;
    using UIKit;

    internal static class SfViewExtensions
    {
        internal static void Announce(this UIView view, string text)
        {
            if (!UIAccessibility.IsVoiceOverRunning)
            {
                return;
            }

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.Announcement, new NSString(text));
        }
    }
}