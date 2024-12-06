namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class LogarithmicAxis : RangeAxisBase
	{
		#region Methods

		#region Override Methods

		///<inheritdoc/>
		protected override DoubleRange CalculateActualRange()
		{
			if (double.IsNaN(DefaultMinimum) && double.IsNaN(DefaultMaximum))
			{
				return CalculateLogRange(base.CalculateActualRange());
			}
			else if (!double.IsNaN(DefaultMinimum) && !double.IsNaN(DefaultMaximum))
			{
				var minimumValue = DefaultMinimum > 0 ? DefaultMinimum : 1;
				VisibleLogRange = new DoubleRange(GetLogValue(minimumValue), GetLogValue(DefaultMaximum));
				return new DoubleRange(minimumValue, DefaultMaximum);
			}
			else
			{
				DoubleRange range = CalculateLogRange(base.CalculateActualRange());

				if (!double.IsNaN(DefaultMinimum))
				{
					var minimumValue = DefaultMinimum > 0 ? DefaultMinimum : 1;
					VisibleLogRange = new DoubleRange(GetLogValue(minimumValue), GetLogValue(range.End));
					return new DoubleRange(minimumValue, range.End);
				}

				if (!double.IsNaN(DefaultMaximum))
				{
					VisibleLogRange = new DoubleRange(GetLogValue(range.Start), GetLogValue(DefaultMaximum));
					return new DoubleRange(range.Start, DefaultMaximum);
				}
			}

			return ActualRange;
		}

		///<inheritdoc/>
		protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
		{
			if (double.IsNaN(AxisInterval) || AxisInterval == 0)
			{
				if (RegisteredSeries.Count == 0 && (double.IsNaN(DefaultMinimum) || double.IsNaN(DefaultMaximum)))
				{
					VisibleLogRange = range;
				}

				return CalculateNiceInterval(VisibleLogRange, availableSize);
			}

			return AxisInterval;
		}

		///<inheritdoc/>
		protected override double CalculateNiceInterval(DoubleRange actualRange, Size availableSize)
		{
			var delta = VisibleLogRange.Delta;
			var actualDesiredIntervalsCount = GetActualDesiredIntervalsCount(availableSize);
			var niceInterval = delta / actualDesiredIntervalsCount;
			var minInterval = Math.Pow(10, Math.Floor(Math.Log10(niceInterval)));

			foreach (int val in IntervalDivs)
			{
				double currentInterval = minInterval * val;

				if (actualDesiredIntervalsCount < (delta / currentInterval))
				{
					break;
				}

				niceInterval = currentInterval;
			}

			return niceInterval >= 1 ? niceInterval : 1;
		}

		///<inheritdoc/>
		protected override DoubleRange CalculateVisibleRange(DoubleRange actualRange, Size availableSize)
		{
			VisibleRange = actualRange;
			VisibleInterval = ActualInterval;

			if (ZoomFactor < 1)
			{
				//Modified the visible range calculation for fixing the data labels hiding issue while zooming interactions. 

				DoubleRange baseRange = VisibleLogRange;
				double start = baseRange.Start + (ZoomPosition * baseRange.Delta);
				double end = start + (ZoomFactor * baseRange.Delta);

				if (start < baseRange.Start)
				{
					end += (baseRange.Start - start);
					start = baseRange.Start;
				}

				if (end > baseRange.End)
				{
					start -= (end - baseRange.End);
					end = baseRange.End;
				}

				VisibleLogRange = new DoubleRange(start, end);
				VisibleRange = new DoubleRange(GetPowValue(start), GetPowValue(end));
			}

			return VisibleRange;
		}

		internal override void UpdateAxisScale()
		{
			var actualLogRange = new DoubleRange(GetLogValue(ActualRange.Start), GetLogValue(ActualRange.End));

			ZoomPosition = (VisibleLogRange.Start - actualLogRange.Start) / actualLogRange.Delta;
			ZoomFactor = (VisibleLogRange.End - VisibleLogRange.Start) / actualLogRange.Delta;
		}

		internal override DoubleRange AddDefaultRange(double start)
		{
			return new DoubleRange(start, start + 1);
		}

		internal override void GenerateVisibleLabels()
		{
			if (VisibleRange.IsEmpty)
			{
				return;
			}

			_isOverriddenOnCreateLabelsMethod = ChartUtils.IsOverriddenMethod(this, "OnCreateLabels");

			var actualLabels = VisibleLabels;

			SmallTickPoints.Clear();

			double position, interval = VisibleInterval;
			var edgeLabelVisibility = EdgeLabelsVisibilityMode;
			var isEdgeLabelAlwaysVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.AlwaysVisible;
			var isEdgeLabelVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.Visible && ZoomFactor.Equals(1.0f);

			if (isEdgeLabelAlwaysVisible || isEdgeLabelVisible || (!double.IsNaN(DefaultMinimum) && !double.IsNaN(AxisInterval) && AxisInterval != 0))
			{
				position = VisibleLogRange.Start;
			}
			else
			{
				position = VisibleLogRange.Start - (VisibleLogRange.Start % ActualInterval);
			}

			var end = Math.Round(VisibleLogRange.End, 3);

			var labelFormat = LabelStyle.LabelFormat;

			while (position <= end)
			{
				if (VisibleLogRange.Start <= position && position <= end)
				{
					var labelContent = "";

					var powValue = GetPowValue(position);

					if (labelFormat != null)
					{
						labelContent = GetActualLabelContent(powValue, labelFormat);
					}

					var axisLabel = new ChartAxisLabel(powValue, labelContent);

					actualLabels.Add(axisLabel);

					if (labelContent.ToString() == axisLabel?.Content?.ToString())
					{
						var labelStyle = axisLabel.LabelStyle;

						if (labelStyle != null)
						{
							if (labelStyle.LabelFormat != null)
							{
								axisLabel.Content = GetActualLabelContent(powValue, labelStyle.LabelFormat);
							}
						}
					}

					if (!_isOverriddenOnCreateLabelsMethod)
					{
						TickPositions.Add(powValue);
						if (MinorTicksPerInterval > 0)
						{
							AddSmallTicksPoint(position, interval);
						}
					}

				}
				position += interval;
			}

			if (isEdgeLabelAlwaysVisible || isEdgeLabelVisible)
			{
				var labelContent = "";

				var powValue = GetPowValue(VisibleLogRange.End);

				if (labelFormat != null)
				{
					labelContent = GetActualLabelContent(powValue, labelFormat);
				}

				var axisLabel = new ChartAxisLabel(powValue, labelContent);
				actualLabels.Add(axisLabel);

				if (labelContent.ToString() == axisLabel.Content?.ToString())
				{
					var labelStyle = axisLabel.LabelStyle;

					if (labelStyle != null)
					{
						if (labelStyle.LabelFormat != null)
						{
							axisLabel.Content = GetActualLabelContent(powValue, labelStyle.LabelFormat);
						}
					}
				}

				if (!_isOverriddenOnCreateLabelsMethod)
				{
					TickPositions.Add(powValue);
				}

			}

			if (_isOverriddenOnCreateLabelsMethod)
			{
				OnCreateLabels();
				AddVisibleLabels();
			}
		}

		internal override float ValueToCoefficient(double value)
		{
			if (value <= 0)
			{
				return 0f;
			}

			value = GetLogValue(value);
			double result;
			double start = VisibleLogRange.Start;
			double delta = VisibleLogRange.Delta;

			result = (value - start) / delta;

			return (float)(IsInversed ? 1f - result : result);
		}

		internal override double CoefficientToValue(double coefficient)
		{
			double value;
			coefficient = IsInversed ? 1d - coefficient : coefficient;
			value = VisibleLogRange.Start + (VisibleLogRange.Delta * coefficient);
			return GetPowValue(value);
		}

		internal override void AddSmallTicksPoint(double position, double interval)
		{
			double logTickStart = GetPowValue(position - VisibleInterval);
			double logTickEnd = GetPowValue(position);
			double logTickInterval = (logTickEnd - logTickStart) / (MinorTicksPerInterval + 1);
			double logTickPos = logTickStart + logTickInterval;
			double logSmallTick = GetLogValue(logTickPos);

			while ((logTickPos < logTickEnd))
			{
				if (VisibleLogRange.Inside(logSmallTick))
				{
					SmallTickPoints.Add(GetPowValue(logSmallTick));
				}

				logTickPos += logTickInterval;
				logSmallTick = GetLogValue(logTickPos);
			}
		}

		internal override void UpdateAutoScrollingDelta(DoubleRange actualRange, double scrollingDelta)
		{
			if (double.IsNaN(scrollingDelta))
			{
				return;
			}

			switch (AutoScrollingMode)
			{
				case ChartAutoScrollingMode.Start:
					VisibleRange = new DoubleRange(ActualRange.Start, GetPowValue(scrollingDelta));
					VisibleLogRange = new DoubleRange(GetLogValue(VisibleRange.Start), GetLogValue(VisibleRange.End));
					ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
					ZoomPosition = 0;
					break;
				case ChartAutoScrollingMode.End:
					VisibleRange = new DoubleRange(ActualRange.End / GetPowValue(scrollingDelta), ActualRange.End);
					VisibleLogRange = new DoubleRange(GetLogValue(VisibleRange.Start), GetLogValue(VisibleRange.End));
					ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
					ZoomPosition = 1 - ZoomFactor;
					break;
			}
		}

		#endregion

		#region Private Methods

		void OnMinMaxChanged()
		{
			if (!double.IsNaN(DefaultMinimum) || !double.IsNaN(DefaultMaximum))
			{
				double minimumValue = double.IsNaN(DefaultMinimum) ? double.MinValue : DefaultMinimum > 0 ? DefaultMinimum : 1;
				double maximumValue = double.IsNaN(DefaultMaximum) ? double.MaxValue : DefaultMaximum;
				ActualRange = new DoubleRange(GetLogValue(minimumValue), GetLogValue(maximumValue));
			}
		}

		DoubleRange CalculateLogRange(DoubleRange range)
		{
			if (range.IsEmpty)
			{
				VisibleLogRange = AddDefaultRange(0d);
				return range;
			}

			double logStart = GetLogValue(range.Start > 0 ? range.Start : 1);
			logStart = double.IsInfinity(logStart) || double.IsNaN(logStart) ? range.Start : logStart;

			double logEnd = GetLogValue(range.End > 0 ? range.End : 10);
			logEnd = double.IsInfinity(logEnd) || double.IsNaN(logEnd) ? LogarithmicBase : logEnd;

			double start = (int)Math.Floor(logStart / 1) * 1;
			double end = (int)Math.Ceiling(logEnd / 1);
			range = SetVisibleRange(start, end);
			return range;
		}

		DoubleRange SetVisibleRange(double start, double end)
		{
			VisibleLogRange = new DoubleRange(start, end);
			return VisibleRange = new DoubleRange(GetPowValue(start), GetPowValue(end));
		}

		#endregion

		#region Internal Methods

		internal double GetLogValue(double value)
		{
			return Math.Log(value) / Math.Log(LogarithmicBase);
		}

		internal double GetPowValue(double power)
		{
			return Math.Pow(LogarithmicBase, power);
		}

		internal override void RaiseCallBackActualRangeChanged()
		{
			if (!ActualRange.IsEmpty)
			{
				ActualMinimum = ActualRange.Start;
				ActualMaximum = ActualRange.End;
			}
		}

		#endregion

		#endregion
	}
}
