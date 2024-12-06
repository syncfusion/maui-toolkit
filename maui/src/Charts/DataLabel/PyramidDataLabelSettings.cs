namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <b> PyramidDataLabelSettings </b> class is used to customize the appearance of pyramid chart data labels.
	/// </summary>
	/// <remarks>
	/// <para>To customize data labels, create an instance of the <see cref="PyramidDataLabelSettings"/> class, and set it to the DataLabelSettings property of the pyramid chart.</para>
	/// 
	/// <para><b>ShowDataLabels</b></para>
	/// <para>Data labels can be added to a chart by enabling the <see cref="SfPyramidChart.ShowDataLabels"/> option.</para>
	/// 
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///    <chart:SfPyramidChart ItemsSource ="{Binding Data}"
	///                          XBindingPath="XValue"
	///                          YBindingPath="YValue"
	///                          ShowDataLabels="True"/>
	///
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfPyramidChart chart = new SfPyramidChart();
	/// ViewModel viewModel = new ViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.ShowDataLabels= true;
	/// 
	/// this.Content = chart;
	///
	/// ]]></code>
	/// ***
	/// 
	/// <para><b>Customization</b></para>
	/// <para>To change the appearance of data labels, it offers <see cref="ChartDataLabelSettings.LabelStyle"/> options.</para>
	/// <para>Null values are invalid for <see cref="ChartDataLabelSettings.LabelStyle"/>.</para>
	///
	/// # [Xaml](#tab/tabid-3)
	/// <code><![CDATA[
	///    <chart:SfPyramidChart ItemsSource ="{Binding Data}"
	///                          XBindingPath="XValue"
	///                          YBindingPath="YValue"
	///                          ShowDataLabels="True">
	///        <chart:SfPyramidChart.DataLabelSettings>
	///            <chart:PyramidDataLabelSettings>
	///                <chart:PyramidDataLabelSettings.LabelStyle>
	///                    <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
	///                </chart:PyramidDataLabelSettings.LabelStyle>
	///            </chart:PyramidDataLabelSettings>
	///        </chart:SfPyramidChart.DataLabelSettings>
	///    </chart:SfPyramidChart>
	///
	/// ]]></code>
	/// # [C#](#tab/tabid-4)
	/// <code><![CDATA[
	/// SfPyramidChart chart = new SfPyramidChart();
	/// ViewModel viewModel = new ViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.ShowDataLabels= true;
	/// 
	/// var labelStyle = new ChartDataLabelStyle()
	/// {
	///     Background = new SolidColorBrush(Colors.Red),
	///     TextColor = Colors.Black,
	///     FontSize = 14,
	/// };
	/// 
	/// chart.DataLabelSettings = new PyramidDataLabelSettings()
	/// {
	///     LabelStyle = labelStyle,
	/// };
	/// 
	/// this.Content = chart;
	///
	/// ]]></code>
	/// ***
	/// 
	/// </remarks>
	public partial class PyramidDataLabelSettings : ChartDataLabelSettings, IPyramidDataLabelSettings
	{
		#region Internal Properties

		internal SmartLabelAlignment SmartLabelAlignment { get; set; } = SmartLabelAlignment.Shift;

		#endregion

		#region Bindable Property

		/// <summary>
		/// Identifies the <see cref="Context"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Context"/> bindable property determines the context of the pyramid data label.
		/// </remarks>
		public static readonly BindableProperty ContextProperty = BindableProperty.Create(
			nameof(Context),
			typeof(PyramidDataLabelContext),
			typeof(PyramidDataLabelSettings),
			PyramidDataLabelContext.YValue,
			BindingMode.Default,
			null,
			null);

		#endregion

		#region Public Property

		/// <summary>
		/// Gets or sets an option that determines the content to be displayed in the data labels.
		/// </summary>
		/// <value>Its accept the <see cref="PyramidDataLabelContext"/> values and its default value is <see cref="PyramidDataLabelContext.YValue"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///    <chart:SfPyramidChart ItemsSource ="{Binding Data}"
		///                          XBindingPath="XValue"
		///                          YBindingPath="YValue"
		///                          ShowDataLabels="True">
		///        <chart:SfPyramidChart.DataLabelSettings>
		///            <chart:PyramidDataLabelSettings Context="XValue"/>
		///        </chart:SfPyramidChart.DataLabelSettings>
		///    </chart:SfPyramidChart>
		///
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfPyramidChart chart = new SfPyramidChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.ShowDataLabels= true;
		/// 
		/// chart.DataLabelSettings = new PyramidDataLabelSettings()
		/// {
		///     Context = PyramidDataLabelContext.XValue
		/// };
		/// 
		/// this.Content = chart;
		///
		/// ]]></code>
		/// ***
		/// </example>
		public PyramidDataLabelContext Context
		{
			get { return (PyramidDataLabelContext)GetValue(ContextProperty); }
			set { SetValue(ContextProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PyramidDataLabelSettings"/>.
		/// </summary>
		public PyramidDataLabelSettings()
		{
			LabelStyle = new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(6, 4) };
		}

		#endregion

		#region Interface Implementation

		string IPyramidDataLabelSettings.GetLabelContent(object? xValue, double yValue)
		{
			string labelContent = string.Empty;

			if (Context == PyramidDataLabelContext.XValue && xValue != null)
			{
				labelContent = xValue.ToString() ?? string.Empty;
			}
			else
			{
				if (double.IsNaN(yValue))
				{
					return labelContent;
				}

				if (LabelStyle != null && !string.IsNullOrEmpty(LabelStyle.LabelFormat))
				{
					labelContent = yValue.ToString(LabelStyle.LabelFormat);
				}
				else
				{
					labelContent = yValue.ToString("#.##");
				}
			}

			return labelContent;
		}

		ChartDataLabelStyle IPyramidDataLabelSettings.LabelStyle => LabelStyle ?? new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(6, 4) };

		SmartLabelAlignment IPyramidDataLabelSettings.SmartLabelAlignment => SmartLabelAlignment;

		#endregion
	}

	/// <summary>
	/// The FunnelDataLabelSettings class is used to customize the appearance of funnel chart data labels.
	/// </summary>
	/// <remarks>
	/// <para>To customize data labels, create an instance of the <see cref="FunnelDataLabelSettings"/> class, and set it to the DataLabelSettings property of the funnel chart.</para>
	/// 
	/// <para><b>ShowDataLabels</b></para>
	/// <para>Data labels can be added to a chart by enabling the <see cref="SfFunnelChart.ShowDataLabels"/> option.</para>
	/// 
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///    <chart:SfFunnelChart ItemsSource ="{Binding Data}"
	///                          XBindingPath="XValue"
	///                          YBindingPath="YValue"
	///                          ShowDataLabels="True"/>
	///
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// ViewModel viewModel = new ViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.ShowDataLabels= true;
	/// 
	/// this.Content = chart;
	///
	/// ]]></code>
	/// ***
	/// 
	/// <para><b>Customization</b></para>
	/// <para>To change the appearance of data labels, it offers <see cref="ChartDataLabelSettings.LabelStyle"/> options.</para>
	///
	/// # [Xaml](#tab/tabid-3)
	/// <code><![CDATA[
	///    <chart:SfFunnelChart ItemsSource ="{Binding Data}"
	///                          XBindingPath="XValue"
	///                          YBindingPath="YValue"
	///                          ShowDataLabels="True">
	///        <chart:SfFunnelChart.DataLabelSettings>
	///            <chart:FunnelDataLabelSettings>
	///                <chart:FunnelDataLabelSettings.LabelStyle>
	///                    <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
	///                </chart:FunnelDataLabelSettings.LabelStyle>
	///            </chart:FunnelDataLabelSettings>
	///        </chart:SfFunnelChart.DataLabelSettings>
	///    </chart:SfFunnelChart>
	///
	/// ]]></code>
	/// # [C#](#tab/tabid-4)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// ViewModel viewModel = new ViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.ShowDataLabels= true;
	/// 
	/// var labelStyle = new ChartDataLabelStyle()
	/// {
	///     Background = new SolidColorBrush(Colors.Red),
	///     TextColor = Colors.Black,
	///     FontSize = 14,
	/// };
	/// 
	/// chart.DataLabelSettings = new FunnelDataLabelSettings()
	/// {
	///     LabelStyle = labelStyle,
	/// };
	/// 
	/// this.Content = chart;
	///
	/// ]]></code>
	/// ***
	/// 
	/// </remarks>
	public partial class FunnelDataLabelSettings : ChartDataLabelSettings, IPyramidDataLabelSettings
	{
		#region Internal Property

		internal SmartLabelAlignment SmartLabelAlignment { get; set; } = SmartLabelAlignment.Shift;

		#endregion

		#region Bindable Property

		/// <summary>
		/// Identifies the <see cref="Context"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Context"/> bindable property determines the context of the funnel data label.
		/// </remarks>
		public static readonly BindableProperty ContextProperty = BindableProperty.Create(
			nameof(Context),
			typeof(FunnelDataLabelContext),
			typeof(FunnelDataLabelSettings),
			FunnelDataLabelContext.YValue,
			BindingMode.Default,
			null,
			null);

		#endregion

		#region Public Property

		/// <summary>
		/// Gets or sets an option that determines the content to be displayed in the data labels.
		/// </summary>
		/// <value>Its accept the <see cref="FunnelDataLabelContext"/> values and its default value is <see cref="FunnelDataLabelContext.YValue"/>.</value>
		/// /// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///    <chart:SfFunnelChart ItemsSource ="{Binding Data}"
		///                          XBindingPath="XValue"
		///                          YBindingPath="YValue"
		///                          ShowDataLabels="True">
		///        <chart:SfFunnelChart.DataLabelSettings>
		///            <chart:FunnelDataLabelSettings Context="XValue"/>
		///        </chart:SfFunnelChart.DataLabelSettings>
		///    </chart:SfFunnelChart>
		///
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.ShowDataLabels= true;
		/// 
		/// chart.DataLabelSettings = new FunnelDataLabelSettings()
		/// {
		///     Context = FunnelDataLabelContext.XValue
		/// };
		/// 
		/// this.Content = chart;
		///
		/// ]]></code>
		/// ***
		/// </example>
		public FunnelDataLabelContext Context
		{
			get { return (FunnelDataLabelContext)GetValue(ContextProperty); }
			set { SetValue(ContextProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="FunnelDataLabelSettings"/>.
		/// </summary>
		public FunnelDataLabelSettings()
		{
			LabelStyle = new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(6, 4) };
		}

		#endregion

		#region Interface Implementation

		string IPyramidDataLabelSettings.GetLabelContent(object? xValue, double yValue)
		{
			string labelContent = string.Empty;

			if (Context == FunnelDataLabelContext.XValue && xValue != null)
			{
				labelContent = xValue.ToString() ?? string.Empty;
			}
			else
			{
				if (double.IsNaN(yValue))
				{
					return labelContent;
				}

				if (LabelStyle != null && !string.IsNullOrEmpty(LabelStyle.LabelFormat))
				{
					labelContent = yValue.ToString(LabelStyle.LabelFormat);
				}
				else
				{
					labelContent = yValue.ToString("#.##");
				}
			}

			return labelContent;
		}

		ChartDataLabelStyle IPyramidDataLabelSettings.LabelStyle => LabelStyle ?? new ChartDataLabelStyle();

		SmartLabelAlignment IPyramidDataLabelSettings.SmartLabelAlignment => SmartLabelAlignment;

		#endregion
	}

	internal interface IPyramidDataLabelSettings
	{
		#region Properties

		ChartDataLabelStyle LabelStyle { get; }

		bool UseSeriesPalette { get; }

		DataLabelPlacement LabelPlacement { get; }

		SmartLabelAlignment SmartLabelAlignment { get; }

		#endregion

		#region Method

		string GetLabelContent(object? xValue, double yValue);

		#endregion
	}
}