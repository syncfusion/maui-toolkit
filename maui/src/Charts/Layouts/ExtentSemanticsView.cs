using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// A transparent semantics layer placed over the circular chart plot area.
	/// It exposes one <see cref="SemanticsNode"/> per rendered segment so the
	/// platform accessibility services (screen readers) can focus and announce
	/// each slice.
	/// </summary>
	internal class ExtentSemanticsView : SfView
	{
		#region Fields

		internal List<SemanticsNode>? semanticsNodes;
		internal Size semanticsSize = new Size(-1, -1);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentSemanticsView"/> class.
        /// </summary>
        internal ExtentSemanticsView()
		{
            // Keep this host in the native accessibility tree. Its semantic children 
            // handle accessibility actions without drawing over the chart. 
            DrawingOrder = DrawingOrder.AboveContentWithTouch;
            InputTransparent = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the chart area that provides segment data.
		/// </summary>
		internal AreaBase? ChartArea { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Builds the semantics nodes for the visible chart segments.
		/// </summary>
		protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
		{
			if (width <= 0 || height <= 0 || ChartArea == null)
			{
				return null;
			}

			var _currentSize = new Size(width, height);
			if (semanticsNodes != null && semanticsNodes.Count > 0 && semanticsSize == _currentSize)
			{
				return semanticsNodes;
			}

			var nodes = new List<SemanticsNode>();

            IEnumerable<ChartSeries>? seriesCollection = ChartArea switch
            {
                CircularChartArea circularChartArea => (circularChartArea.PlotArea as CircularPlotArea)?.Series,
                PolarChartArea polarChartArea => polarChartArea.Series?.Cast<ChartSeries>(),
				CartesianChartArea cartesianChartArea => cartesianChartArea.Series,
                _ => null
            };

			if (seriesCollection == null)
			{
				return null;
			}

			int id = 0;

			foreach (ChartSeries series in seriesCollection)
			{
				// Add annotation semantics nodes (only for Cartesian charts)
				if (series.Chart is SfCartesianChart chart)
				{
					if (chart.Annotations != null && chart.Annotations.Count > 0)
					{
						foreach (ChartAnnotation annotation in chart.Annotations)
						{
							if (!annotation.IsVisible)
							{
								continue;
							}

							Rect annotationBounds = annotation.GetSemanticsBounds();
							if (annotationBounds.Width <= 0 || annotationBounds.Height <= 0)
							{
								continue;
							}

							var annotationNode = CreateAnnotationSemanticsNode(++id, annotation, annotationBounds);
							if (annotationNode != null)
							{
								nodes.Add(annotationNode);
							}
						}
					}
				}

				// Add trendline semantics nodes
				if (series is CartesianSeries cartesianSeries)
				{
					if (cartesianSeries.Trendlines == null)
					{
						continue;
					}

					foreach (ChartTrendline trendline in cartesianSeries.Trendlines)
					{
						if (!trendline.IsVisible)
						{
							continue;
						}

						Rect _trendlineBounds = trendline.GetSemanticsBounds();
						if (_trendlineBounds.Width <= 0 || _trendlineBounds.Height <= 0)
						{
							continue;
						}

						var _trendlineNode = CreateTrendlineSemanticsNode(++id, trendline, series, _trendlineBounds);
						if (_trendlineNode != null)
						{
							nodes.Add(_trendlineNode);
						}
					}
				}

				if (!series.IsVisible || series._segments == null || series._segments.Count == 0)
				{
					continue;
				}

				foreach (ChartSegment segment in series._segments)
				{
					Rect _bounds = segment.GetSemanticsBounds();
					if (_bounds.Width <= 0 || _bounds.Height <= 0)
					{
						continue;
					}

					var _node = CreateSemanticsNode(++id, segment, _bounds);
					if (_node != null)
					{
						nodes.Add(_node);
					}
				}
			}

			semanticsNodes = nodes;
			semanticsSize = _currentSize;
			return nodes;
		}

		/// <summary>
		/// Clears cached semantics and requests a platform refresh.
		/// </summary>
		internal void InvalidateSemanticsNodes()
		{
			semanticsNodes = null;
			semanticsSize = new Size(-1, -1);
			InvalidateSemantics();
		}

		#endregion

		#region Private Methods

		internal SemanticsNode? CreateSemanticsNode(int id, ChartSegment segment, Rect bounds)
		{
			string text = ChartAccessibilityHelper.GetSegmentSemanticText(segment);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}

			return new SemanticsNode
			{
				Id = id,
				Text = text,
				Bounds = bounds,
				IsTouchEnabled = false
			};
		}

		internal SemanticsNode? CreateAnnotationSemanticsNode(int id, ChartAnnotation annotation, Rect bounds)
		{
			string text = ChartAccessibilityHelper.GetAnnotationSemanticText(annotation);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}

			return new SemanticsNode
			{
				Id = id,
				Text = text,
				Bounds = bounds,
				IsTouchEnabled = false
			};
		}

		internal SemanticsNode? CreateTrendlineSemanticsNode(int id, ChartTrendline trendline, ChartSeries series, Rect bounds)
		{
			string text = ChartAccessibilityHelper.GetTrendlineSemanticText(trendline, series);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}

			return new SemanticsNode
			{
				Id = id,
				Text = text,
				Bounds = bounds,
				IsTouchEnabled = false
			};
		}

		#endregion
	}

	internal class PyramidExtentSemanticsView : ExtentSemanticsView
	{

        /// <summary>
        /// Gets or sets the Pyramid chart that provides segment data.
        /// </summary>
        internal IPyramidChartDependent? Chart { get; set; }


        /// <summary>
        /// Builds the semantics nodes for the visible chart segments.
        /// </summary>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
			if (width <= 0 || height <= 0 || Chart == null)
            {
                return null;
            }

            var currentSize = new Size(width, height);
            if (semanticsNodes != null && semanticsSize == currentSize)
            {
                return semanticsNodes;
            }

            var nodes = new List<SemanticsNode>();

            if (Chart == null || Chart.Segments == null)
            {
                return null;
            }

            int id = 0;

            foreach (ChartSegment segment in Chart.Segments)
            {
				Rect bounds = segment.GetSemanticsBounds();
				
                if (bounds.Width > 0 && bounds.Height > 0 && Chart is IChart chartView)
                {
                    bounds = bounds.Offset(
                        (float)(Chart.SeriesBounds.Left + chartView.ActualSeriesClipRect.Left),
                        (float)(Chart.SeriesBounds.Top ));
                }

                if (bounds.Width <= 0 || bounds.Height <= 0)
                {
                    continue;
                }

                var node = CreateSemanticsNode(++id, segment, bounds);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }

            semanticsNodes = nodes;
            semanticsSize = currentSize;
            return nodes;
        }
    }

}
