using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of footer view of the SfPicker.
    /// </summary>
    public class PickerFooterView : Element
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
                typeof(PickerFooterView),
                0d,
                propertyChanged: OnHeightChanged);

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
                typeof(PickerFooterView),
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
                typeof(PickerFooterView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        /// <summary>
        /// Identifies the <see cref="OkButtonText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OkButtonText"/> dependency property.
        /// </value>
        public static readonly BindableProperty OkButtonTextProperty =
            BindableProperty.Create(
                nameof(OkButtonText),
                typeof(string),
                typeof(PickerFooterView),
                "OK",
                propertyChanged: OnOkButtonTextChanged);

        /// <summary>
        /// Identifies the <see cref="CancelButtonText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CancelButtonText"/> dependency property.
        /// </value>
        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(
                nameof(CancelButtonText),
                typeof(string),
                typeof(PickerFooterView),
                "Cancel",
                propertyChanged: OnCancelButtonTextChanged);

        /// <summary>
        /// Identifies the <see cref="ShowOkButton"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowOkButton"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowOkButtonProperty =
            BindableProperty.Create(
                nameof(ShowOkButton),
                typeof(bool),
                typeof(PickerFooterView),
                true,
                propertyChanged: OnShowOkButtonChanged);

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
                typeof(PickerFooterView),
                defaultValueCreator: bindable => GetFooterTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of footer view on SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.Height"/> is 0d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the footer view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView Height="50" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
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
        /// Gets or sets the background of the footer view in SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.Background"/> is Brush.Transparent.</value>
        /// <example>
        /// The following example demonstrates how to set the background of the footer view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView Background="Blue" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
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
        /// Gets or sets the background of the footer separator line background in SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the footer view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView DividerColor="Gray" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
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
        /// Gets or sets the ok button text in the footer view of SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.OkButtonText"/> is "OK".</value>
        /// <example>
        /// The following example demonstrates how to set the ok button text of the footer view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView OkButtonText="Save" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
        /// {
        ///     OkButtonText = "Save"
        /// };
        /// </code>
        /// </example>
        public string OkButtonText
        {
            get { return (string)GetValue(OkButtonTextProperty); }
            set { SetValue(OkButtonTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cancel button text in the footer view of SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.CancelButtonText"/> is "Cancel".</value>
        /// <example>
        /// The following example demonstrates how to set the cancel button text of the footer view.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView CancelButtonText="Exit" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
        /// {
        ///     CancelButtonText = "Exit"
        /// };
        /// </code>
        /// </example>
        public string CancelButtonText
        {
            get { return (string)GetValue(CancelButtonTextProperty); }
            set { SetValue(CancelButtonTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the cancel button in the footer view of SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerFooterView.ShowOkButton"/> is true.</value>
        /// <example>
        /// The following example demonstrates how to hide the ok button in the footer view.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView ShowOkButton="False" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
        /// {
        ///     ShowOkButton = false
        /// };
        /// </code>
        /// </example>
        public bool ShowOkButton
        {
            get { return (bool)GetValue(ShowOkButtonProperty); }
            set { SetValue(ShowOkButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ok and cancel button text style in the footer view of SfPicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the footer view buttons.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView>
        ///             <picker:PickerFooterView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:PickerFooterView.TextStyle>
        ///         </picker:PickerFooterView>
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.FooterView = new PickerFooterView
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

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on the picker footer height changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on the picker footer background changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker footer separator line background changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes to get the default text style of the footer view.
        /// </summary>
        /// <returns>Returns the footer view text style.</returns>
        static ITextElement GetFooterTextStyle(BindableObject bindable)
        {
            var pickerFooterView = (PickerFooterView)bindable;
            PickerTextStyle pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#6750A4"),
                Parent = pickerFooterView,
            };

            return pickerTextStyle;
        }

        /// <summary>
        /// Method invokes on the picker footer ok button text changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnOkButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(OkButtonText));
        }

        /// <summary>
        /// Method invokes on the picker footer cancel button text changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnCancelButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(CancelButtonText));
        }

        /// <summary>
        /// Method invokes on the picker footer show ok button changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnShowOkButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(ShowOkButton));
        }

        /// <summary>
        /// Method invokes on the picker footer ok and cancel button text style changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerFooterView)?.RaisePropertyChanged(nameof(TextStyle));
        }

        /// <summary>
        /// Method invokes on the picker property changed event on footer settings properties changed.
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
        /// Event Invokes on footer settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        #endregion
    }
}