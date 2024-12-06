namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class Selection : SampleView
	{
		readonly SelectionViewModel? _model;

		public Selection()
		{
			InitializeComponent();
			dataPointSelection.SelectionChanging += Chart_SelectionChanging;
			_model = chart1.BindingContext as SelectionViewModel;
		}

		private void Chart_SelectionChanging(object? sender, Syncfusion.Maui.Toolkit.Charts.ChartSelectionChangingEventArgs e)
		{
			if (_model == null)
			{
				return;
			}

			if (e.OldIndexes.Count > 0 && e.NewIndexes.Count == 0)
			{
				series1.PaletteBrushes = _model.ThemePaletteBrushes;
			}

			foreach (var index in e.NewIndexes)
			{
				series1.PaletteBrushes = _model.SelectionBrushes;
				if (_model.ThemePaletteBrushes[index] is SolidColorBrush brush)
				{
					dataPointSelection.SelectionBrush = brush;
				}
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart1.Handler?.DisconnectHandler();
		}
	}
}
