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
			if (sender != null && sender is SfChipGroup chipGroup && chipGroup.BindingContext is ChipViewModel viewModel)
			{
				if (!string.IsNullOrEmpty("SelectedAddOnItems"))
				{
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
						viewModel.WarrantyPrice = 799;
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
						viewModel.WarrantyPrice = 0;
					}
					if (viewModel.SelectedAddOnItems.Contains("Fast Charge") || viewModel.SelectedAddOnItems.Contains("512 MB SD Card") || viewModel.SelectedAddOnItems.Contains("2 Years Extended Warranty"))
					{
						viewModel.TotalAmount = viewModel.FastChargePrice + viewModel.SDCardPrice + viewModel.WarrantyPrice;
						viewModel.TotalPrice = "$ " + viewModel.TotalAmount;
					}
					else
					{
						viewModel.TotalAmount = viewModel.FinalPrice;
						viewModel.TotalPrice = "$ " + viewModel.TotalAmount;
					}

				}

				viewModel.FinalAmount = viewModel.TotalAmount;
			}
		}
	}
}