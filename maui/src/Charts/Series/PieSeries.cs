using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="PieSeries"/> displays data as a proportion of the whole. Its most commonly used to make comparisons among a set of given data.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of the pie series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
	/// 
	/// <para>It Provides options for <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="CircularSeries.Radius"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="PieSeries"/> and refer to the <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in the <see cref="PieSeries"/> class. To customize the chart data labels’ alignment, placement, and label styles, you need to create an instance of <see cref="CircularDataLabelSettings"/> and set it to the <see cref="CircularSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>Selection - </b> To enable the data point selection in the series, create an instance of the <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="ChartSeries.SelectionBehavior"/> property of the pie series. To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// 
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCircularChart>
	///
	///           <chart:SfCircularChart.Series>
	///               <chart:PieSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"/>
	///           </chart:SfCircularChart.Series>  
	///           
	///     </chart:SfCircularChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCircularChart chart = new SfCircularChart();
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     PieSeries series = new PieSeries();
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
	public partial class PieSeries : CircularSeries, IDrawCustomLegendIcon
	{
		#region Fields

		double _total;
		float _yValue;
		float _pieEndAngle;
		float _pieStartAngle;
		double _angleDifference;

		#endregion

		#region internal Properties

		internal IList<double> ActualYValues { get; set; }

		internal List<object> GroupToDataPoints { get; set; }

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ExplodeIndex"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExplodeIndex"/> property determines index value of the segment to be exploded.
		/// </remarks>
		public static readonly BindableProperty ExplodeIndexProperty = BindableProperty.Create(
			nameof(ExplodeIndex),
			typeof(int),
			typeof(PieSeries),
			-1,
			BindingMode.TwoWay,
			null,
			OnExplodeIndexChanged);

		/// <summary>
		/// Identifies the <see cref="ExplodeRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExplodeRadius"/> property defines the exploding distance of the segments.
		/// </remarks>
		public static readonly BindableProperty ExplodeRadiusProperty = BindableProperty.Create(
			nameof(ExplodeRadius),
			typeof(double),
			typeof(PieSeries),
			10d,
			BindingMode.Default,
			null,
			OnExplodeRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="ExplodeOnTouch"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExplodeOnTouch"/> property indicating whether to explode the segment by touch or tap interaction.
		/// </remarks>
		public static readonly BindableProperty ExplodeOnTouchProperty = BindableProperty.Create(
			nameof(ExplodeOnTouch),
			typeof(bool),
			typeof(PieSeries),
			false,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="GroupTo"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="GroupTo"/> property specifies the grouping for segments containing a minimum value of data points below the specified threshold.
		/// </remarks>
		public static readonly BindableProperty GroupToProperty = BindableProperty.Create(
			nameof(GroupTo),
			typeof(double),
			typeof(PieSeries),
			double.NaN,
			BindingMode.Default,
			null,
			OnGroupToPropertiesChanged);

		/// <summary>
		/// Identifies the <see cref="GroupMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="GroupMode"/> property which determines the type of grouping based on slice Angle, actual data point Value, or Percentage.
		/// </remarks>
		public static readonly BindableProperty GroupModeProperty = BindableProperty.Create(
			nameof(GroupMode),
			typeof(PieGroupMode),
			typeof(PieSeries),
			PieGroupMode.Value,
			BindingMode.Default,
			null,
			OnGroupToPropertiesChanged);

		/// <summary>
		/// Identifies the <see cref="ExplodeAll"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExplodeAll"/> property controls whether all segments explode outward from the center of the chart.
		/// </remarks>
		public static readonly BindableProperty ExplodeAllProperty = BindableProperty.Create(
			nameof(ExplodeAll),
			typeof(bool),
			typeof(PieSeries),
			false,
			BindingMode.Default,
			null,
			OnExplodeAllChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the index value of the segment to be exploded.
		/// </summary>
		/// <value>This property takes an <see cref="int"/> value, and its default value is <c>-1</c>.</value>
		/// <remarks>
		/// <para>To explode a segment, create an instance for any circular series, and assign value to the <see cref="ExplodeIndex"/> property. </para>
		///</remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///           <chart:SfCircularChart.Series>
		///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
		///                   YBindingPath="YValue" ExplodeIndex = "3"/>
		///           </chart:SfCircularChart.Series>  
		///           
		///     </chart:SfCircularChart>
		///
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-5)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     
		///     ViewModel viewModel = new ViewModel();
		/// 
		///     PieSeries series = new PieSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     series.ExplodeIndex = 3;
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public int ExplodeIndex
		{
			get { return (int)GetValue(ExplodeIndexProperty); }
			set { SetValue(ExplodeIndexProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that defines the exploding distance of the segments.
		/// </summary>
		/// <value>This property takes a <see cref="double"/> value, and its default value is <c>10d</c>.</value>
		/// <remarks>
		/// <para>To explode a segment to a specific length, create an instance for any circular series, and assign value to both <see cref="ExplodeIndex"/> and <see cref="ExplodeRadius"/> properties.</para>
		/// <para>The value needs to be greater than zero.</para>
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///           <chart:SfCircularChart.Series>
		///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
		///                   YBindingPath="YValue" ExplodeIndex = "3" ExplodeRadius="20"/>
		///           </chart:SfCircularChart.Series>  
		///           
		///     </chart:SfCircularChart>
		///
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-7)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     
		///     ViewModel viewModel = new ViewModel();
		/// 
		///     PieSeries series = new PieSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     series.ExplodeIndex = 3;
		///     series.ExplodeRadius = 20;
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public double ExplodeRadius
		{
			get { return (double)GetValue(ExplodeRadiusProperty); }
			set { SetValue(ExplodeRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to explode the segment by touch or tap interaction.
		/// </summary>
		/// <value>This property takes the <see cref="bool"/> value, and its default value is <c>false</c>.</value>
		/// <remarks>
		/// <para>To explode a selected segment by tap action, create an instance for any circular series, and assign the <see cref="ExplodeOnTouch"/> property value as <c>"True"</c> </para>
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-8)
		///  <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///           <chart:SfCircularChart.Series>
		///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
		///                   YBindingPath="YValue" ExplodeOnTouch="True"/>
		///           </chart:SfCircularChart.Series>  
		///           
		///     </chart:SfCircularChart>
		///
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-9)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     
		///     ViewModel viewModel = new ViewModel();
		/// 
		///     PieSeries series = new PieSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     series.ExplodeOnTouch = true;
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public bool ExplodeOnTouch
		{
			get { return (bool)GetValue(ExplodeOnTouchProperty); }
			set { SetValue(ExplodeOnTouchProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that causes all the data points to explode from the center of the chart.
		/// </summary>
		/// <value>It accepts <c>bool</c>, and the default is false.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-10)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///          <chart:PieSeries ItemsSource = "{Binding Data}"
		///                           XBindingPath = "XValue"
		///                           YBindingPath = "YValue"
		///                           ExplodeAll = "True"/>
		///
		///     </chart:SfCircularChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-11)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     PieSeries series = new PieSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           ExplodeAll = true,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool ExplodeAll
		{
			get { return (bool)GetValue(ExplodeAllProperty); }
			set { SetValue(ExplodeAllProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that specifies the grouping for segments containing a minimum value of data points below the specified threshold.
		/// </summary>
		/// <value>This property takes a <see cref="double"/> value, and its default value is <c>double.NaN</c>.</value>
		/// <remarks>
		/// <para>To group the segments to a specific value, create an instance for pie series, and assign value to <see cref="GroupMode"/> property.</para>
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-12)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
		///                   YBindingPath="YValue" GroupTo = "30" />
		///           
		///     </chart:SfCircularChart>
		///
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-13)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     PieSeries series = new PieSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           GroupTo = 30,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double GroupTo
		{
			get { return (double)GetValue(GroupToProperty); }
			set { SetValue(GroupToProperty, value); }
		}

		/// <summary>
		/// Gets or sets the group mode, which determines the type of grouping based on slice Angle, actual data point Value, or Percentage.
		/// </summary>
		/// <value>This property takes a <see cref="PieGroupMode"/> value, and its default value is <c>PieGroupMode.Value</c>.</value>
		/// <remarks>
		/// <para>To specify group mode type based on angle or data point value or percentage, create an instance for pie series, and assign value to <see cref="GroupTo"/> property.</para>
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-14)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
		///                   YBindingPath="YValue" GroupTo="90" GroupMode="Angle"  />
		///           
		///     </chart:SfCircularChart>
		///
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-15)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     PieSeries series = new PieSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           GroupTo = 30,
		///           GroupMode = PieGroupMode.Angle,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public PieGroupMode GroupMode
		{
			get { return (PieGroupMode)GetValue(GroupModeProperty); }
			set { SetValue(GroupModeProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PieSeries"/> class.
		/// </summary>
		public PieSeries() : base()
		{
			ActualYValues = [];
			GroupToDataPoints = [];
			PaletteBrushes = ChartColorModel.DefaultBrushes;
		}

		#endregion

		#region Interface Implementations

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			if (this is DoughnutSeries)
			{
				float centerX = rect.Center.X;
				float centerY = rect.Center.Y;
				RectF rect1 = new(centerX / 3, centerY / 3, centerX + 2, centerY + 2);

				PathF path = new();
				path.AddArc(rect.Left, rect.Top, rect.Right, rect.Bottom, 0, (float)359.99, false);
				path.AddArc(rect1.Left, rect1.Top, rect1.Right, rect1.Bottom, (float)359.99, 0, true);
				path.Close();
				canvas.SetFillPaint(fillColor, path.Bounds);
				canvas.FillPath(path);
			}
			else
			{
				RectF rect1 = new(2, 2, 8, 8);
				RectF rect2 = new(3, 1, 8, 8);

				PathF pathF = new();
				pathF.MoveTo(12, 6);
				pathF.AddArc(rect1.Left, rect1.Top, rect1.Right, rect1.Bottom, 0, 180, true);
				pathF.Close();
				canvas.SetFillPaint(fillColor, pathF.Bounds);
				canvas.FillPath(pathF);

				PathF pathF1 = new();
				pathF1.MoveTo(6, 2);
				pathF1.AddArc(rect1.Left, rect1.Top, rect1.Right, rect1.Bottom, 90, 270, false);
				pathF1.Close();
				canvas.SetFillPaint(fillColor, pathF1.Bounds);
				canvas.FillPath(pathF1);

				PathF pathF2 = new();
				pathF2.MoveTo(7, 1);
				pathF2.AddArc(rect2.Left, rect2.Top, rect2.Right, rect2.Bottom, 90, 0, true);
				pathF2.LineTo(11, 5);
				pathF2.LineTo(7, 5);
				pathF2.LineTo(7, 1);
				pathF2.Close();
				canvas.SetFillPaint(fillColor, pathF2.Bounds);
				canvas.FillPath(pathF2);
			}

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
			return new PieSegment();
		}

		#endregion

		#region Internal Methods

		internal override void GenerateSegments(SeriesView seriesView)
		{
			if (YValues != null)
			{
				_pieStartAngle = (float)StartAngle;
				_angleDifference = GetAngleDifference();

				_total = CalculateTotalYValues();

				var oldSegments = OldSegments != null && OldSegments.Count > 0 && PointsCount == OldSegments.Count ? OldSegments : null;
				var legendItems = GetLegendItems();

				for (int i = 0; i < ActualYValues.Count; i++)
				{
					_yValue = (legendItems == null || legendItems.Count == 0) ? (float)Math.Abs(double.IsNaN(ActualYValues[i]) ? double.NaN : ActualYValues[i]) : (float)(legendItems[i].IsToggled ? double.NaN : ActualYValues[i]);
					_pieEndAngle = (float)(Math.Abs(float.IsNaN(_yValue) ? 0 : _yValue) * (_angleDifference) / _total);

					if (i < _segments.Count && _segments[i] is PieSegment segment1)
					{
						segment1.SetData(_pieStartAngle, _pieEndAngle, _yValue);
					}
					else
					{
						PieSegment pieSegment = (PieSegment)CreateSegment();
						pieSegment.Series = this;
						pieSegment.SeriesView = seriesView;
						pieSegment.Index = i;
						pieSegment.Exploded = ExplodeIndex == i;
						pieSegment.SetData(_pieStartAngle, _pieEndAngle, _yValue);
						pieSegment.Item = ActualData?[i];
						InitiateDataLabels(pieSegment);
						_segments.Add(pieSegment);

						if (oldSegments != null)
						{
							if (oldSegments[i] is PieSegment oldSegment)
							{
								pieSegment.SetPreviousData([oldSegment.StartAngle, oldSegment.EndAngle]);
							}
						}
					}

					if (_segments[i] is PieSegment segment)
					{
						segment.SegmentStartAngle = _pieStartAngle;
						segment.SegmentEndAngle = _pieStartAngle + _pieEndAngle;
					}

					if (_segments[i].IsVisible)
					{
						_pieStartAngle += _pieEndAngle;
					}
				}
			}
		}

		internal override float GetDataLabelRadius()
		{
			float radius = DataLabelSettings.LabelPosition == ChartDataLabelPosition.Inside ? GetRadius() / 2 : GetRadius();

			return radius;
		}

		internal virtual float GetTooltipRadius()
		{
			return GetRadius() / 2;
		}

		internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
		{
			base.RemoveData(index, e);

			CalculateGroupToYValues();
		}

		internal override void SetIndividualPoint(int index, object obj, bool replace)
		{
			base.SetIndividualPoint(index, obj, replace);

			CalculateGroupToYValues();
		}

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			if (AreaBounds == Rect.Zero)
			{
				return null;
			}

			int index = GetDataPointIndex(x, y);

			if (index < 0 || ActualData == null || ActualYValues == null)
			{
				return null;
			}

			object dataPoint = GroupToDataPoints[index];
			double yValue = ActualYValues[index];

			if (_segments[index] is not PieSegment pieSegment)
			{
				return null;
			}

			float segmentRadius = GetTooltipRadius();
			segmentRadius = pieSegment.Index == ExplodeIndex ? segmentRadius + (float)ExplodeRadius : segmentRadius;
			PointF center = Center;
			double midAngle = _isClockWise ? (pieSegment.StartAngle + (pieSegment.EndAngle / 2)) * 0.0174532925f :
				(pieSegment.StartAngle + pieSegment.EndAngle) / 2 * 0.0174532925f;
			float xPosition = (float)(center.X + (Math.Cos(midAngle) * segmentRadius));
			float yPosition = (float)(center.Y + (Math.Sin(midAngle) * segmentRadius));

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
				Text = yValue.ToString("#.##"),
				Item = dataPoint
			};

			return tooltipInfo;
		}

		internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			float xPosition = tooltipInfo.X;
			float yPosition = tooltipInfo.Y;
			float sizeValue = 1;
			float noseOffset = 2;
			float halfSizeValue = 0.5f;

			Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition + noseOffset, sizeValue, sizeValue);
			tooltipInfo.TargetRect = targetRect;
		}

		internal void UpdateExplodeOnTouch(float pointX, float pointY)
		{
			var dataPointIndex = GetDataPointIndex(pointX, pointY);
			if (dataPointIndex >= 0)
			{
				ExplodeIndex = (ExplodeIndex != dataPointIndex) ? dataPointIndex : -1;
			}
		}

		internal override void OnBindingPathChanged()
		{
			base.OnBindingPathChanged();

			CalculateGroupToYValues();
		}

		internal override void OnDataSourceChanged(object oldValue, object newValue)
		{
			YValues.Clear();
			ActualYValues.Clear();
			GeneratePoints([YBindingPath], YValues);
			CalculateGroupToYValues();
		}

		internal override void ResetData()
		{
			base.ResetData();
			ActualYValues?.Clear();
		}

		internal void CalculateGroupToYValues()
		{
			if (double.IsNaN(GroupTo) && ActualData != null)
			{
				ActualYValues = YValues;
				GroupToDataPoints = ActualData;
			}
			else
			{
				ActualYValues = [];
				var groupedData = new List<object>();
				GroupToDataPoints = [];
				var lessThanGroupTo = 0d;
				var total = SumOfYValues();
				for (int i = 0; i < YValues.Count; i++)
				{
					double yValue = YValues[i];

					if (GetGroupModeValue(yValue, total) > GroupTo)
					{
						ActualYValues.Add(yValue);
						if (ActualData != null)
						{
							GroupToDataPoints.Add(ActualData[i]);
						}
					}
					else
					{
						lessThanGroupTo += double.IsNaN(yValue) ? 0 : yValue;
						if (ActualData != null)
						{
							groupedData.Add(ActualData[i]);
						}
					}

					if (i == PointsCount - 1 && groupedData.Count > 0)
					{
						ActualYValues.Add(lessThanGroupTo);
						GroupToDataPoints.Add(groupedData);
					}
				}
			}
		}

		internal double GetGroupModeValue(double yValue, double total)
		{
			double value = 0;
			switch (GroupMode)
			{
				case PieGroupMode.Value:
					{
						value = yValue;
						break;
					}
				case PieGroupMode.Angle:
					{
						var angleDifference = GetAngleDifference();
						value = Math.Abs(yValue) * (angleDifference / total);
						break;
					}
				case PieGroupMode.Percentage:
					{
						value = Math.Round(yValue / total * 100, 2);
						break;
					}
			}

			return value;
		}

		internal override double CalculateTotalYValues()
		{
			double total = 0;
			var legendItems = GetLegendItems();

			for (int i = 0; i < ActualYValues.Count; i++)
			{
				if (!double.IsNaN(ActualYValues[i]) && (_segments.Count == 0 || ((_segments.Count <= i) || (_segments.Count > i && _segments[i].IsVisible))))
				{
					if (legendItems == null || legendItems.Count == 0)
					{
						total += Math.Abs(ActualYValues[i]);
					}
					else
					{
						total += legendItems[i].IsToggled ? 0 : Math.Abs(ActualYValues[i]);
					}
				}
			}

			return total;
		}

		#endregion

		#region Private Methods

		static void OnExplodeIndexChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is PieSeries series && series.AreaBounds != Rect.Zero)
			{
				series.OnExplodePropertiesChanged();
			}
		}

		static void OnExplodeRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is PieSeries series && series.AreaBounds != Rect.Zero)
			{
				series.OnExplodePropertiesChanged();
			}
		}

		static void OnGroupToPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is PieSeries pieSeries)
			{
				pieSeries.SegmentsCreated = false;
				pieSeries.CalculateGroupToYValues();
				pieSeries.UpdateLegendItems();
				pieSeries.ScheduleUpdateChart();
			}
		}

		static void OnExplodeAllChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is PieSeries pieSeries)
			{
				pieSeries.ScheduleUpdateChart();
			}
		}

		void UpdateExplode()
		{
			for (int i = 0; i < _segments.Count; i++)
			{
				var segment = (PieSegment)_segments[i];
				segment.Exploded = ExplodeIndex == i;
			}
		}

		void OnExplodePropertiesChanged()
		{
			UpdateExplode();
			Invalidate();
			InvalidateSeries();

			if (ShowDataLabels)
			{
				InvalidateDataLabel();
			}
		}

		#endregion

		#endregion
	}
}