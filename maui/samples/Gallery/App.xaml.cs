using System.Reflection;
namespace Syncfusion.Maui.ControlsGallery
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			Assembly assembly = typeof(App).GetTypeInfo().Assembly;
			var mainPage = BaseConfig.MainPageInit(assembly);
			return new Window(mainPage);
		}
	}
}