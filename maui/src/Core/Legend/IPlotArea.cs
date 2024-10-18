using Microsoft.Maui.Graphics;
using System;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal interface IPlotArea
    {
        public ILegend? Legend { get; set; }

        public ReadOnlyObservableCollection<ILegendItem> LegendItems { get; }

        public Rect PlotAreaBounds { get; set; }

        public bool ShouldPopulateLegendItems { get; set; }

        public LegendHandler LegendItemToggleHandler
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<LegendItemEventArgs> LegendItemToggled
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<EventArgs> LegendItemsUpdated
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateLegendItems()
        {

        }
    }
}
