using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// 
	/// </summary>
	internal static class PyramidChartBase
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="GapRatioProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="GapRatioProperty"/> bindable property determines the gap ratio 
		/// between the segments of the chart.
		/// </remarks>
		public static readonly BindableProperty GapRatioProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.GapRatio),
			typeof(double),
			typeof(IPyramidChartDependent),
			0d,
			BindingMode.Default,
			null,
			propertyChanged: OnGapRatioChanged);

		/// <summary>
		/// Identifies the <see cref="PaletteBrushesProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="PaletteBrushesProperty"/> bindable property determines the 
		/// color palette used for the segments of the chart.
		/// </remarks>
		public static readonly BindableProperty PaletteBrushesProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.PaletteBrushes),
			typeof(IList<Brush>),
			typeof(IPyramidChartDependent),
			null,
			BindingMode.Default,
			null,
			propertyChanged: OnPaletteBrushesChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeProperty"/> bindable property determines the 
		/// stroke color of the segments in the chart.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.Stroke),
			typeof(Brush),
			typeof(IPyramidChartDependent),
			SolidColorBrush.Transparent,
			BindingMode.Default,
			null,
			propertyChanged: OnStrokeChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeWidthProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidthProperty"/> bindable property determines the 
		/// width of the stroke for the segments in the chart.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.StrokeWidth),
			typeof(double),
			typeof(IPyramidChartDependent),
			2d,
			BindingMode.Default,
			null,
			propertyChanged: OnStrokeWidthChanged);

		/// <summary>
		/// Identifies the <see cref="LegendIconProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LegendIconProperty"/> bindable property determines the 
		/// icon type used in the legend for the chart.
		/// </remarks>
		public static readonly BindableProperty LegendIconProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.LegendIcon),
			typeof(ChartLegendIconType),
			typeof(IPyramidChartDependent),
			ChartLegendIconType.Circle,
			BindingMode.Default,
			null,
			propertyChanged: OnLegendIconChanged);

		/// <summary>
		/// Identifies the <see cref="TooltipTemplateProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipTemplateProperty"/> bindable property determines the 
		/// template used for displaying tooltips in the chart.
		/// </remarks>
		public static readonly BindableProperty TooltipTemplateProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.TooltipTemplate),
			typeof(DataTemplate),
			typeof(IPyramidChartDependent),
			null);

		/// <summary>
		/// Identifies the <see cref="EnableTooltipProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnableTooltipProperty"/> bindable property determines 
		/// whether tooltips are enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnableTooltipProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.EnableTooltip),
			typeof(bool),
			typeof(IPyramidChartDependent),
			false);

		/// <summary>
		/// Identifies the <see cref="SelectionBehaviorProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionBehaviorProperty"/> bindable property determines the 
		/// selection behavior for the chart.
		/// </remarks>
		public static readonly BindableProperty SelectionBehaviorProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.SelectionBehavior),
			typeof(DataPointSelectionBehavior),
			typeof(IPyramidChartDependent),
			null,
			BindingMode.Default,
			null,
			OnSelectionBehaviorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowDataLabelsProperty"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ShowDataLabelsProperty"/> bindable property determines whether
		/// data labels are shown on the chart.
		/// </remarks>
		public static readonly BindableProperty ShowDataLabelsProperty = BindableProperty.Create(
			nameof(IPyramidChartDependent.ShowDataLabels), 
			typeof(bool), 
			typeof(IPyramidChartDependent), 
			false, 
			BindingMode.Default, 
			null, 
			OnShowDataLabelsChanged);

		#endregion

		#region Call Back methods

		static void OnLegendIconChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnLegendIconChanged((object)oldValue, (object)newValue);
		}

		static void OnGapRatioChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnGapRatioChanged((object)oldValue, (object)newValue);
		}

		static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnPaletteBrushChanged((object)oldValue, (object)newValue);
		}

		static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnStrokeChanged((object)oldValue, (object)newValue);
		}
		static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnStrOnStrokeWidthChanged((object)oldValue, (object)newValue);
		}
		static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnSelectionBehaviorPropertyChanged((object)oldValue, (object)newValue);
		}

		static void OnShowDataLabelsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((IPyramidChartDependent)bindable).OnShowDataLabelsChanged((object)oldValue, (object)newValue);
		}

		#endregion

		#region Internal methods

		internal static void InvokeSegmentsCollectionChanged(IPyramidChartDependent source, ObservableCollection<ChartSegment> Segments)
		{
			Segments.CollectionChanged += source.Segments_CollectionChanged;
		}

		internal static void UnhookSegmentsCollectionChanged(IPyramidChartDependent source, ObservableCollection<ChartSegment> Segments)
		{
			Segments.CollectionChanged -= source.Segments_CollectionChanged;
		}

		internal static Brush? GetFillColor(IPyramidChartDependent source, int index)
		{
			return source.GetFillColor(index);
		}

		internal static void UpdateColor(this IPyramidChartDependent funnel)
		{
			funnel.UpdateColor();
		}

		internal static int GetDataPointIndex(this IPyramidChartDependent source, float x, float y)
		{
			return source.GetDataPointIndex(x, y);
		}

		/// <summary>
		/// Converts a color value to a Brush. Supports Color, Brush, or Color hex string.
		/// </summary>
		internal static Brush? GetBrushFromColor(object? colorValue)
		{
			if (colorValue == null)
			{
				return null;
			}

			// If already a Brush, return it
			if (colorValue is Brush brush)
			{
				return brush;
			}

			// If it's a Color, convert to SolidColorBrush
			if (colorValue is Color color)
			{
				return new SolidColorBrush(color);
			}

			// If it's a string, try to parse as hex color
			if (colorValue is string colorString)
			{
				if (Color.TryParse(colorString, out Color parsedColor))
				{
					return new SolidColorBrush(parsedColor);
				}
			}

			return null;
		}

		#endregion
	}

	/// <summary>
	/// Layout and draw the data labels.
	/// </summary>
	internal class PyramidDataLabelHelper
	{
		const int Spacing = 3;
		const float BendRatio = 0.05f;
		static float DesiredWidth = float.MinValue;
		readonly IPyramidChartDependent _chart;
		internal readonly ObservableCollection<IPyramidLabels> _segments;
		readonly Dictionary<IPyramidLabels, RectF> _labelRects;

		IPyramidDataLabelSettings _dataLabelSettings { get => _chart.DataLabelSettings; }

		public PyramidDataLabelHelper(IPyramidChartDependent chart)
		{
			_chart = chart;
			_segments = [];
			_labelRects = [];
		}

		#region Internal Methods

		/// <summary>
		/// Arranges and positions all data labels within the chart bounds, handling overlaps and visibility.
		/// </summary>
		internal void ArrangeElements()
		{
			// DataLabel clip bounds.
			var clip = new Rect(new Point(0, 0), _chart.AreaBounds.Size);
			var seriesBounds = _chart.SeriesBounds;
			DesiredWidth = (float)(clip.Width - (seriesBounds.X + seriesBounds.Width));
			_labelRects.Clear();

			foreach (var item in _segments)
			{
				if (_dataLabelSettings != null)
				{
					_ = _dataLabelSettings.LabelStyle;

					_ = item.DataLabelSize;
					var actualSize = item.ActualLabelSize;
					var placement = _dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto ? item.Position : _dataLabelSettings.LabelPlacement;
					var labelRect = CalculateLabelRect(item, placement, actualSize);

					if (item.IsLabelVisible)
					{
						var visible = clip.Contains(labelRect);

						if (visible)
						{
							if (_labelRects.ContainsKey(item))
							{
								_labelRects[item] = labelRect;
							}
							else
							{
								_labelRects.Add(item, labelRect);
							}
						}

						item.IsLabelVisible = visible;
					}

					item.LabelRect = labelRect;
				}
			}
		}

		/// <summary>
		/// Calculates the label rectangle position based on placement and size.
		/// </summary>
		Rect CalculateLabelRect(IPyramidLabels item, DataLabelPlacement placement, Size size)
		{
			var settings = _dataLabelSettings;
			if (settings == null || settings.LabelStyle == null)
			{
				return Rect.Zero;
			}

			var style = settings.LabelStyle;
			var labelRect = Rect.Zero;
			var actualPosition = DataLabelPlacement.Inner;
			item.LinePoints = null;

			if (_chart.IsHorizontalOrientation)
			{
				var bounds = _chart.SeriesBounds;
				var originalDataLabelX = item.DataLabelX;
				var originalDataLabelY = item.DataLabelY;

				double offsetX = double.IsNaN(style.OffsetX) ? 0d : style.OffsetX;
				double offsetY = double.IsNaN(style.OffsetY) ? 0d : style.OffsetY;

				var finalDataLabelX = originalDataLabelX + (float)offsetX;
				var finalDataLabelY = originalDataLabelY + (float)offsetY;

				switch (placement)
				{
					case DataLabelPlacement.Inner:
					case DataLabelPlacement.Center:
					case DataLabelPlacement.Auto:
						{
							var x = finalDataLabelX - size.Width / 2;
							var y = finalDataLabelY - size.Height / 2;

							labelRect = new Rect(new Point(x, y), size);
							actualPosition = DataLabelPlacement.Inner;
							break;
						}

					case DataLabelPlacement.Outer:
						{
							var outerY = Math.Min(bounds.Y, 10);
							var x = finalDataLabelX - size.Width / 2;

							var linePoints = new Point[2];
							linePoints[0] = new Point(x + (size.Width / 2), outerY + size.Height);
							linePoints[1] = item.SlopePoint;

							item.LinePoints = linePoints;
							labelRect = new Rect(new Point(x, outerY), size);
							actualPosition = DataLabelPlacement.Outer;
							break;
						}
				}
			}
			else
			{
				var bounds = _chart.SeriesBounds;

				double offsetX = double.IsNaN(style.OffsetX) ? 0d : style.OffsetX;
				double offsetY = double.IsNaN(style.OffsetY) ? 0d : style.OffsetY;
				item.DataLabelX += (float)offsetX;
				item.DataLabelY += (float)offsetY;

				switch (placement)
				{
					case DataLabelPlacement.Inner:
					case DataLabelPlacement.Center:
					case DataLabelPlacement.Auto:
						{
							var x = item.DataLabelX - size.Width / 2;
							var y = item.DataLabelY - size.Height / 2;

							labelRect = new Rect(new Point(x, y), size);
							actualPosition = DataLabelPlacement.Inner;
							break;
						}

					case DataLabelPlacement.Outer:
						{
							var outerX = bounds.X + bounds.Width + offsetX;
							var y = item.DataLabelY;

							var linePoints = new Point[3];
							linePoints[0] = new Point(outerX, y);
							var bend = (outerX - bounds.Center.X) * BendRatio;
							linePoints[1] = new Point(outerX - bend, y);
							linePoints[2] = item.SlopePoint;

							item.LinePoints = linePoints;
							var width = size.Width < DesiredWidth ? size.Width : DesiredWidth;
							var yPosition = item.DataLabelY - size.Height / 2;
							labelRect = new Rect(new Point(outerX, yPosition), new Size(width, size.Height));
							actualPosition = DataLabelPlacement.Outer;
							break;
						}
				}
			}

			return ArrangeSmartLabel(item, actualPosition, labelRect);
		}

		/// <summary>
		/// Arranges smart labels to avoid overlaps, handling visibility and repositioning.
		/// </summary>
		Rect ArrangeSmartLabel(IPyramidLabels item, DataLabelPlacement actualPosition, Rect labelRect)
		{
			if (_chart.IsHorizontalOrientation)
			{
				foreach (var rect in _labelRects)
				{
					var isIntersected = labelRect.IsOverlap(rect.Value);

					if (isIntersected)
					{
						item.IsLabelVisible = false;
						return labelRect;
					}

					item.IsLabelVisible = true;
				}

				return labelRect;
			}

			foreach (var rect in _labelRects)
			{
				var isIntersected = labelRect.IsOverlap(rect.Value);

				if (isIntersected)
				{
					if (actualPosition == DataLabelPlacement.Inner)
					{
						item.IsLabelVisible = false;
						return labelRect;
					}
					else
					{
						var adjacentRect = rect.Value;
						item.IsLabelVisible = true;

						item.DataLabelY = !_chart.ArrangeReverse ?
							(float)(adjacentRect.Y + adjacentRect.Height + Spacing + labelRect.Height / 2) :
							(float)(adjacentRect.Y - Spacing - labelRect.Height / 2);

						labelRect = CalculateLabelRect(item, DataLabelPlacement.Outer, labelRect.Size);
					}
				}
				else
				{
					item.IsLabelVisible = true;
				}
			}

			return labelRect;
		}

#pragma warning disable IDE0060 // Remove unused parameter
		/// <summary>
		/// Draws data labels and connector lines on the canvas.
		/// </summary>
		internal void OnDraw(ICanvas canvas, Rect dirtyRect)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			foreach (var item in _segments)
			{
				if (item.IsLabelVisible)
				{
					canvas.CanvasSaveState();
					canvas.StrokeSize = 1;
					canvas.StrokeColor = item.Fill?.ToColor();
					canvas.StrokeLineCap = LineCap.Round;

					var linePoint = item.LinePoints;

					if (linePoint != null)
					{
						DrawConnectorLines(canvas, linePoint);
					}

					canvas.CanvasRestoreState();
				}
			}

			if (_chart.LabelTemplate == null)
			{
				foreach (var item in _segments)
				{
					if (item.IsLabelVisible)
					{
						var style = _dataLabelSettings.LabelStyle;
						var rect = item.LabelRect;
						var angle = (float)style.Angle;

						if (angle != 0)
						{
							angle = angle > 360 ? angle % 360 : angle;
							canvas.CanvasSaveState();
							canvas.Rotate(angle, (float)rect.X, (float)rect.Y);
						}

						if (style.StrokeWidth > 0)
						{
							canvas.StrokeSize = (float)style.StrokeWidth;
							canvas.StrokeColor = style.Stroke.ToColor();
						}

						var fillColor = style.IsBackgroundColorUpdated ? style.Background : _dataLabelSettings.UseSeriesPalette ? item.Fill : style.Background;
						PyramidDataLabelHelper.DrawBackground(canvas, fillColor ?? SolidColorBrush.Transparent, style, rect);

						Color fontColor = style.TextColor;

						if (fontColor == default(Color) || fontColor == Colors.Transparent)
						{
							fontColor = fillColor == default(Brush) || fillColor.ToColor() == Colors.Transparent ?
								(item.Position == DataLabelPlacement.Inner ?
								ChartUtils.GetContrastColor((item.Fill as SolidColorBrush).ToColor()) :
								(_chart as IChart).GetTextColorBasedOnChartBackground()) :
								ChartUtils.GetContrastColor((fillColor as SolidColorBrush).ToColor());

							//TODO: set animation value for fontColor 
							//Created new font family, as need to pass contrast text color for native font family rendering.
							var labelStyle = style.Clone();

							//TODO: Need to add all values when it use for other cases. 
							labelStyle.Margin = style.Margin;
							labelStyle.TextColor = fontColor;
							PyramidDataLabelHelper.DrawLabel(canvas, item, rect, labelStyle);
						}
						else
						{
							PyramidDataLabelHelper.DrawLabel(canvas, item, rect, style);
						}
					}
				}
			}
			else
			{
				UpdateTemplatePosition();
			}
		}

		/// <summary>
		/// Draws connector lines based on orientation-specific logic.
		/// </summary>
		internal virtual void DrawConnectorLines(ICanvas canvas, Point[] linePoints)
		{
			if (linePoints == null || linePoints.Length == 0)
			{
				return;
			}

			if (_chart.IsHorizontalOrientation)
			{
				if (linePoints.Length >= 2)
				{
					canvas.DrawLine(linePoints[0], linePoints[1]);
				}
			}
			else
			{
				if (linePoints.Length >= 2)
				{
					canvas.DrawLine(linePoints[0], linePoints[1]);
				}

				if (linePoints.Length >= 3)
				{
					canvas.DrawLine(linePoints[1], linePoints[2]);
				}
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Draws a data label at the specified position with the given style.
		/// </summary>
		static void DrawLabel(ICanvas canvas, IPyramidLabels item, Rect rect, ChartDataLabelStyle style)
		{
#if ANDROID
			PyramidDataLabelHelper.DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top / 2 + item.DataLabelSize.Height), style);
#else

			PyramidDataLabelHelper.DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top), style);
#endif
		}

		/// <summary>
		/// Draws the background of a data label with optional corner radius and border.
		/// </summary>
		static void DrawBackground(ICanvas canvas, Brush fill, ChartDataLabelStyle style, Rect backgroundRect)
		{
			canvas.CanvasSaveState();

			canvas.SetFillPaint(fill, backgroundRect);
			//Todo: Need to check condition for label background
			if (style.HasCornerRadius)
			{
				var cornerRadius = style.CornerRadius;
				canvas.FillRoundedRectangle(backgroundRect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
			}
			else
			{
				canvas.FillRectangle(backgroundRect);
			}

			//Todo: Need to check with border width and color in DrawLabel override method.
			if (style.StrokeWidth > 0 && style.IsStrokeColorUpdated)
			{
				if (style.HasCornerRadius)
				{
					var cornerRadius = style.CornerRadius;
					canvas.DrawRoundedRectangle(backgroundRect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
				}
				else
				{
					canvas.DrawRectangle(backgroundRect);
				}
			}

			canvas.CanvasRestoreState();
		}

		/// <summary>
		/// Draws a text label at the specified position with the given style.
		/// </summary>
		static void DrawLabel(ICanvas canvas, string label, PointF point, ChartDataLabelStyle style)
		{
			canvas.DrawText(label, point.X, point.Y, style);
		}

		/// <summary>
		/// Adds a data label segment to the collection.
		/// </summary>
		internal void AddLabel(IPyramidLabels segment)
		{
			if (!_segments.Contains(segment))
			{
				_segments.Add(segment);
			}
		}

		/// <summary>
		/// Clears all stored label data and rectangles.
		/// </summary>
		internal void ClearDefaultValues()
		{
			_segments.Clear();
			_labelRects.Clear();
		}

		/// <summary>
		/// Updates the position of template-based data labels.
		/// </summary>
		internal void UpdateTemplatePosition()
		{
			for (int i = 0; i < _segments.Count; i++)
			{
				var segment = _segments[i];

				if (segment.IsLabelVisible)
				{
					if (_chart.DataLabels != null && _chart.DataLabels.Count > i)
					{
						ChartDataLabel dataLabel = _chart.DataLabels[i];

						dataLabel.XPosition = segment.LabelRect.X + (segment.LabelRect.Width / 2);
						dataLabel.YPosition = segment.LabelRect.Y + (segment.LabelRect.Height / 2);
					}
				}
			}
		}

		#endregion
	}
}