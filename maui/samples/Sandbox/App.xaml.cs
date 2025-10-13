namespace Syncfusion.Maui.Samples.Sandbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

		protected override Window CreateWindow(IActivationState? activationState)
		{
			AppWindow window = new AppWindow();
            window.Page = new AppShell();
            return window;
		}
	}
}
