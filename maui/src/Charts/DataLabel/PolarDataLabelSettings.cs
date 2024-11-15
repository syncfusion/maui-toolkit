namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The PolarDataLabelSettings class is used to customize the appearance of polar series data labels.
    /// </summary>
    /// <remarks>
    /// <para>To customize data labels, create an instance of the <see cref="PolarDataLabelSettings"/> class and set it to the DataLabelSettings property of a polar series.</para>
    /// 
    /// <para><b>ShowDataLabels</b></para>
    /// <para>Data labels can be added to a chart series by enabling the <see cref="ChartSeries.ShowDataLabels"/> option.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///    <chart:SfPolarChart>
    ///
    ///       <!-- ... Eliminated for simplicity-->
    ///       <chart:PolarLineSeries ItemsSource ="{Binding Data}" 
    ///                              XBindingPath="XValue" 
    ///                              YBindingPath="YValue"
    ///                              ShowDataLabels="True"/>
    ///
    ///    </chart:SfPolarChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     // Eliminated for simplicity
    ///     PolarLineSeries series = new PolarLineSeries()
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
    ///    <chart:SfPolarChart>
    ///
    ///     <!-- ... Eliminated for simplicity-->
    ///       <chart:PolarLineSeries ItemsSource ="{Binding Data}" 
    ///                              XBindingPath="XValue" 
    ///                              YBindingPath="YValue"
    ///                              ShowDataLabels="True">
    ///          <chart:PolarLineSeries.PolarDataLabelSettings>
    ///               <chart:PolarDataLabelSettings>
    ///                    <chart:PolarDataLabelSettings.LabelStyle>
    ///                        <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
    ///                    </chart:PolarDataLabelSettings.LabelStyle>
    ///                </chart:PolarDataLabelSettings>
    ///          </chart:PolarLineSeries.PolarDataLabelSettings>
    ///       <chart:PolarLineSeries />
    ///    </chart:SfPolarChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     // Eliminated for simplicity
    ///     PolarLineSeries series = new PolarLineSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true
    ///     };
    ///
    ///     var labelStyle = new ChartDataLabelStyle()
    ///     { 
    ///         Background = new SolidColorBrush(Colors.Red),
    ///         TextColor = Colors.Black,
    ///         FontSize = 14
    ///     };
    ///     series.DataLabelSettings = new PolarDataLabelSettings()
    ///     {
    ///         LabelStyle = labelStyle
    ///     };
    ///
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// *** 
    /// </remarks>
    public class PolarDataLabelSettings : ChartDataLabelSettings
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarDataLabelSettings"/>.
        /// </summary>
        public PolarDataLabelSettings()
        {
            LabelStyle = new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(5) };
        }

        #endregion
    }
}