using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// A class contains Properties and notifies when property value has changed 
	/// </summary>
	public partial class WeatherData : INotifyPropertyChanged
	{
		#region NotifyPropertyChanged

		/// <summary>
		/// Represents the method that will handle the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"></see> event raised when a property is changed on a component
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Triggers when Items Collections Changed.
		/// </summary>
		/// <param name="name">string type parameter named as name</param>
		private void RaisePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Field
		string? _weatherType;
		DateTime? _date;
		string? _temperature;
		string? _weatherIcon;
		string? _weatherImage;
		string? _backgroundColor;
		#endregion

		#region Property

		/// <summary>
		/// Gets or sets the value of WeatherType and notifies user when value gets changed
		/// </summary>
		public string? WeatherType
		{
			get { return _weatherType; }
			set { _weatherType = value; RaisePropertyChanged("WeatherType"); }
		}

		/// <summary>
		/// Gets or sets the value of Date and notifies user when value gets changed
		/// </summary>
		public DateTime? Date
		{
			get { return _date; }
			set { _date = value; RaisePropertyChanged("Date"); }
		}

		/// <summary>
		/// Gets or sets the value of WeatherImage and notifies user when value gets changed
		/// </summary>
		public string? WeatherImage
		{
			get { return _weatherImage; }
			set { _weatherImage = value; RaisePropertyChanged("WeatherImage"); }
		}

		/// <summary>
		/// Gets or sets the value of Temperature and notifies user when value gets changed
		/// </summary>
		public string? Temperature
		{
			get { return _temperature; }
			set { _temperature = value; RaisePropertyChanged("Temperature"); }
		}

		/// <summary>
		/// Gets or sets the value of BackgroundColor and notifies user when value gets changed
		/// </summary>
		public string? BackgroundColor
		{
			get { return _backgroundColor; }
			set { _backgroundColor = value; RaisePropertyChanged("backgroundImage"); }
		}

		/// <summary>
		/// Gets or sets the value of WeatherIcon and notifies user when value gets changed
		/// </summary>
		public string? WeatherIcon
		{
			get { return _weatherIcon; }
			set { _weatherIcon = value; RaisePropertyChanged("WeatherIcon"); }
		}
		#endregion
	}

	/// <summary>
	/// A class contains properties for Wind list.
	/// </summary>
	public class WindDetails
	{
		string? _detail;

		string? _values;

		public string? Detail
		{
			get { return _detail; }
			set { _detail = value; }
		}

		public string? Values
		{
			get { return _values; }
			set { _values = value; }
		}

	}
}