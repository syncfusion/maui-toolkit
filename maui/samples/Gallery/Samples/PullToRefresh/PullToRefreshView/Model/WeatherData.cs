using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh;

/// <summary>
/// A class contains Properties and notifies when property value has changed 
/// </summary>
public class WeatherData : INotifyPropertyChanged
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
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    #endregion

    #region Field
    string? weatherType;
    DateTime? date;
    string? temperature;
    string? weatherIcon;
    string? weatherImage;
    string? backgroundColor;
    #endregion

    #region Property

    /// <summary>
    /// Gets or sets the value of WeatherType and notifies user when value gets changed
    /// </summary>
    public string? WeatherType
    {
        get { return weatherType; }
        set { weatherType = value; RaisePropertyChanged("WeatherType"); }
    }

    /// <summary>
    /// Gets or sets the value of Date and notifies user when value gets changed
    /// </summary>
    public DateTime? Date
    {
        get { return date; }
        set { date = value; RaisePropertyChanged("Date"); }
    }

    /// <summary>
    /// Gets or sets the value of WeatherImage and notifies user when value gets changed
    /// </summary>
    public string? WeatherImage
    {
        get { return weatherImage; }
        set { weatherImage = value; RaisePropertyChanged("WeatherImage"); }
    }

    /// <summary>
    /// Gets or sets the value of Temperature and notifies user when value gets changed
    /// </summary>
    public string? Temperature
    {
        get { return temperature; }
        set { temperature = value; RaisePropertyChanged("Temperature"); }
    }

    /// <summary>
    /// Gets or sets the value of BackgroundColor and notifies user when value gets changed
    /// </summary>
    public string? BackgroundColor
    {
        get { return backgroundColor; }
        set { backgroundColor = value; RaisePropertyChanged("backgroundImage"); }
    }

    /// <summary>
    /// Gets or sets the value of WeatherIcon and notifies user when value gets changed
    /// </summary>
    public string? WeatherIcon
    {
        get { return weatherIcon; }
        set { weatherIcon = value; RaisePropertyChanged("WeatherIcon"); }
    } 
    #endregion
}

/// <summary>
/// A class contains properties for Wind list.
/// </summary>
public class WindDetails
{
    string? detail;

    string? values;

    public string? Detail
    {
        get { return detail; }
        set { detail = value; }
    }

    public string? Values
    {
        get { return values; }
        set { values = value; }
    }

}