namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides data for the <see cref="ChartAxis.ActualRangeChanged"/> event.
	/// </summary>
	/// <remarks>This class contains information about the minimum and maximum value of the range.</remarks>
	public class ActualRangeChangedEventArgs : EventArgs
	{
		#region Internal Properties

		internal object? VisibleMaximum { get; set; }

		internal object? VisibleMinimum { get; set; }

		internal ChartAxis? Axis { get; set; }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a value that represents the new actual maximum value of the range.
		/// </summary>
		public object ActualMaximum { get; internal set; }

		/// <summary>
		/// Gets a value that represents the new actual minimum value of the range.
		/// </summary>
		public object ActualMinimum { get; internal set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ActualRangeChangedEventArgs"/> class.
		/// </summary>
		public ActualRangeChangedEventArgs(object actualMinimum, object actualMaximum)
		{
			ActualMinimum = actualMinimum;
			ActualMaximum = actualMaximum;
		}

		#endregion

		#region Methods

		#region Internal Methods

		internal DoubleRange GetActualRange()
		{
			return new DoubleRange(ToDouble(ActualMinimum), ToDouble(ActualMaximum));
		}

		internal DoubleRange GetVisibleRange()
		{
			if (VisibleMinimum == null && VisibleMaximum == null)
			{
				return DoubleRange.Empty;
			}

			double start, end;

			if (VisibleMaximum == null)
			{
				start = ToDouble(VisibleMinimum);
				end = ToDouble(ActualMaximum);
			}
			else if (VisibleMinimum == null)
			{
				start = ToDouble(ActualMinimum);
				end = ToDouble(VisibleMaximum);
			}
			else
			{
				start = ToDouble(VisibleMinimum);
				end = ToDouble(VisibleMaximum);
			}

			DoubleRange actualRange = GetActualRange();

			if (start < actualRange.Start)
			{
				start = actualRange.Start;
			}

			if (end > actualRange.End)
			{
				end = actualRange.End;
			}

			if (start == end)
			{
				end++;
			}

			return new DoubleRange(start, end);
		}

		#endregion

		#region Private Method

		double ToDouble(object? val)
		{
			if (val == null)
			{ return double.NaN; }

			if (Axis is DateTimeAxis && val is DateTime time)
			{
				DateTime date = time;
				return date.Ticks;
			}
			else
			{
				string? value = val.ToString();
				return string.IsNullOrEmpty(value) ? double.NaN : double.Parse(value);
			}
		}

		#endregion

		#endregion
	}
}