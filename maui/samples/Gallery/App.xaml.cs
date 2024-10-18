using System.Reflection;
namespace Syncfusion.Maui.ControlsGallery;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        var appInfo = typeof(App).GetTypeInfo().Assembly;
        MainPage = Syncfusion.Maui.ControlsGallery.BaseConfig.MainPageInit(appInfo);
    }
}
