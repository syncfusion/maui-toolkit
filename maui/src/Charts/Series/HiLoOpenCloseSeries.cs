using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="HiLoOpenCloseSeries"/> displays a set of OHLC segments used in financial analysis to represent open, high, low, and close values of an asset or security.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="HiLoOpenCloseSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="CandleSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
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
	///               <chart:HiLoOpenCloseSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   High="High"
	///                   Low="Low"
	///                   Open="Open"
	///                   Close="Close"/>
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
	///     HiLoOpenCloseSeries series = new HiLoOpenCloseSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.High = "High";
	///     series.Low = "Low";
	///     series.Open = "Open";
	///     series.Close = "Close";
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
	///        Data.Add(new Model() { XValue = "2000", High = 50, Low = 40, Open = 47, Close = 45});
	///        Data.Add(new Model() { XValue = "2001", High = 50, Low = 35, Open = 45, Close = 40});
	///        Data.Add(new Model() { XValue = "2002", High = 45, Low = 30, Open = 37, Close = 40 });
	///        Data.Add(new Model() { XValue = "2003", High = 50, Low = 35, Open = 40, Close = 45});
	///        Data.Add(new Model() { XValue = "2004", High = 45, Low = 30, Open = 35, Close = 32 });
	///        Data.Add(new Model() { XValue = "2005", High = 50, Low = 35, Open = 40, Close = 45 });
	///        Data.Add(new Model() { XValue = "2006", High = 40, Low = 31, Open = 36, Close = 34 });
	///        Data.Add(new Model() { XValue = "2007", High = 48, Low = 38, Open = 43, Close = 40});
	///        Data.Add(new Model() { XValue = "2008", High = 55, Low = 45, Open = 48, Close = 50});
	///        Data.Add(new Model() { XValue = "2009", High = 45, Low = 30, Open = 35, Close = 40});
	///        Data.Add(new Model() { XValue = "2010", High = 50, Low = 50, Open = 50, Close = 50 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class HiLoOpenCloseSeries : FinancialSeriesBase, IDrawCustomLegendIcon
	{
		#region Internal Properties

		internal override bool IsSideBySide => true;

		#endregion

		#region Interface Implementation

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			if (this is not null && this is not CandleSeries)
			{
				var pathF = new PathF();
				pathF.MoveTo(3, 4);
				pathF.LineTo(3, 5);
				pathF.LineTo(1, 5);
				pathF.LineTo(1, 7);
				pathF.LineTo(3, 7);
				pathF.LineTo(3, 11);
				pathF.LineTo(5, 11);
				pathF.LineTo(5, 10);
				pathF.LineTo(7, 10);
				pathF.LineTo(7, 8);
				pathF.LineTo(5, 8);
				pathF.LineTo(5, 4);
				pathF.LineTo(3, 4);
				pathF.Close();
				pathF.MoveTo(8, 1);
				pathF.LineTo(8, 2);
				pathF.LineTo(6, 2);
				pathF.LineTo(6, 4);
				pathF.LineTo(8, 4);
				pathF.LineTo(8, 8);
				pathF.LineTo(10, 8);
				pathF.LineTo(10, 7);
				pathF.LineTo(12, 7);
				pathF.LineTo(12, 5);
				pathF.LineTo(10, 5);
				pathF.LineTo(10, 1);
				pathF.LineTo(8, 1);
				pathF.Close();
				canvas.FillPath(pathF);
			}
			else if (this is CandleSeries)
			{
				RectF innerRect1 = new(2, 5, 3, 5);
				canvas.SetFillPaint(fillColor, innerRect1);
				canvas.FillRectangle(innerRect1);

				RectF innerRect2 = new(7, 2, 3, 5);
				canvas.SetFillPaint(fillColor, innerRect2);
				canvas.FillRectangle(innerRect2);

				RectF innerRect3 = new(3, 3, 1, 9);
				canvas.SetFillPaint(fillColor, innerRect3);
				canvas.FillRectangle(innerRect3);

				RectF innerRect4 = new(8, 0, 1, 9);
				canvas.SetFillPaint(fillColor, innerRect4);
				canvas.FillRectangle(innerRect4);
			}

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Methods

		#region Protected Method

		/// <inheritdoc/>
		protected override ChartSegment? CreateSegment()
		{
			return new HiLoOpenCloseSegment();
		}

		#endregion

		#region Internal method

		internal override Brush? GetFillColor(object item, int index)
		{
			Brush? fillColor = base.GetFillColor(item, index);

			if (fillColor == Chart?.GetSelectionBrush(this) || fillColor == GetSelectionBrush(item, index))
			{
				return fillColor;
			}

			if (_segments.Count == 0)
			{
				return base.GetFillColor(item, index);
			}

			if (_segments[index] is HiLoOpenCloseSegment segment)
			{
				if (segment.IsBull && segment.IsFill)
				{
					fillColor = Fill != null ? fillColor : BullishFill;
				}
				else if (!(segment.IsBull) && !(segment.IsFill))
				{
					fillColor = Brush.Transparent;
				}
				else if (segment.IsBull)
				{
					if (this is CandleSeries)
					{
						fillColor = Fill != null ? fillColor
									: (this is CandleSeries series && series.EnableSolidCandle ? BullishFill : Brush.Transparent);

					}
					else
					{
						fillColor = Fill != null ? fillColor : BullishFill;
					}
				}
				else
				{
					fillColor = Fill != null ? fillColor : BearishFill;
				}
			}

			return fillColor;
		}

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();

			if (xValues == null || OpenValues.Count == 0)
			{
				return;
			}

			bool isFill;

			for (int i = 0; i < PointsCount; i++)
			{
				if (i > 0)
				{
					_ = CloseValues[i - 1];
				}

				bool isBull;

				if (this is CandleSeries series)
				{
					isFill = series.EnableSolidCandle || !(OpenValues[i] < CloseValues[i]);
					isBull = series.EnableSolidCandle ? OpenValues[i] < CloseValues[i] : (i == 0 ? OpenValues[i] : CloseValues[i - 1]) <= CloseValues[i];
				}
				else
				{
					isBull = OpenValues[i] < CloseValues[i];
					isFill = OpenValues[i] > CloseValues[i];
				}

				var x = xValues[i];

				if (i < _segments.Count)
				{
					_segments[i].SetData([x + SbsInfo.Start, x + SbsInfo.Median, x + SbsInfo.End, OpenValues[i], HighValues[i], LowValues[i], CloseValues[i], x], isFill, isBull);
				}
				else
				{
					CreateSegment(seriesView, [ x + SbsInfo.Start, x + SbsInfo.Median, x + SbsInfo.End, OpenValues[i], HighValues[i]
					 , LowValues[i], CloseValues[i], x ], isFill, isBull, i);
				}
			}
		}

		internal virtual void CreateSegment(SeriesView seriesView, double[] values, bool isFill, bool isBull, int index)
		{
			if (CreateSegment() is HiLoOpenCloseSegment segment)
			{
				segment.Series = this;
				segment.Index = index;
				segment.SeriesView = seriesView;
				segment.SetData(values, isFill, isBull);
				segment.Item = ActualData?[index];
				InitiateDataLabels(segment);
				_segments.Add(segment);
			}
		}

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			int index = IsSideBySide ? GetDataPointIndex(x, y) : SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

			if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
				|| ActualYAxis == null || SeriesYValues == null)
			{
				return null;
			}

			var xValues = GetXValues();

			if (xValues == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
			double highValue = HighValues[index];
			double lowValue = LowValues[index];
			double openValue = OpenValues[index];
			double closeValue = CloseValues[index];
			double xValue = xValues[index];
			double yValue = highValue;

			float xPosition = TransformToVisibleX(xValue, yValue);

			if (!double.IsNaN(xPosition) && !double.IsNaN(yValue))
			{
				float yPosition = TransformToVisibleY(xValue, yValue);

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
					Text = (yValue == 0 ? yValue.ToString(" 0.##") : yValue.ToString(" #.##")) + "/" + (lowValue == 0 ? lowValue.ToString(" 0.##") : lowValue.ToString(" #.##")) + "/" + (openValue == 0 ? openValue.ToString(" 0.##") : openValue.ToString(" #.##")) + "/" + (closeValue == 0 ? closeValue.ToString(" 0.##") : closeValue.ToString(" #.##")),
					Margin = tooltipBehavior.Margin,
					TextColor = tooltipBehavior.TextColor,
					FontFamily = tooltipBehavior.FontFamily,
					FontSize = tooltipBehavior.FontSize,
					FontAttributes = tooltipBehavior.FontAttributes,
					Background = tooltipBehavior.Background,
					Item = dataPoint
				};

				return tooltipInfo;
			}

			return null;
		}

		internal override DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
		{
			var template = new DataTemplate(() =>
			{
				var texts = info.Text.Split('/');

				string highFormat = $"{SfCartesianChartResources.High}    :";
				string lowFormat = $"{SfCartesianChartResources.Low}     :";
				string openFormat = $"{SfCartesianChartResources.Open}  :";
				string closeFormat = $"{SfCartesianChartResources.Close}  :";

				var labels = new[]
					{
						new TooltipLabelValue(highFormat, texts[0]),
						new TooltipLabelValue(lowFormat, texts[1]),
						new TooltipLabelValue(openFormat, texts[2]),
						new TooltipLabelValue(closeFormat, texts[3]),
					};

				StackLayout stackLayout = [];
				BindableLayout.SetItemsSource(stackLayout, labels);
				BindableLayout.SetItemTemplate(stackLayout, new DataTemplate(() =>
				{
					StackLayout stackLayout1 = [];
					stackLayout1.Orientation = StackOrientation.Horizontal;
					stackLayout1.Add(new Label()
					{
						HorizontalOptions = LayoutOptions.Start,
						VerticalOptions = LayoutOptions.Start,
						TextColor = info.TextColor,
						Background = info.Background,
						Margin = info.Margin,
						FontAttributes = info.FontAttributes,
						FontSize = info.FontSize,
					});
					stackLayout1.Add(new Label()
					{
						HorizontalOptions = LayoutOptions.End,
						VerticalOptions = LayoutOptions.Start,
						HorizontalTextAlignment = TextAlignment.Start,
						TextColor = info.TextColor,
						Background = info.Background,
						Margin = info.Margin,
						FontAttributes = info.FontAttributes,
						FontSize = info.FontSize,
					});

					((Label)stackLayout1.Children[0]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty,
							BindingHelper.CreateBinding(nameof(TooltipLabelValue.LabelText), getter: static (TooltipLabelValue label) => label.LabelText));
					((Label)stackLayout1.Children[1]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty,
						BindingHelper.CreateBinding(nameof(TooltipLabelValue.ValueText), getter: static (TooltipLabelValue label) => label.ValueText));
					return stackLayout1;
				}));

				return stackLayout;
			});

			return template;
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
					double highValue = HighValues[index];
					double lowValue = LowValues[index];
					double openValue = OpenValues[index];
					double closeValue = CloseValues[index];
					string label = $"{SfCartesianChartResources.High} : {highValue}\n" +
								   $"{SfCartesianChartResources.Low} : {lowValue}\n" +
								   $"{SfCartesianChartResources.Open} : {openValue}\n" +
								   $"{SfCartesianChartResources.Close} : {closeValue}";

					if (IsSideBySide)
					{
						isSideBySide = true;
						double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
						xPosition = TransformToVisibleX(xMidVal, highValue);
						yPosition = TransformToVisibleY(xMidVal, highValue);
					}

					// Checking YValue is contain in plotArea
					//Todo: need to check with transposed
					//if (!AreaBounds.Contains(xPoint + AreaBounds.Left, yPoint + AreaBounds.Top)) continue;

					TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

					//chartPointInfo.TooltipHelper.Text = label;
					if (chartPointInfo != null)
					{
#if ANDROID
						Size contentSize = ChartUtils.GetLabelSize(label, chartPointInfo.TooltipHelper);
						chartPointInfo.GroupLabelSize = contentSize;
#endif
						chartPointInfo.XValue = xValue;
						chartPointInfo.YValues = [highValue, lowValue, openValue, closeValue];
						pointInfos.Add(chartPointInfo);
					}
				}
			}
		}

		internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
		{
			var yValues = pointInfo.YValues;
			string label = $"{SfCartesianChartResources.High} : {yValues[0].ToString(labelFormat)}\n" +
								  $"{SfCartesianChartResources.Low} : {yValues[1].ToString(labelFormat)}\n" +
								  $"{SfCartesianChartResources.Open} : {yValues[2].ToString(labelFormat)}\n" +
								  $"{SfCartesianChartResources.Close} : {yValues[3].ToString(labelFormat)}";
#if ANDROID
			Size contentSize = ChartUtils.GetLabelSize(label, pointInfo.TooltipHelper);
			pointInfo.GroupLabelSize = contentSize;
#endif
			pointInfo.Label = label;
		}

		internal override double GetActualSpacing()
		{
			return Spacing;
		}

		internal override double GetActualWidth()
		{
			return Width;
		}

		#endregion

		#endregion
	}
}