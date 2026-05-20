using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="HistogramSeries"/> in a chart, displaying a vertical bar.
	/// </summary>
	public partial class HistogramSegment : ColumnSegment
	{
		#region Internal properties

		internal double HistogramLabelPosition { get; set; }
		internal double PointsCount { get; set; }

		#endregion

		#region Internal Methods

		internal override void OnDataLabelLayout()
		{
			CalculateDataLabelPosition(HistogramLabelPosition, PointsCount, PointsCount);
		}

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the histogram segment.
		/// </summary>
		internal override void SetData(double[] values)
		{
			base.SetData(values);

			if (Series is HistogramSeries series)
			{
				series.XRange += new DoubleRange(values[0], values[1]);
			}
		}

		#endregion

		#region Protected Internal Methods

		/// <summary>
		/// Draws the histogram segment on the specified canvas.
		/// </summary>
		/// <param name="canvas"></param>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not HistogramSeries series || series.ActualXAxis == null)
			{
				return;
			}

			if (series.CanAnimate())
			{
				Layout();
			}

			if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
				canvas.Alpha = Opacity;
				var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
				canvas.SetFillPaint(Fill, rect);
				canvas.FillRectangle(rect);

				if (HasStroke)
				{
					canvas.DrawRectangle(rect);
				}
			}
		}

		#endregion
	}

	internal class DistributionSegment : ILineDrawing
	{
		#region Fields

		readonly HistogramSeries _histogramSeries;
		bool _enableAntiAliasing;
		Brush? _stroke;
		double _strokeWidth;
		float _opacity = 1;
		DoubleCollection? _strokeDashArray;

#if ANDROID
		const float _segmentLengthThreshold = 0.01f;
		const float _patternMatchTolerance = 0.01f;
		float[]? _cachedDashPattern;
		List<float>? _dashSegments;
#endif
		#endregion

		#region Internal Properties

		internal float[]? DrawPoints { get; set; }

		#endregion

		#region Interface Implementation

		Color ILineDrawing.Stroke
		{
			get
			{
				if (_stroke is SolidColorBrush brush)
				{
					return brush.Color;
				}
				else
				{
					return Colors.Black;
				}
			}
			set => _stroke = new SolidColorBrush(value);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		double ILineDrawing.StrokeWidth
		{
			get => _strokeWidth;
			set => _strokeWidth = value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		bool ILineDrawing.EnableAntiAliasing
		{
			get => _enableAntiAliasing;
			set => _enableAntiAliasing = value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		float ILineDrawing.Opacity
		{
			get => _opacity;
			set => _opacity = value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		DoubleCollection? ILineDrawing.StrokeDashArray
		{
			get => _strokeDashArray;
			set => _strokeDashArray = value;
		}

		#endregion

		#region Constructor

		internal DistributionSegment(HistogramSeries series)
		{
			_histogramSeries = series;
		}

		#endregion

		#region Internal Methods

		internal void UpdateCurveStyle()
		{
			if (_histogramSeries.CurveStyle is ChartLineStyle chartLineStyle)
			{
				_stroke = chartLineStyle.Stroke;
				_strokeWidth = chartLineStyle.StrokeWidth;
				_strokeDashArray = chartLineStyle.StrokeDashArray;
			}
		}

		internal void OnDraw(ICanvas canvas)
		{
			if (_histogramSeries.ShowNormalDistributionCurve && !_histogramSeries.CanAnimate() && DrawPoints != null)
			{
				if (DrawPoints == null || DrawPoints.Length < 4 || _strokeWidth <= 0 || _stroke == null || IsTransparent(_stroke))
					return;

				_enableAntiAliasing = true;

#if ANDROID
				if (_strokeDashArray != null && _strokeDashArray.Count > 0)
				{
					DrawContinuousDashedLine(canvas);
					return;
				}
#endif

				canvas.DrawLines(DrawPoints, this);

			}
		}

		bool IsTransparent(Brush brush)
		{
			if (brush is SolidColorBrush solidBrush)
			{
				return solidBrush.Color.Alpha == 0;
			}
			return false;
		}

#if ANDROID
		void DrawContinuousDashedLine(ICanvas canvas)
		{
			if (_strokeDashArray == null || DrawPoints == null)
			{
				return;
			}

			int patternLength = _strokeDashArray.Count;
			if (_cachedDashPattern == null || _cachedDashPattern.Length != patternLength)
			{
				_cachedDashPattern = new float[patternLength];
			}

			for (int i = 0; i < patternLength; i++)
			{
				_cachedDashPattern[i] = (float)_strokeDashArray[i];
			}

			float width = (float)_strokeWidth;
			float[] points = DrawPoints;
			int pointCount = points.Length / 2;

			if (_dashSegments == null)
			{
				_dashSegments = new List<float>(pointCount * 4);
			}
			else
			{
				_dashSegments.Clear();
			}

			int patternIndex = 0;
			bool isDash = true;
			float patternDistance = 0;

			for (int i = 0; i < pointCount - 1; i++)
			{
				float x1 = points[i * 2];
				float y1 = points[i * 2 + 1];
				float x2 = points[(i + 1) * 2];
				float y2 = points[(i + 1) * 2 + 1];

				float dx = x2 - x1;
				float dy = y2 - y1;
				float segmentLength = (float)Math.Sqrt(dx * dx + dy * dy);

				if (segmentLength < _segmentLengthThreshold)
					continue;

				float segmentProgress = 0;

				while (segmentProgress < segmentLength)
				{
					float currentDashLength = _cachedDashPattern[patternIndex] * width;
					float remainingInPattern = currentDashLength - patternDistance;
					float remainingInSegment = segmentLength - segmentProgress;
					float drawLength = Math.Min(remainingInPattern, remainingInSegment);

					float t1 = segmentProgress / segmentLength;
					float t2 = (segmentProgress + drawLength) / segmentLength;

					float startX = x1 + dx * t1;
					float startY = y1 + dy * t1;
					float endX = x1 + dx * t2;
					float endY = y1 + dy * t2;

					if (isDash)
					{
						_dashSegments.Add(startX);
						_dashSegments.Add(startY);
						_dashSegments.Add(endX);
						_dashSegments.Add(endY);
					}

					segmentProgress += drawLength;
					patternDistance += drawLength;

					if (patternDistance >= currentDashLength - _patternMatchTolerance)
					{
						patternIndex = (patternIndex + 1) % _cachedDashPattern.Length;
						isDash = !isDash;
						patternDistance = 0;
					}
				}
			}

			if (_dashSegments.Count > 0)
			{
				float[] dashArray = new float[_dashSegments.Count];
				_dashSegments.CopyTo(dashArray);

				canvas.StrokeDashPattern = null;
				canvas.DrawLines(dashArray, this);
			}
		}
#endif

		#endregion
	}
}