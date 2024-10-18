using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Shimmer;

namespace Syncfusion.Maui.ControlsGallery.Shimmer
{
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        bool isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        int waveWidth = 50;

        Color waveColor;

        Color shimmerColor;

        int duration = 1000;

        int repeatCount = 1;

        public int WaveWidth
        {
            get { return waveWidth; }
            set
            {
                waveWidth = value;
                RaisePropertyChanged("WaveWidth");
            }
        }

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        public Color WaveColor
        {
            get { return waveColor; }
            set
            {
                waveColor = value;
                RaisePropertyChanged("WaveColor");
            }
        }

        public Color ShimmerColor
        {
            get { return shimmerColor; }
            set
            {
                shimmerColor = value;
                RaisePropertyChanged("ShimmerColor");
            }
        }

        public int RepeatCount
        {
            get { return repeatCount; }
            set
            {
                repeatCount = value;
                RaisePropertyChanged("RepeatCount");
            }
        }

        public ObservableCollection<Color> ShimmerColors { get; set; }

        public List<Color> WaveColors { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<string> ShimmerTypes
        {
            get
            {
                return new List<string>
                {
                    "Feed",
                    "Video",
                    "Shopping",
                    "Article",
                    "Profile",
                    "Persona"
                };
            }
        }

        public Array WaveDirectionTypes
        {
            get
            {
                return Enum.GetValues(typeof(ShimmerWaveDirection));
            }
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public ViewModel()
        {
            if (this.isLightTheme)
            {
                this.waveColor = Color.FromArgb("#FFFFFF");
                this.shimmerColor = Color.FromArgb("#F3EDF7");
                this.ShimmerColors = new ObservableCollection<Color>
                {
                    Color.FromArgb("#F3EDF7"),
                    Color.FromArgb("#F4E4FF"),
                    Color.FromArgb("#E4FFF5"),
                    Color.FromArgb("#FFF4E4"),
                    Color.FromArgb("#FFE4E4"),
                };

                this.WaveColors = new List<Color>
                {
                    Color.FromArgb("#FFFFFF"),
                    Color.FromArgb("#EACBFF"),
                    Color.FromArgb("#B7FFE4"),
                    Color.FromArgb("#FFE1B6"),
                    Color.FromArgb("#FFCCCC"),
                };
            }
            else
            {
                this.waveColor = Color.FromArgb("#3B3842");
                this.shimmerColor = Color.FromArgb("#25232A");
                this.ShimmerColors = new ObservableCollection<Color>
                {
                    Color.FromArgb("#25232A"),
                    Color.FromArgb("#2E004D"),
                    Color.FromArgb("#003421"),
                    Color.FromArgb("#3F2600"),
                    Color.FromArgb("#3C0000"),
                };

                this.WaveColors = new List<Color>
                {
                    Color.FromArgb("#3B3842"),
                    Color.FromArgb("#440171"),
                    Color.FromArgb("#005F3C"),
                    Color.FromArgb("#6B4000"),
                    Color.FromArgb("#700000"),
                };
            }
        }
    }
}
