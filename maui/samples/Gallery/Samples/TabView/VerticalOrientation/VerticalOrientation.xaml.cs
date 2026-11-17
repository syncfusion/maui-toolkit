using Syncfusion.Maui.Toolkit.TabView;

namespace Syncfusion.Maui.ControlsGallery.TabView.SfTabView;

public partial class VerticalOrientation : SampleView
{
	public VerticalOrientation()
	{
		InitializeComponent();
    }

	private void indicatorplacemntcomboBox_SelectedIndexChanged(object? sender, EventArgs e)
	{
		var picker = sender as Microsoft.Maui.Controls.Picker;
		var selected = picker?.SelectedItem as string;
		if (TabView is null)
			return;

		switch (selected)
		{
			case "Left":
				TabView.IndicatorPlacement = TabIndicatorPlacement.Left;
				break;

			case "Right":
				TabView.IndicatorPlacement = TabIndicatorPlacement.Right;
				break;

			case "Fill":
				TabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
				break;
		}
	}

	private void placemntcomboBox_SelectedIndexChanged(object? sender, EventArgs e)
	{
		var picker = sender as Microsoft.Maui.Controls.Picker;
		var selected = picker?.SelectedItem as string;
		if (TabView is null)
			return;


		switch (selected)
		{
			case "Left":
				TabView.TabBarPlacement = Syncfusion.Maui.Toolkit.TabView.TabBarPlacement.Left;
				break;

			case "Right":
				TabView.TabBarPlacement = Syncfusion.Maui.Toolkit.TabView.TabBarPlacement.Right;
				break;
		}
	}
}