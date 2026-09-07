namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Enables the selection of individual or multiple data points in a series.
	/// </summary>
	public partial class DataPointSelectionBehavior : ChartSelectionBehavior
	{
		#region Internal Properties

		internal IDatapointSelectionDependent? Source { get; set; }

		#endregion

		#region Internal Method

		internal bool OnTapped(float pointX, float pointY)
		{
			if (Source != null)
			{
				RectF bounds = Source.AreaBounds;
				foreach (var segment in Source.Segments)
				{
					if (segment.HitTest(pointX - bounds.Left, pointY - bounds.Top))
					{
						var index = segment.Index;
						if (IsSelectionChangingInvoked(Source, index))
						{
							UpdateSelectionChanging(index, true);
							InvokeSelectionChangedEvent(Source, index);
						}

						return true;
					}
				}
			}

			return false;
		}

		internal override void UpdateSelectedItem(int index)
		{
			if (Source != null && index != -1)
			{
				if (index < Source.Segments.Count && index > -1)
				{
					Source.UpdateSelectedItem(index);
					Source.UpdateLegendIconColor(this, index);
					Source.Invalidate();
				}
			}
		}

		internal override void ChangeSelectionBrushColor(object newValue)
		{
			if (Source is ChartSeries series)
			{
				foreach (var segment in series._segments)
				{
					Source.UpdateSelectedItem(segment.Index);
					Source.UpdateLegendIconColor(this, segment.Index);
					Source.Invalidate();
				}
			}
		}

		internal override void ResetMultiSelection()
		{
			if (Source != null)
			{
				var selectedIndexes = ActualSelectedIndexes.ToList();
				ActualSelectedIndexes.Clear();
				foreach (var index in selectedIndexes)
				{
					if (index < Source.Segments.Count && index > -1)
					{
						Source.SetFillColor(Source.Segments[index]);
						Source.UpdateLegendIconColor(this, index);
					}
				}

				if (selectedIndexes.Count > 0)
				{
					Source.Invalidate();
				}
			}
		}

		internal override void SelectionIndexChanged(int oldValue, int newValue)
		{
			if (oldValue != -1)
			{
				UpdateSelectedItem(oldValue);
			}

			if (newValue != -1 && Source != null)
			{
				UpdateSelectedItem(newValue);
				if (Type != ChartSelectionType.Multiple && Type != ChartSelectionType.None)
				{
					InvokeSelectionChangedEvent(Source, newValue);
				}
			}
		}

		internal override bool CanClearSelection()
		{
			return Source != null && Source.Segments != null;
		}

		internal override void SelectionIndexesPropertyChanged(List<int> newValue)
		{
			if (Source != null)
			{
				foreach (int index in newValue)
				{
					UpdateSelectionChanging(index, false);
					InvokeSelectionChangedEvent(Source, index);
				}
			}
			else
			{
				ActualSelectedIndexes = newValue;
			}
		}

		#endregion
	}
}