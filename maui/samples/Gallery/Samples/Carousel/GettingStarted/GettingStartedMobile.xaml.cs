
namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
	public partial class GettingStartedMobile : SampleView
	{
		public GettingStartedMobile()
		{
			InitializeComponent();
		}

		internal void GettingStarted_Unloaded()
		{
			carousel?.Handler?.DisconnectHandler();
			carousel = null;
		}

		private void carousel_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.Carousel.SelectionChangedEventArgs e)
		{
			if (sender is Syncfusion.Maui.Toolkit.Carousel.SfCarousel sfCarousel)
			{
				if (sfCarousel.ItemsSource.ElementAt(sfCarousel.SelectedIndex) is CarouselModel selectedItem)
				{
					countryDescriptionLabel.Text = selectedItem.Description;
					countryNameLabel.Text = selectedItem.Name;
				}
			}
		}
	}
}