using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    ///  Represents the base class for all circular chart series, such as <see cref="PieSeries"/>, <see cref="DoughnutSeries"/>, and <see cref="RadialBarSeries"/> series.
    /// </summary>
    public abstract class CircularSeries : ChartSeries
    {
        #region Fields  

        const float _labelGap = 4;

        internal bool isClockWise;

        bool _isIncreaseAngle = false;

        #endregion

        #region Internal Properties

        internal CircularChartArea? ChartArea { get; set; }

        internal IList<double> YValues { get; set; }

        internal PointF Center { get; set; }

        internal float ActualRadius { get; set; }

        internal override ChartDataLabelSettings ChartDataLabelSettings => DataLabelSettings;

        internal List<RectF> InnerBounds { get; set; }

        internal List<PieSegment>? LeftPoints { get; set; }

        internal List<PieSegment>? RightPoints { get; set; }

        //TODO: Need to remove by calculate smartLabels without using series. 
        internal Rect AdjacentLabelRect { get; set; } = Rect.Zero;

        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the binding path for the Y-axis values in the circular series.
        /// </remarks>
        public static readonly BindableProperty YBindingPathProperty = BindableProperty.Create(
            nameof(YBindingPath),
            typeof(string),
            typeof(CircularSeries),
            null,
            BindingMode.Default,
            null,
            OnYBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the stroke brush for the circular series. This defines the color of the outline.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(CircularSeries),
            SolidColorBrush.Transparent,
            BindingMode.Default,
            null,
            OnStrokeChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the width of the stroke for the circular series.
        /// </remarks>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(double),
            typeof(CircularSeries),
            2d,
            BindingMode.Default,
            null,
            OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Radius"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the radius of the circular series, where values range from 0 to 1.
        /// </remarks>
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
            nameof(Radius),
            typeof(double),
            typeof(CircularSeries),
            0.8d,
            BindingMode.Default,
            null,
            OnRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the starting angle (in degrees) of the circular series. Default is 0 degrees.
        /// </remarks>
        public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(
            nameof(StartAngle),
            typeof(double),
            typeof(CircularSeries),
            0d,
            BindingMode.Default,
            null,
            OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the ending angle (in degrees) of the circular series. Default is 360 degrees.
        /// </remarks>
        public static readonly BindableProperty EndAngleProperty = BindableProperty.Create(
            nameof(EndAngle),
            typeof(double),
            typeof(CircularSeries),
            360d,
            BindingMode.Default,
            null,
            OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the customization for data labels in the circular series.
        /// </remarks>
        public static readonly BindableProperty DataLabelSettingsProperty = BindableProperty.Create(
            nameof(DataLabelSettings),
            typeof(CircularDataLabelSettings),
            typeof(CircularSeries),
            null,
            BindingMode.Default,
            null,
            OnDataLabelSettingsChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue" />
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is <c>Transparent</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           Stroke = "Red"
        ///                           StrokeWidth = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           Stroke = "Red"
        ///                           StrokeWidth = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to render the series size.
        /// </summary>
        /// <value>It accepts <c>double</c> values, and the default value is 0.8. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           Radius = "0.7"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Radius = 0.7,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series start rendering position.
        /// </summary>
        /// <value>It accepts <c>double</c> values, and the default value is 0.</value>
        /// <remarks>It is used to draw a series in different shapes.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           StartAngle = "180"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StartAngle = 180,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series end rendering position.
        /// </summary>
        /// <value>It accepts <c>double</c> values, and the default value is 360.</value>
        /// <remarks>It is used to draw a series in different shapes.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           EndAngle = "270"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EndAngle = 270,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the circular series.
        /// </summary>
        /// <value>This property takes the <see cref="CircularDataLabelSettings"/>.</value>
        /// <remarks>
        /// This allows us to change the look of the displaying labels' content, appearance, and connector lines at the data point.
        /// </remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                ShowDataLabels="True">
        ///                    <chart:PieSeries.DataLabelSettings>
        ///                          <chart:CircularDataLabelSettings LabelPlacement="Outer" />
        ///                    <chart:PieSeries.DataLabelSettings>
        ///               </chart:PieSeries> 
        ///           </chart:SfCircularChart.Series>
        ///           
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     chart.Series.Add(series);
        ///     
        ///     series.DataLabelSettings = new CircularDataLabelSettings(){ LabelPlacement = DataLabelPlacement.Outer };
        /// ]]></code>
        /// ***
        /// </example>
        public CircularDataLabelSettings DataLabelSettings
        {
            get { return (CircularDataLabelSettings)GetValue(DataLabelSettingsProperty); }
            set { SetValue(DataLabelSettingsProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularSeries"/> class.
        /// </summary>
        public CircularSeries()
        {
            YValues = new List<double>();
            DataLabelSettings = new CircularDataLabelSettings();
            InnerBounds = new List<RectF>();
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (DataLabelSettings != null)
            {
                SetInheritedBindingContext(DataLabelSettings, BindingContext);

                if (DataLabelSettings.ConnectorLineSettings != null)
                {
                    SetInheritedBindingContext(DataLabelSettings.ConnectorLineSettings, BindingContext);
                }
            }
        }

        /// <inheritdoc/>
        protected internal override void DrawDataLabel(ICanvas canvas, Brush? fillColor, string label, PointF point, int index)
        {
            var pieSegment = Segments[index] as PieSegment;

            if (pieSegment == null) return;

            if (DataLabelSettings.SmartLabelAlignment == SmartLabelAlignment.None || (pieSegment.IsVisible && !IsOverlapWithPrevious(pieSegment)))
            {
                if (pieSegment.LabelContent != label)
                {
                    pieSegment.LabelContent = label;
                    var labelSize = DataLabelSettings.LabelStyle.MeasureLabel(label);
                    List<float> xPoints = pieSegment.XPoints;

                    if (xPoints != null && xPoints.Count > 2)
                    {
                        float x = xPoints[2];
                        x = pieSegment.IsLeft ? x - (labelSize.Width / 2) - _labelGap : x + (labelSize.Width / 2) + _labelGap;
                        pieSegment.LabelPositionPoint = new PointF(x, point.Y);
                        point = pieSegment.LabelPositionPoint;
                    }
                }

                if (pieSegment.LabelContent != pieSegment.TrimmedText)
                {
                    EdgeDetectionForLabel(pieSegment);
                }

                pieSegment.IsVisible = !string.IsNullOrEmpty(pieSegment.TrimmedText);
                
                if (pieSegment.IsVisible && AreaBounds.Contains(pieSegment.LabelRect))
                {
                    base.DrawDataLabel(canvas, fillColor, pieSegment.TrimmedText, point, index);
                    DrawConnectorLine(canvas, pieSegment);
                }
            }
        }

        #endregion

        #region Internal Methods

        internal void UpdateDataLabelPositions(ICanvas canvas)
        {
            foreach (PieSegment segment in Segments)
            {
                if (IsSegmentWithinBounds(segment) && segment.IsVisible)
                {
                    DrawConnectorLine(canvas, segment);
                }
                else
                {
                    segment.LabelPositionPoint = new PointF(float.NaN, float.NaN);
                }

                var dataLabel = segment.DataLabels[0];
                dataLabel.XPosition = segment.LabelPositionPoint.X;
                dataLabel.YPosition = segment.LabelPositionPoint.Y;
            }
        }

        internal void UpdateCenterViewBounds(View centerView)
        {
            if (Center != PointF.Zero && centerView != null)
            {
                var xPosition = Center.X / AreaBounds.Width;
                var yPosition = Center.Y / AreaBounds.Height;
                AbsoluteLayout.SetLayoutBounds(centerView, new Rect(xPosition, yPosition, -1, -1));
                AbsoluteLayout.SetLayoutFlags(centerView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
            }
        }

        internal override void OnSeriesLayout()
        {
            ActualRadius = GetRadius();
            Center = GetCenter();
        }

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            YValues.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnBindingPathChanged();
        }

        internal override void GenerateDataPoints()
        {
            GeneratePoints(new[] { YBindingPath }, YValues);
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void LegendItemToggled(LegendItem chartLegendItem)
        {
            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        internal override void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        internal float SumOfYValues()
        {
            float sum = 0f;

            if (YValues != null)
            {
                if (float.IsNaN(sumOfYValues))
                {
                    foreach (double number in YValues)
                    {
                        if (!double.IsNaN(number))
                        {
                            sum += (float)number;
                        }
                    }

                    sumOfYValues = sum;
                }
                else
                {
                    sum = sumOfYValues;
                }
            }

            return sum;
        }

        internal override void UpdateLegendIconColor()
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem, legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        internal override void UpdateLegendIconColor(ChartSelectionBehavior sender, int index)
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null && index < legendItems.Count)
            {
                if (legendItems[index] is LegendItem legendItem)
                {
                    legendItem.IconBrush = GetFillColor(legendItem, index) ?? new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        internal float GetRadius()
        {
            if (AreaBounds != Rect.Zero)
            {
                return (float)Radius * (float)(Math.Min(AreaBounds.Width, AreaBounds.Height) / 2);
            }

            return 0.0f;
        }

        internal PointF GetCenter()
        {
            if (AreaBounds != Rect.Zero)
            {
                return GetActualCenter((float)(AreaBounds.Width * 0.5), (float)(AreaBounds.Height * 0.5), ActualRadius);
            }

            return default(PointF);
        }

        internal double GetAngleDifference()
        {
            var angleDifference = EndAngle - StartAngle;
            isClockWise = EndAngle > StartAngle;

            //Circular series not rendering with single segment, so reduced with 0.01 when the angle difference is 360.
            angleDifference = Math.Abs(angleDifference).Equals(360) ? isClockWise ? Math.Abs(angleDifference - 0.0001) : angleDifference + 0.0001 : angleDifference;

            if (Math.Abs(Math.Round(angleDifference * 100.0) / 100.0) > 360)
            {
                angleDifference = angleDifference % 360;
            }

            return angleDifference;
        }

        internal virtual double CalculateTotalYValues()
        {
            double total = 0;
            var legendItems = GetLegendItems();

            for (int i = 0; i < YValues.Count; i++)
            {
                if (!double.IsNaN(YValues[i]) && (Segments.Count == 0 || ((Segments.Count <= i) || (Segments.Count > i && Segments[i].IsVisible))))
                {
                    if (legendItems == null || legendItems.Count == 0)
                    {
                        total += Math.Abs(YValues[i]);
                    }
                    else
                    {
                        total += legendItems[i].IsToggled ? 0 : Math.Abs(YValues[i]);
                    }
                }
            }

            return total;
        }

        internal ReadOnlyObservableCollection<ILegendItem>? GetLegendItems()
        {
            return ChartArea?.PlotArea.LegendItems;
        }

        internal void ChangeIntersectedLabelPosition()
        {
            if (DataLabelSettings.SmartLabelAlignment == SmartLabelAlignment.Shift)
            {
                LeftPoints = new List<PieSegment>();
                RightPoints = new List<PieSegment>();

                foreach (PieSegment segment in Segments)
                {
                    segment.IsVisible = true;
                    if (segment.DataLabelRenderingPosition == Position.Left &&
                        DataLabelSettings.LabelPosition == ChartDataLabelPosition.Outside)
                    {
                        LeftPoints.Add(segment);
                    }
                    else if (segment.DataLabelRenderingPosition == Position.Right &&
                        DataLabelSettings.LabelPosition == ChartDataLabelPosition.Outside)
                    {
                        RightPoints.Add(segment);
                    }
                }

                LeftPoints.Sort((s1, s2) => s1.SegmentNewAngle.CompareTo(s2.SegmentNewAngle));
                RightPoints.Sort((s1, s2) => s1.LabelRect.Y.CompareTo(s2.LabelRect.Y));

                if (LeftPoints.Count > 0)
                {
                    ArrangeLeftSidePoints();
                }

                _isIncreaseAngle = false;

                if (RightPoints.Count > 0)
                {
                    ArrangeRightSidePoints();
                }

                LeftPoints.Sort((s1, s2) => s1.LabelRect.Y.CompareTo(s2.LabelRect.Y));
                RightPoints.Sort((s1, s2) => s1.LabelRect.Y.CompareTo(s2.LabelRect.Y));

                ArrangeDataLabelsWithoutOverlap(LeftPoints, false);
                ArrangeDataLabelsWithoutOverlap(RightPoints, true);
            }
            else if (DataLabelSettings.SmartLabelAlignment == SmartLabelAlignment.Hide)
            {
                for (int i = 0; i < Segments.Count - 1; i++)
                {
                    PieSegment? currentSegment = Segments[i] as PieSegment;
                    PieSegment? nextSegment = Segments[i + 1] as PieSegment;

                    if (currentSegment == null || nextSegment == null) return;

                    if ((currentSegment.IsVisible && currentSegment.LabelRect.IntersectsWith(nextSegment.LabelRect)) ||
                        (!currentSegment.IsVisible && currentSegment.Index > 0 &&
                         Segments[currentSegment.Index - 1] is PieSegment previousSegment &&
                         previousSegment.LabelRect.IntersectsWith(nextSegment.LabelRect)))
                    {
                        Segments[nextSegment.Index].IsVisible = false;
                    }

                }
            }

            foreach (var segment in Segments)
            {
                if (segment is PieSegment pieSegment)
                {
                    if (LabelTemplate != null && pieSegment.IsVisible)
                    {
                        pieSegment.IsVisible = !IsDataLabelInsideBounds(pieSegment);
                    }
                    else if (pieSegment.IsVisible)
                    {
                        EdgeDetectionForLabel(pieSegment);
                    }
                }
            }
        }

        internal virtual float GetDataLabelRadius()
        {
            return 1;
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            base.DrawDataLabels(canvas);

            if (!NeedToAnimateDataLabel)
            {
                InnerBounds?.Clear();

                AdjacentLabelRect = Rect.Zero;
            }
        }

        internal bool IsOverlap(Rect currentRect, Rect rect)
        {
            return currentRect.Left < rect.Left + rect.Width &&
                currentRect.Left + currentRect.Width > rect.Left &&
                currentRect.Top < (rect.Top + rect.Height) &&
                (currentRect.Height + currentRect.Top) > rect.Top;
        }

        internal PointF calculateOffset(double degree, double radius, PointF center)
        {
            float radian = degreesToRadians((float)degree);

            return new PointF(center.X + (float)Math.Cos(radian) * (float)radius, center.Y + (float)Math.Sin(radian) * (float)radius);
        }

        #endregion

        #region Private Methods

        bool IsSegmentWithinBounds(PieSegment segment)
        {
            return segment.LabelPositionPoint.X > 0 && (segment.LabelPositionPoint.X + (segment.LabelRect.Width / 2)) < this.AreaBounds.Right && segment.LabelPositionPoint.Y > 0;
        }

        void DrawConnectorLine(ICanvas canvas, ChartSegment dataLabel)
        {
            var connectorLineStyle = DataLabelSettings.ConnectorLineSettings;

            if (dataLabel is not PieSegment pieSegment || connectorLineStyle.StrokeWidth < 0) return;

            var strokeBrush = connectorLineStyle.Stroke as SolidColorBrush;
            canvas.StrokeColor = DataLabelSettings.UseSeriesPalette ? dataLabel.Fill?.ToColor() : strokeBrush?.ToColor();
            canvas.StrokeSize = (float)connectorLineStyle.StrokeWidth;

            if (connectorLineStyle.StrokeDashArray != null)
                canvas.StrokeDashPattern = connectorLineStyle.StrokeDashArray?.ToFloatArray();

            canvas.StrokeLineCap = LineCap.Round;

            if (DataLabelSettings != null && pieSegment.IsVisible && pieSegment.XPoints?.Count > 2)
            {
                List<float> xPoints = pieSegment.XPoints;
                List<float> yPoints = pieSegment.YPoints;

                if (DataLabelSettings.ConnectorLineSettings.ConnectorType == ConnectorType.Line)
                {
                    canvas.DrawLine(xPoints[0], yPoints[0], xPoints[1], yPoints[1]);
                    canvas.DrawLine(xPoints[1], yPoints[1], xPoints[2], yPoints[2]);
                }
                else
                {
                    PathF path = new PathF();
                    path.MoveTo(xPoints[0], yPoints[0]);
                    path.QuadTo(xPoints[1], yPoints[1], xPoints[2], yPoints[2]);
                    canvas.DrawPath(path);
                }
            }
        }

        PointF GetActualCenter(float x, float y, float radius)
        {
            PointF actualCenter = new PointF(x, y);

            double startAngle1 = GetWrapAngle(StartAngle, -630, 630);
            double endAngle1 = GetWrapAngle(EndAngle, -630, 630);
            float[] regions = new float[] { -630, -540, -450, -360, -270, -180, -90, 0, 90, 180, 270, 360, 450, 540, 630 };
            List<int> region = new List<int>();
            if (startAngle1 < endAngle1)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] > startAngle1 && regions[i] < endAngle1)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }
            else
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] < startAngle1 && regions[i] > endAngle1)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }

            double startRadian = 2 * Math.PI * (startAngle1 / 360);
            double endRadian = 2 * Math.PI * (endAngle1 / 360);
            PointF startPoint = new PointF((float)(x + (radius * Math.Cos(startRadian))), (float)(y + (radius * Math.Sin(startRadian))));
            PointF endPoint = new PointF((float)(x + (radius * Math.Cos(endRadian))), (float)(y + (radius * Math.Sin(endRadian))));

            switch (region.Count)
            {
                case 0:
                    float longX = Math.Abs(x - startPoint.X) > Math.Abs(x - endPoint.X) ? startPoint.X : endPoint.X;
                    float longY = Math.Abs(y - startPoint.Y) > Math.Abs(y - endPoint.Y) ? startPoint.Y : endPoint.Y;
                    PointF midPoint = new PointF(Math.Abs(x + longX) / 2, Math.Abs(y + longY) / 2);
                    actualCenter.X = x + (x - midPoint.X);
                    actualCenter.Y = y + (y - midPoint.Y);
                    break;
                case 1:
                    PointF point1 = new PointF(), point2 = new PointF();
                    float maxRadian = (float)(2 * Math.PI * region[0] / 360);
                    PointF maxPoint = new PointF((float)(x + (radius * Math.Cos(maxRadian))), (float)(y + (radius * Math.Sin(maxRadian))));

                    switch (region[0])
                    {
                        case 270:
                            point1 = new PointF(startPoint.X, maxPoint.Y);
                            point2 = new PointF(endPoint.X, y);
                            break;
                        case 0:
                        case 360:
                            point1 = new PointF(x, endPoint.Y);
                            point2 = new PointF(maxPoint.X, startPoint.Y);
                            break;
                        case 90:
                            point1 = new PointF(endPoint.X, y);
                            point2 = new PointF(startPoint.X, maxPoint.Y);
                            break;
                        case 180:
                            point1 = new PointF(maxPoint.X, startPoint.Y);
                            point2 = new PointF(x, endPoint.Y);
                            break;
                    }

                    midPoint = new PointF((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                    actualCenter.X = x + ((x - midPoint.X) >= radius ? 0 : (x - midPoint.X));
                    actualCenter.Y = y + ((y - midPoint.Y) >= radius ? 0 : (y - midPoint.Y));
                    break;
                case 2:
                    float minRadian = (float)(2 * Math.PI * region[0] / 360);
                    maxRadian = (float)(2 * Math.PI * region[1] / 360);
                    maxPoint = new PointF((float)(x + (radius * Math.Cos(maxRadian))), (float)(y + (radius * Math.Sin(maxRadian))));
                    PointF minPoint = new PointF((float)(x + (radius * Math.Cos(minRadian))), (float)(y + (radius * Math.Sin(minRadian))));

                    if ((region[0] == 0 && region[1] == 90) || (region[0] == 180
                        && region[1] == 270))
                    {
                        point1 = new PointF(minPoint.X, maxPoint.Y);
                    }
                    else
                    {
                        point1 = new PointF(maxPoint.X, minPoint.Y);
                    }

                    if (region[0] == 0 || region[0] == 180)
                    {
                        point2 = new PointF(GetMinMaxValue(startPoint, endPoint, region[0]), GetMinMaxValue(startPoint, endPoint, region[1]));
                    }
                    else
                    {
                        point2 = new PointF(GetMinMaxValue(startPoint, endPoint, region[1]), GetMinMaxValue(startPoint, endPoint, region[0]));
                    }

                    midPoint = new PointF(
                        Math.Abs(point1.X - point2.X) / 2 >= radius ? 0 : (point1.X + point2.X) / 2,
                        Math.Abs(point1.Y - point2.Y) / 2 >= radius ? 0 : (point1.Y + point2.Y) / 2);
                    actualCenter.X = x + (midPoint.X == 0 ? 0 : (x - midPoint.X) >= radius ? 0 : (x - midPoint.X));
                    actualCenter.Y = y + (midPoint.Y == 0 ? 0 : (y - midPoint.Y) >= radius ? 0 : (y - midPoint.Y));
                    break;
            }

            return actualCenter;
        }

        void AngleValueChanged()
        {
            if (AreaBounds != Rect.Zero)
            {
                SegmentsCreated = false;
                ScheduleUpdateChart();
            }
        }

        void ArrangeDataLabelsWithoutOverlap(List<PieSegment> points, bool isRightSide)
        {
            var arrangedRect = new List<PieSegment>();

            foreach (var current in points)
            {
                var collapsed = false;

                if (current.IsVisible && current.XPoints.Count > 0)
                {
                    if (isRightSide && current.XPoints[2] < current.XPoints[0])
                    {
                        current.IsVisible = false;
                    }
                    else if (!isRightSide && current.XPoints[2] > current.XPoints[0])
                    {
                        current.IsVisible = false;
                    }
                }

                var currentLabelRect = current.LabelRect;
                foreach (var point in arrangedRect)
                {
                    var pointLabelRect = point.LabelRect;
                    var pointRect = new Rect(pointLabelRect.X - pointLabelRect.Width / 2,
                                pointLabelRect.Y - pointLabelRect.Height / 2,
                                pointLabelRect.Width, pointLabelRect.Height);

                    if (currentLabelRect.IntersectsWith(pointLabelRect) && point.IsVisible)
                    {
                        collapsed = true;
                        current.IsVisible = false;
                        break;
                    }
                }

                if (!collapsed)
                {
                    arrangedRect.Add(current);
                }
            }
        }

        bool IsDataLabelInsideBounds(PieSegment segment)
        {
            return segment.LabelRect.X - segment.LabelRect.Width / 2 < AreaBounds.X
                 || segment.LabelRect.Y - segment.LabelRect.Height / 2 < AreaBounds.Y
                 || segment.LabelRect.Bottom + segment.LabelRect.Height / 2 > AreaBounds.Bottom;
        }

        void ArrangeRightSidePoints()
        {
            bool startFresh = false;
            bool angleChanged = false;
            double checkAngle = 0;
            PieSegment currentPoint;
            PieSegment? lastPoint = RightPoints?.Count > 1 ? RightPoints[RightPoints.Count - 1] : null;
            PieSegment nextPoint;
            if (lastPoint != null)
            {
                if (lastPoint.SegmentNewAngle! > 360)
                {
                    lastPoint.SegmentNewAngle = lastPoint.SegmentNewAngle! - 360;
                }

                if (lastPoint.SegmentNewAngle! > 90 && lastPoint.SegmentNewAngle! < 270)
                {
                    _isIncreaseAngle = true;
                    ChangeLabelAngle(lastPoint, 89);
                }
            }

            if (RightPoints == null)
            {
                return;
            }

            for (int i = RightPoints.Count - 2; i >= 0; i--)
            {
                currentPoint = RightPoints[i];
                nextPoint = RightPoints[i + 1];
                if (IsOverlapWithNext(currentPoint, RightPoints, i) && currentPoint.IsVisible ||
                    !(currentPoint.SegmentNewAngle! <= 90 || currentPoint.SegmentNewAngle! >= 270))
                {
                    if (lastPoint != null)
                    {
                        checkAngle = lastPoint.SegmentNewAngle! + 1;
                    }

                    angleChanged = true;
                    // If last's point change angle in beyond the limit,
                    //stop the increasing angle and do decrease the angle.
                    if (startFresh)
                    {
                        _isIncreaseAngle = false;
                    }
                    else if (checkAngle > 90 && checkAngle < 270 && (nextPoint.isLabelUpdated) == 1)
                    {
                        _isIncreaseAngle = true;
                    }

                    if (!_isIncreaseAngle)
                    {
                        for (int k = i + 1; k < RightPoints.Count; k++)
                        {
                            IncreaseAngle(RightPoints[k - 1], RightPoints[k], true);
                        }
                    }
                    else
                    {
                        for (int k = i + 1; k > 0; k--)
                        {
                            DecreaseAngle(RightPoints[k], RightPoints[k - 1], true);
                        }
                    }
                }
                else
                {
                    //If a point did not overlapped with previous points,
                    //increase the angle always for right side points.
                    if (angleChanged &&
                        // ignore: unnecessary_null_comparison
                        nextPoint != null &&
                        (nextPoint.isLabelUpdated) == 1)
                    {
                        startFresh = true;
                    }
                }
            }
        }

        bool IsOverlapWithNext(PieSegment point, List<PieSegment> points, int pointIndex)
        {
            for (int i = pointIndex; i < points.Count; i++)
            {
                if (i != points.IndexOf(point) && points[i].IsVisible &&
                    (!points[i].LabelRect.IsEmpty && !point.LabelRect.IsEmpty) &&
                    IsOverlap(point.LabelRect, points[i].LabelRect))
                {
                    return true;
                }
            }

            return false;
        }

        void IncreaseAngle(PieSegment currentPoint, PieSegment nextPoint, bool isRightSide)
        {
            int count = 1;
            if (isRightSide)
            {
                if (RightPoints == null)
                {
                    return;
                }

                while (IsOverlap(currentPoint.LabelRect, nextPoint.LabelRect))
                {
                    int newAngle = (int)nextPoint.SegmentNewAngle! + count;

                    if (newAngle < 270 && newAngle > 90)
                    {
                        newAngle = 90;
                        _isIncreaseAngle = true;
                        break;
                    }

                    ChangeLabelAngle(nextPoint, newAngle);

                    if (IsOverlap(currentPoint.LabelRect, nextPoint.LabelRect) &&
                        (newAngle + 1 > 90 && newAngle + 1 < 270) &&
                        RightPoints.IndexOf(nextPoint) == RightPoints.Count - 1)
                    {
                        ChangeLabelAngle(
                            currentPoint, currentPoint.SegmentNewAngle! - 1);
                        ArrangeRightSidePoints();
                        break;
                    }

                    count++;
                }
            }
            else
            {
                while (IsOverlap(currentPoint.LabelRect, nextPoint.LabelRect) ||
                    ((currentPoint.LabelRect.Top < (nextPoint.LabelRect.Top + nextPoint.LabelRect.Height))))
                {
                    int newAngle = (int)nextPoint.SegmentNewAngle! + count;
                    if (!(newAngle < 270 && newAngle > 90))
                    {
                        newAngle = 270;
                        _isIncreaseAngle = false;
                        break;
                    }

                    ChangeLabelAngle(nextPoint, newAngle);
                    count++;
                }
            }
        }

        void ArrangeLeftSidePoints()
        {
            PieSegment previousPoint;
            PieSegment currentPoint;
            bool angleChanged = false;
            bool startFresh = false;
            for (int i = 1; i < LeftPoints?.Count; i++)
            {
                currentPoint = LeftPoints[i];
                previousPoint = LeftPoints[i - 1];
                if (IsOverlapWithPrevious(currentPoint) && currentPoint.IsVisible || !(currentPoint.SegmentNewAngle! < 270))
                {
                    angleChanged = true;
                    if (startFresh)
                    {
                        _isIncreaseAngle = false;
                    }

                    if (!_isIncreaseAngle)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            DecreaseAngle(LeftPoints[k], LeftPoints[k - 1], false);
                            for (int index = 1; index < LeftPoints.Count; index++)
                            {
                                if (!double.IsNaN(LeftPoints[index].isLabelUpdated) &&
                                    LeftPoints[index].SegmentNewAngle! - 10 < 100)
                                {
                                    _isIncreaseAngle = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = i; k < LeftPoints.Count; k++)
                        {
                            IncreaseAngle(
                                LeftPoints[k - 1], LeftPoints[k], false);
                        }
                    }
                }
                else
                {
                    if (angleChanged && previousPoint != null && (previousPoint.isLabelUpdated) == 1)
                    {
                        startFresh = true;
                    }
                }
            }
        }

        void DecreaseAngle(PieSegment currentPoint, PieSegment previousPoint, bool isRightSide)
        {
            int count = 1;
            if (isRightSide)
            {
                while (IsOverlap(currentPoint.LabelRect, previousPoint.LabelRect))
                {
                    int newAngle = (int)previousPoint.SegmentNewAngle! - count;

                    if (newAngle < 0)
                    {
                        newAngle = 360 + newAngle;
                    }

                    if (newAngle <= 270 && newAngle >= 90)
                    {
                        newAngle = 270;
                        _isIncreaseAngle = true;
                        break;
                    }

                    ChangeLabelAngle(previousPoint, newAngle);
                    count++;
                }
            }
            else
            {
                if (currentPoint.SegmentNewAngle! > 270)
                {
                    ChangeLabelAngle(currentPoint, 270);
                    previousPoint.SegmentNewAngle = 270;
                    //PointHelper.setNewAngle(previousPoint, 270);
                }

                while (IsOverlap(currentPoint.LabelRect, previousPoint.LabelRect) ||
                    (((currentPoint.LabelRect.Top + currentPoint.LabelRect.Height) >
                            previousPoint.LabelRect.Bottom)))
                {
                    int newAngle = (int)previousPoint.SegmentNewAngle! - count;
                    if (!(newAngle <= 270 && newAngle >= 90))
                    {
                        newAngle = 270;
                        _isIncreaseAngle = true;
                        break;
                    }

                    ChangeLabelAngle(previousPoint, newAngle);

                    if (IsOverlap(currentPoint.LabelRect, previousPoint.LabelRect) && LeftPoints != null &&
                        LeftPoints.Contains(previousPoint) &&
                        (newAngle - 1 < 90 && newAngle - 1 > 270))
                    {
                        ChangeLabelAngle(currentPoint, currentPoint.SegmentNewAngle! + 1);
                        ArrangeLeftSidePoints();
                        break;
                    }

                    count++;
                }
            }
        }

        /// Change the label angle based on the given new angle.
        void ChangeLabelAngle(PieSegment currentPoint, double newAngle)
        {
            var pieSeries = this as PieSeries;
            if (pieSeries == null) return;

            var radius = GetRadius();
            radius = currentPoint.Index == (float)pieSeries.ExplodeIndex ? radius + (float)pieSeries.ExplodeRadius : radius;
            var center = GetCenter();
            double dataLabelMidRadius = radius + (radius * currentPoint.connectorLength);
            PointF startPoint = calculateOffset(newAngle, radius, center!);
            PointF endPoint = calculateOffset(newAngle, dataLabelMidRadius, center!);

            currentPoint.XPoints[1] = endPoint.X;
            currentPoint.YPoints[1] = endPoint.Y;

            Rect rect = new Rect(startPoint.X, startPoint.Y, currentPoint.LabelRect.Width, currentPoint.LabelRect.Height);

            if (currentPoint.DataLabelRenderingPosition == Position.Right)
            {
                currentPoint.XPoints[2] = endPoint.X + currentPoint.connectorBendLineLength;
                currentPoint.YPoints[2] = endPoint.Y;
                rect = new Rect(currentPoint.XPoints[2] + _labelGap + currentPoint.LabelRect.Width / 2, currentPoint.YPoints[2], currentPoint.LabelRect.Width, currentPoint.LabelRect.Height);
            }
            else
            {
                currentPoint.XPoints[2] = endPoint.X - currentPoint.connectorBendLineLength;
                currentPoint.YPoints[2] = endPoint.Y;
                rect = new Rect(currentPoint.XPoints[2] - currentPoint.LabelRect.Width / 2 - _labelGap, currentPoint.YPoints[2], currentPoint.LabelRect.Width, currentPoint.LabelRect.Height);
            }

            currentPoint.LabelRect = rect;
            currentPoint.LabelPositionPoint = new PointF((float)rect.X, (float)rect.Y);

            endPoint = calculateOffset(currentPoint.SegmentMidAngle, dataLabelMidRadius, center!);

            currentPoint.XPoints[1] = endPoint.X;
            currentPoint.YPoints[1] = endPoint.Y;

            currentPoint.isLabelUpdated = 1;
            currentPoint.SegmentNewAngle = newAngle;
        }

        float degreesToRadians(float deg) => deg * (float)(Math.PI / 180);

        /// To find the current point overlapped with previous points
        bool IsOverlapWithPrevious(PieSegment currentPoint)
        {
            if (currentPoint.SeriesView == null) return false;

            var segments = Segments;
            int currentPointIndex = currentPoint.Index;

            for (int i = 0; i < currentPointIndex; i++)
            {
                var segment = segments[i] as PieSegment;

                if (i != segments.IndexOf(currentPoint) && segments[i].IsVisible && segment != null &&
                    IsOverlap(currentPoint.LabelRect, segment.LabelRect))
                {
                    segments[currentPointIndex].IsVisible = false;
                    return true;
                }
            }

            return false;
        }

        void EdgeDetectionForLabel(PieSegment? pieSegment)
        {
            if (pieSegment == null || pieSegment.LabelContent == null) return;

            PointF point = pieSegment.LabelPositionPoint;
            string label = pieSegment.LabelContent;
            Rect clipRect = AreaBounds;
            var x = point.X;
            double left = x - pieSegment.LabelRect.Width / 2;
            double right = x + pieSegment.LabelRect.Width / 2;
            double labelWidth = pieSegment.LabelRect.Width;

            if (left < clipRect.Left)
            {
                label = GetTrimmedText(label, ref x, right - clipRect.Left, DataLabelSettings.LabelStyle, pieSegment.IsLeft, ref labelWidth);
                var labelRect = new Rect(x - labelWidth / 2, point.Y - pieSegment.LabelRect.Height / 2, labelWidth, pieSegment.LabelRect.Height);
                
                if (pieSegment.XPoints.Count > 2)
                pieSegment.XPoints[2] = (float)labelRect.Right + _labelGap;
            }

            if (right > clipRect.Right)
            {
                label = GetTrimmedText(label, ref x, clipRect.Right - left, DataLabelSettings.LabelStyle, pieSegment.IsLeft, ref labelWidth);
                var labelRect = new Rect(x - labelWidth / 2, point.Y - pieSegment.LabelRect.Height / 2, labelWidth, pieSegment.LabelRect.Height);
                
                if (pieSegment.XPoints.Count > 2)
                    pieSegment.XPoints[2] = (float)labelRect.Left - _labelGap;
            }

            point.X = x;
            pieSegment.LabelPositionPoint = point;
            pieSegment.TrimmedText = label;
        }

        /// To trim the text by given width.
        string GetTrimmedText(string text, ref float x, double labelsExtent, ChartLabelStyle labelStyle, bool isLeft, ref double labelWidth)
        {
            string label = text;
            double oldSize = labelStyle.MeasureLabel(label).Width;

            if (oldSize > labelsExtent)
            {
                int textLength = text.Length;

                for (int i = textLength - 1; i >= 0; --i)
                {
                    label = text.Substring(0, i) + "...";
                    double newSize = labelStyle.MeasureLabel(label).Width;
                    labelWidth = newSize;

                    if (newSize <= labelsExtent)
                    {
                        x = (float)(isLeft ? (x + (oldSize / 2) - (newSize / 2)) : (x - (oldSize / 2) + (newSize / 2)));
                        return label == "..." ? "" : label;
                    }
                }
            }

            return label == "..." ? "" : label;
        }

        void ConnectorLineStyleUpdate(CircularDataLabelSettings? oldValue, CircularDataLabelSettings? newValue)
        {
            if (oldValue != null && oldValue.ConnectorLineSettings is ConnectorLineStyle oldStyle)
            {
                oldStyle.Parent = null;
                oldStyle.PropertyChanged -= ConnectorLineSettings_PropertyChanged;
                SetInheritedBindingContext(oldStyle, null);
            }

            if (newValue != null && newValue.ConnectorLineSettings is ConnectorLineStyle newStyle)
            {
                newStyle.Parent = Parent;
                newStyle.PropertyChanged += ConnectorLineSettings_PropertyChanged;
                SetInheritedBindingContext(newStyle, Parent);
            }
        }

        void ConnectorLineSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateDataLabel();
        }

        static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries circularSeries && circularSeries.AreaBounds != Rect.Zero)
            {
                circularSeries.UpdateStrokeColor();
                circularSeries.InvalidateSeries();
            }
        }

        static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries circularSeries && circularSeries.AreaBounds != Rect.Zero)
            {
                circularSeries.UpdateStrokeWidth();
                circularSeries.InvalidateSeries();
            }
        }

        static void OnRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries circularSeries)
            {
                circularSeries.ScheduleUpdateChart();
            }
        }

        static void OnAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries circularSeries)
            {
                circularSeries.AngleValueChanged();
            }
        }

        static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CircularSeries circularSeries)
            {
                circularSeries.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
                circularSeries.ConnectorLineStyleUpdate(oldValue as CircularDataLabelSettings, newValue as CircularDataLabelSettings);
            }
        }

        static double GetWrapAngle(double angle, int min, int max)
        {
            if (max - min == 0)
            {
                return min;
            }

            angle = ((angle - min) % (max - min)) + min;
            while (angle < min)
            {
                angle += max - min;
            }

            return angle;
        }

        static float GetMinMaxValue(PointF point1, PointF point2, int degree)
        {
            float minX = Math.Min(point1.X, point2.Y);
            float minY = Math.Min(point1.Y, point2.Y);
            float maxX = Math.Max(point1.X, point2.X);
            float maxY = Math.Max(point1.Y, point2.Y);

            switch (degree)
            {
                case 270:
                    return maxY;
                case 0:
                case 360:
                    return minX;
                case 90:
                    return minY;
                case 180:
                    return maxX;
            }

            return 0f;
        }

        #endregion

        #endregion
    }
}