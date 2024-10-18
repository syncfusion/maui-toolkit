using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The CircularDataLabelSettings class is used to customize the appearance of circular series data labels.
    /// </summary>
    /// <remarks>
    /// <para>To customize data labels, create an instance of the <see cref="CircularDataLabelSettings"/> class, and set it to the DataLabelSettings property of the circular series.</para>
    /// 
    /// <para><b>ShowDataLabels</b></para>
    /// <para>Data labels can be added to a chart series by enabling the <see cref="ChartSeries.ShowDataLabels"/> option.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///    <chart:SfCircularChart>
    ///
    ///       <chart:PieSeries ItemsSource ="{Binding Data}"
    ///                        XBindingPath="XValue"
    ///                        YBindingPath="YValue"
    ///                        ShowDataLabels="True"/>
    ///
    ///    </chart:SfCircularChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PieSeries series = new PieSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true
    ///     };
    ///
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// ***
    /// 
    /// <para><b>Customization</b></para>
    /// <para>To change the appearance of data labels, it offers <see cref="ChartDataLabelSettings.LabelStyle"/> options.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///    <chart:SfCircularChart>
    ///
    ///       <chart:PieSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"
    ///                        ShowDataLabels="True">
    ///          <chart:PieSeries.DataLabelSettings>
    ///              <chart:CircularDataLabelSettings>
    ///                    <chart:CircularDataLabelSettings.LabelStyle>
    ///                        <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
    ///                    </chart:CircularDataLabelSettings.LabelStyle>
    ///                </chart:CircularDataLabelSettings>
    ///          </chart:PieSeries.DataLabelSettings>
    ///       </chart:PieSeries>
    ///    </chart:SfCircularChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PieSeries series = new PieSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true,
    ///     };
    ///     
    ///     var labelStyle = new ChartDataLabelStyle()
    ///     {
    ///         Background = new SolidColorBrush(Colors.Red),
    ///         TextColor = Colors.Black,
    ///         FontSize = 14,
    ///     };
    ///     series.DataLabelSettings = new CircularDataLabelSettings()
    ///     {
    ///         LabelStyle = labelStyle,
    ///     };
    ///     
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// *** 
    /// 
    /// </remarks>
    public class CircularDataLabelSettings : ChartDataLabelSettings
    {
        #region Internal Properties

        /// <summary>
        /// Identifies the <see cref="OverflowMode"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="OverflowMode"/> bindable property determines how the data labels handle overflow.
        /// </remarks>
        internal static readonly BindableProperty OverflowModeProperty = BindableProperty.Create(
            nameof(OverflowMode),
            typeof(OverflowMode),
            typeof(CircularDataLabelSettings),
            OverflowMode.None,
            BindingMode.Default,
            null,
            null);

        internal OverflowMode OverflowMode
        {
            get { return (OverflowMode)GetValue(OverflowModeProperty); }
            set { SetValue(OverflowModeProperty, value); }
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="SmartLabelAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SmartLabelAlignment"/> bindable property used to 
        /// arrange the data labels smartly when they overlap.
        /// </remarks>
        public static readonly BindableProperty SmartLabelAlignmentProperty = BindableProperty.Create(
            nameof(SmartLabelAlignment),
            typeof(SmartLabelAlignment),
            typeof(CircularDataLabelSettings),
            SmartLabelAlignment.Shift,
            BindingMode.Default,
            null,
            OnSmartLabelAlignmentChanged);

        /// <summary>
        /// Identifies the <see cref="LabelPosition"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="LabelPosition"/> bindable property determines the 
        /// position of the data labels either inside or outside the chart segment.
        /// </remarks>
        public static readonly BindableProperty LabelPositionProperty = BindableProperty.Create(
            nameof(LabelPosition),
            typeof(ChartDataLabelPosition),
            typeof(CircularDataLabelSettings),
            ChartDataLabelPosition.Inside,
            BindingMode.Default,
            null,
            OnLabelPositionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ConnectorLineSettings"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ConnectorLineSettings"/> bindable property determines the 
        /// customization options available for the connector lines.
        /// </remarks>
        public static readonly BindableProperty ConnectorLineSettingsProperty = BindableProperty.Create(
            nameof(ConnectorLineSettings),
            typeof(ConnectorLineStyle),
            typeof(CircularDataLabelSettings),
            null,
            BindingMode.Default,
            null,
            null);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to arrange the data labels smartly when they overlap.
        /// </summary>
        /// <value>It accepts <see cref="Syncfusion.Maui.Toolkit.Charts.SmartLabelAlignment"/> values and its default value is <see cref = "SmartLabelAlignment.Shift"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///            <chart:PieSeries ItemsSource="{Binding Data}"                                                      
        ///                             XBindingPath="Product"
        ///                             YBindingPath="SalesRate"
        ///                             ShowDataLabels="True">
        ///                <chart:PieSeries.DataLabelSettings>
        ///                    <chart:CircularDataLabelSettings LabelPosition="Outside" SmartLabelAlignment="Hide"/>
        ///                </chart:PieSeries.DataLabelSettings>
        ///            </chart:PieSeries>
        ///      </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = new ViewModel().Data;
        ///     series.XBindingPath = "Product";
        ///     series.YBindingPath = "SalesRate";
        ///     series.ShowDataLabels = true;
        ///     series.DataLabelSettings = new CircularDataLabelSettings()
        ///     {
        ///        LabelPosition= ChartDataLabelPosition.Outside,
        ///        SmartLabelAlignment = SmartLabelAlignment.Hide,
        ///     };
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>        
        public SmartLabelAlignment SmartLabelAlignment
        {
            get { return (SmartLabelAlignment)GetValue(SmartLabelAlignmentProperty); }
            set { SetValue(SmartLabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the options to position the data labels either inside or outside the chart segment.
        /// </summary>
        /// <value>It accepts <see cref="ChartDataLabelPosition"/> values and its default value is <see cref="ChartDataLabelPosition.Inside"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///            <chart:PieSeries ItemsSource="{Binding Data}"                                                          
        ///                             XBindingPath="Product"
        ///                             YBindingPath="SalesRate"
        ///                             ShowDataLabels="True">
        ///                <chart:PieSeries.DataLabelSettings>
        ///                    <chart:CircularDataLabelSettings LabelPosition="Outside"/>
        ///                </chart:PieSeries.DataLabelSettings>
        ///            </chart:PieSeries>
        ///      </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = new ViewModel().Data;
        ///     series.XBindingPath = "Product";
        ///     series.YBindingPath = "SalesRate";
        ///     series.ShowDataLabels = true;
        ///     series.DataLabelSettings = new CircularDataLabelSettings()
        ///     {
        ///        LabelPosition= ChartDataLabelPosition.Outside,
        ///     };
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>       
        public ChartDataLabelPosition LabelPosition
        {
            get { return (ChartDataLabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the connector lines in the circular series.
        /// </summary>
        /// <value>It accepts <see cref="ConnectorLineStyle"/> and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///    <chart:SfCircularChart>
        ///       <chart:PieSeries ItemsSource ="{Binding Data}" 
        ///                        XBindingPath="XValue" 
        ///                        YBindingPath="YValue"
        ///                        ShowDataLabels="True">
        ///       <chart:PieSeries.DataLabelSettings>
        ///            <chart:CircularDataLabelSettings LabelPosition="Outside" >
        ///                <chart:CircularDataLabelSettings.ConnectorLineSettings>
        ///                   <chart:ConnectorLineStyle Stroke="Red" StrokeWidth="2" />
        ///                </chart:CircularDataLabelSettings.ConnectorLineSettings>
        ///            </chart:CircularDataLabelSettings>
        ///        </chart:PieSeries.DataLabelSettings>
        ///       </chart:PieSeries>
        ///    </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     
        ///     PieSeries series = new PieSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue",
        ///        ShowDataLabels = true,
        ///     };
        ///
        ///     var connectorLineStyle = new ConnectorLineStyle
        ///     {
        ///       Stroke = new SolidColorBrush(Colors.Red),
        ///       StrokeWidth = 2
        ///     };
        ///     series.DataLabelSettings = new CircularDataLabelSettings()
        ///     {
        ///       LabelPosition = ChartDataLabelPosition.Outside
        ///       ConnectorLineSettings = connectorLineStyle
        ///     };
        ///
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>      
        public ConnectorLineStyle ConnectorLineSettings
        {
            get { return (ConnectorLineStyle)GetValue(ConnectorLineSettingsProperty); }
            set { SetValue(ConnectorLineSettingsProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDataLabelSettings"/>.
        /// </summary>
        public CircularDataLabelSettings()
        {
            LabelStyle = new ChartDataLabelStyle() { FontSize = 14, Margin = new Thickness(8, 6) };
            ConnectorLineSettings = new ConnectorLineStyle();
        }

        #endregion

        #region Methods

        static void OnSmartLabelAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            InvalidateVisibleDataLabelSeries(bindable);
        }

        private static void OnLabelPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            InvalidateVisibleDataLabelSeries(bindable);
        }

        private static void InvalidateVisibleDataLabelSeries(BindableObject bindable)
        {
            if (bindable is CircularDataLabelSettings chartDataLabelSettings && chartDataLabelSettings.Parent is SfCircularChart circularChart)
            {
                foreach (var series in circularChart.Series)
                {
                    if (series.IsVisible && series.ShowDataLabels)
                    {
                        series.Invalidate();
                    }
                }
            }
        }

        #endregion
    }
}