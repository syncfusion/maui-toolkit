using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface IDatapointSelectionDependent
    {
        #region Properties

        DataPointSelectionBehavior? SelectionBehavior { get; }

        ObservableCollection<ChartSegment> Segments { get; }

        Rect AreaBounds { get; }

        #endregion

        #region Methods

        void UpdateSelectedItem(int index);

        void SetFillColor(ChartSegment segment);

        void Invalidate();

        void UpdateLegendIconColor(ChartSelectionBehavior sender, int index);

        #endregion
    }
}

