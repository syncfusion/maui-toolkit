using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="SfPyramidChart"/>, responsible for rendering and customizing the appearance of individual pyramid segments.
	/// </summary>
	public partial class PyramidSegment : ChartSegment, IPyramidLabels
	{
		#region Fields
		internal IPyramidChartDependent? Chart { get; set; }
		RectF _actualBounds;
		float[] _values = new float[8];
		#endregion

		#region Properties
		/// <summary>
		/// Represents the draw points of the pyramid segments.
		/// </summary>
		/// <remarks>
		/// <para>Pyramid segments are often drawn with four points, which are listed below.</para>
		/// Points[0] : Top Left
		/// Points[1] : Top Right
		/// Points[2] : Bottom Right
		/// Points[3] : Bottom Left
		/// </remarks>
		public Point[] Points { get; internal set; } = new Point[4];

		double _yValue;
		double _height;

		double _dataLabelYValue;
		object? _dataLabelXValue;
		Rect _labelRect;
		float _labelXPosition;
		float _labelYPosition;
		Point[]? _linePoints = new Point[3];
		bool _isIntersected = false;
		bool _isLabelVisible = true;
		Size _dataLabelSize = Size.Zero;
		Size _actualSize = Size.Zero;
		Point _slopeCenter = Point.Zero;
		DataLabelPlacement _position = DataLabelPlacement.Auto;
		float IPyramidLabels.DataLabelX { get => _labelXPosition; set => _labelXPosition = value; }
		float IPyramidLabels.DataLabelY { get => _labelYPosition; set => _labelYPosition = value; }
		Point[]? IPyramidLabels.LinePoints { get => _linePoints; set => _linePoints = value; }
		Rect IPyramidLabels.LabelRect { get => _labelRect; set => _labelRect = value; }
		Size IPyramidLabels.DataLabelSize { get => _dataLabelSize; set => _dataLabelSize = value; }
		Size IPyramidLabels.ActualLabelSize { get => _actualSize; set => _actualSize = value; }
		string IPyramidLabels.DataLabel => LabelContent ?? string.Empty;
		bool IPyramidLabels.IsEmpty { get => Empty; set => Empty = value; }
		bool IPyramidLabels.IsIntersected { get => _isIntersected; set => _isIntersected = value; }
		bool IPyramidLabels.IsLabelVisible { get => _isLabelVisible; set => _isLabelVisible = value; }
		Brush IPyramidLabels.Fill => Fill ?? Brush.Transparent;
		Point IPyramidLabels.SlopePoint => _slopeCenter;
		DataLabelPlacement IPyramidLabels.Position { get => _position; set => _position = value; }

		#endregion

		#region Methods

		#region Internal Methods

		internal double Y => _yValue;
		internal double Height => _height;

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the pyramid segment.
		/// </summary>
		internal void SetData(double x, double y, object? xVal, double yVal)
		{
			_yValue = x;
			_height = y;
			_dataLabelYValue = yVal;
			_dataLabelXValue = xVal;
			Empty = double.IsNaN(yVal) || yVal == 0;
		}

		/// <summary>
		/// Gets the data point index for the specified pointer position in the pyramid segment.
		/// </summary>
		internal override int GetDataPointIndex(float x, float y)
		{
			if (Chart != null && Chart.IsHorizontalOrientation)
			{
				return GetHorizontalDataPointIndex(x, y);
			}

			return GetVerticalDataPointIndex(x, y);
		}

		int GetVerticalDataPointIndex(float x, float y)
		{
			if (SegmentBounds.Contains(x, y))
			{
				var slopeLeft = Math.Atan((_values[6] - _values[0]) / (_values[7] - _values[1]));
				var slopeRight = Math.Atan((_values[4] - _values[2]) / (_values[5] - _values[3]));
				if ((Math.Atan((_values[6] - x) / (_values[7] - y)) <= slopeLeft && slopeRight <= (Math.Atan((_values[4] - x) / (_values[5] - y)))))
				{
					return Index;
				}
			}

			return -1;
		}

		int GetHorizontalDataPointIndex(float x, float y)
		{
			if (SegmentBounds.Contains(x, y))
			{
				const float tolerance = 0.001f;
				float leftX = _values[0];
				float rightX = _values[4];

				if (Math.Abs(rightX - leftX) <= tolerance)
				{
					float minY = Math.Min(_values[1], _values[3]);
					float maxY = Math.Max(_values[1], _values[3]);
					return (x >= leftX - tolerance && x <= rightX + tolerance &&
							y >= minY - tolerance && y <= maxY + tolerance) ? Index : -1;
				}

				float normalizedX = (x - leftX) / (rightX - leftX);
				float topEdgeY = _values[1] + normalizedX * (_values[7] - _values[1]);
				float bottomEdgeY = _values[3] + normalizedX * (_values[5] - _values[3]);

				return (y >= topEdgeY - tolerance && y <= bottomEdgeY + tolerance) ? Index : -1;
			}

			return -1;
		}

		/// <summary>
		/// Calculates the segment points based on orientation in pyramid chart.
		/// </summary>
		void CalculateSegmentPoints(bool _isHorizontal)
		{
			if (_isHorizontal)
			{
				CalculateHorizontalSegmentPoints();
			}
			else
			{
				CalculateVerticalSegmentPoints();
			}
		}

		/// <summary>
		/// Calculates the segment points for the vertical pyramid implementation.
		/// </summary>
		void CalculateVerticalSegmentPoints()
		{
			double boundsHeight = _actualBounds.Height;
			double boundsWidth = _actualBounds.Width;
			double boundsTop = _actualBounds.Top;
			double boundsLeft = _actualBounds.Left;
			float top = (float)_yValue;
			float bottom = (float)(_yValue + _height);
			float topRadius = 0.5f * (1 - (float)_yValue);
			float bottomRadius = 0.5f * (1 - bottom);

			_values = new float[8];
			_values[0] = (float)(boundsLeft + (topRadius * boundsWidth));
			_values[1] = _values[3] = (float)(boundsTop + (top * boundsHeight));
			_values[2] = (float)(boundsLeft + ((1 - topRadius) * boundsWidth));
			_values[4] = (float)(boundsLeft + ((1 - bottomRadius) * boundsWidth));
			_values[5] = _values[7] = (float)(boundsTop + (bottom * boundsHeight));
			_values[6] = (float)(boundsLeft + (bottomRadius * boundsWidth));

			Points[0] = new Point(_values[0], _values[1]);
			Points[1] = new Point(_values[2], _values[3]);
			Points[2] = new Point(_values[4], _values[5]);
			Points[3] = new Point(_values[6], _values[7]);

			SegmentBounds = new RectF(_values[6], _values[1], _values[4] - _values[6], _values[5] - _values[1]);
		}

		/// <summary>
		/// Calculates the segment points for the horizontal pyramid implementation.
		/// </summary>
		void CalculateHorizontalSegmentPoints()
		{
			double boundsHeight = _actualBounds.Height;
			double boundsWidth = _actualBounds.Width;
			double boundsTop = _actualBounds.Top;
			double boundsLeft = _actualBounds.Left;


			float start = (float)_yValue;
			float end = (float)(_yValue + _height);

			float left = 1f - end;
			float right = 1f - start;

			float leftRadius = 0.5f * (1f - left);
			float rightRadius = 0.5f * (1f - right);

			_values = new float[8];
			_values[0] = _values[2] = (float)(boundsLeft + (left * boundsWidth));
			_values[4] = _values[6] = (float)(boundsLeft + (right * boundsWidth));

			_values[1] = (float)(boundsTop + ((0.5f - leftRadius) * boundsHeight));
			_values[3] = (float)(boundsTop + ((0.5f + leftRadius) * boundsHeight));
			_values[7] = (float)(boundsTop + ((0.5f - rightRadius) * boundsHeight));
			_values[5] = (float)(boundsTop + ((0.5f + rightRadius) * boundsHeight));

			Points[0] = new Point(_values[0], _values[1]);
			Points[1] = new Point(_values[2], _values[3]);
			Points[2] = new Point(_values[4], _values[5]);
			Points[3] = new Point(_values[6], _values[7]);

			SegmentBounds = new RectF(_values[0], _values[1], _values[4] - _values[0], _values[3] - _values[1]);
		}

		#endregion

		#region Data Label

		/// <summary>
		/// Arranges the data label layout based on orientation for the pyramid segment.
		/// </summary>
		internal override void OnDataLabelLayout()
		{
			if (Chart == null || Empty || Chart.DataLabelSettings.LabelStyle is not ChartDataLabelStyle style)
			{
				return;
			}

			var bounds = Chart.SeriesBounds;

			_labelXPosition = (float)bounds.X + SegmentBounds.Center.X;
			_labelYPosition = (float)bounds.Y + SegmentBounds.Center.Y;

			LabelContent = Chart.DataLabelSettings.GetLabelContent(_dataLabelXValue, _dataLabelYValue);

			if (Chart.IsHorizontalOrientation)
			{
				ArrangeHorizontalDataLabel(style, bounds);
			}
			else
			{
				ArrangeVerticalDataLabel(style, bounds);
			}

			Chart.DataLabelHelper.AddLabel(this);
		}

		/// <summary>
		/// Arranges the data label layout for the vertical pyramid segment.
		/// </summary>
		void ArrangeVerticalDataLabel(ChartDataLabelStyle style, Rect bounds)
		{

			var left = ((_values[0] + _values[6]) / 2) + bounds.Left;
			var right = ((_values[2] + _values[4]) / 2) + bounds.Left;
			var y = ((_values[3] + _values[5]) / 2) + bounds.Top;
			_slopeCenter = new Point(right - 3, y);


			UpdateDataLabels();

			_dataLabelSize = Chart!.LabelTemplate is null ? LabelContent!.Measure(style) : CalculateTemplateSize();
			_actualSize = new Size(_dataLabelSize.Width + style.Margin.HorizontalThickness, _dataLabelSize.Height + style.Margin.VerticalThickness);
			_position = DataLabelPlacement.Inner;
			switch (Chart.DataLabelSettings.LabelPlacement)
			{
				case DataLabelPlacement.Center:
				case DataLabelPlacement.Auto:
					if (_actualSize.Width > right - left || _actualSize.Height > SegmentBounds.Height)
						this._position = DataLabelPlacement.Outer;
					break;
				case DataLabelPlacement.Outer:
					this._position = DataLabelPlacement.Outer;
					break;
				default:
					this._position = DataLabelPlacement.Inner;
					break;
			}

			Chart.DataLabelHelper.AddLabel(this);
		}

		/// <summary>
		/// Arranges the data label layout for the horizontal pyramid segment.
		/// </summary>
		void ArrangeHorizontalDataLabel(ChartDataLabelStyle style, Rect bounds)
		{
			_slopeCenter = new Point(
			bounds.X + SegmentBounds.X + SegmentBounds.Width * 0.5,
			bounds.Y + SegmentBounds.Y + SegmentBounds.Height * 0.5);
			;


			UpdateDataLabels();

			_dataLabelSize = Chart!.LabelTemplate is null ? LabelContent!.Measure(style) : CalculateTemplateSize();
			_actualSize = new Size(_dataLabelSize.Width + style.Margin.HorizontalThickness, _dataLabelSize.Height + style.Margin.VerticalThickness);
			_position = DataLabelPlacement.Inner;
			switch (Chart.DataLabelSettings.LabelPlacement)
			{
				case DataLabelPlacement.Center:
				case DataLabelPlacement.Auto:
					if (_actualSize.Width > SegmentBounds.Width * 0.70 ||
						_actualSize.Height > SegmentBounds.Height * 0.70)
						_position = DataLabelPlacement.Outer;
					break;
				case DataLabelPlacement.Outer:
					_position = DataLabelPlacement.Outer;
					break;
				default:
					_position = DataLabelPlacement.Inner;
					break;
			}
		}

		/// <summary>
		/// Updates the data labels with current settings and values.
		/// </summary>
		internal override void UpdateDataLabels()
		{
			var dataLabelSettings = Chart?.DataLabelSettings;

			if (Chart == null || dataLabelSettings == null)
			{
				return;
			}

			if (DataLabels != null && DataLabels.Count > 0)
			{
				ChartDataLabel dataLabel = DataLabels[0];

				dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
				dataLabel.Background = dataLabelSettings.LabelStyle.Background;
				dataLabel.Index = Index;
				dataLabel.Item = Item;
				dataLabel.Label = LabelContent ?? string.Empty;
			}
		}

		/// <summary>
		/// Calculates the size of a template-based data label.
		/// </summary>
		Size CalculateTemplateSize()
		{
			if (Chart is SfPyramidChart pyramidChart)
			{
				if (pyramidChart.LabelTemplateView != null && pyramidChart.LabelTemplateView.Any())
				{
					if (pyramidChart.LabelTemplateView.Cast<View>().FirstOrDefault(child => DataLabels[0] == child.BindingContext) is DataLabelItemView templateView && templateView.ContentView is View content)
					{
						if (!content.DesiredSize.IsZero)
						{
							return content.DesiredSize;
						}

#if NET9_0_OR_GREATER
                        var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

                        if (desiredSize.IsZero)
						{
							return content.Measure(double.PositiveInfinity, double.PositiveInfinity);
						}
#else
						var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

						if (desiredSize.IsZero)
						{
							return content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
						}
#endif

						return desiredSize;
					}
				}
			}

			return SizeF.Zero;
		}

		#endregion

		#region Protected Internal Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Chart != null)
			{
				_actualBounds = new RectF(new Point(0, 0), Chart.SeriesBounds.Size);
				CalculateSegmentPoints(Chart.IsHorizontalOrientation);
			}
		}

		/// <summary>
		/// Draws the pyramid segment on the canvas with fill and optional stroke.
		/// </summary>
		protected internal override void Draw(ICanvas canvas)
		{
			PathF path = new PathF();
			path.MoveTo(Points[0]);
			for (int i = 1; i < Points.Count(); i++)
			{
				path.LineTo(Points[i]);
			}

			path.Close();
			canvas.SetFillPaint(Fill, path.Bounds);
			canvas.FillPath(path);

			if (HasStroke)
			{
				canvas.StrokeColor = Stroke.ToColor();
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.DrawPath(path);
			}
		}

		#endregion

		#endregion
	}
}