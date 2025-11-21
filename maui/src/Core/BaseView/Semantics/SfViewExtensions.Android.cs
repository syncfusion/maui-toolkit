using Android.OS;
using Android.Views.Accessibility;
namespace Syncfusion.Maui.Toolkit
{
	internal static class SfViewExtensions
	{
		internal static void Announce(this Android.Views.View view, string text)
		{
#if NET10_0
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Build.VERSION.SdkInt < BuildVersionCodes.Baklava)
			{
#pragma warning disable CA1422 // Validate platform compatibility
				view.AnnounceForAccessibility(text);
#pragma warning restore CA1422 // Validate platform compatibility
			}
#else
			view.AnnounceForAccessibility(text);
#endif
		}
	}
}