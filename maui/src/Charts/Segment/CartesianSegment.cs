namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents the base class for various types of cartesian chart segments, including 
    /// <see cref="AreaSegment"/>, <see cref="ColumnSegment"/>, <see cref="LineSegment"/>, 
    /// <see cref="SplineSegment"/>, <see cref="ScatterSegment"/>, <see cref="FastLineSegment"/> and so on.
    /// </summary>
    public abstract class CartesianSegment : ChartSegment
    {
        #region Methods

        #region Animation Methods

        internal float GetDynamicAnimationValue(float animationValue, float value, float oldValue, float newValue)
        {
            if (!double.IsNaN(oldValue) && !double.IsNaN(newValue))
            {
                return (newValue > oldValue) ?
	                oldValue + ((newValue - oldValue) * animationValue)
	                : oldValue - ((oldValue - newValue) * animationValue);
            }
            else
            {
                return double.IsNaN(oldValue) ? newValue : oldValue - (oldValue * animationValue);
            }
        }

        internal void AnimateSeriesClipRect(ICanvas canvas, float animationValue)
        {
            if (Series is CartesianSeries cartesianSeries && cartesianSeries.EnableAnimation && cartesianSeries.ChartArea is CartesianChartArea chartArea)
            {
                RectF seriesClipRect = cartesianSeries.AreaBounds;

                if (chartArea.IsTransposed)
                {
                    canvas.ClipRectangle(0, seriesClipRect.Height - (seriesClipRect.Height * animationValue), seriesClipRect.Width, seriesClipRect.Height);
                }
                else
                {
                    canvas.ClipRectangle(0, 0, seriesClipRect.Right * animationValue, seriesClipRect.Bottom);
                }
            }
        }

        #endregion

        #region DataLabel Methods

        internal void CalculateDataLabelPosition(double xValue, double yValue, double actualYValues)
        {
            if (!(Series is XYDataSeries xyDataSeries) || xyDataSeries.ChartArea == null || !xyDataSeries.ShowDataLabels || xyDataSeries.DataLabelSettings == null)
            {
                return;
            }

            IsEmpty = double.IsNaN(yValue);

            double x = xValue, y = xyDataSeries.GetDataLabelPositionAtIndex(Index);

            InVisibleRange = xyDataSeries.IsDataInVisibleRange(xValue, y);
            xyDataSeries.CalculateDataPointPosition(Index, ref x, ref y);

            DataLabelXPosition = x;
            DataLabelYPosition = y;
            LabelContent = xyDataSeries.GetLabelContent(actualYValues, xyDataSeries.SumOfValues(xyDataSeries.YValues));

            UpdateDataLabels();
        }

        internal override void UpdateDataLabels()
        {
            if (Series is XYDataSeries series && series.DataLabelSettings != null && DataLabels != null && DataLabels.Count > 0)
            {
                var dataLabelSettings = series.DataLabelSettings;

                ChartDataLabel dataLabel = DataLabels[0];

                dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
                dataLabel.Background = dataLabelSettings.LabelStyle.Background;
                dataLabel.Index = Index;
                dataLabel.Item = Item;
                dataLabel.Label = LabelContent ?? string.Empty;

                LabelPositionPoint = InVisibleRange && !IsEmpty ? dataLabelSettings.CalculateDataLabelPoint(series, this, new PointF((float)DataLabelXPosition, (float)DataLabelYPosition), dataLabelSettings.LabelStyle) : new PointF(float.NaN, float.NaN);

                dataLabel.XPosition = LabelPositionPoint.X;
                dataLabel.YPosition = LabelPositionPoint.Y;
            }
        }

        #endregion

        #endregion
    }
}
