using Syncfusion.Maui.Toolkit.Helper;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// The view that is loaded as the header of the PopupView in <see cref="SfPopup"/>.
	/// </summary>
	internal class PopupHeader : SfView
	{
		#region Fields

		/// <summary>
		/// Default left and top padding for <see cref="_headerView"/>.
		/// </summary>
		int _headerViewPadding = 16;

		/// <summary>
		/// Default padding for <see cref="_headerView"/>.
		/// </summary>
		int _headerPadding = 8;

		/// <summary>
		/// Gets PopupView's instance.
		/// </summary>
		internal PopupView? _popupView;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the header of the PopupView.
		/// </summary>
		/// <value>
		/// The CustomView to be loaded in the header of the PopupView.
		/// </value>
		internal View? _headerView;

		/// <summary>
		/// Gets or sets the text view in the header of the PopupView.
		/// </summary>
		/// <value>
		/// The text view in the header of the PopupView.
		/// </value>
		internal SfLabel? _titleLabel;

		/// <summary>
		/// Gets or sets the close button in the header of the PopupView.
		/// </summary>
		/// <value>
		/// The button in the header of the PopupView.
		/// </value>
		internal PopupCloseButton? _popupCloseButton;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupHeader"/> class.
		/// </summary>
		public PopupHeader()
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupHeader"/> class.
		/// </summary>
		/// <param name="popup">The instance of PopupView.</param>
		public PopupHeader(PopupView popup)
		{
#if IOS
			// When Page SafeArea is false, close icon overlaps header,because HeaderView arranging with safeArea.
			IgnoreSafeArea = !Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.GetUseSafeArea(PopupExtension.GetMainPage());
#endif
			_popupView = popup;
			Initialize();
			AddChildViews();
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Update the child views of the header.
		/// </summary>
		internal void UpdateChildViews()
		{
			SfGrid? headerView = _headerView as SfGrid;
			if (headerView is not null && Children.Count > 0)
			{
				headerView.ColumnDefinitions.Clear();
				headerView.Children.Clear();
				Children.Clear();
			}

			AddChildViews();
			UpdateHeaderAppearance();
		}

		/// <summary>
		/// Updates the appearance of the header.
		/// </summary>
		internal void UpdateHeaderAppearance()
		{
			UpdateHeaderChildView();
			UpdateHeaderStyle();
		}

		/// <summary>
		/// Updates the popup view close button.
		/// </summary>
		internal void UpdateHeaderCloseButton()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.PopupStyle is not null && _popupView._headerView is not null && _popupView._headerView._popupCloseButton is not null)
			{
				if (_popupView._popup.ShowCloseButton && _popupView._popup.PopupStyle.CloseButtonIcon is not null)
				{
					_popupView._headerView._popupCloseButton.UpdatePopupCloseButtonContent();
				}
				else if (_popupView._popup.ShowCloseButton && _popupView._popup.PopupStyle.CloseButtonIcon is null)
				{
					_popupView._headerView._popupCloseButton.InvalidateDrawable();
				}
			}
		}

		/// <summary>
		/// Updates the child views if a DataTemplateSelector is applied to the header template.
		/// </summary>
		internal void RefreshChildViews()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.HeaderTemplate is DataTemplateSelector)
			{
				UpdateChildViews();
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Add the child views of the header.
		/// </summary>
		void AddChildViews()
		{
			if (_popupView is not null && _headerView is not null)
			{
				SfGrid? headerView = _headerView as SfGrid;
				if (headerView is not null)
				{
					// The value for the IgnoreSafeArea property is being set by retrieving the safe area value from the main page.
					headerView.IgnoreSafeArea = IgnoreSafeArea;
					bool isFullScreen = _popupView._popup.CanShowPopupInFullScreen;
					if (isFullScreen)
					{
						if (_popupCloseButton is not null && _popupView._popup.ShowCloseButton)
						{
							_popupCloseButton.HorizontalOptions = LayoutOptions.Start;

							headerView.Padding = _popupView._popup.HeaderTemplate is null ? new Thickness(_headerPadding, _headerPadding, _headerViewPadding, _headerViewPadding) : new Thickness(0);
							headerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = _popupView._popup.HeaderTemplate is null ? 40 : isFullScreen ? 48 : 56 });
						}
						else
						{
							headerView.Padding = _popupView._popup.HeaderTemplate is null ? new Thickness(_headerViewPadding) : new Thickness(0);
						}

						headerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
					}
					else
					{
						// Adjust the header padding when the close button is not visible in non-full-screen mode.
						headerView.Padding = _popupView._popup.HeaderTemplate is null ? new Thickness(_headerViewPadding) : new Thickness(0);
						headerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
						if (_popupView._popup.ShowCloseButton && _popupCloseButton is not null)
						{
							_popupCloseButton.HorizontalOptions = LayoutOptions.End;

							// Set the close button column based on HeaderTemplate and IsFullScreen.
							headerView.ColumnDefinitions.Add(new ColumnDefinition() { Width = _popupView._popup.HeaderTemplate is null ? 40 : isFullScreen ? 48 : 56 });
						}
					}

					if (_popupView._popup.HeaderTemplate is null && _titleLabel is not null)
					{
#if WINDOWS
						_titleLabel.Padding = new Thickness(_headerPadding, _headerPadding, _headerPadding, 0);
#else
						_titleLabel.Padding = new Thickness(_headerPadding, 5, _headerPadding, 0);
#endif
						headerView.Children.Add(_titleLabel);
						Grid.SetColumn(_titleLabel, (_popupView._popup.ShowCloseButton && isFullScreen) ? 1 : 0);
					}
					else
					{
						if (_popupView._popup.HeaderTemplate is null)
						{
							return;
						}

						// If Template Selector is used and they have returned template using binding context means, value can be null.
						var template = _popupView._popup.GetTemplate(_popupView._popup.HeaderTemplate);
						if (template is not null)
						{
							View? view = (View)template.CreateContent();
							headerView.Children.Add(view);
							Grid.SetColumn(view, (_popupView._popup.ShowCloseButton && isFullScreen) ? 1 : 0);
						}
					}

					if (_popupView._popup.ShowCloseButton)
					{
						headerView.Children.Add(_popupCloseButton);
						Grid.SetColumn(_popupCloseButton, isFullScreen ? 0 : 1);
						if (_popupCloseButton is null)
						{
							return;
						}

						// margin for popup close icon in header view.
						if (isFullScreen)
						{
							_popupCloseButton.Margin = _popupView._popup.HeaderTemplate is not null ? new Thickness(_headerPadding, _headerPadding, 0, 0) : new Thickness(0);
						}
						else
						{
							_popupCloseButton.Margin = _popupView._popup.HeaderTemplate is not null ? new Thickness(0, _headerViewPadding, _headerViewPadding, 0) : new Thickness(0);
						}
					}
				}

				Children.Add(_headerView);
			}
		}

		/// <summary>
		/// Updates width and height of the child view.
		/// </summary>
		void UpdateHeaderChildView()
		{
			if (_popupView is not null && _popupView._popup.HeaderTemplate is null)
			{
				SfGrid? headerView = _headerView as SfGrid;
				if (headerView is not null && headerView.Children is not null && headerView.Children.Count > 0)
				{
					var view = headerView.Children[0] as View;
					if (view is not null)
					{
						bool isFullScreen = _popupView._popup.CanShowPopupInFullScreen;
						double closeButtonColumnWidth = _popupView._popup.ShowCloseButton ? headerView.ColumnDefinitions[isFullScreen ? 0 : 1].Width.Value : 0;
						int leftRightPadding = (isFullScreen && _popupView._popup.ShowCloseButton) ? (_headerPadding + _headerViewPadding) : (_headerViewPadding * 2);
						int topBottomPadding = (isFullScreen && _popupView._popup.ShowCloseButton) ? (_headerPadding + _headerViewPadding) : (_headerViewPadding * 2);
						view.WidthRequest = _popupView._popup._popupViewWidth - closeButtonColumnWidth - leftRightPadding - (_popupView._popup.PopupStyle.GetStrokeThickness() * 2);
						view.HeightRequest = Math.Max(0, _popupView._popup.AppliedHeaderHeight - topBottomPadding);
					}
				}
			}
		}

		/// <summary>
		/// updates the popup header style.
		/// </summary>
		void UpdateHeaderStyle()
		{
			if (_popupView is not null && _titleLabel is not null)
			{
				_titleLabel.TextColor = _popupView._popup.PopupStyle.GetHeaderTextColor();
				Background = _popupView._popup.PopupStyle.GetHeaderBackground();
				if (_popupView._popup.PopupStyle.HeaderFontFamily is not null)
				{
					_titleLabel.FontFamily = _popupView._popup.PopupStyle.HeaderFontFamily;
				}

				_titleLabel.FontSize = _popupView._popup.PopupStyle.GetHeaderFontSize();
				if (_popupView._popup.PopupStyle.HeaderFontAttribute == FontAttributes.Bold)
				{
					_titleLabel.FontAttributes = FontAttributes.Bold;
				}
				else if (_popupView._popup.PopupStyle.HeaderFontAttribute == FontAttributes.Italic)
				{
					_titleLabel.FontAttributes = FontAttributes.Italic;
				}
				else
				{
					_titleLabel.FontAttributes = FontAttributes.None;
				}

				if (_popupView._popup.PopupStyle.HeaderTextAlignment == TextAlignment.Start)
				{
					_titleLabel.VerticalTextAlignment = TextAlignment.Start;
					_titleLabel.HorizontalTextAlignment = TextAlignment.Start;
				}
				else if (_popupView._popup.PopupStyle.HeaderTextAlignment == TextAlignment.Center)
				{
					_titleLabel.VerticalTextAlignment = TextAlignment.Start;
					_titleLabel.HorizontalTextAlignment = TextAlignment.Center;
				}
				else
				{
					_titleLabel.VerticalTextAlignment = TextAlignment.Start;
					_titleLabel.HorizontalTextAlignment = TextAlignment.End;
				}
			}
		}

		/// <summary>
		/// Used to initialize the popup header.
		/// </summary>

		void Initialize()
		{
			_titleLabel = new SfLabel() { Text = SfPopupResources.GetLocalizedString("Title"), Style = new Style(typeof(SfLabel)) };
			if (_popupView is not null)
			{
				_popupCloseButton = new PopupCloseButton(_popupView) { VerticalOptions = LayoutOptions.Start };
			}

			_headerView = new SfGrid();
			_headerView.Style = new Style(typeof(SfGrid));
		}

		/// <summary>
		/// Triggered when click the close icon.
		/// </summary>
		void OnCloseButtonTapped()
		{
			if (_popupView is not null && _popupView._popup is not null)
			{
				_popupView._popup.IsOpen = false;
			}
		}

		#endregion
	}
}