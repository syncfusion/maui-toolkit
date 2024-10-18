using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class CartesianLegend : SampleView
    {
        public CartesianLegend()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            tooltipChart.Handler?.DisconnectHandler();
        }
    }

    public class LineSeriesExt : LineSeries
    {
        public Geometry? PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static readonly BindableProperty PathDataProperty =
    BindableProperty.Create(
        nameof(PathData),
        typeof(Geometry),
        typeof(LineSeriesExt),
        null,
        BindingMode.Default,
        null);

        public LineSeriesExt()
        {
        }
    }
}
