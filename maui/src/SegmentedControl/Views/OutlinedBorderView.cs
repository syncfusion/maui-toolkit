using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a outlined border view that draws a rounded rectangle border around the segmented control.
	/// </summary>
	internal partial class OutlinedBorderView : SfDrawableView
	{
		#region Fields

		/// <summary>
		/// The segment item info.
		/// </summary>
		readonly ISegmentItemInfo? _itemInfo;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="OutlinedBorderView"/> class.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		internal OutlinedBorderView(ISegmentItemInfo itemInfo)
		{
			_itemInfo = itemInfo;
			InputTransparent = true;
		}

		#endregion

		#region Internal methods

		#endregion

		#region Override methods

		/// <summary>
		/// Method to draw the outlined border for the segmented control.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The area to draw within.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			// In Android, we manually restrict line drawing if stroke thickness is zero.
			if (_itemInfo == null || _itemInfo.StrokeThickness == 0)
			{
				return;
			}

			canvas.CanvasSaveState();

			// Set up the stroke properties.
			float strokeThickness = (float)_itemInfo.StrokeThickness;
			float strokeRadius = (float)_itemInfo.StrokeThickness / 2f;

			// Calculate the coordinates for the draw rectangle with the stroke value.
			float left = dirtyRect.X + strokeRadius;
			float right = dirtyRect.Width - strokeThickness;
			float top = dirtyRect.Top + strokeRadius;
			float bottom = dirtyRect.Height - strokeThickness;

			// Ensure that the calculated values are within valid ranges.
			left = Math.Max(left, dirtyRect.Left);
			top = Math.Max(top, dirtyRect.Top);
			RectF drawRect = new RectF(left, top, right, bottom);
			CornerRadius cornerRadius = _itemInfo.CornerRadius;
			canvas.Antialias = true;
			canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(_itemInfo.Stroke);
			canvas.StrokeSize = strokeThickness;

			// Extract the corner radius values and subtracting stroke radius value to resolve the stroke thickness cropping issue.
			float radiusTopLeft = (float)cornerRadius.TopLeft - strokeRadius;
			float radiusTopRight = (float)cornerRadius.TopRight - strokeRadius;
			float radiusBottomRight = (float)cornerRadius.BottomRight - strokeRadius;
			float radiusBottomLeft = (float)cornerRadius.BottomLeft - strokeRadius;

			// Draw the segment view with rounded corners.
			canvas.DrawRoundedRectangle(drawRect.Left, drawRect.Top, drawRect.Width, drawRect.Height,
										radiusTopLeft, radiusTopRight, radiusBottomRight, radiusBottomLeft);
			canvas.CanvasRestoreState();
			base.OnDraw(canvas, dirtyRect);
		}

		#endregion
	}
}