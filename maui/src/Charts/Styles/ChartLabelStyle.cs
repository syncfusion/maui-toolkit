using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Font = Microsoft.Maui.Font;
using Rect = Microsoft.Maui.Graphics.RectF;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// It is a base class for the <see cref="ChartAxisLabelStyle"/>, <see cref="ChartAxisTitle"/>, and <see cref="ChartDataLabelStyle"/> classes.
	/// </summary>
	/// <remarks>
	/// It provides more options to customize the label.
	/// 
	/// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="TextColor"/> property. </para>
	/// <para> <b>Background - </b> To customize the background color, refer to this <see cref="Background"/> property. </para>
	/// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="StrokeWidth"/> property. </para>
	/// <para> <b>Margin - </b> To adjust the outer margin for labels, refer to this <see cref="Margin"/> property. </para>
	/// <para> <b>LabelFormat - </b> To customize the label format for labels, refer to this <see cref="LabelFormat"/> property. </para>
	/// <para> <b>CornerRadius - </b> To defines the rounded corners for labels, refer to this <see cref="CornerRadius"/> property. </para>
	/// <para> <b>CornerRadius - </b> To change the text size for labels, refer to this <see cref="FontSize"/> property. </para>
	/// <para> <b>FontFamily - </b> To change the font family for labels, refer to this <see cref="FontFamily"/> property. </para>
	/// <para> <b>FontAttributes - </b> To change the font attributes for labels, refer to this <see cref="FontAttributes"/> property. </para>
	/// </remarks>
	public partial class ChartLabelStyle : Element, ITextElement, IThemeElement
	{
		//When a large font size is set, the labels associated with the chart label style may overlap
		//with the axis and series, so the FontAutoScalingEnabled API has been disabled.
		bool _fontAutoScalingEnabled;

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TextColor"/> bindable property determines the color of the text in the chart label.
		/// </remarks>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
			nameof(TextColor),
			typeof(Color),
			typeof(ChartLabelStyle),
			null,
			BindingMode.Default,
			null,
			OnTextColorChanged,
			null,
			defaultValueCreator: TextColorDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="Background"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Background"/> bindable property determines the background brush of the chart label.
		/// </remarks>
		public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
			nameof(Background),
			typeof(Brush),
			typeof(ChartLabelStyle),
			null,
			BindingMode.Default,
			null,
			OnBackgroundColorChanged,
			null,
			defaultValueCreator: BackgroundDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="Margin"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Margin"/> bindable property determines the margin around the chart label.
		/// </remarks>
		public static readonly BindableProperty MarginProperty = BindableProperty.Create(
			nameof(Margin),
			typeof(Thickness),
			typeof(ChartLabelStyle),
			null,
			BindingMode.Default,
			null,
			OnMarginChanged,
			null,
			null,
			defaultValueCreator: MarginDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width around the chart label.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(ChartLabelStyle),
			0d,
			BindingMode.Default,
			null,
			OnBorderThicknessChanged,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the brush used 
		/// for the stroke around the chart label.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(ChartLabelStyle),
			Brush.Transparent,
			BindingMode.Default,
			null,
			OnBorderColorChanged,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="FontSize"/> bindable property determines the size of the font used in the chart label.
		/// </remarks>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
			nameof(FontSize),
			typeof(double),
			typeof(ChartLabelStyle),
			1.0,
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
		/// The identifier for the <see cref="FontFamily"/> bindable property determines the font family used in the chart label.
		/// </remarks>
		public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="FontAttributes"/> bindable property determines the font attributes (e.g., bold, italic) 
		/// used in the chart label.
		/// </remarks>
		public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

		/// <summary>
		/// Identifies the <see cref="LabelFormat"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelFormat"/> bindable property determines the format string used for the chart label.
		/// </remarks>
		public static readonly BindableProperty LabelFormatProperty = BindableProperty.Create(
			nameof(LabelFormat),
			typeof(string),
			typeof(ChartLabelStyle),
			string.Empty,
			BindingMode.Default,
			null,
			OnLabelFormatChanged,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="CornerRadius"/> bindable property determines the corner radius of the chart label.
		/// </remarks>
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
			nameof(CornerRadius),
			typeof(CornerRadius),
			typeof(ChartLabelStyle),
			null,
			BindingMode.Default,
			null,
			OnCornerRadiusChanged,
			null);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value to customize the appearance of the label's text color.
		/// </summary>
		/// <value>It accepts <see cref="Color"/> values.</value>
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the appearance of the label's background.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values.</value>
		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the margin of the label.
		/// </summary>
		/// <value>It accepts <see cref="Thickness"/> values and the default value is 3.5.</value>
		public Thickness Margin
		{
			get { return (Thickness)GetValue(MarginProperty); }
			set { SetValue(MarginProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the stroke thickness of the label.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		/// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the outer stroke appearance of the label.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values.</value>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the label's format.
		/// </summary>
		/// <value>It accepts <see cref="string"/> values.</value>
		public string LabelFormat
		{
			get { return (string)GetValue(LabelFormatProperty); }
			set { SetValue(LabelFormatProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the label's size.
		/// </summary>
		/// <remarks>The value must be greater than zero and both <see cref="double.MinValue"/> and <see cref="double.MaxValue"/> are not valid.</remarks>
		/// <value>It accepts <see cref="double"/> values.</value>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the font family of the label.
		/// </summary>
		/// <value>It accepts <see cref="string"/> values.</value>
		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the font attributes of the label.
		/// </summary>
		/// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values.</value>
		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set { SetValue(FontAttributesProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the rounded corners for labels.
		/// </summary>
		/// <value>It accepts <see cref="Microsoft.Maui.CornerRadius"/> values and the default value is 0.</value>
		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		#endregion

		#region Internal Properties

		internal bool IsBackgroundColorUpdated { get; set; }

		internal bool IsStrokeColorUpdated { get; set; }

		internal bool IsTextColorUpdated { get; set; }

		internal bool HasCornerRadius { get; set; }

		internal Rect Rect { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartLabelStyle"/>.
		/// </summary>
		public ChartLabelStyle()
		{
			CornerRadius = new CornerRadius(0);
		}

		#endregion

		#region Methods

		#region Internal Methods

		internal bool CanDraw()
		{
			return IsBackgroundColorUpdated || IsStrokeColorUpdated;
		}

		internal virtual Color GetDefaultTextColor()
		{
			return Color.FromRgba(170, 170, 170, 255);
		}

		internal virtual Brush GetDefaultBackgroundColor()
		{
			return new SolidColorBrush(Colors.Transparent);
		}

		internal virtual double GetDefaultFontSize()
		{
			return 12;
		}

		internal virtual Thickness GetDefaultMargin()
		{
			return new Thickness(3.5);
		}

		#endregion

		#region DrawLabels

		internal void DrawBackground(ICanvas canvas, string label, Brush? fillColor, PointF point)
		{
			canvas.CanvasSaveState();
			DrawTextBackground(canvas, label, fillColor, point);
			canvas.CanvasRestoreState();
		}

		internal void DrawTextBackground(ICanvas canvas, string label, Brush? fillColor, PointF point)
		{
			var xPos = point.X;
			var yPos = point.Y;
			SizeF labelSize = MeasureLabel(label);
			var actualWidth = labelSize.Width;
			var actualHeight = labelSize.Height;
			var leftMargin = (float)(actualWidth / 2 + Margin.Left / 2 - Margin.Right / 2);
			var topMargin = (float)(actualHeight / 2 + Margin.Top / 2 - Margin.Bottom / 2);
			var backgroundRect = new Rect(xPos - leftMargin, yPos - topMargin, actualWidth, actualHeight);

			canvas.SetFillPaint(fillColor, backgroundRect);
			//Todo: Need to check condition for label background
			if (HasCornerRadius)
			{
				canvas.FillRoundedRectangle(backgroundRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
			}
			else
			{
				canvas.FillRectangle(backgroundRect);
			}

			//Todo: Need to check with border width and color in DrawLabel override method.
			if (StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()))
			{
				if (HasCornerRadius)
				{
					canvas.DrawRoundedRectangle(backgroundRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
				}
				else
				{
					canvas.DrawRectangle(backgroundRect);
				}
			}
		}

		internal SizeF MeasureLabel(string label)
		{
			Size size = label.Measure(this);
			Rect = new Rect() { Height = (float)size.Height, Width = (float)size.Width };

			return new SizeF(Rect.Width + (float)Margin.Left + (float)Margin.Right, Rect.Height + (float)Margin.Top + (float)Margin.Bottom);
		}

		internal void DrawLabel(ICanvas canvas, string label, PointF point)
		{
			//Added platform specific code for position the label.
#if ANDROID
			canvas.DrawText(label, point.X - Rect.Width / 2, point.Y + Rect.Height / 3, this);
#else
			canvas.DrawText(label, point.X - Rect.Width / 2, point.Y - Rect.Height / 2, this);
#endif
		}

		#endregion

		#region Private Methods

		static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartLabelStyle style)
			{
				var color = (Color)newValue;
				style.IsTextColorUpdated = color != null & color != Colors.Transparent;
			}

			if (bindable is ChartThemeLegendLabelStyle legendStyle)
			{
				legendStyle.UpdateItems();
			}
		}

		static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartLabelStyle style)
			{
				style.IsBackgroundColorUpdated = newValue is Brush;
			}
		}

		static void OnMarginChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartLabelStyle style)
			{
				var color = (SolidColorBrush)newValue;
				style.IsStrokeColorUpdated = color != null & color != Brush.Transparent;
			}
		}

		static void OnLabelFormatChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartLabelStyle style)
			{
				var radius = (CornerRadius)newValue;
				style.HasCornerRadius = radius.TopLeft > 0 || radius.TopRight > 0 || radius.BottomLeft > 0 || radius.BottomRight > 0;
			}
		}

		static object TextColorDefaultValueCreator(BindableObject bindable)
		{
			return ((ChartLabelStyle)bindable).GetDefaultTextColor();
		}

		static object BackgroundDefaultValueCreator(BindableObject bindable)
		{
			return ((ChartLabelStyle)bindable).GetDefaultBackgroundColor();
		}

		static object FontSizeDefaultValueCreator(BindableObject bindable)
		{
			return ((ChartLabelStyle)bindable).GetDefaultFontSize();
		}

		static object MarginDefaultValueCreator(BindableObject bindable)
		{
			return ((ChartLabelStyle)bindable).GetDefaultMargin();
		}

		#endregion

		#endregion

		#region implement interface
		Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

		bool ITextElement.FontAutoScalingEnabled { get => _fontAutoScalingEnabled; set => _fontAutoScalingEnabled = value; }

		IFontManager? ITextElement.GetFontManager() => ((IView)Parent).GetFontManager();


		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 9.5d;
		}

		void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
		{

		}

		void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
		{

		}

		void ITextElement.OnFontChanged(Font oldValue, Font newValue)
		{

		}

		void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
		{

		}

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{

		}

		#endregion
	}

	internal partial class ChartThemeLegendLabelStyle : ChartLegendLabelStyle, IParentThemeElement
	{
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public ChartThemeLegendLabelStyle(ChartBase chart)
		{
			SetLegendThemeValue(chart);
		}

		#endregion

		#region Interface Implementation

		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfChartCommonStyle();
		}

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}
		#endregion

		#region Methods

		internal void UpdateItems()
		{
			if (Parent is ChartBase chart)
			{
				chart.UpdateLegendItems();
			}
		}

		void SetLegendThemeValue(ChartBase chart)
		{
			if (chart is SfCartesianChart)
			{
				ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
				SetDynamicResource(TextColorProperty, "SfCartesianChartLegendTextColor");
				SetDynamicResource(FontSizeProperty, "SfCartesianChartLegendFontSize");
				SetDynamicResource(DisableBrushProperty, "SfCartesianChartLegendDisableBrush");
			}

			if (chart is SfCircularChart)
			{
				ThemeElement.InitializeThemeResources(this, "SfCircularChartTheme");
				SetDynamicResource(TextColorProperty, "SfCircularChartLegendTextColor");
				SetDynamicResource(FontSizeProperty, "SfCircularChartLegendFontSize");
				SetDynamicResource(DisableBrushProperty, "SfCircularChartLegendDisableBrush");
			}

			if (chart is SfPyramidChart)
			{
				ThemeElement.InitializeThemeResources(this, "SfPyramidChartTheme");
				SetDynamicResource(TextColorProperty, "SfPyramidChartLegendTextColor");
				SetDynamicResource(FontSizeProperty, "SfPyramidChartLegendFontSize");
				SetDynamicResource(DisableBrushProperty, "SfPyramidChartLegendDisableBrush");
			}

			if (chart is SfFunnelChart)
			{
				ThemeElement.InitializeThemeResources(this, "SfFunnelChartTheme");
				SetDynamicResource(TextColorProperty, "SfFunnelChartLegendTextColor");
				SetDynamicResource(FontSizeProperty, "SfFunnelChartLegendFontSize");
				SetDynamicResource(DisableBrushProperty, "SfFunnelChartLegendDisableBrush");
			}

			if (chart is SfPolarChart)
			{
				ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
				SetDynamicResource(TextColorProperty, "SfPolarChartLegendTextColor");
				SetDynamicResource(FontSizeProperty, "SfPolarChartLegendFontSize");
				SetDynamicResource(DisableBrushProperty, "SfPolarChartLegendDisableBrush");
			}
		}

		#endregion
	}
}
