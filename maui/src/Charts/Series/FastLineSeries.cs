using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="FastLineSeries"/> is a special kind of line series that can render a collection with a large number of data points.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="FastLineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="StrokeDashArray"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastLineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para>The FastLineSeries does not support empty points.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.XAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.XAxes>
	///
	///           <chart:SfCartesianChart.YAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.YAxes>
	///
	///           <chart:SfCartesianChart.Series>
	///               <chart:FastLineSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"/>
	///           </chart:SfCartesianChart.Series>  
	///           
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     NumericalAxis xAxis = new NumericalAxis();
	///     NumericalAxis yAxis = new NumericalAxis();
	///     
	///     chart.XAxes.Add(xAxis);
	///     chart.YAxes.Add(yAxis);
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     FastLineSeries series = new FastLineSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.YBindingPath = "YValue";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<Model> Data { get; set; }
	/// 
	///     public ViewModel()
	///     {
	///        Data = new ObservableCollection<Model>();
	///        Data.Add(new Model() { XValue = 10, YValue = 100 });
	///        Data.Add(new Model() { XValue = 20, YValue = 150 });
	///        Data.Add(new Model() { XValue = 30, YValue = 110 });
	///        Data.Add(new Model() { XValue = 40, YValue = 230 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class FastLineSeries : XYDataSeries, IDrawCustomLegendIcon
	{
		#region Internal Properties

		internal double ToleranceCoefficient { get; set; }

		internal override bool IsColorPathSeries => false;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="EnableAntiAliasing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EnableAntiAliasing"/> property indicates whether to enable smooth line drawing in the <see cref="FastLineSeries"/>. 
		/// </remarks>
		public static readonly BindableProperty EnableAntiAliasingProperty = BindableProperty.Create(
			nameof(EnableAntiAliasing),
			typeof(bool),
			typeof(FastLineSeries),
			false,
			BindingMode.Default,
			null,
			OnInvalidatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeDashArray"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="StrokeDashArray"/> property allows customization of the dash pattern used for the stroke in the <see cref="FastLineSeries"/>.
		/// </remarks>
		public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
			nameof(StrokeDashArray),
			typeof(DoubleCollection),
			typeof(FastLineSeries),
			null,
			BindingMode.Default,
			null,
			OnStrokeDashArrayPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether to enable smooth line drawing for <see cref="FastLineSeries"/>.
		/// </summary>
		/// <value> It accepts <c>bool</c> values and the default value is <c>false</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastLineSeries ItemsSource="{Binding Data}"
		///                                XBindingPath="XValue"
		///                                YBindingPath="YValue"
		///                                EnableAntiAliasing ="True" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-5)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     DoubleCollection doubleCollection = new DoubleCollection();
		///     doubleCollection.Add(5);
		///     doubleCollection.Add(3);
		///     FastLineSeries series = new FastLineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EnableAntiAliasing = true,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnableAntiAliasing
		{
			get { return (bool)GetValue(EnableAntiAliasingProperty); }
			set { SetValue(EnableAntiAliasingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke dash array to customize the appearance of stroke.
		/// </summary>
		/// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastLineSeries ItemsSource="{Binding Data}"
		///                                XBindingPath="XValue"
		///                                YBindingPath="YValue"
		///                                StrokeDashArray="5,3"
		///                                Stroke = "Red" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-7)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     DoubleCollection doubleCollection = new DoubleCollection();
		///     doubleCollection.Add(5);
		///     doubleCollection.Add(3);
		///     FastLineSeries series = new FastLineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           StrokeDashArray = doubleCollection,
		///           Stroke = new SolidColorBrush(Colors.Red),
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DoubleCollection StrokeDashArray
		{
			get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
			set { SetValue(StrokeDashArrayProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="FastLineSeries"/> class.
		/// </summary>
		public FastLineSeries() : base()
		{
			ToleranceCoefficient = 1;
		}

		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			var pathF = new PathF();
			pathF.MoveTo(5, 1);
			pathF.LineTo(5, 2);
			pathF.LineTo(6, 2);
			pathF.LineTo(6, 4);
			pathF.LineTo(7, 4);
			pathF.LineTo(7, 5);
			pathF.LineTo(8, 5);
			pathF.LineTo(8, 7);
			pathF.LineTo(9, 7);
			pathF.LineTo(9, 9);
			pathF.LineTo(10, 9);
			pathF.LineTo(10, 7);
			pathF.LineTo(11, 7);
			pathF.LineTo(11, 5);
			pathF.LineTo(12, 5);
			pathF.LineTo(12, 8);
			pathF.LineTo(11, 8);
			pathF.LineTo(11, 10);
			pathF.LineTo(10, 10);
			pathF.LineTo(10, 12);
			pathF.LineTo(9, 12);
			pathF.LineTo(9, 10);
			pathF.LineTo(8, 10);
			pathF.LineTo(8, 8);
			pathF.LineTo(7, 8);
			pathF.LineTo(7, 7);
			pathF.LineTo(6, 7);
			pathF.LineTo(6, 5);
			pathF.LineTo(5, 5);
			pathF.LineTo(5, 4);
			pathF.LineTo(4, 4);
			pathF.LineTo(4, 6);
			pathF.LineTo(3, 6);
			pathF.LineTo(3, 8);
			pathF.LineTo(2, 8);
			pathF.LineTo(2, 10);
			pathF.LineTo(1, 10);
			pathF.LineTo(1, 12);
			pathF.LineTo(0, 12);
			pathF.LineTo(0, 9);
			pathF.LineTo(1, 9);
			pathF.LineTo(1, 7);
			pathF.LineTo(2, 7);
			pathF.LineTo(2, 5);
			pathF.LineTo(3, 5);
			pathF.LineTo(3, 3);
			pathF.LineTo(4, 3);
			pathF.LineTo(4, 1);
			pathF.LineTo(5, 1);
			pathF.Close();
			canvas.FillPath(pathF);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Methods

		#region Public methods

		/// <inheritdoc />
		public override int GetDataPointIndex(float pointX, float pointY)
		{
			if (ActualXAxis != null && ActualYAxis != null && _segments != null && _segments.Count > 0)
			{
				RectF seriesBounds = AreaBounds;
				float xPos = pointX - seriesBounds.Left;
				float yPos = pointY - seriesBounds.Top;

				for (int i = 0; i < _segments.Count; i++)
				{
					if (_segments[i] is not FastLineSegment segment)
					{
						continue;
					}

					var xValues = segment._xValues;
					var yValues = segment._yValues;

					if (xValues == null || yValues == null)
					{
						return -1;
					}

					for (int j = 0; j < xValues.Count; j++)
					{
						var xVal = xValues[j];
						var yVal = yValues[j];
						float xPoint = TransformToVisibleX(xVal, yVal);
						float yPoint = TransformToVisibleY(xVal, yVal);
						if (ChartSegment.IsRectContains(xPoint, yPoint, xPos, yPos, (float)StrokeWidth))
						{
							return j;
						}
					}
				}
			}

			return -1;
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc />
		protected override ChartSegment CreateSegment()
		{
			return new FastLineSegment();
		}

		#endregion

		#region Internal Methods

		/// <inheritdoc />
		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();
			if (xValues == null || xValues.Count == 0 || ActualData == null)
			{
				return;
			}

			// Gap mode: EmptyPointMode.None with NaN Y-values present → split into per-run segments.
			if (EmptyPointMode == EmptyPointMode.None && YValues != null && ContainsNaN(YValues))
			{
				// Return any existing segments to the pool before rebuilding.
				for (int i = 0; i < _segments.Count; i++)
				{
					if (_segments[i] is FastLineSegment old)
					{
						FastLineSegmentPool.Return(old);
					}
				}

				_segments.Clear();

				var runs = GetValidRuns(YValues);
				foreach (var (start, end) in runs)
				{
					var segment = FastLineSegmentPool.Rent();
					segment.Series = this;
					segment.SeriesView = seriesView;
					segment.Item = ActualData;
					segment.SetData(xValues, YValues, start, end);
					InitiateDataLabels(segment);
					_segments.Add(segment);
				}

				return;
			}

			if (YValues == null)
			{
				return;
			}

			// Zero / Average / no-NaN path: single segment (original behavior).
			if (_segments.Count == 0)
			{
				var segment = CreateSegment() as FastLineSegment;
				if (segment != null)
				{
					segment.Series = this;
					segment.SeriesView = seriesView;
					segment.Item = ActualData;
					segment.SetData(xValues, YValues);
					InitiateDataLabels(segment);
					_segments.Add(segment);
				}
			}
			else
			{
				for (int i = 0; i < _segments.Count; i++)
				{
					if (_segments[i] is not FastLineSegment segment)
					{
						continue;
					}

					segment.SetData(xValues, YValues);
				}
			}
		}

		/// <summary>
		/// Returns the contiguous index ranges of valid (non-NaN) Y-values.
		/// Consecutive NaN values collapse into a single gap — no zero-length runs are produced.
		/// </summary>
		internal static List<(int Start, int End)> GetValidRuns(IList<double> yValues)
		{
			var runs = new List<(int, int)>();
			int count = yValues.Count;
			int i = 0;

			while (i < count)
			{
				// Skip NaN values.
				if (double.IsNaN(yValues[i]))
				{
					i++;
					continue;
				}

				// Found a valid value — find the end of this run.
				int start = i;
				while (i < count && !double.IsNaN(yValues[i]))
				{
					i++;
				}

				runs.Add((start, i - 1));
			}

			return runs;
		}

		/// <summary>
		/// Returns true when the collection contains at least one NaN value.
		/// Used to decide whether Gap-mode splitting is needed.
		/// </summary>
		private static bool ContainsNaN(IList<double> yValues)
		{
			for (int i = 0; i < yValues.Count; i++)
			{
				if (double.IsNaN(yValues[i]))
				{
					return true;
				}
			}

			return false;
		}


		internal override bool IsIndividualSegment()
		{
			return false;
		}

		internal override void InitiateDataLabels(ChartSegment segment)
		{
			if (ShowDataLabels && DataLabels.Count < PointsCount)
			{
				var fastLineSegment = segment as FastLineSegment;

				var xValues = fastLineSegment?._xValues;

				if (xValues == null)
				{
					return;
				}

				for (int i = 0; i < xValues.Count; i++)
				{
					var dataLabel = new ChartDataLabel();
					segment.DataLabels.Add(dataLabel);
					DataLabels.Add(dataLabel);
				}
			}
		}

		internal override void SetDashArray(ChartSegment segment)
		{
			segment.StrokeDashArray = StrokeDashArray;
		}

		internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
		{
			return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
		}

		internal override void DrawDataLabels(ICanvas canvas)
		{
			var dataLabeSettings = DataLabelSettings;

			if (dataLabeSettings == null || _segments == null || _segments.Count <= 0)
			{
				return;
			}

			ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

			for (int segIdx = 0; segIdx < _segments.Count; segIdx++)
			{
				if (_segments[segIdx] is not FastLineSegment segment)
				{
					continue;
				}

				var xValues = segment._xValues;
				var yValues = segment._yValues;

				if (xValues == null || yValues == null)
				{
					return;
				}

				for (int i = 0; i < xValues.Count; i++)
				{
					double x = xValues[i], y = yValues[i];
					var isDataInVisibleRange = IsDataInVisibleRange(x, y);

					if (double.IsNaN(y) || !isDataInVisibleRange)
					{
						continue;
					}

					CalculateDataPointPosition(i, ref x, ref y);
					PointF labelPoint = new PointF((float)x, (float)y);
					segment.Index = i;
					segment.LabelContent = GetLabelContent(yValues[i], SumOfValues(YValues));
					segment.LabelPositionPoint = CartesianDataLabelSettings.CalculateDataLabelPoint(this, segment, labelPoint, labelStyle);
					UpdateDataLabelAppearance(canvas, segment, dataLabeSettings, labelStyle);
				}
			}
		}

		internal override Brush? GetSegmentFillColor(int index)
		{
			var segment = _segments[0];

			if (segment != null)
			{
				return segment.Fill;
			}

			return null;
		}

		internal override void InvalidateMeasureDataLabel()
		{
			foreach (var segment in _segments)
			{
				InitiateDataLabels(segment);
				segment.OnDataLabelLayout();
			}
		}

		#endregion

		#region Nested Types

		/// <summary>
		/// A lightweight object pool that recycles <see cref="FastLineSegment"/> instances across
		/// <see cref="GenerateSegments"/> calls, reducing GC pressure when Gap-mode splitting is active.
		/// </summary>
		internal static class FastLineSegmentPool
		{
			private static readonly Stack<FastLineSegment> _pool = new Stack<FastLineSegment>();

			/// <summary>
			/// Returns a <see cref="FastLineSegment"/> from the pool (or allocates a new one).
			/// </summary>
			internal static FastLineSegment Rent()
			{
				if (_pool.Count > 0)
				{
					var segment = _pool.Pop();
					segment.Reset();
					return segment;
				}

				return new FastLineSegment();
			}

			/// <summary>
			/// Returns a used <see cref="FastLineSegment"/> to the pool for later reuse.
			/// </summary>
			internal static void Return(FastLineSegment segment)
			{
				segment.Reset();
				_pool.Push(segment);
			}
		}

		#endregion

		#region Private Methods

		static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is FastLineSeries series)
			{
				series.UpdateDashArray();
				series.InvalidateSeries();
			}
		}

		static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is FastLineSeries series)
			{
				series.InvalidateSeries();
			}
		}

		#endregion

		#endregion
	}
}