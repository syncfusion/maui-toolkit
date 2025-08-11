using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Expander;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Accordion
{
	/// <summary>
	/// Represents a collapsible accordion control that displays a list of expandable items.
	/// </summary>
	/// <remarks>
	/// The <see cref="SfAccordion"/> control allows you to organize content into collapsible sections called items.
	/// Each item can be expanded or collapsed to show or hide its associated content.
	/// </remarks>
	public partial class SfAccordion : AccordionLayout, IParentThemeElement, IKeyboardListener
	{
		#region Fields

		/// <summary>
		/// Represents whether the change is an internal operation or not.
		/// </summary>
		internal bool _isInInternalChange = false;

		/// <summary>
		/// Represents the scroll view used to add accordion items for scrolling functionality.
		/// </summary>
		internal AccordionScrollView? _scrollView;

		/// <summary>
		/// Backing field for the semantic description of the accordion.
		/// </summary>
		string? _semanticDescription;

		/// <summary>
		/// Represents the currently focused accordion item.
		/// </summary>
		AccordionItem? _currentItem;

		/// <summary>
		/// Represents the container used to arrange the accordion items into a single column.
		/// </summary>
		ItemContainer? _itemContainer;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Items"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ItemsProperty"/> determines the collection of items contained within the control.
		/// </remarks>
		public static readonly BindableProperty ItemsProperty =
			BindableProperty.Create(
				nameof(Items),
				typeof(ObservableCollection<AccordionItem>),
				typeof(SfAccordion),
				null,
				BindingMode.OneWay,
				null,
				OnItemsPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ItemSpacing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ItemSpacingProperty"/> determines the spacing between items within the control.
		/// </remarks>
		public static readonly BindableProperty ItemSpacingProperty =
			BindableProperty.Create(
				nameof(ItemSpacing),
				typeof(double),
				typeof(SfAccordion),
				0d,
				BindingMode.OneWay,
				null,
				OnItemSpacingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ExpandMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExpandModeProperty"/> determines the mode of expansion or collapse behavior within the control.
		/// </remarks>
		public static readonly BindableProperty ExpandModeProperty =
			BindableProperty.Create(
				nameof(ExpandMode),
				typeof(AccordionExpandMode),
				typeof(SfAccordion),
				AccordionExpandMode.Single,
				BindingMode.OneWay,
				null,
				OnExpandModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AnimationDuration"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AnimationDurationProperty"/> determines the duration of animations within the control.
		/// </remarks>
		public static readonly BindableProperty AnimationDurationProperty =
			BindableProperty.Create(
				nameof(AnimationDuration),
				typeof(double),
				typeof(SfAccordion),
				250d,
				BindingMode.OneWay,
				null,
				OnAnimationDurationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AnimationEasingProperty"/> determines the easing function used for animations in the control.
		/// </remarks>
		public static readonly BindableProperty AnimationEasingProperty =
			BindableProperty.Create(
				nameof(AnimationEasing),
				typeof(ExpanderAnimationEasing),
				typeof(SfAccordion),
				ExpanderAnimationEasing.Linear,
				BindingMode.OneWay,
				null,
				OnAnimationEasingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AutoScrollPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AutoScrollPositionProperty"/> determines the automatic scroll position within the control.
		/// </remarks>
		public static readonly BindableProperty AutoScrollPositionProperty =
			BindableProperty.Create(
				nameof(AutoScrollPosition),
				typeof(AccordionAutoScrollPosition),
				typeof(SfAccordion),
				AccordionAutoScrollPosition.MakeVisible,
				BindingMode.OneWay,
				null);

		/// <summary>
		/// Identifies the <see cref="HeaderIconPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderIconPositionProperty"/> determines the location of the header icon within the control.
		/// This bindable property is read-only.
		/// </remarks>
		public static readonly BindableProperty HeaderIconPositionProperty =
			BindableProperty.Create(
				nameof(HeaderIconPosition),
				typeof(ExpanderIconPosition),
				typeof(SfAccordion),
				ExpanderIconPosition.End,
				BindingMode.OneWay,
				null,
				OnHeaderIconPositionPropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfAccordion"/> class and initializes the default values of its members.
		/// </summary>
		public SfAccordion()
		{
			InitializeAccordion();
			Loaded += OnAccordionLoaded;
			ChildRemoved += OnAccordionChildRemoved;
			ThemeElement.InitializeThemeResources(this, "SfAccordionTheme");
			this.AddKeyboardListener(this);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the collection of <see cref="AccordionItem"/> in the <see cref="SfAccordion"/> control.
		/// </summary>
		/// <value>
		/// An observable collection of <see cref="AccordionItem"/> that represents the items contained in the accordion. The default value is null.
		/// </value>
		/// <remarks>
		/// Allows the inclusion of multiple accordion items within the <see cref="SfAccordion"/> control, where each item can have customized headers and content areas.
		/// </remarks>
		/// <example>
		/// Here is an example of how to define items within the <see cref="SfAccordion"/> control:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion>
		///     <syncfusion:SfAccordion.Items>
		///         <syncfusion:AccordionItem>
		///             <syncfusion:AccordionItem.Header>
		///                 <Grid>
		///                     <Label TextColor="#495F6E"
		///                            Text="Cheese burger"
		///                            HeightRequest="50"
		///                            VerticalTextAlignment="Center" />
		///                 </Grid>
		///             </syncfusion:AccordionItem.Header>
		///             <syncfusion:AccordionItem.Content>
		///                 <Grid Padding="10,10,10,10" BackgroundColor="#FFFFFF">
		///                     <Label TextColor="#303030"
		///                            Text="Hamburger accompanied with melted cheese. The term itself is a portmanteau of the words cheese and hamburger. The cheese is usually sliced, then added a short time before the hamburger finishes cooking to allow it to melt."
		///                            VerticalTextAlignment="Center" />
		///                 </Grid>
		///             </syncfusion:AccordionItem.Content>
		///         </syncfusion:AccordionItem>
		///     </syncfusion:SfAccordion.Items>
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var sfAccordion = new SfAccordion();
		/// var accordionItem = new AccordionItem();
		/// var headerGrid = new Grid();
		/// var headerLabel = new Label
		/// {
		///     TextColor = Color.FromArgb("#495F6E"),
		///     Text = "Cheese burger",
		///     HeightRequest = 50,
		///     VerticalTextAlignment = TextAlignment.Center
		/// };
		/// headerGrid.Children.Add(headerLabel);
		/// accordionItem.Header = headerGrid;
		///
		/// var contentGrid = new Grid { Padding = new Thickness(10) };
		/// var contentLabel = new Label
		/// {
		///     TextColor = Color.FromArgb("#303030"),
		///     Text = "Hamburger accompanied with melted cheese. The term itself is a portmanteau of the words cheese and hamburger. The cheese is usually sliced, then added a short time before the hamburger finishes cooking to allow it to melt.",
		///     VerticalTextAlignment = TextAlignment.Center
		/// };
		/// contentGrid.Children.Add(contentLabel);
		/// accordionItem.Content = contentGrid;
		///
		/// sfAccordion.Items.Add(accordionItem);
		/// ]]></code>
		/// </example>
		/// <seealso cref="AccordionItem"/>
		public ObservableCollection<AccordionItem> Items
		{
			get { return (ObservableCollection<AccordionItem>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether an end-user can expand a single or multiple accordion items.
		/// </summary>
		/// <value>
		/// An <see cref="AccordionExpandMode"/> value that organizes the expand and collapse state of the accordion items. The default value is <see cref="AccordionExpandMode.Single"/>.
		/// </value>
		/// <remarks>
		/// The <see cref="AccordionExpandMode"/> property allows the accordion to be configured so that either only a single item can be expanded at a time, or multiple items can be expanded simultaneously.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ExpandMode"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion ExpandMode="Multiple">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.ExpandMode = AccordionExpandMode.Multiple;
		/// ]]></code>
		/// </example>
		public AccordionExpandMode ExpandMode
		{
			get { return (AccordionExpandMode)GetValue(ExpandModeProperty); }
			set { SetValue(ExpandModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the duration of the animation when expanding or collapsing the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// A duration, in milliseconds, for the expand and collapse animation. The default value is 200 milliseconds.
		/// </value>
		/// <remarks>
		/// The animation duration determines how long it takes for accordion items to transition between expanded and collapsed states, enhancing the user experience by providing a smooth visual effect.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="AnimationDuration"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion AnimationDuration="1000">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.AnimationDuration = 1000;
		/// ]]></code>
		/// </example>
		public double AnimationDuration
		{
			get { return (double)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the spacing between the items in the <see cref="SfAccordion"/>.
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value representing the space between each accordion item. The default value is 6.0d.
		/// </value>
		/// <remarks>
		/// The spacing property determines the amount of space that appears between adjacent accordion items.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ItemSpacing"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion ItemSpacing="10.0">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.ItemSpacing = 10.0;
		/// ]]></code>
		/// </example>
		public double ItemSpacing
		{
			get { return (double)GetValue(ItemSpacingProperty); }
			set { SetValue(ItemSpacingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <see cref="ExpanderAnimationEasing"/> function applied to the animation when expanding or collapsing an item in the accordion.
		/// </summary>
		/// <value>
		/// An <see cref="ExpanderAnimationEasing"/> value that specifies the easing function for the expansion and collapse animations. The default value is <see cref="ExpanderAnimationEasing.Linear"/>.
		/// </value>
		/// <remarks>
		/// An easing function determines the speed progression of an animation, allowing for effects such as acceleration and deceleration.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="AnimationEasing"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion AnimationEasing="SinIn">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.AnimationEasing = ExpanderAnimationEasing.SinIn;
		/// ]]></code>
		/// </example>
		public ExpanderAnimationEasing AnimationEasing
		{
			get { return (ExpanderAnimationEasing)GetValue(AnimationEasingProperty); }
			set { SetValue(AnimationEasingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to specify the scroll position of the expanded <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// An <see cref="AccordionAutoScrollPosition"/> value that specifies how the expanded item is scrolled into view. The default value is <see cref="AccordionAutoScrollPosition.MakeVisible"/>.
		/// </value>
		/// <remarks>
		/// The <see cref="AccordionAutoScrollPosition"/> property determines the scroll behavior for an expanded accordion item, ensuring that it is appropriately positioned within the visible area of the control.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="AutoScrollPosition"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion AutoScrollPosition="Top">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.AutoScrollPosition = AccordionAutoScrollPosition.Top;
		/// ]]></code>
		/// </example>
		public AccordionAutoScrollPosition AutoScrollPosition
		{
			get { return (AccordionAutoScrollPosition)GetValue(AutoScrollPositionProperty); }
			set { SetValue(AutoScrollPositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <see cref="ExpanderIconPosition"/> of the header icon in the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// An <see cref="ExpanderIconPosition"/> value that determines the position of the header icon. The default value is <see cref="ExpanderIconPosition.End"/>.
		/// </value>
		/// <remarks>
		/// The <see cref="ExpanderIconPosition"/> property allows customization of the position of the header icon within the accordion item.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderIconPosition"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion HeaderIconPosition="Start">
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordion = new SfAccordion();
		/// accordion.HeaderIconPosition = ExpanderIconPosition.Start;
		/// ]]></code>
		/// </example>
		public ExpanderIconPosition HeaderIconPosition
		{
			get { return (ExpanderIconPosition)GetValue(HeaderIconPositionProperty); }
			set { SetValue(HeaderIconPositionProperty, value); }
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets a value indicating whether Accordion view is loaded or not.
		/// </summary>
		internal bool IsViewLoaded
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the current <see cref="AccordionItem"/>.
		/// </summary>
		internal AccordionItem? CurrentItem
        
		{
			get
			{
				return _currentItem;
			}
			set
			{
				_currentItem = value;
			}
		}

		#endregion

		#region Private Properties

		/// <summary>
		/// Gets or sets the semantic description of the current object.
		/// </summary>
	    string SemanticDescription
		{
			get
			{
				if (string.IsNullOrEmpty(_semanticDescription))
				{
					_semanticDescription = SemanticProperties.GetDescription(this);
				}

				return _semanticDescription;
			}
			set
			{
				_semanticDescription = value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Brings the specified <see cref="AccordionItem"/> into view by scrolling.
		/// </summary>
		/// <param name="item">The <see cref="AccordionItem"/> to scroll into view.</param>
		public void BringIntoView(AccordionItem item)
		{
			if (item == null)
			{
				return;
			}

			if (item._itemIndex >= 0 && _scrollView != null)
			{
				_scrollView.ScrollToAsync(item._accordionItemView, ScrollToPosition.MakeVisible, true);
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Forces an invalidation of the current layout, causing a new measure pass.
		/// </summary>
		internal void InvalidateForceLayout()
		{
			InvalidateMeasure();
		}

		/// <summary>
		/// Updates the IsExpanded property of an accordion item at a specified index.
		/// </summary>
		/// <param name="index">The index of the accordion item to update.</param>
		/// <param name="isExpanded">A boolean indicating whether the accordion item should be expanded.</param>
		internal void UpdateIsExpandValueBasedOnIndex(int index, bool isExpanded)
		{
			var item = Items[index];
			item.IsExpanded = isExpanded;

			// No need to deselect the selected items if the item is already selected
			var accordionItemView = item._accordionItemView;
			if (accordionItemView != null && !accordionItemView.IsSelected && accordionItemView._isRippleAnimationProgress)
			{
				UpdateSelection();
			}
		}

		/// <summary>
		/// Updates the selection status of accordion item views within the collection.
		/// </summary>
		internal void UpdateSelection()
		{
			var selectedItems = Items.ToList().FindAll(x => x._accordionItemView != null && x._accordionItemView.IsSelected);
			if (selectedItems.Count == 1 && Items[selectedItems[0]._itemIndex] is AccordionItem items && items._accordionItemView is AccordionItemView accordionItem)
			{
				accordionItem.IsSelected = false;
			}
		}

		/// <summary>
		/// Updates the accordion items based on the changes in expand modes.
		/// </summary>
		/// <param name="isExpandModeChanged">
		/// A boolean value indicating whether the expand mode has changed.
		/// </param>
		internal void UpdateAccordionItemsBasedOnExpandModes(bool isExpandModeChanged)
		{
			if (Items.Count == 0)
			{
				return;
			}

			var expandedItems = Items.ToList().FindAll(x => x._accordionItemView != null && x._accordionItemView.IsExpanded);
			switch (ExpandMode)
			{
				case AccordionExpandMode.Single:
				case AccordionExpandMode.SingleOrNone:
					{
						if (expandedItems.Count == 0 && ExpandMode == AccordionExpandMode.Single)
						{
							UpdateIsExpandValueBasedOnIndex(0, true);
						}
						else if (expandedItems.Count > 1)
						{
							for (int i = 0; i < expandedItems.Count - 1; i++)
							{
								UpdateIsExpandValueBasedOnIndex(expandedItems[i]._itemIndex, false);
							}
						}
						else if (expandedItems.Count == 1 && !isExpandModeChanged)
						{
							UpdateIsExpandValueBasedOnIndex(expandedItems[0]._itemIndex, false);
						}
					}

					break;
				case AccordionExpandMode.Multiple:
					{
						if (expandedItems.Count == 0)
						{
							UpdateIsExpandValueBasedOnIndex(0, true);
						}
					}

					break;
			}
		}

		/// <summary>
		/// Updates the auto-scroll position at runtime.
		/// </summary>
		/// <param name="item">
		/// The accordion item view that needs to be scrolled into position.
		/// </param>
		internal void UpdateAutoScrollToPosition(AccordionItemView item)
		{
			if (_scrollView != null)
			{
				switch (AutoScrollPosition)
				{
					case AccordionAutoScrollPosition.MakeVisible:
						_scrollView.ScrollToAsync(item, ScrollToPosition.MakeVisible, true);
						break;
					case AccordionAutoScrollPosition.Top:
#if IOS
						var contentSize = _scrollView.ContentSize;
						var itemY = (int)item.Bounds.Y;
						var scrollOffset = _scrollView.ScrollY;
						if (itemY + item.Bounds.Height + scrollOffset < (int)contentSize.Height)
						{
							_scrollView.ScrollToAsync(item, ScrollToPosition.Start, true);
						}
						else
						{
							var diff = (itemY + _scrollView.Height) - contentSize.Height;
							_scrollView.ScrollToAsync(0, itemY - diff, true);
						}
#else
						_scrollView.ScrollToAsync(item, ScrollToPosition.Start, true);
#endif
						break;
				}
			}
		}

		#endregion

		#region Protected Internal Methods

		/// <summary>
		/// Raises the collapsing event for the specified item index.
		/// </summary>
		/// <param name="index">
		/// The index of the item that is collapsing.
		/// </param>
		/// <returns>
		/// A boolean value indicating whether the collapsing action should be canceled.
		/// </returns>
		protected internal bool RaiseCollapsingEvent(int index)
		{
			if (Collapsing != null)
			{
				var args = new ExpandingAndCollapsingEventArgs(index);
				Collapsing(this, args);
				return !args.Cancel;
			}

			return true;
		}

		/// <summary>
		/// Raises the expanding event for the specified item index.
		/// </summary>
		/// <param name="index">
		/// The index of the item that is expanding.
		/// </param>
		/// <returns>
		/// A boolean value indicating whether the expanding action should be canceled.
		/// </returns>
		protected internal bool RaiseExpandingEvent(int index)
		{
			if (Expanding != null)
			{
				var args = new ExpandingAndCollapsingEventArgs(index);
				Expanding(this, args);
				return !args.Cancel;
			}

			return true;
		}

		/// <summary>
		/// Raises the collapsed event for the specified item index.
		/// </summary>
		/// <param name="index">
		/// The index of the item that has collapsed.
		/// </param>
		protected internal void RaiseCollapsedEvent(int index)
		{
			if (Collapsed != null)
			{
				var args = new ExpandedAndCollapsedEventArgs(index);
				Collapsed(this, args);
			}
		}

		/// <summary>
		/// Raises the expanded event for the specified item index.
		/// </summary>
		/// <param name="index">
		/// The index of the item that has expanded.
		/// </param>
		protected internal void RaiseExpandedEvent(int index)
		{
			if (Expanded != null)
			{
				var args = new ExpandedAndCollapsedEventArgs(index);
				Expanded(this, args);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles the event when the down key is pressed.
		/// </summary>
		/// <param name="selectedItem">The currently selected accordion item, or null if none is selected.</param>
		void OnDownKeyPressed(AccordionItem? selectedItem)
		{
			if (CurrentItem != null)
			{
				int currentIndex = CurrentItem._itemIndex;
				if (currentIndex < Items.Count - 1 && CurrentItem._accordionItemView != null)
				{
					AccordionItemView currentView = CurrentItem._accordionItemView;
					CurrentItem = Items[currentIndex + 1];
					if (currentView != null && currentView.HeaderView != null)
					{
						currentView.HeaderView.InvalidateDrawable();
					}

					if (CurrentItem != null && CurrentItem._accordionItemView != null && CurrentItem._accordionItemView.HeaderView != null)
					{
						CurrentItem._accordionItemView.HeaderView.InvalidateDrawable();
					}
				}
			}
			else if (selectedItem != null)
			{
				int selectedIndex = Items.IndexOf(selectedItem);
				int itemsCount = Items.Count - 1;
				if (selectedIndex >= 0)
				{
					if (selectedIndex < itemsCount)
					{
						CurrentItem = Items[selectedIndex + 1];
					}
					else if (selectedIndex == itemsCount)
					{
						CurrentItem = Items[itemsCount];
					}

					if (CurrentItem != null && CurrentItem._accordionItemView != null && CurrentItem._accordionItemView.HeaderView != null)
					{
						CurrentItem._accordionItemView.HeaderView.InvalidateDrawable();
					}
				}
			}
			else
			{
				CurrentItem = Items[0];
				if (CurrentItem != null && CurrentItem._accordionItemView != null && CurrentItem._accordionItemView.HeaderView != null)
				{
					CurrentItem._accordionItemView.HeaderView.InvalidateDrawable();
				}
			}

			if (CurrentItem != null)
			{
				BringIntoView(CurrentItem);
			}
		}

		/// <summary>
		/// Handles the event when the up or tab key is pressed.
		/// </summary>
		/// <param name="selectedItem">The currently selected accordion item, or null if none is selected.</param>
		void OnUpOrTabKeyPressed(AccordionItem? selectedItem)
		{
			if (CurrentItem != null)
			{
				int currentIndex = CurrentItem._itemIndex;
				if (currentIndex > 0 && currentIndex < Items.Count && CurrentItem._accordionItemView != null)
				{
					AccordionItemView currentView = CurrentItem._accordionItemView;
					CurrentItem = Items[currentIndex - 1];
					if (currentView != null && currentView.HeaderView != null)
					{
						currentView.HeaderView.InvalidateDrawable();
					}

					if (CurrentItem != null && CurrentItem._accordionItemView != null && CurrentItem._accordionItemView.HeaderView != null)
					{
						CurrentItem._accordionItemView.HeaderView.InvalidateDrawable();
					}
				}
			}
			else
			{
				if (selectedItem != null && Items.IndexOf(selectedItem) > 0)
				{
					CurrentItem = Items[Items.IndexOf(selectedItem) - 1];
				}
				else if (selectedItem != null && Items.IndexOf(selectedItem) == 0)
				{
					CurrentItem = Items[0];
				}

				if (CurrentItem != null && CurrentItem._accordionItemView != null && CurrentItem._accordionItemView.HeaderView != null)
				{
					CurrentItem._accordionItemView.HeaderView.InvalidateDrawable();
				}
			}

			if (CurrentItem != null)
			{
				BringIntoView(CurrentItem);
			}
		}

		/// <summary>
		/// Handles the event when the enter key is pressed.
		/// </summary>
		/// <param name="selectedItem">The currently selected accordion item, or null if none is selected.</param>
		void OnEnterKeyPressed(AccordionItem? selectedItem)
		{
			if (CurrentItem != null || selectedItem != null)
			{
				AccordionItemView? selectedItemView = null;
				if (selectedItem != null && selectedItem._accordionItemView != null)
				{
					selectedItemView = selectedItem._accordionItemView;
				}

				AccordionItemView? accordionItemView = CurrentItem != null ? CurrentItem._accordionItemView : selectedItemView;
				if (accordionItemView != null && accordionItemView.Accordion != null)
				{
					accordionItemView.Accordion.UpdateSelection();
					if (accordionItemView.IsExpanded)
					{
						accordionItemView.RaiseCollapsingEvent();
					}
					else
					{
						accordionItemView.RaiseExpandingEvent();
					}

					accordionItemView.IsSelected = true;
				}
			}
		}

		/// <summary>
		/// Resets the padding of the specified accordion item to zero.
		/// </summary>
		/// <param name="accordionItem">The <see cref="AccordionItem"/> whose padding is to be reset.</param>
		static void ResetAccordionItemPadding(AccordionItem accordionItem)
		{
			if (accordionItem._accordionItemView != null)
			{
				accordionItem._accordionItemView.Padding = new Thickness(0, 0, 0, 0);
			}
		}

		/// <summary>
		/// Updates the padding of the specified accordion item. The padding is adjusted based on specific criteria or application logic.
		/// </summary>
		/// <param name="accordionItem">The <see cref="AccordionItem"/> whose padding needs to be updated.</param>
		void UpdateAccordionItemPadding(AccordionItem accordionItem)
		{
			if (accordionItem._accordionItemView != null)
			{
				double top = accordionItem._accordionItemView.Padding.Top;
				double left = accordionItem._accordionItemView.Padding.Left;
				double right = accordionItem._accordionItemView.Padding.Right;
				double bottom = ItemSpacing;
				accordionItem._accordionItemView.Padding = new Thickness(left, top, right, bottom);
			}
		}

		/// <summary>
		/// Adds accordion items into the view. This method handles the layout and visual inclusion of each item in the accordion control.
		/// </summary>
		void AddAccordionItemsIntoView()
		{
			if (Items != null)
			{
				AddItemsIntoItemContainer();
				UpdateAccordionItemsBasedOnExpandModes(true);
			}

			if (_scrollView != null)
			{
				_scrollView.Content = _itemContainer;
				Children.Add(_scrollView);
			}
		}

		/// <summary>
		/// Adds items into the item container. This method is responsible for managing and organizing the items within the container.
		/// </summary>
		void AddItemsIntoItemContainer()
		{
			for (int i = 0; i < Items.Count; i++)
			{
				var item = Items[i];
				item._itemIndex = i;
				UpdateAccordionProperties(item);
				if (_itemContainer == null)
				{
					return;
				}

				_itemContainer.AddChildView(item._accordionItemView, i);
			}
		}

		/// <summary>
		/// Updates the properties of the specified accordion item.
		/// </summary>
		/// <param name="accordionItem">The accordion item whose properties will be updated.</param>
		void UpdateAccordionProperties(AccordionItem accordionItem)
		{
			accordionItem.Initialize(this);
			AccordionItemView? accordionItemView = accordionItem._accordionItemView;
			if (accordionItemView != null)
			{
				if (accordionItem.Header != null)
				{
					accordionItemView.Header = accordionItem.Header;
					accordionItemView.Header.BindingContext = accordionItem.BindingContext;
				}

				if (accordionItem.Content != null)
				{
					accordionItemView.Content = accordionItem.Content;
					accordionItemView.Content.BindingContext = accordionItem.BindingContext;
				}

				accordionItemView.HeaderBackground = accordionItem.HeaderBackground;
				accordionItemView.HeaderIconColor = accordionItem.HeaderIconColor;
				accordionItemView.IsExpanded = accordionItem.IsExpanded;

				accordionItemView.AnimationDuration = AnimationDuration;
				accordionItemView.AnimationEasing = AnimationEasing;
				accordionItemView.HeaderIconPosition = HeaderIconPosition;

				// Implementation for IsEnabled property
				if (!IsEnabled)
				{
					accordionItemView.IsEnabled = IsEnabled;
				}
				else if (!accordionItem.IsEnabled)
				{
					accordionItemView.IsEnabled = accordionItem.IsEnabled;
				}

				// Implementation for accessibility.
				accordionItemView.SemanticDescription = SemanticDescription + " Item" + accordionItem._itemIndex;
			}

			// Added spacing between accordion items. Item spacing updeted only bottom of the accordion item.
			if (accordionItem._itemIndex < Items.Count - 1)
			{
				UpdateAccordionItemPadding(accordionItem);
			}
		}

		/// <summary>
		/// Initializes the accordion by setting up its components.
		/// </summary>
		void InitializeAccordion()
		{
			_itemContainer = new ItemContainer(this);
			_scrollView = new()
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Default
			};

			Items = [];
			Background = Colors.Transparent;
		}

		/// <summary>
		/// Event handler that executes when the accordion is loaded.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		void OnAccordionLoaded(object? sender, System.EventArgs e)
		{
			if (!IsViewLoaded)
			{
				IsViewLoaded = true;
				AddAccordionItemsIntoView();
			}
		}

		/// <summary>
		/// Event handler that executes when a child element is removed from the accordion.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data containing details about the removed element.</param>
		void OnAccordionChildRemoved(object? sender, ElementEventArgs e)
		{
			var accordionItem = e.Element as AccordionItem;
			if (accordionItem == null && e.Element is not ScrollView)
			{
				throw new NotImplementedException("Invalid type added as child");
			}

			if (accordionItem != null)
			{
				Items.Remove(accordionItem);
			}
		}

		/// <summary>
		/// This method enables the necessary setting to the KeyListener.
		/// </summary>
		void SetUpKeyListenerRequirements()
		{
			if (Handler != null && Handler.PlatformView != null)
			{
#if WINDOWS
                if (Handler.PlatformView is Microsoft.UI.Xaml.FrameworkElement frameWorkElement)
                {
                    frameWorkElement.IsTabStop = true;
                }
#elif ANDROID
                if (Handler.PlatformView is Android.Views.View view)
                {
                    view.FocusableInTouchMode = true;
                }
#endif
			}
		}

		void AddAccordionItem(NotifyCollectionChangedEventArgs e)
		{
			AccordionItem? accordionItem = null;
			if (e.NewItems != null)
			{
				accordionItem = e.NewItems[0] as AccordionItem;
			}

			if (accordionItem != null)
			{
				accordionItem._itemIndex = e.NewStartingIndex;
				if (e.NewStartingIndex == Items.Count - 1)
				{
					if (e.NewStartingIndex - 1 >= 0)
					{
						UpdateAccordionItemPadding(Items[e.NewStartingIndex - 1]);
					}
				}
				else
				{
					for (int i = e.NewStartingIndex + 1; i < Items.Count; i++)
					{
						Items[i]._itemIndex++;
					}
				}

				UpdateAccordionProperties(accordionItem);
				_itemContainer?.AddChildView(accordionItem._accordionItemView, accordionItem._itemIndex);
				if (e.NewStartingIndex == Items.Count - 1 && accordionItem != null)
				{
					var index = e.NewStartingIndex - 1;
					if (index >= 0)
					{
						Items[index]._accordionItemView?.InvalidateForceLayout();
					}
				}
			}
		}

		void RemoveAccordionItem(NotifyCollectionChangedEventArgs e)
		{
			AccordionItem? accordionItem = null;
			if (e.OldItems != null)
			{
				accordionItem = e.OldItems[0] as AccordionItem;
			}

			if (e.OldStartingIndex - 1 >= 0 && e.OldStartingIndex - 1 == Items.Count - 1)
			{
				ResetAccordionItemPadding(Items[e.OldStartingIndex - 1]);
			}

			if (_itemContainer != null && accordionItem != null && accordionItem._accordionItemView != null)
			{
				_itemContainer.RemoveChildrenInView(accordionItem._accordionItemView);
			}

			for (int i = e.OldStartingIndex; i < Items.Count; i++)
			{
				Items[i]._itemIndex--;
			}

			var oldStartingIndex = e.OldStartingIndex - 1;
			if (oldStartingIndex >= 0)
			{
				Items[oldStartingIndex]._accordionItemView?.InvalidateForceLayout();
			}

		}

		void ReplaceAccordionItem(NotifyCollectionChangedEventArgs e)
		{
			AccordionItem? accordionItem = null;
			if (e.OldItems != null)
			{
				accordionItem = e.OldItems[0] as AccordionItem;
			}

			if (_itemContainer != null && accordionItem != null && accordionItem._accordionItemView != null)
			{
				_itemContainer.RemoveChildrenInView(accordionItem._accordionItemView);

				if (e.NewItems != null)
				{
					accordionItem = e.NewItems[0] as AccordionItem;
				}

				if (accordionItem != null)
				{
					UpdateAccordionProperties(accordionItem);
					_itemContainer.AddChildView(accordionItem._accordionItemView, e.NewStartingIndex);
				}
			}

			if (accordionItem != null)
			{
				accordionItem._itemIndex = e.NewStartingIndex;
			}
		}

		void MoveAccordionItem(NotifyCollectionChangedEventArgs e)
		{
			if (e.NewStartingIndex > e.OldStartingIndex)
			{
				for (int i = e.NewStartingIndex; i >= e.OldStartingIndex; i--)
				{
					Items[i]._itemIndex = i;
				}
			}
			else if (e.NewStartingIndex < e.OldStartingIndex)
			{
				for (int i = e.NewStartingIndex; i <= e.OldStartingIndex; i++)
				{
					Items[i]._itemIndex = i;
				}
			}

			if (e.OldStartingIndex == Items.Count - 1)
			{
				ResetAccordionItemPadding(Items[e.OldStartingIndex]);
				UpdateAccordionItemPadding(Items[e.NewStartingIndex]);
			}
			else if (e.NewStartingIndex == Items.Count - 1)
			{
				ResetAccordionItemPadding(Items[e.NewStartingIndex]);
				UpdateAccordionItemPadding(Items[e.OldStartingIndex]);
			}

			if (_itemContainer != null)
			{
				if (_itemContainer.Children[e.OldStartingIndex] is View accordionItemView)
				{
					_itemContainer.RemoveChildAtIndex(e.OldStartingIndex);

					_itemContainer.AddChildView(accordionItemView, e.NewStartingIndex);
				}
			}

		}

		/// <summary>
		/// Handles changes to the collection of accordion items.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data containing information about the collection changes.</param>
		void OnAccordionItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (!IsViewLoaded)
			{
				return;
			}

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					AddAccordionItem(e);
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveAccordionItem(e);

					break;
				case NotifyCollectionChangedAction.Replace:
					ReplaceAccordionItem(e);
					break;
				case NotifyCollectionChangedAction.Reset:
					if (_itemContainer == null)
					{
						return;
					}

					_itemContainer.RemoveChildrens();
					break;
				case NotifyCollectionChangedAction.Move:
					MoveAccordionItem(e);
					break;
			}
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Method used to measure the accordion layout children based on width and height value.
		/// </summary>
		/// <param name="widthConstraint">The maximum width request of the layout.</param>
		/// <param name="heightConstraint">The maximum height request of the layout.</param>
		/// <returns>The required size of the layout.</returns>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
#if ANDROID
            if (Handler != null && Handler.PlatformView != null && Handler.PlatformView is Android.Views.ViewGroup view)
            {
                view.ForceLayout();
            }
#endif
			return base.MeasureOverride(widthConstraint, heightConstraint);
		}

		/// <summary>
		/// Called whenever a property value changes of <see cref="SfAccordion"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == nameof(IsEnabled))
			{
				foreach (var item in Items)
				{
					var accordionItemView = item._accordionItemView;
					if (accordionItemView != null)
					{
						accordionItemView.IsEnabled = IsEnabled;
						if (IsEnabled && !item.IsEnabled)
						{
							accordionItemView.IsEnabled = item.IsEnabled;
						}
					}
				}
			}

			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Method used to create the accordion layout manager.
		/// </summary>
		/// <returns>An instance of <see cref="AccordionLayoutManager"/> for this control.</returns>
		protected override ILayoutManager CreateLayoutManager()
		{
			return new AccordionLayoutManager(this);
		}

		/// <summary>
		/// Invoked when a child element is added to the layout.
		/// </summary>
		/// <param name="child">The child <see cref="Element"/> that has been added.</param>
		protected override void OnChildAdded(Element child)
		{
			var accordionItem = child as AccordionItem;
			if (accordionItem == null && child is not ScrollView)
			{
				throw new NotImplementedException("Invalid type added as child");
			}

			// To avoid adding ScrollView in Items collection.
			// Only have to add AccordionItem type objects in Items collection.
			if (accordionItem != null)
			{
				var index = Children.IndexOf(child as IView);
				if (index >= Items.Count)
				{
					Items.Add(accordionItem);
				}
				else
				{
					Items.Insert(index, accordionItem);
				}
			}

			base.OnChildAdded(child);
		}

		/// <summary>
		/// Raised when handler gets changed.
		/// </summary>
		protected override void OnHandlerChanged()
		{
			SetUpKeyListenerRequirements();
			base.OnHandlerChanged();
		}

		/// <summary>
		/// Arranges the layout of child elements within the specified bounds.
		/// </summary>
		/// <param name="bounds">
		/// The rectangle that defines the size and position of the arrangement area.
		/// </param>
		/// <returns>
		/// The actual size used to arrange the children.
		/// </returns>
		internal override Size LayoutArrangeChildren(Rect bounds)
		{
			if (_scrollView != null && _scrollView is IView view)
			{
				view.Arrange(bounds);
			}

			return new Size(bounds.Width, bounds.Height);
		}

		/// <summary>
		/// Measures the size required for arranging child elements within the specified constraints.
		/// </summary>
		/// <param name="widthConstraint">
		/// The maximum width available for the layout.
		/// </param>
		/// <param name="heightConstraint">
		/// The maximum height available for the layout.
		/// </param>
		/// <returns>
		/// The size that the layout needs based on the constraints provided.
		/// </returns>
		internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
		{
			// When clear binding collection of BindableLayout, ScrollView is removed from Accordion.
			// So view is getting empty. To avoid this, we have added ScrollView again.
			if (IsViewLoaded && Children.IndexOf(_scrollView) == -1)
			{
				Children.Add(_scrollView);
			}

			double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
			double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;

			// To update width when loaded accordion inside HorizontalStackLayout and AbsoluteLayout.
			if (width == 0)
			{
#if !WINDOWS
				var scaledScreenSize = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
#else
                var scaledScreenSize = new Size(300,300);
#endif
				double scaledWidth = Math.Min(scaledScreenSize.Width, scaledScreenSize.Height);

				width = scaledWidth;
			}

			// SfAccordion Height was not updated properly in BindableLayout when using Grid as "Auto"
			if (_scrollView is IView view)
			{
				view.Measure(width, height);
			}

			// To update height when loaded accordion inside StackLayout, ScrollView,etc.
			if (height == 0 && _itemContainer != null && _itemContainer.DesiredSize.Height > 0)
			{
				height = _itemContainer.DesiredSize.Height;
			}

			return new Size(width, height);
		}

		#endregion

		#region PropertyChanged Methods

		/// <summary>
		/// Handles the property changed method for the expand mode property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of expand mode property.</param>
		/// <param name="newValue">The new value of expand mode property. </param>
		static void OnExpandModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion && accordion.IsViewLoaded)
			{
				accordion.UpdateAccordionItemsBasedOnExpandModes(true);
			}
		}

		/// <summary>
		/// Handles the property changed method for the item spacing property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of item spacing property.</param>
		/// <param name="newValue">The new value of item spacing property. </param>
		static void OnItemSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion && accordion.IsViewLoaded)
			{
				foreach (var item in accordion.Items)
				{
					accordion.UpdateAccordionItemPadding(item);
					if (item is AccordionItem accordionItem && accordionItem._accordionItemView is AccordionItemView accordionItemView)
					{
						accordionItemView.InvalidateForceLayout();
					}
				}
			}
		}

		/// <summary>
		/// Handles the property changed method for the animation duaration property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of animation duration property.</param>
		/// <param name="newValue">The new value of animation duration property. </param>
		static void OnAnimationDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion && accordion.IsViewLoaded)
			{
				foreach (AccordionItem item in accordion.Items)
				{
					if (item._accordionItemView is AccordionItemView accordionItem)
					{
						accordionItem.AnimationDuration = accordion.AnimationDuration;
					}
				}
			}
		}

		/// <summary>
		/// Handles the property changed method for the animation easing property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of animation easing property.</param>
		/// <param name="newValue">The new value of animation easing property. </param>
		static void OnAnimationEasingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion && accordion.IsViewLoaded)
			{
				foreach (AccordionItem item in accordion.Items)
				{
					if (item._accordionItemView is AccordionItemView accordionItem)
					{
						accordionItem.AnimationEasing = accordion.AnimationEasing;
					}
				}
			}
		}

		/// <summary>
		/// Handles the property changed method for the header icon position property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of header icon position property.</param>
		/// <param name="newValue">The new value of header icon position property. </param>
		static void OnHeaderIconPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion && accordion.IsViewLoaded)
			{
				foreach (AccordionItem item in accordion.Items)
				{
					if (item._accordionItemView is AccordionItemView accordionItem)
					{
						accordionItem.HeaderIconPosition = accordion.HeaderIconPosition;
					}
				}
			}
		}

		/// <summary>
		/// Handles the property changed method for the items property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of items property.</param>
		/// <param name="newValue">The new value of items property. </param>
		static void OnItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfAccordion accordion)
			{
				ObservableCollection<AccordionItem> oldItems = (ObservableCollection<AccordionItem>)oldValue;
				ObservableCollection<AccordionItem> newItems = (ObservableCollection<AccordionItem>)newValue;
				if (oldItems != null)
				{
					oldItems.CollectionChanged -= accordion.OnAccordionItemsCollectionChanged;
					oldItems.Clear();
				}

				if (newItems != null)
				{
					newItems.CollectionChanged += accordion.OnAccordionItemsCollectionChanged;
				}

				if (newItems != null && newItems.Count > 0)
				{
					if (accordion._itemContainer != null)
					{
						accordion._itemContainer.RemoveChildrens();
						accordion.AddItemsIntoItemContainer();
					}
				}
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method invokes when control theme changes.
		/// </summary>
		/// <param name="oldTheme">Represents the  old theme.</param>
		/// <param name="newTheme">Represents the  new theme.</param>
		public void OnControlThemeChanged(string oldTheme, string newTheme)
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

		/// <summary>
		/// Retrieves the theme dictionary for the parent theme element.
		/// </summary>
		/// <returns>A <see cref="ResourceDictionary"/> containing the theme styles, specifically an instance of <see cref="SfAccordionStyles"/>.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfAccordionStyles();
		}

		/// <summary>
		/// Handles the key up event for keyboard interactions.
		/// </summary>
		/// <param name="e">The <see cref="KeyEventArgs"/> containing the event data for the key that was released.</param>
		void IKeyboardListener.OnKeyUp(KeyEventArgs e)
		{
		}

		/// <summary>
		/// Handles the preview key down event for keyboard interactions.
		/// </summary>
		/// <param name="args">The <see cref="Syncfusion.Maui.Toolkit.Internals.KeyEventArgs"/> containing the event data.</param>
		void IKeyboardListener.OnPreviewKeyDown(Syncfusion.Maui.Toolkit.Internals.KeyEventArgs args)
		{
			if (args.Key == KeyboardKey.Tab)
			{
				(this as IKeyboardListener).OnKeyDown(args);
				args.Handled = true;
			}
		}

		/// <summary>
		/// Processes the key down event for keyboard interactions.
		/// </summary>
		/// <param name="e">The <see cref="KeyEventArgs"/> containing the event data for the key that was pressed.</param>
		void IKeyboardListener.OnKeyDown(KeyEventArgs e)
		{
			var selectedItem = Items.ToList().FirstOrDefault(x => x._accordionItemView != null && x._accordionItemView.IsSelected);
			if (e.Key == KeyboardKey.Down || (e.Key == KeyboardKey.Tab && !e.IsShiftKeyPressed))
			{
				OnDownKeyPressed(selectedItem);
				e.Handled = true;
			}

			if (e.Key == KeyboardKey.Up || (e.Key == KeyboardKey.Tab && e.IsShiftKeyPressed))
			{
				OnUpOrTabKeyPressed(selectedItem);
				e.Handled = true;
			}

			if (e.Key == KeyboardKey.Enter)
			{
				OnEnterKeyPressed(selectedItem);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when an <see cref="AccordionItem"/> is in the process of expanding within the <see cref="SfAccordion"/> control.
		/// </summary>
		/// <seealso cref="Expanded"/>
		/// <seealso cref="Collapsed"/>
		/// <seealso cref="Collapsing"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Expanding"/> event.
		/// 
		/// <code lang="C#">
		/// accordion.Expanding += OnAccordionExpanding;
		/// private void OnAccordionExpanding(object sender, ExpandingAndCollapsingEventArgs e)
		/// {
		///     var index = e.Index;
		///     e.Cancel = true;
		/// }
		/// </code>
		/// </example>
		public event EventHandler<ExpandingAndCollapsingEventArgs>? Expanding;

		/// <summary>
		/// Occurs when an <see cref="AccordionItem"/> is expanded within the <see cref="SfAccordion"/> control.
		/// </summary>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Collapsed"/>
		/// <seealso cref="Collapsing"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Expanded"/> event.
		/// 
		/// <code lang="C#">
		/// accordion.Expanded += OnAccordionExpanded;
		/// private void OnAccordionExpanded(object sender, ExpandedAndCollapsedEventArgs e)
		/// {
		///     var index = e.Index;
		/// }
		/// </code>
		/// </example>
		public event EventHandler<ExpandedAndCollapsedEventArgs>? Expanded;

		/// <summary>
		/// Occurs when an <see cref="AccordionItem"/> is being collapsed within the <see cref="SfAccordion"/> control.
		/// </summary>
		/// <seealso cref="Collapsed"/>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Expanded"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Collapsing"/> event.
		/// 
		/// <code lang="C#">
		/// accordion.Collapsing += OnAccordionCollapsing;
		/// private void OnAccordionCollapsing(object sender, ExpandingAndCollapsingEventArgs e)
		/// {
		///     e.Cancel = true;
		/// }
		/// </code>
		/// </example>
		public event EventHandler<ExpandingAndCollapsingEventArgs>? Collapsing;

		/// <summary>
		/// Occurs when an <see cref="AccordionItem"/> is collapsed.
		/// </summary>
		/// <example>
		/// <code lang="C#">
		/// accordion.Collapsed += OnAccordionCollapsed;
		/// private void OnAccordionCollapsed(object sender, ExpandedAndCollapsedEventArgs e)
		/// {
		///     var index = e.Index;
		/// }
		/// </code>
		/// </example>
		/// <seealso cref="Collapsing"/>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Expanded"/>
		public event EventHandler<ExpandedAndCollapsedEventArgs>? Collapsed;

		#endregion
	}

	/// <summary>
	///  Custom accordion layout that used to measure and arrange the children by using <see cref="AccordionLayoutManager"/>.
	/// </summary>
	public abstract class AccordionLayout : Layout
	{
		#region Internal Methods

		/// <summary>
		/// Method used to arrange the children with in the bounds.
		/// It is triggered from <see cref="AccordionLayoutManager.ArrangeChildren(Rect)"/>.
		/// </summary>
		/// <param name="bounds">The size of the layout.</param>
		/// <returns>The size.</returns>
		internal abstract Size LayoutArrangeChildren(Rect bounds);

		/// <summary>
		/// Meathod used to measure the children based on width and height value.
		/// It is triggered from <see cref="AccordionLayoutManager.Measure(double, double)"/>.
		/// </summary>
		/// <param name="widthConstraint">The maximum width request of the layout.</param>
		/// <param name="heightConstraint">The maximum height request of the layout.</param>
		/// <returns>The maximum size of the layout.</returns>
		internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);

		#endregion

		#region Override Methods

		/// <summary>
		/// Method to inavlidate layout measure.
		/// </summary>
		protected override void InvalidateMeasure()
		{
			base.InvalidateMeasure();

			//// In framework they are using size cache, so while measure calling with the exising size the child measure not triggering and uses the cache information.
			//// Hence all child control mesures are invalidated while parent measure invalidated.
			if (Children != null && Children.Count > 0)
			{
				foreach (var child in Children)
				{
					if (child is ScrollView)
					{
						child.InvalidateMeasure();
						break;
					}
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// Layout manager used to handle the measure and arrangement logic of the<see cref="AccordionLayout"/> children.
	/// </summary>
	internal class AccordionLayoutManager : LayoutManager
	{

		#region Fields

		readonly AccordionLayout _layout;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AccordionLayoutManager"/> class.
		/// </summary>
		/// <param name="layout">The scheduler layout instance.</param>
		internal AccordionLayoutManager(AccordionLayout layout)
			: base(layout)
		{
			_layout = layout;
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Method used to arrange the accordion layout children with in the bounds.
		/// It triggers <see cref="AccordionLayout.LayoutArrangeChildren(Rect)"/>.
		/// </summary>
		/// <param name="bounds">The size of the layout.</param>
		/// <returns>The size.</returns>
		public override Size ArrangeChildren(Rect bounds)
		{
			return _layout.LayoutArrangeChildren(bounds);
		}

		/// <summary>
		/// Meathod used to measure the accordion layout children based on width and height value.
		/// It triggers from <see cref="AccordionLayout.LayoutMeasure(double, double)"/>.
		/// </summary>
		/// <param name="widthConstraint">The maximum width request of the layout.</param>
		/// <param name="heightConstraint">The maximum height request of the layout.</param>
		/// <returns>The maximum size of the layout.</returns>
		public override Size Measure(double widthConstraint, double heightConstraint)
		{
			var measuredSize = _layout.LayoutMeasure(widthConstraint, heightConstraint);

			if (measuredSize.Height < 0)
			{
				measuredSize.Height = 0;
			}

			return measuredSize;
		}

		#endregion
	}
}
