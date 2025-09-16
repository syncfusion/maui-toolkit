using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfPicker"/> class that represents a control, that allows you pick an item among a list of items.
    /// </summary>
    public class SfPicker : PickerBase, IParentThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="HeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(PickerHeaderView),
                typeof(SfPicker),
                defaultValueCreator: bindable => new PickerHeaderView(),
                propertyChanged: OnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="ColumnHeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ColumnHeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty ColumnHeaderViewProperty =
           BindableProperty.Create(
               nameof(ColumnHeaderView),
               typeof(PickerColumnHeaderView),
               typeof(SfPicker),
               defaultValueCreator: bindable => new PickerColumnHeaderView(),
               propertyChanged: OnColumnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="Columns"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Columns"/> dependency property.
        /// </value>
        public static readonly BindableProperty ColumnsProperty =
            BindableProperty.Create(
                nameof(Columns),
                typeof(ObservableCollection<PickerColumn>),
                typeof(SfPicker),
                defaultValueCreator: bindable => new ObservableCollection<PickerColumn>(),
                propertyChanged: OnColumnsChanged);

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ItemTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(SfPicker),
                null,
                propertyChanged: OnItemTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionChangedCommand"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionChangedCommand"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionChangedCommandProperty =
            BindableProperty.Create(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(SfPicker),
                null);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="PickerBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PickerBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty PickerBackgroundProperty =
            BindableProperty.Create(
                nameof(PickerBackground),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#EEE8F4"),
                propertyChanged: OnPickerBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HeaderTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#49454F"),
                propertyChanged: OnHeaderTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(SfPicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnHeaderFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="FooterTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty FooterTextColorProperty =
            BindableProperty.Create(
                nameof(FooterTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#6750A4"),
                propertyChanged: OnFooterTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="FooterFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty FooterFontSizeProperty =
            BindableProperty.Create(
                nameof(FooterFontSize),
                typeof(double),
                typeof(SfPicker),
                defaultValueCreator: bindable => 14d,
                propertyChanged: OnFooterFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(
                nameof(SelectedTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Colors.White,
                propertyChanged: OnSelectedTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectionTextColorProperty =
            BindableProperty.Create(
                nameof(SelectionTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#6750A4"),
                propertyChanged: OnSelectedTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectedFontSizeProperty =
            BindableProperty.Create(
                nameof(SelectedFontSize),
                typeof(double),
                typeof(SfPicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnSelectedFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalTextColorProperty =
            BindableProperty.Create(
                nameof(NormalTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalFontSize),
                typeof(double),
                typeof(SfPicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalFontSizeChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfPicker"/> class.
        /// </summary>
        public SfPicker()
        {
            Initialize();
            Focus();
            ColumnHeaderView.Parent = this;
            BackgroundColor = PickerBackground;
            InitializePickerStyle();
            Dispatcher.Dispatch(() =>
            {
                InitializeTheme();
            });
        }

        #endregion

        #region fields

        /// <summary>
        /// If the selected item null or selected index has -1 then user click the cancel button.
        /// </summary>
        bool _showSelectionOnNullItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of header view. This property can be used to customize the header in SfPicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to customize the header view of SfPicker.
        /// <code>
        /// <![CDATA[
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     Text = "Select a Color",
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 18,
        ///         FontAttributes = FontAttributes.Bold
        ///     },
        ///     Background = new SolidColorBrush(Colors.LightGray)
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public PickerHeaderView HeaderView
        {
            get { return (PickerHeaderView)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of column header view. This property can be used to customize the column header in SfPicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to customize the column header view of SfPicker.
        /// <code>
        /// <![CDATA[
        /// SfPicker picker = new SfPicker();
        /// picker.ColumnHeaderView = new PickerColumnHeaderView
        /// {
        ///     Background = new SolidColorBrush(Colors.LightBlue),
        ///     Height = 40,
        ///     DividerColor = Colors.Gray,
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.DarkBlue,
        ///         FontSize = 16
        ///     },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public PickerColumnHeaderView ColumnHeaderView
        {
            get { return (PickerColumnHeaderView)GetValue(ColumnHeaderViewProperty); }
            set { SetValue(ColumnHeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of picker columns. This property can be used to customize the column in SfPicker.
        /// </summary>
        /// <example>
        /// The following examples demonstrate how to set the columns in SfPicker.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfPicker>
        ///     <Picker:SfPicker.Columns>
        ///         <Picker:PickerColumn ItemsSource="{Binding Colors}" />
        ///         <Picker:PickerColumn ItemsSource="{Binding Value}" />
        ///     </Picker:SfPicker.Columns>
        /// </Picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// <![CDATA[
        /// SfPicker picker = new SfPicker();
        /// picker.Columns = new ObservableCollection<PickerColumn>
        /// {
        ///     new PickerColumn { ItemsSource = new List<string> { "Red", "Green", "Blue" } },
        ///     new PickerColumn { ItemsSource = new List<int> { 1, 2, 3 } }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public ObservableCollection<PickerColumn> Columns
        {
            get { return (ObservableCollection<PickerColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker item template in SfPicker.
        /// </summary>
        /// <example>
        /// The following examples demonstrate how to set the item template in SfPicker.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Grid>
        /// <Grid.Resources>
        ///    <DataTemplate x:Key="customView">
        ///        <Grid>
        ///            <Label HorizontalTextAlignment="Center" VerticalTextAlignment="Center" TextColor="Red" Text="{Binding Data}"/>    
        ///        </Grid>
        ///    </DataTemplate>
        /// </Grid.Resources>
        /// <Picker:SfPicker x:Name="picker"
        ///                  ItemTemplate="{StaticResource customView}">
        /// </Picker:SfPicker>
        /// </Grid>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// DataTemplate customView = new DataTemplate(() =>
        /// {
        ///    Grid grid = new Grid
        ///    {
        ///        Padding = new Thickness(0, 1, 0, 1),
        ///    };
        ///    Label label = new Label
        ///    {
        ///        HorizontalOptions = LayoutOptions.Center,
        ///        VerticalOptions = LayoutOptions.Center,
        ///        HorizontalTextAlignment = TextAlignment.Center,
        ///        VerticalTextAlignment = TextAlignment.Center,
        ///        TextColor = Colors.Black,
        ///    };
        ///    label.SetBinding(Label.TextProperty, new Binding("Data"));
        ///    grid.Children.Add(label);
        ///    return grid;
        /// });
        /// picker.ItemTemplate = customView;
        /// </code>
        /// </example>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker item selection changed command.
        /// </summary>
        /// <value>The default value of <see cref="SfPicker.SelectionChangedCommand"/> is null.</value>
        /// <example>
        /// The following example demonstrates how to set the selection changed command in SfPicker.
        /// # [XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        /// <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        /// </ContentPage.BindingContext>
        /// <Picker:SfPicker x:Name="Picker"
        ///                      SelectionChangedCommand="{Binding SelectionCommand}">
        /// </Picker:SfPicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command selectionCommand;
        ///    public ICommand SelectionCommand {
        ///        get
        ///        {
        ///            return selectionCommand;
        ///        }
        ///        set
        ///        {
        ///            if (selectionCommand != value)
        ///            {
        ///                selectionCommand = value;
        ///                OnPropertyChanged(nameof(SelectionCommand));
        ///            }
        ///        }
        ///    }
        ///    public ViewModel()
        ///    {
        ///      SelectionCommand = new Command(SelectionChanged);
        ///    }
        ///    private void SelectionChanged()
        ///    {
        ///    }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the background color of the picker.
        /// </summary>
        internal Color PickerBackground
        {
            get { return (Color)GetValue(PickerBackgroundProperty); }
            set { SetValue(PickerBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header text color of the text style.
        /// </summary>
        internal Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header font size of the text style.
        /// </summary>
        internal double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the footer text color of the text style.
        /// </summary>
        internal Color FooterTextColor
        {
            get { return (Color)GetValue(FooterTextColorProperty); }
            set { SetValue(FooterTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the footer font size of the text style.
        /// </summary>
        internal double FooterFontSize
        {
            get { return (double)GetValue(FooterFontSizeProperty); }
            set { SetValue(FooterFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection text color of the text style.
        /// </summary>
        /// <remarks>
        /// This color applicable for default text display mode.
        /// </remarks>
        internal Color SelectedTextColor
        {
            get { return (Color)GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection text color of the text style/
        /// </summary>
        /// <remarks>
        /// This color is used for Fade, Shrink and FadeAndShrink mode.
        /// </remarks>
        internal Color SelectionTextColor
        {
            get { return (Color)GetValue(SelectionTextColorProperty); }
            set { SetValue(SelectionTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection font size of the text style.
        /// </summary>
        internal double SelectedFontSize
        {
            get { return (double)GetValue(SelectedFontSizeProperty); }
            set { SetValue(SelectedFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal text color of the text style.
        /// </summary>
        internal Color NormalTextColor
        {
            get { return (Color)GetValue(NormalTextColorProperty); }
            set { SetValue(NormalTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal font size of the text style.
        /// </summary>
        internal double NormalFontSize
        {
            get { return (double)GetValue(NormalFontSizeProperty); }
            set { SetValue(NormalFontSizeProperty, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method trigged whenever the base panel selection is changed.
        /// </summary>
        /// <param name="sender">Base picker instance value.</param>
        /// <param name="e">Selection changed event arguments.</param>
        void OnPickerSelectionIndexChanged(object? sender, PickerSelectionChangedEventArgs e)
        {
            // If the picker is not in Default mode
            if (IsScrollSelectionAllowed() && BaseColumns.Count == 1)
            {
                // If there was no previously selected item (OldValue is -1),
                // set a flag to indicate that selection started from a null state
                if ((int)e.OldValue == -1)
                {
                    _showSelectionOnNullItem = true;
                    return;
                }
                // If the new selection is invalid (NewValue is -1), do nothing
                if ((int)e.NewValue == -1)
                {
                    return;
                }
            }

            SelectionChanged?.Invoke(this, e);
            if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(e))
            {
                SelectionChangedCommand.Execute(e);
            }
        }

        /// <summary>
        /// Need to update the parent for the new value.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        void SetParent(Element? oldValue, Element? newValue)
        {
            if (oldValue != null)
            {
                oldValue.Parent = null;
            }

            if (newValue != null)
            {
                newValue.Parent = this;
            }
        }

        /// <summary>
        /// Method to initialize the theme and to set dynamic resources.
        /// </summary>
        void InitializeTheme()
        {
            ThemeElement.InitializeThemeResources(this, "SfPickerTheme");

            this.SetDynamicResource(HeaderTextColorProperty, "SfPickerNormalHeaderTextColor");
            this.SetDynamicResource(HeaderFontSizeProperty, "SfPickerNormalHeaderFontSize");

            this.SetDynamicResource(FooterTextColorProperty, "SfPickerNormalFooterTextColor");
            this.SetDynamicResource(FooterFontSizeProperty, "SfPickerNormalFooterFontSize");
        }

        /// <summary>
        /// Method to initialize the defult picker style.
        /// </summary>
        void InitializePickerStyle()
        {
            SetDynamicResource(PickerBackgroundProperty, "SfPickerNormalBackground");

            SetDynamicResource(SelectedTextColorProperty, "SfPickerSelectedTextColor");
            SetDynamicResource(SelectedFontSizeProperty, "SfPickerSelectedFontSize");
            SetDynamicResource(SelectionTextColorProperty, "SfPickerSelectionTextColor");

            SetDynamicResource(NormalTextColorProperty, "SfPickerNormalTextColor");
            SetDynamicResource(NormalFontSizeProperty, "SfPickerNormalFontSize");
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to wire the events.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            BaseColumnHeaderView = ColumnHeaderView;
            BaseColumns = Columns;
            BaseHeaderView = HeaderView;
            BaseItemTemplate = ItemTemplate;
            SelectionIndexChanged += OnPickerSelectionIndexChanged;
        }

        /// <summary>
        /// Method triggers when property binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (HeaderView != null)
            {
                SetInheritedBindingContext(HeaderView, BindingContext);
                if (HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(HeaderView.TextStyle, BindingContext);
                }

                if (HeaderView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(HeaderView.SelectionTextStyle, BindingContext);
                }
            }

            if (ColumnHeaderView != null)
            {
                SetInheritedBindingContext(ColumnHeaderView, BindingContext);
                if (ColumnHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(ColumnHeaderView.TextStyle, BindingContext);
                }
            }

            if (FooterView != null)
            {
                SetInheritedBindingContext(FooterView, BindingContext);
                if (FooterView.TextStyle != null)
                {
                    SetInheritedBindingContext(FooterView.TextStyle, BindingContext);
                }
            }

            if (Columns != null)
            {
                for (int index = 0; index < Columns.Count; index++)
                {
                    PickerColumn column = Columns[index];
                    column._columnIndex = index;
                    SetInheritedBindingContext(column, BindingContext);
                }
            }

            if (SelectedTextStyle != null)
            {
                SetInheritedBindingContext(SelectedTextStyle, BindingContext);
            }

            if (TextStyle != null)
            {
                SetInheritedBindingContext(TextStyle, BindingContext);
            }

            if (SelectionView != null)
            {
                SetInheritedBindingContext(SelectionView, BindingContext);
            }
        }

        /// <summary>
        /// Method triggers when the picker popup closed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosed(EventArgs e)
        {
            if (IsScrollSelectionAllowed() && BaseColumns.Count == 1)
            {
                if (_showSelectionOnNullItem)
                {
                    foreach (var column in this.Columns)
                    {
                        column.SelectedItem = null;
                        _showSelectionOnNullItem = false;
                    }
                }
            }

            InvokeClosedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the picker popup closing.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosing(CancelEventArgs e)
        {
            InvokeClosingEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the picker popup opened.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupOpened(EventArgs e)
        {
            InvokeOpenedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the picker ok button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnOkButtonClicked(EventArgs e)
        {
            // If the picker is not in Default mode
            if (IsScrollSelectionAllowed() && BaseColumns.Count == 1)
            {
                // Iterate through each column in the picker
                foreach (PickerColumn column in this.Columns)
                {
                    // Skip columns that have no internal selection
                    if (column._internalSelectedIndex == -1)
                    {
                        continue;
                    }
                    // If the column has a valid item source and it's a list
                    if (column.ItemsSource != null && column.ItemsSource is IList pickerCollection)
                    {
                        // Check if the selected index is within the valid range of the collection
                        if (column.SelectedIndex >= 0 && column.SelectedIndex < pickerCollection.Count)
                        {
                            // Apply the internal selected index to the actual selected index
                            int selectedIndex = column._internalSelectedIndex;
                            column._internalSelectedIndex = -1;
                            // Apply selection
                            column.SelectedIndex = selectedIndex;
                        }
                    }
                }
            }

            InvokeOkButtonClickedEvent(this, e);
            if (AcceptCommand != null && AcceptCommand.CanExecute(e))
            {
                AcceptCommand.Execute(e);
            }
        }

        /// <summary>
        /// Method triggers when the picker cancel button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnCancelButtonClicked(EventArgs e)
        {
            // If the picker is not in Default mode
            if (IsScrollSelectionAllowed() && BaseColumns.Count == 1)
            {
                // Get the picker container instance to perform scroll operations
                PickerContainer? pickerContainer = this.GetPickerContainerValue();
                // Iterate through each column in the picker
                foreach (PickerColumn column in this.Columns)
                {
                    // Check if the column has a valid item source and it's a list
                    if (column.ItemsSource != null && column.ItemsSource is IList pickerCollection)
                    {
                        // Ensure the selected index is within the valid range of the collection
                        if (column.SelectedIndex >= 0 && column.SelectedIndex < pickerCollection.Count)
                        {
                            // If selection started from a null item, clear the selection
                            if (this._showSelectionOnNullItem)
                            {
                                column.SelectedItem = null;
                                this._showSelectionOnNullItem = false;
                            }
                            else
                            {
                                // Otherwise, scroll to the currently selected index to restore selection
                                pickerContainer?.ScrollToSelectedIndex(column._columnIndex, column.SelectedIndex);
                            }
                        }
                    }
                }
            }

            InvokeCancelButtonClickedEvent(this, e);
            if (DeclineCommand != null && DeclineCommand.CanExecute(e))
            {
                DeclineCommand.Execute(e);
            }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on picker header view changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.Parent = picker;
            picker.BaseHeaderView = picker.HeaderView;
            if (bindable is SfPicker pickerView)
            {
                pickerView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Method invokes on picker column header view changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnColumnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.BaseColumnHeaderView = picker.ColumnHeaderView;
        }

        /// <summary>
        /// Method invokes on picker columns changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.BaseColumns = picker.Columns;
        }

        /// <summary>
        /// Method invokes on the picker item template changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.BaseItemTemplate = picker.ItemTemplate;
        }

        #endregion

        #region Internal Property Changed Methods

        /// <summary>
        /// called when <see cref="PickerBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnPickerBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.BackgroundColor = picker.PickerBackground;
        }

        /// <summary>
        /// Method invokes on the picker header text color changed.
        /// </summary>
        /// <param name="bindable">The header text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.TextStyle.TextColor = picker.HeaderTextColor;
        }

        /// <summary>
        /// Method invokes on the picker header font size changed.
        /// </summary>
        /// <param name="bindable">The header text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.TextStyle.FontSize = picker.HeaderFontSize;
        }

        /// <summary>
        /// Method invokes on the picker footer text color changed.
        /// </summary>
        /// <param name="bindable">The footer text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.FooterView.TextStyle.TextColor = picker.FooterTextColor;
        }

        /// <summary>
        /// Method invokes on the picker footer font size changed.
        /// </summary>
        /// <param name="bindable">The footer text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.FooterView.TextStyle.FontSize = picker.FooterFontSize;
        }

        /// <summary>
        /// Method invokes on the picker selection text color changed.
        /// </summary>
        /// <param name="bindable">The selection text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.SelectedTextStyle.TextColor = picker.TextDisplayMode == PickerTextDisplayMode.Default ? picker.SelectedTextColor : picker.SelectionTextColor;
        }

        /// <summary>
        /// Method invokes on the picker selection font size changed.
        /// </summary>
        /// <param name="bindable">The selection text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.SelectedTextStyle.FontSize = picker.SelectedFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnNormalTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.TextStyle.TextColor = picker.NormalTextColor;
        }

        /// <summary>
        /// Method invokes on the picker normal font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnNormalFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfPicker? picker = bindable as SfPicker;
            if (picker == null)
            {
                return;
            }

            picker.TextStyle.FontSize = picker.NormalFontSize;
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method is declared only in IParentThemeElement
        /// and you need to implement this method only in main control.
        /// </summary>
        /// <returns>ResourceDictionary</returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfPickerStyles();
        }

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

        #region Events

        /// <summary>
        /// Occurs after the selected index changed on SfPicker.
        /// </summary>
        public event EventHandler<PickerSelectionChangedEventArgs>? SelectionChanged;

        #endregion
    }
}