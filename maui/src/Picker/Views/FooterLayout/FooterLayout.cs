using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker footer layout.
    /// </summary>
    internal class FooterLayout : SfView, IThemeElement
    {
        #region Fields

        /// <summary>
        /// The separator line thickness for footer and picker layout.
        /// </summary>
        const float StrokeThickness = 1;

        /// <summary>
        /// Defines the padding value between the layout and the button left and right spacing.
        /// </summary>
        const double LayoutPadding = 10;

        /// <summary>
        /// The action buttons left and right padding.
        /// </summary>
        const double ButtonLeftAndRightPadding = 30;

        /// <summary>
        /// The buttons top and bottom padding.
        /// </summary>
        const double ButtonTopandBottomPadding = 20;

        /// <summary>
        /// The footer view info.
        /// </summary>
        readonly IFooterView _footerViewInfo;

        /// <summary>
        /// The ok button.
        /// </summary>
        SfIconButton? _confirmButton;

        /// <summary>
        /// The cancel button.
        /// </summary>
        SfIconButton? _cancelButton;

        /// <summary>
        /// The ok button view.
        /// </summary>
        SfIconView? _confirmButtonView;

        /// <summary>
        /// The cancel button view.
        /// </summary>
        SfIconView? _cancelButtonView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FooterLayout"/> class.
        /// </summary>
        /// <param name="footerViewInfo">The footer view info.</param>
        internal FooterLayout(IFooterView footerViewInfo)
        {
            DrawingOrder = DrawingOrder.AboveContent;
            _footerViewInfo = footerViewInfo;
            AddOrRemoveFooterButtons();
            ThemeElement.InitializeThemeResources(this, "SfPickerTheme");
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to add the confirm and cancel button to the footer layout.
        /// </summary>
        internal void AddOrRemoveFooterButtons()
        {
            if (_footerViewInfo.FooterView.ShowOkButton)
            {
                AddConfirmButton();
            }
            else
            {
                RemoveConfirmButton();
            }

            AddCancelButton();
        }

        /// <summary>
        /// Method to remove the confirm and cancel button from the footer layout.
        /// </summary>
        internal void RemoveFooterButtons()
        {
            RemoveConfirmButton();
            RemoveCancelButton();
        }

        /// <summary>
        /// Method to update the ok and cancel button text style.
        /// </summary>
        internal void UpdateButtonTextStyle()
        {
            if (_cancelButtonView != null)
            {
                _cancelButtonView.UpdateStyle(_footerViewInfo.FooterView.TextStyle);
                _cancelButtonView.HighlightTextColor = _footerViewInfo.FooterView.TextStyle.TextColor;
                _cancelButtonView.TextStyle.FontAutoScalingEnabled = _footerViewInfo.FooterView.TextStyle.FontAutoScalingEnabled;
            }

            if (_confirmButtonView != null)
            {
                _confirmButtonView.UpdateStyle(_footerViewInfo.FooterView.TextStyle);
                _confirmButtonView.HighlightTextColor = _footerViewInfo.FooterView.TextStyle.TextColor;
                _confirmButtonView.TextStyle.FontAutoScalingEnabled = _footerViewInfo.FooterView.TextStyle.FontAutoScalingEnabled;
            }
        }

        /// <summary>
        /// Method to update the separator color.
        /// </summary>
        internal void UpdateSeparatorColor()
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the confirm button text.
        /// </summary>
        internal void UpdateConfirmButtonText()
        {
            if (_confirmButtonView == null)
            {
                return;
            }

            _confirmButtonView.Text = SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.OkButtonText);
            SemanticProperties.SetDescription(_confirmButton, _footerViewInfo.FooterView.OkButtonText);
            //// While changing the confirm button text, the confirm button view is not updated. So, we have invalidated the drawable.
            _confirmButtonView.InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the cancel button text.
        /// </summary>
        internal void UpdateCancelButtonText()
        {
            if (_cancelButtonView == null)
            {
                return;
            }

            _cancelButtonView.Text = SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.CancelButtonText);
            SemanticProperties.SetDescription(_cancelButton, _footerViewInfo.FooterView.CancelButtonText);
            //// While changing the cancel button text, the cancel button view is not updated. So, we have invalidated the drawable.
            _cancelButtonView.InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the footer style.
        /// </summary>
        internal void UpdateFooterStyle()
        {
            AddOrRemoveFooterButtons();
            UpdateConfirmButtonText();
            UpdateCancelButtonText();
            UpdateButtonTextStyle();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to add the confirm button to the footer layout.
        /// </summary>
        void AddConfirmButton()
        {
            if (_confirmButtonView != null)
            {
                return;
            }

            SemanticsNode confirmButtonNode = new SemanticsNode()
            {
                Id = 0,
                Text = SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.OkButtonText),
                IsTouchEnabled = true,
                OnClick = OnConfirmButtonNodeClicked,
            };

            _confirmButtonView = new SfIconView(SfIcon.TodayButton, _footerViewInfo.FooterView.TextStyle, SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.OkButtonText), null, Colors.Transparent, isSelected: true, semanticsNode: confirmButtonNode, highlightTextColor: _footerViewInfo.FooterView.TextStyle.TextColor);
            _confirmButtonView.DrawingOrder = DrawingOrder.AboveContent;
            _confirmButton = new SfIconButton(_confirmButtonView, isHoveringOnReleased: false);
            _confirmButton.AutomationId = $"{PickerHelper.GetParentName(_footerViewInfo.FooterView.Parent)} Ok";
            _confirmButton.Clicked += ConfirmButtonClicked;
            Children.Add(_confirmButton);
        }

        /// <summary>
        /// Method to add the cancel button to the footer layout.
        /// </summary>
        void AddCancelButton()
        {
            if (_cancelButtonView != null)
            {
                return;
            }

            SemanticsNode cancelButtonNode = new SemanticsNode()
            {
                Id = 0,
                Text = SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.CancelButtonText),
                IsTouchEnabled = true,
                OnClick = OnCancelButtonNodeClicked,
            };

            _cancelButtonView = new SfIconView(SfIcon.TodayButton, _footerViewInfo.FooterView.TextStyle, SfPickerResources.GetLocalizedString(_footerViewInfo.FooterView.CancelButtonText), null, Colors.Transparent, isSelected: true, semanticsNode: cancelButtonNode, highlightTextColor: _footerViewInfo.FooterView.TextStyle.TextColor);
            _cancelButtonView.DrawingOrder = DrawingOrder.AboveContent;
            _cancelButton = new SfIconButton(_cancelButtonView, isHoveringOnReleased: false);
            _cancelButton.AutomationId = $"{PickerHelper.GetParentName(_footerViewInfo.FooterView.Parent)} Cancel";
            _cancelButton.Clicked += CancelButtonClicked;
            Children.Add(_cancelButton);
        }

        /// <summary>
        /// Method to remove the confirm button from the footer layout.
        /// </summary>
        void RemoveConfirmButton()
        {
            if (_confirmButton == null)
            {
                return;
            }

            _confirmButton.Clicked -= ConfirmButtonClicked;
            Remove(_confirmButton);
            for (int i = _confirmButton.Children.Count - 1; i >= 0; i--)
            {
                _confirmButton.RemoveAt(i);
            }

            if (_confirmButton.Handler != null && _confirmButton.Handler.PlatformView != null)
            {
                _confirmButton.Handler.DisconnectHandler();
            }

            _confirmButton = null;
            if (_confirmButtonView == null)
            {
                return;
            }

            if (_confirmButtonView.Handler != null && _confirmButtonView.Handler.PlatformView != null)
            {
                _confirmButtonView.Handler.DisconnectHandler();
            }

            _confirmButtonView = null;
        }

        /// <summary>
        /// Method to remove the cancel button from the footer layout.
        /// </summary>
        void RemoveCancelButton()
        {
            if (_cancelButton == null)
            {
                return;
            }

            _cancelButton.Clicked -= CancelButtonClicked;
            Remove(_cancelButton);
            for (int i = _cancelButton.Children.Count - 1; i >= 0; i--)
            {
                _cancelButton.RemoveAt(i);
            }

            if (_cancelButton.Handler != null && _cancelButton.Handler.PlatformView != null)
            {
                _cancelButton.Handler.DisconnectHandler();
            }

            _cancelButton = null;
            if (_cancelButtonView == null)
            {
                return;
            }

            if (_cancelButtonView.Handler != null && _cancelButtonView.Handler.PlatformView != null)
            {
                _cancelButtonView.Handler.DisconnectHandler();
            }

            _cancelButtonView = null;
        }

        /// <summary>
        /// Method to handle the Ok button clicked event.
        /// </summary>
        /// <param name="obj">The object.</param>
        void ConfirmButtonClicked(string obj)
        {
            _footerViewInfo.OnConfirmButtonClicked();
        }

        /// <summary>
        /// Method to handle the Cancel button clicked event.
        /// </summary>
        /// <param name="obj">The object.</param>
        void CancelButtonClicked(string obj)
        {
            _footerViewInfo.OnCancelButtonClicked();
        }

        /// <summary>
        /// Occurs when confirm button tapped while accessibility enabled.
        /// </summary>
        /// <param name="node">Confirm button semantic node.</param>
        void OnConfirmButtonNodeClicked(SemanticsNode node)
        {
            _footerViewInfo.OnConfirmButtonClicked();
        }

        /// <summary>
        /// Occurs when cancel button tapped while accessibility enabled.
        /// </summary>
        /// <param name="node">Cancel button semantic node.</param>
        void OnCancelButtonNodeClicked(SemanticsNode node)
        {
            _footerViewInfo.OnCancelButtonClicked();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the separator line for footer and picker layout.
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
            if (_footerViewInfo.FooterView.DividerColor != Colors.Transparent)
            {
                canvas.StrokeColor = _footerViewInfo.FooterView.DividerColor;
                canvas.StrokeSize = StrokeThickness;
                float lineTopPosition = dirtyRect.Top + (float)(StrokeThickness * 0.5);
                canvas.DrawLine(dirtyRect.Left, lineTopPosition, dirtyRect.Left + dirtyRect.Width, lineTopPosition);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            //// It holds the size of the confirm button text size.
            Size confirmButtonTextSize = Size.Zero;
            //// It holds the size of the cancel button text size.
            Size cancelButtonTextSize = Size.Zero;
            //// It holds the confirm button width with left and right padding.
            double confirmButtonWidth = 0;
            //// It holds the cancel button width with left and right padding.
            double cancelButtonWidth = 0;
            if (_footerViewInfo.FooterView.ShowOkButton && _confirmButtonView != null)
            {
                confirmButtonTextSize = _confirmButtonView.Text.Measure(_footerViewInfo.FooterView.TextStyle);
                confirmButtonWidth = confirmButtonTextSize.Width + ButtonLeftAndRightPadding;
            }

            if (_cancelButtonView != null)
            {
                cancelButtonTextSize = _cancelButtonView.Text.Measure(_footerViewInfo.FooterView.TextStyle);
                cancelButtonWidth = cancelButtonTextSize.Width + ButtonLeftAndRightPadding;
            }

            double buttonHeight = (cancelButtonTextSize.Height > confirmButtonTextSize.Height ? cancelButtonTextSize.Height : confirmButtonTextSize.Height) + ButtonTopandBottomPadding;
            //// Value 4 denotes the top and bottom 2 padding.
            double maximumButtonHeight = bounds.Height - 4;
            if (buttonHeight > maximumButtonHeight)
            {
                buttonHeight = maximumButtonHeight;
            }

            //// It holds the y position of the button.
            double buttonYPosition = (bounds.Height - buttonHeight) * 0.5;
            if (_footerViewInfo.IsRTLLayout)
            {
                foreach (var child in Children)
                {
                    if (child == _cancelButton)
                    {
                        //// Example: Width = 500, confirmButtonWidth = 30, cancelButtonWidth= 50. xPosition = 500 - 30 - 50 - 16 - 10 = 394. So that the cancel button draw start from the x position(394).
                        //// From above values. cancelButtonXPosition = 30 + 10 = 40. So that the cancel button draw start from the x position(40).
                        double cancelButtonXPosition = confirmButtonWidth + LayoutPadding;
                        //// The confirm button width is 0 while it is not visible, So need to start the cancel button draw from the layout padding(10).
                        //// xPosition = LayoutPadding(10).
                        //// The confirm button with is not 0 when the confirm button is visible, So need to draw the cancel button, The cancel button draw start from based on the addition of cancelButtonXPosition and layout padding.
                        //// xPosition = 40 + 10 => 50.
                        double xPosition = confirmButtonWidth == 0 ? LayoutPadding : cancelButtonXPosition + LayoutPadding;
                        child.Arrange(new Rect(xPosition, buttonYPosition, cancelButtonWidth, buttonHeight));
                    }
                    else if (child == _confirmButton)
                    {
                        double xPosition = LayoutPadding;
                        child.Arrange(new Rect(xPosition, buttonYPosition, confirmButtonWidth, buttonHeight));
                    }
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    if (child == _cancelButton)
                    {
                        //// Here need to subtract confirm and cancel button width from the total width to render the cancel button.
                        //// Example: Width = 500, confirmButtonWidth = 30, cancelButtonWidth= 50. xPosition = 500 - 30 - 50 - 16 - 10 = 394. So that the cancel button draw start from the x position(394).
                        double cancelButtonXPosition = bounds.Width - confirmButtonWidth - cancelButtonWidth - LayoutPadding;
                        //// While the confirm button is not visible then the confirm button width is 0, the cancel button is drawn from the right side of the layout with layout padding value and cancel button width.
                        //// xPosition = cancelButtonXPosition(394) - LayoutPadding(10) => 384.
                        double xPosition = confirmButtonWidth == 0 ? cancelButtonXPosition : cancelButtonXPosition - LayoutPadding;
                        child.Arrange(new Rect(xPosition, buttonYPosition, cancelButtonWidth, buttonHeight));
                    }
                    else if (child == _confirmButton)
                    {
                        //// From above values. xPosition = 500 - 30 - 10 = 460. So that the ok button draw start from the x position(460).
                        double xPosition = bounds.Width - confirmButtonWidth - LayoutPadding;
                        child.Arrange(new Rect(xPosition, buttonYPosition, confirmButtonWidth, buttonHeight));
                    }
                }
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            Size confirmButtonTextSize = Size.Zero;
            Size cancelButtonTextSize = Size.Zero;
            if (_footerViewInfo.FooterView.ShowOkButton && _confirmButtonView != null)
            {
                confirmButtonTextSize = _confirmButtonView.Text.Measure(_footerViewInfo.FooterView.TextStyle);
            }

            if (_cancelButtonView != null)
            {
                cancelButtonTextSize = _cancelButtonView.Text.Measure(_footerViewInfo.FooterView.TextStyle);
            }

            double buttonHeight = (cancelButtonTextSize.Height > confirmButtonTextSize.Height ? cancelButtonTextSize.Height : confirmButtonTextSize.Height) + ButtonTopandBottomPadding;
            //// Value 4 denotes the top and bottom 2 padding.
            double maximumButtonHeight = height - 4;
            if (buttonHeight > maximumButtonHeight)
            {
                buttonHeight = maximumButtonHeight;
            }

            foreach (SfIconButton child in Children)
            {
                if (child == _cancelButton)
                {
                    child.Measure(cancelButtonTextSize.Width + ButtonLeftAndRightPadding, buttonHeight);
                }
                else if (child == _confirmButton)
                {
                    child.Measure(confirmButtonTextSize.Width + ButtonLeftAndRightPadding, buttonHeight);
                }
            }

            return new Size(width, height);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method will be called when a theme dictionary
        /// that contains the value for your control key is merged in application.
        /// </summary>
        /// <param name="oldTheme">The old value.</param>
        /// <param name="newTheme">The new value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion
    }
}