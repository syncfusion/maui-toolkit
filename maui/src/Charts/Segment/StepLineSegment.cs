namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="StepLineSeries"/>, which displays data using a step-like progression.
	/// </summary>
	public partial class StepLineSegment : LineSegment
	{
		#region Methods

		internal override void DrawLine(ICanvas canvas, float x1, float x2, float y1, float y2)
		{
			// Check if fill is a gradient brush
			if (Fill is GradientBrush && (StrokeDashArray == null || StrokeDashArray.Count == 0))
			{
				// For series-level gradients, compute bounds across all segments
				RectF? gradientBounds = null;

				if (ReferenceEquals(Fill, Series?.Fill))
				{
					gradientBounds = GetGradientBounds();
				}

				// Draw horizontal segment with gradient
				canvas.DrawLineWithGradient(x1, y1, x2, y1, Fill, (float)StrokeWidth, gradientBounds);

				// Draw vertical segment if needed
				if (X2 == x2)
				{
					canvas.DrawLineWithGradient(x2, y1, x2, y2, Fill, (float)StrokeWidth, gradientBounds);
				}
			}
			else
			{
				// Fallback to solid color rendering
				canvas.StrokeColor = Fill.ToColor();
				canvas.DrawLine(x1, y1, x2, y1);

				if (X2 == x2)
				{
					canvas.DrawLine(x2, y1, x2, y2);
				}
			}
		}

		internal override void DrawDynamicAnimation(float animationValue, float x1, float y1, ref float x2, ref float y2)
		{
			if (animationValue >= 0 && animationValue <= 0.5)
			{
				x2 = x1 + (x2 - x1) * animationValue * 2;
			}
			else
			{
				y2 = y1 + (y2 - y1) * (animationValue - 0.5f) * 2;
			}
		}

		#endregion
	}
}
