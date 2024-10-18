namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    #region Enums

    /// <summary>
    /// Describes the possible transition types available in the <see cref="SfPullToRefresh"/> control.
    /// </summary>
    public enum PullToRefreshTransitionType
    {
        /// <summary>
        /// Positions the <see cref="SfProgressCircleView"/> over the pullable content.
        /// </summary>
        Push,

        /// <summary>
        /// Positions the <see cref="SfProgressCircleView"/> above the pullable content, translating it based on the pulling progress.
        /// </summary>
        SlideOnTop,
    }

    #endregion
}
