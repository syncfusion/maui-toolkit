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
                DrawingOrder = DrawingOrder.AboveContent;
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
            OnDateButtonClicked("Date");
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

                SfIconView buttonView = new SfIconView(SfIcon.TodayButton, _pickerInfo.HeaderView.SelectionTextStyle, _pickerInfo.HeaderView.DateText.ToString(), null, Colors.Transparent, isTabHighlight: true, semanticsNode: buttonViewNode);
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
                SfIconView buttonView = new SfIconView(SfIcon.TodayButton, _pickerInfo.HeaderView.TextStyle, _pickerInfo.HeaderView.TimeText.ToString(), null, Colors.Transparent, semanticsNode: buttonViewNode);
                buttonView.DrawingOrder = DrawingOrder.AboveContent;
                SfIconButton button = new SfIconButton(buttonView);
                button.AutomationId = $"DateTimePicker TimeHeaderView";
                Add(button);
                button.Clicked = OnTimeButtonClicked;
            }
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

            _pickerInfo.OnTimeButtonClicked();
        }

        /// <summary>
        /// Method to update the date button after clicked.
        /// </summary>
        void OnDateButtonClicked()
        {
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

            _pickerInfo.OnDateButtonClicked();
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

            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.Text) && _pickerInfo.HeaderTemplate == null)
            {
                string headerText = PickerHelper.TrimText(_pickerInfo.HeaderView.Text, width, _pickerInfo.HeaderView.TextStyle);
                SemanticProperties.SetDescription(this, headerText);
                canvas.DrawText(headerText, rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, _pickerInfo.HeaderView.TextStyle);
            }

            if (isDividerEnabled)
            {
                canvas.StrokeColor = _pickerInfo.HeaderView.DividerColor;
                canvas.StrokeSize = StrokeThickness;
                float lineBottomPosition = height - (float)(StrokeThickness * 0.5);
                //// To avoid the separator line overlapping with the text, the height of the rectangle is reduced by the stroke thickness.
                canvas.DrawLine(xPosition, lineBottomPosition, width, lineBottomPosition);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the view.</param>
        /// <returns>The view size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (Children.Count == 0)
            {
                return base.ArrangeContent(bounds);
            }

            double width = bounds.Width / 2;
            //// Stroke thickness denotes the below divider line space.
            double height = bounds.Height - StrokeThickness;
            bool isRTL = _pickerInfo.IsRTLLayout;
            double buttonXPosition = 0;
            if (isRTL)
            {
                buttonXPosition = width;
            }

            if (_pickerInfo.HeaderTemplate != null)
            {
                foreach (var child in Children)
                {
                    child.Arrange(new Rect(0, 0, bounds.Width, height));
                }

                return bounds.Size;
            }

            foreach (var child in Children)
            {
                child.Arrange(new Rect(buttonXPosition, 0, width, height));
                if (isRTL)
                {
                    buttonXPosition -= width;
                }
                else
                {
                    buttonXPosition += width;
                }
            }

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
            if (Children.Count == 0)
            {
                return base.MeasureContent(widthConstraint, heightConstraint);
            }

            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            double buttonWidth = width / 2;
            //// Stroke thickness denotes the below divider line space.
            double buttonHeight = height - 1;
            if (_pickerInfo.HeaderTemplate != null)
            {
                foreach (var child in Children)
                {
                    child.Measure(width, buttonHeight);
                }

                return new Size(width, height);
            }

            foreach (var child in Children)
            {
                child.Measure(buttonWidth, buttonHeight);
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
            Rect rectangle = new Rect(0, 0, width, height - (isDividerEnabled ? StrokeThickness : 0));
            if (!string.IsNullOrEmpty(_pickerInfo.HeaderView.Text))
            {
                SemanticsNode node = new SemanticsNode()
                {
                    Text = _pickerInfo.HeaderView.Text,
                    Bounds = rectangle,
                    IsTouchEnabled = true,
                };
                _semanticsNodes.Add(node);
            }

            return _semanticsNodes;
        }

        #endregion
    }
}