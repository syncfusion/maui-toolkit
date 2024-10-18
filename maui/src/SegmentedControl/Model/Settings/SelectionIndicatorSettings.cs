using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
    /// <summary>
    /// Provides a set of properties to customize the selection indicator in the <see cref="SfSegmentedControl"/>.
    /// The selection indicator is a strip used to indicate the selected index in the <see cref="SfSegmentedControl"/> View.
    /// </summary>
    public class SelectionIndicatorSettings : Element, IThemeElement
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> dependency property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SelectionIndicatorSettings), defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#6750A4")));

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SelectionIndicatorSettings), defaultValueCreator: bindable => Color.FromArgb("#FFFFFF"));

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> dependency property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SelectionIndicatorSettings), defaultValueCreator: bindable => Color.FromArgb("#6750A4"));

        /// <summary>
        /// Identifies the <see cref="SelectionIndicatorPlacement"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionIndicatorPlacement"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionIndicatorPlacementProperty =
            BindableProperty.Create(nameof(SelectionIndicatorPlacement), typeof(SelectionIndicatorPlacement), typeof(SelectionIndicatorSettings), defaultValueCreator: bindable => SelectionIndicatorPlacement.Fill);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeThickness"/> dependency property.
        /// </value>
        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SelectionIndicatorSettings), defaultValueCreator: bindable => 3d);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionIndicatorSettings"/> class.
        /// </summary>
        public SelectionIndicatorSettings()
        {
            ThemeElement.InitializeThemeResources(this, "SfSegmentedControlTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background brush for the selection indicator in the <see cref="SfSegmentedControl"/>.
        /// </summary>
        /// <remarks>
        /// It is applicable to only when the selection mode is set to <see cref="SelectionIndicatorPlacement.Fill"/>, and for border selection, the text color of the selected item is determined by the <see cref="SelectionIndicatorSettings.Background"/> property.
        /// </remarks>
        /// <value>
        /// The default value of <see cref="Background"/> is "new SolidColorBrush(Color.FromArgb("#6750A4"))".
        /// </value>
        /// <example>
        /// The below examples shows, how to set the <see cref="Background"/> property of <see cref="SelectionIndicatorSettings"/> in the <see cref="SfSegmentedControl"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <button:SfSegmentedControl x:Name="segmentedControl">
        ///    <button:SfSegmentedControl.SelectionIndicatorSettings>
        ///       <button:SelectionIndicatorSettings Background="Orange"/>
        ///    </button:SfSegmentedControl.SelectionIndicatorSettings>
        ///    <button:SfSegmentedControl.ItemsSource>
        ///        <x:Array Type="{x:Type x:String}">
        ///            <x:String>Day</x:String>
        ///            <x:String>Week</x:String>
        ///            <x:String>Month</x:String>
        ///            <x:String>Year</x:String>
        ///        </x:Array>
        ///    </button:SfSegmentedControl.ItemsSource>
        /// </button:SfSegmentedControl>
        /// ]]></code>
        ///  # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// segmentedControl.SelectionIndicatorSettings.Background = Brush.Orange;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text color for the selection indicator in the <see cref="SfSegmentedControl"/>.
        /// </summary>
        /// <remarks>
        /// It is not applicable to when the <see cref="SfSegmentItem.Text"/> is <see cref="string.Empty"/>.
        /// </remarks>
        /// <value>
        /// The default value of <see cref="TextColor"/> is <see cref="Colors.White"/>.
        /// </value>
        /// <example>
        /// The below examples shows, how to set the <see cref="TextColor"/> property of <see cref="SelectionIndicatorSettings"/> in the <see cref="SfSegmentedControl"/>.
        /// # [XAML](#tab/tabid-3)
        /// <code Lang="XAML"><![CDATA[
        /// <button:SfSegmentedControl x:Name="segmentedControl">
        ///    <button:SfSegmentedControl.SelectionIndicatorSettings>
        ///       <button:SelectionIndicatorSettings TextColor="Red"/>
        ///    </button:SfSegmentedControl.SelectionIndicatorSettings>
        ///    <button:SfSegmentedControl.ItemsSource>
        ///        <x:Array Type="{x:Type x:String}">
        ///            <x:String>Day</x:String>
        ///            <x:String>Week</x:String>
        ///            <x:String>Month</x:String>
        ///            <x:String>Year</x:String>
        ///        </x:Array>
        ///    </button:SfSegmentedControl.ItemsSource>
        /// </button:SfSegmentedControl>
        /// ]]></code>
        ///  # [C#](#tab/tabid-4)
        /// <code Lang="C#"><![CDATA[
        /// segmentedControl.SelectionIndicatorSettings.TextColor = Colors.Red;
        /// ]]></code>
        /// </example>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke color for the selection indicator in the <see cref="SfSegmentedControl"/>.
        /// </summary>
        /// <remarks>
        /// It is applicable to only when the selection mode is set to <see cref="SelectionIndicatorPlacement.Border"/>, <see cref="SelectionIndicatorPlacement.TopBorder"/>, or <see cref="SelectionIndicatorPlacement.BottomBorder"/>.
        /// </remarks>
        /// <value>
        /// The default value of <see cref="Stroke"/> is "Color.FromArgb("#6750A4")".
        /// </value>
        /// <example>
        /// The below examples shows, how to set the <see cref="Stroke"/> property of <see cref="SelectionIndicatorSettings"/> in the <see cref="SfSegmentedControl"/>.
        /// # [XAML](#tab/tabid-5)
        /// <code Lang="XAML"><![CDATA[
        /// <button:SfSegmentedControl x:Name="segmentedControl">
        ///    <button:SfSegmentedControl.SelectionIndicatorSettings>
        ///       <button:SelectionIndicatorSettings Stroke="Orange"/>
        ///    </button:SfSegmentedControl.SelectionIndicatorSettings>
        ///    <button:SfSegmentedControl.ItemsSource>
        ///        <x:Array Type="{x:Type x:String}">
        ///            <x:String>Day</x:String>
        ///            <x:String>Week</x:String>
        ///            <x:String>Month</x:String>
        ///            <x:String>Year</x:String>
        ///        </x:Array>
        ///    </button:SfSegmentedControl.ItemsSource>
        /// </button:SfSegmentedControl>
        /// ]]></code>
        ///  # [C#](#tab/tabid-6)
        /// <code Lang="C#"><![CDATA[
        /// segmentedControl.SelectionIndicatorSettings.Stroke = Colors.Orange;
        /// ]]></code>
        /// </example>
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection mode for the selection indicator in the <see cref="SfSegmentedControl"/>.
        /// </summary>
        /// <remarks>
        /// When the selection mode is set to <see cref="SelectionIndicatorPlacement.Fill"/>, the selected item's appearance is determined by the <see cref="SelectionIndicatorSettings.Background"/> property. However, when the selection mode is set to <see cref="SelectionIndicatorPlacement.Border"/>, <see cref="SelectionIndicatorPlacement.TopBorder"/>, or <see cref="SelectionIndicatorPlacement.BottomBorder"/>, the selected color is determined by the <see cref="SelectionIndicatorSettings.Stroke"/> and <see cref="SelectionIndicatorSettings.StrokeThickness"/> properties, and for border selection, the text color of the selected item is determined by the <see cref="SelectionIndicatorSettings.Background"/> property.
        /// </remarks>
        /// <value>
        /// The default value of <see cref="SelectionIndicatorPlacement"/> is <see cref="SelectionIndicatorPlacement.Fill"/>.
        /// </value>
        /// <example>
        /// The below examples shows, how to set the <see cref="SelectionIndicatorPlacement"/> property of <see cref="SelectionIndicatorSettings"/> in the <see cref="SfSegmentedControl"/>.
        /// # [XAML](#tab/tabid-7)
        /// <code Lang="XAML"><![CDATA[
        /// <button:SfSegmentedControl x:Name="segmentedControl">
        ///    <button:SfSegmentedControl.SelectionIndicatorSettings>
        ///       <button:SelectionIndicatorSettings SelectionIndicatorPlacement="Border"/>
        ///    </button:SfSegmentedControl.SelectionIndicatorSettings>
        ///    <button:SfSegmentedControl.ItemsSource>
        ///        <x:Array Type="{x:Type x:String}">
        ///            <x:String>Day</x:String>
        ///            <x:String>Week</x:String>
        ///            <x:String>Month</x:String>
        ///            <x:String>Year</x:String>
        ///        </x:Array>
        ///    </button:SfSegmentedControl.ItemsSource>
        /// </button:SfSegmentedControl>
        /// ]]></code>
        ///  # [C#](#tab/tabid-8)
        /// <code Lang="C#"><![CDATA[
        /// segmentedControl.SelectionIndicatorSettings.SelectionIndicatorPlacement = SelectionIndicatorPlacement.Border;
        /// ]]></code>
        /// </example>
        public SelectionIndicatorPlacement SelectionIndicatorPlacement
        {
            get { return (SelectionIndicatorPlacement)GetValue(SelectionIndicatorPlacementProperty); }
            set { SetValue(SelectionIndicatorPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness for the selection indicator in the <see cref="SfSegmentedControl"/>.
        /// </summary>
        /// <remarks>
        /// It is applicable to only when the selection mode is set to <see cref="SelectionIndicatorPlacement.Border"/>, <see cref="SelectionIndicatorPlacement.TopBorder"/>, or <see cref="SelectionIndicatorPlacement.BottomBorder"/>.
        /// </remarks>
        /// <value>
        /// The default value of <see cref="StrokeThickness"/> is 3.
        /// </value>
        /// <example>
        /// The below examples shows, how to set the <see cref="StrokeThickness"/> property of <see cref="SelectionIndicatorSettings"/> in the <see cref="SfSegmentedControl"/>.
        /// # [XAML](#tab/tabid-9)
        /// <code Lang="XAML"><![CDATA[
        /// <button:SfSegmentedControl x:Name="segmentedControl">
        ///    <button:SfSegmentedControl.SelectionIndicatorSettings>
        ///       <button:SelectionIndicatorSettings StrokeThickness="5"/>
        ///    </button:SfSegmentedControl.SelectionIndicatorSettings>
        ///    <button:SfSegmentedControl.ItemsSource>
        ///        <x:Array Type="{x:Type x:String}">
        ///            <x:String>Day</x:String>
        ///            <x:String>Week</x:String>
        ///            <x:String>Month</x:String>
        ///            <x:String>Year</x:String>
        ///        </x:Array>
        ///    </button:SfSegmentedControl.ItemsSource>
        /// </button:SfSegmentedControl>
        /// ]]></code>
        ///  # [C#](#tab/tabid-10)
        /// <code Lang="C#"><![CDATA[
        /// segmentedControl.SelectionIndicatorSettings.StrokeThickness = 5;
        /// ]]></code>
        /// </example>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Handles changes in the theme for individual controls.
        /// </summary>
        /// <param name="oldTheme">The old theme value.</param>
        /// <param name="newTheme">The new theme value.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// Handles changes in the common theme shared across multiple elements.
        /// </summary>
        /// <param name="oldTheme">The old theme value.</param>
        /// <param name="newTheme">The new theme value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion
    }
}