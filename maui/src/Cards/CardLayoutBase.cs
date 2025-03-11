using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Custom card layout that used to measure and arrange the children by using <see cref="CardLayoutManager"/>.
    /// </summary>
    public abstract class CardLayout : Layout
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
        /// <returns>Returns card layout manager.</returns>
        protected override ILayoutManager CreateLayoutManager()
        {
            return new CardLayoutManager(this);
        }

        #endregion
    }

    /// <summary>
    /// Layout manager used to handle the measure and arrangement logic of the <see cref="CardLayout"/> children.
    /// </summary>
    internal class CardLayoutManager : LayoutManager
    {
        #region Fields

        readonly CardLayout _layout;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CardLayoutManager"/> class.
        /// </summary>
        /// <param name="layout">The card layout instance.</param>
        public CardLayoutManager(CardLayout layout)
            : base(layout)
        {
            _layout = layout;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the card layout children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The size.</returns>
        public override Size ArrangeChildren(Rect bounds)
        {
            return _layout.LayoutArrangeChildren(bounds);
        }

        /// <summary>
        /// Method used to measure the card layout children based on width and height value.
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
