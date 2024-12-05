using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Custom calendar layout that used to measure and arrange the children by using <see cref="CalendarLayoutManager"/>.
    /// </summary>
    internal abstract class CalendarLayout : Layout
    {
        #region Internal Methods

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The size.</returns>
        internal abstract Size LayoutArrangeChildren(Rect bounds);

        #endregion

        #region Override Method

        /// <summary>
        /// Creates new layout manager.
        /// </summary>
        /// <returns>Returns calendar layout manager.</returns>
        protected override ILayoutManager CreateLayoutManager()
        {
            return new CalendarLayoutManager(this);
        }

        #endregion
    }

    /// <summary>
    /// Layout manager used to handle the measure and arrangement logic of the <see cref="CalendarLayout"/> children.
    /// </summary>
    internal class CalendarLayoutManager : LayoutManager
    {
        #region Fields

        readonly CalendarLayout _layout;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarLayoutManager"/> class.
        /// </summary>
        /// <param name="layout">The calendar layout instance.</param>
        public CalendarLayoutManager(CalendarLayout layout)
            : base(layout)
        {
            _layout = layout;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the calendar layout children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The size.</returns>
        public override Size ArrangeChildren(Rect bounds)
        {
            return _layout.LayoutArrangeChildren(bounds);
        }

        /// <summary>
        /// Method used to measure the calendar layout children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            return _layout.LayoutMeasure(widthConstraint, heightConstraint);
        }

        #endregion
    }
}
