namespace Syncfusion.Maui.Toolkit.Themes
{

    /// <summary>
    /// Represents a collection of theme resources for Syncfusion controls.
    /// </summary>
    /// <remarks>
    /// This class is used to include various themes for Syncfusion controls. Themes can be added to this ResourceDictionary to apply consistent styling across the application.
    /// </remarks>
    public class SyncfusionThemeResourceDictionary : ResourceDictionary
    {

        private SfVisuals sfVisualTheme = SfVisuals.MaterialLight;

        /// <summary>
        /// Gets or sets the visual theme for the Syncfusion control.
        /// </summary>
        /// <value>
        /// Accepts an <see cref="SfVisuals"/> value, with the default being <see cref="SfVisuals.MaterialLight"/>.
        /// </value>
        public SfVisuals VisualTheme
        {
            get
            {
                return sfVisualTheme;
            }
            set
            {
                sfVisualTheme = value;

                this.UpdateVisualTheme();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncfusionThemeResourceDictionary"/> class.
        /// </summary>
        public SyncfusionThemeResourceDictionary()
        {
            this.UpdateDefaultTheme();
        }

        private void UpdateVisualTheme()
        {

            this.MergedDictionaries.Clear();
            if (this.VisualTheme == SfVisuals.MaterialLight)
            {
                this.UpdateDefaultTheme();
            }
            else if (this.VisualTheme == SfVisuals.MaterialDark)
            {
                this.UpdateDefaultTheme(true);
            }
        }

        private void UpdateDefaultTheme(bool isDark = false)
        {
            this.MergedDictionaries.Clear();
            this.MergedDictionaries.Add(new DefaultTheme(isDark));
        }

    }

    /// <summary>
    /// Specifies the visual theme types in Syncfusion.
    /// </summary>
    public enum SfVisuals
    {
        /// <summary>
        /// 
        /// </summary>
        MaterialLight,

        /// <summary>
        /// 
        /// </summary>
        MaterialDark

    }
}
