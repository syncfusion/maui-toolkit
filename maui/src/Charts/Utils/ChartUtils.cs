using Color = Microsoft.Maui.Graphics.Color;
using PointF = Microsoft.Maui.Graphics.PointF;
using Rect = Microsoft.Maui.Graphics.Rect;
using Microsoft.Maui.Controls.Shapes;
using Core = Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal static class ChartUtils
	{
		internal static float GetMidValue(PointF centerPoint, float x, float y)
		{
			float angle;

			if (x < centerPoint.X)
			{
				var one = Math.Atan2(centerPoint.X - x, centerPoint.Y - y);
				angle = (float)(360 - (one * (180 / Math.PI)));
			}
			else
			{
				var one = Math.Atan2(x - centerPoint.X, centerPoint.Y - y);
				angle = (float)(one * (180 / Math.PI));
			}

			return angle;
		}

		internal static List<float> GetMidAngles(PointF center, List<float> midPoint)
		{
			List<float> midAngle = [];

			for (int i = 0; i < midPoint.Count; i++)
			{
				midAngle.Add(GetMidValue(center, midPoint[i], midPoint[i + 1]));
				i++;
			}

			return midAngle;
		}

		internal static double ConvertToDouble(object? val)
		{
			if (val == null)
			{
				return double.NaN;
			}

			if (double.TryParse(val.ToString(), out double doubleVal))
			{
				return doubleVal;
			}

			//Maui-884 The Following date time formats was acceptable for CrossesAt value. ("MM/dd/yyyy"),("dddd, dd MMMM yyyy"),("dddd, dd MMMM yyyy HH:mm:ss"),("MM/dd/yyyy HH:mm"),("MM/dd/yyyy hh:mm tt"),("MM/dd/yyyy H:mm"),("MM/dd/yyyy h:mm tt"),("MM/dd/yyyy HH:mm:ss"),
			//("MMMM dd"),("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"),("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’"),("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss"),("HH:mm"),("hh:mm tt"),("H:mm"),("h:mm tt"),("HH:mm:ss"),("yyyy MMMM") .
			if (DateTime.TryParse(val.ToString(), out DateTime date))
			{
				if (date == DateTime.MaxValue)
				{
					return double.MaxValue;
				}
				else if (date == DateTime.MinValue)
				{
					return double.MinValue;
				}

				return date.ToOADate();
			}

			return double.NaN;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <returns></returns>
		internal static IFontManager? GetFontManager(this IElement view)
		{
			if (view != null && view.Handler is IElementHandler handler)
			{
				return handler.GetFontManager();
			}

			if (IPlatformApplication.Current != null)
			{
				return IPlatformApplication.Current.Services.GetRequiredService<IFontManager>();
			}

			return default;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handler"></param>
		/// <returns></returns>
		internal static IFontManager? GetFontManager(this IElementHandler? handler)
		{
			if (IPlatformApplication.Current != null && IPlatformApplication.Current.Services != null)
			{
				return IPlatformApplication.Current.Services.GetRequiredService<IFontManager>();
			}
			else if (handler?.MauiContext is IMauiContext context)
			{
				return context.Services.GetRequiredService<IFontManager>();
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static ChartDataLabelStyle Clone(this ChartDataLabelStyle labelStyle)
		{
			var style = new ChartDataLabelStyle
			{
				//Only returned values which help full to render chart label. 
				//TODO: Need to add all values when it use for other cases. 
				FontFamily = labelStyle.FontFamily,
				FontAttributes = labelStyle.FontAttributes,
				FontSize = labelStyle.FontSize,
				Rect = labelStyle.Rect,
				Parent = labelStyle.Parent
			};
			return style;
		}

		internal static int BinarySearch(List<double> xValues, double touchValue, int min, int max)
		{
			var closerIndex = 0;
			var closerDelta = double.MaxValue;

			while (min <= max)
			{
				int mid = (min + max) / 2;
				var xValue = xValues[mid];
				var delta = Math.Abs(touchValue - xValue);

				if (delta < closerDelta)
				{
					closerDelta = delta;
					closerIndex = mid;
				}

				if (touchValue == xValue)
				{
					return mid;
				}
				else if (touchValue < xValues[mid])
				{
					max = mid - 1;
				}
				else
				{
					min = mid + 1;
				}
			}

			return closerIndex;
		}

		internal static string Tostring(this object? obj)
		{
			var value = obj?.ToString();

			if (value != null)
			{
				return value;
			}

			return string.Empty;
		}

		internal static Size GetRotatedSize(Size measuredSize, float degrees)
		{
			var angleRadians = (2 * Math.PI * degrees) / 360;
			var sine = Math.Sin(angleRadians);
			var cosine = Math.Cos(angleRadians);
			var matrix = new Matrix(cosine, sine, -sine, cosine, 0, 0);

			var leftTop = matrix.Transform(new Point(0, 0));
			var rightTop = matrix.Transform(new Point(measuredSize.Width, 0));
			var leftBottom = matrix.Transform(new Point(0, measuredSize.Height));
			var rightBottom = matrix.Transform(new Point(measuredSize.Width, measuredSize.Height));
			var left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
			var top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
			var right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
			var bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));

			return new Size(right - left, bottom - top);
		}

		internal static Color GetContrastColor(Color? color)
		{
			if (color == null)
			{
				return Colors.Black;
			}

			double redValue = color.Red * 255 * color.Red * 255 * .241;
			double greenValue = color.Green * 255 * color.Green * 255 * .691;
			double blueValue = color.Blue * 255 * color.Blue * 255 * .068;
			int luminance = (int)Math.Sqrt(redValue + greenValue + blueValue);

			return luminance >= 120 ? Colors.Black : Colors.White;
		}

		internal static Color GetTextColorBasedOnChartBackground(this IChart? chart)
		{
			var backgroundColor = chart?.BackgroundColor;
			return backgroundColor != null && backgroundColor != Colors.Transparent ? ChartUtils.GetContrastColor(backgroundColor) : Colors.Black;
		}

		internal static Core.ShapeType GetShapeType(ChartLegendIconType iconType)
		{
			return iconType switch
			{
				ChartLegendIconType.Circle => Core.ShapeType.Circle,
				ChartLegendIconType.Rectangle => Core.ShapeType.Rectangle,
				ChartLegendIconType.HorizontalLine => Core.ShapeType.HorizontalLine,
				ChartLegendIconType.Diamond => Core.ShapeType.Diamond,
				ChartLegendIconType.Pentagon => Core.ShapeType.Pentagon,
				ChartLegendIconType.Triangle => Core.ShapeType.Triangle,
				ChartLegendIconType.InvertedTriangle => Core.ShapeType.InvertedTriangle,
				ChartLegendIconType.Cross => Core.ShapeType.Cross,
				ChartLegendIconType.Plus => Core.ShapeType.Plus,
				ChartLegendIconType.Hexagon => Core.ShapeType.Hexagon,
				ChartLegendIconType.VerticalLine => Core.ShapeType.VerticalLine,
				ChartLegendIconType.SeriesType => Core.ShapeType.Custom,
				_ => Core.ShapeType.Circle,
			};
		}

		internal static Core.ShapeType GetShapeType(ShapeType iconType)
		{
			return iconType switch
			{
				ShapeType.Circle => Core.ShapeType.Circle,
				ShapeType.Rectangle => Core.ShapeType.Rectangle,
				ShapeType.HorizontalLine => Core.ShapeType.HorizontalLine,
				ShapeType.Diamond => Core.ShapeType.Diamond,
				ShapeType.Pentagon => Core.ShapeType.Pentagon,
				ShapeType.Triangle => Core.ShapeType.Triangle,
				ShapeType.InvertedTriangle => Core.ShapeType.InvertedTriangle,
				ShapeType.Cross => Core.ShapeType.Cross,
				ShapeType.Plus => Core.ShapeType.Plus,
				ShapeType.Hexagon => Core.ShapeType.Hexagon,
				ShapeType.VerticalLine => Core.ShapeType.VerticalLine,
				ShapeType.Custom => Core.ShapeType.Custom,
				_ => Core.ShapeType.Circle,
			};
		}

		//Calculating series clip rect with chart title.
		internal static Rect GetSeriesClipRect(Rect seriesClipRect, double titleHeight)
		{
			return new Rect(
				seriesClipRect.X,
				seriesClipRect.Y + titleHeight,
				seriesClipRect.Width,
				seriesClipRect.Height);
		}

		internal static bool SegmentContains(LineSegment segment, PointF touchPoint, ChartSeries series)
		{
			var pointX = touchPoint.X;
			var pointY = touchPoint.Y;
			var defaultSelectionStrokeWidth = series._defaultSelectionStrokeWidth;
			var leftPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var rightPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var topPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var botPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var startSegment = new PointF(segment.X1, segment.Y1);
			var endSegment = new PointF(segment.X2, segment.Y2);

			if (LineContains(startSegment, endSegment, leftPoint, rightPoint) ||
				LineContains(startSegment, endSegment, topPoint, botPoint))
			{
				return true;
			}

			return false;
		}

		internal static bool SegmentContains(StepLineSegment segment, PointF touchPoint, ChartSeries series)
		{
			var pointX = touchPoint.X;
			var pointY = touchPoint.Y;
			var defaultSelectionStrokeWidth = series._defaultSelectionStrokeWidth;
			var leftPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var rightPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var topPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var botPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var startSegment = new PointF(segment.X1, segment.Y1);
			var endSegment = new PointF(segment.X2, segment.Y2);
			var midSegmentPoint = new PointF(segment.X2, segment.Y1);

			if (LineContains(startSegment, midSegmentPoint, leftPoint, rightPoint) ||
				LineContains(startSegment, midSegmentPoint, topPoint, botPoint) ||
				LineContains(midSegmentPoint, endSegment, leftPoint, rightPoint) ||
				LineContains(midSegmentPoint, endSegment, topPoint, botPoint))
			{
				return true;
			}

			return false;
		}

		internal static bool SegmentContains(SplineSegment segment, PointF touchPoint, ChartSeries series)
		{
			var pointX = touchPoint.X;
			var pointY = touchPoint.Y;
			var defaultSelectionStrokeWidth = series._defaultSelectionStrokeWidth;
			var leftPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var rightPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var topPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
			var botPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
			var startPoint = new PointF(segment.X1, segment.Y1);
			var endPoint = new PointF(segment.X2, segment.Y2);
			var startControlPoint = new PointF(segment.StartControlX, segment.StartControlY);
			var endControlPoint = new PointF(segment.EndControlX, segment.EndControlY);

			if (LineContains(startPoint, startControlPoint, leftPoint, rightPoint) ||
				LineContains(startPoint, startControlPoint, topPoint, botPoint) ||
				LineContains(startControlPoint, endControlPoint, leftPoint, rightPoint) ||
				LineContains(startControlPoint, endControlPoint, topPoint, botPoint) ||
				LineContains(endControlPoint, endPoint, leftPoint, rightPoint) ||
				LineContains(endControlPoint, endPoint, topPoint, botPoint))
			{
				return true;
			}

			return false;
		}

		internal static bool LineContains(PointF segmentStartPoint, PointF segmentEndPoint, PointF touchStartPoint, PointF touchEndPoint)
		{
			int topPos = GetPointDirection(segmentStartPoint, segmentEndPoint, touchStartPoint);
			int botPos = GetPointDirection(segmentStartPoint, segmentEndPoint, touchEndPoint);
			int leftPos = GetPointDirection(touchStartPoint, touchEndPoint, segmentStartPoint);
			int rightPos = GetPointDirection(touchStartPoint, touchEndPoint, segmentEndPoint);

			return topPos != botPos && leftPos != rightPos;
		}

		internal static bool IsPathContains(List<PointF> segPoints, float xPos, float yPos)
		{
			int i, j;

			for (i = 0, j = segPoints.Count - 1; i < segPoints.Count; j = i++)
			{
				if ((((segPoints[i].Y <= yPos) && (yPos < segPoints[j].Y)) ||
					 ((segPoints[j].Y <= yPos) && (yPos < segPoints[i].Y))) &&
					(xPos < (segPoints[j].X - segPoints[i].X) * (yPos - segPoints[i].Y) / (segPoints[j].Y - segPoints[i].Y) + segPoints[i].X))
				{
					return true;
				}
			}

			return false;
		}

		internal static bool IsAreaContains(List<PointF> segPoints, float xPos, float yPos)
		{
			int i, j;
			bool inside = false;

			for (i = 0, j = segPoints.Count - 1; i < segPoints.Count; j = i++)
			{
				if (((segPoints[i].Y > yPos) != (segPoints[j].Y > yPos)) &&
					(xPos < (segPoints[j].X - segPoints[i].X) * (yPos - segPoints[i].Y) / (segPoints[j].Y - segPoints[i].Y) + segPoints[i].X))
				{
					inside = !inside;
				}
			}

			return inside;
		}

		[RequiresUnreferencedCode("The IsOverriddenMethod is not trim compatible")]
		internal static bool IsOverriddenMethod(object classObject, string methodName)
		{
			var methodInfo = classObject.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

			if (methodInfo == null)
			{
				return false;
			}

			return methodInfo.GetBaseDefinition() != methodInfo;
		}

		static int GetPointDirection(PointF point1, PointF point2, PointF point3)
		{
			int value = (int)(((point2.Y - point1.Y) * (point3.X - point2.X)) - ((point2.X - point1.X) * (point3.Y - point2.Y)));

			if (value == 0)
			{
				return 0;
			}

			return (value > 0) ? 1 : 2;
		}

		internal static Color ToColor(this Brush? brush)
		{
			if (brush is SolidColorBrush solidBrush)
			{
				return solidBrush.Color;
			}

			return Colors.Transparent;
		}

		internal static bool IsOverlap(this Rect currentRect, Rect rect)
		{
			return currentRect.Left < rect.Left + rect.Width &&
				currentRect.Left + currentRect.Width > rect.Left &&
				currentRect.Top < (rect.Top + rect.Height) &&
				(currentRect.Height + currentRect.Top) > rect.Top;
		}

		internal static float CalculateAngleDeviation(float calcRadius, float radius, float angleDifferent)
		{
			var circumference = 2 * (float)Math.PI * calcRadius;
			var deviation = (radius / circumference) * 100;

			return (deviation * angleDifferent) / 100;
		}

		/// <summary>
		/// To convert angle to vector.
		/// </summary>
		/// <param name="angle">The angle</param>
		/// <returns>Vector of given angle</returns>
		internal static Point AngleToVector(double angle)
		{
			double angleRadian = ChartMath.DegreeToRadian((float)angle);
			return new Point(Math.Cos(angleRadian), Math.Sin(angleRadian));
		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal static DataTemplate GetDefaultTooltipTemplate(TooltipInfo info)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			var template = new DataTemplate(() =>
			{
				Label label = new Label
				{
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,					
				};

				label.SetBinding(Label.TextProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.Text), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.Text));
				label.SetBinding(Label.TextColorProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.TextColor), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.TextColor));
				label.SetBinding(Label.MarginProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.Margin), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.Margin));
				label.SetBinding(Label.FontSizeProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.FontSize), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontSize));
				label.SetBinding(Label.FontFamilyProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.FontFamily), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontFamily));
				label.SetBinding(Label.FontAttributesProperty,
					BindingHelper.CreateBinding(nameof(TooltipInfo.FontAttributes), getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontAttributes));

#if NET10_0_OR_GREATER
                return label;
#else
				return new ViewCell { View = label };
#endif
			});

			return template;
		}

		internal static bool CompareStackingSeries(this StackingSeriesBase series, StackingSeriesBase otherSeries)
		{
			return series.GetType() == otherSeries.GetType();
		}

		internal static Size GetLabelSize(string label, ITextElement textElement)
		{
			var size = new Size();

			if (label.Contains('\n', StringComparison.Ordinal))
			{
				var text = label.Split('\n');
				var spacing = text.Length * 3;

				foreach (var content in text)
				{
					var measure = content.Measure(textElement);
					size.Width = Math.Max(size.Width, measure.Width);
					size.Height += measure.Height;
				}
				size.Height += spacing;
			}
			else
			{
				size = label.Measure(textElement);
			}

			return size;
		}
	}

	internal class TooltipLabelValue
	{
		public string LabelText { get; set; }
		public string ValueText { get; set; }

		public TooltipLabelValue(string labelText, string valueText)
		{
			LabelText = labelText;
			ValueText = valueText;
		}
	}
}
