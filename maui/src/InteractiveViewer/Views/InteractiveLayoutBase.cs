namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Microsoft.Maui.Layouts;

    /// <summary>
    /// Represents a layout that measures its children using the available size and arranges them to fill the layout bounds.
    /// </summary>
    internal class InteractiveStackLayout : InteractiveBaseLayout
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveStackLayout"/> class.
        /// </summary>
        internal InteractiveStackLayout()
        {
#if NET10_0_OR_GREATER
            this.SafeAreaEdges = SafeAreaEdges.None;
#else
            this.IgnoreSafeArea = true;
#endif
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Arranges all children within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds used to arrange the child elements.</param>
        /// <returns>The arranged size of the layout.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            foreach (var child in this.Children)
            {
                child.Arrange(bounds);
            }

            return bounds.Size;
        }

        /// <summary>
        /// Measures the layout using the specified size constraints.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">The available height.</param>
        /// <returns>The measured size of the layout.</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            // Determine the actual width and height based on the provided constraints or the minimum and maximum request size.
            double width = double.IsFinite(widthConstraint) ? widthConstraint : this.MinimumWidthRequest;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : this.MinimumHeightRequest;

            foreach (View child in this.Children)
            {
                child.Measure(width, height);
            }

            return new Size(width, height);
        }

        #endregion
    }

    /// <summary>
    /// Represents the base layout that delegates measure and arrange operations to a custom layout manager.
    /// </summary>
    internal abstract class InteractiveBaseLayout : Layout
    {
        #region Override methods

        /// <summary>
        /// Measures the layout using the specified constraints.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">The available height.</param>
        /// <returns>The measured size of the layout.</returns>
        internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Arranges the layout within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds used to arrange the layout.</param>
        /// <returns>The arranged size of the layout.</returns>
        internal abstract Size LayoutArrangeChildren(Rect bounds);

        /// <summary>
        /// Creates the layout manager for this layout.
        /// </summary>
        /// <returns>Returns the layout manager.</returns>
        protected override ILayoutManager CreateLayoutManager()
        {
            return new InteractiveLayoutManager(this);
        }

        #endregion
    }

    /// <summary>
    /// Represents a layout manager that handles the measure and arrange operations of an <see cref="InteractiveBaseLayout"/>.
    /// </summary>
    internal class InteractiveLayoutManager : LayoutManager
    {
        #region Fields

        /// <summary>
        /// The associated interactive layout.
        /// </summary>
        private InteractiveBaseLayout layout;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveLayoutManager"/> class.
        /// </summary>
        /// <param name="layout">The interactive layout instance.</param>
        internal InteractiveLayoutManager(InteractiveBaseLayout layout)
            : base(layout)
        {
            this.layout = layout;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Measures the layout using the specified constraints.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">The available height.</param>
        /// <returns>The measured size of the layout.</returns>
        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            return this.layout.LayoutMeasure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Arranges the layout within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds used to arrange the layout.</param>
        /// <returns>The arranged size of the layout.</returns>
        public override Size ArrangeChildren(Rect bounds)
        {
#if IOS || MACCATALYST
            // To avoid the crash when the bounds size is zero, we are returning default size.
            if (bounds.Size.IsZero)
            {
                return default;
            }
#endif
            return this.layout.LayoutArrangeChildren(bounds);
        }

        #endregion
    }
}