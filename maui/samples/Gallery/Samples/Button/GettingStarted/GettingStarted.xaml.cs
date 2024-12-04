namespace Syncfusion.Maui.ControlsGallery.Buttons.Button
{
    public partial class GettingStarted : SampleView
    {
        public GettingStarted()
        {
            InitializeComponent();
#if ANDROID || IOS
            Content = new GettingStartedMobile();
#elif WINDOWS || MACCATALYST
            Content = new GettingStartedDesktop();
#endif
        }
        
    }
}
