using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class SeriesSelection : SampleView
	{
		public SeriesSelection()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			hyperLinkLayout.IsVisible = !IsCardView;
#if IOS || ANDROID
			if (IsCardView)
			{
				chart.WidthRequest = 350;
				chart.HeightRequest = 400;
				chart.VerticalOptions = LayoutOptions.Start;
			}
#endif
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}

		readonly List<SolidColorBrush> _brushes =
		[
				 new SolidColorBrush(Color.FromArgb("#2A9AF3")),
				 new SolidColorBrush(Color.FromArgb("#0DC920")),
				 new SolidColorBrush(Color.FromArgb("#F5921F")),
				 new SolidColorBrush(Color.FromArgb("#E64191")),
				 new SolidColorBrush(Color.FromArgb("#2EC4B6"))
		];

		readonly List<SolidColorBrush> _alphaBrushes =
		[
				 new SolidColorBrush(Color.FromArgb("#402A9AF3")),
				 new SolidColorBrush(Color.FromArgb("#400DC920")),
				 new SolidColorBrush(Color.FromArgb("#40F5921F")),
				 new SolidColorBrush(Color.FromArgb("#40E64191")),
				 new SolidColorBrush(Color.FromArgb("#402EC4B6"))
		];

		private void checkbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			seriesSelection.Type = e.Value ? ChartSelectionType.Multiple : ChartSelectionType.SingleDeselect;
			_selectedIndexes.Clear();
			foreach (var series in chart.Series)
			{
				series.Fill = _brushes[chart.Series.IndexOf(series)];
			}
		}

		readonly List<int> _selectedIndexes = [];

		private void seriesSelection_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
		{
			foreach (var index in e.NewIndexes)
			{
				if (!_selectedIndexes.Contains(index))
				{
					_selectedIndexes.Add(index);
				}
			}

			var type = seriesSelection.Type;

			if ((type != ChartSelectionType.Multiple && e.OldIndexes.Count > 0 && e.NewIndexes.Count == 0) || (type == ChartSelectionType.Multiple && _selectedIndexes.Count == 0))
			{
				foreach (var series in chart.Series)
				{
					series.Fill = _brushes[chart.Series.IndexOf(series)];
				}
			}
			else
			{
				if (type != ChartSelectionType.Multiple || (type == ChartSelectionType.Multiple && _selectedIndexes.Count == 1))
				{
					foreach (var series in chart.Series)
					{
						series.Fill = _alphaBrushes[chart.Series.IndexOf(series)];
					}
				}

				foreach (var index in e.NewIndexes)
				{
					chart.Series[index].Fill = _brushes[index];
				}

				foreach (var index in e.OldIndexes)
				{
					chart.Series[index].Fill = _alphaBrushes[index];
					if (_selectedIndexes.Contains(index))
					{
						_selectedIndexes.Remove(index);
					}
				}
			}
		}
	}
}