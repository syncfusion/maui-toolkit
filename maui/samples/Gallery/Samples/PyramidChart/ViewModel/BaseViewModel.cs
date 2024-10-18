using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace  Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Brush> PaletteBrushes { get; set; }
        public ObservableCollection<Brush> SelectionBrushes { get; set; }
        public ObservableCollection<Brush> CustomColor_1 { get; set; }
        public ObservableCollection<Brush> CustomColor_2 { get; set; }
        public ObservableCollection<Brush> CustomColor_3 { get; set; }
        public ObservableCollection<Brush> ThemePaletteBrushes3 { get; set; }

        private bool enableAnimation = true;
        public bool EnableAnimation
        {
            get { return enableAnimation; }
            set
            {
                enableAnimation = value;
                OnPropertyChanged("EnableAnimation");
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public BaseViewModel()
        {
            PaletteBrushes = new ObservableCollection<Brush>()
            {
               new SolidColorBrush(Color.FromArgb("#314A6E")),
                 new SolidColorBrush(Color.FromArgb("#48988B")),
                 new SolidColorBrush(Color.FromArgb("#5E498C")),
                 new SolidColorBrush(Color.FromArgb("#74BD6F")),
                 new SolidColorBrush(Color.FromArgb("#597FCA"))
            };

            SelectionBrushes = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#40314A6E")),
                new SolidColorBrush(Color.FromArgb("#4048988B")),
                new SolidColorBrush(Color.FromArgb("#405E498C")),
                new SolidColorBrush(Color.FromArgb("#4074BD6F")),
                new SolidColorBrush(Color.FromArgb("#40597FCA"))
            };

            CustomColor_1 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#95DB9C")),
                new SolidColorBrush(Color.FromArgb("#B95375")),
                new SolidColorBrush(Color.FromArgb("#56BBAF")),
                new SolidColorBrush(Color.FromArgb("#606D7F")),
                new SolidColorBrush(Color.FromArgb("#E99941")),
                new SolidColorBrush(Color.FromArgb("#327DBE")),
                new SolidColorBrush(Color.FromArgb("#E7695A")),
                new SolidColorBrush(Color.FromArgb("#2D4552")),
                new SolidColorBrush(Color.FromArgb("#4E9B8F")),
                new SolidColorBrush(Color.FromArgb("#E9A56C")),
            };

            CustomColor_2 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#346bf5")),
                new SolidColorBrush(Color.FromArgb("#ff9d00")),
                new SolidColorBrush(Color.FromArgb("#346bf5")),
                new SolidColorBrush(Color.FromArgb("#ff9d00")),
                new SolidColorBrush(Color.FromArgb("#346bf5")),
            };

            CustomColor_3 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#4892D7")),
                new SolidColorBrush(Color.FromArgb("#812178")),
                new SolidColorBrush(Color.FromArgb("#D96037")),
                new SolidColorBrush(Color.FromArgb("#1D2B6F")),
                new SolidColorBrush(Color.FromArgb("#3C495C")),
                new SolidColorBrush(Color.FromArgb("#4E9BA2"))

            };

            ThemePaletteBrushes3 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#A033F5")),
                new SolidColorBrush(Color.FromArgb("#E75A6E")),
                new SolidColorBrush(Color.FromArgb("#0DC920")),
                new SolidColorBrush(Color.FromArgb("#2EC4B6")),
            };
        }
    }

    public class ChartColorModel : ObservableCollection<Brush>
    {

    }
}
