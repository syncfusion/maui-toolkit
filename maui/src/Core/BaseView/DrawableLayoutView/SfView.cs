using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Semantics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;

namespace Syncfusion.Maui.Toolkit
{

    /// <summary>
    /// Represents an abstract base class for custom views. 
    /// This class provides common functionality and serves as a foundation for building custom drawable layouts, and visual tree elements.
    /// </summary>
    public abstract class SfView : View, IDrawableLayout, IVisualTreeElement, ISemanticsProvider
    {
        #region fields

        private readonly ILayoutManager layoutManager;

        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

        /// <summary>
        /// This property is used to ignore the safe area.
        /// </summary>
        private bool ignoreSafeArea = false;

		private bool clipToBounds = true;

		/// <summary>
		/// The field indicates whether it is layout based control.
		/// </summary>
		private bool isLayoutControl = false;

        readonly List<IView> children = [];

		#endregion

		#region Bindable Properties
		
		/// <summary>
		/// Identifies the <see cref="Padding"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Padding"/> bindable property.
		/// </value>
		public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SfView), new Thickness(0), propertyChanged: OnPaddingPropertyChanged);
		
		#endregion

		#region properties

		private SfViewHandler? LayoutHandler => Handler as SfViewHandler;


        internal DrawingOrder DrawingOrder
        {
            get
            {
                return drawingOrder;
            }
            set
            {
                drawingOrder = value;
                LayoutHandler?.SetDrawingOrder(value);
            }
        }

#if WINDOWS
        /// <summary>
        /// If the SfView drawing canvas size exceeds MaximumBitmapSizeInPixels when adding more items,
        /// an OS limitation with CanvasImageSource size (refer: https://github.com/dotnet/maui/issues/3785)
        /// requires restricting the draw function when semantics or accessibility are used on SfView. This prevents OS limitation issues on the Windows platform.
        /// For accessibility, SfView should be enabled with AboveContentWithTouch to add a native user control and override the AutomationPeer.
        /// Virtualization isn't possible because automation peers must be added initially to access scrollable content. 
        /// Since accessibility highlights are managed by native framework automation peers, SfView canvas drawing is unnecessary.
        /// </summary>
        internal bool IsCanvasNeeded { get; set; } = true;
#endif

        /// <summary>
        /// This property is used to ignore the safe area.
        /// </summary>
        internal bool IgnoreSafeArea
        {
            get
            {
                return ignoreSafeArea;
            }
            set
            {
                ignoreSafeArea = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether it is layout based control.
		/// </summary>
		internal bool IsLayoutControl
		{
			get
			{
				return this.isLayoutControl;
			}
			set
			{
				this.isLayoutControl = value;
			}
		}

		/// <summary>
		/// Gets the collection of child views contained within this view.
		/// </summary>
		public IList<IView> Children => this;

        /// <summary>
        /// Gets or sets a value indicating whether the content of this view is clipped to its bounds.
        /// </summary>
        public bool ClipToBounds
        {
            get
            {
                return clipToBounds;
            }
            set
            {
                clipToBounds = value;
                LayoutHandler?.UpdateClipToBounds(value);
            }
        }

        /// <summary>
        /// Gets or sets the padding for the view.
        /// </summary>
        public Thickness Padding
        {
			get => (Thickness)GetValue(PaddingProperty);
			set => SetValue(PaddingProperty, value);
		}

		/// <exclude/>
		IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren() => Children.Cast<IVisualTreeElement>().ToList().AsReadOnly();

		/// <exclude/>
		bool Microsoft.Maui.ILayout.ClipsToBounds => ClipToBounds;

		/// <exclude/>
		int ICollection<IView>.Count => children.Count;

		/// <exclude/>
		bool ICollection<IView>.IsReadOnly => ((ICollection<IView>)children).IsReadOnly;

		/// <exclude/>
		bool ISafeAreaView.IgnoreSafeArea => IgnoreSafeArea;

		/// <exclude/>
		Thickness IPadding.Padding => Padding;

		/// <exclude/>
		DrawingOrder IDrawableLayout.DrawingOrder { get => DrawingOrder; set => DrawingOrder = value; }

		/// <exclude/>
		IView IList<IView>.this[int index] { get => children[index]; set => children[index] = value; }

        #endregion

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfView"/> class.
        /// </summary>
        public SfView()
        {
            layoutManager = new DrawableLayoutManager(this);
        }

		#endregion

		#region OnPropertyChanged Methods
		
		/// <summary>
		/// Invoked whenever the <see cref="PaddingProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfView view)
			{
				view.MeasureContent(view.Width, view.Height);
				view.ArrangeContent(view.Bounds);
				view.InvalidateMeasure();
			}
		}
		
		#endregion

		#region virtual methods

		/// <summary>
		/// Called when the control needs to be drawn.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw the control.</param>
		/// <param name="dirtyRect">The rectangle representing the area that needs to be drawn.</param>
		/// <exclude/>
		protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
        }

		/// <summary>
		/// Measures the content of the control.
		/// </summary>
		/// <param name="widthConstraint">The maximum width available for the content.</param>
		/// <param name="heightConstraint">The maximum height available for the content.</param>
        /// <returns>The measured size of the content.</returns>
		/// <exclude/>
		protected virtual Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            return layoutManager.Measure(widthConstraint, heightConstraint);
        }

		/// <summary>
		/// Arranges the content of the control within the specified bounds.
		/// </summary>
		/// <param name="bounds">The rectangular area within which the content should be arranged.</param>
		/// <returns>The final size of the arranged content.</returns>
		/// <exclude/>
		protected virtual Size ArrangeContent(Rect bounds)
        {
            return layoutManager.ArrangeChildren(bounds);
        }

		#endregion

		#region protected methods

		/// <exclude/> 
		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach (var item in children)
            {
                if (item is BindableObject child)
                {
                    UpdateBindingContextToChild(child);
                }
            }
        }

		#endregion

		#region override methods

		/// <exclude/> 
		protected sealed override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

		/// <exclude/> 
		protected sealed override Size ArrangeOverride(Rect bounds)
        {
            return base.ArrangeOverride(bounds);
        }

		/// <exclude/> 
		protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                LayoutHandler?.SetDrawingOrder(DrawingOrder);
                LayoutHandler?.UpdateClipToBounds(ClipToBounds);
                LayoutHandler?.Invalidate();
            }
        }

        #endregion

        #region internal methods

        internal void Add(View view)
        {
            ((Microsoft.Maui.ILayout)this).Add(view);
        }

        private void UpdateBindingContextToChild(BindableObject view)
        {
            SetInheritedBindingContext(view, BindingContext);
        }

        internal void Remove(View view)
        {
            ((Microsoft.Maui.ILayout)this).Remove(view);
        }

        internal void Insert(int index, View view)
        {
            ((Microsoft.Maui.ILayout)this).Insert(index, view);
        }

        internal void Clear()
        {
            ((Microsoft.Maui.ILayout)this).Clear();
        }

        internal void InvalidateDrawable()
        {
            LayoutHandler?.Invalidate();
        }

		#endregion

		#region private methods

		/// <summary>
		/// Invalidates the drawable layout.
		/// </summary>
		/// <exclude/>
		void IDrawableLayout.InvalidateDrawable()
        {
            InvalidateDrawable();
        }

		/// <exclude/>
		Size Microsoft.Maui.ILayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return MeasureContent(widthConstraint, heightConstraint);
        }

		/// <exclude/>
		Size Microsoft.Maui.ILayout.CrossPlatformArrange(Rect bounds)
        {
            return ArrangeContent(bounds);
        }

		/// <summary>
		/// Determines the index of a specific child view in the list.
		/// </summary>
		/// <param name="child">The child view to locate in the list.</param>
		/// <returns>The index of the child view if found in the list.</returns>
		int IList<IView>.IndexOf(IView child)
        {
            return children.IndexOf(child);
        }

		/// <summary>
		/// Inserts a child view into the list at the specified index.
		/// </summary>
		/// <param name="index">The index at which the child view should be inserted.</param>
		/// <param name="child">The child view to insert into the list.</param>
		void IList<IView>.Insert(int index, IView child)
        {
            children.Insert(index, child);
            LayoutHandler?.Insert(index, child);
            if (child is Element element)
            {
                OnChildAdded(element);
            }
        }

		/// <summary>
		/// Removes the child view at the specified index from the list.
		/// </summary>
		/// <param name="index">The index of the child view to remove.</param>
		void IList<IView>.RemoveAt(int index)
        {
            var child = children[index];
            LayoutHandler?.Remove(children[index]);
            children.RemoveAt(index);

            if (child is Element element)
            {
                OnChildRemoved(element, index);
            }
        }

		/// <summary>
		/// Adds a child view to the collection.
		/// </summary>
		/// <param name="child">The child view to add to the collection.</param>
		void ICollection<IView>.Add(IView child)
        {
            children.Add(child);
            LayoutHandler?.Add(child);
            if (child is Element element)
            {
                OnChildAdded(element);
            }
        }

		/// <summary>
		/// Removes all child views from the collection.
		/// </summary>
		void ICollection<IView>.Clear()
        {
            children.Clear();
            LayoutHandler?.Clear();
        }

		/// <summary>
		/// Determines whether the collection contains a specific child view.
		/// </summary>
		/// <param name="child">The child view to locate in the collection.</param>
		/// <returns><c>true</c> if the child view is found in the collection; otherwise, <c>false</c>.</returns>
		bool ICollection<IView>.Contains(IView child)
        {
            return children.Contains(child);
        }

		/// <summary>
		/// Copies the elements of the collection to an array, starting at a particular array index.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection.</param>
		/// <param name="arrayIndex">The index in the array at which copying begins.</param>
		void ICollection<IView>.CopyTo(IView[] array, int arrayIndex)
        {
            children.CopyTo(array, arrayIndex);
        }

		/// <summary>
		/// Removes the first occurrence of a specific child view from the collection.
		/// </summary>
		/// <param name="child">The child view to remove from the collection.</param>
		/// <returns><c>true</c> if the child view was successfully removed from the collection; otherwise, <c>false</c>.</returns>
		bool ICollection<IView>.Remove(IView child)
        {
            var index = children.IndexOf(child);
            LayoutHandler?.Remove(child);
            var childRemoved = children.Remove(child);
            if (child is Element element)
            {
                OnChildRemoved(element, index);
            }
            return childRemoved;
        }

		/// <summary>
		/// Returns an enumerator that iterates through the collection of child views.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection of child views.</returns>
		IEnumerator<IView> IEnumerable<IView>.GetEnumerator()
        {
            return children.GetEnumerator();
        }

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

		/// <exclude/>
		void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
        {
            OnDraw(canvas, dirtyRect);
        }

		/// <summary>
		/// Gets the layout bounds of the specified view.
		/// </summary>
		/// <param name="view">The view for which to get the layout bounds.</param>
		/// <returns>A <see cref="Rect"/> that represents the layout bounds of the specified view.</returns>
		Rect IAbsoluteLayout.GetLayoutBounds(IView view)
        {
            BindableObject bindable = (BindableObject)view;
            return (Rect)bindable.GetValue(AbsoluteLayout.LayoutBoundsProperty);
        }

		/// <summary>
		/// Gets the layout flags of the specified view.
		/// </summary>
		/// <param name="view">The view for which to get the layout flags.</param>
		/// <returns>The <see cref="AbsoluteLayoutFlags"/> that represent the layout flags of the specified view.</returns>
		AbsoluteLayoutFlags IAbsoluteLayout.GetLayoutFlags(IView view)
        {
            BindableObject bindable = (BindableObject)view;
            return (AbsoluteLayoutFlags)bindable.GetValue(AbsoluteLayout.LayoutFlagsProperty);
        }

        /// <summary>
        /// Return the semantics nodes for the view.
        /// </summary>
        /// <param name="width">The view width.</param>
        /// <param name="height">The view height.</param>
        /// <returns>The semantics nodes of the view.</returns>
        List<SemanticsNode>? ISemanticsProvider.GetSemanticsNodes(double width, double height)
        {
            return GetSemanticsNodesCore(width, height);
        }

        /// <summary>
        /// Used to scroll the view based on the node position while the view inside the scroll view.
        /// </summary>
        /// <param name="node">Current navigated semantics node.</param>
        void ISemanticsProvider.ScrollTo(SemanticsNode node)
        {
            ScrollToCore(node);
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            LayoutHandler?.InvalidateSemantics();
        }

        /// <summary>
        /// Return the semantics nodes for the view.
        /// </summary>
        /// <param name="width">The view width.</param>
        /// <param name="height">The view height.</param>
        /// <returns>The semantics nodes of the view.</returns>
		/// <exclude/>
		protected virtual List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            return null;
        }

        /// <summary>
        /// Used to scroll the view based on the node position while the view inside the scroll view.
        /// </summary>
        /// <param name="node">Current navigated semantics node.</param>
        protected virtual void ScrollToCore(SemanticsNode node)
        {

        }
        #endregion
    }
}