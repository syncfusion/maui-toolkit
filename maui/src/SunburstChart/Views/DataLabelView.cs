using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections.Generic;
using System;
using Point = Microsoft.Maui.Graphics.Point;
using Color = Microsoft.Maui.Graphics.Color;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents a view for displaying data labels in a Sunburst chart.
    /// </summary>
    internal class DataLabelView : SfDrawableView
    {
        #region Private Fields

        SfSunburstChart _sunburstChart;
        float _dataLabelOpacity;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLabelView"/> class.
        /// </summary>
        public DataLabelView(SfSunburstChart chart)
        {
            _sunburstChart = chart;
        }

        #endregion

        #region Protected method

        /// <summary>
        /// Draws the data labels for the Sunburst chart segments.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (_sunburstChart.Levels.Count > 0 && _sunburstChart.ShowLabels && !_sunburstChart.NeedToAnimate)
            {
                canvas.SaveState();

                if (_sunburstChart.NeedToAnimateDataLabel)
                {
                    canvas.Alpha = _sunburstChart.AnimationValue;
                }

                var settings = _sunburstChart.DataLabelSettings;

                foreach (var segment in _sunburstChart.Segments)
                {
                    var text = string.Empty;

                    if (segment.Item is List<object> item)
                    {
                        text = item[0].ToString() ?? string.Empty;
                    }

                    _dataLabelOpacity = _sunburstChart.NeedToAnimateDataLabel ? _sunburstChart.AnimationValue : segment.Opacity;
                    double availableWidth;
                    bool isTrimmed;

                    if (settings.RotationMode == SunburstLabelRotationMode.Angle)
                    {
                        availableWidth = segment.OuterRadius - segment.InnerRadius - (segment.HasStroke ? segment.StrokeWidth * 2 : 2);
                        availableWidth *= 0.8;
                    }
                    else
                    {
                        availableWidth = segment.GetSegmentHorizantalWidth(text, settings);
                    }

                    string trimmedText;
                    isTrimmed = RequiredTextTrim(text, out trimmedText, availableWidth, settings);

                    if (settings.OverFlowMode == SunburstLabelOverflowMode.Hide && isTrimmed)
                    {
                        continue;
                    }

                    text = isTrimmed ? trimmedText + "..." : text;

                    var r = (segment.OuterRadius - segment.InnerRadius) / 2 + segment.InnerRadius;

                    double radian = (segment.ArcStartAngle + segment.ArcEndAngle) / 2;

                    if (settings.RotationMode == SunburstLabelRotationMode.Angle)
                    {
                        DrawLabelAtAngle(canvas, settings, text, segment.Fill, r, radian);
                    }
                    else
                    {
                        DrawLabelByDefault(canvas, settings, text, segment.Fill, r, radian);
                    }
                }

                canvas.RestoreState();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Draws the label with the default rotation mode.
        /// </summary>
        void DrawLabelByDefault(ICanvas canvas, SunburstDataLabelSettings settings, string text, Brush? fill, double r, double radian)
        {
            Point center = _sunburstChart.Center;
            var angle = SunburstChartUtils.RadianToDegreeConverter(radian);
            var textSize = text.Measure(settings);

            Color fontColor = settings.TextColor;
            if (fontColor == null || fontColor == Colors.Transparent)
            {
                fontColor = SunburstChartUtils.GetContrastColor((fill as SolidColorBrush)?.Color);
                var textColor = fontColor.WithAlpha(_sunburstChart.NeedToAnimateDataLabel ? _sunburstChart.AnimationValue : _dataLabelOpacity);
                //Created new font family, as need to pass contrast text color for native font family rendering.
                settings = settings.Clone();
                settings.TextColor = textColor;
            }

            if (text != null)
            {
                var x = (float)(center.X + r * Math.Cos(radian));
                var y = (float)(center.Y + r * Math.Sin(radian));

                canvas.SaveState();
#if ANDROID
                canvas.DrawText(text, (float)(x - textSize.Width / 2), (float)(y + textSize.Height / 2), settings);
#else
                canvas.DrawText(text, (float)(x - textSize.Width / 2), (float)(y - textSize.Height / 1.6), settings);
#endif
                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Draws the label at an angle according to rotation mode.
        /// </summary>
        void DrawLabelAtAngle(ICanvas canvas, ITextElement settings, string text, Brush? fill, double r, double radian)
        {
            Point center = _sunburstChart.Center;
            var angle = SunburstChartUtils.RadianToDegreeConverter(radian);
            var textSize = text.Measure(settings);

            Color fontColor = settings.TextColor;
            if (fontColor == null || fontColor == Colors.Transparent)
            {
                fontColor = SunburstChartUtils.GetContrastColor((fill as SolidColorBrush)?.Color);
                var textColor = fontColor.WithAlpha(_sunburstChart.NeedToAnimateDataLabel ? _sunburstChart.AnimationValue : _dataLabelOpacity);
                //Created new font family, as need to pass contrast text color for native font family rendering.
                settings = settings.Clone();
                settings.TextColor = textColor;
            }

            var x = center.X + (float)(r * Math.Cos(radian));
            var y = center.Y + (float)(r * Math.Sin(radian));
            canvas.SaveState();

            if ((angle > 90 && angle <= 180) || (angle > 180 && angle <= 270))
            {
                canvas.Rotate((float)(angle - 180), (float)x, (float)y);
            }
            else
            {
                canvas.Rotate((float)(angle), (float)x, (float)y);
            }
#if ANDROID
                canvas.DrawText(text, (float)(x - textSize.Width / 2), (float)(y + textSize.Height / 3), settings);
#else
                canvas.DrawText(text, (float)(x - textSize.Width / 2), (float)(y - textSize.Height / 1.6), settings);
#endif
                canvas.RestoreState();
        }

        /// <summary>
        /// Trims the text to fit within the available width, if necessary.
        /// </summary>
        static bool RequiredTextTrim(string input, out string trimText, double availableWidth, SunburstDataLabelSettings settings)
        {
            double value = 0;
            bool isTrimmed = false;
            var textSize = input.Measure(settings);

            do
            {
                if (textSize.Width > availableWidth && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    value = input.Measure(settings).Width;
                    isTrimmed = true;

                    if (settings.OverFlowMode == SunburstLabelOverflowMode.Hide && isTrimmed)
                    {
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }
            while (value > availableWidth && input.Length > 0);
            
            trimText = input;
            return isTrimmed;
        }

        #endregion
    }
}
