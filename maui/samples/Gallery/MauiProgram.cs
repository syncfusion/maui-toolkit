namespace Syncfusion.Maui.ControlsGallery;

using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.ControlsGallery.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
				fonts.AddFont("Roboto-Regular.ttf", "Roboto");
                fonts.AddFont("Sync FontIcons.ttf");
                fonts.AddFont("SegmentIcon.ttf", "SegmentIcon");
            });
		builder.ConfigureSampleBrowserBase();
		return builder.Build();
	}
}
