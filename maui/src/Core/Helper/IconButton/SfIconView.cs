using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Specifies the type of sficons.
    /// </summary>
    internal enum SfIcon
    {
        /// <summary>
        /// Specifies the forward icon.
        /// </summary>
        Forward,

        /// <summary>
        /// Specifies the backward icon.
        /// </summary>
        Backward,

        /// <summary>
        /// Specifies the downward icon.
        /// </summary>
        Downward,

        /// <summary>
        /// Specifies the upward icon.
        /// </summary>
        Upward,

        /// <summary>
        /// Specifies the today icon.
        /// </summary>
        Today,

        /// <summary>
        /// Specifies the option icon.
        /// </summary>
        Option,

        /// <summary>
        /// Specifies the view button.
        /// </summary>
        Button,

        /// <summary>
        /// Specifies the combobox view.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Specifies the today button.
        /// </summary>
        TodayButton,

        /// <summary>
        /// Specifies the divider view.
        /// </summary>
        Divider,

        /// <summary>
        /// Specifies the week number text.
        /// </summary>
        WeekNumber,

        /// <summary>
        /// Specifies the cancel button.
        /// </summary>
        Cancel,

        /// <summary>
        /// Specifies the more icon.
        /// </summary>
        More,
    }

    /// <summary>
    /// Represents a class which contains information of icons view.
    /// </summary>
    internal class SfIconView : SfView
    {
        #region Fields

        /// <summary>
        /// This bool is used to identify whether the combobox dropdown is open or not.
        /// </summary>
        bool _isOpen;

        /// <summary>
        /// The icon size.
        /// </summary>
        double _iconSize => _textStyle.FontSize / 1.5;

        /// <summary>
        /// The border color. While the border color is null, then the border color is considered as transparent. The border color is transperent then the border color considered as light gray
        /// </summary>
        Color? _borderColor;

        /// <summary>
        /// The today highlight color.
        /// </summary>
        Color _highlightColor;

        /// <summary>
        /// The selection highlight text color.
        /// </summary>
        Color? _highlightTextColor;

        /// <summary>
        /// The icon Text.
        /// </summary>
        string _text;

        /// <summary>
        /// Holds that the view is visible or not.
        /// </summary>
        bool _visibility;

        /// <summary>
        /// Icon text style value.
        /// </summary>
        TextStyle _textStyle;

        /// <summary>
        /// Defines the icon text is selected or not.
        /// </summary>
        bool _isSelected;

        /// <summary>
        /// Defines the icon text need to be highlight like tab.
        /// </summary>
        bool _isTabHighlight;

        /// <summary>
        /// Defines the flow direction is RTL or not.
        /// </summary>
        bool _isRTL;

        /// <summary>
        /// Used to trim week number text while SfIcon type is Week number.
        /// It is applicable for SfScheduler.
        /// </summary>
        readonly Func<double, string, string>? _getTrimWeekNumberText;

        /// <summary>
        /// Defines the semantic node.
        /// </summary>
        readonly SemanticsNode? _semanticsNode;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfIconView"/> class.
        /// </summary>
        /// <param name="icon">The sficon.</param>
        /// <param name="textStyle">The header text style.</param>
        /// <param name="text">The icon text.</param>
        /// <param name="borderColor">Border color for the button.</param>
        /// <param name="highlightColor">Highlight color to highlight the selected icon.</param>
        /// <param name="isRTL">Defines the icon flow direction is RTL.</param>
        /// <param name="isSelected">Defined the icon is selected or not.</param>
        /// <param name="getTrimWeekNumberText">Used to trim the week number text based on available width and height.</param>
        /// <param name="isTabHighlight">Used to trim the week number text based on available width and height.</param>
        /// <param name="semanticsNode">Semantics information for the button.</param>
        /// <param name="highlightTextColor">The highlight text color.</param>
        internal SfIconView(SfIcon icon, ITextElement textStyle, string text, Color? borderColor, Color highlightColor, bool isSelected = false, bool isRTL = false, Func<double, string, string>? getTrimWeekNumberText = null, bool isTabHighlight = false, SemanticsNode? semanticsNode = null, Color? highlightTextColor = null)
        {
#if __IOS__
            IgnoreSafeArea = true;
#endif
            Icon = icon;
            _textStyle = new TextStyle()
            {
                TextColor = textStyle.TextColor,
                FontSize = textStyle.FontSize,
                FontAttributes = textStyle.FontAttributes,
                FontFamily = textStyle.FontFamily,
                FontAutoScalingEnabled = textStyle.FontAutoScalingEnabled,
            };
            _text = text;
            _isRTL = isRTL;
            _isSelected = isSelected;
            _borderColor = borderColor;
            _highlightColor = highlightColor;
            _highlightTextColor = highlightTextColor;
            _getTrimWeekNumberText = getTrimWeekNumberText;
            _isTabHighlight = isTabHighlight;
            _visibility = true;
            DrawingOrder = DrawingOrder.BelowContent;
            _semanticsNode = semanticsNode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the textstyle.
        /// </summary>
        internal TextStyle TextStyle
        {
            get
            {
                return _textStyle;
            }

            set
            {
                if (_textStyle.FontSize == value.FontSize && _textStyle.TextColor == value.TextColor && _textStyle.FontAttributes == value.FontAttributes && _textStyle.FontFamily == value.FontFamily && _textStyle.FontAutoScalingEnabled == value.FontAutoScalingEnabled)
                {
                    return;
                }

                _textStyle = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        internal Color? BorderColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
                if (_borderColor == value)
                {
                    return;
                }

                _borderColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the highlight color.
        /// </summary>
        internal Color HighlightColor
        {
            get
            {
                return _highlightColor;
            }

            set
            {
                if (_highlightColor == value)
                {
                    return;
                }

                _highlightColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the icon highlight text color.
        /// </summary>
        internal Color? HighlightTextColor
        {
            get
            {
                return _highlightTextColor;
            }

            set
            {
                if (_highlightTextColor == value)
                {
                    return;
                }

                _highlightTextColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the sficon text.
        /// </summary>
        internal string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text == value)
                {
                    return;
                }

                _text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combobox dropdown is open or not.
        /// </summary>
        internal bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            set
            {
                if (_isOpen == value)
                {
                    return;
                }

                _isOpen = value;
            }
        }

        /// <summary>
        /// Gets or sets a value whether the is selected or not.
        /// </summary>
        internal bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value whether the is tab highlight or not.
        /// </summary>
        internal bool IsTabHighlight
        {
            get
            {
                return _isTabHighlight;
            }

            set
            {
                if (_isTabHighlight == value)
                {
                    return;
                }

                _isTabHighlight = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value whether the is RTL or not.
        /// </summary>
        internal bool IsRTL
        {
            get
            {
                return _isRTL;
            }

            set
            {
                if (_isRTL == value)
                {
                    return;
                }

                _isRTL = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view is visible or not.
        /// TODO: IsVisible property breaks in 6.0.400 release.
        /// Issue link -https://github.com/dotnet/maui/issues/7507
        /// -https://github.com/dotnet/maui/issues/8044
        /// -https://github.com/dotnet/maui/issues/7482
        /// </summary>
        internal bool Visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
#if !__MACCATALYST__
                IsVisible = value;
#endif
                _visibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the sficon.
        /// </summary>
        internal SfIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the view selection corner radius.
        /// </summary>
        internal double SelectionCornerRadius { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update icon style.
        /// </summary>
        /// <param name="textStyle">The text style value.</param>
        internal void UpdateStyle(ITextElement textStyle)
        {
            TextStyle = new TextStyle()
            {
                TextColor = textStyle.TextColor,
                FontSize = textStyle.FontSize,
                FontAttributes = textStyle.FontAttributes,
                FontFamily = textStyle.FontFamily,
                FontAutoScalingEnabled = textStyle.FontAutoScalingEnabled,
            };
        }

        /// <summary>
        /// Method to update icon color.
        /// </summary>
        /// <param name="iconColor">The icon color.</param>
        internal void UpdateIconColor(Color iconColor)
        {
            TextStyle = new TextStyle()
            {
                TextColor = iconColor,
                FontSize = _textStyle.FontSize,
                FontAttributes = _textStyle.FontAttributes,
                FontFamily = _textStyle.FontFamily,
                FontAutoScalingEnabled = _textStyle.FontAutoScalingEnabled,
            };
        }

        /// <summary>
        /// Method to update semantics.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="isTouchEnabled">To check istouch enabled or not.</param>
        internal void UpdateSemantics(string? text = null, bool? isTouchEnabled = null)
        {
            if (_semanticsNode == null)
            {
                return;
            }

            if (text != null && _semanticsNode.Text != text)
            {
                _semanticsNode.Text = text;
                InvalidateSemantics();
            }

            if (isTouchEnabled != null && _semanticsNode.IsTouchEnabled != isTouchEnabled)
            {
                _semanticsNode.IsTouchEnabled = (bool)isTouchEnabled;
                InvalidateSemantics();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to draw the forward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawForward(float width, float height, ICanvas canvas)
        {
            float arrowHeight = (float)_iconSize;
            float arrowWidth = arrowHeight / 2;
            float centerPosition = height / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, topPosition);
            path.LineTo(rightPosition, centerPosition);
            path.LineTo(leftPosition, bottomPosition);
            canvas.DrawPath(path);
            SelectionCornerRadius = 5;
        }

        /// <summary>
        /// Method to draw the backward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawBackward(float width, float height, ICanvas canvas)
        {
            float arrowHeight = (float)_iconSize;
            float arrowWidth = arrowHeight / 2;
            float centerPosition = height / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(rightPosition, topPosition);
            path.LineTo(leftPosition, centerPosition);
            path.LineTo(rightPosition, bottomPosition);
            canvas.DrawPath(path);
            SelectionCornerRadius = 5;
        }

        /// <summary>
        /// Method to draw the downward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawDownward(float width, float height, ICanvas canvas)
        {
            float arrowWidth = (float)_iconSize;
            float arrowHeight = arrowWidth / 2;
            float centerPosition = width / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, topPosition);
            path.LineTo(centerPosition, bottomPosition);
            path.LineTo(rightPosition, topPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the upward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawUpward(float width, float height, ICanvas canvas)
        {
            float arrowWidth = (float)_iconSize;
            float arrowHeight = arrowWidth / 2;
            float centerPosition = width / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, bottomPosition);
            path.LineTo(centerPosition, topPosition);
            path.LineTo(rightPosition, bottomPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the today icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawToday(float width, float height, ICanvas canvas)
        {
            float iconSize = (float)_iconSize;
            float secondaryRectSize = iconSize / 5;
            float leftPosition = (width / 2) - (iconSize / 2);
            float rightPosition = leftPosition + iconSize;
            float topPosition = (height / 2) - (iconSize / 2);
            canvas.DrawRoundedRectangle(leftPosition, topPosition, iconSize, iconSize, 1);
            canvas.DrawLine(leftPosition, topPosition + secondaryRectSize, rightPosition, topPosition + secondaryRectSize);
            canvas.DrawLine(leftPosition + secondaryRectSize, topPosition, leftPosition + secondaryRectSize, topPosition - secondaryRectSize);
            canvas.DrawLine(rightPosition - secondaryRectSize, topPosition, rightPosition - secondaryRectSize, topPosition - secondaryRectSize);
            canvas.FillRectangle(leftPosition + secondaryRectSize, topPosition + (2 * secondaryRectSize), secondaryRectSize, secondaryRectSize);
        }

        /// <summary>
        /// Method to draw the option icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawOption(float width, float height, ICanvas canvas)
        {
            float totalHeight = (float)_iconSize;
            float radius = totalHeight / 8;
            float centerYPosition = height / 2;
            float centerXPosition = width / 2;
            float topPosition = (height / 2) - (totalHeight / 2);
            float bottomPosition = (height / 2) + (totalHeight / 2);
            canvas.FillCircle(centerXPosition, topPosition + radius, radius);
            canvas.FillCircle(centerXPosition, centerYPosition, radius);
            canvas.FillCircle(centerXPosition, bottomPosition - radius, radius);
        }

        /// <summary>
        /// Method to draw the option icon. Used for ToolBar button.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawMore(float width, float height, ICanvas canvas)
        {
            float radius = 1.2f;
            float centerYPosition = height / 2;
            float centerXPosition = width / 2;
            canvas.FillCircle(centerXPosition - radius - 3, centerYPosition, radius);
            canvas.FillCircle(centerXPosition, centerYPosition, radius);
            canvas.FillCircle(centerXPosition + radius + 3, centerYPosition, radius);
        }

        /// <summary>
        /// Method to draw the responsive UI today button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawTodayButton(float width, float height, ICanvas canvas)
        {
            DrawTilesButton(width, height, canvas);
        }

        /// <summary>
        /// Method to draw the divider view.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawDivider(float width, float height, ICanvas canvas)
        {
            canvas.StrokeSize = 1;
            canvas.SaveState();
            Color strokeColor = GetCellBorderColor();
            canvas.StrokeColor = strokeColor;
            canvas.FillColor = strokeColor;
            float startPosition = 2;
            canvas.DrawLine(width / 2, startPosition, width / 2, height - startPosition);
            canvas.RestoreState();
        }

        /// <summary>
        /// Get the stroke color based on cell border color.
        /// </summary>
        /// <returns>Return cell border color value.</returns>
        Color GetCellBorderColor()
        {
            if (_borderColor == null)
            {
                return Colors.Transparent;
            }
            else if (_borderColor != Colors.Transparent)
            {
                return _borderColor;
            }

            return Colors.LightGray;
        }

        /// <summary>
        /// Method to draw the combobox button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawComboBox(float width, float height, ICanvas canvas)
        {
            var padding = 5;
            var margin = 2 * padding;
            var dropDownIConWidth = 10;
            canvas.StrokeSize = 1;

            float startPosition = 1;
            canvas.SaveState();
            TextStyle iconTextStyle = new TextStyle()
            {
                FontSize = _textStyle.FontSize,
                TextColor = _isSelected ? HighlightColor : _textStyle.TextColor,
                FontAttributes = _textStyle.FontAttributes,
                FontFamily = _textStyle.FontFamily,
                FontAutoScalingEnabled = _textStyle.FontAutoScalingEnabled
            };
            //// To show the borderline the start position will be 2, the height and width is adjusted to show the border.
            DrawText(canvas, Text, iconTextStyle, new RectF(margin, 0, width - 2, height), HorizontalAlignment.Left, VerticalAlignment.Center);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = iconTextStyle.TextColor;
            canvas.FillColor = iconTextStyle.TextColor;
            //// To show the borderline the start position will be 2, the width and height is adjusted to show the border. The corner radius is 5
            SelectionCornerRadius = 5;
            canvas.DrawRoundedRectangle(startPosition, startPosition, width - 2, height - 2, (float)SelectionCornerRadius);
            canvas.RestoreState();
            canvas.StrokeColor = _textStyle.TextColor.WithAlpha(0.5f);
            canvas.FillColor = _textStyle.TextColor.WithAlpha(0.5f);

            if (!IsOpen)
            {
                canvas.DrawInverseTriangle(new RectF(width - (2 * margin), (height - 4) / 2, dropDownIConWidth, dropDownIConWidth / 2), false);
            }
            else
            {
                canvas.DrawTriangle(new RectF(width - (2 * margin), (height - 4) / 2, dropDownIConWidth, dropDownIConWidth / 2), false);
            }
        }

        /// <summary>
        /// Method to draw text.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="text">The draw text.</param>
        /// <param name="textStyle">The textstyle.</param>
        /// <param name="rect">Rect value.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignemnt.</param>
        void DrawText(ICanvas canvas, string text, ITextElement textStyle, Rect rect, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
        {
            if (rect.Height <= 0 || rect.Width <= 0)
            {
                return;
            }

            canvas.DrawText(text, rect, horizontalAlignment, verticalAlignment, textStyle);
        }

        /// <summary>
        /// Method to draw the tiles button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawTilesButton(float width, float height, ICanvas canvas)
        {
            canvas.StrokeSize = 1;
            Color textColor, strokeColor;

            if (_isSelected)
            {
                textColor = _highlightTextColor ?? HighlightColor;
                strokeColor = HighlightColor;
            }
            else
            {
                textColor = _textStyle.TextColor;
                strokeColor = GetCellBorderColor();
            }

            float startPosition = 1;
            bool isBorderEnabled = strokeColor != Colors.Transparent;
            if (_isTabHighlight && _textStyle.TextColor != Colors.Transparent)
            {
                float highlightHeight = 3;
                double highlightCorner = height / 2;
                float highlightPadding = width * 0.3f;
                float highlightWidth = width * 0.4f;
                Rect highlight = new Rect(highlightPadding, height - highlightHeight - (isBorderEnabled ? 1 : 0), highlightWidth, highlightHeight);
                canvas.FillColor = _textStyle.TextColor;
                canvas.FillRoundedRectangle(highlight, highlightCorner, highlightCorner, highlightCorner, highlightCorner);
            }

            if (isBorderEnabled || IsSelected)
            {
                canvas.SaveState();
                canvas.StrokeColor = strokeColor;
                canvas.FillColor = strokeColor;

                //// To show the borderline the start position will be 2, the width and height is adjusted to show the border. The corner radius is 5
                SelectionCornerRadius = height / 2;
                if (IsSelected)
                {
                    canvas.FillColor = HighlightColor;
                    canvas.FillRoundedRectangle(startPosition, startPosition, width - 2, height - 2, (float)SelectionCornerRadius);
                }
                else
                {
                    canvas.DrawRoundedRectangle(startPosition, startPosition, width - 2, height - 2, (float)SelectionCornerRadius);
                }

                canvas.RestoreState();
            }

            canvas.SaveState();
            TextStyle iconTextStyle = new TextStyle() { FontSize = _textStyle.FontSize, TextColor = textColor, FontAttributes = _textStyle.FontAttributes, FontFamily = _textStyle.FontFamily , FontAutoScalingEnabled = _textStyle.FontAutoScalingEnabled };
            //// To show the borderline the start position will be 2, the height is adjusted to show the border.
            DrawText(canvas, Text, iconTextStyle, new RectF(startPosition, startPosition, width - 2, height - 2), HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.RestoreState();
        }

        /// <summary>
        /// Method to draw the week number text.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        void DrawWeekNumber(float width, float height, ICanvas canvas)
        {
            string text = Text;
            if (_getTrimWeekNumberText != null)
            {
                text = _getTrimWeekNumberText(width, text);
            }

#if WINDOWS
            //// TODO: horizontal and vertical alignments are not center aligned with draw text method. Hence used pading values explicitly.
            HorizontalAlignment alignment = _isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            double startPosition = _isRTL ? 0 : 5;
            DrawText(canvas, text, _textStyle, new Rect(startPosition, 1, width - 5, height - 1), alignment, VerticalAlignment.Top);
#else
            DrawText(canvas, text, _textStyle, new RectF(0, 0, width, height), HorizontalAlignment.Center, VerticalAlignment.Center);
#endif
        }

        /// <summary>
        /// Draw clear button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF leftline = new Point(0, 0);
            PointF rightline = new Point(0, 0);
            float padding = 15f;

            leftline.X = rectF.X + padding;
            leftline.Y = rectF.Y + padding;
            rightline.X = rectF.X + rectF.Width - padding;
            rightline.Y = rectF.Y + rectF.Height - padding;
            canvas.DrawLine(leftline, rightline);

            leftline.X = rectF.X + padding;
            leftline.Y = rectF.Y + rectF.Height - padding;
            rightline.X = rectF.X + rectF.Width - padding;
            rightline.Y = rectF.Y + padding;
            canvas.DrawLine(leftline, rightline);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the sficon.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
#if __MACCATALYST__
            //// TODO: IsVisibility property breaks in 6.0.400 release.
            if (!Visibility)
            {
                return;
            }

#endif
            canvas.SaveState();

            canvas.StrokeSize = 1.5f;
            canvas.FillColor = _textStyle.TextColor;
            canvas.StrokeColor = _textStyle.TextColor;
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            switch (Icon)
            {
                case SfIcon.Forward:
                    {
                        DrawForward(width, height, canvas);
                        break;
                    }

                case SfIcon.Backward:
                    {
                        DrawBackward(width, height, canvas);
                        break;
                    }

                case SfIcon.Downward:
                    {
                        DrawDownward(width, height, canvas);
                        break;
                    }

                case SfIcon.Upward:
                    {
                        DrawUpward(width, height, canvas);
                        break;
                    }

                case SfIcon.Today:
                    {
                        DrawToday(width, height, canvas);
                        break;
                    }

                case SfIcon.Option:
                    {
                        DrawOption(width, height, canvas);
                        break;
                    }

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
                case SfIcon.Button:
                    {
                        DrawTilesButton(width, height, canvas);
                        break;
                    }
#endif
                case SfIcon.ComboBox:
                    {
                        DrawComboBox(width, height, canvas);
                        break;
                    }

                case SfIcon.TodayButton:
                    {
                        DrawTodayButton(width, height, canvas);
                        break;
                    }

                case SfIcon.Divider:
                    {
                        DrawDivider(width, height, canvas);
                        break;
                    }

                case SfIcon.WeekNumber:
                    {
                        DrawWeekNumber(width, height, canvas);
                        break;
                    }
                case SfIcon.Cancel:
                    {
                        DrawClearButton(canvas, dirtyRect);
                        break;
                    }
                case SfIcon.More:
                    {
                        DrawMore(width, height, canvas);
                        break;
                    }
            }

            canvas.RestoreState();
        }

        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            if (_semanticsNode == null)
            {
                return null;
            }

            Rect rect = new Rect(0, 0, width, height);
            _semanticsNode.Bounds = rect;
            return new List<SemanticsNode>() { _semanticsNode };
        }

        #endregion
    }
}