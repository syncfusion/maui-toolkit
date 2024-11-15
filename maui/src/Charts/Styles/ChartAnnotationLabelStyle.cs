using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents properties for customizing annotation labels.
    /// </summary>
    /// <remarks>
    /// This class provides options to customize the appearance of annotation labels.
    /// <para> <b>HorizontalAlignment - </b> Specifies the horizontal alignment of the annotation label. Refer to the <see cref="HorizontalTextAlignment"/> property for details. </para>
    /// <para> <b>VerticalAlignment - </b> Specifies the vertical alignment of the annotation label. Refer to the <see cref="VerticalTextAlignment"/> property for details. </para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///     <!-- ... Eliminated for simplicity-->
    ///     <chart:SfCartesianChart.Annotations>
    ///          <chart:TextAnnotation X1="3" Y1="30" Text="TextAnnotation">
    ///           <chart:TextAnnotation.LabelStyle>
    ///             <chart:ChartAnnotationLabelStyle VerticalTextAlignment="Start"/>
    ///           </chart:TextAnnotation.LabelStyle>
    ///          </chart:TextAnnotation>
    ///     </chart:SfCartesianChart.Annotations>  
    ///     
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();     
    ///
    ///     // Eliminated for simplicity
    ///     var text = new TextAnnotation()
    ///     {
    ///         X1 = 3,
    ///         Y1 = 30,    
    ///         Text = "TextAnnotation",
    ///     };
    ///  
    ///  text.LabelStyle = new ChartAnnotationLabelStyle()
    ///  {
    ///     VerticalTextAlignment = ChartLabelAlignment.Start,
    ///  };
    ///  
    /// chart.Annotations.Add(text);
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class ChartAnnotationLabelStyle : ChartLabelStyle
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="VerticalTextAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="VerticalTextAlignment"/> bindable property determines the vertical 
        /// text alignment of the chart annotation.
        /// </remarks>
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
            nameof(VerticalTextAlignment), 
            typeof(ChartLabelAlignment), 
            typeof(ChartAnnotationLabelStyle), 
            ChartLabelAlignment.Center, 
            BindingMode.Default, 
            null);

        /// <summary>
        /// Identifies the <see cref="HorizontalTextAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="HorizontalTextAlignment"/> bindable property determines the horizontal 
        /// text alignment of the chart annotation.
        /// </remarks>
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
            nameof(HorizontalTextAlignment), 
            typeof(ChartLabelAlignment), 
            typeof(ChartAnnotationLabelStyle), 
            ChartLabelAlignment.Center, 
            BindingMode.Default, 
            null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAnnotationLabelStyle"/>.
        /// </summary>
        public ChartAnnotationLabelStyle()
        {
            ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the vertical text alignment of chart annotation. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartLabelAlignment"/> as its value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:TextAnnotation X1="3" Y1="30" Text="TextAnnotation">
        ///           <chart:HorizontalLineAnnotation.LabelStyle>
        ///             <chart:ChartAnnotationLabelStyle VerticalTextAlignment="Start"/>
        ///           </chart:HorizontalLineAnnotation.LabelStyle>
        ///          </chart:HorizontalLineAnnotation>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///     // Eliminated for simplicity
        ///     var text = new TextAnnotation()
        ///     {
        ///         X1 = 3,
        ///         Y1 = 30,    
        ///         Text = "TextAnnotation",
        ///     };
        ///  
        ///  text.LabelStyle = new ChartAnnotationLabelStyle()
        ///  {
        ///     VerticalTextAlignment = ChartLabelAlignment.Start,
        ///  };
        ///  
        /// chart.Annotations.Add(text);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLabelAlignment VerticalTextAlignment
        {
            get { return (ChartLabelAlignment)GetValue(VerticalTextAlignmentProperty); }
            set { SetValue(VerticalTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal text alignment of chart annotation. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartLabelAlignment"/> as its value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:TextAnnotation X1="3" Y1="30" Text="TextAnnotation">
        ///           <chart:HorizontalLineAnnotation.LabelStyle>
        ///             <chart:ChartAnnotationLabelStyle HorizontalTextAlignment="Start"/>
        ///           </chart:HorizontalLineAnnotation.LabelStyle>
        ///          </chart:HorizontalLineAnnotation>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///     // Eliminated for simplicity
        ///     var text = new TextAnnotation()
        ///     {
        ///         X1 = 3,
        ///         Y1 = 30,    
        ///         Text = "TextAnnotation",
        ///     };
        ///  
        ///  text.LabelStyle = new ChartAnnotationLabelStyle()
        ///  {
        ///     HorizontalTextAlignment = ChartLabelAlignment.Start,
        ///  };
        ///  
        /// chart.Annotations.Add(text);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLabelAlignment HorizontalTextAlignment
        {
            get { return (ChartLabelAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        #endregion

        internal override Color GetDefaultTextColor()
        {
            // Same value for shape , text annotation and show axis label
            return Color.FromArgb("#49454F");
        }

        internal override Brush GetDefaultBackgroundColor()
        {
            // Same value for shape , text annotation and show axis label
            return new SolidColorBrush(Colors.Transparent);
        }
    }
}