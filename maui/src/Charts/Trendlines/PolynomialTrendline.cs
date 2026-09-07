using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a polynomial trendline that calculates and displays a polynomial regression curve.
    /// The polynomial trendline follows the formula: y = a₀ + a₁x + a₂x² + ... + aₙxⁿ, where n is the order.
    /// </summary>
    public class PolynomialTrendline : ChartTrendline
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Order"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OrderProperty =
            BindableProperty.Create(nameof(Order), typeof(int), typeof(PolynomialTrendline), 2, BindingMode.Default, OnOrderValidate, OnOrderPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the order (degree) of the polynomial trendline.
        /// Valid range is 2 to 6. Higher orders provide more flexible curve fitting but may lead to overfitting.
        /// </summary>
        /// <value>The order of the polynomial (default is 2 for quadratic).</value>
        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        /// <summary>
        /// Gets the calculated coefficients of the polynomial trendline.
        /// The array contains coefficients from a₀ (constant term) to aₙ (highest order term).
        /// </summary>
        /// <value>An array of polynomial coefficients.</value>
        internal double[] Coefficients { get; private set; } = Array.Empty<double>();

        /// <summary>
        /// Gets the coefficient of determination (R-squared) for the polynomial regression.
        /// This value indicates how well the polynomial trendline fits the data points.
        /// </summary>
        /// <value>The R-squared value (0.0 to 1.0, where 1.0 indicates perfect fit).</value>
        internal double RSquared { get; private set; }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates points for a polynomial regression: clears state, validates axes, fits coefficients,
		/// updates RSquared, and populates data coordinate points; exits early if prerequisites or regression fail.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();
            Coefficients = Array.Empty<double>();
            RSquared = 0;

            int minimumCount = Math.Max(2, Order + 1);
            if (!TryPrepareSourceData(minimumCount, out List<double> sourceX, out List<double> sourceY))
            {
                return;
            }

            if (!CalculatePolynomialRegression(sourceX, sourceY, out double[] calculatedCoefficients))
            {
                return;
            }

            Coefficients = calculatedCoefficients;

            RSquared = ChartUtils.CalculateRSquaredForPoints(
                sourceX,
                sourceY,
                x => EvaluatePolynomial(x, calculatedCoefficients));

            GenerateDataCoordinatePoints(calculatedCoefficients, sourceX);

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
        /// Draws the trendline as a smooth cubic curve across all points and, if applicable, renders markers; 
        /// skips drawing when the series is null, empty, or has fewer than two points.
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates polynomial regression coefficients using matrix-based least squares method.
        /// Based on dart implementation: _calculatePolynomialPoints method with Gauss-Jordan elimination.
        /// </summary>
        /// <param name="xValues">The X values.</param>
        /// <param name="yValues">The Y values.</param>
        /// <param name="coefficients">The calculated coefficients array.</param>
        /// <returns>True if calculation was successful; otherwise, false.</returns>
        bool CalculatePolynomialRegression(List<double> xValues, List<double> yValues, out double[] coefficients)
        {

            coefficients = Array.Empty<double>();

            int n = xValues.Count;
            int order = Order;

            if (n < order + 1)
            {
                return false;
            }

            var polynomialSlopes = new double[order + 1];

            for (int i = 0; i < n; i++)
            {
                double x = xValues[i];
                double y = yValues[i];
                if (ChartUtils.IsFinite(x) && ChartUtils.IsFinite(y))
                {
                    for (int j = 0; j <= order; j++)
                    {
                        polynomialSlopes[j] += Math.Pow(x, j) * y;
                    }
                }
            }

            var matrix = ComputeMatrix(xValues, order);

            if (!GaussJordanElimination(matrix, polynomialSlopes))
            {
                return false;
            }

            coefficients = polynomialSlopes;
            if (coefficients.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < coefficients.Length; i++)
            {
                if (!ChartUtils.IsFinite(coefficients[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Computes matrix for polynomial regression (from dart implementation).
        /// </summary>
        double[,] ComputeMatrix(List<double> xValues, int polynomialOrder)
        {
            int length = xValues.Count;
            var numArray = new double[2 * polynomialOrder + 1];
            var matrix = new double[polynomialOrder + 1, polynomialOrder + 1];

            for (int i = 0; i < length; i++)
            {
                double d = xValues[i];
                double num2 = 1.0;
                for (int j = 0; j < numArray.Length; j++)
                {
                    numArray[j] += num2;
                    num2 *= d;
                }
            }

            for (int k = 0; k <= polynomialOrder; k++)
            {
                for (int l = 0; l <= polynomialOrder; l++)
                {
                    matrix[k, l] = numArray[k + l];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gauss-Jordan elimination implementation from dart.
        /// </summary>
        static bool GaussJordanElimination(double[,] matrix, double[] polynomialSlopes)
        {
            int length = matrix.GetLength(0);
            var list1 = new int[length];
            var list2 = new int[length];
            var list3 = new int[length];

            for (int i = 0; i < length; i++)
            {
                list3[i] = 0;
            }

            int j = 0;
            while (j < length)
            {
                double value = 0;
                int k = 0, l = 0, m = 0;
                while (m < length)
                {
                    if (list3[m] != 1)
                    {
                        int n = 0;
                        while (n < length)
                        {
                            if (list3[n] == 0 && Math.Abs(matrix[m, n]) >= value)
                            {
                                value = Math.Abs(matrix[m, n]);
                                k = m;
                                l = n;
                            }
                            ++n;
                        }
                    }
                    ++m;
                }
                ++list3[l];

                if (k != l)
                {
                    for (int o = 0; o < length; o++)
                    {
                        double val = matrix[k, o];
                        matrix[k, o] = matrix[l, o];
                        matrix[l, o] = val;
                    }
                    double res = polynomialSlopes[k];
                    polynomialSlopes[k] = polynomialSlopes[l];
                    polynomialSlopes[l] = res;
                }

                list2[j] = k;
                list1[j] = l;

                if (matrix[l, l] == 0.0)
                {
                    return false;
                }

                double v = 1.0 / matrix[l, l];
                matrix[l, l] = 1.0;
                for (int p = 0; p < length; p++)
                {
                    matrix[l, p] *= v;
                }
                polynomialSlopes[l] *= v;

                for (int q = 0; q < length; q++)
                {
                    if (q != l)
                    {
                        double mVal = matrix[q, l];
                        matrix[q, l] = 0.0;
                        for (int r = 0; r < length; r++)
                        {
                            matrix[q, r] -= matrix[l, r] * mVal;
                        }
                        polynomialSlopes[q] -= polynomialSlopes[l] * mVal;
                    }
                }
                ++j;
            }

            for (int s = length - 1; s >= 0; s--)
            {
                if (list2[s] != list1[s])
                {
                    for (int t = 0; t < length; t++)
                    {
                        double number = matrix[t, list2[s]];
                        matrix[t, list2[s]] = matrix[t, list1[s]];
                        matrix[t, list1[s]] = number;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Calculates the coefficient of determination (R-squared) for the polynomial regression.
        /// </summary>
        /// <param name="xValues">The X values.</param>
        /// <param name="yValues">The Y values.</param>
        /// <param name="coefficients">The polynomial coefficients.</param>
        /// <returns>The R-squared value.</returns>
        double CalculateRSquared(List<double> xValues, List<double> yValues, double[] coefficients)
        {
            int n = xValues.Count;
            if (n < 2 || coefficients.Length == 0)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += yValues[i];
            }

            double yMean = sum / n;
            double totalSumSquares = 0;
            double residualSumSquares = 0;

            for (int i = 0; i < n; i++)
            {
                double actualY = yValues[i];
                double predictedY = EvaluatePolynomial(xValues[i], coefficients);

                totalSumSquares += Math.Pow(actualY - yMean, 2);
                residualSumSquares += Math.Pow(actualY - predictedY, 2);
            }

            if (Math.Abs(totalSumSquares) < 1e-15)
            {
                return 1.0;
            }

            return 1.0 - (residualSumSquares / totalSumSquares);
        }

        /// <summary>
        /// Evaluates the polynomial at a given x value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="coefficients">The polynomial coefficients.</param>
        /// <returns>The calculated y value.</returns>
        double EvaluatePolynomial(double x, double[] coefficients)
        {
            double result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * Math.Pow(x, i);
            }
            return result;
        }


        /// <summary>
        /// Generates data coordinate points for the polynomial trendline with forecasting support.
        /// Based on dart implementation: _computePoints method.
        /// </summary>
        /// <param name="coefficients">The calculated polynomial coefficients.</param>
        /// <param name="originalXValues">The original X values from the series.</param>
        /// <returns>A list of data coordinate points.</returns>
        void GenerateDataCoordinatePoints(double[] coefficients, List<double> originalXValues)
        {
            if (coefficients.Length == 0 || originalXValues.Count < 2)
            {
                return;
            }

            int length = originalXValues.Count;
            int polynomialSlopesLength = coefficients.Length;

            double trendBackwardForecast = BackwardForecast;
            double trendForwardForecast = ForwardForecast;

            double value = 1;
            for (int i = 1; i <= polynomialSlopesLength; i++)
            {
                double x1, y1;

                if (i == 1)
                {
                    x1 = originalXValues[0] - trendBackwardForecast;
                    y1 = ComputePolynomialYValue(coefficients, x1);
                }
                else if (i == polynomialSlopesLength)
                {
                    x1 = originalXValues[length - 1] + trendForwardForecast;
                    y1 = ComputePolynomialYValue(coefficients, x1);
                }
                else
                {
                    value += (length + trendForwardForecast) / polynomialSlopesLength;
                    x1 = originalXValues[Math.Max(0, Math.Min(length - 1, (int)Math.Floor(value) - 1))];
                    y1 = ComputePolynomialYValue(coefficients, x1);
                }

                if (ChartUtils.IsFinite(y1))
                {
                    XValues.Add(x1);
                    YValues.Add(y1);
                }
            }
        }

        /// <summary>
        /// Computes polynomial Y value for given X (from dart implementation).
        /// </summary>
        double ComputePolynomialYValue(double[] slopes, double x)
        {
            double sum = 0;
            for (int i = 0; i < slopes.Length; i++)
            {
                sum += slopes[i] * Math.Pow(x, i);
            }
            return sum;
        }

        /// <summary>
        /// Validates the Order property to ensure it's within the valid range (2-6).
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is valid; otherwise, false.</returns>
        static bool OnOrderValidate(BindableObject bindable, object value)
        {
            if (value is int order)
            {
                return order >= 2 && order <= 6;
            }
            return false;
        }

        /// <summary>
        /// Handles changes to the Order property.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnOrderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.MarkForRefresh();
                trendline.ScheduleUpdate();
            }
        }

		#endregion

		#endregion

	}
}
