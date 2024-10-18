
namespace Syncfusion.Maui.ControlsGallery.EffectsView.SfEffectsView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RotateAnimation : SampleView
    {
        public RotateAnimation()
        {
            InitializeComponent();
        }

        private void RotationEffectsViewAnimationCompleted(object sender, EventArgs e)
        {
            if (RotationEffectsView.Angle == 180)
            {
                RotationEffectsView.Angle = 0;
            }
            else
            {
                RotationEffectsView.Angle = 180;
            }
        }
    }
}
