using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class CircularChartArea : AreaBase, IChartArea
    {
        #region Fields
        ChartSeriesCollection? _series;
        readonly CircularPlotArea _circularPlotArea;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CircularChartArea(IChart chart)
        {
            BatchBegin();
            _circularPlotArea = new CircularPlotArea(this);
            _circularPlotArea.Chart = chart;
            AbsoluteLayout.SetLayoutBounds(_circularPlotArea, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_circularPlotArea, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(_circularPlotArea);
            AbsoluteLayout.SetLayoutBounds(chart.BehaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(chart.BehaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(chart.BehaviorLayout);
            BatchCommit();
        }
        #endregion

        #region Properties

        public ChartSeriesCollection? Series
        {
            get
            {
                return _series;
            }
            set
            {
                if (value != _series)
                {
                    _series = value;
                    _circularPlotArea.Series = value;
                }
            }
        }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();
        public override IPlotArea PlotArea => _circularPlotArea;

        #endregion

        #region Methods

        protected override void UpdateAreaCore()
        {
            if (_circularPlotArea.Chart is IChart chart)
            {
                chart.ResetTooltip();
                chart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(AreaBounds, _circularPlotArea.Chart.TitleHeight);
            }

            _circularPlotArea.UpdateVisibleSeries();
        }

        #endregion
    }
}
