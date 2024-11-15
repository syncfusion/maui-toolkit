namespace Syncfusion.Maui.Toolkit.Shimmer
{
    /// <summary>
    /// Interface that holds the properties to render the <see cref="SfShimmer"/> view.
    /// </summary>
    internal interface IShimmer
    {
        /// <summary>
        /// Gets the duration of the wave animation in milliseconds.
        /// </summary>
        double AnimationDuration { get; }

        /// <summary>
        /// Gets the background color of the shimmer view.
        /// </summary>
        Brush Fill { get; }

        /// <summary>
        /// Gets a value indicating whether the shimmer is active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        ///  Gets the color of the shimmer wave.
        /// </summary>
        Color WaveColor { get; }

        /// <summary>
        /// Gets the width of the shimmer wave.
        /// </summary>
        double WaveWidth { get; }

        /// <summary>
        /// Gets the built-in shimmer view type.
        /// </summary>
        ShimmerType Type { get; }

        /// <summary>
        /// Gets the direction of the shimmer wave animation.
        /// </summary>
        ShimmerWaveDirection WaveDirection { get; }

        /// <summary>
        /// Gets the number of times the shimmer view should be repeated.
        /// </summary>
        int RepeatCount { get; }

        /// <summary>
        /// Gets the custom view used for the loading view.
        /// </summary>
        View CustomView { get; }
    }
}
