namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Serves as a base class for all types of range series.
	/// </summary>
	public abstract class RangeSeriesBase : CartesianSeries
	{
		#region Internal Properties

		internal float _sumOfHighValues = float.NaN;
		internal float _sumOfLowValues = float.NaN;

		internal IList<double> HighValues { get; set; }

		internal IList<double> LowValues { get; set; }

		internal override bool IsMultipleYPathRequired
		{
			get
			{
				return !string.IsNullOrEmpty(High) && !string.IsNullOrEmpty(Low);
			}
		}

		#endregion

		#region Bindable Properties
		/// <summary>
		/// Identifies the <see cref="High"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="High"/> property indicates the upper boundary of the range series.
		/// </remarks>
		public static readonly BindableProperty HighProperty = BindableProperty.Create(
			nameof(High),
			typeof(string),
			typeof(RangeSeriesBase),
			string.Empty,
			BindingMode.Default,
			null,
			OnHighAndLowBindingPathChanged);

		/// <summary>
		/// Identifies the <see cref="Low"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Low"/> property indicates the lower boundary of the range series.
		/// </remarks>
		public static readonly BindableProperty LowProperty = BindableProperty.Create(
			nameof(Low),
			typeof(string),
			typeof(RangeSeriesBase),
			string.Empty,
			BindingMode.Default,
			null,
			OnHighAndLowBindingPathChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Stroke"/> property helps to customize the stroke appearance of the range series.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(RangeSeriesBase),
			null,
			BindingMode.Default,
			null,
			OnStrokeColorChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="StrokeWidth"/> property determines the thickness of the stroke in the series.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(RangeSeriesBase),
			1d,
			BindingMode.Default,
			null,
			OnStrokeWidthPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeDashArray"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="StrokeDashArray"/> property helps to customize the stroke dash patterns of the range series.
		/// </remarks>
		public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
			nameof(StrokeDashArray),
			typeof(DoubleCollection),
			typeof(RangeSeriesBase),
			null,
			BindingMode.Default,
			null,
			OnStrokeDashArrayPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a path value on the source object to serve a high value to the series.
		/// </summary>
		/// <value>The <c>string</c> representing the property name for the higher plotting data and its default value is empty.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
		///                                   XBindingPath = "XValue"
		///                                   High = "HighValue"
		///                                   Low = "LowValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     RangeColumnSeries series = new RangeColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           High = "HighValue",
		///           Low = "LowValue",
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string High
		{
			get { return (string)GetValue(HighProperty); }
			set { SetValue(HighProperty, value); }
		}

		/// <summary>
		/// Gets or sets a path value on the source object to serve a low value to the series.
		/// </summary>
		/// <value>The <c>string</c> representing the property name for the lower plotting data and its default value is empty.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
		///                                   XBindingPath = "XValue"
		///                                   High = "HighValue"
		///                                   Low = "LowValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     RangeColumnSeries series = new RangeColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           High = "HighValue",
		///           Low = "LowValue",
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string Low
		{
			get { return (string)GetValue(LowProperty); }
			set { SetValue(LowProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the border appearance of the range series.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values, and its default value is <c>Transparent</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
		///                                   XBindingPath = "XValue"
		///                                   High = "HighValue"
		///                                   Low = "LowValue"
		///                                   StrokeWidth = "3"
		///                                   Stroke = "Red" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     RangeColumnSeries series = new RangeColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           High = "HighValue",
		///           Low = "LowValue",
		///           StrokeWidth = 3,
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
		/// Gets or sets a value to specify the border width of the range series.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		/// <value>It accepts <see cref="double"/> values, and its default value is 0.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
		///                                   XBindingPath = "XValue"
		///                                   High = "HighValue"
		///                                   Low = "LowValue"
		///                                   Stroke = "Red"
		///                                   StrokeWidth = "3" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-8)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     RangeColumnSeries series = new RangeColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           High = "HighValue",
		///           Low = "LowValue",
		///           StrokeWidth = 3,
		///           Stroke = new SolidColorBrush(Colors.Red),
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke dash array to customize the appearance of the series border.
		/// </summary>
		/// <value>It accepts the <see cref="DoubleCollection"/> value, and the default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
		///                                   XBindingPath= "XValue"
		///                                   High = "HighValue"
		///                                   Low = "LowValue"
		///                                   StrokeDashArray = "5,3"
		///                                   Stroke = "Red" />
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-10)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     DoubleCollection doubleCollection = new DoubleCollection();
		///     doubleCollection.Add(5);
		///     doubleCollection.Add(3);
		///     RangeColumnSeries series = new RangeColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           High = "HighValue",
		///           Low = "LowValue",
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
		///  Initializes a new instance of the <see cref="RangeSeriesBase"/>.
		/// </summary>
		public RangeSeriesBase()
		{
			HighValues = [];
			LowValues = [];
		}

		#endregion

		#region Methods

		#region Internal Methods

		internal override void ResetEmptyPointIndexes()
		{
			if (EmptyPointIndexes.Length != 0)
			{
				if (EmptyPointIndexes[0] != null)
				{
					foreach (var index in EmptyPointIndexes[0])
					{
						if (HighValues != null && HighValues.Count != 0 && index < HighValues.Count)
						{
							HighValues[(int)index] = double.NaN;
						}
					}
				}

				if (EmptyPointIndexes[1] != null)
				{
					foreach (var index in EmptyPointIndexes[1])
					{
						if (LowValues != null && LowValues.Count != 0 && index < LowValues.Count)
						{
							LowValues[(int)index] = double.NaN;
						}
					}
				}
			}
		}

		internal override void ValidateYValues()
		{
			bool highValues = HighValues.Any(value => double.IsNaN(value));
			bool lowValues = LowValues.Any(value => double.IsNaN(value));

			if ((highValues || lowValues) && SeriesYValues != null)
			{
				ValidateDataPoints(SeriesYValues);
			}
		}

		internal override void OnDataSourceChanged(object oldValue, object newValue)
		{
			HighValues.Clear();
			LowValues.Clear();
			SegmentsCreated = false;

			if (Chart != null)
			{
				Chart.IsRequiredDataLabelsMeasure = true;
			}

			GeneratePoints([High, Low], HighValues, LowValues);
			base.OnDataSourceChanged(oldValue, newValue);
		}

		internal override void OnBindingPathChanged()
		{
			ResetData();
			GeneratePoints([High, Low], HighValues, LowValues);
			base.OnBindingPathChanged();
		}

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override void SetStrokeWidth(ChartSegment segment)
		{
			segment.StrokeWidth = StrokeWidth;
		}

		internal override void SetDashArray(ChartSegment segment)
		{
			segment.StrokeDashArray = StrokeDashArray;
		}

		internal SizeF GetLabelTemplateSize(ChartSegment segment, string valueType)
		{
			int labelIndex = valueType == "LowValues" ? 1 : 0;

			if (LabelTemplateView != null && LabelTemplateView.Any())
			{
				if (LabelTemplateView.Cast<View>().FirstOrDefault(child => segment.DataLabels[labelIndex] == child.BindingContext) is DataLabelItemView templateView && templateView.ContentView is View content)
				{
					if (!content.DesiredSize.IsZero)
					{
						return content.DesiredSize;
					}

					var desiredSize = (Size)templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

					if (desiredSize.IsZero)
					{
						return (Size)content.Measure(double.PositiveInfinity, double.PositiveInfinity);
					}

					return desiredSize;
				}
			}

			return SizeF.Zero;
		}

		internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
		{
			if (ChartArea == null)
			{
				return;
			}

			var x1 = SbsInfo.Start + x;
			var x2 = SbsInfo.End + x;
			var xCal = x1 + ((x2 - x1) / 2);
			var yCal = y;

			if (ActualYAxis != null && ActualXAxis != null && !double.IsNaN(yCal))
			{
				y = ChartArea.IsTransposed ? ActualXAxis.ValueToPoint(xCal) : ActualYAxis.ValueToPoint(yCal);
			}

			if (ActualXAxis != null && ActualYAxis != null && !double.IsNaN(x))
			{
				x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(yCal) : ActualXAxis.ValueToPoint(xCal);
			}
		}

		internal override void InitiateDataLabels(ChartSegment segment)
		{
			for (int i = 0; i < 2; i++)
			{
				var dataLabel = new ChartDataLabel();
				segment.DataLabels.Add(dataLabel);
				DataLabels.Add(dataLabel);
			}
		}

		internal override List<object>? GetDataPoints(double startX, double endX, double startY, double endY, int minimum, int maximum, List<double> xValues, bool validateYValues)
		{
			List<object> dataPoints = [];
			int count = xValues.Count;
			if (count == HighValues.Count && count == LowValues.Count && ActualData != null)
			{
				for (int i = minimum; i <= maximum; i++)
				{
					if ((startY <= HighValues[i] && HighValues[i] <= endY) || (startY <= LowValues[i] && LowValues[i] <= endY))
					{
						dataPoints.Add(ActualData[i]);
					}
				}

				return dataPoints;
			}
			else
			{
				return null;
			}
		}

		internal override float SumOfValues(IList<double> YValues)
		{
			float sum = 0f;

			if (YValues != null)
			{
				foreach (double number in YValues)
				{
					if (!double.IsNaN(number))
					{
						sum += (float)number;
					}
				}
			}

			return sum;
		}

		internal void SumOfValuesDynamicAdd(string yPath, float yValue)
		{
			switch (yPath)
			{
				case "High":
					_sumOfHighValues = float.IsNaN(_sumOfHighValues) ? yValue : _sumOfHighValues + yValue;
					break;
				case "Low":
					_sumOfLowValues = float.IsNaN(_sumOfLowValues) ? yValue : _sumOfLowValues + yValue;
					break;
			}
		}

		internal void SumOfValuesDynmaicRemove(string yPath, float yValue)
		{
			switch (yPath)
			{
				case "High":
					_sumOfHighValues -= yValue;
					break;
				case "Low":
					_sumOfLowValues -= yValue;
					break;
			}
		}

		internal void ResetSumOfValues()
		{
			_sumOfHighValues = float.NaN;
			_sumOfLowValues = float.NaN;
		}

		internal override DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
		{
			var texts = info.Text.Split('/');

			DataTemplate template = new DataTemplate(() =>
			{
				Grid grid = new Grid()
				{
					RowDefinitions =
					{
						new RowDefinition{Height = GridLength.Auto },
					},
				};

				grid.Add(GetTooltipContent(texts[0], texts[1], info), 0, 1);
				return grid;
			});

			return template;
		}

		#endregion

		#region Private Methods

		static void OnHighAndLowBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is RangeSeriesBase series)
			{
				series.OnBindingPathChanged();
			}
		}

		static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is RangeSeriesBase series)
			{
				series.UpdateStrokeColor();
				series.InvalidateSeries();
			}
		}

		static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is RangeSeriesBase series)
			{
				series.UpdateStrokeWidth();
				series.InvalidateSeries();
			}
		}

		static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is RangeSeriesBase series)
			{
				series.UpdateDashArray();
				series.InvalidateSeries();
			}
		}

		static StackLayout GetTooltipContent(string highValue, string lowValue, TooltipInfo info)
		{
			var layout = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.Fill,
			};

			Label highLabel = new Label()
			{
				Text = SfCartesianChartResources.High + " : " + highValue,
				VerticalTextAlignment = TextAlignment.Center,
#if WINDOWS
                HorizontalOptions = LayoutOptions.Fill,
                LineBreakMode = LineBreakMode.NoWrap,
#else
				HorizontalOptions = LayoutOptions.Start,
#endif
				TextColor = info.TextColor,
				Margin = info.Margin,
				Background = info.Background,
				FontAttributes = info.FontAttributes,
				FontSize = info.FontSize,
			};

			Label lowLabel = new Label
			{
				Text = SfCartesianChartResources.Low + "  : " + lowValue,
				VerticalTextAlignment = TextAlignment.Center,
#if WINDOWS
                HorizontalOptions = LayoutOptions.Fill,
                LineBreakMode = LineBreakMode.NoWrap,
#else
				HorizontalOptions = LayoutOptions.Start,
#endif
				TextColor = info.TextColor,
				Margin = info.Margin,
				Background = info.Background,
				FontAttributes = info.FontAttributes,
				FontSize = info.FontSize,
			};

			layout.Children.Add(highLabel);
			layout.Children.Add(lowLabel);

			return layout;
		}

		#endregion

		#endregion
	}
}