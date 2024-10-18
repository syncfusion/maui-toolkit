using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Brush> PaletteBrushes { get; set; }
        public ObservableCollection<Brush> CustomColor2 { get; set; }
        public ObservableCollection<Brush> CustomColor3 { get; set; }

        public ObservableCollection<Brush> CustomColor4 { get; set; }
        public ObservableCollection<Brush> AlterColor1 { get; set; }

        bool enableAnimation = true;
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

            CustomColor2 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#519085")),
                new SolidColorBrush(Color.FromArgb("#F06C64")),
                new SolidColorBrush(Color.FromArgb("#FDD056")),
                new SolidColorBrush(Color.FromArgb("#81B589")),
                new SolidColorBrush(Color.FromArgb("#88CED2"))
            };

            CustomColor3 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#04ABC1")),
                new SolidColorBrush(Color.FromArgb("#234A76")),
                new SolidColorBrush(Color.FromArgb("#DD6031")),
                new SolidColorBrush(Color.FromArgb("#7642A9")),
                new SolidColorBrush(Color.FromArgb("#495963"))
            };

            CustomColor4 = new ObservableCollection<Brush>()
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

            AlterColor1 = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#314A6E")),
                 new SolidColorBrush(Color.FromArgb("#48988B")),
            };
        }
    }

    public class ChartColorModel : ObservableCollection<Brush>
    {

    }
}
