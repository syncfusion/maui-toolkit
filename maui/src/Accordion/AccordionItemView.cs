using Syncfusion.Maui.Toolkit.Expander;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Accordion
{
    /// <summary>
    /// Represents the item of the <see cref="SfAccordion"/>.
    /// </summary>
    internal partial class AccordionItemView : SfExpander, IThemeElement
    {
		#region Fields

		/// <summary>
		/// Indicates whether auto-scrolling is needed when expanding or collapsing.
		/// </summary>
		bool _needToAutoScroll = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccordionItemView" /> class.
        /// </summary>
        internal AccordionItemView()
        {
			ThemeElement.InitializeThemeResources(this, "SfAccordionTheme");
            SetDynamicResource(HoverHeaderBackgroundProperty, "SfAccordionHoverHeaderBackground");
            SetDynamicResource(HoverIconColorProperty, "SfAccordionHoverHeaderIconColor");
            SetDynamicResource(PressedIconColorProperty, "SfAccordionPressedHeaderIconColor");
            SetDynamicResource(FocusedHeaderBackgroundProperty, "SfAccordionFocusedHeaderBackground");
            SetDynamicResource(FocusedIconColorProperty, "SfAccordionFocusedHeaderIconColor");
            SetDynamicResource(HeaderStrokeProperty, "SfAccordionExpandedItemStroke");
            SetDynamicResource(HeaderRippleBackgroundProperty, "SfAccordionHeaderRippleBackground");
            SetDynamicResource(HeaderStrokeThicknessProperty, "SfAccordionExpandedItemStrokeThickness");
            SetDynamicResource(FocusBorderColorProperty, "SfAccordionFocusedItemStroke");
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets Instance of the <see cref="SfAccordion"/>.
        /// </summary>
        internal SfAccordion? Accordion { get; set; }

        /// <summary>
        /// Gets or sets Instance of the <see cref="AccordionItem"/>.
        /// </summary>
        internal AccordionItem? AccordionItem { get; set; }

		#endregion

		#region Internal Methods

		/// <summary>
		/// Helper method.
		/// </summary>
		/// <returns>Returns boolean value.</returns>
		/// <summary>
		/// Determines whether an AccordionItem can be collapsed in both Single and Multiple expand modes.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the item can be collapsed when only one item is expanded and the accordion is in Single or Multiple expand mode; otherwise, <c>false</c>.
		/// </returns>
		internal bool CanCollapseItemOnSingleAndMultipleExpandMode()
        {
			if (Accordion == null)
			{
				return false;
			}

			var expandedItems = Accordion.Items.ToList().FindAll(x => x._accordionItemView != null && x._accordionItemView.IsExpanded);
			return (Accordion.ExpandMode == AccordionExpandMode.Single ||
					Accordion.ExpandMode == AccordionExpandMode.Multiple) && expandedItems.Count == 1;	
        }

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the index of the currently expanded accordion item.
		/// </summary>
		int GetCurrentAccordionItemIndex()
		{
			if (Accordion == null)
			{
				return -1;
			}

			var expandedItem = Accordion.Items.FirstOrDefault(x => x._accordionItemView == this);
			return expandedItem != null ? expandedItem._itemIndex : -1;
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Raises the collapsing event for the current AccordionItem.
		/// </summary>
		protected internal override void RaiseCollapsingEvent()
		{
			int index = GetCurrentAccordionItemIndex();
			if (index == -1 || Accordion == null)
			{
				return;
			}

			if (Accordion.RaiseCollapsingEvent(index) && !CanCollapseItemOnSingleAndMultipleExpandMode())
			{
				Accordion.UpdateIsExpandValueBasedOnIndex(index, false);
			}
		}

		/// <summary>
		/// Raises the collapsed event for the current AccordionItem.
		/// </summary>
		protected internal override void RaiseCollapsedEvent()
		{
			int index = GetCurrentAccordionItemIndex();
			if (index != -1 && Accordion != null)
			{
				Accordion.RaiseCollapsedEvent(index);
			}
		}

		/// <summary>
		/// Raises the expanding event for the current AccordionItem.
		/// </summary>
		protected internal override void RaiseExpandingEvent()
		{
			int index = GetCurrentAccordionItemIndex();
			if (index != -1 && Accordion != null && Accordion.RaiseExpandingEvent(index))
			{
				_needToAutoScroll = true;
				Accordion.UpdateIsExpandValueBasedOnIndex(index, true);
			}
		}

		/// <summary>
		/// Raises the expanded event for the current AccordionItem.
		/// </summary>
		protected internal override void RaiseExpandedEvent()
		{
			int index = GetCurrentAccordionItemIndex();
			if (index != -1 && Accordion != null)
			{
				Accordion.RaiseExpandedEvent(index);
			}
		}

		/// <summary>
		/// Handles actions to be performed when an animation related to an AccordionItem is completed.
		/// </summary>
		protected override void AnimationCompleted()
		{
			base.AnimationCompleted();
			if (Accordion != null)
			{
				if (Accordion._scrollView is IView scrollView)
				{
					scrollView.InvalidateMeasure();
				}

				if (IsExpanded && _needToAutoScroll)
				{
					Accordion.UpdateAutoScrollToPosition(this);
					_needToAutoScroll = false;
				}
				else if (!IsExpanded && Accordion.Items.Count > 0)
				{
					var accordionItem = Accordion.Items.FirstOrDefault(x => x._accordionItemView == this);
					if (accordionItem != null && accordionItem.Content != null)
					{
						accordionItem.Content.IsVisible = false;
					}
				}
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method invoke when theme changes are applied for internal properties.
		/// </summary>
		/// <param name="oldTheme">Old theme name.</param>
		/// <param name="newTheme">New theme name.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Method invokes at whenever common theme changes.
		/// </summary>
		/// <param name="oldTheme">Represents the  old theme.</param>
		/// <param name="newTheme">Represents the  new theme.</param>
		public void OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion
	}
}
