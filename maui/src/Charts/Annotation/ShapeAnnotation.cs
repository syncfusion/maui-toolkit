using Syncfusion.Maui.Toolkit.Themes;
using PointF = Microsoft.Maui.Graphics.PointF;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// ShapeAnnotation allows you to add annotations in the form of shapes such as rectangles, ellipses, lines, horizontal lines, and vertical lines at specific areas of interest within the chart area.
	/// </summary>
	public abstract class ShapeAnnotation : ChartAnnotation, IThemeElement
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="X2"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="X2"/> property sets the second X-coordinate for the <see cref="ShapeAnnotation"/> in the chart.
		/// </remarks>
		public static readonly BindableProperty X2Property = BindableProperty.Create(
			nameof(X2),
			typeof(object),
			typeof(ShapeAnnotation),
			null,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Y2"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Y2"/> property sets the second Y-coordinate for the <see cref="ShapeAnnotation"/> in the chart.
		/// </remarks>
		public static readonly BindableProperty Y2Property = BindableProperty.Create(
			nameof(Y2),
			typeof(double),
			typeof(ShapeAnnotation),
			double.NaN,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Text"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Text"/> property defines the text content displayed within the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			nameof(Text),
			typeof(string),
			typeof(ShapeAnnotation),
			string.Empty,
			BindingMode.Default,
			null,
			OnAnnotationPropertyInvalidate);

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Fill"/> property sets the fill color of the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty FillProperty = BindableProperty.Create(
			nameof(Fill),
			typeof(Brush),
			typeof(ShapeAnnotation),
			Brush.Default,
			BindingMode.Default,
			null,
			OnAnnotationPropertyInvalidate,
			defaultValueCreator: FillDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Stroke"/> property defines the brush for the border or outline of the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(ShapeAnnotation),
			Brush.Default,
			BindingMode.Default,
			null,
			OnAnnotationPropertyInvalidate,
			defaultValueCreator: StrokeDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="StrokeWidth"/> property sets the thickness of the border or outline of the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(ShapeAnnotation),
			1d,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeDashArray"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="StrokeDashArray"/> property defines the dash pattern used for the border or outline of the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
			nameof(StrokeDashArray),
			typeof(DoubleCollection),
			typeof(ShapeAnnotation),
			null,
			BindingMode.Default,
			null,
			OnAnnotationPropertyInvalidate);

		/// <summary>
		/// Identifies the <see cref="LabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LabelStyle"/> property allows customization of the appearance of the label associated with the <see cref="ShapeAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
			nameof(LabelStyle),
			typeof(ChartAnnotationLabelStyle),
			typeof(ShapeAnnotation),
			null,
			BindingMode.Default,
			null,
			OnLabelStylePropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the DateTime or double value that represents the x2 position of the annotation.
		/// </summary>
		/// <value>This property takes the <c>object</c> as its value and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart.Annotations>
		///   <chart:RectangleAnnotation X1="3" Y1="20" X2="4" Y2="40">
		///   </chart:RectangleAnnotation>
		/// </chart:SfCartesianChart.Annotations>  
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		///  SfCartesianChart chart = new SfCartesianChart();
		///  var rectangle = new RectangleAnnotation()
		///  {
		///    X1 = 3,
		///    Y1 = 20,
		///    X2 = 4,
		///    Y2 = 40,
		///  };
		///  
		/// chart.Annotations.Add(rectangle);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public object X2
		{
			get { return (object)GetValue(X2Property); }
			set { SetValue(X2Property, value); }
		}

		/// <summary>
		/// Gets or sets the double value that represents the y2 position of the annotation.
		/// </summary>
		/// <value>This property takes the <c>double</c> as its value and its default value is double.NaN.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:LineAnnotation X1="1" Y1="10" X2="4" Y2="20"/>
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
		///     var line = new LineAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 20,   
		///     };
		///  
		///     chart.Annotations.Add(line);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double Y2
		{
			get { return (double)GetValue(Y2Property); }
			set { SetValue(Y2Property, value); }
		}

		/// <summary>
		/// Gets or sets the string value that represents the text to be displayed on the annotation.
		/// </summary>
		/// <value>This property takes the <c>string</c> as its value and its default value is string.Empty.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:HorizontalLineAnnotation Y1="10" Text="Text"/>
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
		///     var horizontalAnnotation = new HorizontalLineAnnotation()
		///     {
		///         Y1 = 10,
		///         Text = "Text",
		///     };
		///  
		///     chart.Annotations.Add(horizontalAnnotation);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the fill color of the shape annotation.
		/// </summary>
		/// <value>This property takes the <see cref="Brush"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:RectangleAnnotation X1="1" Y1="10" X2="4" Y2="20" Fill="Green"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-8)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var rectangle = new RectangleAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 20,
		///         Fill = Colors.Green,
		///     };
		///  
		///     chart.Annotations.Add(rectangle);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke for the annotation.
		/// </summary>
		/// <value>This property takes the <see cref="Brush"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1" Y1="10" Stroke="Red"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-10)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         Stroke = Colors.Red,
		///     };
		///  
		///     chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the width of the annotations stroke.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		/// <value>This property takes the <c>double</c> as its value and its default value is 1</value>
		/// <example>
		/// # [Xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1" Y1="10" StrokeWidth="5"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-12)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         StrokeWidth = 5,
		///     };
		///  
		///     chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the appearance of the annotation border.
		/// </summary>
		/// <value>This property takes the <see cref="DoubleCollection"/> as its value and its default value is null</value>
		/// <example>
		/// # [Xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1" Y1="10" StrokeDashArray="5,3"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         StrokeDashArray = new [] {5,3},
		///     };
		///  
		///     chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DoubleCollection StrokeDashArray
		{
			get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
			set { SetValue(StrokeDashArrayProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to customize the appearance of shape annotation text.
		/// </summary>
		/// <value>This property takes the <see cref="ChartAnnotationLabelStyle"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:HorizontalLineAnnotation X1="1" Y1="10" X2="4" Y2="20">
		///           <chart:HorizontalLineAnnotation.LabelStyle>
		///             <chart:ChartAnnotationLabelStyle  HorizontalTextAlignment="Start" VerticalTextAlignment="Start"/>
		///           </chart:HorizontalLineAnnotation.LabelStyle>
		///          </chart:HorizontalLineAnnotation>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var horizontalAnnotation = new HorizontalLineAnnotation()
		///     {
		///         X1 = 1,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 20,
		///     };
		///  
		///    horizontalAnnotation.LabelStyle = new ChartAnnotationLabelStyle()
		///    {
		///       HorizontalTextAlignment = ChartLabelAlignment.Start,
		///       VerticalTextAlignment = ChartLabelAlignment.Start,
		///    };
		///  
		///   chart.Annotations.Add(horizontalAnnotation);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartAnnotationLabelStyle LabelStyle
		{
			get { return (ChartAnnotationLabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

		#endregion

		#region Internal Properties

		internal Rect RenderRect { get; set; }

		internal Rect LabelRect { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ShapeAnnotation"/> class.
		/// </summary>
		public ShapeAnnotation()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
		}

		#endregion

		#region Methods

		#region Theme Interface Implementation

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				DrawLabelBackground(canvas, LabelRect);
				DrawText(canvas, Text, (float)LabelRect.X, (float)LabelRect.Y);
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (_annotationLabelStyle != null)
			{
				SetInheritedBindingContext(_annotationLabelStyle, BindingContext);
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (_annotationLabelStyle != null)
			{
				_annotationLabelStyle.Parent = Parent;
				_annotationLabelStyle.IsShapeAnnotation = this is ShapeAnnotation;
			}
		}

		internal override void Dispose()
		{
			UnHookStylePropertyChanged(_annotationLabelStyle);
			base.Dispose();
		}
		#endregion

		#region Internal Methods

		internal virtual void SetTextAlignment(double x, double y)
		{
			if (_annotationLabelStyle == null)
			{
				return;
			}

			var labelSize = _annotationLabelStyle.MeasureLabel(Text);

			double halfBorderWidth = (float)(_annotationLabelStyle.StrokeWidth / 2);
			double labelHeight = labelSize.Height;
			double labelWidth = labelSize.Width;
			double labelRectX = x;
			double labelRectY = y;
			double annotationX = RenderRect.X;
			double annotationY = RenderRect.Y;
			double annotationHeight = RenderRect.Height;
			double annotationWidth = RenderRect.Width;

			switch (_annotationLabelStyle.VerticalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectY = annotationY - halfBorderWidth - (labelHeight / 2);
					break;
				case ChartLabelAlignment.Center:
					labelRectY = annotationY + (annotationHeight / 2);
					break;
				case ChartLabelAlignment.End:
					labelRectY = annotationY + annotationHeight + halfBorderWidth + (labelHeight / 2);
					break;
			}

			switch (_annotationLabelStyle.HorizontalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectX = annotationX - (labelWidth / 2) - halfBorderWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectX = annotationX + (annotationWidth / 2);
					break;
				case ChartLabelAlignment.End:
					labelRectX = annotationX + annotationWidth + halfBorderWidth + (labelWidth / 2);
					break;
			}

			LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
		}

		internal virtual Brush GetDefaultFillColor()
		{
			return new SolidColorBrush(Color.FromArgb("#146750A4"));
		}

		internal virtual Brush GetDefaultStrokeColor()
		{
			return new SolidColorBrush(Color.FromArgb("#6750A4"));
		}

		internal void InitializeDynamicResource(ShapeAnnotation annotation)
		{
			if (annotation is LineAnnotation)
			{
				SetDynamicResource(StrokeProperty, "SfCartesianChartLineAnnotationStroke");
			}
			else
			{
				SetDynamicResource(FillProperty, "SfCartesianChartShapeAnnotationFill");
				SetDynamicResource(StrokeProperty, "SfCartesianChartShapeAnnotationStroke");
			}
		}

		#endregion

		#region Private Methods

		void DrawText(ICanvas canvas, string text, float x, float y)
		{
			_annotationLabelStyle.DrawLabel(canvas, text, new PointF(x, y));
		}

		void DrawLabelBackground(ICanvas canvas, RectF labelRect)
		{
			if (this is LineAnnotation lineAnnotation)
			{
				canvas.Rotate(lineAnnotation.Angle, labelRect.X, labelRect.Y);
			}

			ShapeAnnotation.DrawTextBackground(canvas, labelRect, Text, _annotationLabelStyle);
		}

		static void DrawTextBackground(ICanvas canvas, RectF labelRect, string text, ChartAnnotationLabelStyle labelStyle)
		{
			canvas.StrokeSize = (float)labelStyle.StrokeWidth;
			canvas.StrokeColor = labelStyle.Stroke.ToColor();
			labelStyle.DrawTextBackground(canvas, text, labelStyle.Background, new PointF(labelRect.X, labelRect.Y));
		}

		static object FillDefaultValueCreator(BindableObject bindable)
		{
			return ((ShapeAnnotation)bindable).GetDefaultFillColor();
		}

		static object StrokeDefaultValueCreator(BindableObject bindable)
		{
			return ((ShapeAnnotation)bindable).GetDefaultStrokeColor();
		}

		#endregion

		#endregion
	}
}