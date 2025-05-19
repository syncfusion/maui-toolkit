using Syncfusion.Maui.Toolkit.ProgressBar;

namespace Syncfusion.Maui.ControlsGallery.ProgressBar.SfCircularProgressBar;

public partial class CircularProgressBar : SampleView
{
    #region Fields

    /// <summary>
    /// Indicates whether the progress bars are currently running.
    /// </summary>
    bool isRunning = true;

    /// <summary>
    /// Indicates whether the view has been disposed.
    /// </summary>
    bool isDisposed;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CircularProgressBar"/> class.
    /// </summary>
    public CircularProgressBar()
    {
        InitializeComponent();

        #region Custom Content

        this.SetCustomContentProgress();
        this.SetVideoPlayerContent();

        #endregion

        #region Circular Radius

        this.SetTrackInsideProgress();

        #endregion

        #region Circular Segment

        this.SetSegmentedFillingStyleProgress();

        #endregion
    }

    #endregion

    #region Private Methods

    #region Custom Content

    /// <summary>
    /// Initializes a timer to update the progress of the video player progress bar every 50 milliseconds.
    /// </summary>
    /// <remarks>
    /// The timer continues to run as long as the <see cref="isRunning"/> flag is true.
    /// </remarks>
    private void SetVideoPlayerContent()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            Dispatcher.DispatchAsync(() =>
            {
                this.SetProgress();
            });

            return isRunning;
        });
    }

    /// <summary>
    /// Updates the progress of the video player progress bar.
    /// </summary>
    /// <remarks>
    /// Increments the progress by 0.5. When the progress reaches 100, it resets to 0.
    /// </remarks>
    private void SetProgress()
    {
        this.VideoPlayerProgressBar.Progress += 0.5;
        if (this.VideoPlayerProgressBar.Progress == 100)
        {
            this.VideoPlayerProgressBar.Progress = 0;
        }
    }

    /// <summary>
    /// Initializes a timer to update the custom content circular progress bar and its label every 50 milliseconds.
    /// </summary>
    /// <remarks>
    /// The progress stops updating when it reaches 74.
    /// </remarks>
    private void SetCustomContentProgress()
    {
        double progress = 0;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            Dispatcher.DispatchAsync(() =>
            {
                this.CustomContentCircularProgressBar.Progress = progress += 1;
                this.CustomContentProgressBarLabel.Text = progress + "%";
            });

            return progress < 74;
        });
    }

    /// <summary>
    /// Handles the click event of the play button to toggle the video player's progress updates.
    /// </summary>
    /// <param name="sender">The source of the event, typically the play button.</param>
    /// <param name="e">An instance of <see cref="EventArgs"/> containing the event data.</param>
    /// <remarks>
    /// Toggles the <see cref="isRunning"/> flag and updates the play button's text.
    /// If the video player is running, it restarts the progress updates.
    /// </remarks>
    private void PlayButton_Clicked(object sender, EventArgs e)
    {
        isRunning = !isRunning;
        this.PlayButton.Text = isRunning ? "\ue769" : "\ue737";
        if (isRunning)
        {
            this.SetVideoPlayerContent();
        }
    }

    #endregion

    #region Circular Radius

    /// <summary>
    /// Handles the ProgressChanged event for the TrackOutsideProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the TrackOutsideProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    /// <remarks>
    /// This method toggles the progress of the TrackOutsideProgressBar between 0 and 100.
    /// When the progress reaches 100, it resets to 0. When it reaches 0, it sets the progress back to 100.
    /// </remarks>
    private void TrackOutsideProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                // Set progress to 100 when it reaches 0.
                this.TrackOutsideProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                // Reset progress to 0 when it reaches 100.
                this.TrackOutsideProgressBar.SetProgress(0, 0);
            }
        });
    }

    /// <summary>
    /// Handles the ProgressChanged event for the FilledIndicatorProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the FilledIndicatorProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    /// <remarks>
    /// This method toggles the progress of the FilledIndicatorProgressBar between 0 and 100.
    /// When the progress reaches 100, it resets to 0. When it reaches 0, it sets the progress back to 100.
    /// </remarks>
    private void FilledIndicatorProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                // Set progress to 100 when it reaches 0.
                this.FilledIndicatorProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                // Reset progress to 0 when it reaches 100.
                this.FilledIndicatorProgressBar.SetProgress(0, 0);
            }
        });
    }

    /// <summary>
    /// Handles the ProgressChanged event for the ThinTrackStyle progress bar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the ThinTrackStyle progress bar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    /// <remarks>
    /// This method toggles the progress of the ThinTrackStyle progress bar between 0 and 100.
    /// When the progress reaches 100, it resets to 0. When it reaches 0, it sets the progress back to 100.
    /// </remarks>
    private void ThinTrackStyle_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                // Set progress to 100 when it reaches 0.
                this.ThinTrackStyle.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                // Reset progress to 0 when it reaches 100.
                this.ThinTrackStyle.SetProgress(0, 0);
            }
        });
    }

    /// <summary>
    /// Sets the progress for the TrackInsideProgressBar and updates the associated label.
    /// </summary>
    /// <remarks>
    /// This method initializes a timer that updates the progress of the TrackInsideProgressBar every 50 milliseconds.
    /// When the progress reaches 100, it resets to 0 and continues.
    /// The timer stops when the view is disposed.
    /// </remarks>
    private void SetTrackInsideProgress()
    {
        double progress = 0;
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            Dispatcher.DispatchAsync(() =>
            {
                // Increment progress by 0.25 and update the progress bar and label.
                this.TrackInsideProgressBar.Progress = progress += 0.25;
                this.TrackInsideProgressBarProgressLabel.Text = (int)progress + "%";

                // Reset progress to 0 when it reaches 100.
                if (progress == 100)
                {
                    this.TrackInsideProgressBar.Progress = progress = 0;
                }
            });

            // Continue the timer until the view is disposed.
            return !isDisposed;
        });
    }

    #endregion

    #region Circular Segment

    /// <summary>
    /// Handles the ProgressChanged event for the SegmentedCircularProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the SegmentedCircularProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    /// <remarks>
    /// This method toggles the progress of the SegmentedCircularProgressBar between 0 and 75.
    /// When the progress reaches 75, it resets to 0. When it reaches 0, it sets the progress back to 75.
    /// </remarks>
    private void SegmentedCircularProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(75))
            {
                // Reset progress to 0 when it reaches 75.
                this.SegmentedCircularProgressBar.SetProgress(0, 0);
            }

            if (e.Progress.Equals(0))
            {
                // Set progress to 75 when it reaches 0.
                this.SegmentedCircularProgressBar.Progress = 75;
            }
        });
    }

    /// <summary>
    /// Handles the ProgressChanged event for the SegmentedPaddingCircularProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the SegmentedPaddingCircularProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    private void SegmentedPaddingCircularProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(75))
            {
                // Reset progress to 0 when it reaches 75.
                this.SegmentedPaddingCircularProgressBar.SetProgress(0, 0);
            }

            if (e.Progress.Equals(0))
            {
                // Set progress to 75 when it reaches 0.
                this.SegmentedPaddingCircularProgressBar.Progress = 75;
            }
        });
    }

    /// <summary>
    /// Sets the progress for the SegmentedFillingStyle progress bar.
    /// </summary>
    /// <remarks>
    /// This method initializes a timer that updates the progress of the SegmentedFillingStyle progress bar every 300 milliseconds.
    /// When the progress reaches 100, it resets to 0 and continues.
    /// The timer stops when the view is disposed.
    /// </remarks>
    private void SetSegmentedFillingStyleProgress()
    {
        double progress = 0;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(300), () =>
        {
            if (progress == 100)
            {
                // Reset progress to 0 when it reaches 100.
                this.SegmentedFillingStyle.Progress = progress = 0;
            }

            Dispatcher.DispatchAsync(() =>
            {
                // Increment progress by 5.
                this.SegmentedFillingStyle.Progress = progress += 5;
            });

            // Continue the timer until the view is disposed.
            return !isDisposed;
        });
    }

    #endregion

    #region Custom Angle

    /// <summary>
    /// Handles the ProgressChanged event for the AngleCustomizationProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the AngleCustomizationProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    private void AngleCustomizationProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                // Reset progress to 100 when it reaches 0.
                this.AngleCustomizationProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                // Reset progress to 0 when it reaches 100.
                this.AngleCustomizationProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #region Range Colors

    /// <summary>
    /// Handles the ProgressChanged event for the RangeColorProgressBar.
    /// </summary>
    /// <param name="sender">The source of the event, typically the RangeColorProgressBar.</param>
    /// <param name="e">An instance of <see cref="ProgressValueEventArgs"/> containing the progress value.</param>
    private void RangeColorProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    {
        Dispatcher.DispatchAsync(() =>
        {
            if (e.Progress.Equals(0))
            {
                // Reset progress to 100 when it reaches 0.
                this.RangeColorProgressBar.Progress = 100;
            }

            if (e.Progress.Equals(100))
            {
                // Reset progress to 0 when it reaches 100.
                this.RangeColorProgressBar.SetProgress(0, 0);
            }
        });
    }

    #endregion

    #endregion

    #region Override Methods

    /// <summary>
    /// Called when the view is disappearing.
    /// </summary>
    /// <remarks>
    /// This method is overridden to handle cleanup tasks when the view is no longer visible.
    /// It stops any running timers and marks the view as disposed to prevent further updates.
    /// </remarks>
    public override void OnDisappearing()
    {
        base.OnDisappearing();
        isRunning = false;
        isDisposed = true;
    }

    #endregion
}