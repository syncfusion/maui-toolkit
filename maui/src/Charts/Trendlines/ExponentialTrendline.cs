using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents an exponential trendline that calculates and displays an exponential regression curve.
    /// The exponential trendline follows the formula: y = a × e^(bx), where a and b are calculated coefficients.
    /// </summary>
    public class ExponentialTrendline : ChartTrendline
    {
        #region Fields

        double _rSquared;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Intercept"/> bindable property.
        /// </summary>
        public static readonly BindableProperty InterceptProperty =
            BindableProperty.Create(nameof(Intercept), typeof(double), typeof(ExponentialTrendline), 0d, BindingMode.Default, null, OnInterceptPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double Intercept
        {
            get { return (double)GetValue(InterceptProperty); }
            set { SetValue(InterceptProperty, value); }
        }

        /// <summary>
        /// Gets the coefficient of determination (R-squared) for the exponential regression.
        /// This value indicates how well the exponential trendline fits the data points.
        /// </summary>
        /// <value>The R-squared value (0.0 to 1.0, where 1.0 indicates perfect fit).</value>
        internal double RSquared
        {
            get => _rSquared;
            private set => _rSquared = value;
        }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates trendline points for an exponential regression:
		/// fits the model, updates RSquared, evaluates at midpoint/forecast X values,
		/// and populates XValues/YValues; exits early if axes are missing or regression fails.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();

            static bool PositiveY(double _, double y) => y > 0;

            if (!TryPrepareSourceData(3, out List<double> sourceX, out List<double> sourceY, PositiveY))
            {
                return;
            }

            (double slope, double intercept) = ComputeSlopeInterceptValues(sourceX, sourceY, sourceX.Count, Intercept);

            if (!ChartUtils.IsFinite(slope) || !ChartUtils.IsFinite(intercept))
            {
                return;
            }

            int midIndex = Math.Max(0, (sourceX.Count - 1) / 2);

            double x1 = ForecastValue(sourceX[0], -BackwardForecast);
            double x2 = sourceX[midIndex];
            double x3 = ForecastValue(sourceX[^1], ForwardForecast);

            double y1 = intercept * Math.Exp(slope * x1);
            double y2 = intercept * Math.Exp(slope * x2);
            double y3 = intercept * Math.Exp(slope * x3);

            XValues.AddRange(new[] { x1, x2, x3 });
            YValues.AddRange(new[]
            {
                ChartUtils.IsFinite(y1) ? y1 : 0,
                ChartUtils.IsFinite(y2) ? y2 : 0,
                ChartUtils.IsFinite(y3) ? y3 : 0
            });

            RSquared = ChartUtils.CalculateRSquaredForPoints(
                sourceX,
                sourceY,
                x => intercept * Math.Exp(slope * x));

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
        /// Draws the trendline as a smooth cubic curve and optionally renders markers; 
        /// skips when the series is null, empty, or has fewer than two points.
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
                x => x,
                y => Math.Log(y),
                raw => Math.Exp(raw),
                customIntercept);
        }

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles changes to the Intercept property.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnInterceptPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
