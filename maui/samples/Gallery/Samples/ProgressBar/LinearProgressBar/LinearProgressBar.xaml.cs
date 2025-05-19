using Syncfusion.Maui.Toolkit.ProgressBar;

namespace Syncfusion.Maui.ControlsGallery.ProgressBar.SfLinearProgressBar;

public partial class LinearProgressBar : SampleView
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearProgressBar"/> class.
    /// </summary>
    public LinearProgressBar()
    {
        InitializeComponent();
    }

    #endregion

    #region Private Methods

    #region Corner Radius

    /// <summary>
    /// Handles the progress change event for the CornerRadiusProgressBar.
    /// Resets the progress to 100 when it reaches 0, and resets it to 0 when it reaches 100.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void CornerRadiusProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.CornerRadiusProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.CornerRadiusProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Padding Progress Bar

    /// <summary>
    /// Handles the progress change event for the PaddingProgressBar.
    /// Resets the progress to 100 when it reaches 0, and resets it to 0 when it reaches 100.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void PaddingProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.PaddingProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.PaddingProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Range Progress Bar

    /// <summary>
    /// Handles the progress change event for the RangeProgressBar.
    /// Resets the progress to 100 when it reaches 0, and resets it to 0 when it reaches 100.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void RangeProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.RangeProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.RangeProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Gradient Progress Bar

    /// <summary>
    /// Handles the progress change event for the GradientProgressBar.
    /// Resets the progress to 100 when it reaches 0, and resets it to 0 when it reaches 100.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void GradientProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.GradientProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.GradientProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Linear Buffer

    /// <summary>
    /// Handles the progress change event for the SecondaryProgressProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void SecondaryProgressProgressBar_ValueChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.SecondaryProgressProgressBar.SecondaryAnimationDuration = 1000;
                this.SecondaryProgressProgressBar.SecondaryProgress = 100;
                this.SecondaryProgressProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.SecondaryProgressProgressBar.SecondaryAnimationDuration = 0;
                this.SecondaryProgressProgressBar.SecondaryProgress = 0;
                this.SecondaryProgressProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Segmented Corner Radius

    /// <summary>
    /// Handles the progress change event for the SegmentedCornerRadiusProgressBar.
    /// Resets the progress to 100 when it reaches 0, and resets it to 0 when it reaches 100.
    /// </summary>
    /// <param name="sender">The source of the event, typically the progress bar.</param>
    /// <param name="e">The event data containing the current progress value.</param>
    private void SegmentedCornerRadiusProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                this.SegmentedCornerRadiusProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                this.SegmentedCornerRadiusProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #endregion
}