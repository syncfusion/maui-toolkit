using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Core = Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the legend for the <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, <see cref="SfFunnelChart"/>, <see cref="SfPyramidChart"/> and <see cref="SfPolarChart"/> classes.
	/// </summary>
	/// <remarks>
	/// The items in the legend contain the key information about the <see cref="ChartSeries"/>. The legend has all abilities such as docking, enabling, or disabling the desired series.
	///</remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.Legend>
	///               <chart:ChartLegend/>
	///           </chart:SfCartesianChart.Legend>
	///           
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     chart.Legend = new ChartLegend();
	///     
	/// ]]></code>
	/// ***
	/// </example>
	public class ChartLegend : Element, IChartLegend
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Placement"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Placement"/> bindable property determines the placement of the chart legend.
        /// </remarks>
        public static readonly BindableProperty PlacementProperty = BindableProperty.Create(
            nameof(Placement),
            typeof(LegendPlacement),
            typeof(ChartLegend),
            LegendPlacement.Top,
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ToggleSeriesVisibility"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ToggleSeriesVisibility"/> bindable property determines 
        /// whether the series visibility can be toggled through the legend.
        /// </remarks>
        public static readonly BindableProperty ToggleSeriesVisibilityProperty = BindableProperty.Create(
            nameof(ToggleSeriesVisibility),
            typeof(bool),
            typeof(ChartLegend),
            false,
            BindingMode.Default,
            null,
            OnToggleVisibilityChanged);

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ItemMargin"/> bindable property determines the margin around each legend item.
        /// </remarks>
        internal static readonly BindableProperty ItemMarginProperty = BindableProperty.Create(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(ChartLegend),
            new Thickness(double.NaN),
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ItemTemplate"/> bindable property determines the 
        /// template used to display each legend item.
        /// </remarks>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(ChartLegend),
            default(DataTemplate));

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="IsVisible"/> bindable property determines the visibility of the chart legend.
        /// </remarks>
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(ChartLegend),
            true,
            BindingMode.Default,
            null,
            OnLegendVisiblePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ItemsLayout"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ItemsLayout"/> bindable property determines the layout of the legend items.
        /// </remarks>
        public static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(
            nameof(ItemsLayout),
            typeof(Layout),
            typeof(ChartLegend),
            null,
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="LabelStyle"/> bindable property determines the style of the legend labels.
        /// </remarks>
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            nameof(LabelStyle),
            typeof(ChartLegendLabelStyle),
            typeof(ChartLegend),
            null,
            BindingMode.Default,
            null,
            propertyChanged: OnLegendStylePropertyChanged, 
            defaultValueCreator: LabelStyleDefaultValueCreator);
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the legend is visible or not.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>True</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend IsVisible = "True"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ IsVisible = true };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placement for the legend in a chart.
        /// </summary>
        /// <value>It accepts <see cref="LegendPlacement"/> values and the default value is <see cref="LegendPlacement.Top"/>.</value>
        /// <remarks>Legends can be placed left, right, top, or bottom around the chart area.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Placement = "Right"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ Placement = LegendPlacement.Right };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the layout configuration for the items in the chart legend. This property allows you to define how individual legend items are arranged within the legend.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="Layout"/>value and its default value is null.
        /// </value>
        /// <b>Note: </b>
        /// Users should not add items explicitly inside the layout. 
        /// Users must initialize the layout instance only and should not bind the item source to the layout.
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCircularChart x:Name="chart">
        ///     <chart:SfCircularChart.BindingContext>
        ///            <model:ViewModel/>
        ///     </chart:SfCircularChart.BindingContext>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///         <model:LegendExt Placement = "Bottom">
        ///             <chart:ChartLegend.ItemsLayout>
        ///                 <FlexLayout WidthRequest = "400" Background="Pink" Wrap="Wrap"/>
        ///             </chart:ChartLegend.ItemsLayout>
        ///         </model:LegendExt>
        ///     </chart:SfCircularChart.Legend>
        ///     
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        ///  LegendExt legend = new LegendExt();
        ///  legend.Placement = LegendPlacement.Bottom;
        ///
        ///  legend.ItemsLayout = new FlexLayout()
        ///  {
        ///    Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
        ///    WidthRequest = 400
        ///  };
        ///
        ///  chart.Legend = legend;
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// 
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// # [LegendExt.cs](#tab/tabid-9)
        /// <code><![CDATA[
        /// 
        ///     public class LegendExt : ChartLegend
        ///     {
        ///         protected override double GetMaximumSizeCoefficient()
        ///         {
        ///             return 0.5;
        ///         }
        ///     }
        /// ]]>
        /// </code>
        /// ***
        /// </example>  
        public Layout ItemsLayout
        {
            get { return (Layout)GetValue(ItemsLayoutProperty); }
            set { SetValue(ItemsLayoutProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the chart series visibility by tapping the legend item.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>False</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-10)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend ToggleSeriesVisibility = "True"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-11)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ ToggleSeriesVisibility = true };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ToggleSeriesVisibility
        {
            get { return (bool)GetValue(ToggleSeriesVisibilityProperty); }
            set { SetValue(ToggleSeriesVisibilityProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a template to customize the appearance of each legend item.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend>
        ///            <chart:ChartLegend.ItemTemplate>
        ///                <DataTemplate>
        ///                    <StackLayout Orientation="Horizontal" >
        ///                        <Rectangle HeightRequest="12" WidthRequest="12" Margin="3"
        ///                                   Background="{Binding IconBrush}"/>
        ///                        <Label Text="{Binding Text}" Margin="3"/>
        ///                    </StackLayout>
        ///                </DataTemplate>
        ///            </chart:ChartLegend.ItemTemplate>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to customize the appearance of chart legend labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.ChartLegendLabelStyle"/> value.</value>
        /// <remarks>
        /// To customize the legend labels appearance, Create an instance of the <see cref="ChartLegendLabelStyle"/> class and set to the <see cref="LabelStyle"/> property.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle TextColor = "Red" FontSize="25"/>
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        ///     
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        /// {
        ///      TextColor = Colors.Red,
        ///      FontSize = 25,
        /// };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Legend=labelStyle;
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
        public ChartLegendLabelStyle LabelStyle
        {
            get { return (ChartLegendLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        internal SfLegend? sfLegend { get; set; }
        IEnumerable? itemsSource;
#pragma warning disable CS8603 // Possible null reference return.
        IEnumerable ILegend.ItemsSource { get => itemsSource; set => itemsSource = value; }
#pragma warning restore CS8603 // Possible null reference return.
        bool ILegend.ToggleVisibility { get => ToggleSeriesVisibility; set => ToggleSeriesVisibility = value; }
        DataTemplate ILegend.ItemTemplate { get => ItemTemplate; set => ItemTemplate = value; }
        Layout ILegend.ItemsLayout { get => ItemsLayout; set => ItemsLayout = value; }
        Func<double> getLegendSizeCoeff = GetDefaultLegendSize;

        Func<double> ILegend.ItemsMaximumHeightRequest
        {
            get => GetMaximumSizeCoefficient;
            set { getLegendSizeCoeff = value; }
        }

        #endregion

        #region Event

        /// <summary>
        /// This event occurs when the legend label is created.
        /// </summary>
        /// <remarks>The <see cref="LegendItemEventArgs"/> contains the information of LegendItem.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend LegendItemCreated="OnLegendLabelCreated" />
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-16)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// ChartLegend legend = new ChartLegend();
        /// legend.LegendItemCreated += OnLegendLabelCreated;
        /// chart.Legend=legend;
        /// 
        /// void OnLegendLabelCreated(object sender, LegendItemEventArgs e)
        /// {
        ///      // You can customize the legend item.
        /// }
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public event EventHandler<LegendItemEventArgs>? LegendItemCreated;

        #endregion

        #region Methods

        #region Protected Methods
        /// <summary>
        /// Used to specify the maximum size coefficient for the chart legend.
        /// </summary>
        /// <remarks>
        /// The value must be between 0 and 1.
        /// </remarks>
        /// <value>It accepts the <c>double</c> values and its default value is 0.25.</value> 
        /// <para>
        /// Example code:
        /// </para>
        /// <code><![CDATA[
        /// 
        ///     public class LegendExt : ChartLegend
        ///     {
        ///         protected override double GetMaximumSizeCoefficient()
        ///         {
        ///             return 0.5;
        ///         }
        ///     }
        /// ]]>
        /// </code>
        protected virtual double GetMaximumSizeCoefficient()
        {
            return GetDefaultLegendSize();
        }

        /// <summary>
        /// Invokes when the parent of the chart legend is changed.
        /// </summary>
        ///  <exclude/>
        protected override void OnParentChanged()
        {
            base.OnParentChanged();

            if (LabelStyle != null)
            {
                LabelStyle.Parent = Parent;
            }
        }

        ///  <inheritdoc/>
        ///  <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, BindingContext);
            }
        }

        #endregion

        #region Internal Methods
        internal void Dispose()
        {
            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, null);
            }
        }
        #endregion

        #region Private Methods

        static void OnToggleVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartLegend legend)
            {
                if (legend.sfLegend != null)
                {
                    legend.sfLegend.ToggleVisibility = (bool)newValue;
                }
            }
        }

        static double GetDefaultLegendSize()
        {
            return 0.25d;
        }

        static object? LabelStyleDefaultValueCreator(BindableObject bindable)
        {
            return null;
        }

        static void OnLegendStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartLegend legend)
            {
                if (oldValue is ChartLegendLabelStyle oldStyle)
                {
                    oldStyle.PropertyChanged -= legend.LabelStylePropertyChanged;
                    SetInheritedBindingContext((Element)oldValue, null);
                    oldStyle.Parent = null;
                }

                if (newValue is ChartLegendLabelStyle newStyle)
                {
                    newStyle.Parent = legend.Parent;
                    newStyle.PropertyChanged += legend.LabelStylePropertyChanged;
                    SetInheritedBindingContext((Element)newValue, legend.BindingContext);
                    legend.UpdateLegendItems(newStyle);
                }
                else
                {
                    if (legend.Parent is IChart chart)
                    {
                        legend.UpdateLegendItems(chart.LegendLabelStyle);
                    }
                }
            }
        }

        void UpdateLegendItems(ChartLegendLabelStyle newStyle)
        {
            if (Parent is ChartBase chart)
            {
                if (chart.LegendLayout.AreaBase.PlotArea is IChartPlotArea plotArea)
                {
                    plotArea.UpdateItemsLabelStyle();
                }
            }
        }

        void LabelStylePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Parent is ChartBase chart && sender is ChartLegendLabelStyle style && chart.LegendLayout.AreaBase.PlotArea is IChartPlotArea plotArea)
            {
                var updateActions = new Dictionary<string, Action<ILegendItem>>
                {
                    { nameof(ChartLegendLabelStyle.TextColor), item => item.TextColor = style.TextColor },
                    { nameof(ChartLegendLabelStyle.FontAttributes), item => item.FontAttributes = style.FontAttributes },
                    { nameof(ChartLegendLabelStyle.FontFamily), item => item.FontFamily = style.FontFamily },
                    { nameof(ChartLegendLabelStyle.Margin), item => item.TextMargin = style.Margin },
                    { nameof(ChartLegendLabelStyle.DisableBrush), item => item.DisableBrush = style.DisableBrush },
                    { nameof(ChartLegendLabelStyle.FontSize), item => item.FontSize = (float)style.FontSize }
                };

                if (updateActions.TryGetValue(e.PropertyName.Tostring(), out var updateAction))
                {
                    foreach (var item in plotArea.LegendItems)
                    {
                        updateAction(item);
                    }
                }
            }
        }

        static void OnLegendVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not ChartLegend legend)
                return;

            if (legend.Parent is not ChartBase chart)
                return;

            if (chart.LegendLayout is not LegendLayout layout)
                return;

            bool isVisible = (bool)newValue;
            layout.Legend = isVisible ? legend : null;
            layout.AreaBase.PlotArea.ShouldPopulateLegendItems = true;
            layout.AreaBase.NeedsRelayout = true;
            layout.AreaBase.ScheduleUpdateArea();
        }

        #endregion

        #region Interface Methods

        void IChartLegend.OnLegendItemCreated(ILegendItem legendItem)
        {
            LegendItemCreated?.Invoke(this, new LegendItemEventArgs(legendItem));
        }

        SfLegend ILegend.CreateLegendView()
        {
            sfLegend = new SfLegendExt();
            return sfLegend;
        }

        #endregion

        #endregion
    }

    internal class SfLegendExt : SfLegend
    {
        protected override SfShapeView CreateShapeView()
        {
            return new ChartLegendShapeView();
        }
    }

    internal class ChartLegendShapeView : SfShapeView
    {
        public override void DrawShape(ICanvas canvas, Rect rect, Core.ShapeType shapeType, Brush strokeColor, float strokeWidth, Brush fillColor)
        {
            if (BindingContext is LegendItem legendItem)
            {
                if (legendItem.IconType == Core.ShapeType.HorizontalLine || legendItem.IconType == Core.ShapeType.VerticalLine)
                {
                    //TODO: Reason for strokeWidth 0, default legend icon not has stroke support.
                    base.DrawShape(canvas, rect, legendItem.IconType, strokeColor, 1, fillColor);

                    if (legendItem.Item is IMarkerDependent markerDependent && markerDependent.ShowMarkers)
                    {
                        canvas.CanvasSaveState();

                        if (markerDependent.MarkerSettings is ChartMarkerSettings setting)
                        {
                            shapeType = ChartUtils.GetShapeType(setting.Type);
                            var center = rect.Center;
                            var iconHeight = (float)setting.Height;
                            var iconWidth = (float)setting.Width;
                            rect = new Rect(center.X - (iconWidth / 2), center.Y - (iconHeight / 2), iconWidth, iconHeight);
                            strokeWidth = (float)setting.StrokeWidth;
                            strokeColor = setting.Stroke;
                            fillColor = setting.Fill ?? fillColor;
                        }

                        base.DrawShape(canvas, rect, shapeType, strokeColor, strokeWidth, fillColor);
                        canvas.CanvasRestoreState();
                    }
                }
                else
                {
                    base.DrawShape(canvas, rect, legendItem.IconType, strokeColor, 0, fillColor);
                }
            }
        }

        internal override void DrawCustomShape(ICanvas canvas, Rect rect, Brush fillColor, Brush strokeColor, float strokeWidth)
        {
            if (BindingContext is LegendItem legendItem)
            {
                canvas.StrokeColor = (strokeColor as SolidColorBrush)?.Color;
                canvas.StrokeSize = strokeWidth;
                canvas.SetFillPaint(fillColor, rect);

                if (legendItem.Source is IDrawCustomLegendIcon source)
                {
                    source.DrawSeriesLegend(canvas, rect, fillColor, true);
                }
                else
                {
                    DrawShape(canvas, rect, Core.ShapeType.Circle, fillColor, strokeWidth > 0, true);
                }
            }
        }
    }
}
