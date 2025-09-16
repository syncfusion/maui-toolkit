using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    internal class SunburstSegmentCollection : ObservableCollection<SunburstSegment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstSegmentCollection"/> class.
        /// </summary>
        public SunburstSegmentCollection()
        {
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="SunburstItem"/> objects.
    /// </summary>
    internal class SunburstItems : ObservableCollection<SunburstItem>
    {
        public SunburstItems()
        {
        }
    }
}
