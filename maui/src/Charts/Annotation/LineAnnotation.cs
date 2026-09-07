using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class is used to add a line annotation to the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection.
	/// </summary>
	/// <remarks>
	/// LineAnnotation is used to draw a line across the chart area.
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart.Annotations>
	///   <chart:LineAnnotation X1="1" Y1="10" X2="4" Y2="20" Text="Line" CoordinateUnit="Axis">
	///   </chart:LineAnnotation>
	/// </chart:SfCartesianChart.Annotations>  
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	///  SfCartesianChart chart = new SfCartesianChart();
	///  var line = new LineAnnotation()
	///  {
	///    X1 = 1,
	///    Y1 = 10,
	///    X2 = 4,
	///    Y2 = 20,
	///    Text = "Line",
	///    CoordinateUnit= ChartCoordinateUnit.Axis,
	///  };
	///  
	/// chart.Annotations.Add(line);
	/// ]]>
	/// </code>
	/// </example>
	public partial class LineAnnotation : ShapeAnnotation
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="LineCap"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineCap"/> bindable property determines the style of the line cap for the <see cref="ChartAnnotation"/>.
		/// </remarks>
		public static readonly BindableProperty LineCapProperty = BindableProperty.Create(
		   nameof(LineCap),
		   typeof(ChartLineCap),
		   typeof(LineAnnotation),
		   ChartLineCap.None,
		   BindingMode.Default,
		   null,
		   OnAnnotationPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Represents the type of cap for line annotation.
		/// Gets or sets the line cap value for the line annotation.
		/// </summary>
		/// <value>This property takes the <see cref="ChartLineCap"/> as its value and its default value is <see cref="ChartLineCap.None"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     <chart:SfCartesianChart.Annotations>
		///          <chart:LineAnnotation X1="0" Y1="10" X2="4" Y2="50" LineCap="Arrow"/>
		///     </chart:SfCartesianChart.Annotations>  
		///     
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///   SfCartesianChart chart = new SfCartesianChart();     
		///
		///   // Eliminated for simplicity
		///   var line = new LineAnnotation()
		///   {
		///       X1 = 0,
		///       Y1 = 10,
		///       X2 = 4,
		///       Y2 = 50,
		///       LineCap = ChartLineCap.Arrow,
		///   };
		///  
		///   chart.Annotations.Add(line);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartLineCap LineCap
		{
			get { return (ChartLineCap)GetValue(LineCapProperty); }
			set { SetValue(LineCapProperty, value); }
		}

		#endregion

		#region Internal Properties

		internal float XPosition1 { get; set; }

		internal float XPosition2 { get; set; }

		internal float YPosition1 { get; set; }

		internal float YPosition2 { get; set; }

		internal float Angle { get; set; }

		internal List<Point> LineCapPoints { get; set; }

		internal ChartLabelStyle _axisLabelStyle;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="LineAnnotation"/>.
		/// </summary>
		public LineAnnotation()
		{
			LineCapPoints = [];
			_axisLabelStyle = new ChartLabelStyle();
		}

		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
		{
			if (Chart != null)
			{
				canvas.CanvasSaveState();

				if (CoordinateUnit == ChartCoordinateUnit.Axis)
				{
					var clipRect = Chart._chartArea.ActualSeriesClipRect;
					canvas.ClipRectangle(clipRect);
				}

				if (StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()))
				{
					canvas.StrokeSize = (float)StrokeWidth;
					canvas.StrokeColor = Stroke.ToColor();

					if (StrokeDashArray != null && StrokeDashArray.Count > 0)
					{
						canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
					}
				}

				if (LineCap == ChartLineCap.Arrow)
				{
					canvas.DrawLine(XPosition1, YPosition1, XPosition2, YPosition2);
					if (LineCapPoints.Count == 3)
					{
						var path = new PathF();
						path.MoveTo((float)LineCapPoints[0].X, (float)LineCapPoints[0].Y);
						path.LineTo((float)LineCapPoints[1].X, (float)LineCapPoints[1].Y);
						path.LineTo((float)LineCapPoints[2].X, (float)LineCapPoints[2].Y);
						path.Close();
						canvas.StrokeColor = Stroke.ToColor();
						canvas.StrokeSize = (float)StrokeWidth;
						canvas.FillColor = Stroke.ToColor();
						canvas.FillPath(path);
					}
				}
				else
				{
					canvas.DrawLine(XPosition1, YPosition1, XPosition2, YPosition2);
				}

				base.Draw(canvas, dirtyRect);
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Internal Methods

		internal override bool HitTest(Point touchPoint)
		{
			// Early exit validation - positions must be valid.
			if (float.IsNaN(XPosition1) || float.IsNaN(YPosition1) ||
				float.IsNaN(XPosition2) || float.IsNaN(YPosition2))
			{
				return false;
			}

			// Check line annotation hit - return immediately if true.
			if (ContainsPointInRotatedBounds(touchPoint, GetLineAnnotationBoundsCorners()))
			{
				return true;
			}

			// Check arrowhead hit and label hit - return result.
			return HitTestArrowHeadAndLabel(touchPoint);
		}

		internal bool HitTestArrowHeadAndLabel(Point touchPoint)
		{
			// Check arrowhead hit - return immediately if true.
			if (ContainsPointInArrowHead(touchPoint))
			{
				return true;
			}

			// Check label hit - return result.
			return ContainsPointInRotatedBounds(touchPoint, GetLabelBoundsCorners());
		}

		internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
		{
			ResetPosition();

			if (X1 == null || X2 == null || double.IsNaN(Y1) || double.IsNaN(Y2))
			{
				return;
			}

			var x2 = ChartUtils.ConvertToDouble(X2);
			var y2 = Y2;

			if (CoordinateUnit == ChartCoordinateUnit.Axis)
			{
				(x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
				(x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
			}

			XPosition1 = (float)x1;
			XPosition2 = (float)x2;
			YPosition1 = (float)y1;
			YPosition2 = (float)y2;

			Angle = (float)(Math.Atan2(YPosition2 - YPosition1, XPosition2 - XPosition1) * (180 / Math.PI));

			if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
			{
				LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
				double x = (LineCapPoints[1].X - LineCapPoints[2].X) / 2;
				double y = (LineCapPoints[1].Y - LineCapPoints[2].Y) / 2;

				XPosition2 = (float)(LineCapPoints[2].X + x);
				YPosition2 = (float)(LineCapPoints[2].Y + y);
			}

			RenderRect = new RectF(XPosition1, YPosition1, XPosition2 - XPosition1, YPosition2 - YPosition1);

			if (!string.IsNullOrEmpty(Text))
			{
				SetTextAlignment(XPosition1, YPosition1);
			}
		}

		internal override void SetTextAlignment(double x, double y)
		{
			if (_annotationLabelStyle == null)
			{
				return;
			}

			var labelSize = _annotationLabelStyle.MeasureLabel(Text);
			double halfBorderWidth = (_annotationLabelStyle.StrokeWidth / 2);
			double labelHeight = labelSize.Height;
			double labelWidth = labelSize.Width;
			double labelRectX = x;
			double labelRectY = y;
			double annotationX = RenderRect.X;
			double annotationY = RenderRect.Y;
			double annotationHeight = RenderRect.Height;
			double annotationWidth = RenderRect.Width;

			switch (_annotationLabelStyle.VerticalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectY = XPosition1 > XPosition2 ? annotationY + (annotationHeight / 2) + (labelHeight / 2) + halfBorderWidth - StrokeWidth : annotationY + (annotationHeight / 2) - (labelHeight / 2) - halfBorderWidth - StrokeWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectY = annotationY + (annotationHeight / 2) - (labelHeight / 2) - _annotationLabelStyle.Margin.Bottom - StrokeWidth;
#if ANDROID
					labelRectY = labelRectY - _annotationLabelStyle.Margin.Top - StrokeWidth;
#endif
					break;
				case ChartLabelAlignment.End:
					labelRectY = XPosition1 > XPosition2 ? annotationY + (annotationHeight / 2) - (labelHeight / 2) - halfBorderWidth - StrokeWidth : annotationY + (annotationHeight / 2) + (labelHeight / 2) + halfBorderWidth - StrokeWidth;
					break;
			}

			switch (_annotationLabelStyle.HorizontalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectX = XPosition1 > XPosition2 ? annotationX + halfBorderWidth + (labelWidth / 2) - labelWidth : annotationX + halfBorderWidth + (labelWidth / 2);
					break;
				case ChartLabelAlignment.Center:
					labelRectX = annotationX + (annotationWidth / 2);
					break;
				case ChartLabelAlignment.End:
					labelRectX = XPosition1 > XPosition2 ? annotationX + annotationWidth - halfBorderWidth - (labelWidth / 2) + labelWidth : annotationX + annotationWidth - halfBorderWidth - (labelWidth / 2);
					break;
			}

			LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
		}

		internal void SetHorizontalTextAlignment(double x, double y)
		{
			var labelSize = _annotationLabelStyle.MeasureLabel(Text);
			double halfBorderWidth = (float)_annotationLabelStyle.StrokeWidth / 2;
			double labelHeight = labelSize.Height, labelWidth = labelSize.Width;
			double labelRectX = x, labelRectY = y;
			double annotationX = RenderRect.X, annotationY = RenderRect.Y;
			double annotationHeight = RenderRect.Height, annotationWidth = RenderRect.Width;

			switch (_annotationLabelStyle.VerticalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectY = annotationY - (labelHeight / 2) - (annotationHeight / 2) - halfBorderWidth - StrokeWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectY = annotationY - StrokeWidth;
					break;
				case ChartLabelAlignment.End:
					labelRectY = annotationY + (labelHeight / 2) + (annotationHeight / 2) + halfBorderWidth - StrokeWidth;
					break;
			}

			switch (_annotationLabelStyle.HorizontalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectX = annotationX + (labelWidth / 2) + halfBorderWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectX = annotationX + (annotationWidth / 2);
					break;
				case ChartLabelAlignment.End:
					labelRectX = annotationX + annotationWidth - halfBorderWidth - (labelWidth / 2);
					break;
			}

			LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
		}

		internal void SetVerticalTextAlignment(double x, double y)
		{
			var labelSize = _annotationLabelStyle.MeasureLabel(Text);
			double halfBorderWidth = (float)(_annotationLabelStyle.StrokeWidth / 2);
			double labelHeight = labelSize.Height, labelWidth = labelSize.Width;
			double labelRectX = x, labelRectY = y;
			double annotationX = RenderRect.X, annotationY = RenderRect.Y;
			double annotationHeight = RenderRect.Height, annotationWidth = RenderRect.Width;

			switch (_annotationLabelStyle.VerticalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectY = annotationY - annotationHeight + (labelWidth / 2) + halfBorderWidth - StrokeWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectY = annotationY - (annotationHeight / 2) - StrokeWidth;
					break;
				case ChartLabelAlignment.End:
					labelRectY = annotationY - halfBorderWidth - (labelWidth / 2) - StrokeWidth;
					break;
			}

			switch (_annotationLabelStyle.HorizontalTextAlignment)
			{
				case ChartLabelAlignment.Start:
					labelRectX = annotationX - (annotationWidth / 2) - (labelHeight / 2) - halfBorderWidth;
					break;
				case ChartLabelAlignment.Center:
					labelRectX = annotationX;
					break;
				case ChartLabelAlignment.End:
					labelRectX = annotationX + (annotationWidth / 2) + (labelHeight / 2) + halfBorderWidth;
					break;
			}

			LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
		}

		internal void CalculatePosition(bool isVertical, ChartAxis xAxis)
		{
			if (xAxis != null)
			{
				if (!isVertical)
				{
					float x = XPosition2 > XPosition1 ? XPosition1 : XPosition2;
					float y = YPosition2 > YPosition1 ? YPosition1 : YPosition2;
					float actualWidth = XPosition2 > XPosition1 ? XPosition2 - XPosition1 : XPosition1 - XPosition2;

					if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
					{
						LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
						float arrowDifference = (float)(LineCapPoints[0].X - LineCapPoints[1].X);

						if (xAxis.IsOpposed())
						{
							if (_annotationLabelStyle.HorizontalTextAlignment == ChartLabelAlignment.Start)
							{
								x += Math.Abs(arrowDifference);
							}
						}
						else
						{
							actualWidth -= arrowDifference;
						}

						RenderRect = new Rect(x, y, actualWidth, (float)StrokeWidth);
						XPosition2 -= arrowDifference;
					}
					else
					{
						RenderRect = new Rect(x, y, actualWidth, (float)StrokeWidth);
					}
				}
				else
				{
					if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
					{
						LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
						float arrowDifference = (float)(LineCapPoints[0].Y - LineCapPoints[1].Y);

						if (xAxis.IsOpposed())
						{
							if (_annotationLabelStyle.VerticalTextAlignment == ChartLabelAlignment.End)
							{
								YPosition2 -= Math.Abs(arrowDifference);
							}

							RenderRect = new Rect(XPosition1, YPosition2, (float)StrokeWidth, YPosition2 - YPosition1);
						}
						else
						{
							RenderRect = new Rect(XPosition1, YPosition1, (float)StrokeWidth, YPosition1 - YPosition2 + arrowDifference);
							YPosition2 -= arrowDifference;
						}
					}
					else
					{
						if (xAxis.IsOpposed())
						{
							RenderRect = new Rect(XPosition1, YPosition2, (float)StrokeWidth, YPosition2 - YPosition1);
						}
						else
						{
							RenderRect = new Rect(XPosition1, YPosition1, (float)StrokeWidth, YPosition1 - YPosition2);
						}
					}
				}
			}
		}

		internal override void ResetPosition()
		{
			XPosition1 = XPosition2 = YPosition1 = YPosition2 = float.NaN;
			LineCapPoints.Clear();
			LabelRect = Rect.Zero;
		}

		internal static void OnAxisLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is LineAnnotation annotation)
			{
				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, annotation.Parent);

				if (oldValue is ChartLabelStyle style)
				{
					annotation.UnHookStylePropertyChanged(style);
				}

				if (newValue is ChartLabelStyle newStyle)
				{
					annotation._axisLabelStyle = newStyle;
					newStyle.Parent = annotation.Parent;
					SetInheritedBindingContext(newStyle, annotation.BindingContext);
					newStyle.PropertyChanged += annotation.Style_PropertyChanged;
				}
				else
				{
					ChartLabelStyle defaultStyle = new ChartLabelStyle() { FontSize = 14 };
					annotation._axisLabelStyle = defaultStyle;
					defaultStyle.Parent = annotation.Parent;
					SetInheritedBindingContext(defaultStyle, annotation.BindingContext);
					defaultStyle.PropertyChanged += annotation.Style_PropertyChanged;
				}

				annotation.UpdateLayout();
				annotation.Invalidate();
			}
		}

		internal override Brush GetDefaultFillColor()
		{
			return new SolidColorBrush(Color.FromArgb("#49454F"));
		}

		internal override Brush GetDefaultStrokeColor()
		{
			return new SolidColorBrush(Color.FromArgb("#49454F"));
		}

		#endregion

		#region Private Methods

		List<Point> CalculateArrowPoints(float xPosition1, float yPosition1, float xPosition2, float yPosition2)
		{
			var height = 10 + StrokeWidth;
			var width = 20 + StrokeWidth;
			var angle = Math.Atan2(yPosition2 - yPosition1, xPosition2 - xPosition1);
			var halfRadius = ((Math.PI * width / 180) / 2) * 3;

			var ax = xPosition2 - (height * Math.Cos(angle - halfRadius));
			var ay = yPosition2 - (height * Math.Sin(angle - halfRadius));
			var bx = xPosition2 - (height * Math.Cos(angle + halfRadius));
			var by = yPosition2 - (height * Math.Sin(angle + halfRadius));

			var points = new List<Point>
			{
				new(xPosition2, yPosition2),
				new(ax, ay),
				new(bx, by)
			};

			return points;
		}

		void Style_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (sender is ChartLabelStyle)
			{
				if (e.PropertyName == ChartLabelStyle.MarginProperty.PropertyName || e.PropertyName == ChartLabelStyle.FontSizeProperty.PropertyName || e.PropertyName == ChartLabelStyle.LabelFormatProperty.PropertyName)
				{
					UpdateLayout();
				}

				Invalidate();
			}
		}

		bool ContainsPointInRotatedBounds(Point touchPoint, PointF[] rectCorners)
		{
			if (rectCorners == null || rectCorners.Length != 4)
				return false;

			// Use the first corner as the origin; its two adjacent edges define the rectangle axes.
			var origin = rectCorners[0];
			var widthCorner = rectCorners[1];   // corner along the width direction from origin
			var heightCorner = rectCorners[3];  // corner along the height direction from origin

			// Edge vectors representing the rectangle’s local width and height directions.
			float widthVecX = widthCorner.X - origin.X;
			float widthVecY = widthCorner.Y - origin.Y;
			float heightVecX = heightCorner.X - origin.X;
			float heightVecY = heightCorner.Y - origin.Y;

			// Reject degenerate rectangles (zero or near-zero edge lengths).
			const float epsilon = 1e-4f;
			float widthLenSq = widthVecX * widthVecX + widthVecY * widthVecY;
			float heightLenSq = heightVecX * heightVecX + heightVecY * heightVecY;
			if (widthLenSq < epsilon || heightLenSq < epsilon)
				return false;

			// Vector from origin to the test point.
			float toPointX = (float)touchPoint.X - origin.X;
			float toPointY = (float)touchPoint.Y - origin.Y;

			// Project the point onto the width and height axes (dot products, no normalization needed).
			float projOnWidth = toPointX * widthVecX + toPointY * widthVecY;   // in “width-squared units”
			float projOnHeight = toPointX * heightVecX + toPointY * heightVecY; // in “height-squared units”

			// Inside if both projections lie within [0, |axis|^2], with a small tolerance.
			bool insideWidth = projOnWidth >= -epsilon && projOnWidth <= widthLenSq + epsilon;
			bool insideHeight = projOnHeight >= -epsilon && projOnHeight <= heightLenSq + epsilon;

			return insideWidth && insideHeight;
		}

		bool ContainsPointInArrowHead(Point p)
		{
			if (LineCapPoints.Count == 3)
			{
				Point a = LineCapPoints[0];
				Point b = LineCapPoints[1];
				Point c = LineCapPoints[2];
				double denominator = ((b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y));
				double alpha = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denominator;
				double beta = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denominator;
				double gamma = 1.0 - alpha - beta;
				return alpha >= 0 && beta >= 0 && gamma >= 0;
			}

			return false;
		}

		/// <summary>
		/// Computes the four corners of a rotated rectangle aligned with the line (X1,Y1)-(X2,Y2).
		/// <param name="halfThickness"> Distance from the line outward on both sides (half of the rectangle’s thickness).</param>
		/// <param name="shrinkAlongLineLengthBy"> Amount to reduce the rectangle’s length along the line (e.g., label width).</param>
		/// Returns corners arranged in sequence: LeftTop → RightTop → RightBottom → LeftBottom.
		/// </summary>
		PointF[] ComputeRotatedRectCorners(float halfThickness, float shrinkAlongLineLengthBy)
		{
			float x1 = (float)XPosition1;
			float y1 = (float)YPosition1;
			float x2 = (float)XPosition2;
			float y2 = (float)YPosition2;
			float dx = x2 - x1;
			float dy = y2 - y1;
			float length = MathF.Sqrt(dx * dx + dy * dy);
			// Degenerate segment: return an axis-aligned square around (x1,y1).
			if (length < 1e-3f)
			{
				return
				[
					new PointF(x1 - halfThickness, y1 - halfThickness),
					new PointF(x1 + halfThickness, y1 - halfThickness),
					new PointF(x1 + halfThickness, y1 + halfThickness),
					new PointF(x1 - halfThickness, y1 + halfThickness),
				];
			}

			float angle = MathF.Atan2(dy, dx);
			float midX = (x1 + x2) / 2f;
			float midY = (y1 + y2) / 2f;
			float halfLength = MathF.Max(0f, (length - shrinkAlongLineLengthBy) / 2f);

			// Local corners in (tangent x, normal y).
			// Order: LT, RT, RB, LB to match your usage.
			(float x, float y)[] raw =
			[
				(-halfLength, -halfThickness),
				( halfLength, -halfThickness),
				( halfLength,  halfThickness),
				(-halfLength,  halfThickness),
			];

			float cosA = MathF.Cos(angle);
			float sinA = MathF.Sin(angle);
			PointF[] rotatedCorners = new PointF[4];
			for (int i = 0; i < 4; i++)
			{
				float rawX = raw[i].x;
				float rawY = raw[i].y;
				float rotatedX = rawX * cosA - rawY * sinA + midX;
				float rotatedY = rawX * sinA + rawY * cosA + midY;
				rotatedCorners[i] = new PointF(rotatedX, rotatedY);
			}

			return rotatedCorners;
		}

		/// <summary>
		/// Computes the pure line annotation bounds corners, using half stroke width as thickness.
		/// </summary>
		PointF[] GetLineAnnotationBoundsCorners()
		{
			return ComputeRotatedRectCorners(
				(float)StrokeWidth / 2f,
				0f // Pure line bounds (no tangential shrink).
			);
		}

		PointF[] GetLabelBoundsCorners()
		{
			if (_annotationLabelStyle == null || string.IsNullOrEmpty(Text) || LabelRect.IsEmpty)
				return [];

			double centerX = LabelRect.X;
			double centerY = LabelRect.Y;
			double labelWidth = LabelRect.Width;
			double labelHeight = LabelRect.Height;
			// Border thickness (half) from the label style.
			float halfLabelBorderWidth = (float)(_annotationLabelStyle.StrokeWidth / 2.0);
			// Convert angle to radians for rotation math.
			float radians = (float)(Angle * Math.PI / 180.0);
			// Half dimensions, expanded to include the border.
			float halfWidth = (float)(labelWidth / 2.0) + halfLabelBorderWidth;
			float halfHeight = (float)(labelHeight / 2.0) + halfLabelBorderWidth;
			// Raw corners relative to center.
			var rawCorners = new (float x, float y)[]
			{
				(-halfWidth, -halfHeight),
				( halfWidth, -halfHeight),
				( halfWidth,  halfHeight),
				(-halfWidth,  halfHeight)
			};

			float cosA = MathF.Cos(radians);
			float sinA = MathF.Sin(radians);
			PointF[] rotatedCorners = new PointF[4];
			for (int i = 0; i < 4; i++)
			{
				float rx = rawCorners[i].x * cosA - rawCorners[i].y * sinA + (float)centerX;
				float ry = rawCorners[i].x * sinA + rawCorners[i].y * cosA + (float)centerY;
				rotatedCorners[i] = new PointF(rx, ry);
			}

			return rotatedCorners;
		}

		#endregion

		#endregion
	}
}
