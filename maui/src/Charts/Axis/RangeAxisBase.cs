namespace Syncfusion.Maui.Toolkit.Charts
{
    public abstract partial class RangeAxisBase : ChartAxis
    {
        #region Fields
        internal List<double> SmallTickPoints { get; }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        internal virtual void AddSmallTicksPoint(double position, double interval)
        {
            double tickInterval = interval / (MinorTicksPerInterval + 1);
            double endPosition = position + interval;
            double tickPosition = position;

            if (SmallTickPoints.Count == 0 && tickPosition > VisibleRange.Start)
            {
                double tickStartPosition = position;

                while (tickStartPosition > VisibleRange.Start &&
                       tickStartPosition < VisibleRange.End)
                {
                    if (!(tickStartPosition == position))
                    {
                        SmallTickPoints.Add(tickStartPosition);
                    }

                    tickStartPosition -= tickInterval;
                }
            }

            while (tickPosition < endPosition && tickPosition < VisibleRange.End)
            {
                if (!(tickPosition == position) && VisibleRange.Inside(tickPosition))
                {
                    SmallTickPoints.Add(tickPosition);
                }

                tickPosition += tickInterval;

                double roundTickPosition = Math.Round(tickPosition * 100000000) / 100000000.0;

                if (roundTickPosition >= endPosition)
                {
                    tickPosition = endPosition;
                }
            }
        }
        #endregion
    }
}
