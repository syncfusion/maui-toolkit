using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface ISBSDependent
    {
        #region Properties

        double Spacing { get; set; }

        double Width { get; set; }

        CornerRadius CornerRadius { get; set; }

        #endregion
    }

    internal interface IDrawCustomLegendIcon
    {
        #region Methods

        void DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState);

        #endregion
    }
}
