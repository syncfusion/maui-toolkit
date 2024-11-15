namespace Syncfusion.Maui.Toolkit
{
	/// <summary>
	/// Provides utility methods for mathematical operations.
	/// </summary>
	internal class MathUtils
    {
        internal static double GetAngle(double x1, double x2, double y1, double y2)
        {
            double radians = Math.Atan2(-(y2 - y1), x2 - x1);
            radians = radians < 0 ? Math.Abs(radians) : (2 * Math.PI) - radians;
            return radians * (180 / Math.PI);
        }


        /// <summary>
        /// Provides the distance between two points based on the Euclidean distance formula.
        /// </summary>
        /// <param name="x1">X value of Point 1</param>
        /// <param name="x2">X value of Point 2</param>
        /// <param name="y1">Y value of Point 1</param>
        /// <param name="y2">Y value of Point 2</param>
        /// <returns>The distance between two points</returns>
        internal static double GetDistance(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
