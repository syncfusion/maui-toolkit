using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Serves as a base class for all types of annotations such as text, shape, and view. The annotations can be added to the chart.
	/// </summary>
	public abstract class ChartAnnotation : Element
	{
		#region Fields

		SfCartesianChart? _chart;
		internal ChartAnnotationLabelStyle _annotationLabelStyle;

		#endregion

		#region Internal properties

		internal SfCartesianChart? Chart
		{
			get { return _chart; }

			set
			{
				_chart = value;
				if (value == null)
				{
					Dispose();
				}
			}
		}

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IsVisible"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsVisible"/> property determines annotation visibility in the <see cref="SfCartesianChart"/>.
		/// </remarks>
		public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
			nameof(IsVisible),
			typeof(bool),
			typeof(ChartAnnotation),
			true,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="X1"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="X1"/> property sets the X-coordinate for the annotation.
		/// </remarks>
		public static readonly BindableProperty X1Property = BindableProperty.Create(
			nameof(X1),
			typeof(object),
			typeof(ChartAnnotation),
			null,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Y1"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Y1"/> property sets the Y-coordinate for the annotation.
		/// </remarks>
		public static readonly BindableProperty Y1Property = BindableProperty.Create(
			nameof(Y1),
			typeof(double),
			typeof(ChartAnnotation),
			double.NaN,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="XAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="XAxisName"/> property specifies the X-axis to which the annotation is tied.
		/// </remarks>
		public static readonly BindableProperty XAxisNameProperty = BindableProperty.Create(
			nameof(XAxisName),
			typeof(string),
			typeof(ChartAnnotation),
			string.Empty,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="YAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="YAxisName"/> property specifies the Y-axis to which the annotation is tied.
		/// </remarks>
		public static readonly BindableProperty YAxisNameProperty = BindableProperty.Create(
			nameof(YAxisName),
			typeof(string),
			typeof(ChartAnnotation),
			string.Empty,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CoordinateUnit"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CoordinateUnit"/> property defines whether the annotation coordinates are based on axis units or pixel units.
		/// </remarks>
		public static readonly BindableProperty CoordinateUnitProperty = BindableProperty.Create(
			nameof(CoordinateUnit),
			typeof(ChartCoordinateUnit),
			typeof(ChartAnnotation),
			ChartCoordinateUnit.Axis,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that indicates whether the annotation is visible or not.
		/// </summary>
		/// <value>This property takes the <c>bool</c> as its value. Its default value is true.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1" IsVisible="True"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();     
		///
		///     // Eliminated for simplicity
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///         IsVisible = true,
		///     };
		///  
		///     chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DateTime or double that represents the x1 position of the chart annotation.
		/// </summary>
		/// <value>This property takes the <c>object</c> as its value. Its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1"/>
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
		///     var verticalLine = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///     };
		///  
		///     chart.Annotations.Add(verticalLine);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public object X1
		{
			get { return GetValue(X1Property); }
			set { SetValue(X1Property, value); }
		}

		/// <summary>
		/// Gets or sets the double that represents the y1 position of the chart annotation.
		/// </summary>
		/// <value>This property takes the <c>double</c> as its value and its default value is double.NaN.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:HorizontalLineAnnotation Y1="10"/>
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
		///     };
		///  
		///     chart.Annotations.Add(horizontalAnnotation);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double Y1
		{
			get { return (double)GetValue(Y1Property); }
			set { SetValue(Y1Property, value); }
		}

		/// <summary>
		/// Get or set the value of a string that represents the name of the x-axis in the annotation.
		/// </summary>
		/// <value>This property takes the <c>string</c> as its value and its default value is string.Empty.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:LineAnnotation X1="0" Y1="10" X2="4" Y2="50" LineCap="Arrow" XAxisName="xAxis"/>
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
		///     var lineAnnotation = new LineAnnotation()
		///     {
		///         X1 = 0,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 50,
		///         LineCap = ChartLineCap.Arrow,
		///         XAxisName= "xAxis",
		///     };
		///  
		///     chart.Annotations.Add(lineAnnotation);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string XAxisName
		{
			get { return (string)GetValue(XAxisNameProperty); }
			set { SetValue(XAxisNameProperty, value); }

		}

		/// <summary>
		/// Get or set the value of a string that represents the name of the y-axis in the annotation.
		/// </summary>
		///  <value>This property takes the <c>string</c> as its value and its default value is string.Empty.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:LineAnnotation X1="0" Y1="10" X2="4" Y2="50" LineCap="Arrow" YAxisName="yAxis"/>
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
		///     var line = new LineAnnotation()
		///     {
		///         X1 = 0,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 50,
		///         LineCap = ChartLineCap.Arrow,
		///         YAxisName= "yAxis",
		///     };
		///  
		///     chart.Annotations.Add(line);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string YAxisName
		{
			get { return (string)GetValue(YAxisNameProperty); }
			set { SetValue(YAxisNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the property that identifies whether the annotation is positioned based on pixel or axis coordinates.
		/// </summary>
		/// <value>This property takes the <see cref="ChartCoordinateUnit"/> as its value. Its default value is <see cref="ChartCoordinateUnit.Axis"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:LineAnnotation X1="0" Y1="10" X2="4" Y2="50" CoordinateUnit="Axis"/>
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
		///     var line = new LineAnnotation()
		///     {
		///         X1 = 0,
		///         Y1 = 10,
		///         X2 = 4,
		///         Y2 = 50,
		///        CoordinateUnit= ChartCoordinateUnit.Axis,
		///     };
		///  
		///     chart.Annotations.Add(line);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartCoordinateUnit CoordinateUnit
		{
			get { return (ChartCoordinateUnit)GetValue(CoordinateUnitProperty); }
			set { SetValue(CoordinateUnitProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAnnotation"/>.
		/// </summary>
		public ChartAnnotation()
		{
			_annotationLabelStyle = new ChartAnnotationLabelStyle();
		}

		#endregion

		#region Methods

		#region Protected Methods


		/// <summary>
		/// Draws the annotation of the chart.
		/// </summary>
		protected internal virtual void Draw(ICanvas canvas, RectF dirtyRect)
		{
		}

		#endregion

		#region Internal Methods

		internal void UpdateLayout()
		{
			if (Chart != null && Chart.SeriesBounds != Rect.Zero)
			{
				LayoutAnnotation();
			}
		}

		internal virtual void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
		{
		}

		internal virtual void Dispose()
		{
		}

		internal virtual void ResetPosition()
		{

		}

		internal (double, double) TransformCoordinates(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x, double y)
		{
			var xValue = x;
			var yValue = y;

			if (!xAxis.IsVertical)
			{
				x = chart.ValueToPoint(xAxis, xValue);
				y = chart.ValueToPoint(yAxis, yValue);
			}
			else
			{
				y = chart.ValueToPoint(xAxis, xValue);
				x = chart.ValueToPoint(yAxis, yValue);
			}

			if (CoordinateUnit == ChartCoordinateUnit.Axis)
			{
				var clipRect = chart._chartArea.ActualSeriesClipRect;
				var seriesBounds = chart.SeriesBounds;

				var translateX = clipRect.X - seriesBounds.X;
				var translateY = clipRect.Y - seriesBounds.Y;

				x += translateX;
				y += translateY;
			}

			return (x, y);
		}

		internal static void OnAnnotationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAnnotation annotation)
			{
				annotation.UpdateLayout();
				annotation.Invalidate();
			}
		}

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0060 // Remove unused parameter
		internal static void OnAnnotationPropertyInvalidate(BindableObject bindable, object oldValue, object newValue)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (bindable is ChartAnnotation annotation)
			{
				if (annotation is ShapeAnnotation shapeAnnotation)
				{
					shapeAnnotation.InitializeDynamicResource(shapeAnnotation);
				}

				annotation.Invalidate();
			}
		}

		internal static void OnLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAnnotation annotation)
			{
				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, annotation.Parent);

				if (oldValue is ChartAnnotationLabelStyle style)
				{
					annotation.UnHookStylePropertyChanged(style);
				}

				if (newValue is ChartAnnotationLabelStyle newStyle)
				{
					annotation._annotationLabelStyle = newStyle;
					newStyle.Parent = annotation.Parent;
					SetInheritedBindingContext(newStyle, annotation.BindingContext);
					newStyle.PropertyChanged += annotation.Style_PropertyChanged;
				}
				else
				{
					annotation._annotationLabelStyle = new ChartAnnotationLabelStyle
					{
						Parent = annotation.Parent
					};
					SetInheritedBindingContext(annotation._annotationLabelStyle, annotation.BindingContext);
				}

				annotation.UpdateLayout();
				annotation.Invalidate();
			}
		}

		internal void Invalidate()
		{
			Chart?.InvalidateAnnotation();
		}

		internal void UnHookStylePropertyChanged(ChartLabelStyle style)
		{
			SetInheritedBindingContext(style, null);
			style.PropertyChanged -= Style_PropertyChanged;
			style.Parent = null;
		}

		#endregion

		#region Private Method

		void LayoutAnnotation()
		{
			if (Chart == null)
			{
				return;
			}

			var axes = Chart._chartArea;
			var xAxis = ChartAnnotation.GetAxis(axes._xAxes, XAxisName);
			var yAxis = ChartAnnotation.GetAxis(axes._yAxes, YAxisName);

			if (xAxis == null || yAxis == null)
			{
				return;
			}

			var x1 = ChartUtils.ConvertToDouble(X1);
			var y1 = Y1;

			OnLayout(Chart, xAxis, yAxis, x1, y1);
		}

		void Style_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (sender is ChartAnnotationLabelStyle)
			{
				if (e.PropertyName == ChartAnnotationLabelStyle.HorizontalTextAlignmentProperty.PropertyName ||
					e.PropertyName == ChartAnnotationLabelStyle.VerticalTextAlignmentProperty.PropertyName ||
					e.PropertyName == ChartLabelStyle.MarginProperty.PropertyName || e.PropertyName == ChartLabelStyle.FontSizeProperty.PropertyName)
				{
					UpdateLayout();
				}

				Invalidate();
			}
		}

		static ChartAxis? GetAxis(IEnumerable<ChartAxis> axes, string name)
		{
			List<ChartAxis> axisList = axes.ToList();

			foreach (var axis in axes)
			{
				if (!string.IsNullOrEmpty(name) && axis.Name.Equals(name, System.StringComparison.Ordinal))
				{
					return axis;
				}
			}

			return axisList.Count > 0 ? axisList[0] : null;
		}

		#endregion

		#endregion
	}
}
