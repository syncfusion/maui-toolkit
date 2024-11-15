namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The ConnectorLineStyle class is used to customize the appearance of the data label connector line when data labels are placed outside.
    /// </summary>
    /// <remarks>
    /// <para>To customize connector lines, create an instance of the <see cref="ConnectorLineStyle"/> class, and set it to the ConnectorLineSettings property of the CircularDataLabelSettings.</para>
    /// # [Xaml](#tab/tabid-1)
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
    /// 
    /// </remarks>
    public class ConnectorLineStyle : ChartLineStyle
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ConnectorType"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ConnectorType"/> bindable property determines the type of
        /// connector line used in the circular chart.
        /// </remarks>
        public static readonly BindableProperty ConnectorTypeProperty = BindableProperty.Create(
            nameof(ConnectorType),
            typeof(ConnectorType),
            typeof(ConnectorLineStyle),
            ConnectorType.Line,
            BindingMode.Default,
            null,
            null);

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorLineStyle"/> class.
        /// </summary>
        public ConnectorLineStyle()
        {
            //Need to update theme keys
            StrokeWidth = 2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the connector type.
        /// </summary>
        /// <value>It accepts <see cref="Syncfusion.Maui.Toolkit.Charts.ConnectorType"/> values and its default value is <see cref="ConnectorType.Line"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///    <chart:SfCircularChart>
        ///       <chart:PieSeries ItemsSource ="{Binding Data}" 
        ///                        XBindingPath="XValue" 
        ///                        YBindingPath="YValue"
        ///                        ShowDataLabels="True">
        ///       <chart:PieSeries.DataLabelSettings>
        ///            <chart:CircularDataLabelSettings LabelPosition="Outside" >
        ///                <chart:CircularDataLabelSettings.ConnectorLineSettings>
        ///                   <chart:ConnectorLineStyle ConnectorType="Curve"/>
        ///                </chart:CircularDataLabelSettings.ConnectorLineSettings>
        ///            </chart:CircularDataLabelSettings>
        ///        </chart:PieSeries.DataLabelSettings>
        ///       </chart:PieSeries>
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
        ///        ShowDataLabels = true,
        ///     };
        ///
        ///     var connectorLineStyle = new ConnectorLineStyle
        ///     {
        ///         ConnectorType = ConnectorType.Curve,
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
        public ConnectorType ConnectorType
        {
            get { return (ConnectorType)GetValue(ConnectorTypeProperty); }
            set { SetValue(ConnectorTypeProperty, value); }
        }

        #endregion

        #region Internal Override Methods

        internal override object UpdateStrokeValue()
        {
            return Brush.Default;
        }

        #endregion
    }
}