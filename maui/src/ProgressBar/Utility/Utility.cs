namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Validate the minimum and maximum values, if maximum is less than minimum then swap the values.
        /// </summary>
        /// <param name="minimum">The minimum</param>
        /// <param name="maximum">The maximum</param>
        /// <returns>Validated minimum and maximum tuple.</returns>
        internal static (double, double) ValidateMinimumMaximumValue(double minimum, double maximum)
        {
            double min = double.IsNaN(minimum) ? 0 : minimum;
            double max = double.IsNaN(maximum) ? 0 : maximum;
            minimum = min < max ? min : max;
            maximum = min > max ? min : max;
            return (minimum, maximum);
        }

        /// <summary>
        /// To convert the given degree to radian.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>Radian equivalent of given degree.</returns>
        internal static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        /// <summary>
        /// To calculate SweepAngle.
        /// </summary>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        /// <returns>Sweep angle between start and end angle</returns>
        internal static double CalculateSweepAngle(double startAngle, double endAngle)
        {
            double actualEndAngle = endAngle > 360 ? endAngle % 360 : endAngle;
            double sweepAngle = actualEndAngle - startAngle;
            return sweepAngle <= 0 ? (sweepAngle + 360) : sweepAngle;
        }

        /// <summary>
        /// To convert angle to vector.
        /// </summary>
        /// <param name="angle">The angle</param>
        /// <returns>Vector of given angle</returns>
        internal static Point AngleToVector(double angle)
        {
            double angleRadian = Utility.DegreeToRadian(angle);
            return new Point(Math.Cos(angleRadian), Math.Sin(angleRadian));
        }

        /// <summary>
        /// To find the minimum and maximum value
        /// </summary>
        /// <param name="point1">Sets the point1</param>
        /// <param name="point2">Sets the point2</param>
        /// <param name="degree">Sets the degree</param>
        /// <returns>Min and Max value.</returns>
        internal static double GetMinMaxValue(Point point1, Point point2, int degree)
        {
            return degree switch
            {
                270 => Math.Max(point1.Y, point2.Y),
                0 or 360 => Math.Min(point1.X, point2.X),
                90 => Math.Min(point1.Y, point2.Y),
                180 => Math.Max(point1.X, point2.X),
                _ => 0d
            };
        }

        /// <summary>
        /// To calculate angle for corner radius
        /// </summary>
        /// <param name="radius">axis radius.</param>
        /// <param name="circleRadius">Corner radius ellipse radius</param>
        /// <returns>Angle for corner radius</returns>
        internal static double CornerRadiusAngle(double radius, double circleRadius)
        {
            var perimeter = ((2 * radius) + circleRadius) / 2;
            var area = Math.Sqrt(perimeter * (perimeter - radius) * (perimeter - radius) * (perimeter - circleRadius));
            return Math.Asin(2 * area / (radius * radius)) * (180 / Math.PI);
        }

        /// <summary>
        /// To update the gradient stops based actual range.
        /// </summary>
        /// <param name="gradientStops">The gradient stops collection.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns>Updated gradient stops collection.</returns>
        internal static List<ProgressGradientStop> UpdateGradientStopCollection(List<ProgressGradientStop> gradientStops, double rangeStart, double rangeEnd)
        {
            gradientStops = gradientStops.OrderBy(x => x.ActualValue).ToList();

            if (gradientStops.First().ActualValue > rangeStart)
            {
                gradientStops.Insert(0, new ProgressGradientStop
                {
                    Color = gradientStops.First().Color,
                    ActualValue = rangeStart
                });
            }

            if (gradientStops.Last().ActualValue < rangeEnd)
            {
                gradientStops.Add(new ProgressGradientStop
                {
                    Color = gradientStops.Last().Color,
                    ActualValue = rangeEnd
                });
            }

            return gradientStops;
        }

        /// <summary>
        /// To get the gradient range based on gradient stops collection, range start and end values.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns>The gradient range based on gradient stops collection, range start and end values.</returns>
        internal static List<double> GetGradientRange(List<ProgressGradientStop> gradientStops, double rangeStart, double rangeEnd)
        {
            List<double> gradientRange = new List<double>()
            {
                rangeStart,
                rangeEnd
            };

            foreach (ProgressGradientStop gradient in gradientStops)
            {
                if (gradient.ActualValue > rangeStart && gradient.ActualValue < rangeEnd)
                {
                    if (!gradientRange.Contains(gradient.ActualValue))
                    {
                        gradientRange.Add(gradient.ActualValue);
                    }
                }
            }

            gradientRange.Sort();
            return gradientRange;
        }
    }
}