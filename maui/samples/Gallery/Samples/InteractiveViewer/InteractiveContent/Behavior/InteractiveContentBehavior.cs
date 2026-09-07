namespace Syncfusion.Maui.ControlsGallery.InteractiveViewer.SfInteractiveViewer
{
    using Syncfusion.Maui.Toolkit.Buttons;
    using Syncfusion.Maui.Toolkit.InteractiveViewer;

    /// <summary>
    /// Behavior class for handling interactions in the InteractiveContent sample view.
    /// </summary>
    public class InteractiveContentBehavior : Behavior<SampleView>
    {
        #region Fields

        /// <summary>
        /// The interactive viewer instance.
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

        /// <summary>
        /// The sign up button hosted inside the interactive viewer.
        /// </summary>
        SfButton? _signUpButton;

        /// <summary>
        /// The name entry hosted inside the interactive viewer.
        /// </summary>
        Entry? _nameEntry;

        /// <summary>
        /// The email entry hosted inside the interactive viewer.
        /// </summary>
        Entry? _emailEntry;

        /// <summary>
        /// The password entry hosted inside the interactive viewer.
        /// </summary>
        Entry? _passwordEntry;

        /// <summary>
        /// The label that shows the submission result.
        /// </summary>
        Label? _statusLabel;

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
            _signUpButton = sampleView.Content.FindByName<SfButton>("signUpButton");
            _nameEntry = sampleView.Content.FindByName<Entry>("nameEntry");
            _emailEntry = sampleView.Content.FindByName<Entry>("emailEntry");
            _passwordEntry = sampleView.Content.FindByName<Entry>("passwordEntry");
            _statusLabel = sampleView.Content.FindByName<Label>("statusLabel");

            _zoomInButton?.Clicked += OnZoomInClicked;
            _zoomOutButton?.Clicked += OnZoomOutClicked;
            _rotateButton?.Clicked += OnRotateClicked;
            _resetButton?.Clicked += OnResetClicked;
            _signUpButton?.Clicked += OnSignUpClicked;
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
            _signUpButton?.Clicked -= OnSignUpClicked;

            _interactiveViewer = null;
            _zoomOutButton = null;
            _zoomInButton = null;
            _rotateButton = null;
            _resetButton = null;
            _signUpButton = null;
            _nameEntry = null;
            _emailEntry = null;
            _passwordEntry = null;
            _statusLabel = null;
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

        /// <summary>
        /// Occurs when the hosted sign up button is clicked.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event args.</param>
        void OnSignUpClicked(object? sender, EventArgs e)
        {
            if (_statusLabel == null)
            {
                return;
            }

            var name = _nameEntry?.Text?.Trim();
            var email = _emailEntry?.Text?.Trim();
            var password = _passwordEntry?.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _statusLabel.Text = "Please fill in all the fields.";
                _statusLabel.TextColor = Colors.OrangeRed;
            }
            else
            {
                _statusLabel.Text = $"Welcome, {name}! Your account is ready.";
                _statusLabel.TextColor = Colors.SeaGreen;
            }

            _statusLabel.IsVisible = true;
        }

        #endregion
    }
}