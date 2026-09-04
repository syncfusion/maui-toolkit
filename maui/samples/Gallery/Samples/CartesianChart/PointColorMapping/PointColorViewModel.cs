using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class PointColorViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> TemperatureData { get; set; }

        public PointColorViewModel()
        {
            TemperatureData = new ObservableCollection<ChartDataModel>
            {
                new() { Name = "Jan", Value = 42, PointColor = Color.FromArgb("#E6CDF9") },
                new() { Name = "Feb", Value = 45, PointColor = Color.FromArgb("#D1A8F3") },
                new() { Name = "Mar", Value = 53, PointColor = Color.FromArgb("#BB83EC") },
                new() { Name = "Apr", Value = 59, PointColor = Color.FromArgb("#A35DE5") },
                new() { Name = "May", Value = 64, PointColor = Color.FromArgb("#8933DE") },
                new() { Name = "Jun", Value = 72, PointColor = Color.FromArgb("#6621AC") },
                new() { Name = "Jul", Value = 73, PointColor = Color.FromArgb("#551E8B") },
                new() { Name = "Aug", Value = 71, PointColor = Color.FromArgb("#7923CE") },
                new() { Name = "Sep", Value = 65, PointColor = Color.FromArgb("#9649E2") },
                new() { Name = "Oct", Value = 58, PointColor = Color.FromArgb("#AF70E9") },
                new() { Name = "Nov", Value = 52, PointColor = Color.FromArgb("#C695F0") },
                new() { Name = "Dec", Value = 50, PointColor = Color.FromArgb("#DCBBF6") }
            };
        }
    }
}
