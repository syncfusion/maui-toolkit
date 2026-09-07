using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal static class TrackballAxisLabelHelper
    {
        internal static string GetAxisLabel(ChartAxis axis, double axisValue, string labelFormat)
        {
            string label = string.Empty;

            if (axis is CategoryAxis categoryAxis)
            {
                var currSeries = categoryAxis.GetActualSeries();
                if (currSeries != null)
                {
                    var value = (int)Math.Round(axisValue);

                    if (value < 0)
                    {
                        value = 0;
                    }

                    label = categoryAxis.GetLabelContent(currSeries, value, labelFormat);
                }
            }
            else if (axis is NumericalAxis numericalAxis)
            {
                label = numericalAxis.GetFormatedAxisLabel(axisValue, labelFormat);
            }
            else if (axis is LogarithmicAxis)
            {
                label = ChartAxis.GetActualLabelContent(axisValue, labelFormat).ToString();
            }
            else if (axis is DateTimeAxis datetimeAxis)
            {
                string format;
                if (labelFormat != null)
                {
                    format = labelFormat;
                }
                else
                {
                    format = ChartAxis.GetSpecificFormattedLabel(datetimeAxis.ActualIntervalType);
                }


                label = ChartAxis.GetFormattedAxisLabel(format, axisValue);
            }
            else if (axis is DateTimeCategoryAxis dateTimeCategoryAxis)
            {
                string format;

                var actualSeries = dateTimeCategoryAxis.GetActualSeries();

                if (actualSeries != null)
                {
                    if (labelFormat != null)
                    {
                        format = labelFormat;
                    }
                    else
                    {
                        format = ChartAxis.GetSpecificFormattedLabel(dateTimeCategoryAxis.ActualIntervalType);
                    }

                    var value = (int)Math.Round(axisValue);

                    if (value < 0)
                    {
                        value = 0;
                    }

                    if (actualSeries.ActualXValues != null && actualSeries.ActualXValues is List<double> xValues && value < xValues.Count)
                    {
                        double xDateTime = xValues[(int)value];
                        return ChartAxis.GetFormattedAxisLabel(format, xDateTime);
                    }
                }
            }
            else
            {
                label = ChartAxis.GetActualLabelContent(axisValue, labelFormat);
            }

            return label;
        }

        internal static void MapChartLabelStyle(SfCartesianChart cartesianChart, TooltipHelper helper, ChartLabelStyle chartLabelStyle)
        {
            var background = chartLabelStyle.Background;
            helper.FontAttributes = chartLabelStyle.FontAttributes;
            helper.FontFamily = chartLabelStyle.FontFamily;
            helper.FontSize = chartLabelStyle.FontSize;
            helper.Padding = chartLabelStyle.Margin;
            helper.Stroke = chartLabelStyle.Stroke;
            helper.StrokeWidth = (float)chartLabelStyle.StrokeWidth;
            helper.Background = chartLabelStyle.Background;
            helper.Font = ((ITextElement)chartLabelStyle).Font;

            if (!chartLabelStyle.IsTextColorUpdated)
            {
                var fontColor = background == default(Brush) || background.ToColor() == Colors.Transparent ?
                        cartesianChart.GetTextColorBasedOnChartBackground() :
                        ChartUtils.GetContrastColor((background as SolidColorBrush).ToColor());
                helper.TextColor = fontColor;
            }
            else
            {
                helper.TextColor = chartLabelStyle.TextColor;
            }
        }
    }
}
