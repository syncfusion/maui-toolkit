namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Enables the selection of individual or multiple series in a <see cref="SfCartesianChart"/>.
	/// </summary>
	public partial class SeriesSelectionBehavior : ChartSelectionBehavior
	{
		#region Internal Properties

		internal SfCartesianChart? Chart { get; set; }

		internal IChartArea? ChartArea { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SeriesSelectionBehavior"/> class.
		/// </summary>
		public SeriesSelectionBehavior()
		{
		}

		#endregion

		#region Internal Methods

		internal bool OnTapped(float pointX, float pointY)
		{
			ChartArea = Chart?._chartArea;
			var visibleSeries = ChartArea?.VisibleSeries;
			if (visibleSeries != null && Chart != null)
			{
				foreach (var series in visibleSeries.Reverse())
				{
					RectF bounds = series.AreaBounds;
					foreach (var segment in series._segments)
					{
						if (segment.HitTest(pointX - bounds.Left, pointY - bounds.Top))
						{
							var index = visibleSeries.IndexOf(series);
							if (IsSelectionChangingInvoked(Chart, index))
							{
								UpdateSelectionChanging(index, true);
								InvokeSelectionChangedEvent(Chart, index);
							}

							return true;
						}
					}
				}
			}

			return false;
		}

		internal override bool CanClearSelection()
		{
			return Chart != null && Chart._chartArea != null && Chart._chartArea.VisibleSeries != null;
		}

		internal override void ChangeSelectionBrushColor(object newValue)
		{
			var visibleSeries = ChartArea?.VisibleSeries;
			if (visibleSeries != null)
			{
				foreach (var series in visibleSeries)
				{
					series.UpdateColor();
					series.UpdateLegendIconColor();
					ChartSelectionBehavior.Invalidate(series);
				}
			}
		}

		internal override void UpdateSelectedItem(int index)
		{
			var visibleSeries = ChartArea?.VisibleSeries;
			if (visibleSeries != null)
			{
				if (index < visibleSeries.Count && index > -1)
				{
					var series = visibleSeries[index];
					series.UpdateColor();
					series.UpdateLegendIconColor(this, index);
					ChartSelectionBehavior.Invalidate(series);
				}
			}
		}

		internal override void ResetMultiSelection()
		{
			var selectedIndexes = ActualSelectedIndexes.ToList();
			ActualSelectedIndexes.Clear();
			foreach (var index in selectedIndexes)
			{
				UpdateSelectedItem(index);
			}
		}

		internal override void SelectionIndexChanged(int oldValue, int newValue)
		{
			if (oldValue != -1)
			{
				UpdateSelectedItem(oldValue);
			}

			if (newValue != -1 && Chart != null)
			{
				UpdateSelectedItem(newValue);
				InvokeSelectionChangedEvent(Chart, newValue);
			}
		}

		internal override void SelectionIndexesPropertyChanged(List<int> newValue)
		{
			var visibleSeries = ChartArea?.VisibleSeries;
			if (visibleSeries != null && Chart != null)
			{
				foreach (var index in newValue)
				{
					if (index < visibleSeries.Count && index > -1)
					{
						UpdateSelectionChanging(index, false);
						InvokeSelectionChangedEvent(Chart, index);
					}
				}
			}
			else
			{
				ActualSelectedIndexes = (List<int>)newValue;
			}
		}

		#endregion
	}
}