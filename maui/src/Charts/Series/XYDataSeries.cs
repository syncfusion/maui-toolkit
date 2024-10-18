using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Serves as the base class for Y-range based Cartesian series.
    /// </summary>
    public abstract class XYDataSeries : CartesianSeries
    {
        #region Internal Properties

        internal IList<double> YValues { get; set; }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="YBindingPath"/> property specifies the Y-values in the <see cref="XYDataSeries"/>.
        /// </remarks>
        public static readonly BindableProperty YBindingPathProperty = BindableProperty.Create(
            nameof(YBindingPath),
            typeof(string),
            typeof(XYDataSeries),
            null,
            BindingMode.Default,
            null,
            OnYBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="StrokeWidth"/> property determines the thickness of the stroke in the series.
        /// </remarks>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(double),
            typeof(XYDataSeries),
            1d,
            BindingMode.Default,
            null,
            OnStrokeWidthPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue" />
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
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeWidth = "3" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="XYDataSeries"/>.
        /// </summary>
        protected XYDataSeries() : base()
        {
            YValues = new List<double>();
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            YValues.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void GenerateDataPoints()
        {
            GeneratePoints(new[] { YBindingPath }, YValues);
        }

        internal override void OnBindingPathChanged()
        {
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnBindingPathChanged();
        }

        internal override void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        internal virtual double GetDataLabelPositionAtIndex(int index)
        {
            return YValues == null ? 0f : YValues[index];
        }

        #endregion

        #region Private Methods

        static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is XYDataSeries series)
            {
                // UpdateIsLoadingProperties();
                series.OnBindingPathChanged();
            }
        }

        static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is XYDataSeries series)
            {
                series.UpdateStrokeWidth();
                series.InvalidateSeries();
            }
        }

        #endregion

        #endregion
    }
}