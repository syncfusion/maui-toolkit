using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;

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
            if (DeviceInfo.Platform == DevicePlatform.WinUI && !(Content is Editor))
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
            if (DeviceInfo.Platform == DevicePlatform.WinUI && !(Content is Editor))
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

                // Clear icon can't draw when isClearIconVisible property updated based on text.
                // So here call the InvalidateDrawable to draw the clear icon.
                if (Text.Length == 1 || Text.Length == 0)
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

            if (string.IsNullOrEmpty(this.Text) && !IsLayoutFocused && !EnableFloating)
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

            // Text is disapper when the rect size is not compatible with text size, so here calculate the rect again.
            if (!IsHintFloated)
            {
                UpdateHintPosition();
            }

            _fontsize = (float)_fontSizeStart;
            var scalingAnimation = new Animation(OnScalingAnimationUpdate, _fontSizeStart, _fontSizeEnd, Easing.Linear);

            var translateAnimation = new Animation(OnTranslateAnimationUpdate, _translateStart, _translateEnd, Easing.SinIn);

            translateAnimation.WithConcurrent(scalingAnimation, 0, 1);

            translateAnimation.Commit(this, "showAnimator", rate: 7, length: (uint)DefaultAnimationDuration, finished: OnTranslateAnimationEnded, repeat: () => false);

        }

        #endregion

        #region Private Methods
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

        void OnTextInputLayoutUnloaded(object? sender, EventArgs e)
        {
            UnwireEvents();
        }

        void OnTextInputLayoutLoaded(object? sender, EventArgs e)
        {
            WireEvents();
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
                internalLabelStyle.FontAttributes = labelStyle.FontAttributes;
            if (propertyName == nameof(LabelStyle.TextColor))
                internalLabelStyle.TextColor = labelStyle.TextColor;
            if (propertyName == nameof(LabelStyle.FontSize))
                internalLabelStyle.FontSize = labelStyle.FontSize;
            if (propertyName == nameof(LabelStyle.FontFamily))
                internalLabelStyle.FontFamily = labelStyle.FontFamily;
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

        void SetupAndroidView(object? sender)
        {
#if ANDROID
            if (sender is View view && view.Handler != null && view.Handler.PlatformView is Android.Views.View androidView)
            {
                androidView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                androidView.SetPadding(0, 0, 0, 0);
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
            else if (sender is Picker picker && picker.Handler != null && picker.Handler.PlatformView is Microsoft.UI.Xaml.Controls.ComboBox windowPicker)
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
            iOSEntry.ShouldEndEditing = (iOSEntry) =>
            {
				return !IsIconPressed;
            };
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
            VisualStateGroupList visualStateGroupList = new VisualStateGroupList() { };

            VisualStateGroup visualStateGroup = new VisualStateGroup();

            visualStateGroup.Name = "CommonStates";

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
                    case Picker picker:
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
                }

#if MACCATALYST || IOS
				if (Content is View view && view.Handler != null && view.Handler.PlatformView is UIKit.UITextField iOSEntry)
				{
					// Setting ShouldEndEditing to null to prevent memory leaks
					iOSEntry.ShouldEndEditing = null;
				}
#endif
			}

            UnWireLabelStyleEvents();
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
                    case Picker picker:
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
                _viewBounds.Y = 0;
                _viewBounds.Width = (int)(Width - _leadViewWidth - _trailViewWidth);
                _viewBounds.Height = (int)Height;

                if (EnablePasswordVisibilityToggle)
                {
                    _viewBounds.Width -= (float)(EnablePasswordVisibilityToggle ? ((IconSize * 2) - RightPadding + DefaultAssistiveLabelPadding + 7) : (IconSize - RightPadding + DefaultAssistiveLabelPadding) + 3);
                }

                if (_viewBounds.Height >= 0 || _viewBounds.Width >= 0)
                    AbsoluteLayout.SetLayoutBounds(Content, _viewBounds);
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
                    AbsoluteLayout.SetLayoutBounds(LeadingView, _viewBounds);
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
                    AbsoluteLayout.SetLayoutBounds(TrailingView, _viewBounds);
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
                    _effectsRenderer.RippleBoundsCollection.Add(_passwordToggleIconRectF);
                    _effectsRenderer.HighlightBoundsCollection.Add(_passwordToggleIconRectF);
                }
            }
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
                    _hintRect.X = (int)(StartX + DefaultAssistiveLabelPadding + (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2));
                _hintRect.Y = 0;
                _hintRect.Width = (float)GetHintTextWidth();
                _hintRect.Height = (int)FloatedHintSize.Height;
            }
            else
            {
                _hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding + BaseLineMaxHeight);
                if (BaseLineMaxHeight > 2)
                    _hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding + (BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2));
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
                _helperTextRect.Y = (int)(Height - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
            else
                _helperTextRect.Y = (int)(Height - TotalAssistiveTextHeight());
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
            if (IsHintFloated) { UpdateTrailViewWidthForBorder(); } else { UpdateTrailViewWidthForContent(); }


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
                _errorTextRect.Y = (int)(Height - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
            else
                _errorTextRect.Y = (int)(Height - TotalAssistiveTextHeight());
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
                _backgroundRectF.Height = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
            else
                _backgroundRectF.Height = (float)(Height - TotalAssistiveTextHeight() - AssistiveLabelPadding - (IsLayoutFocused ? FocusedStrokeThickness / 2 : UnfocusedStrokeThickness / 2));
        }

        /// <summary>
        /// This method updates the rectF of the floated hint text space in outlined type container.
        /// </summary>
        void CalculateClipRect()
        {
            if (!string.IsNullOrEmpty(Hint) && EnableFloating)
            {
                if (BaseLineMaxHeight <= 2)
                    _clipRect.X = (float)(_outlineRectF.X + StartX);
                else
                    _clipRect.X = (float)(_outlineRectF.X + StartX + (BaseLineMaxHeight / 2));
                _clipRect.Y = 0;
                _clipRect.Width = (float)Math.Min((FloatedHintSize.Width + DefaultAssistiveLabelPadding + DefaultAssistiveLabelPadding), GetHintTextWidth());
                if (BaseLineMaxHeight <= 2)
                    _clipRect.Height = FloatedHintSize.Height;
                else
                    _clipRect.Height = (float)((BaseLineMaxHeight > FloatedHintSize.Height / 2) ? BaseLineMaxHeight : FloatedHintSize.Height);
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
            UpdateTrailViewWidthForContent();
            UpdatePasswordToggleIconRectF();
            UpdateEffectsRendererBounds();
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
                _passwordToggleIconRectF.Y = (float)((_outlineRectF.Center.Y) - (UpDownButtonSize / 2));
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
            if (!HintLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)))
            {
                _internalHintLabelStyle.TextColor = HintLabelStyle.TextColor;
            }
            else
            {
                if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme))
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

        /// <summary>
        /// Updates the helper color of UI elements based on state.
        /// </summary>
        void UpdateHelperTextColor()
        {
            if (!HelperLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)))
            {
                _internalHelperLabelStyle.TextColor = HelperLabelStyle.TextColor;
            }
            else
            {
                if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme))
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

        /// <summary>
        /// Updates the error color of UI elements based on state.
        /// </summary>
        void UpdateErrorTextColor()
        {
            _internalErrorLabelStyle.TextColor = IsEnabled ? ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor : DisabledColor;
        }

        /// <summary>
        /// Updates the counter color of UI elements based on state.
        /// </summary>
        void UpdateCounterTextColor()
        {
            _internalCounterLabelStyle.TextColor = IsEnabled ? HasError ? ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? Stroke : ErrorLabelStyle.TextColor : CounterLabelStyle.TextColor : DisabledColor;
        }

        /// <summary>
        /// Check the stroke value is set or not.
        /// </summary>
        bool HasStrokeValue()
        {
            return Stroke != null && !Stroke.Equals(_defaultStrokeColor);
        }

        /// <summary>
        /// Check the container background value is set or not.
        /// </summary>
        bool HasContainerBackgroundValue()
        {
            if (ContainerBackground is not SolidColorBrush)
            {
                return true;
            }

            if (ContainerBackground is SolidColorBrush solidColorBrush && !solidColorBrush.Color.Equals(_defaultContainerBackground))
            {
                return true;
            }

            return false;
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
            if (Application.Current != null && !Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme1) && !VisualStateManager.HasVisualStateGroups(this))
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
                    canvas.FillColor = backgroundColor.Color;
                UpdateBackgroundRectF();
                canvas.FillRoundedRectangle(_backgroundRectF, OutlineCornerRadius, OutlineCornerRadius, 0, 0);
                canvas.CanvasRestoreState();
            }

            if ((IsLayoutFocused && FocusedStrokeThickness > 0) || (!IsLayoutFocused && UnfocusedStrokeThickness > 0))
            {
                canvas.DrawLine(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
            }
        }

        void DrawOutlinedBorder(ICanvas canvas)
        {
            UpdateOutlineRectF();
            if (BaseLineMaxHeight > 2)
                UpdateOutlineBackgroundRectF();

            if (((IsLayoutFocused && !string.IsNullOrEmpty(Hint)) || IsHintFloated) && ShowHint)
            {
                CalculateClipRect();
                canvas.SubtractFromClip(_clipRect);
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
            if (Application.Current != null && Application.Current.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme))
            {
                SetDynamicResource(OutlinedContainerBackgroundProperty, "SfTextInputLayoutOutlinedContainerBackground");
                if (OutlinedContainerBackground is SolidColorBrush backgroundColor)
                {
                    canvas.FillColor = backgroundColor.Color;
                }
            }
            else
            {
                if (ContainerBackground is SolidColorBrush backgroundColor)
                {
                    canvas.FillColor = backgroundColor.Color;
                }
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

                if (!Application.Current!.Resources.TryGetValue("SfTextInputLayoutTheme", out var theme) && !VisualStateManager.HasVisualStateGroups(this))
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

        void OnTranslateAnimationUpdate(double value)
        {
            _isAnimating = true;
            _hintRect.Y = (float)value;
            _animatingFontSize = (float)_fontsize;
            InvalidateDrawable();
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
