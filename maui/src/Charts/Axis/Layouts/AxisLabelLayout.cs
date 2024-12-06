using Syncfusion.Maui.Toolkit.Graphics.Internals;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class AxisLabelLayout
	{
		#region Internal Properties
		internal bool NeedToRotate { get; set; }

		internal ChartAxis Axis { get; set; }

		internal List<RectF>? LabelsRect { get; set; }

		internal List<SizeF>? ComputedSizes { get; set; }

		internal List<SizeF>? DesiredSizes { get; set; }

		internal double DesiredHeight { get; set; }

		internal double DesiredWidth { get; set; }

		internal List<Dictionary<int, RectF>>? RectByRowsAndCols { get; set; }

		internal float MarginTop { get; set; }

		internal float MarginBottom { get; set; }

		internal float MarginLeft { get; set; }

		internal float MarginRight { get; set; }

		internal float Offset { get; set; }

		internal AxisLabelLayout(ChartAxis axis)
		{
			Axis = axis;
		}
		#endregion

		#region Methods

		#region Internal Methods

		internal static void DrawLabelBackground(ICanvas canvas, float x, float y, SizeF size, ChartAxisLabelStyle labelStyle)
		{
			float halfStrokeWidth = (float)labelStyle.StrokeWidth / 2;

			CornerRadius cornerRadius = labelStyle.CornerRadius;

			var rect = new Rect(x, y, size.Width, size.Height);

			canvas.SetFillPaint(labelStyle.Background, rect);

			if (!labelStyle.HasCornerRadius && labelStyle.IsBackgroundColorUpdated)
			{
				canvas.FillRectangle(rect);
			}
			else
			{
				canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
			}

			if (labelStyle.IsStrokeColorUpdated)
			{
				canvas.StrokeColor = labelStyle.Stroke.ToColor();
				canvas.StrokeSize = halfStrokeWidth;
				if (labelStyle.HasCornerRadius)
				{
					canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
				}
				else
				{
					canvas.DrawRectangle(rect);
				}
			}
		}

		/// <summary>
		/// Method used to Align Axis label at Start and End position
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		/// <param name="labelStyle">The label style.</param>
		/// <returns>The position</returns>
		internal static float OnAxisLabelAlignment(float position, float size, ChartAxisLabelStyle labelStyle)
		{
			var labelAlignment = labelStyle.LabelAlignment;

			switch (labelAlignment)
			{
				case ChartAxisLabelAlignment.Start:
					position -= size / 2;
					break;
				case ChartAxisLabelAlignment.End:
					position += size / 2;
					break;
			}

			return position;
		}

		/// <summary>
		/// Method used to align wrapped axis labels at start,center and end position.
		/// </summary>
		/// <param name="labelStyle">The label style.</param>
		/// <returns>The position</returns>
		internal static HorizontalAlignment OnWrapAxisLabelAlignment(ChartAxisLabelStyle labelStyle)
		{
			return labelStyle.WrappedLabelAlignment switch
			{
				ChartAxisLabelAlignment.Start => HorizontalAlignment.Left,
				ChartAxisLabelAlignment.Center => HorizontalAlignment.Center,
				ChartAxisLabelAlignment.End => HorizontalAlignment.Right,
				_ => (HorizontalAlignment)labelStyle.WrappedLabelAlignment,
			};
		}

		/// <summary>
		/// Method used to create the axis layout.
		/// </summary>
		/// <param name="chartAxis">The ChartAxis.</param>
		/// <returns>The layout.</returns>
		internal static AxisLabelLayout CreateAxisLayout(ChartAxis chartAxis)
		{
			if (!chartAxis.IsVertical)
			{
				return new HorizontalLabelLayout(chartAxis);
			}

			return new VerticalLabelLayout(chartAxis);
		}

		internal virtual SizeF Measure(SizeF availableSize)
		{
			double rotateAngle = Axis.LabelRotation;
			ComputedSizes = [];
			DesiredSizes = ComputedSizes;

			NeedToRotate = !double.IsNaN(rotateAngle) &&
									Math.Abs(rotateAngle % 360) > double.Epsilon;

			if (NeedToRotate)
			{
				ComputedSizes = [];//TODO: Need of creating compute size
			}

			if (Axis != null && Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
			{
				foreach (var axisLabel in Axis.VisibleLabels)
				{
					ChartAxisLabelStyle labelStyle = axisLabel.LabelStyle ?? Axis.LabelStyle;
					labelStyle.LabelsIntersectAction = Axis.LabelsIntersectAction;
					labelStyle.WrapWidthCollection = [];

					MarginTop = (float)labelStyle.Margin.Top;
					MarginLeft = (float)labelStyle.Margin.Left;
					MarginRight = (float)labelStyle.Margin.Right;
					MarginBottom = (float)labelStyle.Margin.Bottom;

					var label = axisLabel.Content.Tostring();
					Size desiredSize = new Size(0, 0);
					if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap && !double.IsNaN(labelStyle.MaxWidth))
					{
						if (labelStyle.MaxWidth > 0)
						{
							Size wrapSize = label.Measure(labelStyle.MaxWidth, labelStyle);
							desiredSize = new Size(wrapSize.Width + MarginLeft + MarginRight, wrapSize.Height + MarginTop + MarginBottom);
						}
					}
					else
					{
						Size measuredSize = label.Measure(labelStyle);
						desiredSize = new Size(measuredSize.Width + MarginLeft + MarginRight, measuredSize.Height + MarginTop + MarginBottom);
					}
					DesiredWidth = desiredSize.Width > DesiredWidth ? desiredSize.Width : DesiredWidth;
					DesiredHeight = desiredSize.Height > DesiredHeight ? desiredSize.Height : DesiredHeight;
					DesiredSizes.Add(desiredSize);
					if (NeedToRotate)
					{
						ComputedSizes.Add(GetRotatedSize(desiredSize, rotateAngle));
					}
				}

				CalculateActualPlotOffset(availableSize);
			}

			return new SizeF();
		}

		internal bool IsOpposed()
		{
			if (Axis == null)
			{
				return false;
			}

			var opposedPos = Axis.IsOpposed();
			var labelPos = Axis.LabelsPosition;
			return (opposedPos && labelPos == AxisElementPosition.Outside)
				|| (!opposedPos && labelPos == AxisElementPosition.Inside);
		}

		internal virtual float LayoutElements()
		{
			int i = 1;
			int prevIndex = 0;
			var labels = Axis.VisibleLabels;

			if (labels == null || Axis.LabelsIntersectAction == AxisLabelsIntersectAction.None)
			{
				return 0;
			}

			if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Hide)
			{
				int length = labels.Count;
				for (; i < length; i++)
				{
					var label = labels[i];
					var rowColElement = RectByRowsAndCols?.ElementAt(0);
					if (rowColElement != null && AxisLabelLayout.IntersectsWith(rowColElement[prevIndex], rowColElement[i], prevIndex, i))
					{
						if (i == length - 1 && Axis is RangeAxisBase rangeAxis)
						{
							if (rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible ||
								 ((rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible) && !(Axis.ZoomFactor < 1)))
							{
								if ((rangeAxis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift || rangeAxis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit) && LabelsRect != null)
								{
									var lastRect = LabelsRect[i];
									for (int j = i - 1; j >= 0; j--)
									{
										RectF rectBefore = LabelsRect[j];

										if (AxisLabelLayout.IntersectsWith(rectBefore, lastRect, j, i))
										{
											labels[j].IsVisible = false;
										}
										else
										{
											break;
										}
									}
								}
								else
								{
									labels[i - 1].IsVisible = false;
								}
							}
							else
							{
								label.IsVisible = false;
							}
						}
						else
						{
							label.IsVisible = false;
						}
					}
					else
					{
						prevIndex = i;
					}
				}
			}
			else if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.MultipleRows || Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap)
			{
				i = 1;
				prevIndex = 0;
				int length = labels.Count;

				for (; i < length; i++)
				{
					var rectByRowsAndCol = RectByRowsAndCols?.ElementAt(0);
					if (rectByRowsAndCol != null && AxisLabelLayout.IntersectsWith(rectByRowsAndCol[prevIndex], rectByRowsAndCol[i], prevIndex, i)
						&& labels[i].IsVisible && labels[prevIndex].IsVisible)
					{
						RectF rect = rectByRowsAndCol[i];
						rectByRowsAndCol.Remove(i);
						InsertToRowOrColumn(1, i, rect);
					}
					else
					{
						prevIndex = i;
					}
				}
			}
			return 0;
		}

		internal static bool IntersectsWith(RectF r1, RectF r2, int prevIndex, int currentIndex)
		{
			//TODO:validate rect to position next label.
			return !(r2.Left > r1.Right || r2.Right < r1.Left || r2.Top > r1.Bottom || r2.Bottom < r1.Top);
		}

		internal virtual void CalculateActualPlotOffset(SizeF availableSize)
		{
			Axis.ActualPlotOffset = 0f;
			Axis.ActualPlotOffsetStart = (float)(double.IsNaN(Axis.PlotOffsetStart) ? 0f : Axis.PlotOffsetStart);
			Axis.ActualPlotOffsetEnd = (float)(double.IsNaN(Axis.PlotOffsetEnd) ? 0f : Axis.PlotOffsetEnd);
		}

		internal virtual void OnDraw(ICanvas drawing, SizeF finalSize)
		{
		}

		#endregion

		#region Private Methods

		void InsertToRowOrColumn(int rowOrColIndex, int itemIndex, RectF rect)
		{
			if (RectByRowsAndCols == null)
			{
				return;
			}

			if (RectByRowsAndCols.Count <= rowOrColIndex)
			{
				RectByRowsAndCols.Add([]);
				RectByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
			}
			else
			{
				var lastRowOrColumn = RectByRowsAndCols[rowOrColIndex];
				int lastIndex = lastRowOrColumn.Count - 1;
				var lastKey = lastRowOrColumn.Keys.ToArray()[lastIndex];
				RectF prevRect = lastRowOrColumn[lastKey];

				if (AxisLabelLayout.IntersectsWith(prevRect, rect, lastIndex, itemIndex))
				{
					InsertToRowOrColumn(++rowOrColIndex, itemIndex, rect);
				}
				else
				{
					RectByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
				}
			}
		}

		//TODO:Calculate rotation size for label.
		static Size GetRotatedSize(Size size, double degrees)
		{
			return ChartUtils.GetRotatedSize(size, (float)degrees);
		}
		#endregion

		#endregion
	}

	internal class HorizontalLabelLayout : AxisLabelLayout
	{
		#region Fields
		float _desiredSize;
		#endregion

		#region Internal Properties
		internal SizeF AvailableSize { get; set; }
		#endregion

		#region Constructor
		internal HorizontalLabelLayout(ChartAxis axis)
			: base(axis)
		{
		}
		#endregion

		#region Methods

		internal override void OnDraw(ICanvas canvas, SizeF finalSize)
		{
			if (RectByRowsAndCols == null || Axis.VisibleLabels == null || DesiredSizes == null || ComputedSizes == null)
			{
				return;
			}

			bool isOpposed = IsOpposed();
			double left = Axis.GetActualPlotOffsetStart();
			double rotateAngle = Axis.LabelRotation % 360;

			if (!(rotateAngle >= -90 && rotateAngle <= 90))
			{
				rotateAngle = 0;
				NeedToRotate = false;
			}

			double maxHeight = 0, top = isOpposed ? finalSize.Height - Offset : Offset;

			foreach (var rowsOrColumn in RectByRowsAndCols)
			{
				foreach (var rowsOrCols in rowsOrColumn)
				{
					int i = rowsOrCols.Key;
					var label = Axis.VisibleLabels[i];
					var labelStyle = label.LabelStyle ?? Axis.LabelStyle;
					MarginLeft = (float)labelStyle.Margin.Left;
					MarginBottom = (float)labelStyle.Margin.Bottom;

					var content = label.Content.Tostring();

					if (!label.IsVisible || string.IsNullOrEmpty(content))
					{
						continue;
					}

					SizeF actualSize = DesiredSizes[i];
#if ANDROID
					actualSize = new SizeF(actualSize.Width, (float)DesiredHeight);
#endif
					SizeF rotatedSize = ComputedSizes[i];
					RectF labelRect = rowsOrCols.Value;
					float yPos = (float)top;

					float xPos = labelRect.Left + (float)left - (actualSize.Width / 2) + (rotatedSize.Width / 2);

					yPos -= isOpposed ? actualSize.Height : 0;

					float height = rotatedSize.Height;
					float textX = xPos + MarginLeft;

					SizeF measuredSize = content.Measure(labelStyle);

					//Added platform specific code for position the label.
#if ANDROID
					float textY = ((float)yPos + MarginTop + (float)DesiredHeight / 2);
#else
					float textY = yPos + MarginTop;
#endif

					float rotateOriginX = xPos;
					float rotateOriginY = yPos;

					if (NeedToRotate)
					{
						if ((!Axis.IsOpposed() && Axis.LabelsPosition == AxisElementPosition.Inside) ||
							  (Axis.IsOpposed() && Axis.LabelsPosition == AxisElementPosition.Outside))
						{
							rotateOriginX = xPos + (actualSize.Width / 2);
							rotateOriginY = yPos + actualSize.Height;

							if (rotateAngle == 90)
							{
								textX -= actualSize.Width / 2;
								xPos -= actualSize.Width / 2;

								textY += actualSize.Height / 2;
								yPos += actualSize.Height / 2;
							}
							else if (rotateAngle == -90)
							{
								textX += actualSize.Width / 2;
								xPos += actualSize.Width / 2;

								textY += actualSize.Height / 2;
								yPos += actualSize.Height / 2;
							}
							else if (rotateAngle > 0 && rotateAngle <= 45)
							{
								textX -= actualSize.Width / 2;
								xPos -= actualSize.Width / 2;
							}
							else if (rotateAngle < 0 && rotateAngle >= -45)
							{
								textX += actualSize.Width / 2;
								xPos += actualSize.Width / 2;
							}
							else
							{
								if (rotateAngle > 45 && rotateAngle < 90)
								{
									textX -= actualSize.Width / 2;
									xPos -= actualSize.Width / 2;
								}
								else
								{
									textX += actualSize.Width / 2;
									xPos += actualSize.Width / 2;
								}

								textY += actualSize.Height / 2;
								yPos += actualSize.Height / 2;
							}
						}
						else
						{
							rotateOriginX = xPos + (actualSize.Width / 2);
							rotateOriginY = yPos;

							if (rotateAngle == 90)
							{
								textX += actualSize.Width / 2;
								xPos += actualSize.Width / 2;

								textY -= actualSize.Height / 2;
								yPos -= actualSize.Height / 2;
							}
							else if (rotateAngle == -90)
							{
								textX -= actualSize.Width / 2;
								xPos -= actualSize.Width / 2;

								textY -= actualSize.Height / 2;
								yPos -= actualSize.Height / 2;
							}
							else if (rotateAngle > 0 && rotateAngle <= 45)
							{
								textX += actualSize.Width / 2;
								xPos += actualSize.Width / 2;

								textY -= actualSize.Height / 4;
								yPos -= actualSize.Height / 4;
							}
							else if (rotateAngle < 0 && rotateAngle >= -45)
							{
								textX -= actualSize.Width / 2;
								xPos -= actualSize.Width / 2;

								textY -= actualSize.Height / 4;
								yPos -= actualSize.Height / 4;
							}
							else
							{
								if (rotateAngle > 45 && rotateAngle < 90)
								{
									textX += actualSize.Width / 2;
									xPos += actualSize.Width / 2;
								}
								else
								{
									textX -= actualSize.Width / 2;
									xPos -= actualSize.Width / 2;
								}

								textY -= actualSize.Height / 2;
								yPos -= actualSize.Height / 2;
							}
						}
					}

					label.RotateOriginX = rotateOriginX;
					label.RotateOriginY = rotateOriginY;

					canvas.CanvasSaveState();

					canvas.Rotate((float)rotateAngle, rotateOriginX, rotateOriginY);

					if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap && !double.IsNaN(labelStyle.MaxWidth))
					{
						if (labelStyle.MaxWidth > 0)
						{
							SizeF wrapSize = content.Measure(labelStyle.MaxWidth, labelStyle);
							SizeF wrapDesiredSize = new Size(wrapSize.Width + MarginLeft + MarginRight, wrapSize.Height + MarginTop + MarginBottom);

							if (labelStyle.CanDraw())
							{
								AxisLabelLayout.DrawLabelBackground(canvas, xPos, yPos, wrapDesiredSize, labelStyle);
							}

							canvas.DrawText(content, new Rect(xPos + MarginLeft, yPos + MarginTop, wrapSize.Width, wrapSize.Height), OnWrapAxisLabelAlignment(labelStyle), VerticalAlignment.Top, labelStyle);
						}
					}
					else
					{
						if (labelStyle.CanDraw())
						{
							AxisLabelLayout.DrawLabelBackground(canvas, xPos, yPos, actualSize, labelStyle);
						}

						canvas.DrawText(content, textX, textY, labelStyle);
					}

					if (LabelsRect != null)
					{
						LabelsRect[i] = new RectF(xPos, yPos, actualSize.Width, actualSize.Height);
					}

					canvas.CanvasRestoreState();

					if (height > maxHeight)
					{
						maxHeight = height;
					}
				}

				if (isOpposed)
				{
					top -= maxHeight;
				}
				else
				{
					top += maxHeight;
				}
			}
		}

		internal override SizeF Measure(SizeF availableSize)
		{
			if (Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
			{
				AvailableSize = availableSize;
				base.Measure(availableSize);
				CalcBounds(availableSize.Width - (float)Axis.GetActualPlotOffset());
				_desiredSize = Math.Max(LayoutElements(), (float)Axis.LabelExtent);
				return new SizeF(availableSize.Width, _desiredSize);
			}

			return new SizeF(availableSize.Width, 0);
		}

		internal override void CalculateActualPlotOffset(SizeF availableSize)
		{
			if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit && ComputedSizes != null && Axis.VisibleLabels != null)
			{
				float plotOffset = 0f;
				SizeF computedSize = ComputedSizes[0];
				var label = Axis.VisibleLabels[0];
				float coEff = Axis.ValueToCoefficient(label.Position);
				float position = (coEff * availableSize.Width) - (computedSize.Width / 2);

				float firstElementWidth = 0;
				float lastElementWidth = 0;
				float width = Axis.IsInversed ? availableSize.Width : 0f;

				if ((position - (computedSize.Width / 2) + plotOffset) < width)
				{
					firstElementWidth = computedSize.Width;
				}

				int index = Axis.VisibleLabels.Count - 1;
				computedSize = ComputedSizes[index];
				width = Axis.IsInversed ? 0f : availableSize.Width;

				if ((position + (computedSize.Width / 2) - plotOffset) > width)
				{
					lastElementWidth = computedSize.Width;
				}

				float offset = Math.Max(firstElementWidth / 2, lastElementWidth / 2);
				Axis.ActualPlotOffset = Math.Max(offset, plotOffset);
			}
			else
			{
				base.CalculateActualPlotOffset(availableSize);
			}
		}

		// Calculate horizontal axis label width
		internal override float LayoutElements()
		{
			base.LayoutElements();
			float totalHeight = 0;

			if (RectByRowsAndCols != null)
			{
				foreach (var rowsOrColumns in RectByRowsAndCols)
				{
					float max = 0;

					foreach (var rowOrCol in rowsOrColumns)
					{
						var currValue = rowOrCol.Value.Height;
						if (currValue > max)
						{
							max = currValue;
						}
					}

					totalHeight += max;
				}
			}

			return totalHeight;
		}

		void CalcBounds(float size)
		{
			RectByRowsAndCols = [];
			Dictionary<int, RectF> rowsOrColumn = [];
			RectByRowsAndCols.Add(rowsOrColumn);
			var axisLabels = Axis.VisibleLabels;

			if (axisLabels == null || ComputedSizes == null)
			{
				return;
			}

			int length = axisLabels.Count;

			LabelsRect = [];

			for (int i = 0; i < length; i++)
			{
				var chartAxisLabel = axisLabels[i];
				var labelStyle = chartAxisLabel.LabelStyle ?? Axis.LabelStyle;
				MarginLeft = (float)labelStyle.Margin.Left;
				MarginRight = (float)labelStyle.Margin.Right;

				float coEff = Axis.ValueToCoefficient(chartAxisLabel.Position);
				float position = (coEff * size) - (ComputedSizes[i].Width / 2); //TODO: ensure the width need to calculate,
				position -= (MarginLeft - MarginRight) / 2;
				position = OnAxisLabelAlignment(position, ComputedSizes[i].Width + (float)labelStyle.StrokeWidth, labelStyle);
				RectF rect = new RectF(position, 0, ComputedSizes[i].Width, ComputedSizes[i].Height);
				rowsOrColumn.Add(i, rect);
				LabelsRect.Add(rect);
			}

			if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
			{
				var computedSize = ComputedSizes[0];

				var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
				if (rectByRowAndCols[0].Left < 0)
				{
					LabelsRect[0] = rectByRowAndCols[0] = new RectF(0, 0, computedSize.Width, computedSize.Height);
				}

				int index = axisLabels.Count - 1;
				if (rectByRowAndCols[index].Right > size)
				{
					computedSize = ComputedSizes[index];
					LabelsRect[index] = rectByRowAndCols[index] = new RectF(size - computedSize.Width, 0, computedSize.Width, computedSize.Height);
				}
			}
			else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
			{
				var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
				if (rectByRowAndCols[0].Left < 0)
				{
					rectByRowAndCols[0] = RectF.Zero;
					LabelsRect[0] = RectF.Zero;
					axisLabels[0].IsVisible = false;
				}

				int index = axisLabels.Count - 1;
				if (rectByRowAndCols[index].Right > size)
				{
					rectByRowAndCols[index] = RectF.Zero;
					LabelsRect[index] = RectF.Zero;
					axisLabels[index].IsVisible = false;
				}
			}
		}

		#endregion
	}

	internal class VerticalLabelLayout : AxisLabelLayout
	{
		#region Fields
		float _desiredSize;
		#endregion

		#region Internal Properties
		internal SizeF AvailableSize { get; set; }
		#endregion

		#region Constructor
		internal VerticalLabelLayout(ChartAxis axis)
			: base(axis)
		{
		}
		#endregion

		#region Methods

		internal override void OnDraw(ICanvas canvas, SizeF finalSize)
		{
			if (RectByRowsAndCols == null || Axis.VisibleLabels == null || DesiredSizes == null || ComputedSizes == null)
			{
				return;
			}

			bool isOpposed = IsOpposed();
			float top = (float)Axis.GetActualPlotOffsetEnd();
			float rotateAngle = (float)Axis.LabelRotation % 360;

			if (!(rotateAngle >= -90 && rotateAngle <= 90))
			{
				rotateAngle = 0;
				NeedToRotate = false;
			}

			float maxWidth = 0, left = isOpposed ? Offset : finalSize.Width - Offset;

			foreach (var rowsOrColumn in RectByRowsAndCols)
			{
				foreach (var rowsOrCols in rowsOrColumn)
				{
					int i = rowsOrCols.Key;
					var label = Axis.VisibleLabels[i];
					var labelStyle = label.LabelStyle ?? Axis.LabelStyle;
					MarginTop = (float)labelStyle.Margin.Top;
					MarginRight = (float)labelStyle.Margin.Right;

					var content = label.Content.Tostring();

					if (!label.IsVisible || string.IsNullOrEmpty(content))
					{
						continue;
					}

					SizeF actualSize = DesiredSizes[i];
					SizeF rotatedSize = ComputedSizes[i];
					RectF labelRect = rowsOrCols.Value;
					float xPos, yPos, textX, textY;

					SizeF measuredSize = content.Measure(labelStyle);
					xPos = left;
					yPos = labelRect.Top + top - actualSize.Height / 2 + rotatedSize.Height / 2;
					xPos -= isOpposed ? 0 : actualSize.Width;

					textX = xPos + MarginRight; //TODO: ensure the width need to calculate,
												//Added platform specific code for position the label.

					float rotateOriginY = 0;

#if ANDROID
					textY = yPos + MarginTop + (float)actualSize.Height / 2;
					if (Axis.IsPolarArea && (Axis.PolarStartAngle == 0 || Axis.PolarStartAngle == 180))
					{
						xPos = labelRect.Top + top + (actualSize.Height / 2) + rotatedSize.Height;
						yPos = finalSize.Height + (actualSize.Height / 2);
						yPos -= isOpposed ? rotatedSize.Height : 0;
						textX = xPos + (MarginRight);
						textY = yPos + MarginTop;
					}

					rotateOriginY = yPos + MarginTop;
#else
					textY = yPos + (measuredSize.Height) / 2 - MarginTop;

					if (Axis.IsPolarArea && (Axis.PolarStartAngle == 0 || Axis.PolarStartAngle == 180))
					{
						xPos = labelRect.Top + top + (actualSize.Height / 2) + rotatedSize.Height;
						yPos = finalSize.Height + (actualSize.Height / 2);
						yPos -= isOpposed ? rotatedSize.Height : 0;
						textX = xPos + (MarginRight);
						textY = yPos + MarginTop;
					}

					rotateOriginY = yPos + MarginTop;
#endif
					float width = rotatedSize.Width;
					float rotateOriginX = textX;

					//TODO: if label need to rotate.
					if (NeedToRotate)
					{
						if ((Axis.IsOpposed() && Axis.LabelsPosition == AxisElementPosition.Inside) ||
							(!Axis.IsOpposed() && Axis.LabelsPosition == AxisElementPosition.Outside))
						{
							rotateOriginX += measuredSize.Width;
							rotateOriginY += measuredSize.Height / 2;

							if (rotateAngle > 0 && rotateAngle <= 90)
							{
								textY += measuredSize.Height / 2;
								yPos += measuredSize.Height / 2;

								if (rotateAngle == 90)
								{
									textX += measuredSize.Width / 2;
									xPos += measuredSize.Width / 2;
								}
							}
							else if (rotateAngle < 0 && rotateAngle >= -90)
							{
								textY -= measuredSize.Height / 2;
								yPos -= measuredSize.Height / 2;

								if (rotateAngle == -90)
								{
									textX += measuredSize.Width / 2;
									xPos += measuredSize.Width / 2;
								}
							}
						}
						else
						{
							rotateOriginY += measuredSize.Height / 2;

							if (rotateAngle > 0 && rotateAngle <= 90)
							{
								textY -= measuredSize.Height / 2;
								yPos -= measuredSize.Height / 2;

								if (rotateAngle == 90)
								{
									textX -= measuredSize.Width / 2;
									xPos -= measuredSize.Width / 2;
								}
							}
							else if (rotateAngle < 0 && rotateAngle >= -90)
							{
								textY += measuredSize.Height / 2;
								yPos += measuredSize.Height / 2;

								if (rotateAngle == -90)
								{
									textX -= measuredSize.Width / 2;
									xPos -= measuredSize.Width / 2;
								}
							}
						}
					}

					label.RotateOriginX = rotateOriginX;
					label.RotateOriginY = rotateOriginY;

					canvas.CanvasSaveState();
					canvas.Rotate(rotateAngle, rotateOriginX, rotateOriginY);

					if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap && !double.IsNaN(labelStyle.MaxWidth))
					{
						if (labelStyle.MaxWidth > 0)
						{
							SizeF wrapSize = content.Measure(labelStyle.MaxWidth, labelStyle);
							SizeF wrapDesiredSize = new Size(wrapSize.Width + MarginLeft + MarginRight, wrapSize.Height + MarginTop + MarginBottom);

							if (labelStyle.CanDraw())
							{
								AxisLabelLayout.DrawLabelBackground(canvas, xPos, yPos, wrapDesiredSize, labelStyle);
							}

							canvas.DrawText(content, new Rect(xPos + MarginLeft, yPos + MarginTop, wrapSize.Width, wrapSize.Height), OnWrapAxisLabelAlignment(labelStyle), VerticalAlignment.Top, labelStyle);
						}
					}
					else
					{
						if (labelStyle.CanDraw())
						{
							AxisLabelLayout.DrawLabelBackground(canvas, xPos, yPos, actualSize, labelStyle);
						}

						canvas.DrawText(content, textX, textY, labelStyle);
					}

					if (LabelsRect != null)
					{
						LabelsRect[i] = new RectF(xPos, yPos, actualSize.Width, actualSize.Height);
					}

					canvas.CanvasRestoreState();

					if (width > maxWidth)
					{
						maxWidth = width;
					}
				}

				if (isOpposed)
				{
					left += maxWidth;
				}
				else
				{
					left -= maxWidth;
				}
			}
		}

		internal override SizeF Measure(SizeF availableSize)
		{
			if (Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
			{
				AvailableSize = availableSize;
				base.Measure(availableSize);
				CalcBounds(availableSize.Height - (float)Axis.GetActualPlotOffset());
				_desiredSize = Math.Max(LayoutElements(), (float)Axis.LabelExtent);
				return new SizeF(_desiredSize, availableSize.Height);
			}

			return new SizeF(0, availableSize.Height);
		}

		internal override void CalculateActualPlotOffset(SizeF availableSize)
		{
			if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit && ComputedSizes != null && Axis.VisibleLabels != null)
			{
				float plotOffset = 0f;
				SizeF computedSize = ComputedSizes[0];
				var label = Axis.VisibleLabels[0];
				float coEff = Axis.ValueToCoefficient(label.Position);
				float position = ((1 - coEff) * availableSize.Height) - (computedSize.Height / 2);

				float firstElementHeight = 0;
				float lastElementHeight = 0;
				float height = Axis.IsInversed ? 0f : availableSize.Height;

				if (position + (computedSize.Height / 2) - plotOffset > height)
				{
					firstElementHeight = computedSize.Height;
				}

				int index = Axis.VisibleLabels.Count - 1;
				label = Axis.VisibleLabels[index];
				coEff = Axis.ValueToCoefficient(label.Position);
				computedSize = ComputedSizes[index];
				position = ((1 - coEff) * availableSize.Height) - (computedSize.Height / 2);
				height = Axis.IsInversed ? availableSize.Height : 0f;

				if (position - (computedSize.Height / 2) + plotOffset < height)
				{
					lastElementHeight = computedSize.Height;
				}

				float offset = Math.Max(firstElementHeight / 2, lastElementHeight / 2);
				Axis.ActualPlotOffset = Math.Max(offset, plotOffset);
			}
			else
			{
				base.CalculateActualPlotOffset(availableSize);
			}
		}

		/// <summary>
		/// Returns desired width
		/// </summary>
		/// <returns>The total width.</returns>
		internal override float LayoutElements()
		{
			base.LayoutElements();
			float totalWidth = 0;

			if (RectByRowsAndCols != null)
			{
				foreach (var rowsOrColumns in RectByRowsAndCols)
				{
					float max = 0;

					foreach (var rowOrCol in rowsOrColumns)
					{
						var currValue = rowOrCol.Value.Width;

						if (Axis.IsPolarArea && (Axis.PolarStartAngle == 0 || Axis.PolarStartAngle == 180))
						{
							currValue = rowOrCol.Value.Height;
						}

						if (currValue > max)
						{
							max = currValue;
						}
					}

					totalWidth += max;
				}
			}

			return totalWidth;
		}

		void CalcBounds(float size)
		{
			RectByRowsAndCols = [];
			Dictionary<int, RectF> rowsOrColumn = [];
			RectByRowsAndCols.Add(rowsOrColumn);
			var axisLabels = Axis.VisibleLabels;
			var computedSizes = ComputedSizes;
			LabelsRect = [];

			if (axisLabels == null || computedSizes == null)
			{
				return;
			}

			if (axisLabels != null)
			{
				int length = axisLabels.Count;

				for (int i = 0; i < length; i++)
				{
					var chartAxisLabel = axisLabels[i];
					float position;
					float coEff = Axis.ValueToCoefficient(chartAxisLabel.Position);
					var computedSize = computedSizes[i];
					float height = computedSize.Height, width = computedSize.Width;
					var labelStyle = chartAxisLabel.LabelStyle ?? Axis.LabelStyle;
					MarginTop = (float)labelStyle.Margin.Top;
					MarginBottom = (float)labelStyle.Margin.Bottom;

					if (Axis.IsPolarArea)
					{
						position = CalculatePolarPosition(coEff, width, height, size);
					}
					else
					{
						position = ((1 - coEff) * size) - (height / 2);
					}

					position -= (MarginTop - MarginBottom) / 2;
					position = OnAxisLabelAlignment(position, computedSizes[i].Height, labelStyle);
					RectF rect = new RectF(0, position, width, height);
					rowsOrColumn.Add(i, rect);
					LabelsRect.Add(rect);
				}
			}

			if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift && computedSizes != null)
			{
				var computedSize = computedSizes[0];

				var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
				if (rectByRowAndCols[0].Bottom > size)
				{
					rectByRowAndCols[0] = new RectF(0, size - computedSize.Height, computedSize.Width, computedSize.Height);
				}

				if (axisLabels != null)
				{
					int index = axisLabels.Count - 1;
					if (rectByRowAndCols[index].Top < 0)
					{
						computedSize = computedSizes[index];
						rectByRowAndCols[index] = new RectF(0, 0, computedSize.Width, computedSize.Height);
					}
				}
			}
			else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
			{
				var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
				if (axisLabels != null && axisLabels.Count > 0)
				{
					if (rectByRowAndCols[0].Bottom > size)
					{
						rectByRowAndCols[0] = RectF.Zero;
						LabelsRect[0] = RectF.Zero;
						axisLabels[0].IsVisible = false;
					}

					int index = axisLabels.Count - 1;
					if (rectByRowAndCols[index].Top < 0)
					{
						rectByRowAndCols[index] = RectF.Zero;
						LabelsRect[index] = RectF.Zero;
						axisLabels[index].IsVisible = false;
					}
				}
			}
		}

		float CalculatePolarPosition(float coEff, float width, float height, float size)
		{
			float polarPosition = ((1 - coEff) * size) - (height / 2);
			var angle = Axis.PolarStartAngle;

			if (angle == 180)
			{
				polarPosition = (-coEff * size) - (width / 2);
			}
			else if (angle == 90)
			{
				polarPosition = ((1 + coEff) * size) - (height / 2);
			}
			else if (angle == 0)
			{
				polarPosition = (coEff * size) - (width / 2);
			}

			return polarPosition;
		}

		#endregion
	}
}
