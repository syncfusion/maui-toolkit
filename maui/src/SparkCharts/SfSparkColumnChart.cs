using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Renders a Spark Column Chart with customizable fill for specific data points.
	/// </summary>
	public class SfSparkColumnChart : SfSparkChart
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the StrokeWidth bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeWidthProperty =
			BindableProperty.Create(
				nameof(StrokeWidth),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the Fill bindable property.
		/// </summary>
		public static readonly BindableProperty FillProperty =
			BindableProperty.Create(
				nameof(Fill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				new SolidColorBrush(Color.FromArgb("#116DF9")),
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the FirstPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty FirstPointFillProperty =
			BindableProperty.Create(
				nameof(FirstPointFill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the LastPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty LastPointFillProperty =
			BindableProperty.Create(
				nameof(LastPointFill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the HighPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty HighPointFillProperty =
			BindableProperty.Create(
				nameof(HighPointFill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the LowPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty LowPointFillProperty =
			BindableProperty.Create(
				nameof(LowPointFill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the NegativePointsFill bindable property.
		/// </summary>
		public static readonly BindableProperty NegativePointsFillProperty =
			BindableProperty.Create(
				nameof(NegativePointsFill),
				typeof(Brush),
				typeof(SfSparkColumnChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the stroke width for the Spark Column Chart.
		/// </summary>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the Fill property to fill the interior of the column spark chart.
		/// </summary>
		public Brush Fill
		{
			get => (Brush)GetValue(FillProperty);
			set => SetValue(FillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the first point in the Spark Column Chart.
		/// </summary>
		public Brush FirstPointFill
		{
			get => (Brush)GetValue(FirstPointFillProperty);
			set => SetValue(FirstPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the last point in the Spark Column Chart.
		/// </summary>
		public Brush LastPointFill
		{
			get => (Brush)GetValue(LastPointFillProperty);
			set => SetValue(LastPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the highest point in the Spark Column Chart.
		/// </summary>
		public Brush HighPointFill
		{
			get => (Brush)GetValue(HighPointFillProperty);
			set => SetValue(HighPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the lowest point in the Spark Column Chart.
		/// </summary>
		public Brush LowPointFill
		{
			get => (Brush)GetValue(LowPointFillProperty);
			set => SetValue(LowPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for negative points in the Spark Column Chart.
		/// </summary>
		public Brush NegativePointsFill
		{
			get => (Brush)GetValue(NegativePointsFillProperty);
			set => SetValue(NegativePointsFillProperty, value);
		}

		#endregion

		#region Draw Method

		/// <summary>
		/// Draws the visual elements of the Spark Column Chart.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="rect">The rectangle area to draw within.</param>
		internal override void OnDraw(ICanvas canvas, Rect rect)
		{
			if (yValues == null || yValues.Count == 0 || rect.Width <= 0 || rect.Height <= 0)
			{
				return;
			}

			canvas.SaveState();
			rect = GetTranslatedRect(rect);
			canvas.Translate((float)rect.X, (float)rect.Y);

			// Ensure valid Y range
			double _yMin = minYValue;
			double _yMax = maxYValue;

			if (_yMin == _yMax)
			{
				_yMin = _yMin > 0 ? 0 : _yMin;
				_yMax = _yMax < 0 ? 0 : _yMax;
			}

			double _baseline = Math.Max(_yMin, Math.Min(_yMax, 0d));
			float _baseY = TransformToVisible(0, _baseline, rect).Y;
			float _slotWidth = (float)(rect.Width / DataCount);
			float _columnWidth = Math.Max(_slotWidth * 0.8f, 2f); // Ensure minimum width
			for (int i = 0; i < DataCount; i++)
			{
				double _yVal = yValues[i];
				if (double.IsNaN(_yVal))
				{
					continue;
				}

				float _columnTop = TransformToVisible(0, _yVal, rect).Y;
				float _columnLeft = (i * _slotWidth) + (_slotWidth - _columnWidth) / 2;
				float _height = Math.Abs(_baseY - _columnTop);
				// Ensure minimum height for visibility
				if (_height < 4f && _yVal != 0)
				{
					_height = 4f;
					_columnTop = _baseY - _height;
				}

				float _top = Math.Min(_columnTop, _baseY);
				var _columnRect = new RectF(_columnLeft, _top, _columnWidth, _height);
				if (_columnRect.Width <= 0 || _columnRect.Height <= 0)
				{
					continue;
				}

				canvas.StrokeSize = (float)StrokeWidth;
				if (Stroke != null)
				{
					canvas.StrokeColor = Stroke.ToColor();
				}

				// Set fill and stroke for the column.
				Brush _fillBrush = GetFillPaint(i, _yVal, _baseline);
				if (_fillBrush != null)
				{
					canvas.SetFillPaint(_fillBrush, _columnRect);
					canvas.FillRectangle(_columnRect);
				}

				if (StrokeWidth > 0 && Stroke != null)
				{
					canvas.DrawRectangle(_columnRect);
				}
			}

			base.OnDraw(canvas, rect);

			canvas.RestoreState();
		}

		internal override void GetAxisPosition(ref PointF point1, ref PointF point2, Rect rect)
		{
			var y = double.IsNaN(AxisOrigin) ? 0 : AxisOrigin;
			point1 = TransformToVisible(-0.5, y, rect);
			point2 = new PointF((float)rect.Width, point1.Y);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Determines the appropriate brush for a column based on its value and position.
		/// </summary>
		Brush GetFillPaint(int index, double yValue, double baseline)
		{
			if (HighPointFill != null && yValue == maxYValue && yValue != baseline)
			{
				return HighPointFill;
			}

			if (LowPointFill != null && yValue == minYValue && yValue != baseline)
			{
				return LowPointFill;
			}

			if (FirstPointFill != null && index == 0)
			{
				return FirstPointFill;
			}

			if (LastPointFill != null && index == DataCount - 1)
			{
				return LastPointFill;
			}

			if (NegativePointsFill != null && yValue < baseline)
			{
				return NegativePointsFill;
			}

			return Fill;
		}
		#endregion
	}
}
