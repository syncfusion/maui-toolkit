namespace Syncfusion.Maui.Toolkit
{
	internal static class SfViewExtensions
	{
		internal static void Announce(this Android.Views.View view, string text)
		{
			view.AnnounceForAccessibility(text);
		}
	}
}