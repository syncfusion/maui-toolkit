namespace Syncfusion.Maui.Toolkit.Picker
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Toolkit;
    using Syncfusion.Maui.Toolkit.Internals;

    /// <summary>
    /// Custom class for Image.
    /// </summary>
    internal class SfImage : Image
    {
    }

    /// <summary>
    /// Represents the close button rendered inside the picker header.
    /// </summary>
    internal class PickerCloseButton : SfContentView, ITouchListener
    {
        #region Bindable Properties

        /// <summary>
        /// Bindable stroke color for the close-icon. Bound to theme dynamic resource.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(
                nameof(Stroke),
                typeof(Color),
                typeof(PickerCloseButton),
                Color.FromArgb("#49454F"));

        /// <summary>
        /// Bindable stroke thickness for the close-icon.
        /// </summary>
        public static readonly BindableProperty IconStrokeThicknessProperty =
            BindableProperty.Create(
                nameof(IconStrokeThickness),
                typeof(double),
                typeof(PickerCloseButton),
                1.8d);

        /// <summary>
        /// Background color for the hover highlight.
        /// </summary>
        public static readonly BindableProperty HoverBackgroundProperty =
            BindableProperty.Create(
                nameof(HoverBackground),
                typeof(Color),
                typeof(PickerCloseButton),
                Color.FromArgb("#1449454F"));

        /// <summary>
        /// Background color for the pressed highlight.
        /// </summary>
        public static readonly BindableProperty PressedBackgroundProperty =
            BindableProperty.Create(
                nameof(PressedBackground),
                typeof(Color),
                typeof(PickerCloseButton),
                Color.FromArgb("#00000000"));

        #endregion

        #region Fields

        /// <summary>
        /// The picker instance that owns this button.
        /// </summary>
        readonly PickerBase _picker;

        /// <summary>
        /// Image view that displays the custom close icon when specified.
        /// </summary>
        private readonly SfImage _closeButtonImage;

        /// <summary>
        /// Indicates whether the pointer is hovering the button.
        /// </summary>
        private bool _isHover;

        /// <summary>
        /// Indicates whether the button is in pressed state.
        /// </summary>
        private bool _isPressed;

        /// <summary>
        /// Center used to draw the highlight and hover.
        /// </summary>
        private PointF _hoverCenter;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerCloseButton"/> class.
        /// </summary>
        /// <param name="picker">The picker instance associated with the close button.</param>
        internal PickerCloseButton(PickerBase picker)
        {
            _picker = picker;
            _hoverCenter = new PointF(20, 20);

            //// Ensure the close button receives pointer events even when visually transparent.
            InputTransparent = false;
            BackgroundColor = Colors.Transparent;
            _closeButtonImage = new SfImage
            {
                Style = new Style(typeof(SfImage)),
                HeightRequest = 24,
                WidthRequest = 24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false,
            };

            //// Draw the close button above the header content so it overlays the header.
            DrawingOrder = DrawingOrder.AboveContent;
            InitializeTheme();
            AddTapGesture();
            this.AddTouchListener(this);
            UpdateIcon();
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the Stroke color used to draw the X icon.
        /// </summary>
        private Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Stroke thickness used to draw the X icon.
        /// </summary>
        private double IconStrokeThickness
        {
            get { return (double)GetValue(IconStrokeThicknessProperty); }
            set { SetValue(IconStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Color used to draw the hover highlight.
        /// </summary>
        private Color HoverBackground
        {
            get { return (Color)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Color used to draw the pressed highlight.
        /// </summary>
        private Color PressedBackground
        {
            get { return (Color)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pointer event handler for hover/press states.
        /// </summary>
        /// <param name="e">Represents the event data.</param>
        public void OnTouch(Toolkit.Internals.PointerEventArgs e)
        {
            // Process pointer events for hover/pressed visuals regardless of whether
            // a custom CloseButtonIcon is provided so the highlight works for both cases.
            switch (e.Action)
            {
                case PointerActions.Entered:
                    _isHover= true;
                    InvalidateDrawable();
                    break;
                case PointerActions.Exited:
                    _isHover = false;
                    _isPressed = false;
                    InvalidateDrawable();
                    break;
                case PointerActions.Pressed:
                    _isPressed = true;
                    _isHover = false;
                    InvalidateDrawable();
                    break;
                case PointerActions.Released:
                    _isPressed = false;
                    InvalidateDrawable();
                    break;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the image/icon displayed inside the button.
        /// </summary>
        internal void UpdateIcon()
        {
            if (_picker.CloseButtonIcon != null)
            {
                _closeButtonImage.Source = _picker.CloseButtonIcon;
                _closeButtonImage.IsVisible = true;
                if (Content != _closeButtonImage)
                {
                    Content = _closeButtonImage;
                }

                InvalidateDrawable();
            }
            else
            {
                _closeButtonImage.IsVisible = false;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Initializes themes according to the Picker's Parent.
        /// </summary>
        internal void InitializeTheme()
        {
            if (_picker is SfPicker)
            {
                SetDynamicResource(StrokeProperty, "SfPickerNormalCloseButtonStroke");
                SetDynamicResource(IconStrokeThicknessProperty, "SfPickerNormalCloseButtonStrokeThickness");
                SetDynamicResource(HoverBackgroundProperty, "SfPickerHoverCloseButtonBackground");
                SetDynamicResource(PressedBackgroundProperty, "SfPickerPressedCloseButtonBackground");
            }
            else if (_picker is SfDatePicker)
            {
                SetDynamicResource(StrokeProperty, "SfDatePickerNormalCloseButtonStroke");
                SetDynamicResource(IconStrokeThicknessProperty, "SfDatePickerNormalCloseButtonStrokeThickness");
                SetDynamicResource(HoverBackgroundProperty, "SfDatePickerHoverCloseButtonBackground");
                SetDynamicResource(PressedBackgroundProperty, "SfDatePickerPressedCloseButtonBackground");
            }
            else if (_picker is SfTimePicker)
            {
                SetDynamicResource(StrokeProperty, "SfTimePickerNormalCloseButtonStroke");
                SetDynamicResource(IconStrokeThicknessProperty, "SfTimePickerNormalCloseButtonStrokeThickness");
                SetDynamicResource(HoverBackgroundProperty, "SfTimePickerHoverCloseButtonBackground");
                SetDynamicResource(PressedBackgroundProperty, "SfTimePickerPressedCloseButtonBackground");
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Draws the close icon and interaction highlights.
        /// </summary>
        /// <param name="canvas">The Canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            if (_picker.CloseButtonIcon != null)
            {
                return;
            }

            float size = 14f;
            float halfSize = size / 2;
            var center = _hoverCenter;
            RectF iconBounds = new RectF(center.X - halfSize, center.Y - halfSize, size, size);
            canvas.StrokeColor = Stroke;
            canvas.StrokeSize = (float)IconStrokeThickness;
            canvas.DrawLine(iconBounds.Left, iconBounds.Top, iconBounds.Right, iconBounds.Bottom);
            canvas.DrawLine(iconBounds.Left, iconBounds.Bottom, iconBounds.Right, iconBounds.Top);
            DrawHoverHighlight(canvas, dirtyRect);
            DrawPressedHighlight(canvas, dirtyRect);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds the tap gesture that closes the picker popup.
        /// </summary>
        private void AddTapGesture()
        {
            var tapGesture = new TapGestureRecognizer() { Command = new Command(OnCloseTapped) };
            GestureRecognizers.Add(tapGesture);
        }

        /// <summary>
        /// Invoked when the close button is tapped.
        /// </summary>
        private void OnCloseTapped()
        {
            _picker.IsOpen = false;
        }

        /// <summary>
        /// Draws the hover highlight when the pointer is over the button.
        /// </summary>
        private void DrawHoverHighlight(ICanvas canvas, RectF dirtyRect)
        {
            if (!_isHover)
            {
                return;
            }

            canvas.FillColor = HoverBackground;
            canvas.FillCircle(_hoverCenter, 20);
        }

        /// <summary>
        /// Draws the pressed highlight when the button is pressed.
        /// </summary>
        private void DrawPressedHighlight(ICanvas canvas, RectF dirtyRect)
        {
            if (!_isPressed)
            {
                return;
            }

            canvas.FillColor = PressedBackground;
            canvas.FillCircle(_hoverCenter, 20);
        }

        #endregion
    }
}