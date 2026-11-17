using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the range style class used to customize the grid lines, tick lines and labels of a chart axis within a specified range.
	/// </summary>
	/// <remarks>
	/// <para>To customize axis elements within a specific range, create an instance of this class, set the <see cref="Start"/> and <see cref="End"/> values, customize the style properties, and add it to the <see cref="ChartAxis.RangeStyles"/> collection.</para>
	/// <para>The <see cref="Start"/> and <see cref="End"/> values accept numeric, date-time, or category index values depending on the axis type.</para>
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///     <chart:SfCartesianChart.XAxes>
	///         <chart:NumericalAxis>
	///             <chart:NumericalAxis.RangeStyles>
	///                 <chart:ChartAxisRangeStyleCollection>
	///                     <chart:ChartAxisRangeStyle Start="15" End="21">
	///                         <chart:ChartAxisRangeStyle.MajorGridLineStyle>
	///                             <chart:ChartLineStyle Stroke="#096EBF" StrokeWidth="3"/>
	///                         </chart:ChartAxisRangeStyle.MajorGridLineStyle>
	///                         <chart:ChartAxisRangeStyle.LabelStyle>
	///                             <chart:ChartAxisLabelStyle TextColor="#096EBF" FontAttributes="Bold"/>
	///                         </chart:ChartAxisRangeStyle.LabelStyle>
	///                     </chart:ChartAxisRangeStyle>
	///                 </chart:ChartAxisRangeStyleCollection>
	///             </chart:NumericalAxis.RangeStyles>
	///         </chart:NumericalAxis>
	///     </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// NumericalAxis xAxis = new NumericalAxis();
	/// ChartAxisRangeStyle rangeStyle = new ChartAxisRangeStyle()
	/// {
	///     Start = 15,
	///     End = 21,
	///     MajorGridLineStyle = new ChartLineStyle()
	///     {
	///         Stroke = Color.FromArgb("#096EBF"),
	///         StrokeWidth = 3
	///     },
	///     LabelStyle = new ChartAxisLabelStyle()
	///     {
	///         TextColor = Color.FromArgb("#096EBF"),
	///         FontAttributes = FontAttributes.Bold
	///     }
	/// };
	/// xAxis.RangeStyles.Add(rangeStyle);
	/// chart.XAxes.Add(xAxis);
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// </example>
	public class ChartAxisRangeStyle : Element
	{
		#region Fields

		internal event EventHandler? StyleValueChanged;

		#endregion

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
			OnRangePropertyChanged);

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
			OnRangePropertyChanged);

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
			OnRangeStylePropertyChanged);

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
			OnRangeStylePropertyChanged);

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
			OnRangeStylePropertyChanged);

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
			OnRangeStylePropertyChanged);

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
			OnRangeStylePropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisRangeStyle"/> class.
		/// </summary>
		/// <remarks>
		/// The style properties are intentionally left null by default. Only the styles explicitly set on a range
		/// are applied within that range; unset styles fall back to the axis-level styles, and overlapping ranges
		/// resolve each element style independently (the last range that sets a style wins).
		/// </remarks>
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

		internal bool IsValueInRange(double value)
		{
			if (double.IsNaN(value))
			{
				return false;
			}

			var start = ChartUtils.ConvertToDouble(Start);
			var end = ChartUtils.ConvertToDouble(End);

			// When both Start and End are not set, the range style is not applied.
			if (double.IsNaN(start) && double.IsNaN(end))
			{
				return false;
			}

			// When only Start is set, the range is open-ended (Start -> axis maximum).
			if (double.IsNaN(end))
			{
				return value >= start;
			}

			// When only End is set, the range is open-ended (axis minimum -> End).
			if (double.IsNaN(start))
			{
				return value <= end;
			}

			var range = start <= end ? new DoubleRange(start, end) : new DoubleRange(end, start);

			return range.Inside(value);
		}

		internal void RaiseStyleChanged()
		{
			StyleValueChanged?.Invoke(this, EventArgs.Empty);
		}

		void OnStylePropertyChanged(object? oldValue, object? newValue)
		{
			if (oldValue is BindableObject oldStyle)
			{
				SetInheritedBindingContext(oldStyle, null);
				oldStyle.PropertyChanged -= RangeStyle_PropertyChanged;
			}

			if (newValue is BindableObject newStyle)
			{
				SetInheritedBindingContext(newStyle, BindingContext);
				newStyle.PropertyChanged += RangeStyle_PropertyChanged;
			}

			RaiseStyleChanged();
		}

		void RangeStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			RaiseStyleChanged();
		}

		static void OnRangePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as ChartAxisRangeStyle)?.RaiseStyleChanged();
		}

		static void OnRangeStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as ChartAxisRangeStyle)?.OnStylePropertyChanged(oldValue, newValue);
		}

		#endregion
	}
}
