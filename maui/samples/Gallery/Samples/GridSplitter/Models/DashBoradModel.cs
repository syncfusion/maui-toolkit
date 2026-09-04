using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.ControlsGallery.GridSplitter;

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public int TotalOrders { get; set; }
    public double TotalSpent { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime LastOrderDate { get; set; }
    public int CustomerSinceDays { get; set; }

    public string Avatar { get; set; } = string.Empty;
}

public class ActivityItem
{
    public string CustomerId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime Time { get; set; }

    public string Icon { get; set; } = string.Empty;

    public Color Color { get; set; } = Colors.Transparent;
}

public class PhotoItem : INotifyPropertyChanged
{
	private bool _isSelected;

	public string Image { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public string Dimensions { get; set; } = string.Empty;

	public DateTime CapturedDate { get; set; }

	public string Location { get; set; } = string.Empty;

	public string Photographer { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public string FileSize { get; set; } = string.Empty;

	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			if (_isSelected == value)
			{
				return;
			}

			_isSelected = value;
			OnPropertyChanged();
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}