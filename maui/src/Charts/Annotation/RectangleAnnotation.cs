using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// This class is used to add a rectangle annotation in <see cref="SfCartesianChart"/>. An instance of this class need to be added to <see cref="SfCartesianChart.Annotations"/> collection.
    /// </summary>
    /// <remarks>
    /// RectangleAnnotation is used to draw a rectangle over the chart area.
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart.Annotations>
    ///   <chart:RectangleAnnotation X1="1" Y1="10" X2="4" Y2="20">
    ///   </chart:RectangleAnnotation>
    /// </chart:SfCartesianChart.Annotations>  
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfCartesianChart chart = new SfCartesianChart();
    ///  var rectangle = new RectangleAnnotation()
    ///  {
    ///    X1 = 1,
    ///    Y1 = 10,
    ///    X2 = 4,
    ///    Y2 = 20,
    ///  };
    ///  
    /// chart.Annotations.Add(rectangle);
    /// ]]>
    /// </code>
    /// </example>
    public class RectangleAnnotation : ShapeAnnotation
    {
        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Chart != null)
            {
                if (CoordinateUnit == ChartCoordinateUnit.Axis)
                {
                    var clipRect = Chart.ChartArea.ActualSeriesClipRect;
                    canvas.ClipRectangle(clipRect);
                }

                canvas.SetFillPaint(Fill, RenderRect);
                canvas.FillRectangle(RenderRect);
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;

                if (StrokeDashArray != null && StrokeDashArray.Count > 0)
                {
                    canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
                }

                canvas.DrawRectangle(RenderRect);

                base.Draw(canvas, dirtyRect);
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition();

            if (X1 == null || X2 == null || double.IsNaN(Y1) || double.IsNaN(Y2))
                return;

            var x2 = ChartUtils.ConvertToDouble(X2);
            var y2 = Y2;

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                if (!xAxis.IsVertical)
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                    (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
                }
                else
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                    (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
                }
            }

            double x = x2 > x1 ? x1 : x2;
            double y = y2 > y1 ? y1 : y2;
            double width = x2 > x1 ? x2 - x1 : x1 - x2;
            double height = y2 > y1 ? y2 - y1 : y1 - y2;
            RenderRect = new Rect(x, y, width, height);

            if (!string.IsNullOrEmpty(Text))
            {
                SetTextAlignment(x, y);
            }
        }

        internal override void ResetPosition()
        {
            RenderRect = Rect.Zero;
            LabelRect = Rect.Zero;
        }

        #endregion

        #endregion
    }
}