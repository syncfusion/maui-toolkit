using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class NumericalAxis : RangeAxisBase
	{
		#region Properties
		internal double DefaultMinimum { get; set; } = double.NaN;

		internal double DefaultMaximum { get; set; } = double.NaN;
		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected override DoubleRange CalculateActualRange()
		{
			if (ActualRange.IsEmpty)
			{
				//Executes when Minimum and Maximum aren't set.
				return base.CalculateActualRange();
			}
			else if (!double.IsNaN(DefaultMinimum) && !double.IsNaN(DefaultMaximum))
			{
				//Executes when Minimum and Maximum are set.
				return new DoubleRange(DefaultMinimum, DefaultMaximum);
			}
			else
			{
				DoubleRange baseRange = base.CalculateActualRange();
				if (!double.IsNaN(DefaultMinimum))
				{
					return new DoubleRange(DefaultMinimum, double.IsNaN(baseRange.End) ? ActualRange.Start + 1 : baseRange.End);
				}

				if (!double.IsNaN(DefaultMaximum))
				{
					return new DoubleRange(double.IsNaN(baseRange.Start) ? ActualRange.End - 1 : baseRange.Start, DefaultMaximum);
				}

				return baseRange;
			}
		}

		/// <inheritdoc/>
		protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
		{
			if (!double.IsNaN(DefaultMinimum) && !double.IsNaN(DefaultMaximum))
			{
				return range;
			}

			if (double.IsNaN(DefaultMinimum) && double.IsNaN(DefaultMaximum))
			{
				return CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
			}
			else
			{
				DoubleRange baseRange = CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
				return !double.IsNaN(DefaultMinimum) ? new DoubleRange(range.Start, baseRange.End) : new DoubleRange(baseRange.Start, range.End);
			}
		}

		/// <inheritdoc/>
		protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
		{
			if (double.IsNaN(AxisInterval) || AxisInterval == 0)
			{
				return CalculateNiceInterval(range, availableSize);
			}

			return AxisInterval;
		}

		/// <inheritdoc/>
		protected override DoubleRange CalculateVisibleRange(DoubleRange range, Size availableSize)
		{
			var visibleRange = base.CalculateVisibleRange(range, availableSize);

			if (ZoomFactor < 1 || ZoomPosition > 0)
			{
				if (!double.IsNaN(AxisInterval))
				{
					double actualInterval = AxisInterval;
					VisibleInterval = EnableAutoIntervalOnZooming
							? CalculateNiceInterval(visibleRange, availableSize)
							: actualInterval;
				}
				else
				{
					VisibleInterval = CalculateNiceInterval(visibleRange, availableSize);
				}
			}

			return visibleRange;
		}

		/// <summary>
		/// Update the ActualMinimum and ActualMaximum property value of NumericalAxis.
		/// </summary>
		internal override void RaiseCallBackActualRangeChanged()
		{
			if (!ActualRange.IsEmpty)
			{
				ActualMinimum = ActualRange.Start;
				ActualMaximum = ActualRange.End;
			}
		}

		#endregion

		#region Internal Methods
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		internal override void GenerateVisibleLabels()
		{
			var actualLabels = VisibleLabels;

			if (VisibleRange.IsEmpty || actualLabels == null)
			{
				return;
			}

			_isOverriddenOnCreateLabelsMethod = ChartUtils.IsOverriddenMethod(this, "OnCreateLabels");

			SmallTickPoints.Clear();
			double interval = VisibleInterval, position;
			var edgeLabelVisibility = EdgeLabelsVisibilityMode;
			var isEdgeLabelAlwaysVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.AlwaysVisible;
			var isEdgeLabelVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.Visible && ZoomFactor.Equals(1.0f);

			position = GetStartDoublePosition();

			while (position <= VisibleRange.End && !double.IsInfinity(position))
			{
				if (VisibleRange.Inside(position))
				{
					var labelFormat = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
					var labelContent = GetActualLabelContent(position, labelFormat);
					var axisLabel = new ChartAxisLabel(position, labelContent);
					actualLabels.Add(axisLabel);

					if (!_isOverriddenOnCreateLabelsMethod)
					{
						TickPositions.Add(position);
					}
				}
				if (!_isOverriddenOnCreateLabelsMethod)
				{
					if (MinorTicksPerInterval > 0)
					{
						AddSmallTicksPoint(position, interval);
					}
				}

				position += interval;
			}

			var count = actualLabels.Count - 1;
			if (count < 0)
			{
				return;
			}

			double pos = Math.Round(actualLabels[count].Position, 6);
			double endValue = Math.Round(VisibleRange.End, 6);

			if ((actualLabels.Count > 0 && pos != endValue && isEdgeLabelAlwaysVisible)
				|| (isEdgeLabelVisible && actualLabels[count].Position > VisibleRange.End))
			{
				var format = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
				var labelContent = GetActualLabelContent(VisibleRange.End, format);
				var axisLabel =
					new ChartAxisLabel(VisibleRange.End, labelContent);
				actualLabels.Add(axisLabel);

				if (labelContent.ToString() == axisLabel.Content.ToString())
				{
					var labelStyle = axisLabel.LabelStyle;

					if (labelStyle != null)
					{
						if (!string.IsNullOrEmpty(labelStyle.LabelFormat))
						{
							axisLabel.Content = GetActualLabelContent(VisibleRange.End, labelStyle.LabelFormat);
						}
					}
				}
				if (!_isOverriddenOnCreateLabelsMethod)
				{
					TickPositions.Add(VisibleRange.End);
				}
			}

			if (_isOverriddenOnCreateLabelsMethod)
			{
				OnCreateLabels();
				AddVisibleLabels();
			}
		}

		internal override double GetStartDoublePosition()
		{
			if ((!double.IsNaN(DefaultMinimum) && ZoomFactor.Equals(1.0f))
			 || EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible ||
			 (EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && ZoomFactor.Equals(1.0f)))
			{
				return VisibleRange.Start;
			}
			else
			{
				return VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
			}
		}

		internal void OnMinMaxChanged()
		{
			if (!double.IsNaN(DefaultMinimum) || !double.IsNaN(DefaultMaximum))
			{
				double minimumValue = double.IsNaN(DefaultMinimum) ? double.MinValue : DefaultMinimum;
				double maximumValue = double.IsNaN(DefaultMaximum) ? double.MaxValue : DefaultMaximum;
				ActualRange = new DoubleRange(minimumValue, maximumValue);
			}
		}
		#endregion

		#region Private Methods
		NumericalPadding ActualRangePadding()
		{
			var visibleSeries = GetVisibleSeries();
			bool isTransposed;
			//TODO: Change range based on series transpose.
			if (RangePadding == NumericalPadding.Auto && (IsPolarArea || CartesianArea != null) && visibleSeries != null && visibleSeries.Count > 0)
			{
				isTransposed = !IsPolarArea && (CartesianArea?.IsTransposed ?? false);

				if ((IsVertical && !isTransposed) || (!IsVertical && isTransposed))
				{
					return NumericalPadding.Round;
				}
			}

			return RangePadding;
		}

		DoubleRange CalculateNumericalRangePadding(DoubleRange range, double interval)
		{
			var actualRangePadding = ActualRangePadding();
			double startRange = range.Start;
			double endRange = range.End;

			if (actualRangePadding == NumericalPadding.Normal)
			{
				double minimum, remaining, start = startRange;

				if (startRange < 0)
				{
					start = 0;
					minimum = startRange + (startRange / 20);
					remaining = interval + (minimum % interval);

					if ((0.365 * interval) >= remaining)
					{
						minimum -= interval;
					}

					if (minimum % interval < 0)
					{
						minimum = (minimum - interval) - (minimum % interval);
					}
				}
				else
				{
					minimum = startRange < ((5.0 / 6.0) * endRange) ? 0 : (startRange - ((endRange - startRange) / 2));

					if (minimum % interval > 0)
					{
						minimum -= minimum % interval;
					}
				}

				double rangeDiff = endRange - start;
				double adjustment = rangeDiff / 20.0;
				double maximum = endRange + ((endRange > 0) ? adjustment : -adjustment);
				remaining = interval - (maximum % interval);

				if ((0.365 * interval) >= remaining)
				{
					maximum += interval;
				}

				if (maximum % interval > 0)
				{
					maximum = (maximum + interval) - (maximum % interval);
				}

				range = new DoubleRange(minimum, maximum);

				if (minimum == 0d)
				{
					ActualInterval = CalculateActualInterval(range, AvailableSize);
					return new DoubleRange(0, Math.Ceiling(maximum / ActualInterval) * ActualInterval);
				}
			}
			else if (actualRangePadding != NumericalPadding.Auto && actualRangePadding != NumericalPadding.None && actualRangePadding != NumericalPadding.Normal)
			{
				double minimum = Math.Floor(startRange / interval) * interval;
				double maximum = Math.Ceiling(endRange / interval) * interval;
				double additionalMinimum = minimum - interval;
				double additionalMaximum = maximum + interval;

				switch (actualRangePadding)
				{
					case NumericalPadding.Round:
						return new DoubleRange(minimum, maximum);
					case NumericalPadding.RoundStart:
						return new DoubleRange(minimum, endRange);
					case NumericalPadding.RoundEnd:
						return new DoubleRange(startRange, maximum);
					case NumericalPadding.PrependInterval:
						return new DoubleRange(additionalMinimum, endRange);
					case NumericalPadding.AppendInterval:
						return new DoubleRange(startRange, additionalMaximum);
					case NumericalPadding.Auto:
					case NumericalPadding.None:
					case NumericalPadding.Normal:
					case NumericalPadding.Additional:
						return new DoubleRange(additionalMinimum, additionalMaximum);
				}
			}

			return range;
		}

		/// <summary>
		/// Methods to update the default minimum value.
		/// </summary>
		/// <param name="minimum">The minimum value.</param>
		void UpdateDefaultMinimum(double? minimum)
		{
			DefaultMinimum = Convert.ToDouble(minimum ?? double.NaN);
		}

		/// <summary>
		/// Methods to update the default minimum value.
		/// </summary>
		/// <param name="maximum">The minimum value.</param>
		void UpdateDefaultMaximum(double? maximum)
		{
			DefaultMaximum = Convert.ToDouble(maximum ?? double.NaN);
		}

		#endregion

		#endregion
	}
}
