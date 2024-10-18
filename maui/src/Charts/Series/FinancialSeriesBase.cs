using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents an abstract base class for financial chart series such as Candle and OHLC (Open-High-Low-Close).
	/// </summary>
	public abstract class FinancialSeriesBase : CartesianSeries
    {
        #region Internal Properties

        internal float SumOfHighValues = float.NaN;
        internal float SumOfLowValues = float.NaN;
        internal float SumOfOpenValues = float.NaN;
        internal float SumOfCloseValues = float.NaN;

        internal IList<double> HighValues { get; set; }

        internal IList<double> LowValues { get; set; }

        internal IList<double> OpenValues { get; set; }

        internal IList<double> CloseValues { get; set; }

        internal override bool IsMultipleYPathRequired => true;

        #endregion

        #region Bindable Property

        /// <summary>
        /// Identifies the <see cref="BearishFill"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="BearishFill"/> property determines the fill color of the bearish points of the financial series.
        /// </remarks>
        public static readonly BindableProperty BearishFillProperty = BindableProperty.Create(
            nameof(BearishFill),
            typeof(Brush),
            typeof(FinancialSeriesBase),
            new SolidColorBrush(Color.FromArgb("#C15146")),
            BindingMode.Default,
            null,
            OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BullishFill"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="BullishFill"/> property determines the fill color to the bullish points of the financial series.
        /// </remarks>
        public static readonly BindableProperty BullishFillProperty = BindableProperty.Create(
            nameof(BullishFill),
            typeof(Brush),
            typeof(FinancialSeriesBase),
            new SolidColorBrush(Color.FromArgb("#90A74F")),
            BindingMode.Default,
            null,
            OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="High"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="High"/> property represents the maximum value of a data point in a financial series.
        /// </remarks>
        public static readonly BindableProperty HighProperty = BindableProperty.Create(
            nameof(High),
            typeof(string),
            typeof(FinancialSeriesBase),
            string.Empty,
            BindingMode.Default,
            null,
            OnOHCLPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Low"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Low"/> property represents the minimum value of a data point in a financial series.
        /// </remarks>
        public static readonly BindableProperty LowProperty = BindableProperty.Create(
            nameof(Low),
            typeof(string),
            typeof(FinancialSeriesBase),
            string.Empty,
            BindingMode.Default,
            null,
            OnOHCLPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Open"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Open"/> property represents the open value of a data point in a financial series.
        /// </remarks>
        public static readonly BindableProperty OpenProperty = BindableProperty.Create(
            nameof(Open),
            typeof(string),
            typeof(FinancialSeriesBase),
            string.Empty,
            BindingMode.Default,
            null,
            OnOHCLPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Close"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Close"/> property represents the close value of a data point in a financial series.
        /// </remarks>
        public static readonly BindableProperty CloseProperty = BindableProperty.Create(
            nameof(Close),
            typeof(string),
            typeof(FinancialSeriesBase),
            string.Empty,
            BindingMode.Default,
            null,
            OnOHCLPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Spacing"/> property indicate spacing between the data points across the series.
        /// </remarks>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(FinancialSeriesBase),
            0d,
            BindingMode.Default,
            null,
            OnSpacingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Width"/> property indicates the width of the data points across the series.
        /// </remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(FinancialSeriesBase),
            0.8d,
            BindingMode.Default,
            null,
            OnWidthPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the brush to be used for bearish points in a financial chart. It is typically used in conjunction with a <see cref="CandleSeries"/> or <see cref="HiLoOpenCloseSeries"/> series to visually represent negative price movements or bearish market conditions.
        /// </summary>
        /// <value>It accepts the <see cref="Brush"/> values</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              BearishFill="Orange"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///           BearishFill = Colors.Orange,
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush BearishFill
        {
            get { return (Brush)GetValue(BearishFillProperty); }
            set { SetValue(BearishFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush to be used for bullish points in a financial chart. It is typically used in conjunction with a <see cref="CandleSeries"/> or <see cref="HiLoOpenCloseSeries"/> series to visually represent positive price movements or bullish market conditions.
        /// </summary>
        /// <value>It accepts the <see cref="Brush"/> values</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              BullishFill="Blue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///           BullishFill = Colors.Blue,
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush BullishFill
        {
            get { return (Brush)GetValue(BullishFillProperty); }
            set { SetValue(BullishFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a high value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y (high) plotting data, and its default value is <c>string.Empty</c>>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
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
        /// <value>
        /// The <c>string</c> that represents the property name for the y (low) plotting data, and its default value is <c>string.Empty</c>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string Low
        {
            get { return (string)GetValue(LowProperty); }
            set { SetValue(LowProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a open value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y (open) plotting data, and its default value is <c>string.Empty</c>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string Open
        {
            get { return (string)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a close value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y (close) plotting data, and its default value is <c>string.Empty</c>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string Close
        {
            get { return (string)GetValue(CloseProperty); }
            set { SetValue(CloseProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate spacing between the data points across the series.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values ranging from 0 to 1, where the default value is 0.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              Spacing="0.3"/>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "High"
        ///           Low = "Low"
        ///           Open = "Open"
        ///           Close = "Close"
        ///           Spacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
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
        /// Gets or sets a value to change the width of the data points across the series.
        /// </summary>
        /// <value>
        /// It accepts <c>double</c> values ranging from 0 to 1, where the default value is <c>0.8</c>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              Width="0.5"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries candleSeries = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High="High",
        ///           Low="Low"
        ///           Open="Open"
        ///           Close="Close",
        ///           Width = 0.5,
        ///     };
        ///     
        ///     chart.Series.Add(candleSeries);
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialSeriesBase"/> class.
        /// </summary>
        public FinancialSeriesBase()
        {
            HighValues = new List<double>();
            LowValues = new List<double>();
            OpenValues = new List<double>();
            CloseValues = new List<double>();
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            HighValues.Clear();
            LowValues.Clear();
            OpenValues.Clear();
            CloseValues.Clear();
            GeneratePoints(new string[] { High, Low, Open, Close }, HighValues, LowValues, OpenValues, CloseValues);
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void GenerateDataPoints()
        {
            GeneratePoints(new string[] { High, Low, Open, Close }, HighValues, LowValues, OpenValues, CloseValues);
        }

        internal override void OnBindingPathChanged()
        {
            ResetData();
            HighValues.Clear();
            LowValues.Clear();
            OpenValues.Clear();
            CloseValues.Clear();

            GeneratePoints(new string[] { High, Low, Open, Close }, HighValues, LowValues, OpenValues, CloseValues);
            base.OnBindingPathChanged();
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

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;
            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (HiLoOpenCloseSegment dataLabel in Segments)
            {
                if (!dataLabel.InVisibleRange || dataLabel.IsEmpty) continue;

                CandleSeriesDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
            }
        }

        internal override void InitiateDataLabels(ChartSegment segment)
        {
            for (int i = 0; i < 4; i++)
            {
                var dataLabel = new ChartDataLabel();
                segment.DataLabels.Add(dataLabel);
                DataLabels.Add(dataLabel);
            }
        }

        internal SizeF GetLabelTemplateSize(ChartSegment segment, string valueType)
        {
            int labelIndex = (valueType == "LowType") ? 1 : ((valueType == "OpenType") ? 2 : ((valueType == "CloseType") ? 3 : 0));

            if (LabelTemplateView != null && LabelTemplateView.Any())
            {
                var templateView = LabelTemplateView.Cast<View>().FirstOrDefault(child => segment.DataLabels[labelIndex] == child.BindingContext) as DataLabelItemView;

                if (templateView != null && templateView.ContentView is View content)
                {
                    if (!content.DesiredSize.IsZero)
                    {
                        return content.DesiredSize;
                    }

                    var desiredSize = (Size)templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

                    if (desiredSize.IsZero)
                        return (Size)content.Measure(double.PositiveInfinity, double.PositiveInfinity);

                    return desiredSize;
                }
            }

            return SizeF.Zero;
        }

        internal override List<object>? GetDataPoints(double startX, double endX, double startY, double endY, int minimum, int maximum, List<double> xValues, bool validateYValues)
        {
            List<object> dataPoints = new List<object>();
            int count = xValues.Count;
            if (count == HighValues.Count && count == LowValues.Count && count == OpenValues.Count && count == CloseValues.Count && ActualData != null)
            {
                for (int i = minimum; i <= maximum; i++)
                {
                    double xValue = xValues[i];
                    if (validateYValues || (startX <= xValue && xValue <= endX))
                    {
                        if ((startY <= HighValues[i] && HighValues[i] <= endY) || (startY <= LowValues[i] && LowValues[i] <= endY) || (startY <= OpenValues[i] && OpenValues[i] <= endY) || (startY <= CloseValues[i] && CloseValues[i] <= endY))
                        {
                            dataPoints.Add(ActualData[i]);
                        }
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
                    SumOfHighValues = float.IsNaN(SumOfHighValues) ? yValue : SumOfHighValues + yValue;
                    break;
                case "Low":
                    SumOfLowValues = float.IsNaN(SumOfLowValues) ? yValue : SumOfLowValues + yValue;
                    break;
                case "Open":
                    SumOfOpenValues = float.IsNaN(SumOfOpenValues) ? yValue : SumOfOpenValues + yValue;
                    break;
                case "Close":
                    SumOfCloseValues = float.IsNaN(SumOfCloseValues) ? yValue : SumOfCloseValues + yValue;
                    break;
            }
        }

        internal void SumOfValuesDynmaicRemove(string yPath, float yValue)
        {
            switch (yPath)
            {
                case "High":
                    SumOfHighValues -= yValue;
                    break;
                case "Low":
                    SumOfLowValues -= yValue;
                    break;
                case "Open":
                    SumOfOpenValues -= yValue;
                    break;
                case "Close":
                    SumOfCloseValues -= yValue;
                    break;
            }
        }

        internal void ResetSumOfValues()
        {
            SumOfHighValues = float.NaN;
            SumOfLowValues = float.NaN;
            SumOfOpenValues = float.NaN;
            SumOfCloseValues = float.NaN;
        }

        #endregion

        #region Private Method

        static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FinancialSeriesBase series)
            {
                series.UpdateColor();
                series.InvalidateSeries();
            }
        }

        static void OnOHCLPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FinancialSeriesBase series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FinancialSeriesBase series)
            {
                series.UpdateSbsSeries();
            }
        }

        static void OnSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FinancialSeriesBase series && series.ChartArea != null)
            {
                series.InvalidateSeries();
            }
        }

        void CandleSeriesDataLabelAppearance(ICanvas canvas, HiLoOpenCloseSegment dataLabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
        {
            for (int i = 0; i < 4; i++)
            {
                string labelText;
                PointF position;
                labelText = dataLabel.DataLabel[i] ?? string.Empty;
                position = dataLabel.LabelPositions[i];

                if (labelStyle.Angle != 0)
                {
                    float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
                    canvas.CanvasSaveState();
                    canvas.Rotate(angle, position.X, position.Y);
                }

                canvas.StrokeSize = (float)labelStyle.StrokeWidth;
                canvas.StrokeColor = labelStyle.Stroke.ToColor();

                var fillColor = labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? dataLabel.Fill : labelStyle.Background;
                DrawDataLabel(canvas, fillColor, labelText, position, dataLabel.Index);
            }
        }

        #endregion

        #endregion
    }
}