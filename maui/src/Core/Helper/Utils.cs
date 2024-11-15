#if MONOANDROID
using PlatformView = Android.Views.View;
#elif __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#else
using PlatformView = System.Object;
#endif

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal static class WindowOverlayUtils
    {
        internal static PlatformView ToPlatform(this IElement view)
        {
            if (view is IReplaceableView replaceableView && replaceableView.ReplacedView != view)
            {
                return replaceableView.ReplacedView.ToPlatform();
            }

            _ = view.Handler ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set on parent.");

            if (view.Handler is IViewHandler viewHandler)
            {
                if (viewHandler.ContainerView is PlatformView containerView)
                {
                    return containerView;
                }

                if (viewHandler.PlatformView is PlatformView platformView)
                {
                    return platformView;
                }
            }

            return (view.Handler?.PlatformView as PlatformView)
                ?? throw new InvalidOperationException($"Unable to convert {view} to {typeof(PlatformView)}");
        }
    }
}
