using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Internals;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="ChartBase"/> class is the base for <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, <see cref="SfFunnelChart"/>, <see cref="SfPyramidChart"/> and <see cref="SfPolarChart"/> types.
	/// </summary>
	public abstract class ChartBase : View, IContentView, IChart
	{
		#region Fields
		readonly AreaBase _area;
		internal readonly LegendLayout _legendLayout;
		ChartTooltipBehavior? _defaultToolTipBehavior;
		ChartTooltipBehavior? _toolTipBehavior;
		SfTooltip? _tooltipView;
		Rect _seriesBounds;
		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Title"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Title"/> bindable property determines the title of the chart.
		/// </remarks>
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(
			nameof(Title),
			typeof(object),
			typeof(ChartBase),
			null,
			propertyChanged: OnTitlePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Legend"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Legend"/> bindable property determines the legend of the chart.
		/// </remarks>
		public static readonly BindableProperty LegendProperty = BindableProperty.Create(
			nameof(Legend),
			typeof(ChartLegend),
			typeof(ChartBase),
			null,
			propertyChanged: OnLegendPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TooltipBehavior"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipBehavior"/> bindable property determines the 
		/// tooltip behavior of the chart.
		/// </remarks>
		public static readonly BindableProperty TooltipBehaviorProperty = BindableProperty.Create(
			nameof(TooltipBehavior),
			typeof(ChartTooltipBehavior),
			typeof(ChartBase),
			null,
			BindingMode.Default,
			null,
			OnTooltipBehaviorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="InteractiveBehavior"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="InteractiveBehavior"/> bindable property determines the 
		/// interactive behavior of the chart.
		/// </remarks>
		public static readonly BindableProperty InteractiveBehaviorProperty = BindableProperty.Create(
			nameof(InteractiveBehavior),
			typeof(ChartInteractiveBehavior),
			typeof(ChartBase),
			null,
			BindingMode.Default,
			null,
			OnInteractiveBehaviorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlotAreaBackgroundView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="PlotAreaBackgroundView"/> bindable property determines the 
		/// background view of the plot area.
		/// </remarks>
		public static readonly BindableProperty PlotAreaBackgroundViewProperty = BindableProperty.Create(
			nameof(PlotAreaBackgroundView),
			typeof(View),
			typeof(ChartBase),
			null,
			propertyChanged: OnPlotAreaBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="LegendStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LegendStyle"/> bindable property determines the style of the chart legend.
		/// </remarks>
		internal static readonly BindableProperty LegendStyleProperty = BindableProperty.Create(
			nameof(LegendStyle),
			typeof(ChartThemeLegendLabelStyle),
			typeof(ChartBase),
			propertyChanged: OnLegendStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TooltipBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipBackground"/> bindable property determines the 
		/// background color of the tooltip.
		/// </remarks>
		internal static readonly BindableProperty TooltipBackgroundProperty = BindableProperty.Create(
			nameof(TooltipBackground),
			typeof(Brush),
			typeof(ChartBase),
			null,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="TooltipTextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipTextColor"/> bindable property determines the 
		/// text color of the tooltip.
		/// </remarks>
		internal static readonly BindableProperty TooltipTextColorProperty = BindableProperty.Create(
			nameof(TooltipTextColor),
			typeof(Color),
			typeof(ChartBase),
			null,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="TooltipFontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipFontSize"/> bindable property determines the 
		/// font size of the tooltip text.
		/// </remarks>
		internal static readonly BindableProperty TooltipFontSizeProperty = BindableProperty.Create(
			nameof(TooltipFontSize),
			typeof(double),
			typeof(ChartBase),
			double.NaN,
			BindingMode.Default,
			null);

		ChartThemeLegendLabelStyle IChart.LegendLabelStyle => LegendStyle;
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the title for chart. It supports the string or any view as title.
		/// </summary>
		/// <value>This property takes the <c>object</c> as its value. Its default value is null.</value>
		/// 
		/// <remarks>
		/// 
		/// <para>Example code for string as title.</para>
		/// 
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart Title="Average High/Low Temperature">
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// 
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     chart.Title = "Average High / Low Temperature";
		/// ]]></code>
		/// ***
		/// 
		/// <para>Example code for View as title.</para>
		/// 
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.Title>
		///               <Label Text = "Average High/Low Temperature" 
		///                      HorizontalOptions="Fill"
		///                      HorizontalTextAlignment="Center"
		///                      VerticalOptions="Center"
		///                      FontSize="16"
		///                      TextColor="Black"/>
		///           </chart:SfCartesianChart.Title>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// 
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     chart.Title = new Label()
		///     { 
		///         Text = "Average High / Low Temperature",
		///         HorizontalOptions = LayoutOptions.Fill,
		///         HorizontalTextAlignment = TextAlignment.Center,
		///         VerticalOptions = LayoutOptions.Center,
		///         FontSize = 16,
		///         TextColor = Colors.Black
		///     };
		/// ]]></code>
		/// ***
		/// </remarks>
		public object Title
		{
			get { return (object)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the legend that helps to identify the corresponding series or data point in chart.
		/// </summary>
		/// <value>This property takes a <see cref="ChartLegend"/> instance as value and its default value is null.</value>
		///<remarks>
		/// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="Legend"/> property. </para>
		///</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code> <![CDATA[
		/// <chart:SfCircularChart>
		///
		///        <chart:SfCircularChart.BindingContext>
		///            <local:ViewModel/>
		///        </chart:SfCircularChart.BindingContext>
		///        
		///        <chart:SfCircularChart.Legend>
		///            <chart:ChartLegend/>
		///        </chart:SfCircularChart.Legend>
		///
		///        <chart:SfCircularChart.Series>
		///            <chart:PieSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue" />
		///        </chart:SfCircularChart.Series>
		///        
		/// </chart:SfCircularChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCircularChart chart = new SfCircularChart();
		/// 
		/// ViewModel viewModel = new ViewModel();
		///	chart.BindingContext = viewModel;
		///	
		/// chart.Legend = new ChartLegend();
		/// 
		/// PieSeries series = new PieSeries()
		/// {
		///     ItemsSource = viewModel.Data,
		///     XBindingPath = "XValue",
		///     YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartLegend Legend
		{
			get { return (ChartLegend)GetValue(LegendProperty); }
			set { SetValue(LegendProperty, value); }
		}

		/// <summary>
		/// Gets or sets a tooltip behavior that allows to customize the default tooltip appearance in the chart. 
		/// </summary>
		/// <value>This property takes <see cref="ChartTooltipBehavior"/> instance as value and its default value is null.</value>
		/// <remarks>
		/// 
		/// <para>To display the tooltip on the chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <b>ChartSeries</b>.</para>
		/// 
		/// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="TooltipBehavior"/> property. </para>
		/// 
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCircularChart>
		/// 
		///         <chart:SfCircularChart.BindingContext>
		///             <local:ViewModel/>
		///         </chart:SfCircularChart.BindingContext>
		/// 
		///         <chart:SfCircularChart.TooltipBehavior>
		///             <chart:ChartTooltipBehavior/>
		///         </chart:SfCircularChart.TooltipBehavior>
		/// 
		///         <chart:SfCircularChart.Series>
		///             <chart:PieSeries EnableTooltip="True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
		///         </chart:SfCircularChart.Series>
		/// 
		/// </chart:SfCircularChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		///  SfCircularChart chart = new SfCircularChart();
		///  
		///  ViewModel viewModel = new ViewModel();
		///  chart.BindingContext = viewModel;
		///  
		///  chart.TooltipBehavior = new ChartTooltipBehavior();
		///  
		///  PieSeries series = new PieSeries()
		///  {
		///     ItemsSource = viewModel.Data,
		///     XBindingPath = "XValue",
		///     YBindingPath = "YValue",
		///     EnableTooltip = true
		///  };
		///  chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		/// <seealso cref="ChartSeries.EnableTooltip"/>
		public ChartTooltipBehavior TooltipBehavior
		{
			get { return (ChartTooltipBehavior)GetValue(TooltipBehaviorProperty); }
			set { SetValue(TooltipBehaviorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the interactive behavior that allows to override and customize the touch interaction methods.
		/// </summary>
		/// <value>This property takes <see cref="ChartInteractiveBehavior"/> instance as value and its default value is null.</value>
		/// <remarks>
		/// <para>To use those touch interaction methods, create the class inherited by ChartInteractiveBehavior class.</para>
		/// <para>Then Create the instance for that class and instance has to be added in the chart's <see cref="ChartBase.InteractiveBehavior"/> as per the following code snippet.</para>
		/// </remarks>
		/// <example>
		/// # [ChartInteractionExt.cs](#tab/tabid-1)
		/// <code><![CDATA[
		/// public class ChartInteractionExt : ChartInteractiveBehavior
		/// {
		///    <!--omitted for brevity-->
		/// }
		/// ]]>
		/// </code>
		/// # [MainPage.xaml](#tab/tabid-2)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCircularChart.BindingContext>
		///            <local:ViewModel/>
		///        </chart:SfCircularChart.BindingContext>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.InteractiveBehavior>
		///          <local:ChartInteractionExt/>
		///     </chart:SfCartesianChart.InteractiveBehavior>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-3)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ViewModel viewModel = new ViewModel();
		/// chart.BindingContext = viewModel;
		/// ...
		/// ChartInteractionExt interaction = new ChartInteractionExt();
		/// chart.ChartInteractiveBehavior = interaction;
		/// ...
		/// ]]>
		/// </code>
		/// ***
		///
		/// </example>
		public ChartInteractiveBehavior InteractiveBehavior
		{
			get { return (ChartInteractiveBehavior)GetValue(InteractiveBehaviorProperty); }
			set { SetValue(InteractiveBehaviorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view to the background of chart area.
		/// </summary>
		/// <value>This property takes the <c>View</c> as its value. Its default value is null.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCircularChart>
		/// 
		///         <chart:SfCircularChart.BindingContext>
		///             <local:ViewModel/>
		///         </chart:SfCircularChart.BindingContext>
		/// 
		///         <chart:SfCartesianChart.PlotAreaBackgroundView>
		///             <BoxView Color="Aqua" Margin = "10" CornerRadius = "5" />
		///         </chart:SfCartesianChart.PlotAreaBackgroundView>
		/// 
		///         <chart:SfCircularChart.Series>
		///             <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
		///         </chart:SfCircularChart.Series>
		/// 
		/// </chart:SfCircularChart>
		/// ]]>
		/// </code>
		/// 
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		///  SfCircularChart chart = new SfCircularChart();
		///  
		///  ViewModel viewModel = new ViewModel();
		///  chart.BindingContext = viewModel;
		///  
		///  BoxView boxView = new BoxView()
		///  {
		///     Color = Colors.Aqua,
		///     Margin = 10,
		///     CornerRadius = 5,
		///  };
		///  
		///  chart.PlotAreaBackgroundView = boxView
		///  
		///  DoughnutSeries series = new DoughnutSeries()
		///  {
		///     ItemsSource = viewModel.Data,
		///     XBindingPath = "XValue",
		///     YBindingPath = "YValue",
		///     SelectionBrush = Colors.Blue
		///  };
		///  chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public View PlotAreaBackgroundView
		{
			get { return (View)GetValue(PlotAreaBackgroundViewProperty); }
			set { SetValue(PlotAreaBackgroundViewProperty, value); }
		}

		internal ChartThemeLegendLabelStyle LegendStyle
		{
			get { return (ChartThemeLegendLabelStyle)GetValue(LegendStyleProperty); }
			set { SetValue(LegendStyleProperty, value); }
		}

		/// <summary>
		/// Get the actual rendering bounds of the chart series.
		/// </summary>
		public Rect SeriesBounds
		{
			get
			{
				return _seriesBounds;
			}

			internal set
			{
				_seriesBounds = value;
				OnPropertyChanged(nameof(SeriesBounds));
			}
		}

		#endregion

		#region Internal Properties
		internal View? Content { get; set; }

		internal ChartTitleView TitleView { get; set; }

		internal AbsoluteLayout BehaviorLayout { get; set; }

		internal bool IsRequiredDataLabelsMeasure
		{
			get; set;
		}

		bool IChart.IsRequiredDataLabelsMeasure { get => IsRequiredDataLabelsMeasure; set => IsRequiredDataLabelsMeasure = value; }

		internal Brush TooltipBackground
		{
			get { return (Brush)GetValue(TooltipBackgroundProperty); }
			set { SetValue(TooltipBackgroundProperty, value); }
		}
		internal Color TooltipTextColor
		{
			get { return (Color)GetValue(TooltipTextColorProperty); }
			set { SetValue(TooltipTextColorProperty, value); }
		}

		internal double TooltipFontSize
		{
			get { return (double)GetValue(TooltipFontSizeProperty); }
			set { SetValue(TooltipFontSizeProperty, value); }
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="ChartBase"/> class.
		/// </summary>
		public ChartBase()
		{
			TitleView = new ChartTitleView();
			BehaviorLayout = [];
			LegendStyle = new ChartThemeLegendLabelStyle(this);
			LegendStyle.Parent = this;
			_area = CreateChartArea();
			_legendLayout = new LegendLayout(_area);
			FlowDirection = FlowDirection.LeftToRight;
			Content = CreateTemplate(_legendLayout);
		}

		internal virtual AreaBase CreateChartArea()
		{
			throw new ArgumentNullException("Chart area cannot be null");
		}

		Grid CreateTemplate(LegendLayout legendLayout)
		{
			Grid grid = [];
			grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Auto });
			grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Star });
			Grid.SetRow(TitleView, 0);
			grid.Add(TitleView);
			Grid.SetRow(legendLayout, 1);
			grid.Add(legendLayout);
			grid.Parent = this;

			return grid;
		}
		#endregion

		#region Protected Methods

		/// <summary>
		/// Invoked when binding context changed.
		/// </summary>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			void UpdateBindingContext(object? target)
			{
				if (target is BindableObject bindableObject)
				{
					SetInheritedBindingContext(bindableObject, BindingContext);
				}
			}

			UpdateBindingContext(Content);
			UpdateBindingContext(Title);
			UpdateBindingContext(TooltipBehavior);
			UpdateBindingContext(InteractiveBehavior);
			UpdateBindingContext(Legend);
			UpdateBindingContext(PlotAreaBackgroundView);
		}

		/// <summary>
		/// Enforces LeftToRight flow direction for the chart, overriding any inherited RightToLeft flow direction.
		/// </summary>
		/// <exclude/>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == nameof(ChartBase.FlowDirection) && (FlowDirection == FlowDirection.RightToLeft || FlowDirection == FlowDirection.MatchParent))
			{
				// If the new FlowDirection is RightToLeft, enforce LeftToRight
				FlowDirection = FlowDirection.LeftToRight;
				return;
			}

			base.OnPropertyChanged(propertyName);
		}

		#endregion

		#region Internal Methods

		internal virtual TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y)
		{
			var visibleSeries = (_area as IChartArea)?.VisibleSeries;
			if (visibleSeries == null)
			{
				return null;
			}

			foreach (var chartSeries in visibleSeries.Reverse())
			{
				if (!chartSeries.EnableTooltip || chartSeries.PointsCount <= 0)
				{
					continue;
				}

				var tooltipInfo = chartSeries.GetTooltipInfo(behavior, x, y);
				if (tooltipInfo != null)
				{
					return tooltipInfo;
				}
			}

			return null;
		}

		internal virtual void UpdateLegendItems()
		{
		}

		#endregion

		#region export

		/// <summary>
		/// <para> To convert a chart view to a stream, the <b> GetStreamAsync </b> method is used. Currently, the supported file formats are <b> JPEG and PNG </b>. </para>
		/// <para> To get the stream for the chart view in <b> PNG </b> file format, use <b> await chart.GetStreamAsync(ImageFileFormat.Png); </b> </para>
		/// <para> To get the stream for the chart view in <b> JPEG </b> file format, use <b> await chart.GetStreamAsync(ImageFileFormat.Jpeg); </b> </para>
		/// <remarks> The charts stream can only be rendered when the chart view is added to the visual tree. </remarks>
		/// <para> <b> imageFileFormat </b> Pass the required file format. </para>
		/// </summary>
		/// <param name="imageFileFormat">  Pass the required file format. </param>
		/// <returns> Returns the chart view's stream in the desired file format. </returns>
		public Task<Stream> GetStreamAsync(ImageFileFormat imageFileFormat)
		{
			return Syncfusion.Maui.Toolkit.Internals.ViewExtensions.GetStreamAsync(this, imageFileFormat);
		}

		/// <summary>
		/// <para> To save a chart view as an image in the desired file format, the <b> SaveAsImage </b> is used. Currently, the supported image formats are <b> JPEG and PNG </b>. </para>
		/// <para> By default, the image format is <b> PNG</b>. For example, <b> chart.SaveAsImage("Test"); </b> </para>
		/// <para> To save a chart view in the <b> PNG </b> image format, the filename should be passed with the <b> ".png" extension </b> 
		/// while to save the image in the <b> JPEG </b> image format, the filename should be passed with the <b> ".jpeg" extension </b>, 
		/// for example, <b> "chart.SaveAsImage("Test.png")" and "chart.SaveAsImage("Test.jpeg")" </b> respectively. </para>
		/// <para> <b> Saved location: </b>
		/// For <b> Windows, Android, and Mac </b>, the image will be saved in the <b> Pictures folder </b>, and for <b> iOS </b>, the image will be saved in the <b> Photos Album folder </b>. </para>
		/// <remarks> <para> In <b> Windows and Mac </b>, when you save an image with an already existing filename, the existing file is replaced with a new file, but the filename remains the same. </para>
		/// <para> In <b> Android </b>, when you save the same view with an existing filename, the new image will be saved with a filename with a number appended to it, 
		/// for example, Test(1).jpeg and the existing filename Test.jpeg will be removed.When you save a different view with an already existing filename, 
		/// the new image will be saved with a filename with a number will be appended to it, for example, Test(1).jpeg, and the existing filename Test.jpeg will remain in the folder. </para>
		/// <para> In <b> iOS </b>, due to its platform limitation, the image will be saved with the default filename, for example, IMG_001.jpeg, IMG_002.png and more. </para> </remarks>
		/// <remarks> The chart view can be saved as an image only when the view is added to the visual tree. </remarks>
		/// </summary>
		/// <param name="fileName"></param>
		public void SaveAsImage(string fileName)
		{
			Syncfusion.Maui.Toolkit.Internals.ViewExtensions.SaveAsImage(this, fileName);
		}

		#endregion

		#region Interface Overrides

		object? IContentView.Content => Content;

		/// <summary>
		/// Gets the presented content value.
		/// </summary>
		IView? IContentView.PresentedContent => Content;

		/// <summary>
		/// Gets the padding value.
		/// </summary>
		Thickness IPadding.Padding => Thickness.Zero;

		ChartTooltipBehavior? IChart.ActualTooltipBehavior
		{
			get
			{
				if (TooltipBehavior == null)
				{
					if (_defaultToolTipBehavior == null)
					{
						_defaultToolTipBehavior = new ChartTooltipBehavior();
						_defaultToolTipBehavior.Chart = this;
						// defaultToolTipBehavior.IsDefault = true;
					}

					return _defaultToolTipBehavior;
				}

				return _toolTipBehavior;
			}
			set
			{
				_toolTipBehavior = value;
			}
		}

		SfTooltip? IChart.TooltipView
		{
			get { return _tooltipView; }
			set { _tooltipView = value; }
		}

		Rect IChart.ActualSeriesClipRect
		{
			get { return SeriesBounds; }
			set { SeriesBounds = value; }
		}

		IArea IChart.Area => _area;

		Color IChart.BackgroundColor => BackgroundColor;

		AbsoluteLayout IChart.BehaviorLayout => BehaviorLayout;

		double IChart.TitleHeight => TitleView != null ? TitleView.Height : 0;

		Brush? IChart.GetSelectionBrush(ChartSeries series)
		{
			return GetSelectionBrush(series);
		}

		//Return selection brush if the series was get selected from Selection behavior. 
		//As circular series not has series selection no need to consider.
		internal virtual Brush? GetSelectionBrush(ChartSeries series)
		{
			return null;
		}

		Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
		{
			return this.MeasureContent(widthConstraint, heightConstraint);
		}

		Size IContentView.CrossPlatformArrange(Rect bounds)
		{
			this.ArrangeContent(bounds);
			return bounds.Size;
		}

		TooltipInfo? IChart.GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y) => GetTooltipInfo(behavior, x, y);

		#endregion

		#region Property call back methods

		static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not ChartBase chart || chart.TitleView == null)
			{
				return;
			}

			if (newValue is View view)
			{
				chart.TitleView.IsVisible = true;
				chart.TitleView.Content = view;
			}
			else if (!string.IsNullOrEmpty(newValue?.ToString()))
			{
				chart.TitleView.IsVisible = true;
				chart.TitleView.InitTitle(newValue.ToString());
			}
			else
			{
				chart.TitleView.IsVisible = false;
			}
		}

		static void OnLegendPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartBase chart && chart._legendLayout is LegendLayout layout)
			{
				if (oldValue is ILegend)
				{
					SetInheritedBindingContext((Element)oldValue, null);
				}

				if (newValue is ILegend legend)
				{
					layout.Legend = legend.IsVisible ? legend : null;
					SetInheritedBindingContext((Element)newValue, chart.BindingContext);
				}
				else
				{
					layout.Legend = null;
				}

				SetParent((Element)oldValue, (Element)newValue, (Element)bindable);
				layout._areaBase.ScheduleUpdateArea();
			}
		}

		static void OnTooltipBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartBase chart)
			{
				if (newValue is ChartTooltipBehavior behavior)
				{
					SetInheritedBindingContext(behavior, chart.BindingContext);
					chart._toolTipBehavior = behavior;
					behavior.Chart = chart;
				}

				if (oldValue is ChartTooltipBehavior oldBehavior)
				{
					SetInheritedBindingContext(oldBehavior, null);
				}
			}

			SetParent((Element)oldValue, (Element)newValue, (Element)bindable);
		}

		static void OnInteractiveBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartBase chart)
			{
				if (newValue is ChartInteractiveBehavior behavior)
				{
					SetInheritedBindingContext(behavior, chart.BindingContext);
				}

				if (oldValue is ChartInteractiveBehavior oldBehavior)
				{
					SetInheritedBindingContext(oldBehavior, null);
				}

				SetParent((Element)oldValue, (Element)newValue, chart);
			}
		}

		static void OnPlotAreaBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartBase chart)
			{
				chart.OnPlotAreaBackgroundChanged(newValue);
			}
		}

		internal virtual void OnPlotAreaBackgroundChanged(object newValue)
		{
			if (_area.PlotArea is ChartPlotArea plotArea)
			{
				plotArea.PlotAreaBackgroundView = (View)newValue;
			}
		}

		static void OnLegendStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartBase chart)
			{
				SetParent((Element)oldValue, (Element)newValue, chart);
			}
		}

		internal static void SetParent(Element? oldElement, Element? newElement, Element parent)
		{
			if (oldElement != null)
			{
				oldElement.Parent = null;
			}

			if (newElement != null)
			{
				newElement.Parent = parent;
			}
		}

		#endregion
	}
}
