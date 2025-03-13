using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="FastScatterSeries"/> is a special kind of scatter series that can render a collection with a large number of data points.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="FastScatterSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="XYDataSeries.StrokeWidth"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastScatterSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para>Considering performance, animation, data labels, selection, and palette brushes are currently not supported for the <see cref="FastScatterSeries"/>.</para>
	/// <para>The FastScatterSeries does not support empty points.</para>
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
	///               <chart:FastScatterSeries
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
	///     FastScatterSeries series = new FastScatterSeries();
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
	public partial class FastScatterSeries : XYDataSeries, IDrawCustomLegendIcon
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="PointHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PointHeight"/> property defines the height of the fastscatter segment size.
		/// </remarks>
		public static readonly BindableProperty PointHeightProperty = BindableProperty.Create(nameof(PointHeight), typeof(double), typeof(FastScatterSeries), 5d, BindingMode.Default, null, OnScatterInvalidatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PointWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PointWidth"/> property defines the width of the fastscatter segment size.
		/// </remarks>
		public static readonly BindableProperty PointWidthProperty = BindableProperty.Create(nameof(PointWidth), typeof(double), typeof(FastScatterSeries), 5d, BindingMode.Default, null, OnScatterInvalidatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Stroke"/> property helps to customize the stroke appearance of the fastscatter segment.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(FastScatterSeries), null, BindingMode.Default, null, OnStrokeColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Type"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Type"/> property indicates the shape of the fastscatter segment.
		/// </remarks>
		public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(ShapeType), typeof(FastScatterSeries), ShapeType.Circle, BindingMode.Default, null, OnScatterInvalidatePropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that defines the height of the fastscatter segment size.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is 5.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastScatterSeries ItemsSource = "{Binding Data}"
		///                               XBindingPath = "XValue"
		///                               YBindingPath = "YValue"
		///                               PointHeight = "20"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-5)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     FastScatterSeries series = new FastScatterSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           PointHeight = 20,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double PointHeight
		{
			get { return (double)GetValue(PointHeightProperty); }
			set { SetValue(PointHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that defines the width of the fastscatter segment size.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is 5.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastScatterSeries ItemsSource = "{Binding Data}"
		///                               XBindingPath = "XValue"
		///                               YBindingPath = "YValue"
		///                               PointWidth = "20"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-5)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     FastScatterSeries series = new FastScatterSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           PointWidth = 20,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double PointWidth
		{
			get { return (double)GetValue(PointWidthProperty); }
			set { SetValue(PointWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the stroke appearance of the fastscatter segment.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-8)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastScatterSeries ItemsSource = "{Binding Data}"
		///                               XBindingPath = "XValue"
		///                               YBindingPath = "YValue"
		///                               StrokeWidth = "2"
		///                               Stroke = "Red" />
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
		///     FastScatterSeries series = new FastScatterSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Stroke = new SolidColorBrush(Colors.Red),
		///           StrokeWidth = 2,
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
		/// Gets or sets a value that indicates the shape of the fastscatter segment.
		/// </summary>
		/// <value>It accepts <see cref="ShapeType"/> values and its default value is <see cref="ShapeType.Circle"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-10)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:FastScatterSeries ItemsSource = "{Binding Data}"
		///                               XBindingPath = "XValue"
		///                               YBindingPath = "YValue"
		///                               Type = "Diamond"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-11)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     FastScatterSeries series = new FastScatterSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Type = ShapeType.Diamond,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public ShapeType Type
		{
			get { return (ShapeType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		internal override bool IsFillEmptyPoint { get { return false; } }

		#endregion

		#region Constructor

		/// <summary>
		/// Initialize the constructor
		/// </summary>
		public FastScatterSeries() : base()
		{

		}
		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			RectF circleRect1 = new(3, 6, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect1);
			canvas.FillEllipse(circleRect1);

			RectF circleRect2 = new(6, 3, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect2);
			canvas.FillEllipse(circleRect2);

			RectF circleRect3 = new(8, 6, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect3);
			canvas.FillEllipse(circleRect3);

			RectF circleRect4 = new(2, 2, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect4);
			canvas.FillEllipse(circleRect4);

			RectF circleRect5 = new(10, 2, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect5);
			canvas.FillEllipse(circleRect5);

			RectF circleRect6 = new(6, 10, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect6);
			canvas.FillEllipse(circleRect6);

			RectF circleRect7 = new(10, 9, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect7);
			canvas.FillEllipse(circleRect7);

			RectF circleRect8 = new(2, 10, 1, 1);
			canvas.SetFillPaint(fillColor, circleRect8);
			canvas.FillEllipse(circleRect8);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Methods

		#region Override Methods

		/// <inheritdoc/>
		protected override ChartSegment? CreateSegment()
		{
			return new FastScatterSegment();
		}

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();

			if (xValues == null || xValues.Count == 0 || ActualData == null)
			{
				return;
			}

			if (_segments.Count == 0)
			{
				var segment = CreateSegment() as FastScatterSegment;

				if (segment != null)
				{
					segment.Series = this;
					segment.SeriesView = seriesView;
					segment.Item = ActualData;
					segment.SetData(xValues, YValues);
					_segments.Add(segment);
				}
			}
			else
			{
				foreach (var segment in _segments)
				{
					if (segment is FastScatterSegment fastScatterSegment)
					{
						fastScatterSegment.SetData(xValues, YValues);
					}
				}
			}
		}

		/// <inheritdoc/>
		public override int GetDataPointIndex(float pointX, float pointY)
		{
			RectF seriesBounds = AreaBounds;
			float xPos = pointX - seriesBounds.Left;
			float yPos = pointY - seriesBounds.Top;

			foreach (var item in _segments)
			{
				if (!(item is FastScatterSegment segment))
				{
					continue;
				}

				var xValues = segment.XValues;
				var yValues = segment.YValues;

				if (xValues == null || yValues == null)
				{
					return -1;
				}

				for (int i = 0; i < xValues.Count; i++)
				{
					var xval = xValues[i];
					var yval = yValues[i];
					float xPoint = TransformToVisibleX(xval, yval);
					float yPoint = TransformToVisibleY(xval, yval);

					if (ChartSegment.IsRectContains(xPoint, yPoint, xPos, yPos, (float)StrokeWidth))
					{
						return i;
					}
				}
			}

			return -1;
		}

		#endregion

		#region Private Methods

		static void OnScatterInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is FastScatterSeries series)
			{
				series.InvalidateSeries();
			}
		}

		static void OnStrokeColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is FastScatterSeries series)
			{
				series.UpdateStrokeColor();
				series.InvalidateSeries();
			}
		}

		#endregion

		#endregion
	}
}
