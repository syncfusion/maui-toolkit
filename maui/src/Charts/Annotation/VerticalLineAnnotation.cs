using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class is used to add a vertical line annotation to the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection.
	/// </summary>
	/// <remarks>
	/// VerticalLineAnnotation is used to draw a vertical line across the chart area.
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart.Annotations>
	///   <chart:VerticalLineAnnotation X1="3">
	///   </chart:VerticalLineAnnotation>
	/// </chart:SfCartesianChart.Annotations>  
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	///  SfCartesianChart chart = new SfCartesianChart();
	///  var vertical = new VerticalLineAnnotation()
	///  {
	///    X1 = 3,
	///  };
	///  
	/// chart.Annotations.Add(vertical);
	/// ]]>
	/// </code>
	/// </example>
	public partial class VerticalLineAnnotation : LineAnnotation
	{
		#region Private Fields

		bool _isVertical = true;

		RectF _axisLabelRect;

		string _labelText = string.Empty;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ShowAxisLabel"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowAxisLabel"/> property determines whether the axis label is displayed for the <see cref="VerticalLineAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty ShowAxisLabelProperty = BindableProperty.Create(
			nameof(ShowAxisLabel),
			typeof(bool),
			typeof(VerticalLineAnnotation),
			false,
			BindingMode.Default,
			null,
			OnAnnotationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AxisLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AxisLabelStyle"/> property allows customization of the axis label in the <see cref="VerticalLineAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty AxisLabelStyleProperty = BindableProperty.Create(
			nameof(AxisLabelStyle),
			typeof(ChartLabelStyle),
			typeof(VerticalLineAnnotation),
			null,
			BindingMode.Default,
			null,
			OnAxisLabelStylePropertyChanged,
			defaultValueCreator: AxisLabelStyleDefaultValueCreator);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether to enable or disable the display of the annotation label on the axis. 
		/// </summary>
		/// <value>This property takes the <c>bool</c> as its value and its default value is false.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1" ShowAxisLabel="True"/>
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
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///         ShowAxisLabel = true,
		///     };
		///  
		///     chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowAxisLabel
		{
			get { return (bool)GetValue(ShowAxisLabelProperty); }
			set { SetValue(ShowAxisLabelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the customized style for the annotation axis label.
		/// </summary>
		/// <value>This property takes the <see cref="ChartLabelStyle"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:VerticalLineAnnotation X1="1">
		///           <chart:VerticalLineAnnotation.AxisLabelStyle>
		///             <chart:ChartLabelStyle Background="Yellow"/>
		///           </chart:VerticalLineAnnotation.AxisLabelStyle>
		///          </chart:VerticalLineAnnotation>
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
		///     var vertical = new VerticalLineAnnotation()
		///     {
		///         X1 = 1,
		///     };
		///  
		///  horizontal.AxisLabelStyle = new ChartLabelStyle()
		///  {
		///     Background = Colors.Yellow,
		///  };
		///  
		/// chart.Annotations.Add(vertical);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartLabelStyle AxisLabelStyle
		{
			get { return (ChartLabelStyle)GetValue(AxisLabelStyleProperty); }
			set { SetValue(AxisLabelStyleProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public VerticalLineAnnotation()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
			_annotationLabelStyle.HorizontalTextAlignment = ChartLabelAlignment.Start;
			_annotationLabelStyle.VerticalTextAlignment = ChartLabelAlignment.Start;
		}
		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
		{
			base.Draw(canvas, dirtyRect);

			if (ShowAxisLabel == true)
			{
				canvas.StrokeSize = double.IsNaN(_axisLabelStyle.StrokeWidth) ? 0 : (float)_axisLabelStyle.StrokeWidth;
				canvas.StrokeColor = _axisLabelStyle.Stroke == null ? Colors.Transparent : _axisLabelStyle.Stroke.ToColor();
				_axisLabelStyle.DrawTextBackground(canvas, _labelText, _axisLabelStyle.Background, new PointF(_axisLabelRect.X, _axisLabelRect.Y));
				_axisLabelStyle.DrawLabel(canvas, _labelText, new PointF(_axisLabelRect.X, _axisLabelRect.Y));
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (AxisLabelStyle != null)
			{
				SetInheritedBindingContext(AxisLabelStyle, BindingContext);
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnParentSet()
		{
			base.OnParentSet();
			if (AxisLabelStyle != null)
			{
				AxisLabelStyle.Parent = Parent;
			}
		}

		#endregion

		#region Internal Methods

		internal override void Dispose()
		{
			UnHookStylePropertyChanged(AxisLabelStyle);
			base.Dispose();
		}

		internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
		{
			ResetPosition();

			if (X1 == null)
			{
				return;
			}

			var actualSeriesClipRect = chart._chartArea.ActualSeriesClipRect;
			var x2 = x1;
			var y2 = Y2;
			var isOpposed = xAxis.IsOpposed();

			if (CoordinateUnit == ChartCoordinateUnit.Axis)
			{
				if (!xAxis.IsVertical)
				{
					_isVertical = true;
					(x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
					(x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
					y1 = double.IsNaN(Y1) ? isOpposed ? actualSeriesClipRect.Top : actualSeriesClipRect.Bottom : y1;
					y2 = double.IsNaN(Y2) ? isOpposed ? actualSeriesClipRect.Bottom : actualSeriesClipRect.Top : y2;
				}
				else
				{
					_isVertical = false;
					(_, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
					(_, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
					x1 = double.IsNaN(Y1) ? isOpposed ? actualSeriesClipRect.Right : actualSeriesClipRect.Left : y1;
					x2 = double.IsNaN(Y2) ? isOpposed ? actualSeriesClipRect.Left : actualSeriesClipRect.Right : y2;
				}
			}
			else
			{
				if (!xAxis.IsVertical)
				{
					y1 = double.IsNaN(Y1) ? isOpposed ? -actualSeriesClipRect.Top : actualSeriesClipRect.Bottom - actualSeriesClipRect.Top : y1;
					y2 = double.IsNaN(Y2) ? isOpposed ? actualSeriesClipRect.Bottom - actualSeriesClipRect.Top : -actualSeriesClipRect.Top : y2;
				}
			}

			XPosition1 = (float)x1;
			XPosition2 = (float)x2;
			YPosition1 = (float)y1;
			YPosition2 = (float)y2;
			Angle = (float)(Math.Atan2(YPosition2 - YPosition1, XPosition2 - XPosition1) * (180 / Math.PI));

			CalculatePosition(_isVertical, xAxis);

			if (ShowAxisLabel == true)
			{
				CalculateVerticalAxisLabelPosition(xAxis, actualSeriesClipRect);
			}

			if (Text != null && !string.IsNullOrEmpty(Text))
			{
				SetTextAlignment(XPosition1, YPosition1);
			}
		}

		internal override void SetTextAlignment(double x, double y)
		{
			if (_isVertical)
			{
				SetVerticalTextAlignment(x, y);
			}
			else
			{
				SetHorizontalTextAlignment(x, y);
			}
		}

		internal override void ResetPosition()
		{
			XPosition1 = XPosition2 = YPosition1 = YPosition2 = float.NaN;
			LineCapPoints.Clear();
			LabelRect = Rect.Zero;
			_axisLabelRect = Rect.Zero;
		}

		#endregion

		#region Private Methods

		void CalculateVerticalAxisLabelPosition(ChartAxis xAxis, Rect actualSeriesClipRect)
		{
			var xValue = Convert.ToDouble(X1);

			_ = (float)_axisLabelStyle.Margin.Top;
			float marginBottom = (float)_axisLabelStyle.Margin.Bottom;
			float marginLeft = (float)_axisLabelStyle.Margin.Left;
			float marginRight = (float)_axisLabelStyle.Margin.Right;
			string labelFormat = !string.IsNullOrEmpty(_axisLabelStyle.LabelFormat) ?
				_axisLabelStyle.LabelFormat : xAxis is DateTimeAxis ?
				"MM-dd-yyyy" : "##.##";
			_labelText = xValue.ToString(labelFormat);
			var labelSize = _axisLabelStyle.MeasureLabel(_labelText);

			float y;
			float x;
			if (xAxis.IsVertical)
			{
				x = xAxis.IsOpposed() ? (float)(actualSeriesClipRect.Right) + labelSize.Width / 2 + marginRight : (float)(actualSeriesClipRect.Left) - labelSize.Width / 2 - marginLeft;
				y = YPosition1;
			}
			else
			{
				x = XPosition1;
				y = (float)(xAxis.ArrangeRect.Y + labelSize.Height / 2 + marginBottom);
			}

			var labelPosition = new PointF(x, y);
			_axisLabelRect = new Rect(labelPosition, labelSize);
		}

		static object AxisLabelStyleDefaultValueCreator(BindableObject bindable)
		{
			return new ChartLabelStyle() { FontSize = 14 };
		}

		#endregion

		#endregion
	}
}