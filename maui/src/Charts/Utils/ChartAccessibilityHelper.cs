using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Shared helpers for building screen-reader semantic text for chart visuals.
	/// </summary>
	internal static class ChartAccessibilityHelper
	{

		/// <summary>
		/// Builds a screen-reader announcement for a single pie/doughnut segment.
		/// </summary>
		/// <param name="segment">The pie or doughnut segment.</param>
		/// <returns>Localized accessibility text describing the segment value.</returns>
		internal static string GetSegmentSemanticText(ChartSegment segment)
		{
			if (segment == null)
			{
				return string.Empty;
			}

			if (segment is FunnelSegment funnelSegment)
			{
				return funnelSegment.GetAccessibilityText();
			}

			if (segment is PyramidSegment pyramidSegment)
			{
				return pyramidSegment.GetAccessibilityText();
			}

			var label = string.Empty;
			var idx = segment.Index;
			var valueText = segment.Series?.XValues != null && idx >= 0
			? segment.Series.XValues.Cast<object>().ElementAtOrDefault(idx)?.ToString() ?? string.Empty
			: string.Empty;

			var legendText = GetLegendText(segment);

			if (segment is PieSegment pieSegment)
			{
				label = pieSegment.YValue.ToString() ?? string.Empty;
			}
			else if (segment is HistogramSegment histogramSegment)
			{
				return "Range " + histogramSegment.IntervalStart + " to " + histogramSegment.IntervalEnd
					+ " Count " + histogramSegment.PointsCount;
			}
			else if (segment.Series is PolarSeries polarSeries)
			{
				label = polarSeries?.YValues != null && idx >= 0
			   ? polarSeries.YValues.Cast<object>().ElementAtOrDefault(idx)?.ToString() ?? string.Empty
			   : string.Empty;
			}
			else if (segment is BoxAndWhiskerSegment boxAndWhiskerSegment)
			{
				label = SfCartesianChartResources.Maximum + " Value " + boxAndWhiskerSegment.Maximum + " "
					+ SfCartesianChartResources.Q3 + " Value " + boxAndWhiskerSegment.UpperQuartile + " "
					+ SfCartesianChartResources.Median + " Value " + boxAndWhiskerSegment.Median + " "
					+ SfCartesianChartResources.Q1 + " Value " + boxAndWhiskerSegment.LowerQuartile + " "
					+ SfCartesianChartResources.Minimum + " Value " + boxAndWhiskerSegment.Minimum;
			}
			else if (segment.Series is XYDataSeries cartesianSeries)
			{
				var yValues = cartesianSeries.YValues;
				label = yValues != null && idx >= 0
					? yValues.Cast<object>().ElementAtOrDefault(idx)?.ToString()
					: null;
			}
			else if (segment.Series is RangeColumnSeries rangeColumnSeries)
			{
				var yvalues = rangeColumnSeries.SeriesYValues;
				if (yvalues != null && yvalues.Count() >= 0)
				{
					IList<double> highValues = yvalues[0];
					IList<double> lowValues = yvalues[1];

					var segmentHighValue = highValues != null && idx >= 0
					? highValues.Cast<object>().ElementAtOrDefault(idx)?.ToString()
					: null;

					var segmentLowValue = lowValues != null && idx >= 0
					? lowValues.Cast<object>().ElementAtOrDefault(idx)?.ToString()
					: null;

					label = SfCartesianChartResources.High + "Value " + segmentHighValue + " " + 
						SfCartesianChartResources.Low + "Value " + segmentLowValue;
				}
			}
			else if (segment.Series is FinancialSeriesBase financialSeries)
			{
				var highValue = GetSegmentYValue(financialSeries.HighValues, idx);
				var lowValue = GetSegmentYValue(financialSeries.LowValues, idx);
				var openValue = GetSegmentYValue(financialSeries.OpenValues, idx);
				var closeValue = GetSegmentYValue(financialSeries.CloseValues, idx);

				label = SfCartesianChartResources.High + "Value :" + highValue + " "
					+ SfCartesianChartResources.Low + "Value :" + lowValue + " "
					+ SfCartesianChartResources.Open + "Value :" + openValue + " "
					+ SfCartesianChartResources.Close + "Value :" + closeValue;
			}

			var result = string.Empty;

			if (string.IsNullOrEmpty(label) || label == valueText)
			{
				result = valueText;
			}
			else if (string.IsNullOrEmpty(valueText))
			{
				result = label;
			}
			else
			{
				result = valueText + ":" + label;
			}

			// Append legend text if available and legend is visible
			if (!string.IsNullOrEmpty(legendText))
			{
				result = string.IsNullOrEmpty(result) ? legendText : result + ", " + legendText;
			}

			return result;
		}

		private static string GetSegmentYValue(IList<double> values, int idx)
		{
			var segmentValue = values != null && idx >= 0
					? values.Cast<object>().ElementAtOrDefault(idx)?.ToString()
					: null;

			return segmentValue ?? string.Empty;
		}

		/// <summary>
		/// Gets the legend text for a segment if the legend is visible.
		/// </summary>
		/// <param name="segment">The chart segment.</param>
		/// <returns>Legend label text if legend is visible, otherwise empty string.</returns>
		private static string GetLegendText(ChartSegment segment)
		{
			if (segment?.Series == null)
			{
				return string.Empty;
			}

			// Check if legend exists and is visible
			var series = segment.Series;
			var chart = (series as ChartSeries)?.Chart;
			var legend = chart?.Legend;

			if (legend == null || !legend.IsVisible)
			{
				return string.Empty;
			}

			// Get the legend label from the series
			string legendLabel = string.Empty;

			if (series is CartesianSeries cartesianSeries)
			{
				legendLabel = cartesianSeries.Label;
			}
			else if (series is PolarSeries polarSeries)
			{
				legendLabel = polarSeries.Label;
			}

			return legendLabel ?? string.Empty;
		}

		/// <summary>
		/// Builds a screen-reader announcement for a chart annotation.
		/// </summary>
		/// <param name="annotation">The chart annotation.</param>
		/// <returns>Localized accessibility text describing the annotation.</returns>
		internal static string GetAnnotationSemanticText(ChartAnnotation annotation)
		{
			if (annotation == null)
			{
				return string.Empty;
			}

			var semanticText = string.Empty;

			// Handle different annotation types
			if (annotation is TextAnnotation textAnnotation)
			{
				semanticText = GetTextAnnotationSemanticText(textAnnotation);
			}
			else if (annotation is RectangleAnnotation rectangleAnnotation)
			{
				semanticText = GetRectangleAnnotationSemanticText(rectangleAnnotation);
			}
			else if (annotation is LineAnnotation lineAnnotation)
			{
				semanticText = GetLineAnnotationSemanticText(lineAnnotation);
			}
			else if (annotation is EllipseAnnotation ellipseAnnotation)
			{
				semanticText = GetEllipseAnnotationSemanticText(ellipseAnnotation);
			}
			else if (annotation is HorizontalLineAnnotation horizontalLineAnnotation)
			{
				semanticText = GetHorizontalLineAnnotationSemanticText(horizontalLineAnnotation);
			}
			else if (annotation is VerticalLineAnnotation verticalLineAnnotation)
			{
				semanticText = GetVerticalLineAnnotationSemanticText(verticalLineAnnotation);
			}
			else if (annotation is ViewAnnotation viewAnnotation)
			{
				semanticText = GetViewAnnotationSemanticText(viewAnnotation);
			}
			else
			{
				semanticText = "Annotation";
			}

			return semanticText;
		}

		/// <summary>
		/// Gets semantic text for a TextAnnotation.
		/// </summary>
		private static string GetTextAnnotationSemanticText(TextAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Text Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for a RectangleAnnotation.
		/// </summary>
		private static string GetRectangleAnnotationSemanticText(RectangleAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Rectangle Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for a LineAnnotation.
		/// </summary>
		private static string GetLineAnnotationSemanticText(LineAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Line Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for an EllipseAnnotation.
		/// </summary>
		private static string GetEllipseAnnotationSemanticText(EllipseAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Ellipse Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for a HorizontalLineAnnotation.
		/// </summary>
		private static string GetHorizontalLineAnnotationSemanticText(HorizontalLineAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Horizontal Line Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for a VerticalLineAnnotation.
		/// </summary>
		private static string GetVerticalLineAnnotationSemanticText(VerticalLineAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("Vertical Line Annotation");

			if (!string.IsNullOrEmpty(annotation.Text))
			{
				text.Append(". ");
				text.Append(annotation.Text);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets semantic text for a ViewAnnotation.
		/// </summary>
		private static string GetViewAnnotationSemanticText(ViewAnnotation annotation)
		{
			var text = new System.Text.StringBuilder();
			text.Append("View Annotation");

			// Add view type if available
			if (annotation.View != null)
			{
				text.Append(". ");
				text.Append(annotation.View.GetType().Name);
			}

			return text.ToString();
		}

		/// <summary>
		/// Builds a screen-reader announcement for a chart trendline.
		/// </summary>
		/// <param name="trendline">The chart trendline.</param>
		/// <param name="series">The parent series associated with the trendline.</param>
		/// <returns>Localized accessibility text describing the trendline type.</returns>
		internal static string GetTrendlineSemanticText(ChartTrendline trendline, ChartSeries series)
		{
			if (trendline == null)
			{
				return string.Empty;
			}

			var trendlineType = GetTrendlineTypeName(trendline);
			return $"{trendlineType} Trendline";
		}

		/// <summary>
		/// Gets the type name of a trendline for accessibility text.
		/// </summary>
		private static string GetTrendlineTypeName(ChartTrendline trendline)
		{
			return trendline switch
			{
				LinearTrendline => "Linear",
				ExponentialTrendline => "Exponential",
				LogarithmicTrendline => "Logarithmic",
				PowerTrendline => "Power",
				PolynomialTrendline => "Polynomial",
				MovingAverageTrendline => "Moving Average",
				_ => "Unknown"
			};
		}
	}
}
