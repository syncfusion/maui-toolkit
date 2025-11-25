using Syncfusion.Maui.Toolkit.Helper;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// The message view of the PopupView, which is the content of the PopupView, when the <see cref="_contentView"/> is null.
	/// </summary>
	internal class PopupMessageView : SfContentView
	{
		#region Fields

		// Todo - used same properties in all the popup views
		/// <summary>
		/// Default padding for <see cref="_contentView"/>.
		/// </summary>
		int _contentPadding = 24;

		/// <summary>
		/// Gets PopupView's instance.
		/// </summary>
		internal PopupView? _popupView;

		/// <summary>
		/// Gets or sets the CustomView to be loaded in the message view of the PopupView.
		/// </summary>
		/// <value>
		/// The CustomView to be loaded in the content of the PopupView.
		/// </value>
		internal View? _contentView;

		/// <summary>
		/// Gets or sets the text view in the message of the PopupView.
		/// </summary>
		/// <value>
		/// The text view in the content of the PopupView.
		/// </value>
		internal SfLabel? _messageView;

		/// <summary>
		/// Boolean value indicating whether the content is updated in runtime or not.
		/// </summary>
		internal bool _isContentModified;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupMessageView"/> class.
		/// </summary>
		public PopupMessageView()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupMessageView"/> class.
		/// </summary>
		/// <param name="popupView">The instance of PopupView.</param>
		public PopupMessageView(PopupView popupView)
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

		#region Internal methods

		/// <summary>
		/// Update the child views of the message view.
		/// </summary>
		internal void UpdateChildViews()
		{
			if (Content is not null)
			{
				MeasureInvalidated -= OnContentMeasureInvalidated;
				DescendantAdded -= OnContentDescendantAdded;
				DescendantRemoved -= OnContentDescendantRemoved;
				_isContentModified = false;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
				Content = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			}

			AddChildViews();
			UpdateMessageViewStyle();
		}

		/// <summary>
		/// Updates the popup message view style.
		/// </summary>
		internal void UpdateMessageViewStyle()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.PopupStyle is not null)
			{
				Background = _popupView._popup.PopupStyle.GetMessageBackground();
				if (_popupView._popup.ContentTemplate is null && _messageView is not null)
				{
					_messageView.TextColor = _popupView._popup.PopupStyle.GetMessageTextColor();
					_messageView.FontFamily = _popupView._popup.PopupStyle.MessageFontFamily;
					_messageView.FontSize = _popupView._popup.PopupStyle.GetMessageFontSize();

					if (_popupView._popup.PopupStyle.MessageFontAttribute == FontAttributes.Bold)
					{
						_messageView.FontAttributes = FontAttributes.Bold;
					}
					else if (_popupView._popup.PopupStyle.MessageFontAttribute == FontAttributes.Italic)
					{
						_messageView.FontAttributes = FontAttributes.Italic;
					}
					else
					{
						_messageView.FontAttributes = FontAttributes.None;
					}

					if (_popupView._popup.PopupStyle.MessageTextAlignment == TextAlignment.Start)
					{
						_messageView.VerticalTextAlignment = TextAlignment.Start;
						_messageView.HorizontalTextAlignment = TextAlignment.Start;
					}
					else if (_popupView._popup.PopupStyle.MessageTextAlignment == TextAlignment.Center)
					{
						_messageView.VerticalTextAlignment = TextAlignment.Start;
						_messageView.HorizontalTextAlignment = TextAlignment.Center;
					}
					else
					{
						_messageView.VerticalTextAlignment = TextAlignment.Start;
						_messageView.HorizontalTextAlignment = TextAlignment.End;
					}
				}
			}
		}

		/// <summary>
		/// Update message view padding.
		/// </summary>
		internal void UpdatePadding()
		{
			// Updated content padding for default popup and skipped when ContentTemplate given.
			// Also, considered padding based on ShowHeader and ShowFooter.
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.ContentTemplate is null && Content is not null)
			{
				Content.Margin = new Thickness(_contentPadding, _popupView._popup.ShowHeader ? 0 : _contentPadding, _contentPadding, _popupView._popup.ShowFooter ? 0 : _contentPadding);
			}
		}

		/// <summary>
		/// Updates the child views if a DataTemplateSelector is applied to the content template.
		/// </summary>
		internal void RefreshChildViews()
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.ContentTemplate is DataTemplateSelector)
			{
				UpdateChildViews();
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Add the child views of the message view.
		/// </summary>
		void AddChildViews()
		{
			if (Content is null && _popupView is not null)
			{
				if (_popupView._popup.ContentTemplate is null && _messageView is not null)
				{
					Content = _messageView;

					// Updating the default padding for content view
					UpdatePadding();
				}
				else
				{
					if (_popupView._popup.ContentTemplate is not null)
					{
						var template = _popupView._popup.GetTemplate(_popupView._popup.ContentTemplate);
						if (template is not null)
						{
							View? view = (View)template.CreateContent();
							Content = view;
						}
					}

					// Added event handlers to update the popup height dynamically when the content changes.
					MeasureInvalidated += OnContentMeasureInvalidated;
					DescendantAdded += OnContentDescendantAdded;
					DescendantRemoved += OnContentDescendantRemoved;
				}
			}
		}

		/// <summary>
		/// Used to initialize the popup message view.
		/// </summary>
		void Initialize()
		{
			_messageView = new SfLabel() { Text = SfPopupResources.GetLocalizedString("Message"), Style = new Style(typeof(SfLabel)) };
		}

		/// <summary>
		/// Raises when child added.
		/// </summary>
		/// <param name="sender">Instance of child view.</param>
		/// <param name="e">Corresponding Event args.</param>
		void OnContentDescendantAdded(object? sender, ElementEventArgs e)
		{
			UpdateDynamicContent();
		}

		/// <summary>
		/// Raises when child removed.
		/// </summary>
		/// <param name="sender">Instance of child view.</param>
		/// <param name="e">Corresponding Event args.</param>
		void OnContentDescendantRemoved(object? sender, ElementEventArgs e)
		{
			UpdateDynamicContent();
		}

		/// <summary>
		/// Method raises when the content is changed at runtime.
		/// </summary>
		/// <param name="sender">Instance of child view.</param>
		/// <param name="e">Represents the event data.</param>
		void OnContentMeasureInvalidated(object? sender, System.EventArgs e)
		{
			UpdateDynamicContent();
		}

		/// <summary>
		/// Used when the Content is updated dynamically.
		/// </summary>
		void UpdateDynamicContent()
		{
			// Skipping the IsContentModified property getting updated at intial loading when the Animation is in running.
			if (_popupView is null || _popupView._popup.AutoSizeMode == PopupAutoSizeMode.None || _popupView._popup._isContainerAnimationInProgress || _popupView._popup._isPopupAnimationInProgress)
			{
				return;
			}

			_isContentModified = true;
			_popupView.InvalidateForceLayout();
		}

		#endregion
	}
}