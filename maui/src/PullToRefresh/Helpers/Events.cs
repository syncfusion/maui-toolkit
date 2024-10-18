using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
    /// <summary>
    /// Provides data for the <see cref="SfPullToRefresh.Pulling"/> event.
    /// </summary>
    public class PullingEventArgs : CancelEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets the progress of <see cref="SfPullToRefresh.Pulling"/>.
        /// </summary>
        /// <value>The progress of pulling.</value>
        public double Progress { get; set; }

        #endregion
    }
}
