namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the segment of the <see cref="BoxAndWhiskerSeries"/>.
	/// <para>This class is responsible for defining the bounds and key values (maximum, minimum, quartiles, median) used to render a segment in a box and whisker plot.</para>
	/// <para>The segment's appearance, including its stroke color and fill color, is derived from the associated series.</para>
	/// </summary>
	public partial class BoxAndWhiskerSegment : CartesianSegment
	{
		#region Fields

		#region Private Fields

		double _x1, _y1, _x2, _y2;
		double _average;
		readonly float _crossWidth = 10;
		readonly float _crossHeight = 10;
		readonly float _outlierHeight = 10;
		readonly float _outlierWidth = 10;
		float _showMedianPointX;
		float _showMedianPointY;
		float _midPoint;
		float _medianLinePointX;
		float _medianLinePointY;
		float _maximumLinePointX;
		float _maximumLinePointY;
		float _minimumLinePointX;
		float _minimumLinePointY;
		float _lowerQuartileLinePointX;
		float _lowerQuartileLinePointY;
		float _upperQuartileLinePointX;
		float _upperQuartileLinePointY;
		PointF _outlierPoints;
		readonly List<RectF> _outlierSegmentBounds;

		#endregion

		#region Internal Fields

		internal int _outlierIndex;
		internal List<double> _outliers;

		#endregion

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the maximum value for the box plot.
		/// </summary>
		public double Maximum { get; internal set; }

		/// <summary>
		/// Gets the minimum value for the box plot.
		/// </summary>
		public double Minimum { get; internal set; }

		/// <summary>
		/// Gets the median value for the box plot.
		/// </summary>
		public double Median { get; internal set; }

		/// <summary>
		/// Gets the lower quartile value for the box plot.
		/// </summary>
		public double LowerQuartile { get; internal set; }

		/// <summary>
		/// Gets the upper quartile value for the box plot.
		/// </summary>
		public double UpperQuartile { get; internal set; }

		/// <summary>
		/// Gets the left value for the box plot.
		/// </summary>
		public float Left { get; internal set; }

		/// <summary>
		/// Gets the right position value for the box plot.
		/// </summary>
		public float Right { get; internal set; }

		/// <summary>
		/// Gets the top position value for the box plot.
		/// </summary>
		public float Top { get; internal set; }

		/// <summary>
		/// Gets the bottom position value for the box plot.
		/// </summary>
		public float Bottom { get; internal set; }

		/// <summary>
		/// Gets the center position value for the box plot.
		/// </summary>
		public double Center { get; internal set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxAndWhiskerSegment"/> class.
		/// </summary>
		public BoxAndWhiskerSegment()
		{
			_outliers = [];
			_outlierSegmentBounds = [];
		}

		#endregion

		#region Methods

		#region Internal Override Methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the box plot segment.
		/// </summary>
		/// <param name="values"></param>
		internal override void SetData(double[] values)
		{
			if (Series is not BoxAndWhiskerSeries series)
			{
				return;
			}

			_x1 = values[0];
			_x2 = values[1];
			_y1 = values[2];
			_y2 = values[8];

			Minimum = values[3];
			LowerQuartile = values[4];
			Median = values[5];
			UpperQuartile = values[6];
			Maximum = values[7];
			Center = values[9];
			_average = values[10];

			series.XRange += new DoubleRange(_x1, _x2);
			series.YRange += new DoubleRange(_y1, _y2);
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			if (Series is not BoxAndWhiskerSeries series)
			{
				return -1;
			}

			bool horizontalTop = IsRectContains(Left, _maximumLinePointY, Right, _maximumLinePointY, x, y, (float)StrokeWidth);
			bool horizontalBottom = IsRectContains(Left, _minimumLinePointY, Right, _minimumLinePointY, x, y, (float)StrokeWidth);
			bool verticalTop = IsRectContains(_midPoint, _maximumLinePointY, _midPoint, _upperQuartileLinePointY, x, y, (float)StrokeWidth);
			bool verticalBottom = IsRectContains(_midPoint, _minimumLinePointY, _midPoint, _lowerQuartileLinePointY, x, y, (float)StrokeWidth);

			if (Series != null && (SegmentBounds.Contains(x, y) || horizontalTop || horizontalBottom || verticalTop || verticalBottom))
			{
				series.IsOutlierTouch = false;
				return Series._segments.IndexOf(this);
			}
			else if (Series != null && _outlierSegmentBounds.Count > 0)
			{
				for (int i = 0; i < _outlierSegmentBounds.Count; i++)
				{
					if (_outlierSegmentBounds[i].Contains(x, y))
					{
						_outlierIndex = i;
						series.IsOutlierTouch = true;
						return Series._segments.IndexOf(this);
					}
				}
			}

			return -1;
		}

		#endregion

		#region Protected  Internal Override Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is BoxAndWhiskerSeries series)
			{
				Layout(series);
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not BoxAndWhiskerSeries series)
			{
				return;
			}

			if (series.CanAnimate())
			{
				Layout(series);
			}

			if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
			{
				//Stroke Width is Zero it gives the width=1
				if (StrokeWidth <= 0)
				{
					canvas.StrokeSize = 1;
				}
				else
				{
					canvas.StrokeSize = (float)StrokeWidth;
				}

				canvas.StrokeColor = Stroke.ToColor();
				canvas.Alpha = Opacity;

				//Drawing segment.
				var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
				canvas.SetFillPaint(Fill, rect);
				canvas.FillRectangle(rect);

				// Draw the Rectangle
				canvas.DrawRectangle(rect);

				//Set ShowMedian=true ,the cross Mark will be drawn with respect to average value of segment.
				if (series.ShowMedian)
				{
					var rectMedian = new RectF(_showMedianPointX - (_crossWidth / 2), _showMedianPointY - (_crossHeight / 2), _crossWidth, _crossHeight);
					canvas.DrawShape(rectMedian, ShapeType.Cross, true, true);
				}

				//Set an IsTransposed=True Cartesian chart will be drawn in Horizontal.
				if (series.ChartArea?.IsTransposed is true)
				{
					canvas.DrawLine(_medianLinePointX, Top, _medianLinePointX, Bottom);
					canvas.DrawLine(_maximumLinePointX, Top, _maximumLinePointX, Bottom);
					canvas.DrawLine(_minimumLinePointX, Top, _minimumLinePointX, Bottom);
					float centerPoint = (Top + Bottom) / 2;
					canvas.DrawLine(_maximumLinePointX, centerPoint, _upperQuartileLinePointX, centerPoint);
					canvas.DrawLine(_minimumLinePointX, centerPoint, _lowerQuartileLinePointX, centerPoint);
				}
				else
				{
					//Draw the Median line
					canvas.DrawLine(Left, _medianLinePointY, Right, _medianLinePointY);

					//Draw the Maximum Line
					canvas.DrawLine(Left, _maximumLinePointY, Right, _maximumLinePointY);

					//Draw the Minimum Line 
					canvas.DrawLine(Left, _minimumLinePointY, Right, _minimumLinePointY);

					_midPoint = (Left + Right) / 2;

					//Draw the vertical Line over the UpperQuartile to Maximum point
					canvas.DrawLine(_midPoint, _maximumLinePointY, _midPoint, _upperQuartileLinePointY);

					//Draw the vertical Line over the LowerQuartile to Minimum point
					canvas.DrawLine(_midPoint, _minimumLinePointY, _midPoint, _lowerQuartileLinePointY);
				}

				//Drawing ellipse above or below in the Box and Whisker segment.                            
				if (_outlierSegmentBounds.Count > 0)
				{
					for (int i = 0; i < _outlierSegmentBounds.Count; i++)
					{
						canvas.SetFillPaint(Fill, _outlierSegmentBounds[i]);
						canvas.DrawShape(_outlierSegmentBounds[i], series.OutlierShapeType, true, false);
					}
				}

			}
		}

		#endregion

		#region Private methods

		void Layout(BoxAndWhiskerSeries? series)
		{
			var xAxis = series?.ActualXAxis;

			if (series == null || series.ChartArea == null || xAxis == null)
			{
				return;
			}

			var start = Math.Floor(xAxis.VisibleRange.Start);
			var end = Math.Ceiling(xAxis.VisibleRange.End);

			Left = Top = Bottom = Right = float.NaN;

			if (_x1 <= end && _x2 >= start)
			{
				Left = series.TransformToVisibleX(_x1, LowerQuartile);
				Top = series.TransformToVisibleY(_x1, LowerQuartile);
				Right = series.TransformToVisibleX(_x2, UpperQuartile);
				Bottom = series.TransformToVisibleY(_x2, UpperQuartile);

				_medianLinePointX = series.TransformToVisibleX(_x1, Median);
				_medianLinePointY = series.TransformToVisibleY(_x2, Median);

				_maximumLinePointX = series.TransformToVisibleX(_x1, Maximum);
				_maximumLinePointY = series.TransformToVisibleY(_x2, Maximum);

				_minimumLinePointX = series.TransformToVisibleX(_x1, Minimum);
				_minimumLinePointY = series.TransformToVisibleY(_x2, Minimum);

				_upperQuartileLinePointX = series.TransformToVisibleX(_x1, UpperQuartile);
				_upperQuartileLinePointY = series.TransformToVisibleY(_x2, UpperQuartile);

				_lowerQuartileLinePointX = series.TransformToVisibleX(_x1, LowerQuartile);
				_lowerQuartileLinePointY = series.TransformToVisibleY(_x2, LowerQuartile);

				_showMedianPointX = series.TransformToVisibleX(Center, _average);
				_showMedianPointY = series.TransformToVisibleY(Center, _average);

				//Calculated animation values for column and line segment
				float MedianPoint = ((Top + Bottom) / 2) - Top;
				Top += MedianPoint * (1 - series.AnimationValue);
				Bottom -= MedianPoint * (1 - series.AnimationValue);

				float MedianLinePoint = ((_maximumLinePointY + _minimumLinePointY) / 2) - _maximumLinePointY;
				_maximumLinePointY += MedianLinePoint * (1 - series.AnimationValue);
				_minimumLinePointY -= MedianLinePoint * (1 - series.AnimationValue);

				_upperQuartileLinePointY -= (MedianPoint * (1 - series.AnimationValue));
				_lowerQuartileLinePointY += (MedianPoint * (1 - series.AnimationValue));
				_maximumLinePointY += (MedianPoint / 6) * (1 - series.AnimationValue);

				if (!series.CanAnimate() && _outliers.Count > 0)
				{
					_outlierSegmentBounds.Clear();

					for (int i = 0; i < _outliers.Count; i++)
					{
						_outlierPoints = new PointF(series.TransformToVisibleX(Center, _outliers[i]), series.TransformToVisibleY(Center, _outliers[i]));
						var rectF = new RectF(_outlierPoints.X - (_outlierHeight / 2), _outlierPoints.Y - (_outlierWidth / 2), _outlierWidth, _outlierHeight);
						_outlierSegmentBounds.Add(rectF);
					}
				}

				if (Left > Right)
				{
					(Right, Left) = (Left, Right);
				}

				if (Top > Bottom)
				{
					(Bottom, Top) = (Top, Bottom);
				}
			}
			else
			{
				Left = float.NaN;
			}

			SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
		}

		#endregion

		#endregion
	}
}
