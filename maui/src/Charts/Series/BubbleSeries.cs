using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="BubbleSeries"/> displays a collection of data points represented by a bubble of different size.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="BubbleSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>MaximumRadius - </b> Specifies the maximum radius to the bubble series <see cref="MaximumRadius"/> property.</para>
	/// <para> <b>MinimumRadius - </b> Specifies the minimum radius to the bubble series <see cref="MinimumRadius"/> property.</para>
	/// <para> <b>SizeValuePath - </b> Specify the bubble size using the <see cref="SizeValuePath"/> property.</para>
	/// <para> <b>ShowZeroSizeBubbles - </b> Specifies the option to show zero size bubble, when its true the zero size bubble render with minimum radius <see cref="ShowZeroSizeBubbles"/> property.</para>
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="BubbleSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="BubbleSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
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
	///               <chart:BubbleSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"
	///                   SizeValuePath="Size"/>
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
	///     BubbleSeries series = new BubbleSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.YBindingPath = "YValue";
	///     series.SizeValuePath = "Size";
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
	///        Data.Add(new Model() { XValue = 10, YValue = 100, Size = 10 });
	///        Data.Add(new Model() { XValue = 20, YValue = 150, Size = 50 });
	///        Data.Add(new Model() { XValue = 30, YValue = 110, Size = 20 });
	///        Data.Add(new Model() { XValue = 40, YValue = 230, Size = 60 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class BubbleSeries : XYDataSeries, IDrawCustomLegendIcon
	{
		#region Fields

		readonly List<double> _sizeValues;

		#endregion

		#region Internal Properties

		internal override bool IsMultipleYPathRequired => true;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="MaximumRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the maximum radius for the bubbles in the series.
		/// </remarks>
		public static readonly BindableProperty MaximumRadiusProperty = BindableProperty.Create(
			nameof(MaximumRadius),
			typeof(double),
			typeof(BubbleSeries),
			10d,
			BindingMode.Default,
			null,
			OnMaximumRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="MinimumRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the minimum radius for the bubbles in the series.
		/// </remarks>
		public static readonly BindableProperty MinimumRadiusProperty = BindableProperty.Create(
			nameof(MinimumRadius),
			typeof(double),
			typeof(BubbleSeries),
			3d,
			BindingMode.Default,
			null,
			OnMinimumRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the stroke brush used for the outline of the bubbles.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(BubbleSeries),
			SolidColorBrush.Transparent,
			BindingMode.Default,
			null,
			OnStrokeChanged);

		/// <summary>
		/// Identifies the <see cref="SizeValuePath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Specifies the path to the property that determines the size of the bubbles.
		/// </remarks>
		public static readonly BindableProperty SizeValuePathProperty = BindableProperty.Create(
			nameof(SizeValuePath),
			typeof(string),
			typeof(BubbleSeries),
			string.Empty,
			BindingMode.Default,
			null,
			OnSizeValuePathChanged);

		/// <summary>
		/// Identifies the <see cref="ShowZeroSizeBubbles"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines whether to display bubbles with a size of zero.
		/// </remarks>
		public static readonly BindableProperty ShowZeroSizeBubblesProperty = BindableProperty.Create(
			nameof(ShowZeroSizeBubbles),
			typeof(bool),
			typeof(BubbleSeries),
			true,
			BindingMode.Default,
			null,
			OnShowZeroSizeBubblesChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value to customize the border appearance of the bubble.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/>, and its default is Transparent.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BubbleSeries ItemsSource="{Binding Data}"
		///                               XBindingPath="XValue"
		///                               YBindingPath="YValue"
		///                               SizeValuePath="Size"
		///                               Stroke ="Red" />
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
		///     BubbleSeries series = new BubbleSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           SizeValuePath = "Size",
		///           Stroke = new SolidColorBrush(Colors.Red)
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a path value on the source object to serve a size to the bubble series.
		/// </summary>
		/// <value>It accepts <see cref="string"/> and its default is <c>string.Empty</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BubbleSeries ItemsSource="{Binding Data}"
		///                               XBindingPath="XValue"
		///                               YBindingPath="YValue"
		///                               SizeValuePath="Size"/>
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
		///     BubbleSeries series = new BubbleSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           SizeValuePath = "Size",
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string SizeValuePath
		{
			get { return (string)GetValue(SizeValuePathProperty); }
			set { SetValue(SizeValuePathProperty, value); }
		}

		/// <summary>
		/// Gets or sets maximum radius to the bubble series.  
		/// </summary>
		/// <value>It accepts <see cref="double"/> and its default is 10.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-8)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BubbleSeries ItemsSource="{Binding Data}"
		///                               XBindingPath="XValue"
		///                               YBindingPath="YValue"
		///                               SizeValuePath="Size"
		///                               MaximumRadius="15" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-9)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     BubbleSeries series = new BubbleSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           SizeValuePath = "Size",
		///           MaximumRadius = 15,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double MaximumRadius
		{
			get { return (double)GetValue(MaximumRadiusProperty); }
			set { SetValue(MaximumRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets minimum radius to the bubble series.  
		/// </summary>
		/// <value>It accepts <see cref="double"/> and its default is 3.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-10)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BubbleSeries ItemsSource="{Binding Data}"
		///                               XBindingPath="XValue"
		///                               YBindingPath="YValue"
		///                               SizeValuePath="Size"
		///                               MinimumRadius="5" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-11)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     BubbleSeries series = new BubbleSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           SizeValuePath = "Size",
		///           MinimumRadius = 5,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double MinimumRadius
		{
			get { return (double)GetValue(MinimumRadiusProperty); }
			set { SetValue(MinimumRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets the option to show zero size bubble. When it's true, the zero-size bubble renders with a minimum radius.
		/// </summary>
		/// <value>It accepts <see cref="bool"/> and its default is true.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-12)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BubbleSeries ItemsSource="{Binding Data}"
		///                               XBindingPath="XValue"
		///                               YBindingPath="YValue"
		///                               SizeValuePath="Size"
		///                               ShowZeroSizeBubbles="False"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-13)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     BubbleSeries series = new BubbleSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           SizeValuePath = "Size",
		///           ShowZeroSizeBubbles = false,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowZeroSizeBubbles
		{
			get { return (bool)GetValue(ShowZeroSizeBubblesProperty); }
			set { SetValue(ShowZeroSizeBubblesProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BubbleSeries"/> class.
		/// </summary>
		public BubbleSeries() : base()
		{
			_sizeValues = [];
		}

		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			RectF circleRect1 = new(9, 3, 2, 2);
			canvas.SetFillPaint(fillColor, circleRect1);
			canvas.FillEllipse(circleRect1);

			RectF circleRect2 = new((float)2.5, (float)3.5, (float)1.5, (float)1.5);
			canvas.SetFillPaint(fillColor, circleRect2);
			canvas.FillEllipse(circleRect2);

			RectF circleRect3 = new(6, 6, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect3);
			canvas.FillEllipse(circleRect3);

			RectF circleRect4 = new(3, 9, 2, 2);
			canvas.SetFillPaint(fillColor, circleRect4);
			canvas.FillEllipse(circleRect4);

			RectF circleRect5 = new((float)9.5, (float)8.5, (float)1.5, (float)1.5);
			canvas.SetFillPaint(fillColor, circleRect5);
			canvas.FillEllipse(circleRect5);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Methods

		#region Protected Override Methods

		/// <inheritdoc/>
		protected override ChartSegment? CreateSegment()
		{
			return new BubbleSegment();
		}

		#endregion

		#region Internal Methods

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();

			if (xValues == null || _sizeValues.Count == 0)
			{
				return;
			}

			double maximumSizeValue = _sizeValues[0];

			double segmentRadius;

			for (int i = 0; i < _sizeValues.Count; i++)
			{
				if (_sizeValues[i] > maximumSizeValue)
				{
					maximumSizeValue = _sizeValues[i];
				}
			}

			var radius = MaximumRadius - MinimumRadius;

			for (int i = 0; i < PointsCount; i++)
			{
				var bubbleSize = _sizeValues[i];
				double relativeSize = radius * (Math.Abs(bubbleSize) / maximumSizeValue);
				segmentRadius = !ShowZeroSizeBubbles && bubbleSize == 0d ? 0d : MinimumRadius +
					(double.IsNaN(relativeSize) ? 0 : relativeSize);

				if (i < _segments.Count)
				{
					_segments[i].SetData([xValues[i], YValues[i], bubbleSize, segmentRadius]);
				}
				else
				{
					CreateSegment(i, xValues, bubbleSize, segmentRadius);
				}
			}
		}

		internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			if (Chart == null)
			{
				return;
			}

			if (_segments[tooltipInfo.Index] is BubbleSegment bubbleSegment)
			{
				RectF targetRect = bubbleSegment.SegmentBounds;

				float xPosition = targetRect.X;
				float yPosition = targetRect.Y;
				float height = targetRect.Height;
				float width = targetRect.Width;

				if ((xPosition + width / 2 + seriesBounds.Left) == seriesBounds.Left)
				{
					targetRect = new Rect(xPosition + width / 2, yPosition, width / 2, height);
					tooltipInfo.Position = TooltipPosition.Right;
				}
				else if ((xPosition + width / 2) == seriesBounds.Width)
				{
					targetRect = new Rect(xPosition, yPosition, width, height);
					tooltipInfo.Position = TooltipPosition.Left;
				}

				tooltipInfo.TargetRect = targetRect;
			}
		}

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
		{
			if (dataLabel is BubbleSegment label)
			{
				return DataLabelSettings.GetLabelPositionForSeries(this, labelSize, labelPosition, padding, new Size(label.Radius, label.Radius * 2));
			}

			return PointF.Zero;
		}

		internal override void OnDataSourceChanged(object oldValue, object newValue)
		{
			ResetAutoScroll();
			YValues.Clear();
			_sizeValues.Clear();
			GeneratePoints([YBindingPath, SizeValuePath], YValues, _sizeValues);
		}

		internal override void GenerateDataPoints()
		{
			base.GenerateDataPoints();
			GeneratePoints([YBindingPath, SizeValuePath], YValues, _sizeValues);
		}

		internal override void OnBindingPathChanged()
		{
			ResetData();
			_sizeValues.Clear();
			GeneratePoints([YBindingPath, SizeValuePath], YValues, _sizeValues);
			SegmentsCreated = false;

			if (Chart != null)
			{
				Chart.IsRequiredDataLabelsMeasure = true;
			}

			ScheduleUpdateChart();
		}

		internal override void ResetEmptyPointIndexes()
		{
			if (EmptyPointIndexes.Length != 0)
			{
				if (EmptyPointIndexes[0] != null)
				{
					foreach (var index in EmptyPointIndexes[0])
					{
						if (YValues != null && YValues.Count != 0)
						{
							YValues[(int)index] = double.NaN;
						}
					}
				}

				if (EmptyPointIndexes[1] != null)
				{
					foreach (var index in EmptyPointIndexes[1])
					{
						if (_sizeValues != null && _sizeValues.Count != 0)
						{
							_sizeValues[(int)index] = double.NaN;
						}
					}
				}
			}
		}

		internal override void ValidateYValues()
		{
			bool yValues = YValues.Any(value => double.IsNaN(value));
			bool values = _sizeValues.Any(value => double.IsNaN(value));

			if ((yValues || values) && SeriesYValues != null)
			{
				ValidateDataPoints(SeriesYValues);
			}
		}
		
		#endregion

		#region Private Methods

		static void OnMaximumRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BubbleSeries series)
			{
				series.SegmentsCreated = false;
				series.ScheduleUpdateChart();
			}
		}

		static void OnMinimumRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BubbleSeries series)
			{
				series.SegmentsCreated = false;
				series.ScheduleUpdateChart();
			}
		}

		static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BubbleSeries series)
			{
				series.UpdateStrokeColor();
				series.InvalidateSeries();
			}
		}

		static void OnSizeValuePathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BubbleSeries series)
			{
				series.OnBindingPathChanged();
			}
		}

		static void OnShowZeroSizeBubblesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BubbleSeries series)
			{
				series.SegmentsCreated = false;
				series.ScheduleUpdateChart();
			}
		}

		void CreateSegment(int i, List<double> xValues, double bubbleSize, double segmentRadius)
		{
			if (CreateSegment() is not BubbleSegment segment || ActualData == null)
			{
				return;
			}

			segment.Series = this;
			segment.Index = i;
			segment.Item = ActualData[i];
			segment.SetData([xValues[i], YValues[i], bubbleSize, segmentRadius]);
			InitiateDataLabels(segment);
			_segments.Add(segment);

			if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > i && OldSegments[i] is BubbleSegment oldSegment)
			{
				segment.SetPreviousData([oldSegment.CenterX, oldSegment.CenterY, oldSegment.Radius]);
			}
		}

		#endregion

		#endregion
	}
}