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
		internal override bool IsFillEmptyPoint { get { return false; } }

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

				foreach (FastLineSegment segment in _segments.Cast<FastLineSegment>())
				{
					var xValues = segment._xValues;
					var yValues = segment._yValues;

					if (xValues == null || yValues == null)
					{
						return -1;
					}

					for (int i = 0; i < xValues.Count; i++)
					{
						var xVal = xValues[i];
						var yVal = yValues[i];
						float xPoint = TransformToVisibleX(xVal, yVal);
						float yPoint = TransformToVisibleY(xVal, yVal);
						if (ChartSegment.IsRectContains(xPoint, yPoint, xPos, yPos, (float)StrokeWidth))
						{
							return i;
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

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();
			if (xValues == null || xValues.Count == 0 || ActualData == null)
			{
				return;
			}

			if (_segments.Count == 0)
			{
				if (CreateSegment() is FastLineSegment segment)
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
				foreach (FastLineSegment segment in _segments.Cast<FastLineSegment>())
				{
					segment.SetData(xValues, YValues);
				}
			}
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

			foreach (FastLineSegment segment in _segments.Cast<FastLineSegment>())
			{
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