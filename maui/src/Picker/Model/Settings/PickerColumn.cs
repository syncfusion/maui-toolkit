using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of picker column of the SfPicker.
    /// </summary>
    public class PickerColumn : Element, IThemeElement
    {
        #region Fields

        /// <summary>
        /// The column index holds the current column index value.
        /// </summary>
        internal int _columnIndex = -1;

        /// <summary>
        /// Determines whether to check the selected item while the selected index change.
        /// Set to true to selected item only change.
        /// </summary>
        internal bool _isSelectedItemChanged = true;

		/// <summary>
		/// Holds selected index on without default mode.
		/// </summary>
		internal int _internalSelectedIndex = -1;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Width"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Width"/> dependency property.
		/// </value>
		public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(
                nameof(Width),
                typeof(double),
                typeof(PickerColumn),
                -1d,
                propertyChanged: OnWidthChanged);

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ItemsSource"/> dependency property.
        /// </value>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(object),
                typeof(PickerColumn),
                defaultValueCreator: bindable => null,
                propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisplayMemberPath"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(PickerColumn),
                string.Empty,
                propertyChanged: OnDisplayMemberPathChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedIndex"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(PickerColumn),
                0,
                propertyChanged: OnSelectedIndexChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderTextProperty =
            BindableProperty.Create(
                nameof(HeaderText),
                typeof(string),
                typeof(PickerColumn),
                string.Empty,
                propertyChanged: OnHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedItem"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(PickerColumn),
                defaultValueCreator: bindable => PickerHelper.GetSelectedItemDefaultValue(bindable),
                propertyChanged: OnSelectedItemChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerColumn"/> class.
        /// </summary>
        public PickerColumn()
        {
            ThemeElement.InitializeThemeResources(this, "SfPickerTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the column width on SfPicker
        /// </summary>
        /// <value>The default value of <see cref="PickerColumn.Width"/> is -1d.</value>
        /// <example>
        /// The following example demonstrates how to set the width of the picker column.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn Width="100" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///     Width = 100
        /// };
        /// </code>
        /// </example>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the column text on SfPicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the items source of the picker column.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn ItemsSource="{Binding Items}" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///    ItemsSource = dataDource
        /// };
        /// </code>
        /// </example>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected index of the columns on SfPicker.
        /// </summary>
        /// <remarks>
        /// In multi-column setups, if any column contains a selected index as -1 or less than -1, the selection UI will not be drawn,
        /// and each column scrolling will move to the zeroth position.
        /// </remarks>
        /// <value>The default value of <see cref="PickerColumn.SelectedIndex"/> is 0.</value>
        /// <example>
        /// The following example demonstrates how to set the selected index of the picker column.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn SelectedIndex="2" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///     SelectedIndex = 2
        /// };
        /// </code>
        /// </example>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the header text of the columns on SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerColumn.HeaderText"/> is an string.Empty.</value>
        /// <example>
        /// The following example demonstrates how to set the header text of the picker column.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn HeaderText="Colors" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///     HeaderText = "Colors"
        /// };
        /// </code>
        /// </example>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the path value for items source.
        /// </summary>
        /// <value>The default value of <see cref="PickerColumn.DisplayMemberPath"/> is an string.empty.</value>
        /// <example>
        /// The following example demonstrates how to set the display member path of the picker column.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn DisplayMemberPath="Name" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///     DisplayMemberPath = "Name"
        /// };
        /// </code>
        /// </example>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item of the columns on SfPicker.
        /// </summary>
        /// <remarks>
        /// In multi-column setups, if any column contains a selected item as null, the selection UI will not be drawn,
        /// and each column scrolling will move to the zeroth position.
        /// When selected item changed its trigger selection index changed events.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the selected item of the picker column.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:PickerColumn SelectedItem="{Binding SelectedItem}" />
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// PickerColumn column = new PickerColumn
        /// {
        ///     SelectedItem = "Blue"
        /// };
        /// </code>
        /// </example>
        public object? SelectedItem
        {
            get { return (object?)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to wire the items source collection.
        /// </summary>
        internal void WireCollectionChanged()
        {
            if (ItemsSource == null)
            {
                return;
            }

            if (ItemsSource is INotifyCollectionChanged itemsSource)
            {
                itemsSource.CollectionChanged += OnItemsSourceCollectionChanged;
            }
        }

        /// <summary>
        /// Method to remove the wiring of items source collection.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        internal void UnWireCollectionChanged(object itemsSource)
        {
            if (itemsSource == null)
            {
                return;
            }

            if (itemsSource is INotifyCollectionChanged items)
            {
                items.CollectionChanged -= OnItemsSourceCollectionChanged;
            }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on the picker columns width changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(Width));
        }

        /// <summary>
        /// Method invokes on the picker columns text changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(ItemsSource));
        }

        /// <summary>
        /// Method invokes on the picker columns selected index changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(SelectedIndex), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker columns header text changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(HeaderText));
        }

        /// <summary>
        /// Method invokes on the picker columns display member path changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisplayMemberPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(DisplayMemberPath));
        }

        /// <summary>
        /// Method invokes on the picker columns selected item changed.
        /// </summary>
        /// <param name="bindable">The columns settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumn)?.RaisePropertyChanged(nameof(SelectedItem), oldValue);
        }

        /// <summary>
        /// Method to invoke picker property changed event on columns settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            PickerPropertyChanged?.Invoke(this, new PickerPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
        }

        /// <summary>
        /// Method to invoke items source collection changed.
        /// </summary>
        /// <param name="sender">The column items source object.</param>
        /// <param name="e">The collection changed event arguments.</param>
        void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PickerColumnCollectionChanged?.Invoke(this, e);
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

        #region Events

        /// <summary>
        /// Event Invokes on picker columns settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        /// <summary>
        /// Event Invokes on picker column collection changed.
        /// </summary>
        internal event EventHandler<NotifyCollectionChangedEventArgs>? PickerColumnCollectionChanged;

        #endregion
    }
}