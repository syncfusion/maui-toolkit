using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a moving average trendline that calculates and displays a moving average curve.
    /// The moving average trendline smooths data by creating a series of averages over a specified period.
    /// This implementation follows the dart trendline.dart logic for moving average calculation.
    /// </summary>
    public class MovingAverageTrendline : ChartTrendline
	{
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Period"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PeriodProperty =
            BindableProperty.Create(nameof(Period), typeof(int), typeof(MovingAverageTrendline), 2, BindingMode.Default, OnPeriodValidate, OnPeriodPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the period (number of data points) for the moving average calculation.
        /// Valid range is 2 to the number of data points minus 1. Higher periods provide smoother curves but with more lag.
        /// </summary>
        /// <value>The period of the moving average (default is 2).</value>
        public int Period
        {
            get { return (int)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates points for a moving average trendline: clears state, validates axes/period,
		/// retrieves source values, and computes points; exits early if prerequisites fail.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();

            int minimumCount = Math.Max(Period, 2);
            if (!TryPrepareSourceData(minimumCount, out List<double> sourceX, out List<double> sourceY))
            {
                return;
            }

            CalculateMovingAveragePoints(sourceX, sourceY);

            UpdateBoundsFromValues();
        }

        /// <summary>
        /// Transforms data points to visible coordinates, rebuilds XPoints/YPoints,
        /// and sets Empty when fewer than two points are available.
        /// </summary>
        internal override void OnLayout()
        {
            base.OnLayout();
        }

        /// <summary>
        /// Draws the trendline as a connected polyline over all points and, if applicable, renders markers; 
        /// skips when series is null, empty, or has fewer than two points.
        /// </summary>
        /// <param name="canvas">The canvas on which to render the trendline.</param>
        internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty || XCoordinates.Count < 2)
            {
                return;
            }

            ConfigureCanvasRenderingStyle(canvas);

            PathF path = new PathF();

            path.MoveTo(XCoordinates[0], YCoordinates[0]);

            for (int i = 1; i < XCoordinates.Count; i++)
            {
                path.LineTo(XCoordinates[i], YCoordinates[i]);
            }

            canvas.DrawPath(path);

            if (!Series.NeedToAnimateSeries)
            {
                DrawMarkers(canvas);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates moving average points following exact dart implementation.
        /// Based exactly on dart implementation: _calculateMovingAveragePoints method.
        /// </summary>
        /// <param name="xValues">The sorted X values.</param>
        /// <param name="yValues">The Y values.</param>
        /// <returns>A list of moving average points.</returns>
        void CalculateMovingAveragePoints(List<double> xValues, List<double> yValues)
        {
            int xLength = xValues.Count;
            int yLength = yValues.Count;
            int trendPeriod = Period;

            int periods = trendPeriod >= xLength ? xLength - 1 : trendPeriod;
            periods = Math.Max(2, periods);

            double? x1;
            double? y1;

            for (int i = 0; i < xLength - 1; i++)
            {
                y1 = 0.0;
                int count = 0;
                int nullCount = 0;

                for (int j = i; count < periods; j++)
                {
                    count++;
                    if (j >= yLength)
                    {
                        nullCount++;
                    }
                    y1 = y1.Value + (j >= yLength ? 0 : yValues[j]);
                }

                y1 = ((periods - nullCount) <= 0) ? (double?)null : (y1.Value / (periods - nullCount));

                if (y1.HasValue && !double.IsNaN(y1.Value) && i + periods < xLength + 1)
                {
                    x1 = xValues[periods - 1 + i];
                    XValues.Add(x1.Value);
                    YValues.Add(y1.Value);
                }
            }
        }

        /// <summary>
        /// Validates the Period property to ensure it's within the valid range (2 to data length - 1).
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is valid; otherwise, false.</returns>
        static bool OnPeriodValidate(BindableObject bindable, object value)
        {
            if (value is int period)
            {
                return period >= 2;
            }
            return false;
        }

        /// <summary>
        /// Handles changes to the Period property.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnPeriodPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.GeneratePoints();
                trendline.OnLayout();
                trendline.InvalidateTrendlines();
            }
        }

		#endregion

		#endregion

	}
}
