using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Accordion
{
    /// <summary>
    /// Represents an item within the <see cref="SfAccordion"/> control.
    /// </summary>
    public partial class AccordionItem : SfView, IThemeElement
    {
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Header"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderProperty"/> determines the appearance of the header.
		/// </remarks>
		public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
				nameof(Header), typeof(View),
				typeof(AccordionItem),
				null,
				BindingMode.OneWay,
				null,
				OnHeaderPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Content"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ContentProperty"/> determines the content displayed within the control.
		/// </remarks>
		public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(
				nameof(Content),
				typeof(View),
				typeof(AccordionItem),
				null,
				BindingMode.OneWay,
				null,
				OnContentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsExpanded"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsExpandedProperty"/> determines whether the control is currently expanded or collapsed.
		/// </remarks>
		public static readonly BindableProperty IsExpandedProperty =
             BindableProperty.Create(
				 nameof(IsExpanded),
				 typeof(bool),
				 typeof(AccordionItem),
				 false,
				 BindingMode.TwoWay,
				 null,
				 OnIsExpandedPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderBackgroundProperty"/> determines the background appearance of the header.
		/// </remarks>
		public static readonly BindableProperty HeaderBackgroundProperty =
            BindableProperty.Create(
				nameof(HeaderBackground),
				typeof(Brush),
				typeof(AccordionItem),
				new SolidColorBrush(Color.FromArgb("#00000000")),
				BindingMode.Default,
				null,
				OnHeaderBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderIconColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderIconColorProperty"/> determines the color of the icon displayed in the header.
		/// </remarks>
		public static readonly BindableProperty HeaderIconColorProperty =
          BindableProperty.Create(
			  nameof(HeaderIconColor),
			  typeof(Color),
			  typeof(AccordionItem),
			  Color.FromArgb("#49454F"),
			  BindingMode.OneWay,
			  null,
			  OnHeaderIconColorPropertyChanged);

        #endregion

		#region Properties

		/// <summary>
		/// Gets or sets the custom view that represents the header of the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// A <see cref="View"/> that defines the appearance of the header. The default value is null.
		/// </value>
		/// <remarks>
		/// By default, the header of an <see cref="AccordionItem"/> can be customized to display any view as per the requirement.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="Header"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:AccordionItem.Header>
		///     <Grid>
		///         <Label TextColor="#495F6E"
		///                Text="Cheese burger"
		///                HeightRequest="50"
		///                VerticalTextAlignment="Center" />
		///     </Grid>
		/// </syncfusion:AccordionItem.Header>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordionItem = new AccordionItem();
		/// var grid = new Grid();
		/// var label = new Label
		/// {
		///		TextColor = Color.FromArgb("#495F6E"),
		///		Text = "Cheese burger",
		///		HeightRequest = 50,
		///		VerticalTextAlignment = TextAlignment.Center
		/// };
		/// grid.Children.Add(label);
		/// accordionItem.Header = grid;
		/// ]]></code>
		/// </example>
		/// <seealso cref="Content"/>
		public View Header
        {
            get { return (View)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

		/// <summary>
		/// Gets or sets the view that represents the content of the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="View"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// This property allows you to define the content displayed when the accordion item is expanded.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="Content"/> property for an <see cref="AccordionItem"/>:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:AccordionItem.Content>
		///		<Grid Padding="10,10,10,10" BackgroundColor="#FFFFFF">
		///			<Label TextColor="#303030"
		///				   Text="Hamburger accompanied with melted cheese. The term itself is a portmanteau of the words cheese and hamburger. The cheese is usually sliced, then added a short time before the hamburger finishes cooking to allow it to melt."
		///                VerticalTextAlignment="Center" />
		///     </Grid>
		/// </syncfusion:AccordionItem.Content>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordionItem = new AccordionItem();
		/// var grid = new Grid
		/// {
		///     Padding = new Thickness(10, 10, 10, 10),
		///     BackgroundColor = Colors.White
		/// };
		/// var label = new Label
		/// {
		///     TextColor = Color.FromHex("#303030"),
		///     Text = "Hamburger accompanied with melted cheese. The term itself is a portmanteau of the words cheese and hamburger. The cheese is usually sliced, then added a short time before the hamburger finishes cooking to allow it to melt.",
		///     VerticalTextAlignment = TextAlignment.Center
		/// };
		/// grid.Children.Add(label);
		/// accordionItem.Content = grid;
		/// ]]></code>
		/// </example>
		/// <seealso cref="Header"/>
		public View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="AccordionItem"/> is expanded or collapsed.
		/// </summary>
		/// <value>
		/// It accepts <see cref="bool"/>. The default value is false.
		/// </value>
		/// <remarks>
		/// This property controls the expanded state of the accordion item.
		/// When set to <c>true</c>, the item's content is shown, and when set to <c>false</c>, the content is hidden.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="IsExpanded"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion>
		///     <syncfusion:SfAccordion.Items>
		///         <syncfusion:AccordionItem IsExpanded="True">
		///             <!-- Header and Content definitions -->
		///         </syncfusion:AccordionItem>
		///     </syncfusion:SfAccordion.Items>
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordionItem = new AccordionItem
		/// {
		///     IsExpanded = true
		/// };
		/// // Define the header and content for the accordion item here.
		/// ]]></code>
		/// </example>
		public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

		/// <summary>
		/// Gets or sets the background color of the header in the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="Color"/> value. The default value is Color.FromArgb("#00000000").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the color appearance of the accordion header.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderBackground"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion>
		///     <syncfusion:SfAccordion.Items>
		///         <syncfusion:AccordionItem HeaderBackground="LightBlue">
		///             <!-- Header and Content definitions -->
		///         </syncfusion:AccordionItem>
		///     </syncfusion:SfAccordion.Items>
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordionItem = new AccordionItem
		/// {
		///     HeaderBackground = Colors.LightBlue
		/// };
		/// // Define the header and content for the accordion item here
		/// ]]></code>
		/// </example>
		/// <seealso cref="HeaderIconColor"/>
		public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

		/// <summary>
		/// Gets or sets the color of the header icon in the <see cref="AccordionItem"/>.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="Color"/> value. The default value is Color.FromArgb("#49454F").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the color of the icon displayed in the header of the accordion item. 
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderIconColor"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfAccordion>
		///     <syncfusion:SfAccordion.Items>
		///         <syncfusion:AccordionItem HeaderIconColor="DarkRed">
		///             <!-- Header and Content definitions -->
		///         </syncfusion:AccordionItem>
		///     </syncfusion:SfAccordion.Items>
		/// </syncfusion:SfAccordion>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var accordionItem = new AccordionItem
		/// {
		///     HeaderIconColor = Color.DarkRed
		/// };
		/// // Define the header and content for the accordion item here
		/// ]]></code>
		/// </example>
		/// <seealso cref="HeaderBackground"/>
		public Color HeaderIconColor
        {
            get { return (Color)GetValue(HeaderIconColorProperty); }
            set { SetValue(HeaderIconColorProperty, value); }
        }

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the view associated with the accordion item.
		/// </summary>
		internal AccordionItemView? _accordionItemView { get; set; }

		/// <summary>
		/// Gets or sets the index of the accordion item within the <see cref="SfAccordion"/>.
		/// </summary>
		internal int _itemIndex { get; set; }

		/// <summary>
		/// Gets or sets the instance of the accordion.
		/// </summary>
		internal SfAccordion? _accordion { get; set; }

		#endregion

		#region Internal Methods

		/// <summary>
		/// Initializes the specified SfAccordion.
		/// </summary>
		/// <param name="accordion">The SfAccordion instance to initialize.</param>
		internal void Initialize(SfAccordion accordion)
		{
			if (_accordion == null)
			{
				_accordion = accordion;
				Parent = accordion;
			}

			_accordionItemView ??= new AccordionItemView()
			{
				Accordion = accordion,
				AccordionItem = this
			};
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the visual state of the specified AccordionItem based on its expansion state.
		/// </summary>
		/// <param name="accordionItem">The AccordionItem whose visual state needs to be updated.</param>
		static void UpdateVisualState(AccordionItem accordionItem)
        {
			var isExpanded = accordionItem.IsExpanded ? "Expanded" : "Collapsed";
			VisualStateManager.GoToState(accordionItem, isExpanded);
        }

		/// <summary>
		/// Handles the logic when the IsExpanded property is changing for an AccordionItem.
		/// </summary>
		/// <param name="value">The new expanded state to apply to the AccordionItem.</param>
		void OnIsExpandedChanging(bool value)
		{
			if (_accordionItemView != null)
			{
				if (_accordion != null)
				{
					if (_accordion._isInInternalChange)
					{
						_accordionItemView.IsExpanded = value;
						IsExpanded = value;
						return;
					}

					if (value)
					{
						_accordion._isInInternalChange = true;
						_accordion.UpdateAccordionItemsBasedOnExpandModes(false);
						_accordion._isInInternalChange = false;
						_accordionItemView.IsExpanded = value;
						IsExpanded = value;
					}
					else
					{
						// In Single and Multiple mode, we should not allow to collapse all items.
						if (!_accordionItemView.CanCollapseItemOnSingleAndMultipleExpandMode())
						{
							_accordionItemView.IsExpanded = value;
							IsExpanded = value;
						}
						else
						{
							// Update the IsExpanded value to true when no need to collapse the accordion item.
							_accordion.UpdateIsExpandValueBasedOnIndex(_itemIndex, true);
						}
					}
				}
				else
				{
					_accordionItemView.IsExpanded = value;
				}
			}
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Changes the visual state of the current object.
		/// </summary>
		/// /// <remarks>
		/// This method overrides the base implementation to update the visual state based on the current state of this object.
		/// </remarks>
		protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            UpdateVisualState(this);
        }

		/// <summary>
		/// Handles the event when the binding context changes for this object.
		/// </summary>
		/// <remarks>
		/// This method updates the binding context of the Header and Content properties to match the new binding context of this object.
		/// </remarks>
		protected override void OnBindingContextChanged()
        {
            if (Header != null)
            {
                Header.BindingContext = BindingContext;
            }

            if (Content != null)
            {
                Content.BindingContext = BindingContext;
            }

            base.OnBindingContextChanged();
        }

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Handles the property changed method for the IsExpanded property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of IsExpanded property.</param>
		/// <param name="newValue">The new value of IsExpanded property. </param>
		static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// Content does not get collapsed when item is being collapsed in PCL view.
			if (bindable is AccordionItem accordionItem)
			{
				if (accordionItem._accordion != null && accordionItem._accordion.IsViewLoaded && accordionItem._accordionItemView != null && accordionItem._accordionItemView.IsExpanded != accordionItem.IsExpanded)
				{
					accordionItem.OnIsExpandedChanging((bool)newValue);
				}

				UpdateVisualState(accordionItem);
			}
		}

		/// <summary>
		/// Handles the property changed method for the header property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of header property.</param>
		/// <param name="newValue">The new value of header property. </param>
		static void OnHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is AccordionItem accordionItem && accordionItem._accordion != null && accordionItem._accordion.IsViewLoaded && accordionItem._accordionItemView != null)
			{
				accordionItem._accordionItemView.Header = (View)newValue;
			}
		}

		/// <summary>
		/// Handles the property changed method for the content property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of content property.</param>
		/// <param name="newValue">The new value of content property. </param>
		static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// When the Content is changed at runtime, need to update its visibility based on IsExpanded property.
			if (bindable is AccordionItem accordionItem && newValue is View content && accordionItem._accordion != null && accordionItem._accordion.IsViewLoaded)
			{
				content.IsVisible = accordionItem.IsExpanded;
				if (accordionItem._accordionItemView != null)
				{
					accordionItem._accordionItemView.Content = content;
				}
				
			}
		}

		/// <summary>
		/// Handles the property changed method for the header background property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of header background property.</param>
		/// <param name="newValue">The new value of header background property. </param>
		static void OnHeaderBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is AccordionItem accordionItem && accordionItem._accordion != null && accordionItem._accordion.IsViewLoaded && accordionItem._accordionItemView != null)
			{
				accordionItem._accordionItemView.HeaderBackground = (Brush)newValue;
			}
		}

		/// <summary>
		/// Handles the property changed method for the header icon color property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue"> The old value of header icon color property.</param>
		/// <param name="newValue">The new value of header icon color property. </param>
		static void OnHeaderIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is AccordionItem accordionItem && accordionItem._accordion != null && accordionItem._accordion.IsViewLoaded && accordionItem._accordionItemView != null)
			{
				accordionItem._accordionItemView.HeaderIconColor = (Color)newValue;
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

		#endregion
	}
}
