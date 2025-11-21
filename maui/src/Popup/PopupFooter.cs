using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Popup.Helper;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// The view that is loaded as the footer of the PopupView in <see cref="SfPopup"/>.
	/// </summary>
	internal class PopupFooter : SfView
	{
		#region Fields

		/// <summary>
		/// Gets PopupView's instance.
		/// </summary>
		internal PopupView? _popupView;

		/// <summary>
		/// Gets or sets the accept button in the footer.
		/// </summary>
		/// <value>
		/// The accept button in the footer.
		/// </value>
		internal SfButton? _acceptButton;

		/// <summary>
		/// Gets or sets the decline button in the footer.
		/// </summary>
		/// <value>
		/// The decline button in the footer.
		/// </value>
		internal SfButton? _declineButton;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the footer of the PopupView.
		/// </summary>
		/// <value>
		/// The CustomView to be loaded in the header of the PopupView.
		/// </value>
		internal View? _footerView;

		/// <summary>
		/// Default padding for <see cref="_footerView"/>.
		/// </summary> 
		int _footerPadding = 24;

		/// <summary>
		/// Height of the footer buttons.
		/// </summary>
		int _footerButtonHeight = 40;

		/// <summary>
		///  The distance between footer buttons.
		/// </summary>
		int _distanceBetweenFooterButtons = 8;

		/// <summary>
		/// Width of the accept button.
		/// </summary>
		double _acceptButtonWidth;

		/// <summary>
		/// Width of the decline button.
		/// </summary>
		double _declineButtonWidth;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupFooter" /> class.
		/// </summary>
		public PopupFooter()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupFooter" /> class.
		/// </summary>
		/// <param name="popupView">The instance of PopupView.</param>
		public PopupFooter(PopupView popupView)
		{
#if IOS
			// The value for the IgnoreSafeArea property is being set by retrieving the safe area value from the main page.
#pragma warning disable CS0618 // Suppressing CS0618 warning because Page.GetUseSafeArea is marked obsolete in .NET 10.
			IgnoreSafeArea = !Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.GetUseSafeArea(PopupExtension.GetMainPage());
#pragma warning restore CS0618
#endif
			_popupView = popupView;
			Initialize();
			AddChildViews();
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// update the child views and appearance of the footer.
		/// </summary>
		internal void UpdateChildViews()
		{
			UpdateFooterChild();
			UpdateFooterAppearance();
		}

		/// <summary>
		/// Update the child views of the footer.
		/// </summary>
		internal void UpdateFooterChild()
		{
			SfGrid? footerView = _footerView as SfGrid;
			if (footerView is not null && Children.Count > 0)
			{
				footerView.ColumnDefinitions.Clear();
				footerView.Children.Clear();
				Children.Clear();
			}

			AddChildViews();
		}

		/// <summary>
		/// Updates the appearance of the footer.
		/// </summary>
		internal void UpdateFooterAppearance()
		{
			// Accept and Decline button text in SfPopup is cropped initially when FooterFontSize is set.
			UpdateFooterStyle();
			UpdateFooterChildView();
		}

		/// <summary>
		/// Updates the child views if a DataTemplateSelector is applied to the footer template.
		/// </summary>
		internal void RefreshChildViews()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.FooterTemplate is DataTemplateSelector)
			{
				UpdateChildViews();
			}
		}

		/// <summary>
		/// Updates the width and height of the child views.
		/// </summary>
		internal void UpdateFooterChildProperties()
		{
			if (_acceptButton is null || _declineButton is null)
			{
				return;
			}

			if (_popupView is not null && _popupView._popup is not null)
			{
				if (_popupView._popup.AppearanceMode is PopupButtonAppearanceMode.OneButton)
				{
					_acceptButton.HorizontalOptions = LayoutOptions.End;
					_acceptButtonWidth = GetFooterButtonWidth(_acceptButton);
					_acceptButton.WidthRequest = Math.Max(0, _acceptButtonWidth);
					_acceptButton.HeightRequest = Math.Max(0, _footerButtonHeight);
				}
				else if (_popupView._popup.AppearanceMode is PopupButtonAppearanceMode.TwoButton)
				{
					_acceptButtonWidth = GetFooterButtonWidth(_acceptButton);
					_acceptButton.WidthRequest = Math.Max(0, _acceptButtonWidth);
					_acceptButton.HeightRequest = Math.Max(0, _footerButtonHeight);

					_declineButtonWidth = GetFooterButtonWidth(_declineButton);
					_declineButton.WidthRequest = Math.Max(0, _declineButtonWidth);
					_declineButton.HeightRequest = Math.Max(0, _footerButtonHeight);

					_declineButton.IsVisible = _popupView._popup.ShowFooter;
				}

				_acceptButton.IsVisible = _popupView._popup.ShowFooter;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Add the child views of the footer.
		/// </summary>
		void AddChildViews()
		{
			if (_footerView is null || _popupView is null || _popupView._popup is null)
			{
				return;
			}

			SfGrid? footerView = _footerView as SfGrid;
			if (footerView is not null)
			{
				// The value for the IgnoreSafeArea property is being set by retrieving the safe area value from the main page.
#pragma warning disable CS0618 // Suppressing CS0618 warning because Layout.IgnoreSafeArea is marked obsolete in .NET 10.
				footerView.IgnoreSafeArea = IgnoreSafeArea;
#pragma warning restore CS0618
				if (_popupView._popup.FooterTemplate is null)
				{
					if (_popupView._popup.AppearanceMode == PopupButtonAppearanceMode.OneButton)
					{
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
						footerView.Children.Add(_acceptButton as IView);
						Grid.SetColumn(_acceptButton, 0);
					}
					else
					{
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = _distanceBetweenFooterButtons });
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
						footerView.Children.Add(_declineButton as IView);
						Grid.SetColumn(_declineButton, 1);
						footerView.Children.Add(_acceptButton as IView);
						Grid.SetColumn(_acceptButton, 3);
					}
				}
				else
				{
					// If Template Selector is used and they have returned template using binding context means, value can be null.
					var template = _popupView._popup.GetTemplate(_popupView._popup.FooterTemplate);
					if (template is not null)
					{
						var view = (View)template.CreateContent();
						footerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
						footerView.HorizontalOptions = LayoutOptions.Fill;
						footerView.Children.Add(view);
						Grid.SetColumn(view, 0);
					}
				}

				footerView.Padding = _popupView is not null && _popupView._popup.FooterTemplate is null ? new Thickness(_footerPadding) : new Thickness(0);
			}

			Children.Add(_footerView);
		}

		/// <summary>
		/// Updates the width and height of the child views.
		/// </summary>
		void UpdateFooterChildView()
		{
			if (_popupView is not null && _popupView._popup is not null)
			{
				if (_popupView._popup.AppearanceMode == PopupButtonAppearanceMode.OneButton)
				{
					// Need to measure the button, only when the button is not visible(On Initial loading), remeasuring multiple times will cause the improper size for the button(Run time scenario).
					if (_acceptButton is not null && !_acceptButton.IsVisible)
					{
						_acceptButton.HorizontalOptions = LayoutOptions.End;
						_acceptButtonWidth = GetFooterButtonWidth(_acceptButton);
						_acceptButton.WidthRequest = Math.Max(0, _acceptButtonWidth);
						_acceptButton.HeightRequest = Math.Max(0, _footerButtonHeight);
					}
				}
				else if (_popupView._popup.AppearanceMode == PopupButtonAppearanceMode.TwoButton)
				{
					if (_acceptButton is not null && !_acceptButton.IsVisible)
					{
						_acceptButtonWidth = GetFooterButtonWidth(_acceptButton);
						_acceptButton.WidthRequest = Math.Max(0, _acceptButtonWidth);
						_acceptButton.HeightRequest = Math.Max(0, _footerButtonHeight);
					}

					if (_declineButton is not null && !_declineButton.IsVisible)
					{
						_declineButtonWidth = GetFooterButtonWidth(_declineButton);
						_declineButton.WidthRequest = Math.Max(0, _declineButtonWidth);
						_declineButton.HeightRequest = Math.Max(0, _footerButtonHeight);
					}
					if (_declineButton is not null)
					{
						_declineButton.IsVisible = _popupView._popup.ShowFooter;
					}
				}
				if (_acceptButton is not null)
				{
					_acceptButton.IsVisible = _popupView._popup.ShowFooter;
				}
			}
		}


		/// <summary>
		/// Gets the footer button width based on text size.
		/// </summary>
		/// <param name="button">Instance of the button.</param>
		/// <returns>returns the button width.</returns>
		double GetFooterButtonWidth(SfButton button)
		{
			button.IsVisible = true;
			button.WidthRequest = -1;
			Size viewSize = new Size(double.PositiveInfinity, _popupView is not null ? _popupView._popup.FooterHeight : 0);
			Size measuredSize = button.Measure(viewSize.Width, viewSize.Height);
#if !WINDOWS
			// Add left and right padding to the button text.
			return measuredSize.Width + 10;
#else
			return measuredSize.Width;
#endif
		}

		/// <summary>
		/// updates the popup footer style.
		/// </summary>
		void UpdateFooterStyle()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.PopupStyle is not null)
			{
				Background = _popupView._popup.PopupStyle.GetFooterBackground();
				if (_acceptButton is not null)
				{
					_acceptButton.Stroke = Colors.Transparent;
					_acceptButton.Background = _popupView._popup.PopupStyle.GetAcceptButtonBackground();
					_acceptButton.TextColor = _popupView._popup.PopupStyle.GetAcceptButtonTextColor();
					_acceptButton.FontAttributes = _popupView._popup.PopupStyle.FooterFontAttribute;
					_acceptButton.FontSize = _popupView._popup.PopupStyle.GetFooterFontSize();
					_acceptButton.FontFamily = _popupView._popup.PopupStyle.FooterFontFamily;
					_acceptButton.CornerRadius = _popupView._popup.PopupStyle.FooterButtonCornerRadius;
				}
				if (_popupView._popup.AppearanceMode == PopupButtonAppearanceMode.TwoButton && _declineButton is not null)
				{
					_declineButton.CornerRadius = _popupView._popup.PopupStyle.FooterButtonCornerRadius;
					_declineButton.Stroke = Colors.Transparent;
					_declineButton.FontAttributes = _popupView._popup.PopupStyle.FooterFontAttribute;
					_declineButton.FontSize = _popupView._popup.PopupStyle.GetFooterFontSize();
					_declineButton.FontFamily = _popupView._popup.PopupStyle.FooterFontFamily;
					_declineButton.Background = _popupView._popup.PopupStyle.GetDeclineButtonBackground();
					_declineButton.TextColor = _popupView._popup.PopupStyle.GetDeclineButtonTextColor();
				}
			}
		}

		/// <summary>
		/// Used to initialize the popup footer.
		/// </summary>
		void Initialize()
		{
			_footerView = new SfGrid();
			_footerView.Style = new Style(typeof(SfGrid));
			_acceptButton = new SfButton() { IsVisible = false };
			_acceptButton.AutomationId = "PopupAcceptButton";
			_acceptButton.Style = new Style(typeof(SfButton));
			_acceptButton.Text = SfPopupResources.GetLocalizedString("AcceptButtonText");
			SemanticProperties.SetDescription(_acceptButton, $"{_acceptButton.Text}");
			_acceptButton.Clicked += OnAcceptButtonClicked;
			_declineButton = new SfButton() { IsVisible = false };
			_declineButton.AutomationId = "PopupDeclineButton";
			_declineButton.Style = new Style(typeof(SfButton));
			_declineButton.Text = SfPopupResources.GetLocalizedString("DeclineButtonText");
			SemanticProperties.SetDescription(_declineButton, $"{_declineButton.Text}");
			_declineButton.Clicked += OnDeclineButtonClicked;
		}

		/// <summary>
		/// Handles, when click the decline button.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/>.</param>
		void OnDeclineButtonClicked(object? sender, EventArgs e)
		{
			if (_popupView is not null && _popupView._popup is not null)
			{
				_popupView.RaiseDeclineCommand(_popupView._popup.DeclineCommand);
			}
		}

		/// <summary>
		/// Handles, when click the accept button
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/>.</param>
		void OnAcceptButtonClicked(object? sender, EventArgs e)
		{
			if (_popupView is not null && _popupView._popup is not null)
			{
				_popupView.RaiseAcceptCommand(_popupView._popup.AcceptCommand);
			}
		}

		#endregion
	}
}