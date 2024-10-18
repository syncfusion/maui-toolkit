using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;


namespace Syncfusion.Maui.Toolkit.Chips
{
	/// <summary>
	/// Represents a Chip control which can be added in a layout and grouped using SfChipGroup.
	/// Represents the <see cref="SfChip"/> class.
	/// </summary>
	public class SfChip : ButtonBase, IParentThemeElement
    {
        #region Fields

        const float _selectionIndicatorPadding = 7f;
        const float _selectionIndicatorHeight = 18f;
        const float _linePadding = 5f;
        const float _closeButtonRadius = 12f;
        const float _closeButtonTopPadding = 3;
        const int _defaultCloseButtonWidth = 18;
        const int _closeButtonPadding = 2;
        const int _defaultSelectionIndicatorWidth = 18;

        Color _chipCloseButtonColor = Color.FromArgb("#49454E");
        Color _selectionIndicatorColorValue = Color.FromRgb(30, 25, 43);
        Color _circleColor = Colors.Transparent;

        RectF _closeButtonRectF = new RectF();
        RectF _closeButtonRippleRectF = new RectF();

        PointF _circlePoint = new Point();
        PointF _startLine = new Point();
        PointF _endLine = new Point();

        Point _pressedPoint;
        Point _releasedPoint;

        static readonly PathBuilder _pathBuilder = new();
        PathF _customPath = new();
        internal SfChipsType? chipType;
        bool _isDescriptionNotSetByUser;
        List<SemanticsNode> chipSemanticsNodes = new List<SemanticsNode>();

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ShowSelectionIndicator"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowSelectionIndicator"/> property determines whether the selection indicator is visible or not.
        /// </remarks>
        public static readonly BindableProperty ShowSelectionIndicatorProperty =
            BindableProperty.Create(
                nameof(ShowSelectionIndicator),
                typeof(bool),
                typeof(SfChip),
                false,
                BindingMode.Default,
                null,
                OnShowSelectionIndicatorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionIndicatorColor"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="SelectionIndicatorColor"/> property determines the color of the selection indicator.
        /// </remarks>
        public static readonly BindableProperty SelectionIndicatorColorProperty =
            BindableProperty.Create(
                nameof(SelectionIndicatorColor),
                typeof(Color),
                typeof(SfChip),
                Color.FromArgb("#49454F"),
                BindingMode.Default,
                null,
                OnSelectionIndicatorColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowCloseButton"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowCloseButton"/> property determines whether the close button is visible or not.
        /// </remarks>
        public static readonly BindableProperty ShowCloseButtonProperty =
            BindableProperty.Create(
                nameof(ShowCloseButton),
                typeof(bool),
                typeof(SfChip),
                false,
                BindingMode.Default,
                null,
                OnShowCloseButtonPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CloseButtonColor"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CloseButtonColor"/> property determines the color of the close button.
        /// </remarks>
        public static readonly BindableProperty CloseButtonColorProperty =
            BindableProperty.Create(
                nameof(CloseButtonColor),
                typeof(Color),
                typeof(SfChip),
                Color.FromArgb("#1C1B1F"),
                BindingMode.Default,
                null,
                OnCloseButtonColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CloseButtonPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CloseButtonPath"/> property determines the path of the close button.
        /// </remarks>
        public static readonly BindableProperty CloseButtonPathProperty =
            BindableProperty.Create(
                nameof(CloseButtonPath),
                typeof(string),
                typeof(SfChip),
                string.Empty,
                BindingMode.Default,
                null,
                OnCloseButtonPathPropertyChanged);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SfChip"/> class.
        /// </summary>
        public SfChip()
        {
            ThemeElement.InitializeThemeResources(this, "SfChipTheme");
            InitializeElements();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SfChip"/> class.
        /// </summary>
        /// <param name="isCreatedInternally">It sets to <c>true</c> if it's created internally.</param>
        internal SfChip(bool isCreatedInternally)
        {
            ThemeElement.InitializeThemeResources(this, "SfChipTheme");
            InitializeElements();
            IsCreatedInternally = isCreatedInternally;
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value that can be used to change the color of the close button.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Color"/> values and the default value is Color.FromArgb("#1C1B1F").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the appearance of the close button by setting its color.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="CloseButtonColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip CloseButtonColor="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     CloseButtonColor = Color.FromArgb("#FF0000")
		/// };
		/// ]]></code>
		/// </example>
		public Color CloseButtonColor
        {
            get { return (Color)GetValue(CloseButtonColorProperty); }
            set { SetValue(CloseButtonColorProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicating whether the close button is visible or not. The close button is in a visible state when this property is set to true.
		/// </summary>
		/// <value>
		/// Specifies the show close button property. The default value is false.
		/// </value>
		/// <remarks>
		/// This property allows you to control the visibility of the close button on the chip.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ShowCloseButton"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip ShowCloseButton="True" />
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     ShowCloseButton = true
		/// };
		/// ]]></code>
		/// </example>
		public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicating whether the selection indicator is visible or not. The selection indicator is in a visible state when this property is set to true.
		/// </summary>
		/// <value>
		/// Specifies the show selection indicator property. The default value is false.
		/// </value>
		/// <remarks>
		/// This property allows you to control the visibility of the selection indicator on the chip.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ShowSelectionIndicator"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip ShowSelectionIndicator="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     ShowSelectionIndicator = true
		/// };
		/// ]]></code>
		/// </example>
		public bool ShowSelectionIndicator
        {
            get => (bool)GetValue(ShowSelectionIndicatorProperty);
            set => SetValue(ShowSelectionIndicatorProperty, value);
        }

		/// <summary>
		/// Gets or sets the value of SelectionIndicatorColor. This property can be used to change the color of the selection indicator.
		/// </summary>
		/// <value>
		/// Specifies the selection indicator color. The default value is Color.FromArgb("#49454F").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the color of the selection indicator.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectionIndicatorColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip SelectionIndicatorColor="Blue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     SelectionIndicatorColor = Color.FromRgb(0, 0, 255)
		/// };
		/// ]]></code>
		/// </example>
		public Color SelectionIndicatorColor
        {
            get => (Color)GetValue(SelectionIndicatorColorProperty);
            set => SetValue(SelectionIndicatorColorProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SfChip"/> is selected.
        /// </summary>
        /// <value>
        /// A boolean value indicating the selection state of the chip. The default value is false.
        /// </value>
        /// <remarks>
        /// This property allows you to determine if the chip is currently selected. It is internally set and cannot be modified directly.
        /// </remarks>
        public new bool IsSelected
        {
            get;
            internal set;
        }

		/// <summary>
		/// Gets or sets the value of CloseButtonPath. This property can be used to customize the close button.
		/// </summary>
		/// <value>
		/// Specifies the close button path. The default value is <see cref="string.Empty"/>.
		/// </value>
		/// <remarks>
		/// This property allows you to set a custom path for the close button icon, enabling you to use a custom SVG or other path data.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="CloseButtonPath"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip CloseButtonPath="M10 10 H 90 V 90 H 10 L 10 10" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     CloseButtonPath = "M10 10 H 90 V 90 H 10 L 10 10"
		/// };
		/// ]]></code>
		/// </example>
		public string CloseButtonPath
		{
			get { return (string)GetValue(CloseButtonPathProperty); }
			set { SetValue(CloseButtonPathProperty, value); }
		}

        #endregion

        #region Internal Properties

        /// <summary>
        /// Close button color update property.
        /// </summary>
        internal Color ChipCloseButtonColor
        {
            get { return _chipCloseButtonColor; }
            set { _chipCloseButtonColor = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the SemanticProperties.Description is not set by user.
        /// </summary>
        internal bool IsDescriptionNotSetByUser
        {
            get { return _isDescriptionNotSetByUser; }
            set { _isDescriptionNotSetByUser = value; }
        }

        internal Color SelectionIndicatorColorValue
        {
            get { return _selectionIndicatorColorValue; }
            set { _selectionIndicatorColorValue = value; }
        }

        internal bool IsCloseButtonClicked;

        internal bool IsKeyDown { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// The raise close button clicked event.
        /// </summary>
        /// <param name="args"> The Event Argument.</param>
        internal void RaiseCloseButtonClicked(EventArgs args)
        {
            if (CloseButtonClicked != null)
            {
                CloseButtonClicked(this, args);
            }
        }

        /// <summary>
        /// This method is used to check whether the touch is within close button when touch interaction
        /// </summary>
        /// <returns></returns>
        internal bool IsTouchInsideCloseButton(Point touchPoint)
        {
            double touchXPosition = touchPoint.X;
            double touchYPosition = touchPoint.Y;
#if WINDOWS
	    float closeButtonRight = (isRTL && !IsCreatedInternally) ? (float)Width - _circlePoint.X + _closeButtonRadius : _circlePoint.X + _closeButtonRadius + _closeButtonPadding;
            float closeButtonLeft = (isRTL && !IsCreatedInternally) ? (float)Width - _circlePoint.X - _closeButtonRadius : _circlePoint.X - _closeButtonRadius + _closeButtonPadding;
#else
	    float closeButtonRight = _circlePoint.X + _closeButtonRadius + _closeButtonPadding;
	    float closeButtonLeft = _circlePoint.X - _closeButtonRadius + _closeButtonPadding;
#endif
	    float closeButtonTop = _circlePoint.Y - _closeButtonRadius + _closeButtonPadding;
            float closeButtonBottom = _circlePoint.Y + _closeButtonRadius + _closeButtonPadding;

            return touchXPosition <= closeButtonRight && touchXPosition >= closeButtonLeft
                && touchYPosition <= closeButtonBottom && touchYPosition >= closeButtonTop;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the chip text based on its type.
        /// </summary>
        /// <returns>A string describing the type of the chip.</returns>
        string GetChipNodeText()
        {

            if (IsEnabled)
            {
                switch (chipType)
                {
                    case SfChipsType.Input:
                        return Text + " Chip";

                    case SfChipsType.Action:
                        return Text + " Chip Double tap to trigger the Action";

                    case SfChipsType.Filter:
                        return Text + (IsSelected ? " Selected Filter Chip - Double tap to unselected" : " Chip Double tap to apply Filter");

                    case SfChipsType.Choice:
                        return Text + (IsSelected ? " Selected Choice Chip" : "Chip Double tap to choose");

                    default:
                        return Text + "Chip";
                }

            }
            else
            {
                return Text + "Chip is disabled";
            }

        }

        /// <summary>
        /// Gets the semantic reader text for the chip based on its type.
        /// </summary>
        /// <returns>A string describing the action associated with the chip.</returns>
        string GetSemanticReaderText()
        {
            switch (chipType)
            {
                case SfChipsType.Action:
                    return Text + " Chip Action Triggered.";

                case SfChipsType.Filter:
                    return Text + (IsSelected ? " Chip is unselected" : " Chip Selected for Filtering.");

                case SfChipsType.Choice:
                    return Text + " Chip is Chosen.";

                default:
                    return Text + " Chip.";
            }
        }

        void HandleEnabledState()
        {
            Color default1 = Color.FromArgb("FFFBFE");
            Color default2 = Color.FromArgb("#E8DEF8");

            BaseTextColor = TextColor;

            if (Stroke != null)
            {
                BaseStrokeColor = ((SolidColorBrush)Stroke).Color;
            }

            SelectionIndicatorColorValue = SelectionIndicatorColor;
            ChipCloseButtonColor = CloseButtonColor;

            if (ShowSelectionIndicator || ShowCloseButton)
            {
                if (Background == null || Background.Equals(default1))
                {
                    BaseBackgroundColor = default2;
                }
            }
            else if (Background == null || Background.Equals(default2))
            {
                BaseBackgroundColor = default1;
            }
        }

        void HandleDisabledState()
        {
            if (!VisualStateManager.HasVisualStateGroups(this) && Application.Current != null && !Application.Current.Resources.TryGetValue("SfChipTheme", out var LightTheme))
            {
                BaseBackgroundColor = Color.FromRgba(28, 27, 31, 30);
                SelectionIndicatorColorValue = Color.FromRgba(28, 27, 31, 97);
                ChipCloseButtonColor = Color.FromRgba(28, 27, 31, 97);
                BaseStrokeColor = disabledBorderColor;
                BaseTextColor = disabledTextColor;
            }
            else
            {
                BaseBackgroundColor = Background;
                SelectionIndicatorColorValue = SelectionIndicatorColor;
                ChipCloseButtonColor = CloseButtonColor;
                BaseStrokeColor = ((SolidColorBrush)Stroke).Color;
                BaseTextColor = TextColor;
            }
        }

        double CalculateWidth(double widthConstraint)
        {
            if (widthConstraint == double.PositiveInfinity || widthConstraint < 0 || IsCreatedInternally)
            {
                double width = TextSize.Width + Padding.HorizontalThickness;
                double androidCloseButtonWidth = DeviceInfo.Platform == DevicePlatform.Android ? _defaultCloseButtonWidth + leftIconPadding / 2 : _defaultCloseButtonWidth + leftIconPadding;

                if (ShowCloseButton && !ShowIcon && !ShowSelectionIndicator)
                {
                    width += androidCloseButtonWidth + rightIconPadding + leftPadding;
                }
                else if (ShowCloseButton && ShowIcon && !ShowSelectionIndicator)
                {
                    width += _defaultCloseButtonWidth + 2 * leftIconPadding + rightIconPadding + leftPadding / 3 + ImageSize + 6;
                }
                else if (ShowIcon && !ShowCloseButton && !ShowSelectionIndicator)
                {
                    width += textSizePadding + ImageSize + leftIconPadding + rightIconPadding + rightPadding;
                }
                else if (ShowSelectionIndicator && !ShowCloseButton && !ShowIcon)
                {
                    width += _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding / 2 + rightPadding;
                }
                else if (ShowCloseButton && ShowSelectionIndicator && !ShowIcon)
                {
                    width += textSizePadding + _defaultCloseButtonWidth + leftIconPadding + rightIconPadding + _defaultSelectionIndicatorWidth;
                }
                else if (ShowSelectionIndicator && ShowIcon && !ShowCloseButton)
                {
                    width += _defaultSelectionIndicatorWidth + ImageSize + 2 * leftIconPadding + rightIconPadding + (rightPadding - textAreaPadding);
                }
                else if (ShowCloseButton && ShowSelectionIndicator && ShowIcon)
                {
                    width += textSizePadding + _defaultCloseButtonWidth + _defaultSelectionIndicatorWidth + ImageSize + 2 * leftIconPadding + rightIconPadding + (rightPadding - textAreaPadding);
                }
                else
                {
                    width += leftPadding + rightPadding;
                }

                return width;
            }

            return widthConstraint;
        }

        double CalculateHeight(double heightConstraint)
        {
            if (heightConstraint == double.PositiveInfinity || heightConstraint < 0 || IsCreatedInternally)
            {
                if (ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom)
                {
                    return ShowIcon ? 2 * ImageSize + TextSize.Height + Padding.Top + Padding.Bottom
                        : TextSize.Height + textHeightPadding + Padding.Top + Padding.Bottom;
                }
                else
                {
                    return ShowIcon ? FontSize + ImageSize + Padding.Top + Padding.Bottom
                        : TextSize.Height + textHeightPadding + Padding.Top + Padding.Bottom;
                }
            }

            return heightConstraint;
        }

        void HandlePressedAction(PointerEventArgs e, Point touchPoint)
        {
            _pressedPoint = e.TouchPoint;

            if (ShowCloseButton)
            {
                IsCloseButtonClicked = IsTouchInsideCloseButton(touchPoint);
                if (IsCloseButtonClicked && _circleColor != HighlightColor)
                {
                    _circleColor = HighlightColor;
                    InvalidateDrawable();
                }
                else
                {
                    effectsRenderer?.RippleBoundsCollection.Clear();
                }
            }

            if (IsTouchInsideCloseButton(touchPoint) && IsEnabled)
            {
                UpdateCloseButtonRippleEffectsRenderer();
            }
            else if (!ShowCloseButton)
            {
                UpdateRippleEffectsRenderer();
            }
        }

        void HandleReleasedAction(PointerEventArgs e, Point touchPoint)
        {
            _releasedPoint = e.TouchPoint;

#if IOS || MACCATALYST
            double diffX = Math.Abs(_pressedPoint.X - _releasedPoint.X);
            double diffY = Math.Abs(_pressedPoint.Y - _releasedPoint.Y);

            if (diffX >= 10 || diffY >= 10)
            {
                return;
            }
#endif

            if (IsTouchInsideCloseButton(touchPoint) && IsEnabled)
            {
                RaiseCloseButtonClicked(EventArgs.Empty);
            }

            if (!_circleColor.Equals(Colors.Transparent))
            {
                _circleColor = Colors.Transparent;
                InvalidateDrawable();
            }
        }

        void HandleMovedAction(Point touchPoint)
        {
#if WINDOWS || MACCATALYST
            Color default2 = Color.FromArgb("#E8DEF8");
            Color default1 = Color.FromArgb("FFFBFE");

            if (Background is SolidColorBrush brush && (brush.Color.Equals(default1) || brush.Color.Equals(default2)))
            {
                if (ShowCloseButton && IsTouchInsideCloseButton(touchPoint))
                {
                    if (_circleColor != HighlightColor)
                    {
                        _circleColor = HighlightColor;
                        InvalidateDrawable();
                    }
                    if (!background.Equals(Colors.Transparent))
                    {
                        background = Colors.Transparent;
                        InvalidateDrawable();
                    }
                }
                else if (!_circleColor.Equals(Colors.Transparent))
                {
                    _circleColor = Colors.Transparent;
                    InvalidateDrawable();
                }
            }
            else
            {
                if (ShowCloseButton && IsTouchInsideCloseButton(touchPoint))
                {
                    if (_circleColor != HighlightColor)
                    {
                        _circleColor = HighlightColor;
                        InvalidateDrawable();
                    }
                    BaseBackgroundColor = Background;
                }
                else if (!_circleColor.Equals(Colors.Transparent))
                {
                    _circleColor = Colors.Transparent;
                    InvalidateDrawable();
                }
            }
#endif

#if ANDROID || IOS
            _circleColor = Colors.Transparent;
#endif
        }

        void HandleExitedAction()
        {
            _circleColor = Colors.Transparent;
            VisualStateManager.GoToState(this, "Normal");
            InvalidateDrawable();
        }

        /// <summary>
        /// The CreateImageIcon Method is used to add the image based on height and width and also for the calculation of image placement.
        /// </summary>
        void CreateImageIcon()
        {
            if (imageViewGrid != null && ShowIcon)
            {
                if (Children.Contains(imageViewGrid))
                    Children.Remove(imageViewGrid);

                if (ShowCloseButton && !ShowSelectionIndicator)
                {
                    SetPositionForCloseButton();
                }
                else if (ShowSelectionIndicator && !ShowCloseButton)
                {
                    SetPositionForSelectionIndicator();
                }
                else if (ShowSelectionIndicator && ShowCloseButton)
                {
                    SetPositionForBothCloseButtonAndSelectionIndicator();
                }
                else
                {
                    SetDefaultPosition();
                }
            }
        }

        void SetPositionForCloseButton()
        {
            double x = 0;
            double y = 0;

            if (ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left)
            {
                x = leftIconPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Right || ImageAlignment == Alignment.End)
            {
                x = Math.Abs((float)Width - rightIconPadding - (float)ImageSize - leftIconPadding - _defaultCloseButtonWidth);
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Top)
            {
                x = leftIconPadding;
                y = StrokeThickness / 2;
            }
            else
            {
                x = leftIconPadding;
                y = Height - ImageSize - textSizePadding - StrokeThickness / 2;
            }

            if (imageViewGrid != null)
            {
                Children.Add(imageViewGrid);
                AbsoluteLayout.SetLayoutBounds(imageViewGrid, new Rect(x, y, ImageSize, ImageSize));
            }
        }

        void SetPositionForSelectionIndicator()
        {
            double x = 0;
            double y = 0;

            if (ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left)
            {
                x = 1.20 * _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Right || ImageAlignment == Alignment.End)
            {
                x = Width - ImageSize - (rightPadding - textAreaPadding);
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Top)
            {
                x = _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = StrokeThickness / 2;
            }
            else
            {
                x = _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = Height - ImageSize - textSizePadding - StrokeThickness / 2;
            }

            if (imageViewGrid != null)
            {
                Children.Add(imageViewGrid);
                AbsoluteLayout.SetLayoutBounds(imageViewGrid, new Rect(x, y, ImageSize, ImageSize));
            }
        }

        void SetPositionForBothCloseButtonAndSelectionIndicator()
        {
            double x = 0;
            double y = 0;

            if (ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left)
            {
                x = 1.15 * _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Right || ImageAlignment == Alignment.End)
            {
                x = Width - ImageSize - _defaultCloseButtonWidth - (rightPadding - textAreaPadding) - leftIconPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Top)
            {
                x = _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = StrokeThickness / 2;
            }
            else
            {
                x = _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                y = Height - ImageSize - textSizePadding - StrokeThickness / 2;
            }

            if (imageViewGrid != null)
            {
                Children.Add(imageViewGrid);
                AbsoluteLayout.SetLayoutBounds(imageViewGrid, new Rect(x, y, ImageSize, ImageSize));
            }
        }

        void SetDefaultPosition()
        {
            double x = 0;
            double y = 0;

            if (ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left)
            {
                x = leftIconPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Right || ImageAlignment == Alignment.End)
            {
                x = Width - ImageSize - rightPadding;
                y = Height - Height / 2 - ImageSize / 2;
            }
            else if (ImageAlignment == Alignment.Top)
            {
                x = leftIconPadding;
                y = StrokeThickness / 2;
            }
            else
            {
                x = leftIconPadding;
                y = Height - ImageSize - textSizePadding - StrokeThickness / 2;
            }

            if (imageViewGrid != null)
            {
                Children.Add(imageViewGrid);
                AbsoluteLayout.SetLayoutBounds(imageViewGrid, new Rect(x, y, ImageSize, ImageSize));
            }
        }

        static PathF PathBuilder(string path)
        {
            return _pathBuilder.BuildPath(path);
        }


        void UpdateCloseButtonColor()
        {
            ChipCloseButtonColor = CloseButtonColor;
        }

        void UpdateSelectionIndicatorColor()
        {
            SelectionIndicatorColorValue = SelectionIndicatorColor;
        }

        void UpdateClearIcon(RectF dirtyRect)
        {
            if (ShowCloseButton)
            {
                float xOffset;

                if (ShowIcon)
                {
                    xOffset = isRTL ? (float)(leftIconPadding + (float)(_linePadding / 0.95)) : (float)(Width - _defaultCloseButtonWidth - rightIconPadding + (float)(_linePadding * 1.75));
                }
                else
                {
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                        xOffset = isRTL ? (float)(leftIconPadding + (float)(_linePadding / 0.95)) : (float)(Width - _defaultCloseButtonWidth - rightIconPadding + (float)(_linePadding * 1.75));
                    else
                        xOffset = isRTL ? (float)(leftIconPadding + (float)(_linePadding / 0.95)) : dirtyRect.Width - _defaultCloseButtonWidth / 2 - rightIconPadding;
                }

                _closeButtonRectF.X = xOffset;
                _closeButtonRectF.Y = (float)(Height / 2);

                // The ripple effect rectange of close button
                _closeButtonRippleRectF.X = (_circlePoint.X - _closeButtonRadius) - _closeButtonTopPadding;
                _closeButtonRippleRectF.Y = (_closeButtonRectF.Y - _closeButtonRadius) - _closeButtonTopPadding;
                _closeButtonRippleRectF.Width = (_closeButtonRadius * 2) + (leftPadding / 2);
                _closeButtonRippleRectF.Height = (_closeButtonRadius * 2) + (leftPadding / 2);
            }
        }

        void DrawSelectionIndicator(ICanvas canvas, RectF dirtyRect)
        {
            if (!ShowSelectionIndicator)
                return;

            canvas.StrokeColor = SelectionIndicatorColorValue;
            dirtyRect.X = isRTL ? (float)(Width - _defaultSelectionIndicatorWidth - rightIconPadding + (float)(_linePadding / 0.95f)) : leftIconPadding;
            dirtyRect.Y = (float)Height / 2 - _selectionIndicatorPadding;
            dirtyRect.Width = _defaultSelectionIndicatorWidth;
            dirtyRect.Height = _selectionIndicatorHeight;
            DrawTick(canvas, dirtyRect);
        }

        static void DrawTick(ICanvas canvas, RectF tickRect)
        {
            float padding = 2f;
            float horizotalThickness = 4f;
            float moveXOffset = .5f;
            float moveYOffset = 8.5f;

            var rect = new RectF(tickRect.X + padding, tickRect.Y + (padding / 2), tickRect.Width, tickRect.Height - horizotalThickness);
            var path = new PathF();
            path.MoveTo(rect.X + moveXOffset, rect.Y + rect.Height - moveYOffset);
            path.LineTo(rect.X + horizotalThickness, rect.Y + rect.Height - horizotalThickness);
            path.LineTo(rect.X + rect.Width - horizotalThickness, rect.Y + padding);
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);
        }

        void DrawKeyDownBorder(ICanvas canvas, RectF dirtyRect)
        {
            float minStrokeThickness = 4;
            float halfStrokeThickness = (float)StrokeThickness / 2;

            backgroudRectF.X = dirtyRect.X + halfStrokeThickness;
            backgroudRectF.Y = dirtyRect.Y + halfStrokeThickness;
            backgroudRectF.Width = dirtyRect.Width;
            backgroudRectF.Height = dirtyRect.Height;

            float topLeftRadius = (float)CornerRadius.TopLeft - halfStrokeThickness;
            float topRightRadius = (float)CornerRadius.TopRight - halfStrokeThickness;
            float bottomLeftRadius = (float)CornerRadius.BottomLeft - halfStrokeThickness;
            float bottomRightRadius = (float)CornerRadius.BottomRight - halfStrokeThickness;

            var keyDownRectF = new RectF
            {
                X = backgroudRectF.X,
                Y = backgroudRectF.Y,
                Width = backgroudRectF.Width - (float)StrokeThickness,
                Height = backgroudRectF.Height - (float)StrokeThickness
            };

            canvas.CanvasSaveState();
            canvas.StrokeSize = (float)Math.Max(StrokeThickness - 1, minStrokeThickness);
            canvas.StrokeColor = Colors.Black;
            canvas.DrawRoundedRectangle(keyDownRectF, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
            canvas.CanvasRestoreState();
        }

        void UpdateCloseButtonRippleEffectsRenderer()
        {
            if (IsEnabled)
            {
                if (EnableRippleEffect)
                {
                    if (effectsRenderer != null)
                    {
                        effectsRenderer.RippleBoundsCollection.Clear();
                        effectsRenderer.RippleBoundsCollection.Add(_closeButtonRippleRectF);
                    }
                }
                else
                {
                    if (effectsRenderer != null)
                    {
                        effectsRenderer?.RippleBoundsCollection.Clear();
                    }
                }
            }
        }

        void DrawCloseButton(ICanvas canvas, RectF dirtyRect)
        {
            if (ShowCloseButton)
            {
                UpdateClearIcon(dirtyRect);
                DrawCloseButtonBackgroundCircle(canvas, _closeButtonRectF);
                DrawCloseIcon(canvas, _closeButtonRectF);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="closeButtonRectF"></param>
        void DrawCloseIcon(ICanvas canvas, RectF closeButtonRectF)
        {
            _circlePoint.X = closeButtonRectF.X;
            _circlePoint.Y = closeButtonRectF.Y;
            _startLine.X = closeButtonRectF.X - _linePadding;
            _startLine.Y = closeButtonRectF.Y - _linePadding;
            _endLine.X = closeButtonRectF.X + _linePadding;
            _endLine.Y = closeButtonRectF.Y + _linePadding;
            canvas.CanvasSaveState();
            canvas.StrokeSize = 1;
            canvas.StrokeColor = ChipCloseButtonColor;
            if (CloseButtonPath != string.Empty)
            {
                canvas.Translate(_circlePoint.X - _customPath.Bounds.Center.X, _circlePoint.Y - _customPath.Bounds.Center.Y);
                canvas.DrawPath(_customPath);
            }
            else
            {
                canvas.DrawLine(_startLine, _endLine);
                _startLine.X = closeButtonRectF.X + _linePadding;
                _startLine.Y = closeButtonRectF.Y - _linePadding;
                _endLine.X = closeButtonRectF.X - _linePadding;
                _endLine.Y = closeButtonRectF.Y + _linePadding;
                canvas.DrawLine(_startLine, _endLine);
            }
            canvas.CanvasRestoreState();
        }

        void DrawCloseButtonBackgroundCircle(ICanvas canvas, RectF closeButtonRectF)
        {
            _circlePoint.X = closeButtonRectF.X;
            _circlePoint.Y = closeButtonRectF.Y;
            canvas.CanvasSaveState();
            canvas.FillColor = _circleColor;
            canvas.FillCircle(_circlePoint, _closeButtonRadius);
            canvas.CanvasRestoreState();
        }

        void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            UpdateBackgroundRectF(dirtyRect);

            float halfStrokeThickness = (float)(StrokeThickness / 2);
            float topLeftRadius = (float)CornerRadius.TopLeft - halfStrokeThickness;
            float topRightRadius = (float)CornerRadius.TopRight - halfStrokeThickness;
            float bottomLeftRadius = (float)CornerRadius.BottomLeft - halfStrokeThickness;
            float bottomRightRadius = (float)CornerRadius.BottomRight - halfStrokeThickness;

            canvas.CanvasSaveState();
            canvas.FillColor = background;
            canvas.FillRoundedRectangle(backgroudRectF, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
            canvas.CanvasRestoreState();
        }

        void SetTextBoundsForCloseButton(RectF dirtyRect)
        {
            textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + _defaultCloseButtonWidth + leftIconPadding + rightIconPadding : (float)Padding.Left - (float)Padding.Right + leftPadding;
            textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
            textRectF.Width = (float)Width - _defaultCloseButtonWidth - leftIconPadding - rightIconPadding - leftPadding;
            textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
        }

        void SetTextBoundsForCloseButtonAndIcon(RectF dirtyRect)
        {
            if (ImageAlignment == Alignment.Default || ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left || ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + leftIconPadding + rightIconPadding + _defaultCloseButtonWidth : (float)Padding.Left - (float)Padding.Right + (float)ImageSize + leftIconPadding + rightIconPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - (float)ImageSize - 2 * leftIconPadding - 2 * rightIconPadding - _defaultCloseButtonWidth);
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
            else if (ImageAlignment == Alignment.End || ImageAlignment == Alignment.Right)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + 2 * rightIconPadding + leftIconPadding + (float)ImageSize + _defaultCloseButtonWidth : (float)Padding.Left - (float)Padding.Right + leftIconPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - 2 * rightIconPadding - (float)ImageSize - 2 * leftIconPadding - _defaultCloseButtonWidth);
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
        }

        void SetTextBoundsForSelectionIndicator(RectF dirtyRect)
        {
            textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + rightPadding : (float)Padding.Left - (float)Padding.Right + _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding / (float)1.75;
            textRectF.Y = IsSelected ? (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textSelectionPadding : (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textSelectionPadding + (float)StrokeThickness / 2;
            textRectF.Width = (float)Width - _defaultSelectionIndicatorWidth - leftIconPadding - rightIconPadding - rightPadding;
            textRectF.Height = IsSelected ? Math.Abs((float)Height) : Math.Abs((float)Height - (float)StrokeThickness);
        }

        void SetTextBoundsForCloseButtonAndSelectionIndicator(RectF dirtyRect)
        {
            textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + _defaultCloseButtonWidth + leftIconPadding + rightIconPadding : (float)Padding.Left - (float)Padding.Right + _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
            textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
            textRectF.Width = (float)Width - _defaultSelectionIndicatorWidth - 2 * leftIconPadding - 2 * rightIconPadding - _defaultCloseButtonWidth;
            textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
        }

        void SetTextBoundsForSelectionIndicatorAndIcon(RectF dirtyRect)
        {
            if (ImageAlignment == Alignment.Default || ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left || ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + rightPadding : (float)Padding.Left - (float)Padding.Right + (float)ImageSize + _defaultSelectionIndicatorWidth + 2 * leftIconPadding + rightIconPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - (float)ImageSize - _defaultSelectionIndicatorWidth - 2 * leftIconPadding - 2 * rightIconPadding - (rightPadding - textAreaPadding));
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
            else if (ImageAlignment == Alignment.End || ImageAlignment == Alignment.Right)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + rightIconPadding + leftIconPadding + (float)ImageSize : (float)Padding.Left - (float)Padding.Right + _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - _defaultSelectionIndicatorWidth - (float)ImageSize - 2 * leftIconPadding - rightIconPadding - (rightPadding - textAreaPadding));
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
        }

        void SetTextBoundsForAllIndicators(RectF dirtyRect)
        {
            if (ImageAlignment == Alignment.Default || ImageAlignment == Alignment.Start || ImageAlignment == Alignment.Left || ImageAlignment == Alignment.Top || ImageAlignment == Alignment.Bottom)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + _defaultCloseButtonWidth + leftIconPadding + rightIconPadding : (float)Padding.Left - (float)Padding.Right + (float)ImageSize + _defaultSelectionIndicatorWidth + 2 * leftIconPadding + rightIconPadding - textAreaPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - (float)ImageSize - _defaultSelectionIndicatorWidth - _defaultCloseButtonWidth - 2 * leftIconPadding - rightIconPadding - (rightPadding - textAreaPadding));
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
            else if (ImageAlignment == Alignment.End || ImageAlignment == Alignment.Right)
            {
                textRectF.X = isRTL ? (float)Padding.Left - (float)Padding.Right + 2 * rightIconPadding + leftIconPadding + (float)ImageSize + _defaultCloseButtonWidth : (float)Padding.Left - (float)Padding.Right + _defaultSelectionIndicatorWidth + leftIconPadding + rightIconPadding;
                textRectF.Y = (float)Padding.Top - (float)Padding.Bottom - dirtyRect.Y - (float)textAlignmentPadding + (float)StrokeThickness / 2;
                textRectF.Width = Math.Abs((float)Width - (float)ImageSize - _defaultSelectionIndicatorWidth - _defaultCloseButtonWidth - 2 * leftIconPadding - rightIconPadding - (rightPadding - textAreaPadding));
                textRectF.Height = Math.Abs((float)Height - (float)StrokeThickness);
            }
        }

        void AdjustTextBoundsForVerticalAlignment(RectF dirtyRect)
        {
#if ANDROID || MACCATALYST || IOS
            if (VerticalTextAlignment == TextAlignment.End)
            {
                if ((Height - TextSize.Height) > 0)
                    textRectF.Y = (float)(Height - TextSize.Height - textPadding - Padding.Bottom);
                else
                    textRectF.Y = (float)(Padding.Top - Padding.Bottom);
            }
            else
            {
                if (Padding.Top - (float)Padding.Bottom + Height > Height)
                {
                    textRectF.Y = (float)(Height - Padding.Top - Padding.Bottom - TextSize.Height);
                }
                else
                {
                    textRectF.Y = (float)Padding.Top - (float)Padding.Bottom + normalTextPadding;
                }
            }
            textRectF.Height = Math.Abs((float)Height);
#endif
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
		/// <exclude/>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            chipSemanticsNodes = new();

            SemanticsNode chipNode = new SemanticsNode();
            chipNode.Id = 0;
            chipNode.IsTouchEnabled = true;
            chipNode.Text = GetChipNodeText();
            chipNode.Bounds = new Rect(0, 0, Width, Height);
            chipNode.OnClick = (chipNode) =>
            {
                if (chipType == null || chipType != SfChipsType.Input)
                {
                    if (_isDescriptionNotSetByUser)
                        SemanticScreenReader.Default.Announce(GetSemanticReaderText());
                    RaiseClicked(EventArgs.Empty);
                    InvalidateSemantics();
                }
            };
            chipSemanticsNodes.Add(chipNode);

            if (IsEnabled)
            {
                //semantics node for clear button
                if (ShowCloseButton)
                {
                    SemanticsNode chipClearButtonNode = new SemanticsNode();
                    chipClearButtonNode.Id = 1;
                    chipClearButtonNode.IsTouchEnabled = true;
                    chipClearButtonNode.Bounds = new Rect(_closeButtonRectF.X - (_closeButtonRadius),
                        _closeButtonRectF.Y - (_closeButtonRadius), _closeButtonRadius * 2, _closeButtonRadius * 2);
                    chipClearButtonNode.Text = $"close button double tap to remove {Text} chip";
                    chipClearButtonNode.OnClick = (clickedNode) =>
                    {
                        if (_isDescriptionNotSetByUser)
                            SemanticScreenReader.Default.Announce("close button pressed");
                        RaiseCloseButtonClicked(EventArgs.Empty);
                        InvalidateSemantics();
                    };
                    chipSemanticsNodes.Add(chipClearButtonNode);
                }
            }

            return chipSemanticsNodes;
        }

        internal override void UpdateTextBounds(RectF dirtyRect, TextAlignment verticalTextAlignment)
        {
            base.UpdateTextBounds(dirtyRect, VerticalTextAlignment);

            if (ShowCloseButton && !ShowIcon && !ShowSelectionIndicator)
            {
                SetTextBoundsForCloseButton(dirtyRect);
            }
            else if (ShowCloseButton && ShowIcon && !ShowSelectionIndicator)
            {
                SetTextBoundsForCloseButtonAndIcon(dirtyRect);
            }
            else if (ShowSelectionIndicator && !ShowIcon && !ShowCloseButton)
            {
                SetTextBoundsForSelectionIndicator(dirtyRect);
            }
            else if (ShowCloseButton && ShowSelectionIndicator && !ShowIcon)
            {
                SetTextBoundsForCloseButtonAndSelectionIndicator(dirtyRect);
            }
            else if (ShowSelectionIndicator && ShowIcon && !ShowCloseButton)
            {
                SetTextBoundsForSelectionIndicatorAndIcon(dirtyRect);
            }
            else if (ShowCloseButton && ShowIcon && ShowSelectionIndicator)
            {
                SetTextBoundsForAllIndicators(dirtyRect);
            }

            AdjustTextBoundsForVerticalAlignment(dirtyRect);
        }

        /// <summary>
        /// Touch action method for SfChip.
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
		/// <exclude/>
        public override void OnTouch(PointerEventArgs e)
        {
            base.OnTouch(e);
            Point touchPoint = e.TouchPoint;

            if (isRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                touchPoint.X = Width - touchPoint.X;
            }

            switch (e.Action)
            {
                case PointerActions.Pressed:
                    HandlePressedAction(e, touchPoint);
                    break;

                case PointerActions.Released:
                    HandleReleasedAction(e, touchPoint);
                    break;

                case PointerActions.Moved:
                    HandleMovedAction(touchPoint);
                    break;

                case PointerActions.Exited:
                    HandleExitedAction();
                    break;
            }
        }

        /// <summary>
        /// Measure content method.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
		/// <exclude/>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            base.MeasureContent(widthConstraint, heightConstraint);

            double width = CalculateWidth(widthConstraint);
            double height = CalculateHeight(heightConstraint);

#if ANDROID || WINDOWS || MACCATALYST || IOS
            if (IsCreatedInternally)
            {
                if (Children.Count > 0 && IsItemTemplate)
                {
                    foreach (var child in Children)
                    {
                       var size= child.Measure(widthConstraint, heightConstraint);
                        if (!size.Width.Equals(double.PositiveInfinity) && !size.Height.Equals(double.PositiveInfinity) && size.Width > 0 && size.Height > 0)
                        {
                            return new Size(size.Width, size.Height);

                        }
                    }
                }

            }
#endif
#if ANDROID
            UpdateBaseClip();
#endif

            return new Size(width, height);
        }

        /// <summary>
        /// Arrange content method.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns>the size.</returns>
		/// <exclude/>
        protected override Size ArrangeContent(Rect bounds)
        {
#if ANDROID || MACCATALYST || IOS
            CreateImageIcon();
#endif
            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// To change the visual state of the chip control.
        /// </summary>
		/// <exclude/>
        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();

            if (IsEnabled)
            {
                HandleEnabledState();
            }
            else
            {
                HandleDisabledState();
            }

            InvalidateDrawable();
        }

        /// <summary>
        /// Drawing methods of chip control.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The rect</param>
		/// <exclude/>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            canvas.Antialias = true;
            if (dirtyRect.Width > 0 && dirtyRect.Height > 0)
            {
                DrawBackground(canvas, dirtyRect);
                DrawCloseButton(canvas, dirtyRect);
                DrawSelectionIndicator(canvas, dirtyRect);
                if (IsKeyDown)
                {
                    DrawKeyDownBorder(canvas, dirtyRect);
                }
#if WINDOWS
                CreateImageIcon();
#endif
                UpdateBaseClip();


                if (effectsRenderer != null)
                {
                    effectsRenderer.ControlWidth = Width;
                    effectsRenderer.DrawEffects(canvas, ShowCloseButton);
                }
            }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Property changed method for close button color property.
        /// </summary>
        /// <param name="bindable">The original source of property changed event.</param>
        /// <param name="oldValue">The old value of close button color property.</param>
        /// <param name="newValue">The new value of close button color property.</param>
        static void OnCloseButtonColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfChip chip)
            {
                chip.UpdateCloseButtonColor();
                chip.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for show close button property.
        /// </summary>
        /// <param name="bindable">The original source of property changed event.</param>
        /// <param name="oldValue"> The old value of show close button property.</param>
        /// <param name="newValue">The new value of show close button property. </param>
        static void OnShowCloseButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfChip chip)
            {
                chip.InvalidateMeasure();
                chip.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for show selection indicator property.
        /// </summary>
        /// <param name="bindable">The original source of property changed event.</param>
        /// <param name="oldValue">The old value of show selection indicator property.</param>
        /// <param name="newValue">The new value of  show selection indicator property.</param>
        static void OnShowSelectionIndicatorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfChip chip)
            {
                chip.InvalidateMeasure();
                chip.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for  selection indicator color property.
        /// </summary>
        /// <param name="bindable">The original source of property changed event.</param>
        /// <param name="oldValue">The old value of selection indicator color property.</param>
        /// <param name="newValue">The new value of selection indicator color property.</param>
        static void OnSelectionIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfChip chip)
            {
                chip.UpdateSelectionIndicatorColor();
                chip.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for custom path property.
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        static void OnCloseButtonPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfChip chip)
            {
                chip._customPath = PathBuilder(chip.CloseButtonPath);
                chip.InvalidateDrawable();
            }
        }

        #endregion

        #region Interface Implementation

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfChipStyles();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {

        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {

        }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the close button is clicked/tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="CloseButtonClicked"/> event.
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfChip chip = new SfChip();
		/// chip.CloseButtonClicked += chip_CloseButtonClicked;
		/// 
		/// private async void chip_CloseButtonClicked(object sender, EventArgs e)
		/// {
		///		await DisplayAlert("Message", "Chip CloseButton Clicked", "close");
		///	}
		/// ]]></code>
		/// </example>
		public event EventHandler<EventArgs>? CloseButtonClicked;

        #endregion
    }
}
