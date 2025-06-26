using Syncfusion.Maui.Toolkit.EntryRenderer;
using Syncfusion.Maui.Toolkit.EntryView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.NumericUpDown;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.Themes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
	public partial class SfTextInputLayout
	{

		#region Public Methods

		/// <summary>
		/// Sets focus to the input control within the <see cref="SfTextInputLayout"/>.
		/// </summary>
		/// <example>
		///  SfTextInputLayout textInputLayout = new SfTextInputLayout();
		///  textInputLayout.Focus();
		/// </example>
		/// <exclude/>
		public new void Focus()
		{
			if (Content is InputView inputView)
			{
				inputView.Focus();
			}
			else if(Content is SfNumericEntry numericEntry)
			{
				numericEntry.Focus();
			}
			else
			{
				Content?.Focus();
			}
		}

		/// <summary>
		/// Removes focus from the input control within the <see cref="SfTextInputLayout"/>.
		/// </summary>
		/// <example>
		///  SfTextInputLayout textInputLayout = new SfTextInputLayout();
		///  textInputLayout.Unfocus();
		/// </example>
		/// <exclude/>
		public new void Unfocus()
		{
			if (Content is InputView inputView)
			{
				inputView.Unfocus();
			}
		}
		#endregion

		#region Internal Methods
		/// <summary>
		/// Invokes <see cref="PasswordVisibilityToggled"/> event.
		/// </summary>
		/// <param name="args">String.Empty event args.</param>
		internal void InvokePasswordVisibilityToggledEvent(PasswordVisibilityToggledEventArgs args)
		{
			args.IsPasswordVisible = _isPasswordTextVisible;
			PasswordVisibilityToggled?.Invoke(this, args);
		}

		/// <summary>
		/// To get desired left padding.
		/// </summary>
		/// <returns>double value</returns>
		internal double GetLeftPadding()
		{
			return InputViewPadding.Left < 0 ? LeftPadding : InputViewPadding.Left;
		}

		/// <summary>
		/// To get desired top padding.
		/// </summary>
		/// <returns>double value</returns>
		internal double GetTopPadding()
		{
			double topPadding = TopPadding;

			// In widows entry has default padding value 6 so we reduce the value.
			if (DeviceInfo.Platform == DevicePlatform.WinUI && Content is not Editor)
			{
				topPadding -= 6;
			}
			return InputViewPadding.Top < 0 ? topPadding : InputViewPadding.Top + GetDefaultTopPadding();
		}

		/// <summary>
		/// To get desired right padding.
		/// </summary>
		/// <returns>double value</returns>
		internal double GetRightPadding()
		{
			if (BaseLineMaxHeight <= 2)
			{
				return InputViewPadding.Right < 0 ? RightPadding : InputViewPadding.Right;
			}
			return InputViewPadding.Right < 0 ? RightPadding + (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2) : InputViewPadding.Right;
		}

		/// <summary>
		/// To get desired bottom padding.
		/// </summary>
		/// <returns>double value</returns>
		internal double GetBottomPadding()
		{
			double bottomPadding = BottomPadding;

			// In widows entry has default padding value 8 so we reduce the value.
			if (DeviceInfo.Platform == DevicePlatform.WinUI && Content is not Editor)
			{
				bottomPadding -= 8;
			}
			return InputViewPadding.Bottom < 0 ? bottomPadding : InputViewPadding.Bottom + GetDefaultBottomPadding();
		}

		/// <summary>
		/// This method raised when input view OnHandlerChanged event has been occur.
		/// </summary>
		/// <param name="sender">Entry.</param>
		/// <param name="e">EventArgs.</param>
		internal void OnTextInputViewHandlerChanged(object? sender, EventArgs e)
		{
			SetupAndroidView(sender);
			SetupWindowsView(sender);
			SetupIOSView(sender);
		}

		/// <summary>
		/// This method raised when input view of this control has been focused.
		/// </summary>
		/// <param name="sender">Input View.</param>
		/// <param name="e">FocusEventArgs.</param>
		internal void OnTextInputViewFocused(object? sender, FocusEventArgs e)
		{
			IsLayoutFocused = true;
		}

		/// <summary>
		/// This method raised when input view of this control has been unfocused.
		/// </summary>
		/// <param name="sender">Input Layout Base.</param>
		/// <param name="e">Focus Event args. </param>
		internal void OnTextInputViewUnfocused(object? sender, FocusEventArgs e)
		{
			IsLayoutFocused = false;
		}

		/// <summary>
		/// This method raised when input view text has been changed.
		/// </summary>
		/// <param name="sender">InputView.</param>
		/// <param name="e">TextChangedEventArgs.</param>
		internal void OnTextInputViewTextChanged(object? sender, TextChangedEventArgs e)
		{
			if (sender is InputView)
			{
				Text = e.NewTextValue;

				if (string.IsNullOrEmpty(Text) && !IsLayoutFocused)
				{
					if (!IsHintAlwaysFloated)
					{
						IsHintFloated = false;
						IsHintDownToUp = true;
						InvalidateDrawable();
					}
				}
				else if (!string.IsNullOrEmpty(Text) && !IsHintFloated)
				{
					IsHintFloated = true;
					IsHintDownToUp = false;
					InvalidateDrawable();
				}

				SetCustomDescription(this.Content);

				// Clear icon can't draw when isClearIconVisible property updated based on text.
				// So here call the InvalidateDrawable to draw the clear icon.
				if (Text?.Length <= 1)
				{
					InvalidateDrawable();
				}

				//Call this method after bouncing issue has been fixed and implement the ShowCharCount API.
				//UpdateCounterLabelText();
			}

			// In Windows platform editors auto size is not working so here we manually call the measure method.
			if (DeviceInfo.Platform == DevicePlatform.WinUI && Content is Editor)
			{
				InvalidateMeasure();
			}

		}

		internal void StartAnimation()
		{
			if (IsHintFloated && IsLayoutFocused)
			{
				InvalidateDrawable();
				return;
			}

			if (string.IsNullOrEmpty(Text) && !IsLayoutFocused && !EnableFloating)
			{
				IsHintFloated = false;
				IsHintDownToUp = true;
				InvalidateDrawable();
				return;
			}

			if (!string.IsNullOrEmpty(Text) || IsHintAlwaysFloated || !EnableHintAnimation)
			{
				IsHintFloated = true;
				IsHintDownToUp = false;
				if (!EnableHintAnimation && !IsLayoutFocused && string.IsNullOrEmpty(Text))
				{
					IsHintFloated = false;
					IsHintDownToUp = true;
				}
				InvalidateDrawable();
				return;
			}

			_animatingFontSize = IsHintFloated ? FloatedHintFontSize : HintFontSize;
			UpdateStartAndEndValue();
			UpdateSizeStartAndEndValue();
			IsHintFloated = !IsHintFloated;

			// Text is disappear when the rect size is not compatible with text size, so here calculate the rect again.
			if (!IsHintFloated)
			{
				UpdateHintPosition();
			}
			_fontsize = (float)_fontSizeStart;
			var scalingAnimation = new Animation(OnScalingAnimationUpdate, _fontSizeStart, _fontSizeEnd, Easing.Linear);
			var translateAnimation = new Animation(OnTranslateAnimationUpdate, _translateStart, _translateEnd, Easing.SinIn);
			// Add scaling animation concurrently
			translateAnimation.WithConcurrent(scalingAnimation, 0, 1);
			translateAnimation.Commit(this, "showAnimator", rate: 7, length: (uint)DefaultAnimationDuration, finished: OnTranslateAnimationEnded, repeat: () => false);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// This refresh the semantic nodes of the textinputlayout
		/// </summary>
		void ResetSemantics()
		{
			if (_textInputLayoutSemanticsNodes != null)
			{
				_textInputLayoutSemanticsNodes.Clear();
				this.InvalidateSemantics();
			}
		}

		/// <summary>
		/// Resets semantics on first character input or when text is cleared.
		/// </summary>
		void HandleSemanticsReset()
		{
			if (string.IsNullOrEmpty(_text))
			{
				_hasResetSemantics = false;
				ResetSemantics();
			}
			else if (_text.Length == 1 && !_hasResetSemantics)
			{
				ResetSemantics();
				_hasResetSemantics = true;
			}
		}

		/// <summary>
		/// Gets the button size based on the vertical alignment and icon templates.
		/// </summary>
		/// <returns>
		/// The size of the button. If the up/down buttons are vertically aligned and both icons are set,  
		/// it returns <see cref="VerticalUpDownButtonSize"/>; otherwise, it returns <see cref="UpDownButtonSize"/>.
		/// </returns>
		float GetButtonSize()
		{
			if (IsUpDownVerticalAlignment && (UpIconTemplate != null && DownIconTemplate != null))
			{
				return VerticalUpDownButtonSize;
			}
			return UpDownButtonSize;
		}

		/// <summary>
		/// Updates the position of an up/down button view within the layout.
		/// </summary>
		/// <param name="view">The view to be positioned.</param>
		/// <param name="position">The rectangle defining the new position and size of the view.</param>
		void UpdateUpDownButtonViewPosition(View view, RectF position)
		{
			view.Measure(position.Width, position.Height);
			AbsoluteLayout.SetLayoutBounds(view, position);
		}

		/// <summary>
		/// Removes a specified view from the current control if it exists.
		/// </summary>
		/// <param name="view">The view to be removed.</param>
		void RemovedExistingView(View? view)
		{
			if (view != null && this.Contains(view))
			{
				Remove(view);
			}
		}

		/// <summary>
		/// Updates the button view by adding it to the children if not present,
		/// updating its position, and invalidating the measure override.
		/// </summary>
		/// <param name="newView">The new view to be updated.</param>
		/// <param name="rectF">The rectangle defining the position and size of the view.</param>
		void UpdateButtonView(View? newView, RectF rectF)
		{
			if (newView != null)
			{
				if (!this.Children.Contains(newView))
				{
					this.Children.Add(newView);
					this.InvalidateMeasureOverride();
				}
				UpdateUpDownButtonViewPosition(newView, rectF);
			}
		}
		
		/// <summary>
		/// Updates the translation animation by modifying the Y-coordinate of the hint text rectangle.
		/// Also updates the font size and triggers a redraw of the drawable.
		/// </summary>
		/// <param name="value">The new Y-coordinate value for the hint text rectangle.</param>
		void OnTranslateAnimationUpdate(double value)
		{
			_isAnimating = true;
			_hintRect.Y = (float)value;
			_animatingFontSize = _fontsize;
			InvalidateDrawable();
		}

		double GetDefaultTopPadding()
		{
			return (ShowHint ? (IsFilled ? DefaultAssistiveTextHeight : DefaultAssistiveTextHeight / 2) : BaseLineMaxHeight);
		}

		double GetDefaultBottomPadding()
		{
			return (ReserveSpaceForAssistiveLabels ? DefaultAssistiveLabelPadding + TotalAssistiveTextHeight() : 0);
		}

		double TotalAssistiveTextHeight()
		{
			return (ErrorTextSize.Height > HelperTextSize.Height ? ErrorTextSize.Height : HelperTextSize.Height);
		}

		void UpdateContentMargin(View view)
		{
			if (view == null)
			{
				return;
			}

			view.Margin = new Thickness()
			{
				Top = GetTopPadding(),
				Bottom = GetBottomPadding(),
				Left = GetLeftPadding(),
				Right = GetRightPadding()
			};
		}

		void WireLabelStyleEvents()
		{
			if (HelperLabelStyle != null)
			{
				HelperLabelStyle.PropertyChanged += OnHelperLabelStylePropertyChanged;
			}
			if (ErrorLabelStyle != null)
			{
				ErrorLabelStyle.PropertyChanged += OnErrorLabelStylePropertyChanged;
			}
			if (HintLabelStyle != null)
			{
				HintLabelStyle.PropertyChanged += OnHintLabelStylePropertyChanged;
			}
			if (CounterLabelStyle != null)
			{
				CounterLabelStyle.PropertyChanged += OnCounterLabelStylePropertyChanged;
			}
		}

		void UnWireLabelStyleEvents()
		{
			if (HelperLabelStyle != null)
			{
				HelperLabelStyle.PropertyChanged -= OnHelperLabelStylePropertyChanged;
			}
			if (ErrorLabelStyle != null)
			{
				ErrorLabelStyle.PropertyChanged -= OnErrorLabelStylePropertyChanged;
			}
			if (HintLabelStyle != null)
			{
				HintLabelStyle.PropertyChanged -= OnHintLabelStylePropertyChanged;
			}
			if (CounterLabelStyle != null)
			{
				CounterLabelStyle.PropertyChanged -= OnCounterLabelStylePropertyChanged;
			}
		}

		void OnHintLabelStylePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
			{
				HintFontSize = (float)(labelStyle.FontSize < 12d ? FloatedHintFontSize : labelStyle.FontSize);
				MatchLabelStyleProperty(_internalHintLabelStyle, labelStyle, e.PropertyName);
				InvalidateDrawable();
			}
		}

		void OnHelperLabelStylePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
			{
				MatchLabelStyleProperty(_internalHelperLabelStyle, labelStyle, e.PropertyName);
				InvalidateDrawable();
			}
		}

		void OnErrorLabelStylePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
			{
				MatchLabelStyleProperty(_internalErrorLabelStyle, labelStyle, e.PropertyName);
				InvalidateDrawable();
			}
		}

		void OnCounterLabelStylePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
			{
				MatchLabelStyleProperty(_internalCounterLabelStyle, labelStyle, e.PropertyName);
				InvalidateDrawable();
			}
		}

		void MatchLabelStyleProperty(LabelStyle internalLabelStyle, LabelStyle labelStyle, string propertyName)
		{
			if (propertyName == nameof(LabelStyle.FontAttributes))
			{
				internalLabelStyle.FontAttributes = labelStyle.FontAttributes;
			}

			if (propertyName == nameof(LabelStyle.TextColor))
			{
				internalLabelStyle.TextColor = labelStyle.TextColor;
			}

			if (propertyName == nameof(LabelStyle.FontSize))
			{
				internalLabelStyle.FontSize = labelStyle.FontSize;
			}

			if (propertyName == nameof(LabelStyle.FontFamily))
			{
				internalLabelStyle.FontFamily = labelStyle.FontFamily;
			}
		}

		/// <summary>
		/// Raised when the <see cref="IsEnabled"/> property was changed.
		/// </summary>
		/// <param name="isEnabled">Boolean</param>
		void OnEnabledPropertyChanged(bool isEnabled)
		{
			base.IsEnabled = isEnabled;

			if (Content != null)
			{
				Content.IsEnabled = isEnabled;
			}
			InvalidateDrawable();
			if (!IsEnabled)
			{
				if (ContainerType == ContainerType.Filled)
				{
					VisualStateManager.GoToState(this, "Disabled");
				}
				else if (ContainerType == ContainerType.Outlined)
				{
					VisualStateManager.GoToState(this, "OutlinedDisabled");
				}
				else if (ContainerType == ContainerType.None)
				{
					VisualStateManager.GoToState(this, "LineDisabled");
				}
			}
		}

		/// <summary>
		/// This method update the InputView, TrailView and LeadView position.
		/// </summary>
		void UpdateViewBounds()
		{
			UpdateLeadingViewPosition();
			UpdateTrailingViewPosition();
			UpdateContentPosition();
			InvalidateDrawable();
		}

		/// <summary>
		/// Gets the total number of lines required for given helper and error text.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <returns>Number of Lines</returns>
		int GetHintLineCount(double totalWidth)
		{
			int number = 1;

			var availableWidth = GetHintTextWidth();

			if (availableWidth == 1 || availableWidth >= totalWidth)
			{
				return number;
			}

			number += (int)(totalWidth / availableWidth);

			return number;
		}

		/// <summary>
		/// Gets the totol number of lines required for given helper and error text.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <returns>Number of Lines</returns>
		int GetAssistiveTextLineCount(double totalWidth)
		{
			int number = 1;

			var availableWidth = GetAssistiveTextWidth();

			if (availableWidth == 1)
			{
				return number;
			}

			number += (int)(totalWidth / availableWidth);

			return number;
		}

		/// <summary>
		/// This method return size based on ReserveSpaceForAssistiveLabels boolean property.
		/// </summary>
		/// <param name="size"></param>
		/// <returns>Size</returns>
		SizeF GetLabelSize(SizeF size)
		{
			if (ReserveSpaceForAssistiveLabels)
			{
				return size;
			}
			else
			{
				size.Height = 0;
				size.Width = 0;
				return size;
			}
		}

#if ANDROID

		private void ApplyNativeProperties(Entry entry)
		{
			if (entry.Handler?.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
			{
				androidEntry.ImeOptions = entry.ReturnType switch
				{
					ReturnType.Default => Android.Views.InputMethods.ImeAction.Unspecified,
					ReturnType.Next => Android.Views.InputMethods.ImeAction.Next,
					ReturnType.Done => Android.Views.InputMethods.ImeAction.Done,
					ReturnType.Go => Android.Views.InputMethods.ImeAction.Go,
					ReturnType.Search => Android.Views.InputMethods.ImeAction.Search,
					ReturnType.Send => Android.Views.InputMethods.ImeAction.Send,
					_ => Android.Views.InputMethods.ImeAction.Unspecified
				};
			}
		}

#endif

		void SetupAndroidView(object? sender)
		{
#if ANDROID
			if (sender is View view && view.Handler != null && view.Handler.PlatformView is Android.Views.View androidView)
			{
				androidView.SetBackgroundColor(Android.Graphics.Color.Transparent);
				androidView.SetPadding(0, 0, 0, 0);
			}
			if (sender is Entry entry)
			{
				ApplyNativeProperties(entry);
			}
#endif
		}

		void SetupWindowsView(object? sender)
		{
#if WINDOWS
			if (sender is InputView inputView && inputView.Handler != null && inputView.Handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox windowEntry)
			{
				ConfigureWindowsEntry(windowEntry);
			}
			else if (sender is Microsoft.Maui.Controls.Picker picker && picker.Handler != null && picker.Handler.PlatformView is Microsoft.UI.Xaml.Controls.ComboBox windowPicker)
			{
				ConfigureWindowsPicker(windowPicker);
			}
			else if (sender is DatePicker datePicker && datePicker.Handler != null && datePicker.Handler.PlatformView is Microsoft.UI.Xaml.Controls.CalendarDatePicker windowDatePicker)
			{
				ConfigureWindowsDatePicker(windowDatePicker);
			}
			else if (sender is TimePicker timePicker && timePicker.Handler != null && timePicker.Handler.PlatformView is Microsoft.UI.Xaml.Controls.TimePicker windowTimePicker)
			{
				ConfigureWindowsTimePicker(windowTimePicker);
			}
#endif
		}

		void SetupIOSView(object? sender)
		{
#if IOS || MACCATALYST
			if (sender is View view && view.Handler != null && view.Handler.PlatformView is UIKit.UITextField iOSEntry)
			{
				ConfigureIOSTextField(iOSEntry);
			}

			if (sender is Editor editor && editor.Handler != null && editor.Handler.PlatformView is UIKit.UITextView iOSEditor)
			{
				ConfigureIOSTextView(iOSEditor);
			}
#endif
		}

#if WINDOWS

		void ConfigureWindowsEntry(Microsoft.UI.Xaml.Controls.TextBox windowEntry)
		{
			windowEntry.LosingFocus -= OnWindowsEntryLosingFocus;
			windowEntry.LosingFocus += OnWindowsEntryLosingFocus;
			windowEntry.PointerEntered -= OnPointerEntered;
			windowEntry.PointerEntered += OnPointerEntered;
			windowEntry.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			windowEntry.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			windowEntry.Padding = new Microsoft.UI.Xaml.Thickness(0);
			windowEntry.Resources["TextControlBorderThemeThicknessFocused"] = windowEntry.BorderThickness;
		}

		void ConfigureWindowsPicker(Microsoft.UI.Xaml.Controls.ComboBox windowPicker)
		{
			windowPicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			windowPicker.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			windowPicker.Padding = new Microsoft.UI.Xaml.Thickness(0);
			windowPicker.Resources["TextControlBorderThemeThicknessFocused"] = windowPicker.BorderThickness;
			windowPicker.Resources["ComboBoxDropDownGlyphForeground"] = windowPicker.Background;
			windowPicker.Header = null;
			windowPicker.HeaderTemplate = null;
		}

		void ConfigureWindowsDatePicker(Microsoft.UI.Xaml.Controls.CalendarDatePicker windowDatePicker)
		{
			windowDatePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			windowDatePicker.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			//In native windows date picker text block has default padding value of (12, 0,0,2).
			windowDatePicker.Margin = new Microsoft.UI.Xaml.Thickness(-12, 0, 0, -2);
			windowDatePicker.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
			windowDatePicker.Resources["TextControlBorderThemeThicknessFocused"] = windowDatePicker.BorderThickness;
			windowDatePicker.Resources["SystemColorHighlightColorBrush"] = windowDatePicker.Background;
			windowDatePicker.Resources["CalendarDatePickerCalendarGlyphForeground"] = windowDatePicker.Background;
			windowDatePicker.Resources["CalendarDatePickerCalendarGlyphForegroundPointerOver"] = windowDatePicker.Background;
			windowDatePicker.Resources["CalendarDatePickerCalendarGlyphForegroundPressed"] = windowDatePicker.Background;
		}

		void ConfigureWindowsTimePicker(Microsoft.UI.Xaml.Controls.TimePicker windowTimePicker)
		{
			windowTimePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			windowTimePicker.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			windowTimePicker.Padding = new Microsoft.UI.Xaml.Thickness(0);
			windowTimePicker.Resources["TextControlBorderThemeThicknessFocused"] = windowTimePicker.BorderThickness;
			windowTimePicker.Resources["TimePickerSpacerThemeWidth"] = "0";
		}

		void OnPointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (_effectsRenderer != null && _effectsRenderer.HighlightBounds.Width > 0 && _effectsRenderer.HighlightBounds.Height > 0)
			{
				_effectsRenderer.RemoveHighlight();
			}
		}

		void OnWindowsEntryLosingFocus(Microsoft.UI.Xaml.UIElement element, Microsoft.UI.Xaml.Input.LosingFocusEventArgs args)
		{
			if (IsIconPressed && element is Microsoft.UI.Xaml.Controls.TextBox textbox)
			{
				args.TryCancel();
				if (Content is Entry entry)
				{
					textbox.SelectionStart = entry.CursorPosition;
				}
				IsIconPressed = false;
			}
		}
#endif

#if IOS || MACCATALYST
		void ConfigureIOSTextField(UIKit.UITextField iOSEntry)
		{
			iOSEntry.BackgroundColor = UIKit.UIColor.Clear;
			iOSEntry.BorderStyle = UIKit.UITextBorderStyle.None;
			iOSEntry.Layer.BorderWidth = 0f;
			iOSEntry.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
			iOSEntry.LeftViewMode = UIKit.UITextFieldViewMode.Never;
			iOSEntry.ShouldEndEditing += ShouldEndEditing;
			
			// Configure accessibility for Tab navigation support
			iOSEntry.IsAccessibilityElement = true;
			iOSEntry.AccessibilityTraits = UIKit.UIAccessibilityTrait.None;
			
			uiEntry = iOSEntry;
		}

		/// <summary>
		/// Determines whether editing should end for the given text field.
		/// Returns false if an icon is pressed, otherwise returns true.
		/// </summary>
		/// <param name="textField">The UITextField being edited.</param>
		/// <returns>A boolean indicating whether editing should end.</returns>
		bool ShouldEndEditing(UIKit.UITextField textField)
		{
			return (this.IsIconPressed ? false : true);
		}

		void ConfigureIOSTextView(UIKit.UITextView iOSEditor)
		{
			iOSEditor.BackgroundColor = UIKit.UIColor.Clear;
			iOSEditor.Layer.BorderWidth = 0f;
			iOSEditor.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
			iOSEditor.TextContainer.LineFragmentPadding = 0f;
		}
#endif

		/// <summary>
		/// This method sets the default stroke value based on control states.
		/// </summary>
		void AddDefaultVSM()
		{
			VisualStateGroupList visualStateGroupList = [];

			VisualStateGroup visualStateGroup = new VisualStateGroup
			{
				Name = "CommonStates"
			};

			VisualState focusedState = new VisualState() { Name = "Focused" };
			Setter focusedStrokeSetter = new Setter() { Property = StrokeProperty, Value = _defaultFocusStrokeColor };
			Setter focusedBackgroundSetter = new Setter() { Property = ContainerBackgroundProperty, Value = ContainerBackground };
			focusedState.Setters.Add(focusedStrokeSetter);
			focusedState.Setters.Add(focusedBackgroundSetter);

			VisualState errorState = new VisualState() { Name = "Error" };
			Setter errorStrokeSetter = new Setter() { Property = StrokeProperty, Value = _defaultErrorStrokeColor };
			errorState.Setters.Add(errorStrokeSetter);

			VisualState normalState = new VisualState() { Name = "Normal" };
			Setter normalStrokeSetter = new Setter() { Property = StrokeProperty, Value = Stroke };
			Setter normalBackgroundSetter = new Setter() { Property = ContainerBackgroundProperty, Value = ContainerBackground };
			normalState.Setters.Add(normalStrokeSetter);
			normalState.Setters.Add(normalBackgroundSetter);

			VisualState disabledState = new VisualState() { Name = "Disabled" };
			Setter disabledStrokeSetter = new Setter() { Property = StrokeProperty, Value = _defaultDisabledStrokeColor };
			Setter disabledBackgroundSetter = new Setter() { Property = ContainerBackgroundProperty, Value = _defaultDisabledContainerBackground };
			disabledState.Setters.Add(disabledStrokeSetter);
			disabledState.Setters.Add(disabledBackgroundSetter);


			visualStateGroup.States.Add(normalState);
			visualStateGroup.States.Add(focusedState);
			visualStateGroup.States.Add(errorState);
			visualStateGroup.States.Add(disabledState);
			visualStateGroupList.Add(visualStateGroup);
			VisualStateManager.SetVisualStateGroups(this, visualStateGroupList);
		}

		/// <summary>
		/// This method unhook the all the hooked event of the entry.
		/// </summary>
		void UnwireEvents()
		{
			if (Content != null)
			{
				switch (Content)
				{
					case InputView inputView:
						inputView.Focused -= OnTextInputViewFocused;
						inputView.Unfocused -= OnTextInputViewUnfocused;
						inputView.TextChanged -= OnTextInputViewTextChanged;
						inputView.HandlerChanged -= OnTextInputViewHandlerChanged;
						break;
					case Microsoft.Maui.Controls.Picker picker:
						picker.Focused -= OnTextInputViewFocused;
						picker.Unfocused -= OnTextInputViewUnfocused;
						picker.HandlerChanged -= OnTextInputViewHandlerChanged;
						picker.SelectedIndexChanged -= OnPickerSelectedIndexChanged;
						break;
					case TimePicker timePicker:
						timePicker.HandlerChanged -= OnTextInputViewHandlerChanged;
						timePicker.Focused -= OnTextInputViewFocused;
						timePicker.Unfocused -= OnTextInputViewUnfocused;
						break;
					case DatePicker datePicker:
						datePicker.HandlerChanged -= OnTextInputViewHandlerChanged;
						datePicker.Focused -= OnTextInputViewFocused;
						datePicker.Unfocused -= OnTextInputViewUnfocused;
						break;
					case Syncfusion.Maui.Toolkit.NumericEntry.SfNumericEntry numericEntryView:
						if (numericEntryView.Children[0] is Entry numericInputView)
						{
							numericInputView.Focused -= OnTextInputViewFocused;
							numericInputView.Unfocused -= OnTextInputViewUnfocused;
							numericInputView.TextChanged -= OnTextInputViewTextChanged;
							numericInputView.HandlerChanged -= OnTextInputViewHandlerChanged;
						}
						break;
				}

#if MACCATALYST || IOS
				if (Handler == null && uiEntry != null)
				{
					// Setting ShouldEndEditing to null to prevent memory leaks
					uiEntry.ShouldEndEditing -= ShouldEndEditing;
					uiEntry = null;
				}
#endif
			}

			UnWireLabelStyleEvents();
		}

		/// <summary>
		/// Initializes or reinitializes the cancellation token source for long press operations.
		/// </summary>
		void InitializeTokenSource()
		{
			// Cancel any ongoing operation
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		/// <summary>
		/// Stops the long press timer and cancels any ongoing long press operation.
		///</summary>
		void StopLongPressTimer()
		{
			_isPressOccurring = false;
			// Cancel any ongoing operation
			_cancellationTokenSource?.Cancel();
		}

		/// <summary>
		/// Recursively call the increment and decrement value
		/// </summary>
		/// <param name="touchpoint">The touchpoint</param>
		/// <returns></returns>
		async Task RecursivePressedAsync(Point touchpoint)
		{
			if (this.Content is ITextInputLayout numericEntry)
			{
				if ((_downIconRectF.Contains(touchpoint) && IsUpDownVerticalAlignment) || (_upIconRectF.Contains(touchpoint) && !IsUpDownVerticalAlignment))
				{
					numericEntry.UpButtonPressed();
				}
				else if ((_upIconRectF.Contains(touchpoint) && IsUpDownVerticalAlignment) || (_downIconRectF.Contains(touchpoint) && !IsUpDownVerticalAlignment))
				{
					numericEntry.DownButtonPressed();
				}
			}

			InitializeTokenSource();
			// Wait for the long press duration without throwing TaskCanceledException
			// If the delay was completed and not cancelled
			if (await IsLongPressActivate(ContinueDelay))
			{
				await RecursivePressedAsync(touchpoint);
			}
		}

		/// <summary>
		/// Determines if a long press has been activated based on a specified delay.
		/// </summary>
		/// <param name="delay">The duration in milliseconds to wait before considering the press as a long press.</param>
		/// <returns>True if the long press is activated, false otherwise.</returns>
		private async Task<bool> IsLongPressActivate(int delay)
		{
			if (_cancellationTokenSource == null)
			{
				return false;
			}

			try
			{
				var delayTask = Task.Delay(delay, _cancellationTokenSource.Token);
				var completedTask = await Task.WhenAny(delayTask, Task.Delay(Timeout.Infinite));
				return delayTask == completedTask && _isPressOccurring;
			}
			catch (Exception)
			{
				// The task was canceled, which means the press was released before the delay completed
				return false;
			}
		}

		/// <summary>
		/// Start the long press timer.
		///</summary>
		async void StartPressTimer(Point touchpoint)
		{
			_isPressOccurring = true;
			InitializeTokenSource();

			if (await IsLongPressActivate(StartDelay))
			{
				await RecursivePressedAsync(touchpoint);
			}
		}

		void WireEvents()
		{
			if (Content != null)
			{
				switch (Content)
				{
					case InputView inputView:
						inputView.Focused += OnTextInputViewFocused;
						inputView.Unfocused += OnTextInputViewUnfocused;
						inputView.TextChanged += OnTextInputViewTextChanged;
						inputView.HandlerChanged += OnTextInputViewHandlerChanged;
						break;
					case Microsoft.Maui.Controls.Picker picker:
						picker.Focused += OnTextInputViewFocused;
						picker.Unfocused += OnTextInputViewUnfocused;
						picker.HandlerChanged += OnTextInputViewHandlerChanged;
						picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
						break;
					case TimePicker timePicker:
						timePicker.HandlerChanged += OnTextInputViewHandlerChanged;
						timePicker.Focused += OnTextInputViewFocused;
						timePicker.Unfocused += OnTextInputViewUnfocused;
						break;
					case DatePicker datePicker:
						datePicker.HandlerChanged += OnTextInputViewHandlerChanged;
						datePicker.Focused += OnTextInputViewFocused;
						datePicker.Unfocused += OnTextInputViewUnfocused;
						break;
					case Syncfusion.Maui.Toolkit.NumericEntry.SfNumericEntry numericEntryView:
						if(numericEntryView.Children[0] is Entry numericInputView)
						{
							numericInputView.Focused += OnTextInputViewFocused;
							numericInputView.Unfocused += OnTextInputViewUnfocused;
							numericInputView.TextChanged += OnTextInputViewTextChanged;
							numericInputView.HandlerChanged += OnTextInputViewHandlerChanged;
						}
						break;
				}
			}

			WireLabelStyleEvents();
		}

		/// <summary>
		/// This method changed the password toggle visibility icon and drop down window visibility into collapesed icon and viceversa.
		/// </summary>
#if IOS || MACCATALYST
		async void ToggleIcon()
#else
		void ToggleIcon()
#endif
		{
			if (Content is Entry entry)
			{
				_isPasswordTextVisible = !_isPasswordTextVisible;
				entry.IsPassword = !_isPasswordTextVisible;
				InvokePasswordVisibilityToggledEvent(new PasswordVisibilityToggledEventArgs());
				InvalidateDrawable();
			}
#if IOS || MACCATALYST
			await Task.Delay(10);
			IsIconPressed = false;
#endif
		}

		/// <summary>
		/// This method perform adding a new view and removing the old view from the control.
		/// </summary>
		/// <param name="oldValue">New View.</param>
		/// <param name="newValue">Old View.</param>
		void AddView(object oldValue, object newValue)
		{
			var oldView = (View)oldValue;
			if (oldView != null && this.Contains(oldView))
			{
				Remove(oldView);
			}

			var newView = (View)newValue;
			if (newView != null)
			{
				Add(newView);
			}

			UpdateLeadingViewVisibility(ShowLeadingView);
			UpdateTrailingViewVisibility(ShowTrailingView);
		}

		/// <summary>
		/// This method update the Content RectF.
		/// </summary>
		void UpdateContentPosition()
		{
			UpdateLeadViewWidthForContent();
			UpdateTrailViewWidthForContent();

			if (Content != null)
			{
				_viewBounds.X = (int)_leadViewWidth;
				UpdatePosition();
				_viewBounds.Y = 0;
				_viewBounds.Width = (int)(Width - _leadViewWidth - _trailViewWidth);
				_viewBounds.Height = (int)Height;

				if (EnablePasswordVisibilityToggle && !ShowUpDownButton)
				{
					_viewBounds.Width -= (float)(EnablePasswordVisibilityToggle ? ((IconSize * 2) - RightPadding + DefaultAssistiveLabelPadding + 7) : (IconSize - RightPadding + DefaultAssistiveLabelPadding) + 3);
				}

				if (IsClearIconVisible && ShowUpDownButton)
				{
					_viewBounds.Width -= (float)(IconSize - RightPadding + DefaultAssistiveLabelPadding) + 3;
				}
				if (ShowUpDownButton)
				{
					_viewBounds.Width -= (float)(IconSize * (IsUpDownVerticalAlignment ? 1 : 2));
				}

				if (_viewBounds.Height >= 0 || _viewBounds.Width >= 0)
				{
					AbsoluteLayout.SetLayoutBounds(Content, _viewBounds);
				}
			}
		}

		void UpdatePosition()
		{
			if (!IsUpDownVerticalAlignment)
			{
				if (IsUpDownAlignmentLeft)
				{
					_viewBounds.X = !IsRTL ? _downIconRectF.X + (IsNone ? _downIconRectF.Width : _downIconRectF.Width / 2) : (float)this.Width - _downIconRectF.X - (IsNone ? 0 : _downIconRectF.Width / 2);
				}
				else if (IsUpDownAlignmentBoth)
				{
					_viewBounds.X = !IsRTL ? _upIconRectF.X + (IsNone ? _upIconRectF.Width : _upIconRectF.Width / 2) : (float)this.Width - _upIconRectF.X - (IsNone ? 0 : _upIconRectF.Width / 2);
				}
			}
			else
			{
				if (IsUpDownAlignmentLeft)
				{
					_viewBounds.X = !IsRTL ? _downIconRectF.X + (IsNone ? _downIconRectF.Width : _downIconRectF.Width / 2) : (float)this.Width - _downIconRectF.X - (IsNone ? 0 : _downIconRectF.Width / 2);
				}
			}
		}

		/// <summary>
		/// This method update the LeadingView RectF.
		/// </summary>
		void UpdateLeadingViewPosition()
		{
			if (ShowLeadingView && LeadingView != null)
			{
				_viewBounds.X = (float)(_leadingViewLeftPadding + ((IsOutlined && LeadingViewPosition == ViewPosition.Inside) ? BaseLineMaxHeight : 0));
				_viewBounds.Y = (float)(IsOutlined ? BaseLineMaxHeight : 0);
#if WINDOWS || MACCATALYST || IOS
				_viewBounds.Width = (float)(LeadingView.WidthRequest == -1 ? (LeadingView.Width == -1 || (int)LeadingView.Width == (int)Width) ? _defaultLeadAndTrailViewWidth : LeadingView.Width : LeadingView.WidthRequest);
#else
				_viewBounds.Width = (float)(LeadingView.WidthRequest == -1 ? (LeadingView.Width == -1 || LeadingView.Width == Width) ? _defaultLeadAndTrailViewWidth : LeadingView.Width : LeadingView.WidthRequest);
#endif
				_viewBounds.Height = (float)(Height - AssistiveLabelPadding - TotalAssistiveTextHeight());

				if (IsOutlined || IsFilled)
				{
					LeadingView.VerticalOptions = LayoutOptions.Center;
				}

				if (IsNone)
				{
					_viewBounds.Height = (float)(_viewBounds.Height - BaseLineMaxHeight - NoneBottomPadding);
					LeadingView.VerticalOptions = LayoutOptions.End;
				}

				if (_viewBounds.Height >= 0 || _viewBounds.Width >= 0)
				{
					AbsoluteLayout.SetLayoutBounds(LeadingView, _viewBounds);
				}
			}
		}

		/// <summary>
		/// This method update the TrailingView RectF.
		/// </summary>
		void UpdateTrailingViewPosition()
		{
			if (ShowTrailingView && TrailingView != null)
			{
#if WINDOWS || MACCATALYST || IOS
				_viewBounds.Width = (float)(TrailingView.WidthRequest == -1 ? (TrailingView.Width == -1 || (int)TrailingView.Width == (int)Width) ? _defaultLeadAndTrailViewWidth : TrailingView.Width : TrailingView.WidthRequest);
#else
				_viewBounds.Width = (float)(TrailingView.WidthRequest == -1 ? (TrailingView.Width == -1 || TrailingView.Width == Width) ? _defaultLeadAndTrailViewWidth : TrailingView.Width : TrailingView.WidthRequest);
#endif
				_viewBounds.X = (float)(Width - _viewBounds.Width - _trailingViewRightPadding);
				_viewBounds.Y = (float)(IsOutlined ? BaseLineMaxHeight : 0);
				_viewBounds.Height = (float)(Height - AssistiveLabelPadding - TotalAssistiveTextHeight());

				if (IsOutlined || IsFilled)
				{
					TrailingView.VerticalOptions = LayoutOptions.Center;
				}

				if (IsNone)
				{
					_viewBounds.Height = (float)(_viewBounds.Height - BaseLineMaxHeight - NoneBottomPadding);
					TrailingView.VerticalOptions = LayoutOptions.End;
				}

				if (_viewBounds.Height >= 0 || _viewBounds.Width >= 0)
				{
					AbsoluteLayout.SetLayoutBounds(TrailingView, _viewBounds);
				}
			}
		}

		/// <summary>
		/// This method changes the LeadingView visibility based on boolean parameter.
		/// </summary>
		/// <param name="showLeadingView">Boolean.</param>
		void UpdateLeadingViewVisibility(bool showLeadingView)
		{
			if (LeadingView != null)
			{
				LeadingView.IsVisible = showLeadingView;
			}
		}

		/// <summary>
		/// This method changes the TrailingView visibility based on boolean parameter.
		/// </summary>
		/// <param name="showTrailingView">Boolean.</param>
		void UpdateTrailingViewVisibility(bool showTrailingView)
		{
			if (TrailingView != null)
			{
				TrailingView.IsVisible = showTrailingView;
			}
		}

		/// <summary>
		/// This method updates the LeadingView width with their padding values.
		/// </summary>
		void UpdateLeadViewWidthForContent()
		{
			_leadViewWidth = 0;

			if (ShowLeadingView && LeadingView != null)
			{
#if WINDOWS || MACCATALYST || IOS
				_leadViewWidth = ((LeadingView.Width == -1 || (int)LeadingView.Width == (int)Width) ? _defaultLeadAndTrailViewWidth : LeadingView.Width) + (LeadingViewPosition == ViewPosition.Outside ? _leadingViewLeftPadding + _leadingViewRightPadding : IsNone ? _leadingViewLeftPadding + _leadingViewRightPadding : _leadingViewLeftPadding);
#else
				_leadViewWidth = ((LeadingView.Width == -1 || LeadingView.Width == Width) ? _defaultLeadAndTrailViewWidth : LeadingView.Width) + (LeadingViewPosition == ViewPosition.Outside ? _leadingViewLeftPadding + _leadingViewRightPadding : IsNone ? _leadingViewLeftPadding + _leadingViewRightPadding : _leadingViewLeftPadding);
#endif
			}
		}

		/// <summary>
		/// This method updates the TrailingView width with their padding values.
		/// </summary>
		void UpdateTrailViewWidthForContent()
		{
			_trailViewWidth = 0;

			if (ShowTrailingView && TrailingView != null)
			{
#if WINDOWS || MACCATALYST || IOS
				_trailViewWidth = ((TrailingView.Width == -1 || (int)TrailingView.Width == (int)Width) ? _defaultLeadAndTrailViewWidth : TrailingView.Width) + (TrailingViewPosition == ViewPosition.Outside ? _trailingViewLeftPadding + _trailingViewRightPadding : _trailingViewLeftPadding);
#else
				_trailViewWidth = ((TrailingView.Width == -1 || TrailingView.Width == Width) ? _defaultLeadAndTrailViewWidth : TrailingView.Width) + (TrailingViewPosition == ViewPosition.Outside ? _trailingViewLeftPadding + _trailingViewRightPadding : _trailingViewLeftPadding);
#endif
			}
		}

		/// <summary>
		/// This method update the leading view width only in leading view position is outside.
		/// </summary>
		void UpdateLeadViewWidthForBorder()
		{
			_leadViewWidth = 0;
			if (ShowLeadingView && LeadingView != null && LeadingViewPosition == ViewPosition.Outside)
			{
				_leadViewWidth = LeadingView.Width + _leadingViewLeftPadding + _leadingViewRightPadding;
			}
		}

		/// <summary>
		/// This method update the leading view width only in leading view position is outside.
		/// </summary>
		void UpdateTrailViewWidthForBorder()
		{
			_trailViewWidth = 0;
			if (ShowTrailingView && TrailingView != null && TrailingViewPosition == ViewPosition.Outside)
			{
				_trailViewWidth = TrailingView.Width + _trailingViewLeftPadding + _trailingViewRightPadding;
			}
		}

		/// <summary>
		/// This method updates the effects tilRenderer rectF.
		/// </summary>
		void UpdateEffectsRendererBounds()
		{
			if (_effectsRenderer != null)
			{
				_effectsRenderer.RippleBoundsCollection.Clear();
				_effectsRenderer.HighlightBoundsCollection.Clear();

				if (IsPassowordToggleIconVisible)
				{
					UpdateEffectsBounds(_passwordToggleIconRectF);
				}

				if (ShowUpDownButton)
				{
					if (Content is SfNumericUpDown numericUpDown )
					{
						if (numericUpDown._valueStates != ValueStates.Minimum)
						{
							UpdateEffectsBounds(_isUpDownVerticalAlignment ? _upIconRectF : _downIconRectF);
						}

						if (numericUpDown._valueStates != ValueStates.Maximum)
						{
							UpdateEffectsBounds(_isUpDownVerticalAlignment ? _downIconRectF : _upIconRectF);
						}
					}
				}

				if (IsClearIconVisible && (IsLayoutFocused))
				{
					UpdateEffectsBounds(_clearIconRectF);
				}
			}
		}

		/// <summary>
		/// Updates the bounds of effects (such as shadows or highlights) according to the specified rectangle area.
		/// </summary>
		/// <param name="bounds">The bounding rectangle defining the area within which effects should be applied.</param>
		internal void UpdateEffectsBounds(RectF bounds)
		{
			if (_effectsRenderer is null)
			{
				return;
			}
			_effectsRenderer.RippleBoundsCollection.Add(bounds);
			_effectsRenderer.HighlightBoundsCollection.Add(bounds);
		}

		/// <summary>
		/// This method update the counter text string value.
		/// </summary>
		void UpdateCounterLabelText()
		{
			if (ShowCharCount)
			{
				var textLength = string.IsNullOrEmpty(Text) ? 0 : Text.Length;
				_counterText = CharMaxLength == int.MaxValue ? $"{textLength}" : $"{textLength}/{CharMaxLength}";
				InvalidateDrawable();
			}
		}

		/// <summary>
		/// This method updates the hint text position in none type container.
		/// </summary>
		void UpdateNoneContainerHintPosition()
		{
			if (IsHintFloated)
			{
				_hintRect.X = 0;
				_hintRect.Y = (float)DefaultAssistiveLabelPadding;
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = FloatedHintSize.Height;
			}
			else
			{
				_hintRect.X = 0;
				_hintRect.Y = (int)(Height - HintSize.Height - BaseLineMaxHeight - AssistiveLabelPadding - (DefaultAssistiveLabelPadding * 2) - (ErrorTextSize.Height > HelperTextSize.Height ? ErrorTextSize.Height : HelperTextSize.Height));
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = HintSize.Height;
			}
		}


		/// <summary>
		/// This method updates the hint text position in filled type container.
		/// </summary>
		void UpdateFilledContainerHintPosition()
		{
			if (IsHintFloated)
			{
				_hintRect.X = (float)((StartX) + DefaultAssistiveLabelPadding);
				_hintRect.Y = (float)((DefaultAssistiveLabelPadding * 2) + (DefaultAssistiveLabelPadding / 2));
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = FloatedHintSize.Height;
			}
			else
			{
				_hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding);
				_hintRect.Y = (float)((Height - TotalAssistiveTextHeight() - AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = HintSize.Height;
			}
		}

		/// <summary>
		/// This method updates the hint text position in outlined type container.
		/// </summary>
		void UpdateOutlinedContainerHintPosition()
		{
			if (IsHintFloated)
			{
				_hintRect.X = (int)(StartX + DefaultAssistiveLabelPadding + BaseLineMaxHeight);
				if (BaseLineMaxHeight > 2)
				{
					_hintRect.X = (int)(StartX + DefaultAssistiveLabelPadding + (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2));
				}

				_hintRect.Y = 0;
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = (int)FloatedHintSize.Height;
			}
			else
			{
				_hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding + BaseLineMaxHeight);
				if (BaseLineMaxHeight > 2)
				{
					_hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding + (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2));
				}

				_hintRect.Y = _outlineRectF.Center.Y - (HintSize.Height / 2);
				_hintRect.Width = (float)GetHintTextWidth();
				_hintRect.Height = HintSize.Height;
			}
		}

		/// <summary>
		/// This method updates the starting point of the hint text need.
		/// </summary>
		void UpdateHintPosition()
		{
			if (_isAnimating)
			{
				return;
			}

			if (IsNone)
			{
				UpdateNoneContainerHintPosition();
			}

			if (IsFilled)
			{
				UpdateFilledContainerHintPosition();
			}

			if (IsOutlined)
			{
				UpdateOutlinedContainerHintPosition();
			}

			if (ShowLeadingView && LeadingView != null)
			{
				_leadViewWidth = LeadingView.Width + (LeadingViewPosition == ViewPosition.Outside ? _leadingViewLeftPadding + _leadingViewRightPadding : IsNone ? _leadingViewLeftPadding + _leadingViewRightPadding : _leadingViewLeftPadding);
				_hintRect.X += (float)_leadViewWidth;
			}

			if (ShowUpDownButton && (IsUpDownAlignmentLeft || IsUpDownAlignmentBoth) && ((IsUpDownVerticalAlignment && !IsUpDownAlignmentBoth) || !IsUpDownVerticalAlignment))
			{
				if ((IsUpDownAlignmentBoth && !IsUpDownVerticalAlignment) || (IsUpDownVerticalAlignment && !IsUpDownAlignmentBoth))
				{
					_hintRect.X += _upIconRectF.Width ;
				}
				else
				{
					_hintRect.X += _upIconRectF.Width * 2;
				}
				if (IsNone)
				{
					_hintRect.X += (float)BaseWidth();
				}
				else
				{
					_hintRect.X -= (float)BaseWidth();
				}
			}

			if (IsRTL)
			{
				_hintRect.X = (float)(Width - _hintRect.X - _hintRect.Width);
			}
		}

		/// <summary>
		/// This method updates the starting point of the assistive text.
		/// </summary>
		void UpdateHelperTextPosition()
		{
			UpdateLeadViewWidthForContent();
			UpdateTrailViewWidthForBorder();
			var startPadding = IsNone ? 0 : StartX + DefaultAssistiveLabelPadding + (IsOutlined ? (BaseLineMaxHeight) : 0);
			_helperTextRect.X = (int)(startPadding + _leadViewWidth);
			if (BaseLineMaxHeight <= 2)
			{
				_helperTextRect.Y = (int)(Height - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
			}
			else
			{
				_helperTextRect.Y = (int)(Height - TotalAssistiveTextHeight());
			}

			_helperTextRect.Width = (int)(Width - startPadding - CounterTextPadding - DefaultAssistiveLabelPadding - ((ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - _trailViewWidth - _leadViewWidth);
			_helperTextRect.Height = HelperTextSize.Height;

			if (IsRTL)
			{
				_helperTextRect.X = (int)(Width - _helperTextRect.X - _helperTextRect.Width);
			}

		}

		double GetAssistiveTextWidth()
		{
			UpdateLeadViewWidthForContent();
			UpdateTrailViewWidthForBorder();

			if (Width >= 0)
			{
				return Width - (IsNone ? 0 : StartX + DefaultAssistiveLabelPadding) - CounterTextPadding - DefaultAssistiveLabelPadding - ((ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - _trailViewWidth - _leadViewWidth;
			}
			else if (WidthRequest != -1)
			{
				return WidthRequest - (IsNone ? 0 : StartX + DefaultAssistiveLabelPadding) - CounterTextPadding - DefaultAssistiveLabelPadding - ((ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - _trailViewWidth - _leadViewWidth;
			}
			else
			{
				return 1;
			}
		}


		double GetHintTextWidth()
		{
			UpdateLeadViewWidthForContent();
			if (IsHintFloated)
			{ UpdateTrailViewWidthForBorder(); }
			else
			{ UpdateTrailViewWidthForContent(); }


			if (Width >= 0)
			{
				return Width - (IsNone ? 0 : ((2 * StartX) + DefaultAssistiveLabelPadding)) - _trailViewWidth - _leadViewWidth;
			}
			else if (WidthRequest != -1)
			{
				return WidthRequest - (IsNone ? 0 : 2 * (StartX + DefaultAssistiveLabelPadding)) - _trailViewWidth - _leadViewWidth;
			}
			else
			{
				return 1;
			}
		}

		/// <summary>
		/// This method updates the starting point of the assistive text.
		/// </summary>
		void UpdateErrorTextPosition()
		{
			UpdateLeadViewWidthForContent();
			UpdateTrailViewWidthForBorder();
			var startPadding = IsNone ? 0 : StartX + DefaultAssistiveLabelPadding + (IsOutlined ? (BaseLineMaxHeight) : 0);
			_errorTextRect.X = (int)(startPadding + _leadViewWidth);
			if (BaseLineMaxHeight <= 2)
			{
				_errorTextRect.Y = (int)(Height - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
			}
			else
			{
				_errorTextRect.Y = (int)(Height - TotalAssistiveTextHeight());
			}

			_errorTextRect.Width = (int)(Width - startPadding - CounterTextPadding - DefaultAssistiveLabelPadding - ((ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - _trailViewWidth - _leadViewWidth);
			_errorTextRect.Height = ErrorTextSize.Height;

			if (IsRTL)
			{
				_errorTextRect.X = (int)(Width - _errorTextRect.X - _errorTextRect.Width);
			}

		}

		/// <summary>
		/// This method updates the starting point of the counter.
		/// </summary>
		void UpdateCounterTextPosition()
		{
			UpdateTrailViewWidthForBorder();
			_counterTextRect.X = (int)(Width - CounterTextSize.Width - CounterTextPadding - _trailViewWidth);
			_counterTextRect.Y = (int)(Height - CounterTextSize.Height);
			_counterTextRect.Width = (int)CounterTextSize.Width;
			_counterTextRect.Height = (int)CounterTextSize.Height;

			if (IsRTL)
			{
				_counterTextRect.X = (float)(Width - _counterTextRect.X - _counterTextRect.Width);
			}
		}

		/// <summary>
		/// This method updates the start point and end point of the base line in filled and none type container.
		/// </summary>
		void UpdateBaseLinePoints()
		{
			UpdateLeadViewWidthForBorder();
			_startPoint.X = (float)_leadViewWidth;
			_startPoint.Y = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
			UpdateTrailViewWidthForBorder();
			_endPoint.X = (float)(Width - _trailViewWidth);
			_endPoint.Y = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
		}

		/// <summary>
		/// This method updates the rectF of the border line in outlined type container.
		/// </summary>
		void UpdateOutlineRectF()
		{
			UpdateLeadViewWidthForBorder();
			if (BaseLineMaxHeight <= 2)
			{
				_outlineRectF.X = (float)(BaseLineMaxHeight + _leadViewWidth);
				_outlineRectF.Y = (float)((BaseLineMaxHeight > FloatedHintSize.Height / 2) ? BaseLineMaxHeight : FloatedHintSize.Height / 2);
			}
			else
			{
				_outlineRectF.X = (float)((BaseLineMaxHeight / 2) + _leadViewWidth);
				_outlineRectF.Y = (float)((BaseLineMaxHeight > FloatedHintSize.Height / 2) ? BaseLineMaxHeight / 2 : FloatedHintSize.Height / 2);
			}
			UpdateLeadViewWidthForBorder();
			UpdateTrailViewWidthForBorder();
			if (BaseLineMaxHeight <= 2)
			{
				_outlineRectF.Width = (float)((Width - (BaseLineMaxHeight * 2)) - _leadViewWidth - _trailViewWidth);
			}
			else
			{
				_outlineRectF.Width = (float)((Width - (BaseLineMaxHeight)) - _leadViewWidth - _trailViewWidth);
			}

			_outlineRectF.Height = (float)(Height - _outlineRectF.Y - TotalAssistiveTextHeight() - AssistiveLabelPadding);
		}

		void UpdateOutlineBackgroundRectF()
		{
			if (IsLayoutFocused)
			{
				_backgroundRectF.X = (float)(_outlineRectF.X + (FocusedStrokeThickness / 2));
				_backgroundRectF.Y = (float)(_outlineRectF.Y + (FocusedStrokeThickness / 2));
				_backgroundRectF.Width = (float)(_outlineRectF.Width - (FocusedStrokeThickness));
				_backgroundRectF.Height = (float)(_outlineRectF.Height - (FocusedStrokeThickness));
			}
			else
			{
				_backgroundRectF.X = (float)(_outlineRectF.X + (UnfocusedStrokeThickness / 2));
				_backgroundRectF.Y = (float)(_outlineRectF.Y + (UnfocusedStrokeThickness / 2));
				_backgroundRectF.Width = (float)(_outlineRectF.Width - (UnfocusedStrokeThickness));
				_backgroundRectF.Height = (float)(_outlineRectF.Height - (UnfocusedStrokeThickness));
			}

		}

		/// <summary>
		/// This method updates the rectF of the background color in filled type container.
		/// </summary>
		void UpdateBackgroundRectF()
		{
			UpdateLeadViewWidthForBorder();
			UpdateTrailViewWidthForBorder();
			_backgroundRectF.X = (float)_leadViewWidth;
			_backgroundRectF.Y = 0;
			_backgroundRectF.Width = (float)(Width - _leadViewWidth - _trailViewWidth);
			if (BaseLineMaxHeight <= 2)
			{
				_backgroundRectF.Height = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
			}
			else
			{
				_backgroundRectF.Height = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding - (IsLayoutFocused ? FocusedStrokeThickness / 2 : UnfocusedStrokeThickness / 2));
			}
		}

		/// <summary>
		/// Adjust the hint rect clip X position based on the spin button alignment
		/// </summary>
		/// <returns>returns spin button width</returns>
		double GetAdditionalWidth()
		{
			double tempWidth = 0;
			if (ShowUpDownButton)
			{
				if ((IsUpDownVerticalAlignment && IsUpDownAlignmentLeft) || (!IsUpDownVerticalAlignment && IsUpDownAlignmentBoth))
				{
					tempWidth = IconSize - BaseWidth();
				}
				else
				{
					if (IsUpDownAlignmentLeft && !IsUpDownVerticalAlignment)
					{
						tempWidth = (IconSize * 2) - BaseWidth();
					}
				}
			}
			return tempWidth;
		}

		/// <summary>
		/// Calculate the starting X point for hint rect
		/// </summary>
		/// <returns>returns the starting position</returns>
		double BaseWidth()
		{
			return BaseLineMaxHeight > 2 ? (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2) : (DefaultAssistiveLabelPadding * 2 + BaseLineMaxHeight);
		}

		/// <summary>
		/// This method updates the rectF of the floated hint text space in outlined type container.
		/// </summary>
		void CalculateClipRect()
		{
			if (!string.IsNullOrEmpty(Hint) && EnableFloating)
			{
				if (BaseLineMaxHeight <= 2)
				{
					_clipRect.X = _outlineRectF.X + StartX + (float) GetAdditionalWidth();
				}
				else
				{
					_clipRect.X = (float)(_outlineRectF.X + StartX + (BaseLineMaxHeight / 2)) + (float)GetAdditionalWidth();
				}

				_clipRect.Y = 0;
				_clipRect.Width = (float)Math.Min((FloatedHintSize.Width + DefaultAssistiveLabelPadding + DefaultAssistiveLabelPadding), GetHintTextWidth());
				if (BaseLineMaxHeight <= 2)
				{
					_clipRect.Height = FloatedHintSize.Height;
				}
				else
				{
					_clipRect.Height = (float)((BaseLineMaxHeight > FloatedHintSize.Height / 2) ? BaseLineMaxHeight : FloatedHintSize.Height);
				}

				if (ShowLeadingView && LeadingView != null && LeadingViewPosition == ViewPosition.Inside)
				{
					_clipRect.X = (float)(_clipRect.X + LeadingView.Width + _leadingViewLeftPadding);
				}
			}
			else
			{
				_clipRect = new Rect(0, 0, 0, 0);
			}
		}

		/// <summary>
		/// This method updates the downIconRectF and clearIconRectF.
		/// </summary>
		void UpdateIconRectF()
		{
			UpdateOutlineRectF();
			UpdateBackgroundRectF();
			UpdateLeadViewWidthForContent();
			UpdateTrailViewWidthForContent();
			UpdateDownIconRectF();
			UpdateUpIconRectF();
			UpdateClearIconRectF();
			UpdatePasswordToggleIconRectF();
			UpdateEffectsRendererBounds();
		}


		/// <summary>
		/// Configures the `_tilRenderer` field with a platform-specific `SfEntry` renderer:
		/// - `MaterialSfEntryRenderer` for Android with padding.
		/// - `FluentSfEntryRenderer` for Windows.
		/// - `CupertinoSfEntryRenderer` for other platforms.
		/// </summary>
		void SetRendererBasedOnPlatform()
		{
#if ANDROID
            _tilRenderer = new MaterialSfEntryRenderer() { _clearButtonPadding = 12};
#elif WINDOWS
            _tilRenderer = new FluentSfEntryRenderer();
#else
			_tilRenderer = new CupertinoSfEntryRenderer();
#endif
		}
		
		/// <summary>
		/// This method calculate the password toggle icon rectF position.
		/// </summary>
		void UpdatePasswordToggleIconRectF()
		{
			_passwordToggleIconRectF.X = (float)(Width - _trailViewWidth - (IsOutlined ? BaseLineMaxHeight * 2 : BaseLineMaxHeight) - IconSize - DefaultAssistiveLabelPadding);
			if (IsNone && Content != null)
			{
				_passwordToggleIconRectF.Y = (float)((Content.Y + (Content.Height / 2)) - (UpDownButtonSize / 2));
			}
			else if (IsOutlined)
			{
				_passwordToggleIconRectF.Y = ((_outlineRectF.Center.Y) - (UpDownButtonSize / 2));
			}
			else
			{
				_passwordToggleIconRectF.Y = (float)(((Height - AssistiveLabelPadding - TotalAssistiveTextHeight()) / 2) - (UpDownButtonSize / 2));
			}

			_passwordToggleIconRectF.Width = IsPassowordToggleIconVisible ? UpDownButtonSize : 0;
			_passwordToggleIconRectF.Height = IsPassowordToggleIconVisible ? UpDownButtonSize : 0;

			if (IsRTL)
			{
				_passwordToggleIconRectF.X = (float)(Width - _passwordToggleIconRectF.X - _passwordToggleIconRectF.Width);
			}

		}

		/// <summary>
		/// Updates the hint color of UI elements based on state.
		/// </summary>
		void UpdateHintColor()
		{
			if(HintLabelStyle != null)
			{
				if (!HintLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)))
				{
					_internalHintLabelStyle.TextColor = HintLabelStyle.TextColor;
				}
				else
				{
					if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out _))
					{
						_internalHintLabelStyle.TextColor = IsEnabled ? ((IsLayoutFocused || HasError) && Stroke != null) ? Stroke : HintLabelStyle.TextColor : DisabledColor;
					}
					else
					{
						if (IsEnabled)
						{
							if (IsLayoutFocused)
							{
								SetDynamicResource(HintTextColorProperty, "SfTextInputLayoutFocusedHintTextColor");
								_internalHintLabelStyle.TextColor = HintTextColor;
							}
							else
							{
								SetDynamicResource(HintTextColorProperty, "SfTextInputLayoutHintTextColor");
								_internalHintLabelStyle.TextColor = HintTextColor;
							}

						}
						else
						{
							SetDynamicResource(HintTextColorProperty, "SfTextInputLayoutDisabledHintTextColor");
							_internalHintLabelStyle.TextColor = HintTextColor;
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates the helper color of UI elements based on state.
		/// </summary>
		void UpdateHelperTextColor()
		{
			if(HelperLabelStyle != null)
			{
				if (!HelperLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)))
				{
					_internalHelperLabelStyle.TextColor = HelperLabelStyle.TextColor;
				}
				else
				{
					if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out _))
					{
						_internalHelperLabelStyle.TextColor = IsEnabled ? ((HasError) && Stroke != null) ? Stroke : HelperLabelStyle.TextColor : DisabledColor;
					}
					else
					{
						if (IsEnabled)
						{
							SetDynamicResource(HelperTextColorProperty, "SfTextInputLayoutHelperTextColor");
							_internalHelperLabelStyle.TextColor = HelperTextColor;
						}
						else
						{
							SetDynamicResource(HelperTextColorProperty, "SfTextInputLayoutDisabledHelperTextColor");
							_internalHelperLabelStyle.TextColor = HelperTextColor;
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates the error color of UI elements based on state.
		/// </summary>
		void UpdateErrorTextColor()
		{
			if(ErrorLabelStyle != null)
			{
				_internalErrorLabelStyle.TextColor = IsEnabled ? ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor : DisabledColor;
			}
		}

		/// <summary>
		/// Updates the counter color of UI elements based on state.
		/// </summary>
		void UpdateCounterTextColor()
		{
			if(ErrorLabelStyle != null)
			{
				_internalCounterLabelStyle.TextColor = IsEnabled ? HasError ? ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor : CounterLabelStyle.TextColor : DisabledColor;
			}
		}

		/// <summary>
		/// Check the stroke value is set or not.
		/// </summary>
		bool HasStrokeValue()
		{
			return Stroke is not null && Stroke.Equals(_defaultStrokeColor);
		}

		/// <summary>
		/// Check the container background value is set or not.
		/// </summary>
		bool HasContainerBackgroundValue()
		{
			return ContainerBackground is not null && ContainerBackground.Equals(_defaultContainerBackground);
		}

		void DrawBorder(ICanvas canvas, RectF dirtyRect)
		{
			canvas.CanvasSaveState();
			if (IsRTL)
			{
				canvas.Translate((float)Width, 0);
				canvas.Scale(-1, 1);
			}
			canvas.StrokeSize = (float)(IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness);

			SetStrokeColor(canvas);

			if (!IsOutlined)
			{
				DrawFilledOrNoneBorder(canvas);
			}
			else
			{
				DrawOutlinedBorder(canvas);
			}

			canvas.CanvasRestoreState();
		}

		void SetStrokeColor(ICanvas canvas)
		{
			if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out _) && !VisualStateManager.HasVisualStateGroups(this))
			{
				canvas.StrokeColor = IsEnabled ? Stroke : DisabledColor;
			}
			else
			{
				canvas.StrokeColor = Stroke;
			}
		}

		void DrawFilledOrNoneBorder(ICanvas canvas)
		{
			UpdateBaseLinePoints();
			if (IsFilled)
			{
				canvas.CanvasSaveState();
				if (ContainerBackground is SolidColorBrush backgroundColor)
				{
					canvas.FillColor = backgroundColor.Color;
				}

				UpdateBackgroundRectF();
				canvas.FillRoundedRectangle(_backgroundRectF, OutlineCornerRadius, OutlineCornerRadius, 0, 0);
				canvas.CanvasRestoreState();
			}

			if ((IsLayoutFocused && FocusedStrokeThickness > 0) || (!IsLayoutFocused && UnfocusedStrokeThickness > 0))
			{
				canvas.DrawLine(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
			}
		}

		/// <summary>
		/// Draws the clear button path on the canvas.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="clearButtonPath">The path of the clear button to draw.</param>
		void DrawClearButtonPath(ICanvas canvas, Microsoft.Maui.Controls.Shapes.Path clearButtonPath)
		{
			PathF path = clearButtonPath.GetPath();
			canvas.FillColor = clearButtonPath.Fill is SolidColorBrush solidColorBrushFill ? solidColorBrushFill.Color : Colors.Transparent;
			canvas.StrokeColor = clearButtonPath.Stroke is SolidColorBrush solidColorBrushStroke ? solidColorBrushStroke.Color : ClearIconStrokeColor;
			canvas.ClipRectangle(UpdatePathRect());
			canvas.Translate(_clearIconRectF.Center.X - path.Bounds.Center.X, _clearIconRectF.Center.Y - path.Bounds.Center.Y);
			canvas.FillPath(path, WindingMode.EvenOdd);
			canvas.DrawPath(path);
		}

		/// <summary>
		/// Draws the clear icon on the canvas if it's visible.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The rectangle that needs to be redrawn.</param>
		void DrawClearIcon(ICanvas canvas, RectF dirtyRect)
		{
			if (IsClearIconVisible)
			{
				canvas.SaveState();
				if (ClearButtonPath != null)
				{
					DrawClearButtonPath(canvas, ClearButtonPath);
				}
				else
				{
					canvas.StrokeColor = ClearButtonColor;
					canvas.StrokeSize = 1.5f;
					_tilRenderer?.DrawClearButton(canvas, _clearIconRectF);
				}
				canvas.RestoreState();
			}
		}

		void DrawOutlinedBorder(ICanvas canvas)
		{
			UpdateOutlineRectF();
			if (BaseLineMaxHeight > 2)
			{
				UpdateOutlineBackgroundRectF();
			}

			if (((IsLayoutFocused && !string.IsNullOrEmpty(Hint)) || IsHintFloated) && ShowHint)
			{
				CalculateClipRect();
				if (_clipRect.Width >= 0 && _clipRect.Height >= 0)
				{
					canvas.SubtractFromClip(_clipRect);
				}
			}

			SetOutlinedContainerBackground(canvas);

			if (OutlineCornerRadius != 0)
			{
				DrawRoundedOutlinedBorder(canvas);
			}
			else
			{
				canvas.FillRectangle(_backgroundRectF);
				if ((IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness) > 0)
				{
					canvas.DrawRectangle(_outlineRectF);
				}
			}
		}

		void SetOutlinedContainerBackground(ICanvas canvas)
		{
			if (_outlinedContainerBackground is SolidColorBrush backgroundColor)
			{
				canvas.FillColor = backgroundColor.Color;
			}
		}

		void DrawRoundedOutlinedBorder(ICanvas canvas)
		{
			if (BaseLineMaxHeight <= 2)
			{
				canvas.FillRoundedRectangle(_outlineRectF, OutlineCornerRadius);
			}
			else
			{
				var cornerRadius = ((IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness) / 2) < OutlineCornerRadius ? OutlineCornerRadius - ((IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness) / 2) : 0;
				canvas.FillRoundedRectangle(_backgroundRectF, cornerRadius);
			}
			if ((IsLayoutFocused ? FocusedStrokeThickness : UnfocusedStrokeThickness) > 0)
			{
				canvas.DrawRoundedRectangle(_outlineRectF, OutlineCornerRadius);
			}
		}

		void DrawHintText(ICanvas canvas, RectF dirtyRect)
		{
			if ((IsHintFloated && !EnableFloating) || (!IsHintFloated && !string.IsNullOrEmpty(Text)))
			{
				return;
			}

			if (ShowHint && !string.IsNullOrEmpty(Hint) && HintLabelStyle != null)
			{
				canvas.CanvasSaveState();
				UpdateOutlineRectF();
				UpdateHintPosition();
				UpdateHintColor();

				_internalHintLabelStyle.FontSize = _isAnimating ? (float)_animatingFontSize : IsHintFloated ? FloatedHintFontSize : HintLabelStyle.FontSize;

				HorizontalAlignment horizontalAlignment = IsRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left;
#if IOS || MACCATALYST
				VerticalAlignment verticalAlignment = VerticalAlignment.Top;
#else
				VerticalAlignment verticalAlignment = VerticalAlignment.Center;
#endif

				canvas.DrawText(Hint, _hintRect, horizontalAlignment, verticalAlignment, _internalHintLabelStyle);
				canvas.CanvasRestoreState();
			}
		}

		void DrawAssistiveText(ICanvas canvas, RectF dirtyRect)
		{
			if (HasError)
			{
				DrawErrorText(canvas, dirtyRect);
			}
			else
			{
				DrawHelperText(canvas, dirtyRect);
			}

			DrawCounterText(canvas, dirtyRect);
		}

		void DrawHelperText(ICanvas canvas, RectF dirtyRect)
		{
			if (ShowHelperText && !string.IsNullOrEmpty(HelperText) && HelperLabelStyle != null)
			{
				canvas.CanvasSaveState();
				UpdateHelperTextPosition();
				UpdateHelperTextColor();

				canvas.DrawText(HelperText, _helperTextRect, IsRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, _internalHelperLabelStyle);

				canvas.CanvasRestoreState();
			}
		}

		void DrawErrorText(ICanvas canvas, RectF dirtyRect)
		{
			if (!string.IsNullOrEmpty(ErrorText) && ErrorLabelStyle != null)
			{
				canvas.CanvasSaveState();
				UpdateErrorTextPosition();
				UpdateErrorTextColor();

				canvas.DrawText(ErrorText, _errorTextRect, IsRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, _internalErrorLabelStyle);

				canvas.CanvasRestoreState();
			}
		}

		void DrawCounterText(ICanvas canvas, RectF dirtyRect)
		{
			if (ShowCharCount && !string.IsNullOrEmpty(_counterText) && CounterLabelStyle != null)
			{
				canvas.CanvasSaveState();
				UpdateCounterTextPosition();
				UpdateCounterTextColor();

				canvas.DrawText(_counterText, _counterTextRect, IsRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, _internalCounterLabelStyle);

				canvas.CanvasRestoreState();
			}
		}

		/// <summary>
		/// Updates the clipping bound of the clear button path.
		/// </summary>
		/// <returns>The RectF</returns>
		RectF UpdatePathRect()
		{
			RectF clipSize = new()
			{
				Width = _clearIconRectF.Width / 2,
				Height = _clearIconRectF.Height / 2,
			};
			clipSize.X = _clearIconRectF.Center.X - clipSize.Width / 2;
			clipSize.Y = _clearIconRectF.Center.Y - clipSize.Height / 2;

			return clipSize;
		}

		void DrawPasswordToggleIcon(ICanvas canvas, RectF dirtyRect)
		{
			if (IsPassowordToggleIconVisible)
			{
				canvas.CanvasSaveState();

				if (!Application.Current!.Resources.TryGetValue("SfTextInputLayoutTheme", out _) && !VisualStateManager.HasVisualStateGroups(this))
				{
					canvas.FillColor = IsEnabled ? Stroke : DisabledColor;
				}
				else
				{
					canvas.FillColor = Stroke;
				}

				canvas.Translate(_passwordToggleIconRectF.X + _passwordToggleIconEdgePadding, _passwordToggleIconRectF.Y + (_isPasswordTextVisible ? _passwordToggleVisibleTopPadding : _passwordToggleCollapsedTopPadding));

				// Using Path data value for toggle icon it seems smaller in Android platform when compared to Xamarin.Forms UI
				// so fix the above issue by scaling the canvas.
				if (DeviceInfo.Platform == DevicePlatform.Android)
				{
					canvas.Scale(1.2f, 1.2f);
				}

				canvas.FillPath(ToggleIconPath);
				canvas.CanvasRestoreState();
			}
		}

		void UpdateSizeStartAndEndValue()
		{
			_fontSizeStart = IsHintFloated ? FloatedHintFontSize : HintFontSize;
			_fontSizeEnd = IsHintFloated ? HintFontSize : FloatedHintFontSize;
		}

		void UpdateStartAndEndValue()
		{
			if (IsNone && (Width > 0 && Height > 0))
			{
				if (IsHintFloated)
				{
					_translateStart = DefaultAssistiveLabelPadding;
					_translateEnd = Height - HintSize.Height - BaseLineMaxHeight - (DefaultAssistiveLabelPadding * 2) - AssistiveLabelPadding - TotalAssistiveTextHeight() - DefaultAssistiveLabelPadding;
				}
				else
				{
					_translateStart = Height - HintSize.Height - BaseLineMaxHeight - (DefaultAssistiveLabelPadding * 2) - AssistiveLabelPadding - TotalAssistiveTextHeight() - DefaultAssistiveLabelPadding;
					_translateEnd = DefaultAssistiveLabelPadding;
				}
			}

			if (IsOutlined && (Width > 0 && Height > 0))
			{
				if (IsHintFloated)
				{
					_translateStart = 0;
					_translateEnd = _outlineRectF.Center.Y - (HintSize.Height / 2);
				}
				else
				{
					_translateStart = _outlineRectF.Center.Y - (HintSize.Height / 2);
					_translateEnd = 0;
				}
			}

			if (IsFilled && (Width > 0 && Height > 0))
			{
				if (IsHintFloated)
				{
					_translateStart = (float)((DefaultAssistiveLabelPadding * 2) + (DefaultAssistiveLabelPadding / 2));
					_translateEnd = ((Height - TotalAssistiveTextHeight() - AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
				}
				else
				{
					_translateStart = ((Height - TotalAssistiveTextHeight() - AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
					_translateEnd = (float)((DefaultAssistiveLabelPadding * 2) + (DefaultAssistiveLabelPadding / 2));
				}
			}
		}

		void OnScalingAnimationUpdate(double value)
		{
			_fontsize = (float)value;
		}

		void OnTranslateAnimationEnded(double value, bool isCompleted)
		{
			_isAnimating = false;
			IsHintDownToUp = !IsHintDownToUp;
		}

		/// <summary>
		/// Returns the resource dictionary for the current theme of the parent element.
		/// </summary>   
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfTextInputLayoutStyles();
		}

		/// <summary>
		/// Handles changes in the theme for individual controls.
		/// </summary>
		/// <param name="oldTheme"></param>
		/// <param name="newTheme"></param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		/// <summary>
		/// Handles changes in the common theme shared across multiple elements.
		/// </summary>
		/// <param name="oldTheme"></param>
		/// <param name="newTheme"></param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		#endregion

	}
}
