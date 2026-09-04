using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a power trendline that calculates and displays a power regression curve.
    /// The power trendline follows the formula: y = a × x^b, where a and b are calculated coefficients.
    /// This implementation follows the dart trendline.dart logic for power regression.
    /// </summary>
    public class PowerTrendline : ChartTrendline
    {
        #region Private Fields

        double _coefficientA;
        double _coefficientB;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the calculated coefficient 'a' of the power trendline.
        /// This value represents the scaling factor in the equation y = a × x^b.
        /// </summary>
        /// <value>The coefficient 'a' of the power regression.</value>
        internal double CoefficientA
        {
            get => _coefficientA;
            private set => _coefficientA = value;
        }

        /// <summary>
        /// Gets the calculated coefficient 'b' of the power trendline.
        /// This value represents the power exponent in the equation y = a × x^b.
        /// </summary>
        /// <value>The coefficient 'b' of the power regression.</value>
        internal double CoefficientB
        {
            get => _coefficientB;
            private set => _coefficientB = value;
        }

        /// <summary>
        /// Gets the coefficient of determination (R-squared) for the power regression.
        /// This value indicates how well the power trendline fits the data points.
        /// </summary>
        /// <value>The R-squared value (0.0 to 1.0, where 1.0 indicates perfect fit).</value>
        internal double RSquared { get; private set; }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates points for a power regression: validates axes and minimum count, fits coefficients,
		/// updates CoefficientA/CoefficientB and RSquared, and populates data coordinate points; exits early on failure.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();
            CoefficientA = 0;
            CoefficientB = 0;
            RSquared = 0;

            if (!TryPrepareSourceData(3, out List<double> sourceX, out List<double> sourceY))
            {
                return;
            }

            (double slope, double intercept) = ComputeSlopeInterceptValues(sourceX, sourceY, sourceX.Count);

            if (!ChartUtils.IsFinite(slope) || !ChartUtils.IsFinite(intercept))
            {
                return;
            }

            CoefficientA = intercept;
            CoefficientB = slope;

            RSquared = ChartUtils.CalculateRSquaredForPoints(
                sourceX,
                sourceY,
                x => intercept * Math.Pow(x, slope));

            GenerateDataCoordinatePoints(intercept, slope, sourceX);

            if (XValues.Count < 2 || YValues.Count < 2)
            {
                XValues.Clear();
                YValues.Clear();
                Empty = true;
                return;
            }

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
        /// Renders the power trendline using screen coordinates and canvas operations.
        /// This method follows the ChartSegment Draw pattern exactly like LineSegment.
        /// </summary>
        /// <param name="canvas">The canvas on which to render the trendline.</param>
        internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty || XCoordinates.Count < 2)
            {
                return;
            }

            ConfigureCanvasRenderingStyle(canvas);
            DrawCubicCurveToCanvas(canvas);    

            if (!Series.NeedToAnimateSeries)
            {
                DrawMarkers(canvas);
            }
        }

        /// <inheritdoc/>
        internal override (double slope, double intercept) ComputeSlopeInterceptValues(
          List<double> xValues, List<double> yValues, int length, double? customIntercept = null)
        {

            return ComputeTransformedRegression(
                xValues, yValues, length,
                x => double.IsFinite(Math.Log(x)) ? Math.Log(x) : x,
                y => Math.Log(y),
                raw => Math.Exp(raw),
                null);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates power points following exact dart implementation of _calculatePowerPoints.
        /// Creates 3 points (start, mid, end) with forecasting support.
        /// </summary>
        /// <param name="intercept">The calculated intercept coefficient.</param>
        /// <param name="slope">The calculated slope coefficient.</param>
        /// <param name="inputXValues">The original valid data points.</param>
        /// <returns>A list of power trendline points.</returns>
        private void GenerateDataCoordinatePoints(double intercept, double slope, List<double> inputXValues)
        {
            if (inputXValues.Count < 2)
            {
                return;
            }

            List<double> xValues = new List<double>(inputXValues);
            xValues.Sort();

            int length = xValues.Count;
            int midPointIndex = Math.Max(0, (int)Math.Round(length / 2.0) - 1);

            double x1 = ForecastValue(xValues[0], -BackwardForecast);
            double x2 = xValues[midPointIndex];
            double x3 = ForecastValue(xValues[^1], ForwardForecast);

            if (x1 <= 0)
            {
                x1 = x1 > -1 ? x1 : 0;
            }

            double y1 = x1 == 0 ? 0 : intercept * Math.Pow(x1, slope);
            double y2 = intercept * Math.Pow(x2, slope);
            double y3 = intercept * Math.Pow(x3, slope);

            XValues.AddRange(new[] { x1, x2, x3 });
            YValues.AddRange(new[]
            {
                double.IsNaN(y1) ? 0 : y1,
                double.IsNaN(y2) ? 0 : y2,
                double.IsNaN(y3) ? 0 : y3
            });
        }


		#endregion

		#endregion
	}
}
