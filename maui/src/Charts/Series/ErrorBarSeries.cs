using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="ErrorBarSeries"/> indicate the uncertainty or error in data points, making it easy to identify patterns and trends in the data.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of ErrorBarSeries, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// <para>The <see cref="ErrorBarSeries"/> had no tooltip, data label, animation, and selection support.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.XAxes>
	///               <chart:CategoryAxis/>
	///           </chart:SfCartesianChart.XAxes>
	///
	///           <chart:SfCartesianChart.YAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.YAxes>
	///
	///           <chart:SfCartesianChart.Series>
	///              <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
	///                                    XBindingPath="Name"   
	///                                    YBindingPath="Value"/>
	///           </chart:SfCartesianChart.Series>  
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     NumericalAxis xAxis = new CategoryAxis();
	///     NumericalAxis yAxis = new NumericalAxis();
	///     
	///     chart.XAxes.Add(xAxis);
	///     chart.YAxes.Add(yAxis);
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     ErrorBarSeries series = new ErrorBarSeries();
	///     series.ItemsSource = viewModel.ThermalExpansion;
	///     series.XBindingPath = "Name";
	///     series.YBindingPath = "Value";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<Model> ThermalExpansion { get; set; }
	/// 
	///     public ViewModel()
	///     {
	///        ThermalExpansion = new ObservableCollection<Model>();
	///        ThermalExpansion.Add(new Model() { Name="Erbium",Value=8.2,High=7.6 });
	///        ThermalExpansion.Add(new Model() { Name="Samarium",Value=8.15,High=5.7 });
	///        ThermalExpansion.Add(new Model() { Name="Yttritium",Value=7.15,High=6.8 });
	///        ThermalExpansion.Add(new Model() { Name="Carbide",Value=6.45,High=5.9 });
	///        ThermalExpansion.Add(new Model() { Name="Uranium",Value=7.45,High=7.1 });
	///        ThermalExpansion.Add(new Model() { Name="Iron",Value=6.7,High=5 });
	///        ThermalExpansion.Add(new Model() { Name="Thuilium",Value=8.45,High=7.1 });
	///        ThermalExpansion.Add(new Model() { Name="Steel",Value=9.7,High=8.6});
	///        ThermalExpansion.Add(new Model() { Name="Tin",Value=14.6,High=10.8 });
	///        ThermalExpansion.Add(new Model() { Name="Uranium",Value=7.45,High=7.1 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public class ErrorBarSeries : XYDataSeries, IDrawCustomLegendIcon
    {
        #region Internal Properties

        internal IList<double> HorizontalErrorValues { get; set; }

        internal IList<double> VerticalErrorValues { get; set; }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="HorizontalErrorPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalErrorPath"/> property specifies the data source path used to retrieve horizontal error 
        /// values for the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty HorizontalErrorPathProperty = BindableProperty.Create(
            nameof(HorizontalErrorPath),
            typeof(string),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
            propertyChanged: OnErrorPathChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalErrorPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalErrorPath"/> property specifies the data source path used to retrieve vertical error 
        /// values for the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty VerticalErrorPathProperty = BindableProperty.Create(
            nameof(VerticalErrorPath),
            typeof(string),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
            propertyChanged: OnErrorPathChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalErrorValue"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalErrorValue"/> property specifies the fixed horizontal error value applied 
        /// to all data points in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty HorizontalErrorValueProperty = BindableProperty.Create(
            nameof(HorizontalErrorValue),
            typeof(double),
            typeof(ErrorBarSeries),
            0d,
            BindingMode.Default,
            propertyChanged: OnErrorValueChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalErrorValue"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalErrorValue"/> property specifies the fixed vertical error value applied
        /// to all data points in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty VerticalErrorValueProperty = BindableProperty.Create(
            nameof(VerticalErrorValue),
            typeof(double),
            typeof(ErrorBarSeries),
            0d,
            BindingMode.Default,
            propertyChanged: OnErrorValueChanged);

        /// <summary>
        /// Identifies the <see cref="Mode"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Mode"/> property defines whether the error bars display both horizontal and vertical errors, 
        /// or only one of them in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty ModeProperty = BindableProperty.Create(
            nameof(Mode),
            typeof(ErrorBarMode),
            typeof(ErrorBarSeries),
            ErrorBarMode.Both,
            BindingMode.Default,
            propertyChanged: OnModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Type"/> property specifies whether the error bars in the <see cref="ErrorBarSeries"/> represent
        /// a standard deviation, a standard error, a percentage, or a fixed value.
        /// </remarks>
        public static readonly BindableProperty TypeProperty = BindableProperty.Create(
            nameof(Type),
            typeof(ErrorBarType),
            typeof(ErrorBarSeries),
            ErrorBarType.Fixed,
            BindingMode.Default,
            propertyChanged: OnTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalDirection"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalDirection"/> property specifies whether the horizontal error bars in the <see cref="ErrorBarSeries"/> should 
        /// display positive, negative, or both directions of the error values.
        /// </remarks>
        public static readonly BindableProperty HorizontalDirectionProperty = BindableProperty.Create(
            nameof(HorizontalDirection),
            typeof(ErrorBarDirection),
            typeof(ErrorBarSeries),
            ErrorBarDirection.Both,
            BindingMode.Default,
            propertyChanged: OnHorizontalDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalDirection"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalDirection"/> property specifies whether the vertical error bars in the <see cref="ErrorBarSeries"/> should
        /// display positive, negative, or both directions of the error values.
        /// </remarks>
        public static readonly BindableProperty VerticalDirectionProperty = BindableProperty.Create(
            nameof(VerticalDirection),
            typeof(ErrorBarDirection),
            typeof(ErrorBarSeries),
            ErrorBarDirection.Both,
            BindingMode.Default,
            propertyChanged: OnVerticalDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalLineStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalLineStyle"/> property allows customization of the horizontal error bars in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty HorizontalLineStyleProperty = BindableProperty.Create(
            nameof(HorizontalLineStyle),
            typeof(ErrorBarLineStyle),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
            propertyChanged: OnLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalLineStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalLineStyle"/> property allows customization of the vertical error bar in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty VerticalLineStyleProperty = BindableProperty.Create(
            nameof(VerticalLineStyle),
            typeof(ErrorBarLineStyle),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
             propertyChanged: OnLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalCapLineStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalCapLineStyle"/> property allows customization of the horizontal caps in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty HorizontalCapLineStyleProperty = BindableProperty.Create(
            nameof(HorizontalCapLineStyle),
            typeof(ErrorBarCapLineStyle),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
            propertyChanged: OnLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalCapLineStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalCapLineStyle"/> property allows customization of the vertical caps in the <see cref="ErrorBarSeries"/>.
        /// </remarks>
        public static readonly BindableProperty VerticalCapLineStyleProperty = BindableProperty.Create(
            nameof(VerticalCapLineStyle),
            typeof(ErrorBarCapLineStyle),
            typeof(ErrorBarSeries),
            null,
            BindingMode.Default,
            propertyChanged: OnLineStyleChanged);

        #endregion

        #region Public  Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a horizontal error value to the series.
        /// </summary>
        /// <value>It accepts <see cref="string"/> and the default <c>String.Empty</c>.</value>
        /// <remarks>
        /// If the <see cref="HorizontalErrorPath"/> is set, the <see cref="HorizontalErrorValue"/> will be ignored when type is <see cref="ErrorBarType.Custom"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name" 
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorPath = "Low"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorPath = "Low"
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string HorizontalErrorPath
        {
            get { return (string)GetValue(HorizontalErrorPathProperty); }
            set { SetValue(HorizontalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a vertical error value to the series.
        /// </summary>
        /// <value>It accepts <see cref="string"/>and the default is <c>String.Empty</c>.</value>
        /// <remarks>
        /// If the <see cref="VerticalErrorPath"/> is set, the <see cref="VerticalErrorValue"/> will be ignored when type is <see cref="ErrorBarType.Custom"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///      <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"    
        ///                            YBindingPath = "Value"
        ///                            XBindingPath = "Name"
        ///                            VerticalErrorPath = "High"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           VerticalErrorPath = "High"
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string VerticalErrorPath
        {
            get { return (string)GetValue(VerticalErrorPathProperty); }
            set { SetValue(VerticalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the horizontal errors of the series.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalErrorValue"/> works when there is no <see cref="HorizontalErrorPath"/> set and the <see cref="Mode"/> is <see cref="ErrorBarMode.Both"/> or <see cref="ErrorBarMode.Horizontal"/>.
        /// </remarks>
        /// <value>It accepts <see cref="double"/> and the default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double HorizontalErrorValue
        {
            get { return (double)GetValue(HorizontalErrorValueProperty); }
            set { SetValue(HorizontalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the vertical errors of the series.
        /// </summary>
        ///  <value>It accepts <see cref="double"/> and the default is 0.</value>
        /// <remarks>
        /// The <see cref="VerticalErrorValue"/> works when there is no <see cref="VerticalErrorPath"/> set and the <see cref="Mode"/> is <see cref="ErrorBarMode.Both"/> or <see cref="ErrorBarMode.Vertical"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 VerticalErrorValue = "5"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           VerticalErrorValue = 5
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double VerticalErrorValue
        {
            get { return (double)GetValue(VerticalErrorValueProperty); }
            set { SetValue(VerticalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode of the error bar to be displayed, whether horizontal, vertical, or both.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarMode"/> values and its defaults is <see cref="ErrorBarMode.Both"/></value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///      <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                            XBindingPath = "Name"   
        ///                            YBindingPath = "Value"
        ///                            HorizontalErrorValue = 0.25
        ///                            VerticalErrorValue = 5
        ///                            Mode = "Horizontal"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5,
        ///           Mode = ErrorBarMode.Horizontal
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarMode Mode
        {
            get { return (ErrorBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the error bar type, whether it is specified as a standard deviation, a standard error, a percentage, or a fixed value.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarType"/> values and its defaults is <see cref="ErrorBarType.Fixed"/></value>
        /// <example>
        /// # [Xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25"
        ///                                 VerticalErrorValue = "5"
        ///                                 Type="Percentage"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5,
        ///           Type = ErrorBarType.Percentage
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarType Type
        {
            get { return (ErrorBarType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction to specify whether to show positive, negative or both directions of horizontal error values to display.
        /// </summary>
        /// <value>It accept the <see cref="ErrorBarDirection"/> values and its defaults is <see cref="ErrorBarDirection.Both"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25",
        ///                                 VerticalErrorValue = "5"
        ///                                 HorizontalDirection = "Minus"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///           HorizontalDirection = ErrorBarDirection.Minus
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarDirection HorizontalDirection
        {
            get { return (ErrorBarDirection)GetValue(HorizontalDirectionProperty); }
            set { SetValue(HorizontalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction to specify whether to show positive, negative or both directions of vertical error values to display.
        /// </summary>
        /// <value>It accept the <see cref="ErrorBarDirection"/> values and its defaults is <see cref="ErrorBarDirection.Both"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25",
        ///                                 VerticalErrorValue = "5"
        ///                                 VerticalDirection = "Both"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///           VerticalDirection = ErrorBarDirection.Both
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarDirection VerticalDirection
        {
            get { return (ErrorBarDirection)GetValue(VerticalDirectionProperty); }
            set { SetValue(VerticalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal lines, and it is often used to customize the appearance of horizontal error bars for visual purposes.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarLineStyle"/> values and its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-20)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25",
        ///                                 VerticalErrorValue = "5"/>
        ///                                                     
        ///                    <chart:ErrorBarSeries.HorizontalLineStyle>
        ///                   <chart:ErrorBarLineStyle Stroke = "Black"></ chart:ErrorBarLineStyle>
        ///          </chart:ErrorBarSeries.HorizontalLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-21)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///     };
        ///     errorBarSeries.HorizontalLineStyle = new ErrorBarLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarLineStyle HorizontalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical lines, and it is often used to customize the appearance of the vertical error bar for visual purposes.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarLineStyle"/> values and its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-22)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25"
        ///                                 VerticalErrorValue = "5"/>
        ///                                                      
        ///                    <chart:ErrorBarSeries.VerticalLineStyle>
        ///                   <chart:ErrorBarLineStyle Stroke = "Black"></ chart:ErrorBarLineStyle>
        ///          </chart:ErrorBarSeries.VerticalLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-23)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///     };
        ///     errorBarSeries.VerticalLineStyle = new ErrorBarLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarLineStyle VerticalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal caps, and it is often used to customize the appearance of horizontal caps in the error bar for visual purposes.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarCapLineStyle"/> values and its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-24)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25"
        ///                                 VerticalErrorValue = "5"/>                   
        ///                    <chart:ErrorBarSeries.HorizontalCapLineStyle>
        ///                   <chart:ErrorBarCapLineStyle Stroke="Black"></chart:ErrorBarCapLineStyle>
        ///          </chart:ErrorBarSeries.HorizontalCapLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-25)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///     };
        ///     errorBarSeries.HorizontalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarCapLineStyle HorizontalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(HorizontalCapLineStyleProperty); }
            set { SetValue(HorizontalCapLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical caps, and it is often used to customize the appearance of vertical caps in the error bar for visual purposes.
        /// </summary>
        /// <value>It accepts the <see cref="ErrorBarCapLineStyle"/> values and its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-26)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource = "{Binding ThermalExpansion}"   
        ///                                 XBindingPath = "Name"   
        ///                                 YBindingPath = "Value"
        ///                                 HorizontalErrorValue = "0.25"
        ///                                 VerticalErrorValue = "5"/>
        ///                                                      
        ///                    <chart:ErrorBarSeries.VerticalCapLineStyle>
        ///                   <chart:ErrorBarCapLineStyle Stroke="Black" CapSize="20"></chart:ErrorBarCapLineStyle>
        ///          </chart:ErrorBarSeries.VerticalCapLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-27)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.ThermalExpansion,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue = 0.25,
        ///           VerticalErrorValue = 5
        ///     };
        ///     errorBarSeries.VerticalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///          CapSize=20;
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarCapLineStyle VerticalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(VerticalCapLineStyleProperty); }
            set { SetValue(VerticalCapLineStyleProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSeries"/> class.
        /// </summary>
        public ErrorBarSeries()
        {
            HorizontalErrorValues = new List<double>();
            VerticalErrorValues = new List<double>();

            HorizontalLineStyle = new ErrorBarLineStyle();
            VerticalLineStyle = new ErrorBarLineStyle();
            HorizontalCapLineStyle = new ErrorBarCapLineStyle();
            VerticalCapLineStyle = new ErrorBarCapLineStyle();
        }

        #endregion

        #region Interface Implementation

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            RectF innerRect1 = new(2, 2, 3, 1);
            canvas.SetFillPaint(fillColor, innerRect1);
            canvas.FillRectangle(innerRect1);

            RectF innerRect2 = new(8, 0, 3, 1);
            canvas.SetFillPaint(fillColor, innerRect2);
            canvas.FillRectangle(innerRect2);

            RectF innerRect3 = new(2, 10, 3, 1);
            canvas.SetFillPaint(fillColor, innerRect3);
            canvas.FillRectangle(innerRect3);

            RectF innerRect4 = new(8, 8, 3, 1);
            canvas.SetFillPaint(fillColor, innerRect4);
            canvas.FillRectangle(innerRect4);

            RectF innerRect5 = new(3, 3, 1, 8);
            canvas.SetFillPaint(fillColor, innerRect5);
            canvas.FillRectangle(innerRect5);

            RectF innerRect6 = new(9, 1, 1, 8);
            canvas.SetFillPaint(fillColor, innerRect6);
            canvas.FillRectangle(innerRect6);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Methods

        #region Protected  Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new ErrorBarSegment();
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (HorizontalLineStyle != null)
            {
                SetInheritedBindingContext(HorizontalLineStyle, BindingContext);
            }

            if (VerticalLineStyle != null)
            {
                SetInheritedBindingContext(VerticalLineStyle, BindingContext);
            }

            if (HorizontalCapLineStyle != null)
            {
                SetInheritedBindingContext(HorizontalCapLineStyle, BindingContext);
            }

            if (VerticalCapLineStyle != null)
            {
                SetInheritedBindingContext(VerticalCapLineStyle, BindingContext);
            }
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }

            if (Segments.Count == 0)
            {
                var segment = CreateSegment() as ErrorBarSegment;
                if (segment != null)
                {
                    segment.Series = this;
                    segment.SeriesView = seriesView;
                    segment.Item = ActualData;
                    segment.SetData(xValues, (IList)YValues);
                    Segments.Add(segment);
                }
            }
        }

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            HorizontalErrorValues?.Clear();
            VerticalErrorValues?.Clear();
            YValues.Clear();
            GeneratePoints();
            ScheduleUpdateChart();
        }

        internal override void OnBindingPathChanged()
        {
            ResetData();
            HorizontalErrorValues?.Clear();
            VerticalErrorValues?.Clear();
            YValues.Clear();
            GeneratePoints();
            SegmentsCreated = false;

            if (Chart != null)
                Chart.IsRequiredDataLabelsMeasure = true;

            ScheduleUpdateChart();
        }

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                bool yPathDecision = Type is ErrorBarType.Custom ? true : false;
                return yPathDecision;
            }
        }

        internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
        {
            if (XValues is IList<double>)
            {
                ((IList<double>)XValues).RemoveAt(index);
                PointsCount--;
            }
            else if (XValues is IList<string>)
            {
                ((IList<string>)XValues).RemoveAt(index);
                PointsCount--;
            }

            SeriesYValues?[0].RemoveAt(index);
            ActualData?.RemoveAt(index);
        }

        #endregion

        #region Private Methods

        static void OnErrorPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnErrorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnHorizontalDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;

                if (series.Chart != null)
                    series.Chart.IsRequiredDataLabelsMeasure = true;

                series.ScheduleUpdateChart();
            }
        }

        static void OnVerticalDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ErrorBarLineStyles_PropertyChanged;
                SetInheritedBindingContext(oldValue, null);
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ErrorBarLineStyles_PropertyChanged; ;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        void ErrorBarLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        void GeneratePoints()
        {
            if (YBindingPath is not null && HorizontalErrorPath is not null && VerticalErrorPath is not null)
            {
                GeneratePoints(new[] { YBindingPath, HorizontalErrorPath, VerticalErrorPath }, YValues, HorizontalErrorValues!, VerticalErrorValues!);
            }
            else if (YBindingPath != null && HorizontalErrorPath != null)
            {
                GeneratePoints(new[] { YBindingPath, HorizontalErrorPath }, YValues, HorizontalErrorValues!);
            }
            else if (YBindingPath != null && VerticalErrorPath != null)
            {
                GeneratePoints(new[] { YBindingPath, VerticalErrorPath }, YValues, VerticalErrorValues!);
            }
            else if (YBindingPath != null)
            {
                GeneratePoints(new[] { YBindingPath }, YValues);
            }
        }

        #endregion

        #endregion
    }
}