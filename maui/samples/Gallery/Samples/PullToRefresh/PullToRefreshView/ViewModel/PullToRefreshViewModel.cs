namespace Syncfusion.Maui.ControlsGallery.PullToRefresh;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
/// <summary>
/// A ViewModel for PullToRefresh sample.
/// </summary>
public class PullToRefreshViewModel : INotifyPropertyChanged
{
    readonly Random randomNumber = new Random();
    readonly string[] weatherTypes = { "Sunny", "Rain", "Snow", "Cloudy", "Thunderstorms", "Partly Cloudy", "Foggy" };
    readonly string[] backgrounds = { "#FFF4C3", "#D5F3FF", "#E4EDF6", "#D5F7FF", "#D0D0D0", "#FFE2B8", "#D5EBFF" };
    readonly string[] weatherImages = { "sunny", "heavyrain", "snowwithcloudy", "mostlycloudy", "thunderstrom", "partlysunny", "foggywithcloudy" };
    readonly string[] weatherIcons = { "\ue78e", "\ue793", "\ue792", "\ue790", "\ue791", "\ue795", "\ue794"};
    WeatherData? data;
    ObservableCollection<WeatherData>? selectedData;
    ObservableCollection<WindDetails>? windDetailList;

    /// <summary>
    /// Initializes a new instance of the PullToRefreshViewModel class.
    /// </summary>
    public PullToRefreshViewModel()
    {
        this.SelectedData = this.GetWeatherData();
        this.WindDetailList = this.GetWindDetails();
        this.Data = this.SelectedData[0];
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
            return this.selectedData;
        }

        set
        {
            this.selectedData = value;
            this.RaisePropertyChanged("SelectedData");
        }
    }

    /// <summary>
    /// Gets or sets the value of Selected Data and notifies user when collection value gets changed.
    /// </summary>
    public ObservableCollection<WindDetails>? WindDetailList
    {
        get
        {
            return this.windDetailList;
        }

        set
        {
            this.windDetailList = value;
            this.RaisePropertyChanged("WindDetailList");
        }
    }

    /// <summary>
    /// Gets or sets the value of Data and notifies user when collection value gets changed.
    /// </summary>
    public WeatherData? Data
    {
        get
        {
            return this.data;
        }

        set
        {
            this.data = value;
            this.RaisePropertyChanged("Data");
        }
    }

    /// <summary>
    /// Generates weather data values
    /// </summary>
    /// <returns>data value</returns>
    private ObservableCollection<WeatherData> GetWeatherData()
    {
        DateTime now = DateTime.Now;
        ObservableCollection<WeatherData> array = new ObservableCollection<WeatherData>();
        for (int i = 0; i < 7; i++)
        {
            WeatherData data = new WeatherData();
            data.WeatherType = this.weatherTypes[i];
            data.Date = GetDate(i);
            data.Temperature = this.UpdateTemperature(data.WeatherType);
            data.BackgroundColor = this.backgrounds[i];
            data.WeatherImage = this.weatherImages[i] + ".png";
            data.WeatherIcon = this.weatherIcons[i];

            array.Add(data);
        }

        return array;
    }

    private ObservableCollection<WindDetails>? GetWindDetails()
    {
        ObservableCollection<WindDetails> array = new ObservableCollection<WindDetails>()
        {
            new WindDetails(){ Detail = "Air Quality", Values = "38" },
            new WindDetails(){ Detail = "Wind", Values = "18km/h" },
            new WindDetails(){ Detail = "Humidity", Values = "77%" },
        };

        return array;
    }

    /// <summary>
    /// Generates date time
    /// </summary>
    /// <returns>date time value</returns>
    private DateTime GetDate(int i)
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
        switch (weatherType)
        {
            case "Sunny":
                return this.sunnyTemperatures[randomNumber.Next(0, sunnyTemperatures.Length)];
            case "Rain":
                return this.rainyTemperatures[randomNumber.Next(0, rainyTemperatures.Length)];
            case "Snow":
                return this.snowTemperatures[randomNumber.Next(0, snowTemperatures.Length)];
            case "Cloudy":
                return this.cloudyTemperatures[randomNumber.Next(0, cloudyTemperatures.Length)];
            case "Thunderstorms":
                return this.thunderstormTemperatures[randomNumber.Next(0, thunderstormTemperatures.Length)];
            case "Partly Cloudy":
                return this.partiallyCloudyTemperatures[randomNumber.Next(0, partiallyCloudyTemperatures.Length)];
            case "Foggy":
                return this.foggyTemperatures[randomNumber.Next(0, foggyTemperatures.Length)];
            default:
                return "";
        }
    }

    string[] sunnyTemperatures = new string[]
    {
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
    };

    string[] rainyTemperatures = new string[]
    {
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
    };

    string[] snowTemperatures = new string[]
    {
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
    };

    string[] cloudyTemperatures = new string[]
    {
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
    };

    string[] partiallyCloudyTemperatures = new string[]
    {
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
    };

    string[] thunderstormTemperatures = new string[]
    {
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
    };

    string[] foggyTemperatures = new string[]
    {
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
    };

    #region INotifyPropertyChanged

    /// <summary>
    /// Triggers when Items Collections Changed.
    /// </summary>
    /// <param name="name">string type parameter named as name represent propertyName</param>
    private void RaisePropertyChanged(string name)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    #endregion
}