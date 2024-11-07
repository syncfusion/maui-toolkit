using System;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class DateTimeAxis : RangeAxisBase
    {
        #region Fields
        DateTimeIntervalType _dateTimeIntervalType = DateTimeIntervalType.Auto;

        internal DateTimeIntervalType ActualIntervalType { get; set; }

        internal DateTime? DefaultMinimum { get; set; }

        internal DateTime? DefaultMaximum { get; set; }
        #endregion

        #region Methods

        #region Protected Overrides

        /// <inheritdoc/>
        protected override DoubleRange CalculateActualRange()
        {
            if (ActualRange.IsEmpty) //Executes when Minimum and Maximum aren't set.
            {
                return base.CalculateActualRange();
            }
            else if (DefaultMinimum != null && DefaultMaximum != null) //Executes when Minimum and Maximum are set.
            {
                return ActualRange;
            }
            else
            {
                var baseRange = base.CalculateActualRange();
                if (DefaultMinimum != null)
                {
                    return new DoubleRange(ActualRange.Start, double.IsNaN(baseRange.End) ? ActualRange.Start + 1 : baseRange.End);
                }

                if (DefaultMaximum != null)
                {
                    return new DoubleRange(double.IsNaN(baseRange.Start) ? ActualRange.End - 1 : baseRange.Start, ActualRange.End);
                }

                return baseRange;
            }
        }

        /// <inheritdoc/>
        protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
        {
            _dateTimeIntervalType = IntervalType;

            if (double.IsNaN(AxisInterval) || AxisInterval == 0)
            {
                return CalculateNiceInterval(range, availableSize);
            }

            if (IntervalType == DateTimeIntervalType.Auto)
            {
                CalculateNiceInterval(range, availableSize);
            }

            return AxisInterval;
        }

        /// <inheritdoc/>
        protected override DoubleRange CalculateVisibleRange(DoubleRange range, Size availableSize)
        {
            var visibleRange = base.CalculateVisibleRange(range, availableSize);
            if (ZoomFactor < 1 || ZoomPosition > 0)
            {
                if (EnableAutoIntervalOnZooming)
                {
                    _dateTimeIntervalType = DateTimeIntervalType.Auto;
                }

                if (AxisInterval != 0 || !double.IsNaN(AxisInterval))
                {
                    VisibleInterval = EnableAutoIntervalOnZooming
                        ? CalculateNiceInterval(visibleRange, availableSize)
                        : ActualInterval;
                }
                else
                {
                    VisibleInterval = CalculateNiceInterval(visibleRange, availableSize);
                }
            }
            else
            {
                _dateTimeIntervalType = IntervalType;

                if (IntervalType != DateTimeIntervalType.Auto)
                {
                    ActualIntervalType = IntervalType;
                }

                if (AxisInterval != 0 || !double.IsNaN(AxisInterval))
                {
                    VisibleInterval = ActualInterval;
                }
                else
                {
                    VisibleInterval = CalculateNiceInterval(visibleRange, availableSize);
                }
            }

            return visibleRange;
        }

        /// <inheritdoc/>
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            if (DefaultMinimum == null && DefaultMaximum == null)
            {
                return ApplyDateRagePadding(range, interval);
            }
            else if (DefaultMinimum != null && DefaultMaximum != null)
            {
                return range;
            }
            else
            {
                var baseRange = ApplyDateRagePadding(range, interval);
                return DefaultMinimum != null
                    ? new DoubleRange(range.Start, baseRange.End)
                    : new DoubleRange(baseRange.Start, range.End);
            }
        }

        /// <inheritdoc/>
        internal override void AddSmallTicksPoint(double position, double interval)
        {
            double tickInterval = interval / (MinorTicksPerInterval + 1);
            double tickPos = position + tickInterval;
            double end = VisibleRange.End;
            position += interval;
            while (Math.Round(tickPos * 100.0) / 100.0 < position && tickPos <= end)
            {
                if (VisibleRange.Inside(tickPos))
                {
                    SmallTickPoints.Add(tickPos);
                }

                tickPos += tickInterval;
            }
        }

        /// <inheritdoc/>
        protected override double CalculateNiceInterval(DoubleRange actualRange, Size availableSize)
        {
            double interval = CalculateDateTimeIntervalType(actualRange, availableSize, ref _dateTimeIntervalType);

            ActualIntervalType = _dateTimeIntervalType;

            return interval;
        }

        /// <summary>
        /// Update the ActualMinimum and ActualMaximum property value of DateTimeAxis.
        /// </summary>
        internal override void RaiseCallBackActualRangeChanged()
        {
            if (!ActualRange.IsEmpty)
            {
                ActualMinimum = DateTime.FromOADate(ActualRange.Start);
                ActualMaximum = DateTime.FromOADate(ActualRange.End);
            }
        }

        #endregion

        #region Internal Methods
        internal void OnMinMaxChanged()
        {
            if (DefaultMinimum != null || DefaultMaximum != null)
            {
                var minimumValue = DefaultMinimum?.ToOADate() ?? DateTime.MinValue.ToOADate();
                var maximumValue = DefaultMaximum?.ToOADate() ?? DateTime.MaxValue.ToOADate();
                ActualRange = new DoubleRange(minimumValue, maximumValue);
            }
        }

        internal override void GenerateVisibleLabels()
        {
            SmallTickPoints.Clear();
            if (VisibleRange.IsEmpty || VisibleInterval == 0 || VisibleRange.Start < 0 || VisibleRange.End < 0)
            {
                return;
            }

			isOverriddenOnCreateLabelsMethod = ChartUtils.IsOverriddenMethod(this, "OnCreateLabels");

            var actualLabels = VisibleLabels;
            var format = LabelStyle != null ? LabelStyle.LabelFormat : String.Empty;
            var labelFormat = !string.IsNullOrEmpty(format) ? format : GetSpecificFormattedLabel(ActualIntervalType);

            var rangeStart = ValidateFromOADate(VisibleRange.Start);
            var date = DateTime.FromOADate(rangeStart);
            date = ChartDataUtils.IncreaseInterval(date, VisibleInterval, ActualIntervalType);
            var interval = date.ToOADate() - VisibleRange.Start;

            var startDate = GetStartPosition();
            var position = startDate.ToOADate();

            var startEdgeDate = DateTime.FromOADate(rangeStart);
            var startEdgePos = startEdgeDate.ToOADate();
            if (position != startEdgePos && EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible
                || (EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && ZoomFactor == 1))
            {
                var labelContent = GetFormattedAxisLabel(labelFormat, startEdgePos);
                var axisLabel = new ChartAxisLabel(startEdgePos, labelContent);
                actualLabels?.Add(axisLabel);

                if (!isOverriddenOnCreateLabelsMethod)
                {
                    TickPositions.Add(startEdgePos);
                }
            }

            while (position <= VisibleRange.End)
            {
                if (VisibleRange.Inside(position))
                {
                    var labelContent = GetFormattedAxisLabel(labelFormat, position);
                    var axisLabel = new ChartAxisLabel(position, labelContent);
                    actualLabels?.Add(axisLabel);

                    if (labelContent.ToString() == axisLabel.Content.ToString())
                    {
                        var labelStyle = axisLabel.LabelStyle;

                        if (labelStyle != null)
                        {
                            if (!string.IsNullOrEmpty(labelStyle.LabelFormat))
                            {
                                axisLabel.Content = GetFormattedAxisLabel(labelStyle.LabelFormat, position);
                            }
                        }
                    }

                    if (!isOverriddenOnCreateLabelsMethod)
                    {
                        TickPositions.Add(position);
                    }
                }

                if (!isOverriddenOnCreateLabelsMethod)
                {
                    if (SmallTickRequired)
                    {
                        AddSmallTicksPoint(position, interval);
                    }
                }

                startDate = ChartDataUtils.IncreaseInterval(startDate, VisibleInterval, ActualIntervalType);
                position = startDate.ToOADate();
            }

            if (VisibleLabels != null && VisibleLabels.Count > 0 && VisibleLabels[VisibleLabels.Count - 1].Position != VisibleRange.End
                && EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible ||
                (EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && ZoomFactor == 1))
            {
                var rangeEnd = ValidateFromOADate(VisibleRange.End);
                var endDate = DateTime.FromOADate(rangeEnd);
                var endDatePos = endDate.ToOADate();

                var labelContent = GetFormattedAxisLabel(labelFormat, endDatePos);
                var axisLabel = new ChartAxisLabel(endDatePos, labelContent);
                actualLabels?.Add(axisLabel);

                if (labelContent.ToString() == axisLabel.Content.ToString())
                {
                    var labelStyle = axisLabel.LabelStyle;

                    if (labelStyle != null)
                    {
                        if (!string.IsNullOrEmpty(labelStyle.LabelFormat))
                        {
                            axisLabel.Content = GetFormattedAxisLabel(labelStyle.LabelFormat, endDatePos);
                        }
                    }
                }
                if (!isOverriddenOnCreateLabelsMethod)
                {
                    TickPositions.Add(endDatePos);
                }
            }

            if (isOverriddenOnCreateLabelsMethod)
            {
                OnCreateLabels();
                AddVisibleLabels();
            }

        }

        internal double CalculateDateTimeIntervalType(DoubleRange actualRange, Size availableSize, ref DateTimeIntervalType dateTimeAxisIntervalType)
        {
            var dateTimeMin = DateTime.FromOADate(actualRange.Start);
            var dateTimeMax = DateTime.FromOADate(actualRange.End);
            var timeSpan = dateTimeMax.Subtract(dateTimeMin);
            double interval = 0;

            switch (dateTimeAxisIntervalType)
            {
                case DateTimeIntervalType.Years:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays / 365), availableSize);
                    break;
                case DateTimeIntervalType.Months:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays / 30), availableSize);
                    break;
                case DateTimeIntervalType.Days:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays), availableSize);
                    break;
                case DateTimeIntervalType.Hours:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalHours), availableSize);
                    break;
                case DateTimeIntervalType.Minutes:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMinutes), availableSize);
                    break;
                case DateTimeIntervalType.Seconds:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalSeconds), availableSize);
                    break;
                case DateTimeIntervalType.Milliseconds:
                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMilliseconds), availableSize);
                    break;
                case DateTimeIntervalType.Auto:
                    var range = new DoubleRange(0, timeSpan.TotalDays / 365);
                    interval = base.CalculateNiceInterval(range, availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Years;

                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays / 30), availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Months;
                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays), availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Days;
                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalHours), availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Hours;
                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMinutes), availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Minutes;
                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalSeconds), availableSize);

                    if (interval >= 1)
                    {
                        dateTimeAxisIntervalType = DateTimeIntervalType.Seconds;
                        return interval;
                    }

                    interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMilliseconds), availableSize);

                    dateTimeAxisIntervalType = DateTimeIntervalType.Milliseconds;

                    break;
            }

            return interval;
        }

        internal DateTime GetStartPosition()
        {
            var startDate = DateTime.FromOADate(VisibleRange.Start);
            NiceStart(ref startDate);
            return startDate;
        }

        internal override double GetStartDoublePosition()
        {
            return GetStartPosition().ToOADate();
        }

        internal override DoubleRange AddDefaultRange(double start)
        {
            if (start != 0)
            {
                return base.AddDefaultRange(start);
            }

            /* 25569.2291666667 is OADate of new DateTime(1970, 1, 1, 5, 30, 0) and 
			25570.2291666667 is OADate of new DateTime(1970, 1, 2, 5, 30, 0). 
			This is for setting default range in Android */
            return new DoubleRange(25569.2291666667, 25570.2291666667);
        }

        internal override void UpdateAutoScrollingDelta(DoubleRange actualRange, double autoScrollingDelta)
        {
            var dateTime = DateTime.FromOADate(actualRange.End);
            if (double.IsNaN(autoScrollingDelta)) return;

            var ActualScrollingDelta = AutoScrollingDelta;

            switch (GetActualAutoScrollingDeltaType())
            {
                case DateTimeIntervalType.Years:
                    var value = dateTime.AddYears((int)-AutoScrollingDelta);
                    autoScrollingDelta = actualRange.End - value.ToOADate();
                    break;
                case DateTimeIntervalType.Months:
                    value = dateTime.AddMonths((int)-AutoScrollingDelta);
                    autoScrollingDelta = actualRange.End - value.ToOADate();
                    break;
                case DateTimeIntervalType.Days:
                    autoScrollingDelta = ActualScrollingDelta;
                    break;
                case DateTimeIntervalType.Hours:
                    autoScrollingDelta = TimeSpan.FromHours(ActualScrollingDelta).TotalDays;
                    break;
                case DateTimeIntervalType.Minutes:
                    autoScrollingDelta = TimeSpan.FromMinutes(ActualScrollingDelta).TotalDays;
                    break;
                case DateTimeIntervalType.Seconds:
                    autoScrollingDelta = TimeSpan.FromSeconds(ActualScrollingDelta).TotalDays;
                    break;
                case DateTimeIntervalType.Milliseconds:
                    autoScrollingDelta = TimeSpan.FromMilliseconds(ActualScrollingDelta).TotalDays;
                    break;
            }

            base.UpdateAutoScrollingDelta(actualRange, autoScrollingDelta);
        }

        internal double GetMinWidthForSingleDataPoint()
        {
            switch (ActualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    return 365;
                case DateTimeIntervalType.Months:
                    return 30;
                case DateTimeIntervalType.Days:
                    return 1;
                case DateTimeIntervalType.Hours:
                    return 24;
                case DateTimeIntervalType.Minutes:
                case DateTimeIntervalType.Seconds:
                    return 60;
                case DateTimeIntervalType.Milliseconds:
                    return 1000;
                default:
                    return 1;
            }
        }

        #endregion

        #region Private Methods
        DateTimeIntervalType GetActualAutoScrollingDeltaType()
        {
            if (AutoScrollingDeltaType == DateTimeIntervalType.Auto)
            {
                CalculateDateTimeIntervalType(ActualRange, AvailableSize, ref _dateTimeIntervalType);

                switch (_dateTimeIntervalType)
                {
                    case DateTimeIntervalType.Years:
                        return DateTimeIntervalType.Years;
                    case DateTimeIntervalType.Months:
                        return DateTimeIntervalType.Months;
                    case DateTimeIntervalType.Days:
                        return DateTimeIntervalType.Days;
                    case DateTimeIntervalType.Hours:
                        return DateTimeIntervalType.Hours;
                    case DateTimeIntervalType.Minutes:
                        return DateTimeIntervalType.Minutes;
                    case DateTimeIntervalType.Seconds:
                        return DateTimeIntervalType.Seconds;
                    case DateTimeIntervalType.Milliseconds:
                        return DateTimeIntervalType.Milliseconds;
                    default:
                        return DateTimeIntervalType.Auto;
                }
            }
            else
            {
                return AutoScrollingDeltaType;
            }
        }

        DoubleRange ApplyDateRagePadding(DoubleRange range, double interval)
        {
            if (RangePadding != DateTimeRangePadding.Auto && RangePadding != DateTimeRangePadding.None)
            {
                var startDate = DateTime.FromOADate(range.Start);
                var endDate = DateTime.FromOADate(range.End);
                switch (ActualIntervalType)
                {
                    case DateTimeIntervalType.Years:
                        return ApplyRangePaddingYears(interval, startDate, endDate);
                    case DateTimeIntervalType.Months:
                        return ApplyRangePaddingForMonths(range, interval, startDate, endDate);
                    case DateTimeIntervalType.Days:
                        return ApplyRangePaddingForDays(interval, startDate, endDate);
                    case DateTimeIntervalType.Hours:
                        return ApplyRangePaddingForHours(interval, startDate, endDate);
                    case DateTimeIntervalType.Minutes:
                        return ApplyRangePaddingForMinutes(range, interval, startDate, endDate);
                    case DateTimeIntervalType.Seconds:
                        return ApplyRangePaddingForSeconds(range, interval, startDate, endDate);
                    case DateTimeIntervalType.Milliseconds:
                        return ApplyRangePaddingForMillis(range, interval, startDate, endDate);
                }
            }

            return range;
        }

        DoubleRange ApplyRangePaddingForMillis(DoubleRange range, double interval, DateTime startDate, DateTime endDate)
        {
            var milliseconds = (int)((int)(startDate.Millisecond / interval) * interval);
            var endMilliseconds = DateTime.FromOADate(range.End).Millisecond + (startDate.Millisecond - milliseconds);
            if (endMilliseconds > 999)
            {
                endMilliseconds = 999;
            }

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                startDate.Minute,
                startDate.Second,
                milliseconds);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                endDate.Minute,
                endDate.Second,
                endMilliseconds);
            DateTime additionalStartDate = startDate.AddMilliseconds(-interval);
            DateTime additionalEndDate = endDate.AddMilliseconds(interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingForSeconds(DoubleRange range, double interval, DateTime startDate, DateTime endDate)
        {
            var second = (int)((int)(startDate.Second / interval) * interval);
            var endSecond = DateTime.FromOADate(range.End).Second + (startDate.Second - second);
            if (endSecond > 59)
            {
                endSecond = 59;
            }

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                startDate.Minute,
                second,
                0);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                endDate.Minute,
                endSecond,
                999);
            DateTime additionalStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                startDate.Minute,
                startDate.Second,
                0).AddSeconds(-interval);
            DateTime additionalEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                endDate.Minute,
                endDate.Second,
                999).AddSeconds(interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingForMinutes(DoubleRange range, double interval, DateTime startDate, DateTime endDate)
        {
            var minute = (int)((int)(startDate.Minute / interval) * interval);
            var endMinute = DateTime.FromOADate(range.End).Minute + (startDate.Minute - minute);
            if (endMinute > 59)
            {
                endMinute = 59;
            }

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                minute,
                0);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                endMinute,
                59);
            DateTime additionalStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                startDate.Minute,
                0).AddMinutes(-interval);
            DateTime additionalEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                endDate.Minute,
                59).AddMinutes(interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingForHours(double interval, DateTime startDate, DateTime endDate)
        {
            var hour = (int)((int)(startDate.Hour / interval) * interval);
            var endHour = endDate.Hour + (startDate.Hour - hour);
            if (endHour > 23)
            {
                endHour = 23;
            }

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                hour,
                0,
                0);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endHour,
                59,
                59);
            DateTime additionalStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startDate.Hour,
                0,
                0).AddHours((int)-interval);
            DateTime additionalEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endDate.Hour,
                59,
                59).AddHours((int)interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingForDays(double interval, DateTime startDate, DateTime endDate)
        {
            var start = (int)((int)(startDate.Day / interval) * interval);
            if (start <= 0)
            {
                start = 1;
            }

            var diff = startDate.Day - start;

            startDate = startDate.AddDays(-diff);
            endDate = endDate.AddDays(diff);

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                0,
                0,
                0);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                23,
                59,
                59);
            DateTime additionalStartDate = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                0,
                0,
                0).AddDays((int)-interval);
            DateTime additionalEndDate = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                23,
                59,
                59).AddDays((int)interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingForMonths(DoubleRange range, double interval, DateTime startDate, DateTime endDate)
        {
            var month = (int)((int)(startDate.Month / interval) * interval);
            if (month <= 0)
            {
                month = 1;
            }

            var endMonth = DateTime.FromOADate(range.End).Month + (startDate.Month - month);
            if (endMonth <= 0)
            {
                endMonth = 1;
            }

            if (endMonth > 12)
            {
                endMonth = 12;
            }

            DateTime roundStartDate = new DateTime(
                startDate.Year,
                month,
                1,
                0,
                0,
                0);
            DateTime roundEndDate = new DateTime(
                endDate.Year,
                endMonth,
                endMonth == 2 ? 28 : 30,
                0,
                0,
                0);
            DateTime additionalStartDate = new DateTime(
                startDate.Year,
                month,
                1,
                0,
                0,
                0).AddMonths((int)(-interval));
            DateTime additionalEndDate = new DateTime(
                endDate.Year,
                endMonth,
                endMonth == 2 ? 28 : 30,
                0,
                0,
                0).AddMonths((int)interval);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePaddingYears(double interval, DateTime startDate, DateTime endDate)
        {
            var startYear = (int)((int)(startDate.Year / interval) * interval);
            var endYear = endDate.Year + (startDate.Year - startYear);
            if (startYear <= 0)
            {
                startYear = 1;
            }

            if (endYear <= 0)
            {
                endYear = 1;
            }

            DateTime additionalStartDate = new DateTime(startYear - (int)interval, 1, 1, 0, 0, 0);
            DateTime additionalEndDate = new DateTime(endYear + (int)interval, 12, 31, 23, 59, 59);
            DateTime roundStartDate = new DateTime(startYear, 1, 1, 0, 0, 0);
            DateTime roundEndDate = new DateTime(endYear, 12, 31, 23, 59, 59);

            return ApplyRangePadding(additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
        }

        DoubleRange ApplyRangePadding(DateTime additionalStartDate, DateTime additionalEndDate, DateTime roundStartDate, DateTime roundEndDate, DateTime startDate, DateTime endDate)
        {
            switch (RangePadding)
            {
                case DateTimeRangePadding.Round:
                    return new DoubleRange(roundStartDate.ToOADate(), roundEndDate.ToOADate());
                case DateTimeRangePadding.RoundStart:
                    return new DoubleRange(roundStartDate.ToOADate(), endDate.ToOADate());
                case DateTimeRangePadding.RoundEnd:
                    return new DoubleRange(startDate.ToOADate(), roundEndDate.ToOADate());
                case DateTimeRangePadding.PrependInterval:
                    return new DoubleRange(additionalStartDate.ToOADate(), endDate.ToOADate());
                case DateTimeRangePadding.AppendInterval:
                    return new DoubleRange(startDate.ToOADate(), additionalEndDate.ToOADate());
                case DateTimeRangePadding.Additional:
                    return new DoubleRange(additionalStartDate.ToOADate(), additionalEndDate.ToOADate());
                default:
                    return new DoubleRange(startDate.ToOADate(), endDate.ToOADate());
            }
        }

        double ValidateFromOADate(double value)
        {
            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        void NiceStart(ref DateTime startDate)
        {
            if (ActualIntervalType == DateTimeIntervalType.Auto)
            {
                return;
            }

            var actualStart = DateTime.FromOADate(ActualRange.Start);
            var diff = startDate - actualStart;

            switch (ActualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    var year = (int)((int)(startDate.Year / VisibleInterval) * VisibleInterval);
                    if (year <= 0)
                    {
                        year = 1;
                    }

                    startDate = new DateTime(year, 1, 1, 0, 0, 0);
                    return;
                case DateTimeIntervalType.Months:
                    var month = (int)((int)(diff.TotalDays / 30.436875 / VisibleInterval) * VisibleInterval);
                    if (month < 0)
                    {
                        month = 0;
                    }

                    startDate = actualStart.AddMonths(month);
                    startDate = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0);
                    return;
                case DateTimeIntervalType.Days:
                    var day = (int)((int)(diff.TotalDays / VisibleInterval) * VisibleInterval);
                    if (day < 0)
                    {
                        day = 0;
                    }

                    startDate = actualStart.AddDays(day);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                    return;
                case DateTimeIntervalType.Hours:
                    var hour = (int)((int)(diff.TotalHours / VisibleInterval) * VisibleInterval);
                    startDate = actualStart.AddHours(hour);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0);
                    return;
                case DateTimeIntervalType.Minutes:
                    var minute = (int)((int)(diff.TotalMinutes / VisibleInterval) * VisibleInterval);
                    startDate = actualStart.AddMinutes(minute);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0);
                    return;
                case DateTimeIntervalType.Seconds:
                    var second = (int)((int)(diff.TotalSeconds / VisibleInterval) * VisibleInterval);
                    startDate = actualStart.AddSeconds(second);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second);
                    return;
                case DateTimeIntervalType.Milliseconds:
                    var millisecond = (int)((int)(diff.TotalMilliseconds / VisibleInterval) * VisibleInterval);
                    startDate = actualStart.AddMilliseconds(millisecond);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second, startDate.Millisecond);
                    return;
            }
        }

        /// <summary>
        /// Methods to update axis interval.
        /// </summary>
        /// <param name="interval">The axis interval.</param>
        void UpdateAxisInterval(double interval)
        {
            AxisInterval = interval;
        }

        /// <summary>
        /// Methods to update default minimum value.
        /// </summary>
        /// <param name="dateTime">The minimum value.</param>
        void UpdateDefaultMinimum(DateTime? dateTime)
        {
            DefaultMinimum = dateTime;
        }

        /// <summary>
        /// Methods to update default maximum value.
        /// </summary>
        /// <param name="dateTime">The maximum value.</param>
        void UpdateDefaultMaximum(DateTime? dateTime)
        {
            DefaultMaximum = dateTime;
        }

        /// <summary>
        /// Methods to update ActualIntervalType.
        /// </summary>
        /// <param name="intervalType">The interval type.</param>
        void UpdateActualIntervalType(DateTimeIntervalType intervalType)
        {
            ActualIntervalType = intervalType;
        }

        #endregion

        #endregion
    }
}
