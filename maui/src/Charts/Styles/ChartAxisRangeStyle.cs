namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class ChartAxisRangeStyle : BindableObject
	{
		#region Bindabale Properties

		/// <summary>
		/// Identifies the <see cref="Start"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Start"/> bindable property determines the starting value of the axis range style.
		/// </remarks>
		public static readonly BindableProperty StartProperty = BindableProperty.Create(
			nameof(Start),
			typeof(object),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnStartPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="End"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="End"/> bindable property determines the ending value of the axis range style.
		/// </remarks>
		public static readonly BindableProperty EndProperty = BindableProperty.Create(
			nameof(End),
			typeof(object),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnEndPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="MajorGridLineStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MajorGridLineStyle"/> bindable property determines the style of the major grid lines.
		/// </remarks>
		public static readonly BindableProperty MajorGridLineStyleProperty = BindableProperty.Create(
			nameof(MajorGridLineStyle),
			typeof(ChartLineStyle),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnMajorGridLineStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="MinorGridLineStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MinorGridLineStyle"/> bindable property determines the style of the minor grid lines.
		/// </remarks>
		public static readonly BindableProperty MinorGridLineStyleProperty = BindableProperty.Create(
			nameof(MinorGridLineStyle),
			typeof(ChartLineStyle),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnMinorGridLineStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="MajorTickStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MajorTickStyle"/> bindable property determines the style of the major ticks.
		/// </remarks>
		public static readonly BindableProperty MajorTickStyleProperty = BindableProperty.Create(
			nameof(MajorTickStyle),
			typeof(ChartAxisTickStyle),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnMajorTickStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="MinorTickStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MinorTickStyle"/> bindable property determines the style of the minor ticks.
		/// </remarks>
		public static readonly BindableProperty MinorTickStyleProperty = BindableProperty.Create(
			nameof(MinorTickStyle),
			typeof(ChartAxisTickStyle),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnMinorTickStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelStyle"/> bindable property determines the style of the axis labels.
		/// </remarks>
		public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
			nameof(LabelStyle),
			typeof(ChartAxisLabelStyle),
			typeof(ChartAxisRangeStyle),
			null,
			BindingMode.Default,
			null,
			OnLabelStylePropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisRangeStyle"/> class.
		/// </summary>
		public ChartAxisRangeStyle()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Start Range of ChartAxisRangeStyle.
		/// </summary>
		public object Start
		{
			get { return GetValue(StartProperty); }
			set { SetValue(StartProperty, value); }
		}

		/// <summary>
		/// Gets or sets the End Range of ChartAxisRangeStyle.
		/// </summary>
		public object End
		{
			get { return GetValue(EndProperty); }
			set { SetValue(EndProperty, value); }
		}

		/// <summary>
		/// Gets or sets the customized style for the major grid line. 
		/// </summary>
		/// <value>This property takes <see cref="ChartLineStyle"/> instance as value, which encapsulates the customization properties for major gridlines.</value>
		public ChartLineStyle MajorGridLineStyle
		{
			get { return (ChartLineStyle)GetValue(MajorGridLineStyleProperty); }
			set { SetValue(MajorGridLineStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the customized style for the minor grid line. 
		/// </summary>
		/// <value>This property takes <see cref="ChartLineStyle"/> instance as value, which encapsulates the customization properties for minor gridlines.</value>
		public ChartLineStyle MinorGridLineStyle
		{
			get { return (ChartLineStyle)GetValue(MinorGridLineStyleProperty); }
			set { SetValue(MinorGridLineStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the options for customizing the axis labels. 
		/// </summary>
		/// <value>This property takes the <see cref="ChartAxisLabelStyle"/> as its value.</value>
		public ChartAxisLabelStyle LabelStyle
		{
			get { return (ChartAxisLabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the customized style for the minor tick lines. 
		/// </summary>
		/// <value>This property takes <see cref="ChartAxisTickStyle"/> instance as value, which encapsulates the customization properties for minor ticks.</value>
		public ChartAxisTickStyle MinorTickStyle
		{
			get { return (ChartAxisTickStyle)GetValue(MinorTickStyleProperty); }
			set { SetValue(MinorTickStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the customized style for the major tick lines. 
		/// </summary>
		/// <value>This property takes <see cref="ChartAxisTickStyle"/> instance as value, which encapsulates the customization properties for major ticks.</value>
		public ChartAxisTickStyle MajorTickStyle
		{
			get { return (ChartAxisTickStyle)GetValue(MajorTickStyleProperty); }
			set { SetValue(MajorTickStyleProperty, value); }
		}

		#endregion

		#region Methods  

		static void OnStartPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		static void OnEndPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		static void OnMajorGridLineStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnMinorGridLineStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		static void OnMajorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		static void OnMinorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		static void OnLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		#endregion
	}
}
