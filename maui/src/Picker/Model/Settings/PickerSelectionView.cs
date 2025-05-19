namespace Syncfusion.Maui.Toolkit.Picker;

/// <summary>
/// Represents a class which is used to customize all the properties of selection view of the SfPicker.
/// </summary>
public class PickerSelectionView : Element
{
    #region Bindable Properties

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
            typeof(PickerSelectionView),
            defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#6750A4")));

    /// <summary>
    /// Identifies the <see cref="Stroke"/> dependency property.
    /// </summary>
    /// <value>
    /// The identifier for <see cref="Stroke"/> dependency property.
    /// </value>
    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Color),
            typeof(PickerSelectionView),
            defaultValueCreator: bindable => Colors.Transparent);

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    /// <value>
    /// The identifier for <see cref="CornerRadius"/> dependency property.
    /// </value>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(PickerSelectionView),
            defaultValueCreator: bindable => new CornerRadius(20));

    /// <summary>
    /// Identifies the <see cref="Padding"/> dependency property.
    /// </summary>
    /// <value>
    /// The identifier for <see cref="Padding"/> dependency property.
    /// </value>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(PickerSelectionView),
            defaultValueCreator: bindable => new Thickness(5, 2));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the background of the selection view in SfPicker.
    /// </summary>
    /// <value>The default value of <see cref="PickerSelectionView.Background"/> is SolidColorBrush(Color.FromArgb("#6750A4")).</value>
    /// <example>
    /// The following example demonstrates how to set the background of the selection view.
    /// # [XAML](#tab/tabid-1)
    /// <code language="xaml">
    /// <![CDATA[
    /// <picker:SfPicker>
    ///     <picker:SfPicker.SelectionView>
    ///         <picker:PickerSelectionView Background="LightBlue" />
    ///     </picker:SfPicker.SelectionView>
    /// </picker:SfPicker>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code language="C#">
    /// SfPicker picker = new SfPicker();
    /// picker.SelectionView = new PickerSelectionView
    /// {
    ///     Background = new SolidColorBrush(Colors.LightBlue)
    /// };
    /// </code>
    /// </example>
    public Brush Background
    {
        get { return (Brush)GetValue(BackgroundProperty); }
        set { SetValue(BackgroundProperty, value); }
    }

    /// <summary>
    /// Gets or sets the stroke color of the selection view in SfPicker.
    /// </summary>
    /// <value>The default value of <see cref="PickerSelectionView.Stroke"/> is Colors.Transparent.</value>
    /// <example>
    /// The following example demonstrates how to set the stroke of the selection view.
    /// # [XAML](#tab/tabid-3)
    /// <code language="xaml">
    /// <![CDATA[
    /// <picker:SfPicker>
    ///     <picker:SfPicker.SelectionView>
    ///         <picker:PickerSelectionView Stroke="Blue" />
    ///     </picker:SfPicker.SelectionView>
    /// </picker:SfPicker>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code language="C#">
    /// SfPicker picker = new SfPicker();
    /// picker.SelectionView = new PickerSelectionView
    /// {
    ///     Stroke = Colors.Blue
    /// };
    /// </code>
    /// </example>
    public Color Stroke
    {
        get { return (Color)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    /// <summary>
    /// Gets or sets the corner radius of the selection view in SfPicker.
    /// </summary>
    /// <value>The default value of <see cref="PickerSelectionView.CornerRadius"/> is 20.</value>
    /// <example>
    /// The following example demonstrates how to set the corner radius of the selection view.
    /// # [XAML](#tab/tabid-5)
    /// <code language="xaml">
    /// <![CDATA[
    /// <picker:SfPicker>
    ///     <picker:SfPicker.SelectionView>
    ///         <picker:PickerSelectionView CornerRadius="10" />
    ///     </picker:SfPicker.SelectionView>
    /// </picker:SfPicker>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code language="C#">
    /// SfPicker picker = new SfPicker();
    /// picker.SelectionView = new PickerSelectionView
    /// {
    ///     CornerRadius = new CornerRadius(10)
    /// };
    /// </code>
    /// </example>
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    /// <summary>
    /// Gets or sets the padding value of the selection view in SfPicker.
    /// </summary>
    /// <value>The default value of <see cref="PickerSelectionView.Padding"/> is new Thickness(5, 2).</value>
    /// <example>
    /// The following example demonstrates how to set the padding of the selection view.
    /// # [XAML](#tab/tabid-7)
    /// <code language="xaml">
    /// <![CDATA[
    /// <picker:SfPicker>
    ///     <picker:SfPicker.SelectionView>
    ///         <picker:PickerSelectionView Padding="8,4" />
    ///     </picker:SfPicker.SelectionView>
    /// </picker:SfPicker>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-8)
    /// <code language="C#">
    /// SfPicker picker = new SfPicker();
    /// picker.SelectionView = new PickerSelectionView
    /// {
    ///     Padding = new Thickness(8, 4)
    /// };
    /// </code>
    /// </example>
    public Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    #endregion
}