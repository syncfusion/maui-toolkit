using Microsoft.Maui.Controls;
 
namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{ 
    public partial class Selection : SampleView
    {
        readonly SelectionViewModel? model;

        public Selection()
        {
            InitializeComponent();
            dataPointSelection.SelectionChanging += Chart_SelectionChanging;
            model = chart1.BindingContext as SelectionViewModel;
        }

        private void Chart_SelectionChanging(object? sender, Syncfusion.Maui.Toolkit.Charts.ChartSelectionChangingEventArgs e)
        {
            if (model == null) return;

            if (e.OldIndexes.Count > 0 && e.NewIndexes.Count == 0)
            {
                series1.PaletteBrushes = model.ThemePaletteBrushes;
            }

            foreach (var index in e.NewIndexes)
            {
                series1.PaletteBrushes = model.SelectionBrushes;
                if (model.ThemePaletteBrushes[index] is SolidColorBrush brush)
                    dataPointSelection.SelectionBrush = brush;
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            chart1.Handler?.DisconnectHandler();
        }
    }
}
