using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    ///  Represents hierarchical data with concentric circles, where each ring signifies a hierarchy level, and segments denote data categories.
    /// </summary>
    /// <remarks>
    /// <para> The Sunburst chart control ensures a well-defined hierarchical structure of the data and effectively communicates relationships between different levels of information. </para>
    /// 
    /// <para> SfSunburstChart class properties provide an option to add the levels collection, allowing customization of the chart elements such as legend, data label, center view, and tooltip features.</para>
    /// 
    /// <para><b>Levels</b></para>
    /// <para>Levels are used to visualize different layers or depths in a hierarchy, aiding in the visualization of structured data. SfSunburstChart offers <see cref="Levels"/> property.</para>
    /// 
    /// <para>To add the levels, create an instance of the required <see cref="SunburstHierarchicalLevel"/> class, and add it to the  <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> property.</para>
    ///  
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    /// 
    ///   <sunburst:SfSunburstChart.BindingContext>
    ///        <model:SunburstViewModel x:Name="viewModel"/>
    ///   </sunburst:SfSunburstChart.BindingContext>
    ///
    ///   <sunburst:SfSunburstChart.Levels>
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath = "Country" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobDescription" />
    ///     <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    ///   
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// 
    ///  SfSunburstChart sunburstChart = new SfSunburstChart();
    ///  
    ///  BindingContext = new SunburstViewModel();
    ///  
    ///  sunburstChart.SetBinding(SfSunburstChart.ItemsSourceProperty, "DataSource");
    ///  sunburstChart.ValueMemberPath = "EmployeesCount";
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobGroup" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    ///  
    ///  Content = sunburstChart;
    /// ]]>
    /// </code>
    /// # [SunburstModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public class SunburstModel
    ///     {
    ///         public string JobDescription { get; set; }
    ///         public string JobGroup { get; set; }
    ///         public string JobRole { get; set; }
    ///         public double EmployeesCount { get; set; }
    ///         public double Count { get; set; }
    ///         public string Country { get; set; }
    ///     }
    /// ]]>
    /// </code>
    /// # [SunburstViewModel.cs](#tab/tabid-4)
    /// <code><![CDATA[
    /// public ObservableCollection<SunburstModel> DataSource { get; set; }
    /// 
    /// public SunburstViewModel()
    /// {
    ///    DataSource = new ObservableCollection<SunburstModel>
    ///    {
    ///         new SunburstModel { Country = "USA", JobDescription = "Sales", JobGroup="Executive", EmployeesCount = 50 , Count = 200},
    ///         new SunburstModel { Country = "USA", JobDescription = "Sales", JobGroup = "Analyst", EmployeesCount = 40 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Marketing", EmployeesCount = 40 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Technical", JobGroup = "Testers", EmployeesCount = 35 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Windows", EmployeesCount = 175 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Web", EmployeesCount = 70 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Management", EmployeesCount = 40 },
    ///         new SunburstModel { Country = "USA", JobDescription = "Accounts", EmployeesCount = 60 },
    ///         new SunburstModel { Country = "India", JobDescription = "Technical", JobGroup = "Testers", EmployeesCount = 33 },
    ///         new SunburstModel { Country = "India", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Windows", EmployeesCount = 125 },
    ///         new SunburstModel { Country = "India", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Web", EmployeesCount = 60 },
    ///         new SunburstModel { Country = "India", JobDescription = "HR Executives", EmployeesCount = 70 },
    ///         new SunburstModel { Country = "India", JobDescription = "Accounts", EmployeesCount = 45 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Sales", JobGroup = "Executive", EmployeesCount = 30 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Sales", JobGroup = "Analyst", EmployeesCount = 40 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Marketing", EmployeesCount = 50 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Technical", JobGroup = "Testers", EmployeesCount = 40 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Windows", EmployeesCount = 65 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Web", EmployeesCount = 27 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Management", EmployeesCount = 33 },
    ///         new SunburstModel { Country = "Germany", JobDescription = "Accounts", EmployeesCount = 55 },
    ///         new SunburstModel { Country = "UK", JobDescription = "Technical", JobGroup = "Testers", EmployeesCount = 25 },
    ///         new SunburstModel { Country = "UK", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Windows", EmployeesCount = 96 },
    ///         new SunburstModel { Country = "UK", JobDescription = "Technical", JobGroup = "Developers", JobRole = "Web", EmployeesCount = 55 },
    ///         new SunburstModel { Country = "UK", JobDescription = "HR Executives", EmployeesCount = 60 },
    ///         new SunburstModel { Country = "UK", JobDescription = "Accounts", EmployeesCount = 30 }
    ///    };
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend includes data points from the first-level items. The information provided in each legend item helps identify the corresponding sunburst sub levels. The Levels of <see cref="SunburstHierarchicalLevel.GroupMemberPath"/> property value will be displayed in the legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="SunburstLegend"/>and assign it to the <see cref="Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-5)
    /// <code> <![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" ValueMemberPath="EmployeesCount">
    /// 
    ///   <sunburst:SfSunburstChart.BindingContext>
    ///         <model:SunburstViewModel x:Name="viewModel"/>
    ///   </sunburst:SfSunburstChart.BindingContext>
    ///   
    ///   <sunburst:SfSunburstChart.Legend>
    ///         <sunburst:SunburstLegend x:Name="legend"/>
    ///   </sunburst:SunburstLegend>
    /// 
    ///   <sunburst:SfSunburstChart.Levels>
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "Country" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobDescription" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    ///   
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-6)
    /// <code><![CDATA[
    /// 
    ///  SfSunburstChart sunburstChart = new SfSunburstChart();
    ///  
    ///  BindingContext = new SunburstViewModel();
    ///  
    ///  sunburstChart.Legend = new SunburstLegend();
    ///  
    ///  sunburstChart.SetBinding(SfSunburstChart.ItemsSourceProperty, "DataSource");
    ///  sunburstChart.ValueMemberPath = "EmployeesCount";
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobGroup" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    ///  
    ///  Content = sunburstChart;
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a sunburst chart segment. To render the data labels, you need to enable the <see cref="ShowLabels"/> property as <b>true</b> in <see cref="SfSunburstChart"/> class. </para>
    /// 
    /// <para>To customize the sunburst chart data labels rotation mode using <see cref="SunburstLabelRotationMode"/> and its default value is <see cref="SunburstLabelRotationMode.Angle"/>, overflow mode using <see cref="SunburstLabelOverflowMode"/> and its default value is <see cref="SunburstLabelOverflowMode.Hide"/>, and label styles, you need to create an instance of <see cref="SunburstDataLabelSettings"/> and set it to the <see cref="DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-7)
    /// <code><![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" 
    ///              ShowLabels="True" ValueMemberPath="EmployeesCount">
    /// 
    ///   <sunburst:SfSunburstChart.BindingContext>
    ///         <model:SunburstViewModel x:Name="viewModel"/>
    ///   </sunburst:SfSunburstChart.BindingContext>
    ///   
    ///   <sunburst:SfSunburstChart.DataLabelSettings>
    ///           <sunburst:SunburstDataLabelSettings FontSize="13" FontAttributes="Italic"
    ///                                              RotationMode="Angle" OverFlowMode="Trim" />
    ///   </sunburst:SfSunburstChart.DataLabelSettings>
    ///  
    ///   <sunburst:SfSunburstChart.Levels>
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "Country" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobDescription" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    ///   
    /// </sunburst:SfSunburstChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-8)
    /// <code><![CDATA[
    ///  SfSunburstChart sunburstChart = new SfSunburstChart();
    ///  
    ///  BindingContext = new SunburstViewModel();
    ///  
    ///  sunburstChart.ShowLabels = true;
    ///  sunburstChart.DataLabelSettings = new SunburstDataLabelSettings()
    ///  {
    ///        OverFlowMode = SunburstLabelOverflowMode.Trim,
    ///        RotationMode = SunburstLabelRotationMode.Angle,
    ///        FontAttributes = FontAttributes.Italic,
    ///        FontSize = 13
    ///  };
    ///  sunburstChart.SetBinding(SfSunburstChart.ItemsSourceProperty, "DataSource");
    ///  sunburstChart.ValueMemberPath = "EmployeesCount";
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobGroup" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    ///  
    ///  Content = sunburstChart;
    ///         
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Tooltip</b></para>
    /// 
    /// <para>Tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the sunburst chart, you need to set the <see cref="EnableTooltip"/> property as <b>true</b> in <see cref="SfSunburstChart"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor, and Font, create an instance of the <see cref="SunburstTooltipSettings"/> class, modify the values, and assign it to the <see cref="TooltipSettings"/> property in <see cref="SfSunburstChart"/>. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-9)
    /// <code><![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" 
    ///              EnableTooltip="True" ValueMemberPath="EmployeesCount">
    /// 
    ///   <sunburst:SfSunburstChart.BindingContext>
    ///         <model:SunburstViewModel x:Name="viewModel"/>
    ///   </sunburst:SfSunburstChart.BindingContext>
    ///   
    ///   <sunburst:SfSunburstChart.TooltipSettings>
    ///         <sunburst:SunburstTooltipSettings />
    ///   </sunburst:SfSunburstChart.TooltipSettings>
    ///  
    ///   <sunburst:SfSunburstChart.Levels>
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "Country" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobDescription" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    ///   
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-10)
    /// <code><![CDATA[
    /// SfSunburstChart sunburstChart = new SfSunburstChart();
    ///  
    ///  BindingContext = new SunburstViewModel();
    /// 
    ///  sunburstChart.TooltipSettings = new SunburstTooltipSettings();
    /// 
    ///  sunburstChart.SetBinding(SfSunburstChart.ItemsSourceProperty, "DataSource");
    ///  sunburstChart.ValueMemberPath = "EmployeesCount";
    ///  sunburstChart.EnableTooltip = true;
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobGroup" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    ///  
    ///  Content = sunburstChart;
    ///
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>CenterView</b></para>
    /// 
    /// <para>Center is used to share additional information about the sunburst chart. The binding context of the Centerview will be the respective sunburst. To display the center view on the sunburst chart, you need to set the<see cref="CenterView"/> property in <see cref="SfSunburstChart"/>. </para>
    /// 
    ///  <para>CenterHoleSize is used to prevent overlapping with segments in the sunburst center view.</para>
    /// # [MainPage.xaml](#tab/tabid-11)
    /// <code><![CDATA[
    /// <sunburst:SfSunburstChart ItemsSource="{Binding DataSource}" 
    ///              ValueMemberPath="EmployeesCount">
    /// 
    ///   <sunburst:SfSunburstChart.BindingContext>
    ///         <model:SunburstViewModel x:Name="viewModel"/>
    ///   </sunburst:SfSunburstChart.BindingContext>
    ///   
    ///   <sunburst:SfSunburstChart.CenterView>
    ///            <StackLayout Orientation="Vertical"
    ///                         HeightRequest="{Binding CenterHoleSize}"
    ///                         WidthRequest="{Binding CenterHoleSize}"
    ///                         HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand">
    ///                <Label Text = "CenterView" />
    ///            </StackLayout>
    ///   </sunburst:SfSunburstChart.CenterView>
    ///  
    ///   <sunburst:SfSunburstChart.Levels>
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "Country" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobDescription" />
    ///         <sunburst:SunburstHierarchicalLevel GroupMemberPath = "JobRole" />
    ///   </sunburst:SfSunburstChart.Levels>
    ///   
    /// </sunburst:SfSunburstChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-12)
    /// <code><![CDATA[
    ///  SfSunburstChart sunburstChart = new SfSunburstChart();
    ///  
    ///  BindingContext = new SunburstViewModel();
    /// 
    ///  StackLayout layout = new StackLayout();
    ///  layout.HorizontalOptions = LayoutOptions.CenterAndExpand;
    ///  layout.VerticalOptions = LayoutOptions.CenterAndExpand;
    ///  layout.SetBinding(HeightRequestProperty, "CenterHoleSize");
    ///  layout.SetBinding(WidthRequestProperty, "CenterHoleSize");
    ///    
    ///  Label label = new Label();
    ///  label.Text = "CenterView";
    ///  layout.Children.Add(label);
    ///  sunburstChart.CenterView = layout;
    /// 
    ///  sunburstChart.SetBinding(SfSunburstChart.ItemsSourceProperty, "DataSource");
    ///  sunburstChart.ValueMemberPath = "EmployeesCount";
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "Country" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobDescription" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobGroup" });
    ///  sunburstChart.Levels.Add(new SunburstHierarchicalLevel() { GroupMemberPath = "JobRole" });
    ///  
    ///  Content = sunburstChart;
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    public class SfSunburstChart : View, IContentView, ITouchListener, ITapGestureListener, IParentThemeElement
    {
        #region private Fields
        List<SunburstItem>? _groupItems;
        IEnumerable? _internalDataSource;
        SunburstHierarchicalLevel _innerLevel;
        double _outerRadius;
        double _centerHoleSize = 1;
        private readonly LegendLayout legendLayout;
		private readonly SunburstChartArea area;
		private int ZoomingLevel = -1;
		ChartTitleView _titleView;

        View? _content;
        SunburstLevelCollection _actualLevel;

		#endregion

		#region Interface Override Properties

		/// <summary>
		/// Gets the raw content assigned to the view.
		/// </summary>
		object? IContentView.Content => _content;

        /// <summary>
        /// Gets the presented content value.
        /// </summary>
        IView? IContentView.PresentedContent => _content;

		/// <summary>
		/// Gets the padding applied to the view.
		/// </summary>
		Thickness IPadding.Padding => Thickness.Zero;

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfSunburstChartStyles();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        internal ChartLegendLabelStyle LegendStyle
        {
            get => (ChartLegendLabelStyle)GetValue(LegendStyleProperty);
            set => SetValue(LegendStyleProperty, value);
        }

        internal static readonly BindableProperty LegendStyleProperty = 
			BindableProperty.Create(
				nameof(LegendStyle), 
				typeof(ChartLegendLabelStyle), 
				typeof(SfSunburstChart), 
				new ChartLegendLabelStyle(), 
				propertyChanged: OnLegendPropertyChanged);

        #endregion

        #region Internal Properties

		/// <summary>
		/// Gets or sets the number of levels in the sunburst chart.
		/// </summary>
		internal int LevelsCount { get; set; }

		/// <summary>
		/// Gets or sets the actual clipping rectangle for the chart series.
		/// </summary>
		internal Rect ActualSeriesClipRect { get; set; }

		/// <summary>
		/// Gets or sets the rendering bounds for the chart series.
		/// </summary>
		internal Rect SeriesRenderBounds { get; set; }

		/// <summary>
		/// Gets the height of the chart title.
		/// </summary>
		internal double TitleHeight => _titleView != null ? _titleView.Height : 0;

		/// <summary>
		/// Gets or sets the center point of the sunburst chart.
		/// </summary>
		internal Point Center { get; set; }

		/// <summary>
		/// Gets or sets the size of each ring in the sunburst chart.
		/// </summary>
		internal double RingSize { get; set; }

		/// <summary>
		/// Gets or sets the layout for chart behaviors.
		/// </summary>
		internal AbsoluteLayout? BehaviorLayout { get; set; }

		/// <summary>
		/// Gets or sets the collection of segments in the sunburst chart.
		/// </summary>
		internal SunburstSegmentCollection Segments { get; set; }

		/// <summary>
		/// Gets or sets the current value of the animation.
		/// </summary>
		internal float AnimationValue { get; set; } = 1;

		/// <summary>
		/// Gets or sets the animation for operations.
		/// </summary>
		internal Animation? SunburstAnimation  { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the chart needs to be animated.
		/// </summary>
		internal bool NeedToAnimate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the data labels need to be animated.
		/// </summary>
		internal bool NeedToAnimateDataLabel { get; set; }

        /// <summary>
        /// Gets or sets the collection of selected segments.
        /// </summary>
        internal SunburstItems SelectedSunburstItems { get; set; }

        #endregion

        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="Legend"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LegendProperty =
           BindableProperty.Create(
			   nameof(Legend), 
			   typeof(SunburstLegend), 
			   typeof(SfSunburstChart), 
			   null,
               propertyChanged: OnLegendPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
				nameof(ItemsSource), 
				typeof(object), 
				typeof(SfSunburstChart), 
				null,
                propertyChanged: OnItemSourceChanged);

        /// <summary>
        /// Identifies the <see cref="ValueMemberPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ValueMemberPathProperty =
            BindableProperty.Create(
				nameof(ValueMemberPath), 
				typeof(string), 
				typeof(SfSunburstChart), 
				null,
                propertyChanged: OnValuePathChanged);

        /// <summary>
        /// Identifies the <see cref="Levels"/> bindable property.
        /// </summary>
        public static readonly BindableProperty LevelsProperty =
            BindableProperty.Create(
				nameof(Levels), 
				typeof(SunburstLevelCollection), 
				typeof(SfSunburstChart),
                null, 
				propertyChanged: OnLevelChanged);

        /// <summary>
        /// Identifies the <see cref="Title"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TitleProperty =
           BindableProperty.Create(
			   nameof(Title), 
			   typeof(object), 
			   typeof(SfSunburstChart), 
			   null,
               propertyChanged: OnTitlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> bindable property.
        /// </summary>  
        public static readonly BindableProperty PaletteBrushesProperty =
            BindableProperty.Create(
				nameof(PaletteBrushes), 
				typeof(IList<Brush>), 
				typeof(SfSunburstChart),
                null, 
				BindingMode.Default, 
				null,
                propertyChanged: OnPaletteBrushesChanged);

        /// <summary>
        /// Identifies the <see cref="Radius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty RadiusProperty =
            BindableProperty.Create(
				nameof(Radius), 
				typeof(double), 
				typeof(SfSunburstChart), 
				0.9d,
				BindingMode.Default, 
				null, 
				propertyChanged: OnRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InnerRadius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty InnerRadiusProperty =
            BindableProperty.Create(
				nameof(InnerRadius), 
				typeof(double), 
				typeof(SfSunburstChart), 
				0.25d,
                BindingMode.Default, 
				null, 
				propertyChanged: OnInnerRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StartAngleProperty =
            BindableProperty.Create(
				nameof(StartAngle), 
				typeof(double), 
				typeof(SfSunburstChart), 
				0d,
                BindingMode.Default, 
				null, 
				propertyChanged: OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EndAngleProperty =
            BindableProperty.Create(
				nameof(EndAngle), 
				typeof(double), 
				typeof(SfSunburstChart), 
				360d,
                BindingMode.Default, 
				null, 
				propertyChanged: OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(
				nameof(Stroke), 
				typeof(Brush), 
				typeof(SfSunburstChart), 
				null,
                BindingMode.Default, 
				null, 
				propertyChanged: OnStrokeChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(
				nameof(StrokeWidth), 
				typeof(double), 
				typeof(SfSunburstChart), 
				1d,
                BindingMode.Default, 
				null, 
				propertyChanged: OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableAnimationProperty =
            BindableProperty.Create(
				nameof(EnableAnimation), 
				typeof(bool), 
				typeof(SfSunburstChart), 
				false,
                BindingMode.Default, 
				null, 
				propertyChanged: OnEnableAnimationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(
				nameof(AnimationDuration), 
				typeof(double), 
				typeof(SfSunburstChart), 
				1d,
                BindingMode.Default);

        /// <summary>      
        /// Identifies the <see cref="CenterView"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty CenterViewProperty =
            BindableProperty.Create(
				nameof(CenterView), 
				typeof(View), 
				typeof(SfSunburstChart), 
				null,
                BindingMode.Default, 
				null, 
				propertyChanged: OnCenterViewPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TooltipSettings"/> bindable property.
        /// </summary> 
        public static readonly BindableProperty TooltipSettingsProperty =
            BindableProperty.Create(
				nameof(TooltipSettings), 
				typeof(SunburstTooltipSettings), 
				typeof(SfSunburstChart), 
				null,
                propertyChanged: OnTooltipSettingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
        /// </summary>
        public static readonly BindableProperty DataLabelSettingsProperty =
            BindableProperty.Create(
				nameof(DataLabelSettings), 
				typeof(SunburstDataLabelSettings), 
				typeof(SfSunburstChart), 
				null,
                propertyChanged: OnDataLabelSettingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableTooltip"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableTooltipProperty =
            BindableProperty.Create(
				nameof(EnableTooltip), 
				typeof(bool), 
				typeof(SfSunburstChart), 
				false,
                BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> bindable property.
        /// </summary>  
        public static readonly BindableProperty TooltipTemplateProperty =
           BindableProperty.Create(
			   nameof(TooltipTemplate), 
			   typeof(DataTemplate), 
			   typeof(SunburstTooltipSettings), 
			   null,
               BindingMode.Default, 
			   null, 
			   propertyChanged: OnTooltipTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="ShowLabels"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowLabelsProperty =
            BindableProperty.Create(
				nameof(ShowLabels), 
				typeof(bool), 
				typeof(SfSunburstChart), 
				false,
                BindingMode.Default, 
				null, 
				propertyChanged: OnDataLabelPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionSettings"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SelectionSettingsProperty =
            BindableProperty.Create(
				nameof(SelectionSettings), 
				typeof(SunburstSelectionSettings), 
				typeof(SfSunburstChart), 
				null,
                propertyChanged: OnSelectionSettingsPropertyChanged);

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SfSunburstChart"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public SfSunburstChart()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            
            ThemeElement.InitializeThemeResources(this, "SfSunburstChartTheme");
            _titleView = new ChartTitleView();
            area = new SunburstChartArea(this);
            legendLayout = new LegendLayout(area);
            LegendStyle.Parent = this;
            Segments = new SunburstSegmentCollection();
            PaletteBrushes = ChartColorModel.DefaultBrushes;
            Stroke = new SolidColorBrush(Color.FromArgb("#FFFBFE"));
            Levels = new SunburstLevelCollection();
            Segments.CollectionChanged += Segments_CollectionChanged;
            SelectedSunburstItems = new SunburstItems();
            SelectedSunburstItems.CollectionChanged += SelectedSegments_CollectionChanged;
            _actualLevel = new SunburstLevelCollection();
            _innerLevel = new SunburstHierarchicalLevel() { SunburstChart = this };
            DataLabelSettings = new SunburstDataLabelSettings();
            _content = CreateTemplate(legendLayout);
            this.AddTouchListener(this);
            this.AddGestureListener(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a legend that is used to display a legend that helps to identify the parent of each level in the sunburst chart.
        /// </summary>
        /// <value>This property takes a <see cref="SunburstLegend"/> instance as value and its default value is null.</value>
        public SunburstLegend Legend
        {
            get => (SunburstLegend)GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }

        /// <summary>
        /// Gets or sets the start angle for the Sunburst chart.
        /// </summary>
        ///  <value>It accepts double values, and the default value is 0.</value>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// Gets or sets the end angle for the sunburst chart.
        /// </summary>
        /// <value>It accepts double values, and the default value is 360.</value>
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        /// <summary>
        /// Gets or sets the radius property, used for defining the sunburst size.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.9. Here, the value is between 0 and 1.</value>
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the radius property. This property value is used for defining the sunburst size. Value must be specified from 0 to 1.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.25. Here, the value is between 0 and 1.</value>
        public double InnerRadius
        {
            get => (double)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke width property for customizing the width of the sunburst chart segment border.
        /// </summary>
        /// <value>It accepts double values and its default value is 1.</value>
        public double StrokeWidth
        {
            get => (double)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke color property used to define the stroke color for the Sunburst chart.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is White/>.</value>
        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Gets or sets the title for the chart. It supports the string or any view as a title.
        /// </summary>
        /// <value>The default value is null.</value>
        public object Title
        {
            get => (object)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        public object ItemsSource
        {
            get => (object)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label is visible. 
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>False</c>.</value>
        public bool ShowLabels
        {
            get => (bool)GetValue(ShowLabelsProperty);
            set => SetValue(ShowLabelsProperty, value);
        }

        /// <summary>
        /// Gets or sets path of the value data member in the ItemsSource.
        /// </summary>
        /// <value> It accepts string values and its default value is null.</value>
        public string ValueMemberPath
        {
            get => (string)GetValue(ValueMemberPathProperty);
            set => SetValue(ValueMemberPathProperty, value);
        }

        /// <summary>
        /// Gets or sets the levels for the sunburst chart.
        /// </summary>
        public SunburstLevelCollection Levels
        {
            get => (SunburstLevelCollection)GetValue(LevelsProperty);
            set => SetValue(LevelsProperty, value);
        }

        /// <summary>
        /// Gets or sets an IList of brush collection property, used as the color scheme for the sunburst chart.
        /// </summary>
        public IList<Brush> PaletteBrushes
        {
            get => (IList<Brush>)GetValue(PaletteBrushesProperty);
            set => SetValue(PaletteBrushesProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is enabled for the sunburst chart. 
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>False</c>.</value>
        public bool EnableAnimation
        {
            get => (bool)GetValue(EnableAnimationProperty);
            set => SetValue(EnableAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets the tooltip settings property, used to customize the tooltip.
        /// </summary> 
        /// <value>This property takes <see cref="SunburstTooltipSettings"/> instance as value and its default value is null.</value>
        public SunburstTooltipSettings TooltipSettings
        {
            get => (SunburstTooltipSettings)GetValue(TooltipSettingsProperty);
            set => SetValue(TooltipSettingsProperty, value);
        }

        /// <summary>
        /// Gets or sets a DataLabelSettings, used to customize the appearance of the displaying data labels in the sunburst chart. 
        /// </summary>
        /// <value>This property takes <see cref="SunburstDataLabelSettings"/> instance as value and its default value is null.</value>
        public SunburstDataLabelSettings DataLabelSettings
        {
            get => (SunburstDataLabelSettings)GetValue(DataLabelSettingsProperty);
            set => SetValue(DataLabelSettingsProperty, value);
        }

        /// <summary>
        /// Gets or sets the view to be added to the center of the sunburst chart.
        /// </summary>
        /// <value>It accepts any <see cref="View"/> object, and its default value is null.</value> 
        public View CenterView
        {
            get => (View)GetValue(CenterViewProperty);
            set => SetValue(CenterViewProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip is visible when mouse hovers or taps on the segment.
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>False</c>.</value>
        public bool EnableTooltip
        {
            get => (bool)GetValue(EnableTooltipProperty);
            set => SetValue(EnableTooltipProperty, value);
        }

        /// <summary>
        /// Gets or sets the Animation duration property, used to set the duration for the animation to be done.
        /// </summary>
        public double AnimationDuration
        {
            get => (double)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        /// <summary>
        /// Gets the size of the sunburst center hole.
        /// </summary>
        public double CenterHoleSize
        {
            get { return _centerHoleSize; }
            internal set
            {
                if (value >= 1)
                {
                    _centerHoleSize = value;
                }

                OnPropertyChanged(nameof(CenterHoleSize));
            }
        }

        /// <summary>
        /// Gets or sets the data template for the tooltip, which can customize the tooltip labels.
        /// </summary>  
        /// <value> It accepts a <see cref="DataTemplate"/> value.</value>
        public DataTemplate TooltipTemplate
        {
            get => (DataTemplate)GetValue(TooltipTemplateProperty);
            set => SetValue(TooltipTemplateProperty, value);
        }

		/// <summary>
		/// Gets or sets the settings for segment selection in the sunburst chart.
		/// </summary>
		/// <value>
		/// An instance of <see cref="SelectionSettings"/> class.
		/// </value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <sunburst:SfSunburstChart>
		///     <sunburst:SfSunburstChart.SelectionSettings>
		///         <sunburst:SunburstSelectionSettings 
		///             Type="Child" 
		///             SunburstSelectionDisplayMode="HighlightByColor"
		///             Fill="Blue" />
		///     </sunburst:SfSunburstChart.SelectionSettings>
		/// </sunburst:SfSunburstChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfSunburstChart sunburstChart = new SfSunburstChart();
		/// 
		/// sunburstChart.SelectionSettings = new SunburstSelectionSettings
		/// {
		///     Type = Type = SunburstSelectionType.Child,
		///     DisplayMode = SunburstSelectionDisplayMode.HighlightByColor,
		///     Fill = new SolidColorBrush(Colors.Blue)
		/// }; 
		/// ]]>
		/// </code>
		/// </example>
		public SunburstSelectionSettings SelectionSettings
        {
            get => (SunburstSelectionSettings)GetValue(SelectionSettingsProperty);
            set => SetValue(SelectionSettingsProperty, value);
        }

        /// <summary>
        /// Occurs when a segment is selected or deselected in the sunburst chart.
        /// </summary>
        /// <remarks>
        /// This event is triggered when a segment is selected or deselected.
        /// The event arguments provide information about which segment was affected and whether it was selected or deselected.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// sunburstChart.SelectionChanged += OnSelectionChanged;
        /// 
        /// private void OnSelectionChanged(object sender, SunburstSelectionChangedEventArgs args)
        /// {
        ///     var newSegment = args.NewSegment;
        ///     var oldSegment = args.OldSegment;
        ///     bool isSelected = args.IsSelected;
        ///     // Handle the selection change
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public event EventHandler<SunburstSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Occurs before a segment selection changes.
        /// </summary>
        /// <remarks>
        /// <para>The SelectionChanging event is raised before a segment selection is applied. This event provides an opportunity to cancel the selection change by setting the Cancel property to <c>true</c>.</para>
        /// 
        /// <para>This event provides information about both the previously selected segment and the new segment being selected.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// sunburstChart.SelectionChanging += OnSelectionChanging;
        /// 
        /// private void OnSelectionChanging(object sender, SunburstSelectionChangingEventArgs args)
        /// {
        ///     var oldSegment = args.OldSegment;
        ///     var newSegment = args.NewSegment;
        ///     // Cancel selection change, if needed.
        ///     args.Cancel = true;
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public event EventHandler<SunburstSelectionChangingEventArgs> SelectionChanging;

        /// <summary>
        /// Gets or sets the copy of <see cref="ItemsSource"/>
        /// </summary>
        internal IEnumerable? InternalDataSource
        {
            get
            {
                return _internalDataSource;
            }

            set
            {
                if (_internalDataSource != value)
                {
                    _internalDataSource = value;
                    GenerateSunburstItems();
                    ScheduleUpdate();
                }
            }
        }

        internal double OuterRadius
        {
            get { return _outerRadius; }
            set { _outerRadius = value; }
        }

        internal SfTooltip? TooltipView { get; set; }

        internal SunburstTooltipSettings ActualTooltipSettings
        {
            get
            {
                if (TooltipSettings == null)
                {
                    TooltipSettings = new SunburstTooltipSettings();
                    return TooltipSettings;
                }

                return TooltipSettings;
            }
        }

        #endregion

        #region Override methods
        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return this.MeasureContent(widthConstraint, heightConstraint);
        }

        Size IContentView.CrossPlatformArrange(Rect bounds)
        {
            this.ArrangeContent(bounds);
            return bounds.Size;
        }

        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnTapAction(this, e.TapPoint, e.TapCount);
        }

        /// <inheritdoc/>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            Point point = e.TouchPoint;
            switch (e.Action)
            {
                case PointerActions.Moved:
                    OnTouchMove(point, e.PointerDeviceType);
                    break;
            }
        }

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Title != null && Title is View view)
            {
                SetInheritedBindingContext(view, BindingContext);
            }

            if (TooltipSettings != null)
            {
                SetInheritedBindingContext(TooltipSettings, BindingContext);
            }

            if (SelectionSettings != null)
            {
                SetInheritedBindingContext(SelectionSettings, BindingContext);
            }

            if (DataLabelSettings != null)
            {
                SetInheritedBindingContext(DataLabelSettings, BindingContext);
            }

            if (_content != null)
            {
                SetInheritedBindingContext(_content, BindingContext);
            }

            if (CenterView != null)
            {
                SetInheritedBindingContext(CenterView, BindingContext);
            }
        }


		#endregion

		#region Internal Methods

		/// <summary>
		/// Generates the segments for the sunburst chart.
		/// </summary>
		internal void GenerateSegments()
        {
            if (Levels == null)
            {
                return;
            }

            ClearSegments();
            CreateSegments();
        }

		/// <summary>
		/// Determines whether the chart can be animated.
		/// </summary>
		/// <returns>True if animation is enabled and needed; otherwise, false.</returns>
		internal bool CanAnimate()
        {
            return EnableAnimation && NeedToAnimate;
        }

		/// <summary>
		/// Schedules an update for the chart area.
		/// </summary>
		internal void ScheduleUpdate()
        {
            var area = this.area;
            if (area != null)
            {
                area.NeedsRelayout = true;
                area.ScheduleUpdateArea();
            }
        }

		/// <summary>
		/// Calculates the radius and center of the sunburst chart.
		/// </summary>
		internal void GetRadius()
        {
            if (SeriesRenderBounds != Rect.Zero)
            {
                if (Levels != null)
                {
                    Center = new Point((int)(SeriesRenderBounds.Left + (SeriesRenderBounds.Width / 2)), (int)(SeriesRenderBounds.Top + (SeriesRenderBounds.Height / 2)));
                    LevelsCount = Levels.Count;
                    OuterRadius = Math.Abs(Radius * Math.Min(SeriesRenderBounds.Width, SeriesRenderBounds.Height));
                    CenterHoleSize = Math.Abs(InnerRadius * Math.Min(SeriesRenderBounds.Width, SeriesRenderBounds.Height));

                    var size = OuterRadius - CenterHoleSize;
					RingSize = size / LevelsCount; // Size has to be calculated based on the current levels count.
				}
            }
        }

        internal void AddCenterView()
        {
            if (area != null && CenterView != null && !area.Children.Contains(CenterView))
            {
                CenterView.BindingContext = this;
                area.Insert(2, CenterView);
                AbsoluteLayout.SetLayoutBounds(CenterView, new Rect(0.5, 0.5, -1, -1));
                AbsoluteLayout.SetLayoutFlags(CenterView, AbsoluteLayoutFlags.PositionProportional);
            }
        }

		/// <summary>
		/// Creates a new sunburst segment.
		/// </summary>
		/// <returns>A new <see cref="SunburstSegment"/> instance.</returns>
		internal SunburstSegment CreateSegment()
        {
            return new SunburstSegment();
        }

        /// <summary>
        /// Create the segments based upon the grouped items.
        /// </summary>
        internal void CreateSegments()
        {
            var index = -1;
            var segmentIndex = 0;

            if (Levels.Count > 0)
            {
                foreach (var level in Levels.Where(level => level.SunburstItems != null))
                {
                    index++;
                    if (level.SunburstItems != null)
                    {
                        foreach (var item in level.SunburstItems.Where(c => !string.IsNullOrEmpty(c.Key) && c.KeyValue != 0))
                        {
                            SunburstSegment segment = CreateSegment();
                            segment.SunburstItems = item;
                            segment.IsSelected = SelectedSunburstItems.Contains(item);
                            segment.CurrentLevel = index;
                            segment.Index = item.SliceIndex;

                            segment.ArcStartAngle = item.ArcStart;
                            segment.ArcEndAngle = item.ArcEnd;
                            var items = new List<object?>();
                            items.Add(item.Key?.ToString() ?? string.Empty);
                            items.Add(item.KeyValue);
                            segment.Item = items;

                            Segments.Add(segment);
                            ++segmentIndex;
                        }
                    }
                }

                FindSegmentsChilds();
            }
        }

		/// <summary>
		/// Updates the stroke color for all segments in the chart.
		/// Applies the current <see cref="Stroke"/> value to each segment.
		/// </summary>
		private void UpdateStrokeColor()
        {
            foreach (var segment in Segments)
            {
                segment.Stroke = Stroke;
            }
        }

		/// <summary>
		/// Updates the stroke width for all segments in the chart.
		/// Converts the current <see cref="StrokeWidth"/> to float and applies it to each segment.
		/// </summary>
		private void UpdateStrokeWidth()
        {
            foreach (var segment in Segments)
            {
                segment.StrokeWidth = (float)StrokeWidth;
            }
        }

        /// <summary>
        /// Group the data source based upon the group member path.
        /// </summary>
        internal void GenerateSunburstItems()
        {
            ClearExistingCollections();

            if (Levels == null || InternalDataSource == null || !InternalDataSource.GetEnumerator().MoveNext() || string.IsNullOrEmpty(ValueMemberPath))
            {
                return;
            }

            _innerLevel = new SunburstHierarchicalLevel() { CurrentActualData = InternalDataSource, SunburstChart = this };

			foreach (var level in Levels)
            {
                if (level.GroupingPath != null)
                {
                    if (_groupItems != null && _groupItems.Count > 0)
                    {
                        List<SunburstItem>? cloneItems;
                        (cloneItems = _groupItems.ToList()).Clear();

						foreach (var item in _groupItems)
                        {
                            _innerLevel = new SunburstHierarchicalLevel() { CurrentActualData = item.Values, SunburstChart = this };
                            var subItems = _innerLevel.GenerateItem(level.GroupingPath?.ToString() ?? string.Empty);
                            if (subItems != null)
                            {
                                _innerLevel.GetKeyValue(subItems, ValueMemberPath);
                                item.ChildItems = subItems;
                                _innerLevel.GetSliceInfo(subItems, item.ArcStart, item.ArcEnd, item.SliceIndex);
                                if (subItems.Count > 0)
                                {
                                    foreach (var subItem in subItems)
                                    {
                                        if (subItem.Key != null && !string.IsNullOrEmpty(subItem.Key) && (subItem.ArcEnd - subItem.ArcStart) > 0)
                                        {
                                            cloneItems.Add(subItem);
                                        }
                                    }
                                }
                            }
                        }

                       level.SunburstItems = _groupItems = cloneItems;
                       level.SunburstChart = this;
                    }
                    else
                    {
                        _groupItems = _innerLevel.GenerateItem(level.GroupingPath);
                        _innerLevel.GetKeyValue(_groupItems, ValueMemberPath);
                        level.SunburstItems = _groupItems;
                        var arcStartAngle = SunburstChartUtils.DegreeToRadianConverter(StartAngle);
                        var arcEndAngle = SunburstChartUtils.DegreeToRadianConverter(EndAngle);
                        _innerLevel.GetSliceInfo(_groupItems, arcStartAngle, arcEndAngle, -1);
                    }
                }
            }

            area.ShouldPopulateLegendItems = true;
        }

        internal Brush? GetFillColor(int index)
        {
            Brush fillColor = Brush.Transparent;

            if (PaletteBrushes != null && PaletteBrushes.Count > 0)
            {
                fillColor = PaletteBrushes[index % PaletteBrushes.Count];
            }

            return fillColor;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Finds the segments relation.
        /// </summary>
        private void FindSegmentsChilds()
        {
            if (StartAngle < EndAngle)
            {
                foreach (var segment in Segments)
                {
                    segment.Childs = (from seg in Segments.Where(s => s.CurrentLevel >= segment.CurrentLevel)
                                      where seg.ArcStartAngle >= segment.ArcStartAngle
                                      && Math.Round(seg.ArcEndAngle, 2) <= Math.Round(segment.ArcEndAngle, 2)
                                      select seg).ToList();
                    segment.HasChild = segment.Childs.Any();

                    segment.Parent = (from seg in Segments.Where(s => s.CurrentLevel == segment.CurrentLevel - 1)
                                      where segment.ArcStartAngle >= seg.ArcStartAngle
                                      && Math.Round(segment.ArcEndAngle, 2) <= Math.Round(seg.ArcEndAngle, 2)
                                      select seg).FirstOrDefault();
                    segment.HasParent = segment.Parent != null;
                }
            }
            else
            {
                foreach (var segment in Segments)
                {
                    segment.Childs = (from seg in Segments.Where(s => s.CurrentLevel >= segment.CurrentLevel)
                                      where seg.ArcStartAngle <= segment.ArcStartAngle
                                      && Math.Round(seg.ArcEndAngle, 2) >= Math.Round(segment.ArcEndAngle, 2)
                                      select seg).ToList();
                    segment.HasChild = segment.Childs.Any();

                    segment.Parent = (from seg in Segments.Where(s => s.CurrentLevel == segment.CurrentLevel - 1)
                                      where segment.ArcStartAngle <= seg.ArcStartAngle
                                      && Math.Round(segment.ArcEndAngle, 2) >= Math.Round(seg.ArcEndAngle, 2)
                                      select seg).FirstOrDefault();
                    segment.HasParent = segment.Parent != null;
                }
            }
        }

        private View CreateTemplate(LegendLayout legendLayout)
        {
            Grid grid = new Grid();
            grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Auto });
            grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Star });
            Grid.SetRow(_titleView, 0);
            grid.Add(_titleView);
            Grid.SetRow(legendLayout, 1);
            grid.Add(legendLayout);
            grid.Parent = this;

            return grid;
        }

        private void SetFillColor(SunburstSegment segment)
        {
            segment.Fill = SelectionSettings != null && segment.IsSelected &&
                           SelectionSettings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByBrush) && SelectionSettings.Fill != null
                           ? SelectionSettings.Fill
                           : GetFillColor(segment.Index) ?? Brush.Transparent;
        }

        internal void SetOpacity(SunburstSegment segment)
        {
            if (SelectionSettings == null || !SelectionSettings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByOpacity))
            {
                segment.Opacity = 1.0f;
                return;
            }

            segment.Opacity = segment.IsSelected ? 1.0f : (float)SelectionSettings.Opacity;
        }

        void SetStroke(SunburstSegment segment)
        {
            if (SelectionSettings != null && segment.IsSelected &&
                                      SelectionSettings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByStroke) &&
                                      SelectionSettings.Stroke != null && SelectionSettings.StrokeWidth > 0)
            {
                segment.Stroke = SelectionSettings.Stroke;
                segment.StrokeWidth = (float)SelectionSettings.StrokeWidth;
            }
            else
            {
                segment.Stroke = Stroke;
                segment.StrokeWidth = (float)StrokeWidth;
            }
        }

        /// <summary>
        /// Clears the existing collections.
        /// </summary>
        private void ClearExistingCollections()
        {
            if (Levels != null)
            {
                foreach (var level in Levels.Where(level => level.SunburstItems != null))
                {
                    level.SunburstItems?.Clear();
                }

                // Levels.Clear();
            }

            _groupItems?.Clear();
        }

        private void Segments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.Apply((obj, index, isTrue) => AddSegment(obj), (obj, index) => RemoveSegment(obj), ResetSegment);
        }

        private void AddSegment(object chartSegment)
        {
            if (chartSegment is SunburstSegment segment)
            {
                segment.Chart = this;
                SetFillColor(segment);
                SetStroke(segment);
                SetOpacity(segment);
            }
        }

        private void RemoveSegment(object chartSegment)
        {
            //Todo: Need to consider this case later.
        }

        private void ResetSegment()
        {
            //Todo: Need to consider this case later.
        }

        private void OnBindingPathChanged()
        {

        }

        /// <summary>
        /// Clears the unused segments.
        /// </summary>
        private void ClearSegments()
        {
            Segments?.Clear();
        }

        private void SelectedSegments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.Apply((obj, index, isTrue) => AddSelectedSegment(obj), (obj, index) => RemoveSelectedSegment(obj), ResetSelectedSegment);
        }

        private void AddSelectedSegment(object sunburstItem)
        {
            if (sunburstItem is not SunburstItem item || GetSelectedSegment(item) is not SunburstSegment segment)
                return;

            segment.IsSelected = true;
            bool isRootSegment = segment.Parent == null;
            SetFillColor(segment);
            SetStroke(segment);
            SetOpacity(segment);

            // Apply legend selection for root segments only
            if (isRootSegment)
            {
                UpdateLegendItems(segment, SelectionSettings);
            }
        }

        private void RemoveSelectedSegment(object sunburstItem)
        {
            if (sunburstItem is not SunburstItem item || GetSelectedSegment(item) is not SunburstSegment segment)
                return;

            var settings = SelectionSettings;
            segment.IsSelected = false;
            SetFillColor(segment);
            SetStroke(segment);
            SetOpacity(segment);

            if (segment.Parent == null)
                UpdateLegendItems(segment, settings);
        }


        /// <summary>
        /// Common method to update legend items for both selection and deselection
        /// </summary>
        /// <param name="segment">The segment being processed</param>
        /// <param name="settings">Selection settings</param>
        private void UpdateLegendItems(SunburstSegment segment, SunburstSelectionSettings settings)
        {
            if (Legend?.IsVisible != true || area?.legendItems?.Count == 0)
                return;

            bool useBrush = settings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByBrush);
            bool useOpacity = settings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByOpacity);

            if (!useBrush && !useOpacity)
                return;

            if (useBrush)
            {
                // Reset only the specific legend item 
                ResetSpecificLegendItem(segment);
            }

            if (useOpacity)
            {
                if (!segment.IsSelected)
                    // Reset all legend items to full opacity (original RemoveSelectedSegment logic)
                    ResetAllLegendItems();
                else
                    UpdateAllLegendItems();
            }
        }

        /// <summary>
        /// Updates all legend items for selection mode
        /// </summary>
        private void UpdateAllLegendItems()
        {
            float opacity = (float)SelectionSettings.Opacity;

            if (!SelectionSettings.DisplayMode.HasFlag(SunburstSelectionDisplayMode.HighlightByOpacity) || SelectedSunburstItems.Count <= 0) return;

            foreach (var item in area.legendItems.OfType<LegendItem>())
            {
                var originalBrush = GetFillColor(item.Index) ?? Brush.Transparent;
                bool isSelectedItem = item.Index == SelectedSunburstItems[0].SliceIndex;

                if (isSelectedItem)
                {
                    item.IconBrush = originalBrush;
                    SetLegendItemTextOpacity(item, 1.0f);
                }
                else
                {
                    item.IconBrush = ApplyOpacityToBrush(originalBrush, opacity);
                    SetLegendItemTextOpacity(item, opacity);
                }
            }
        }

        /// <summary>
        /// Resets a specific legend item (used in deselection for brush mode)
        /// </summary>
        private void ResetSpecificLegendItem(SunburstSegment segment)
        {
            var item = area.legendItems.FirstOrDefault(sel => sel.Index.Equals(segment.Index));
            if (item is LegendItem legendItem)
            {
                legendItem.IconBrush = !segment.IsSelected ? GetFillColor(legendItem.Index) ?? Brush.Transparent : SelectionSettings.Fill;
                var currentColor = legendItem.TextColor;
                legendItem.TextColor = Color.FromRgba(
                    currentColor.Red,
                    currentColor.Green,
                    currentColor.Blue,
                    1.0f);
            }
        }

        /// <summary>
        /// Resets all legend items to full opacity (used in deselection for opacity mode)
        /// </summary>
        private void ResetAllLegendItems()
        {
            foreach (var item in area.legendItems.OfType<LegendItem>())
            {
                item.IconBrush = GetFillColor(item.Index) ?? Brush.Transparent;
                var currentColor = item.TextColor;
                item.TextColor = Color.FromRgba(
                    currentColor.Red,
                    currentColor.Green,
                    currentColor.Blue,
                    1.0f);
            }
        }

        /// <summary>
        /// Sets the text opacity for a legend item by modifying its TextColor.
        /// </summary>
        /// <param name="legendItem">The legend item to modify.</param>
        /// <param name="opacity">The opacity value to apply (0.0 to 1.0).</param>
        private void SetLegendItemTextOpacity(LegendItem legendItem, float opacity)
        {
            if (legendItem == null)
                return;

            var legendItemInterface = (ILegendItem)legendItem;
            Color currentColor = legendItem.TextColor;
            Color newColor = Color.FromRgba(
                currentColor.Red,
                currentColor.Green,
                currentColor.Blue,
                opacity
            );
            legendItemInterface.TextColor = newColor;
        }

        /// <summary>
        /// Applies opacity to a brush by creating a new brush with modified alpha
        /// </summary>
        private Brush ApplyOpacityToBrush(Brush brush, double opacity)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                Color color = solidBrush.Color;
                return new SolidColorBrush(
                    Color.FromRgba(
                        color.Red,
                        color.Green,
                        color.Blue,
                        (float)opacity
                    )
                );
            }
            return brush;
        }

        private void ResetSelectedSegment()
        {

        }

        /// <summary>
        /// On the angle changed.
        /// </summary>
        private void OnAngleChanged()
        {
            if (Levels == null || Levels.Count == 0 || InternalDataSource == null)
            {
                ClearSegments();
                return;
            }

            for (int i = 0; i < Levels.Count; i++)
            {
                if (this.ZoomingLevel <= i && Levels[i].GroupMemberPath != null)
                {
                    var prevItems = i - 1 == -1 ? null : Levels[i - 1].SunburstItems;

                    if (prevItems != null && prevItems.Any())
                    {
                        foreach (var item in prevItems)
                        {
                            _innerLevel.GetSliceInfo(item.ChildItems, item.ArcStart, item.ArcEnd, item.SliceIndex);
                        }
                    }
                    else
                    {
                        var arcStartAngle = SunburstChartUtils.DegreeToRadianConverter(StartAngle);
                        var arcEndAngle = SunburstChartUtils.DegreeToRadianConverter(EndAngle);
                        _innerLevel.GetSliceInfo(Levels[i].SunburstItems, arcStartAngle, arcEndAngle, -1);
                    }
                }
            }

            ClearSegments();
            CreateSegments();
        }

        private void OnLevelPropertyChanged(object oldValue, object newValue)
        {
            if (oldValue is SunburstLevelCollection oldCollection)
            {
                oldCollection.CollectionChanged -= Level_CollectionChanged;
            }

            if (newValue is SunburstLevelCollection newCollection)
            {
                newCollection.CollectionChanged += Level_CollectionChanged;
                newCollection.SunburstChart = this;
                _actualLevel = new SunburstLevelCollection();

                foreach (var level in newCollection)
                {
                    level.BindingContext = this.BindingContext;
                    Levels.Add(level);
                }
            }

        }

        private void Level_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.Apply((obj, index, isTrue) => AddLevel(obj), (obj, index) => RemoveLevel(obj), ResetLevel);

            if (Levels != null)
            {
                foreach (SunburstHierarchicalLevel level in this.Levels)
                {
                    SetInheritedBindingContext(level, this.BindingContext);
                    level.SunburstChart = this;
                    GenerateSunburstItems();
                    ScheduleUpdate();
                }
            }
        }

        private void ResetLevel()
        {
            _actualLevel.Clear();
        }

        private void RemoveLevel(object obj)
        {
            if (obj is SunburstHierarchicalLevel level && _actualLevel.Contains(level))
                _actualLevel.Remove(level);
        }

        private void AddLevel(object obj)
        {
            if (obj is SunburstHierarchicalLevel level && !_actualLevel.Contains(level))
                _actualLevel.Add(level);
        }

        private SunburstSegment? GetSelectedSegment(float pointX, float pointY)
        {
            double adjustedX = pointX - ActualSeriesClipRect.Left;
            double adjustedY = pointY - ActualSeriesClipRect.Top;

            foreach (var segment in Segments)
            {
                if (segment.IsPointInSunburstSegment(adjustedX, adjustedY))
                {
                    return segment;
                }
            }

            return null;
        }

        private void OnTouchMove(Point point, PointerDeviceType deviceType)
        {
            if (deviceType == PointerDeviceType.Mouse && !NeedToAnimate)
            {
                Show((float)point.X, (float)point.Y, true);
            }
        }

        #endregion

        #region private static methods

        private static void OnAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.OnAngleChanged();
                chart.ScheduleUpdate();
            }
        }

        private static void OnInnerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.ScheduleUpdate();
            }
        }

        private static void OnTooltipTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        private static void OnRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.ScheduleUpdate();
            }
        }

        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart && chart._titleView != null)
            {
                if (newValue != null)
                {
                    if (newValue is View view)
                    {
                        chart._titleView.Content = view;
                    }
                    else
                    {
                        chart._titleView.InitTitle(newValue.ToString());
                    }
                }
            }

        }

        private static void OnLegendPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                var layout = chart.legendLayout;
                if (layout != null && newValue is ILegend legend)
                {
                    layout.Legend = legend;
                    layout._areaBase.ScheduleUpdateArea();
                }
            }
        }

        private static void OnEnableAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        private static void OnLevelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.OnLevelPropertyChanged(oldValue, newValue);
                if (chart.InternalDataSource != null)
                {
                    chart.GenerateSunburstItems();
                    chart.ScheduleUpdate();
                }
            }
        }

        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.UpdateStrokeWidth();
                chart.ScheduleUpdate();
            }
        }

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.UpdateStrokeColor();
                chart.ScheduleUpdate();
            }
        }

        private static void OnValuePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                if (newValue != null)
                {
                    chart.ValueMemberPath = (string)newValue;
                    chart.GenerateSunburstItems();
                    chart.ScheduleUpdate();
                }

                chart.OnBindingPathChanged();
            }
        }

        private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart && newValue is IEnumerable source)
            {
                chart.InternalDataSource = source;
                chart.NeedToAnimate = chart.EnableAnimation;
                chart.HookAndUnhookCollectionChangedEvent(oldValue, newValue);
                chart.GenerateSunburstItems();
                chart.ScheduleUpdate();
            }
        }

        private static void OnTooltipSettingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart sunburstChart)
            {
                if (oldValue is SunburstTooltipSettings oldTooltipSettings)
                {
                    oldTooltipSettings.Parent = null;
                    SetInheritedBindingContext(oldTooltipSettings, null);
                }

                if (newValue is SunburstTooltipSettings newTooltipSettings)
                {
                    newTooltipSettings.Parent = sunburstChart;
                    SetInheritedBindingContext(newTooltipSettings, sunburstChart.BindingContext);
                }
            }
        }

        private void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
        {
            if (newValue != null)
            {
                var newCollectionValue = newValue as INotifyCollectionChanged;
                if (newCollectionValue != null)
                {
                    newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
                }
            }

            if (oldValue != null)
            {
                var oldCollectionValue = oldValue as INotifyCollectionChanged;
                if (oldCollectionValue != null)
                {
                    oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
                }
            }
        }

        private void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.Apply((obj, index, isTrue) => AddDataPoint(index, obj, e), (obj, index) => RemoveData(index, e), ResetDataPoint);
            area.ShouldPopulateLegendItems = true;
        }

        private void AddDataPoint(int index, object data, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && EnableAnimation && AnimationDuration > 0)
            {
                // Animate sunburst chart
                NeedToAnimate = true;
            }

            GenerateSunburstItems();
            ScheduleUpdate();
        }

        private void ResetDataPoint()
        {
            GenerateSunburstItems();
            ScheduleUpdate();
        }

        private void RemoveData(int index, NotifyCollectionChangedEventArgs e)
        {
            GenerateSunburstItems();
            ScheduleUpdate();
        }

        private static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (bindable is SfSunburstChart chart)
            {
                chart.OnCustomBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);
                chart.ScheduleUpdate();
            }
        }

        private static void OnCenterViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart sunburstChart)
            {
                if (oldValue is View oldView)
                {
                    sunburstChart.RemoveCenterView(oldView);
                }

                if (newValue is View)
                {
                    sunburstChart.AddCenterView();
                }
            }
        }

        private void CustomBrushes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ScheduleUpdate();
        }

        private void OnCustomBrushesChanged(ObservableCollection<Brush>? oldValue, ObservableCollection<Brush>? newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= CustomBrushes_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += CustomBrushes_CollectionChanged;
            }
        }

        private static void OnDataLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart sunburstChart)
            {
                sunburstChart.ScheduleUpdate();
            }
        }

        private static void OnDataLabelSettingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.OnDataLabelPropertiesChanged(oldValue as SunburstDataLabelSettings, newValue as SunburstDataLabelSettings);
            }
        }

        private static void OnSelectionSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfSunburstChart chart)
            {
                chart.OnSelectionSettingsPropertyChanged(oldValue as SunburstSelectionSettings, newValue as SunburstSelectionSettings);
            }
        }

        private void OnSelectionSettingsPropertyChanged(SunburstSelectionSettings? oldValue, SunburstSelectionSettings? newValue)
        {
            if (oldValue != null)
            {
                SetInheritedBindingContext(oldValue, null);
                oldValue.PropertyChanged -= SelectionSettings_PropertyChanged;
            }

            if (newValue != null)
            {
                SetInheritedBindingContext(newValue, BindingContext);
                newValue.PropertyChanged += SelectionSettings_PropertyChanged;
            }

            ClearSelection();
            SelectionInvalidate();
        }

        private void SelectionSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is SunburstSelectionSettings settings)
            {
                if (SelectedSunburstItems.Count > 0)
                {
                    if (e.PropertyName == SunburstSelectionSettings.DisplayModeProperty.PropertyName ||
                       e.PropertyName == SunburstSelectionSettings.TypeProperty.PropertyName)
                        // For selection type or display mode changes, clear the selection
                        ClearSelection();

                    foreach (var sunburstItem in SelectedSunburstItems)
                    {
                        if (sunburstItem is not SunburstItem item || GetSelectedSegment(item) is not SunburstSegment segment)
                            return;

                        SetFillColor(segment);
                        SetStroke(segment);

                        if (!segment.HasParent)
                            UpdateAllLegendItems();
                    }
                }

                ApplyHighlightByOpacity();
                SelectionInvalidate();
            }
        }

        private void OnDataLabelPropertiesChanged(SunburstDataLabelSettings? oldValue, SunburstDataLabelSettings? newValue)
        {
            if (oldValue != null)
            {
                SetInheritedBindingContext(oldValue, null);
                oldValue.PropertyChanged -= DataLabelSettings_PropertyChanged;
            }

            if (newValue != null)
            {
                SetInheritedBindingContext(newValue, BindingContext);
                newValue.PropertyChanged += DataLabelSettings_PropertyChanged;
            }

            if (SeriesRenderBounds != Rect.Zero)
            {
                ScheduleUpdate();
            }
        }

        private void DataLabelSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ScheduleUpdate();
        }

        private void RemoveCenterView(View oldView)
        {
            if (area.Children.Contains(oldView))
            {
                oldView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                oldView.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                BindableObject.SetInheritedBindingContext(oldView, null);
                area.Children.Remove(oldView);
            }
        }

        internal void InvalidateDataLabel()
        {
            area.DataLabelView.InvalidateDrawable();
        }

        #endregion

        #region TooltipHelper

        #region Field
        private SunburstSegment? previousSegmentInfo = null;
        #endregion

        #region Internal Methods

        /// <summary>
        /// Method used to show tooltip view at nearest datapoint for given x and y value.
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="canAnimate"></param>
        private void Show(float pointX, float pointY, bool canAnimate)
        {
            GenerateTooltip(pointX, pointY, canAnimate);
        }

		/// <summary>
		/// Hides the tooltip view and clears any previously stored segment information.
		/// </summary>
		internal void Hide()
        {
            previousSegmentInfo = null;
            TooltipView?.Hide(false);
        }

        #endregion

        #region Private Methods

        private void OnTapAction(SfSunburstChart sunburstChart, Point tapPoint, int tapCount)
        {
            Hide();

            if (tapCount == 1 && !NeedToAnimate)
            {
                Show((float)tapPoint.X, (float)tapPoint.Y, true);

                if (SelectionSettings != null)
                {
                    var tappedSegment = GetSelectedSegment((float)tapPoint.X, (float)tapPoint.Y);

                    if (tappedSegment != null)
                    {
                        SunburstSegment? oldSegment = GetSelectedSegment(SelectedSunburstItems.Count > 0 ? SelectedSunburstItems[0] : null);

                        // If same segment is tapped again, always deselect it (toggle behavior).
                        if (IsSegmentSelected(tappedSegment))
                        {
                            // Raise SelectionChanging event for deselection and check if it's canceled
                            if (RaiseSelectionChanging(oldSegment, tappedSegment))
                            {
                                ClearSelection();
                                RaiseSelectionChanged(oldSegment, tappedSegment, false);
                            }
                        }
                        else
                        {
                            // Raise SelectionChanging event and check if it's canceled
                            if (!RaiseSelectionChanging(oldSegment, tappedSegment))
                            {
                                // Selection was canceled, do not proceed
                                return;
                            }

                            ClearSelection();
                            SelectSegments(tappedSegment);
                            RaiseSelectionChanged(oldSegment, tappedSegment, true);
                            ApplyHighlightByOpacity();
                        }

                        SelectionInvalidate();
                    }
                }
            }
        }

        void ApplyHighlightByOpacity()
        {
            if (SelectedSunburstItems.Count > 0)
            {
                //Set opacity when highlight by opacity. 
                foreach (var segment in Segments)
                {
                    if (!segment.IsSelected)
                        SetOpacity(segment);
                }
            }
        }

        private SunburstSegment? GetSelectedSegment(SunburstItem? sunburstItem)
        {
            if (sunburstItem != null)
            {
                foreach (var segment in Segments)
                {
                    if (segment.SunburstItems == sunburstItem)
                        return segment;
                }
            }

            return null;
        }

        private void SelectSegments(SunburstSegment tappedSegment)
        {
            var settings = SelectionSettings;

            if (tappedSegment.SunburstItems is not SunburstItem selectedItem) return;

            if (!SelectedSunburstItems.Contains(selectedItem))
                SelectedSunburstItems.Add(selectedItem);

            switch (settings.Type)
            {
                case SunburstSelectionType.Child:
                    if (selectedItem.ChildItems != null)
                    {
                        foreach (var child in selectedItem.ChildItems)
                        {
                            if (!SelectedSunburstItems.Contains(child))
                                SelectedSunburstItems.Add(child);

                            if (child.ChildItems != null)
                            {
                                foreach (var subChild in child.ChildItems)
                                    if (!SelectedSunburstItems.Contains(subChild))
                                        SelectedSunburstItems.Add(subChild);
                            }
                        }
                    }
                    break;

                case SunburstSelectionType.Group:

                    var groupSegments = Segments.Where(seg => seg.Index == tappedSegment.Index);

                    foreach (var segment in groupSegments)
                    {
                        if (segment.SunburstItems is not SunburstItem item) continue;
                        if (!SelectedSunburstItems.Contains(item))
                            SelectedSunburstItems.Add(item);
                    }

                    break;

                case SunburstSelectionType.Parent:

                    var currentSegment = tappedSegment;

                    while (currentSegment.Parent != null)
                    {
                        if (currentSegment.Parent.SunburstItems is not SunburstItem item) continue;

                        if (!SelectedSunburstItems.Contains(item))
                            SelectedSunburstItems.Add(item);

                        currentSegment = currentSegment.Parent;
                    }
                    break;
            }
        }

        private void SelectionInvalidate()
        {
            if (area != null)
            {
                area.SeriesView?.InvalidateDrawable();
                area.DataLabelView?.InvalidateDrawable();
            }
        }

        private void ClearSelection()
        {
            if (SelectedSunburstItems == null || SelectedSunburstItems.Count == 0)
                return;

            var sunburstItems = SelectedSunburstItems.ToList();

            foreach (var item in sunburstItems)
            {
                SelectedSunburstItems.Remove(item);
            }

            ResetOpacity(); //Resetting opacity when Highlight by opacity.
            ResetAllLegendItems(); //Resetting legend.
        }

        private void ResetOpacity()
        {
            if (SelectedSunburstItems.Count == 0)
            {
                foreach (var segment in Segments)
                {
                    segment.Opacity = 1.0f;
                }
            }
        }
        private bool IsSegmentSelected(SunburstSegment segment)
        {
            if (segment.SunburstItems == null) return false;

            return segment.IsSelected || SelectedSunburstItems.Contains(segment.SunburstItems);
        }

        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        /// <param name="oldSegment">The previously selected segment.</param>
        /// <param name="newSegment">The newly selected segment.</param>
        /// <param name="isSelected">Whether the segment was selected (true) or deselected (false).</param>
        private void RaiseSelectionChanged(SunburstSegment? oldSegment, SunburstSegment newSegment, bool isSelected)
        {
            SelectionChanged?.Invoke(this, new SunburstSelectionChangedEventArgs(oldSegment, newSegment, isSelected));
        }

        /// <summary>
        /// Raises the SelectionChanging event.
        /// </summary>
        /// <param name="oldSegment">The previously selected segment.</param>
        /// <param name="newSegment">The segment being selected.</param>
        /// <returns>True if the selection change should proceed; false if it was canceled.</returns>
        private bool RaiseSelectionChanging(SunburstSegment? oldSegment, SunburstSegment newSegment)
        {
            if (SelectionChanging != null)
            {
                var args = new SunburstSelectionChangingEventArgs(oldSegment, newSegment);
                SelectionChanging.Invoke(this, args);
                return !args.Cancel;
            }

            // If no event handler, proceed
            return true;
        }

        #endregion

        private void GenerateTooltip(float x, float y, bool canAnimate)
        {
            Rect seriesBounds = ActualSeriesClipRect;
            var settings = ActualTooltipSettings;

            if (seriesBounds.Contains(x, y))
            {
                SunburstSegment? segmentInfo = GetTooltip(x, y);

                if (segmentInfo != null)
                {
                    if (TooltipView is not SfTooltip tooltip)
                    {
                        tooltip = new SfTooltip();
                        TooltipView = tooltip;
                        tooltip.TooltipClosed += Tooltip_TooltipClosed;
                        BehaviorLayout?.Add(TooltipView);
                    }

                    SetTooltipPosition(segmentInfo);

                    if (previousSegmentInfo != null && previousSegmentInfo == segmentInfo)
                    {
                        tooltip.Show(seriesBounds, SetTooltipTargetRect(segmentInfo.TooltipXPosition, segmentInfo.TooltipYPosition), false);
                    }
                    else
                    {
                        tooltip.BindingContext = segmentInfo;
                        tooltip.Duration = settings.Duration;
                        tooltip.Background = settings.Background;
                        tooltip.Content = GetTooltipTemplate(settings, segmentInfo);
                        tooltip.Show(seriesBounds, SetTooltipTargetRect(segmentInfo.TooltipXPosition, segmentInfo.TooltipYPosition), canAnimate);
                    }

                    previousSegmentInfo = segmentInfo;
                }
            }
        }

        private Rect SetTooltipTargetRect(float x, float y)
        {
            float sizeValue = 1;
            float noseOffset = 2;
            float halfSizeValue = 0.5f;

            Rect targetRect = new Rect(x - halfSizeValue, y + noseOffset, sizeValue, sizeValue);
            return targetRect;
        }

        private void SetTooltipPosition(SunburstSegment segmentInfo)
        {
            var r = (segmentInfo.OuterRadius + segmentInfo.InnerRadius) / 2;
            var radians = (segmentInfo.ArcStartAngle + segmentInfo.ArcEndAngle) / 2;
            segmentInfo.TooltipXPosition = (float)(Center.X + r * Math.Cos(radians));
            segmentInfo.TooltipYPosition = (float)(Center.Y + r * Math.Sin(radians));
        }

        private View? GetTooltipTemplate(SunburstTooltipSettings tooltipSettings, SunburstSegment sunburstSegment)
        {
            View? view;
            object layout;

            if (TooltipTemplate is DataTemplate template)
            {
                layout = template.CreateContent();
            }
            else
            {
                layout = GetDefaultTooltipTemplate(tooltipSettings, sunburstSegment).CreateContent();
            }

#if NET10_0_OR_GREATER
			view = layout as View;
#else
			view = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
#endif
			if (view != null)
            {
#if NET9_0_OR_GREATER
                var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity);
#else
                var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
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

        private DataTemplate GetDefaultTooltipTemplate(SunburstTooltipSettings settings, SunburstSegment? segment)
        {
            Label category = new Label();
            category.VerticalOptions = LayoutOptions.Fill;
            category.HorizontalOptions = LayoutOptions.Fill;
            category.VerticalTextAlignment = TextAlignment.Center;
            category.HorizontalTextAlignment = TextAlignment.Start;
#if WINDOWS
			category.LineBreakMode = LineBreakMode.NoWrap;
# endif
			category.SetBinding(Label.TextProperty, BindingHelper.CreateBinding("Item[0]", getter: static (SunburstSegment segment) => ((List<object>)segment.Item!)[0], source: segment));
            category.SetBinding(Label.TextColorProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.TextColor), getter: static (SunburstTooltipSettings settings) => settings.TextColor, source: settings));
            category.SetBinding(Label.FontSizeProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontSize), getter: static (SunburstTooltipSettings settings) => settings.FontSize, source: settings));
            category.SetBinding(Label.FontFamilyProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontFamily), getter: static (SunburstTooltipSettings settings) => settings.FontFamily, source: settings));
            category.SetBinding(Label.FontAttributesProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontAttributes), getter: static (SunburstTooltipSettings settings) => settings.FontAttributes, source: settings));

            Label value = new Label();
            value.VerticalOptions = LayoutOptions.Fill;
            value.HorizontalOptions = LayoutOptions.Fill;
            value.VerticalTextAlignment = TextAlignment.Center;
            value.HorizontalTextAlignment = TextAlignment.Start;
#if WINDOWS
			value.LineBreakMode = LineBreakMode.NoWrap;
#endif
			value.SetBinding(Label.TextProperty, BindingHelper.CreateBinding("Item[1]", getter: static (SunburstSegment segment) => ((List<object>)segment.Item!)[1], source: segment));
            value.SetBinding(Label.TextColorProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.TextColor), getter: static (SunburstTooltipSettings settings) => settings.TextColor, source: settings));
            value.SetBinding(Label.FontSizeProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontSize), getter: static (SunburstTooltipSettings settings) => settings.FontSize, source: settings));
            value.SetBinding(Label.FontFamilyProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontFamily), getter: static (SunburstTooltipSettings settings) => settings.FontFamily, source: settings));
            value.SetBinding(Label.FontAttributesProperty, BindingHelper.CreateBinding(nameof(SunburstTooltipSettings.FontAttributes), getter: static (SunburstTooltipSettings settings) => settings.FontAttributes, source: settings));
            var template = new DataTemplate(() =>
            {
                var stackLayout = new VerticalStackLayout
                {
                    Children = { category, value },
                    Margin = settings.Margin
                };

#if NET10_0_OR_GREATER
				return stackLayout;
#else
				return new ViewCell { View = stackLayout };
#endif
			});

            return template;
        }

        private void Tooltip_TooltipClosed(object? sender, TooltipClosedEventArgs e)
        {
            previousSegmentInfo = null;
        }

        private SunburstSegment? GetTooltip(float x, float y)
        {
            if (EnableTooltip && LevelsCount > 0)
            {
                if (ActualSeriesClipRect.IsEmpty && !ActualSeriesClipRect.Contains(x, y)) return null;

                if (GetSelectedSegment(x, y) is not SunburstSegment segment) return null;

                return segment;
            }

            return null;
        }

        internal void UpdateLegendItems()
        {
            area.ShouldPopulateLegendItems = true;
            area.UpdateLegendItemAttributes();
        }

#endregion
    }
}