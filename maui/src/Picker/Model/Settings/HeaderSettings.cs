using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of header view of the picker.
    /// </summary>
    public class PickerHeaderView : Element
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Height"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeightProperty =
            BindableProperty.Create(
                nameof(Height),
                typeof(double),
                typeof(PickerHeaderView),
                0d,
                propertyChanged: OnHeightChanged);

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Text"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(PickerHeaderView),
                string.Empty,
                propertyChanged: OnHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="TextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(
                nameof(TextStyle),
                typeof(PickerTextStyle),
                typeof(PickerHeaderView),
                defaultValueCreator: bindable => GetHeaderTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> dependency property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(
                nameof(Background),
                typeof(Brush),
                typeof(PickerHeaderView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="DividerColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DividerColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty DividerColorProperty =
            BindableProperty.Create(
                nameof(DividerColor),
                typeof(Color),
                typeof(PickerHeaderView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="DateText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DateText"/> dependency property.
        /// </value>
        internal static readonly BindableProperty DateTextProperty =
            BindableProperty.Create(
                nameof(DateText),
                typeof(string),
                typeof(PickerHeaderView),
                string.Empty,
                propertyChanged: OnHeaderDateTextChanged);

        /// <summary>
        /// Identifies the <see cref="TimeText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TimeText"/> dependency property.
        /// </value>
        internal static readonly BindableProperty TimeTextProperty =
            BindableProperty.Create(
                nameof(TimeText),
                typeof(string),
                typeof(PickerHeaderView),
                string.Empty,
                propertyChanged: OnHeaderTimeTextChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextStyle"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectionTextStyleProperty =
            BindableProperty.Create(
                nameof(SelectionTextStyle),
                typeof(PickerTextStyle),
                typeof(PickerHeaderView),
                defaultValueCreator: bindable => GetHeaderSelectionTextStyle(bindable),
                propertyChanged: OnSelectionTextStyleChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerHeaderView"/> class.
        /// </summary>
        public PickerHeaderView()
        {
            SelectionTextStyle.Parent = this;
            TextStyle.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of header view on picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerHeaderView.Height"/> is 0d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the header view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.HeaderView>
        ///         <picker:PickerHeaderView Height="50" />
        ///     </picker:SfPicker.HeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     Height = 50
        /// };
        /// </code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the text of header view on picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerHeaderView.Text"/> is string.Empty.</value>
        /// <example>
        /// The following example demonstrates how to set the text of the header view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.HeaderView>
        ///         <picker:PickerHeaderView Text="Select an item" />
        ///     </picker:SfPicker.HeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     Text = "Select an item"
        /// };
        /// </code>
        /// </example>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the header text in picker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the header view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.HeaderView>
        ///         <picker:PickerHeaderView>
        ///             <picker:PickerHeaderView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:PickerHeaderView.TextStyle>
        ///         </picker:PickerHeaderView>
        ///     </picker:SfPicker.HeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 16
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerTextStyle TextStyle
        {
            get { return (PickerTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the header view in picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerHeaderView.Background"/> is Brush.Transparent.</value>
        /// <example>
        /// The following example demonstrates how to set the background of the header view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.HeaderView>
        ///         <picker:PickerHeaderView Background="Blue" />
        ///     </picker:SfPicker.HeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     Background = Brush.Blue
        /// };
        /// </code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the header separator line in picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerHeaderView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the header view.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.HeaderView>
        ///         <picker:PickerHeaderView DividerColor="Gray" />
        ///     </picker:SfPicker.HeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.HeaderView = new PickerHeaderView
        /// {
        ///     DividerColor = Colors.Gray
        /// };
        /// </code>
        /// </example>
        public Color DividerColor
        {
            get { return (Color)GetValue(DividerColorProperty); }
            set { SetValue(DividerColorProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value to specify the date format of header view on Picker.
        /// </summary>
        internal string DateText
        {
            get { return (string)GetValue(DateTextProperty); }
            set { SetValue(DateTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the time format of header view on picker.
        /// </summary>
        internal string TimeText
        {
            get { return (string)GetValue(TimeTextProperty); }
            set { SetValue(TimeTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection text style of the header text in picker.
        /// </summary>
        internal PickerTextStyle SelectionTextStyle
        {
            get { return (PickerTextStyle)GetValue(SelectionTextStyleProperty); }
            set { SetValue(SelectionTextStyleProperty, value); }
        }

        #endregion

        #region Private Methods

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

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on the picker header height changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on the picker header text changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(Text));
        }

        /// <summary>
        /// Method invokes on picker header text style property changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker header background changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker header separator line background changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes on the picker header date format changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderDateTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(DateText));
        }

        /// <summary>
        /// Method invokes on the picker header time format changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTimeTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(TimeText));
        }

        /// <summary>
        /// Method invokes on picker header selection text style property changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectionTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PickerHeaderView pickerHeaderView)
            {
                pickerHeaderView.SetParent(oldValue as Element, newValue as Element);
            }

            (bindable as PickerHeaderView)?.RaisePropertyChanged(nameof(SelectionTextStyle), oldValue);
        }

        /// <summary>
        /// Method to get the default text style for the header view.
        /// </summary>
        /// <returns>Returns the default header text style.</returns>
        static ITextElement GetHeaderTextStyle(BindableObject bindable)
        {
            var pickerHeaderView = (PickerHeaderView)bindable;
            PickerTextStyle textStyle = new PickerTextStyle()
            {
                FontSize = 16,
                TextColor = Color.FromArgb("#1C1B1F"),
                Parent = pickerHeaderView,
            };

            return textStyle;
        }

        /// <summary>
        /// Method to get the default selection text style for the header view.
        /// </summary>
        /// <returns>Returns the default header selection text style.</returns>
        static ITextElement GetHeaderSelectionTextStyle(BindableObject bindable)
        {
            var pickerHeaderView = (PickerHeaderView)bindable;
            PickerTextStyle textStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#6750A4"),
                Parent = pickerHeaderView,
            };

            return textStyle;
        }

        /// <summary>
        /// Method to invoke picker property changed event on header settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            PickerPropertyChanged?.Invoke(this, new PickerPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
        }

        #endregion

        #region Events

        /// <summary>
        /// Event Invokes on header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        #endregion
    }
}