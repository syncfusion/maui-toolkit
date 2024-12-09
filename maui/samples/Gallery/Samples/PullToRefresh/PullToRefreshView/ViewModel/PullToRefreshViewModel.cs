using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// A ViewModel for PullToRefresh sample.
	/// </summary>
	public partial class PullToRefreshViewModel : INotifyPropertyChanged
	{
		readonly Random _randomNumber = new();
		readonly string[] _weatherTypes = ["Sunny", "Rain", "Snow", "Cloudy", "Thunderstorms", "Partly Cloudy", "Foggy"];
		readonly string[] _backgrounds = ["#FFF4C3", "#D5F3FF", "#E4EDF6", "#D5F7FF", "#D0D0D0", "#FFE2B8", "#D5EBFF"];
		readonly string[] _weatherImages = ["sunny", "heavyrain", "snowwithcloudy", "mostlycloudy", "thunderstrom", "partlysunny", "foggywithcloudy"];
		readonly string[] _weatherIcons = ["\ue78e", "\ue793", "\ue792", "\ue790", "\ue791", "\ue795", "\ue794"];
		WeatherData? _data;
		ObservableCollection<WeatherData>? _selectedData;
		ObservableCollection<WindDetails>? _windDetailList;

		/// <summary>
		/// Initializes a new instance of the PullToRefreshViewModel class.
		/// </summary>
		public PullToRefreshViewModel()
		{
			SelectedData = GetWeatherData();
			WindDetailList = PullToRefreshViewModel.GetWindDetails();
			Data = SelectedData[0];
		}

		/// <summary>
		/// Represents the method that will handle the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"></see> event raised when a property is changed on a component
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Gets or sets the value of Selected Data and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<WeatherData>? SelectedData
		{
			get
			{
				return _selectedData;
			}

			set
			{
				_selectedData = value;
				RaisePropertyChanged("SelectedData");
			}
		}

		/// <summary>
		/// Gets or sets the value of Selected Data and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<WindDetails>? WindDetailList
		{
			get
			{
				return _windDetailList;
			}

			set
			{
				_windDetailList = value;
				RaisePropertyChanged("WindDetailList");
			}
		}

		/// <summary>
		/// Gets or sets the value of Data and notifies user when collection value gets changed.
		/// </summary>
		public WeatherData? Data
		{
			get
			{
				return _data;
			}

			set
			{
				_data = value;
				RaisePropertyChanged("Data");
			}
		}

		/// <summary>
		/// Generates weather data values
		/// </summary>
		/// <returns>data value</returns>
		private ObservableCollection<WeatherData> GetWeatherData()
		{
			_ = DateTime.Now;
			ObservableCollection<WeatherData> array = [];
			for (int i = 0; i < 7; i++)
			{
				WeatherData data = new WeatherData
				{
					WeatherType = _weatherTypes[i],
					Date = PullToRefreshViewModel.GetDate(i)
				};
				data.Temperature = UpdateTemperature(data.WeatherType);
				data.BackgroundColor = _backgrounds[i];
				data.WeatherImage = _weatherImages[i] + ".png";
				data.WeatherIcon = _weatherIcons[i];

				array.Add(data);
			}

			return array;
		}

		private static ObservableCollection<WindDetails>? GetWindDetails()
		{
			ObservableCollection<WindDetails> array =
		[
			new WindDetails(){ Detail = "Air Quality", Values = "38" },
			new WindDetails(){ Detail = "Wind", Values = "18km/h" },
			new WindDetails(){ Detail = "Humidity", Values = "77%" },
		];

			return array;
		}

		/// <summary>
		/// Generates date time
		/// </summary>
		/// <returns>date time value</returns>
		private static DateTime GetDate(int i)
		{
			return DateTime.Today.AddDays(i);
		}

		/// <summary>
		/// Methods helps to Update the temperature based on WeatherType.
		/// </summary>
		/// <param name="weatherType">Represent weatherType.</param>
		/// <returns>Returns temperature based on weatherType.</returns>
		internal string UpdateTemperature(string weatherType)
		{
			return weatherType switch
			{
				"Sunny" => _sunnyTemperatures[_randomNumber.Next(0, _sunnyTemperatures.Length)],
				"Rain" => _rainyTemperatures[_randomNumber.Next(0, _rainyTemperatures.Length)],
				"Snow" => _snowTemperatures[_randomNumber.Next(0, _snowTemperatures.Length)],
				"Cloudy" => _cloudyTemperatures[_randomNumber.Next(0, _cloudyTemperatures.Length)],
				"Thunderstorms" => _thunderstormTemperatures[_randomNumber.Next(0, _thunderstormTemperatures.Length)],
				"Partly Cloudy" => _partiallyCloudyTemperatures[_randomNumber.Next(0, _partiallyCloudyTemperatures.Length)],
				"Foggy" => _foggyTemperatures[_randomNumber.Next(0, _foggyTemperatures.Length)],
				_ => "",
			};
		}

		readonly string[] _sunnyTemperatures =
		[
			"28°",
			"32°",
			"26°",
			"30°",
			"34°",
			"29°",
			"31°",
			"27°",
			"33°",
			"25°"
		];

		readonly string[] _rainyTemperatures =
		[
			"18°",
			"22°",
			"16°",
			"20°",
			"15°",
			"23°",
			"17°",
			"21°",
			"19°",
			"14°"
		];

		readonly string[] _snowTemperatures =
		[
			"-5°",
			"2°",
			"-8°",
			"-3°",
			"0°",
			"-10°",
			"3°",
			"-6°",
			"-2°",
			"1°",
		];

		readonly string[] _cloudyTemperatures =
		[
			"17°",
			"20°",
			"15°",
			"18°",
			"22°",
			"16°",
			"19°",
			"21°",
			"14°",
			"23°"
		];

		readonly string[] _partiallyCloudyTemperatures =
		[
			"22°",
			"26°",
			"20°",
			"24°",
			"18°",
			"28°",
			"23°",
			"19°",
			"25°",
			"21°"
		];

		readonly string[] _thunderstormTemperatures =
		[
			"25°",
			"28°",
			"22°",
			"30°",
			"18°",
			"26°",
			"21°",
			"29°",
			"23°",
			"27°"
		];

		readonly string[] _foggyTemperatures =
		[
			"15°",
			"12°",
			"18°",
			"10°",
			"14°",
			"20°",
			"8°",
			"16°",
			"22°",
			"11°"
		];

		#region INotifyPropertyChanged

		/// <summary>
		/// Triggers when Items Collections Changed.
		/// </summary>
		/// <param name="name">string type parameter named as name represent propertyName</param>
		private void RaisePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}