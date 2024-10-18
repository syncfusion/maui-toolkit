using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Provides a collection of default brushes for chart visualization. This static class contains a list of predefined color brushes
    /// used to enhance the appearance of chart segments.
    /// </summary>
    internal static class ChartColorModel
    {
        /// <summary>
        /// A collection of default brushes for chart segments.
        /// </summary>
        public static List<Brush> DefaultBrushes = new List<Brush>
                {
                new SolidColorBrush(Color.FromArgb("#00bdae")),
                    /*#00bdae*/
                   new SolidColorBrush(Color.FromArgb("#404041")),
                    /*#404041*/
                   new SolidColorBrush (Color.FromArgb("#357cd2")),
                    /*#357cd2*/
                   new SolidColorBrush( Color.FromArgb("#e56590")),
                    /*#e56590*/
                    new SolidColorBrush(Color.FromArgb("#f8b883")),
                    /*#f8b883*/				 	    
                    new SolidColorBrush(Color.FromArgb("#70ad47")),
                    /*#70ad47*/				 	    
                    new SolidColorBrush(Color.FromArgb("#dd8abd")),
                    /*#dd8abd*/				 	    
                    new SolidColorBrush(Color.FromArgb("#7f84e8")),
                    /*#7f84e8*/				 	    
                    new SolidColorBrush(Color.FromArgb("#7bb4eb")),
                    /*#7bb4eb*/				 	    
                    new SolidColorBrush(Color.FromArgb("#ea7a57"))
                    /*#ea7a57*/
                };
    }
}
