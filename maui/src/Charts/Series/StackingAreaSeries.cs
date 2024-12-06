using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="StackingAreaSeries"/> is a collection of data points, where the areas are stacked on top of each other.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="StackingAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="StackingSeriesBase.Stroke"/>, <see cref="StackingSeriesBase.StrokeDashArray"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackingAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackingAreaSeries"/> class. To customize the chart data labels placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///     <chart:SfCartesianChart.XAxes>
	///         <chart:CategoryAxis/>
	///     </chart:SfCartesianChart.XAxes>
	///
	///     <chart:SfCartesianChart.YAxes>
	///         <chart:NumericalAxis/>
	///     </chart:SfCartesianChart.YAxes>
	///
	///     <chart:StackingAreaSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "GoldMedals"/>
	///
	///     <chart:StackingAreaSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "SilverMedals"/>
	///
	///     <chart:StackingAreaSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "BronzeMedals"/>
	/// </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	///
	/// CategoryAxis xAxis = new CategoryAxis();
	/// NumericalAxis yAxis = new NumericalAxis();
	///
	/// chart.XAxes.Add(xAxis);
	/// chart.YAxes.Add(yAxis);
	///
	/// ViewModel viewModel = new ViewModel();
	///
	/// StackingAreaSeries goldSeries = new StackingAreaSeries();
	/// goldSeries.ItemsSource = viewModel.MedalDetails;
	/// goldSeries.XBindingPath = "CountryName";
	/// goldSeries.YBindingPath = "GoldMedals";
	///
	/// StackingAreaSeries silverSeries = new StackingAreaSeries();
	/// silverSeries.ItemsSource = viewModel.MedalDetails;
	/// silverSeries.XBindingPath = "CountryName";
	/// silverSeries.YBindingPath = "SilverMedals";
	///
	/// StackingAreaSeries bronzeSeries = new StackingAreaSeries();
	/// bronzeSeries.ItemsSource = viewModel.MedalDetails;
	/// bronzeSeries.XBindingPath = "CountryName";
	/// bronzeSeries.YBindingPath = "BronzeMedals";
	///
	/// chart.Series.Add(goldSeries);
	/// chart.Series.Add(silverSeries);
	/// chart.Series.Add(bronzeSeries);
	///
	/// this.Content = chart;
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<MedalData> MedalDetails { get; set; }
	///
	///     public ViewModel()
	///     {
	///         MedalDetails = new ObservableCollection<MedalData>
	///         {
	///             new MedalData() { CountryName = "USA", GoldMedals = 10, SilverMedals = 5, BronzeMedals = 7 },
	///             new MedalData() { CountryName = "China", GoldMedals = 8, SilverMedals = 10, BronzeMedals = 6 },
	///             new MedalData() { CountryName = "Russia", GoldMedals = 6, SilverMedals = 4, BronzeMedals = 8 },
	///             new MedalData() { CountryName = "UK", GoldMedals = 4, SilverMedals = 7, BronzeMedals = 3 }
	///         };
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	/// <seealso cref="StackingAreaSegment"/>
	public partial class StackingAreaSeries : StackingSeriesBase, IDrawCustomLegendIcon, IMarkerDependent
	{
		#region Fields

		bool _needToAnimateMarker;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ShowMarkers"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowMarkers"/> property determines whether the chart markers are visible on the series.
		/// </remarks>
		public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

		/// <summary>
		/// Identifies the <see cref="MarkerSettings"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="MarkerSettings"/> property allows customize the series markers.
		/// </remarks>
		public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the value indicating whether to show markers for the series data point.
		/// </summary>
		/// <value>It accepts <c>bool</c>> values and its default value is false.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:StackingAreaSeries ItemsSource = "{Binding Data}"
		///                                    XBindingPath = "XValue"
		///                                    YBindingPath = "YValue"
		///                                    ShowMarkers = "True"/>
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
		///     StackingAreaSeries series = new StackingAreaSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           ShowMarkers = true,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowMarkers
		{
			get { return (bool)GetValue(ShowMarkersProperty); }
			set { SetValue(ShowMarkersProperty, value); }
		}

		/// <summary>
		/// Gets or sets the option to customize the series markers.
		/// </summary>
		/// <value>It accepts <see cref="ChartMarkerSettings"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:StackingAreaSeries ItemsSource = "{Binding Data}"
		///                                    XBindingPath = "XValue"
		///                                    YBindingPath = "YValue"
		///                                    ShowMarkers = "True">
		///               <chart:StackingAreaSeries.MarkerSettings>
		///                     <chart:ChartMarkerSettings Fill = "Red" Height = "15" Width = "15"/>
		///               </chart:StackingAreaSeries.MarkerSettings>
		///          </chart:StackingAreaSeries>
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
		///    ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings()
		///    {
		///        Fill = new SolidColorBrush(Colors.Red),
		///        Height = 15,
		///        Width = 15,
		///    };
		///     StackingAreaSeries series = new StackingAreaSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           ShowMarkers = true,
		///           MarkerSettings = chartMarkerSettings,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartMarkerSettings MarkerSettings
		{
			get { return (ChartMarkerSettings)GetValue(MarkerSettingsProperty); }
			set { SetValue(MarkerSettingsProperty, value); }
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
			pathF.MoveTo(0, 7);
			pathF.LineTo(4, 0);
			pathF.LineTo(7, 7);
			pathF.LineTo(12, 5);
			pathF.LineTo(12, 12);
			pathF.LineTo(0, 12);
			pathF.LineTo(0, 7);
			pathF.Close();
			canvas.FillPath(pathF);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}
		}

		void IMarkerDependent.InvalidateDrawable()
		{
			InvalidateSeries();
		}

		ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

		void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => DrawMarker(canvas, index, type, rect);

		bool IMarkerDependent.NeedToAnimateMarker { get => _needToAnimateMarker; set => _needToAnimateMarker = EnableAnimation; }

		#endregion

		#region Methods

		#region Public Methods

		/// <inheritdoc/>
		public override int GetDataPointIndex(float pointX, float pointY)
		{
			if (Chart != null)
			{
				var dataPoint = FindNearestChartPoint(pointX, pointY);
				if (dataPoint == null || ActualData == null)
				{
					return -1;
				}

				var tooltipIndex = ActualData.IndexOf(dataPoint);
				if (tooltipIndex < 0)
				{
					return -1;
				}

				List<PointF> segPoints = [];

				foreach (StackingAreaSegment segment in _segments.Cast<StackingAreaSegment>())
				{
					var points = segment.FillPoints;
					if (points != null)
					{
						for (int i = 0; i < points.Count; i += 2)
						{
							if (i + 1 < points.Count)
							{
								float x = points[i];
								float y = points[i + 1];
								segPoints.Add(new PointF(x, y));
							}
						}
					}
				}

				float xPos = pointX - (float)AreaBounds.Left;
				float yPos = pointY - (float)AreaBounds.Top;

				if (ChartUtils.IsAreaContains(segPoints, xPos, yPos))
				{
					return tooltipIndex;
				}
			}

			return -1;
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected override ChartSegment? CreateSegment()
		{
			return new StackingAreaSegment();
		}

		/// <summary>
		/// Draws the markers for the stacking area series at each data point location on the chart.
		/// </summary>
		protected virtual void DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect)
		{
			if (this is IMarkerDependent markerDependent)
			{
				canvas.DrawShape(rect, shapeType: type, markerDependent.MarkerSettings.HasBorder, false);
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (MarkerSettings != null)
			{
				SetInheritedBindingContext(MarkerSettings, BindingContext);
			}
		}

		#endregion

		#region Internal Methods

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var actualXValues = GetXValues();

			if (actualXValues == null || actualXValues.Count == 0)
			{
				return;
			}

			List<double>? xValues = null, yEnds = null, yStarts = null;
			List<object>? items = null;

			if (BottomValues == null)
			{
				ChartArea?.UpdateStackingSeries();
			}

			if (BottomValues == null)
			{
				return;
			}

			for (int i = 0; i < PointsCount; i++)
			{
				var yStartValues = BottomValues;
				var yEndValues = TopValues;

				if (!double.IsNaN(YValues[i]))
				{
					if (xValues == null)
					{
						xValues = [];
						yEnds = [];
						yStarts = [];
						items = [];
					}

					xValues.Add(actualXValues[i]);
					yEnds?.Add(yEndValues![i]);
					yStarts?.Add(yStartValues![i]);
					items?.Add(ActualData![i]);
				}

				if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
				{
					if (xValues != null)
					{
						if (CreateSegment() is StackingAreaSegment segment)
						{
							segment.Series = this;
							segment.SeriesView = seriesView;
							if (yStarts != null && yEnds != null)
							{
								segment.SetData(xValues, yEnds, yStarts);
							}

							segment.Item = items;
							InitiateDataLabels(segment);
							_segments.Add(segment);
						}

						yEnds = xValues = yStarts = null;
						items = null;
					}

					if (double.IsNaN(YValues[i]))
					{
						xValues = [actualXValues[i]];
						yStarts = [YValues[i]];
						yEnds = [YValues[i]];
						items = [ActualData![i]];

						if (CreateSegment() is StackingAreaSegment segment)
						{
							segment.Series = this;
							segment.Item = items;
							segment.SetData(xValues, yEnds, yStarts);
							yEnds = xValues = yStarts = null;
							items = null;
						}
					}
				}
			}
		}

		internal override void InitiateDataLabels(ChartSegment segment)
		{
			for (int i = 0; i < PointsCount; i++)
			{
				var dataLabel = new ChartDataLabel();
				segment.DataLabels.Add(dataLabel);
				DataLabels.Add(dataLabel);
			}
		}

		internal override void UpdateRange()
		{
			bool isStacking100 = this is StackingArea100Series;
			double yStart = YRange.Start;
			double yEnd = YRange.End;

			if (yStart > 0)
			{
				yStart = 0;
			}

			if (yEnd < 0)
			{
				yEnd = 0;
			}

			if (isStacking100)
			{
				yStart = yStart <= -100 ? -100 : yStart;
				yEnd = yEnd >= 100 ? 100 : yEnd;
			}

			YRange = new DoubleRange(yStart, yEnd);
			base.UpdateRange();
		}

		internal override bool IsIndividualSegment()
		{
			return false;
		}

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			if (_segments == null)
			{
				return null;
			}

			int index = SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

			if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
				|| ActualYAxis == null || SeriesYValues == null)
			{
				return null;
			}

			var xValues = GetXValues();

			if (xValues == null || ChartArea == null || TopValues == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
			double xValue = xValues[index];
			IList<double> yValues = SeriesYValues[0];
			double content = Convert.ToDouble(yValues[index]);
			double yValue = TopValues[index];
			float xPosition = TransformToVisibleX(xValue, yValue);

			if (!double.IsNaN(xPosition) && !double.IsNaN(yValue) && !double.IsNaN(content))
			{
				float yPosition = TransformToVisibleY(xValue, yValue);

				RectF seriesBounds = AreaBounds;
				seriesBounds = new RectF(0, 0, seriesBounds.Width, seriesBounds.Height);
				yPosition = seriesBounds.Top < yPosition ? yPosition : seriesBounds.Top;
				yPosition = seriesBounds.Bottom > yPosition ? yPosition : seriesBounds.Bottom;
				xPosition = seriesBounds.Left < xPosition ? xPosition : seriesBounds.Left;
				xPosition = seriesBounds.Right > xPosition ? xPosition : seriesBounds.Right;

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
					Text = content.ToString(),
					Item = dataPoint
				};

				return tooltipInfo;
			}

			return null;
		}

		internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
		{
			var xValues = GetXValues();

			if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null && TopValues != null)
			{
				IList<double> topValues = TopValues;
				IList<double> yValues = SeriesYValues[0];

				foreach (object point in nearestDataPoints)
				{
					int index = ActualData.IndexOf(point);
					var xValue = xValues[index];
					double topValue = topValues[index];
					double yValue = yValues[index];
					if (double.IsNaN(yValue))
					{
						continue;
					}

					string label = yValue.ToString();
					var xPoint = TransformToVisibleX(xValue, topValue);
					var yPoint = TransformToVisibleY(xValue, topValue);

					TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPoint, yPoint, label, point);

					if (chartPointInfo != null)
					{
						chartPointInfo.XValue = xValue;
						chartPointInfo.YValues.Add(yValue);
						pointInfos.Add(chartPointInfo);
					}
				}
			}
		}

		internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
		{
			var label = pointInfo.YValues[0].ToString(labelFormat);
			pointInfo.Label = label;
		}

		internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
		{
			return DataLabelSettings.GetLabelPositionForAreaSeries(this, dataLabel, labelSize, labelPosition, padding);
		}

		internal override void DrawDataLabels(ICanvas canvas)
		{
			var dataLabelSettings = DataLabelSettings;

			List<double> xValues = GetXValues()!;

			IList<double> actualYValues = YValues;

			if (dataLabelSettings == null || _segments == null || _segments.Count <= 0)
			{
				return;
			}

			ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

			foreach (StackingAreaSegment dataLabel in _segments.Cast<StackingAreaSegment>())
			{
				if (dataLabel == null || dataLabel.XValues == null || dataLabel.BottomValues == null || dataLabel.TopValues == null)
				{
					return;
				}

				for (int i = 0; i < PointsCount; i++)
				{
					double x = xValues[i];
					double y = TopValues![i];

					if (double.IsNaN(y))
					{
						continue;
					}

					CalculateDataPointPosition(i, ref x, ref y);
					PointF labelPoint = new PointF((float)x, (float)y);
					dataLabel.Index = i;
					dataLabel.LabelContent = GetLabelContent(actualYValues[i], SumOfValues(YValues));
					dataLabel.LabelPositionPoint = CartesianDataLabelSettings.CalculateDataLabelPoint(this, dataLabel, labelPoint, labelStyle);
					UpdateDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
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

		internal override void UpdateLegendItemToggle()
		{
			if (ChartArea != null)
			{
				var visibleSeries = ChartArea.VisibleSeries;

				if (visibleSeries == null)
				{
					return;
				}

				foreach (var chartSeries in visibleSeries)
				{
					chartSeries.SegmentsCreated = false;
				}
			}
		}

		#endregion

		#endregion
	}
}