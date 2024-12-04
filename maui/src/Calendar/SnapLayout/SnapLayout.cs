using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which contains information of scroll view in the scheduler.
    /// </summary>
    internal abstract class SnapLayout : Layout
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ILayoutManager CreateLayoutManager()
        {
            return new SnapLayoutManager(this);
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// It is triggered from <see cref="SnapLayoutManager.Measure(double, double)"/>.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// It is triggered from <see cref="SnapLayoutManager.ArrangeChildren(Rect)"/>.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The size.</returns>
        internal abstract Size LayoutArrangeChildren(Rect bounds);

#if __ANDROID__
        /// <summary>
        ///  Return false then pass the touch to its children.
        ///  Return true then it holds the touch to its own.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">The touch action.</param>
        /// <returns>True, If action is performed with condition or else false. </returns>
        internal abstract bool OnInterceptTouchEvent(Point touchPoint, string action);

        /// <summary>
        ///  Return false then pass the touch to its parent.
        ///  Return true then it holds the touch to its own.
        ///  Its will triggered from intercept touch event method.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">The touch action.</param>
        /// <returns>True, If action is performed with condition or else false. </returns>
        internal abstract bool? OnDisAllowInterceptTouchEvent(Point touchPoint, string action);

        /// <summary>
        /// Used to handle the swiping action for snap layout.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">Touch action.</param>
        /// <param name="velocity">The velocity.</param>
        internal abstract void OnHandleTouch(Point touchPoint, string action, Point velocity);
#endif
    }

    /// <summary>
    /// Layout manager used to handle the measure and arrangement logic of the<see cref="SnapLayout"/> children.
    /// </summary>
    internal class SnapLayoutManager : LayoutManager
    {
        /// <summary>
        /// The <see cref="SnapLayout"/> instance used to trigger arrange and measure.
        /// </summary>
        readonly SnapLayout _layout;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapLayout"/> class.
        /// </summary>
        /// <param name="layout">The scheduler layout instance.</param>
        internal SnapLayoutManager(SnapLayout layout)
            : base(layout)
        {
            _layout = layout;
        }

        /// <summary>
        /// Method used to measure the scheduler layout children based on width and height value.
        /// It triggers from <see cref="SnapLayout.LayoutMeasure(double, double)"/>.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            return _layout.LayoutMeasure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Method used to arrange the scheduler layout children with in the bounds.
        /// It triggers <see cref="SnapLayout.LayoutArrangeChildren(Rect)"/>.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The size.</returns>
        public override Size ArrangeChildren(Rect bounds)
        {
            return _layout.LayoutArrangeChildren(bounds);
        }
    }
}
