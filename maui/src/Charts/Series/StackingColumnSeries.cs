using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="StackingColumnSeries"/> is a chart type that shows a set of vertical bars stacked on top of each other to represent data point values.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="StackingColumnSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="StackingSeriesBase.Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackingColumnSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackingColumnSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para> <b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
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
	///     <chart:StackingColumnSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "GoldMedals"/>
	///
	///     <chart:StackingColumnSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "SilverMedals"/>
	///
	///     <chart:StackingColumnSeries
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
	/// StackingColumnSeries goldSeries = new StackingColumnSeries();
	/// goldSeries.ItemsSource = viewModel.MedalDetails;
	/// goldSeries.XBindingPath = "CountryName";
	/// goldSeries.YBindingPath = "GoldMedals";
	///
	/// StackingColumnSeries silverSeries = new StackingColumnSeries();
	/// silverSeries.ItemsSource = viewModel.MedalDetails;
	/// silverSeries.XBindingPath = "CountryName";
	/// silverSeries.YBindingPath = "SilverMedals";
	///
	/// StackingColumnSeries bronzeSeries = new StackingColumnSeries();
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
	public partial class StackingColumnSeries : StackingSeriesBase, IDrawCustomLegendIcon, ISBSDependent
	{
		#region Fields

		double _yValue;

		#endregion

		#region Internal Properties

		internal override bool IsSideBySide => true;

		#endregion

		#region Bindable Properties
		/// <summary>
		/// Identifies the <see cref="Spacing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Spacing"/> property indicate spacing between the segments across the series.
		/// </remarks>
		public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
			nameof(Spacing),
			typeof(double),
			typeof(StackingColumnSeries),
			0d,
			BindingMode.Default,
			null,
			OnSizeChanged);

		/// <summary>
		/// Identifies the <see cref="Width"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Width"/> property indicates the width of the stacking column segment.
		/// </remarks>
		public static readonly BindableProperty WidthProperty = BindableProperty.Create(
			nameof(Width),
			typeof(double),
			typeof(StackingColumnSeries),
			0.8d,
			BindingMode.Default,
			null,
			OnSizeChanged);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CornerRadius"/> property helps to smooth column edges in stacking column series.
		/// </remarks>
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
			nameof(CornerRadius),
			typeof(CornerRadius),
			typeof(StackingColumnSeries),
			null,
			BindingMode.Default,
			null,
			OnCornerRadiusChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value to indicate spacing between the segments across the series.
		/// </summary>
		/// <value>It accepts <c>double</c> values ranging from 0 to 1, where the default value is 0.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-4)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:StackingColumnSeries ItemsSource = "{Binding Data}"
		///                                      XBindingPath = "XValue"
		///                                      YBindingPath = "YValue"
		///                                      Spacing = "0.3"/>
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
		///     StackingColumnSeries stackingSeries = new StackingColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Spacing = 0.3,
		///     };
		///     
		///     chart.Series.Add(stackingSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double Spacing
		{
			get { return (double)GetValue(SpacingProperty); }
			set { SetValue(SpacingProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to change the width of the stacking column segment.
		/// </summary>
		/// <value>It accepts <c>double</c> values ranging from 0 to 1, where the default value is <c>0.8</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-6)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:StackingColumnSeries ItemsSource = "{Binding Data}"
		///                                      XBindingPath = "XValue"
		///                                      YBindingPath = "YValue"
		///                                      Width = "0.7"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-7)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     StackingColumnSeries stackingSeries = new StackingColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Width = 0.7,
		///     };
		///     
		///     chart.Series.Add(stackingSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for the corner radius that helps to smooth column edges in stacking column series.
		/// </summary>
		/// <value>It accepts <see cref="Microsoft.Maui.CornerRadius"/> value, and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-8)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:StackingColumnSeries ItemsSource = "{Binding Data}"
		///                            XBindingPath = "XValue"
		///                            YBindingPath = "YValue"
		///                            CornerRadius = "5"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-9)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     StackingColumnSeries stackingSeries = new StackingColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           CornerRadius = new CornerRadius(5)
		///     };
		///     
		///     chart.Series.Add(stackingSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			if (this is StackingColumn100Series)
			{
				var pathF = new PathF();
				pathF.MoveTo(1, 0);
				pathF.LineTo(3, 0);
				pathF.LineTo(3, 3);
				pathF.LineTo(1, 3);
				pathF.LineTo(1, 0);
				pathF.Close();

				pathF.MoveTo(5, 0);
				pathF.LineTo(7, 0);
				pathF.LineTo(7, 5);
				pathF.LineTo(5, 5);
				pathF.LineTo(5, 0);
				pathF.Close();

				pathF.MoveTo(7, 6);
				pathF.LineTo(5, 6);
				pathF.LineTo(5, 9);
				pathF.LineTo(7, 9);
				pathF.LineTo(7, 6);
				pathF.Close();

				pathF.MoveTo(1, 4);
				pathF.LineTo(3, 4);
				pathF.LineTo(3, 7);
				pathF.LineTo(1, 7);
				pathF.LineTo(1, 4);
				pathF.Close();

				pathF.MoveTo(7, 10);
				pathF.LineTo(5, 10);
				pathF.LineTo(5, 12);
				pathF.LineTo(7, 12);
				pathF.LineTo(7, 10);
				pathF.Close();

				pathF.MoveTo(1, 8);
				pathF.LineTo(3, 8);
				pathF.LineTo(3, 12);
				pathF.LineTo(1, 12);
				pathF.LineTo(1, 8);
				pathF.Close();

				pathF.MoveTo(11, 8);
				pathF.LineTo(9, 8);
				pathF.LineTo(9, 12);
				pathF.LineTo(11, 12);
				pathF.LineTo(11, 8);
				pathF.Close();

				pathF.MoveTo(9, 3);
				pathF.LineTo(11, 3);
				pathF.LineTo(11, 7);
				pathF.LineTo(9, 7);
				pathF.LineTo(9, 3);
				pathF.Close();

				pathF.MoveTo(11, 0);
				pathF.LineTo(9, 0);
				pathF.LineTo(9, 2);
				pathF.LineTo(11, 2);
				pathF.LineTo(11, 0);
				pathF.Close();

				canvas.FillPath(pathF);
			}
			else
			{
				RectF innerRect1 = new(1, 7, 2, 2);
				canvas.SetFillPaint(fillColor, innerRect1);
				canvas.FillRectangle(innerRect1);

				RectF innerRect2 = new(1, 10, 2, 2);
				canvas.SetFillPaint(fillColor, innerRect2);
				canvas.FillRectangle(innerRect2);

				RectF innerRect3 = new(1, 4, 2, 2);
				canvas.SetFillPaint(fillColor, innerRect3);
				canvas.FillRectangle(innerRect3);

				RectF innerRect4 = new(5, 0, 2, 3);
				canvas.SetFillPaint(fillColor, innerRect4);
				canvas.FillRectangle(innerRect4);

				RectF innerRect5 = new(5, 4, 2, 3);
				canvas.SetFillPaint(fillColor, innerRect5);
				canvas.FillRectangle(innerRect5);

				RectF innerRect6 = new(5, 8, 2, 4);
				canvas.SetFillPaint(fillColor, innerRect6);
				canvas.FillRectangle(innerRect6);

				RectF innerRect7 = new(9, 9, 2, 3);
				canvas.SetFillPaint(fillColor, innerRect7);
				canvas.FillRectangle(innerRect7);

				RectF innerRect8 = new(9, 5, 2, 3);
				canvas.SetFillPaint(fillColor, innerRect8);
				canvas.FillRectangle(innerRect8);

				RectF innerRect9 = new(9, 2, 2, 2);
				canvas.SetFillPaint(fillColor, innerRect9);
				canvas.FillRectangle(innerRect9);

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
			return new ColumnSegment();
		}

		#endregion

		#region Internal Methods

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();
			if (xValues == null)
			{
				return;
			}

			if (BottomValues == null)
			{
				ChartArea?.UpdateStackingSeries();
			}

			if (BottomValues == null)
			{
				return;
			}

			var yStartValues = BottomValues;
			var yEndValues = TopValues;

			if (yEndValues != null && yStartValues != null)
			{
				if (IsGrouped && (IsIndexed || xValues == null))
				{
					for (int i = 0; i < PointsCount; i++)
					{
						if (i < _segments.Count && _segments[i] is ColumnSegment segment)
						{
							segment.SetData([i + SbsInfo.Start, i + SbsInfo.End, yEndValues[i], yStartValues[i], i, YValues[i]]);
						}
						else
						{
							CreateSegment(i, [i + SbsInfo.Start, i + SbsInfo.End, yEndValues[i], yStartValues[i], i, YValues[i]]);
						}
					}
				}
				else
				{
					for (var i = 0; i < PointsCount; i++)
					{
						if (xValues != null)

						{
							var x = xValues[i];

							if (i < _segments.Count && _segments[i] is ColumnSegment segment)
							{
								segment.SetData([x + SbsInfo.Start, x + SbsInfo.End, yEndValues[i], yStartValues[i], x, YValues[i]]);
							}
							else
							{
								CreateSegment(i, [x + SbsInfo.Start, x + SbsInfo.End, yEndValues[i], yStartValues[i], x, YValues[i]]);
							}
						}
					}
				}
			}
		}

		internal override double GetActualSpacing()
		{
			return Spacing;
		}

		internal override double GetActualWidth()
		{
			return Width;
		}

		internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			if (ChartArea == null)
			{
				return;
			}

			float xPosition = tooltipInfo.X;
			float yPosition = tooltipInfo.Y;

			if (_segments[tooltipInfo.Index] is ColumnSegment columnSegment)
			{
				if (ChartArea.IsTransposed && columnSegment.Series is CartesianSeries series && series.ActualXAxis != null)
				{
					float width = columnSegment.SegmentBounds.Width;
					float height = columnSegment.SegmentBounds.Height;
					Rect targetRect;
					var actualCrossingValue = series.ActualXAxis.ActualCrossingValue;

					if (_yValue < (double.IsNaN(actualCrossingValue) ? 0 : actualCrossingValue))
					{
						targetRect = new Rect(xPosition, yPosition - height / 2, width, height);
					}
					else
					{
						//In negative segment target rect
						targetRect = new Rect(xPosition - width, yPosition - height / 2, width, height);
					}

					tooltipInfo.TargetRect = targetRect;
				}
				else
				{
					float width = columnSegment.SegmentBounds.Width;
					float height = 1;
					Rect targetRect = new Rect(xPosition - width / 2, yPosition, width, height);
					tooltipInfo.TargetRect = targetRect;
				}
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

			if (xValues == null || ChartArea == null || TopValues == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
			double xValue = xValues[index];
			IList<double> yValues = SeriesYValues[0];
			double content = Convert.ToDouble(yValues[index]);
			_yValue = TopValues[index];
			float xPosition = TransformToVisibleX(xValue, _yValue);

			if (!double.IsNaN(xPosition) && !double.IsNaN(_yValue) && !double.IsNaN(content))
			{
				float yPosition = TransformToVisibleY(xValue, _yValue);

				if (IsSideBySide)
				{
					double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
					xPosition = TransformToVisibleX(xMidVal, _yValue);
					yPosition = TransformToVisibleY(xMidVal, _yValue);
				}

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
					FontFamily = tooltipBehavior.FontFamily,
					FontAttributes = tooltipBehavior.FontAttributes,
					Text = content.ToString()
				};

				UpdateTooltipAppearance(tooltipInfo, tooltipBehavior);
				tooltipInfo.Item = dataPoint;

				return tooltipInfo;
			}

			return null;
		}

		internal override double GetDataLabelPositionAtIndex(int index)
		{
			if (DataLabelSettings == null)
			{
				return 0;
			}

			double yValue = 0;
			double top = TopValues?[index] ?? 0f;
			double bottom = BottomValues?[index] ?? 0f;

			switch (DataLabelSettings.BarAlignment)
			{
				case DataLabelAlignment.Bottom:
					yValue = bottom;
					break;
				case DataLabelAlignment.Middle:
					yValue = bottom + ((top - bottom) / 2);
					break;
				case DataLabelAlignment.Top:
					yValue = top;
					break;
			}

			return yValue;
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

		internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
		{
			if (ChartArea == null)
			{
				return labelPosition;
			}

			if (ChartArea.IsTransposed)
			{
				return DataLabelSettings.GetLabelPositionForTransposedRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
			}

			return DataLabelSettings.GetLabelPositionForRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
		}

		internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> PointInfos, ref bool isSideBySide)
		{
		}

		#endregion

		#region Private Methods

		static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is StackingColumnSeries columnSeries && columnSeries.Chart != null)
			{
				columnSeries.InvalidateSeries();
			}
		}

		static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is StackingColumnSeries columnSeries && columnSeries.ChartArea != null)
			{
				columnSeries.UpdateSbsSeries();
			}
		}

		void CreateSegment(int index, double[] values)
		{
			if (CreateSegment() is ColumnSegment segment)
			{
				segment.Series = this;
				segment.SetData(values);
				segment.Index = index;
				segment.Item = ActualData?[index];
				InitiateDataLabels(segment);
				_segments.Add(segment);
			}
		}

		#endregion

		#endregion
	}
}