using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// ChartTooltipBehavior is often used to specify extra information when the mouse pointer moved over an element.
    /// </summary>
    /// <remarks>
    /// <para>The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in chart series.</para>
    /// <para>Create an instance of the <see cref="ChartTooltipBehavior"/> and set it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para>It provides the following options to customize the appearance of the tooltip:</para>
    /// <para> <b>Label Customization - </b> To customize the appearance of the tooltip, refer to the <see cref="TextColor"/>, <see cref="FontSize"/>, <see cref="FontAttributes"/>, and <see cref="FontFamily"/> properties.</para>
    /// <para> <b>Duration - </b> To show the tooltip with delay and indicate how long the tooltip will be visible, refer to the <see cref="Duration"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.ChartBehaviors>
    ///               <chart:ChartTooltipBehavior />
    ///           </chart:SfCartesianChart.ChartBehaviors>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     ChartTooltipBehavior tooltipBehavior = new ChartTooltipBehavior();
    ///     chart.ChartBehaviors.Add(tooltipBehavior);
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public partial class ChartTooltipBehavior : ChartBehavior, IParentThemeElement
    {
        #region Fields 

        TooltipInfo? _previousTooltipInfo = null;

#if ANDROID || IOS
        bool _isSingleTapActivated = false;
#endif

        #endregion

        #region Internal Properties

        internal IChart? Chart { get; set; }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Background"/> bindable property determines the 
        /// background brush of the chart tooltip.
        /// </remarks>
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
            nameof(Background),
            typeof(Brush),
            typeof(ChartTooltipBehavior),
            null,
            BindingMode.Default,
            null,
            null);

        /// <summary>
        /// Identifies the <see cref="Duration"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Duration"/> bindable property determines the duration 
        /// for which the chart tooltip is displayed.
        /// </remarks>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(
            nameof(Duration),
            typeof(int),
            typeof(ChartTooltipBehavior),
            2,
            BindingMode.Default,
            null);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="TextColor"/> bindable property determines the text color of the chart tooltip.
        /// </remarks>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(ChartTooltipBehavior),
            null,
            BindingMode.Default,
            null,
            null);

        /// <summary>
        /// Identifies the <see cref="Margin"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Margin"/> bindable property determines the margin around the chart tooltip.
        /// </remarks>
        public static readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(ChartTooltipBehavior),
            new Thickness(0),
            BindingMode.Default,
            null);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontSize"/> bindable property determines the 
        /// font size of the chart tooltip text.
        /// </remarks>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(float),
            typeof(ChartTooltipBehavior),
            float.NaN,
            BindingMode.Default,
            null);

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontFamily"/> bindable property determines the 
        /// font family of the chart tooltip text.
        /// </remarks>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(ChartTooltipBehavior),
            null,
            BindingMode.Default,
            null);

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontAttributes"/> bindable property determines the 
        /// font attributes (e.g., bold, italic) of the chart tooltip text.
        /// </remarks>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(ChartTooltipBehavior),
            FontAttributes.None,
            BindingMode.Default,
            null);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the brush value to customize the tooltip background.
        /// </summary>
        /// <value>It accepts the <see cref="Brush"/> value and the default value is Black.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior Background ="Red"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    Background = new SolidColorBrush(Colors.Red)
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the duration time in seconds for which tooltip will be displayed.
        /// </summary>
        /// <value>It accepts the <c>int</c>> values and the default value is 2.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior Duration ="3"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    Duration = 3
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color value to customize the text color of the tooltip label.
        /// </summary>
        /// <value>It accepts the <see cref="Color"/> values and the default value is White.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior TextColor ="Red"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    TextColor = Colors.Red,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a thickness value to adjust the tooltip margin.
        /// </summary>
        /// <value>It accepts the <see cref="Thickness"/> values and the default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior Margin ="5"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    Margin = new Thickness(5),
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to change the label's text size.
        /// </summary>
        /// <value>It accepts the float values and the default value is 14.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior FontSize ="20"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    FontSize = 20,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontFamily for the tooltip label.
        /// </summary>
        /// <value>It accepts <c>string</c> values and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior FontFamily ="OpenSansRegular"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    FontFamily = "OpenSansRegular",
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontAttributes for the tooltip label.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values and the default value is <see cref="FontAttributes.None"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior FontAttributes="Bold"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-16)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    FontAttributes = FontAttributes.Bold;
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTooltipBehavior"/> class.
        /// </summary>
        public ChartTooltipBehavior()
        {
        }

        #endregion

        #region Interface Implementation

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfChartCommonStyle();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method used to show tooltip view at nearest datapoint for given x and y value.
        /// </summary>
        public void Show(float pointX, float pointY, bool canAnimate)
        {
            if (Chart == null)
			{
				return;
			}

			var visibleSeries = (Chart.Area as IChartArea)?.VisibleSeries;

            //While the animation is in progress, ignore the tooltip show. 
            if (visibleSeries != null)
            {
                foreach (var series in visibleSeries)
                {
                    if (series.NeedToAnimateSeries || series.NeedToAnimateDataLabel)
					{
						return;
					}
				}
            }

            GenerateTooltip(Chart, pointX, pointY, canAnimate);
        }

        /// <summary>
        /// Hides the tooltip view.
        /// </summary>
        public void Hide()
        {
            if (Chart != null)
            {
                _previousTooltipInfo = null;
                Chart.TooltipView?.Hide(false);
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnSingleTap(IChart chart, float pointX, float pointY)
        {
            base.OnSingleTap(chart, pointX, pointY);

#if ANDROID || IOS
            if (chart is ChartBase chartBase)
            {
                _isSingleTapActivated = true;
                OnTouchUp(chartBase, pointX, pointY);
            }
#else
            if (chart != null)
            {
                Show(pointX, pointY, true);
            }
#endif
        }

        /// <inheritdoc/>
        protected internal override void OnTouchUp(ChartBase chart, float pointX, float pointY)
        {
            base.OnTouchUp(chart, pointX, pointY);

#if ANDROID || IOS
            if (_isSingleTapActivated)
            {
                Show(pointX, pointY, true);
                _isSingleTapActivated = false;
            }
#endif
        }

        /// <inheritdoc/>
        protected internal override void OnTouchMove(ChartBase chart, float pointX, float pointY)
        {
            if (chart is SfCartesianChart cartesianChart)
            {
                if (cartesianChart.ZoomPanBehavior is ChartZoomPanBehavior behavior && behavior.IsSelectionZoomingActivated)
                {
                    Hide();
                    return;
                }
            }

            if (DeviceType == PointerDeviceType.Mouse)
            {
                Show(pointX, pointY, true);
            }
        }

        #endregion

        #region Private Methods

        void GenerateTooltip(IChart chart, float x, float y, bool canAnimate)
        {
            Rect seriesBounds = chart.ActualSeriesClipRect;

            if (seriesBounds.Contains(x, y))
            {
                TooltipInfo? tooltipInfo = chart.GetTooltipInfo(this, x, y);

                if (tooltipInfo != null && tooltipInfo.Source is ITooltipDependent source)
                {
                    if (chart.TooltipView is not SfTooltip tooltip)
                    {
                        tooltip = new SfTooltip();
                        chart.TooltipView = tooltip;
                        tooltip.TooltipClosed += Tooltip_TooltipClosed;
                        chart.BehaviorLayout.Add(chart.TooltipView);
                    }

                    source.SetTooltipTargetRect(tooltipInfo, seriesBounds);

                    if (_previousTooltipInfo != null && _previousTooltipInfo.Source == tooltipInfo.Source && _previousTooltipInfo.Index == tooltipInfo.Index)
                    {
                        tooltip.Show(seriesBounds, tooltipInfo.TargetRect, false);
                    }
                    else
                    {
                        tooltip.BindingContext = tooltipInfo;
                        tooltip.Duration = Duration;
                        tooltip.Position = tooltipInfo.Position;
                        tooltip.SetBinding(SfTooltip.BackgroundProperty, 
							BindingHelper.CreateBinding(nameof(TooltipInfo.Background), getter: static(TooltipInfo tooltipInfo1) => tooltipInfo1.Background));
                        tooltip.Content = GetTooltipTemplate(tooltipInfo);
                        tooltip.Show(seriesBounds, tooltipInfo.TargetRect, canAnimate);
                    }

                    _previousTooltipInfo = tooltipInfo;
                }
            }
        }

        void Tooltip_TooltipClosed(object? sender, TooltipClosedEventArgs e)
        {
            _previousTooltipInfo = null;
        }

        static View? GetTooltipTemplate(TooltipInfo tooltipInfo)
        {
            View? view;

            if (tooltipInfo.Source is ITooltipDependent tooltip && tooltip.TooltipTemplate != null)
            {
                var layout = tooltip.TooltipTemplate.CreateContent();
#if NET10_0_OR_GREATER
				view = layout as View;
#else
				view = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
#endif
			}
            else
            {
                var layout = tooltipInfo.Source is ITooltipDependent source ? source.GetDefaultTooltipTemplate(tooltipInfo)?.CreateContent() : null;
#if NET10_0_OR_GREATER
				view = layout as View;
#else
				view = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
#endif
			}

			if (view != null)
            {
#if NET9_0_OR_GREATER
                var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity);
#else
				var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity).Request;
#endif
#if NET10_0_OR_GREATER
				view.Frame = new Rect(0, 0, size.Width, size.Height);
				view.InvalidateMeasure();
#else
				view.Layout(new Rect(0, 0, size.Width, size.Height));
#endif
			}

			return view;
        }

        #endregion

        #endregion
    }
}