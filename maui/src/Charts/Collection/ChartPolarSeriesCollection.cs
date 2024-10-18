using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// It's a collection class that holds PolarSeries.
    /// </summary>
    public class ChartPolarSeriesCollection : ObservableCollection<PolarSeries>
    {
        #region Fields

        readonly ReadOnlyObservableCollection<PolarSeries> _readOnlyItems;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPolarSeriesCollection"/> class.
        /// </summary>
        public ChartPolarSeriesCollection()
        {
            _readOnlyItems = new ReadOnlyObservableCollection<PolarSeries>(this);
        }

        #endregion

        #region Methods

        internal ReadOnlyObservableCollection<ChartSeries>? GetVisibleSeries()
        {
            return new ReadOnlyObservableCollection<ChartSeries>(new ObservableCollection<ChartSeries>(from chartSeries in this
                                                                                                       where chartSeries.IsVisible
                                                                                                       select chartSeries));
        }

        internal ReadOnlyObservableCollection<PolarSeries> AsReadOnly()
        {
            return _readOnlyItems;
        }

        #endregion
    }
}