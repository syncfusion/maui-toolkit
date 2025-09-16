using Syncfusion.Maui.Toolkit.Internals;
using Color = Microsoft.Maui.Graphics.Color;

#if WINDOWS
using WColor = Windows.UI.Color;
using GradientBrush = Microsoft.UI.Xaml.Media.GradientBrush;
#elif ANDROID
using Android.Content;
using Android.OS;
using Android.Util;
#elif MACCATALYST||IOS
using UIKit;
#endif

namespace Syncfusion.Maui.Toolkit.OtpInput;

    /// <summary>
    /// Represents a custom entry control for OTP input.
    /// </summary>
    internal class OTPEntry:Entry,IDrawable,ITouchListener
    {
        #region Fields

        /// <summary>
        /// A reference to the associated SfOtpInput control.
        /// </summary>
        OtpInputStyle _styleMode = OtpInputStyle.Outlined;

        /// <summary>
        /// Defines the corner radius for drawing rounded rectangles, affecting all style modes.
        /// </summary>
        double _cornerRadius;

        /// <summary>
        /// Represents the starting point for line drawing, typically used for underlining.
        /// </summary>
        PointF _startPoint;

        /// <summary>
        /// Represents the ending point for line drawing, typically used for underlining.
        /// </summary>
        PointF _endPoint;

        /// <summary>
        ///A reference to the associated SfOtpInput control.
        /// </summary>
        SfOtpInput? _sfOtpInput;

        /// <summary>
        /// Indicates whether the input field is being hovered over.
        /// </summary>
        bool _isHovered = false;

        /// <summary>
        /// Indicates whether the input field is enabled.
        /// </summary>
        bool _isEnabled = false;

        /// <summary>
        /// Captures the current OTP input state such as success, error, or warning.
        /// </summary>
        OtpInputState _inputState;


        /// <summary>
        /// Specifies the default background color for the control when rendered.
        /// </summary>
        Color _background = Color.FromArgb("#E7E0EC");

        /// <summary>
        /// Specifies the default stroke color for the control boundary.
        /// </summary>
        Color _stroke = Color.FromArgb("#49454F");

		/// <summary>
		/// Specifies the default disabled text color for the control
		/// </summary>
		Color _disabledTextColor = Color.FromArgb("#611c1b1f");

		/// <summary>
		/// Specifies the default text color for the control
		/// </summary>
		Color _textColor = Color.FromArgb("#1C1B1F");

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OTPEntry"/> class.
        /// </summary>
        internal OTPEntry()
        {
            this.AddTouchListener(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text value of the control.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the text value.
        /// </value>
        internal new string Text
        {
            get
            {
                return string.IsNullOrEmpty(base.Text) ? string.Empty : base.Text;
            }
            set
            {
                if (value is not "\0")
                {
                    base.Text = value;
                }
            }
        }

		#endregion

        #region Public Methods

        /// <summary>
        /// Draws the OTP input control based on its style mode, which can be outlined, filled, or underlined.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing the control.</param>
        /// <param name="dirtyRect">The rectangle area that needs to be redrawn.</param>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = _stroke;
            Background = _background;
            switch (_styleMode)
            {
                case OtpInputStyle.Outlined:
                    canvas.DrawRoundedRectangle(dirtyRect, _cornerRadius);
					Background = Colors.Transparent;
                    break;

                case OtpInputStyle.Filled:
                    canvas.DrawLine(_startPoint, _endPoint);
                    canvas.FillColor = _background;
                    canvas.FillRoundedRectangle(dirtyRect, _cornerRadius, _cornerRadius, 0, 0);
                    break;

                case OtpInputStyle.Underlined:
                    canvas.DrawLine(_startPoint, _endPoint);
					Background = Colors.Transparent;
					break;
            }
        }

        /// <summary>
        /// Handles touch events for the OTP entry control to manage its hover and normal states.
        /// </summary>
        /// <param name="e">The event arguments containing details about the touch action.</param>
        public void OnTouch(Toolkit.Internals.PointerEventArgs e)
        {
             bool isEntered = e.Action is PointerActions.Entered;
            _isHovered = isEntered;
            _sfOtpInput?.InvalidateDrawable();
        }

	#endregion

		#region Internal Methods

	/// <summary>
	/// Updates parameters used for rendering the OTP input control, such as style, corner radius, and focus state.
	/// </summary>
	/// <param name="StylingMode">The style mode for the OTP input (e.g., outlined, filled, underlined).</param>
	/// <param name="cornerRadius">The corner radius for rounded edges.</param>
	/// <param name="startPoint">The start point for drawing lines or shapes.</param>
	/// <param name="endPoint">The end point for drawing lines or shapes.</param>
	/// <param name="sfotpinput">The instance of the OTP input control.</param>
	/// <param name="isEnabled">Indicates whether the control is enabled.</param>
	/// <param name="inputState">Input state for the OTP input.</param>
	/// <param name="background">Background for the OTP input.</param>
	/// <param name="stroke">Stroke for the OTP input.</param>
	/// <param name="textColor">TextColor for the OTP input.</param>
	/// <param name="disabledTextColor">Disabled texxt Color for the OTP input.</param>
	internal void UpdateParameters(OtpInputStyle StylingMode, double cornerRadius, PointF startPoint, PointF endPoint, SfOtpInput sfotpinput, bool isEnabled, OtpInputState inputState,Color stroke, Color background,Color textColor, Color disabledTextColor)
        {
            _styleMode = StylingMode;
            _cornerRadius = cornerRadius;
            _startPoint = startPoint;
            _endPoint = endPoint;
            _sfOtpInput = sfotpinput;
            _isEnabled = isEnabled;
            _inputState = inputState;
			_background = background;
			_stroke = stroke;
            _textColor = textColor;
			_disabledTextColor = disabledTextColor;

             GetVisualState();
        }

	/// <summary>
	/// Determines and sets the visual state based on the control's focus, hover, and enabled state.
	/// </summary>
	void GetVisualState()
	{
		if (_sfOtpInput != null)
		{
			if (_isEnabled)
			{
				if (IsFocused)
				{
					_stroke = _sfOtpInput.FocusedStroke;

					 _background = _styleMode == OtpInputStyle.Filled 
                        ? _sfOtpInput.InputBackground 
                        : Colors.Transparent;

				}
				else if (_isHovered && !IsFocused)
				{
					_stroke = _sfOtpInput.HoveredStroke;

					_background = _styleMode == OtpInputStyle.Filled 
                        ? _sfOtpInput.FilledHoverBackground 
                        : Colors.Transparent;
				}
				else
				{
					// Apply specific state strokes and backgrounds for success, error, warning, or default.
                    _stroke = _inputState switch
                    {
                        OtpInputState.Success => _sfOtpInput.SuccessStroke,
                        OtpInputState.Error => _sfOtpInput.ErrorStroke,
                        OtpInputState.Warning => _sfOtpInput.WarningStroke,
                        _ => _sfOtpInput.Stroke
                    };

					 _background = _styleMode == OtpInputStyle.Filled 
                        ? _sfOtpInput.InputBackground 
                        : Colors.Transparent;
				}

                TextColor = _textColor;
			}
			else
			{
				// The control is disabled: apply disable styles.
                _stroke = _sfOtpInput.DisabledStroke;
                _background = _styleMode == OtpInputStyle.Filled 
                    ? _sfOtpInput.FilledDisableBackground 
                    : Colors.Transparent;
                TextColor = _disabledTextColor;
			}

		}

	}

	#endregion

        #region Override Methods

        /// <summary>
        /// Invoked when the handler for the control is initialized or changed. 
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler is null) return;

            switch (Handler.PlatformView)
            {
#if WINDOWS
                case Microsoft.UI.Xaml.Controls.TextBox textbox:
                    OptimizeWindowsTextBox(textbox);
					textbox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(4, 4, 0, 0);
                    break;
#elif ANDROID
                case AndroidX.AppCompat.Widget.AppCompatEditText textbox:
                    textbox.Background = null;
					textbox.SetPadding(0, 0, 0, 0); 
					textbox.SetIncludeFontPadding(false);
					textbox.Gravity = Android.Views.GravityFlags.Center;
					textbox.TextAlignment = Android.Views.TextAlignment.Center;
                    break;
#elif MACCATALYST || IOS
                case UIKit.UITextField textField:
                    textField.BorderStyle = UITextBorderStyle.None;
                    textField.BackgroundColor = UIKit.UIColor.Clear;
					textField.Layer.CornerRadius = 4;
					textField.TextAlignment = UIKit.UITextAlignment.Center; 
					textField.VerticalAlignment = UIKit.UIControlContentVerticalAlignment.Center;
					textField.AutocorrectionType = UITextAutocorrectionType.No;
					break;
#endif
			default: break;
            }
        }

#if WINDOWS
        /// <summary>
        /// Applies specific optimizations to Windows TextBox controls to align with custom styles.
        /// </summary>
        /// <param name="textbox">The TextBox control being optimized.</param>
        void OptimizeWindowsTextBox(Microsoft.UI.Xaml.Controls.TextBox textbox)
        {
            // Set the border thickness to none to remove default stylings.
            textbox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			textbox.Padding = new Microsoft.UI.Xaml.Thickness(0);
			textbox.TextAlignment = Microsoft.UI.Xaml.TextAlignment.Center;
			textbox.VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
			textbox.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
			// Update resource properties for removing border under focus.
			textbox.Resources["TextControlBorderThemeThicknessFocused"] = textbox.BorderThickness;
            
            // Safely handle the brush resource for focus with null checking.
            if (textbox.Resources["TextControlBorderBrushFocused"] is GradientBrush brush && brush.GradientStops is not null)
            {
                var color = brush.GradientStops.FirstOrDefault()?.Color;
            }
        }
#endif

	#endregion
}

