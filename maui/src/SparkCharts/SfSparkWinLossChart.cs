using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Renders a Spark Win-Loss Chart for binary data visualization.
	/// </summary>
	public class SfSparkWinLossChart : SfSparkChart
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
		/// Identifies the PositivePointsFill bindable property.
		/// </summary>
		public static readonly BindableProperty PositivePointsFillProperty =
			BindableProperty.Create(
				nameof(PositivePointsFill),
				typeof(Brush),
				typeof(SfSparkWinLossChart),
				new SolidColorBrush(Color.FromArgb("#116DF9")),
				BindingMode.Default);

		/// <summary>
		/// Identifies the NegativePointsFill bindable property.
		/// </summary>
		public static readonly BindableProperty NegativePointsFillProperty =
			BindableProperty.Create(
				nameof(NegativePointsFill),
				typeof(Brush),
				typeof(SfSparkWinLossChart),
				new SolidColorBrush(Color.FromArgb("#FF4E4E")),
				BindingMode.Default);

		/// <summary>
		/// Identifies the NeutralPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty NeutralPointFillProperty =
			BindableProperty.Create(
				nameof(NeutralPointFill),
				typeof(Brush),
				typeof(SfSparkWinLossChart),
				new SolidColorBrush(Color.FromArgb("#E2227E")),
				BindingMode.Default);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the stroke width for the Spark Win-Loss Chart.
		/// </summary>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush used to fill positive columns in the Spark Win-Loss Chart.
		/// </summary>
		public Brush PositivePointsFill
		{
			get => (Brush)GetValue(PositivePointsFillProperty);
			set => SetValue(PositivePointsFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush used to fill negative columns in the Spark Win-Loss Chart.
		/// </summary>
		public Brush NegativePointsFill
		{
			get => (Brush)GetValue(NegativePointsFillProperty);
			set => SetValue(NegativePointsFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush used to fill neutral columns in the Spark Win-Loss Chart.
		/// </summary>
		public Brush NeutralPointFill
		{
			get => (Brush)GetValue(NeutralPointFillProperty);
			set => SetValue(NeutralPointFillProperty, value);
		}

		#endregion

		#region Draw Method

		/// <summary>
		/// Draws the visual elements of the Spark WinLoss Chart.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="rect">The rectangle area to draw within.</param>
		internal override void OnDraw(ICanvas canvas, Rect rect)
		{
			if (yValues == null || yValues.Count == 0 || rect.Width <= 0)
			{
				return;
			}

			canvas.SaveState();
			rect = GetTranslatedRect(rect);
			canvas.Translate((float)rect.X, (float)rect.Y);

			float _slotWidth = (float)(rect.Width / DataCount);
			float _drawingTop = (float)rect.Top;
			float _drawingHeight = (float)rect.Height;
			float _midPointY = _drawingTop + (_drawingHeight / 2);
			float _barHeight = _drawingHeight / 2;
			float _neutralBarHeight = _drawingHeight / 10;

			for (int i = 0; i < DataCount; i++)
			{
				var _yVal = yValues[i];
				if (double.IsNaN(_yVal))
				{
					continue;
				}

				// Calculate column geometry similar to Column chart
				float _columnWidth = _slotWidth * 0.8f;
				float _columnLeft = (i * _slotWidth) + (_slotWidth * 0.1f); // Centered in slot
				float _top = _drawingTop;
				float _bottom = _top + _barHeight;
				if (_yVal < 0)
				{
					_top = _midPointY;
					_bottom = _top + _barHeight;
				}
				else if (_yVal == 0)
				{
					_top = _midPointY - (_neutralBarHeight / 2);
					_bottom = _top + _neutralBarHeight;
				}

				float _height = _bottom - _top;
				if (_height <= 0 || _columnWidth <= 0)
				{
					continue;
				}

				var _barRect = new RectF(_columnLeft, _top, _columnWidth, _height);
				Brush _fillBrush = GetFillPaint(_yVal);
				if (_fillBrush != null)
				{
					canvas.SetFillPaint(_fillBrush, _barRect);
					canvas.FillRectangle(_barRect);
				}

				canvas.StrokeSize = (float)StrokeWidth;

				if (StrokeWidth > 0 && Stroke != null)
				{
					canvas.StrokeColor = Stroke.ToColor();
					canvas.DrawRectangle(_barRect);
				}
			}

			base.OnDraw(canvas, rect);

			canvas.RestoreState();
		}

		internal override void GetAxisPosition(ref PointF point1, ref PointF point2, Rect rect)
		{
			point1 = new PointF(0, (float)rect.Height / 2);
			point2 = new PointF((float)rect.Width, (float)rect.Height / 2);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Determines the appropriate brush for a win-loss bar based on its value.
		/// </summary>
		private Brush GetFillPaint(double yValue)
		{
			if (yValue > 0)
			{
				return PositivePointsFill;
			}

			if (yValue < 0)
			{
				return NegativePointsFill;
			}

			return NeutralPointFill;
		}

		#endregion
	}
}
