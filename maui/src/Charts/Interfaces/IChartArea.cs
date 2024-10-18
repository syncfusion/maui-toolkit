using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface IChartArea
    {
        #region Properties

        ChartSeriesCollection? Series { get; set; }

        ReadOnlyObservableCollection<ChartSeries>? VisibleSeries { get; }

        #endregion

        #region Methods

        void UpdateVisibleSeries()
        {

        }

        #endregion
    }

    internal interface IChartPlotArea : IPlotArea
    {
        #region Properties

        SfDrawableView DataLabelView { get; }

        #endregion

        #region Methods

        void UpdateLegendLabelStyle(ILegendItem legendItem, ChartLegendLabelStyle? style)
        {
            if (style != null)
            {
                legendItem.TextColor = style.TextColor;
                legendItem.FontSize = (float)style.FontSize;
                legendItem.DisableBrush = style.DisableBrush;
                legendItem.TextMargin = style.Margin;
                legendItem.FontAttributes = style.FontAttributes;
                legendItem.FontFamily = style.FontFamily;
            }
        }

        void UpdateItemsLabelStyle()
        {
            if (Legend is IChartLegend legend)
            {
                foreach (var item in LegendItems)
                {
                    UpdateLegendLabelStyle(item, legend.LabelStyle);
                }
            }
        }

        #endregion
    }
}