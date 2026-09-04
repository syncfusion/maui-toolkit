using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the layout for rendering trendlines in the chart.
	/// </summary>
	internal class ChartTrendlineView : SfDrawableView
	{
		#region Fields

		readonly ChartPlotArea plotArea;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartTrendlineView"/> class.
		/// </summary>
		/// <param name="plotArea">The parent plot area.</param>
		internal ChartTrendlineView(ChartPlotArea plotArea)
		{
			this.plotArea = plotArea;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Called when the view needs to be drawn.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.SaveState();
			canvas.ClipRectangle(dirtyRect);
			DrawTrendlines(canvas, dirtyRect);
			canvas.RestoreState();
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Draws all trendlines for the visible series in the plot area.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="bounds">The drawing bounds.</param>
		internal void DrawTrendlines(ICanvas canvas, RectF bounds)
		{
			if (canvas == null || plotArea?._chart is not SfCartesianChart chart)
				return;

			// Directly iterate through all series and check for trendlines during iteration
			foreach (var series in chart.Series)
			{
				// Check if series is CartesianSeries and has trendlines in one condition
				if (series is CartesianSeries cartesianSeries &&
					cartesianSeries.Trendlines?.Count > 0)
				{
					if (series.NeedToAnimateSeries)
					{
						DrawTrendlineWithAnimation(canvas, cartesianSeries);
					}
					else
						DrawTrendlines(canvas, cartesianSeries);
				}
			}
		}

		internal void AnimateSeriesClipRect(ICanvas canvas, float animationValue, CartesianSeries series)
		{
			if (series != null && series.EnableAnimation && series.ChartArea is CartesianChartArea chartArea)
			{
				RectF seriesClipRect = series.AreaBounds;

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

		/// <summary>
		/// Called after the series OnLayout method to handle trendline layout calculations.
		/// Iterates through trendlines collection from series and calls each trendline's OnLayout method.
		/// </summary>
		/// <param name="series">The CartesianSeries containing trendlines.</param>
		internal void OnTrendlineLayout(CartesianSeries series)
		{
			if (series == null || !HasTrendlines(series))
				return;

			var trendlines = GetTrendlines(series);
			if (trendlines != null)
			{
				foreach (var trendline in trendlines)
				{
					if (trendline != null && trendline.IsVisible)
					{
						trendline.OnLayout();
					}
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks if a series has trendlines.
		/// </summary>
		/// <param name="series">The series to check.</param>
		/// <returns>True if the series has trendlines; otherwise, false.</returns>
		static bool HasTrendlines(CartesianSeries series)
		{
			return series.Trendlines != null && series.Trendlines.Count > 0;
		}

		/// <summary>
		/// Gets the trendlines collection from a series.
		/// </summary>
		/// <param name="series">The series.</param>
		/// <returns>The trendlines collection or null if not available.</returns>
		ChartTrendlineCollection? GetTrendlines(CartesianSeries series)
		{
			return series.Trendlines;
		}

		void DrawTrendlineWithAnimation(ICanvas canvas, CartesianSeries series)
		{
			canvas.SaveState();

			AnimateSeriesClipRect(canvas, series.AnimationValue, series);

			DrawTrendlines(canvas, series);
			canvas.RestoreState();

		}

		void DrawTrendlines(ICanvas canvas, CartesianSeries series)
		{

			foreach (var trendline in series.Trendlines)
			{

				if (series.IsVisible && trendline?.IsVisible == true)
				{
					trendline.Draw(canvas);
				}
			}
		}

		#endregion
	}
}
