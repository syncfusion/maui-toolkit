namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Theme resource dictionary for the <see cref="SfGridSplitter"/> control.
    /// Maps the control-level bindable properties (separator background,
    /// resize icon color, expand/collapse icon color) to the
    /// <see cref="Syncfusion.Maui.Toolkit.Themes.SyncfusionThemeDictionary"/>
    /// keys defined in <c>DefaultTheme.xaml</c>, <c>LightThemeColors.xaml</c>,
    /// and <c>DarkThemeColors.xaml</c>.
    /// </summary>
    /// <remarks>
    /// The dictionary is loaded by <see cref="SfGridSplitter"/> via
    /// <see cref="Syncfusion.Maui.Toolkit.Themes.IParentThemeElement.GetThemeDictionary"/>
    /// when the <c>CommonTheme</c> or <c>SfGridSplitterTheme</c> dynamic
    /// resource is merged into the application. The merged dictionary
    /// causes the SetDynamicResource calls in
    /// the constructor to re-resolve the values to the active theme
    /// (light / dark), so the user does not have to set the colors
    /// manually to follow the system theme.
    /// <para>
    /// The hover-state colors of the separator strip and the floating
    /// expand/collapse buttons are looked up directly from the theme
    /// dictionary inside <c>SeparatorView</c> (the canvas drawing API
    /// does not consume bindable properties — it reads colors from the
    /// platform application resource dictionary at draw time).
    /// </para>
    /// </remarks>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SfGridSplitterStyles : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SfGridSplitterStyles"/> class.
        /// </summary>
        public SfGridSplitterStyles()
        {
            InitializeComponent();
        }
    }
}
