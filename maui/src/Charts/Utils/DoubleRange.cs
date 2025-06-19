using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Defines a custom DoubleRange data type for <see cref="ChartBase"/> library.
	/// </summary>
	public struct DoubleRange
	{
		#region Members
		/// <summary>
		/// Initializes _empty
		/// </summary>
		static readonly DoubleRange _empty = new(double.NaN, double.NaN);
		readonly bool _isEmpty;

		/// <summary>
		/// Initializes endValue
		/// </summary>
		readonly double _startValue;

		/// <summary>
		/// Initializes endValue
		/// </summary>
		readonly double _endValue;

		#endregion

		#region Properties

		/// <summary>
		/// Gets an empty DoubleRange.
		/// </summary>
		/// <returns>A DoubleRange that represents an empty range.</returns>
		public static DoubleRange Empty
		{
			get
			{
				return _empty;
			}
		}

		/// <summary>
		/// Gets the start value of a DoubleRange.
		/// </summary>
		/// <returns>A double that represents the start value.</returns>
		public double Start
		{
			get
			{
				return _startValue;
			}
		}

		/// <summary>
		/// Gets the end value of a DoubleRange.
		/// </summary>
		/// <returns>A double that represents the end value.</returns>
		public double End
		{
			get
			{
				return _endValue;
			}
		}

		/// <summary>
		/// Gets the difference between the end and start of a DoubleRange.
		/// </summary>
		/// <returns>A double that represents the difference between the end and start of a DoubleRange.</returns>
		public double Delta
		{
			get
			{
				return _endValue - _startValue;
			}
		}

		/// <summary>
		/// Gets the median value of a DoubleRange.
		/// </summary>
		/// <returns>A double that represents the median value of a DoubleRange.</returns>
		public double Median
		{
			get
			{
				return (_startValue + _endValue) / 2d;
			}
		}

		/// <summary>
		/// Gets a value indicating whether a DoubleRange is empty.
		/// </summary>
		/// <returns>A boolean indicating whether the DoubleRange is empty.</returns>
		public bool IsEmpty
		{
			get
			{
				return _isEmpty;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DoubleRange"/> struct.
		/// </summary>
		/// <param name="start">The start value.</param>
		/// <param name="end">The end value.</param>
		public DoubleRange(double start, double end)
		{
			if (!double.IsNaN(start) && !double.IsNaN(end))
			{
				_isEmpty = false;
			}
			else
			{
				_isEmpty = true;
			}

			if (start > end)
			{
				_startValue = end;
				_endValue = start;
			}
			else
			{
				_startValue = start;
				_endValue = end;
			}
		}
		#endregion

		#region Operators

		/// <summary>
		/// Adds two DoubleRanges.
		/// </summary>
		/// <param name="leftRange">The first DoubleRange to add.</param>
		/// <param name="rightRange">The second DoubleRange to add.</param>
		/// <returns>A DoubleRange that represents the result of adding the two DoubleRanges.</returns>
		public static DoubleRange operator +(DoubleRange leftRange, DoubleRange rightRange)
		{
			return Union(leftRange, rightRange);
		}

		/// <summary>
		/// Adds a specified value to a DoubleRange.
		/// </summary>
		/// <param name="range">The DoubleRange to add the value to.</param>
		/// <param name="value">The value to add to the DoubleRange.</param>
		/// <returns>A DoubleRange that represents the result of adding the specified value to the DoubleRange.</returns>
		public static DoubleRange operator +(DoubleRange range, double value)
		{
			return Union(range, value);
		}

		/// <summary>
		/// Determines whether a DoubleRange is greater than a specified value.
		/// </summary>
		/// <param name="range">The DoubleRange to compare.</param>
		/// <param name="value">The value to compare.</param>
		/// <returns>A boolean indicating whether the DoubleRange is greater than the specified value.</returns>
		public static bool operator >(DoubleRange range, double value)
		{
			return range._startValue > value;
		}

		/// <summary>
		/// Determines whether a DoubleRange is greater than another DoubleRange.
		/// </summary>
		/// <param name="range">The first DoubleRange to compare.</param>
		/// <param name="value">The second DoubleRange to compare.</param>
		/// <returns>A boolean indicating whether the first DoubleRange is greater than the second.</returns>
		public static bool operator >(DoubleRange range, DoubleRange value)
		{
			return range._startValue > value._startValue && range._endValue > value._endValue;
		}

		/// <summary>
		/// Determines whether a DoubleRange is less than another DoubleRange.
		/// </summary>
		/// <param name="range">The first DoubleRange to compare.</param>
		/// <param name="value">The second DoubleRange to compare.</param>
		/// <returns>A boolean indicating whether the first DoubleRange is less than the second.</returns>
		public static bool operator <(DoubleRange range, DoubleRange value)
		{
			return range._startValue < value._startValue && range._endValue < value._endValue;
		}

		/// <summary>
		/// Determines whether a DoubleRange is less than a specified value.
		/// </summary>
		/// <param name="range">The DoubleRange to compare.</param>
		/// <param name="value">The value to compare.</param>
		/// <returns>A boolean indicating whether the DoubleRange is less than the specified value.</returns>
		public static bool operator <(DoubleRange range, double value)
		{
			return range._endValue < value;
		}

		/// <summary>
		/// Determines whether two DoubleRanges are equal.
		/// </summary>
		/// <param name="leftRange">The first DoubleRange to compare.</param>
		/// <param name="rightRange">The second DoubleRange to compare.</param>
		/// <returns>A boolean indicating whether the two DoubleRanges are equal.</returns>
		public static bool operator ==(DoubleRange leftRange, DoubleRange rightRange)
		{
			return leftRange.Equals(rightRange);
		}

		/// <summary>
		/// Determines whether two DoubleRanges are not equal.
		/// </summary>
		/// <param name="leftRange">The first DoubleRange to compare.</param>
		/// <param name="rightRange">The second DoubleRange to compare.</param>
		/// <returns>A boolean indicating whether the two DoubleRanges are not equal.</returns>
		public static bool operator !=(DoubleRange leftRange, DoubleRange rightRange)
		{
			return !leftRange.Equals(rightRange);
		}
		#endregion

		#region Public methods

		/// <summary>
		/// Finds the union of a set of values.
		/// </summary>
		/// <param name="values">The set of values to find the union of.</param>
		/// <returns>A DoubleRange that represents the union of the specified set of values.</returns>
		public static DoubleRange Union(params double[] values)
		{
			double min = double.MaxValue;
			double max = double.MinValue;

			foreach (double val in values)
			{
				if (double.IsNaN(val))
				{
					min = val;
				}
				else if (min > val)
				{
					min = val;
				}

				if (max < val)
				{
					max = val;
				}
			}

			return new DoubleRange(min, max);
		}

		/// <summary>
		/// Finds the union of two DoubleRanges.
		/// </summary>
		/// <param name="leftRange">The first DoubleRange to find the union with.</param>
		/// <param name="rightRange">The second DoubleRange to find the union with.</param>
		/// <returns>A DoubleRange that represents the union of the two specified ranges.</returns>
		public static DoubleRange Union(DoubleRange leftRange, DoubleRange rightRange)
		{
			if (leftRange.IsEmpty)
			{
				return rightRange;
			}
			else if (rightRange.IsEmpty)
			{
				return leftRange;
			}

			return new DoubleRange(Math.Min(leftRange._startValue, rightRange._startValue), Math.Max(leftRange._endValue, rightRange._endValue));
		}

		/// <summary>
		/// Finds the union of a DoubleRange with a specified value.
		/// </summary>
		/// <param name="range">The DoubleRange to find the union with.</param>
		/// <param name="value">The value to find the union with.</param>
		/// <returns>A DoubleRange that represents the union of the specified range and value.</returns>
		public static DoubleRange Union(DoubleRange range, double value)
		{
			if (range.IsEmpty)
			{
				return new DoubleRange(value, value);
			}

			return new DoubleRange(Math.Min(range._startValue, value), Math.Max(range._endValue, value));
		}

		/// <summary>
		/// Scales a DoubleRange by a specified value.
		/// </summary>
		/// <param name="range">The DoubleRange to be scaled.</param>
		/// <param name="value">The value by which to scale the range.</param>
		/// <returns>A DoubleRange that has been scaled by the specified value.</returns>
		public static DoubleRange Scale(DoubleRange range, double value)
		{
			if (range.IsEmpty)
			{
				return range;
			}

			return new DoubleRange(range._startValue - (value * range.Delta), range._endValue + (value * range.Delta));
		}

		/// <summary>
		/// Offsets a DoubleRange by a specified value.
		/// </summary>
		/// <param name="range">The DoubleRange to be offset.</param>
		/// <param name="value">The value by which to offset the range.</param>
		/// <returns>A DoubleRange that has been offset by the specified value.</returns>
		public static DoubleRange Offset(DoubleRange range, double value)
		{
			if (range.IsEmpty)
			{
				return range;
			}

			return new DoubleRange(range._startValue + value, range._endValue + value);
		}

		/// <summary>
		/// Excludes a range from another range.
		/// </summary>
		/// <param name="range">The range.</param>
		/// <param name="excluder">The excluder.</param>
		/// <param name="leftRange">The left range.</param>
		/// <param name="rightRange">The right range.</param>
		/// <returns>A boolean indicating whether the exclusion was successful.</returns>
		public static bool Exclude(DoubleRange range, DoubleRange excluder, out DoubleRange leftRange, out DoubleRange rightRange)
		{
			leftRange = new DoubleRange(double.NaN, double.NaN);
			rightRange = new DoubleRange(double.NaN, double.NaN);

			if (!(range.IsEmpty || excluder.IsEmpty))
			{
				if (excluder._endValue < range._startValue)
				{
					if (excluder._endValue > range._startValue)
					{
						leftRange = new DoubleRange(excluder._startValue, range._startValue);
					}
					else
					{
						leftRange = excluder;
					}
				}

				if (excluder._endValue > range._endValue)
				{
					if (excluder._startValue < range._endValue)
					{
						rightRange = new DoubleRange(range._endValue, excluder._endValue);
					}
					else
					{
						rightRange = excluder;
					}
				}
			}

			return !(leftRange.IsEmpty && rightRange.IsEmpty);
		}

		/// <summary>
		/// Determines whether this DoubleRange intersects with another range.
		/// </summary>
		/// <param name="range">The other range to check for intersection with.</param>
		/// <returns>A boolean indicating whether the two ranges intersect.</returns>
		public bool Intersects(DoubleRange range)
		{
			if (IsEmpty)
			{
				return false;
			}

			return Inside(range._startValue) || Inside(range._endValue) || range.Inside(_startValue) || range.Inside(_endValue);
		}

		/// <summary>
		/// Determines whether this DoubleRange intersects with a given start and end value.
		/// </summary>
		/// <param name="start">The start value of the range to check for intersection with.</param>
		/// <param name="end">The end value of the range to check for intersection with.</param>
		/// <returns>A boolean indicating whether this DoubleRange intersects with the given start and end values.</returns>
		public bool Intersects(double start, double end)
		{
			return Intersects(new DoubleRange(start, end));
		}

		/// <summary>
		/// Determines whether a given value is inside this DoubleRange.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>A boolean indicating whether the value is inside this DoubleRange.</returns>
		public bool Inside(double value)
		{
			if (IsEmpty)
			{
				return false;
			}

			return (value <= _endValue) && (value >= _startValue);
		}

		/// <summary>
		/// Determines whether a given range is inside this DoubleRange.
		/// </summary>
		/// <param name="range">The range to check.</param>
		/// <returns>A boolean indicating whether the given range is inside this DoubleRange.</returns>
		public bool Inside(DoubleRange range)
		{
			if (IsEmpty)
			{
				return false;
			}

			return _startValue <= range._startValue && _endValue >= range._endValue;
		}

		/// <summary>
		/// Determines whether the specified object is equal to this DoubleRange.
		/// </summary>
		/// <param name="obj">The object to compare with this DoubleRange.</param>
		/// <returns>A boolean indicating whether the specified object is equal to this DoubleRange.</returns>
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is DoubleRange range)
			{
				return (_startValue == range._startValue) && (_endValue == range._endValue);
			}

			return false;
		}

		/// <summary>
		/// Returns the hash code for this DoubleRange.
		/// </summary>
		/// <returns>An integer representing the hash code of this DoubleRange.</returns>
		public override int GetHashCode()
		{
			return _startValue.GetHashCode() ^ _endValue.GetHashCode();
		}
		#endregion
	}
}
