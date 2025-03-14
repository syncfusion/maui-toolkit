using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class CartesianAxisLayout : AbsoluteLayout, IAxisLayout
	{
		#region Fields
		readonly CartesianChartArea _area;
		double _left, _bottom, _right, _top;
		List<double>? _leftSizes;
		List<double>? _rightSizes;
		List<double>? _topSizes;
		List<double>? _bottomSizes;
		readonly ChartAxisView _axisView;
		#endregion

		#region Internal Fields
		internal ObservableCollection<ChartAxis> VerticalAxes { get; set; }
		internal ObservableCollection<ChartAxis> HorizontalAxes { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="area"></param>
		public CartesianAxisLayout(CartesianChartArea area)
		{
			_area = area;
			_axisView = new ChartAxisView(area);
			AbsoluteLayout.SetLayoutBounds(_axisView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_axisView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			Add(_axisView);
			VerticalAxes = [];
			HorizontalAxes = [];
			Init();
		}
		#endregion

		#region Methods

		internal void AssignAxisToSeries()
		{
			var visibleSeries = _area.VisibleSeries;
			if (visibleSeries == null)
			{
				return;
			}

			UpdateAxisTransposed(); //Implement axis transpose.

			ClearActualAxis(visibleSeries); //Clear actual axis of the series if required. 
			UpdateActualAxis(visibleSeries);//Assign axis to series

			_area.UpdateStackingSeries(); //Calculate stacking series values. 

			UpdateSeriesRange(visibleSeries); // Create segment and update range.
		}

		void ClearActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			if (_area.RequiredAxisReset)
			{
				foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
				{
					if (series != null)
					{
						if (series.ActualXAxis != null && series.ActualYAxis != null)
						{
							series.ActualXAxis.AssociatedAxes.Clear();
							series.ActualYAxis.AssociatedAxes.Clear();
						}

						series.ActualXAxis = null;
						series.ActualYAxis = null;
					}
				}

				_area.RequiredAxisReset = false;
			}
		}

		internal void LayoutAxis(Rect bounds)
		{
			Measure(bounds.Size);

			var thickness = _area.PlotAreaMargin;
			_area.SeriesClipRect = new Rect(
				bounds.X + thickness.Left,
				bounds.Y + thickness.Top,
				bounds.Width - thickness.Left - thickness.Right, bounds.Height - thickness.Top - thickness.Bottom);
		}

		internal void InvalidateRender()
		{
			_axisView?.InvalidateDrawable();
		}

		#region Private Methods
		void Init()
		{
			_leftSizes = [];
			_topSizes = [];
			_rightSizes = [];
			_bottomSizes = [];
		}

		public Size Measure(Size availableSize)
		{
			_left = _right = _top = _bottom = 0;

			_leftSizes?.Clear();
			_rightSizes?.Clear();
			_topSizes?.Clear();
			_bottomSizes?.Clear();

			_area.ActualSeriesClipRect = MeasureAxis(availableSize);

			_area.PlotAreaMargin = new Thickness(_left, _top, _right, _bottom);

			UpdateArrangeRect(availableSize);

			UpdateCrossingAxes();

			return Size.Zero;
		}

		void UpdateAxisTransposed()
		{
			VerticalAxes = _area.IsTransposed ? _area._xAxes : new ObservableCollection<ChartAxis>(from axis in _area._yAxes select axis);
			HorizontalAxes = _area.IsTransposed ? new ObservableCollection<ChartAxis>(from axis in _area._yAxes select axis) : _area._xAxes;

			foreach (var axis in VerticalAxes)
			{
				axis.IsVertical = true;
			}

			foreach (var axis in HorizontalAxes)
			{
				axis.IsVertical = false;
			}
		}

		void UpdateArrangeRect(Size availableSize)
		{
			if (availableSize == Size.Zero)
			{
				return;
			}

			double currRight = availableSize.Width - _right;
			double currLeft = _left;
			double currTop = _top;
			double currBottom = availableSize.Height - _bottom;
			bool isFirstLeft = true, isFirstTop = true, isFirstRight = true, isFirstBottom = true;

			foreach (ChartAxis verticalAxis in VerticalAxes)
			{
				var canRenderAtCross = verticalAxis.CanRenderNextToCrossingValue();
				if (verticalAxis.IsOpposed())
				{
					currRight -= isFirstRight ? verticalAxis.InsidePadding : 0;
					double axisWidth = verticalAxis.ComputedDesiredSize.Width;
					double axisHeight = verticalAxis.ComputedDesiredSize.Height;

					verticalAxis.ArrangeRect = canRenderAtCross ? new Rect(0, _top, axisWidth, axisHeight) : new Rect(currRight, _top, axisWidth, axisHeight);

					if (!canRenderAtCross)
					{
						currRight += axisWidth;
					}

					isFirstRight = false;
				}
				else
				{
					double axisWidth = isFirstLeft ? verticalAxis.ComputedDesiredSize.Width - verticalAxis.InsidePadding :
							verticalAxis.ComputedDesiredSize.Width;
					double axisHeight = verticalAxis.ComputedDesiredSize.Height;

					if (!canRenderAtCross)
					{
						currLeft -= axisWidth;
					}

					verticalAxis.ArrangeRect = canRenderAtCross ? new Rect(0, _top, axisWidth, axisHeight) : new Rect(currLeft, _top, axisWidth, axisHeight);
					isFirstLeft = false;
				}
			}

			foreach (ChartAxis horizontalAxis in HorizontalAxes)
			{
				var canRenderAtCross = horizontalAxis.CanRenderNextToCrossingValue();
				if (horizontalAxis.IsOpposed())
				{
					double axisWidth = horizontalAxis.ComputedDesiredSize.Width;
					double axisHeight = isFirstTop ? horizontalAxis.ComputedDesiredSize.Height - horizontalAxis.InsidePadding :
							horizontalAxis.ComputedDesiredSize.Height;
					if (!canRenderAtCross)
					{
						currTop -= axisHeight;
					}

					horizontalAxis.ArrangeRect = canRenderAtCross ? new Rect(_left, 0, axisWidth, axisHeight) : new Rect(_left, currTop, axisWidth, axisHeight);
					isFirstTop = false;
				}
				else
				{
					currBottom -= isFirstBottom ? horizontalAxis.InsidePadding : 0;
					double axisWidth = horizontalAxis.ComputedDesiredSize.Width;
					double axisHeight = horizontalAxis.ComputedDesiredSize.Height;

					horizontalAxis.ArrangeRect = canRenderAtCross ? new Rect(_left, 0, axisWidth, axisHeight) : new Rect(_left, currBottom, axisWidth, axisHeight);

					if (!canRenderAtCross)
					{
						currBottom += axisHeight;
					}

					isFirstBottom = false;
				}
			}
		}

		void UpdateCrossingAxes()
		{
			foreach (var axis in VerticalAxes)
			{
				if (axis.CanRenderNextToCrossingValue())
				{
					var isOpposed = axis.IsOpposed();
					var horizontalCrossing = ValidateMinMaxWithAxisCrossingValue(axis);
					var axisWidth = axis.ComputedDesiredSize.Width;
					var axisHeight = axis.ComputedDesiredSize.Height;
					bool isTickInside = axis.TickPosition == AxisElementPosition.Inside;
					bool isLabelInside = axis.LabelsPosition == AxisElementPosition.Inside;
					double offset = isTickInside || isLabelInside ? axisWidth - axis.InsidePadding : axisWidth - _left;

					axis.ArrangeRect = isOpposed ? new Rect(horizontalCrossing + _left, _top, axisWidth, axisHeight)
						: new Rect(horizontalCrossing - offset, _top, axisWidth, axisHeight);
				}
			}

			foreach (var axis in HorizontalAxes)
			{
				if (axis.CanRenderNextToCrossingValue())
				{
					var isOpposed = axis.IsOpposed();
					var verticalCrossing = ValidateMinMaxWithAxisCrossingValue(axis);
					var axisWidth = axis.ComputedDesiredSize.Width;
					var axisHeight = axis.ComputedDesiredSize.Height;
					bool isTickInside = axis.TickPosition == AxisElementPosition.Inside;
					bool isLabelInside = axis.LabelsPosition == AxisElementPosition.Inside;
					double offset = isTickInside || isLabelInside ? -axis.InsidePadding : _top;

					axis.ArrangeRect = isOpposed ? new Rect(_left, verticalCrossing - (axisHeight - _top), axisWidth, axisHeight) : new Rect(_left, verticalCrossing + offset, axisWidth, axisHeight);

				}
			}
		}

		double ValidateMinMaxWithAxisCrossingValue(ChartAxis currentAxis)
		{
			double crossingPosition;
			var associatedAxis = currentAxis.GetCrossingAxis(_area);
			if (associatedAxis != null)
			{
				var minimum = associatedAxis.ActualRange.Start;
				var maximum = associatedAxis.ActualRange.End;
				crossingPosition = currentAxis.ActualCrossingValue < minimum ? minimum : currentAxis.ActualCrossingValue > maximum ? maximum : currentAxis.ActualCrossingValue;
				return associatedAxis.ValueToPoint(crossingPosition);
			}

			return double.NaN;
		}

		Rect MeasureAxis(Size size)
		{
			bool needLayout = true;
			bool isFirstLayout = true;
			Rect currentClipRect;
			Rect seriesClipRect = new Rect(0, 0, size.Width, size.Height);

			while (needLayout)
			{
				_top = _bottom = _left = _right = 0f;
				needLayout = false;

				_leftSizes?.Clear();
				_rightSizes?.Clear();

				MeasureVerticalAxis(VerticalAxes, new Size(size.Width, seriesClipRect.Height));

				_left = _leftSizes != null ? _leftSizes.Sum() : 0;
				_right = _rightSizes != null ? _rightSizes.Sum() : 0;

				_top = _topSizes != null && _topSizes.Count > 0 ? _topSizes.Sum() : 0;
				_bottom = _bottomSizes != null && _bottomSizes.Count > 0 ? _bottomSizes.Sum() : 0;
				var thickness = new Thickness(_left, _top, _right, _bottom);
				currentClipRect = (new Rect(0, 0, size.Width, size.Height)).SubtractThickness(thickness);

				if (Math.Abs(seriesClipRect.Width - currentClipRect.Width) > 0.5 || isFirstLayout)
				{
					_topSizes?.Clear();
					_bottomSizes?.Clear();

					seriesClipRect = currentClipRect;

					MeasureHorizontalAxis(HorizontalAxes, new Size(seriesClipRect.Width, size.Height));

					_top = _bottom = 0f;
					_top = _topSizes != null ? _topSizes.Sum() : 0;
					_bottom = _bottomSizes != null ? _bottomSizes.Sum() : 0;
					currentClipRect = (new Rect(0, 0, size.Width, size.Height)).SubtractThickness(new Thickness(_left, _top, _right, _bottom));
					needLayout = Math.Abs(seriesClipRect.Height - currentClipRect.Height) > 0.5;
					seriesClipRect = currentClipRect;
				}

				isFirstLayout = false;
			}

			return seriesClipRect;
		}

		void MeasureHorizontalAxis(ObservableCollection<ChartAxis> axes, Size size)
		{
			bool isFirstTop = true, isFirstBottom = true;

			foreach (ChartAxis chartAxis in axes)
			{
				UpdateAxisComputeSize(chartAxis, size);
				if (!chartAxis.CanRenderNextToCrossingValue())
				{
					if (chartAxis.IsOpposed())
					{
						_topSizes?.Add(isFirstTop ? chartAxis.ComputedDesiredSize.Height - chartAxis.InsidePadding :
								chartAxis.ComputedDesiredSize.Height);
						isFirstTop = false;
					}
					else
					{
						_bottomSizes?.Add(isFirstBottom ? chartAxis.ComputedDesiredSize.Height - chartAxis.InsidePadding :
								chartAxis.ComputedDesiredSize.Height);
						isFirstBottom = false;
					}
				}
			}
		}

		void MeasureVerticalAxis(ObservableCollection<ChartAxis>? axes, Size size)
		{
			bool isFirstLeft = true;
			bool isFirstRight = true;

			if (axes == null)
			{
				return;
			}

			foreach (ChartAxis chartAxis in axes)
			{
				UpdateAxisComputeSize(chartAxis, size);
				if (!chartAxis.CanRenderNextToCrossingValue())
				{
					if (chartAxis.IsOpposed())
					{
						_rightSizes?.Add(isFirstRight ? chartAxis.ComputedDesiredSize.Width - chartAxis.InsidePadding :
								chartAxis.ComputedDesiredSize.Width);
						isFirstRight = false;
					}
					else
					{
						_leftSizes?.Add(isFirstLeft ? chartAxis.ComputedDesiredSize.Width - chartAxis.InsidePadding :
								chartAxis.ComputedDesiredSize.Width);
						isFirstLeft = false;
					}
				}
			}
		}

		void UpdateAxisComputeSize(ChartAxis chartAxis, Size size)
		{
			var area = chartAxis.Area;
			if (area == null)
			{ 
				return; 
			}

			var crossingAxis = chartAxis.GetCrossingAxis(area);
			if (crossingAxis != null)
			{
				chartAxis.ComputeSize(size);
			}
			else
			{
				chartAxis.ResetComputeSize(size);
			}
		}

		public void OnDraw(ICanvas canvas)
		{
		}

		void UpdateActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			foreach (var item in visibleSeries)
			{
				if (item is CartesianSeries series)
				{
					if (series.ActualXAxis == null)
					{
						var axes = _area.IsTransposed ? VerticalAxes : HorizontalAxes;
						var xName = series.XAxisName;
						var axis = string.IsNullOrEmpty(xName) ? _area.PrimaryAxis : (GetAxisByName(xName, axes) ?? _area.PrimaryAxis);
						series.ActualXAxis = axis;
						axis?.AddRegisteredSeries(series);
					}

					if (series.ActualYAxis == null)
					{
						var axes = _area.IsTransposed ? HorizontalAxes : VerticalAxes;
						var yName = series.YAxisName;
						var axis = string.IsNullOrEmpty(yName) ? _area.SecondaryAxis : (GetAxisByName(yName, axes) ?? _area.SecondaryAxis);
						series.ActualYAxis = axis;
						axis?.AddRegisteredSeries(series);
					}

					series.UpdateAssociatedAxes();

					if (series.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex)
					{
						categoryAxis.GroupData();
					}
				}
			}
		}

		void UpdateSeriesRange(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			var isStackingSegmentCreated = false;
			foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
			{
				if (isStackingSegmentCreated && series.IsStacking && series.SegmentsCreated)
				{
					series.SegmentsCreated = false;
				}

				if (!series.IsStacking)
				{
					if (series.RequiredEmptyPointReset)
					{
						series.ResetEmptyPointIndexes();
						series.RequiredEmptyPointReset = false;
					}

					series.ValidateYValues();
				}

				if (!series.SegmentsCreated) //creates segment if segmentsCreated is false. 
				{
					if (series.IsStacking)
					{
						isStackingSegmentCreated = true;
					}

					series.XRange = DoubleRange.Empty;
					series.YRange = DoubleRange.Empty;

					if (series.SbsInfo.IsEmpty)
					{
						series.InvalidateSideBySideSeries();
					}

					if (series.IsSideBySide)
					{
						_area.CalculateSbsPosition();
					}

					InternalCreateSegments(series);
				}

				series.UpdateRange();
			}
		}

		void InternalCreateSegments(ChartSeries series)
		{
			if (_area.PlotArea is ChartPlotArea plotArea)
			{
				foreach (SeriesView seriesView in plotArea._seriesViews.Children.Cast<SeriesView>())
				{
					if (seriesView != null && seriesView.IsVisible && series == seriesView._series)
					{
						seriesView.InternalCreateSegments();
					}
				}
			}
		}

		static ChartAxis? GetAxisByName(string name, ObservableCollection<ChartAxis>? axes)
		{
			var item = (from x in axes where x.Name == name select x).ToList();
			if (item != null && item.Count > 0)
			{
				return item[0];
			}

			return null;
		}
		#endregion
		#endregion
	}
}
