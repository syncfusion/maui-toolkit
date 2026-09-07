using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker header layout.
    /// </summary>
    internal class HeaderLayout : SfView
    {
        #region Fields

        /// <summary>
        /// The separator line thickness.
        /// </summary>
        const int StrokeThickness = 1;

        /// <summary>
        /// The default width and height that should be reserved for the close button.
        /// </summary>
        const double _closeButtonSize = 40d;
 
        /// <summary>
        /// Default padding applied around the close button.
        /// </summary>
        const double _closeButtonPadding = 8d;

        /// <summary>
        /// The optional close button rendered inside the header.
        /// </summary>
        PickerCloseButton? _closeButton;

        /// <summary>
        /// The header view.
        /// </summary>
        readonly IHeaderView _pickerInfo;

        /// <summary>
        /// Gets or sets the virtual header layout semantic nodes.
        /// </summary>
        List<SemanticsNode>? _semanticsNodes;

        /// <summary>
        /// Gets or sets the size of the semantic.
        /// </summary>
        Size _semanticsSize = Size.Zero;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLayout"/> class.
        /// </summary>
        /// <param name="pickerInfo">The picker header view details.</param>
        internal HeaderLayout(IHeaderView pickerInfo)
        {
            _pickerInfo = pickerInfo;
            if (_pickerInfo.HeaderView.Parent != null)
            {
                DrawingOrder = DrawingOrder.BelowContent;
                //// Draw header background/content below child views so child elements
                //// like the close button (which should overlay) remain visible.
                AutomationId = $"{PickerHelper.GetParentName(_pickerInfo.HeaderView.Parent)} HeaderView";
                BackgroundColor = Colors.Transparent;
            }
            else
            {
                DrawingOrder = DrawingOrder.BelowContent;
            }

            //// Create a template view by checking the template property else default view is created.
            if (_pickerInfo.HeaderTemplate != null)
            {
                InitializeTemplateView();
            }
            else
            {
                AddChildren();
            }
#if IOS
            IgnoreSafeArea = true;
#endif
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the header view.
        /// </summary>
        internal void InvalidateHeaderView()
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the header date text value.
        /// </summary>
        internal void UpdateHeaderDateText()
        {
            if (Children.Count != 2 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (Children[0] is SfIconButton dateButton && dateButton.EffectsView != null && dateButton.EffectsView.Content is SfIconView iconView)
            {
                iconView.Text = _pickerInfo.HeaderView.DateText;
                SemanticProperties.SetDescription(dateButton, _pickerInfo.HeaderView.DateText);
                iconView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update the header time text value.
        /// </summary>
        internal void UpdateHeaderTimeText()
        {
            if (Children.Count != 2 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (Children[1] is SfIconButton timeButton && timeButton.EffectsView != null && timeButton.EffectsView.Content is SfIconView iconView)
            {
                iconView.Text = _pickerInfo.HeaderView.TimeText;
                SemanticProperties.SetDescription(timeButton, _pickerInfo.HeaderView.TimeText);
                iconView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Update icon button text style.
        /// </summary>
        internal void UpdateIconButtonTextStyle()
        {
            if (Children.Count != 2 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (Children[0] is SfIconButton dateButton && dateButton.EffectsView != null && dateButton.EffectsView.Content is SfIconView dateIconView)
            {
                dateButton.UpdateStyle(dateIconView.IsTabHighlight ? _pickerInfo.HeaderView.SelectionTextStyle : _pickerInfo.HeaderView.TextStyle);
            }

            if (Children[1] is SfIconButton timeButton && timeButton.EffectsView != null && timeButton.EffectsView.Content is SfIconView iconView)
            {
                timeButton.UpdateStyle(iconView.IsTabHighlight ? _pickerInfo.HeaderView.SelectionTextStyle : _pickerInfo.HeaderView.TextStyle);
            }
        }

        /// <summary>
        /// Used to reset the header interaction highlight.
        /// </summary>
        internal void ResetHeaderHighlight()
        {
            if (_pickerInfo is SfDateTimePicker dateTimePicker)
            {
                if (dateTimePicker.ActiveView == DateTimePickerView.Time)
                {
                    OnTimeButtonClicked("Time");
                }
                else
                {
                    OnDateButtonClicked("Date");
                }
            }
            else
            {
                OnDateButtonClicked("Date");
            }
        }

        /// <summary>
        /// Updates the close button visibility based on the picker configuration.
        /// </summary>
        internal void UpdateCloseButton()
        {
            if (!CanShowCloseButton())
            {
                if (_closeButton != null && this.Children.Contains(_closeButton))
                {
                    Children.Remove(_closeButton);
                    InvalidateMeasure();
                    InvalidateDrawable();
                    _closeButton = null;
                }

                return;
            }

            if (_pickerInfo is not PickerBase picker)
            {
                return;
            }

            if (_closeButton == null)
            {
                _closeButton = new PickerCloseButton(picker);
                Children.Add(_closeButton);
            }
        }

        /// <summary>
        /// Updates the icon of the close button.
        /// </summary>
        internal void UpdateCloseButtonIcon()
        {
            if (!CanShowCloseButton())
            {
                return;
            }
 
            if (_closeButton == null)
            {
                UpdateCloseButton();
                return;
            }
 
            _closeButton.UpdateIcon();
            this.InvalidateDrawable();
         }

        /// <summary>
        /// Method to create a template view.
        /// </summary>
        internal void InitializeTemplateView()
        {
            View? headerTemplateView = null;
            //// Need to ensure the template is not null
            if (_pickerInfo.HeaderTemplate == null)
            {
                return;
            }

            //// Clear the previous data in headerlayout.
            if (Children.Count > 0)
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    Children.RemoveAt(i);
                }
            }

            DataTemplate headerTemplate = _pickerInfo.HeaderTemplate;

            switch (_pickerInfo)
            {
                case SfDatePicker datePicker:
                    headerTemplateView = PickerHelper.CreateLayoutTemplateViews(headerTemplate, _pickerInfo.HeaderView, datePicker);
                    break;
                case SfDateTimePicker:
                    SfDateTimePicker dateTimePicker = (SfDateTimePicker)_pickerInfo;
                    headerTemplateView = PickerHelper.CreateLayoutTemplateViews(headerTemplate, _pickerInfo.HeaderView, dateTimePicker);
                    break;
                case SfPicker picker:
                    headerTemplateView = PickerHelper.CreateLayoutTemplateViews(headerTemplate, _pickerInfo.HeaderView, picker);
                    break;
                case SfTimePicker timePicker:
                    headerTemplateView = PickerHelper.CreateLayoutTemplateViews(headerTemplate, _pickerInfo.HeaderView, timePicker);
                    break;
            }

            if (headerTemplateView != null && Children.Count == 0)
            {
                Children.Add(headerTemplateView);
                InvalidateDrawable();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the header should render the close button.
        /// </summary>
        /// <returns><c>true</c> when the close button can be displayed; otherwise, <c>false</c>.</returns>
        bool CanShowCloseButton()
        {
            if (_pickerInfo is not PickerBase picker)
            {
                return false;
            }

            //// Do not show the close button when a header template is applied
            //// as templates control their own layout and content.
            if (_pickerInfo.HeaderTemplate != null)
            {
                return false;
            }

            if (picker is SfDateTimePicker)
            {
                return false;
            }

            //// Show close button only when explicitly enabled and when the picker
            //// is in Dialog or RelativeDialog mode (not Default).
            return picker.ShowCloseButton && picker.Mode != PickerMode.Default;
        }

        /// <summary>
        /// Arranges the close button inside the header bounds.
        /// </summary>
        /// <param name="width">The available width.</param>
        /// <param name="height">The available height.</param>
        void ArrangeCloseButton(double width, double height)
        {
            if (_closeButton == null || !CanShowCloseButton())
            {
                return;
            }

            double buttonWidth = _closeButtonSize;
            double buttonHeight = height < _closeButtonSize ? height : _closeButtonSize;
            double xPosition = _pickerInfo.IsRTLLayout ? _closeButtonPadding : width - buttonWidth - _closeButtonPadding;
            xPosition = Math.Max(0, xPosition);
            double yPosition = (height - buttonHeight) / 2;
            yPosition = Math.Max(0, yPosition);
            _closeButton.Arrange(new Rect(xPosition, yPosition, buttonWidth, buttonHeight));
        }

        /// <summary>
        /// Method to draw the button.
        /// </summary>
        void AddChildren()
        {
            if (Children.Count != 0 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.DateText))
            {
                SemanticsNode buttonViewNode = new SemanticsNode()
                {
                    Id = 0,
                    Text = _pickerInfo.HeaderView.DateText,
                    IsTouchEnabled = true,
                    OnClick = OnDateNodeButtonClicked,
                };

                bool isDateActive = true;
                if (_pickerInfo is SfDateTimePicker dateTimePicker)
                {
                    isDateActive = dateTimePicker.ActiveView == DateTimePickerView.Date;
                }

                var dateStyle = isDateActive ? _pickerInfo.HeaderView.SelectionTextStyle : _pickerInfo.HeaderView.TextStyle;
                SfIconView buttonView = new SfIconView(SfIcon.TodayButton, dateStyle, _pickerInfo.HeaderView.DateText.ToString(), null, Colors.Transparent, isTabHighlight: isDateActive, semanticsNode: buttonViewNode);
                buttonView.DrawingOrder = DrawingOrder.AboveContent;
                SfIconButton button = new SfIconButton(buttonView);
                button.AutomationId = $"DateTimePicker DateHeaderView";
                Add(button);
                button.Clicked = OnDateButtonClicked;
            }

            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.TimeText))
            {
                SemanticsNode buttonViewNode = new SemanticsNode()
                {
                    Id = 0,
                    Text = _pickerInfo.HeaderView.TimeText,
                    IsTouchEnabled = true,
                    OnClick = OnTimeNodeButtonClicked,
                };
                
                bool isTimeActive = false;
                if (_pickerInfo is SfDateTimePicker dateTimePicker)
                {
                    isTimeActive = dateTimePicker.ActiveView == DateTimePickerView.Time;
                }

                var timeStyle = isTimeActive ? _pickerInfo.HeaderView.SelectionTextStyle : _pickerInfo.HeaderView.TextStyle;
                SfIconView buttonView = new SfIconView(SfIcon.TodayButton, timeStyle, _pickerInfo.HeaderView.TimeText.ToString(), null, Colors.Transparent, isTabHighlight: isTimeActive, semanticsNode: buttonViewNode);
                buttonView.DrawingOrder = DrawingOrder.AboveContent;
                SfIconButton button = new SfIconButton(buttonView);
                button.AutomationId = $"DateTimePicker TimeHeaderView";
                Add(button);
                button.Clicked = OnTimeButtonClicked;
            }

            this.UpdateCloseButton();
            //// While remove and add header view, need to reset the header highlight and columns based on active view.
            ResetHeaderHighlight();
        }

        /// <summary>
        /// Method to update the time button after clicked.
        /// </summary>
        /// <param name="buttonText">The button text.</param>
        void OnTimeButtonClicked(string buttonText)
        {
            OnTimeButtonClicked();
        }

        /// <summary>
        /// Method to update the date button after clicked.
        /// </summary>
        /// <param name="buttonText">The button text.</param>
        void OnDateButtonClicked(string buttonText)
        {
            OnDateButtonClicked();
        }

        /// <summary>
        /// Occurs when date button clicked while accessibility enabled.
        /// </summary>
        /// <param name="node">Date button semantic node.</param>
        void OnDateNodeButtonClicked(SemanticsNode node)
        {
            OnDateButtonClicked();
        }

        /// <summary>
        /// Occurs when time button clicked while accessibility enabled.
        /// </summary>
        /// <param name="node">Time button semantic node.</param>
        void OnTimeNodeButtonClicked(SemanticsNode node)
        {
            OnTimeButtonClicked();
        }

        /// <summary>
        /// Method to update the time button after clicked.
        /// </summary>
        void OnTimeButtonClicked()
        {
            _pickerInfo.OnTimeButtonClicked();

            if (Children.Count != 2 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (Children[0] is SfIconButton dateButton && dateButton.EffectsView != null && dateButton.EffectsView.Content is SfIconView dateIconView)
            {
                dateButton.UpdateStyle(_pickerInfo.HeaderView.TextStyle);
                dateIconView.IsTabHighlight = false;
            }

            if (Children[1] is SfIconButton timeButton && timeButton.EffectsView != null && timeButton.EffectsView.Content is SfIconView iconView)
            {
                timeButton.UpdateStyle(_pickerInfo.HeaderView.SelectionTextStyle);
                iconView.IsTabHighlight = true;
            }

        }

        /// <summary>
        /// Method to update the date button after clicked.
        /// </summary>
        void OnDateButtonClicked()
        {
            _pickerInfo.OnDateButtonClicked();

            if (Children.Count != 2 || _pickerInfo.HeaderTemplate != null)
            {
                return;
            }

            if (Children[1] is SfIconButton timeButton && timeButton.EffectsView != null && timeButton.EffectsView.Content is SfIconView iconView)
            {
                timeButton.UpdateStyle(_pickerInfo.HeaderView.TextStyle);
                iconView.IsTabHighlight = false;
            }

            if (Children[0] is SfIconButton dateButton && dateButton.EffectsView != null && dateButton.EffectsView.Content is SfIconView dateIconView)
            {
                dateButton.UpdateStyle(_pickerInfo.HeaderView.SelectionTextStyle);
                dateIconView.IsTabHighlight = true;
            }

        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the separator line for header and picker layout.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The dirtyRect.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (dirtyRect.Width == 0 || dirtyRect.Height == 0)
            {
                return;
            }

            canvas.SaveState();
            float height = dirtyRect.Height;
            float width = dirtyRect.Width;
            float xPosition = dirtyRect.Left;
            float yPosition = dirtyRect.Top;
            bool isDividerEnabled = _pickerInfo.HeaderView.DividerColor != Colors.Transparent;
            //// To avoid the separator line overlapping with the text, the height of the rectangle is reduced by the stroke thickness.
            Rect rectangle = new Rect(xPosition, yPosition, width, height - (isDividerEnabled ? StrokeThickness : 0));
            Color fillColor = _pickerInfo.HeaderView.Background.ToColor();
            if (fillColor != Colors.Transparent && _pickerInfo.HeaderTemplate == null)
            {
                canvas.FillColor = fillColor;
                canvas.FillRectangle(rectangle);
            }

            //// Compute text bounds. Prefer to keep the header text visually centered across
            //// the full header width, but trim it so it won't overlap an overlayed close button.
            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.Text) && _pickerInfo.HeaderTemplate == null)
            {
                string originalText = _pickerInfo.HeaderView.Text;
                double maxTextWidth = width;
                double reserved = 0;

                if (CanShowCloseButton() && _closeButton != null)
                {
                    reserved = _closeButtonSize + _closeButtonPadding;
                    maxTextWidth = Math.Max(0, width - reserved);
                }

                string headerText = PickerHelper.TrimText(originalText, maxTextWidth, _pickerInfo.HeaderView.TextStyle);
                var textStyle = _pickerInfo.HeaderView.TextStyle;
                double measuredWidth = headerText.Measure(textStyle).Width;

                //// If the original text fits completely far enough from the close button,
                //// keep it visually centered across the full header. Otherwise center it
                //// inside the safe area (excluding reserved space for the overlay close button)
                //// to avoid overlap when centering.
                double availableForFullCenter = width - (2 * reserved);
                bool usedFullCenter = headerText == originalText && measuredWidth <= availableForFullCenter;
                SemanticProperties.SetDescription(this, headerText);
                if (usedFullCenter)
                {
                    Rect textRect = new Rect(xPosition, yPosition, width, height - (isDividerEnabled ? StrokeThickness : 0));
                    canvas.DrawText(headerText, textRect, HorizontalAlignment.Center, VerticalAlignment.Center, textStyle);
                }
                else
                {
                    double safeLeft = xPosition;
                    double safeWidth = Math.Max(0, width - reserved);
                    if (_pickerInfo.IsRTLLayout)
                    {
                        safeLeft += reserved;
                    }

                    Rect safeRect = new Rect(safeLeft, yPosition, safeWidth, height - (isDividerEnabled ? StrokeThickness : 0));
                    canvas.DrawText(headerText, safeRect, HorizontalAlignment.Center, VerticalAlignment.Center, textStyle);
                }
            }

            if (isDividerEnabled)
            {
                canvas.StrokeColor = _pickerInfo.HeaderView.DividerColor;
                canvas.StrokeSize = StrokeThickness;
                float lineBottomPosition = height - (float)(StrokeThickness * 0.5);
                //// To avoid the separator line overlapping with the text, the height of the rectangle is reduced by the stroke thickness.
                canvas.DrawLine(xPosition, lineBottomPosition, width, lineBottomPosition);
            }

            //// Close-icon drawing moved to `PickerCloseButton`; header no longer draws fallback icon here.
            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the view.</param>
        /// <returns>The view size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (Children.Count == 0 && !CanShowCloseButton())
            {
                return base.ArrangeContent(bounds);
            }

            double totalWidth = bounds.Width;
            //// Stroke thickness denotes the below divider line space.
            double height = bounds.Height - StrokeThickness;
            bool isRTL = _pickerInfo.IsRTLLayout;

            if (_pickerInfo is SfDateTimePicker && _pickerInfo.HeaderTemplate == null)
            {
                double width = totalWidth / 2;
                double buttonXPosition = isRTL ? width : 0;
                foreach (var child in this.Children)
                {
                    if (child == _closeButton)
                    {
                        continue;
                    }

                    child.Arrange(new Rect(buttonXPosition, 0, width, height));
                    buttonXPosition = isRTL ? buttonXPosition - width : buttonXPosition + width;
                }

                return bounds.Size;
            }

            if (_pickerInfo.HeaderTemplate != null)
            {
                foreach (var child in Children)
                {
                    if (child == _closeButton)
                    {
                        continue;
                    }

                    child.Arrange(new Rect(0, 0, totalWidth, height));
                }
            }

            ArrangeCloseButton(totalWidth, height);
            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the view.</param>
        /// <param name="heightConstraint">The maximum height request of the view.</param>
        /// <returns>The maximum size of the view.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0 && !CanShowCloseButton())
            {
                return base.MeasureContent(widthConstraint, heightConstraint);
            }

            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            double buttonWidth = width / 2;
            //// Stroke thickness denotes the below divider line space.
            double buttonHeight = height - 1;
            if (_pickerInfo is SfDateTimePicker && _pickerInfo.HeaderTemplate == null)
            {
                foreach (var child in this.Children)
                {
                    if (child == _closeButton)
                    {
                        continue;
                    }

                    child.Measure(buttonWidth, buttonHeight);
                }
            }
            else if (_pickerInfo.HeaderTemplate != null)
            {
                foreach (var child in this.Children)
                {
                    if (child == _closeButton)
                    {
                        continue;
                    }

                    child.Measure(width, buttonHeight);
                }
            }

            if (_closeButton != null && CanShowCloseButton())
            {
                _closeButton.Measure(_closeButtonSize, buttonHeight);
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Method to create the semantics node for header layout.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Returns semantic virtual view.</returns>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            Size newSize = new Size(width, height);

            _semanticsNodes = new List<SemanticsNode>();
            _semanticsSize = newSize;
            bool isDividerEnabled = _pickerInfo.HeaderView.DividerColor != Colors.Transparent;

            //// To avoid the separator line overlapping with the text, the height of the rectangle is reduced by the stroke thickness.
            //// Use the full header bounds for the semantics node but trim the text
            //// so it matches the visually centered text (trimmed to avoid overlapping the close button).
            double maxTextWidth = width;
            if (this.CanShowCloseButton() && _closeButton != null)
            {
                double reserved = _closeButtonSize + _closeButtonPadding;
                maxTextWidth = Math.Max(0, width - reserved);
            }

            Rect rectangle = new Rect(0, 0, width, height - (isDividerEnabled ? StrokeThickness : 0));
            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.Text))
            {
                string originalText = _pickerInfo.HeaderView.Text;
                string semanticText = PickerHelper.TrimText(originalText, maxTextWidth, _pickerInfo.HeaderView.TextStyle);
                double measuredWidth = semanticText.Measure(_pickerInfo.HeaderView.TextStyle).Width;
                double reservedForSemantics = 0;
                if (this.CanShowCloseButton() && _closeButton != null)
                {
                    reservedForSemantics = _closeButtonSize + _closeButtonPadding;
                }

                double availableForFullCenter = width - (2 * reservedForSemantics);
                bool useFullBounds = semanticText == originalText && measuredWidth <= availableForFullCenter;

                Rect boundsRect;
                if (useFullBounds)
                {
                    boundsRect = rectangle;
                }
                else
                {
                    double safeLeft = 0;
                    double safeWidth = Math.Max(0, width - (this.CanShowCloseButton() && _closeButton != null ? _closeButtonSize + _closeButtonPadding : 0));
                    if (_pickerInfo.IsRTLLayout && this.CanShowCloseButton() && _closeButton != null)
                    {
                        safeLeft += _closeButtonSize + _closeButtonPadding;
                    }

                    boundsRect = new Rect(safeLeft, 0, safeWidth, height - (isDividerEnabled ? StrokeThickness : 0));
                }

                SemanticsNode node = new SemanticsNode()
                {
                    Text = semanticText,
                    Bounds = boundsRect,
                    IsTouchEnabled = true,
                };

                _semanticsNodes.Add(node);
            }

            return _semanticsNodes;
        }

        #endregion
    }
}