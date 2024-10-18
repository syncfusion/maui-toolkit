using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="ChartLegendLabelStyle"/> class provides customizable styling options for the labels in a chart's legend, allowing you to define properties such as font size, font family, text color, margin, and font attributes to enhance the visual appearance of the legend labels.
    /// </summary>
    /// <remarks>
    /// It provides more options to customize the chart legend label
    /// 
    /// <para> <b>FontAttributes - </b> To customize the font attribute, refer to this <see cref="ChartLabelStyle.FontAttributes"/> property. </para>
    /// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLegendLabelStyle.TextColor"/> property. </para>
    /// <para> <b>Margin - </b> To customize the label margin, refer to this <see cref="ChartLegendLabelStyle.Margin"/> property. </para>
    /// <para> <b>FontSize - </b> To customize the font size, refer to this <see cref="ChartLegendLabelStyle.FontSize"/> property. </para>
    /// <para> <b>FontFamily - </b> To customize the font family, refer to this <see cref="ChartLegendLabelStyle.FontFamily"/> property. </para>
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
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
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    /// 
    /// chart.Legend = new ChartLegend(); 
    /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
    /// {
    ///     TextColor = Colors.Red,
    ///     FontSize = 25,
    /// };
    /// 
    /// chart.Legend=labelStyle;
    ///
    /// ]]>
    /// </code>
    /// *** 
    /// </example>
    public class ChartLegendLabelStyle : Element
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="TextColor"/> bindable property determines the text color of the chart legend label.
        /// </remarks>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(ChartLegendLabelStyle),
            null,
            BindingMode.Default,
            null,
            null,
            null,
            defaultValueCreator: TextColorDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="Margin"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Margin"/> bindable property determines the margin around the chart legend label.
        /// </remarks>
        public static readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(ChartLegendLabelStyle),
            new Thickness(0),
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontSize"/> bindable property determines the font size of the chart legend label.
        /// </remarks>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(ChartLegendLabelStyle),
            double.NaN,
            BindingMode.Default,
            null,
            null,
            null,
            null,
            FontSizeDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontFamily"/> bindable property determines the font family of the chart legend label.
        /// </remarks>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(ChartLegendLabelStyle),
            null,
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="FontAttributes"/> bindable property determines the font attributes 
        /// of the chart legend label.
        /// </remarks>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(ChartLegendLabelStyle),
            null,
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="DisableBrush"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="DisableBrush"/> bindable property determines the brush color of the chart legend label 
        /// when a legend item is toggled.
        /// </remarks>
        internal static readonly BindableProperty DisableBrushProperty = BindableProperty.Create(
            nameof(DisableBrush),
            typeof(Brush),
            typeof(ChartLegendLabelStyle),
            null,
            BindingMode.Default,
            null,
            null,
            null,
            defaultValueCreator: DisableBrushDefaultValueCreator);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's text color. 
        /// </summary>
        /// <value>It accepts the <see cref="Color"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle TextColor = "Red"/>
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        ///    {
        ///        TextColor = Colors.Red,
        ///    };
        /// 
        /// chart.Legend=labelStyle;
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
        /// Gets or sets a value that indicates the margin of the label.
        /// </summary>
        /// <value>It accepts <see cref="Thickness"/> values.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle Margin="10"/>
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        ///    {
        ///         Margin=10,
        ///    };
        /// 
        /// chart.Legend=labelStyle;
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
        /// Gets or sets a value that indicates the label's size.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle FontSize="25"/>
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        ///    {
        ///        FontSize = 25,
        ///    };
        /// 
        /// chart.Legend=labelStyle;
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the font family of the label.
        /// </summary>
        /// <value>It accepts <see cref="string"/> values.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle FontFamily="PlaywriteAR-Regular"/>
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        ///    {
        ///         FontFamily="PlaywriteAR-Regular",
        ///    };
        /// 
        /// chart.Legend=labelStyle;
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
        /// Gets or sets a value that indicates the font attributes of the label.
        /// </summary>
        /// <value>It accepts <see cref="string"/> values.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///  
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend>
        ///            <chart:ChartLegend.LabelStyle>
        ///                <chart:ChartLegendLabelStyle FontAttributes="Bold" />
        ///            </chart:ChartLegend.LabelStyle>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// chart.Legend = new ChartLegend(); 
        /// ChartLegendLabelStyle labelStyle = new ChartLegendLabelStyle()
        ///    {
        ///         FontAttributes = FontAttributes.Bold,
        ///    };
        /// 
        /// chart.Legend=labelStyle;
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

        internal Brush DisableBrush
        {
            get { return (Brush)GetValue(DisableBrushProperty); }
            set { SetValue(DisableBrushProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLegendLabelStyle"/> class.
        /// </summary>
        public ChartLegendLabelStyle()
        {

        }

        #endregion

        #region Private Methods

        static object TextColorDefaultValueCreator(BindableObject bindable)
        {
            return Color.FromArgb("#49454F");
        }

        static object FontSizeDefaultValueCreator(BindableObject bindable)
        {
            return 12.0;
        }

        static object DisableBrushDefaultValueCreator(BindableObject bindable)
        {
            return new SolidColorBrush(Color.FromRgba("#611C1B1F"));
        }
        #endregion
    }
}