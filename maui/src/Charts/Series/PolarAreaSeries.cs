namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="PolarAreaSeries"/> is a series that displays data in terms of values and angles using a filled polygon shape. It allows for visually comparing several quantitative or qualitative aspects of a situation.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="PolarAreaSeries"/> class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="PolarSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="PolarAreaSeries"/> class, and also refer <see cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="PolarAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, create an instance of <see cref="PolarDataLabelSettings"/> and set it to the <see cref="PolarSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>true</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> Customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfPolarChart>
	///
	///           <chart:SfPolarChart.PrimaryAxis>
	///               <chart:NumericalAxis/>
	///           </chart:SfPolarChart.PrimaryAxis>
	///
	///           <chart:SfPolarChart.SecondaryAxis>
	///               <chart:NumericalAxis/>
	///           </chart:SfPolarChart.SecondaryAxis>
	///
	///               <chart:PolarAreaSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"/> 
	///           
	///     </chart:SfPolarChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfPolarChart chart = new SfPolarChart();
	///     
	///     NumericalAxis primaryAxis = new NumericalAxis();
	///     NumericalAxis secondaryAxis = new NumericalAxis();
	///     
	///     chart.PrimaryAxis = primaryAxis;
	///     chart.SecondaryAxis = secondaryAxis;
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     PolarAreaSeries series = new PolarAreaSeries();
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
	///        Data.Add(new Model() { XValue = A, YValue = 100 });
	///        Data.Add(new Model() { XValue = B, YValue = 150 });
	///        Data.Add(new Model() { XValue = C, YValue = 110 });
	///        Data.Add(new Model() { XValue = D, YValue = 230 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class PolarAreaSeries : PolarSeries
	{
		#region Bindable Property

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Stroke"/> property determines the brush used for the stroke (outline) of the series.
		/// </remarks> 
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(PolarAreaSeries),
			null,
			BindingMode.Default,
			null,
			OnStrokeChanged);

		#endregion

		#region Public Property

		/// <summary>
		/// Gets or sets a value to customize the stroke appearance of the polar area series.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfPolarChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
		///                                 XBindingPath="XValue"
		///                                 YBindingPath="YValue"
		///                                 Stroke = "Red" />
		///
		///     </chart:SfPolarChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-5)
		/// <code><![CDATA[
		///     SfPolarChart chart = new SfPolarChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     PolarAreaSeries series = new PolarAreaSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
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

		#endregion

		#region Methods

		#region Protected Method

		/// <inheritdoc/>
		protected override ChartSegment? CreateSegment()
		{
			return new PolarAreaSegment();
		}

		#endregion

		#region Internal Method

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var actualXValues = GetXValues();
			if (actualXValues == null || ActualData == null)
			{
				return;
			}

			List<double>? xValues = null, yValues = null;
			List<object>? items = null;
			for (int i = 0; i < PointsCount; i++)
			{
				if (!double.IsNaN(YValues[i]))
				{
					if (xValues == null)
					{
						xValues = [];
						yValues = [];
						items = [];
					}

					xValues.Add(actualXValues[i]);
					yValues?.Add(YValues[i]);
					items?.Add(ActualData[i]);
				}

				if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
				{
					if (xValues != null)
					{
						if (CreateSegment() is PolarAreaSegment segment)
						{
							segment.Series = this;
							segment.SeriesView = seriesView;
							if (yValues != null)
							{
								segment.SetData(xValues, yValues);
							}

							segment.Item = items;
							InitiateDataLabels(segment);
							_segments.Add(segment);
						}

						yValues = xValues = null;
						items = null;
					}

					if (double.IsNaN(YValues[i]))
					{
						xValues = [actualXValues[i]];
						yValues = [YValues[i]];
						items = [ActualData[i]];
						if (CreateSegment() is PolarAreaSegment segment)
						{
							segment.Series = this;
							segment.SetData(xValues, yValues);
							segment.Item = items;
							_segments.Add(segment);
						}

						yValues = xValues = null;
						items = null;
					}
				}
			}
		}

		internal override void InitiateDataLabels(ChartSegment segment)
		{
			for (int i = 0; i < PointsCount; i++)
			{
				ChartDataLabel dataLabel = new ChartDataLabel();
				segment.DataLabels.Add(dataLabel);
				DataLabels.Add(dataLabel);
			}
		}

		internal override void DrawDataLabels(ICanvas canvas)
		{
			var dataLabeSettings = DataLabelSettings;

			List<double> xValues = GetXValues()!;

			if (dataLabeSettings == null || _segments == null || _segments.Count <= 0)
			{
				return;
			}

			ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

			if (_segments[0] is not PolarAreaSegment dataLabel || xValues == null || YValues == null)
			{
				return;
			}

			for (int i = 0; i < PointsCount; i++)
			{
				double x = xValues[i], y = YValues[i];

				if (double.IsNaN(y))
				{
					continue;
				}

				CalculateDataPointPosition(i, ref x, ref y);
				PointF labelPoint = new PointF((float)x, (float)y);
				dataLabel.Index = i;
				dataLabel.LabelContent = GetLabelContent(YValues[i], SumOfValues(YValues));
				dataLabel.LabelPositionPoint = CalculateDataLabelPoint(dataLabel, labelPoint, labelStyle);
				UpdateDataLabelAppearance(canvas, dataLabel, dataLabeSettings, labelStyle);
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

		#endregion

		#region Private Method

		static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is PolarAreaSeries series)
			{
				series.UpdateStrokeColor();
				series.InvalidateSeries();
			}
		}

		#endregion

		#endregion
	}
}