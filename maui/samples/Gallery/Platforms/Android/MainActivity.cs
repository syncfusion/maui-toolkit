using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Syncfusion.Maui.ControlsGallery;

[Activity(Theme = "@style/Maui.SplashTheme", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	/// <inheritdoc/>
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		//Window.AddFlags(WindowManagerFlags.Fullscreen);
		//Window.AddFlags(WindowManagerFlags.LayoutNoLimits);

		//// Git issue link-https://github.com/dotnet/maui/issues/11274
		//// Layout moves up on keyboard open in android.
		Window?.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);
	}
}
