using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// This interface contains the legend elements.
    /// </summary>
    internal interface ILegend
    {
        /// <summary>
        ///  Gets or sets the ItemsSource for the legend.
        /// </summary>
        IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to bind the series visibility with its corresponding legend item in the legend.
        /// </summary>
        bool ToggleVisibility { get; set; }

        /// <summary>
        /// Gets or sets placement of the legend. 
        /// </summary>
        LegendPlacement Placement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Layout ItemsLayout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsVisible { get; set; }

        SfLegend? CreateLegendView()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        DataTemplate ItemTemplate { get; set; }

        Func<double> ItemsMaximumHeightRequest { get; set; }
    }

    /// <summary>
    /// Represents an interface that defines the elements of a legend item.
    /// </summary>
    public interface ILegendItem
    {
        /// <summary>
        /// Gets or sets the corresponding label for legend item.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the font family for the legend item label.
        /// </summary>
        string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font attributes for the legend item label.
        /// </summary>
        FontAttributes FontAttributes { get; set; }

        /// <summary>
        /// Gets or sets the corresponding icon color for legend item.
        /// </summary>
        Brush IconBrush { get; set; }

        /// <summary>
        /// Gets or sets the corresponding text color for legend item.
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        double IconWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        double IconHeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for the legend label text. 
        /// </summary>
        /// <value>This property takes float value.</value>
        float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin of the legend text.
        /// </summary>
        Thickness TextMargin { get; set; }

        /// <summary>
        /// Gets or sets the icon type in legend.
        /// </summary>
        ShapeType IconType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item is toggled or not.
        /// </summary>
        bool IsToggled { get; set; }

        /// <summary>
        /// Gets or sets the legend icon and text disable color when toggled.
        /// </summary>
        Brush DisableBrush { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the legend icon.
        /// </summary>
        bool IsIconVisible { get; set; }

        /// <summary>
        /// Gets the corresponding index for legend item.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the corresponding data point for series.
        /// </summary>
        object? Item { get; }
    }
}
