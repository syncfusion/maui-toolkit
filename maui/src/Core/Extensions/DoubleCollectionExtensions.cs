namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Represents <see cref="DoubleCollection"/> conversion.
    /// </summary>
    public static class DoubleCollectionExtensions
    {
        /// <summary>
        /// Method used to convert <see cref="DoubleCollection"/> to float array. 
        /// </summary>
        /// <param name="doubleCollection">DoubleCollection value.</param>
        /// <returns>Returns float array.</returns>
        public static float[] ToFloatArray(this DoubleCollection doubleCollection)
        {
            float[] array = new float[doubleCollection.Count];

            for (int i = 0; i < doubleCollection.Count; i++)
            {
                array[i] = (float)doubleCollection[i];
            }

            return array;
        }
    }
}
