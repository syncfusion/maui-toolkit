using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Microsoft.Maui.Controls.Shapes;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Chips;

namespace Syncfusion.Maui.Toolkit.Buttons
{
	public partial class SfButton
	{
		#region Private Methods

		/// <summary>
		/// Initializes the elements of button control.
		/// </summary>
		void InitializeElement()
		{
			ThemeElement.InitializeThemeResources(this, "SfButtonTheme");
			base.Background = Background;
			this.AddTouchListener(this);
			InitializeEffectsView();
			HookEvents();
			UpdateButtonClip();
		}

		/// <summary>
		/// Initializes the effects view for the button, setting up visual effects and styling.
		/// </summary>
		void InitializeEffectsView()
		{
			_effectsView = [];
			_effectsView.Style = new Style(typeof(SfEffectsView));
			//By setting it to true, we restrict user gestures, allowing visual effects to be triggered programmatically based on custom logic instead.
			_effectsView.ShouldIgnoreTouches = true;
			_effectsView.ClipToBounds = true;
			Children.Add(_effectsView);
		}

		/// <summary>
		/// Transforms the input text based on the specified TextTransform value.
		/// </summary>
		/// <param name="text"></param>
		/// <returns>The transformed text</returns>
		string ApplyTextTransform(string text)
		{
			return TextTransform switch
			{
				TextTransform.Uppercase => text.ToUpper(System.Globalization.CultureInfo.CurrentCulture),
				TextTransform.Lowercase => text.ToLower(System.Globalization.CultureInfo.CurrentCulture),
				_ => text,
			};
		}

		/// <summary>
		/// Updates the binding context of the custom view to match the button's binding context.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SfButton_BindingContextChanged(object? sender, System.EventArgs e)
		{
			if (_customView != null)
			{
				_customView.BindingContext = BindingContext;
			}
		}

		/// <summary>
		/// Updates the button's background
		/// </summary>
		void UpdateBackground()
		{
			base.Background = Background;
		}

		/// <summary>
		/// Updates the button's corner radius
		/// </summary>
		void UpdateCornerRadius()
		{
			base.CornerRadius = CornerRadius;
		}

		/// <summary>
		/// Updates the button's stroke thickness
		/// </summary>
		void UpdateStrokeThickness()
		{
			base.StrokeThickness = StrokeThickness;
		}

		/// <summary>
		/// Positions the icon image within the button based on specified alignment and size settings.
		/// </summary>
		void UpdateImageIcon()
		{
			if (_imageViewGrid != null && ShowIcon)
			{
				if (Children.Contains(_imageViewGrid))
				{
					Children.Remove(_imageViewGrid);
				}

				if (IsItemTemplate)
				{
					return;
				}
				switch (ImageAlignment)
				{
					case Alignment.Left:
					case Alignment.Start:
						ComputeIconRectForLeftAlignment();
						break;

					case Alignment.Right:
					case Alignment.End:
						ComputeIconRectForRightAlignment();
						break;

					case Alignment.Top:
						ComputeIconRectForTopAlignment();
						break;

					case Alignment.Default:
						ComputeIconRectForDefaultAlignment();
						break;

					default:
						ComputeIconRectForBottomAlignment();
						break;
				}

				_iconRectF.Width = (float)ImageSize;
				_iconRectF.Height = (float)ImageSize;

				Children.Add(_imageViewGrid);
				AbsoluteLayout.SetLayoutBounds(_imageViewGrid, _iconRectF);
			}
		}

		/// <summary>
		/// Computes the rectangular position of the icon when aligned to the left of the button.
		/// </summary>
		void ComputeIconRectForLeftAlignment()
		{
			_iconRectF.X = (ImageAlignment == Alignment.Left && _isRightToLeft)
									? (float)(Width - Padding.Right - ImageSize - (_rightIconPadding * 2) - (StrokeThickness / 2))
									: (float)(Padding.Left + (_leftIconPadding * 2) + (StrokeThickness / 2));

			_iconRectF.Y = (float)(Height - (Height / 2) - (ImageSize / 2));
		}

		/// <summary>
		/// Computes the rectangular position of the icon when aligned to the right of the button.
		/// </summary>
		void ComputeIconRectForRightAlignment()
		{
			_iconRectF.X = (ImageAlignment == Alignment.Right && _isRightToLeft)
						? (float)(Padding.Left + (_leftIconPadding * 2) + (StrokeThickness / 2))
						: (float)(Width - Padding.Right - ImageSize - (_rightIconPadding * 2) - (StrokeThickness / 2));

			_iconRectF.Y = (float)(Height - (Height / 2) - (ImageSize / 2));
		}

		/// <summary>
		/// Computes the rectangular position of the icon when aligned to the top of the button.
		/// </summary>
		void ComputeIconRectForTopAlignment()
		{
			_iconRectF.X = (float)(Width - Width / 2 - ImageSize / 2);
			_iconRectF.Y = (float)(Padding.Top + (StrokeThickness / 2) + _leftPadding);
		}

		/// <summary>
		/// Computes the rectangular position of the icon for the default alignment.
		/// </summary>
		void ComputeIconRectForDefaultAlignment()
		{
			float totalCenterWidth = (float)(ImageSize + _textAreaPadding + TextSize.Width);
			_iconRectF.X = (TextSize.Width != 0) ? ((float)((Width / 2) - Padding.Right - (totalCenterWidth / 2) - (StrokeThickness / 2))) : ((float)((Width / 2) - (ImageSize / 2)));
			if (_iconRectF.X < 0 && _iconRectF.X < StrokeThickness)
			{
				_iconRectF.X = (float)(Padding.Left + (StrokeThickness / 2));
			}
			_iconRectF.Y = (float)(Height - (Height / 2) - (ImageSize / 2));
		}

		/// <summary>
		/// Computes the rectangular position of the icon when aligned to the bottom of the button.
		/// </summary>
		void ComputeIconRectForBottomAlignment()
		{
			_iconRectF.X = (float)(Width - (Width / 2) - (ImageSize / 2));
			_iconRectF.Y = (float)(Height - Padding.Top - ImageSize - _rightIconPadding - (StrokeThickness / 2));
		}

		/// <summary>
		/// Updates the bounds of the text within the view based on the provided dirty rectangle.
		/// </summary>
		/// <param name="dirtyRect">The area to be updated on the canvas (dirty rectangle).</param>
		void UpdateTextRect(RectF dirtyRect)
		{
			if (ShowIcon && ImageSource != null)
			{
				switch (ImageAlignment)
				{
					case Alignment.Start:
					case Alignment.Left:
						ComputeTextRectForLeftAlignment();
						break;
					case Alignment.End:
					case Alignment.Right:
						ComputeTextRectForRightAlignment();
						break;
					case Alignment.Top:
						ComputeTextRectForTopAlignment();
						break;
					case Alignment.Bottom:
						ComputeTextRectForBottomAlignment();
						break;
					default:
						ComputeTextRectForDefaultAlignment();
						break;
				}
			}
			else
			{
				ComputeTextRectWithoutIcon();
			}
#if ANDROID
			AdjustVerticalAlignment();
#endif
		}

		/// <summary>
		///  Computes the rectangular bounds for the text when the icon is aligned to the left of the button.
		/// </summary>
		void ComputeTextRectForLeftAlignment()
		{
			_textRectF.X = (ImageAlignment == Alignment.Start && _isRightToLeft) ? (float)(Padding.Left + _textAreaPadding + (StrokeThickness / 2))
										: (float)(Padding.Left + ImageSize + (StrokeThickness / 2) + (_leftIconPadding * 2) + _textAreaPadding);
			_textRectF.Width = (ImageAlignment == Alignment.Start && _isRightToLeft) ? Math.Abs((float)(Width - _textRectF.X - ImageSize - (_leftIconPadding * 2) - _textAreaPadding - (StrokeThickness / 2) - Padding.Right))
										: Math.Abs((float)(Width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right));
			_textRectF.Y = (float)(Padding.Top + _textAreaPadding + (StrokeThickness / 2));
			_textRectF.Height = Math.Abs((float)(Height - _textRectF.Y - Padding.Bottom - _textAreaPadding - StrokeThickness / 2));
		}

		/// <summary>
		/// Computes the rectangular bounds for the text when the icon is aligned to the right of the button.
		/// </summary>
		void ComputeTextRectForRightAlignment()
		{
			_textRectF.X = (ImageAlignment == Alignment.End && _isRightToLeft) ? (float)(Padding.Left + ImageSize + StrokeThickness / 2 + _leftIconPadding * 2 + _textAreaPadding)
										: (float)(Padding.Left + _textAreaPadding + StrokeThickness / 2);
			_textRectF.Width = (ImageAlignment == Alignment.End && _isRightToLeft) ? Math.Abs((float)(Width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right))
										: Math.Abs((float)(Width - _textRectF.X - ImageSize - _leftIconPadding - _leftIconPadding - _textAreaPadding - (StrokeThickness / 2) - Padding.Right));
			_textRectF.Y = (float)(Padding.Top + _textAreaPadding + StrokeThickness / 2);
			_textRectF.Height = Math.Abs((float)(Height - _textRectF.Y - Padding.Bottom - _textAreaPadding - StrokeThickness / 2));
		}

		/// <summary>
		/// Computes the rectangular bounds for the text when the icon is aligned above the text (top alignment).
		/// </summary>
		void ComputeTextRectForTopAlignment()
		{
			_textRectF.X = (float)(Padding.Left + _textAreaPadding + (StrokeThickness / 2));
			_textRectF.Y = (float)(Padding.Top + ImageSize + _leftPadding + (StrokeThickness / 2) + _rightIconPadding);
			_textRectF.Width = Math.Abs((float)(Width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - (StrokeThickness / 2) - (StrokeThickness / 2)));
			_textRectF.Height = Math.Abs((float)(Height - _textRectF.Y - Padding.Bottom - (StrokeThickness / 2) - _textAreaPadding));
		}

		/// <summary>
		/// Computes the rectangular bounds for the text when the icon is aligned below the text (bottom alignment).
		/// </summary>
		void ComputeTextRectForBottomAlignment()
		{
			_textRectF.X = (float)(Padding.Left + _textAreaPadding + (StrokeThickness / 2));
			_textRectF.Y = (float)(Padding.Top + (StrokeThickness / 2) + _textAreaPadding);
			_textRectF.Width = Math.Abs((float)(Width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - (StrokeThickness / 2) - (StrokeThickness / 2)));
			_textRectF.Height = Math.Abs((float)(Height - _textRectF.Y - Padding.Bottom - (StrokeThickness / 2) - _textAreaPadding - ImageSize - _leftIconPadding));
		}

		/// <summary>
		/// Computes the rectangular bounds for the text when the icon is centered and aligned with the text (default alignment).
		/// </summary>
		void ComputeTextRectForDefaultAlignment()
		{
			_textRectF.X = (_iconRectF.X > 0 && _iconRectF.X > StrokeThickness) ? (float)(_iconRectF.X + _textAreaPadding + ImageSize) : (float)(Padding.Left + ImageSize + (StrokeThickness / 2) + _textAreaPadding);
			_textRectF.Y = (float)(Padding.Top + _textAreaPadding + (StrokeThickness / 2));
			_textRectF.Width = Math.Abs((float)(Width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right));
			_textRectF.Height = Math.Abs((float)(Height - _textRectF.Y - Padding.Bottom - _textAreaPadding - (StrokeThickness / 2)));
		}

		/// <summary>
		/// Computes the rectangular bounds for the text when there is no icon.
		/// </summary>
		void ComputeTextRectWithoutIcon()
		{
			_textRectF.X = (float)(Padding.Left + (StrokeThickness / 2) + _textAreaPadding);
			_textRectF.Y = (float)(Padding.Top + (StrokeThickness / 2) + _textAreaPadding);
			_textRectF.Width = Math.Abs((float)(Width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - StrokeThickness));
			_textRectF.Height = Math.Abs((float)(Height - Padding.Top - Padding.Bottom - (StrokeThickness) - (_textAreaPadding * 2)));
		}

#if ANDROID
		/// <summary>
		/// Adjusts the vertical alignment of the text, ensuring proper placement for platform-specific requirements.
		/// </summary>
		void AdjustVerticalAlignment()
		{
			// Vertical Text Alignment workaround specific to certain platforms
			if (VerticalTextAlignment == TextAlignment.End)
			{
				_textRectF.Y = (float)((Height - TextSize.Height) > 0
							? Height - TextSize.Height - (StrokeThickness / 2) - Padding.Bottom
							: Padding.Top + (StrokeThickness / 2));
			}
		}
#endif

		/// <summary>
		/// Updates the button's view content by adding a new view and removing the old view if present.
		/// </summary>
		/// <param name="oldValue">New View.</param>
		/// <param name="newValue">Old View.</param>
		void AddButtonContent(object oldValue, object newValue)
		{
			if (_customView != null && Children.Contains(_customView))
			{
				Children.Remove(_customView);
				_customView = null;
			}
			if (newValue is DataTemplate dataTemplate)
			{
				InsertCustomViewInChildren(dataTemplate);
			}
			else
			{
				IsItemTemplate = false;
			}
		}

		/// <summary>
		/// Inserts a custom view generated from the specified <see cref="DataTemplate"/> into the button's child views.
		/// </summary>
		/// <param name="dataTemplate"></param>
		void InsertCustomViewInChildren(DataTemplate dataTemplate)
		{			
			var layout = dataTemplate.CreateContent();
#if NET10_0_OR_GREATER
			_customView = layout as View;
#else
			_customView = layout is ViewCell cell ? cell.View : (View)layout;
#endif
			if (_customView != null)
			{
				if (_effectsView != null && Children.Contains(_effectsView))
				{
					var index = Children.IndexOf(_effectsView);
					if (index >= 0)
					{
						Insert(index, _customView);
					}
				}
				else
				{
					Add(_customView);
				}
				IsItemTemplate = true;
			}
			else
			{
				IsItemTemplate = false;
			}
		}

		/// <summary>
		/// Resets the effects view by clearing any applied effects if it exists.
		/// </summary>
		void RemoveEffects()
		{
			if (_effectsView == null)
			{
				return;
			}

			_effectsView.Reset();
		}

		/// <summary>
		/// Updates the clip of the button based on the corner radius
		/// </summary>
		void UpdateButtonClip()
		{
#if WINDOWS && NET8_0
			if (Parent is Frame)
			{
				return;
			}
#endif
#if WINDOWS || ANDROID
			Clip = CornerRadius != 0 ? new RoundRectangleGeometry(CornerRadius, new Rect(0, 0, Width, Height)) : null;
#else
			if (Width > 0 && Height > 0)
			{
				Clip = new RoundRectangleGeometry(CornerRadius, new Rect(0, 0, Width, Height));
			}
#endif
#if !WINDOWS
			if (_effectsView != null)
			{
				_effectsView.Clip = new RoundRectangleGeometry(CornerRadius, new Rect(0, 0, Width, Height));
			}
#endif
		}

		/// <summary>
		/// Handles property change events for the <see cref="SfButton"/> and updates the control accordingly.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SfButton_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName != null)
			{
				if (e.PropertyName.Equals("IsEnabled", StringComparison.Ordinal))
				{
					OnIsEnabledPropertyChanged();
				}
				else if (e.PropertyName.Equals("CornerRadius", StringComparison.Ordinal))
				{
					UpdateButtonClip();
				}
				else if (e.PropertyName.Equals("IsVisible", StringComparison.Ordinal))
				{
					if (_customView != null)
					{
						_customView.IsVisible = IsVisible;
					}
					InvalidateDrawable();
				}
				else if (e.PropertyName.Equals("FlowDirection", StringComparison.Ordinal))
				{
					InvalidateDrawable();
				}
				else if (e.PropertyName.Equals("Width", StringComparison.Ordinal) || e.PropertyName.Equals("Height", StringComparison.Ordinal))
				{
#if MACCATALYST || IOS
					InvalidateMeasure(); 
#endif
					UpdateImageIcon();
				}
			}
		}

		/// <summary>
		/// Subscribes to the necessary events for the <see cref="SfButton"/> to handle property changes, loaded state, and binding context changes.
		/// </summary>
		void HookEvents()
		{
			PropertyChanged += SfButton_PropertyChanged;
			BindingContextChanged += SfButton_BindingContextChanged;
		}

		/// <summary>
		/// Unsubscribes from the events that were previously hooked in the <see cref="SfButton"/>.
		/// </summary>
		void UnhookEvents()
		{
			PropertyChanged -= SfButton_PropertyChanged;
			BindingContextChanged -= SfButton_BindingContextChanged;
		}

		/// <summary>
		/// Handles the changes of the IsCheckable and IsChecked properties and updates the visual appearance of the button accordingly.
		/// </summary>
		new void CheckPropertyChanged()
		{
			OnIsEnabledPropertyChanged();
			ChangeVisualState();
		}

		/// <summary>
		/// Handles the changes of the IsEnabled property and updates the visual appearance of the button accordingly.
		/// </summary>
		void OnIsEnabledPropertyChanged()
		{
			if (IsCheckable && IsChecked && !this.HasVisualStateGroups())
			{
				base.Background = _disabledPressedBackgroundColor;
			}
			else if (IsEnabled)
			{
				base.Background = Background;
				base.TextColor = TextColor;
			}
			else
			{
				if (!this.HasVisualStateGroups())
				{
					base.Background = Color.FromRgb(227, 227, 228);
					base.TextColor = Color.FromArgb("A9A6A9");
				}
			}
		}

		/// <summary>
		/// Draws the outline of the button with a rounded rectangle shape.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="dirtyRect"></param>
		void DrawButtonOutline(ICanvas canvas, RectF dirtyRect)
		{
			if (Stroke != null && StrokeThickness > 0)
			{
				var x = (float)(dirtyRect.X);
				var y = (float)(dirtyRect.Y);
				var width = (float)(dirtyRect.Width);
				var height = (float)(dirtyRect.Height);

				var topLeftRadius = (float)CornerRadius.TopLeft;
				var topRightRadius = (float)CornerRadius.TopRight;
				var bottomLeftRadius = (float)CornerRadius.BottomLeft;
				var bottomRightRadius = (float)CornerRadius.BottomRight;

				canvas.SaveState();
				canvas.StrokeColor = BaseStrokeColor;
				canvas.StrokeSize = (float)StrokeThickness;
				if (!IsSelected)
				{
					canvas.DrawRoundedRectangle(x, y, width, height, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
				}
				canvas.RestoreState();
			}
		}

		/// <summary>
		/// Calculates the width based on constraints and alignment.
		/// </summary>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <returns>Calculated width.</returns>
		double CalculateWidth(double widthConstraint)
		{
			if (widthConstraint == double.PositiveInfinity || widthConstraint < 0 || WidthRequest < 0)
			{
				double buttonContentWidth = 0;
				if (ShowIcon && ImageSource != null)
				{
					buttonContentWidth = ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom
						? Math.Max(ImageSize, TextSize.Width) + Padding.Left + Padding.Right + StrokeThickness + (_leftPadding * 2) + (_rightPadding * 2)
						: ImageSize + TextSize.Width + StrokeThickness + Padding.Left + Padding.Right + (_leftPadding * 2) + (_rightPadding * 2);
				}
				else
				{
					buttonContentWidth = TextSize.Width + Padding.Left + Padding.Right + StrokeThickness + (_leftPadding * 2) + (_rightPadding * 2);
				}
				if (buttonContentWidth <= widthConstraint)
				{
					return buttonContentWidth;
				}
			}
			return widthConstraint;
		}

		/// <summary>
		/// Calculates the height based on constraints, alignment, and line breaks.
		/// </summary>
		/// <param name="heightConstraint">Height constraint.</param>
		/// <param name="width">Calculated width.</param>
		/// <returns>Calculated height.</returns>
		double CalculateHeight(double heightConstraint, double width)
		{
			if (heightConstraint == double.PositiveInfinity || heightConstraint < 0 || VerticalOptions != LayoutOptions.Fill)
			{
				width = CalculateAvailableTextWidth(width);
				if (LineBreakMode == LineBreakMode.WordWrap || LineBreakMode == LineBreakMode.CharacterWrap)
					{
						_numberOfLines = StringExtensions.GetLinesCount(Text, (float)width, this, LineBreakMode, out _);
					}
					else
					{
						_numberOfLines = 1;
					}
				if (ShowIcon && ImageSource != null)
				{
					return ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom
						? Padding.Top + Padding.Bottom + StrokeThickness + (TextSize.Height * _numberOfLines) + ImageSize + _leftPadding + _rightIconPadding + _textSizePadding
						: Math.Max(ImageSize, TextSize.Height * _numberOfLines) + (_textSizePadding * 2) + Padding.Bottom + Padding.Top + StrokeThickness;
				}
				else
				{
					
					return (TextSize.Height * _numberOfLines) + Padding.Bottom + Padding.Top + StrokeThickness + (_textSizePadding * 2);
				}
			}
			return heightConstraint;
		}

		/// <summary>
		/// Calculates the available width for the text area within the button.
		/// </summary>
		/// <param name="width"></param>
		/// <returns></returns>
		double CalculateAvailableTextWidth(double width)
		{
			if (ShowIcon && ImageSource != null)
			{
				if (ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left)
				{
					width = (ImageAlignment == Alignment.Start && _isRightToLeft) ? Math.Abs((float)(width - _textRectF.X - ImageSize - (_leftIconPadding * 2) - _textAreaPadding - (StrokeThickness / 2) - Padding.Right))
										: Math.Abs((float)(width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right));

				}
				else if (ImageAlignment == Alignment.End || ImageAlignment == Alignment.Right)
				{
					width = (ImageAlignment == Alignment.End && _isRightToLeft) ? Math.Abs((float)(width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right))
										: Math.Abs((float)(width - _textRectF.X - ImageSize - _leftIconPadding - _leftIconPadding - _textAreaPadding - (StrokeThickness / 2) - Padding.Right));
				}
				else if (ImageAlignment == Alignment.Top)
				{
					width = Math.Abs((float)(width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - (StrokeThickness / 2) - (StrokeThickness / 2)));
				}
				else if (ImageAlignment == Alignment.Bottom)
				{
					width = Math.Abs((float)(width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - (StrokeThickness / 2) - (StrokeThickness / 2)));
				}
				else
				{
					width =  Math.Abs((float)(width - _textRectF.X - (StrokeThickness / 2) - _textAreaPadding - Padding.Right));
				}
			}
			else
			{

				width = Math.Abs((float)(width - _textAreaPadding - Padding.Left - Padding.Right - _textAreaPadding - StrokeThickness));
			}
			return width;
		}

		/// <summary>
		/// Measures the size of child elements if present and returns the size.
		/// </summary>
		/// <param name="widthConstraint"></param>
		/// <param name="heightConstraint"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns>Size of the child element</returns>
		Size MeasureChildren(double widthConstraint, double heightConstraint, double width, double height)
		{
			foreach (var child in Children)
			{
				if (child.Equals(_customView))
				{
					var size = child.Measure(widthConstraint, heightConstraint);
					if (!size.Width.Equals(double.PositiveInfinity) && !size.Height.Equals(double.PositiveInfinity) && size.Width > 0 && size.Height > 0)
					{
						return new Size(size.Width, size.Height);
					}
				}
			}
			return new Size(width, height);
		}

		/// <summary>
		/// Handle the button pressed state.
		/// </summary>
		/// <param name="e"></param>
		void HandlePressedState(PointerEventArgs e)
		{
#if IOS || MACCATALYST
			_touchDownPoint = e.TouchPoint;
#endif
			_isPressed = true;
#if WINDOWS
			//Workaround: SfButton consumes the click while double tapping on the overlay SfListView.
			_isPressInvoked = true;
#endif
			if (EnableRippleEffect && _effectsView != null)
			{
				_effectsView.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
			}

			ChangeVisualState();
		}

		/// <summary>
		/// Handle the button released state.
		/// </summary>
		/// <param name="e"></param>
		void HandleReleasedState(PointerEventArgs e)
		{
			_isPressed = false;
			RemoveEffects();
			ChangeVisualState();

#if IOS || MACCATALYST
			double diffY = Math.Abs(_touchDownPoint.Y - e.TouchPoint.Y);
			if (diffY >= ScrollThreshold)
			{
				return;
			}
#endif
			RectF elementBounds = new(0, 0, (float)Width, (float)Height);
			if (IsCheckable)
			{
				IsChecked = !IsChecked;
			}
#if WINDOWS
            if (elementBounds.Contains(e.TouchPoint) && _isPressInvoked)
#else
			if (elementBounds.Contains(e.TouchPoint))
#endif
			{
				RaiseClicked(EventArgs.Empty);
			}
			else
			{
#if WINDOWS || MACCATALYST
				_isMouseHover = false;
#endif
				RemoveEffects();
				ChangeVisualState();
			}
#if WINDOWS
                _isPressInvoked = false;
#endif
		}
#endregion

		#region Override Methods

		/// <summary>
		/// Measures the required size for the content based on given width and height constraints.
		/// </summary>
		/// <param name="widthConstraint"></param>
		/// <param name="heightConstraint"></param>
		/// <returns></returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			base.MeasureContent(widthConstraint, heightConstraint);

			double width = CalculateWidth(widthConstraint);
			double height = CalculateHeight(heightConstraint, WidthRequest > 0 ? WidthRequest : width);

			if (Children.Count > 0 && IsItemTemplate)
			{
				return MeasureChildren(widthConstraint, heightConstraint, width, height);
			}

			return new Size(width, height);
		}

		/// <summary>
		/// Draws an outline on the provided canvas for the current view.
		/// </summary>
		/// <param name="canvas">The canvas on which the outline will be drawn.</param>
		/// <param name="dirtyRect">The area to be updated on the canvas (dirty rectangle).</param>
		protected override void DrawOutline(ICanvas canvas, RectF dirtyRect)
		{
			canvas.SaveState();

			if (Stroke is SolidColorBrush stroke)
			{
				BaseStrokeColor = stroke.Color;
			}
			if (DashArray != null)
			{
				canvas.StrokeDashPattern = DashArray;
			}
			DrawButtonOutline(canvas, dirtyRect);
			canvas.RestoreState();
		}

		/// <summary>
		/// Renders the text within the specified area, aligning it according to the text alignment settings.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="dirtyRect"></param>
		internal override void DrawText(ICanvas canvas, RectF dirtyRect)
		{
			TextAlignment horizontalTextAlignment = _isRightToLeft
									? (HorizontalTextAlignment == TextAlignment.Start ? TextAlignment.End : HorizontalTextAlignment == TextAlignment.End ? TextAlignment.Start : TextAlignment.Center)
									: TextAlignment.Center;
			UpdateTextRect(dirtyRect);
			canvas.SaveState();
			float availableWidth = _textRectF.Width;
#if ANDROID
			if (LineBreakMode != LineBreakMode.WordWrap)
			{
				availableWidth -= AndroidTextMargin;
			}
#endif
			var trimmedText = _isFontIconText ? Text : StringExtensions.GetTextBasedOnLineBreakMode(ApplyTextTransform(Text), this, availableWidth, _textRectF.Height, LineBreakMode);
			if (_textRectF.Width > 0 && _textRectF.Height > 0)
			{
				canvas.DrawText(trimmedText, _textRectF, _isRightToLeft ? (HorizontalAlignment)horizontalTextAlignment : (HorizontalAlignment)HorizontalTextAlignment, (VerticalAlignment)VerticalTextAlignment, this);
			}
			canvas.RestoreState();
		}

		/// <summary>
		/// Drawing method of button control.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rect</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.Antialias = true;
			if (dirtyRect.Width > 0 && dirtyRect.Height > 0)
			{
				DrawOutline(canvas, dirtyRect);
				if (!IsItemTemplate)
				{
#if WINDOWS
					UpdateImageIcon();
#endif
					_isImageIconUpdated = true;
					DrawText(canvas, dirtyRect);
				}
			}
		}

		/// <summary>
		/// Arrange content method.
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns>the size.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			var contentBounds = base.ArrangeContent(bounds);
#if ANDROID || MACCATALYST || IOS
            if (_isImageIconUpdated)
            {
                UpdateImageIcon();
                _isImageIconUpdated = false;
            }
#endif
			UpdateButtonClip();
			return contentBounds;
		}

		/// <summary>
		/// Touch action method for button control.
		/// </summary>
		/// <param name="e"></param>
		/// <exception cref="NotImplementedException"></exception>
		public override void OnTouch(PointerEventArgs e)
		{
			if (!IsEnabled)
			{
				return;
			}

			if (e.Action == PointerActions.Pressed)
			{
				HandlePressedState(e);
			}
#if WINDOWS || MACCATALYST
            if (e.Action == PointerActions.Entered)
            {
                _isMouseHover = true;
                ChangeVisualState();
            }
            else if (e.Action == PointerActions.Moved)
            {
                _isMouseHover = true;
                ChangeVisualState();
            }
#endif
			else if (e.Action == PointerActions.Released)
			{
				HandleReleasedState(e);
			}
#if ANDROID || IOS
            else if (e.Action == PointerActions.Cancelled)
            {
                _isPressed = false;
                RemoveEffects();
                ChangeVisualState();
            }
#endif
			else if (e.Action == PointerActions.Exited)
			{
#if WINDOWS || MACCATALYST
                _isMouseHover = false;
#endif
				RemoveEffects();
				ChangeVisualState();
			}
		}

		/// <summary>
		/// Changes the visual state of the button based on its current properties
		/// </summary>
		protected override void ChangeVisualState()
		{
#if WINDOWS || MACCATALYST
            string stateName = IsEnabled ? !_isPressed && _isMouseHover ? "Hovered" : _isPressed ? "Pressed" : IsCheckable && IsChecked ? "Checked" : "Normal" : IsCheckable && IsChecked ? "Checked" : "Disabled";
#else
			string stateName = IsEnabled ? _isPressed ? "Pressed" : IsCheckable && IsChecked ? "Checked" : "Normal" : IsCheckable && IsChecked ? "Checked" : "Disabled";
#endif
			var stateGroup = VisualStateManager.GetVisualStateGroups(this);
			var findName = "Normal";
			foreach (VisualStateGroup state in stateGroup)
			{
				foreach (var a in state.States)
				{
					findName = a.Name == stateName ? stateName : findName;
				}
			}
			VisualStateManager.GoToState(this, findName);
			InvalidateDrawable();
		}

		/// <inheritdoc/>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfButtonStyles();
		}

		/// <inheritdoc/>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		/// <inheritdoc/>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		/// <summary>   
		/// Returns the semantics node list
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		protected override List<SemanticsNode> GetSemanticsNodesCore(double width, double height)
		{
			Size buttonSize = new(Width, Height);
			if (_buttonSemanticsNodes.Count != 0 && _buttonSemanticsSize == buttonSize && !_isSemanticTextChanged)
			{
				return _buttonSemanticsNodes;
			}
			if (_buttonSemanticsNodes.Count > 0)
			{
				_buttonSemanticsNodes.Clear();
			}
			_buttonSemanticsSize = buttonSize;
			SemanticsNode buttonNode = new()
			{
				Id = 1,
				Bounds = new(0, 0, Width, Height),
				Text = Text + "Button" + "Double tap to activate"
			};
			if (width > 0 && height > 0)
			{
				_buttonSemanticsNodes.Add(buttonNode);
			}
			_isSemanticTextChanged = false;
			return _buttonSemanticsNodes;
		}

		#endregion
	}
}
