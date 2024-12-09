using Syncfusion.Maui.Toolkit.Chips;

namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{

	public partial class ChipGettingStartedDesktop : SampleView
	{
		public ChipGettingStartedDesktop()
		{
			InitializeComponent();
		}
		private void SfChipGroup_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.Chips.SelectionChangedEventArgs e)
		{
			if (sender != null && ((sender as SfChipGroup)?.BindingContext as ChipViewModel) != null)
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				if (!string.IsNullOrEmpty("SelectedAddOnItems"))
				{
					var viewModel = ((sender as SfChipGroup).BindingContext as ChipViewModel);
					if (viewModel.SelectedAddOnItems.Contains("Fast Charge"))
					{
						viewModel.FastChargePrice = 657;
					}
					if (viewModel.SelectedAddOnItems.Contains("512 MB SD Card"))
					{
						viewModel.SDCardPrice = 599;
					}
					if (viewModel.SelectedAddOnItems.Contains("2 Years Extended Warranty"))
					{
						viewModel.WarrentyPrice = 799;
					}
					if (!viewModel.SelectedAddOnItems.Contains("Fast Charge"))
					{
						viewModel.FastChargePrice = 0;
					}
					if (!viewModel.SelectedAddOnItems.Contains("512 MB SD Card"))
					{
						viewModel.SDCardPrice = 0;
					}
					if (!viewModel.SelectedAddOnItems.Contains("2 Years Extended Warranty"))
					{
						viewModel.WarrentyPrice = 0;
					}
					if (viewModel.SelectedAddOnItems.Contains("Fast Charge") || viewModel.SelectedAddOnItems.Contains("512 MB SD Card") || viewModel.SelectedAddOnItems.Contains("2 Years Extended Warranty"))
					{
						viewModel.TotalAmount = viewModel.FastChargePrice + viewModel.SDCardPrice + viewModel.WarrentyPrice;
						viewModel.TotalPrice = "$ " + viewModel.TotalAmount;
					}
					else
					{
						viewModel.TotalAmount = viewModel.FinalPrice;
						viewModel.TotalPrice = "$ " + viewModel.TotalAmount;
					}

				}
			} ((sender as SfChipGroup).BindingContext as ChipViewModel).FinalAmount = ((sender as SfChipGroup).BindingContext as ChipViewModel).TotalAmount;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
		}
	}
}