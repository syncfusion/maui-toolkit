using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Shimmer;

namespace Syncfusion.Maui.ControlsGallery.Shimmer
{
	public partial class ViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Check the application theme is light or dark.
		/// </summary>
		readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

		int _waveWidth = 50;

		Color _waveColor;

		Color _shimmerColor;

		int _duration = 1000;

		int _repeatCount = 1;

		public int WaveWidth
		{
			get { return _waveWidth; }
			set
			{
				_waveWidth = value;
				RaisePropertyChanged("WaveWidth");
			}
		}

		public int Duration
		{
			get { return _duration; }
			set
			{
				_duration = value;
				RaisePropertyChanged("Duration");
			}
		}

		public Color WaveColor
		{
			get { return _waveColor; }
			set
			{
				_waveColor = value;
				RaisePropertyChanged("WaveColor");
			}
		}

		public Color ShimmerColor
		{
			get { return _shimmerColor; }
			set
			{
				_shimmerColor = value;
				RaisePropertyChanged("ShimmerColor");
			}
		}

		public int RepeatCount
		{
			get { return _repeatCount; }
			set
			{
				_repeatCount = value;
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
				return
				[
					"Feed",
					"Video",
					"Shopping",
					"Article",
					"Profile",
					"Persona"
				];
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
			if (_isLightTheme)
			{
				_waveColor = Color.FromArgb("#FFFFFF");
				_shimmerColor = Color.FromArgb("#F3EDF7");
				ShimmerColors =
				[
					Color.FromArgb("#F3EDF7"),
					Color.FromArgb("#F4E4FF"),
					Color.FromArgb("#E4FFF5"),
					Color.FromArgb("#FFF4E4"),
					Color.FromArgb("#FFE4E4"),
				];

				WaveColors =
				[
					Color.FromArgb("#FFFFFF"),
					Color.FromArgb("#EACBFF"),
					Color.FromArgb("#B7FFE4"),
					Color.FromArgb("#FFE1B6"),
					Color.FromArgb("#FFCCCC"),
				];
			}
			else
			{
				_waveColor = Color.FromArgb("#3B3842");
				_shimmerColor = Color.FromArgb("#25232A");
				ShimmerColors =
				[
					Color.FromArgb("#25232A"),
					Color.FromArgb("#2E004D"),
					Color.FromArgb("#003421"),
					Color.FromArgb("#3F2600"),
					Color.FromArgb("#3C0000"),
				];

				WaveColors =
				[
					Color.FromArgb("#3B3842"),
					Color.FromArgb("#440171"),
					Color.FromArgb("#005F3C"),
					Color.FromArgb("#6B4000"),
					Color.FromArgb("#700000"),
				];
			}
		}
	}
}
