namespace Syncfusion.Maui.Toolkit.Themes
{
    /// <summary>
    /// Dark theme resource dictionary.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class DefaultTheme : SyncfusionThemeDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Themes.DefaultTheme"/> class.
        /// </summary>
        public DefaultTheme() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDark"></param>
        public DefaultTheme(bool isDark = false) : base(isDark)
        {
            InitializeComponent();
        }

    }

}