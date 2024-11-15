namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a base class for the <see cref="CartesianDataLabelSettings"/>, <see cref="CircularDataLabelSettings"/> , <see cref="PyramidDataLabelSettings"/> , <see cref="FunnelDataLabelSettings"/> and <see cref="PolarDataLabelSettings"/> classes.
    /// </summary>
    /// <remarks>
    /// <para>Data labels can be added to a chart or chart series by enabling the <c>ShowDataLabels</c> option.</para>
    /// <para><see cref="ChartDataLabelSettings"/> is used to customize the appearance of the data label that appears on a series.</para>
    /// </remarks>
    public abstract class ChartDataLabelSettings : BindableObject
    {
        #region Fields

        Element? _parent;

        #endregion

        #region Internal Properties

        internal List<string> IsNeedDataLabelMeasure { get; set; } = new List<string>() { nameof(LabelPlacement), nameof(LabelStyle) };

        internal Element? Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnParentSet();
                }
            }
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="UseSeriesPalette"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="UseSeriesPalette"/> bindable property determines 
        /// whether the data labels use the series palette colors.
        /// </remarks>
        public static readonly BindableProperty UseSeriesPaletteProperty = BindableProperty.Create(
            nameof(UseSeriesPalette),
            typeof(bool),
            typeof(ChartDataLabelSettings),
            true,
            BindingMode.Default,
            null,
            null);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="LabelStyle"/> bindable property determines the style of the data labels.
        /// </remarks>
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            nameof(LabelStyle),
            typeof(ChartDataLabelStyle),
            typeof(ChartDataLabelSettings),
            null,
            BindingMode.Default,
            null,
            null);

        /// <summary>
        /// Identifies the <see cref="LabelPlacement"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="LabelPlacement"/> bindable property determines the placement of the data labels.
        /// </remarks>
        public static readonly BindableProperty LabelPlacementProperty = BindableProperty.Create(
            nameof(LabelPlacement),
            typeof(DataLabelPlacement),
            typeof(ChartDataLabelSettings),
            DataLabelPlacement.Auto,
            BindingMode.Default,
            null,
            null);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the background of the data label should be filled with the series color or not.
        /// </summary>
        /// <value>It accepts the <c>bool</c> value and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True">
        ///                <chart:LineSeries.DataLabelSettings>
        ///                    <chart:CartesianDataLabelSettings UseSeriesPalette="False" />
        ///                </chart:LineSeries.DataLabelSettings>
        ///              </chart:LineSeries>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     var dataLabelSettings = new CartesianDataLabelSettings()
        ///     {
        ///         UseSeriesPalette = false,
        ///     };
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///           DataLabelSettings = dataLabelSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool UseSeriesPalette
        {
            get { return (bool)GetValue(UseSeriesPaletteProperty); }
            set { SetValue(UseSeriesPaletteProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for customizing the data labels.
        /// </summary>
        /// <value>It accepts <see cref="ChartDataLabelStyle"/> and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True">
        ///                <chart:LineSeries.DataLabelSettings>
        ///            <chart:CartesianDataLabelSettings>
        ///                <chart:CartesianDataLabelSettings.LabelStyle>
        ///                    <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
        ///                </chart:CartesianDataLabelSettings.LabelStyle>
        ///            </chart:CartesianDataLabelSettings>
        ///        </chart:LineSeries.DataLabelSettings>
        ///              </chart:LineSeries>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     var labelStyle = new ChartDataLabelStyle()
        ///     { 
        ///         Background = new SolidColorBrush(Colors.Red),
        ///         TextColor = Colors.Black,
        ///         FontSize = 14,
        ///     };
        ///     var dataLabelSettings = new CartesianDataLabelSettings()
        ///     {
        ///         LabelStyle = labelStyle,
        ///     };
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///           DataLabelSettings = dataLabelSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ChartDataLabelStyle LabelStyle
        {
            get { return (ChartDataLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// Determines the placement of data labels relative to the data points in a chart, such as inside, outside, or at the center of the data point.
        /// </summary>
        /// <value>It accepts <see cref="DataLabelPlacement"/> values and its default value is <see cref="DataLabelPlacement.Auto"/>.</value>
        /// <remarks> Applies to Cartesian, Funnel, and Pyramid charts.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True">
        ///                <chart:LineSeries.DataLabelSettings>
        ///                     <chart:CartesianDataLabelSettings LabelPlacement ="Center"/>
        ///                </chart:LineSeries.DataLabelSettings>
        ///              </chart:LineSeries>
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
        ///     var dataLabelSettings = new CartesianDataLabelSettings()
        ///     {
        ///         LabelPlacement = DataLabelPlacement.Center,
        ///     };
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///           DataLabelSettings = dataLabelSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public DataLabelPlacement LabelPlacement
        {
            get { return (DataLabelPlacement)GetValue(LabelPlacementProperty); }
            set { SetValue(LabelPlacementProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataLabelSettings"/> class.
        /// </summary>
        public ChartDataLabelSettings()
        {
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, BindingContext);
            }
        }

        #endregion

        #region Internal DataLabel Calculation Methods

        internal Color GetContrastTextColor(ChartSeries series, Brush? background, Brush? segmentBrush)
        {
            Color textColor = Colors.Black;

            if (background != null && (background as SolidColorBrush)?.Color != Colors.Transparent)
            {
                textColor = background is SolidColorBrush ? ChartUtils.GetContrastColor((background as SolidColorBrush)?.Color) : textColor;
            }
            else
            {
                var cartesianLabelSettings = this as CartesianDataLabelSettings;

                if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Center || (cartesianLabelSettings != null && (cartesianLabelSettings.BarAlignment == DataLabelAlignment.Middle || cartesianLabelSettings.BarAlignment == DataLabelAlignment.Bottom)))
                {
                    textColor = series.IsIndividualSegment() ? segmentBrush is SolidColorBrush ? ChartUtils.GetContrastColor((segmentBrush as SolidColorBrush)?.Color) : textColor : GetTextColorBasedOnChartBackground(series?.Chart);
                }
                else if (LabelPlacement == DataLabelPlacement.Outer)
                {
                    textColor = GetTextColorBasedOnChartBackground(series?.Chart);
                }
            }

            return textColor;
        }

        internal PointF GetAutoLabelPosition(ChartSeries chartSeries, float x, float y, SizeF labelSize, float padding, float borderWidth)
        {
            if (chartSeries == null) return PointF.Zero;

            var scatter = chartSeries as ScatterSeries;
            Size size = scatter != null ? new Size(scatter.PointWidth, scatter.PointHeight) : Size.Zero;
            float width = (float)size.Width;
            float height = (float)size.Height / 2;
            Rect clipRect = chartSeries.AreaBounds;

            if ((x - ((labelSize.Width / 2) + padding)) <= 0)
            {
                x = (labelSize.Width / 2) + padding + borderWidth + width;
            }
            else if ((x + ((labelSize.Width / 2) + padding)) >= clipRect.Width)
            {
                x = (float)clipRect.Width - (labelSize.Width / 2) - padding - borderWidth - width;
            }

            if ((y - ((labelSize.Height / 2) + padding)) <= 0)
            {
                y = (labelSize.Height / 2) + padding + borderWidth + height;
            }
            else if ((y + ((labelSize.Height / 2) + padding)) >= clipRect.Height)
            {
                y = (float)clipRect.Height - (labelSize.Height / 2) - padding - borderWidth - height;
            }

            return new PointF(x, y);
        }

        internal string GetLabelContent(double value)
        {
            string labelContent = string.Empty;

            if (double.IsNaN(value))
            {
                return labelContent;
            }

            if (LabelStyle != null && !string.IsNullOrEmpty(LabelStyle.LabelFormat))
            {
                labelContent = value.ToString(LabelStyle.LabelFormat);
            }
            else
            {
                labelContent = value.ToString("#.##");
            }

            return labelContent;
        }

        #endregion

        #region Private Method

        Color GetTextColorBasedOnChartBackground(IChart? chart)
        {
            var backgroundColor = chart?.BackgroundColor;

            return backgroundColor != null && backgroundColor != Colors.Transparent ? ChartUtils.GetContrastColor(backgroundColor) : Colors.Black;
        }

        void OnParentSet()
        {
            if (LabelStyle != null)
            {
                LabelStyle.Parent = Parent;
            }
        }

        #endregion

        #endregion
    }
}