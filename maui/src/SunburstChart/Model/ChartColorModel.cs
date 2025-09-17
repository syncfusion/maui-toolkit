using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Provides the default color palette for the Sunburst chart.
    /// </summary>
    internal static class ChartColorModel
    {
        /// <summary>
        /// Gets the collection of default brushes used for chart elements.
        /// </summary>
        public static readonly List<Brush> DefaultBrushes = new List<Brush>
        {
            new SolidColorBrush(Color.FromArgb("#2196F3")),
            /*#00bdae*/
            new SolidColorBrush(Color.FromArgb("#0DB496")),
            /*#404041*/
            new SolidColorBrush (Color.FromArgb("#F4890B")),
            /*#357cd2*/
            new SolidColorBrush( Color.FromArgb("#9215F3")),
            /*#e56590*/
            new SolidColorBrush(Color.FromArgb("#CDB509")),
            /*#f8b883*/				 	    
            new SolidColorBrush(Color.FromArgb("#ED1B80")),
            /*#70ad47*/				 	    
            new SolidColorBrush(Color.FromArgb("#25E739")),
            /*#dd8abd*/				 	    
            new SolidColorBrush(Color.FromArgb("#00AEE0")),
            /*#7f84e8*/				 	    
            new SolidColorBrush(Color.FromArgb("#FF4E4E")),
            /*#7bb4eb*/				 	    
            new SolidColorBrush(Color.FromArgb("#05BB3D"))
            /*#ea7a57*/
        };
    }
}
