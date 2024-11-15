using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// It's a collection class that holds ChartSeries.
    /// </summary>
    public class ChartSeriesCollection : ObservableCollection<ChartSeries>
    {
        #region Field

        readonly ReadOnlyObservableCollection<ChartSeries> _readOnlyItems;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSeriesCollection"/> class.
        /// </summary>
        public ChartSeriesCollection()
        {
            _readOnlyItems = new ReadOnlyObservableCollection<ChartSeries>(this);
        }

        #endregion

        #region Methods

        internal ReadOnlyObservableCollection<ChartSeries> GetVisibleSeries()
        {
            //TODO:Need to check the alternative solution.
            return new ReadOnlyObservableCollection<ChartSeries>(new ObservableCollection<ChartSeries>(from chartSeries in this
                                                                                                       where chartSeries.IsVisible
                                                                                                       select chartSeries));
        }

        internal ReadOnlyObservableCollection<ChartSeries> AsReadOnly()
        {
            return _readOnlyItems;
        }

        #endregion
    }
}