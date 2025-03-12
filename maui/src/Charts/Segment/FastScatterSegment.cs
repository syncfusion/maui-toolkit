namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="FastScatterSeries"/> chart, responsible for drawing the scatter and managing its visual appearance.
	/// </summary>
	public partial class FastScatterSegment : CartesianSegment
	{
		#region Fields

		float _preXPos;
		float _preYPos;
		float _preXValue;
		float _preYValue;
		readonly List<PointF> _fastScatterPlottingPoints = [];
		RectF _actualRectF;

		#endregion

		#region Internal Properties

		internal List<double>? XValues { get; set; }
		internal IList<double>? YValues { get; set; }

		#endregion

		#region Methods

		#region Internal Methods

		internal void SetData(List<double> xValues, IList<double> yValues)
		{
			if (Series is XYDataSeries series)
			{
				// Initialize min/max values
				double xMin = double.MaxValue, xMax = double.MinValue, yMin = double.MaxValue, yMax = double.MinValue;
				XValues = xValues;
				YValues = yValues;
				int dataCount = series.PointsCount;

				if (dataCount == 0)
				{
					return;
				}

				// Adjust for indexed series by setting xMin/xMax
				if (series.IsIndexed && dataCount > 0)
				{
					xMin = XValues[0];
					xMax = XValues[dataCount - 1];
				}

				for (int i = 0; i < dataCount; i++)
				{
					// Ensure we are in range for both lists
					if (i >= XValues.Count)
					{
						break;
					}

					double xValue = XValues[i];
					double yValue = YValues[i];

					// Mark as empty if any NaN values are encountered
					if (double.IsNaN(xValue) || double.IsNaN(yValue))
					{						
						continue;
					}

					if (!series.IsIndexed)
					{
						// Use Math.Min/Max for cleaner comparisons
						xMin = Math.Min(xMin, xValue);
						xMax = Math.Max(xMax, xValue);
					}

					yMin = Math.Min(yMin, yValue);
					yMax = Math.Max(yMax, yValue);
				}

				// Handle no valid entries scenario by setting NaNs
				if (xMin == double.MaxValue)
				{
					xMin = double.NaN;
				}

				if (xMax == double.MinValue)
				{
					xMax = double.NaN;
				}

				if (yMin == double.MaxValue)
				{
					yMin = double.NaN;
				}

				if (yMax == double.MinValue)
				{
					yMax = double.NaN;
				}

				// Update series range
				Series.XRange += new DoubleRange(xMin, xMax);
				Series.YRange += new DoubleRange(yMin, yMax);
			}
		}

		#endregion

		#region Override Methods
		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is FastScatterSeries series && series.ActualXAxis is ChartAxis xAxis && series.ActualYAxis is ChartAxis yAxis && XValues is not null && YValues is not null)
			{
				int dataCount = series.PointsCount;

				var _xRange = xAxis.VisibleRange;
				var _yRange = yAxis.VisibleRange;

				double xStart = _xRange.Start;
				double xEnd = _xRange.End;

				double yStart = _yRange.Start;
				double yEnd = _yRange.End;

				_preXValue = (float)XValues[0];
				_preYValue = (float)YValues[0];

				_preXPos = series.TransformToVisibleX(_preXValue, _preYValue);
				_preYPos = series.TransformToVisibleY(_preXValue, _preYValue);

				_fastScatterPlottingPoints.Clear();

				if (!series.IsIndexed)
				{
					for (int i = 1; i < dataCount; i++)
					{
						double xValue = XValues[i];
						double yValue = YValues[i];

						if (xEnd <= xValue && xStart >= XValues[i - 1])
						{
							float x = series.TransformToVisibleX(xValue, yValue);
							float y = series.TransformToVisibleY(xValue, yValue);
							_preXPos = series.TransformToVisibleX(XValues[i - 1], YValues[i - 1]);
							_preYPos = series.TransformToVisibleY(XValues[i - 1], YValues[i - 1]);

							_fastScatterPlottingPoints.Add(new PointF(_preXPos, _preYPos));

							_preXPos = x;
							_preYPos = y;
							_preXValue = (float)xValue;
							_preYValue = (float)yValue;
						}
						else if ((xValue <= xEnd && xValue >= xStart) || (yValue >= yStart && yValue <= yEnd))
						{
							float x = series.TransformToVisibleX(xValue, yValue);
							float y = series.TransformToVisibleY(xValue, yValue);

							_fastScatterPlottingPoints.Add(new PointF(_preXPos, _preYPos));

							_preXPos = x;
							_preYPos = y;
							_preXValue = (float)xValue;
							_preYValue = (float)yValue;
						}
					}
				}
				else
				{
					for (int i = 1; i < dataCount; i++)
					{
						double yValue = YValues[i];

						if ((i <= xEnd + 1) && (i >= xStart - 1))
						{
							float x = series.TransformToVisibleX(i, yValue);
							float y = series.TransformToVisibleY(i, yValue);

							_fastScatterPlottingPoints.Add(new PointF(_preXPos, _preYPos));

							_preXPos = x;
							_preYPos = y;
							_preXValue = (float)i;
							_preYValue = (float)yValue;
						}
					}
				}

				if (_fastScatterPlottingPoints.Count != dataCount)
				{
					float lastX = series.TransformToVisibleX(XValues[dataCount - 1], YValues[dataCount - 1]);
					float lastY = series.TransformToVisibleY(XValues[dataCount - 1], YValues[dataCount - 1]);
					_fastScatterPlottingPoints.Add(new PointF(lastX, lastY));
				}
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is FastScatterSeries series)
			{
				float scatterHeight = (float)series.PointHeight;
				float scatterWidth = (float)series.PointWidth;

				float halfHeight = scatterHeight / 2;
				float halfWidth = scatterWidth / 2;

				if (HasStroke)
				{
					canvas.StrokeColor = Stroke.ToColor();
					canvas.StrokeSize = (float)StrokeWidth;
				}

				if (series.Type == ShapeType.Circle)
				{
					foreach (var point in _fastScatterPlottingPoints)
					{
						if((double.IsNaN(point.Y) || double.IsNaN(point.X)))
						{
							continue;
						}

						_actualRectF = new RectF((float)point.X - halfWidth, point.Y - halfHeight, scatterWidth, scatterHeight);

						canvas.SetFillPaint(Fill, _actualRectF);
						canvas.FillEllipse(_actualRectF);

						if (HasStroke)
						{
							canvas.DrawEllipse(_actualRectF);
						}
					}
				}
				else
				{
					foreach (var point in _fastScatterPlottingPoints)
					{
						_actualRectF = new RectF((float)point.X - halfWidth, point.Y - halfHeight, scatterWidth, scatterHeight);

						canvas.SetFillPaint(Fill, _actualRectF);

						canvas.DrawShape(_actualRectF, series.Type, HasStroke, false);
					}

				}
			}
		}

		#endregion
		#endregion
	}
}
