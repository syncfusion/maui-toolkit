using System.Collections;
using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="BoxAndWhiskerSeries"/> class represents a series that plots box-and-whisker diagrams in a <see cref="SfCartesianChart"/>.
	/// </summary>
	/// <remarks>
	/// <para>A box-and-whisker plot is a chart that shows the distribution of a dataset, indicating the median, quartiles, and outliers.</para>
	/// <para>To render a series, create an instance of <see cref="BoxAndWhiskerSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>,<see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="BoxAndWhiskerSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> Customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para><b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
	/// <para><b>Width - </b> To specify the width of the box using the <see cref="Width"/> property.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.XAxes>
	///               <chart:CategoryAxis/>
	///           </chart:SfCartesianChart.XAxes>
	///
	///           <chart:SfCartesianChart.YAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.YAxes>
	///
	///           <chart:SfCartesianChart.Series>
	///               <chart:BoxAndWhiskerSeries
	///                   ItemsSource="{Binding BoxWhiskerData}"
	///                   XBindingPath="Department"
	///                   YBindingPath="Age"/>
	///           </chart:SfCartesianChart.Series>  
	///           
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     CategoryAxis xAxis = new CategoryAxis();
	///     NumericalAxis yAxis = new NumericalAxis();
	///     
	///     chart.XAxes.Add(xAxis);
	///     chart.YAxes.Add(yAxis);
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries();
	///     series.ItemsSource = viewModel.BoxWhiskerData;
	///     series.XBindingPath = "Department";
	///     series.YBindingPath = "Age";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<Model> BoxWhiskerData { get; set; }
	/// 
	///     public ViewModel()
	///     {
	///         BoxWhiskerData= new ObservableCollection<Model>(); 
	///         BoxWhiskerData.Add(new Model(){Department="Development",Age=new List<double>{22, 22,23,25,25,25,26,27,27,28,28,29,30,32,34,32, 34,36,35,38};
	///         BoxWhiskerData.Add(new Model(){Department="HR",Age=new List<double>{22, 24, 25, 30, 32, 34, 36, 38, 39, 41, 35, 36, 40, 56};
	///         BoxWhiskerData.Add(new Model(){Department="Finance",Age=new List<double>{26, 27, 28, 30, 32, 34, 35, 37, 35, 37, 45};
	///         BoxWhiskerData.Add(new Model(){Department="Inventory",Age=new List<double>{21, 23, 24, 25, 26, 27, 28, 30, 34, 36, 38};
	///         BoxWhiskerData.Add(new Model(){Department="R&D",Age=new List<double>{27, 26, 28, 29, 29, 29, 32, 35, 32, 38, 53};
	///         BoxWhiskerData.Add(new Model(){Department="Graphics",Age=new List<double>{26, 27, 29, 32, 34, 35, 36, 37, 38, 39, 41, 43, 58};
	///      }
	///         
	/// ]]></code>
	/// ***
	/// </example>
	public partial class BoxAndWhiskerSeries : XYDataSeries, IDrawCustomLegendIcon
	{
		#region Private Field

		bool _isEvenList;

		#endregion

		#region Internal Properties

		internal List<IList<double>>? YDataCollection { get; set; }

		internal bool IsOutlierTouch { get; set; }

		internal override bool IsSideBySide => true;

		#endregion

		#region BindableProperties
		/// <summary>
		/// Identifies the <see cref="BoxPlotMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the mode of the box plot (Exclusive or Inclusive).
		/// </remarks>
		public static readonly BindableProperty BoxPlotModeProperty = BindableProperty.Create(
			nameof(BoxPlotMode),
			typeof(BoxPlotMode),
			typeof(BoxAndWhiskerSeries),
			BoxPlotMode.Exclusive,
			BindingMode.Default,
			null,
			OnBoxPlotModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="OutlierShapeType"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the shape used for outliers in the box plot.
		/// </remarks>
		public static readonly BindableProperty OutlierShapeTypeProperty = BindableProperty.Create(
			nameof(OutlierShapeType),
			typeof(ShapeType),
			typeof(BoxAndWhiskerSeries),
			ShapeType.Circle,
			BindingMode.Default,
			null,
			OnShapeTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowMedian"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines whether to display the median line in the box plot.
		/// </remarks>
		public static readonly BindableProperty ShowMedianProperty = BindableProperty.Create(
			nameof(ShowMedian),
			typeof(bool),
			typeof(BoxAndWhiskerSeries),
			false,
			BindingMode.Default,
			null,
			OnMedianPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowOutlier"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines whether to display outliers in the box plot.
		/// </remarks>
		public static readonly BindableProperty ShowOutlierProperty = BindableProperty.Create(
			nameof(ShowOutlier),
			typeof(bool),
			typeof(BoxAndWhiskerSeries),
			true,
			BindingMode.Default,
			null,
			OnBoxPlotModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Spacing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the spacing between box plots in the series.
		/// </remarks>
		public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
			nameof(Spacing),
			typeof(double),
			typeof(BoxAndWhiskerSeries),
			0d,
			BindingMode.Default,
			propertyChanged: OnSpacingChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the stroke brush used for the box plot series.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(BoxAndWhiskerSeries),
			SolidColorBrush.Black,
			BindingMode.Default,
			propertyChanged: OnStrokeColorChanged);

		/// <summary>
		/// Identifies the <see cref="Width"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the width of the box plots in the series.
		/// </remarks>
		public static readonly BindableProperty WidthProperty = BindableProperty.Create(
			nameof(Width),
			typeof(double),
			typeof(BoxAndWhiskerSeries),
			0.8d,
			BindingMode.Default,
			propertyChanged: OnWidthChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the plot mode, which defines how the box plot series should be rendered.
		/// </summary>
		/// <value>It accepts the <see cref="Charts.BoxPlotMode"/> values and its default value is <see cref="BoxPlotMode.Exclusive"/>.</value>
		/// <remarks>
		/// <para> <b><see cref="BoxPlotMode.Normal"/> - </b> This is the default value, which plots the minimum, maximum, median, and quartiles of the data.</para>
		/// <para> <b><see cref="BoxPlotMode.Inclusive"/> - </b> This plots all the data values within the interquartile range (IQR), in addition to the minimum, maximum, median, and quartiles.</para>
		/// <para> <b><see cref="BoxPlotMode.Exclusive"/> - </b> This plots all the data values outside the interquartile range (IQR), in addition to the minimum, maximum, median, and quartiles.</para>
		/// </remarks>
		/// 
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///         <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                    XBindingPath="Department"
		///                                    YBindingPath="Age"
		///                                    BoxPlotMode = "Inclusive" />
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
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           BoxPlotMode = BoxPlotMode.Inclusive,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public BoxPlotMode BoxPlotMode
		{
			get { return (BoxPlotMode)GetValue(BoxPlotModeProperty); }
			set { SetValue(BoxPlotModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the shape type for the outlier.
		/// </summary>
		/// <value>It accepts <see cref="ShapeType"/> values and its default value is <see cref="ShapeType.Circle"/>.</value>
		///  <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     OutlierShapeType="Plus" />
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
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           OutlierShapeType = ShapeType.Plus,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ShapeType OutlierShapeType
		{
			get { return (ShapeType)GetValue(OutlierShapeTypeProperty); }
			set { SetValue(OutlierShapeTypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the median in the box plot or not. 
		/// </summary>
		/// <value>Its accepts the <c>bool</c> values and its default is <c>False</c></value>
		/// <example>
		/// # [Xaml](#tab/tabid-8)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     ShowMedian="True"/>
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
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           ShowMedian = true,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowMedian
		{
			get { return (bool)GetValue(ShowMedianProperty); }
			set { SetValue(ShowMedianProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether to show or hide the outlier of the box plot.
		/// </summary>
		/// <value>Its accepts the <c>bool</c> values and its default is <c>True</c></value>
		/// <example>
		/// # [Xaml](#tab/tabid-10)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     ShowOutlier="False" />
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
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           ShowOutlier = false,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowOutlier
		{
			get { return (bool)GetValue(ShowOutlierProperty); }
			set { SetValue(ShowOutlierProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the border's appearance of the BoxAndWhisker series.
		/// </summary>
		/// <value>It accept the <see cref="Brush"/> values and its default is <c>Black</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-12)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     Stroke="Red" />
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
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           Stroke = new SolidColorBrush(Colors.Red),
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
		/// Gets or sets a value to indicate gap between the segments across the series.
		/// </summary>
		/// <value> It accepts <c>double</c> values ranging from 0 to 1, where the default value is 0.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-14)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     Spacing="0.5" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-15)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           Spacing = 0.5,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double Spacing
		{
			get { return (double)GetValue(SpacingProperty); }
			set { SetValue(SpacingProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to indicate the width of the box plot.
		/// </summary>
		/// <value>It accepts <c>double</c> values ranging from 0 to 1, where the default value is <c>0.8</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-16)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
		///                                     XBindingPath="Department"
		///                                     YBindingPath="Age"
		///                                     Width="0.7"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-17)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
		///     {
		///           ItemsSource = viewModel.BoxWhiskerData,
		///           XBindingPath = "Department",
		///           YBindingPath = "Age",
		///           Width = 0.7,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			RectF innerRect1 = new(1, 4, 3, 6);
			canvas.SetFillPaint(fillColor, innerRect1);
			canvas.FillRectangle(innerRect1);

			RectF innerRect2 = new(1, 2, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect2);
			canvas.FillRectangle(innerRect2);

			RectF innerRect3 = new(1, 6, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect3);
			canvas.FillRectangle(innerRect3);

			RectF innerRect4 = new(8, 4, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect4);
			canvas.FillRectangle(innerRect4);

			RectF innerRect5 = new(8, 0, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect5);
			canvas.FillRectangle(innerRect5);

			RectF innerRect6 = new(8, 9, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect6);
			canvas.FillRectangle(innerRect6);

			RectF innerRect7 = new(1, 11, 3, 1);
			canvas.SetFillPaint(fillColor, innerRect7);
			canvas.FillRectangle(innerRect7);

			RectF innerRect8 = new(8, 2, 3, 6);
			canvas.SetFillPaint(fillColor, innerRect8);
			canvas.FillRectangle(innerRect8);

			RectF innerRect9 = new(2, 2, 1, 3);
			canvas.SetFillPaint(fillColor, innerRect9);
			canvas.FillRectangle(innerRect9);

			RectF innerRect10 = new(2, 9, 1, 3);
			canvas.SetFillPaint(fillColor, innerRect10);
			canvas.FillRectangle(innerRect10);

			RectF innerRect11 = new(9, 7, 1, 3);
			canvas.SetFillPaint(fillColor, innerRect11);
			canvas.FillRectangle(innerRect11);

			RectF innerRect12 = new(9, 0, 1, 3);
			canvas.SetFillPaint(fillColor, innerRect12);
			canvas.FillRectangle(innerRect12);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}

		}

		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected override ChartSegment CreateSegment()
		{
			return new BoxAndWhiskerSegment();
		}

		#endregion

		#region Internal Methods 

		internal override void ValidateYValues()
		{
		}

		internal override void OnDataSourceChanged(object oldValue, object newValue)
		{
			if (YDataCollection != null)
			{
				YDataCollection.Clear();
			}
			else
			{
				YDataCollection = [];
			}

			base.OnDataSourceChanged(oldValue, newValue);
		}

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override double GetActualSpacing()
		{
			return Spacing;
		}

		internal override double GetActualWidth()
		{
			return Width;
		}

		internal override void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && enumerator.MoveNext())
			{
				var currObj = enumerator.Current;

				FastReflection xProperty = new FastReflection();

				if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
				{
					return;
				}

				XValueType = GetDataType(xProperty, enumerable);

				if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
					XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
				{
					if (ActualXValues is not List<double>)
					{
						ActualXValues = XValues = new List<double>();
					}
				}
				else
				{
					if (ActualXValues is not List<string>)
					{
						ActualXValues = XValues = new List<string>();
					}
				}

				FastReflection yProperty = new FastReflection();

				if (!yProperty.SetPropertyName(yPaths[0], currObj) || yProperty.IsArray(currObj))
				{
					return;
				}

				if (XValueType == ChartValueType.String)
				{
					if (XValues is List<string> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							var yVal = yProperty.GetValue(enumerator.Current);
							xValue.Add(xVal != null ? (string)xVal : string.Empty);
							YDataCollection?.Add((IList<double>?)yVal ?? []);
							ActualData?.Add(enumerator.Current);
						}

						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.Double ||
					  XValueType == ChartValueType.Logarithmic)
				{
					if (XValues is List<double> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							var yVal = yProperty.GetValue(enumerator.Current);
							XData = Convert.ToDouble(xVal ?? double.NaN);
							xValue.Add(XData);
							YDataCollection?.Add((IList<double>?)yVal ?? []);
							ActualData?.Add(enumerator.Current);
						}

						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.DateTime)
				{
					if (XValues is List<double> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							if (xVal != null)
							{
								var yVal = yProperty.GetValue(enumerator.Current);
								XData = ((DateTime)xVal).ToOADate();
								xValue.Add(XData);
								YDataCollection?.Add((IList<double>?)yVal ?? []);
								ActualData?.Add(enumerator.Current);
							}
						}

						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.TimeSpan)
				{
					//TODO:Validate on timespan implementation.
				}
			}
		}

		internal override void SetIndividualPoint(object obj, int index, bool replace)
		{
			if (YDataCollection != null && YPaths != null && ItemsSource != null)
			{
				var xvalueType = GetArrayPropertyValue(obj, XComplexPaths);
				if (xvalueType != null)
				{
					XValueType = GetDataType(xvalueType);
				}

				var tempYPath = YComplexPaths?[0];
				var yVal = GetArrayPropertyValue(obj, tempYPath);
				if (yVal != null)
				{
					IList<double> yValue = (IList<double>)yVal;
					if (XValueType == ChartValueType.String)
					{
						if (XValues is not List<string>)
						{
							XValues = ActualXValues = new List<string>();
						}

						var xValue = (List<string>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);

						if (replace && xValue.Count > index)
						{
							xValue[index] = xVal.Tostring();
						}
						else
						{
							xValue.Insert(index, xVal.Tostring());
						}

						if (replace && YDataCollection.Count > index)
						{
							YDataCollection[index] = yValue;
						}
						else
						{
							YDataCollection.Insert(index, yValue);
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.Double ||
						XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						//var yVal = GetArrayPropertyValue(obj, tempYPath);
						XData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;

						// Check the Data Collection is linear or not
						if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							xValue[index] = XData;
						}
						else
						{
							xValue.Insert(index, XData);
						}

						if (replace && YDataCollection.Count > index)
						{
							YDataCollection[index] = yValue;
						}
						else
						{
							YDataCollection[index] = yValue;
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDateTime(xVal).ToOADate();

						// Check the Data Collection is linear or not
						if (IsLinearData && index > 0 && XData < xValue[index - 1])
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							xValue[index] = XData;
						}
						else
						{
							xValue.Insert(index, XData);
						}

						if (replace && YDataCollection.Count > index)
						{
							YDataCollection[index] = yValue;
						}
						else
						{
							YDataCollection[index] = yValue;
						}

						PointsCount = xValue.Count;
					}

					if (ActualData != null)
					{
						if (replace && ActualData.Count > index)
						{
							ActualData[index] = obj;
						}
						else if (ActualData.Count == index)
						{
							ActualData.Add(obj);
						}
						else
						{
							ActualData.Insert(index, obj);
						}
					}
				}
			}
		}

		internal override void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && XComplexPaths != null && YComplexPaths != null)
			{
				XValueType = GetDataType(enumerator, XComplexPaths);
				if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
					XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
				{
					if (XValues is not List<double>)
					{
						ActualXValues = XValues = new List<double>();
					}
				}
				else
				{
					if (XValues is not List<string>)
					{
						ActualXValues = XValues = new List<string>();
					}
				}

				string[] tempYPath = YComplexPaths[0];
				if (string.IsNullOrEmpty(yPaths[0]))
				{
					return;
				}

				_ = yLists[0];
				object? xVal, yVal;
				if (XValueType == ChartValueType.String)
				{
					if (XValues is List<string> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, XComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);
							if (xVal == null)
							{
								return;
							}

							xValue.Add((string)xVal);
							YDataCollection?.Add((IList<double>?)yVal ?? []);
							ActualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.Double ||
					XValueType == ChartValueType.Logarithmic)
				{
					if (XValues is List<double> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, XComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);
							XData = Convert.ToDouble(xVal ?? double.NaN);

							// Check the Data Collection is linear or not
							if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
							{
								IsLinearData = false;
							}

							xValue.Add(XData);
							YDataCollection?.Add((IList<double>?)yVal ?? []);
							ActualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.DateTime)
				{
					if (XValues is List<double> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, XComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);

							XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

							// Check the Data Collection is linear or not
							if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
							{
								IsLinearData = false;
							}

							xValue.Add(XData);
							YDataCollection?.Add((IList<double>?)yVal ?? []);
							ActualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						PointsCount = xValue.Count;
					}
				}
				else if (XValueType == ChartValueType.TimeSpan)
				{
					//TODO: Ensure for timespan;
				}
			}
		}

		internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
		{
			if (XValues is IList<double> list)
			{
				list.RemoveAt(index);
				PointsCount--;
			}
			else if (XValues is IList<string> list1)
			{
				list1.RemoveAt(index);
				PointsCount--;
			}

			YDataCollection?.RemoveAt(index);

			ActualData?.RemoveAt(index);
		}

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();

			double bottomValue = 0, median = 0d, lowerQuartile = 0d, upperQuartile = 0d, minimum = 0d, maximum = 0d, average = 0d;

			if (ActualXAxis != null)
			{
				bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
			}

			if (YDataCollection == null || YDataCollection.Count == 0)
			{
				return;
			}

			for (int i = 0; i < PointsCount; i++)
			{
				var yList = YDataCollection[i].Where(x => !double.IsNaN(x)).ToArray();

				List<double> outliers = [];

				if (yList.Length > 0)
				{
					Array.Sort(yList);
					average = yList.Average();
				}

				int yCount = yList.Length;

				_isEvenList = yCount % 2 == 0;

				if (yCount == 0)
				{
					if (i < _segments.Count && _segments[i] is BoxAndWhiskerSegment segment)
					{
						segment.SetData([i + SbsInfo.Start, i + SbsInfo.End, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN]);
					}
					else
					{
						CreateSegment(seriesView, [i + SbsInfo.Start, i + SbsInfo.End, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN], i, outliers);
					}

					continue;
				}

				if (BoxPlotMode == BoxPlotMode.Exclusive)
				{
					lowerQuartile = GetExclusiveQuartileValue(yList, yCount, 0.25d);
					upperQuartile = GetExclusiveQuartileValue(yList, yCount, 0.75d);
					median = GetExclusiveQuartileValue(yList, yCount, 0.5d);
				}
				else if (BoxPlotMode == BoxPlotMode.Inclusive)
				{
					lowerQuartile = GetInclusiveQuartileValue(yList, yCount, 0.25d);
					upperQuartile = GetInclusiveQuartileValue(yList, yCount, 0.75d);
					median = GetInclusiveQuartileValue(yList, yCount, 0.5d);
				}
				else
				{
					median = BoxAndWhiskerSeries.GetMedianValue(yList);
					GetQuartileValues(yList, yCount, out lowerQuartile, out upperQuartile);
				}

				if (ShowOutlier)
				{
					GetMinMaxandOutlier(lowerQuartile, upperQuartile, yList, out minimum, out maximum, outliers);
				}
				else
				{
					minimum = yList.Min();
					maximum = yList.Max();
				}

				double actualMinimum = minimum;
				double actualMaximum = maximum;

				if (outliers.Count > 0)
				{
					actualMinimum = Math.Min(outliers.Min(), actualMinimum);
					actualMaximum = Math.Max(outliers.Max(), actualMaximum);
				}

				if (IsIndexed && IsGrouped && xValues != null)
				{
					if (i < _segments.Count && _segments[i] is BoxAndWhiskerSegment segment)
					{
						segment.SetData([i + SbsInfo.Start, i + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average]);
					}
					else
					{
						CreateSegment(seriesView, [i + SbsInfo.Start, i + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average], i, outliers);
					}
				}
				else
				{
					if (xValues != null)
					{
						var x = xValues[i];

						if (i < _segments.Count && _segments[i] is BoxAndWhiskerSegment segment)
						{
							segment.SetData([x + SbsInfo.Start, x + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average]);
						}
						else
						{
							CreateSegment(seriesView, [x + SbsInfo.Start, x + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average], i, outliers);
						}
					}
				}

				_isEvenList = false;
			}
		}

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			if (_segments == null)
			{
				return null;
			}

			int index = IsSideBySide ? GetDataPointIndex(x, y) : SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

			if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
				|| ActualYAxis == null || SeriesYValues == null)
			{
				return null;
			}

			var xValues = GetXValues();

			if (xValues == null || ChartArea == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
			double xValue = xValues[index];

			if (_segments[index] is BoxAndWhiskerSegment segment)
			{
				double yValue;
				float xPosition, yPosition;

				if (IsOutlierTouch)
				{
					yValue = segment._outliers[segment._outlierIndex];
				}
				else
				{
					yValue = segment.Maximum;
				}

				xPosition = TransformToVisibleX(xValue, yValue);

				if (!double.IsNaN(xPosition) && !double.IsNaN(yValue))
				{
					yPosition = TransformToVisibleY(xValue, yValue);

					if (IsSideBySide)
					{
						double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
						xPosition = TransformToVisibleX(xMidVal, yValue);
						yPosition = TransformToVisibleY(xMidVal, yValue);
					}

					TooltipInfo tooltipInfo = new TooltipInfo(this)
					{
						X = xPosition,
						Y = yPosition,
						Index = index,
						Margin = tooltipBehavior.Margin,
						TextColor = tooltipBehavior.TextColor,
						FontFamily = tooltipBehavior.FontFamily,
						FontSize = tooltipBehavior.FontSize,
						FontAttributes = tooltipBehavior.FontAttributes,
						Background = tooltipBehavior.Background,
						Item = dataPoint
					};

					if (IsOutlierTouch)
					{
						tooltipInfo.Text = yValue.ToString();
					}
					else
					{
						tooltipInfo.Text = yValue.ToString("  #.##") + "/" + segment.UpperQuartile.ToString("  #.##") + "/" + segment.Median.ToString("  #.##") + "/" + segment.LowerQuartile.ToString("  #.##") + "/" + segment.Minimum.ToString("  #.##");
					}

					return tooltipInfo;
				}
			}

			return null;
		}

		internal override DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
		{
			return new DataTemplate(() =>
			{
				StackLayout stackLayout = [];

				if (IsOutlierTouch)
				{
					stackLayout.Children.Add(new Label()
					{
						Text = info.Text,
						Background = info.Background,
						FontAttributes = info.FontAttributes,
						FontSize = info.FontSize,
						TextColor = info.TextColor,
						Margin = info.Margin
					});
				}
				else
				{
					var texts = info.Text.Split('/');
					string maximumFormat = SfCartesianChartResources.Maximum + " :";
					string minimumFormat = SfCartesianChartResources.Minimum + "  :";
					string Q3Format = SfCartesianChartResources.Q3 + "\t    :";
					string Q1Format = SfCartesianChartResources.Q1 + "\t    :";
					string medianFormat = SfCartesianChartResources.Median + "      :";

					var labels = new[]
					{
						new TooltipLabelValue(maximumFormat, texts[0]),
						new TooltipLabelValue(Q3Format, texts[1]),
						new TooltipLabelValue(medianFormat, texts[2]),
						new TooltipLabelValue(Q1Format, texts[3]),
						new TooltipLabelValue(minimumFormat, texts[4])
					};

					BindableLayout.SetItemsSource(stackLayout, labels);

					BindableLayout.SetItemTemplate(stackLayout, new DataTemplate(() =>
					{
						StackLayout stackLayout1 = [];
						stackLayout1.Orientation = StackOrientation.Horizontal;

						stackLayout1.Add(new Label
						{
							HorizontalOptions = LayoutOptions.Start,
							VerticalOptions = LayoutOptions.Start,
							TextColor = info.TextColor,
							Margin = info.Margin,
							Background = info.Background,
							FontAttributes = info.FontAttributes,
							FontSize = info.FontSize,
						});

						stackLayout1.Add(new Label
						{
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Start,
							HorizontalTextAlignment = TextAlignment.Start,
							TextColor = info.TextColor,
							Margin = info.Margin,
							Background = info.Background,
							FontAttributes = info.FontAttributes,
							FontSize = info.FontSize,
						});

						((Label)stackLayout1.Children[0]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty,
							BindingHelper.CreateBinding(nameof(TooltipLabelValue.LabelText), getter: static (TooltipLabelValue label) => label.LabelText));
						((Label)stackLayout1.Children[1]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty,
							BindingHelper.CreateBinding(nameof(TooltipLabelValue.ValueText), getter: static (TooltipLabelValue label) => label.ValueText));
						return stackLayout1;
					}));
				}

				return stackLayout;
			});
		}

		internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
		{
			var xValues = GetXValues();
			float xPosition = 0f;
			float yPosition = 0f;
			if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
			{
				foreach (object point in nearestDataPoints)
				{
					int index = ActualData.IndexOf(point);
					var xValue = xValues[index];
					double yValue;

					if (_segments[index] is BoxAndWhiskerSegment segment)
					{
						if (IsOutlierTouch)
						{
							yValue = segment._outliers[segment._outlierIndex];
						}
						else
						{
							yValue = segment.Maximum;
						}

						xPosition = TransformToVisibleX(xValue, yValue);

						string label = $"{SfCartesianChartResources.Maximum} : {segment.Maximum}\n" +
									   $"{SfCartesianChartResources.Q3}\t   : {segment.UpperQuartile}\n" +
									   $"{SfCartesianChartResources.Median}      : {segment.Median}\n" +
									   $"{SfCartesianChartResources.Q1}\t   : {segment.LowerQuartile}\n" +
									   $"{SfCartesianChartResources.Minimum}  : {segment.Minimum}";

						if (IsSideBySide)
						{
							isSideBySide = true;
							double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
							xPosition = TransformToVisibleX(xMidVal, yValue);
							yPosition = TransformToVisibleY(xMidVal, yValue);
						}

						TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

						if (chartPointInfo != null)
						{
#if ANDROID
							Size contentSize = ChartUtils.GetLabelSize(label, chartPointInfo.TooltipHelper);
							chartPointInfo.GroupLabelSize = contentSize;
#endif
							chartPointInfo.XValue = xValue;
							chartPointInfo.YValues = [segment.Maximum, segment.UpperQuartile, segment.Median, segment.LowerQuartile, segment.Minimum];
							pointInfos.Add(chartPointInfo);
						}
					}
				}
			}
		}

		internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
		{
			var yValues = pointInfo.YValues;
			string label = $"{SfCartesianChartResources.Maximum} : {yValues[0].ToString(labelFormat)}\n" +
									   $"{SfCartesianChartResources.Q3}\t   : {yValues[1].ToString(labelFormat)}\n" +
									   $"{SfCartesianChartResources.Median}      : {yValues[2].ToString(labelFormat)}\n" +
									   $"{SfCartesianChartResources.Q1}\t   : {yValues[3].ToString(labelFormat)}\n" +
									   $"{SfCartesianChartResources.Minimum}  : {yValues[4].ToString(labelFormat)}";
#if ANDROID
			Size contentSize = ChartUtils.GetLabelSize(label, pointInfo.TooltipHelper);
			pointInfo.GroupLabelSize = contentSize;
#endif
			pointInfo.Label = label;
		}

		#endregion

		#region Private Methods

		static void OnBoxPlotModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
			{
				boxAndWhiskerSeries.UpdateSbsSeries();
			}
		}

		static void OnShapeTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
			{
				boxAndWhiskerSeries.InvalidateSeries();
			}
		}

		static void OnMedianPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
			{
				boxAndWhiskerSeries.InvalidateSeries();
			}
		}

		static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries series)
			{
				series.UpdateStrokeColor();
				series.InvalidateSeries();
			}
		}

		static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries series)
			{
				series.UpdateSbsSeries();
			}
		}

		static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is BoxAndWhiskerSeries series)
			{
				series.UpdateSbsSeries();
			}
		}

		static double GetExclusiveQuartileValue(double[] yList, int yCount, double percentile)
		{
			if (yCount == 0)
			{
				return 0;
			}
			else if (yCount == 1)
			{
				return yList[0];
			}

			double rank = percentile * (yCount + 1);
			int integerRank = (int)Math.Abs(rank);
			double fractionRank = rank - integerRank;
			double value;

			if (integerRank == 0)
			{
				value = yList[0];
			}
			else if (integerRank > yCount - 1)
			{
				value = yList[yCount - 1];
			}
			else
			{
				value = fractionRank * (yList[integerRank] - yList[integerRank - 1]) + yList[integerRank - 1];
			}

			return value;
		}

		static double GetInclusiveQuartileValue(double[] yList, int yCount, double percentile)
		{
			if (yCount == 0)
			{
				return 0;
			}
			else if (yCount == 1)
			{
				return yList[0];
			}

			double rank = percentile * (yCount - 1);
			int integerRank = (int)Math.Abs(rank);
			double fractionRank = rank - integerRank;
			double value = fractionRank * (yList[integerRank + 1] - yList[integerRank]) + yList[integerRank];

			return value;
		}

		static void GetMinMaxandOutlier(double lowerQuartile, double upperQuartile, double[] yList,
										out double minimum, out double maximum, List<double> outliers)
		{
			minimum = 0d;
			maximum = 0d;
			double interquartile = upperQuartile - lowerQuartile;
			double rangeIQR = 1.5 * interquartile;

			for (int i = 0; i < yList.Length; i++)
			{
				if (yList[i] < lowerQuartile - rangeIQR)
				{
					outliers.Add(yList[i]);
				}
				else
				{
					minimum = yList[i];
					break;
				}
			}

			for (int i = yList.Length - 1; i >= 0; i--)
			{
				if (yList[i] > upperQuartile + rangeIQR)
				{
					outliers.Add(yList[i]);
				}
				else
				{
					maximum = yList[i];
					break;
				}
			}
		}

		void GetQuartileValues(double[] yList, int len, out double lowerQuartile, out double upperQuartile)
		{
			double[] lowerQuartileArray;
			double[] upperQuartileArray;

			if (len == 1)
			{
				lowerQuartile = yList[0];
				upperQuartile = yList[0];
				return;
			}

			var halfLength = len / 2;

			lowerQuartileArray = new double[halfLength];
			upperQuartileArray = new double[halfLength];

			Array.Copy(yList, 0, lowerQuartileArray, 0, halfLength);
			Array.Copy(yList, _isEvenList ? halfLength : halfLength + 1, upperQuartileArray, 0, halfLength);

			lowerQuartile = BoxAndWhiskerSeries.GetMedianValue(lowerQuartileArray);
			upperQuartile = BoxAndWhiskerSeries.GetMedianValue(upperQuartileArray);
		}

		static double GetMedianValue(double[] yList)
		{
			int len = yList.Length;

			int middleIndex = (int)Math.Round(len / 2.0, MidpointRounding.AwayFromZero);

			if (len % 2 == 0)
			{
				return (yList[middleIndex - 1] + yList[middleIndex]) / 2;
			}
			else
			{
				return yList[middleIndex - 1];
			}
		}

		void CreateSegment(SeriesView seriesView, double[] values, int index, List<double> outliers)
		{
			var xValues = GetXValues();

			if (CreateSegment() is BoxAndWhiskerSegment segment)
			{
				foreach (var outlier in outliers)
				{
					if (xValues != null)
					{
						segment._outliers.Add(outlier);
					}
				}

				segment.Series = this;
				segment.SeriesView = seriesView;
				segment.SetData(values);
				segment.Item = ActualData?[index];
				segment.Index = index;
				_segments.Add(segment);
			}
		}

		#endregion

		#endregion
	}
}