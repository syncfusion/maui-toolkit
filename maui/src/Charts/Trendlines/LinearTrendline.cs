using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a linear trendline that calculates and displays a linear regression line.
    /// The linear trendline follows the formula: y = mx + c, where m is the slope and c is the y-intercept.
    /// This class follows the ChartSegment OnLayout/OnDraw pattern for calculation and rendering.
    /// </summary>
    public class LinearTrendline : ChartTrendline
    {

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Intercept"/> bindable property.
        /// </summary>
        public static readonly BindableProperty InterceptProperty =
            BindableProperty.Create(nameof(Intercept), typeof(double), typeof(LinearTrendline), 0d, BindingMode.Default, null, OnInterceptPropertyChanged);

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

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Generates points for a linear regression:
		/// fits the model from source values, evaluates at forecast x, validates results, and populates XValues/YValues.
		/// </summary>
		internal override void GeneratePoints()
        {
            ResetComputationState();

            if (!TryPrepareSourceData(2, out List<double> sourceX, out List<double> sourceY))
            {
                return;
            }

            (double slope, double intercept) = ComputeSlopeInterceptValues(sourceX, sourceY, sourceX.Count, Intercept);

            if (!ChartUtils.IsFinite(slope) || !ChartUtils.IsFinite(intercept))
            {
                return;
            }

            double startX = sourceX[0];
            double endX = sourceX[sourceX.Count - 1];

            double x1 = ForecastValue(startX, -BackwardForecast);
            double x2 = ForecastValue(endX, ForwardForecast);

            double y1 = slope * x1 + intercept;
            double y2 = slope * x2 + intercept;

            XValues.Add(x1);
            XValues.Add(x2);
            YValues.Add(ChartUtils.IsFinite(y1) ? y1 : 0);
            YValues.Add(ChartUtils.IsFinite(y2) ? y2 : 0);

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
        /// Draws the trendline as a straight line between the first two points and, if applicable, renders markers.
        /// Skips drawing when the series is null, empty, or has fewer than two points.
        /// </summary>
        /// <param name="canvas">The canvas on which to render the trendline.</param>
        internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty || XCoordinates.Count < 2)
            {
                return;
            }

            ConfigureCanvasRenderingStyle(canvas);

            canvas.DrawLine(XCoordinates[0], YCoordinates[0], XCoordinates[1], YCoordinates[1]);

            if (!Series.NeedToAnimateSeries)
            {
                DrawMarkers(canvas);
            }
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