using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a logarithmic trendline that calculates and displays a logarithmic regression curve.
    /// The logarithmic trendline follows the formula: y = a × ln(x) + b, where a and b are calculated coefficients.
    /// </summary>
    public class LogarithmicTrendline : ChartTrendline
    {
        #region Fields

        double _coefficientA;
        double _coefficientB;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the calculated coefficient 'a' of the logarithmic trendline.
        /// This value represents the slope in the logarithmic equation y = a × ln(x) + b.
        /// </summary>
        /// <value>The coefficient 'a' of the logarithmic regression.</value>
        internal double CoefficientA
        {
            get => _coefficientA;
            private set => _coefficientA = value;
        }

        /// <summary>
        /// Gets the calculated coefficient 'b' (intercept) of the logarithmic trendline.
        /// This value represents the y-intercept in the equation y = a × ln(x) + b.
        /// </summary>
        /// <value>The coefficient 'b' of the logarithmic regression.</value>
        internal double CoefficientB
        {
            get => _coefficientB;
            private set => _coefficientB = value;
        }

        /// <summary>
        /// Gets the coefficient of determination (R-squared) for the logarithmic regression.
        /// This value indicates how well the logarithmic trendline fits the data points.
        /// </summary>
        /// <value>The R-squared value (0.0 to 1.0, where 1.0 indicates perfect fit).</value>
        internal double RSquared { get; private set; }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates trendline data by computing a logarithmic regression from source values,
		/// updates CoefficientA, CoefficientB, RSquared, and populates rendered points;
		/// skips when axes are missing or regression fails, leaving the series empty.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();

            if (!TryPrepareSourceData(3, out List<double> sourceX, out List<double> sourceY))
            {
                return;
            }

            (double slope, double intercept) = ComputeSlopeInterceptValues(sourceX, sourceY, sourceX.Count);

            if (!ChartUtils.IsFinite(slope) || !ChartUtils.IsFinite(intercept))
            {
                return;
            }

            CoefficientA = slope;
            CoefficientB = intercept;

            RSquared = ChartUtils.CalculateRSquaredForPoints(
                sourceX,
                sourceY,
                x =>
                {
                    double logX = Math.Log(x);
                    return intercept + (slope * (double.IsFinite(logX) ? logX : x));
                });

            GenerateDataCoordinatePoints(slope, intercept, sourceX);

            if (XValues.Count >= 3 && YValues.Count >= 3)
            {
                XMin = Math.Min(XValues[0], Math.Min(XValues[1], XValues[2]));
                XMax = Math.Max(XValues[0], Math.Max(XValues[1], XValues[2]));
                YMin = Math.Min(YValues[0], Math.Min(YValues[1], YValues[2]));
                YMax = Math.Max(YValues[0], Math.Max(YValues[1], YValues[2]));
                Empty = false;
            }
            else
            {
                UpdateBoundsFromValues();
            }
        }

        /// <summary>
        /// Lays out the trendlines by transforming data points to visible coordinates, rebuilding XPoints/YPoints,
        /// and setting Empty when fewer than two points are available.
        /// </summary>
        internal override void OnLayout()
        {
            base.OnLayout();
        }

        /// <summary>
        /// Renders the trendline as a cubic curve on the provided canvas and optionally draws markers.
        /// Skips rendering if the series is null, empty, or has fewer than two data points.
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

            if (ShowMarkers && MarkerSettings != null && !Series.NeedToAnimateSeries)
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
                x => double.IsFinite(Math.Log(x)) ? Math.Log(x) : 1 + x,
                y => y,
                raw => raw,
                customIntercept);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates logarithmic points following exact dart implementation of _calculateLogarithmicPoints.
        /// Creates 3 points (start, mid, end) with forecasting support.
        /// </summary>
        /// <param name="slope">The calculated slope coefficient.</param>
        /// <param name="intercept">The calculated intercept coefficient.</param>
        /// <param name="xValues">The sorted x values.</param>
        /// <returns>A list of logarithmic trendline points.</returns>
        void GenerateDataCoordinatePoints(double slope, double intercept, List<double> xValues)
        {
            int length = xValues.Count;
            if (length < 2)
            {
                return;
            }

            int midPoint = (int)Math.Round((double)length / 2.0);
            int midPointIndex = Math.Max(0, midPoint - 1);

            double x1 = ForecastValue(xValues[0], -BackwardForecast);
            double x2 = xValues[midPointIndex];
            double x3 = ForecastValue(xValues[length - 1], ForwardForecast);

            double y1 = intercept + (slope * (double.IsFinite(Math.Log(x1)) ? Math.Log(x1) : x1));
            double y2 = intercept + (slope * (double.IsFinite(Math.Log(x2)) ? Math.Log(x2) : x2));
            double y3 = intercept + (slope * (double.IsFinite(Math.Log(x3)) ? Math.Log(x3) : x3));

            XValues.Add(x1);
            XValues.Add(x2);
            XValues.Add(x3);

            YValues.Add(y1);
            YValues.Add(y2);
            YValues.Add(y3);
        }

		#endregion

		#endregion

	}
}
