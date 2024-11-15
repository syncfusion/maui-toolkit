namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Defines methods and properties that must be implemented to get notification of the pull to refresh actions.
    /// </summary>
    internal interface IPullToRefresh
    {
        /// <summary>
        /// Gets or sets a value indicating whether to bounce SfDataGrid body region only with TransitionMode-Push or SfDataGrid to bounce with HeaderRow also.
        /// </summary>
        bool CanCustomizeContentLayout { get; set; }

        /// <summary>
        /// Provides notifications regarding the pulling action with the progress value and an out parameter to cancel the pulling event.
        /// </summary>
        /// <param name="progress">Progress value of the pulling action.</param>
        /// <param name="pullToRefresh">Instance of PullToRefresh.</param>
        /// <param name="cancel"> If cancel is set to true, pulling will be cancelled.</param>
        void Pulling(double progress, object pullToRefresh, out bool cancel);

        /// <summary>
        /// Invoked during the refreshing action.
        /// </summary>
        /// <param name="pullToRefresh">Instance of PullToRefresh.</param>
        void Refreshing(object pullToRefresh);

        /// <summary>
        /// Invoked when the view refreshing action is completed.
        /// </summary>
        /// <param name="pullToRefresh">Instance of PullToRefresh.</param>
        void Refreshed(object pullToRefresh);

        /// <summary>
        /// Invoked when the pulling action is cancelled before the progress meeting 100.
        /// </summary>
        /// <param name="pullToRefresh">Instance of PullToRefresh.</param>
        void PullingCancelled(object pullToRefresh);

        /// <summary>
        /// Method that determines whether the gesture should be handled by the child elements or not.
        /// </summary>
        /// <param name="pullToRefresh">Instance of PullToRefresh.</param>
        /// <returns>Returns a boolean value indicating whether the child elements can handle gestures.</returns>
        bool CanHandleGesture(object pullToRefresh);
    }
}
