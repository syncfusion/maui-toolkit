using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer;


public class Notification : INotifyPropertyChanged
{
    private string? name;
    private string? subject;
    private string? description;
    private string? imagesource;
	private string? timeStamp;

	public string? Name
    {
        get => name;
        set { name = value; OnPropertyChanged(nameof(Name)); }
    }

    public string? Subject
    {
        get => subject;
        set { subject = value; OnPropertyChanged(nameof(Subject)); }
    }

    public string? Description
    {
        get => description;
        set { description = value; OnPropertyChanged(nameof(Description)); }
    }

    public string? ImageSource
    {
        get => imagesource;
        set { imagesource = value; OnPropertyChanged(nameof(ImageSource)); }
    }

	public string? TimeStamp
	{
		get => timeStamp;
		set { timeStamp = value; OnPropertyChanged(nameof(TimeStamp)); }
	}

	public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
