using Android.App;
using Android.Content.PM;
using Android.OS;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Syncfusion.Maui.Samples.Sandbox
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
	}
}
