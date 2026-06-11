using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to hold the view changed event details.
    /// </summary>
    public class CalendarViewChangedEventArgs : EventArgs
    {
        #region Fields

        static readonly ReadOnlyCollection<DateTime> s_emptyDates = new List<DateTime>(0).AsReadOnly();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the visible dates that visible on current view.
        /// </summary>
        public ReadOnlyCollection<DateTime> NewVisibleDates { get; internal set; } = s_emptyDates;

        /// <summary>
        /// Gets the visible dates that visible on previous view.
        /// </summary>
        public ReadOnlyCollection<DateTime> OldVisibleDates { get; internal set; } = s_emptyDates;

        /// <summary>
        /// Gets the new calendar view.
        /// </summary>
        public CalendarView NewView { get; internal set; }

        /// <summary>
        /// Gets the old calendar view.
        /// </summary>
        public CalendarView OldView { get; internal set; }

        #endregion
    }
}