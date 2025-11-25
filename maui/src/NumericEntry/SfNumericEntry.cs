using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.EntryRenderer;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.EntryView;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;
#if ANDROID
using Android.Text;
using Android.Provider;
using Android.Content;
using Android.Views.InputMethods;
using System.Linq;
#elif MACCATALYST || IOS
using UIKit;
using Foundation;
using Microsoft.Maui.ApplicationModel;
#if IOS
using CoreGraphics;
#endif
#endif

namespace Syncfusion.Maui.Toolkit.NumericEntry
{
    /// <summary>
    /// Represents a control that can be used to display and edit numbers.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <numeric:SfNumericEntry Placeholder = "Enter your age"
    ///                         Value="12" />
    /// ]]></code>
    /// </example>
    public partial class SfNumericEntry : SfView, ITextElement, ITouchListener, IKeyboardListener, IParentThemeElement, ITextInputLayout
    {
		#region Fields

		/// <summary>
		/// Renderer for the dropdown component.
		/// </summary>
		internal IUpDownButtonRenderer? _entryRenderer;

		/// <summary>
		/// Rectangle for the clear button's area.
		/// </summary>
		internal RectF _clearButtonRectF;

		/// <summary>
		/// Renderer for effects applied to the control.
		/// </summary>
		EffectsRenderer? _effectsRenderer;

		/// <summary>
		/// Size of the buttons in the control.
		/// </summary>
		internal const int ButtonSize = 30;

		/// <summary>
		/// Space between buttons.
		/// </summary>
		int _buttonSpace;

		/// <summary>
		/// Stroke color for the clear icon.
		/// </summary>
		static readonly Color ClearIconStrokeColor = Colors.Black;

		/// <summary>
		/// Text input layout associated with the control.
		/// </summary>
		internal SfTextInputLayout? _textInputLayout;

		/// <summary>
		/// Previous bounds of the rectangle for layout calculations.
		/// </summary>
		internal RectF _previousRectBounds;

		/// <summary>
		/// Indicates if the clear button is visible.
		/// </summary>
		internal bool _isClearButtonVisible;

		/// <summary>
		/// Point representing the touch location.
		/// </summary>
		Point _touchPoint;

		/// <summary>
		/// Current state of the value in the control.
		/// </summary>
		internal ValueStates _valueStates;

		/// <summary>
		/// Size for the semantics of the clear button.
		/// </summary>
		internal Size _clearButtonSemanticsSize = Size.Zero;

		/// <summary>
		/// Indicates if the description has been set by the user.
		/// </summary>
		bool _isDescriptionNotSetByUser;

		/// <summary>
		/// List of semantic nodes for numeric entry.
		/// </summary>
		readonly List<SemanticsNode> _numericEntrySemanticsNodes = [];

		/// <summary>
		/// Represents the text entry control where user input is displayed.
		/// </summary>
		internal SfEntryView? _textBox;

		/// <summary>
		/// Gets or sets the maximum positive fraction digits. 
		/// </summary>
		int _maximumPositiveFractionDigit = -1;

		/// <summary>
		/// Gets or sets the maximum negative fraction digits. 
		/// </summary>
		int _maximumNegativeFractionDigit = -1;

		// Flag to avoid duplicate value changing event.
		// Use this flag only in 'OnValueChanged', do not use anywhere else.
		bool _valueUpdating = false;

#if MACCATALYST || IOS
        /// <summary>
        /// Indicates whether the current text in the UI entry is not valid.
        /// </summary>
        bool _isNotValidText = false;

        /// <summary>
        /// Represents the underlying iOS UI text field used in the control.
        /// </summary>
        UIKit.UITextField? _uiEntry;

        /// <summary>
        /// Represents the initial corner radius in the control.
        /// </summary>
        readonly CornerRadius _initialCornderRadius = 6;

#if IOS
		/// <summary>
		/// Button for returning to the previous screen.
		/// </summary>
		UIButton? _returnButton;

		/// <summary>
		/// Button for decreasing the numeric value.
		/// </summary>
		UIButton? _minusButton;

		/// <summary>
		/// Separator button in the UI.
		/// </summary>
		UIButton? _separatorButton;

		/// <summary>
		/// Views for the lines in the toolbar.
		/// </summary>
		UIView? _lineView1, _lineView2, _lineView3, _lineView4, _toolbarView;

		/// <summary>
		/// Width of the buttons in the toolbar.
		/// </summary>
		nfloat _buttonWidth;

		/// <summary>
		/// Observer for device rotation events.
		/// </summary>
		NSObject? _deviceRotatedObserver;

		/// <summary>
		/// Another separator string for UI layout.
		/// </summary>
		string? _anotherSeparator;

		/// <summary>
		/// Height of the toolbar in points.
		/// </summary>
		const float ToolbarHeight = 50.0f;

		/// <summary>
		/// Thickness of separator lines in points.
		/// </summary>
		const float LineThickness = 0.5f;

		/// <summary>
		/// Width threshold for small screens in points.
		/// </summary>
		const float SmallScreenWidth = 320.0f;

		/// <summary>
		/// Width threshold for medium screens in points.
		/// </summary>
		const float LargeScreenWidth = 667.0f;

		/// <summary>
		/// Divisor used to calculate button width.
		/// </summary>
		const int ButtonDivider = 3;

		/// <summary>
		/// Button spacing for small landscape screens.
		/// </summary>
		const float SmallLandscapeButtonSpacing = 2.5f;

		/// <summary>
		/// Separator spacing for small landscape screens.
		/// </summary>
		const float SmallLandscapeSeparatorSpacing = 5.0f;

		/// <summary>
		/// Button spacing for large landscape screens.
		/// </summary>
		const float LargeLandscapeButtonSpacing = 3.1f;

		/// <summary>
		/// Separator spacing for large landscape screens.
		/// </summary>
		const float LargeLandscapeSeparatorSpacing = 6.1f;

		/// <summary>
		/// Button spacing for medium portrait screens.
		/// </summary>
		const float MediumPortraitButtonSpacing = 2.0f;

		/// <summary>
		/// Separator spacing for medium portrait screens.
		/// </summary>
		const float MediumPortraitSeparatorSpacing = 4.0f;

		/// <summary>
		/// Button spacing for small portrait screens.
		/// </summary>
		const float SmallPortraitButtonSpacing = 1.5f;

		/// <summary>
		/// Separator spacing for small portrait screens.
		/// </summary>
		const float SmallPortraitSeparatorSpacing = 3.0f;

#endif
#elif WINDOWS
        /// <summary>
        /// Gets or sets the redo text.
        /// </summary>
        string? _redoText = null;

        /// <summary>
        /// Indicates whether the control has focus.
        /// </summary>
        internal bool _isFocus = false;

        /// <summary>
        ///  It can be used to store a reference to a parent view element.
        /// </summary>
        Microsoft.Maui.Platform.WrapperView? _grandParentElement;

        /// <summary>
        /// Indicates whether this is the initial assignment or interaction with the control
        /// (used specifically for first-time operations).
        /// </summary>
        bool _isFirst = true;

        /// <summary>
        /// Represents the initial corner radius in the control.
        /// </summary>
        readonly CornerRadius _initialCornderRadius = 5;
#elif ANDROID
        /// <summary>
        /// Indicates whether the samsung device use samsung keyboard or any other.
        /// </summary>
        bool _isSamsungWithSamsungKeyboard;

        /// <summary>
        /// Holds the length of text currently selected by the user.
        /// </summary>
        int _selectionLength;

        /// <summary>
        /// Represents the current cursor position within the text box.
        /// </summary>
        int _cursorPosition;

        /// <summary>
        /// Flag indicating whether cursor position adjustments should occur after modifications.
        /// </summary>
        bool _shiftCursor;

        /// <summary>
        /// Valid characters allowed for direct input within the text field.
        /// </summary>
        const string KEYS = "1234567890,.-";

        /// <summary>
        /// Indicates whether the text field is being focused for the first time, often used for initializing behavior.
        /// </summary>
        bool _isFirstFocus = true;

#endif

#if !WINDOWS
        /// <summary>
        /// Stores the text prior to the latest input modification.
        /// </summary>
        string _previousText = string.Empty;
#endif

#if MACCATALYST || IOS
		const double MinimumMargin = 7;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfNumericEntry"/> class.
		/// </summary>
		public SfNumericEntry()
		{
			ThemeElement.InitializeThemeResources(this, "SfNumericEntryTheme");
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SfNumericEntry"/> class.
		/// </summary>
		/// <param name="isInternallyCreated">To indicate internally created for SfNumericUpDown.</param>
		public SfNumericEntry(bool isInternallyCreated)
		{
			if (isInternallyCreated)
			{
				Initialize();
			}
		}

		/// <summary>
		/// Destructor of the <see cref="SfNumericEntry"/> class.
		/// </summary>
		~SfNumericEntry()
        {
            UnHookEvents();
        }

        #endregion

		#region Protected methods

		/// <summary>
		/// Method called when the control's parent changes, allowing for adjustments related to the change in hierarchy or parent container.
		/// </summary>
		protected override void OnParentChanged()
		{
			if (Parent != null)
			{
				ValidateParentIsTextInputLayout();
			}
			else
			{
				IsTextInputLayout = false;
				_textInputLayout = null;
			}
			base.OnParentChanged();
		}

		/// <summary>
		/// Arranges the content of the control within the specified bounds post-measurement.
		/// </summary>
		/// <param name="bounds">The rectangle that defines the bounds available for the content arrangement.</param>
		/// <returns>The actual size used by the arranged content, typically equal to the size of the bounds.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			UpdateElementsBounds(bounds);
			return base.ArrangeContent(bounds);
		}

		/// <summary>
		/// Measures the content of the control to determine the desired size based on the given constraints.
		/// </summary>
		/// <param name="widthConstraint">The maximum width available for the content.</param>
		/// <param name="heightConstraint">The maximum height available for the content.</param>
		/// <returns>The desired size for the content, which should be within the given constraints.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (_textBox == null)
            {
                return Size.Zero;
            }
    
            Size textBoxSize = _textBox.Measure(widthConstraint, heightConstraint);
            double measuredWidth = double.IsInfinity(widthConstraint) ? textBoxSize.Width : widthConstraint;
            double measuredHeight = double.IsInfinity(heightConstraint) ? textBoxSize.Height : heightConstraint;

            return new Size(measuredWidth, measuredHeight);
        }

		/// <summary>
		/// Called during the render pass to draw the control's content onto the canvas.
		/// </summary>
		/// <param name="canvas">The canvas to draw on, providing methods and properties for rendering.</param>
		/// <param name="dirtyRect">The rectangle area that needs to be redrawn, typically due to invalidation.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            DrawEntryUI(canvas, dirtyRect);

            #if WINDOWS || MACCATALYST || IOS
                bool needClip = false;

                #if WINDOWS && NET8_0
                    if (Parent is not Microsoft.Maui.Controls.Frame)
                    {
                        needClip = !IsRTL();
                    }
                #elif MACCATALYST || IOS
                        needClip = true;
                #endif

                if (needClip)
                {
                    Clip = new Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry(_initialCornderRadius, dirtyRect);
                }
            #endif
        }

		/// <inheritdoc cref="SfView.OnHandlerChanged()"/>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
#if ANDROID
			if (!IsLoaded && _textBox is not null)
			{
				_textBox.HeightRequest = HeightRequest;
			}
#endif
		}

		/// <summary>
		/// Called when a property value changes.
		/// This method is used to react to property updates, typically for updating UI or state.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property that changed. By default, this is automatically provided by the caller.
		/// </param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null || _textBox == null)
            {
                return;
            }

            switch (propertyName)
            {
                case nameof(IsEnabled):
					_textBox.IsEnabled = IsEnabled;
					break;

        #if WINDOWS
                case nameof(FlowDirection):
                    SetFlowDirection();
                    break;
		#endif
				case nameof(IsVisible):
					_textBox.IsVisible = IsVisible;
					break;
			}

            // Ensure the base method is always called
            base.OnPropertyChanged(propertyName);
        }

		/// <summary>
		/// Retrieves the list of semantics nodes for accessibility support,
		/// which represent how the control is perceived by assistive technologies.
		/// </summary>
		/// <param name="width">The width constraint for the semantics nodes.</param>
		/// <param name="height">The height constraint for the semantics nodes.</param>
		/// <returns>A list of semantics nodes that describe the accessible elements within this control.</returns>
		protected override List<SemanticsNode> GetSemanticsNodesCore(double width, double height)
		{
            // Ensure any necessary size updates are performed
			UpdateSemanticsSizes();
			if (SemanticsDataIsCurrent() || IsTextInputLayout)
			{
				if(IsTextInputLayout)
				{
					_numericEntrySemanticsNodes.Clear();
				}
				return _numericEntrySemanticsNodes;
			}

			if (_isClearButtonVisible)
			{
				AddSemanticsNode(_clearButtonRectF, 1, "Clear button");
			}

			return _numericEntrySemanticsNodes;
		}

		/// <summary>
		/// Called when the numeric entry receives focus.
		/// </summary>

#if ANDROID
        protected async void OnGotFocus()
#else
        protected void OnGotFocus()
#endif
        {
            #if ANDROID
            // Avoid IndexOutOfBoundException for drag-and-drop text on Android 11 API 30 or below
            if (Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.R)
            {
                await Task.Delay(150);
            }
            #endif

            _textBox?.Focus();
            VisualStateManager.GoToState(this, "Focused");
            
            if (_textBox == null)
            {
                return;
            }

            // Set the text in the text box, ensuring non-null entries
            _textBox.Text = Value != null && !double.IsNaN((double)Value)
                ? RestrictFractionDigit(Value.Value.ToString(GetNumberFormat()))
                : AllowNull ? string.Empty : "0";

            // Set cursor position and selection
            _textBox.CursorPosition = 0;
        #if WINDOWS
            if (!_isFirst)
            {
                _textBox.CursorPosition = _textBox.Text.Length;
                _textBox.SelectionLength = 0;
            }
            _isFirst = false;
        #endif
        #if ANDROID
            await Task.Delay(1);
        #endif
            _textBox.SelectionLength = _textBox.Text.Length;

            // Set accessibility description if not set by the user
            if (_isDescriptionNotSetByUser)
            {
                SemanticProperties.SetDescription(this, $"{Placeholder} NumericEntry");
            }
        }

        /// <summary>
        /// Called when the numeric entry loses focus.
        /// </summary>
        protected void OnLostFocus()
        {
#if ANDROID
            if (_isSamsungWithSamsungKeyboard && _textBox != null)
            {
                // Ensure _textBox.Text isn't just a decimal separator or a minus sign
                if (_textBox.Text == GetNumberDecimalSeparator(GetNumberFormat()) || _textBox.Text == "-")
                {
                    _textBox.Text = AllowNull ? string.Empty : "0";
                }
            }
#endif
            UpdateValue();
#if WINDOWS
            if (_isFocus)
            {
                SetInputViewFocus();
            }
            else
            {
                FormatValue();
                _isFirst = true;
            }
#else
            FormatValue();
#endif
            VisualStateManager.GoToState(this, "Normal");
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the user triggers evaluation of new input by pressing the Enter key, clicking a spin button, or by changing focus.
        /// </summary>
        public event EventHandler<NumericEntryValueChangedEventArgs>? ValueChanged;

		/// <summary>
		/// Occurs when the user finalizes the text in an numeric entry with the return key.
		/// </summary>
		public event EventHandler? Completed;

		/// <summary>
		/// Occurs when the control get focused.
		/// </summary>
		public new event EventHandler<FocusEventArgs>? Focused;

        /// <summary>
        /// Occurs when the control get unfocused.
        /// </summary>
        public new event EventHandler<FocusEventArgs>? Unfocused;

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets focus to the control, allowing user input.
        /// </summary>
        public new void Focus()
        {
            _textBox?.Focus();
        }

        /// <summary>
        /// Removes focus from the control, disabling user input.
        /// </summary>
        public new void Unfocus()
        {
            _textBox?.Unfocus();
        }

        /// <summary>
        /// Raises the <see cref="Focused"/> event.
        /// </summary>
        /// <param name="args">Arguments related to the focus event.</param>
        internal void RaiseFocusedEvent(FocusEventArgs args)
        {
            Focused?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="Unfocused"/> event.
        /// </summary>
        /// <param name="args">Arguments related to the unfocus event.</param>>
        internal void RaiseUnfocusedEvent(FocusEventArgs args)
        {
            Unfocused?.Invoke(this, args);
        }

		#endregion

		#region Keyboard_iOS

#if IOS
        /// <summary>
        /// Adds toolbar items to the numeric entry control on iOS. 
        /// This setup includes the return, minus, and separator buttons, as well as the toolbar view. It also handles the setup of any visual lines or dividers within the toolbar.
        /// </summary>
        void AddToolBarItems()
        {
            SetupReturnButton();
            SetupMinusButton();
            SetupSeparatorButton();
            SetupToolbarView();
            UpdateFrames();
		}

        /// <summary>
        /// To update the return button text
        /// </summary>
		void UpdateReturnButtonText()
		{
			_returnButton?.SetTitle(GetReturnButtonText(), UIControlState.Normal);
		}

        /// <summary>
        /// To get the return button text 
        /// </summary>
		string GetReturnButtonText()
		{
			string returnText = "return";
			if (ReturnType != ReturnType.Default)
			{
				returnText = ReturnType.ToString().ToLowerInvariant();
			}
            
			return SfNumericEntryResources.GetLocalizedString(returnText);
		}

		/// <summary>
		/// Sets up the return button on the toolbar, configuring its appearance and 
		/// behavior to provide feedback or perform actions when the user completes input.
		/// </summary>
		void SetupReturnButton()
        {
            _returnButton = CreateToolbarButton(GetReturnButtonText());
            _returnButton.TouchDown += ReturnButton_TouchDown;
        }

        /// <summary>
        /// Configures the minus button on the toolbar, allowing users to input negative numbers.
        /// It initializes the button and attaches the necessary event handler.
        /// </summary>
        void SetupMinusButton()
        {
            _minusButton = CreateToolbarButton("-");
            _minusButton.TouchDown += MinusButton_TouchDown;
        }

        /// <summary>
        /// Sets up an optional separator button on the toolbar if an additional separator is required.
        /// This helps in providing quick access to additional input characters or functions.
        /// </summary>
        void SetupSeparatorButton()
        {
			_anotherSeparator = SfNumericEntry.GetAnotherSeparatorText();
			_separatorButton = CreateToolbarButton(_anotherSeparator);
			_separatorButton.SetTitle(_anotherSeparator, UIControlState.Normal);
			_separatorButton.TouchDown += SeparatorButton_TouchDown;
		}

		/// <summary>
		/// Retrieves an alternative decimal separator text.
		/// If the current locale's decimal separator is a period ("."), it returns a comma (",").
		/// Otherwise, it returns a period (".").
		/// </summary>
		/// <returns>A string representing the alternative decimal separator.</returns>
		static string GetAnotherSeparatorText()
        {
            string separator = NSLocale.CurrentLocale.DecimalSeparator;
            if (separator == ".")
            {
               return ",";
            }
            else
            {
                return ".";
            }
        }

        /// <summary>
        /// Creates a UIButton for the iOS toolbar with the specified title.
        /// </summary>
        /// <param name="title">The title to set on the button.</param>
        /// <returns>A UIButton configured with the specified title and styling.</returns>
        static UIButton CreateToolbarButton(string? title)
        {
			UIButton button = [];
            button.SetTitle(title, UIControlState.Normal);
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.BackgroundColor = UIColor.FromRGB(210, 213, 218);
            return button;
        }

        /// <summary>
        /// Initializes the toolbar view and adds all configured buttons and lines to it. Sets the background color and ensures the elements are layered correctly for user interaction.
        /// </summary>
        void SetupToolbarView()
        {
            _toolbarView = [];
            _lineView1 = SetupLineView();
            _lineView2 = SetupLineView();
            _lineView3 = SetupLineView();
            _lineView4 = SetupLineView();

            if (_minusButton != null && _separatorButton != null && _returnButton != null)
            {
                _toolbarView.AddSubviews(_minusButton, _separatorButton, _returnButton, _lineView1, _lineView2, _lineView3, _lineView4);
            }
            _toolbarView.BackgroundColor = UIColor.FromRGB(249, 249, 249);

            _toolbarView.BringSubviewToFront(_lineView1);
            _toolbarView.BringSubviewToFront(_lineView2);
            _toolbarView.BringSubviewToFront(_lineView3);
            _toolbarView.BringSubviewToFront(_lineView4);
        }

        /// <summary>
        /// Creates a line view used for visually dividing toolbar items. Each line is styled with a subtle color to match typical UI conventions on iOS.
        /// </summary>
        /// <returns>A configured UIView representing the line.</returns>
        static UIView SetupLineView()
        {
            UIView lineView = new UIView
            {
                BackgroundColor = UIColor.FromRGB(175, 175, 180)
            };
            return lineView;
        }

        /// <summary>
        /// Removes handlers associated with iOS toolbar buttons and sets related fields to null.
        /// </summary>
        void RemoveToolBarItems()
        {
			if (_minusButton != null)
			{
				_minusButton.TouchDown -= MinusButton_TouchDown;
			}
			if (_returnButton != null)
			{
				_returnButton.TouchDown -= ReturnButton_TouchDown;
			}
			if (_separatorButton != null)
			{
				_separatorButton.TouchDown -= SeparatorButton_TouchDown;
			}
            _minusButton = _separatorButton = _returnButton = null ;
            _lineView1 = _lineView2 = _lineView3 = _lineView4 = null;
            _toolbarView = null;
        }

        /// <summary>
        /// Handles the touch down event for the separator button on iOS toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments, usually empty in this context.</param>
        void SeparatorButton_TouchDown(object? sender, EventArgs e)
        {
            if (_textBox != null &&  _textBox.Text != null)
            {
                string decimalSeperator = GetNumberDecimalSeparator(GetNumberFormat());
                _previousText = _textBox.Text;
                if (decimalSeperator == _anotherSeparator) 
				{ 
					HandleDecimalSeparator(decimalSeperator, CursorPosition); 
				}
            }
        }

        /// <summary>
        /// Handles the touch down event for the minus button on iOS toolbar,
        /// toggling the negative sign of the numeric value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments, usually empty in this context.</param>
        void MinusButton_TouchDown(object? sender, EventArgs e)
        {
            if (_textBox != null && _textBox.Text != null)
            {
                bool isNegative = IsNegative(_textBox.Text, GetNumberFormat());
                int caretPosition = CursorPosition;
                _previousText = _textBox.Text;
                HandleNegativeKey(isNegative, caretPosition);
            }
        }

		/// <summary>
		/// Retrieves the next focusable element within the visual tree.
		/// If no next element is found, it searches in the parent hierarchy.
		/// </summary>
		/// <param name="parent">The starting parent element to search from.</param>
		/// <returns>The next focusable VisualElement, or null if none is found.</returns>
		static VisualElement? GetNextFocusableElement(VisualElement parent)
		{
			var elements = parent.GetVisualTreeDescendants()
                     .Where(e => (e is Entry || e is InputView || e is Microsoft.Maui.Controls.Picker || e is DatePicker || e is TimePicker || e is SearchBar))
                     .OfType<VisualElement>()
                     .Where(ve => ve.IsEnabled && ve.IsVisible)
                     .ToList();

			var focusedElement = elements.FirstOrDefault(e => e.IsFocused);

			if (focusedElement != null)
			{
				int currentIndex = elements.IndexOf(focusedElement);

				// Return the next focusable element if available
				if (currentIndex >= 0 && currentIndex < elements.Count - 1)
				{
					return elements[currentIndex + 1];
				}
			}

			// If no next focusable element is found, search in the parent
			return parent.Parent is VisualElement currentParent ? GetNextFocusableElement(currentParent) : null;
		}


		/// <summary>
		/// Searches for the next focusable element in the UI tree starting from the given element.
		/// </summary>
		/// <param name="currentElement">The current visual element from which the search starts.</param>
		/// <returns>The next focusable <see cref="VisualElement"/> if found; otherwise, null.</returns>
		static VisualElement? FindNextFocusableElement(VisualElement currentElement)
        {
			// Start searching from the parent of the current element
			if (currentElement.Parent is not VisualElement rootElement || rootElement == null)
			{
				return null;
			}

			return GetNextFocusableElement(rootElement);
        }

        /// <summary>
        /// Handles the touch down event for the return button on iOS toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments, usually empty in this context.</param>
        void ReturnButton_TouchDown(object? sender, EventArgs e)
        {
            if (_textBox != null)
            {
                if (ReturnType == ReturnType.Next)
                {
                    VisualElement? nextControl = FindNextFocusableElement(this);
					switch (nextControl)
					{
						case SfNumericEntry numeric:
							numeric.Focus();
							break;

						case SfTextInputLayout textInputLayout:
							textInputLayout.Focus();
							break;

						default:
							nextControl?.Focus();
							break;
					}
				}
                else
                {
                    Unfocus();
                }
            }
        }

        /// <summary>
        /// Handler for the device rotation event that updates the toolbar button frames.
        /// </summary>
        /// <param name="notification">The NSNotification triggered by the device rotation.</param>
        void DeviceRotated(NSNotification notification)
        {
            UpdateFrames();
        }

        /// <summary>
        /// Determines whether the current device orientation is portrait or not.
        /// </summary>
        /// <returns>True if the orientation is portrait or face up/down; otherwise, false.</returns>
        static bool IsLandscapeOrientation()
        {
			return UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight;

		}

        /// <summary>
        /// Configures the frames for minus, separator, and return buttons based on the screen width and provided adjustments.
        /// </summary>
        /// <param name="adjustment">The amount to adjust the width of the minus and separator buttons.</param>
        /// <param name="extraAdjustment">Additional adjustment for the separator button width.</param>
        void SetButtonFrames(nfloat adjustment, nfloat extraAdjustment)
		{
			if (_minusButton is UIButton minusButton)
			{
				minusButton.Frame = new CGRect(0, 0, _buttonWidth - adjustment, 50);
			}

			if (_separatorButton is UIButton seperateButton)
			{
				seperateButton.Frame = new CGRect(_buttonWidth - adjustment, 0, _buttonWidth + extraAdjustment, 50);
			}

			if (_returnButton != null && _toolbarView != null)
			{
				_returnButton.Frame = new CGRect(_toolbarView.Frame.Size.Width - _buttonWidth + adjustment, 0, _buttonWidth, 50);
			}
        }

		/// <summary>
		/// Updates the frames and layout of toolbar buttons, adjusting based on screen orientation and size.
		/// Ensures the components maintain appropriate spacing and sizing for touch interactions.
		/// </summary>
		void UpdateFrames()
        {

			if (_toolbarView == null || _minusButton == null || _separatorButton == null || _returnButton == null
					|| _lineView1 == null || _lineView2 == null || _lineView3 == null || _lineView4 == null)
			{
				return;
			}


			if (IsLandscapeOrientation())
			{
				nfloat width = (nfloat)Math.Max((double)UIScreen.MainScreen.Bounds.Height, (double)UIScreen.MainScreen.Bounds.Width);
				_buttonWidth = width / ButtonDivider;
				_toolbarView.Frame = new CGRect(0.0f, 0.0f, (float)width, ToolbarHeight);
				if (width <= LargeScreenWidth)
				{
					SetButtonFrames(SmallLandscapeButtonSpacing, SmallLandscapeSeparatorSpacing);
				}
				else if (width > LargeScreenWidth)
				{
					SetButtonFrames(LargeLandscapeButtonSpacing, LargeLandscapeSeparatorSpacing);
				}
			}
			else
			{
				nfloat width = (nfloat)Math.Min((double)UIScreen.MainScreen.Bounds.Height, (double)UIScreen.MainScreen.Bounds.Width);
				_buttonWidth = width / ButtonDivider;
				_toolbarView.Frame = new CGRect(0.0f, 0.0f, (float)width, ToolbarHeight);
				if (width > SmallScreenWidth)
				{
					SetButtonFrames(MediumPortraitButtonSpacing, MediumPortraitSeparatorSpacing);
				}
				else if (width <= SmallScreenWidth)
				{
					SetButtonFrames(SmallPortraitButtonSpacing, SmallPortraitSeparatorSpacing);

				}
			}
			_lineView1.Frame = new CGRect(0, 0, _toolbarView.Frame.Size.Width, LineThickness);
			_lineView2.Frame = new CGRect(0, _toolbarView.Frame.Size.Height - LineThickness, _toolbarView.Frame.Size.Width, LineThickness);
			_lineView3.Frame = new CGRect(_minusButton.Frame.Right - LineThickness, 0, LineThickness, _minusButton.Frame.Size.Height);
			_lineView4.Frame = new CGRect(_returnButton.Frame.Left, 0, LineThickness, _minusButton.Frame.Size.Height);
		}
#endif
		#endregion
	}

#if ANDROID

	/// <summary>
	/// A utility class for checking the current keyboard in use on an Android device.
	/// </summary>
	internal static class KeyboardChecker
	{

		/// <summary>
		/// A constant representing the package name for the Google Keyboard application.
		/// </summary>
		const string GboardPackage = "com.google.android.inputmethod.latin";


		// Update the known package names for Samsung Keyboard and Gboard
		static readonly string[] SamsungKeyboardPackages = ["com.samsung.android.keyboard", "com.sec.android.inputmethod", "com.samsung.android.honeyboard"];

		/// <summary>
		/// Gets the name of the current keyboard being used on the Android device.
		/// </summary>
		/// <param name="context">The context of the application, used to access system services.</param>
		/// <returns>A string representing the name of the keyboard: "Samsung Keyboard", "Gboard", "Other Keyboard", or "Unknown Keyboard".</returns>
		public static string GetCurrentKeyboard(Context context)
		{
			if (context.GetSystemService(Context.InputMethodService) is InputMethodManager inputMethodManager && inputMethodManager != null)
			{
				var defaultInputMethodId = Settings.Secure.GetString(
					context.ContentResolver,
					Settings.Secure.DefaultInputMethod
				);

				if (string.IsNullOrEmpty(defaultInputMethodId))
				{
					return "Unknown Keyboard";
				}

				var inputMethodList = inputMethodManager.InputMethodList;
				foreach (var inputMethod in inputMethodList)
				{
					if (inputMethod?.Id != null && inputMethod.Id.Equals(defaultInputMethodId, StringComparison.Ordinal))
					{
						var packageName = inputMethod.PackageName;
						if (!string.IsNullOrEmpty(packageName))
						{
							if (SamsungKeyboardPackages.Contains(packageName))
							{
								return "Samsung Keyboard";
							}
							else if (packageName.Equals(GboardPackage, StringComparison.OrdinalIgnoreCase))
							{
								return "Gboard";
							}
							else
							{
								return "Other Keyboard";
							}
						}
					}
				}
			}

			return "Unknown Keyboard";
		}
	}
#endif
}
