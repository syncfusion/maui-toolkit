namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Specifies the size units in the circular progress bar.
    /// </summary>
    public enum SizeUnit
    {
        /// <summary>
        /// Indicates to treat the provided value as pixel.
        /// </summary>
        Pixel,

        /// <summary>
        /// Indicates to treat the provided value as factor.
        /// </summary>
        Factor
    }

    /// <summary>
    /// Specifies the corner style for <see cref="SfCircularProgressBar"/>.
    /// </summary>
    public enum CornerStyle
    {
        /// <summary>
        /// Flat does not apply the rounded corner on both side
        /// </summary>
        BothFlat,

        /// <summary>
        /// Curve apply the rounded corner on both side.
        /// </summary>
        BothCurve,

        /// <summary>
        /// Curve apply the rounded corner on end(right) side.
        /// </summary>
        EndCurve,

        /// <summary>
        /// Curve apply the rounded corner on start(left) side.
        /// </summary>
        StartCurve
    }
}
