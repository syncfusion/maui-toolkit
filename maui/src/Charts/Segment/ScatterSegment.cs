namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="ScatterSeries"/>.
	/// </summary>
	public partial class ScatterSegment : CartesianSegment
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value that indicates the shape type of the scatter segment.
		/// </summary>
		public ShapeType Type { get; set; }

		/// <summary>
		/// Gets the width of the scatter segment from the associated series.
		/// </summary>
		public float PointWidth { get; internal set; }

		/// <summary>
		/// Gets the height of the scatter segment from the associated series.
		/// </summary>
		public float PointHeight { get; internal set; }

		/// <summary>
		/// Gets the x-coordinate for the center of the scatter segment.
		/// </summary>
		public float CenterX { get; internal set; }

		/// <summary>
		/// Gets the y-coordinate for the center of the scatter segment.
		/// </summary>
		public float CenterY { get; internal set; }

		internal RectF PreviousSegmentBounds { get; set; } = RectF.Zero;

		/// <summary>
		/// Initialize the double fields.
		/// </summary>
		double _x, _y;

		Rect _actualRectF;

		#endregion

		#region Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is ScatterSeries scatterSeries)
			{
				PointWidth = (float)scatterSeries.PointWidth;
				PointHeight = (float)scatterSeries.PointHeight;

				CenterX = scatterSeries.TransformToVisibleX(_x, _y);
				CenterY = scatterSeries.TransformToVisibleY(_x, _y);
				RectF rectF = new RectF(CenterX - (PointWidth / 2), CenterY - (PointHeight / 2), PointWidth, PointHeight);

				RectF actualSeriesClipRect = scatterSeries.AreaBounds;

				if (rectF.X + rectF.Width > 0 && rectF.X - rectF.Width < actualSeriesClipRect.Width &&
					rectF.Y + rectF.Height > 0 && rectF.Y - rectF.Height < actualSeriesClipRect.Height)
				{
					SegmentBounds = rectF;
				}
				else
				{
					SegmentBounds = Rect.Zero;
				}
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			_actualRectF = SegmentBounds;

			if (Series == null || _actualRectF.IsEmpty)
			{
				return;
			}

			if (Series.CanAnimate())
			{
				float animationValue = Series.AnimationValue;

				if (!Series.XRange.Equals(Series.PreviousXRange) || PreviousSegmentBounds == RectF.Zero)
				{
					float newWidth = SegmentBounds.Width * animationValue;
					float newHeight = SegmentBounds.Height * animationValue;
					_actualRectF = new RectF(SegmentBounds.Left + (SegmentBounds.Width / 2) - (newWidth / 2), SegmentBounds.Top + (SegmentBounds.Height / 2) - (newHeight / 2), newWidth, newHeight);
				}
				else
				{
					float x = CartesianSegment.GetDynamicAnimationValue(animationValue, SegmentBounds.X, PreviousSegmentBounds.X, SegmentBounds.X);
					float y = CartesianSegment.GetDynamicAnimationValue(animationValue, SegmentBounds.Y, PreviousSegmentBounds.Y, SegmentBounds.Y);
					_actualRectF = new RectF(x, y, (float)_actualRectF.Width, (float)_actualRectF.Height);
				}
			}

			canvas.SetFillPaint(Fill, SegmentBounds);
			canvas.Alpha = Opacity;

			if (HasStroke)
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
			}

			canvas.DrawShape(_actualRectF, shapeType: Type, HasStroke, false);
		}

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the scatter segment.
		/// </summary>
		internal override void SetData(double[] values)
		{
			_x = values[0];
			_y = values[1];

			if (Series != null)
			{
				Series.XRange += !double.IsNaN(_x) ? DoubleRange.Union(_x) : DoubleRange.Empty;
				Series.YRange += !double.IsNaN(_y) ? DoubleRange.Union(_y) : DoubleRange.Empty;
			}
		}

		internal override int GetDataPointIndex(float valueX, float valueY)
		{
			float defaultSize = 10;

			RectF touchPointRect = new RectF(valueX - defaultSize / 2, valueY - defaultSize / 2, defaultSize, defaultSize);

			if (Series != null && SegmentBounds.IntersectsWith(touchPointRect))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
		}

		internal override void OnDataLabelLayout()
		{
			CalculateDataLabelPosition(_x, _y, _y);
		}

		#endregion
	}
}