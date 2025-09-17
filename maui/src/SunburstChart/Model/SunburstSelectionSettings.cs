using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents the settings for configuring selection behavior in sunburst chart.
    /// </summary>
    /// <remarks>
    /// <para>The SunburstSelectionSettings class provides properties to customize how segments are selected in the sunburst chart. It controls the selection behavior, visual representation of selected segments, and various highlighting options.</para>
    /// 
    /// <para>This class holds properties to provide options for choosing different selection types, and customize the visual appearance of selected segments.</para>
    /// 
    /// <para><b>Sunburst Selection Types</b></para>
    /// <para>Selection Types determine which segments to be selected when a user interacts with the chart. SunburstSelectionSettings offers <see cref="Type"/> property with four options.</para>
    /// 
    /// <para>To configure the selection type, set the <see cref="Type"/> property to one of the <see cref="SunburstSelectionType"/> enum values:</para>
    /// 
    /// <list type="bullet">
    ///   <item><description><b>Single</b>: Only one segment can be selected at a time.</description></item>
    ///   <item><description><b>Group</b>: Selects the entire hierarchical group related to the clicked segment, including its including its ancestors and descendants.</description></item>
    ///   <item><description><b>Parent</b>: Selects the parent segment of the clicked segment.</description></item>
    ///   <item><description><b>Child</b>: Selects all child segments of the clicked segment.</description></item>
    /// </list>
    ///  
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    ///   <sunburst:SfSunburstChart.SelectionSettings>
    ///     <sunburst:SunburstSelectionSettings
    ///         Type="Child"
    ///         DisplayMode="HighlightByBrush"
    ///         Fill="Blue" />
    ///   </sunburst:SfSunburstChart.SelectionSettings>
    ///   
    ///   <sunburst:SfSunburstChart.Levels>
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="Country" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobDescription" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfSunburstChart sunburstChart = new SfSunburstChart();
    /// 
    /// // Configure selection settings
    /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
    /// {
    ///     Type = SunburstSelectionType.Child,
    ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush,
    ///     Fill = new SolidColorBrush(Colors.Blue)
    /// };
    /// 
    /// // Configure other chart properties
    /// sunburstChart.ItemsSource = (new SunburstViewModel()).DataSource;
    /// sunburstChart.ValueMemberPath = "EmployeesCount";
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    /// 
    /// Content = sunburstChart;
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Sunburst Selection Display Modes</b></para>
    /// 
    /// <para>Selection Display Modes determine how selected segments are visually highlighted in the chart. The SunburstSelectionSettings provides <see cref="DisplayMode"/> property with three options.</para>
    /// 
    /// <para>To configure the selection display mode, set the <see cref="DisplayMode"/> property to one of the <see cref="SunburstSelectionDisplayMode"/> enum values:</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-3)
    /// <code> <![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    ///   <sunburst:SfSunburstChart.SelectionSettings>
    ///     <sunburst:SunburstSelectionSettings
    ///         DisplayMode="HighlightByOpacity"
    ///         Opacity="0.6" />
    ///   </sunburst:SfSunburstChart.SelectionSettings>
    ///   
    ///   <sunburst:SfSunburstChart.Levels>
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="Country" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobDescription" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-4)
    /// <code><![CDATA[
    /// SfSunburstChart sunburstChart = new SfSunburstChart();
    /// 
    /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
    /// {
    ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity,
    ///     Opacity = 0.6
    /// };
    /// 
    /// sunburstChart.ItemsSource = (new SunburstViewModel()).DataSource;
    /// sunburstChart.ValueMemberPath = "EmployeesCount";
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    /// 
    /// Content = sunburstChart;
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Selection by Brush</b></para>
    /// 
    /// <para>When using <see cref="SunburstSelectionDisplayMode.HighlightByBrush"/>, you can customize the color applied to selected segments using the <see cref="Fill"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-5)
    /// <code><![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    ///   <sunburst:SfSunburstChart.SelectionSettings>
    ///     <sunburst:SunburstSelectionSettings
    ///         Type="Group"
    ///         DisplayMode="HighlightByBrush"
    ///         Fill="Green" />
    ///   </sunburst:SfSunburstChart.SelectionSettings>
    ///   
    ///   <sunburst:SfSunburstChart.Levels>
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="Country" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobDescription" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-6)
    /// <code><![CDATA[
    /// SfSunburstChart sunburstChart = new SfSunburstChart();
    /// 
    /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
    /// {
    ///     Type = SunburstSelectionType.Group,
    ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush,
    ///     Fill = new SolidColorBrush(Colors.Green)
    /// };
    /// 
    /// sunburstChart.ItemsSource = (new SunburstViewModel()).DataSource;
    /// sunburstChart.ValueMemberPath = "EmployeesCount";
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    /// 
    /// Content = sunburstChart;
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Selection by Stroke</b></para>
    /// 
    /// <para>When using <see cref="SunburstSelectionDisplayMode.HighlightByStroke"/>, you can customize the stroke color and width applied to selected segments using the <see cref="Stroke"/> and <see cref="StrokeWidth"/> properties.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-7)
    /// <code><![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    ///   <sunburst:SfSunburstChart.SelectionSettings>
    ///     <sunburst:SunburstSelectionSettings
    ///         Type="Parent"
    ///         DisplayMode="HighlightByStroke"
    ///         Stroke="Purple"
    ///         StrokeWidth="3" />
    ///   </sunburst:SfSunburstChart.SelectionSettings>
    ///   
    ///   <sunburst:SfSunburstChart.Levels>
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="Country" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobDescription" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath="JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-8)
    /// <code><![CDATA[
    /// SfSunburstChart sunburstChart = new SfSunburstChart();
    /// 
    /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
    /// {
    ///     Type = SunburstSelectionType.Parent,
    ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke,
    ///     Stroke = new SolidColorBrush(Colors.Purple),
    ///     StrokeWidth = 3
    /// };
    /// 
    /// sunburstChart.ItemsSource = (new SunburstViewModel()).DataSource;
    /// sunburstChart.ValueMemberPath = "EmployeesCount";
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    /// sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    /// 
    /// Content = sunburstChart;
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><see cref="Opacity"/>: Controls the transparency of the unselected segment's fill.</para>
    /// <para>The value must be between <b>0.0</b> (fully transparent) and <b>1.0</b> (fully opaque). The default value is <b>0.7</b>. This property is applied only to unselected segments to visually de-emphasize them. Selected segments are always rendered with full opacity (1.0).</para>
    ///
    /// <para><see cref="Fill"/>: Brush used to fill the selected segment.</para>
    /// <para>This property defines the interior color or pattern of the selected segment. The default is a solid color brush with the color <b>#1C1B1F</b>.</para>
    ///
    /// <para><see cref="Stroke"/>: Brush used for the border of the selected segment.</para>
    /// <para>This property defines the outline color or pattern of the selected segment. Like <see cref="Fill"/>, it supports various brush types. The default is a solid color brush with the color <b>#1C1B1F</b>.</para>
    ///
    /// <para><see cref="StrokeWidth"/>: Thickness of the stroke around the selected segment.</para>
    /// <para>This property sets the width of the border applied to selected segments. The default value is <b>2.0</b>. It is recommended to use values greater than 0 for visible borders.</para>
    /// </remarks>
    public class SunburstSelectionSettings : Element
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(
                nameof(Type), 
                typeof(SunburstSelectionType), 
                typeof(SunburstSelectionSettings),
                SunburstSelectionType.Single, 
                BindingMode.Default, 
                null, 
                OnSelectionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DisplayMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty DisplayModeProperty =
            BindableProperty.Create(
                nameof(DisplayMode), 
                typeof(SunburstSelectionDisplayMode), 
                typeof(SunburstSelectionSettings),
                SunburstSelectionDisplayMode.HighlightByBrush, 
                BindingMode.Default, 
                null, 
                OnSelectionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Opacity"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The value should be between 0 and 1, where 0 is fully transparent and 1 is fully opaque.
        /// </remarks>
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(
                nameof(Opacity), 
                typeof(double), 
                typeof(SunburstSelectionSettings),
                0.7, 
                BindingMode.Default, 
                null, 
                OnOpacityPropertyChanged, 
                null, 
                coerceValue: CoerceOpacity);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(
                nameof(Fill), 
                typeof(Brush), 
                typeof(SunburstSelectionSettings),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")), 
                BindingMode.Default, 
                null, 
                OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(
                nameof(Stroke), 
                typeof(Brush), 
                typeof(SunburstSelectionSettings),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")), 
                BindingMode.Default, 
                null, 
                OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// it is recommended to use when value is greater than 0.
        /// </remarks>
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(
                nameof(StrokeWidth), 
                typeof(double), 
                typeof(SunburstSelectionSettings),
                2.0, 
                BindingMode.Default, 
                null, 
                OnStrokeWidthPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of selection behavior.
        /// </summary>
        /// <value>The default value is <see cref="SunburstSelectionType.Single"/>.</value>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings Type="Child" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]></code>
        /// # [MainPage.xaml.cs](#tab/tabid-9)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     Type = SunburstSelectionType.Child,
        /// };
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public SunburstSelectionType Type
        {
            get { return (SunburstSelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the visual display mode for selected segments.
        /// </summary>
        /// <value>The default value is <see cref="SunburstSelectionDisplayMode.HighlightByBrush"/>.</value>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings DisplayMode="HighlightByStroke" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke,
        /// };
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public SunburstSelectionDisplayMode DisplayMode
        {
            get { return (SunburstSelectionDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the opacity value for selected segments when <see cref="DisplayMode"/> is set to <see cref="SunburstSelectionDisplayMode.HighlightByOpacity"/>.
        /// </summary>
        /// <remarks>
        /// The value should be between 0 and 1, where 0 is fully transparent and 1 is fully opaque.
        /// </remarks>
        /// <value>The default value is 0.7.</value>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings DisplayMode="HighlightByOpacity" Opacity="0.5" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-11)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity,
        ///     Opacity= 0.5
        /// };
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush to be applied to selected segments when <see cref="DisplayMode"/> is set to <see cref="SunburstSelectionDisplayMode.HighlightByBrush"/>.
        /// </summary>
        /// <value>The default value is  #1C1B1F.</value>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings DisplayMode="HighlightByBrush" Fill="Blue" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush,
        ///     Fill = new SolidColorBrush(Colors.Blue)
        /// };
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush to be applied to the stroke of selected segments when <see cref="DisplayMode"/> is set to <see cref="SunburstSelectionDisplayMode.HighlightByStroke"/>.
        /// </summary>
        /// <value>The default value is  #1C1B1F.</value>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings DisplayMode="HighlightByStroke" Stroke="Green" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-13)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke,
        ///     Stroke = new SolidColorBrush(Colors.Green),
        /// };
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke width for selected segments when <see cref="DisplayMode"/> is set to <see cref="SunburstSelectionDisplayMode.HighlightByStroke"/>.
        /// </summary>
        /// <value>The default value is 2.0.</value>
        /// <remarks>
        /// It is recommended to use it when value is greater than 0.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <sunburst:SfSunburstChart>
        ///     <sunburst:SfSunburstChart.SelectionSettings>
        ///         <sunburst:SunburstSelectionSettings DisplayMode="HighlightByStroke" StrokeWidth="3" />
        ///     </sunburst:SfSunburstChart.SelectionSettings>
        /// </sunburst:SfSunburstChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfSunburstChart sunburstChart = new SfSunburstChart();
        /// 
        /// sunburstChart.SelectionSettings = new SunburstSelectionSettings
        /// {
        ///     DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke,
        ///     StrokeWidth = 3
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
		/// Handles changes to the selection-related bindable properties.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
        static void OnSelectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        /// <summary>
		/// Handles changes to the <see cref="OpacityProperty"/>.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
        static void OnOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        /// <summary>
		/// Handles changes to the <see cref="FillProperty"/>.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
        static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           
        }

        /// <summary>
		/// Handles changes to the <see cref="StrokeProperty"/>.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
        static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        /// <summary>
		/// Handles changes to the <see cref="StrokeWidthProperty"/>.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
        static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           
        }

        #endregion

        #region Static Methods

        /// <summary>
		/// Coerces the <see cref="Opacity"/> value to be within the valid range of 0.0 to 1.0.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="value">The value to coerce.</param>
		/// <returns>The coerced value.</returns>
        static object CoerceOpacity(BindableObject bindable, object value)
        {
            return Math.Clamp(Convert.ToDouble(value), 0.0, 1.0);
        }

        #endregion
    }
}