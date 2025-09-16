using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Class that represents and render the picker control,
    /// </summary>
    public abstract partial class PickerBase
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="FooterView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterView"/> dependency property.
        /// </value>
        public static readonly BindableProperty FooterViewProperty =
            BindableProperty.Create(
                nameof(FooterView),
                typeof(PickerFooterView),
                typeof(PickerBase),
                defaultValueCreator: bindable => new PickerFooterView(),
                propertyChanged: OnFooterViewChanged);

        /// <summary>
        /// Identifies the <see cref="ColumnDividerColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ColumnDividerColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty ColumnDividerColorProperty =
            BindableProperty.Create(
                nameof(ColumnDividerColor),
                typeof(Color),
                typeof(PickerBase),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnColumnDividerColorChanged);

        /// <summary>
        /// Identifies the <see cref="ItemHeight"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ItemHeight"/> dependency property.
        /// </value>
        public static readonly BindableProperty ItemHeightProperty =
            BindableProperty.Create(
                nameof(ItemHeight),
                typeof(double),
                typeof(PickerBase),
                40d,
                propertyChanged: OnItemHeightChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedTextStyleProperty =
            BindableProperty.Create(
                nameof(SelectedTextStyle),
                typeof(PickerTextStyle),
                typeof(PickerBase),
                defaultValueCreator: bindable => GetPickerSelectionTextStyle(bindable),
                propertyChanged: OnSelectedTextStyleChanged);

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
                typeof(PickerBase),
                defaultValueCreator: bindable => GetPickerTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionView"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionViewProperty =
            BindableProperty.Create(
                nameof(SelectionView),
                typeof(PickerSelectionView),
                typeof(PickerBase),
                defaultValueCreator: bindable => new PickerSelectionView(),
                propertyChanged: OnSelectionViewChanged);

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsOpen"/> dependency property.
        /// </value>
        public static readonly BindableProperty IsOpenProperty =
            BindableProperty.Create(
                nameof(IsOpen),
                typeof(bool),
                typeof(PickerBase),
                false,
                propertyChanged: OnIsOpenChanged);

        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Mode"/> dependency property.
        /// </value>
        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(
                nameof(Mode),
                typeof(PickerMode),
                typeof(PickerBase),
                PickerMode.Default,
                propertyChanged: OnModeChanged);

        /// <summary>
        /// Identifies the <see cref="TextDisplayMode"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextDisplayMode"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextDisplayModeProperty =
            BindableProperty.Create(
                nameof(TextDisplayMode),
                typeof(PickerTextDisplayMode),
                typeof(PickerBase),
                PickerTextDisplayMode.Default,
                propertyChanged: OnTextDisplayModeChanged);

        /// <summary>
        /// Identifies the <see cref="RelativePosition"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RelativePosition"/> dependency property.
        /// </value>
        public static readonly BindableProperty RelativePositionProperty =
            BindableProperty.Create(
                nameof(RelativePosition),
                typeof(PickerRelativePosition),
                typeof(PickerBase),
                PickerRelativePosition.AlignTop,
                propertyChanged: OnRelativePositionChanged);

        /// <summary>
        /// Identifies the <see cref="RelativeView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RelativeView"/> dependency property.
        /// </value>
        public static readonly BindableProperty RelativeViewProperty =
            BindableProperty.Create(
                nameof(RelativeView),
                typeof(View),
                typeof(PickerBase),
                null,
                propertyChanged: OnRelativeViewChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderTemplateProperty =
            BindableProperty.Create(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(PickerBase),
                null,
                propertyChanged: OnHeaderTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="ColumnHeaderTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ColumnHeaderTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty ColumnHeaderTemplateProperty =
            BindableProperty.Create(
                nameof(ColumnHeaderTemplate),
                typeof(DataTemplate),
                typeof(PickerBase),
                null,
                propertyChanged: OnColumnHeaderTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="FooterTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty FooterTemplateProperty =
            BindableProperty.Create(
                nameof(FooterTemplate),
                typeof(DataTemplate),
                typeof(PickerBase),
                null,
                propertyChanged: OnFooterTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="AcceptCommand"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AcceptCommand"/> dependency property.
        /// </value>
        public static readonly BindableProperty AcceptCommandProperty =
            BindableProperty.Create(
                nameof(AcceptCommand),
                typeof(ICommand),
                typeof(PickerBase),
                null);

        /// <summary>
        /// Identifies the <see cref="DeclineCommand"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DeclineCommand"/> dependency property.
        /// </value>
        public static readonly BindableProperty DeclineCommandProperty =
            BindableProperty.Create(
                nameof(DeclineCommand),
                typeof(ICommand),
                typeof(PickerBase),
                null);

        /// <summary>
        /// Identifies the <see cref="EnableLoopingProperty"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EnableLoopingProperty"/> dependency property.
        /// </value>
        public static readonly BindableProperty EnableLoopingProperty =
            BindableProperty.Create(
                nameof(EnableLooping),
                typeof(bool),
                typeof(PickerBase),
                false,
                propertyChanged: OnEnableLoopingChanged);

        /// <summary>
        /// Identifies the <see cref="PopupWidthProperty"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupWidthProperty"/> dependency property.
        /// </value>
        public static readonly BindableProperty PopupWidthProperty =
            BindableProperty.Create(
                nameof(PopupWidth),
                typeof(double),
                typeof(PickerBase),
                -1.0,
                propertyChanged: OnPopupWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PopupHeightProperty"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupHeightProperty"/> dependency property.
        /// </value>
        public static readonly BindableProperty PopupHeightProperty =
            BindableProperty.Create(
                nameof(PopupHeight),
                typeof(double),
                typeof(PickerBase),
                -1.0,
                propertyChanged: OnPopupHeightPropertyChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="BaseHeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BaseHeaderView"/> dependency property.
        /// </value>
        internal static readonly BindableProperty BaseHeaderViewProperty =
            BindableProperty.Create(
                nameof(BaseHeaderView),
                typeof(PickerHeaderView),
                typeof(PickerBase),
                defaultValueCreator: bindable => new PickerHeaderView(),
                propertyChanged: OnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="BaseColumnHeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BaseColumnHeaderView"/> dependency property.
        /// </value>
        internal static readonly BindableProperty BaseColumnHeaderViewProperty =
            BindableProperty.Create(
                nameof(BaseColumnHeaderView),
                typeof(PickerColumnHeaderView),
                typeof(PickerBase),
                defaultValueCreator: bindable => new PickerColumnHeaderView(),
                propertyChanged: OnColumnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="BaseColumns"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BaseColumns"/> dependency property.
        /// </value>
        internal static readonly BindableProperty BaseColumnsProperty =
            BindableProperty.Create(
                nameof(BaseColumns),
                typeof(ObservableCollection<PickerColumn>),
                typeof(PickerBase),
                defaultValueCreator: bindable => new ObservableCollection<PickerColumn>(),
                propertyChanged: OnColumnsChanged);

        /// <summary>
        /// Identifies the <see cref="BaseItemTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BaseItemTemplate"/> dependency property.
        /// </value>
        internal static readonly BindableProperty BaseItemTemplateProperty =
            BindableProperty.Create(
                nameof(BaseItemTemplate),
                typeof(DataTemplate),
                typeof(PickerBase),
                null,
                propertyChanged: OnItemTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledTextStyle"/> dependency property.
        /// </value>
        internal static readonly BindableProperty DisabledTextStyleProperty =
            BindableProperty.Create(
                nameof(DisabledTextStyle),
                typeof(PickerTextStyle),
                typeof(PickerBase),
                defaultValueCreator: bindable => GetPickerDisabledTextStyle(bindable),
                propertyChanged: OnDisabledTextStyleChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the item height of picker view on Picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.ItemHeight"/> is 40d.</value>
        /// <example>
        /// The following example demonstrates how to set the item height of the picker view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker ItemHeight="50" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     ItemHeight = 50
        /// };
        /// </code>
        /// </example>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker selected text style in Picker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the selected text style of the picker.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.SelectedTextStyle>
        ///         <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///     </picker:SfPicker.SelectedTextStyle>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     SelectedTextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 16
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerTextStyle SelectedTextStyle
        {
            get { return (PickerTextStyle)GetValue(SelectedTextStyleProperty); }
            set { SetValue(SelectedTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker item text style in Picker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the picker items.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.TextStyle>
        ///         <picker:PickerTextStyle TextColor="Gray" FontSize="14" />
        ///     </picker:SfPicker.TextStyle>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Gray,
        ///         FontSize = 14
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
        /// Gets or sets the value of selection view. This property can be used to customize the selection in Picker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the selection view of the picker.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.SelectionView>
        ///         <picker:PickerSelectionView BackgroundColor="LightBlue" />
        ///     </picker:SfPicker.SelectionView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     SelectionView = new PickerSelectionView
        ///     {
        ///         BackgroundColor = Colors.LightBlue
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerSelectionView SelectionView
        {
            get { return (PickerSelectionView)GetValue(SelectionViewProperty); }
            set { SetValue(SelectionViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of footer view. This property can be used to customize the Footer in Picker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the footer view of the picker.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.FooterView>
        ///         <picker:PickerFooterView OkButtonText="Save" CancelButtonText="Exit" />
        ///     </picker:SfPicker.FooterView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     FooterView = new PickerFooterView
        ///     {
        ///         OkButtonText = "Save",
        ///         CancelButtonText = "Exit"
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerFooterView FooterView
        {
            get { return (PickerFooterView)GetValue(FooterViewProperty); }
            set { SetValue(FooterViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of column divider color in SfPicker. This property can be used to customize the column divider color in Picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.ColumnDividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the column divider color of the picker.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker ColumnDividerColor="Gray" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     ColumnDividerColor = Colors.Gray
        /// };
        /// </code>
        /// </example>
        public Color ColumnDividerColor
        {
            get { return (Color)GetValue(ColumnDividerColorProperty); }
            set { SetValue(ColumnDividerColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the picker is open or not.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.IsOpen"/> is "False".</value>
        /// <remarks>
        /// It will be applicable to set the <see cref="PickerMode.Dialog"/> or <see cref="PickerMode.RelativeDialog"/>.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the IsOpen property of the picker.
        /// # [XAML](#tab/tabid-13)
        /// <code lang="XAML">
        /// <![CDATA[
        /// <?xml version = "1.0" encoding="utf-8" ?>
        /// <ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
        ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        ///     xmlns:syncfusion="clr-namespace:Syncfusion.Maui.Toolkit.Picker;assembly=Syncfusion.Maui.Toolkit"
        ///     xmlns:local="clr-namespace:PickerMAUI"
        ///     x:Class="PickerMAUI.MainPage">
        /// <ContentPage.Content>
        ///    <StackLayout WidthRequest = "500" >
        ///        <syncfusion:SfPicker x:Name="Picker" Mode="Dialog"/>
        ///        <Button x:Name="clickToShowPicker" Text="Click To Show Picker" Clicked="clickToShowPopup_Clicked"/>
        ///    </StackLayout>
        /// </ContentPage.Content>
        /// </ContentPage>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code lang="C#">
        /// <![CDATA[
        /// using System.ComponentModel;
        ///
        /// namespace PickerMAUI
        /// {
        ///     public partial class MainPage : ContentPage
        ///     {
        ///         public MainPage()
        ///         {
        ///            InitializeComponent();
        ///         }
        ///         void clickToShowPopup_Clicked(object sender, EventArgs e)
        ///         {
        ///             Picker.IsOpen = true;
        ///         }
        ///      }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode of the picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.Mode"/> is <see cref="PickerMode.Default"/>.</value>
        /// <remarks>
        /// The <see cref="PickerMode.Dialog"/> and <see cref="PickerMode.RelativeDialog"/> only visible to set the <see cref="PickerBase.IsOpen"/> is "True".
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the Mode property in the picker.
        /// # [XAML](#tab/tabid-15)
        /// <code Lang="XAML"><![CDATA[
        /// <picker:SfPicker x:Name="Picker"
        ///                  Mode="Dialog">
        /// </picker:SfPicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-16)
        /// <code Lang="C#"><![CDATA[
        /// Picker.IsOpen = True;
        /// ]]></code>
        /// </example>
        public PickerMode Mode
        {
            get { return (PickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text display mode of the picker.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.TextDisplayMode"/> is <see cref="PickerTextDisplayMode.Default"/>.</value>
        /// <example>
        /// The following example demonstrates how to set the TextDisplayMode property of the picker.
        /// # [XAML](#tab/tabid-17)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker TextDisplayMode="Fade" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker
        /// {
        ///     TextDisplayMode = PickerTextDisplayMode.Fade
        /// };
        /// </code>
        /// </example>
        public PickerTextDisplayMode TextDisplayMode
        {
            get { return (PickerTextDisplayMode)GetValue(TextDisplayModeProperty); }
            set { SetValue(TextDisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the relative position of the picker popup.
        /// </summary>
        /// <value>The default value of <see cref="PickerBase.RelativePosition"/> is <see cref="PickerRelativePosition.AlignTop"/>.</value>
        /// <remarks>
        /// It will be applicable to set <see cref="PickerBase.Mode"/> is <see cref="PickerMode.RelativeDialog"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the RelativePosition property in the picker.
        /// # [XAML](#tab/tabid-19)
        /// <code Lang="XAML"><![CDATA[
        /// <picker:SfPicker x:Name="Picker"
        ///                  Mode="RelativeDialog" RelativePosition="AlignBottom">
        /// </picker:SfPicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-20)
        /// <code Lang="C#"><![CDATA[
        /// Picker.IsOpen = True;
        /// ]]></code>
        /// </example>
        public PickerRelativePosition RelativePosition
        {
            get { return (PickerRelativePosition)GetValue(RelativePositionProperty); }
            set { SetValue(RelativePositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view relative to which the picker dialog should be displayed based on the RelativePosition.
        /// <seealso cref="PickerBase.RelativePosition"/>
        /// </summary>
        /// <remarks>
        /// It is only applicable for RelativeDialog mode. If no relative view is given, the picker base will be set as the relative view.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the RelativeView property in the picker.
        /// # [XAML](#tab/tabid-21)
        /// <code Lang="XAML"><![CDATA[
        /// <Grid WidthRequest="500">
        ///     <picker:SfDatePicker x:Name="picker"
        ///                         Mode="RelativeDialog"
        ///                         RelativePosition="AlignToRightOf"
        ///                         RelativeView="{x:Reference pickerButton}">
        ///     </picker:SfDatePicker>
        ///     <Button Text="Open Picker"
        ///             x:Name="pickerButton"
        ///             Clicked="Button_Clicked"
        ///             HorizontalOptions="Center"
        ///             VerticalOptions="Center"
        ///             HeightRequest="50"
        ///             WidthRequest="150">
        ///     </Button>
        /// </Grid>
        /// ]]></code>
        /// # [C#](#tab/tabid-22)
        /// <code Lang="C#"><![CDATA[
        /// private void Button_Clicked(object sender, System.EventArgs e)
        /// {
        ///     this.picker.IsOpen = true;
        /// }
        /// ]]></code>
        /// </example>
        public View RelativeView
        {
            get { return (View)GetValue(RelativeViewProperty); }
            set { SetValue(RelativeViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header template or template selector for picker header
        /// </summary>
        /// <remarks>
        /// The BindingContext of the HeaderTemplate is respective picker control. When using header template, the header style customization will not be applicable.
        /// </remarks>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column header template or template selector for picker column header.
        /// </summary>
        /// <remarks>
        /// The BindingContext of the ColumnHeaderTemplate is respective picker control. When using column header template, the column header style customization will not be applicable.
        /// </remarks>
        public DataTemplate ColumnHeaderTemplate
        {
            get { return (DataTemplate)GetValue(ColumnHeaderTemplateProperty); }
            set { SetValue(ColumnHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the footer template or template selector for picker footer.
        /// </summary>
        /// <remarks>
        /// The BindingContext of the FooterTemplate is respective picker control. When using footer template, the footer style customization will not be applicable.
        /// </remarks>
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker ok button clicked command.
        /// </summary>
        /// <value> The default value of <see cref="PickerBase.AcceptCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the AcceptCommand property in the picker.
        /// # [XAML](#tab/tabid-23)
        /// <code Lang="XAML"><![CDATA[
        /// <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        /// </ContentPage.BindingContext>
        /// <picker:SfPicker x:Name="Picker"
        ///                  AcceptCommand="{Binding AcceptCommand}">
        /// </picker:SfPicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-24)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command okButtonClickedCommand;
        ///    public ICommand AcceptCommand {
        ///        get
        ///        {
        ///            return okButtonClickedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (okButtonClickedCommand != value)
        ///            {
        ///                okButtonClickedCommand = value;
        ///                OnPropertyChanged(nameof(AcceptCommand));
        ///            }
        ///        }
        ///    }
        ///    public ViewModel()
        ///    {
        ///       AcceptCommand = new Command(OkButtonClicked);
        ///    }
        ///    private void OkButtonClicked()
        ///    {
        ///    }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker cancel button clicked command.
        /// </summary>
        /// <value> The default value of <see cref="PickerBase.DeclineCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the DeclineCommand property in the picker.
        /// # [XAML](#tab/tabid-25)
        /// <code Lang="XAML"><![CDATA[
        /// <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        /// </ContentPage.BindingContext>
        /// <picker:SfPicker x:Name="Picker"
        ///                  DeclineCommand="{Binding DeclineCommand}">
        /// </picker:SfPicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-26)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command cancelButtonCommand;
        ///    public ICommand DeclineCommand {
        ///        get
        ///        {
        ///            return cancelButtonCommand;
        ///        }
        ///        set
        ///        {
        ///            if (cancelButtonCommand != value)
        ///            {
        ///                cancelButtonCommand = value;
        ///                OnPropertyChanged(nameof(DeclineCommand));
        ///            }
        ///        }
        ///    }
        ///    public ViewModel()
        ///    {
        ///       DeclineCommand = new Command(CancelButtonCanceled);
        ///    }
        ///    private void CancelButtonCanceled()
        ///    {
        ///    }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand DeclineCommand
        {
            get { return (ICommand)GetValue(DeclineCommandProperty); }
            set { SetValue(DeclineCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the picker can perform looping.
        /// </summary>
        public bool EnableLooping
        {
            get { return (bool)GetValue(EnableLoopingProperty); }
            set { SetValue(EnableLoopingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the popup in the picker.
        /// </summary>
        /// <remarks>
        /// The default width of the popup will be the addition of picker column width.
        /// </remarks>
        public double PopupWidth
        {
            get { return (double)GetValue(PopupWidthProperty); }
            set { SetValue(PopupWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the popup in the picker.
        /// </summary>
        /// <remarks>
        /// The default height of the popup will be the addition of header height, column header height, picker items' five items height, and footer height.
        /// </remarks>
        public double PopupHeight
        {
            get { return (double)GetValue(PopupHeightProperty); }
            set { SetValue(PopupHeightProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the value of column header view settings.
        /// </summary>
        PickerColumnHeaderView IColumnHeaderView.ColumnHeaderView => BaseColumnHeaderView;

        /// <summary>
        /// Gets the value of picker columns.
        /// </summary>
        ObservableCollection<PickerColumn> IPicker.Columns => BaseColumns;

        /// <summary>
        /// Gets the value of header view settings.
        /// </summary>
        PickerHeaderView IHeaderView.HeaderView => BaseHeaderView;

        /// <summary>
        /// Gets the item template for the picker item.
        /// </summary>
        DataTemplate IPickerView.ItemTemplate => BaseItemTemplate;

        /// <summary>
        /// Gets the disabled text style for the picker item.
        /// </summary>
        PickerTextStyle IPickerView.DisabledTextStyle => DisabledTextStyle;

        /// <summary>
        /// Gets a value indicating whether the parent value have valid value because dynamic scrolling on dialog opening does not scroll because the picker stack layout does not have a parent value.
        /// </summary>
        bool IPickerView.IsValidParent => _pickerStackLayout != null && _pickerStackLayout.Parent != null && Parent != null;

        /// <summary>
        /// Gets a value indicating whether the enable looping value of the picker.
        /// </summary>
        bool IPickerView.EnableLooping => EnableLooping;

        /// <summary>
        /// Gets or sets the value of header view. This property can be used to customize the of header in Picker.
        /// </summary>
        internal PickerHeaderView BaseHeaderView
        {
            get { return (PickerHeaderView)GetValue(BaseHeaderViewProperty); }
            set { SetValue(BaseHeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker item template in Picker.
        /// </summary>
        internal DataTemplate BaseItemTemplate
        {
            get { return (DataTemplate)GetValue(BaseItemTemplateProperty); }
            set { SetValue(BaseItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of column header view. This property can be used to customize the of header column in Picker.
        /// </summary>
        internal PickerColumnHeaderView BaseColumnHeaderView
        {
            get { return (PickerColumnHeaderView)GetValue(BaseColumnHeaderViewProperty); }
            set { SetValue(BaseColumnHeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of picker columns. This property can be used to customize the of column in Picker.
        /// </summary>
        internal ObservableCollection<PickerColumn> BaseColumns
        {
            get { return (ObservableCollection<PickerColumn>)GetValue(BaseColumnsProperty); }
            set { SetValue(BaseColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker disabled item text style in Picker.
        /// </summary>
        internal PickerTextStyle DisabledTextStyle
        {
            get { return (PickerTextStyle)GetValue(DisabledTextStyleProperty); }
            set { SetValue(DisabledTextStyleProperty, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to update the selected text style.
        /// </summary>
        /// <returns> Returns the picker text style.</returns>
        PickerTextStyle UpdateSelectedTextStyle()
        {
            PickerTextStyle selectedTextStyle;
            if (TextDisplayMode == PickerTextDisplayMode.Default)
            {
                selectedTextStyle = new PickerTextStyle
                {
                    FontSize = 14,
                    TextColor = GetDefaultModeSelectedTextColor(),
                };

                return selectedTextStyle;
            }
            else
            {
                selectedTextStyle = new PickerTextStyle
                {
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = GetOpacityModeSelectedTextColor(),
                };

                return selectedTextStyle;
            }
        }

        /// <summary>
        /// Method to get the default mode selected text color.
        /// </summary>
        /// <returns>The selected color.</returns>
        Color GetDefaultModeSelectedTextColor()
        {
            Color selectedColor;
            switch (Children)
            {
                case SfPicker picker:
                    selectedColor = picker.SelectedTextColor;
                    break;
                case SfTimePicker timePicker:
                    selectedColor = timePicker.SelectedTextColor;
                    break;
                case SfDatePicker datePicker:
                    selectedColor = datePicker.SelectedTextColor;
                    break;
                case SfDateTimePicker dateTimePicker:
                    selectedColor = dateTimePicker.SelectedTextColor;
                    break;
                default:
                    selectedColor = Colors.White;
                    break;
            }

            return selectedColor;
        }

        /// <summary>
        /// Method to get the special mode selected text color.
        /// </summary>
        /// <returns>The selected color.</returns>
        Color GetOpacityModeSelectedTextColor()
        {
            Color selectedColor;
            switch (Children)
            {
                case SfPicker picker:
                    selectedColor = picker.SelectionTextColor;
                    break;
                case SfTimePicker timePicker:
                    selectedColor = timePicker.SelectionTextColor;
                    break;
                case SfDatePicker datePicker:
                    selectedColor = datePicker.SelectionTextColor;
                    break;
                case SfDateTimePicker dateTimePicker:
                    selectedColor = dateTimePicker.SelectionTextColor;
                    break;
                default:
                    selectedColor = Colors.White;
                    break;
            }

            return selectedColor;
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
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerHeaderView? oldStyle = oldValue as PickerHeaderView;
            if (oldStyle != null)
            {
                oldStyle.PickerPropertyChanged -= picker.OnHeaderSettingsPropertyChanged;
                if (oldStyle.TextStyle != null)
                {
                    oldStyle.TextStyle.PropertyChanged -= picker.OnHeaderTextStylePropertyChanged;
                    oldStyle.TextStyle.BindingContext = null;
                }

                if (oldStyle.SelectionTextStyle != null)
                {
                    oldStyle.SelectionTextStyle.PropertyChanged -= picker.OnHeaderSelectionTextStylePropertyChanged;
                    oldStyle.SelectionTextStyle.BindingContext = null;
                }

                oldStyle.BindingContext = null;
            }

            PickerHeaderView? newStyle = newValue as PickerHeaderView;
            if (newStyle != null)
            {
                PickerHelper.SetHeaderDynamicResource(newStyle, picker);
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PickerPropertyChanged += picker.OnHeaderSettingsPropertyChanged;
                if (newStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(newStyle.TextStyle, picker.BindingContext);
                    newStyle.TextStyle.PropertyChanged += picker.OnHeaderTextStylePropertyChanged;
                }

                if (newStyle.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(newStyle.SelectionTextStyle, picker.BindingContext);
                    newStyle.SelectionTextStyle.PropertyChanged += picker.OnHeaderSelectionTextStylePropertyChanged;
                }
            }

            picker.AddOrRemoveHeaderLayout();
            picker.UpdatePopupSize();
            if (picker._headerLayout == null)
            {
                return;
            }

            if (newStyle != null)
            {
                picker._headerLayout.InvalidateHeaderView();
                picker._headerLayout.UpdateHeaderDateText();
                picker._headerLayout.UpdateHeaderTimeText();
                picker._headerLayout.UpdateIconButtonTextStyle();
            }

            if (picker._availableSize == Size.Zero)
            {
                return;
            }

            picker.InvalidatePickerView();
        }

        /// <summary>
        /// Method invokes on picker footer view changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnFooterViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerFooterView? oldStyle = oldValue as PickerFooterView;
            if (oldStyle != null)
            {
                oldStyle.PickerPropertyChanged -= picker.OnFooterSettingsPropertyChanged;
                if (oldStyle.TextStyle != null)
                {
                    oldStyle.TextStyle.PropertyChanged -= picker.OnFooterTextStylePropertyChanged;
                    oldStyle.TextStyle.BindingContext = null;
                }

                oldStyle.Parent = null;
                oldStyle.BindingContext = null;
            }

            PickerFooterView? newStyle = newValue as PickerFooterView;
            if (newStyle != null)
            {
                newStyle.Parent = picker;
                PickerHelper.SetFooterDynamicResource(newStyle, picker);
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PickerPropertyChanged += picker.OnFooterSettingsPropertyChanged;
                if (newStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(newStyle.TextStyle, picker.BindingContext);
                    newStyle.TextStyle.PropertyChanged += picker.OnFooterTextStylePropertyChanged;
                }
            }

            picker.AddOrRemoveFooterLayout();
            picker.UpdatePopupSize();
            if (picker._footerLayout == null)
            {
                return;
            }

            picker._footerLayout.Background = picker.FooterView.Background;
            picker._footerLayout.UpdateFooterStyle();
            if (picker._availableSize == Size.Zero)
            {
                return;
            }

            picker.InvalidatePickerView();
        }

        /// <summary>
        /// Method invokes on the picker item height changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnItemHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker._pickerContainer?.UpdateItemHeight();
        }

        /// <summary>
        /// Method invokes on the picker item template changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker._pickerContainer?.UpdateItemTemplate();
        }

        /// <summary>
        /// Method invokes on picker selected text style property changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null || picker._isInternalPropertyChange)
            {
                return;
            }

            picker._isExternalStyle = true;
            PickerTextStyle? oldStyle = oldValue as PickerTextStyle;
            if (oldStyle != null)
            {
                oldStyle.PropertyChanged -= picker.OnSelectedTextStylePropertyChanged;
                oldStyle.BindingContext = null;
                oldStyle.Parent = null;
            }

            PickerTextStyle? newStyle = newValue as PickerTextStyle;
            if (newStyle != null)
            {
                newStyle.Parent = picker;
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PropertyChanged += picker.OnSelectedTextStylePropertyChanged;
            }

            //// No need to update the picker scroll view when the picker size is not defined.
            if (picker._availableSize == Size.Zero || picker._pickerContainer == null)
            {
                return;
            }

            picker._pickerContainer.UpdateScrollViewDraw();
        }

        /// <summary>
        /// Method invokes on picker unselected text style property changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerTextStyle? oldStyle = oldValue as PickerTextStyle;
            if (oldStyle != null)
            {
                oldStyle.PropertyChanged -= picker.OnUnSelectedTextStylePropertyChanged;
                oldStyle.BindingContext = null;
                oldStyle.Parent = null;
            }

            PickerTextStyle? newStyle = newValue as PickerTextStyle;
            if (newStyle != null)
            {
                newStyle.Parent = picker;
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PropertyChanged += picker.OnUnSelectedTextStylePropertyChanged;
            }

            //// No need to update the picker scroll view when the picker size is not defined.
            if (picker._availableSize == Size.Zero || picker._pickerContainer == null)
            {
                return;
            }

            picker._pickerContainer.UpdateScrollViewDraw();
        }

        /// <summary>
        /// Method invokes on the picker selection view changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectionViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (oldValue is PickerSelectionView oldStyle)
            {
                oldStyle.PropertyChanged -= picker.OnSelectionViewPropertyChanged;
                oldStyle.Parent = null;
            }

            if (newValue is PickerSelectionView newStyle)
            {
                newStyle.Parent = picker;
                newStyle.PropertyChanged += picker.OnSelectionViewPropertyChanged;
            }

            //// No need to update the picker selection view when the picker size is not defined.
            if (picker._availableSize == Size.Zero)
            {
                return;
            }

            picker.UpdatePickerSelectionView();
        }

        /// <summary>
        /// Method invokes on the picker columns changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            ObservableCollection<PickerColumn>? oldColumns = oldValue as ObservableCollection<PickerColumn>;
            if (oldColumns != null)
            {
                oldColumns.CollectionChanged -= picker.OnColumnsCollectionChanged;
                foreach (PickerColumn column in oldColumns)
                {
                    column.BindingContext = null;
                    column.PickerPropertyChanged -= picker.OnColumnPropertyChanged;
                    column.PickerColumnCollectionChanged -= picker.OnItemsSourceCollectionChanged;
                    column.UnWireCollectionChanged(column.ItemsSource);
                    column.Parent = null;
                }
            }

            ObservableCollection<PickerColumn>? newColumns = newValue as ObservableCollection<PickerColumn>;
            if (newColumns != null)
            {
                newColumns.CollectionChanged += picker.OnColumnsCollectionChanged;
                for (int index = 0; index < newColumns.Count; index++)
                {
                    PickerColumn column = newColumns[index];
                    column._columnIndex = index;
                    SetInheritedBindingContext(column, picker.BindingContext);
                    if (column.SelectedIndex != 0 && column.SelectedItem != null)
                    {
                        picker.SelectionIndexChanged?.Invoke(picker, new PickerSelectionChangedEventArgs { NewValue = column.SelectedIndex, OldValue = 0, ColumnIndex = column._columnIndex });
                    }

                    column.Parent = picker;
                    column.PickerPropertyChanged += picker.OnColumnPropertyChanged;
                    column.WireCollectionChanged();
                    column.PickerColumnCollectionChanged += picker.OnItemsSourceCollectionChanged;
                }

                picker.UpdatePopupSize();
                picker._pickerContainer?.ResetPickerColumns(newColumns);
            }
        }

        /// <summary>
        /// Method invokes on the picker column divider color changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnColumnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker.SetParent(oldValue as Element, newValue as Element);
            picker._pickerContainer?.UpdateColumnDividerColor();
        }

        /// <summary>
        /// Method invokes on the picker column header view changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnColumnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerColumnHeaderView? oldStyle = oldValue as PickerColumnHeaderView;
            if (oldStyle != null)
            {
                oldStyle.PickerPropertyChanged -= picker.OnColumnHeaderViewPropertyChanged;
                if (oldStyle.TextStyle != null)
                {
                    oldStyle.TextStyle.PropertyChanged -= picker.OnColumnHeaderTextStylePropertyChanged;
                    oldStyle.TextStyle.BindingContext = null;
                    oldStyle.Parent = null;
                }

                oldStyle.BindingContext = null;
            }

            PickerColumnHeaderView? newStyle = newValue as PickerColumnHeaderView;
            if (newStyle != null)
            {
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PickerPropertyChanged += picker.OnColumnHeaderViewPropertyChanged;
                if (newStyle.TextStyle != null)
                {
                    newStyle.Parent = picker;
                    SetInheritedBindingContext(newStyle.TextStyle, picker.BindingContext);
                    newStyle.TextStyle.PropertyChanged += picker.OnColumnHeaderTextStylePropertyChanged;
                }
            }

            //// Apply the column header template to the sfpicker control.
            if (picker.BaseColumnHeaderView.Height > 0 && picker.ColumnHeaderTemplate != null && picker._columnHeaderLayout == null)
            {
                picker.AddorRemoveColumnHeaderLayout();
            }

            if (picker.ColumnHeaderTemplate == null)
            {
                picker._pickerContainer?.UpdateColumnHeaderHeight();
                picker._pickerContainer?.UpdateColumnHeaderDraw();
                picker._pickerContainer?.UpdateColumnHeaderDividerColor();
#if WINDOWS
				//// When looping is enabled, the selected item is not updated properly when the column height changes in Windows. Therefore, the item height is updated.
				if (picker.EnableLooping)
				{
					picker._pickerContainer?.UpdateItemHeight();
				}
#elif ANDROID || IOS
                //// While adding the picker item height, the picker selected item not updated properly in android and ios. So, we have updated that.
                picker._pickerContainer?.UpdateItemHeight();
#endif
            }
        }

        /// <summary>
        /// Method invokes on IsOpen property changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            //// Here we have checked the picker visibility because the picker is not visible, then the picker popup will not be opened.
            //// Here we just stored the previous value of the picker visibility and then return.
            //// Storing the previous value is used to define the state of the picker popup while the picker visibility is changing.
            if (!picker.IsVisible)
            {
                if (!(bool)newValue)
                {
                    picker._isPickerPreviouslyOpened = !(bool)newValue;
                }
                else
                {
                    picker._isPickerPreviouslyOpened = (bool)newValue;
                }

                return;
            }

            bool isOpen = false;
            if (newValue is bool value)
            {
                isOpen = value;
            }

            if (isOpen && (picker.Mode == PickerMode.Dialog || picker.Mode == PickerMode.RelativeDialog))
            {
                if (picker.Children.Count != 0 && picker._pickerStackLayout != null && picker._pickerStackLayout.Parent == picker)
                {
                    picker.Remove(picker._pickerStackLayout);
                }

                picker.AddPickerToPopup();
                picker.ShowPopup();
            }
            else
            {
                picker.ClosePickerPopup();
                if (picker._pickerStackLayout != null && picker._pickerStackLayout.Parent != null && picker._pickerStackLayout.Parent != picker && picker._pickerStackLayout.Parent is ICollection<IView> view)
                {
                    view.Remove(picker._pickerStackLayout);
                }
            }
        }

        /// <summary>
        /// Method invokes on the picker mode changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerMode mode = PickerMode.Default;
            if (newValue is PickerMode value)
            {
                mode = value;
            }

            if (mode == PickerMode.Dialog || mode == PickerMode.RelativeDialog)
            {
                if (picker.Children.Count != 0 && picker._pickerStackLayout != null && picker._pickerStackLayout.Parent == picker)
                {
                    picker.Remove(picker._pickerStackLayout);
                }

                if (picker.IsOpen)
                {
                    picker.AddPickerToPopup();
                    picker.ShowPopup();
                }
                else
                {
                    picker.ClosePickerPopup();
                }
            }
            else
            {
                picker.ResetPopup();
                picker.OnPickerLoading();
                if (picker._pickerStackLayout != null)
                {
                    if (picker._pickerStackLayout.Parent != null && picker._pickerStackLayout.Parent is ICollection<IView> view)
                    {
                        view.Remove(picker._pickerStackLayout);
                    }

                    picker.Add(picker._pickerStackLayout);
#if ANDROID
                    //// While adding the picker to the popup, the picker selected item not updated properly in android. So, we have updated that.
                    picker.Dispatcher.Dispatch(() =>
                    {
                        picker._pickerContainer?.UpdateItemHeight();
                    });
#endif
                }
            }

            picker.InvalidateMeasure();
        }

        /// <summary>
        /// Method invokes on the picker text display mode changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextDisplayModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker._pickerContainer?.UpdateScrollViewDraw();
            picker._pickerContainer?.UpdatePickerSelectionView();
        }

        /// <summary>
        /// Method invokes on the picker popup relative position changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRelativePositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (picker.IsOpen && picker.Mode == PickerMode.RelativeDialog)
            {
                picker.ShowPopup();
            }
        }

        /// <summary>
        /// Method invokes on the picker enable looping changed.
        /// </summary>
        /// <param name="bindable">The picker setting object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnEnableLoopingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker._pickerContainer?.UpdateEnableLooping();
        }

        /// <summary>
        /// Update the PopupWidth.
        /// </summary>
        /// <param name="bindable">the Picker settings object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnPopupWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker.UpdatePopupSize();
        }

        /// <summary>
        /// Update the PopupHeight.
        /// </summary>
        /// <param name="bindable">the Picker settings object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnPopupHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            picker.UpdatePopupSize();
        }

        /// <summary>
        /// Method invokes on the picker popup relative view changed.
        /// </summary>
        /// <param name="bindable">The picker setting object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRelativeViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (picker.IsOpen && picker.Mode == PickerMode.RelativeDialog && picker.RelativeView != null)
            {
                picker.ShowPopup();
            }
        }

        /// <summary>
        /// Method to invoke the header template changed.
        /// </summary>
        /// <param name="bindable">The picker setting object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (picker._headerLayout != null && picker.BaseHeaderView.Height > 0 && picker.HeaderTemplate != null)
            {
                picker._headerLayout.InitializeTemplateView();
            }
        }

        /// <summary>
        /// Method to invoke the column header template changed.
        /// </summary>
        /// <param name="bindable">The picker setting object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnColumnHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (picker.BaseColumnHeaderView.Height > 0 && picker.ColumnHeaderTemplate != null)
            {
                picker.AddorRemoveColumnHeaderLayout();
            }
        }

        /// <summary>
        /// Method to invoke the footer template view changed.
        /// </summary>
        /// <param name="bindable">The picker setting object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            if (picker._footerLayout != null && picker.FooterView.Height > 0 && picker.FooterTemplate != null)
            {
                picker._footerLayout?.InitializeTemplateView();
            }
        }

        /// <summary>
        /// Method to set the default width for the popup
        /// </summary>
        /// <returns>Returns the default value of PopupWidth</returns>
        static double GetDefaultPopupWidth(BindableObject bindable)
        {
            var pickerBase = (PickerBase)bindable;
            double popupWidth = 0;
            double width = 0;
            int columnCount = pickerBase.BaseColumns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                width += pickerBase.BaseColumns[i].Width <= 0 ? 100 : pickerBase.BaseColumns[i].Width;
            }

            popupWidth = width < 200 ? 200 : width;

            //// Based on the Header,Column Header and Footer Height popupwidth value gets returned.
            if (pickerBase.BaseHeaderView.Height != 0 || pickerBase.FooterView.Height != 0 || pickerBase.BaseColumnHeaderView.Height != 0)
            {
                return popupWidth;
            }

            return columnCount == 0 ? 0 : popupWidth;
        }

        /// <summary>
        /// Method to set the default height of the popup
        /// </summary>
        /// <returns>Returns the default value of PopupHeight</returns>
        static double GetDefaultPopupHeight(BindableObject bindable)
        {
            var pickerBase = (PickerBase)bindable;
            double popupHeight = 0;
            int count = 0;
            int columnCount = pickerBase.BaseColumns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                ICollection itemsSource = (ICollection)pickerBase.BaseColumns[i].ItemsSource;
                if (itemsSource != null)
                {
                    count = count < itemsSource.Count ? itemsSource.Count : count;
                    if (pickerBase.BaseColumns[i].SelectedIndex <= -1)
                    {
                        count = count + 1;
                    }
                }
            }

            popupHeight = pickerBase.BaseHeaderView.Height + pickerBase.BaseColumnHeaderView.Height + (pickerBase.ItemHeight * (count >= 5 ? 5 : count)) + pickerBase.FooterView.Height;
            return popupHeight;
        }

        /// <summary>
        /// Method to invokes while the item source collection changed.
        /// </summary>
        /// <param name="sender">The picker column item source object.</param>
        /// <param name="e">The collection changed event arguments.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PickerColumn? pickerColumn = sender as PickerColumn;
            if (pickerColumn == null || _pickerContainer == null)
            {
                return;
            }

            //// While the items source collection changed, the picker popup size also need to be updated. Because based on the item source collection, the picker popup size will be changed.
            UpdatePopupSize();
            _pickerContainer.UpdateItemsSource(pickerColumn._columnIndex);
        }

        /// <summary>
        /// Method to invokes while the column property changed.
        /// </summary>
        /// <param name="sender">The picker column object.</param>
        /// <param name="e">The collection changed event arguments.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        void OnColumnPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            PickerColumn? column = sender as PickerColumn;
            if (column == null)
            {
                return;
            }

            if (e.PropertyName == nameof(PickerColumn.SelectedIndex))
            {
                // If the selected index is updated dynamically beyond the items count, then the old and new index will be actual index.
                //// Assume items count = 10.
                //// Here old index = 9.
                int oldIndex = -1;
                int itemsCount = PickerHelper.GetItemsCount(column.ItemsSource);
                if (e.OldValue != null)
                {
                    oldIndex = PickerHelper.GetValidSelectedIndex((int)e.OldValue, itemsCount);
                }

                //// The old index changed from 9 to 10 then the actual new index value is 9.
                int newIndex = PickerHelper.GetValidSelectedIndex(column.SelectedIndex, itemsCount);
                column._isSelectedItemChanged = false;
                column.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(column);
                SelectionIndexChanged?.Invoke(this, new PickerSelectionChangedEventArgs { NewValue = column.SelectedIndex, OldValue = oldIndex, ColumnIndex = column._columnIndex });
                if (EnableLooping && newIndex <= -1)
                {
                    return;
                }

                if (newIndex == -1)
                {
                    //// If selected index is -1, then change all column selected item value as null.
                    foreach (var columns in BaseColumns)
                    {
                        columns.SelectedItem = null;
                        columns.SelectedIndex = -1;
                    }
                }
                else if (newIndex != -1)
                {
                    //// If selected index is not equal to -1, then check the remaining column selected index value is -1.
                    //// If any column value as -1, then change the seelcted index value to 0.
                    for (int index = 0; index < BaseColumns.Count; index++)
                    {
                        if (column._columnIndex == index)
                        {
                            continue;
                        }
                        else if (BaseColumns[index].SelectedIndex <= -1)
                        {
                            BaseColumns[index].SelectedIndex = 0;
                        }
                    }
                }

                //// From the above example, the old index and new index are same. So, no need to update the selection changed event and UI.
                if (oldIndex == newIndex)
                {
                    return;
                }

                UpdateSelectedIndexValue(column._columnIndex);
                _pickerContainer?.UpdateItemHeight();
            }
            else if (e.PropertyName == nameof(PickerColumn.ItemsSource))
            {
                if (e.OldValue != null)
                {
                    column.UnWireCollectionChanged(e.OldValue);
                    column.PickerColumnCollectionChanged -= OnItemsSourceCollectionChanged;
                }

                if (column.ItemsSource != null)
                {
                    column.WireCollectionChanged();
                    column.PickerColumnCollectionChanged += OnItemsSourceCollectionChanged;
                }

                UpdatePopupSize();
                _pickerContainer?.UpdateItemsSource(column._columnIndex);
            }
            else if (e.PropertyName == nameof(PickerColumn.Width))
            {
                UpdatePopupSize();
                _pickerContainer?.UpdatePickerColumnWidth();
            }
            else if (e.PropertyName == nameof(PickerColumn.HeaderText))
            {
                _pickerContainer?.UpdateHeaderText(column._columnIndex);
            }
            else if (e.PropertyName == nameof(PickerColumn.SelectedItem))
            {
                if (column.SelectedItem == null)
                {
                    column.Parent = this;
                    if (column.Parent is SfPicker)
                    {
                        if (!this.EnableLooping)
                        {
                            column.SelectedIndex = -1;
                            column._isSelectedItemChanged = true;
                            return;
                        }
                        else
                        {
                            _pickerContainer?.UpdateScrollViewDraw();
                            _pickerContainer?.InvalidateDrawable();
                            return;
                        }
                    }
                    else
                    {
                        _pickerContainer?.UpdateScrollViewDraw();
                        _pickerContainer?.InvalidateDrawable();
                        return;
                    }
                }

                int itemsCount = PickerHelper.GetItemsCount(column.ItemsSource);
                int valueCount = PickerHelper.GetSelectedItemIndex(column);
                int newIndex = PickerHelper.GetValidSelectedIndex(valueCount, itemsCount);
                if (column.SelectedIndex == newIndex)
                {
                    column._isSelectedItemChanged = true;
                    return;
                }

                column.SelectedIndex = newIndex;
                column._isSelectedItemChanged = true;
            }
        }

        /// <summary>
        /// Method to invokes while the columns collection changed.
        /// </summary>
        /// <param name="sender">The picker column object.</param>
        /// <param name="e">The collection changed event arguments.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        void OnColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                if (e.NewStartingIndex == BaseColumns.Count - 1)
                {
                    PickerColumn column = BaseColumns[e.NewStartingIndex];
                    column._columnIndex = e.NewStartingIndex;
                    SetInheritedBindingContext(column, BindingContext);
                    if (column.SelectedIndex != 0)
                    {
                        SelectionIndexChanged?.Invoke(this, new PickerSelectionChangedEventArgs { NewValue = column.SelectedIndex, OldValue = 0, ColumnIndex = column._columnIndex });
                    }

                    column.PickerPropertyChanged += OnColumnPropertyChanged;
                    column.WireCollectionChanged();
                    column.PickerColumnCollectionChanged += OnItemsSourceCollectionChanged;
                }
                else
                {
                    PickerColumn column = BaseColumns[e.NewStartingIndex];
                    column._columnIndex = e.NewStartingIndex;
                    SetInheritedBindingContext(column, BindingContext);
                    if (column.SelectedIndex != 0)
                    {
                        SelectionIndexChanged?.Invoke(this, new PickerSelectionChangedEventArgs { NewValue = column.SelectedIndex, OldValue = 0, ColumnIndex = column._columnIndex });
                    }

                    column.PickerPropertyChanged += OnColumnPropertyChanged;
                    column.WireCollectionChanged();
                    column.PickerColumnCollectionChanged += OnItemsSourceCollectionChanged;
                    for (int index = e.NewStartingIndex + 1; index < BaseColumns.Count; index++)
                    {
                        PickerColumn pickerColumn = BaseColumns[index];
                        pickerColumn._columnIndex = index;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex == -1)
                {
                    return;
                }

                ObservableCollection<PickerColumn>? oldColumns = e.OldItems as ObservableCollection<PickerColumn>;
                if (oldColumns != null)
                {
                    for (int index = 0; index < oldColumns.Count; index++)
                    {
                        PickerColumn column = oldColumns[index];
                        column.PickerPropertyChanged -= OnColumnPropertyChanged;
                        column.UnWireCollectionChanged(column.ItemsSource);
                        column.PickerColumnCollectionChanged -= OnItemsSourceCollectionChanged;
                        column.BindingContext = null;
                    }
                }

                for (int index = e.OldStartingIndex; index < BaseColumns.Count; index++)
                {
                    PickerColumn pickerColumn = BaseColumns[index];
                    pickerColumn._columnIndex = index;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                if (e.OldItems != null)
                {
                    foreach (PickerColumn pickerColumn in e.OldItems)
                    {
                        pickerColumn.PickerPropertyChanged -= OnColumnPropertyChanged;
                        pickerColumn.UnWireCollectionChanged(pickerColumn.ItemsSource);
                        pickerColumn.PickerColumnCollectionChanged -= OnItemsSourceCollectionChanged;
                        pickerColumn.BindingContext = null;
                    }
                }

                PickerColumn column = BaseColumns[e.NewStartingIndex];
                column._columnIndex = e.NewStartingIndex;
                SetInheritedBindingContext(column, BindingContext);
                if (column.SelectedIndex != 0)
                {
                    SelectionIndexChanged?.Invoke(this, new PickerSelectionChangedEventArgs { NewValue = column.SelectedIndex, OldValue = 0, ColumnIndex = column._columnIndex });
                }

                column.PickerPropertyChanged += OnColumnPropertyChanged;
                column.WireCollectionChanged();
                column.PickerColumnCollectionChanged += OnItemsSourceCollectionChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                for (int index = 0; index < BaseColumns.Count; index++)
                {
                    PickerColumn pickerColumn = BaseColumns[index];
                    pickerColumn._columnIndex = index;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    foreach (PickerColumn pickerColumn in e.OldItems)
                    {
                        pickerColumn.PickerPropertyChanged -= OnColumnPropertyChanged;
                        pickerColumn.UnWireCollectionChanged(pickerColumn.ItemsSource);
                        pickerColumn.PickerColumnCollectionChanged -= OnItemsSourceCollectionChanged;
                        pickerColumn.BindingContext = null;
                    }
                }

                for (int index = 0; index < BaseColumns.Count; index++)
                {
                    PickerColumn pickerColumn = BaseColumns[index];
                    pickerColumn._columnIndex = index;
                    SetInheritedBindingContext(pickerColumn, BindingContext);
                    pickerColumn.PickerPropertyChanged += OnColumnPropertyChanged;
                    pickerColumn.WireCollectionChanged();
                    pickerColumn.PickerColumnCollectionChanged += OnItemsSourceCollectionChanged;
                }
            }

            UpdatePopupSize();
            if (_pickerContainer == null)
            {
                return;
            }

            _pickerContainer.OnColumnsCollectionChanged(e);
        }

        /// <summary>
        /// Method invokes on selection view settings property changed.
        /// </summary>
        /// <param name="sender">The picker object.</param>
        /// <param name="e">The property changed argument.</param>
        void OnSelectionViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //// No need to update the picker selection view when the picker size is not defined.
            if (_availableSize == Size.Zero)
            {
                return;
            }

            UpdatePickerSelectionView();
        }

        /// <summary>
        /// Method invokes on header settings property changed.
        /// </summary>
        /// <param name="sender">The picker object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnHeaderSettingsPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PickerHeaderView.Height))
            {
                AddOrRemoveHeaderLayout();
                UpdatePopupSize();
                if (_availableSize == Size.Zero)
                {
                    return;
                }

                InvalidatePickerView();
#if ANDROID || IOS
                //// While adding the picker item height, the picker selected item not updated properly in android and ios. So, we have updated that.
                _pickerContainer?.UpdateItemHeight();
#endif
            }
            else if (e.PropertyName == nameof(PickerHeaderView.TextStyle))
            {
                PickerTextStyle? oldStyle = e.OldValue as PickerTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.PropertyChanged -= OnHeaderTextStylePropertyChanged;
                    oldStyle.BindingContext = null;
                }

                if (BaseHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(BaseHeaderView.TextStyle, BindingContext);
                    BaseHeaderView.TextStyle.PropertyChanged += OnHeaderTextStylePropertyChanged;
                }

                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.InvalidateHeaderView();
                _headerLayout.UpdateIconButtonTextStyle();
            }
            else if (e.PropertyName == nameof(PickerHeaderView.SelectionTextStyle))
            {
                PickerTextStyle? oldStyle = e.OldValue as PickerTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.PropertyChanged -= OnHeaderSelectionTextStylePropertyChanged;
                    oldStyle.BindingContext = null;
                }

                if (BaseHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(BaseHeaderView.SelectionTextStyle, BindingContext);
                    BaseHeaderView.SelectionTextStyle.PropertyChanged += OnHeaderSelectionTextStylePropertyChanged;
                }

                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.InvalidateHeaderView();
                _headerLayout.UpdateIconButtonTextStyle();
            }
            else if (e.PropertyName == nameof(PickerHeaderView.Background) || e.PropertyName == nameof(PickerHeaderView.DividerColor) || e.PropertyName == nameof(PickerHeaderView.Text))
            {
                if (_headerLayout == null || _availableSize == Size.Zero)
                {
                    return;
                }

                _headerLayout.InvalidateHeaderView();
            }
            else if (e.PropertyName == nameof(PickerHeaderView.DateText))
            {
                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.UpdateHeaderDateText();
            }
            else if (e.PropertyName == nameof(PickerHeaderView.TimeText))
            {
                if (_headerLayout == null)
                {
                    return;
                }

                _headerLayout.UpdateHeaderTimeText();
            }
        }

        /// <summary>
        /// Method invokes on header text style property changed.
        /// </summary>
        /// <param name="sender">The picker object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnHeaderTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_headerLayout == null || _availableSize == Size.Zero)
            {
                return;
            }

            _headerLayout.InvalidateHeaderView();
            _headerLayout.UpdateIconButtonTextStyle();
        }

        /// <summary>
        /// Method invokes on header selection text style property changed.
        /// </summary>
        /// <param name="sender">The picker object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnHeaderSelectionTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_headerLayout == null || _availableSize == Size.Zero)
            {
                return;
            }

            _headerLayout.InvalidateHeaderView();
            _headerLayout.UpdateIconButtonTextStyle();
        }

        /// <summary>
        /// Method invokes on column header text style property changed.
        /// </summary>
        /// <param name="sender">The picker column object.</param>
        /// <param name="e">The property changed event arguments.</param>
        void OnColumnHeaderTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_pickerContainer == null || _availableSize == Size.Zero)
            {
                return;
            }

            _pickerContainer.UpdateColumnHeaderDraw();
        }

        /// <summary>
        /// Method invokes on footer button text style property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events arguments.</param>
        void OnFooterTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _footerLayout?.UpdateButtonTextStyle();
        }

        /// <summary>
        /// Method to update the footer property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events args.</param>
        void OnFooterSettingsPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PickerFooterView.Background))
            {
                if (_footerLayout == null || FooterTemplate != null)
                {
                    return;
                }

                _footerLayout.Background = FooterView.Background;
            }
            else if (e.PropertyName == nameof(PickerFooterView.OkButtonText))
            {
                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.UpdateConfirmButtonText();
            }
            else if (e.PropertyName == nameof(PickerFooterView.CancelButtonText))
            {
                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.UpdateCancelButtonText();
            }
            else if (e.PropertyName == nameof(PickerFooterView.Height))
            {
                AddOrRemoveFooterLayout();
                UpdatePopupSize();
                if (_availableSize == Size.Zero)
                {
                    return;
                }

                InvalidatePickerView();
#if ANDROID || IOS
                //// While adding the picker item height, the picker selected item not updated properly in android and ios. So, we have updated that.
                _pickerContainer?.UpdateItemHeight();
#endif
            }
            else if (e.PropertyName == nameof(PickerFooterView.DividerColor))
            {
                if (_footerLayout == null || _availableSize == Size.Zero)
                {
                    return;
                }

                _footerLayout.UpdateSeparatorColor();
            }
            else if (e.PropertyName == nameof(PickerFooterView.ShowOkButton))
            {
                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.AddOrRemoveFooterButtons();
            }
            else if (e.PropertyName == nameof(PickerFooterView.TextStyle))
            {
                PickerTextStyle? oldStyle = e.OldValue as PickerTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.PropertyChanged -= OnFooterTextStylePropertyChanged;
                    oldStyle.BindingContext = null;
                }

                if (FooterView.TextStyle != null)
                {
                    SetInheritedBindingContext(FooterView.TextStyle, BindingContext);
                    FooterView.TextStyle.PropertyChanged += OnFooterTextStylePropertyChanged;
                }

                if (_footerLayout == null)
                {
                    return;
                }

                _footerLayout.UpdateButtonTextStyle();
            }
        }

        /// <summary>
        /// Method to update the selected text style.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events args.</param>
        void OnSelectedTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_pickerContainer == null || _availableSize == Size.Zero || _isInternalPropertyChange)
            {
                return;
            }

            _isExternalStyle = true;
            _pickerContainer.UpdateScrollViewDraw();
        }

        /// <summary>
        /// Method to update the unselected test style.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events args.</param>
        void OnUnSelectedTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_pickerContainer == null || _availableSize == Size.Zero)
            {
                return;
            }

            _pickerContainer.UpdateScrollViewDraw();
        }

        /// <summary>
        /// Method to update the column header property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events args.</param>
        void OnColumnHeaderViewPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PickerColumnHeaderView.Height))
            {
                if (ColumnHeaderTemplate != null)
                {
                    AddorRemoveColumnHeaderLayout();
                    UpdatePopupSize();
                    if (_availableSize == Size.Zero)
                    {
                        return;
                    }

                    InvalidatePickerView();
                }
                else
                {
                    _pickerContainer?.UpdateColumnHeaderHeight();
#if WINDOWS || MACCATALYST
                    if (EnableLooping)
                    {
                        _pickerContainer?.UpdateItemHeight();
                    }
#endif
                }

#if ANDROID || IOS
                //// While adding the picker item height, the picker selected item not updated properly in android and ios. So, we have updated that.
                _pickerContainer?.UpdateItemHeight();
#endif
            }
            else if (e.PropertyName == nameof(PickerColumnHeaderView.TextStyle))
            {
                PickerTextStyle? oldStyle = e.OldValue as PickerTextStyle;
                if (oldStyle != null)
                {
                    oldStyle.PropertyChanged -= OnColumnHeaderTextStylePropertyChanged;
                    oldStyle.BindingContext = null;
                }

                if (BaseHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(BaseColumnHeaderView.TextStyle, BindingContext);
                    BaseColumnHeaderView.TextStyle.PropertyChanged += OnColumnHeaderTextStylePropertyChanged;
                }

                _pickerContainer?.UpdateColumnHeaderDraw();
            }
            else if (e.PropertyName == nameof(PickerColumnHeaderView.DividerColor))
            {
                _pickerContainer?.UpdateColumnHeaderDividerColor();
            }
            else if (e.PropertyName == nameof(PickerColumnHeaderView.Background))
            {
                _pickerContainer?.UpdateColumnHeaderDraw();
            }
        }

        /// <summary>
        /// Method to update the picker property change.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The property changed event args</param>
        void OnPickerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_pickerStackLayout == null)
            {
                return;
            }

            if (e.PropertyName == nameof(BackgroundColor))
            {
                _pickerStackLayout.BackgroundColor = BackgroundColor;
            }
            else if (e.PropertyName == nameof(Background))
            {
                _pickerStackLayout.Background = Background;
            }
            else if (e.PropertyName == nameof(TextDisplayMode))
            {
                if (_isExternalStyle)
                {
                    return;
                }

                _isInternalPropertyChange = true;
                SelectedTextStyle = UpdateSelectedTextStyle();
                _isInternalPropertyChange = false;
            }
#if ANDROID
            else if (e.PropertyName == nameof(HeightRequest))
            {
                _pickerContainer?.UpdateItemHeight();
            }
#endif
        }

        /// <summary>
        /// Method invokes on picker disabled text style property changed.
        /// </summary>
        /// <param name="bindable">The picker settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PickerBase? picker = bindable as PickerBase;
            if (picker == null)
            {
                return;
            }

            PickerTextStyle? oldStyle = oldValue as PickerTextStyle;
            if (oldStyle != null)
            {
                oldStyle.PropertyChanged -= picker.OnDisabledTextStylePropertyChanged;
                oldStyle.BindingContext = null;
                oldStyle.Parent = null;
            }

            PickerTextStyle? newStyle = newValue as PickerTextStyle;
            if (newStyle != null)
            {
                newStyle.Parent = picker;
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PropertyChanged += picker.OnDisabledTextStylePropertyChanged;
            }

            //// No need to update the picker scroll view when the picker size is not defined.
            if (picker._availableSize == Size.Zero || picker._pickerContainer == null)
            {
                return;
            }

            picker._pickerContainer.UpdateScrollViewDraw();
        }

        /// <summary>
        /// Method to update the disabled text style.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The property changed events args.</param>
        void OnDisabledTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_pickerContainer == null || _availableSize == Size.Zero)
            {
                return;
            }

            _pickerContainer.UpdateScrollViewDraw();
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Method to get the default text style for the picker view.
        /// </summary>
        /// <returns>Returns the default text style.</returns>
        static ITextElement GetPickerTextStyle(BindableObject bindable)
        {
            var pickerBase = (PickerBase)bindable;
            PickerTextStyle pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#1C1B1F"),
                Parent = pickerBase,
            };

            return pickerTextStyle;
        }

        /// <summary>
        /// Method to get the default selected text style for the picker view.
        /// </summary>
        /// <returns>Returns the default selected text style.</returns>
        static ITextElement GetPickerSelectionTextStyle(BindableObject bindable)
        {
            var pickerBase = (PickerBase)bindable;
            PickerTextStyle pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Colors.White,
                Parent = pickerBase,
            };

            return pickerTextStyle;
        }

        /// <summary>
        /// Method to get the default text style for the picker view.
        /// </summary>
        /// <returns>Returns the default text style.</returns>
        static ITextElement GetPickerDisabledTextStyle(BindableObject bindable)
        {
            var pickerBase = (PickerBase)bindable;
            PickerTextStyle pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#611c1b1f"),
                Parent = pickerBase,
            };

            return pickerTextStyle;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the ok button clicked on SfPicker. This event is not applicable for while the footer view is not visible and the ok button is not visible.
        /// </summary>
        public event EventHandler? OkButtonClicked;

        /// <summary>
        /// Occurs after the cancel button clicked on SfPicker. This event is not applicable for while the footer view is not visible.
        /// </summary>
        public event EventHandler? CancelButtonClicked;

        /// <summary>
        /// Occurs after the picker popup is opened.
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when the picker popup is closed.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when the picker popup is closing.
        /// </summary>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <summary>
        /// Occurs after the selected index changed on SfPicker.
        /// </summary>
        internal event EventHandler<PickerSelectionChangedEventArgs>? SelectionIndexChanged;

        #endregion
    }
}