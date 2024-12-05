﻿using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.EntryRenderer;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.EntryView;
#if ANDROID
using Android.Text;
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
            if (propertyName == null)
            {
                return;
            }

            switch (propertyName)
            {
                case nameof(IsEnabled):
                    if (_textBox != null)
                    {
                        _textBox.IsEnabled = IsEnabled;
                    }
                    break;

        #if WINDOWS
                case nameof(FlowDirection):
                    SetFlowDirection();
                    break;
        #endif
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
			if (SemanticsDataIsCurrent())
			{
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
            if (IsSamsungDevice() && _textBox != null)
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

            _toolbarView.AddSubviews(_minusButton, _separatorButton, _returnButton, _lineView1, _lineView2, _lineView3, _lineView4);
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

            // Track if we have found the current element in the tree
            bool foundCurrent = false;

            // Recursive method to search for the next focusable element
            VisualElement? SearchNextFocusableElement(VisualElement parent)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                foreach (var child in parent.LogicalChildren)
                {
                    if (foundCurrent && child is VisualElement ve && ve.IsEnabled && ve.IsVisible && ve.Focus())
                    {
                        return ve; // Found the next focusable element
                    }

                    if (child == currentElement)
                    {
                        foundCurrent = true; // Mark that we found the current element
                    }

                    if (child is VisualElement childVisualElement && childVisualElement.LogicalChildren.Count > 0)
                    {
                        var result = SearchNextFocusableElement(childVisualElement);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
#pragma warning restore CS0618 // Type or member is obsolete

                return null;
            }

            return SearchNextFocusableElement(rootElement);
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
					if(nextControl is not null && nextControl is SfNumericEntry numeric && numeric is not null)
					{
						numeric.Focus();
					}
					else
					{
						nextControl?.Focus();
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
        static bool IsPortraitOrientation()
        {
            return 	UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.Portrait || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.PortraitUpsideDown ||
                UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown;
        }

        /// <summary>
        /// Configures the frames for minus, separator, and return buttons based on the screen width and provided adjustments.
        /// </summary>
        /// <param name="adjustment">The amount to adjust the width of the minus and separator buttons.</param>
        /// <param name="extraAdjustment">Additional adjustment for the separator button width.</param>
        void SetButtonFrames(nfloat adjustment, nfloat extraAdjustment)
		{
			if (_minusButton != null)
			{
				_minusButton.Frame = new CGRect(0, 0, _buttonWidth - adjustment, 50);
			}

			if (_separatorButton != null)
			{
				_separatorButton.Frame = new CGRect(_buttonWidth - adjustment, 0, _buttonWidth + extraAdjustment, 50);
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
            if (UIDevice.CurrentDevice.Orientation != UIDeviceOrientation.Unknown)
            {
				if (_toolbarView == null || _minusButton == null || _separatorButton == null || _returnButton == null
					|| _lineView1 == null || _lineView2 == null || _lineView3 == null || _lineView4 == null)
				{
					return;
				}

                nfloat width = UIScreen.MainScreen.Bounds.Width;
                _buttonWidth = width / 3;
                _toolbarView.Frame = new CGRect(0.0f, 0.0f, (float)width, 50.0f);
                if (IsPortraitOrientation())
                {
                    if (width <= 375 && width > 320)
                    {
                        SetButtonFrames(2, 4);
                    }
                    else if (width <= 320)
                    {
                        SetButtonFrames((nfloat)1.5, 3);
                        
                    }
                    else if (width > 375)
                    {
                        SetButtonFrames(2, 4);
                    }
                }
                else if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight)
                {
                    if (width <= 667)
                    {
                        SetButtonFrames((nfloat)2.5, 5);
                    }
                    else if (width > 667)
                    {
                        SetButtonFrames((nfloat)3.1, (nfloat)6.1);
                    }
                }
                _lineView1.Frame = new CGRect(0, 0, _toolbarView.Frame.Size.Width, 0.5f);
                _lineView2.Frame = new CGRect(0, _toolbarView.Frame.Size.Height - 0.5f, _toolbarView.Frame.Size.Width, 0.5f);
                _lineView3.Frame = new CGRect(_minusButton.Frame.Right - 0.5f, 0, 0.5, _minusButton.Frame.Size.Height);
                _lineView4.Frame = new CGRect(_returnButton.Frame.Left, 0, 0.5, _minusButton.Frame.Size.Height);
            }
        }
#endif
		#endregion
	}
}
