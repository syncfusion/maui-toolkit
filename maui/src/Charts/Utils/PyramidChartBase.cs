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

		//Layout the data labels.
		internal void ArrangeElements()
		{
			//DataLabel clip bounds.
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

		Rect CalculateLabelRect(IPyramidLabels item, DataLabelPlacement placement, Size size)
		{
			var bounds = _chart.SeriesBounds;
			var labelRect = Rect.Zero;
			var actualPosition = DataLabelPlacement.Inner;
			item.LinePoints = null;
			ChartDataLabelStyle style = _dataLabelSettings.LabelStyle;
			double offsetX = double.IsNaN(style.OffsetX) ? 0f : style.OffsetX;
			double offsetY = double.IsNaN(style.OffsetY) ? 0f : style.OffsetY;
			item.DataLabelX += (float)offsetX;
			item.DataLabelY += (float)offsetY;

			switch (placement)
			{
				case DataLabelPlacement.Inner:
				case DataLabelPlacement.Center:
				case DataLabelPlacement.Auto:
					var x = item.DataLabelX - size.Width / 2;
					var y = item.DataLabelY - size.Height / 2;
					labelRect = new Rect(new Point(x, y), size);
					actualPosition = DataLabelPlacement.Inner;
					break;
				case DataLabelPlacement.Outer:
					var outerX = bounds.X + bounds.Width + offsetX;
					y = item.DataLabelY;

					var linePoints = new Point[3];
					linePoints[0] = new Point(outerX, y);
					var bend = (outerX - bounds.Center.X) * BendRatio;
					linePoints[1] = new Point(outerX - bend, y);
					linePoints[2] = item.SlopePoint;

					item.LinePoints = linePoints;
					x = outerX;
					y = item.DataLabelY - size.Height / 2;

					var width = size.Width < DesiredWidth ? size.Width : DesiredWidth;
					labelRect = new Rect(new Point(x, y), new Size(width, size.Height));
					actualPosition = DataLabelPlacement.Outer;
					break;
			}

			labelRect = ArrangeSmartLabel(item, actualPosition, labelRect);

			return labelRect;
		}

		Rect ArrangeSmartLabel(IPyramidLabels item, DataLabelPlacement actualPosition, Rect labelRect)
		{
			foreach (var rect in _labelRects)
			{
				var IsIntersected = labelRect.IsOverlap(rect.Value);

				if (IsIntersected && actualPosition == DataLabelPlacement.Auto)
				{
					actualPosition = DataLabelPlacement.Outer;
					item.IsLabelVisible = true;
					labelRect = CalculateLabelRect(item, actualPosition, labelRect.Size);
					return labelRect;
				}

				if (IsIntersected && actualPosition == DataLabelPlacement.Inner)
				{
					item.IsLabelVisible = false;
					return labelRect;
				}
				else if (IsIntersected && actualPosition == DataLabelPlacement.Outer)
				{
					var adjacentRect = rect.Value;
					item.IsLabelVisible = true;
					item.DataLabelY = !_chart.ArrangeReverse ? adjacentRect.Y + adjacentRect.Height + Spacing + (float)labelRect.Height / 2
						: adjacentRect.Y - Spacing - (float)labelRect.Height / 2;
					labelRect = CalculateLabelRect(item, actualPosition, labelRect.Size);
				}
				else
				{
					item.IsLabelVisible = true;
				}
			}

			return labelRect;
		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal void OnDraw(ICanvas canvas, Rect dirtyRect)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			//TODO:Check label empty
			//Check rotation angle
			//Canvas stroke size
			//Canvas stroke color
			//Set fill paint
			//Draw rectangle with fill & corner radius
			//Draw stroke with corner radius.
			//Canvas font color, set contrast fontColor. 
			//Draw text.

			foreach (var item in _segments)
			{
				if (item.IsLabelVisible)
				{
					//Draw Line
					canvas.CanvasSaveState();
					canvas.StrokeSize = 1;
					canvas.StrokeColor = item.Fill?.ToColor();
					canvas.StrokeLineCap = LineCap.Round;

					var linePoint = item.LinePoints;

					if (linePoint != null)
					{
						canvas.DrawLine(linePoint[0], linePoint[1]);
						canvas.DrawLine(linePoint[1], linePoint[2]);
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

		static void DrawLabel(ICanvas canvas, IPyramidLabels item, Rect rect, ChartDataLabelStyle style)
		{
#if ANDROID
			PyramidDataLabelHelper.DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top / 2 + item.DataLabelSize.Height), style);
#else

			PyramidDataLabelHelper.DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top), style);
#endif
		}

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

		static void DrawLabel(ICanvas canvas, string label, PointF point, ChartDataLabelStyle style)
		{
			canvas.DrawText(label, point.X, point.Y, style);
		}

		internal void AddLabel(IPyramidLabels segment)
		{
			if (!_segments.Contains(segment))
			{
				_segments.Add(segment);
			}
		}

		internal void ClearDefaultValues()
		{
			_segments.Clear();
			_labelRects.Clear();
		}

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
	}
}