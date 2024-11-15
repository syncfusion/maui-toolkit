using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="SfPyramidChart"/>, responsible for rendering and customizing the appearance of individual pyramid segments.
    /// </summary>
    public class PyramidSegment : ChartSegment, IPyramidLabels
    {
        #region Fields

        internal IPyramidChartDependent? Chart { get; set; }
        RectF _actualBounds;
        float[] _values = new float[8];

        #endregion

        #region Properties
        /// <summary>
        /// Represents the draw points of the pyramid segments.
        /// </summary>
        /// <remarks>
        /// <para>Pyramid segments are often drawn with four points, which are listed below.</para>
        /// Points[0] : Top Left
        /// Points[1] : Top Right
        /// Points[2] : Bottom Right
        /// Points[3] : Bottom Left
        /// </remarks>
        public Point[] Points { get; internal set; } = new Point[4];

        double yValue;
        double height;

        double dataLabelYValue;
        object? dataLabelXValue;
        Rect labelRect;
        float labelXPosition;
        float labelYPosition;
        Point[]? linePoints = new Point[3];
        bool isIntersected = false;
        bool isLabelVisible = true;
        Size dataLabelSize = Size.Zero;
        Size actualSize = Size.Zero;
        Point slopeCenter = Point.Zero;
        DataLabelPlacement position = DataLabelPlacement.Auto;
        float IPyramidLabels.DataLabelX { get => labelXPosition; set => labelXPosition = value; }
        float IPyramidLabels.DataLabelY { get => labelYPosition; set => labelYPosition = value; }
        Point[]? IPyramidLabels.LinePoints { get => linePoints; set => linePoints = value; }
        Rect IPyramidLabels.LabelRect { get => labelRect; set => labelRect = value; }
        Size IPyramidLabels.DataLabelSize { get => dataLabelSize; set => dataLabelSize = value; }
        Size IPyramidLabels.ActualLabelSize { get => actualSize; set => actualSize = value; }
        string IPyramidLabels.DataLabel => LabelContent ?? string.Empty;
        bool IPyramidLabels.IsEmpty { get => Empty; set => Empty = value; }
        bool IPyramidLabels.IsIntersected { get => isIntersected; set => isIntersected = value; }
        bool IPyramidLabels.IsLabelVisible { get => isLabelVisible; set => isLabelVisible = value; }
        Brush IPyramidLabels.Fill => Fill ?? Brush.Transparent;
        Point IPyramidLabels.SlopePoint => slopeCenter;
        DataLabelPlacement IPyramidLabels.Position { get => position; set => position = value; }

        #endregion

        #region Methods

        #region Internal Methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the pyramid segment.
        /// </summary>
        internal void SetData(double x, double y, object? xVal, double yVal)
        {
            yValue = x;
            height = y;
            dataLabelYValue = yVal;
            dataLabelXValue = xVal;
            Empty = double.IsNaN(yVal) || yVal == 0;
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (SegmentBounds.Contains(x, y))
            {
                var slopeLeft = Math.Atan((_values[6] - _values[0]) / (_values[7] - _values[1]));
                var slopeRight = Math.Atan((_values[4] - _values[2]) / (_values[5] - _values[3]));
                if ((Math.Atan((_values[6] - x) / (_values[7] - y)) <= slopeLeft && slopeRight <= (Math.Atan((_values[4] - x) / (_values[5] - y)))))
                {
                    return Index;
                }
            }

            return -1;
        }

        #endregion

        #region Protected Internal methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Chart != null)
            {
                _actualBounds = new RectF(new Point(0, 0), Chart.SeriesBounds.Size);
                double boundsHeight = _actualBounds.Height;
                double boundsWidth = _actualBounds.Width;
                double boundsTop = _actualBounds.Top;
                double boundsLeft = _actualBounds.Left;
                float top = (float)yValue;
                float bottom = (float)(yValue + height);
                float topRadius = 0.5f * (1 - (float)yValue);
                float bottomRadius = 0.5f * (1 - bottom);

                _values = new float[8];
                _values[0] = (float)(boundsLeft + (topRadius * boundsWidth));
                _values[1] = _values[3] = (float)(boundsTop + (top * boundsHeight));
                _values[2] = (float)(boundsLeft + ((1 - topRadius) * boundsWidth));
                _values[4] = (float)(boundsLeft + ((1 - bottomRadius) * boundsWidth));
                _values[5] = _values[7] = (float)(boundsTop + (bottom * boundsHeight));
                _values[6] = (float)(boundsLeft + (bottomRadius * boundsWidth));

                Points[0] = new Point(_values[0], _values[1]);
                Points[1] = new Point(_values[2], _values[3]);
                Points[2] = new Point(_values[4], _values[5]);
                Points[3] = new Point(_values[6], _values[7]);

                SegmentBounds = new RectF(_values[6], _values[1], _values[4] - _values[6], _values[5] - _values[1]);
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            PathF path = new PathF();
            path.MoveTo(Points[0]);

            for (int i = 1; i < Points.Length; i++)
            {
                path.LineTo(Points[i]);
            }

            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.FillPath(path);

            if (HasStroke)
            {
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #endregion

        #region Data label

        internal override void OnDataLabelLayout()
        {
            if (Chart != null && !Empty && Chart.DataLabelSettings.LabelStyle is ChartLabelStyle style)
            {
                labelXPosition = (float)Chart.SeriesBounds.X + SegmentBounds.Center.X;
                labelYPosition = (float)Chart.SeriesBounds.Y + SegmentBounds.Center.Y;

                LabelContent = Chart.DataLabelSettings.GetLabelContent(dataLabelXValue, dataLabelYValue);
                var left = ((_values[0] + _values[6]) / 2) + Chart.SeriesBounds.Left;
                var right = ((_values[2] + _values[4]) / 2) + Chart.SeriesBounds.Left;
                var y = ((_values[3] + _values[5]) / 2) + Chart.SeriesBounds.Top;
                slopeCenter = new Point(right - 3, y);

                //Add the DataLabel property.
                UpdateDataLabels();

                dataLabelSize = Chart.LabelTemplate is null ? LabelContent.Measure(style) : CalculateTemplateSize();
                actualSize = new Size(dataLabelSize.Width + style.Margin.HorizontalThickness, dataLabelSize.Height + style.Margin.VerticalThickness);

                position = DataLabelPlacement.Inner;
                switch (Chart.DataLabelSettings.LabelPlacement)
                {
                    case DataLabelPlacement.Center:
                    case DataLabelPlacement.Auto:
                        if (actualSize.Width > right - left || actualSize.Height > SegmentBounds.Height)
                            position = DataLabelPlacement.Outer;
                        break;
                    case DataLabelPlacement.Outer:
                        position = DataLabelPlacement.Outer;
                        break;
                }

                Chart.DataLabelHelper.AddLabel(this);
            }
        }

        internal override void UpdateDataLabels()
        {
            var dataLabelSettings = Chart?.DataLabelSettings;

            if (Chart == null || dataLabelSettings == null)
            {
                return;
            }

            if (DataLabels != null && DataLabels.Count > 0)
            {
                ChartDataLabel dataLabel = DataLabels[0];

                dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
                dataLabel.Background = dataLabelSettings.LabelStyle.Background;
                dataLabel.Index = Index;
                dataLabel.Item = Item;
                dataLabel.Label = LabelContent ?? string.Empty;
            }
        }

        private Size CalculateTemplateSize()
        {
            if (Chart is SfPyramidChart pyramidChart)
            {
                if (pyramidChart.LabelTemplateView != null && pyramidChart.LabelTemplateView.Any())
                {
                    var templateView = pyramidChart.LabelTemplateView.Cast<View>().FirstOrDefault(child => DataLabels[0] == child.BindingContext) as DataLabelItemView;

                    if (templateView != null && templateView.ContentView is View content)
                    {
                        if (!content.DesiredSize.IsZero)
                        {
                            return content.DesiredSize;
                        }

#if NET9_0_OR_GREATER
                        var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

                        if (desiredSize.IsZero)
                            return content.Measure(double.PositiveInfinity, double.PositiveInfinity);
#else
						var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

						if (desiredSize.IsZero)
							return content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
#endif

						return desiredSize;
                    }
                }
            }

            return SizeF.Zero;
        }

        #endregion
    }
}