
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Themes
{
    /// <summary>
    /// The interface from which control's subviews/styles should be inherited.
    /// </summary>
    internal interface IThemeElement
    {
        /// <summary>
        /// This method will be called when a theme dictionary that contains the value for your control key is merged in application. 
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void OnControlThemeChanged(string oldTheme, string newTheme);

        /// <summary>
        /// This method will be called when users merge a theme dictionary that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void OnCommonThemeChanged(string oldTheme, string newTheme);
    }

    /// <summary>
    /// The interface from which control should be inherited.
    /// </summary>
    internal interface IParentThemeElement : IThemeElement
    {
        /// <summary>
        /// This method is declared only in IParentThemeElement and you need to implement this method only in main control.
        /// </summary>
        /// <returns>ResourceDictionary</returns>
        ResourceDictionary GetThemeDictionary();
    }
}