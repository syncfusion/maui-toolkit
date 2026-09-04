namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    using Syncfusion.Maui.Toolkit.Internals;
    using Syncfusion.Maui.Toolkit.Themes;
    using SfView = Syncfusion.Maui.Toolkit.SfView;

    /// <summary>
    /// Represents a control that enables users to interact with content through zooming and panning operations.
    /// The <see cref="SfInteractiveViewer"/> control can host any .NET MAUI view and provides an intuitive way to view, navigate, and explore content such as images, graphics, diagrams, and custom controls.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public partial class SfInteractiveViewer : SfView, IInteractiveViewerInfo, IParentThemeElement
    {
        #region Fields

        /// <summary>
        /// Holds the interactive scroll view that manages zoom and pan interactions.
        /// </summary>
        SfInteractiveScrollView? _scrollView;

        /// <summary>
        /// Holds the layout content displayed inside the interactive viewer.
        /// </summary>
        InteractiveLayout? _interactiveLayout;

        /// <summary>
        /// Holds the root layout for the interactive viewer content.
        /// </summary>
        InteractiveStackLayout? _layout;

        /// <summary>
        /// Holds the previous zoom factor for comparison during zoom updates.
        /// </summary>
        double _oldZoomFactor = 1d;

        /// <summary>
        /// Indicates whether the zoom factor is being updated internally.
        /// </summary>
        bool _skipZoomUpdate;

        /// <summary>
        /// Specifies that the rotation animation is running.
        /// </summary>
        bool _isAnimationRunning;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfInteractiveViewer"/> class.
        /// </summary>
        public SfInteractiveViewer()
        {
            ThemeElement.InitializeThemeResources(this, "SfInteractiveViewerTheme");
            MinimumWidthRequest = 100;
            MinimumHeightRequest = 100;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the interactive viewer content.
        /// </summary>
        View? IInteractiveViewerInfo.Content => this.Content;

        #endregion

        #region Public methods

        /// <summary>
        /// Method to rotate the content clockwise by 90 degrees.
        /// </summary>
        /// <remarks>
        /// This method applies a 90-degree clockwise rotation to the content displayed in the <see cref="SfInteractiveViewer"/>.
        /// Consecutive calls continue rotating the content in 90-degree increments.
        /// </remarks> 
        public void Rotate()
        {
            if (_isAnimationRunning)
                return;

            _interactiveLayout?.RotateView();
            _isAnimationRunning = true;
            Measure(this.Width, this.Height);
            Arrange(new Rect(0, 0, this.Width, this.Height));
            _isAnimationRunning = false;
        }

        /// <summary>
        /// Method to reset the interactive viewer to its default state.
        /// </summary>
        /// <remarks>
        /// Resets the content's zoom level, rotation, and pan position to their original values.
        /// </remarks>
        public void Reset()
        {
            _interactiveLayout?.ResetView();
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Measures the size of the content within the interactive viewer based on the provided width and height constraints.
        /// </summary>
        /// <exclude/>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(Width) ? widthConstraint : this.MinimumWidthRequest;
            double height = double.IsFinite(Height) ? heightConstraint : this.MinimumHeightRequest;

            if (Content != null && (Children.Count == 0 || _scrollView == null))
            {
                InitializeInteractiveViewer();
            }

            //// Due to scroll view we are getting infinite size for interactive layout, hence passing the value form here.
            _interactiveLayout?.UpdateAvailableSize(new Size(width - _interactiveLayout.Margin.HorizontalThickness,
                height - _interactiveLayout.Margin.VerticalThickness));
#if ANDROID
            if (_scrollView != null && _scrollView.Width == -1 
                && _scrollView.Height == -1)
            {
                _scrollView.WidthRequest = width;
                _scrollView.HeightRequest = height;
            }
#endif
            foreach (var child in this.Children)
            {
                if (child is View view)
                {
                    view.Measure(width, height);
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Arranges the content of the interactive viewer within the specified bounds.
        /// </summary>
        /// <exclude/>
        /// <param name="bounds">The available rectangle.</param>
        /// <returns>The layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            foreach (var child in this.Children)
            {
                if (child is View view)
                {
                    view.Arrange(bounds);
                }
            }

            if (_scrollView != null)
            {
                // Set the ScrollView height and width to the available size to fix the following issues:
                // 1. The ScrollView is initially allocated based on the image size.
                // 2. When zoomed, the image scales up but is still limited by the previously allocated ScrollView size.
                _scrollView.WidthRequest = bounds.Width;
                _scrollView.HeightRequest = bounds.Height;
            }

            return bounds.Size;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (this.Handler == null)
            {
                RemoveInteractiveViewHandler();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to initialize the interactive viewer.
        /// </summary>
        void InitializeInteractiveViewer()
        {
            InitializeScrollView();
            InitializeInteractiveLayout();
        }

        /// <summary>
        /// Initializes the interactive scroll view.
        /// </summary>
        void InitializeScrollView()
        {
            _scrollView = new SfInteractiveScrollView()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            ApplyZoomAndPanSettings();
            WireScrollViewEvents();
        }

        /// <summary>
        /// Initializes the interactive layout.
        /// </summary>
        void InitializeInteractiveLayout()
        {
            _interactiveLayout = new InteractiveLayout(this)
            {
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            if (_scrollView == null)
            {
                InitializeScrollView();
            }

            // Initialize a new layout for organizing visual elements.
            _layout = new InteractiveStackLayout();
            _scrollView!.Content = _interactiveLayout;
            _layout.Add(_scrollView);
            this.Children.Add(_layout);
        }

        /// <summary>
        /// Applies the current zoom and pan settings to the interactive viewer.
        /// </summary>
        void ApplyZoomAndPanSettings()
        {
            if (_scrollView == null)
                return;

            _scrollView.AllowZoom = IsZoomEnabled;
            _scrollView.Orientation = InteractiveViewerHelper.GetScrollOrientation(IsPanEnabled, PanAxis);
            _scrollView.MinZoomFactor = MinimumZoomFactor;
            _scrollView.MaxZoomFactor = MaximumZoomFactor;
            _scrollView.ZoomFactor = ZoomFactor;
        }

        /// <summary>
        /// Removes the interactive viewer and cleans up resources.
        /// </summary>
        void RemoveInteractiveViewHandler()
        {
            UnWireScrollViewEvents();
            if (_layout != null)
            {
                Children.Remove(_layout);
                _layout.Handler?.DisconnectHandler();
            }

            _scrollView?.Handler?.DisconnectHandler();
            _interactiveLayout?.RemoveItemsViewHandler();
            _layout = null;
            _interactiveLayout = null;
            _scrollView = null;
        }

        /// <summary>
        /// Method to wire the zoom related events.
        /// </summary>
        void WireScrollViewEvents()
        {
            if (_scrollView == null)
                return;

            _scrollView.ZoomStarted += OnScrollViewZoomStarted;
            _scrollView.ZoomChanged += OnScrollViewZoomChanged;
            _scrollView.ZoomEnded += OnScrollViewZoomEnded;
            _scrollView.ScrollChanged += OnScrollViewScrollChanged;
            _scrollView.SizeChanged += OnScrollViewSizeChanged;
            _scrollView.Loaded += OnscrollViewLoaded;
        }

        /// <summary>
        /// Method to unwire the zoom related events.
        /// </summary>
        void UnWireScrollViewEvents()
        {
            if (_scrollView == null)
                return;

            _scrollView.ZoomStarted -= OnScrollViewZoomStarted;
            _scrollView.ZoomChanged -= OnScrollViewZoomChanged;
            _scrollView.ZoomEnded -= OnScrollViewZoomEnded;
            _scrollView.ScrollChanged -= OnScrollViewScrollChanged;
            _scrollView.SizeChanged -= OnScrollViewSizeChanged;
            _scrollView.Loaded -= OnscrollViewLoaded;
        }

        /// <summary>
        /// Called when zooming starts.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The zoom event data.</param>
        void OnScrollViewZoomStarted(object? sender, ZoomEventArgs e)
        {
            if (_scrollView == null || _interactiveLayout == null 
                || _scrollView.Orientation == ScrollOrientation.Neither)
            {
                return;
            }

            _interactiveLayout.ProcessOnZoomStarted(e);
        }

        /// <summary>
        /// Called when zooming changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The zoom event data.</param>
        void OnScrollViewZoomChanged(object? sender, ZoomEventArgs e)
        {
            if (_scrollView == null || _interactiveLayout == null 
                || _scrollView.Orientation == ScrollOrientation.Neither)
                return;

            _interactiveLayout.ProcessOnZoomChanged(e);
            if (this.ZoomFactor != e.ZoomFactor)
            {
                _skipZoomUpdate = true;
                this.SetValue(ZoomFactorProperty, e.ZoomFactor);
                _skipZoomUpdate = false;
            }
        }

        /// <summary>
        /// Called when zooming ends.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The zoom event data.</param>
        void OnScrollViewZoomEnded(object? sender, ZoomEventArgs e)
        {
            if (_scrollView == null || _interactiveLayout == null 
                || _scrollView.Orientation == ScrollOrientation.Neither)
                return;

            _interactiveLayout.ProcessOnZoomEnded(e);
            _scrollView.ZoomLocation = ZoomLocation.Default;

            var args = new ZoomFactorChangedEventArgs(_oldZoomFactor, e.ZoomFactor);
            ZoomFactorChanged?.Invoke(this, args);
            _oldZoomFactor = e.ZoomFactor;
        }

        /// <summary>
        /// Called when scrolling changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The scroll event data.</param>
        void OnScrollViewScrollChanged(object? sender, ScrollChangedEventArgs e)
        {
            var args = new InteractiveScrollChangedEventArgs(this.PanAxis, this.ZoomFactor);
            ScrollChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Called when size is changed.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event args.</param>
        void OnScrollViewSizeChanged(object? sender, EventArgs e)
        {
            _interactiveLayout?.ProcessOnSizeChanged();
        }

        /// <summary>
        /// Called when scroll view is loaded.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event args.</param>
        void OnscrollViewLoaded(object? sender, EventArgs e)
        {
            if (_scrollView == null)
                return;

            _scrollView.ZoomLocation = ZoomLocation.Centered;
            _scrollView.ZoomTo(ZoomFactor);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Retrieves the resource dictionary for the current theme of the parent element.
        /// </summary>
        /// <returns>Returns the resource dictionary for the current theme of the parent element.</returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfInteractiveViewerStyles();
        }

        /// <summary>
        /// Handles changes in the theme for individual controls.
        /// </summary>
        /// <param name="oldTheme">The old theme value.</param>
        /// <param name="newTheme">The new theme value.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// Handles changes in the common theme shared across multiple elements.
        /// </summary>
        /// <param name="oldTheme">The old theme value.</param>
        /// <param name="newTheme">The new theme value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// Method to reset the zoom related properties.
        /// </summary>
        void IInteractiveViewerInfo.ResetZoomAndPan()
        {
            if (_scrollView == null)
                return;

            _scrollView.AllowZoom = IsZoomEnabled;
            if (_scrollView.ZoomFactor != 1)
            {
                ZoomTo(1);
            }

            _scrollView.ExtentSize = null;
        }

        /// <summary>
        /// Zooms the content to the specified zoom factor.
        /// </summary>
        /// <param name="zoomFactor">The zoom factor.</param>
        void ZoomTo(double zoomFactor)
        {
            if (_scrollView == null || !_scrollView.AllowZoom || !IsZoomEnabled)
            {
                return;
            }

            _scrollView.ZoomLocation = ZoomLocation.Centered;
            zoomFactor = zoomFactor < 1 ? 1 : zoomFactor;
            UpdateZoomFactor(zoomFactor);
        }

        /// <summary>
        /// Updates the zoom factor.
        /// </summary>
        /// <param name="zoomFactor">The zoom factor.</param>
        void UpdateZoomFactor(double zoomFactor)
        {
            if (_scrollView == null || _scrollView.ZoomFactor == zoomFactor)
            {
                return;
            }

            _scrollView.ZoomTo(zoomFactor);
            ZoomFactor = zoomFactor;
        }

        /// <summary>
        /// Method to update the content size.
        /// </summary>
        /// <param name="contentSize">The content size.</param>
        /// <param name="isInitialZoom">Indicates whether the content size is updated for the first zoom operation.</param>
        void IInteractiveZoomInfo.UpdateContentSize(Size contentSize, bool isInitialZoom)
        {
            if (_scrollView == null)
            {
                return;
            }

            if (isInitialZoom && _scrollView.ExtentSize != Size.Zero)
            {
                return;
            }

            _scrollView.ExtentSize = contentSize;
        }

        /// <summary>
        /// Gets the viewport size.
        /// </summary>
        /// <returns>The viewport size.</returns>
        Size IInteractiveZoomInfo.GetViewportSize()
        {
            if (_scrollView == null)
                return Size.Zero;

            return new Size(_scrollView.ViewportWidth, _scrollView.ViewportHeight);
        }

        /// <summary>
        /// Gets the scroll view size.
        /// </summary>
        /// <returns>The scroll view size.</returns>
        Size IInteractiveZoomInfo.GetScrollViewDesiredSize()
        {
            if (_scrollView == null)
                return Size.Zero;

            return new Size(_scrollView.Width, _scrollView.Height);
        }

        /// <summary>
        /// Gets the horizontal scroll offset.
        /// </summary>
        /// <returns>The horizontal scroll offset.</returns>
        double IInteractiveZoomInfo.GetScrollX()
        {
            if (_scrollView == null)
                return 0;

            return _scrollView.ScrollX;
        }

        /// <summary>
        /// Gets the vertical scroll offset.
        /// </summary>
        /// <returns>The vertical scroll offset.</returns>
        double IInteractiveZoomInfo.GetScrollY()
        {
            if (_scrollView == null)
                return 0;

            return _scrollView.ScrollY;
        }

        /// <summary>
        /// Scrolls to the specified offsets.
        /// </summary>
        /// <param name="xOffset">The horizontal offset.</param>
        /// <param name="yOffset">The vertical offset.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        async Task IInteractiveZoomInfo.ScrollToAsync(double xOffset, double yOffset)
        {
            if (_scrollView == null)
                return;

            await _scrollView.ScrollToAsync(xOffset, yOffset, false);
        }

        #endregion
    }
}