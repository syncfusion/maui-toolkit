// <copyright file="InputLayoutBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
﻿using System.Diagnostics.CodeAnalysis;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.EntryRenderer;
using Syncfusion.Maui.Toolkit.EntryView;
using Syncfusion.Maui.Toolkit.NumericUpDown;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
	/// <summary>
	/// Represents the <see cref="SfTextInputLayout"/> control that enhances input views with decorative elements such as floating labels, icons, and assistive labels.
	/// </summary>
	/// <example>
	/// <code>
	/// <![CDATA[
	///<inputLayout:SfTextInputLayout Hint="Hint" HelperText="Helper" ErrorText="Error">
	///     <Entry />
	/// </inputLayout:SfTextInputLayout>
	/// ]]>
	/// </code>
	/// </example>
	[ContentProperty("Content")]
    public partial class SfTextInputLayout : SfContentView, ITouchListener, IParentThemeElement
    {
		#region Fields

		/// <summary>
		/// Timer for handling long press events.
		/// </summary>
		bool _isPressOccurring = false;

		/// <summary>
		/// The initial delay in milliseconds before activating a long press.
		/// </summary>
		const int StartDelay = 500;

		/// <summary>
		/// The interval in milliseconds between repeated actions during a continuous long press.
		/// </summary>
		const int ContinueDelay = 200;

		/// <summary>
		/// Gets or sets a value updown button alignment is vertical or not.
		/// </summary>
		bool _isUpDownVerticalAlignment = false;

        /// <summary>
        /// Gets the padding value of the helper text, error text, counter text.
        /// </summary>
        const double DefaultAssistiveLabelPadding = 4;

        /// <summary>
        /// Gets the height value of the helper text, error text, counter text.
        /// </summary>
        const double DefaultAssistiveTextHeight = 16;

        /// <summary>
        /// Gets the top padding of the input view when container type was filled.
        /// </summary>
        const double FilledTopPadding = 26;

        /// <summary>
        /// Gets the bottom padding of the input view when container type was filled.
        /// </summary>
        const double FilledBottomPadding = 10;

        /// <summary>
        /// Gets the bottom padding of the input view when container type was none.
        /// </summary>
        const double NoneBottomPadding = 12;

        /// <summary>
        /// Gets the top padding of the input view when container type was none.
        /// </summary>
        const double NoneTopPadding = 28;

        /// <summary>
        /// Gets the top and bottom padding of the input view when container type was outlined.
        /// </summary>
        const double OutlinedPadding = 18;

        /// <summary>
        /// Gets the left padding of the input view when container type was outlined and filled.
        /// </summary>
        const double EdgePadding = 16;

        /// <summary>
        /// Gets the right padding of the input view when container type was outlined and filled.
        /// </summary>
        const double RightPadding = 16;

        /// <summary>
        /// Gets the value of floated hint starting x position when container type is outlined.
        /// </summary>
        const float StartX = 12;

        /// <summary>
        /// Gets the font size of the hint text when layout in unfocused state.
        /// </summary>
        const float DefaultHintFontSize = 16;

        /// <summary>
        /// scaling ratio for hint label.
        /// </summary>
        const double HintFontSizeScalingRatio = 0.75;

        /// <summary>
        /// Gets the duration of the animation in milliseconds.
        /// </summary>
        const double DefaultAnimationDuration = 107;

        /// <summary>
        /// Gets the size of the clear button and password toggle visibilityS icon.
        /// </summary>
        const int IconSize = 32;

        /// <summary>
        /// Gets the size of the numeric updown button.
        /// </summary>
        const int UpDownButtonSize = 32;

		/// <summary>
		/// Represents the size of the up/down button when placed vertically.
		/// </summary>
		const int VerticalUpDownButtonSize = 28;

		/// <summary>
		/// Gets or sets a value of right side space for counter text.
		/// </summary>
		const float CounterTextPadding = 12;

        /// <summary>
        /// Gets or sets the string value of counter text.
        /// </summary>
        string _counterText = "0";

        /// <summary>
        /// Gets or sets a value indicating the hint was animating.
        /// </summary>
        bool _isAnimating = false;

        /// <summary>
        /// Gets or sets a value indicating the font size of hint during animation.
        /// </summary>
        double _animatingFontSize;

        /// <summary>
        /// Gets or sets the left padding value of leading view.
        /// </summary>
        readonly double _leadingViewLeftPadding = 8;

        /// <summary>
        /// Gets or sets the right padding value of leading view.
        /// </summary>
        readonly double _leadingViewRightPadding = 12;

        /// <summary>
        /// Gets or sets the left padding value of trailing view.
        /// </summary>
        readonly double _trailingViewLeftPadding = 12;

        /// <summary>
        /// Gets or sets the right padding value of trailing view.
        /// </summary>
        readonly double _trailingViewRightPadding = 8;

        /// <summary>
        /// Gets or sets the leading or trailing view default width.
        /// </summary>
        readonly double _defaultLeadAndTrailViewWidth = 24;

		/// <summary>
		/// The list to add the bounds values of drawing elements as nodes.
		/// </summary>
		readonly List<SemanticsNode> _textInputLayoutSemanticsNodes = new();

		/// <summary>
		/// List of semantic nodes for numeric entry.
		/// </summary>
		readonly List<SemanticsNode> _numericSemanticsNodes = new();

		/// <summary>
		/// Gets or sets a value of leading view width.
		/// </summary>
		double _leadViewWidth = 0;

        /// <summary>
        /// Gets or sets a value of trailing view width.
        /// </summary>
        double _trailViewWidth = 0;

        /// <summary>
        /// Provides a cancellation token source for managing long press operations.
        /// This allows for cancellation of ongoing long press tasks when needed.
        /// </summary>
        CancellationTokenSource? _cancellationTokenSource;

        /// <summary>
        /// Gets or sets the left and right padding value for password visibility toggle icon.
        /// </summary>
        readonly float _passwordToggleIconEdgePadding = 8;

        /// <summary>
        /// Gets or sets the top and bottom padding value for password visibility toggle icon in collapsed state.
        /// </summary>
        readonly float _passwordToggleCollapsedTopPadding = 9;

        /// <summary>
        /// Gets or sets the top and bottom padding value for password visibility toggle icon in visible state.
        /// </summary>
        readonly float _passwordToggleVisibleTopPadding = 10;

        /// <summary>
        /// Gets the path data value for password toggle visible icon.
        /// </summary>
        readonly string _toggleVisibleIconPath = "M8,3.3000005C9.2070007,3.3000005 10.181999,4.2819988 10.181999,5.5000006 10.181999,6.7179991 9.2070007,7.6999975 8,7.6999975 6.7929993,7.6999975 5.8180008,6.7179991 5.8180008,5.5000006 5.8180008,4.2819988 6.7929993,3.3000005 8,3.3000005z M8,1.8329997C5.993,1.8329997 4.3639984,3.475999 4.3639984,5.5000006 4.3639984,7.5249983 5.993,9.1669999 8,9.1669999 10.007,9.1669999 11.636002,7.5249983 11.636002,5.5000006 11.636002,3.475999 10.007,1.8329997 8,1.8329997z M8,0C11.636002,-1.1919138E-07 14.742001,2.2800001 16,5.5000006 14.742001,8.7199975 11.636002,11 8,11 4.3639984,11 1.2579994,8.7199975 0,5.5000006 1.2579994,2.2800001 4.3639984,-1.1919138E-07 8,0z";

        /// <summary>
        /// Gets the path data value for password toggle collapsed icon.
        /// </summary>
        readonly string _toggleCollapsedIconPath = "M4.7510004,4.9479995C4.5109997,5.4359995 4.3660002,5.9739994 4.3660002,6.5489993 4.3660002,8.5569992 5.9949999,10.186999 8.0030003,10.186999 8.5780001,10.186999 9.1160002,10.040999 9.6040001,9.8009992L8.4760003,8.6729991C8.3239999,8.7099991 8.1630001,8.7319992 8.0030003,8.7319992 6.796,8.7319992 5.8210001,7.7569993 5.8210001,6.5489993 5.8210001,6.3889993 5.8429999,6.2289994 5.8790002,6.0759995z M8.0110002,4.3729997C9.2189999,4.3729995,10.194,5.3479995,10.194,6.5559993L10.179,6.6719995 7.8870001,4.3809996z M7.9960003,1.092C11.634,1.092 14.741,3.3549995 16,6.5489993 15.469,7.9019992 14.603,9.0879991 13.504,10.004999L11.38,7.8799992C11.547,7.4659992 11.641,7.0219994 11.641,6.5489993 11.641,4.5399996 10.012,2.9099996 8.0030003,2.9099996 7.5310001,2.9099996 7.0869999,3.0049996 6.6719999,3.1729996L5.1000004,1.6009998C6.0030003,1.2739997,6.9770002,1.092,7.9960003,1.092z M1.6520004,0L14.552,12.900999 13.628,13.823998 11.496,11.699999 11.19,11.394999C10.208,11.786999 9.131,12.005999 8.0030003,12.005999 4.3660002,12.005999 1.2589998,9.7429991 0,6.5489993 0.56700039,5.1089995 1.5130005,3.8569996 2.7209997,2.9179997L2.3870001,2.5829997 0.72700024,0.92399979z";

        /// <summary>
        /// Gets or sets a value indicating the input password text is visible.
        /// </summary>
        bool _isPasswordTextVisible = false;

		Brush _outlinedContainerBackground = Colors.Transparent;

#if !ANDROID
		/// <summary>
		/// Gets or sets a value indicating the clear icon or toggle icon is pressing.
		/// </summary>
		internal bool IsIconPressed { get; private set; } = false;
#endif

		/// <summary>
		/// Gets or sets a value indicating whether the layout has been tapped.
		/// </summary>
		internal bool IsLayoutTapped { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the hint was animating from down to up.
		/// </summary>
		internal bool IsHintDownToUp = true;

        ///<summary>
        /// Gets or sets a value indicating whether the alignment of the up-down button alignment is left.
        /// </summary>
        bool _isUpDownAlignmentLeft = false;

        /// <summary>
        /// Gets or sets a value indicating whether the alignment of the up-down button alignment is Both.
        /// </summary>
        private bool _isUpDownAlignmentBoth = false;

        /// <summary>
        /// Gets or sets the vertical start value for custom animation.
        /// </summary>
        double _translateStart = 0;

        /// <summary>
        /// Gets or sets the vertical end value for custom animation.
        /// </summary>
        double _translateEnd = 1;

		/// <summary>
		/// Gets or sets the start value for scaling animation.
		/// </summary>
		double _fontSizeStart = DefaultHintFontSize;

        /// <summary>
        /// Gets or sets the end value for scaling animation.
        /// </summary>
        double _fontSizeEnd = DefaultHintFontSize * HintFontSizeScalingRatio;

        /// <summary>
        /// Gets or sets the animating font value for scaling custom animation.
        /// </summary>
        float _fontsize = DefaultHintFontSize;

		/// <summary>
		/// The field sets and checks the new size of the drawn elements.
		/// </summary>
		Size _controlSize = Size.Zero;

		/// <summary>
		/// Indicates whether semantics need to be reset.
		/// </summary>
		bool _hasResetSemantics = false;

		/// <summary>
		/// The field sets and checks the text.
		/// </summary>
		string _text = string.Empty;

		/// <summary>
		/// Gets the stroke color of the clear button
		/// </summary>
		static readonly Color ClearIconStrokeColor = Color.FromArgb("#49454F");

		/// <summary>
		/// Label control for displaying helper text
		/// </summary>
		Label? _helperLabel;

		/// <summary>
		/// Label control for displaying error text
		/// </summary>
		Label? _errorLabel;

		readonly EffectsRenderer _effectsRenderer;

        readonly PathBuilder _pathBuilder = new();

        RectF _passwordToggleIconRectF = new();

        RectF _clearIconRectF = new();

        RectF _outlineRectF = new();

        RectF _hintRect = new();

        RectF _backgroundRectF = new();

        RectF _clipRect = new();

        RectF _viewBounds = new();

        PointF _startPoint = new();

        PointF _endPoint = new();

        RectF _helperTextRect = new();

        RectF _errorTextRect = new();

        RectF _counterTextRect = new();

        RectF _downIconRectF = new();

        RectF _upIconRectF = new();

        IUpDownButtonRenderer? _tilRenderer;

        readonly LabelStyle _internalHintLabelStyle = new();

        readonly LabelStyle _internalHelperLabelStyle = new();

        readonly LabelStyle _internalErrorLabelStyle = new();

        readonly LabelStyle _internalCounterLabelStyle = new();

        static Color _defaultStrokeColor = Color.FromRgba("#49454F");

        static Brush _defaultContainerBackground = new SolidColorBrush(Color.FromRgba("#E7E0EC"));

        static Color _defaultFocusStrokeColor = Color.FromRgba("#6750A4");

        static Color _defaultErrorStrokeColor = Color.FromRgba("#B3261E");

        static Color _defaultDisabledStrokeColor = Color.FromRgba("#611c1b1f");

        static Brush _defaultDisabledContainerBackground = new SolidColorBrush(Color.FromRgba("#0a1c1b1f"));

        bool _initialLoaded = false;

#if ANDROID
         Rect? _oldBounds;
#elif IOS || MACCATALYST
         Point _touchDownPoint;

         /// <summary>
         /// To access to the native iOS text control.
         /// </summary>
         UIKit.UITextField? uiEntry;
#endif

		#endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfTextInputLayout"/> class.
        /// </summary>
        public SfTextInputLayout()
        {
            ThemeElement.InitializeThemeResources(this, "SfTextInputLayoutTheme");
            DrawingOrder = DrawingOrder.BelowContent;
			if(HintLabelStyle != null)
			{
				HintFontSize = (float)HintLabelStyle.FontSize;
			}
            this.AddTouchListener(this);
            _effectsRenderer = new EffectsRenderer(this);
            SetRendererBasedOnPlatform();

		}
        #endregion

        #region Private Methods
        Point GetTouchPoint(PointerEventArgs e)
        {
            Point touchPoint;
            if (IsRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                touchPoint = e.TouchPoint;
                touchPoint.X = Width - touchPoint.X;
            }
            else
            {
                touchPoint = e.TouchPoint;
            }

            return touchPoint;
        }

#if IOS || MACCATALYST
        async void UpDownButtonPressed(Point touchpoint)
#else
        void UpDownButtonPressed(Point touchpoint)
#endif
        {
            if (this.Content is ITextInputLayout numericEntry)
            {
                if ((_downIconRectF.Contains(touchpoint) && IsUpDownVerticalAlignment) || (_upIconRectF.Contains(touchpoint) && !IsUpDownVerticalAlignment))
                {
					// Update the semantic description when the button is pressed
					SemanticProperties.SetDescription(this, "Up button pressed");
					numericEntry.UpButtonPressed();
                }
                else if ((_upIconRectF.Contains(touchpoint) && IsUpDownVerticalAlignment) || (_downIconRectF.Contains(touchpoint) && !IsUpDownVerticalAlignment))
                {
					// Update the semantic description when the button is pressed
					SemanticProperties.SetDescription(this, "Down button pressed");
					numericEntry.DownButtonPressed();
                }
				SemanticScreenReader.Announce(SemanticProperties.GetDescription(this));
			}
#if IOS || MACCATALYST
            await Task.Delay(10);
            IsIconPressed = false;
#endif
        }

#if IOS || MACCATALYST
        async void ClearText()
#else
		void ClearText()
#endif
		{
			if (this.Content is ITextInputLayout numericEntry)
            {
				// Update the semantic description when the button is pressed
				SemanticProperties.SetDescription(this, "Clear button pressed");
				SemanticScreenReader.Announce(SemanticProperties.GetDescription(this));
				numericEntry.ClearIconPressed();
                if (!IsClearIconVisible && _effectsRenderer != null && _effectsRenderer.HighlightBounds.Width > 0 && _effectsRenderer.HighlightBounds.Height > 0)
                {
                    _effectsRenderer.RemoveHighlight();
                }
            }
#if IOS || MACCATALYST
            await Task.Delay(10);
            IsIconPressed = false;
#endif
		}
		async Task HandlePointerActions(PointerEventArgs e, Point touchPoint)
        {
            if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
            {
                IsLayoutTapped = false;
				if (ShowUpDownButton)
				{
					StopLongPressTimer();
				}
            }
#if WINDOWS
            IsIconPressed = false;
#endif

			if (e.Action == PointerActions.Pressed)
			{
#if IOS || MACCATALYST
				_touchDownPoint = e.TouchPoint;
#endif
				if (IsUpDownButtonPressed(touchPoint))
				{
					StartPressTimer(touchPoint);
					UpDownButtonPressed(touchPoint);
				}
			}
			if (e.Action == PointerActions.Released)
            {
                _isPressOccurring = false;
#if IOS || MACCATALYST
                double diffX = Math.Abs(_touchDownPoint.X - e.TouchPoint.X);
                double diffY = Math.Abs(_touchDownPoint.Y - e.TouchPoint.Y);

                if (diffX >= 20 || diffY >= 10)
                {
                    return;
                }
#endif
                if (IsClearIconVisible && _clearIconRectF.Contains(touchPoint) && (IsLayoutFocused))
                {
#if !ANDROID
                    IsIconPressed = true;
#endif
                    ClearText();
                    return;
                }
                if (IsUpDownButtonPressed(touchPoint))
                {
#if WINDOWS
                    IsIconPressed = true;
#endif
					StopLongPressTimer();
                    return;
                }

                if ((EnablePasswordVisibilityToggle) && _passwordToggleIconRectF.Contains(touchPoint))
                {
#if !ANDROID
                    IsIconPressed = true;
#endif
                    if (EnablePasswordVisibilityToggle)
                    {
                        ToggleIcon();
                    }
                    return;
                }

                // The control does not focus when touch the outside of the content region
                // so here we call focus manually.
                if (Content != null && !Content.IsFocused)
                {
                    if ((IsOutlined && _outlineRectF.Contains(touchPoint)) || ((IsFilled || IsNone) && _backgroundRectF.Contains(touchPoint)))
                    {

                        if (ShowTrailingView && TrailingViewPosition == ViewPosition.Inside && TrailingView != null && TrailingView.Bounds.Contains(touchPoint))
                        {
                            return;
                        }
                        if (ShowLeadingView && LeadingViewPosition == ViewPosition.Inside && LeadingView != null && LeadingView.Bounds.Contains(touchPoint))
                        {
                            return;
                        }

                        await Task.Delay(1);
                        if (Content is InputView inputView && !inputView.IsReadOnly && inputView.IsEnabled)
                        {
                            inputView.Focus();
                        }
                        else if (Content is SfEntryView entryView && !entryView.IsReadOnly && entryView.IsEnabled)
                        {
                            entryView.Focus();
                        }
                        else if (Content is SfView sfView && sfView.Children.Count > 0 && sfView.Children[0] is Entry entry)
                        {
                            entry.Focus();
                        }
                        else if (Content is View)
                        {
                            Content.Focus();
                        }
                    }
                }
			}
		}


		/// <summary>
		/// Determines whether the specified touch point intersects with the up or down button.
		/// </summary>
		/// <param name="touchPoint">The point of touch input to evaluate.</param>
		/// <returns>
		/// True if the touch point is within the bounds of the up or down button; otherwise, false.
		/// </returns>
		bool IsUpDownButtonPressed(Point touchPoint)
		{
			return ShowUpDownButton && ((_downIconRectF.Contains(touchPoint) || _upIconRectF.Contains(touchPoint)));
		}

		void HandleEntryView(object newValue)
        {
            if (newValue is SfView numericEntry && numericEntry.Children.Count > 0)
            {
                numericEntry.BackgroundColor = Colors.Transparent;
                if (numericEntry.Children[0] is Entry numericInputView)
                {
                    numericInputView.Focused += OnTextInputViewFocused;
                    numericInputView.Unfocused += OnTextInputViewUnfocused;
                    numericInputView.TextChanged += OnTextInputViewTextChanged;
                    numericInputView.HandlerChanged += OnTextInputViewHandlerChanged;
                    if (!string.IsNullOrEmpty(numericInputView.Text))
                    {
                        IsHintFloated = true;
                        IsHintDownToUp = false;
                        Text = numericInputView.Text;
                    }
                }
            }
        }

        void HandleInputView(object newValue)
        {
            if (newValue is InputView inputView)
            {
                inputView.Focused += OnTextInputViewFocused;
                inputView.Unfocused += OnTextInputViewUnfocused;
                inputView.BackgroundColor = Colors.Transparent;
                inputView.TextChanged += OnTextInputViewTextChanged;
                inputView.HandlerChanged += OnTextInputViewHandlerChanged;
                if (!string.IsNullOrEmpty(inputView.Text))
                {
                    IsHintFloated = true;
                    IsHintDownToUp = false;
                    Text = inputView.Text;
                }

                if (newValue is Entry entry && EnablePasswordVisibilityToggle)
                {
                    entry.IsPassword = true;
                    _isPasswordTextVisible = false;
                }
            }
        }

        void HandlePickerView(object newValue)
        {
            if (newValue is Microsoft.Maui.Controls.Picker picker)
            {
                picker.Focused += OnTextInputViewFocused;
                picker.Unfocused += OnTextInputViewUnfocused;

                if (picker.SelectedItem != null)
                {
                    IsHintFloated = true;
                    IsHintDownToUp = false;
                }
                picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
                picker.HandlerChanged += OnTextInputViewHandlerChanged;
            }
        }

        void HandleDatePickerView(object newValue)
        {
            if (newValue is DatePicker datePicker)
            {
                IsHintAlwaysFloated = true;
                datePicker.HandlerChanged += OnTextInputViewHandlerChanged;
                datePicker.Focused += OnTextInputViewFocused;
                datePicker.Unfocused += OnTextInputViewUnfocused;
            }
        }

        void HandleTimePickerView(object newValue)
        {
            if (newValue is TimePicker timePicker)
            {
                IsHintAlwaysFloated = true;
                timePicker.HandlerChanged += OnTextInputViewHandlerChanged;
                timePicker.Focused += OnTextInputViewFocused;
                timePicker.Unfocused += OnTextInputViewUnfocused;
            }
        }

        /// <summary>
        /// This method invokes when the picker selected index changed.
        /// </summary>
        /// <param name="sender">instance of picker</param>
        /// <param name="e">Represents the event data</param>
        void OnPickerSelectedIndexChanged(object? sender, EventArgs e)
        {
            var picker = sender as Microsoft.Maui.Controls.Picker;
            if (picker == null)
            {
                return;
            }

            if (picker.SelectedIndex >= 0)
            {
                IsHintFloated = true;
                IsHintDownToUp = false;
                Text = picker.SelectedItem.ToString() ?? string.Empty;
            }
            else
            {
                IsHintFloated = !IsLayoutFocused ? false : true;
                IsHintDownToUp = !IsLayoutFocused ? true : false;
                Text = string.Empty;
            }
            InvalidateDrawable();
        }

		/// <summary>
		/// Populates the list of numeric semantics nodes.
		/// </summary>
		/// <param name="content">The content object.</param>
		void PopulateNumericSemanticsNodes(object? content)
		{
			_numericSemanticsNodes.Clear();

			switch (content)
			{
				case SfNumericUpDown numericUpDown:
					AddNumericUpDownNodes(numericUpDown);
					break;
				case SfNumericEntry when IsClearIconVisible:
					AddClearButtonNodes(2);
					break;
			}
		}

		/// <summary>
		/// Adds semantic nodes for the numeric up-down control.
		/// </summary>
		/// <param name="numericUpDown">The numeric up/down control.</param>
		void AddNumericUpDownNodes(SfNumericUpDown numericUpDown)
		{
			bool isUpEnabled = numericUpDown.AutoReverse || numericUpDown._valueStates != ValueStates.Maximum;
			bool isDownEnabled = numericUpDown.AutoReverse || numericUpDown._valueStates != ValueStates.Minimum;
			AddUpDownNodes(numericUpDown, isUpEnabled, isDownEnabled);
		}

		/// <summary>
		/// Adds semantic nodes for up-down buttons.
		/// </summary>
		void AddUpDownNodes(SfNumericUpDown numericUpDown, bool isUpEnabled, bool isDownEnabled)
		{
			bool isVerticalInline = numericUpDown.IsInlineVerticalPlacement();
			bool isLeftAlignment = numericUpDown.UpDownButtonAlignment == UpDownButtonAlignment.Left;
			bool addClearIconFirst = isVerticalInline ? !isLeftAlignment : !isVerticalInline && !isLeftAlignment;

			if (addClearIconFirst && IsClearIconVisible)
			{
				AddClearButtonNodes(2);
			}

			AddSemanticsNode(isVerticalInline ? _downIconRectF : _upIconRectF, addClearIconFirst ? 3 : 2, "Up button", isUpEnabled);
			AddSemanticsNode(isVerticalInline ? _upIconRectF : _downIconRectF, addClearIconFirst ? 4 : 3, "Down button", isDownEnabled);

			if (!addClearIconFirst && IsClearIconVisible)
			{
				AddClearButtonNodes(4);
			}
		}

		/// <summary>
		/// Adds a semantic node for clear button.
		/// </summary>
		void AddClearButtonNodes(int index)
		{
			var tempBounds = _clearIconRectF;
			tempBounds.Width = tempBounds.Height = IconSize;
			AddSemanticsNode(tempBounds, index, "Clear button");
		}

		/// <summary>
		/// Adds a semantic node with specified properties.
		/// </summary>
		void AddSemanticsNode(RectF bounds, int id, string description, bool isEnabled = true)
		{
			string stateDescription = isEnabled ? $"{description}, double tap to activate" : $"{description}, disabled";
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				_numericSemanticsNodes.Add(CreateSemanticsNode(id, new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height), stateDescription));
			}
		}

		/// <summary>
		/// Creates a semantic node with specified ID, bounds, and description.
		/// </summary>
		/// <param name="id">The ID of the semantics node.</param>
		/// <param name="rect">The bounds of the node.</param>
		/// <param name="description">The description associated with the node.</param>
		/// <returns>A newly created SemanticsNode object.</returns>
		SemanticsNode CreateSemanticsNode(int id, Rect rect, string description) =>
			new SemanticsNode
			{
				Id = id,
				Bounds = rect,
				Text = description
			};
		#endregion

		#region Override Methods

#if IOS
        /// <summary>
        /// Handles property changes and updates child element flow directions appropriately, especially for right-to-left layouts.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            if (Content == null || string.IsNullOrEmpty(propertyName))
                return;
 
            if (propertyName == nameof(FlowDirection))
            {
                var flowDirection = IsRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                Content.FlowDirection = flowDirection;
                if (_helperLabel != null)
                {
                    _helperLabel.FlowDirection = flowDirection;;
                }
                if (_errorLabel != null)
                {
                    _errorLabel.FlowDirection = flowDirection;
                }
            }
        }
 
#endif

		/// <summary>
		/// Returns the semantics node list.
		/// </summary>
		/// <param name="width">The width of the element.</param>
		/// <param name="height">The height of the element.</param>
		/// <returns>A list of semantics nodes.</returns>
		protected override List<SemanticsNode> GetSemanticsNodesCore(double width, double height)
		{
			Size newControlSize = new(Width, Height);
			if (_controlSize == newControlSize && _textInputLayoutSemanticsNodes.Count != 0)
			{
				return _textInputLayoutSemanticsNodes;
			}
			_controlSize = newControlSize;
			PopulateNumericSemanticsNodes(Content);
			_textInputLayoutSemanticsNodes.AddRange(_numericSemanticsNodes);
			return _textInputLayoutSemanticsNodes;
		}

		/// <summary>
		/// Invoked when the size of the element is allocated.
		/// </summary>
		/// <param name="width">The width allocated to the element.</param>
		/// <param name="height">The height allocated to the element.</param>
		/// <exclude/>
		protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (!VisualStateManager.HasVisualStateGroups(this) && HasStrokeValue() && HasContainerBackgroundValue() && !Application.Current!.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme))
            {
                AddDefaultVSM();
            }
        }

		/// <summary>
		/// Invoked when the content of the element changes.
		/// </summary>
		/// <param name="oldValue">The previous content.</param>
		/// <param name="newValue">The new content.</param>
		/// <exclude/>
		protected override void OnContentChanged(object oldValue, object newValue)
        {
            ResetNumericUpDown();
            HandleInputView(newValue);
            HandlePickerView(newValue);
            HandleDatePickerView(newValue);
            HandleTimePickerView(newValue);
            HandleEntryView(newValue);

            if (newValue is View view)
            {
                UpdateContentMargin(view);
            }
			
			double minOpacity = 0;
#if ANDROID || IOS
			minOpacity  = 0.00001;
#elif MACCATALYST
			minOpacity  = 0.011;
#endif

			//For placeholder overlap issue here handled the opacity value for controls.
			if (newValue is InputView entryEditorContent)
            {
				entryEditorContent.Opacity = IsHintFloated ? 1 : minOpacity;
			}
            else if (newValue is SfView numericEntryContent && numericEntryContent.Children.Count > 0)
            {
                if (numericEntryContent.Children[0] is Entry numericInputView)
                {
					numericInputView.Opacity = IsHintFloated ? 1 : minOpacity;
				}
            }
            else if (newValue is Microsoft.Maui.Controls.Picker picker)
            {
                if (DeviceInfo.Platform != DevicePlatform.WinUI)
                {
					picker.Opacity = IsHintFloated ? 1 : minOpacity;
                }
            }

            base.OnContentChanged(oldValue, newValue);

            if (!IsEnabled)
            {
                OnEnabledPropertyChanged(IsEnabled);
            }
			InitializeAssistiveLabels();
			SetCustomDescription(newValue);
			ResetSemantics();
        }

		/// <summary>
		/// Sets a custom semantic description for the content.
		/// </summary>
		private void SetCustomDescription(object content)
		{

			if (this.Content == null || content == null)
				return;

			var customDescription = string.Empty;
#if ANDROID || MACCATALYST || IOS
			
			if (content is InputView entryEditorContent)
			{
				customDescription = (string.IsNullOrEmpty(entryEditorContent.Text) && !string.IsNullOrEmpty(entryEditorContent.Placeholder) && DeviceInfo.Platform ==  DevicePlatform.Android) ? entryEditorContent.Placeholder : string.Empty;
			}

			var layoutDescription = GetLayoutDescription();
			
			var contentDescription = layoutDescription + (IsHintFloated ? customDescription : string.Empty);

			SemanticProperties.SetDescription(this.Content, contentDescription);
#elif WINDOWS

			customDescription = SemanticProperties.GetDescription(this);

			if (string.IsNullOrEmpty(customDescription))
			{
				customDescription = ShowHint && !string.IsNullOrEmpty(Hint) ? Hint : string.Empty;
				SemanticProperties.SetDescription(this, customDescription);
			}
#endif
		}

#if ANDROID || MACCATALYST || IOS
		/// <summary>
		/// Retrieves the layout semantic description.
		/// </summary>
		private string GetLayoutDescription()
		{
			var description = SemanticProperties.GetDescription(this);

			if (string.IsNullOrEmpty(description))
			{
				description = ShowHint && !string.IsNullOrEmpty(Hint) ? Hint : string.Empty;
			}
			if(!string.IsNullOrEmpty(description))
			{
				description = description.EndsWith(".") ? description : description + ". ";
			}
			return description ?? string.Empty;
		}
#endif
		/// <summary>
		/// Measures the size requirements for the content of the element.
		/// </summary>
		/// <param name="widthConstraint">The available width for the content.</param>
		/// <param name="heightConstraint">The available height for the content.</param>
		/// <exclude/>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            base.MeasureContent(widthConstraint, heightConstraint);
            var availableWidth = widthConstraint;
            var availableHeight = heightConstraint;

            var measuredWidth = 0d;
            var measuredHeight = 0d;

            var measure = new Size(0, 0);

            if (Content != null)
            {
#if NET8_0
				measure = Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
#else
                measure = Content.Measure(widthConstraint, heightConstraint);
#endif
            }

            if (availableWidth == -1 || availableWidth == double.PositiveInfinity)
            {
                measuredWidth = measure.Width;
            }
            else
            {
                measuredWidth = availableWidth;
            }
            if (heightConstraint == -1 || heightConstraint == double.PositiveInfinity)
            {
                measuredHeight = measure.Height;
                if (IsUpDownVerticalAlignment && measuredHeight < 90 && ShowUpDownButton)
                {
                    measuredHeight = 90;
                }
            }
            else
            {
                measuredHeight = availableHeight;
            }

            UpdateViewBounds();

            _initialLoaded = true;

            return new Size(measuredWidth, measuredHeight);
        }

		/// <summary>
		/// Positions and sizes the content of the element.
		/// </summary>
		/// <param name="bounds">The bounds in which the content should be arranged.</param>
		/// <returns>The actual size used by the content.</returns>
		/// <exclude/>
		protected override Size ArrangeContent(Rect bounds)
        {
#if WINDOWS
            if (LeadingView != null && LeadingView.WidthRequest == -1)
            {
                LeadingView.WidthRequest = _defaultLeadAndTrailViewWidth;
            }

            if (TrailingView != null && TrailingView.WidthRequest == -1)
            {
                TrailingView.WidthRequest = _defaultLeadAndTrailViewWidth;
            }
#endif

#if ANDROID
            if (_oldBounds == null)
            {
                _oldBounds = bounds;
                UpdateViewBounds();
            }
            else if (_oldBounds?.Width != bounds.Width ||
                  _oldBounds?.Height != bounds.Height ||
                  _oldBounds?.X != bounds.X ||
                  _oldBounds?.Y != bounds.Y)
            {
                UpdateViewBounds();
            }
#endif
            return base.ArrangeContent(bounds);
        }

		/// <summary>
		/// Resets the numeric up-down control settings
		/// </summary>
		void ResetNumericUpDown()
		{
			ShowUpDownButton = false;
			ShowClearButton = false;
			IsUpDownVerticalAlignment = false;
			IsUpDownAlignmentLeft = false;
			IsUpDownAlignmentBoth = false;
			RemovedExistingView(UpIconTemplate);
			RemovedExistingView(DownIconTemplate);
			UpIconTemplate = null;
			DownIconTemplate = null;
		}

		/// <summary>
		/// This method calculate the up icon rectF position.
		/// </summary>
		void UpdateUpIconRectF()
        {
            if (IsUpDownVerticalAlignment)
            {
                _upIconRectF.X = _downIconRectF.X;
				_upIconRectF.Y = (float)(_downIconRectF.Y + IconSize / 2 + DefaultAssistiveLabelPadding);
				if (UpIconTemplate != null && DownIconTemplate != null)
				{
					_upIconRectF.Y = (float)(_downIconRectF.Bottom);
				}
				
            }
            else
            {
				if (!(IsUpDownAlignmentBoth || IsUpDownAlignmentLeft))
				{
					_upIconRectF.X = (_downIconRectF.Width != 0 && ShowUpDownButton) ? this.IsRTL ? (_downIconRectF.X + IconSize) : _downIconRectF.X - IconSize : _downIconRectF.X;
				}               
				_upIconRectF.Y = _downIconRectF.Y;
				HandleUpIconInlinePosition();
            }
            _upIconRectF.Width = (ShowUpDownButton) ? GetButtonSize() : 0;
            _upIconRectF.Height = (ShowUpDownButton) ? GetButtonSize() : 0;
        }

		/// <summary>
		/// Handles the positioning of the up icon inline with other elements.
		/// Adjusts the X-coordinate of the up icon based on alignment settings and RTL layout.
		/// </summary>
		void HandleUpIconInlinePosition()
        {
			if (IsUpDownAlignmentLeft)
			{
				_downIconRectF.X = (_upIconRectF.Width != 0 && ShowUpDownButton) ? !IsRTL ?
(_upIconRectF.X + IconSize) : _upIconRectF.X - IconSize : _upIconRectF.X;
			}
			else if (IsUpDownAlignmentBoth)
			{
				_downIconRectF.X = !IsRTL ? (float)(Width - _trailViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding)
: (float)(_trailViewWidth + (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
			}
		}

		/// <summary>
		/// This method calculate the clear icon rectF position.
		/// </summary>
		bool IsClearIconVisible
        {
            get { return (ShowClearButton && IsLayoutFocused && !string.IsNullOrEmpty(Text)); }
        }

        /// <summary>
        /// This method calculate the clear icon rectF position.
        /// </summary>
        void UpdateClearIconRectF()
        {
            _clearIconRectF.X = (_downIconRectF.Width != 0) ? GetClearIconX() : IsRTL ? _upIconRectF.X - IconSize : _upIconRectF.X;
            _clearIconRectF.Y = IsUpDownVerticalAlignment ? (float)(_downIconRectF.Y + (DefaultAssistiveLabelPadding / 2) + (IconSize / 2) - (IconSize / 4)) : _downIconRectF.Y;
            _clearIconRectF.Width = IsClearIconVisible ? IconSize : 0;
            _clearIconRectF.Height = IsClearIconVisible ? IconSize : 0;
            if (_downIconRectF.Width != 0)
            {
                _clearIconRectF.X -= (float)(DefaultAssistiveLabelPadding);
            }
        }

        /// <summary>
        /// Updates clear icon position based on up down alignment.
        /// </summary>
        private float GetClearIconX()
        {
			if (IsUpDownVerticalAlignment && !IsRTL && IsUpDownAlignmentLeft && ShowUpDownButton)
			{
				return (float)(Width - _trailViewWidth  - IconSize - DefaultAssistiveLabelPadding) ;
			}
			if (IsUpDownAlignmentLeft && ShowUpDownButton)
            {
                return !IsRTL ? (float)(Width - _trailViewWidth - (IsOutlined ? BaseLineMaxHeight  : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding) : (float)(_leadViewWidth + (IsOutlined ? BaseLineMaxHeight : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
            }
			
			if (IsUpDownVerticalAlignment && IsUpDownAlignmentLeft && IsRTL)
			{
				return (float)(_trailViewWidth + (IsOutlined ? BaseLineMaxHeight : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
			}
			else if ((IsUpDownAlignmentBoth || IsUpDownVerticalAlignment))
			{
				return IsRTL ? _downIconRectF.X + _downIconRectF.Width : _downIconRectF.X - _downIconRectF.Width;
			}
			return !IsRTL ? _upIconRectF.X - IconSize : _upIconRectF.X + IconSize;
        }

		/// <summary>
		/// Handles the positioning of the down icon.
		/// Adjusts the X-coordinate of the down icon based on down button alignment settings.
		/// </summary>
		void HandleDownIconPositionRTL()
        {
            if (IsUpDownVerticalAlignment)
            {
                if (IsUpDownAlignmentLeft)
                {
                    _downIconRectF.X = (float)(Width - _leadViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding);
                }
                else
                {
                    _downIconRectF.X = (float)(_trailViewWidth + (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
                }
            }
            else
            {
                if (IsUpDownAlignmentLeft || IsUpDownAlignmentBoth)
                {
                    _upIconRectF.X = (float)(Width - _leadViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding);
                }
				else
				{
					_downIconRectF.X = (float)(Width - _downIconRectF.X - _downIconRectF.Width);
				}
            }
        }

		/// <summary>
		/// Handles the positioning of the down icon for Right-to-Left (RTL) layouts.
		/// Adjusts the X-coordinate of the down icon based on down button alignment settings.
		/// </summary>
		void HandleDownIconPosition()
        {
            if (IsUpDownVerticalAlignment)
            {
                if (IsUpDownAlignmentLeft)
                {
                    _downIconRectF.X = (float)(_leadViewWidth + (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
                }
                else
                {
                    _downIconRectF.X = (float)(Width - _trailViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding);
                }
            }
            else
            {
                if (IsUpDownAlignmentLeft || IsUpDownAlignmentBoth)
                {
                    _upIconRectF.X = (float)(_leadViewWidth + (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) + DefaultAssistiveLabelPadding);
                }
            }
        }

		/// <summary>
		/// This method calculate the down rectF position.
		/// </summary>
		void UpdateDownIconRectF()
        {
            _downIconRectF.X = (float)(Width - _trailViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding);
			HandleDownIconPosition();
            if (IsNone)
            {
                _downIconRectF.Y = (float)((Content.Y + (Content.Height / 2)) - (UpDownButtonSize / 2));
                if (IsUpDownVerticalAlignment)
                {
					_downIconRectF.Y = (float)(Content.Y + (Content.Height / 2) - GetButtonSize() - (DefaultAssistiveLabelPadding * 1.5));
				}
            }
            else if (IsOutlined)
            {
                _downIconRectF.Y = (float)((_outlineRectF.Center.Y) - (UpDownButtonSize / 2));
                if (IsUpDownVerticalAlignment)
                {
                    _downIconRectF.Y = (float)((_outlineRectF.Center.Y) - ((IconSize * 1.5) / 2) - (DefaultAssistiveLabelPadding / 2));
                }
            }
            else
            {
                _downIconRectF.Y = (float)(((Height - AssistiveLabelPadding - TotalAssistiveTextHeight()) / 2) - (UpDownButtonSize / 2));
                if (IsUpDownVerticalAlignment)
                {
                    _downIconRectF.Y = (float)(((Height - AssistiveLabelPadding - TotalAssistiveTextHeight()) / 2) - ((IconSize * 1.5) / 2) - (DefaultAssistiveLabelPadding / 2));
                }
            }

            _downIconRectF.Width = (IsPassowordToggleIconVisible || ShowUpDownButton) ? GetButtonSize() : 0;
            _downIconRectF.Height = (IsPassowordToggleIconVisible || ShowUpDownButton) ? GetButtonSize() : 0;

            if (IsRTL)
            {
				HandleDownIconPositionRTL();
            }

        }

		/// <summary>
		/// Draws the content of the element.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		/// <exclude/>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            canvas.CanvasSaveState();
            canvas.Antialias = true;
            UpdateIconRectF();
            DrawBorder(canvas, dirtyRect);
            DrawHintText(canvas, dirtyRect);
            DrawClearIcon(canvas, _clearIconRectF);
            DrawUpDownIcon(canvas, dirtyRect);
            DrawAssistiveText(canvas, dirtyRect);
			UpdateAssistiveLabels();
			DrawPasswordToggleIcon(canvas, dirtyRect);
            if (_effectsRenderer != null)
            {
                _effectsRenderer.ControlWidth = Width;
                _effectsRenderer.DrawEffects(canvas);
            }

            if (Content != null)
            {
                UpdateContentMargin(Content);
            }

            UpdateContentPosition();
            canvas.ResetState();
        }

		/// <summary>
		/// Draws a button on the canvas with the specified color and icon.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="color">The color of the button.</param>
		/// <param name="iconRectF">The rectangle defining the button's boundaries.</param>
		/// <param name="drawAction">The action to draw the button's icon.</param>
		void DrawButton(ICanvas canvas, Color color, RectF iconRectF, Action<ICanvas, RectF> drawAction)
        {
            canvas.SaveState();
            canvas.FillColor = color;
            canvas.StrokeColor = color;
            canvas.StrokeSize = 1.5f;
            drawAction.Invoke(canvas, iconRectF);
            canvas.RestoreState();
        }

		/// <summary>
		/// Draws the up and down icons for the numeric input control.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="rectF">The rectangle defining the control's boundaries.</param>
		void DrawUpDownIcon(ICanvas canvas, RectF rectF)
        {
            if (!this.ShowUpDownButton || _tilRenderer is null)
            {
				return;
            }
			
			if (DownIconTemplate != null)
			{
				RectF tempRect = IsUpDownVerticalAlignment ? _upIconRectF : _downIconRectF;
				if (IsRTL)
				{
					tempRect.X = (float)Width - tempRect.X - IconSize;
				}
				UpdateButtonView(DownIconTemplate, tempRect);

			}
			else
			{
				DrawButton(canvas, DownButtonColor, IsUpDownVerticalAlignment ? _upIconRectF : _downIconRectF, _tilRenderer.DrawDownButton);
			}
			if (UpIconTemplate != null)
			{
				RectF tempRect = IsUpDownVerticalAlignment ? _downIconRectF : _upIconRectF;
				if (IsRTL)
				{
					tempRect.X = (float)Width - tempRect.X - IconSize;
				}
				UpdateButtonView(UpIconTemplate, tempRect);
			}
			else
			{
				DrawButton(canvas, UpButtonColor, IsUpDownVerticalAlignment ? _downIconRectF : _upIconRectF, _tilRenderer.DrawUpButton);
			}
		}

		/// <summary>
		/// Changes the visual state of the control.
		/// </summary>
		/// <exclude/>
		protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            if (Application.Current != null && Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme))
            {
                string stateName = !IsEnabled ? "LineDisabled" : (HasError ? "Error" : (IsLayoutFocused ? "LineFocused" : "LineNormal"));

                if (ContainerType == ContainerType.Outlined)
                {
                    stateName = !IsEnabled ? "OutlinedDisabled" : (HasError ? "OutlinedError" : (IsLayoutFocused ? "OutlinedFocused" : "OutlinedNormal"));
                }
                else if (ContainerType == ContainerType.Filled)
                {
                    stateName = !IsEnabled ? "Disabled" : (HasError ? "FilledError" : IsLayoutFocused ? "Focused" : "Normal");
                }

                VisualStateManager.GoToState(this, stateName);

                if (!HasError)
                {
                    CurrentActiveColor = Stroke;
                }
            }
            else
            {
                string stateName = !IsEnabled ? "Disabled" : (HasError ? "Error" : IsLayoutFocused ? "Focused" : "Normal");
                VisualStateManager.GoToState(this, stateName);

                if (!HasError)
                {
                    CurrentActiveColor = IsEnabled ? Stroke : DisabledColor;
                }
            }
        }

		/// <summary>
		/// Invoked when the handler for the element changes.
		/// </summary>
		/// <exclude/>
		protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            UnwireEvents();

            if (Handler != null)
            {
                WireEvents();
                OnTextInputViewHandlerChanged(this.Content, new EventArgs());
#if ANDROID
				ConfigureAccessibilityForAssistiveLabels();
#endif
			}
			else
			{
				if (HintLabelStyle != null && HelperLabelStyle != null && ErrorLabelStyle != null)
				{
					HintLabelStyle = null;
					ErrorLabelStyle = null;
					HelperLabelStyle = null;
				}
			}
		}

        #endregion

        #region Interface Implementation        

        /// <summary>
        /// Handles touch events for the control.
        /// </summary>
        /// <param name="e">Contains information about the touch event.</param>
        async void ITouchListener.OnTouch(PointerEventArgs e)
        {
            Point touchPoint = GetTouchPoint(e);
            await HandlePointerActions(e, touchPoint);

            if (ContainerType == ContainerType.Filled)
            {
                if (HasError == true)
                {
                    VisualStateManager.GoToState(this, "FilledError");
                    return;
                }
                switch (e.Action)
                {
                    case PointerActions.Entered:
                        {
                            if (!IsFocused)
                            {
                                VisualStateManager.GoToState(this, "Hover");
                            }
                            break;
                        }
                    case PointerActions.Exited:
                        {
                            if (IsFocused)
                            {
                                VisualStateManager.GoToState(this, "Focused");
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "Normal");
                            }
                            break;
                        }
                }
            }
            else if (ContainerType == ContainerType.Outlined)
            {
                if (HasError == true)
                {
                    VisualStateManager.GoToState(this, "OutlinedError");
                    return;
                }
                switch (e.Action)
                {
                    case PointerActions.Entered:
                        {
                            if (!IsFocused)
                            {
                                VisualStateManager.GoToState(this, "OutlinedHover");
                            }
                            break;
                        }
                    case PointerActions.Exited:
                        {
                            if (IsFocused)
                            {
                                VisualStateManager.GoToState(this, "OutlinedFocused");
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "OutlinedNormal");
                            }
                            break;
                        }
                }
            }
            else if (ContainerType == ContainerType.None)
            {
                if (HasError == true)
                {
                    VisualStateManager.GoToState(this, "Error");
                    return;
                }
                switch (e.Action)
                {
                    case PointerActions.Entered:
                        {
                            if (!IsFocused)
                            {
                                VisualStateManager.GoToState(this, "LineHover");
                            }
                            break;
                        }

                    case PointerActions.Exited:
                        {
                            if (IsFocused)
                            {
                                VisualStateManager.GoToState(this, "LineFocused");
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "LineNormal");
                            }
                            break;
                        }
                }
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Occurs when the visibility of the password input is toggled.
        /// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="PasswordVisibilityToggled"/> event.
		///  # [C#](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.EnablePasswordVisibilityToggle = true;
		/// inputLayout.PasswordVisibilityToggled += OnPasswordVisibilityToggled;
		/// 
		/// private void OnPasswordVisibilityToggled(object sender, PasswordVisibilityToggledEventArgs e)
		/// {
		///    var passwordVisibility = e.IsPasswordVisible;
		/// }
		/// ]]></code>
		/// </example>
        public event EventHandler<PasswordVisibilityToggledEventArgs>? PasswordVisibilityToggled;

        #endregion
    }
}
