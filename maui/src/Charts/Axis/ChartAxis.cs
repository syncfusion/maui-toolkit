using Syncfusion.Maui.Toolkit.Themes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public abstract partial class ChartAxis : Element, IThemeElement
	{
		#region Fields

		const int CRoundDecimals = 12;
		bool _isVertical;
		RectF _arrangeRect;
		double _previousLabelCoefficientValue = -1;
		CartesianChartArea? _cartesianArea;
		internal bool _isOverriddenOnCreateLabelsMethod;
		DoubleRange _actualRange = DoubleRange.Empty;
		internal static readonly int[] IntervalDivs = [10, 5, 2, 1];

		#region labelFormats
		const string DayFormat = "MMM - dd";
		const string MonthFormat = "MMM-yyyy";
		const string YearFormat = "yyyy";
		const string HourFormat = "hh:mm:ss tt";
		const string MinutesFormat = "hh:mm:ss";
		const string SecondsFormat = "hh:mm:ss";
		const string MilliSecondsFormat = "ss.fff";
		const string DefaultFormat = "dd/MMM/yyyy";
		#endregion
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the double value that represents the minimum visible value of the axis range.
		/// </summary>
		public double VisibleMinimum
		{
			get
			{
				return VisibleRange.IsEmpty ? double.NaN : VisibleRange.Start;
			}
		}

		/// <summary>
		/// Gets the double value that represents the maximum visible value of the axis range.
		/// </summary>
		public double VisibleMaximum
		{
			get
			{
				return VisibleRange.IsEmpty ? double.NaN : VisibleRange.End;
			}
		}

		#endregion

		#region Internal Properties

		internal float LeftOffset { get; set; }

		internal float TopOffset { get; set; }

		internal List<double> TickPositions { get; set; }

		internal double ActualPlotOffset { get; set; }

		internal double ActualPlotOffsetStart { get; set; }

		internal double ActualPlotOffsetEnd { get; set; }

		internal List<ChartAxis> AssociatedAxes { get; }

		internal Size ComputedDesiredSize { get; set; }

		internal double InsidePadding { get; set; }

		internal Size AvailableSize { get; set; }

		internal double ActualInterval { get; set; }

		internal DoubleRange ActualRange { get; set; }

		internal double VisibleInterval { get; set; }

		internal bool SmallTickRequired { get; set; }

		internal RectF RenderedRect { get; set; }

		internal double AxisInterval { get; set; } = double.NaN;

		internal bool IsPolarArea { get { return Parent is SfPolarChart; } }

		internal double ActualAutoScrollDelta { get; set; } = double.NaN;

		internal bool CanAutoScroll { get; set; }

		internal DoubleRange VisibleRange
		{
			get { return _actualRange; }

			set
			{
				_actualRange = value;
				OnPropertyChanged(nameof(VisibleMinimum));
				OnPropertyChanged(nameof(VisibleMaximum));
			}
		}

		internal bool IsVertical
		{
			get
			{
				return _isVertical;
			}

			set
			{
				if (_isVertical != value)
				{
					_isVertical = value;
				}
			}
		}

		internal Rect ArrangeRect
		{
			get { return _arrangeRect; }
			set
			{
				_arrangeRect = value;
				if (!IsVertical)
				{
					double left = _arrangeRect.Left + GetActualPlotOffsetStart();
					double width = Math.Max(0, _arrangeRect.Width - GetActualPlotOffset());
					double top = _arrangeRect.Top;
					RenderedRect = new Rect(left, top, width, _arrangeRect.Height);
				}
				else
				{
					double left = _arrangeRect.Left;
					double top = _arrangeRect.Top + GetActualPlotOffsetEnd();
					double height = Math.Max(0, _arrangeRect.Height - GetActualPlotOffset());
					RenderedRect = new Rect(left, top, _arrangeRect.Width, height);
				}

				LeftOffset = RenderedRect.Left - _arrangeRect.Left;
				TopOffset = RenderedRect.Top - _arrangeRect.Top;
				UpdateAxisLayout();
			}
		}

		internal CartesianChartArea? Area
		{
			get { return _cartesianArea; }
			set
			{
				if (_cartesianArea != value)
				{
					//TODO: dispose events and dispose other values.
				}

				if (value is CartesianChartArea chartArea)
				{
					_cartesianArea = chartArea;
				}
			}
		}

		internal CartesianChartArea? CartesianArea { get => _cartesianArea; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxis"/> class.
		/// </summary>
		public ChartAxis()
		{
			TickPositions = [];
			VisibleLabels = [];
			//TODO: need dispose on clear axis.
			VisibleLabels.CollectionChanged += VisibleLabels_CollectionChanged;
			AssociatedAxes = [];
			LabelsIntersectAction = AxisLabelsIntersectAction.Hide;
			RegisteredSeries = [];
			ActualPlotBands = [];
			InitializeConstructor();
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Converts a value from the chart's axis into a corresponding pixel or screen coordinate (point) on the chart.
		/// </summary>
		public float ValueToPoint(double value)
		{
			float coefficient = ValueToCoefficient(value);

			if (!IsVertical)
			{
				return RenderedRect.Width * coefficient + LeftOffset;
			}

			return RenderedRect.Height * (1 - coefficient) + TopOffset;
		}

		/// <summary>
		/// Converts a given pixel or screen coordinate (point) into a corresponding value on the chart's axis.
		/// </summary>
		public double PointToValue(double x, double y)
		{
			if (Area != null)
			{
				if (!IsVertical)
				{
					return CoefficientToValue((x - LeftOffset) / RenderedRect.Width);
				}

				return CoefficientToValue(1d - ((y - TopOffset) / RenderedRect.Height));
			}

			return double.NaN;
		}

		#endregion

		#region Protected Overrides

		/// <summary>
		/// Retrieves the actual number of intervals desired on an axis, typically based on the axis range, size, and interval settings.
		/// </summary>
		protected internal double GetActualDesiredIntervalsCount(Size availableSize)
		{
			double size = !IsVertical ? availableSize.Width : availableSize.Height;

			double spacingFactor = 1.0; //If the Axis is Vertical

			if (!IsVertical)
			{
				// Base factor for horizontal labels
				double factor = 0.6;

				// Adjust based on label rotation
				double rotationRadians = Math.Abs(LabelRotation) * Math.PI / 180;

				if (rotationRadians > 0)
				{
					// Rotated labels can be packed more densely
					factor *= 1.0 + 0.3 * Math.Sin(rotationRadians);
				}

				spacingFactor = factor;
			}

			double adjustedDesiredIntervalsCount = size * spacingFactor * MaximumLabels;
			return Math.Max(adjustedDesiredIntervalsCount / 100, 1.0);
		}

		/// <summary>
		/// Calculates the actual interval for the axis based on the provided range of values and the available size, ensuring appropriate spacing between axis labels or ticks.
		/// </summary>
		protected virtual double CalculateNiceInterval(DoubleRange actualRange, Size availableSize)
		{
			var delta = actualRange.Delta;
			var actualDesiredIntervalsCount = GetActualDesiredIntervalsCount(availableSize);
			var niceInterval = delta / actualDesiredIntervalsCount;
			var minInterval = Math.Pow(10, Math.Floor(Math.Log10(niceInterval)));

			foreach (int mul in IntervalDivs)
			{
				double currentInterval = minInterval * mul;

				if (actualDesiredIntervalsCount < (delta / currentInterval))
				{
					break;
				}

				niceInterval = currentInterval;
			}

			return niceInterval;
		}

		/// <summary>
		/// Determines the actual interval between axis labels or gridlines based on the axis range and chart size.
		/// </summary>
		protected virtual double CalculateActualInterval(DoubleRange range, Size availableSize)
		{
			return 1.0;
		}

		/// <summary>
		/// Determines whether padding is applied to the axis range, typically used to add space between the plotted data points and the axis edges.
		/// </summary>
		protected virtual DoubleRange ApplyRangePadding(DoubleRange range, double interval)
		{
			if (RegisteredSeries.Count > 0 && RegisteredSeries[0] is PolarSeries)
			{
				double minimum = Math.Floor(range.Start / interval) * interval;
				double maximum = Math.Ceiling(range.End / interval) * interval;
				return new DoubleRange(minimum, maximum);
			}

			return range;
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal virtual void OnLabelCreated(ChartAxisLabel label)
		{

		}

		/// <summary>
		/// Using this method, we can adjust the axis labels visible
		/// </summary>
		protected internal virtual void OnCreateLabels()
		{

		}

		/// <summary>
		/// The RaiseCallBackActualRangeChanged method is used to update the ActualMinimum and ActualMaximum property value while changing the actual range of ChartAxis.
		/// </summary>
		internal virtual void RaiseCallBackActualRangeChanged()
		{

		}

		/// <summary>
		/// Computes the range of values currently visible on the axis, typically after zooming or panning operations.
		/// </summary>
		protected virtual DoubleRange CalculateVisibleRange(DoubleRange actualRange, Size availableSize)
		{
			var visibleRange = actualRange;

			if (ZoomFactor < 1)
			{
				DoubleRange baseRange = actualRange;
				double start = baseRange.Start + (ZoomPosition * baseRange.Delta);
				double end = start + (ZoomFactor * baseRange.Delta);

				if (start < baseRange.Start)
				{
					end += baseRange.Start - start;
					start = baseRange.Start;
				}

				if (end > baseRange.End)
				{
					start -= end - baseRange.End;
					end = baseRange.End;
				}

				visibleRange = new DoubleRange(start, end);
			}

			return visibleRange;
		}

		internal ReadOnlyObservableCollection<ChartSeries>? GetVisibleSeries()
		{
			return IsPolarArea ? PolarArea?.VisibleSeries : _cartesianArea?.VisibleSeries;
		}

		/// <summary>
		/// Calculates the actual range of values for an axis based on the data points and axis settings.
		/// </summary>
		protected virtual DoubleRange CalculateActualRange()
		{
			var range = DoubleRange.Empty;
			var visibleSeries = GetVisibleSeries();

			if (visibleSeries == null)
			{
				return range;
			}

			if (!IsPolarArea)
			{
				foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
				{
					if (series.ActualXAxis == this)
					{
						range += series.VisibleXRange;
					}
					else if (series.ActualYAxis == this)
					{
						range += series.VisibleYRange;
					}
				}
			}
			else
			{
				foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
				{
					if (series.ActualXAxis == this)
					{
						range += series.VisibleXRange;
					}
					else if (series.ActualYAxis == this)
					{
						range += series.VisibleYRange;
					}
				}
			}

			//TODO: Modify range on technical indicator or trendline added. 
			return range;
		}

		/// <summary>
		/// Calculates the interval between visible axis labels or gridlines, considering the zoom level or any applied range restrictions.
		/// </summary>
		protected virtual double CalculateVisibleInterval(DoubleRange visibleRange, Size availableSize)
		{
			if (ZoomFactor < 1 || ZoomPosition > 0)
			{
				return EnableAutoIntervalOnZooming
						? CalculateNiceInterval(visibleRange, availableSize)
						: ActualInterval;
			}

			return ActualInterval;
		}

		/// <summary>
		/// Method implementation for Generate Labels in ChartAxis
		/// </summary>
		[RequiresUnreferencedCode("The GenerateVisibleLabels is not trim compatible")]
		internal virtual void GenerateVisibleLabels()
		{
		}

		internal virtual double CoefficientToValue(double coefficient)
		{
			double result;

			coefficient = IsInversed ? 1d - coefficient : coefficient;

			result = VisibleRange.Start + (VisibleRange.Delta * coefficient);

			return result;
		}

		internal virtual float ValueToCoefficient(double value)
		{
			double result;

			double start = VisibleRange.Start;
			double delta = VisibleRange.Delta;

			result = (value - start) / delta;

			return (float)(IsInversed ? 1f - result : result);
		}

		#endregion

		#region Internal Methods

		internal virtual DoubleRange AddDefaultRange(double start)
		{
			return new DoubleRange(start, start + 1);
		}

		internal double GetActualPlotOffsetStart()
		{
			return ActualPlotOffset == 0 ? ActualPlotOffsetStart : ActualPlotOffset;
		}

		internal double GetActualPlotOffsetEnd()
		{
			return ActualPlotOffset == 0 ? ActualPlotOffsetEnd : ActualPlotOffset;
		}

		internal double GetActualPlotOffset()
		{
			return ActualPlotOffset == 0 ? ActualPlotOffsetStart + ActualPlotOffsetEnd : ActualPlotOffset * 2;
		}

		internal void CalculateRangeAndInterval(Size plotSize)
		{
			if (IsScrolling && !double.IsNaN(AutoScrollingDelta))
			{
				var visibleRange = CalculateVisibleRange(ActualRange, plotSize);
				VisibleRange = visibleRange.IsEmpty ? ActualRange : visibleRange;
			}
			else
			{
				DoubleRange range = ValidateRange(CalculateActualRange());

				ActualInterval = CalculateActualInterval(range, plotSize);
				ActualRange = ApplyRangePadding(range, ActualInterval);

				if (!double.IsNaN(ActualAutoScrollDelta) && ActualAutoScrollDelta >= 0 && CanAutoScroll)
				{
					UpdateAutoScrollingDelta(ActualRange, ActualAutoScrollDelta);
					CanAutoScroll = false;
				}

				RaiseCallBackActualRangeChanged();

				if (ActualRangeChanged == null)
				{
					var visibleRange = CalculateVisibleRange(ActualRange, plotSize);
					VisibleRange = visibleRange.IsEmpty ? ActualRange : visibleRange;

					var visibleInterval = CalculateVisibleInterval(VisibleRange, plotSize);
					VisibleInterval = double.IsNaN(visibleInterval) || visibleInterval == 0 ? ActualInterval : visibleInterval;
				}
				else
				{
					if (ActualRangeChanged != null)
					{
						var visibleRange = CalculateVisibleRange(ActualRange, plotSize);
						VisibleRange = visibleRange.IsEmpty ? ActualRange : visibleRange;
						RaiseActualRangeChangedEvent(visibleRange, plotSize);
					}
				}

				UpdateAxisScale();
			}
		}

		internal void UpdateSmallTickRequired(int value)
		{
			SmallTickRequired = value > 0;
		}

		internal void UpdateActualPlotOffsetStart(double offset)
		{
			ActualPlotOffsetStart = double.IsNaN(offset) ? 0 : offset;
		}

		internal void UpdateActualPlotOffsetEnd(double offset)
		{
			ActualPlotOffsetEnd = double.IsNaN(offset) ? 0 : offset;
		}

		internal virtual double GetStartDoublePosition()
		{
			return double.NaN;
		}

		internal void AddVisibleLabels()
		{
			var rangeAxisBase = this as RangeAxisBase;
			bool isSmallTicksRequired = false;
			double previousPosition = double.NaN;

			if (rangeAxisBase != null)
			{
				var startPosition = GetStartDoublePosition();
				previousPosition = double.IsNaN(startPosition) ? previousPosition : startPosition;
				isSmallTicksRequired = rangeAxisBase.MinorTicksPerInterval > 0;
			}

			foreach (var item in VisibleLabels)
			{
				if (VisibleRange.Inside(item.Position))
				{
					var axisLabel = new ChartAxisLabel(item.Position, item.Content);
					TickPositions.Add(item.Position);

					if (isSmallTicksRequired)
					{
						double interval = item.Position - previousPosition;
						rangeAxisBase?.AddSmallTicksPoint(previousPosition, interval);
						previousPosition = item.Position;
					}
				}
			}
		}

		//Made this calculation in a virtual method for implementing separate logic for logarithmic axis
		internal virtual void UpdateAxisScale()
		{
			ZoomPosition = (VisibleRange.Start - ActualRange.Start) / ActualRange.Delta;
			ZoomFactor = (VisibleRange.End - VisibleRange.Start) / ActualRange.Delta;
		}

		/// <summary>
		///  Update Auto Scrolling Delta value  based on auto scrolling delta mode option.
		/// </summary>
		/// <param name="actualRange"></param>
		/// <param name="scrollingDelta"></param>
		internal virtual void UpdateAutoScrollingDelta(DoubleRange actualRange, double scrollingDelta)
		{
			if (double.IsNaN(scrollingDelta))
			{
				return;
			}

			switch (AutoScrollingMode)
			{
				case ChartAutoScrollingMode.Start:
					VisibleRange = new DoubleRange(ActualRange.Start, actualRange.Start + scrollingDelta);
					ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
					ZoomPosition = 0;
					break;
				case ChartAutoScrollingMode.End:
					VisibleRange = new DoubleRange(actualRange.End - scrollingDelta, ActualRange.End);
					ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
					ZoomPosition = 1 - ZoomFactor;
					break;
			}
		}

		internal static string GetActualLabelContent(object? value, string labelFormat)
		{
			var format = labelFormat ?? "";
			double position = double.NaN;

			if (value != null && double.TryParse(value.ToString(), out position))
			{
				position = Math.Round(position, CRoundDecimals);
			}

			if (string.IsNullOrEmpty(format))
			{
				return position.ToString();
			}

			return position.ToString(format);
		}

		/// <summary>
		/// Gets the specific formatted label depending on the interval type.
		/// </summary>
		/// <param name = "actualIntervalType" > The interval type.</param>
		/// <returns>The label format.</returns>
		internal static string GetSpecificFormattedLabel(DateTimeIntervalType actualIntervalType)
		{
			return actualIntervalType switch
			{
				DateTimeIntervalType.Days => DayFormat,
				DateTimeIntervalType.Months => MonthFormat,
				DateTimeIntervalType.Years => YearFormat,
				DateTimeIntervalType.Hours => HourFormat,
				DateTimeIntervalType.Minutes => MinutesFormat,
				DateTimeIntervalType.Seconds => SecondsFormat,
				DateTimeIntervalType.Milliseconds => MilliSecondsFormat,
				_ => DefaultFormat,
			};
		}

		internal static string GetFormattedAxisLabel(string labelFormat, object? currentDate)
		{
			if (currentDate != null && double.TryParse(currentDate.ToString(), out var value))
			{
				return DateTime.FromOADate(value).ToString(labelFormat);
			}

			return string.Empty;
		}

		/// <summary>
		/// Methods to update axis interval.
		/// </summary>
		/// <param name="interval">The axis interval.</param>
		internal void UpdateAxisInterval(double interval)
		{
			AxisInterval = Math.Abs(interval);
		}

		#endregion

		#region Private Methods

		DoubleRange ValidateRange(DoubleRange range)
		{
			if (range.IsEmpty)
			{
				range = AddDefaultRange(0d);
			}

			if (ChartDataUtils.EqualDoubleValues(range.Start, range.End))
			{
				range = AddDefaultRange(range.Start);
			}

			return range;
		}

		//TODO: Check collection changes working fine or not
		void VisibleLabels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (e.NewItems == null)
				{
					return;
				}

				if (e.NewItems[0] is ChartAxisLabel item)
				{
					var currentLabelCoefficientValue = ValueToCoefficient(item.Position);

					//Checking the position of the current label already has a label or not.
					if (double.Equals(Math.Round(_previousLabelCoefficientValue, 3), Math.Round(currentLabelCoefficientValue, 3)))
					{
						VisibleLabels?.Remove(item);
					}
					else
					{
						_previousLabelCoefficientValue = currentLabelCoefficientValue;
					}

					InvokeLabelCreated(item);
				}
			}
		}

		/// <summary>
		/// Invoke label created overrides and created event. 
		/// </summary>
		/// <param name="item"></param>
		void InvokeLabelCreated(ChartAxisLabel item)
		{
			OnLabelCreated(item);

			if (LabelCreated != null)
			{
				var content = item.Content != null ? item.Content.ToString() : string.Empty;
				var args = new ChartAxisLabelEventArgs(content, item.Position);
				LabelCreated(this, args);
				content = args.Label ?? string.Empty;
				item.Content = content;
				if (args.LabelStyle != null)
				{
					args.LabelStyle.Parent = Parent;
					item.LabelStyle = args.LabelStyle;
				}
			}
		}

		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		void UpdateLabels()
		{
			if (VisibleRange.Delta > 0)
			{
				_previousLabelCoefficientValue = -1;
				VisibleLabels?.Clear();
				TickPositions.Clear();
			}

			if (MaximumLabels > 0)
			{
				GenerateVisibleLabels();
			}
		}

		void RaiseActualRangeChangedEvent(DoubleRange visibleRange, Size plotSize)
		{
			if (ActualRangeChanged == null)
			{
				return;
			}

			var actualRangeChangedEvent = new ActualRangeChangedEventArgs(ActualRange.Start, ActualRange.End)
			{
				Axis = this
			};
			//Hook ActualRangeChanged event.
			ActualRangeChanged(this, actualRangeChangedEvent);

			var customActualRange = actualRangeChangedEvent.GetActualRange();

			if (customActualRange != ActualRange)
			{
				ActualRange = customActualRange;
				ActualInterval = CalculateActualInterval(ActualRange, plotSize);
			}

			visibleRange = actualRangeChangedEvent.GetVisibleRange();

			if (visibleRange.IsEmpty)
			{
				VisibleRange = CalculateVisibleRange(ActualRange, plotSize);
				VisibleInterval = ActualInterval;
			}
			else
			{
				VisibleRange = visibleRange;
				VisibleInterval = EnableAutoIntervalOnZooming ? CalculateNiceInterval(VisibleRange, plotSize) : ActualInterval;
			}
		}

		#endregion

		#endregion

		#region Destructor

		/// <summary>
		/// Removed unmanaged resources
		/// </summary>
		/// <exclude/>
		~ChartAxis()
		{
			VisibleLabels.CollectionChanged -= VisibleLabels_CollectionChanged;
		}

		#endregion
	}
}
