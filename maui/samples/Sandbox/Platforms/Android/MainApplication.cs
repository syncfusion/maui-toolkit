using Android.App;
using Android.Runtime;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Syncfusion.Maui.Samples.Sandbox
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}
