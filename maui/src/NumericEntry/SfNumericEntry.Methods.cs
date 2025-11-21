using System.Globalization;
using System.Text.RegularExpressions;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.EntryRenderer;
using Syncfusion.Maui.Toolkit.EntryView;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Path = Microsoft.Maui.Controls.Shapes.Path;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Windows.Globalization.NumberFormatting;
using Windows.System;
using System.Collections.Generic;
#elif ANDROID
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
	public partial class SfNumericEntry
	{
		#region Private Methods

		/// <summary>
		/// Validate the parent is a TextInputLayout or not.
		/// </summary>
		void ValidateParentIsTextInputLayout()
		{
			for (Element? parentElement = Parent; parentElement != null; parentElement = parentElement.Parent)
			{
				if (parentElement is SfTextInputLayout inputLayout && inputLayout.Content == this)
				{
					IsTextInputLayout = true;
					_textInputLayout = inputLayout;
					UpdateTextInputLayoutUI();
					MinimumHeightRequest = 0;
					BackgroundColor = Colors.Transparent;
					break;
				}
			}
		}

		/// <summary>
		/// Updates the current touch point's position based on the provided touch event data.
		/// Adjusts the touch point for RTL (right-to-left) layouts if necessary.
		/// </summary>
		/// <param name="touch">The touch point on the screen.</param>
		void UpdateTouchPoint(Point touch)
		{
#if WINDOWS
			_touchPoint = IsRTL() ? new Point(Width - touch.X, touch.Y) : touch;
#else
			_touchPoint = touch;
#endif
		}

		/// <summary>
		/// Handles the logic when a touch press is detected. This includes handling
		/// button presses and starting a long-press timer if applicable.
		/// </summary>
		/// <param name="e">The event arguments containing details about the touch press interaction.</param>
		void HandlePressed(PointerEventArgs e)
		{
#if WINDOWS
			// Check if the text box is focused on Windows before doing anything.
			if (_textBox != null && _textBox.IsFocused)
			{
				_isFocus = true;
			}
#else
 			// Set focus for other platforms when a touch press occurs.
			Focus();
#endif
		}

		/// <summary>
		/// Handles the logic when a touch release or cancellation is detected, stopping
		/// the long-press timer and triggering release-related actions.
		/// </summary>
		/// <param name="e">The event arguments detailing the touch release or cancel interaction.</param>
		void HandleReleasedOrCancelled(PointerEventArgs e)
		{
			if (e.Action == PointerActions.Released)
			{
				if (ClearButtonClicked(_touchPoint))
				{
					OnClearButtonTouchUp(e);
				}
				ClearHighlightIfNecessary();
			}
		}

		static Brush GetDefaultStroke()
		{
#if ANDROID
            return new SolidColorBrush(Color.FromRgba(100, 100, 100, 255));
#elif WINDOWS
			return new SolidColorBrush(Color.FromRgba(141, 141, 141, 255));
#else
			return new SolidColorBrush(Colors.LightGray);
#endif
		}

		/// <summary>
		/// Initializes platform-specific settings and resources.
		/// </summary>
		void SetRendererBasedOnPlatform()
		{
#if ANDROID
            _entryRenderer = new MaterialSfEntryRenderer() { _clearButtonPadding = 12 };
#elif WINDOWS
			_entryRenderer = new FluentSfEntryRenderer();
#else
			_entryRenderer = new CupertinoSfEntryRenderer() { _borderStrokeSize = 6 };
#endif
		}

		/// <summary>
		/// Configures the positioning and size of the clear button within the given bounding rectangle.
		/// </summary>
		/// <param name="bounds">The bounding rectangle that defines the available space for positioning the clear button.</param>
		void ConfigureClearButton(RectF bounds)
		{
			// Exit method early if the clear button is not visible to avoid unnecessary calculations.
			if (!_isClearButtonVisible)
			{
				return;
			}

			// Determine if the layout direction is RTL (right-to-left).
			bool isRTL = IsRTL();

			// Set the X position based on RTL status; place on left for RTL, right otherwise.
			_clearButtonRectF.X = isRTL ? 0 : (bounds.Width - ButtonSize);

			// Set other dimensions and properties of the clear button.
			_clearButtonRectF.Y = bounds.Center.Y - (ButtonSize / 2);
			_clearButtonRectF.Width = ButtonSize;
			_clearButtonRectF.Height = ButtonSize;

			// Adjusts button space to the size of the button.
			_buttonSpace = ButtonSize;

			// Clear existing semantics nodes to prepare for the new configuration.
			_numericEntrySemanticsNodes.Clear();

			// Invalidate semantics to ensure the changes are reflected.
			InvalidateSemantics();
		}

		/// <summary>
		/// Sets the margin for the text box within the numeric entry control based on its layout requirements.
		/// </summary>
		void SetTextBoxMargin()
		{
			if (_textBox != null)
			{
				_textBox.ButtonSize = ButtonSize + 4;
				double rightMargin = _buttonSpace;
				_textBox.Margin = GetMarginBasedOnTextAlignment(0, 0, rightMargin, 0);
			}
		}


		/// <summary>
		/// Clears the collection of effect bounds, typically used to reset or remove visual effects on the control.
		/// </summary>
		void ClearEffectsBoundsCollection()
		{
			if (_effectsRenderer is null)
			{
				return;
			}
			_effectsRenderer.RippleBoundsCollection.Clear();
			_effectsRenderer.HighlightBoundsCollection.Clear();
		}

		/// <summary>
		/// Draws the path for the clear button using the specified canvas and path.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw the clear button path.</param>
		/// <param name="clearButtonPath">The path that defines the shape and outline of the clear button.</param>
		void DrawClearButtonPath(ICanvas canvas, Path clearButtonPath)
		{
			PathF path = clearButtonPath.GetPath();
			canvas.SaveState();
			canvas.FillColor = clearButtonPath.Fill is SolidColorBrush solidColorBrushFill ? solidColorBrushFill.Color : Colors.Transparent;
			canvas.StrokeColor = clearButtonPath.Stroke is SolidColorBrush solidColorBrushStroke ? solidColorBrushStroke.Color : ClearIconStrokeColor;
			canvas.ClipRectangle(UpdatePathRect());
			canvas.Translate(_clearButtonRectF.Center.X - path.Bounds.Center.X, _clearButtonRectF.Center.Y - path.Bounds.Center.Y);
			canvas.FillPath(path, WindingMode.EvenOdd);
			canvas.DrawPath(path);
			canvas.RestoreState();
		}

		/// <summary>
		/// Updates and returns the bounding rectangle used for positioning drawable elements.
		/// </summary>
		/// <returns>A rectangle that defines the area for drawing paths or other elements within the control.</returns>
		RectF UpdatePathRect()
		{
			RectF clipSize = new()
			{
				Width = _clearButtonRectF.Width / 2,
				Height = _clearButtonRectF.Height / 2
			};
			clipSize.X = _clearButtonRectF.Center.X - clipSize.Width / 2;
			clipSize.Y = _clearButtonRectF.Center.Y - clipSize.Height / 2;

			return clipSize;
		}

		/// <summary>
		/// Initializes the settings of the given canvas, preparing it for drawing operations.
		/// </summary>
		/// <param name="canvas">The canvas to be initialized for rendering content.</param>
		void InitializeCanvas(ICanvas canvas)
		{
			canvas.Alpha = 1f;
			canvas.FillColor = Colors.Black;
			canvas.StrokeColor = Colors.Black;
		}

		/// <summary>
		/// Draws the clear button on the specified canvas.
		/// </summary>
		/// <param name="canvas">The canvas on which the clear button is drawn.</param>
		void DrawClearButton(ICanvas canvas)
		{
			if (!_isClearButtonVisible)
			{
				return;
			}

			if (ClearButtonPath is not null)
			{
				DrawClearButtonPath(canvas, ClearButtonPath);
			}
			else
			{
				canvas.StrokeColor = ClearButtonColor;
				_entryRenderer?.DrawClearButton(canvas, _clearButtonRectF);
			}
		}

		/// <summary>
		/// Draws a border around the control using the specified canvas and rectangle.
		/// </summary>
		/// <param name="canvas">The canvas on which the border is drawn.</param>
		/// <param name="dirtyRect">The rectangle defining the boundary area for the border.</param>
		void DrawBorder(ICanvas canvas, Rect dirtyRect)
		{
			if (_textBox is null || _textBox.FocusedStroke is null || (Stroke is not SolidColorBrush stroke) || !ShowBorder)
			{
				return;
			}

			Color strokeColor = Colors.Equals(stroke.Color, (GetDefaultStroke() as SolidColorBrush)?.Color)
								? _textBox.FocusedStroke
								: stroke.Color;

			_entryRenderer?.DrawBorder(canvas, dirtyRect, _textBox.IsFocused, stroke.Color, strokeColor);
		}

		/// <summary>
		/// Draws a effects.
		/// </summary>
		/// <param name="canvas">The canvas on which the effects is drawn.</param>
		void DrawEffects(ICanvas canvas)
		{
			if (_effectsRenderer is null)
			{
				return;
			}

			_effectsRenderer.ControlWidth = Width;
			_effectsRenderer.IsRTL = IsRTL();
			_effectsRenderer.DrawEffects(canvas);
		}

		/// <summary>
		/// Updates the semantic sizes for the clear, down, and up buttons
		/// based on their current rectangle dimensions.
		/// </summary>
		void UpdateSemanticsSizes()
		{
			_clearButtonSemanticsSize = new Size(_clearButtonRectF.Width, _clearButtonRectF.Height);
		}

		/// <summary>
		/// Checks whether the current semantic data is still valid and does not need rebuilding.
		/// </summary>
		/// <returns>True if the semantic data is current, otherwise false.</returns>
		bool SemanticsDataIsCurrent()
		{
			return _numericEntrySemanticsNodes.Count != 0 &&
				   _clearButtonSemanticsSize == new Size(_clearButtonRectF.Width, _clearButtonRectF.Height);
		}

		/// <summary>
		/// Updates the properties of the entry view.
		/// </summary>
		void UpdateEntryProperties()
		{
			if (_textBox != null)
			{
				_textBox.FontAttributes = FontAttributes;
				_textBox.FontFamily = FontFamily;
				_textBox.FontAutoScalingEnabled = FontAutoScalingEnabled;
				_textBox.FontSize = FontSize;
				_textBox.TextColor = TextColor;
				_textBox.HorizontalTextAlignment = HorizontalTextAlignment;
				_textBox.VerticalTextAlignment = VerticalTextAlignment;
#if ANDROID
                _textBox.HeightRequest = HeightRequest;
#endif
			}
		}

		/// <summary>
		/// Sets the data binding for the <see cref="SfEntryView"/> and <see cref="SfNumericEntry"/>.
		/// </summary>
		void SetBinding()
		{
			if (_textBox != null)
			{
				_textBox.BindingContext = this;
				_textBox.SetBinding(SfEntryView.IsEnabledProperty, BindingHelper.CreateBinding(nameof(SfNumericEntry.IsEnabled), getter: static(SfNumericEntry entry) => entry.IsEnabled));	
				_textBox.SetBinding(SfEntryView.IsVisibleProperty, BindingHelper.CreateBinding(nameof(SfNumericEntry.IsVisible), getter: static(SfNumericEntry entry) => entry.IsVisible));
				_textBox.SetBinding(SfEntryView.PlaceholderColorProperty, BindingHelper.CreateBinding(nameof(SfNumericEntry.PlaceholderColor), getter: static(SfNumericEntry entry) => entry.PlaceholderColor));
				_textBox.SetBinding(SfEntryView.CursorPositionProperty, BindingHelper.CreateBinding<SfNumericEntry, int>(nameof(SfNumericEntry.CursorPosition), getter: entry => entry.CursorPosition, setter: (entry, value) => entry.CursorPosition = value, mode: BindingMode.TwoWay));
				_textBox.SetBinding(SfEntryView.SelectionLengthProperty, BindingHelper.CreateBinding<SfNumericEntry, int>(nameof(SfNumericEntry.SelectionLength), getter: entry => entry.SelectionLength, setter: (entry, value) => entry.SelectionLength = value, mode: BindingMode.TwoWay));

#if WINDOWS
				if(_textBox.Text != null)
				{
					_textBox.CursorPosition = _textBox.Text.Length;
				}
#endif
			}
		}

		/// <summary>
		/// Initializes the elements needed for the control's GUI and behavior.
		/// </summary>
		void InitializeElements()
		{
			_effectsRenderer ??= new EffectsRenderer(this) { RippleAnimationDuration = 100};

			UpdateEntryProperties();
			UpdateClearButtonVisibility();
			SetRendererBasedOnPlatform();
			GetMinimumSize();
			HookEvents();
		}

#if WINDOWS

        /// <summary>
        /// Helps to get the core window based on the configuration.
        /// </summary>
        /// <param name="virtualKey">The virtual key.</param>
        /// <returns>The CoreVirtualKeyStates.</returns>
        static Windows.UI.Core.CoreVirtualKeyStates GetVirtualKeyStates(VirtualKey virtualKey)
        {
            return Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(virtualKey);
        }

		/// <summary>
		/// Handles the event when a pointer enters the control's area.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A PointerRoutedEventArgs that contains the event data.</param>
		void OnPointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_effectsRenderer != null && _effectsRenderer.HighlightBounds.Width > 0 && _effectsRenderer.HighlightBounds.Height > 0)
            {
                _effectsRenderer.RemoveHighlight();
            }
            if (_textBox != null && !_textBox.IsFocused)
            {
                VisualStateManager.GoToState(this, "PointerOver");
            }
        }

		/// <summary>
		/// Handles the event when a pointer exits the control's area.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A PointerRoutedEventArgs that contains the event data.</param>
		void OnPointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_textBox != null && !_textBox.IsFocused)
            {
                VisualStateManager.GoToState(this, "Normal");
            }
        }

		/// <summary>
		/// Sets focus to the input view.
		/// </summary>
		void SetInputViewFocus()
        {
            _textBox?.Focus();
            _isFocus = false;
        }
#endif

#if ANDROID

		/// <summary>
		/// Handles the BeforeTextChanged event for the Android entry control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A TextChangedEventArgs that contains the event data.</param>
		void AndroidEntry_BeforeTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
        {
            var entry = sender as AndroidX.AppCompat.Widget.AppCompatEditText;
            if (entry != null && e.Text != null)
            {
                _previousText = e.Text.ToString() ?? (AllowNull ? "" : "0");
            }
            _shiftCursor = false;
            if (entry != null && entry.Text != null && entry.Text.Contains('-', StringComparison.Ordinal) && _textBox != null && _textBox.CursorPosition == 0)
            {
                _shiftCursor = true;
			}
		}

		/// <summary>
		/// Determines whether the current text change is valid.
		/// </summary>
		/// <returns>True if the change is valid; otherwise, false.</returns>
		bool IsValidChange()
        {
			return string.IsNullOrEmpty(_previousText) ||
				   double.TryParse(_previousText.Replace(GetNumberDecimalSeparator(GetNumberFormat()),
				   GetNumberDecimalSeparator(CultureInfo.CurrentUICulture.NumberFormat), StringComparison.Ordinal), out _);
        }

        /// <summary>
        /// Determines if a new character has been typed.
        /// </summary>
        /// <param name="lengthBefore">Length of the text before the change.</param>
        /// <param name="lengthAfter">Length of the text after the change.</param>
        /// <param name="selectionStart">The starting position of the selection.</param>
        /// <returns>True if a new character has been typed, false otherwise.</returns>
        bool IsNewCharacterTyped(int lengthBefore, int lengthAfter, int selectionStart)
        {
            return ((lengthAfter == 1 && lengthBefore == 0) || ((lengthAfter - lengthBefore) == 1) || (lengthBefore > 0 && lengthAfter == 1)) && selectionStart > 0;
        }

        /// <summary>
        /// Retrieves the character that was typed.
        /// </summary>
        /// <param name="text">The current text in the input field.</param>
        /// <param name="start">The starting position of the change.</param>
        /// <param name="lengthBefore">Length of the text before the change.</param>
        /// <param name="lengthAfter">Length of the text after the change.</param>
        /// <param name="entry">The EditText control.</param>
        /// <returns>The character that was typed.</returns>
        char GetTypedChar(string text, int start, int lengthBefore, int lengthAfter, AndroidX.AppCompat.Widget.AppCompatEditText entry)
        {
            if (start > 0)
            {
                int charIndex = (lengthAfter > 1) ? lengthBefore : 0;
                if (text.Length > start)
                {
                    return text.Remove(0, start)[charIndex];
                }
            }
            else if (start == 0)
            {
                return text.Substring(entry.SelectionStart - 1, 1)[0];
            }
            return '\0';
        }

        /// <summary>
        /// Handles the input of a new character.
        /// </summary>
        /// <param name="text">The current text in the input field.</param>
        /// <param name="start">The starting position of the change.</param>
        /// <param name="lengthBefore">Length of the text before the change.</param>
        /// <param name="lengthAfter">Length of the text after the change.</param>
        /// <param name="entry">The EditText control.</param>
        /// <param name="caretPosition">The current position of the caret.</param>
        /// <param name="isNegative">Indicates if the number is negative.</param>
        /// <param name="decimalSeparator">The decimal separator for the current culture.</param>
        void HandleNewCharacter(string text, int start, int lengthBefore, int lengthAfter, AndroidX.AppCompat.Widget.AppCompatEditText entry, int caretPosition, bool isNegative, string decimalSeparator)
        {
            ////If we typed new character below condition become true
            char typedChar = GetTypedChar(text, start, lengthBefore, lengthAfter, entry);

            if (typedChar.Equals(decimalSeparator[0]))
            {
                HandleDecimalSeparatorInput(caretPosition, decimalSeparator);
            }
            else if (typedChar.Equals('-'))
            {
                HandleNegativeInput(isNegative, caretPosition);
            }
            else if (char.IsNumber(typedChar))
            {
                HandleNumbers(typedChar.ToString(), caretPosition, decimalSeparator);
            }
            else
            {
                RevertToPreviousText(caretPosition);
            }
        }

        /// <summary>
        /// Handles the input of a decimal separator.
        /// </summary>
        /// <param name="caretPosition">The current position of the caret.</param>
        /// <param name="decimalSeparator">The decimal separator for the current culture.</param>
        void HandleDecimalSeparatorInput(int caretPosition, string decimalSeparator)
        {
            if (_textBox is null)
            {
                return;
            }

            if ((_maximumPositiveFractionDigit != 0 && !_textBox.Text.Contains('-', StringComparison.Ordinal)) ||
                (_maximumNegativeFractionDigit != 0 && _textBox.Text.Contains('-', StringComparison.Ordinal)))
            {
                HandleDecimalSeparator(decimalSeparator, caretPosition);
            }
            else
            {
                RevertToPreviousText(caretPosition);
            }
        }

        /// <summary>
        /// Reverts the text to its previous state and updates the cursor position.
        /// </summary>
        /// <param name="caretPosition">The current position of the caret.</param>
        void RevertToPreviousText(int caretPosition)
        {
             if (_textBox is null)
            {
                return;
            }
            _textBox.Text = _previousText;
            _cursorPosition = caretPosition;
        }

        /// <summary>
        /// Handles the input of a negative sign.
        /// </summary>
        /// <param name="isNegative">Indicates if the number is already negative.</param>
        /// <param name="caretPosition">The current position of the caret.</param>
        void HandleNegativeInput(bool isNegative, int caretPosition)
        {
            //HandleNegativeKey(isNegative, caretPosition);
            //// Unable to achieve -0, app breaks without any exception.
            if (_textBox is not null && string.IsNullOrEmpty(_previousText))
            {
                _textBox.Text = "-0";
                _cursorPosition = 2;
            }
            else
            {
                HandleNegativeKey(isNegative, caretPosition);
            }
        }

        /// <summary>
        /// Handles the deletion of a character.
        /// </summary>
        /// <param name="caretPosition">The current position of the caret.</param>
        /// <param name="decimalSeparator">The decimal separator for the current culture.</param>
        void HandleDeletion(int caretPosition, string decimalSeparator)
        {
            if (_textBox is null)
            {
                return;
            }

            if (double.TryParse(_textBox.Text, out double result) && Maximum != result)
            {
                _cursorPosition = caretPosition;
            }
        }

        /// <summary>
        /// Validates text input to ensure it conforms with desired number formatting.
        /// </summary>
        void ValidateTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
        {
            if (e is not null && sender is AndroidX.AppCompat.Widget.AppCompatEditText entry &&
            !string.IsNullOrEmpty(entry.Text) && entry.IsFocused && _textBox != null && IsValidChange())
            {
                string? text = e.Text?.ToString();
                int lengthBefore = e.BeforeCount;
                int lengthAfter = e.AfterCount;
                int start = e.Start;
                int caretPosition = _shiftCursor ? e.Start + 1 : e.Start;
                bool isNegative = IsNegative(_previousText, GetNumberFormat());
                string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
                _selectionLength = lengthBefore - lengthAfter + 1;
                _selectionLength = _selectionLength < 0 ? 0 : _selectionLength;
                if (text == null)
                {
                    return;
                }

                if (IsNewCharacterTyped(lengthBefore, lengthAfter, entry.SelectionStart))
                {
                    HandleNewCharacter(text, start, lengthBefore, lengthAfter, entry, caretPosition, isNegative, decimalSeparator);
                }
                else if (lengthBefore - lengthAfter == 1)
                {
                    HandleDeletion(caretPosition, decimalSeparator);
                }
                else if (lengthBefore > lengthAfter)
                {
                    HandleMultipleCharactersDeletion(text, caretPosition, lengthBefore);
                }
                else if (IsMultipleCharactersInsertion(lengthBefore, lengthAfter))
                {
                    HandleMultipleCharactersInsertion(text, caretPosition, lengthAfter);
                }
                else
                {
                    _cursorPosition = caretPosition + lengthBefore;
                }

                _previousText = _textBox.Text;
                
            }
        }

        /// <summary>
        /// Determines if multiple characters are being inserted.
        /// </summary>
        /// <param name="lengthBefore">Length of the text before the change.</param>
        /// <param name="lengthAfter">Length of the text after the change.</param>
        /// <returns>True if multiple characters are being inserted, false otherwise.</returns>
        bool IsMultipleCharactersInsertion(int lengthBefore, int lengthAfter)
        {
            return lengthBefore < lengthAfter && lengthAfter - lengthBefore > 1;
        }

        /// <summary>
        /// Handles the insertion of multiple characters.
        /// </summary>
        /// <param name="text">The current text in the input field.</param>
        /// <param name="caretPosition">The current position of the caret.</param>
        /// <param name="lengthAfter">Length of the text after the change.</param>
        void HandleMultipleCharactersInsertion(string text, int caretPosition, int lengthAfter)
        {
            if (double.TryParse(text, out _))
            {
                _cursorPosition = caretPosition + lengthAfter;
            }
            else
            {
                if (_textBox is not null)
                {
                    _textBox.Text = _previousText;
                }
            }
        }

        /// <summary>
        /// Handles the deletion of multiple characters.
        /// </summary>
        /// <param name="text">The current text in the input field.</param>
        /// <param name="caretPosition">The current position of the caret.</param>
        void HandleMultipleCharactersDeletion(string text, int caretPosition, int lengthBefore)
        {
            if (!double.TryParse(text, out _))
            {
                RevertToPreviousText(caretPosition);
            }
            else
            {
                _cursorPosition = caretPosition + lengthBefore;
            }
        }

		/// <summary>
		/// Handles the AfterTextChanged event for the Android entry control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An AfterTextChangedEventArgs that contains the event data.</param>
		void AndroidEntry_AfterTextChanged(object? sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var entry = sender as AndroidX.AppCompat.Widget.AppCompatEditText;
            string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
            if (entry != null && (entry.Text == "-" || entry.Text == decimalSeparator))
            {
                if (!_isSamsungWithSamsungKeyboard && _textBox != null)
                {
                    _textBox.Text = AllowNull ? string.Empty : "0";
                }
            }
            if (_isSamsungWithSamsungKeyboard && _textBox != null && _textBox.Text.Length > 1)
            {
                if (_textBox.Text[0] == decimalSeparator[0] && _textBox.Text.LastIndexOf(decimalSeparator) == 0)
                {
                    _textBox.Text = "0" + _textBox.Text;
                }
            }
            if (!AllowNull && entry != null && entry.Text == string.Empty && _textBox != null)
            {
                _textBox.Text = "0";
                _cursorPosition = 1;
            }
            if (ValueChangeMode == ValueChangeMode.OnLostFocus && _textBox != null)
            {
                _textBox.CursorPosition = _cursorPosition;
                _cursorPosition = 0;
            }
        }

		/// <summary>
		/// Validate the device is Samsung and which keyboard is used.
		/// </summary>
		void CheckDeviceAndKeyboard()
		{
			if (string.Equals(global::Android.OS.Build.Manufacturer, "samsung", StringComparison.OrdinalIgnoreCase))
			{
				var context = Android.App.Application.Context;
				string currentKeyboard = KeyboardChecker.GetCurrentKeyboard(context);
				_isSamsungWithSamsungKeyboard = currentKeyboard == "Samsung Keyboard";
			}
			else
			{
				_isSamsungWithSamsungKeyboard = false;
			}
		}

#endif

#if MACCATALYST
        /// <summary>
        /// Occurs when key is pressed.
        /// </summary>
        /// <param name="e">The KeyEventArgs.</param>
        void OnTextBoxPreviewKeyDown(KeyEventArgs e)
        {
            if (_textBox == null || _textBox.IsEnabled)
            {
                return;
            }

            if (_textBox.IsReadOnly && e.Key != KeyboardKey.Up && e.Key != KeyboardKey.Down
                && e.Key != KeyboardKey.PageUp && e.Key != KeyboardKey.PageDown)
            {
                return;
            }

            //Reset SelectionStart value when it increased to higher than text's length. 
            if (_textBox.Text.Length < _textBox.CursorPosition)
            {
                _textBox.CursorPosition = _textBox.Text.Length;
            }

            int caretPosition = _textBox.CursorPosition;
            bool isNegative = IsNegative(_textBox.Text, GetNumberFormat());

            if (e.Key == KeyboardKey.Left || e.Key == KeyboardKey.Right || e.Key == KeyboardKey.Home ||
                e.Key == KeyboardKey.End || e.Key == KeyboardKey.Up || e.Key == KeyboardKey.Down ||
                e.Key == KeyboardKey.PageUp || e.Key == KeyboardKey.PageDown)
            {
                // Handles when the arrow key or page up/down key is pressed.
                e.Handled = HandleNavigation(e.Key, isNegative, caretPosition);
            }
            else
            {
                e.Handled = false;
            }


        }
#endif

#if MACCATALYST || IOS

		/// <summary>
		/// Determines whether the specified text field should allow the proposed character changes across multiple ranges.
		/// </summary>
		/// <remarks>This method is typically used to validate complex text changes, such as those resulting from
		/// multi-range edits or input methods. It merges the provided ranges before validation to ensure consistent
		/// behavior.</remarks>
		/// <param name="textField">The text field whose contents are being modified.</param>
		/// <param name="ranges">An array of ranges, each represented by an NSValue, indicating the portions of text to be replaced.</param>
		/// <param name="replacementString">The string to replace the text in the specified ranges.</param>
		/// <returns>true if the character changes are valid and should be allowed; otherwise, false.</returns>
		private bool Handle_ShouldChangeCharactersInRanges(UITextField textField, NSValue[] ranges, string replacementString)
		{
			return ValidateTextChanged(textField, MergeRanges(ranges), replacementString);
		}

		/// <summary>
		/// Validates and handles text changes in the UITextField.
		/// </summary>
		/// <param name="textField">The UITextField being modified.</param>
		/// <param name="range">The range of text being replaced.</param>
		/// <param name="replacementString">The string to replace the text in the specified range.</param>
		/// <returns>Always returns false to indicate that the text field should not handle the text change itself.</returns>
		bool ValidateTextChanged(UITextField textField, NSRange range, string replacementString)
		{
			if(textField.Text==null)
			{
				return false;
			}
			_previousText = textField.Text;
			char typedChar;
			int caretPosition = (int)range.Location;
			bool isNegative = IsNegative(_previousText, GetNumberFormat());
			string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());

			if (string.IsNullOrEmpty(replacementString) && range.Length > 1)
			{
				PerformCut(caretPosition);
			}
			else if (string.IsNullOrEmpty(replacementString))
			{
				HandleBackspace(caretPosition + 1, decimalSeparator);
			}
			//If we typed new character below condition become true
			else if (replacementString.Length == 1)
			{
				typedChar = replacementString[0];

				if (typedChar.Equals(decimalSeparator[0]))
				{
					if ((_maximumPositiveFractionDigit != 0 && _textBox != null && !_textBox.Text.Contains('-', StringComparison.Ordinal)) || (_maximumNegativeFractionDigit != 0 && _textBox != null && _textBox.Text.Contains('-', StringComparison.Ordinal)))
					{
						HandleDecimalSeparator(decimalSeparator, caretPosition);
					}
				}
				else if (typedChar.Equals('-'))
				{
					if (string.IsNullOrEmpty(_previousText) && _textBox != null)
					{
						_textBox.Text = "-0";
						_textBox.CursorPosition = 2;
					}
					else
					{
						HandleNegativeKey(isNegative, caretPosition);
					}
				}
				else if (char.IsNumber(typedChar))
				{
					HandleNumbers(replacementString, caretPosition, decimalSeparator);
				}
				else
				{
					textField.Text = _previousText;
				}

			}
			else if (replacementString.Length > 1)
			{
				OnTextBoxPaste(textField, caretPosition);
			}
			return false;
		}

		/// <summary>
		/// Combines multiple NSRange values into a single range that encompasses all specified ranges.
		/// </summary>
		/// <remarks>If only one range is provided, that range is returned unchanged. The merged range may not be
		/// contiguous if the input ranges are disjoint.</remarks>
		/// <param name="ranges">An array of NSValue objects representing the ranges to merge. Must contain at least one element.</param>
		/// <returns>An NSRange that starts at the minimum location of the input ranges and has a length equal to the sum of their
		/// lengths.</returns>
		private static NSRange MergeRanges(NSValue[] ranges)
		{
			var combinedRange = ranges.Length == 1
				? ranges[0].RangeValue
				: new Foundation.NSRange(
					ranges.Min(r => r.RangeValue.Location),
					(nint)ranges.Sum(r => r.RangeValue.Length)
				);
			return combinedRange;
		}

		/// <summary>
		/// Performs a cut operation on the text in the associated text box.
		/// </summary>
		/// <param name="caretPosition">The current caret position in the text box.</param>
		void PerformCut(int caretPosition)
		{
			if (_textBox != null)
			{
				string selectedText = _textBox.Text.Substring(caretPosition, _textBox.SelectionLength);
				Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.SetTextAsync(selectedText);
				_textBox.Text = RemoveSelectedText(_previousText, selectedText, caretPosition, GetNumberFormat());
				_textBox.CursorPosition = caretPosition;
			}
		}

		/// <summary>
		/// Occurs when text is pasted in Entry.
		/// </summary>
		/// <param name="sender">NumberBox control.</param>
		/// <param name="caretPosition">character's cursor position</param>
		async void OnTextBoxPaste(object? sender, int caretPosition)
		{
			if (_textBox != null)
			{
				if (Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.HasText)
				{
					try
					{
						// Validate the input and insert the copied text in cursor position.
						var copiedText = await Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.GetTextAsync();
						if (!string.IsNullOrEmpty(copiedText))
						{
							copiedText = ValidatePastedText(copiedText, GetNumberFormat());
							string displayText = _previousText;
#if ANDROID
                            string selectedText = _previousText.Substring(caretPosition, _selectionLength);
                            _cursorPosition = caretPosition;
#else
							string selectedText = _previousText.Substring(_textBox.CursorPosition, _textBox.SelectionLength);
							caretPosition = _textBox.CursorPosition;
#endif
							if (!string.IsNullOrEmpty(copiedText) && CanPaste(copiedText))
							{
								InsertNumbers(displayText, selectedText, copiedText, caretPosition);
							}
						}
					}
					catch (System.Exception)
					{
						throw;
					}
				}
			}
		}
#endif

		/// <summary>
		/// Validates and updates the current text input by resetting the value when it goes beyond allowable limits.
		/// </summary>
		static double? ValidateAndUpdateValue(SfNumericEntry numberBox, double? newValue)
		{
			bool isFocused = numberBox.IsFocused;
			bool onKeyFocusMode = numberBox.ValueChangeMode == ValueChangeMode.OnKeyFocus;
			if(!isFocused && newValue != null && !double.IsNaN((double)newValue))
			{
				newValue = numberBox.ValidateMinMax(newValue);
			}
			
			if (isFocused && onKeyFocusMode && newValue != null && !double.IsNaN((double)newValue))
			{
				newValue = Math.Clamp((double)newValue, double.MinValue, numberBox.Maximum);
			}

			numberBox.SetValue(ValueProperty, newValue);
			return newValue;
		}

		/// <summary>
		/// Raises the ValueChanged event for the SfNumericEntry control.
		/// </summary>
		/// <param name="numberBox">The SfNumericEntry control.</param>
		/// <param name="oldValue">The previous value.</param>
		/// <param name="newValue">The new value.</param>
		static async void RaiseValueChangedEvent(SfNumericEntry numberBox, double? oldValue, double? newValue)
		{
			var valueChangedEventArgs = new NumericEntryValueChangedEventArgs(newValue, oldValue);
			//Fix included for Value loop issue.
			await Task.Yield();
			numberBox.ValueChanged?.Invoke(numberBox, valueChangedEventArgs);
		}

		/// <summary>
		/// Converts various numeric types and strings to a nullable double.
		/// </summary>
		/// <param name="newValue">The value to be converted.</param>
		/// <param name="oldValue">The previous value, used as fallback for string conversion.</param>
		/// <returns>A nullable double representation of the input value.</returns>
		static double? ConvertToDouble(object newValue, object oldValue)
		{
			if (newValue is int or byte or short or ushort or uint or double or long or ulong or float)
			{
				return Convert.ToDouble(newValue);
			}

			if (newValue is string stringValue)
			{
				return double.TryParse(stringValue, out double result) ? result : Convert.ToDouble((double?)oldValue);
			}

			if (!(newValue is not char))
			{
				return Convert.ToDouble((char)newValue);
			}

			return newValue as double?;
		}

		/// <summary>
		/// Updates the text box display and button colors of the SfNumericEntry control.
		/// </summary>
		/// <param name="numberBox">The SfNumericEntry control to update.</param>
		/// <param name="newValue">The new value to be displayed.</param>
		static void UpdateTextBoxAndButtonColors(SfNumericEntry numberBox, double? newValue)
		{
#if WINDOWS
			numberBox.UpdateDisplayText(newValue,resetCursor: false);
#else
			numberBox.UpdateDisplayText(newValue);
#endif

			if ((newValue == null || double.IsNaN(newValue.Value)) && numberBox._textBox != null)
			{
				double? defaultValue = numberBox.ValidateMinMax(0.0);
				numberBox._textBox.Text = numberBox.AllowNull ? string.Empty :
										 defaultValue?.ToString(numberBox.GetNumberFormat());
			}

			numberBox.UpdateButtonColor(newValue);

			if (numberBox._textBox != null && !numberBox.IsFocused && !numberBox._textBox.IsFocused)
			{
				var isValueChange = newValue != numberBox.Value;
				numberBox.FormatValue(isValueChange);
			}
			numberBox._valueUpdating = false;
		}

#if WINDOWS
		/// <summary>
		/// Retrieves the grandparent element of the _textBox control.
		/// </summary>
		void GetParentElement()
		{
			if (_textBox is not null && _textBox.Handler is Microsoft.Maui.Handlers.EntryHandler handler && handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox nativeTextBox)
			{
				_grandParentElement = (nativeTextBox.Parent as Microsoft.UI.Xaml.FrameworkElement)?.Parent as Microsoft.Maui.Platform.WrapperView;
			}
		}

		/// <summary>
		/// Sets the flow direction of the _grandParentElement based on the text direction (RTL or LTR).
		/// </summary>
		void SetFlowDirection()
		{
			if (_grandParentElement is not null)
			{
				// Set the flow direction based on RTL or LTR
				_grandParentElement.FlowDirection = IsRTL() ? Microsoft.UI.Xaml.FlowDirection.RightToLeft
																: Microsoft.UI.Xaml.FlowDirection.LeftToRight;
			}
		}
#endif

#if MACCATALYST || IOS
		/// <summary>
		/// Returns the valid entered text to the Value of NumericEntry.
		/// </summary>
		void GetValidText(string accentKeys)
		{
			if (_textBox is null)
			{
				return;
			}
			for (int i = 0; i < accentKeys.Length; i++)
			{
				if (_textBox.Text.Contains(accentKeys[i], StringComparison.Ordinal))
				{
					_isNotValidText = true;
					return;
				}
			}
			_isNotValidText = false;
		}
#endif

		/// <summary>
		/// Handles the focus event on the text box.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">Focus event arguments.</param>
		void TextBoxOnGotFocus(object? sender, FocusEventArgs e)
		{
#if ANDROID
            if (_isFirstFocus && (sender is SfEntryView entry) && entry != null && entry.Handler != null && entry.Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
            {
                androidEntry.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagSigned | InputTypes.NumberFlagDecimal;
                androidEntry.KeyListener = global::Android.Text.Method.DigitsKeyListener.GetInstance(KEYS);
                _isFirstFocus = false;
            }
#endif
			SetValue(VisualElement.IsFocusedPropertyKey, e.IsFocused);
			RaiseFocusedEvent(e);
			OnGotFocus();

			UpdateClearButtonVisibility();
		}

		/// <summary>
		/// Updates the color of the button based on the provided value.
		/// </summary>
		/// <param name="value">A nullable double that determines the color change. 
		/// If null, the button may revert to a default color.</param>
		internal virtual void UpdateButtonColor(double? value)
		{

		}

		/// <summary>
		/// Handles a key press event.
		/// </summary>
		/// <param name="key">The keyboard key that was pressed.</param>
		/// <returns>True if the key press was handled; otherwise, false.</returns>
		internal virtual bool HandleKeyPressed(KeyboardKey key)
		{
			return false;
		}

		/// <summary>
		/// Handles a key press event for paging operations.
		/// </summary>
		/// <param name="key">The keyboard key that was pressed.</param>
		/// <returns>True if the key press was handled; otherwise, false.</returns>
		internal virtual bool HandleKeyPagePressed(KeyboardKey key)
		{
			return false;
		}

		/// <summary>
		/// Handles the lost focus event on the text box.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">Focus event arguments.</param>	
		void TextBoxOnLostFocus(object? sender, FocusEventArgs e)
		{
			OnLostFocus();
			SetValue(VisualElement.IsFocusedPropertyKey, e.IsFocused);
			RaiseUnfocusedEvent(e);
			UpdateClearButtonVisibility();
		}


		/// <summary>
		/// Initialize the numeric entry
		/// </summary>
		void Initialize()
		{
			// Initialize resources for the numeric entry control.
			SfNumericEntryResources.InitializeDefaultResource("Syncfusion.Maui.Toolkit.NumericEntry.Resources.SfNumericEntry", typeof(SfNumericEntry));

			// Set the drawing order to ensure key elements are drawn correctly
			DrawingOrder = DrawingOrder.AboveContent;

			// Add touch and keyboard listeners to the control
			this.AddTouchListener(this);
			this.AddKeyboardListener(this);

			// Initialize and setup the text box entry view
			_textBox = new SfEntryView();

			// Initialize graphical and interactive elements of the control
			InitializeElements();

			// Add the text box to the control's visual tree
			Add(_textBox);

			// Determine if the description was set by the user, used for accessibility
			_isDescriptionNotSetByUser = string.IsNullOrEmpty(SemanticProperties.GetDescription(this));
		}

		/// <summary>
		/// Updates the clear button visibility based on control's state.
		/// </summary>
		void UpdateClearButtonVisibility()
		{
			if (_textBox == null)
			{
				return;
			}

			if (ShowClearButton && _textBox.IsFocused && !_textBox.IsReadOnly
						&& !string.IsNullOrEmpty(_textBox.Text) && IsEnabled && IsEditable)
			{
				_isClearButtonVisible = true;
			}
			else
			{
				_isClearButtonVisible = false;
			}

			UpdateElementsBounds(_previousRectBounds);
		}

#if !ANDROID

		/// <summary>
		/// Validates the pasted text and formats it according to the number format of the control.
		/// </summary>
		/// <param name="input">The text input to validate and format.</param>
		/// <param name="numberFormatInfo">Information on the numerical format to apply.</param>
		/// <returns>A formatted string.</returns>
		static string? ValidatePastedText(string input, NumberFormatInfo numberFormatInfo)
		{
			string? result = string.Empty;
			string decimalSeparator = GetNumberDecimalSeparator(numberFormatInfo);
			string negativeSign = GetNegativeSign(numberFormatInfo);

			// Removes the non-numeric characters except decimal separator and negative sign.
			string pattern = @"[^\d" + decimalSeparator + negativeSign + "]+";
			input = Regex.Replace(input, pattern, string.Empty);

			foreach (char character in input)
			{
				// Ignores the duplicate decimal separator.
				// Ignores the input containing negative sign after the first digit.
				if ((character.ToString() == decimalSeparator && result.Contains(decimalSeparator, StringComparison.CurrentCulture)) ||
					((character.ToString() == negativeSign) && !string.IsNullOrEmpty(result)))
				{
					continue;
				}

				result += character;
			}

			result = double.TryParse(result, out _) || result == decimalSeparator ? result : null;
			return result;
		}
#endif

		/// <summary>
		/// Gets the current culture number decimal separator.
		/// </summary>
		static string GetNumberDecimalSeparator(NumberFormatInfo numberFormatInfo)
		{
			return numberFormatInfo.NumberDecimalSeparator;
		}

		/// <summary>
		/// Gets the current culture negative sign.
		/// </summary>
		static string GetNegativeSign(NumberFormatInfo numberFormatInfo)
		{
			return numberFormatInfo.NegativeSign;
		}

		/// <summary>
		/// Checks whether zero is needed for the display text. Zero is needed when
		/// display text is empty or no numbers before the decimal separator.
		/// </summary>
		bool PrefixZeroIfNeeded(ref string displayText, bool isNegative, NumberFormatInfo numberFormatInfo)
		{
			string negativeSign = GetNegativeSign(numberFormatInfo);
			string decimalSeparator = GetNumberDecimalSeparator(numberFormatInfo);
			displayText = Culture != null && Culture.TextInfo.IsRightToLeft && negativeSign.Length > 1 ? displayText.Trim(negativeSign[1]) : displayText.TrimStart(negativeSign[0]);

			bool startsWithDecimal = displayText.StartsWith(decimalSeparator, StringComparison.CurrentCulture);
			bool isZeroNeeded = startsWithDecimal || string.IsNullOrEmpty(displayText);
			bool isCultureRightToLeftWithNegativeSign = Culture != null &&
														Culture.TextInfo.IsRightToLeft &&
														negativeSign.Length > 1 &&
														displayText.Contains(negativeSign[1], StringComparison.Ordinal)? false : true;
			if (isCultureRightToLeftWithNegativeSign)
			{
				displayText = isNegative ? (negativeSign + displayText) : displayText;
			}
			displayText = isZeroNeeded ? displayText.Insert(isNegative ? 1 : 0, "0") : displayText;

			return isZeroNeeded;
		}

		/// <summary>
		/// Removes selected text if it does not contain a decimal separator.
		/// </summary>
		static string RemoveSelectedText(string text, string selectedText, int caretPosition, NumberFormatInfo numberFormatInfo)
		{
			bool hasSelectedAll = text.Length == selectedText.Length;
			bool selectedTextHasDecimal = selectedText.Contains(GetNumberDecimalSeparator(numberFormatInfo), StringComparison.CurrentCulture);
			if (hasSelectedAll || !selectedTextHasDecimal)
			{
				text = text.Remove(caretPosition, selectedText.Length);
			}
#if WINDOWS
            else if ((text.Length == selectedText.Length + 1) && !selectedText.Contains('-', StringComparison.Ordinal) && text.Contains('-', StringComparison.Ordinal))
            {
                text = text.Remove(0, selectedText.Length + 1);
            }
#endif
			return text;
		}

		/// <summary>
		/// Checks whether the given value is negative or not.
		/// </summary>
		static bool IsNegative(string text, NumberFormatInfo numberFormatInfo)
		{
			return text.StartsWith(GetNegativeSign(numberFormatInfo), StringComparison.CurrentCulture);
		}

		/// <summary>
		/// Update the textbox visibility based on <see cref="EntryVisibility"/> property.
		/// </summary>
		void UpdateEntryVisibility()
		{
			if (_textBox != null)
			{
				_textBox.IsVisible = true;
			}
		}

		/// <summary>
		/// To get the NumberFormat from number formatter, culture property or current UI culture.
		/// </summary>
		NumberFormatInfo GetNumberFormat()
		{
			// Determine the culture to use for number formatting.
			CultureInfo selectedCulture = CustomFormat != null ?
										(Culture ?? CultureInfo.CurrentUICulture) :
										(Culture ?? CultureInfo.CurrentUICulture);

			// Return the NumberFormatInfo of the selected culture.
			return selectedCulture.NumberFormat;
		}

		/// <summary>
		/// Handles the event when the IsEnabled property changes.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		void OnIsEnabledChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			UpdateVisualState();
		}

#if WINDOWS
        /// <summary>
        /// Helps to convert the virtual key to the Keyboard Key. 
        /// </summary>
        KeyboardKey ConvertToKeyboardKey(VirtualKey virtualKey)
        {
            KeyboardKey key = KeyboardKey.None;

            switch (virtualKey)
            {
                case VirtualKey.Left:
                    key = KeyboardKey.Left;
                    break;
                case VirtualKey.Right:
                    key = KeyboardKey.Right;
                    break;
                case VirtualKey.Up:
                    key = KeyboardKey.Up;
                    break;
                case VirtualKey.Down:
                    key = KeyboardKey.Down;
                    break;
                case VirtualKey.PageUp:
                    key = KeyboardKey.PageUp;
                    break;
                case VirtualKey.PageDown:
                    key = KeyboardKey.PageDown;
                    break;
                case VirtualKey.Home:
                    key = KeyboardKey.Home;
                    break;
            }

            return key;
        }
#endif

#if WINDOWS
        /// <summary>
        /// Occurs when key is pressed.
        /// </summary>
        /// <param name="sender">The NumberBox control.</param>
        /// <param name="e">The DependencyPropertyChanged EventArgs.</param>
        internal void OnTextBoxPreviewKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (_textBox != null)
            {
                if (_textBox.IsReadOnly && e.Key != VirtualKey.Up && e.Key != VirtualKey.Down
                    && e.Key != VirtualKey.PageUp && e.Key != VirtualKey.PageDown)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(_textBox.Text) && _textBox.Text.Length < _textBox.CursorPosition)
                {
                    _textBox.CursorPosition = _textBox.Text.Length;
                }
                int caretPosition = _textBox.CursorPosition;
                KeyboardKey key = ConvertToKeyboardKey(e.Key);
                string text = e.Key.ToString();
                bool isNegative = !string.IsNullOrEmpty(_textBox.Text) && IsNegative(_textBox.Text, GetNumberFormat());
                bool isCtrlKeyPressed = GetVirtualKeyStates(VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
                bool isAltKeyPressed = GetVirtualKeyStates(VirtualKey.Menu).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
                bool isShiftKeyPressed = GetVirtualKeyStates(VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
                string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
                bool areModifierKeysPressed = isCtrlKeyPressed || isAltKeyPressed || isShiftKeyPressed;

                if (!areModifierKeysPressed && text.Contains("Number", StringComparison.CurrentCulture))
                {
                    // Handles when the number is pressed.
                    e.Handled = true;
                    HandleNumbers(text, caretPosition, decimalSeparator);
                }

                else if (!areModifierKeysPressed && (e.Key == VirtualKey.Decimal || (int)e.Key == 190 && decimalSeparator == "." || (int)e.Key == 188 && decimalSeparator == ","))
                {
                    // Handles when the pressed key is decimal and current culture decimal separator. Suppose if we type decimal in french culture, we need to
                    // insert the decimal separator based on french culture.
                    e.Handled = true;

                    //Allow to enter the decimal separator when maximum fraction digit is not 0.
                    if ((_maximumPositiveFractionDigit != 0 && !_textBox.Text.Contains('-', StringComparison.Ordinal)) || (_maximumNegativeFractionDigit != 0 && _textBox.Text.Contains('-', StringComparison.Ordinal)))
                    {
                        HandleDecimalSeparator(decimalSeparator, caretPosition);
                    }
                }
                else if (e.Key == VirtualKey.Back)
                {
                    // Handles when the backspace key is pressed.
                    e.Handled = true;
                    HandleBackspace(caretPosition, decimalSeparator);
                }
                else if (!isShiftKeyPressed && e.Key == VirtualKey.Delete)
                {
                    // Handles when the delete key is pressed
                    e.Handled = true;
                    HandleDelete(caretPosition, decimalSeparator);
                }
                else if ((int)e.Key == 189 || e.Key == VirtualKey.Subtract)
                {
                    // Handles when the negative key is pressed.
                    e.Handled = true;
                    HandleNegativeKey(isNegative, caretPosition);
                }
                else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.Right || e.Key == VirtualKey.Home ||
                    e.Key == VirtualKey.End || e.Key == VirtualKey.Up || e.Key == VirtualKey.Down ||
                    e.Key == VirtualKey.PageUp || e.Key == VirtualKey.PageDown)
                {
                    // Handles when the arrow key or page up/down key is pressed.
                    e.Handled = HandleNavigation(key, isNegative, caretPosition);
                }
                else if (e.Key == VirtualKey.Enter)
                {
                    // Handles when enter key is pressed.
                    UpdateValue();
                    e.Handled = false;
                }
                else if (e.Key == VirtualKey.Escape)
                {
                    // Handles when escape key is pressed and sets the last value to the textbox.
                    UpdateDisplayText(Value, resetCursor: false);
                    e.Handled = false;
                }
                else if (isCtrlKeyPressed && e.Key == VirtualKey.Z)
                {
                    // Performs undo operation.
                    var oldText = _textBox.Text;
                    UpdateDisplayText(Value);
                    _redoText = (oldText != _textBox.Text) ? oldText : _redoText;
                    e.Handled = true;
                }
                else if (isCtrlKeyPressed && e.Key == VirtualKey.Y)
                {
                    // Performs redo operation.
                    e.Handled = true;
                    if (_redoText != null)
                    {
                        var redoValue = Parse(_redoText);
                        UpdateDisplayText(redoValue);
                        _redoText = null;
                    }
                }
                else if ((isCtrlKeyPressed && (e.Key == VirtualKey.C || e.Key == VirtualKey.X || e.Key == VirtualKey.Insert
                    || e.Key == VirtualKey.V || e.Key == VirtualKey.A)) || (!isCtrlKeyPressed && e.Key == VirtualKey.Tab)
                    || (isShiftKeyPressed && (e.Key == VirtualKey.Delete || e.Key == VirtualKey.Insert)))
                {
                    // Cut, Copy, Paste, and Tab key operations are performed by TextBox control.
                    e.Handled = false;
                }
                else
                {
                    // Do nothing.
                    e.Handled = true;
                }
            }
        }
#endif

#if WINDOWS
        /// <summary>
        /// Occurs when text is pasted in TextBox.
        /// </summary>
        /// <param name="sender">NumberBox control.</param>
        /// <param name="e">TextControlPasteEventArgs.</param>
        async void OnTextBoxPaste(object sender, Microsoft.UI.Xaml.Controls.TextControlPasteEventArgs e)
        {
			// Ensure paste handling is consistent with custom logic
			e.Handled = true;
			if (sender is Microsoft.UI.Xaml.Controls.TextBox textBox)
			{
				var clipboardContent = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

				if (clipboardContent.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Text))
				{
					try
					{
						var copiedText = await clipboardContent.GetTextAsync();
						copiedText = ValidatePastedText(copiedText, GetNumberFormat());

						if (!string.IsNullOrEmpty(copiedText) && CanPaste(copiedText))
						{
							InsertNumbers(
								textBox.Text,
								textBox.SelectedText,
								copiedText,
								textBox.SelectionStart
							);
						}
					}
					catch (Exception ex)
					{
						// Consider logging or handling the error as per your needs
						throw new InvalidOperationException("An error occurred while processing the clipboard text.", ex);
					}
				}
            }
        }

        /// <summary>
        /// Occurs before cut text is moved to the clipboard.
        /// </summary>
        void OnTextBoxCuttingToClipboard(TextBox sender, TextControlCuttingToClipboardEventArgs args)
        {
            // Prevent the default cut operation to implement custom logic
			args.Handled = true;

			if (sender is null)
			{
				return;
			}

			var selectedText = sender.SelectedText;
			var caretPosition = sender.SelectionStart;
			var displayText = sender.Text;

			// Ensure there is text selected before modifying the TextBox
			if (selectedText.Length > 0)
			{
				sender.CopySelectionToClipboard();

				displayText = RemoveSelectedText(displayText, selectedText, caretPosition, GetNumberFormat());
				PrefixZeroAndUpdateCursor(ref displayText, ref caretPosition);

				// Update TextBox properties based on changes made to ensure coherent UI state
				sender.Text = displayText;
				sender.SelectionLength = 0;
				sender.SelectionStart = caretPosition;
			}
        }

        /// <summary>
        /// Occurs when the text selection starts to change.
        /// </summary>
        void OnTextBoxSelectionChanging(TextBox sender, TextBoxSelectionChangingEventArgs args)
        {
            // Should not allow the cursor before the negative sign.
            // Allowing keyboard input before negative sign, gives invalid input.
            // | -> cursor position
            // Eg: |-3.12 -> 1|-3.12 -> invalid input.
            if (_textBox != null && IsNegative(_textBox.Text, GetNumberFormat()) && args.SelectionLength == 0 && args.SelectionStart == 0)
            {
                args.Cancel = true;
                _textBox.CursorPosition = 1;
            }
        }
#endif

		/// <summary>
		/// Checks whether to allow perform paste operation or not.
		/// </summary>
		bool CanPaste(string copiedText)
		{
			bool allowPaste = false;
			if (copiedText != null && _textBox != null)
			{
				string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
				bool copiedTextHasDecimal = copiedText.Contains(decimalSeparator, StringComparison.CurrentCulture);
				bool hasSelectedAll = _textBox.Text.Length == _textBox.SelectionLength;
				bool selectedTextHasDecimal = _textBox.Text.Substring(_textBox.CursorPosition, _textBox.SelectionLength).Contains(decimalSeparator, StringComparison.CurrentCulture);
				bool textHasDecimal = _textBox.Text.Contains(decimalSeparator, StringComparison.CurrentCulture);

				if (hasSelectedAll || (!copiedTextHasDecimal && !selectedTextHasDecimal) ||
					(copiedTextHasDecimal && (selectedTextHasDecimal || !textHasDecimal)))
				{
					allowPaste = true;
				}
			}

			return allowPaste;
		}

		/// <summary>
		/// Setup logic when clearing of text is requested by a user.
		/// </summary>
		void ClearButtonOnClick(object sender, EventArgs e)
		{
			UpdateDisplayText(null);
		}

		/// <summary>
		/// Occurs when control is loaded.
		/// </summary>
		/// <param name="sender">NumberBox control.</param>
		/// <param name="e">RoutedEventArgs.</param>
		void OnLoaded(object? sender, EventArgs e)
		{
			FormatValue();
#if WINDOWS
			GetParentElement();
			if (IsRTL())
			{
				SetFlowDirection();
			}

#elif ANDROID
			CheckDeviceAndKeyboard();
#endif

		}

		/// <summary>
		/// Formats the current value of the numeric entry based on the defined custom format or maximum decimal digits.
		/// </summary>
		void FormatValue(bool isValueChange = false)
		{
			// Only format the value when the control is not focused
			if (_textBox == null || _textBox.IsFocused)
			{
				return;
			}

			if (Value == null || double.IsNaN(Value.Value))
			{
				return;
			}

			string formattedValue;
			var numberFormat = GetNumberFormat();

			if (CustomFormat != null)
			{
				bool containsPercentageSymbol = CustomFormat.Contains('p', StringComparison.OrdinalIgnoreCase);
				if(_maximumPositiveFractionDigit >=0 && !isValueChange && _maximumPositiveFractionDigit < 16)
				{
					Value = (double)Math.Round((double)Value, _maximumPositiveFractionDigit);
				}
				// Apply percentage conversion if PercentDisplayMode demands it.
				double valueToFormat = containsPercentageSymbol && PercentDisplayMode == PercentDisplayMode.Value 
					? Value.Value / 100 
					: Value.Value;

				formattedValue = valueToFormat.ToString(CustomFormat, numberFormat);
			}
			else
			{
				int maximumDecimalDigits = MaximumNumberDecimalDigits < 0 ? 0 : MaximumNumberDecimalDigits;
				if(MaximumNumberDecimalDigits >=0 && !isValueChange && MaximumNumberDecimalDigits < 16)
				{
					Value = (double)Math.Round((double)Value, maximumDecimalDigits);
				}
				formattedValue = Value.Value.ToString("N" + maximumDecimalDigits, numberFormat);
			}

			_textBox.Text = formattedValue;
		}

		/// <summary>
		/// Inserts the number on the current cursor position or over the selected text.
		/// </summary>
		void HandleNumbers(string key, int caretPosition, string decimalSeparator)
		{
			if (_textBox == null)
			{
				return;
			}

			// Extract the last character from the key input which represents the digit.
			char input = key.Last();
			string displayText = GetDisplayTextForPlatform(caretPosition, GetSelectionLength());
			string selectedText = GetSelectedText(caretPosition);

			// Check if the entire text or selected part includes a decimal to control fraction input.
			bool hasSelectedAll = selectedText.Equals(displayText, StringComparison.Ordinal);
			bool selectedTextHasDecimal = selectedText.Contains(decimalSeparator, StringComparison.CurrentCulture);
#if IOS
			// On iOS, the selectedText is empty, but it incorrectly returns true when checking if it contains the decimalSeparator.
			// Added a condition to set selectedTextHasDecimal to false if selectedText is empty.
			if (string.IsNullOrEmpty(selectedText))
			{
				selectedTextHasDecimal = false;
			}
#endif
			// Proceed with number insertion only if the selected text doesn't have a decimal or the entire text is selected.
			if (!selectedTextHasDecimal || hasSelectedAll)
			{
				InsertNumbers(displayText, selectedText, input.ToString(), caretPosition);
			}
		}

		/// <summary>
		/// If the text is not selected, removes the left character of the current cursor position
		/// If text is selected, removes the selected text.
		/// </summary>
		void HandleBackspace(int caretPosition, string decimalSeparator)
		{
			if (_textBox == null)
			{
				return;
			}

			string displayText = _textBox.Text;
			string selectedText = GetSelectedOrAdjacentText(caretPosition);

			bool hasSelectedAll = displayText.Equals(selectedText, StringComparison.Ordinal);
			var isTextParsed = double.TryParse(displayText, out double value);

			if (selectedText.Length > 0)
			{
				value = hasSelectedAll ? 0 : value;
#if MACCATALYST || IOS
				displayText = RemoveSelectedText(displayText, selectedText, caretPosition-1, GetNumberFormat());
#else
				displayText = RemoveSelectedText(displayText, selectedText, caretPosition, GetNumberFormat());
#endif
			}
			else if (caretPosition > 0 && displayText.Length > 0)
			{
				displayText = displayText.Remove(caretPosition - 1, 1);
			}

			PrefixZeroAndUpdateCursor(ref displayText, ref caretPosition, isBackspace: true, value);

			UpdateTextBoxAndCursorPosition(displayText, caretPosition);
		}

		/// <summary>
		/// Retrieves either the selected text or the text to the left of the caret position.
		/// </summary>
		string GetSelectedOrAdjacentText(int caretPosition)
		{
			if (_textBox == null)
			{
				return "0";
			}

		#if MACCATALYST || IOS
			return _textBox.Text.Substring(caretPosition - 1, _textBox.SelectionLength);
		#else
			return _textBox.Text.Substring(_textBox.CursorPosition, _textBox.SelectionLength);
		#endif
		}

		/// <summary>
		/// Updates the text box content and cursor position after processing backspace logic.
		/// </summary>
		void UpdateTextBoxAndCursorPosition(string displayText, int caretPosition)
		{
			if (_textBox == null)
			{
				return;
			}

#if (MACCATALYST || IOS)
			Task.Run(() =>
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					_textBox.Text = displayText;
					_textBox.CursorPosition = Math.Max(0, caretPosition);
				});
			});
#else
			_textBox.Text = displayText;
#endif
			_textBox.CursorPosition = Math.Max(0, caretPosition);
		}

		/// <summary>
		/// Removes the right character of the current cursor position
		/// if the text is not selected, or removes the selected text.
		/// </summary>
		void HandleDelete(int caretPosition, string decimalSeparator)
		{
			if (_textBox == null)
			{
				return;
			}

			string displayText = _textBox.Text;
			string selectedText = GetSelectedText(caretPosition);

			if (selectedText.Length > 0)
			{
				// Remove selected text
				displayText = RemoveSelectedText(displayText, selectedText, caretPosition, GetNumberFormat());
			}
			else if (caretPosition >= 0 && caretPosition < displayText.Length)
			{
				// Remove character to the right of caret, skipping the decimal separator.
				if (displayText[caretPosition].ToString() != decimalSeparator)
				{
					displayText = displayText.Remove(caretPosition, 1);
				}
				else
				{
					caretPosition++; // Skip the decimal separator
				}
			}

			PrefixZeroAndUpdateCursor(ref displayText, ref caretPosition, isBackspace: false);
			UpdateTextBoxAndCursorPosition(displayText, caretPosition);
		}

		/// <summary>
		/// Performs arrow key navigation or updates the value by small/large change.
		/// </summary>
		bool HandleNavigation(KeyboardKey key, bool isNegative, int caretPosition)
		{
			bool isHandled = false;
			if (key == KeyboardKey.Home && isNegative && _textBox != null)
			{
				// Should not allow the cursor before the negative sign.
				_textBox.CursorPosition = 1;
				_textBox.SelectionLength = 0;
				isHandled = true;
			}
			else if (key == KeyboardKey.Up || key == KeyboardKey.Down)
			{
				isHandled = HandleKeyPressed(key);

			}
			else if (key == KeyboardKey.PageUp || key == KeyboardKey.PageDown)
			{
				isHandled = HandleKeyPagePressed(key);
			}
			else if (key == KeyboardKey.Left && isNegative && caretPosition == 1 && _textBox != null)
			{
				// If text is selected then it should be cleared.
				_textBox.SelectionLength = 0;
				isHandled = true;
			}

			return isHandled;
		}

		/// <summary>
		/// Gets the current display text based on the platform.
		/// </summary>
		/// <returns>The current display text or an empty string if not available.</returns>
		string GetDisplayText()
		{
#if WINDOWS
			if (_textBox is null)
			{
				return string.Empty;
			}

            return _textBox.Text;
#else
			return _previousText;
#endif
		}

		/// <summary>
		/// Gets the currently selected text based on the platform and caret position.
		/// </summary>
		/// <param name="caretPosition">The current caret position in the text.</param>
		/// <returns>The selected text or an empty string if no selection.</returns>
		string GetSelectedText(int caretPosition)
		{
#if WINDOWS
			if (_textBox != null && _textBox.Text != null)
			{
				return _textBox.Text.Substring(_textBox.CursorPosition, _textBox.SelectionLength);
			}
			return string.Empty;
#elif ANDROID
			return _previousText != null ? _previousText.Substring(caretPosition, _selectionLength) : string.Empty;
#elif MACCATALYST || IOS
			return _textBox != null && _previousText != null ? _previousText.Substring(caretPosition, _textBox.SelectionLength) : string.Empty;
#else
			return string.Empty;
#endif
		}

		/// <summary>
		/// Processes the display text for the current platform, applying platform-specific logic.
		/// </summary>
		/// <param name="displayText">The text to be displayed, passed by reference.</param>
		void ProcessDisplayTextForPlatform(ref string displayText)
		{
			if (_textBox == null)
			{
				return;
			}

		#if ANDROID
			// Prefix zero if needed, except on Samsung devices
			if (!_isSamsungWithSamsungKeyboard)
			{
				PrefixZeroIfNeeded(ref displayText, IsNegative(displayText, GetNumberFormat()), GetNumberFormat());
			}
		#elif MACCATALYST || IOS
			// On iOS platforms, ensure valid text before assigning
			if (_isNotValidText)
			{
				return;
			}
		#endif

			// Update the text box for supported platforms
			_textBox.Text = displayText;
		}

		/// <summary>
		/// Inserts decimal separator if text not contains the decimal separator or
		/// move the cursor after the decimal separator.
		/// </summary>
		void HandleDecimalSeparator(string decimalSeparator, int caretPosition)
		{
			if (_textBox != null)
			{
				string displayText = GetDisplayText();
				string selectedText = GetSelectedText(caretPosition);

				bool displayTextHasDecimal = displayText.Contains(GetNumberDecimalSeparator(GetNumberFormat()), StringComparison.CurrentCulture);
				bool selectedTextHasDecimal = selectedText.Contains(GetNumberDecimalSeparator(GetNumberFormat()), StringComparison.CurrentCulture);
#if IOS
				// On iOS, the selectedText is empty, but it incorrectly returns true when checking if it contains the decimalSeparator.
				// Added a condition to set selectedTextHasDecimal to false if selectedText is empty.
				if (string.IsNullOrEmpty(selectedText))
				{
					selectedTextHasDecimal = false;
				}
#endif
				if (!displayTextHasDecimal || selectedTextHasDecimal)
				{
					displayText = displayText.Remove(caretPosition, selectedText.Length);
					displayText = displayText.Insert(caretPosition, decimalSeparator);
					ProcessDisplayTextForPlatform(ref displayText);
				}

				ProcessCursorPosition(decimalSeparator, caretPosition, displayText);
			}
		}

		/// <summary>
		/// Processes the cursor position based on the decimal separator, caret position, and display text.
		/// </summary>
		/// <param name="decimalSeparator">The decimal separator character.</param>
		/// <param name="caretPosition">The current caret position.</param>
		/// <param name="displayText">The text being displayed.</param>
		void ProcessCursorPosition(string decimalSeparator, int caretPosition, string displayText)
		{
			if (_textBox == null)
			{
				return;
			}
#if !WINDOWS
			if (!displayText.Equals(decimalSeparator, StringComparison.Ordinal) && !displayText.StartsWith(decimalSeparator))
			{
#if ANDROID
                _textBox.Text = displayText;
#elif MACCATALYST || IOS
				if (!_isNotValidText)
				{
					_textBox.Text = displayText;
				}
#endif
			}
#endif
			if (ShouldUpdateCursorPosition())
			{
				int decimalIndex = _textBox.Text.IndexOf(decimalSeparator, StringComparison.CurrentCulture);
				UpdateCursorPosition(decimalIndex, caretPosition, displayText, decimalSeparator);
			}
		}

		/// <summary>
		/// Determines whether the cursor position should be updated.
		/// </summary>
		/// <returns>True if the cursor position should be updated; otherwise, false.</returns>
		bool ShouldUpdateCursorPosition()
		{
			if (_textBox == null)
			{
				return false;
			}
#if ANDROID
            return _selectionLength >= 0;
#elif WINDOWS
            return _textBox.SelectionLength >= 0;
#else
			return _textBox.SelectionLength == 0;
#endif
		}

		/// <summary>
		/// Updates the cursor position based on the decimal index and platform-specific conditions.
		/// </summary>
		/// <param name="decimalIndex">The index of the decimal separator.</param>
		/// <param name="caretPosition">The current caret position.</param>
		/// <param name="displayText">The text being displayed.</param>
		/// <param name="decimalSeparator">The decimal separator character.</param>
		void UpdateCursorPosition(int decimalIndex, int caretPosition, string displayText, string decimalSeparator)
		{
			if (_textBox == null)
			{
				return;
			}
#if ANDROID
            if (!_isSamsungWithSamsungKeyboard)
            {
                _cursorPosition = decimalIndex + 1;
            }
            else
            {
                if (caretPosition == 0 && displayText != null && displayText.Contains(decimalSeparator, StringComparison.Ordinal))
                {
                    HandleNegativeKey(false, 0);
                }
                else
                {
                    _cursorPosition = decimalIndex + 1;
                }
            }
#else
			_textBox.CursorPosition = decimalIndex + 1;
#endif
		}

		/// <summary>
		/// Gets the length of the current text selection.
		/// </summary>
		/// <returns>The length of the selected text.</returns>
		int GetSelectionLength()
		{
#if ANDROID
            return _selectionLength;
#else
			if (_textBox is null)
			{
				return 0;
			}
			return _textBox.SelectionLength;
#endif
		}

		/// <summary>
		/// Gets the display text for the current platform.
		/// </summary>
		/// <param name="caretPosition">The current caret position.</param>
		/// <param name="selectionLength">The length of the current selection.</param>
		/// <returns>The current text for display.</returns>
		string GetDisplayTextForPlatform(int caretPosition, int selectionLength)
		{
#if WINDOWS
			if (_textBox is null)
			{
				return string.Empty;
			}
            return _textBox.Text;
#else
			return _previousText;
#endif
		}

		/// <summary>
		/// Conditionally appends a negative sign to current input if needed or removes it.
		/// </summary>
		void HandleNegativeKey(bool isNegative, int caretPosition)
		{
			if (_textBox != null)
			{
				int selectionLength = GetSelectionLength();
#if WINDOWS
                string displayText = _textBox.Text;
                if (displayText == _textBox.Text.Substring(_textBox.CursorPosition, _textBox.SelectionLength))
#elif !WINDOWS
				string displayText = _previousText;
#if ANDROID
                _selectionLength = selectionLength;
                if (displayText == _previousText.Substring(caretPosition, selectionLength))
#else
				if (displayText == _previousText.Substring(caretPosition, _textBox.SelectionLength))
#endif

#endif
				{
					// Appends zero when the negative key is pressed and the text is empty or selected all.
					displayText = displayText.Remove(caretPosition, selectionLength);
					displayText = displayText.Insert(0, GetNegativeSign(GetNumberFormat()));
					bool negativeZero = PrefixZeroIfNeeded(ref displayText, isNegative: true, GetNumberFormat());
					_textBox.Text = displayText;
					_textBox.CursorPosition = caretPosition + 2;
					if(negativeZero && IsTextInputLayout)
					{
						_textInputLayout?.InvalidateDrawable();
					}
#if ANDROID
                    _cursorPosition = caretPosition + 2;
#endif
				}
#if ANDROID
                else if (displayText.StartsWith('.') && _isSamsungWithSamsungKeyboard)
                {
                    string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
                    displayText = string.Concat("", displayText.AsSpan(1));
                    _textBox.Text = displayText;
                    _cursorPosition = 1;
                }
#endif
				else
				{
					// Adds a negative sign if it is not available, or removes the negative sign.
					_textBox.Text = isNegative ? displayText.Remove(0, 1) :
						displayText.Insert(0, GetNegativeSign(GetNumberFormat()));
#if !WINDOWS
					_textBox.CursorPosition = caretPosition != 0 ? (caretPosition + (isNegative ? -1 : 1)) : caretPosition;
#if ANDROID
                    _cursorPosition = caretPosition != 0 ? (caretPosition + (isNegative ? -1 : 1)) : caretPosition;
#endif
#elif WINDOWS
                    _textBox.CursorPosition = caretPosition + (isNegative ? -1 : 1);
#endif
					_textBox.SelectionLength = selectionLength;
				}
			}
		}

		/// <summary>
		/// To get the maximum fraction digits from valid custom format.
		/// </summary>
		/// <param name="customFormat">The <see cref="CustomFormat"/>.</param>
		/// <returns>the maximum fraction digits</returns>
		int GetMaximumFractionDigitFromValidFormat(string customFormat)
		{
			int maxFractionDigit = -1;
			string validFormat = string.Empty;

			//To remove escape characters and other symbols from custom format.
			for (int i = 0; i < customFormat.Length; i++)
			{
				char formatCharecter = customFormat[i];
				if (formatCharecter != '\\')
				{
					if (formatCharecter == '0' || formatCharecter == '#' || (formatCharecter == '.' && !validFormat.Contains('.', StringComparison.Ordinal)))
					{
						validFormat += formatCharecter;
					}
				}
				else
				{
					i++;
				}
			}

			//To get the maximum fraction digit from the valid format.
			int dotIndex = validFormat.IndexOf('.', StringComparison.Ordinal);
			bool hasMoreThanOneCharacterAfterDot = dotIndex >= 0 && validFormat.Length - dotIndex > 1;
			if (validFormat.Contains('.', StringComparison.Ordinal) && hasMoreThanOneCharacterAfterDot)
			{
				maxFractionDigit = (dotIndex >= 0) ? (validFormat.Length - dotIndex - 1) : 0;
			}
			else if (!string.IsNullOrEmpty(validFormat))
			{
				maxFractionDigit = 0;
			}

			return maxFractionDigit;
		}

		/// <summary>
		/// Determines if the given character represents a standard numeric format specifier.
		/// </summary>
		bool IsStandardFormat(char formatChar)
		{
			return formatChar == 'C' || formatChar == 'E' || formatChar == 'F' ||
				   formatChar == 'N' || formatChar == 'P' || formatChar == 'G' || formatChar == 'R';
		}

		/// <summary>
		/// Returns the default number of fraction digits for a given numeric format specifier.
		/// </summary>
		int GetDefaultFractionDigitsForFormat(char formatChar)
		{
			if (formatChar == 'G' || formatChar == 'R')
			{
				return 15; // Default maximum fraction digits for 'G' and 'R'
			}

			return (formatChar != 'E') ? 2 : -1; // Default for others except 'E'
		}

		/// <summary>
		/// Gets the maximum fraction digits from the custom format.
		/// </summary>
		/// <param name="customFormat">The custom number format.</param>
		/// <returns>The maximum fraction digits allowed by the format.</returns>
		int GetMaximumFractionDigits(string customFormat)
		{
			int maxFractionDigit = -1;
			char customFormatFirstChar = char.ToUpper(Convert.ToChar(customFormat[0]));

			//To get the maximum fraction digit from standard custom formats.
			if (IsStandardFormat(customFormatFirstChar))
			{
				if (customFormat.Length == 1)
				{
					GetDefaultFractionDigitsForFormat(customFormatFirstChar);
				}
				else
				{
					string maxFractionFormat = customFormat.Remove(0, 1);

					//To get the assigned maximum fraction digit for the standard format.(ex:'C10' --> Maximum fraction digits is 10.)
					if (maxFractionFormat.Length <= 2)
					{
						if (customFormatFirstChar != 'E')
						{
							string maxDigit = string.Empty;
							foreach (char formatDigit in maxFractionFormat)
							{
								if (formatDigit >= '0' && formatDigit <= '9')
								{
									maxDigit += formatDigit.ToString();
								}
								else
								{
									//To get the maximum fraction digit for the normal custom formats.(ex:'C#.0' --> Maximum fraction digits is 1.)
									maxFractionDigit = GetMaximumFractionDigitFromValidFormat(maxFractionFormat);
									maxDigit = string.Empty;
									break;
								}
							}

							if (!string.IsNullOrEmpty(maxDigit))
							{
								maxFractionDigit = int.Parse(maxDigit) > 99 ? -1 : int.Parse(maxDigit);
							}
						}
					}
					else
					{
						//To get the maximum fraction digit from normal custom formats.(ex:'C#.00##' --> Maximum fraction digits is 4.)
						maxFractionDigit = GetMaximumFractionDigitFromValidFormat(maxFractionFormat);
					}
				}
			}
			else
			{
				//To get the maximum fraction digit from common custom formats.
				maxFractionDigit = GetMaximumFractionDigitFromValidFormat(customFormat);
			}

			return maxFractionDigit;
		}

		/// <summary>
		/// Updates maximum fraction digit properties based on custom formatting or defaults.
		/// </summary>
		void UpdateMaximumFractionDigit()
		{
			if (CustomFormat == null)
			{
				_maximumPositiveFractionDigit = MaximumNumberDecimalDigits < 0 ? 0 : MaximumNumberDecimalDigits;
				_maximumNegativeFractionDigit = MaximumNumberDecimalDigits < 0 ? 0 : MaximumNumberDecimalDigits;
			}
			else if (!string.IsNullOrEmpty(CustomFormat))
			{
				//To get the positive and negative custom formats.
				string[] customFormats = CustomFormat.Split(';');

				if (customFormats.Length == 1)
				{
					_maximumPositiveFractionDigit = GetMaximumFractionDigits(customFormats[0]);
					_maximumNegativeFractionDigit = GetMaximumFractionDigits(customFormats[0]);
				}
				else if (customFormats.Length > 1)
				{
					_maximumPositiveFractionDigit = GetMaximumFractionDigits(customFormats[0]);
					_maximumNegativeFractionDigit = GetMaximumFractionDigits(customFormats[1]);
				}
			}
		}

		/// <summary> 
		/// On iOS and MacCatalyst, it ensures a minimum left or right margin value when the 
		/// text alignment is Start or End respectively.
		/// On other platforms, it returns the provided thickness without modification.
		/// <param name="left">The original left margin.</param>
		/// <param name="top">The original top margin.</param>
		/// <param name="right">The original right margin.</param>
		/// <param name="bottom">The original bottom margin.</param>
		/// <returns>Returns a <see cref="Thickness"/> value adjusted based on the specified text alignment.</returns>
		/// </summary>
		internal Thickness GetMarginBasedOnTextAlignment(double left, double top, double right, double bottom)
		{
#if MACCATALYST || IOS
			return HorizontalTextAlignment switch
			{
				TextAlignment.Start => new Thickness(left > 0 ? left : MinimumMargin, top, right, bottom),
				TextAlignment.End => new Thickness(left, top, right > 0 ? right : MinimumMargin, bottom),
				_ => new Thickness(left, top, right, bottom)
			};
#else
			return new Thickness(left, top, right, bottom);
#endif
		}

		/// <summary>
		/// Restricts digits in the fractional part of the number in the <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <param name="displayText">The current text displayed.</param>
		/// <returns>The result after applying fraction digit restrictions.</returns>
		internal string RestrictFractionDigit(string displayText)
		{
			string numberDecimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());
			if (!string.IsNullOrEmpty(displayText))
			{
				int fractionDigitCount = 0;
				int maxDigitCount;

				//To get the fraction digits in the display text.
				if (displayText.Contains(numberDecimalSeparator, StringComparison.Ordinal))
				{
					int separatorIndex = displayText.IndexOf(numberDecimalSeparator);
					fractionDigitCount = (separatorIndex >= 0) ? (displayText.Length - separatorIndex - 1) : 0;
				}

				//To get the maximum fraction digits from NumberFormatter and CustomFormat.
				if (IsNegative(displayText, GetNumberFormat()) && CustomFormat != null)
				{
					//Assigning the negative format's maximum fraction digit.
					maxDigitCount = _maximumNegativeFractionDigit;
				}
				else
				{
					//Assigning the positive format's maximum fraction digit.
					maxDigitCount = _maximumPositiveFractionDigit;
				}

				//To remove the extra fraction digit from the display text.
				if (maxDigitCount > -1 && fractionDigitCount > maxDigitCount)
				{
					int decimalSeparatorIndex = displayText.IndexOf(numberDecimalSeparator);
					if (maxDigitCount == 0)
					{
						decimalSeparatorIndex += maxDigitCount;
					}
					else
					{
						decimalSeparatorIndex += maxDigitCount + 1;
					}
					displayText = displayText.Remove(decimalSeparatorIndex);
				}
			}
			return displayText;
		}

		/// <summary>
		/// Updates the text box with new display text and caret position.
		/// </summary>
		/// <param name="displayText">The new display text for the text box.</param>
		/// <param name="caretPosition">The new caret position after the update.</param>
		void UpdateTextBox(string displayText, int caretPosition)
		{
			if (_textBox != null)
			{
#if !(MACCATALYST || IOS)
				UpdateTextBoxCursorPosition(displayText,caretPosition);
#else
				// If the cursor position is in middle then the position not updated properly
				if(_textBox.Text.Length != _textBox.CursorPosition)
				{
				Task.Run(() =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						UpdateTextBoxCursorPosition(displayText,caretPosition);
					});
				});
				}
				else
				{
					UpdateTextBoxCursorPosition(displayText,caretPosition);
				}

				// Added to resolve cursor positioning issue after reaching Maximum.
				bool isValid = double.TryParse(_textBox.Text, out double result);
				if (Maximum != result)
				{
					_textBox.CursorPosition = caretPosition >= 0 ? (caretPosition <= displayText.Length) ? caretPosition : displayText.Length : 0;
				}
#endif

#if ANDROID
                _cursorPosition = caretPosition >= 0 ? caretPosition : 0;
#endif
				_textBox.SelectionLength = 0;
			}
		}

		/// <summary>
		/// Update the cursor position <see cref="SfNumericEntry"/>.
		/// </summary>
		private void UpdateTextBoxCursorPosition(string displayText, int caretPosition)
		{
			if (_textBox == null) 
			{
				return;
			}

			_textBox.Text = displayText;
			if (ValueChangeMode == ValueChangeMode.OnKeyFocus && _textBox.Text != null && _textBox.Text != displayText)
			{
				if (Parse(_textBox.Text) == Maximum)
				{
					caretPosition = _textBox.Text.Length;
				}
			}
			if (_textBox.Text != null)
			{
				_textBox.CursorPosition = caretPosition >= 0 ? (caretPosition <= _textBox.Text.Length) ? caretPosition : _textBox.Text.Length : 0;
			}
			else
			{
				_textBox.CursorPosition = 0;
			}
		}

		/// <summary>
		/// Inserts numbers at the specified position in the <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <param name="displayText">Current display text.</param>
		/// <param name="selectedText">Text that is currently selected.</param>
		/// <param name="input">The input to insert.</param>
		/// <param name="caretPosition">The position of the caret where input should be inserted.</param>
		void InsertNumbers(string displayText, string selectedText, string input, int caretPosition)
		{
#if WINDOWS
            displayText ??= "";
#endif
			displayText = displayText.Remove(caretPosition, Math.Min(selectedText.Length, displayText.Length - caretPosition));
			string negativeSign = GetNegativeSign(GetNumberFormat());
			bool isNegative = IsNegative(displayText, GetNumberFormat());

			// Removes the negative sign from input to avoid conflict if display text has a negative sign.
			input = isNegative ? input.Replace(negativeSign, string.Empty, StringComparison.CurrentCulture) : input;
			displayText = displayText.Insert(caretPosition, input);
			displayText = RestrictFractionDigit(displayText);
			isNegative = isNegative || IsNegative(input, GetNumberFormat());
			int textLength = displayText.Length;
			AdjustDisplayText(ref displayText, negativeSign, isNegative);

			// Updates the cursor index to next to the given input by considering the duplicate digits from input text.
			// For example, The display text is 1.45, the cursor index is 0, the selected text is 1,
			// and the input text is 0000. Now, the result is 0.45, and the cursor index is 1.
			caretPosition += input.Length - (textLength - displayText.Length);

			UpdateTextBox(displayText, caretPosition);
		}

		/// <summary>
		/// Adjusts the display text by removing leading zeroes and appending necessary prefixes.
		/// </summary>
		/// <param name="displayText">The display text to adjust.</param>
		/// <param name="negativeSign">The negative sign string based on the current culture's number format.</param>
		/// <param name="isNegative">Indicates if the number is negative, determining if a negative sign should be prefixed.</param>
		void AdjustDisplayText(ref string displayText, string? negativeSign, bool isNegative)
		{
			if (negativeSign == null)
			{
				return;
			}
			string prefix = isNegative ? negativeSign : string.Empty;
			displayText = prefix + displayText.Replace(negativeSign, string.Empty, StringComparison.CurrentCulture).TrimStart('0');
			PrefixZeroIfNeeded(ref displayText, isNegative, GetNumberFormat());
		}

		/// <summary>
		/// Ensures the display text is valid and adjusts the caret position accordingly.
		/// </summary>
		/// <param name="displayText">The current display text to be validated and adjusted.</param>
		/// <param name="caretPosition">The current position of the caret within the text.</param>
		/// <param name="isBackspace">Indicates if the operation is triggered by a backspace action.</param>
		/// <param name="value">The current numeric value, used in certain validation contexts.</param>
		void PrefixZeroAndUpdateCursor(ref string displayText, ref int caretPosition, bool isBackspace = false, double value = 0)
		{
			string negativeSign = GetNegativeSign(GetNumberFormat());
			string decimalSeparator = GetNumberDecimalSeparator(GetNumberFormat());

			if (AllowNull && string.IsNullOrEmpty(displayText.TrimStart(negativeSign[0]).TrimEnd(decimalSeparator[0])))
			{
				// Sets display text as empty when it has no value.
				displayText = string.Empty;
				caretPosition = 0;
			}
			else
			{
				bool isNegative = IsNegative(displayText, GetNumberFormat());
				bool isZeroNeeded = PrefixZeroIfNeeded(ref displayText, isNegative, GetNumberFormat());

				if (isBackspace && _textBox != null)
				{
					// The negative sign is removed only after the value is 0.
					// For example, -1 will be changed into -0 followed by 0.
					displayText = (isNegative && value == 0 && caretPosition == 2) ?
						displayText.TrimStart(negativeSign[0]) : displayText;
					isNegative = IsNegative(displayText, GetNumberFormat());
					caretPosition = string.IsNullOrEmpty(_textBox.Text.Substring(_textBox.CursorPosition, _textBox.SelectionLength)) ?
						(caretPosition - 1) : caretPosition;
				}

				if (isZeroNeeded)
				{
					// It sets the cursor after 0 if the cursor position is before 0. We should not allow cursor before 0.
					caretPosition = isNegative ? ((caretPosition > 1) ? caretPosition : 2) :
						((caretPosition > 0) ? caretPosition : 1);
				}
			}
		}

		/// <summary>
		/// Converts the string into a nullable double.
		/// </summary>
		double? Parse(string input)
		{
			double? value = null;
			if (string.IsNullOrEmpty(input))
			{
				value = AllowNull ? value : 0.0;
			}
			else
			{
				if (Culture != null && Culture.TextInfo.IsRightToLeft)
				{
					if (double.TryParse(input, NumberStyles.Number, GetNumberFormat(), out double parsedValue))
					{
						value = parsedValue;
					}
				}
				else
				{
					if (double.TryParse(input, GetNumberFormat(), out double parsedValue))
					{
						value = parsedValue;
					}
				}
			}

			return value;
		}

		/// <summary>
		/// Checks if number input is within allowed min and max range and adjusts accordingly.
		/// </summary>
		double? ValidateMinMax(double? value)
		{
			value ??= AllowNull ? value : 0;
			if (value != null)
			{
				return Math.Clamp((double)value, Minimum, Maximum);
			}

			return value;
		}

		/// <summary>
		/// Updates display text on the control, resetting to default when necessary.
		/// </summary>
		internal void UpdateDisplayText(double? value, bool resetCursor = true)
		{
			if (_textBox != null)
			{
				if (value == null || double.IsNaN((double)value))
				{
					double? defaultValue = ValidateMinMax(0.0);
					_textBox.Text = AllowNull ? string.Empty :
						defaultValue?.ToString(GetNumberFormat());
				}
				else
				{
					// Added to resolve the decimal seperator removal issue in OnKeyFocus.
					if (_textBox.Text != null)
					{
						if (ValueChangeMode == ValueChangeMode.OnKeyFocus)
						{
							_ = double.TryParse(_textBox.Text, out double result);
							if (!_textBox.Text.EndsWith(GetNumberDecimalSeparator(GetNumberFormat())) && result != value)
							{
								_textBox.Text = value?.ToString(GetNumberFormat());
							}
						}
						else
						{
							_textBox.Text = value?.ToString(GetNumberFormat());
						}
					}
					else
					{
						_textBox.Text = value?.ToString(GetNumberFormat());
					}
				}

#if WINDOWS
				if ((resetCursor || _isFirst) && _textBox.Text != null)
#else
				if (resetCursor && _textBox.Text != null)
#endif
				{
					_textBox.CursorPosition = _textBox.Text.Length;
				}
			}
		}

		/// <summary>
		/// Updates the placeholder text by the given value.
		/// </summary>
		void UpdatePlaceHolderText(string value)
		{
			if (_textBox != null)
			{
				_textBox.Placeholder = value;
			}
		}

		/// <summary>
		/// Updates the text box editability by the given value.
		/// </summary>
		void UpdateTextBoxEditability(bool value)
		{
			if (_textBox == null)
			{
				return;
			}
			_textBox.IsReadOnly = !value;
#if ANDROID
            if (!_textBox.IsReadOnly)
            {
                if (_textBox.Handler != null && _textBox.Handler?.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
                {
                    androidEntry.KeyListener = global::Android.Text.Method.DigitsKeyListener.GetInstance(KEYS);
                }
            }
#endif
		}

		/// <summary>
		/// Helps to wire the events.
		/// </summary>
		void HookEvents()
		{
			UnHookEvents();
			Loaded += OnLoaded;
			if (_textBox != null)
			{
				HookTextBoxEvents();
				SetTextBoxProperties();
				UpdateClearButtonVisibility();
				UpdateEntryVisibility();
				UpdateMaximumFractionDigit();
			}

			UpdateVisualState();

		}

		/// <summary>
		/// Initializes the internal TextBox control with essential properties for the NumericEntry.
		/// </summary>
		void SetTextBoxProperties()
		{
			if (_textBox is null)
			{
				return;
			}
			_textBox.Placeholder = Placeholder;
			_textBox.IsReadOnly = !IsEditable;
			_textBox.Drawable = this;
#if ANDROID
            _textBox.Keyboard = Keyboard.Numeric;
#endif
		}

		/// <summary>
		/// Attaches event handlers for the required events on the text box.
		/// </summary>
		void HookTextBoxEvents()
		{
			if (_textBox is null)
			{
				return;
			}
#if !ANDROID
			_textBox.TextChanged += OnTextBoxTextChanged;
#endif
			_textBox.Completed += TextBox_Completed;
			_textBox.HandlerChanged += TextBox_HandlerChanged;
			_textBox.Focused += TextBoxOnGotFocus;
			_textBox.Unfocused += TextBoxOnLostFocus;
		}

		/// <summary>
		/// Invokes the Completed event when the text input is finished.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="eventArgs">The event arguments.</param>
		void TextBox_Completed(object? sender, EventArgs eventArgs)
		{
			Completed?.Invoke(this, eventArgs);
		}

#if IOS
		/// <summary>
		/// Removes a custom keyboard accessory view for iOS devices, specifically for iPhone.
		/// </summary>
		void RemoveCustomKeyboard()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
			{
				RemoveToolBarItems();
				if (_deviceRotatedObserver != null)
				{
					NSNotificationCenter.DefaultCenter.RemoveObserver(_deviceRotatedObserver);
				}
				_deviceRotatedObserver = null;
			}
		}

		/// <summary>
		/// Adds a custom keyboard accessory view for iOS devices, specifically for iPhone.
		/// </summary>
		void AddCustomKeyboard()
		{
			if (_textBox is View view && view.Handler != null && view.Handler.PlatformView is UIKit.UITextField iOSEntry)
			{
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				{
					AddToolBarItems();
					_deviceRotatedObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("UIDeviceOrientationDidChangeNotification"), DeviceRotated);
					iOSEntry.InputAccessoryView ??= _toolbarView;
				}
			}
		}
#endif

		/// <summary>
		/// Configures platform-specific behavior for Windows Entry controls.
		/// This method is called when the Entry's handler changes.
		/// </summary>
		/// <param name="sender">The Entry control whose handler has changed, typically a SfEntryView instance.</param>
		void WindowEntryHandler(object? sender)
		{
			if (sender is SfEntryView textBox)
			{
#if WINDOWS
                if (textBox.Handler is not null && textBox.Handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox windowTextBox)
                {
                    windowTextBox.PreviewKeyDown += OnTextBoxPreviewKeyDown;
                    windowTextBox.Paste += OnTextBoxPaste;
                    windowTextBox.CuttingToClipboard += OnTextBoxCuttingToClipboard;
                    windowTextBox.SelectionChanging += OnTextBoxSelectionChanging;
                    windowTextBox.PointerEntered += OnPointerEntered;
                    windowTextBox.PointerExited += OnPointerExited;
                }
#endif
			}
		}

		/// <summary>
		/// Configures platform-specific behavior for Android Entry controls.
		/// This method is called when the Entry's handler changes.
		/// </summary>
		/// <param name="sender">The Entry control whose handler has changed, typically a SfEntryView instance.</param>
		void AndroidEntryHandler(object? sender)
		{
#if ANDROID
            if ((sender is SfEntryView textBox) && textBox.Handler != null && textBox.Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
            {
                androidEntry.EditorAction += AndroidEntry_EditorAction;
                androidEntry.TextChanged += OnTextBoxTextChanged;
                androidEntry.BeforeTextChanged += AndroidEntry_BeforeTextChanged;
                androidEntry.AfterTextChanged += AndroidEntry_AfterTextChanged;
                androidEntry.KeyListener = global::Android.Text.Method.DigitsKeyListener.GetInstance(KEYS);
                androidEntry.Click += AndroidEntry_Click;
                // Disable EmojiCompat to prevent the IllegalArgumentException crash on Android when start typing text.
                androidEntry.EmojiCompatEnabled = false;
            }
#endif
		}

		/// <summary>
		/// Configures platform-specific behavior for iOS and MacCatalyst Entry controls.
		/// This method is called when the Entry's handler changes.
		/// </summary>
		/// <param name="sender">The Entry control whose handler has changed, typically a SfEntryView instance.</param>
		void IOSEntryHandler(object? sender)
		{

#if MACCATALYST || IOS
			if ((sender is not SfEntryView textBox) || textBox == null)
			{
				return;
			}

			if (textBox.Handler != null && textBox.Handler.PlatformView is UIKit.UITextField macEntry)
			{
				_uiEntry = macEntry;
				if (OperatingSystem.IsIOSVersionAtLeast(26,0) || OperatingSystem.IsMacCatalystVersionAtLeast(26, 0))
				{
#if NET10_0_OR_GREATER
					macEntry.ShouldChangeCharactersInRanges += Handle_ShouldChangeCharactersInRanges;
#else
					macEntry.ShouldChangeCharacters += ValidateTextChanged;
#endif
				}
				else
				{
					macEntry.ShouldChangeCharacters += ValidateTextChanged;
				}
				macEntry.BorderStyle = UITextBorderStyle.None;
				macEntry.KeyboardType = UIKeyboardType.DecimalPad;
			}
#if IOS
			if (textBox.Handler != null)
			{
				AddCustomKeyboard();
			}
			else
			{
				RemoveCustomKeyboard();
			}
#endif
			if (textBox.Handler == null && _uiEntry != null)
			{
				if (OperatingSystem.IsIOSVersionAtLeast(26, 0) || OperatingSystem.IsMacCatalystVersionAtLeast(26, 0))
				{
#if NET10_0_OR_GREATER
					_uiEntry.ShouldChangeCharactersInRanges -= Handle_ShouldChangeCharactersInRanges;
#else
					_uiEntry.ShouldChangeCharacters -= ValidateTextChanged;
#endif
				}
				else
				{
					_uiEntry.ShouldChangeCharacters -= ValidateTextChanged;
				}

				_uiEntry = null;
			}
#endif
		}

		/// <summary>
		/// Handles changes to the TextBox control in a .NET MAUI application.
		/// This method is triggered whenever the TextBox's state changes,
		/// allowing for custom behavior in response to those changes.
		/// </summary>
		/// <param name="sender">The source of the event, typically the TextBox.</param>
		/// <param name="e">Event arguments containing data related to the change event.</param>
		protected virtual void TextBox_HandlerChanged(object? sender, EventArgs e)
		{
			WindowEntryHandler(sender);

			if (_textBox != null)
			{
				BackgroundColor ??= Colors.White;
				if(_textBox.Handler is not null)
				{
					SetBinding();
				}
			}

			AndroidEntryHandler(sender);
			IOSEntryHandler(sender);
		}

#if ANDROID

		/// <summary>
		/// Handles the completion of an editor action on an Android Entry.
		/// This includes transitioning focus when "Next" or "Done" actions are invoked from the keyboard.
		/// </summary>
		/// <param name="sender">The object that triggered the event, typically an Android Entry component.</param>
		/// <param name="e">The event arguments containing information about the editor action.</param>
		void AndroidEntry_EditorAction(object? sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == Android.Views.InputMethods.ImeAction.Next || e.ActionId == Android.Views.InputMethods.ImeAction.Done)
            {
                ////Null
                _textBox?.Unfocus();
            }
        }

		/// <summary>
		/// Handles the click event for an Android Entry control.
		/// Adjusts the cursor position when the text contains a negative sign to prevent accidental deletion.
		/// </summary>
		/// <param name="sender">The object that triggered the event, typically an Android Entry component.</param>
		/// <param name="e">The event arguments for the click event.</param>
		void AndroidEntry_Click(object? sender, EventArgs e)
        {
            if (sender is AndroidX.AppCompat.Widget.AppCompatEditText entry && entry != null)
            {
                if (entry.Text != null)
                {
                    if (entry.Text.Contains('-', StringComparison.Ordinal) && _textBox != null && _textBox.CursorPosition < 1)
                    {
                        _textBox.CursorPosition = 1;
                    }
                }
            }
        }
#endif

		/// <summary>
		/// Removes any existing event handlers from the text box.
		/// </summary>
		void UnHookEvents()
		{
			if (_textBox != null)
			{
#if !ANDROID
				_textBox.TextChanged -= OnTextBoxTextChanged;
#endif
				_textBox.Completed -= TextBox_Completed;
				_textBox.HandlerChanged -= TextBox_HandlerChanged;
				_textBox.Focused -= TextBoxOnGotFocus;
				_textBox.Unfocused -= TextBoxOnLostFocus;

			}
		}
		#endregion

		#region Internal Methods

		/// <summary>
		/// Update the TextInputLayout properties
		/// </summary>
		internal virtual void UpdateTextInputLayoutUI()
		{
			if (_textInputLayout != null)
			{
				_textInputLayout.ShowClearButton = ShowClearButton;
				_textInputLayout.ClearButtonPath = ClearButtonPath;
				_textInputLayout.ClearButtonColor = ClearButtonColor;
			}
		}

		/// <summary>
		/// Checks if a highlight should be cleared based on the visibility and bounds
		/// of the effects renderer.
		/// </summary>
		internal void ClearHighlightIfNecessary()
		{
			if (!_isClearButtonVisible && _effectsRenderer != null &&
				_effectsRenderer.HighlightBounds.Width > 0 && _effectsRenderer.HighlightBounds.Height > 0)
			{
				_effectsRenderer.RemoveHighlight();
			}
		}

		/// <summary>
		/// Gets the minimum size required for the control's components.
		/// </summary>
		internal virtual void GetMinimumSize()
		{
			if (!IsTextInputLayout)
			{
#if !ANDROID
				MinimumHeightRequest = ButtonSize;
#else
                MinimumHeightRequest = ButtonSize;
#endif
				MinimumWidthRequest = 2 * ButtonSize;

			}
		}

		/// <summary>
		/// Set RTL flow direction method.
		/// </summary>
		internal bool IsRTL()
		{
			return ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
		}

		/// <summary>
		/// Check the touch point in the clear button rect.
		/// </summary>
		internal bool ClearButtonClicked(Point touchPoint) =>
			_clearButtonRectF.Contains(touchPoint) && _isClearButtonVisible;

		/// <summary>
		/// Updates the bounds of the UI elements based on the specified rectangle.
		/// </summary>
		/// <param name="bounds">The rectangle defining the new bounds for the elements.</param>
		/// <param name="isOverride">A boolean indicating whether to override existing bounds. 
		/// Defaults to false, meaning existing bounds will be preserved unless specified.</param>
		internal virtual void UpdateElementsBounds(RectF bounds, bool isOverride = false)
		{
			_previousRectBounds = bounds;
			if (!isOverride)
			{
				_buttonSpace = 0;
				if (!IsTextInputLayout)
				{
					ConfigureClearButton(bounds);
				}
				else
				{
					if (_isClearButtonVisible)
					{
						_buttonSpace = ButtonSize;
					}
				}

				SetTextBoxMargin();
			}
			_numericEntrySemanticsNodes.Clear();
			InvalidateSemantics();
			UpdateEffectsRendererBounds();
			InvalidateDrawable();
		}

		/// <summary>
		/// Updates the bounds of effects (such as shadows or highlights) according to the specified rectangle area.
		/// </summary>
		/// <param name="bounds">The bounding rectangle defining the area within which effects should be applied.</param>
		internal void UpdateEffectsBounds(RectF bounds)
		{
			if ((_effectsRenderer is null))
			{
				return;
			}
			_effectsRenderer.RippleBoundsCollection.Add(bounds);
			_effectsRenderer.HighlightBoundsCollection.Add(bounds);
		}

		/// <summary>
		/// Updates the bounds of the effects renderer, ensuring it matches the current layout configuration of the control.
		/// </summary>
		internal virtual void UpdateEffectsRendererBounds()
		{
			if (_effectsRenderer != null)
			{
				ClearEffectsBoundsCollection();

				if (_isClearButtonVisible)
				{
					UpdateEffectsBounds(_clearButtonRectF);
					_effectsRenderer.RippleBoundsCollection.Add(_clearButtonRectF);
				}
			}
		}

		/// <summary>
		/// Draws the entry user interface elements within the specified rectangle on the given canvas.
		/// </summary>
		/// <param name="canvas">The canvas where the entry UI elements are drawn.</param>
		/// <param name="dirtyRect">The rectangle defining the area to be updated and redrawn.</param>
		internal virtual void DrawEntryUI(ICanvas canvas, Rect dirtyRect)
		{
			if (_textBox is null || IsTextInputLayout)
			{
				return;
			}

			InitializeCanvas(canvas);
			if (_entryRenderer != null)
			{
				DrawClearButton(canvas);
				DrawBorder(canvas, dirtyRect);
			}

			DrawEffects(canvas);
		}

		/// <summary>
		/// Adds a semantics node for a given element in the control.
		/// </summary>
		/// <param name="bounds">The rectangular bounds of the element.</param>
		/// <param name="id">The ID of the semantics node.</param>
		/// <param name="description">The text description for the node, used for accessibility.</param>
		void AddSemanticsNode(RectF bounds, int id, string description)
		{
			SemanticsNode node = new SemanticsNode
			{
				Id = id,
				Bounds = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height),
				Text = $"{description} double tap to activate"
			};
			_numericEntrySemanticsNodes.Add(node);
		}

		/// <summary>
		/// Update the visual states.
		/// </summary>
		internal virtual void UpdateVisualState()
		{
			if (IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal");
			}
			else
			{
				VisualStateManager.GoToState(this, "Disabled");
			}
		}

		/// <summary>
		/// Updates the value property by display text value.
		/// </summary>
		internal void UpdateValue()
		{
			if (_textBox != null)
			{
#if (MACCATALYST || IOS)
				string accentKeys = "`^ˆ˙´˳¨ʼ¯ˍ˝˚ˀ¸ˇ˘˜˛‸";
				GetValidText(accentKeys);
				if (_isNotValidText)
				{
					return;
				}
#endif
				double? value = Parse(_textBox.Text);
				value = ValidateMinMax(value);
				SetValue(ValueProperty, value);
				UpdateDisplayText(Value);
			}
		}

		#endregion

		#region Interface Methods

		/// <summary>
		/// Handles touch events for the control, interpreting the touch interactions
		/// to trigger specific behaviors or actions such as presses or focus changes.
		/// </summary>
		/// <param name="e">The event arguments containing data about the touch interaction.</param>
		public virtual void OnTouch(PointerEventArgs e)
		{
			if (IsTextInputLayout)
			{ return; }
			UpdateTouchPoint(e.TouchPoint);

			switch (e.Action)
			{
				case PointerActions.Pressed:
					HandlePressed(e);
					break;
				case PointerActions.Released:
				case PointerActions.Cancelled:
				case PointerActions.Exited:
					HandleReleasedOrCancelled(e);
					break;
			}
		}

		/// <summary>
		/// Method that handles the logic when the clear button is tapped.
		/// </summary>
		/// <param name="e">The event arguments for the clear button touch up interaction.</param>
		void OnClearButtonTouchUp(PointerEventArgs e)
		{
			UpdateDisplayText(null);
			if (IsDescriptionNotSetByUser)
			{
				SemanticProperties.SetDescription(this, "Clear button pressed");
			}
			SemanticScreenReader.Announce(SemanticProperties.GetDescription(this));
			_numericEntrySemanticsNodes.Clear();
			InvalidateSemantics();
		}

		/// <summary>
		/// Invoked during a key down event, allowing the control to handle key down actions.
		/// </summary>
		/// <param name="args">The <see cref="KeyEventArgs"/> containing details of the key event.</param>
		public void OnKeyDown(KeyEventArgs args)
		{
#if MACCATALYST
            OnTextBoxPreviewKeyDown(args);
#endif
		}

		/// <summary>
		/// Invoked during a key up event, providing an opportunity to handle key release actions.
		/// </summary>
		/// <param name="args">The <see cref="KeyEventArgs"/> containing details of the key event.</param>
		public void OnKeyUp(KeyEventArgs args)
		{

		}

		/// <summary>
		/// Invoked when the clear icon is pressed within the text input layout, clearing the display text.
		/// </summary>
		void ITextInputLayout.ClearIconPressed()
		{
			UpdateDisplayText(null);
		}

		/// <summary>
		/// Invoked when the down button is pressed within the text input layout, decreasing the value if possible.
		/// </summary>
		void ITextInputLayout.DownButtonPressed()
		{
			DownButtonPressed();
		}
		internal virtual void DownButtonPressed()
		{

		}

		void ITextInputLayout.UpButtonPressed()
		{
			UpButtonPressed();
		}

		internal virtual void UpButtonPressed()
		{

		}

		/// <summary>
		/// Determines if this keyboard listener can become the first responder, indicating it is able to receive keyboard input.
		/// </summary>
		bool IKeyboardListener.CanBecomeFirstResponder
		{
			get { return true; }
		}

		/// <summary>
		/// Event handler invoked on tap events, typically used to bring focus to the control.
		/// </summary>
		/// <param name="e">The <see cref="TapEventArgs"/> containing details of the tap event.</param>
		public void OnTap(TapEventArgs e)
		{
			Focus();
		}

		///<inheritdoc/>   
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfNumericEntryStyles();
		}

		///<inheritdoc/>     
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		///<inheritdoc/>   
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		#endregion

	}
}
