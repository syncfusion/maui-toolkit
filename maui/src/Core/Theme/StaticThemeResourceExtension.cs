using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Themes
{
    /// <summary>
    /// Static Theme Resource Extension is a Markup Extension for simple key mapping in resource components.
    /// </summary>
    /// <example>
    /// The following example shows how to map a key with another key.
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    ///  <local:StaticThemeResource x:Key="BadgeSettingsBackground" ResourceKey="SyncfusionPrimaryLightColor"/>
    /// ]]></code>
    /// ***
    /// </example>

    [Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]

    [ContentProperty(nameof(ResourceKey))]

    public class StaticThemeResourceExtension : IMarkupExtension
    {
        /// <summary>
        /// Gets or sets the Resource Key value of the Markup Extension. This Resource Key is used to get the extended key's object.
        /// </summary>
        public string? ResourceKey { get; set; }

        /// <summary>
        /// Returns the object created for Resource Key from the Resource Dictionary. If the Resource Key is not found in the Resource Dictionary, an exception will be thrown.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourceKey == null || serviceProvider == null)
                return null;

            if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider rootObjectProvider)
            {
                if (rootObjectProvider.RootObject is ResourceDictionary themeResourceDictionary)
                {
                    var mergedDictionaries = themeResourceDictionary.MergedDictionaries ?? Enumerable.Empty<ResourceDictionary>();

                    foreach (var item in mergedDictionaries)
                    {
                        if (item.Keys.Contains(ResourceKey))
                        {
                            return item[ResourceKey];
                        }
                    }

                    if (themeResourceDictionary.Keys.Contains(ResourceKey))
                        return themeResourceDictionary[ResourceKey];
                    else
                        throw new KeyNotFoundException("The resource '" + ResourceKey + "' is not present in the dictionary.");
                }
            }

            return null;
        }
    }
}
