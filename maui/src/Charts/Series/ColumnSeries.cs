using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="ColumnSeries"/> displays a set of vertical bars for the given data point values.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="ColumnSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ColumnSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="ColumnSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para><b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
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
	///               <chart:ColumnSeries
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
	///     ColumnSeries series = new ColumnSeries();
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
	public class ColumnSeries : XYDataSeries, IDrawCustomLegendIcon, ISBSDependent
    {
        #region Internal Properties

        internal override bool IsSideBySide => true;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the stroke brush for the column series, defining the outline color of the columns.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(ColumnSeries),
            SolidColorBrush.Transparent,
            BindingMode.Default,
            null,
            OnStrokeColorChanged);

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the spacing between the columns in the column series.
        /// </remarks>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(ColumnSeries),
            0d,
            BindingMode.Default,
            null,
            OnColumnSizeChanged);

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the width of the columns in the column series. Default value is 0.8.
        /// </remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(ColumnSeries),
            0.8d,
            BindingMode.Default,
            null,
            OnColumnSizeChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the corner radius of the columns in the column series.
        /// </remarks>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ColumnSeries),
            null,
            BindingMode.Default,
            null,
            OnCornerRadiusChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the column series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is <c>Transparent</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Stroke = "Red" />
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
        ///     ColumnSeries series = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
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
        /// Gets or sets a value to indicate spacing between the segments across the series.
        /// </summary>
        /// <value> It accepts <c>double</c> values ranging from 0 to 1, where the default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Spacing = "0.3"/>
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
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Spacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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
        /// Gets or sets a value to change the width of the column segment.
        /// </summary>
        /// <value> It accepts <c>double</c> values ranging from 0 to 1, where the default value is <c>0.8</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Width = "0.7"/>
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
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Width = 0.7,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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
        /// Gets or sets the corner radius that smooths the edges of columns in a column series.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.CornerRadius"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              CornerRadius = "5"/>
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
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           CornerRadius = new CornerRadius(5)
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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

            RectF innerRect = new(1, 4, 2, 8);
            RectF innerRect1 = new(5, 2, 2, 10);
            RectF innerRect2 = new(9, 7, 2, 5);

            canvas.SetFillPaint(fillColor, innerRect);
            canvas.FillRectangle(innerRect);
            canvas.SetFillPaint(fillColor, innerRect1);
            canvas.FillRectangle(innerRect1);
            canvas.SetFillPaint(fillColor, innerRect2);
            canvas.FillRectangle(innerRect2);

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
            double bottomValue = 0;
            if (ActualXAxis != null)
            {
                bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }

            if (IsGrouped && (IsIndexed || xValues == null))
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    if (i < Segments.Count && Segments[i] is ColumnSegment)
                    {
                        ((ColumnSegment)Segments[i]).SetData(new[] { i + SbsInfo.Start, i + SbsInfo.End, YValues[i], bottomValue, i, YValues[i] });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { i + SbsInfo.Start, i + SbsInfo.End, YValues[i], bottomValue, i, YValues[i] }, i);
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

                        if (i < Segments.Count && Segments[i] is ColumnSegment)
                        {
                            ((ColumnSegment)Segments[i]).SetData(new[] { x + SbsInfo.Start, x + SbsInfo.End, YValues[i], bottomValue, x, YValues[i] });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x + SbsInfo.Start, x + SbsInfo.End, YValues[i], bottomValue, x, YValues[i] }, i);
                        }
                    }
                }
            }
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

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            if (DataLabelSettings == null) return 0;

            var yValue = YValues?[index] ?? 0f;

            switch (DataLabelSettings.BarAlignment)
            {
                case DataLabelAlignment.Bottom:
                    yValue = 0;
                    break;
                case DataLabelAlignment.Middle:
                    yValue /= 2;
                    break;
            }

            return yValue;
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null) return;

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
            if (ChartArea == null) return labelPosition;

            if (ChartArea.IsTransposed)
            {
                return DataLabelSettings.GetLabelPositionForTransposedRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
            }

            return DataLabelSettings.GetLabelPositionForRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            if (ChartArea == null) return;

            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            ColumnSegment? columnSegment = Segments[tooltipInfo.Index] as ColumnSegment;

            if (columnSegment != null)
            {
                if (ChartArea.IsTransposed)
                {
                    float width = columnSegment.SegmentBounds.Width;
                    float height = columnSegment.SegmentBounds.Height;
                    Rect targetRect = new Rect(xPosition - width, yPosition - height / 2, width, height);
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

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> PointInfos, ref bool isSideBySide)
        {
            var xValues = GetXValues();
            float xPosition = 0f;
            float yPosition = 0f;
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    double yValue = yValues[index];
                    string label = yValue.ToString();

                    if (IsSideBySide)
                    {
                        isSideBySide = true;
                        double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                        xPosition = TransformToVisibleX(xMidVal, yValue);
                        yPosition = TransformToVisibleY(xMidVal, yValue);
                    }

                    // Checking YValue is contain in plotArea
                    //Todo: need to check with transposed
                    //if (!AreaBounds.Contains(xPoint + AreaBounds.Left, yPoint + AreaBounds.Top)) continue;

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

                    if (chartPointInfo != null)
                    {
                        chartPointInfo.XValue = xValue;
                        chartPointInfo.YValues.Add(yValue);
                        PointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
        {
            var label = pointInfo.YValues[0].ToString(labelFormat);
            pointInfo.Label = label;
        }

        #endregion

        #region Private Methods

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColumnSeries columnSeries && columnSeries.Chart != null)
            {
                columnSeries.InvalidateSeries();
            }
        }

        static void OnColumnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColumnSeries columnSeries && columnSeries.ChartArea != null)
            {
                columnSeries.UpdateSbsSeries();
            }
        }

        static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColumnSeries columnSeries && columnSeries.Chart != null)
            {
                columnSeries.UpdateStrokeColor();
                columnSeries.InvalidateSeries();
            }
        }

        void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            ColumnSegment? segment = CreateSegment() as ColumnSegment;

            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Item = ActualData?[index];
                segment.Index = index;
                InitiateDataLabels(segment);
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    ColumnSegment? oldSegment = OldSegments[index] as ColumnSegment;

                    if (oldSegment != null)
                        segment.SetPreviousData(new[] { oldSegment.Y1, oldSegment.Y2 });
                }
            }
        }

        #endregion

        #endregion
    }
}