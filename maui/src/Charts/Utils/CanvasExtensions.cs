using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal static class CanvasExtensions
    {
        internal static void DrawShape(this ICanvas canvas, RectF rect, ShapeType shapeType, bool hasBorder, bool isSaveState)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);

            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    DrawRectangle(canvas, rect, hasBorder, isSaveState);
                    break;
                case ShapeType.Circle:
                    DrawCircle(canvas, x, y, rect.Width, rect.Height, hasBorder, isSaveState);
                    break;
                case ShapeType.HorizontalLine:
                    DrawLine(canvas, new PointF(x, midHeight), new PointF(width, midHeight), isSaveState);
                    break;
                case ShapeType.VerticalLine:
                    DrawLine(canvas, new PointF(midWidth, y), new PointF(midWidth, height), isSaveState);
                    break;
                case ShapeType.Triangle:
                    canvas.DrawTriangle(rect, hasBorder);
                    break;
                case ShapeType.InvertedTriangle:
                    canvas.DrawInverseTriangle(rect, hasBorder);
                    break;
                case ShapeType.Cross:
                    canvas.DrawCross(rect, hasBorder);
                    break;
                case ShapeType.Plus:
                    canvas.DrawPlus(rect, hasBorder);
                    break;
                case ShapeType.Diamond:
                    canvas.DrawDiamond(rect, hasBorder);
                    break;
                case ShapeType.Hexagon:
                    canvas.DrawHexagon(rect, hasBorder);
                    break;
                case ShapeType.Pentagon:
                    canvas.DrawPentagon(rect, hasBorder);
                    break;
            }
        }

        static void DrawLine(ICanvas canvas, PointF start, PointF end, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            canvas.DrawLine(start, end);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        static void DrawCircle(ICanvas canvas, float x, float y, float width, float height, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            canvas.FillEllipse(x, y, width, height);

            if (hasBorder)
            {
                canvas.DrawEllipse(x, y, width, height);
            }

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        static void DrawRectangle(ICanvas canvas, RectF rect, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            canvas.FillRectangle(rect);

            if (hasBorder)
            {
                canvas.DrawRectangle(rect);
            }

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        //Similar to canvas.ResetStroke(), this method resets only the StrokeDashArray for the trackball marker and label, while preserving the default stroke and stroke width.
        internal static void ResetStrokeDashPattern(this ICanvas canvas)
        {
            canvas.StrokeDashPattern = null;
        }
    }
}