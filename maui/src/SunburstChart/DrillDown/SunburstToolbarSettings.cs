using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
	/// <summary>
	/// Represents the settings for the drill down functionality in the SfSunburstChart.
	/// </summary>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <chart:SfSunburstChart EnableDrillDown="True">
	///     <chart:SfSunburstChart.ToolbarSettings>
	///         <chart:SunburstToolbarSettings />
	///     <chart:SfSunburstChart.ToolbarSettings>
	/// <chart:SfSunburstChart>
	/// ]]>
	/// </code>
	/// </example>
	public class SunburstToolbarSettings : Element
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OffsetXProperty =BindableProperty.Create(
            nameof(OffsetX), 
            typeof(double), 
            typeof(SunburstToolbarSettings), 
            0.0d, 
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OffsetYProperty = BindableProperty.Create(
            nameof(OffsetY),
            typeof(double),
            typeof(SunburstToolbarSettings),
            0.0d,
            BindingMode.Default); 

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalAlignmentProperty = BindableProperty.Create(
            nameof(HorizontalAlignment),
            typeof(SunburstToolbarAlignment),
            typeof(SunburstToolbarSettings),
            SunburstToolbarAlignment.End, 
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalAlignmentProperty = BindableProperty.Create(
            nameof(VerticalAlignment),
            typeof(SunburstToolbarAlignment),
            typeof(SunburstToolbarSettings),
            SunburstToolbarAlignment.Start,
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
            nameof(Background),
            typeof(Brush),
            typeof(SunburstToolbarSettings),
            new SolidColorBrush(Color.FromArgb("#F7F2FB")),
            BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="IconBrush"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IconBrushProperty = BindableProperty.Create(
            nameof(IconBrush),
            typeof(Brush),
            typeof(SunburstToolbarSettings),
            new SolidColorBrush(Color.FromArgb("#49454F")),
            BindingMode.Default);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the horizontal offset for the toolbar.
		/// </summary>
		/// <value>The default value is 0.0.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings OffsetX="0.5"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

		/// <summary>
		/// Gets or sets the vertical offset for the toolbar.
		/// </summary>
		/// <value>The default value is 0.0.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings OffsetY="0.5"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

		/// <summary>
		/// Gets or sets the horizontal alignment of the toolbar.
		/// </summary>
		/// <value>The default value is SunburstToolbarAlignment.End.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings SunburstToolbarAlignment="Center"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public SunburstToolbarAlignment HorizontalAlignment
        {
            get { return (SunburstToolbarAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

		/// <summary>
		/// Gets or sets the vertical alignment of the toolbar.
		/// </summary>
		/// <value>The default value is VerticalAlignment.Start.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings VerticalAlignment="End"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public SunburstToolbarAlignment VerticalAlignment
        {
            get { return (SunburstToolbarAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicates the background of the drill-down toolbar.
		/// </summary>
		/// <value>The default value is #F7F2FB.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings Background="Red"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicates the background of the drill-down toolbar icons.
		/// </summary>
		/// <value>The default value is #49454F.</value>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// <chart:SfSunburstChart EnableDrillDown="True">
		///     <chart:SfSunburstChart.ToolbarSettings>
		///         <chart:SunburstToolbarSettings IconBrush="Green"/>
		///     <chart:SfSunburstChart.ToolbarSettings>
		/// <chart:SfSunburstChart>
		/// ]]>
		/// </code>
		/// </example>
		public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstToolbarSettings"/> class.
        /// </summary>
        public SunburstToolbarSettings()
        {
            // Default constructor
        }

        #endregion

    }
}