
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.ComponentModel.Design;

namespace Syncfusion.Maui.Toolkit.Themes
{
    /// <summary>
    /// ThemeDictionary class for Syncfusion in which controls themes are to be included.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class SyncfusionThemeDictionary : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Themes.SyncfusionThemeDictionary"/> class.
        /// </summary>
        public SyncfusionThemeDictionary()
        {
            this.InitializeElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDark"></param>
        public SyncfusionThemeDictionary(bool isDark = false)
        {
            if (isDark)
            {
                this.MergedDictionaries.Add(new DarkThemeColors());
            }
            else
            {
                this.MergedDictionaries.Add(new LightThemeColors());
            }

            this.InitializeElement();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeElement()
        {
            InitializeComponent();

            ThemeElement.AddStyleDictionary(this);
        }
    }
}