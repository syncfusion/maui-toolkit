using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Defines the pop-up view of <see cref="SfPopup"/>.
	/// </summary>
	internal class PopupView : SfView
	{
		#region Fields

		/// <summary>
		/// Gets or sets the <see cref="SfPopup"/>.
		/// </summary>
		internal SfPopup _popup;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the header of the PopupView.
		/// </summary>
		internal PopupHeader? _headerView;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the footer of the PopupView.
		/// </summary>
		/// <value>
		/// The footer of the PopupView.
		/// </value>
		internal PopupFooter? _footerView;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the body of the PopupView.
		/// </summary>
		/// <value>
		/// The footer of the PopupView.
		/// </value>
		internal PopupMessageView? _popupMessageView;

		/// <summary>
		/// Indicates whether the popup was closed by using the accept button.
		/// </summary>
		internal bool _acceptButtonClicked = false;

		/// <summary>
		/// Backing field for the <see cref="AppliedHeaderHeight"/> property.
		/// </summary>
		int _appliedHeaderHeight;

		/// <summary>
		/// Backing field for the <see cref="AppliedFooterHeight"/> property.
		/// </summary>
		int _appliedFooterHeight;

		/// <summary>
		/// Backing field for the <see cref="AppliedBodyHeight"/> property.
		/// </summary>
		int _appliedBodyHeight;

		/// <summary>
		/// Backing field for the <see cref="IsViewLoaded"/> property.
		/// </summary>
		bool _isViewLoaded = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupView"/> class.
		/// </summary>
		/// <param name="sfPopup"><see cref="SfPopup"/> instance.</param>
		public PopupView(SfPopup sfPopup)
		{
			_popup = sfPopup;
			DrawingOrder = DrawingOrder.AboveContent;
#if !ANDROID && !WINDOWS
			// Shadow property is used for iOS alone because in other platforms we have some known issues in framework.
			// For other platforms we have used native implementation directly.
			Shadow = new Shadow() { Brush = Colors.Transparent };
#endif
			Initialize();
			AddChildViews();
		}

		#endregion

		#region Internal properties

		/// <summary>
		/// Gets or sets the height applied to the header of the PopupView.
		/// </summary>
		internal int AppliedHeaderHeight
		{
			get => _appliedHeaderHeight;
			set => _appliedHeaderHeight = value;
		}

		/// <summary>
		/// Gets or sets the height applied to the header of the PopupView.
		/// </summary>
		internal int AppliedFooterHeight
		{
			get => _appliedFooterHeight;
			set => _appliedFooterHeight = value;
		}

		/// <summary>
		/// Gets or sets the height applied to the body of the PopupView.
		/// </summary>
		internal int AppliedBodyHeight
		{
			get => _appliedBodyHeight;
			set => _appliedBodyHeight = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the popupview is loaded or not.
		/// </summary>
		internal bool IsViewLoaded
		{
			get => _isViewLoaded;
			set => _isViewLoaded = value;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Raises the Accept command.
		/// </summary>
		/// <param name="command">The command to be raised.</param>
		internal void RaiseAcceptCommand(ICommand command)
		{
			if (command is not null)
			{
				if (command.CanExecute(_popup.AcceptCommand))
				{
					command.Execute(_popup);
					_acceptButtonClicked = true;
					_popup.IsOpen = false;
				}
			}
			else
			{
				_acceptButtonClicked = true;
				_popup.IsOpen = false;
			}
		}

		/// <summary>
		/// Raises the Declline command.
		/// </summary>
		/// <param name="command">The command to be raised.</param>
		internal void RaiseDeclineCommand(ICommand command)
		{
			if (command is not null && _popup.DeclineCommand is not null)
			{
				if (command.CanExecute(_popup.DeclineCommand))
				{
					command.Execute(_popup);
					_popup.IsOpen = false;
				}
			}
			else
			{
				_popup.IsOpen = false;
			}
		}

		/// <summary>
		/// used to update footer view.
		/// </summary>
		internal void UpdateFooterView()
		{
			_footerView?.UpdateChildViews();
		}

		/// <summary>
		/// used to set the accept button text.
		/// </summary>
		/// <param name="text">text to be shown in the accept button.</param>
		internal void SetAcceptButtonText(string text)
		{
			if (_footerView is not null && _footerView._acceptButton is not null)
			{
				_footerView._acceptButton.Text = text;
				SemanticProperties.SetDescription(_footerView._acceptButton, $"{_footerView._acceptButton.Text}");

				// When the AcceptButtonText is updated, the AcceptButton width needs to be updated.
				if (IsViewLoaded)
				{
					_footerView.UpdateFooterChildProperties();
				}
			}
		}

		/// <summary>
		/// used to set the decline button text.
		/// </summary>
		/// <param name="text">text to be shown in the decline button.</param>
		internal void SetDeclineButtonText(string text)
		{
			if (_footerView is not null && _footerView._declineButton is not null)
			{
				_footerView._declineButton.Text = text;
				SemanticProperties.SetDescription(_footerView._declineButton, $"{_footerView._declineButton.Text}");

				// When the DeclineButtonText is updated, the DeclineButton width needs to be updated.
				if (IsViewLoaded)
				{
					_footerView.UpdateFooterChildProperties();
				}
			}
		}

		/// <summary>
		/// Used to set the header title text.
		/// </summary>
		/// <param name="title">text to be shown in the header title.</param>
		internal void SetHeaderTitleText(string title)
		{
			if (_headerView is not null && _headerView._titleLabel is not null)
			{
				_headerView._titleLabel.Text = title;
			}
		}

		/// <summary>
		/// Used to set the popup message text.
		/// </summary>
		/// <param name="message">text to be shown in the popup message.</param>
		internal void SetMessageText(string message)
		{
			if (_popupMessageView is not null && _popupMessageView._messageView is not null)
			{
				_popupMessageView._messageView.Text = message;
			}
		}

		/// <summary>
		/// used to update header view.
		/// </summary>
		internal void UpdateHeaderView()
		{
			_headerView?.UpdateChildViews();
		}

		/// <summary>
		/// used to update the popup message view.
		/// </summary>
		internal void UpdateMessageView()
		{
			_popupMessageView?.UpdateChildViews();
		}

		/// <summary>
		/// Method to invalidate the measure to triger the measure pass.
		/// </summary>
		internal void InvalidateForceLayout()
		{
			(this as IView).InvalidateMeasure();
		}

		/// <summary>
		/// Render the border around the <see cref="PopupView"/>.
		/// </summary>
		internal void ApplyShadowAndCornerRadius()
		{
#if ANDROID || WINDOWS
			_popup.ApplyNativePopupViewShadow();
#elif IOS
			// shadow is not applied for popupview in ios and mac when popupview have clip.
			// so need to apply clip for native view.
			_popup.ApplyNativePopupViewClip();
#endif
			// Clip
#if WINDOWS
			if (_popup.PopupStyle.CornerRadius.TopRight != 0 || _popup.PopupStyle.CornerRadius.TopLeft != 0 || _popup.PopupStyle.CornerRadius.BottomLeft != 0 || _popup.PopupStyle.CornerRadius.BottomRight != 0)
			{
				Clip = new RoundRectangleGeometry(_popup.PopupStyle.CornerRadius, new Rect(0, 0, _popup._popupViewWidth, _popup._popupViewHeight));
			}
#else
			Clip = null;
#endif

			// Shadow
#if !ANDROID && !WINDOWS
			if (Shadow is not null)
			{
				if (_popup.PopupStyle.HasShadow)
				{
					Shadow.Brush = Color.FromRgba(0, 0, 0, 0.3);
				}
				else
				{
					Shadow.Brush = SolidColorBrush.Transparent;
				}
			}
#endif
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Initializes the properties of the <see cref="PopupView"/> class.
		/// </summary>
		void Initialize()
		{
			_headerView = new PopupHeader(this);
			_footerView = new PopupFooter(this);
			_popupMessageView = new PopupMessageView(this);
		}

		/// <summary>
		/// Add the child views of the PopupView.
		/// </summary>
		void AddChildViews()
		{
			if (_popup.ShowHeader && _headerView is not null)
			{
				Children.Add(_headerView);
			}

			if (_popupMessageView is not null)
			{
				Children.Add(_popupMessageView);
			}

			if (_popup.ShowFooter && _footerView is not null)
			{
				Children.Add(_footerView);
			}
		}

		/// <summary>
		/// Measures the children of the PopupView.
		/// </summary>
		void MeasureChildren()
		{
			// Considering the borderThickness for child views.
			var borderThickness = _popup.PopupStyle.GetStrokeThickness();
			if (_popup.ShowHeader)
			{
				(_headerView as IView)?.Measure(Math.Max(0, _popup._popupViewWidth - borderThickness), Math.Max(0, _popup.AppliedHeaderHeight));
			}

			(_popupMessageView as IView)?.Measure(Math.Max(0, _popup._popupViewWidth - borderThickness), Math.Max(0, _popup.AppliedBodyHeight));
			if (_popup.ShowFooter)
			{
				(_footerView as IView)?.Measure(Math.Max(0, _popup._popupViewWidth - borderThickness), Math.Max(0, _popup.AppliedFooterHeight));
			}
		}

		#endregion

		#region Protected Override Methods

		/// <summary>
		/// Measures the PopupView.
		/// </summary>
		/// <param name="widthConstraint">Width of the PopupView.</param>
		/// <param name="heightConstraint">Height of the PopupView.</param>
		/// <returns>Returns the size of the view.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (_popupMessageView is not null && _popupMessageView._isContentModified)
			{
				_popup.ResetPopupBasedOnDynamicContentSize();
			}

			MeasureChildren();
			return new Size(Math.Max(0, _popup._popupViewWidth), Math.Max(0, _popup._popupViewHeight));
		}

		/// <summary>
		/// Arranges the view.
		/// </summary>
		/// <param name="bounds">The bounds of the view.</param>
		/// <returns>Returns the size of the view.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			var strokeThickness = _popup.PopupStyle.GetStrokeThickness();

			// Considering the borderThickness for child views.
			var padding = strokeThickness / 2;
			if (_popup.ShowHeader)
			{
				(_headerView as IView)?.Arrange(new Rect(Math.Max(0, padding), Math.Max(0, padding), Math.Max(0, _popup._popupViewWidth - strokeThickness), Math.Max(0, _popup.AppliedHeaderHeight)));
			}

			(_popupMessageView as IView)?.Arrange(new Rect(Math.Max(0, padding), Math.Max(0, padding + _popup.AppliedHeaderHeight), Math.Max(0, _popup._popupViewWidth - strokeThickness), Math.Max(0, _popup.AppliedBodyHeight)));
			if (_popup.ShowFooter)
			{
				(_footerView as IView)?.Arrange(new Rect(Math.Max(0, padding), Math.Max(0, padding + _popup.AppliedHeaderHeight + _popup.AppliedBodyHeight), Math.Max(0, _popup._popupViewWidth - strokeThickness), Math.Max(0, _popup.AppliedFooterHeight)));
			}

			return new Size(Math.Max(0, _popup._popupViewWidth), Math.Max(0, _popup._popupViewHeight));
		}

		/// <summary>
		/// To draw the border for the PopupView.
		/// </summary>
		/// <param name="canvas">The canvas to be drawn.</param>
		/// <param name="dirtyRect">The bounds of the view.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
#if ANDROID
            // Corner clipping
            PathF pathF = new PathF();
            if (OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                // In Android, the corner radius is applied LTR for RTL also. So we need to clip the corners based on FlowDirection.
                if (!_popup._isRTL)
                {
                    pathF.AppendRoundedRectangle(dirtyRect, (float)_popup.PopupStyle.CornerRadius.TopLeft, (float)_popup.PopupStyle.CornerRadius.TopRight, (float)_popup.PopupStyle.CornerRadius.BottomLeft, (float)_popup.PopupStyle.CornerRadius.BottomRight);
                }
                else
                {
                    pathF.AppendRoundedRectangle(dirtyRect, (float)_popup.PopupStyle.CornerRadius.TopRight, (float)_popup.PopupStyle.CornerRadius.TopLeft, (float)_popup.PopupStyle.CornerRadius.BottomRight, (float)_popup.PopupStyle.CornerRadius.BottomLeft);
                }
            }
            else
            {
                pathF.AppendRoundedRectangle(dirtyRect, (float)_popup._radiusValue);
            }

            canvas.ClipPath(pathF);
#endif

			var strokeThickness = _popup.PopupStyle.GetStrokeThickness();
			if (strokeThickness > 0)
			{
				canvas.StrokeColor = _popup.PopupStyle.GetStroke();
				canvas.StrokeSize = (float)strokeThickness;
#if ANDROID
				if (OperatingSystem.IsAndroidVersionAtLeast(33))
#endif
				{
					if (!_popup._isRTL || DeviceInfo.Platform == DevicePlatform.iOS)
					{
						canvas.DrawRoundedRectangle(dirtyRect, _popup.PopupStyle.CornerRadius.TopLeft, _popup.PopupStyle.CornerRadius.TopRight, _popup.PopupStyle.CornerRadius.BottomLeft, _popup.PopupStyle.CornerRadius.BottomRight);
					}
					else
					{
						canvas.DrawRoundedRectangle(dirtyRect, _popup.PopupStyle.CornerRadius.TopRight, _popup.PopupStyle.CornerRadius.TopLeft, _popup.PopupStyle.CornerRadius.BottomRight, _popup.PopupStyle.CornerRadius.BottomLeft);
					}
				}
#if ANDROID
				else
				{
					canvas.DrawRoundedRectangle(dirtyRect, _popup._radiusValue);
				}
#endif
			}
		}

		/// <summary>
		/// Occurs when the PopupView binding context is changed.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			// If Template selector along with binding context used, template will not be updated
			if (_popup is not null && _popup._popupView is PopupView _popupView)
			{
				if (_popupView._headerView is not null && _popup.ShowHeader)
				{
					_popupView._headerView.RefreshChildViews();
				}

				_popupView._popupMessageView?.RefreshChildViews();

				if (_popupView._footerView is not null && _popup.ShowFooter)
				{
					_popupView._footerView.RefreshChildViews();
				}
			}

			base.OnBindingContextChanged();
		}

		/// <summary>
		/// Rasies when the handler changes.
		/// </summary>
		protected override void OnHandlerChanged()
		{
			if (Handler is not null)
			{
				if (Handler.PlatformView is not null)
				{
#if ANDROID
					Toolkit.Platform.LayoutViewGroupExt platformView = (Toolkit.Platform.LayoutViewGroupExt)Handler.PlatformView;
					platformView.ClipToOutline = true;
#elif WINDOWS
					Handler.HasContainer = true;
					_popup.UpdateRTL();
#endif
				}

				// BindingContext Null in SfPopup When Using DataTemplateSelector.
				if (Handler is not null && !IsViewLoaded)
				{
					IsViewLoaded = true;
				}
			}

			base.OnHandlerChanged();
		}

		#endregion
	}
}