using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Maui.ControlsGallery.Samples.TabView.CenterButton;

namespace Syncfusion.Maui.ControlsGallery.TabView.SfTabView;

public partial class CenterButton : SampleView
{
	public ObservableCollection<ContactsInfo> PrimaryListSource { get; set; }
	public ObservableCollection<ContactsInfo> SecondaryListSource { get; set; }
	public ObservableCollection<ContactsInfo> ThirdListSource { get; set; }

	public CenterButton()
	{
		InitializeComponent();
		this.PrimaryListSource = new ContactsInfoRepository().GetContactDetails(7);
		this.SecondaryListSource = new ContactsInfoRepository().GetContactDetails(4);
		this.ThirdListSource = new ContactsInfoRepository().GetContactDetails(16);
		this.BindingContext = this;
	}

	void Handle_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (TabView.SelectedIndex == 3)
		{
			// Navigation.PushAsync(new Profile());
		}
	}

	void CenterButton_Tapped(object sender, System.EventArgs e)
	{
		ComposeDialog.Opacity = 0;
		boxView.BackgroundColor = Color.FromArgb("#1E90FF");
		ComposeDialog.IsVisible = true;
#if NET10_0
		ComposeDialog.FadeToAsync(1, 600, Easing.Linear);
#else
		ComposeDialog.FadeTo(1, 600, Easing.Linear);
#endif

	}
	void Button_Clicked(object sender, System.EventArgs e)
	{
		TabView.SelectedIndex = 0;
	}

	public void CloseDialog()
	{
#if NET10_0
		ComposeDialog.FadeToAsync(0, 600, Easing.Linear);
#else
		ComposeDialog.FadeTo(0, 600, Easing.Linear);
#endif
		ComposeDialog.IsVisible = false;
		TabView.SelectedIndex = 0;
	}

	private void Grid_Tapped(object sender, TappedEventArgs e)
	{
		if (ComposeDialog.IsVisible)
		{
			CloseDialog();
		}
	}

	private void SendButton_Clicked(object sender, TappedEventArgs e)
	{
		CloseDialog();
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
	}
}

public class DateTimeToStringConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is DateTime item)
		{
			return item.ToString("hh:mm tt");
		}

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

}