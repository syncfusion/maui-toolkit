using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.TabView;

/// <summary>
/// A customizable center button with text and image support, handling touch interactions and tapped events.
/// </summary>
internal class CenterButtonView : ContentView, ITouchListener
{
	#region Fields

	// Center button fields
	SfBorder? _centerButtonBorderView;
	RoundRectangle? _centerButtonRoundRectangle;
	SfGrid? _centerButtonParentGrid;
	SfEffectsView? _centerButtonEffects;
	bool _isCenterButtonPressed;
	SfImage? _centerButtonImage;
	SfLabel? _centerButtonTitle;
	SfVerticalStackLayout? _centerButtonVerticalLayout;

	#endregion

	#region Events

	/// <summary>
	///  Occurs when a center button is tapped.
	/// </summary>
	internal event EventHandler<EventArgs>? CenterButtonTapped;

	#endregion

	#region Constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CenterButtonView"/> class.
	/// </summary>
	internal CenterButtonView()
	{
		InitializeCenterButton();
		Content = _centerButtonBorderView;
	}

	#endregion

	#region Interface Implementation

	/// <summary>
	/// Handles touch events to determine if a center button is tapped.
	/// </summary>
	/// <param name="e">The pointer event args.</param>
	public void OnTouch(Internals.PointerEventArgs e)
	{
		if (_centerButtonEffects is not null)
		{
			if (e.Action == PointerActions.Pressed)
			{
				_isCenterButtonPressed = true;
				_centerButtonEffects?.ApplyEffects();
			}
			else if (e.Action == PointerActions.Released)
			{
#if WINDOWS || MACCATALYST
					_centerButtonEffects.Reset(true);
#else
				_centerButtonEffects.Reset();
#endif

#if WINDOWS || MACCATALYST
					if (_centerButtonBorderView is not null && _centerButtonBorderView.Bounds.Contains(e.TouchPoint))
					{
						_centerButtonEffects.ApplyEffects(SfEffects.Highlight);
					}
#endif
				if (_isCenterButtonPressed)
				{
					var eventArgs = new EventArgs();
					RaiseCenterButtonTappedEvent(eventArgs);
					_isCenterButtonPressed = false;
				}
			}
			else if (e.Action == PointerActions.Cancelled)
			{
				_centerButtonEffects.Reset(true);
			}
#if WINDOWS || MACCATALYST
				else if (e.Action == PointerActions.Entered)
				{
					_centerButtonEffects?.ApplyEffects(SfEffects.Highlight);
				}
				else if (e.Action == PointerActions.Exited)
				{
					_centerButtonEffects.Reset(true);
				}
#endif
		}
	}

	#endregion

	#region Internal methods

	/// <summary>
	/// Invokes <see cref="CenterButtonTapped"/> event.
	/// </summary>
	/// <param name="args">The event args.</param>
	internal void RaiseCenterButtonTappedEvent(EventArgs args)
	{
		CenterButtonTapped?.Invoke(this, args);
	}

	/// <summary>
	/// Updates the text of the center button's title.
	/// </summary>
	/// <param name="title">The new title text to be set for the center button.</param>
	internal void UpdateCenterButtonTitle(string title)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.Text = title;
		}
	}

	/// <summary>
	/// Updates the width of the center button.
	/// </summary>
	/// <param name="width">The new width to be set for the center button.</param>
	internal void UpdateCenterButtonWidth(double width)
	{
		if (_centerButtonBorderView is not null && width >= 0)
		{
			_centerButtonBorderView.WidthRequest = width;
		}
	}

	/// <summary>
	/// Updates the height of the center button.
	/// </summary>
	/// <param name="height">The new height to be set for the center button.</param>
	internal void UpdateCenterButtonHeight(double height)
	{
		if (_centerButtonBorderView is not null && height >= 0)
		{
			_centerButtonBorderView.HeightRequest = height;
			HeightRequest = height;
		}
	}

	/// <summary>
	/// Updates the background of the center button using the specified Brush.
	/// </summary>
	/// <param name="background">The new background brush for the center button.</param>
	internal void UpdateCenterButtonBackground(Brush background)
	{
		if (_centerButtonBorderView is not null)
		{
			_centerButtonBorderView.Background = background;
		}
	}

	/// <summary>
	/// Updates the stroke color of the center button using the specified Color.
	/// </summary>
	/// <param name="color">The new stroke color for the center button.</param>
	internal void UpdateCenterButtonStroke(Color color)
	{
		if (_centerButtonBorderView is not null && color is not null)
		{
			_centerButtonBorderView.Stroke = new SolidColorBrush(color);
		}
	}

	/// <summary>
	/// Updates the stroke thickness of the center button with the specified value.
	/// </summary>
	/// <param name="strokeThickness">The new stroke thickness for the center button.</param>
	internal void UpdateCenterButtonStrokeThickness(double strokeThickness)
	{
		if (_centerButtonBorderView is not null)
		{
			_centerButtonBorderView.StrokeThickness = strokeThickness;
		}
	}

	/// <summary>
	/// Updates the corner radius of the center button using the specified CornerRadius.
	/// </summary>
	/// <param name="cornerRadius">The new corner radius for the center button.</param>
	internal void UpdateCenterButtonCornerRadius(CornerRadius cornerRadius)
	{
		if (_centerButtonRoundRectangle is not null)
		{
			_centerButtonRoundRectangle.CornerRadius = cornerRadius;
		}
	}

	/// <summary>
	/// Updates the font family of the center button's title with the specified font family.
	/// </summary>
	/// <param name="fontFamily">The new font family for the center button's title.</param>
	internal void UpdateCenterButtonFontFamily(string fontFamily)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.FontFamily = fontFamily;
		}
	}

	/// <summary>
	/// Updates the font attributes of the center button's title with the specified font attributes.
	/// </summary>
	/// <param name="fontAttributes">The new font attributes for the center button's title.</param>
	internal void UpdateCenterButtonFontAttributes(FontAttributes fontAttributes)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.FontAttributes = fontAttributes;
		}
	}

	/// <summary>
	/// Updates the font size of the center button's title with the specified size.
	/// </summary>
	/// <param name="fontSize">The new font size for the center button's title.</param>
	internal void UpdateCenterButtonFontSize(double fontSize)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.FontSize = fontSize;
		}
	}

	/// <summary>
	/// Updates the text color of the center button's title with the specified Color.
	/// </summary>
	/// <param name="textColor">The new text color for the center button's title.</param>
	internal void UpdateCenterButtonTextColor(Color textColor)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.TextColor = textColor;
		}
	}

	/// <summary>
	/// Updates the image source of the center button with the specified ImageSource.
	/// </summary>
	/// <param name="imageSource">The new image source for the center button.</param>
	internal void UpdateCenterButtonImageSource(ImageSource imageSource)
	{
		if (_centerButtonImage is not null)
		{
			_centerButtonImage.Source = imageSource;
		}
	}

	/// <summary>
	/// Updates the image size of the center button with the specified dimension.
	/// </summary>
	/// <param name="imageSize">The new size for the center button's image.</param>
	internal void UpdateCenterButtonImageSize(double imageSize)
	{
		if (_centerButtonImage is not null)
		{
			_centerButtonImage.WidthRequest = imageSize;
			_centerButtonImage.HeightRequest = imageSize;
		}
	}

	/// <summary>
	/// Updates the display mode of the center button to control visibility of the text and image based on the specified CenterButtonDisplayMode.
	/// </summary>
	/// <param name="displayMode">The new display mode for the center button.</param>
	internal void UpdateCenterButtonDisplayMode(CenterButtonDisplayMode displayMode)
	{
		if (_centerButtonVerticalLayout is not null)
		{
			_centerButtonVerticalLayout.Clear();
			if (displayMode == CenterButtonDisplayMode.Text)
			{
				if (_centerButtonTitle != null)
				{
					_centerButtonVerticalLayout.Children.Add(_centerButtonTitle);
				}
			}
			else if (displayMode == CenterButtonDisplayMode.Image)
			{
				if (_centerButtonImage != null)
				{
					_centerButtonVerticalLayout.Children.Add(_centerButtonImage);
				}
			}
			else
			{
				_centerButtonVerticalLayout.Children.Add(_centerButtonImage);
				_centerButtonVerticalLayout.Children.Add(_centerButtonTitle);
			}
		}
	}

	/// <summary>
	/// Updates the FontAutoScalingEnabled property of the center button's title.
	/// </summary>
	/// <param name="fontAutoScalingEnabled">A boolean value indicating whether font auto-scaling is enabled.</param>
	internal void UpdateCenterButtonFontAutoScalingEnabled(bool fontAutoScalingEnabled)
	{
		if (_centerButtonTitle is not null)
		{
			_centerButtonTitle.FontAutoScalingEnabled = fontAutoScalingEnabled;
		}
	}

	#endregion

	#region Private method

	/// <summary>
	/// Initialize the center button.
	/// </summary>
	void InitializeCenterButton()
	{
		_centerButtonTitle = new SfLabel()
		{
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center
		};

		_centerButtonImage = new SfImage()
		{
			Aspect = Aspect.AspectFit,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
		};

		_centerButtonBorderView = new SfBorder()
		{
			BackgroundColor = Colors.Transparent,
		};

		_centerButtonBorderView.AddTouchListener(this);
		_centerButtonRoundRectangle = new RoundRectangle();
		_centerButtonBorderView.StrokeShape = _centerButtonRoundRectangle;
		_centerButtonParentGrid = new SfGrid
		{
			Style = new Style(typeof(SfGrid)),
		};

		_centerButtonEffects = new SfEffectsView
		{
			RippleAnimationDuration = 150,
			InitialRippleFactor = 0.75,
			ShouldIgnoreTouches = true,
		};

		_centerButtonVerticalLayout = new SfVerticalStackLayout()
		{
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Center,
		};

		_centerButtonVerticalLayout.Children.Add(_centerButtonTitle);
		_centerButtonParentGrid.Children.Add(_centerButtonVerticalLayout);
		_centerButtonParentGrid.Children.Add(_centerButtonEffects);
		_centerButtonBorderView.Content = _centerButtonParentGrid;
	}

	#endregion
}
