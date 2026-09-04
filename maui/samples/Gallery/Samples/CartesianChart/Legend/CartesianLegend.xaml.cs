using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class CartesianLegend : SampleView
	{
		public CartesianLegend()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			tooltipChart.Handler?.DisconnectHandler();
		}

		private void FloatingLegend_CheckedChanged(object? sender, CheckedChangedEventArgs e)
		{
			if (sender is not CheckBox checkBox || CartesianChartLegend is null)
				return;

			CartesianChartLegend.IsFloating = checkBox.IsChecked;
			if (xOffsetStepperLabel != null)
			{
				xOffsetStepperLabel.IsEnabled = checkBox.IsChecked;
				xOffsetStepperLabel.Opacity = checkBox.IsChecked ? 1 : 0.5;
			}
			if (xOffsetStepper != null)
			{
				xOffsetStepper.IsEnabled = checkBox.IsChecked;
				xOffsetStepper.Opacity = checkBox.IsChecked ? 1 : 0.5;
			}
			if (yOffsetStepperLabel != null)
			{
				yOffsetStepperLabel.IsEnabled = checkBox.IsChecked;
				yOffsetStepperLabel.Opacity = checkBox.IsChecked ? 1 : 0.5;
			}
			if (yOffsetStepper != null)
			{
				yOffsetStepper.IsEnabled = checkBox.IsChecked;
				yOffsetStepper.Opacity = checkBox.IsChecked ? 1 : 0.5;
			}
		}

		private void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender is not Microsoft.Maui.Controls.Picker value || CartesianChartLegend is null)
				return;

			int selectedIndex = value.SelectedIndex;
			if (selectedIndex == 0)
			{
				CartesianChartLegend.Placement = LegendPlacement.Top;
			}
			else if (selectedIndex == 1)
			{
				CartesianChartLegend.Placement = LegendPlacement.Left;
			}
			else if (selectedIndex == 2)
			{
				CartesianChartLegend.Placement = LegendPlacement.Bottom;
			}
			else if (selectedIndex == 3)
			{
				CartesianChartLegend.Placement = LegendPlacement.Right;
			}
		}

		int xOffset = 0;
		int yOffset = 0;

		private void OnXIncrementClicked(object? sender, EventArgs e)
		{
			if (xOffset < 100)
			{
				xOffset += 10;
				if (xOffsetValue != null)
					xOffsetValue.Text = xOffset.ToString();
				if (CartesianChartLegend != null)
					CartesianChartLegend.OffsetX = xOffset;
			}
		}

		private void OnXDecrementClicked(object? sender, EventArgs e)
		{
			if (xOffset > -100)
			{
				xOffset -= 10;
				if (xOffsetValue != null)
					xOffsetValue.Text = xOffset.ToString();
				if (CartesianChartLegend != null)
					CartesianChartLegend.OffsetX = xOffset;
			}
		}

		private void OnYIncrementClicked(object? sender, EventArgs e)
		{
			if (yOffset < 100)
			{
				yOffset += 10;
				if (yOffsetValue != null)
					yOffsetValue.Text = yOffset.ToString();
				if (CartesianChartLegend != null)
					CartesianChartLegend.OffsetY = yOffset;
			}
		}

		private void OnYDecrementClicked(object? sender, EventArgs e)
		{
			if (yOffset > -100)
			{
				yOffset -= 10;
				if (yOffsetValue != null)
					yOffsetValue.Text = yOffset.ToString();
				if (CartesianChartLegend != null)
					CartesianChartLegend.OffsetY = yOffset;
			}
		}
	}

	public partial class LineSeriesExt : LineSeries
	{
		public Geometry? PathData
		{
			get { return (Geometry)GetValue(PathDataProperty); }
			set { SetValue(PathDataProperty, value); }
		}

		public static readonly BindableProperty PathDataProperty =
	BindableProperty.Create(
		nameof(PathData),
		typeof(Geometry),
		typeof(LineSeriesExt),
		null,
		BindingMode.Default,
		null);

		public LineSeriesExt()
		{
		}
	}
}
