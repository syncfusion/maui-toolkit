using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Provides a drawable view for rendering Sunburst chart series segments.
    /// </summary>
    internal class SeriesView : SfDrawableView
    {
        #region Private Fields

        SfSunburstChart _chart;
        const string _animationName = "SunburstChartAnimation";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesView"/> class.
        /// </summary>
        /// <param name="chart">The sunburst chart to render.</param>
        internal SeriesView(SfSunburstChart chart)
        {
            _chart = chart;
        }

        #endregion

        #region Protected method

        /// <summary>
        /// Draws Sunburst segments on the canvas with selection state and animation support.
        /// </summary>
        /// <param name="canvas">The target drawing canvas.</param>
        /// <param name="rect">The dirty rectangle area to redraw.</param>
        protected override void OnDraw(ICanvas canvas, RectF rect)
        {
            canvas.SaveState();
            canvas.Translate(0, 0);

            if (_chart.Segments.Count > 0)
            {
                // Determine if selection is active
                var hasSelection = _chart.SelectionSettings != null && _chart.SelectedSunburstItems.Count > 0;
                var segmentsCollection = hasSelection ? _chart.Segments.OrderBy(s => s.IsSelected ? 1 : 0).ToList() : _chart.Segments.ToList();

#if WINDOWS
                foreach  (var segment in segmentsCollection)
                {
                    canvas.SaveState();
                    segment.Draw(canvas);
                    canvas.RestoreState();
                }
#else
				// Other platforms drawing approach
				canvas.SaveState();

				foreach (var segment in segmentsCollection)
                {
                    segment.Draw(canvas);
                }

                canvas.RestoreState();
#endif
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Updates the layout of each segment and triggers animation if enabled.
        /// </summary>
        internal void Layout()
        {
            if (_chart.Segments.Count > 0)
            {
                foreach (var segment in _chart.Segments)
                {
                    segment.OnLayout();
                }
            }

            if (_chart.CanAnimate())
            {
                StartAnimation();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles the animation step for the chart, updating the animation value and invalidating the view.
        /// </summary>
        /// <param name="value">The current animation progress value (0 to 1).</param>
        void OnAnimationStart(double value)
        {
            if (value >= 0.0)
            {
                _chart.AnimationValue = (float)value;

                if (_chart.NeedToAnimate)
                {
                    InvalidateDrawable();
                }
                else if (_chart.NeedToAnimateDataLabel)
                {
                    _chart.InvalidateDataLabel();
                }
            }
        }

        /// <summary>
        /// Called when an animation is finished, handling the transition to the next state or completing the animation cycle.
        /// </summary>
        /// <param name="value">The final animation value.</param>
        /// <param name="isCompleted">A flag indicating if the animation completed successfully.</param>
        void OnAnimationFinished(double value, bool isCompleted)
        {
            if (_chart.NeedToAnimate)
            {
                _chart.NeedToAnimate = false;
                InvalidateDrawable();
                _chart.InvalidateDataLabel();
                AbortAnimation();

                if (_chart.ShowLabels)
                {
                    _chart.NeedToAnimateDataLabel = true;
                    _chart.SunburstAnimation ?.Commit(this, _animationName, 16, 1000, null, OnAnimationFinished, () => false);
                }
            }
            else
            {
                _chart.NeedToAnimateDataLabel = false;
                AbortAnimation();
            }
        }

        /// <summary>
        /// Initializes and starts the chart's animation sequence.
        /// </summary>
        void StartAnimation()
        {
            //Todo: Need to move this code to series property changed event. Fow now added here.
            if (_chart.EnableAnimation && _chart.SunburstAnimation  == null)
            {
                _chart.SunburstAnimation  = new Animation(OnAnimationStart);
            }
            else if (!_chart.EnableAnimation && _chart.SunburstAnimation  != null)
            {
                AbortAnimation();
            }

			//chart.AnimateSunburstChart(OnAnimationStart);
			_chart.SunburstAnimation ?.Commit(this, _animationName, 1, (uint)(_chart.AnimationDuration * 1000), null, OnAnimationFinished, () => false);
		}

        /// <summary>
        /// Stops the currently active animation on the chart.
        /// </summary>
        void AbortAnimation()
        {
            this.AbortAnimation(_animationName);
            _chart.NeedToAnimate = false;
        }

        #endregion
    }
}
