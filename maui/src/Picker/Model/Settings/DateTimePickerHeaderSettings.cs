using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of header view of the SfDateTimePicker.
    /// </summary>
    public class DateTimePickerHeaderView : Element, IThemeElement
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
                typeof(DateTimePickerHeaderView),
                50d,
                propertyChanged: OnHeightChanged);

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
                typeof(DateTimePickerHeaderView),
                defaultValueCreator: bindable => GetHeaderTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionTextStyleProperty =
            BindableProperty.Create(
                nameof(SelectionTextStyle),
                typeof(PickerTextStyle),
                typeof(DateTimePickerHeaderView),
                defaultValueCreator: bindable => GetHeaderSelectionTextStyle(bindable),
                propertyChanged: OnSelectionTextStyleChanged);

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
                typeof(DateTimePickerHeaderView),
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#F7F2FB")),
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
                typeof(DateTimePickerHeaderView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        /// <summary>
        /// Identifies the <see cref="DateFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DateFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty DateFormatProperty =
            BindableProperty.Create(
                nameof(DateFormat),
                typeof(string),
                typeof(DateTimePickerHeaderView),
                "dd/MM/yyyy",
                propertyChanged: OnHeaderDateFormatChanged);

        /// <summary>
        /// Identifies the <see cref="TimeFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TimeFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty TimeFormatProperty =
            BindableProperty.Create(
                nameof(TimeFormat),
                typeof(string),
                typeof(DateTimePickerHeaderView),
                "hh:mm:ss tt",
                propertyChanged: OnHeaderTimeFormatChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePickerHeaderView"/> class.
        /// </summary>
        public DateTimePickerHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfDateTimePickerTheme");
            SelectionTextStyle.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of header view on SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerHeaderView.Height"/> is 50d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the header view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView Height="60" />
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     Height = 60
        /// };
        /// </code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the header text in SfDateTimePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the header view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView>
        ///             <picker:DateTimePickerHeaderView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:DateTimePickerHeaderView.TextStyle>
        ///         </picker:DateTimePickerHeaderView>
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
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
        /// Gets or sets the selection text style of the header text in SfDateTimePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the selection text style of the header view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView>
        ///             <picker:DateTimePickerHeaderView.SelectionTextStyle>
        ///                 <picker:PickerTextStyle TextColor="Green" FontSize="18" />
        ///             </picker:DateTimePickerHeaderView.SelectionTextStyle>
        ///         </picker:DateTimePickerHeaderView>
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     SelectionTextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Green,
        ///         FontSize = 18
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerTextStyle SelectionTextStyle
        {
            get { return (PickerTextStyle)GetValue(SelectionTextStyleProperty); }
            set { SetValue(SelectionTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the header view in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerHeaderView.Background"/> is a "#F7F2FB".</value>
        /// <example>
        /// The following example demonstrates how to set the background of the header view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView Background="#E0E0E0" />
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     Background = new SolidColorBrush(Color.FromHex("#E0E0E0"))
        /// };
        /// </code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the header separator line in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerHeaderView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the header view.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView DividerColor="Gray" />
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
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

        /// <summary>
        /// Gets or sets the value to specify the date format of header view on SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerHeaderView.DateFormat"/> is "dd/MM/yyyy".</value>
        /// <example>
        /// The following example demonstrates how to set the date format of the header view.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView DateFormat="MM-dd-yyyy" />
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     DateFormat = "MM-dd-yyyy"
        /// };
        /// </code>
        /// </example>
        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the time format of header view on SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerHeaderView.TimeFormat"/> is "hh:mm:ss tt".</value>
        /// <example>
        /// The following example demonstrates how to set the time format of the header view.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.HeaderView>
        ///         <picker:DateTimePickerHeaderView TimeFormat="HH:mm" />
        ///     </picker:SfDateTimePicker.HeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     TimeFormat = "HH:mm"
        /// };
        /// </code>
        /// </example>
        public string TimeFormat
        {
            get { return (string)GetValue(TimeFormatProperty); }
            set { SetValue(TimeFormatProperty, value); }
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
            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on picker header text style property changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DateTimePickerHeaderView pickerHeaderView)
            {
                pickerHeaderView.SetParent(oldValue as Element, newValue as Element);
            }

            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on picker header selection text style property changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectionTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DateTimePickerHeaderView pickerHeaderView)
            {
                pickerHeaderView.SetParent(oldValue as Element, newValue as Element);
            }

            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(SelectionTextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker header background changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker header separator line background changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes on the picker header date format changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderDateFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(DateFormat));
        }

        /// <summary>
        /// Method invokes on the picker header time format changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTimeFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerHeaderView)?.RaisePropertyChanged(nameof(TimeFormat));
        }

        /// <summary>
        /// Method to get the default text style for the header view.
        /// </summary>
        /// <returns>Returns the default header text style.</returns>
        static ITextElement GetHeaderTextStyle(BindableObject bindable)
        {
            var pickerHeaderView = (DateTimePickerHeaderView)bindable;
            PickerTextStyle textStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#49454F"),
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
            var pickerHeaderView = (DateTimePickerHeaderView)bindable;
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
        /// Event Invokes on header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        #endregion
    }
}