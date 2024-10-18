using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface IChart
    {
        #region Properties

        //Todo: Need to recheck BackgroundColor property for contrast color.
        Color BackgroundColor { get; }

        ChartTooltipBehavior? ActualTooltipBehavior { get; set; }

        SfTooltip? TooltipView { get; set; }

        AbsoluteLayout BehaviorLayout { get; }

        ChartLegend Legend { get; set; }

        ChartThemeLegendLabelStyle LegendLabelStyle { get; }

        public IArea Area { get; }

        double TitleHeight { get; }

        Rect ActualSeriesClipRect { get; set; }

        bool IsRequiredDataLabelsMeasure { get; set; }

        #endregion

        #region Methods

        Brush? GetSelectionBrush(ChartSeries series);

        TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y);

        void ResetTooltip()
        {
            ActualTooltipBehavior?.Hide();
        }

        #endregion
    }

    internal interface IChartLegend : ILegend
    {
        #region Properties

        ChartLegendLabelStyle LabelStyle { get; set; }

        #endregion

        #region Methods

        void OnLegendItemCreated(ILegendItem item);

        #endregion
    }
}
