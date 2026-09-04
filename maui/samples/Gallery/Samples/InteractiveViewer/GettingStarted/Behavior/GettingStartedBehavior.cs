namespace Syncfusion.Maui.ControlsGallery.InteractiveViewer.SfInteractiveViewer
{
    using Syncfusion.Maui.Toolkit.Buttons;
    using Syncfusion.Maui.Toolkit.InteractiveViewer;

    /// <summary>
    /// Behavior class for handling interactions in the GettingStarted sample view.
    /// </summary>
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        #region Fields

        /// <summary>
        /// The Interactive viewer instance.
        /// </summary>
        SfInteractiveViewer? _interactiveViewer;

        /// <summary>
        /// The zoom out button.
        /// </summary>
        SfButton? _zoomOutButton;

        /// <summary>
        /// The zoom in button.
        /// </summary>
        SfButton? _zoomInButton;

        /// <summary>
        /// The rotate button.
        /// </summary>
        SfButton? _rotateButton;

        /// <summary>
        /// The reset button.
        /// </summary>
        SfButton? _resetButton;

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked when behavior is attached to a view.
        /// </summary>
        /// <param name="sampleView">The sample view to which the behavior is attached.</param>
        protected override void OnAttachedTo(SampleView sampleView)
        {
            base.OnAttachedTo(sampleView);
            _interactiveViewer = sampleView.Content.FindByName<SfInteractiveViewer>("interactiveViewer");
            _zoomOutButton = sampleView.Content.FindByName<SfButton>("zoomOutButton");
            _zoomInButton = sampleView.Content.FindByName<SfButton>("zoomInButton");
            _rotateButton = sampleView.Content.FindByName<SfButton>("rotateButton");
            _resetButton = sampleView.Content.FindByName<SfButton>("resetButton");

            _zoomInButton?.Clicked += OnZoomInClicked;
            _zoomOutButton?.Clicked += OnZoomOutClicked;
            _rotateButton?.Clicked += OnRotateClicked;
            _resetButton?.Clicked += OnResetClicked;
        }

        /// <summary>
        /// Invoked when behavior is detached from a view.
        /// </summary>
        /// <param name="sampleView">The sample view from which the behavior is detached.</param>
        protected override void OnDetachingFrom(SampleView sampleView)
        {
            base.OnDetachingFrom(sampleView);
            _zoomInButton?.Clicked -= OnZoomInClicked;
            _zoomOutButton?.Clicked -= OnZoomOutClicked;
            _rotateButton?.Clicked -= OnRotateClicked;
            _resetButton?.Clicked -= OnResetClicked;

            _interactiveViewer = null;
            _zoomOutButton = null;
            _zoomInButton = null;
            _rotateButton = null;
            _resetButton = null;
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Occurs when the zoom in button is clicked.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event args.</param>
        void OnZoomInClicked(object? sender, EventArgs e)
        {
            if (_interactiveViewer == null)
            {
                return;
            }

            _interactiveViewer.ZoomFactor = Math.Min(_interactiveViewer.MaximumZoomFactor, _interactiveViewer.ZoomFactor + 0.25);
        }

        /// <summary>
        /// Occurs when the zoom out button is clicked.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event args.</param>
        void OnZoomOutClicked(object? sender, EventArgs e)
        {
            if (_interactiveViewer == null)
            {
                return;
            }

            _interactiveViewer.ZoomFactor = Math.Max(_interactiveViewer.MinimumZoomFactor, _interactiveViewer.ZoomFactor - 0.25);
        }

        /// <summary>
        /// Occurs when the rotate button is clicked.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event args.</param>
        void OnRotateClicked(object? sender, EventArgs e)
        {
            _interactiveViewer?.Rotate();
        }

        /// <summary>
        /// Occurs when the reset button is clicked.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event args.</param>
        void OnResetClicked(object? sender, EventArgs e)
        {
            _interactiveViewer?.Reset();
        }

        #endregion
    }
}