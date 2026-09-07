namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Layouts;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a scroll view with interactive zooming and panning capabilities.
    /// </summary>
    [ContentProperty(nameof(Content))]
    internal class SfInteractiveScrollView : View, ITapGestureListener
    {
        #region Fields

        /// <summary>
        /// Tracks asynchronous scroll operations.
        /// </summary>
        TaskCompletionSource<bool>? _scrollCompletionSource;

        /// <summary>
        /// Stores the current horizontal scroll offset.
        /// </summary>
        double _currentHorizontalOffset = 0;

        /// <summary>
        /// Stores the last requested vertical offset to avoid redundant scroll operations.
        /// </summary>
        double _currentVerticalOffset = 0;

        /// <summary>
        /// Stores the current horizontal scroll proportion.
        /// </summary>
        double _currentHorizontalProportion = 0;

        /// <summary>
        /// Stores the current vertical scroll offset.
        /// </summary>
        double _currentVerticalProportion = 0;

        /// <summary>
        /// Handles pan, zoom, and gesture interactions.
        /// </summary>
        internal PanZoomListener? _panZoomListener;

        /// <summary>
        /// Stores the measured content size.
        /// </summary>
        internal Size _contentSize = Size.Zero;
#if IOS && !MACCATALYST

        /// <summary>
        /// Indicates whether scrolling should move to the RTL end position.
        /// </summary>
        bool _rtlScrollEndRequired;
#endif
#if WINDOWS

        /// <summary>
        /// Handles pan gesture interactions on Windows.
        /// </summary>
        internal PanGestureManager? _panGestureManager;
#elif IOS || MACCATALYST

        /// <summary>
        /// Handles keyboard gestures on iOS and Mac Catalyst.
        /// </summary>
        internal KeyboardGestureManager? _keyboardGestureManager;

        /// <summary>
        /// Indicates whether the content contains editor controls.
        /// </summary>
        internal bool IsEditors { get; set; }
#endif
        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="ZoomLocation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ZoomLocation"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ZoomLocationProperty =
            BindableProperty.Create(
                nameof(ZoomLocation),
                typeof(ZoomLocation),
                typeof(SfInteractiveScrollView),
                ZoomLocation.Default);

        /// <summary>
        /// Identifies the <see cref="ZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ZoomFactor"/> bindable property.
        /// </value>
        static readonly BindablePropertyKey ZoomFactorPropertyKey =
            BindableProperty.CreateReadOnly(
                nameof(ZoomFactor),
                typeof(double),
                typeof(SfInteractiveScrollView),
                1.0);

        /// <summary>
        /// Identifies the <see cref="ZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ZoomFactor"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ZoomFactorProperty = ZoomFactorPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the <see cref="MinZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinZoomFactor"/> bindable property.
        /// </value>
        internal static readonly BindableProperty MinZoomFactorProperty =
            BindableProperty.Create(
                nameof(MinZoomFactor),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0.1,
                coerceValue: CoerceMinZoom,
                propertyChanged: OnMinZoomChanged);

        /// <summary>
        /// Identifies the <see cref="SuppressAutoScroll"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SuppressAutoScroll"/> bindable property.
        /// </value>
        internal static readonly BindableProperty SuppressAutoScrollProperty =
            BindableProperty.Create(
                nameof(SuppressAutoScroll),
                typeof(bool),
                typeof(SfInteractiveScrollView),
                false,
                propertyChanged: OnSuppressAutoScrollChanged);

        /// <summary>
        /// Identifies the <see cref="MaxZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MaxZoomFactor"/> bindable property.
        /// </value>
        internal static readonly BindableProperty MaxZoomFactorProperty =
            BindableProperty.Create(
                nameof(MaxZoomFactor),
                typeof(double),
                typeof(SfInteractiveScrollView),
                10.0,
                coerceValue: CoerceMaxZoom,
                propertyChanged: OnMaxZoomChanged);

        /// <summary>
        /// Identifies the <see cref="CanBecomeFirstResponder"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CanBecomeFirstResponder"/> bindable property.
        /// </value>
        private static readonly BindableProperty CanBecomeFirstResponderProperty =
            BindableProperty.Create(
                nameof(CanBecomeFirstResponder),
                typeof(bool),
                typeof(SfInteractiveScrollView),
                true);

        /// <summary>
        /// Identifies the <see cref="AllowZoom"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AllowZoom"/> bindable property.
        /// </value>
        internal static readonly BindableProperty AllowZoomProperty =
                BindableProperty.Create(
                    nameof(AllowZoom),
                    typeof(bool),
                    typeof(SfInteractiveScrollView),
                    false,
                    propertyChanged: OnAllowZoomChanged);

        /// <summary>
        /// Identifies the <see cref="ViewportHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ViewportHeight"/> bindable property.
        /// </value>
        static readonly BindablePropertyKey ViewportHeightPropertyKey =
            BindableProperty.CreateReadOnly(
                nameof(ViewportHeight),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0.0);

        /// <summary>
        /// Identifies the <see cref="ViewportHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ViewportHeight"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ViewportHeightProperty = ViewportHeightPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the <see cref="ViewportWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ViewportWidth"/> bindable property.
        /// </value>
        static readonly BindablePropertyKey ViewportWidthPropertyKey =
            BindableProperty.CreateReadOnly(
                nameof(ViewportWidth),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0.0);

        /// <summary>
        /// Identifies the <see cref="ViewportWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ViewportWidth"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ViewportWidthProperty = ViewportWidthPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the <see cref="Orientation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Orientation"/> bindable property.
        /// </value>
        internal static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(
                nameof(Orientation),
                typeof(ScrollOrientation),
                typeof(SfInteractiveScrollView),
                ScrollOrientation.Vertical,
                propertyChanged: OnOrientationChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollY"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollY"/> bindable property.
        /// </value>
        static readonly BindablePropertyKey ScrollYPropertyKey =
            BindableProperty.CreateReadOnly(
                nameof(ScrollY),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0d,
                propertyChanged: OnVerticalOffsetChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollY"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollY"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ScrollYProperty = ScrollYPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the <see cref="ScrollX"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollX"/> bindable property.
        /// </value>
        static readonly BindablePropertyKey ScrollXPropertyKey =
            BindableProperty.CreateReadOnly(
                nameof(ScrollX),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0d,
                propertyChanged: OnHorizontalOffsetChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollX"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollX"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ScrollXProperty = ScrollXPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the <see cref="ScrollYProportion"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollYProportion"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ScrollYProportionProperty =
            BindableProperty.Create(
                nameof(ScrollYProportion),
                typeof(double),
                typeof(SfInteractiveScrollView),
                0d,
                propertyChanged: OnVerticalProportionalOffsetChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollXProportion"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScrollXProportion"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ScrollXProportionProperty =
            BindableProperty.Create(
                nameof(ScrollXProportion), 
                typeof(double), 
                typeof(SfInteractiveScrollView), 
                0d, 
                propertyChanged: OnHorizontalProportionalOffsetChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="VerticalScrollBarVisibility"/> bindable property.
        /// </value>
        internal static readonly BindableProperty VerticalScrollBarVisibilityProperty =
            BindableProperty.Create(
                nameof(VerticalScrollBarVisibility),
                typeof(ScrollBarVisibility),
                typeof(SfInteractiveScrollView),
                ScrollBarVisibility.Default);

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HorizontalScrollBarVisibility"/> bindable property.
        /// </value>
        internal static readonly BindableProperty HorizontalScrollBarVisibilityProperty =
            BindableProperty.Create(
                nameof(HorizontalScrollBarVisibility),
                typeof(ScrollBarVisibility),
                typeof(SfInteractiveScrollView),
                ScrollBarVisibility.Default);

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ContentProperty =
            BindableProperty.Create(
                nameof(Content),
                typeof(View),
                typeof(SfInteractiveScrollView),
                null,
                propertyChanged: OnContentChanged);

        /// <summary>
        /// Identifies the <see cref="ContentSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ContentSize"/> bindable property.
        /// </value>
        static readonly BindableProperty ContentSizeProperty =
            BindableProperty.Create(
                nameof(ContentSize),
                typeof(Size),
                typeof(SfInteractiveScrollView),
                default(Size),
                propertyChanged: OnExtentSizeChanged);

        /// <summary>
        /// Identifies the <see cref="ExtentSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ExtentSize"/> bindable property.
        /// </value>
        internal static readonly BindableProperty ExtentSizeProperty =
            BindableProperty.Create(
                nameof(ExtentSize),
                typeof(Size?),
                typeof(ScrollView),
                null,
                propertyChanged: OnExtentSizeRequestChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfInteractiveScrollView"/> class.
        /// </summary>
        internal SfInteractiveScrollView()
        {
            InitializeControl();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the zoom is changed.
        /// </summary>
        internal event EventHandler<ZoomEventArgs>? ZoomChanged;

        /// <summary>
        /// Occurs when the zoom is started.
        /// </summary>
        internal event EventHandler<ZoomEventArgs>? ZoomStarted;

        /// <summary>
        /// Occurs when the zoom is ended.
        /// </summary>
        internal event EventHandler<ZoomEventArgs>? ZoomEnded;

        /// <summary>
        /// Occurs when the content is scrolled.
        /// </summary>
        internal event EventHandler<ScrollChangedEventArgs>? ScrollChanged;

        /// <summary>
        /// Occurs when the content is Pan.
        /// </summary>
        internal event EventHandler<PanEventArgs>? OnPan;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the desired location at which the zoom should occur.
        /// The default value is <see cref="ZoomLocation.Default"/>, which causes zooming to occur at the pointer position.
        /// </summary>
        internal ZoomLocation ZoomLocation
        {
            get { return (ZoomLocation)GetValue(ZoomLocationProperty); }
            set { SetValue(ZoomLocationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical scroll offset as a proportion of the total scrollable content.
        /// </summary>
        internal double ScrollYProportion
        {
            get { return (double)GetValue(ScrollYProportionProperty); }
            set { SetValue(ScrollYProportionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal scroll offset as a proportion of the total scrollable width.
        /// </summary>
        internal double ScrollXProportion
        {
            get { return (double)GetValue(ScrollXProportionProperty); }
            set { SetValue(ScrollXProportionProperty, value); }
        }

        /// <summary>
        /// Gets the vertical size of the viewport.
        /// </summary>
        internal double ViewportHeight
        {
            get { return (double)GetValue(ViewportHeightProperty); }
            set { SetValue(ViewportHeightPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal size of the viewport.
        /// </summary>
        internal double ViewportWidth
        {
            get { return (double)GetValue(ViewportWidthProperty); }
            set { SetValue(ViewportWidthPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the minimum zoom factor permitted for the content.
        /// </summary>
        /// <remarks>
        /// The default value is <c>0.1</c>. The value should not exceed <see cref="MaxZoomFactor"/>.
        /// </remarks>
        internal double MinZoomFactor
        {
            get { return (double)GetValue(MinZoomFactorProperty); }
            set { SetValue(MinZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum zoom factor permitted for the content.
        /// </summary>
        /// <remarks>
        /// The default value is <c>10.0</c>. The value should not be less than <see cref="MinZoomFactor"/>.
        /// </remarks>
        internal double MaxZoomFactor
        {
            get { return (double)GetValue(MaxZoomFactorProperty); }
            set { SetValue(MaxZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets the double-tap gesture settings used for zoom operations.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="null"/> when the pan and zoom listener is not available.
        /// </remarks>
        internal DoubleTapSettings? DoubleTapSettings
        {
            get
            {
                if (_panZoomListener != null)
                    return _panZoomListener.DoubleTapSettings;

                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether the vertical scroll bar is visible.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="ScrollBarVisibility.Default"/>.
        /// </remarks>
        internal ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that determines whether the horizontal scroll bar is visible.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="ScrollBarVisibility.Default"/>.
        /// </remarks>
        internal ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scrolling direction of the scroll view.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="ScrollOrientation.Vertical"/>.
        /// This value enables scrolling in both the vertical and horizontal directions.
        /// Scrolling can be disabled by setting the value to <see cref="ScrollOrientation.Neither"/>.
        /// </remarks>
        internal ScrollOrientation Orientation
        {
            get { return (ScrollOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the overall logical horizontal and vertical size of the scrollable area. The value is described in device-independent units.
        /// </summary>
        /// <remarks>
        /// To prevent layout calls for each UI update during virtualization (or) an asynchronous process, it is advised to specify the total scrollable area using the <see cref="ExtentSize"/> property at the beginning itself.
        /// </remarks>
        internal Size? ExtentSize
        {
            get { return (Size?)GetValue(ExtentSizeProperty); }
            set { SetValue(ExtentSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical scroll offset of the content. The value is specified in device-independent units.
        /// </summary>
        internal double ScrollY
        {
            get { return (double)GetValue(ScrollYProperty); }
            set { SetValue(ScrollYPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal scroll offset of the content. The value is specified in device-independent units.
        /// </summary>
        internal double ScrollX
        {
            get { return (double)GetValue(ScrollXProperty); }
            set { SetValue(ScrollXPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the current zoom factor.
        /// </summary>
        /// <remarks>
        /// The default value is <c>1.0</c>. A value of <c>1.0</c> indicates that no scaling is applied to the content.
        /// </remarks>
        internal double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the view to display in the <see cref="SfInteractiveScrollView"/>.
        /// </summary>
        internal View? Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether content zooming is enabled.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="false"/>.
        /// When set to <see langword="false"/>, zooming operations are disabled.
        /// </remarks>
        internal bool AllowZoom
        {
            get { return (bool)GetValue(AllowZoomProperty); }
            set { SetValue(AllowZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the logical width and height of the scrollable content area. The value is specified in device-independent units.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="Size.Zero"/>. This value is updated during the layout phase.
        /// </remarks>
        internal Size ContentSize
        {
            get { return (Size)GetValue(ContentSizeProperty); }
            set { SetValue(ContentSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic scrolling to a child element when it receives focus is suppressed.
        /// </summary>
        internal bool SuppressAutoScroll
        {
            get { return (bool)GetValue(SuppressAutoScrollProperty); }
            set { SetValue(SuppressAutoScrollProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can receive keyboard focus.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the control can become the first responder and receive keyboard input.
        /// </remarks>
        internal bool CanBecomeFirstResponder
        {
            get { return (bool)GetValue(CanBecomeFirstResponderProperty); }
            set { SetValue(CanBecomeFirstResponderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a scrolling operation is currently in progress.
        /// </summary>
        /// <remarks>
        /// This property is used to prevent conflicts between scrolling and other interactions, such as panning. Applicable only on Windows.
        /// </remarks>
        internal bool IsScrolling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a scroll request should be deferred until the content layout or platform view measurement is complete.
        /// </summary>
        internal bool IsContentLayoutRequested { get; set; }

        /// <summary>
        /// Gets or sets the content host that presents the scrollable content.
        /// </summary>
        internal ContentPlaceHolder? PresentedContent { get; set; }

        /// <summary>
        /// Gets the center point of the currently visible viewport.
        /// </summary>
        /// <remarks>
        /// The center point is calculated using the current scroll offsets and viewport dimensions.
        /// </remarks>
        Point CenteredOrigin
        {
            get
            {
                return new Point(ScrollX + (ViewportWidth / 2), ScrollY + (ViewportHeight / 2));
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Zooms the content by the specified factor at the given origin.
        /// </summary>
        /// <param name="zoomFactor">The factor by which the content is zoomed.</param>
        /// <param name="zoomOrigin">The origin point from which the zoom operation is performed.</param>
        internal void ZoomBy(double zoomFactor, Point? zoomOrigin = null)
        {
            ZoomTo(this.ZoomFactor * zoomFactor, zoomOrigin);
        }

        /// <summary>
        /// Asynchronously scrolls the content to the specified position.
        /// </summary>
        /// <param name="x">The horizontal scroll offset, in device-independent units.</param>
        /// <param name="y">The vertical scroll offset, in device-independent units.</param>
        /// <param name="animated">A value indicating whether the scroll operation is animated.</param>
        /// <returns>A task that represents the asynchronous scroll operation.</returns>
        internal Task ScrollToAsync(double x, double y, bool animated = true)
        {
            switch (Orientation)
            {
                case ScrollOrientation.Neither:
                    return Task.FromResult(false);
                case ScrollOrientation.Vertical:
                    x = ScrollX;
                    break;
                case ScrollOrientation.Horizontal:
                    y = ScrollY;
                    break;
            }

            CheckTaskCompletionSource();
            ScrollTo(x, y, animated);

            if (_scrollCompletionSource != null)
                return _scrollCompletionSource.Task;
            else
                return Task.FromResult(false);
        }

        /// <summary>
        /// Asynchronously scrolls the content by the specified offset.
        /// </summary>
        /// <param name="x">The horizontal offset by which to scroll, in device-independent units.</param>
        /// <param name="y">The vertical offset by which to scroll, in device-independent units.</param>
        /// <param name="animated">A value indicating whether the scroll operation is animated.</param>
        /// <returns>A task that represents the asynchronous scroll operation.</returns>
        internal Task ScrollByAsync(double x, double y, bool animated = true)
        {
            return ScrollToAsync(ScrollX + x, ScrollY + y, animated);
        }

        /// <summary>
        /// Sets the <see cref="ZoomFactor"/> to the specified value and triggers the zoom-related events.
        /// </summary>
        /// <param name="zoomFactor">The desired zoom factor. The valid range is from <see cref="MinZoomFactor"/> to <see cref="MaxZoomFactor"/>.
        /// </param>
        /// <param name="zoomOrigin">An optional point that specifies the origin from which the zoom operation is performed.
        /// </param>
        internal void ZoomTo(double zoomFactor, Point? zoomOrigin = null)
        {
            if (Handler != null)
                _panZoomListener?.ZoomTo(zoomFactor, zoomOrigin);
            else
                ZoomFactor = zoomFactor;
        }

        /// <summary>
        /// Scrolls the content to the specified position.
        /// </summary>
        /// <param name="x">The horizontal scroll offset, in device-independent units.</param>
        /// <param name="y">The vertical scroll offset, in device-independent units.</param>
        /// <param name="animated">A value indicating whether the scroll operation is animated.</param>
        internal void ScrollTo(double x, double y, bool animated = true)
        {
            if (Handler != null)
            {
                ScrollToParameters? request = new ScrollToParameters(x, y, animated);
                Handler.Invoke(nameof(SfInteractiveScrollView.ScrollTo), request);
                request = null;
            }
            else
            {
                ScrollX = x;
                ScrollY = y;
            }
        }

        /// <summary>
        /// Scrolls the content horizontally to the specified offset.
        /// </summary>
        /// <param name="x">The horizontal scroll offset, in device-independent units.</param>
        /// <param name="animated">A value indicating whether the scroll operation is animated.</param>
        internal void ScrollToX(double x, bool animated = true)
        {
            ScrollTo(x, ScrollY, animated);
            ScrollX = x;
        }

        /// <summary>
        /// Scrolls the content vertically to the specified offset.
        /// </summary>
        /// <param name="y">The vertical scroll offset, in device-independent units.</param>
        /// <param name="animated">A value indicating whether the scroll operation is animated.</param>
        internal void ScrollToY(double y, bool animated = true)
        {
            ScrollTo(ScrollX, y, animated);
            ScrollY = y;
        }

        /// <summary>
        /// Updates the scroll offsets and raises the <see cref="ScrollChanged"/> event.
        /// </summary>
        /// <param name="scrolledEventArgs">The information about the scroll change.</param>
        internal void OnScrollChanged(ScrollChangedEventArgs scrolledEventArgs)
        {
            if (ScrollY == scrolledEventArgs.ScrollY && ScrollX == scrolledEventArgs.ScrollX)
            {
                return;
            }

            ScrollY = scrolledEventArgs.ScrollY;
            ScrollX = scrolledEventArgs.ScrollX;
            ScrollChanged?.Invoke(this, scrolledEventArgs);
        }

        /// <summary>
        /// Updates the control layout using the specified dimensions.
        /// </summary>
        internal void RequestLayout(double controlWidth, double controlHeight)
        {
            if (controlWidth <= 0 || controlHeight <= 0)
            {
                if (controlWidth <= 0)
                    controlWidth = _contentSize.Width;
                else if (controlHeight <= 0)
                    controlHeight = _contentSize.Height;
            }

#if NET9_0
            this.Layout(new Rect(this.Bounds.X, this.Bounds.Y, controlWidth, controlHeight));
#elif NET10_0
            this.Frame = new Rect(this.Bounds.X, this.Bounds.Y, controlWidth, controlHeight);
            this.InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Updates the horizontal scroll proportion based on the specified horizontal offset.
        /// </summary>
        /// <param name="horizontalOffset">The current horizontal scroll offset.</param>
        internal void UpdateHorizontalProportion(double horizontalOffset)
        {
            if (horizontalOffset == _currentHorizontalOffset)
                return;

            if (ContentSize.Width > 0)
            {
                double desiredHorizontalProportion = horizontalOffset / ContentSize.Width;
                _currentHorizontalProportion = desiredHorizontalProportion;
                ScrollXProportion = desiredHorizontalProportion;
            }
        }

        /// <summary>
        /// Completes the pending asynchronous scroll operation.
        /// </summary>
        internal void SendScrollFinished()
        {
            if (_scrollCompletionSource != null)
                _scrollCompletionSource?.TrySetResult(true);
        }

        /// <summary>
        /// Updates the vertical scroll proportion based on the specified vertical offset.
        /// </summary>
        /// <param name="verticalOffset">The current vertical scroll offset.</param>
        internal void UpdateVerticalProportion(double verticalOffset)
        {
            if (verticalOffset == _currentVerticalOffset)
                return;
#if ANDROID
            if (ContentSize.Height > 0 && verticalOffset > 0)
            {
                double desiredVerticalProportion = verticalOffset / ContentSize.Height;
                _currentVerticalProportion = desiredVerticalProportion;
                ScrollYProportion = desiredVerticalProportion;
                if (Math.Abs(_currentVerticalOffset - verticalOffset) >= 1)
                {
                    _currentVerticalOffset = verticalOffset;
                }
            }
#else
            if (ContentSize.Height > 0)
            {
                double desiredVerticalProportion = verticalOffset / ContentSize.Height;
                _currentVerticalProportion = desiredVerticalProportion;
                ScrollYProportion = desiredVerticalProportion;
                if (Math.Abs(_currentVerticalOffset - verticalOffset) >= 1)
                {
                    _currentVerticalOffset = verticalOffset;
                }
            }
#endif
        }

        /// <summary>
        /// Raises the <see cref="OnPan"/> event.
        /// </summary>
        /// <param name="pan">The pan event args.</param>
        internal void OnPanUpdated(PanEventArgs pan)
        {
            OnPan?.Invoke(this, pan);
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Overrides the handler changed occurrence.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                AddOrRemoveZoomGestures(AllowZoom);
                if (Content != null)
                    UpdatePresentedContent(Content);
            }
            else
            {
                PresentedContent?.Unload();
            }
        }

        /// <summary>
        /// Handles size changes that occur during the layout cycle.
        /// </summary>
        /// <param name="width">The new width of the element.</param>
        /// <param name="height">The new height of the element.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            if (!_contentSize.IsZero)
            {
                RequestLayout(width, height);
                // Invalidate extent size to maintain the content alignment on window size change as well as for pan gesture validation.
                if (ExtentSize == null)
                    InvalidateExtentSize(_contentSize);
                else
                    InvalidateExtentSize(ExtentSize.Value);
            }

            base.OnSizeAllocated(this.Bounds.Width, this.Bounds.Height);
        }

        #endregion

        #region Interface implementations

        /// <summary>
        /// Occurs on tap interaction inside the view.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
#if IOS || MACCATALYST
            CanBecomeFirstResponder = true;
            if (!IsEditors)
                Focus();
#endif
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Initializes gesture handling, content, and event subscriptions.
        /// </summary>
        void InitializeControl()
        {
            InitializePanZoomListener();
            InitializePlatformGestures();
            InitializePresentedContent();
            WireEvents();
        }

        /// <summary>
        /// Creates and configures the pan and zoom listener.
        /// </summary>
        void InitializePanZoomListener()
        {
            _panZoomListener = new PanZoomListener
            {
                PanMode = PanMode.Vertical
            };

#if WINDOWS
            if (_panZoomListener.MouseWheelSettings != null)
                _panZoomListener.MouseWheelSettings.ZoomKeyModifier = KeyboardKey.Ctrl;
#endif
        }

        /// <summary>
        /// Creates a new task completion source for the current scroll operation.
        /// </summary>
        void CheckTaskCompletionSource()
        {
            if (_scrollCompletionSource != null &&
                _scrollCompletionSource.Task.Status == TaskStatus.Running)
            {
                _scrollCompletionSource.TrySetCanceled();
            }

            _scrollCompletionSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Initializes platform-specific gesture handlers.
        /// </summary>
        void InitializePlatformGestures()
        {
            if (_panZoomListener != null)
            {
#if WINDOWS
                _panGestureManager = new PanGestureManager(this, _panZoomListener);
#elif IOS || MACCATALYST
                // Restrict mouse wheel zoom on iOS/MacCatalyst due to framework limitations.
                _panZoomListener.AllowMouseWheelZoom = false;

                _keyboardGestureManager = new KeyboardGestureManager(this);
                this.AddGestureListener(this);
#endif
            }
        }

        /// <summary>
        /// Initializes the content host used to display the presented content.
        /// </summary>
        void InitializePresentedContent()
        {
            if (_panZoomListener != null)
            {
                PresentedContent = new ContentPlaceHolder(_panZoomListener);
            }
        }

        /// <summary>
        /// Updates the zoom origin when zooming is configured to occur at the center of the viewport.
        /// </summary>
        /// <param name="e">The zoom event args.</param>
        void CheckIfZoomLocationRequested(ZoomEventArgs e)
        {
            if (ZoomLocation == ZoomLocation.Centered)
            {
                if (ViewportHeight > 0 && ViewportWidth > 0)
                    e.ZoomOrigin = CenteredOrigin;
            }
        }

        /// <summary>
        /// Enables or disables zoom gesture support for the presented content.
        /// </summary>
        /// <param name="allowZoom">A value indicating whether zoom gestures should be enabled.</param>
        void AddOrRemoveZoomGestures(bool allowZoom)
        {
            if (allowZoom)
                PresentedContent?.AddZoomGestures();
            else
                PresentedContent?.RemoveZoomGestures();
        }

        /// <summary>
        /// Scrolls vertically to the specified proportional offset.
        /// </summary>
        /// <param name="offsetProportion">The vertical scroll position expressed as a proportion of the total scrollable height.</param>
        void ScrollToVerticalOffsetProportionately(double offsetProportion)
        {
            if (ContentSize.Height > 0)
            {
                double desiredVerticalOffset = offsetProportion * ContentSize.Height;
                _currentVerticalOffset = desiredVerticalOffset;
                ScrollToY(desiredVerticalOffset, false);
            }
        }

        /// <summary>
        /// Scrolls horizontally to the specified proportional offset.
        /// </summary>
        /// <param name="offsetProportion">The horizontal scroll position expressed as a proportion of the total scrollable width.</param>
        void ScrollToHorizontalOffsetProportionately(double offsetProportion)
        {
            if (ContentSize.Width > 0)
            {
                double desiredHorizontalOffset = offsetProportion * ContentSize.Width;
                _currentHorizontalOffset = desiredHorizontalOffset;
                ScrollToX(desiredHorizontalOffset, false);
            }
        }

        /// <summary>
        /// Converts the specified scroll orientation to its corresponding pan mode.
        /// </summary>
        /// <param name="orientation">The scroll orientation to convert.</param>
        /// <returns>The pan mode that corresponds to the specified scroll orientation.</returns>
        PanMode ConvertOrientationToPanMode(ScrollOrientation orientation)
        {
            switch (orientation)
            {
                case ScrollOrientation.Neither:
                    {
#if WINDOWS
                        _panGestureManager?.ResetValues();
#endif
                        return PanMode.None;
                    }
                case ScrollOrientation.Horizontal:
                    return PanMode.Horizontal;
                case ScrollOrientation.Vertical:
                    return PanMode.Vertical;
                default:
                    return PanMode.Both;
            }
        }

        /// <summary>
        /// Applies the initial zoom factor and scroll position after the content has been loaded.
        /// </summary>
        void ApplyPresetProperties()
        {
            if (MinZoomFactor > ZoomFactor)
                ZoomTo(MinZoomFactor);
            else if (ZoomFactor != (double)ZoomFactorProperty.DefaultValue)
                ZoomTo(ZoomFactor);

            if (ScrollY != (double)ScrollYProperty.DefaultValue || ScrollX != (double)ScrollXProperty.DefaultValue)
            {
                IsContentLayoutRequested = true;
                ScrollTo(ScrollX, ScrollY, false);
            }
        }

        /// <summary>
        /// Adds the specified content to the presentation host and updates its layout.
        /// </summary>
        /// <param name="content">The content view to be displayed.</param>
        void UpdatePresentedContent(View content)
        {
            if (PresentedContent != null)
            {
                PresentedContent.Children.Add(content);
                PresentedContent.UpdateLayoutContent();
                ApplyPresetProperties();
            }
        }

        /// <summary>
        /// Updates the content size based on the specified extent size and the current size of the control.
        /// </summary>
        /// <param name="extentSize">The extent size to be validated and applied.</param>
        void InvalidateExtentSize(Size extentSize)
        {
            if (this.Bounds.Width != -1 && this.Bounds.Height != -1)
            {
                extentSize.Width = Math.Max(extentSize.Width, this.Bounds.Width);
                extentSize.Height = Math.Max(extentSize.Height, this.Bounds.Height);
            }
            else if (this.WidthRequest > 0 && this.HeightRequest > 0)
            {
                extentSize.Width = Math.Max(extentSize.Width, this.WidthRequest);
                extentSize.Height = Math.Max(extentSize.Height, this.HeightRequest);
            }

            ContentSize = extentSize;
        }

        /// <summary>
        /// Resets the scroll view state, including the zoom factor, scroll offsets, content size, and presented content.
        /// </summary>
        void Reset()
        {
            this.ScrollTo(0, 0, false);
            if (_panZoomListener != null)
            {
                _panZoomListener.CurrentZoomFactor = 1;
            }

            if (ExtentSize == null)
                ContentSize = Size.Zero;

            PresentedContent?.Reset();
            ZoomFactor = 1;
            ScrollY = ScrollX = 0;
            _contentSize = Size.Zero;
        }

        /// <summary>
        /// Subscribes to zoom-related events.
        /// </summary>
        void WireZoomEvents()
        {
            if (_panZoomListener != null)
            {
                _panZoomListener.ZoomStarted += OnZoomStarted;
                _panZoomListener.ZoomChanged += OnZoomChanged;
                _panZoomListener.ZoomEnded += OnZoomEnded;
            }
        }

        /// <summary>
        /// Unsubscribes from zoom-related events.
        /// </summary>
        void UnwireZoomEvents()
        {
            if (_panZoomListener != null)
            {
                _panZoomListener.ZoomStarted -= OnZoomStarted;
                _panZoomListener.ZoomChanged -= OnZoomChanged;
                _panZoomListener.ZoomEnded -= OnZoomEnded;
            }
        }

        /// <summary>
        /// Subscribes to control and zoom-related events.
        /// </summary>
        void WireEvents()
        {
            this.PropertyChanging += OnPropertyChanging;
            this.PropertyChanged += OnPropertyChanged;
            WireZoomEvents();
        }

        /// <summary>
        /// Unsubscribes from control and zoom-related events.
        /// </summary>
        void UnwireEvents()
        {
            this.PropertyChanging -= OnPropertyChanging;
            this.PropertyChanged -= OnPropertyChanged;
            UnwireZoomEvents();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Occurs when a zoom operation begins.
        /// </summary>
        void OnZoomStarted(object? sender, ZoomEventArgs e)
        {
            if (ZoomStarted != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomStarted(this, e);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="SuppressAutoScroll"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSuppressAutoScrollChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView != null && scrollView.Handler != null)
            {
                scrollView.Handler.UpdateValue(nameof(SuppressAutoScroll));
            }
        }

        /// <summary>
        /// Raises the <see cref="ZoomChanged"/> event and updates the current <see cref="ZoomFactor"/>
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The zoom event args.</param>
        void OnZoomChanged(object? sender, ZoomEventArgs e)
        {
            if (ZoomChanged != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomChanged(this, e);
            }

            ZoomFactor = e.ZoomFactor;
        }

        /// <summary>
        /// Handles the <see cref="ZoomEnded"/> event.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The zoom event args.</param>
        void OnZoomEnded(object? sender, ZoomEventArgs e)
        {
            if (ZoomEnded != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomEnded(this, e);
            }
        }

        /// <summary>
        /// Handles property changes before the property value is updated.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The property changing event data.</param>
        void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Content):
                    if (Content != null)
                    {
                        Content.SizeChanged -= OnContentSizeChanged;
                    }

                    Reset();
                    break;
            }
        }

        /// <summary>
        /// Handles property changes after the property value has been updated.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The property changed event data.</param>
        void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Content):
                    if (Content != null)
                    {
                        Content.SizeChanged += OnContentSizeChanged;
                    }

                    InvalidateMeasure();
                    break;
                case nameof(BackgroundColor):
                    if (PresentedContent != null)
                    {
                        PresentedContent.BackgroundColor = this.BackgroundColor;
                    }

                    break;
#if !WINDOWS
                case nameof(IsEnabled):
                    if (PresentedContent != null)
                    {
                        PresentedContent.IsEnabled = IsEnabled;
                    }

                    break;
                // Since the flow direction - "MatchParent" is not working when RTL (Right To Left) applied in Android and iOS for the scroll view "content", forcing the effective flow direction.
                case nameof(FlowDirection):
                    if (PresentedContent != null)
                    {
                        PresentedContent.FlowDirection = this.FlowDirection;
#if IOS || MACCATALYST
                        PresentedContent.UpdateLayoutContent();
#endif
                    }
#if IOS && !MACCATALYST
                    FlowDirection effectiveFlowDirection = this.FlowDirection;
                    if (effectiveFlowDirection == FlowDirection.MatchParent)
                    {
                        effectiveFlowDirection = this.GetEffectiveFlowDirection(this);
                    }

                    if ((effectiveFlowDirection == FlowDirection.RightToLeft) && (this.Orientation == ScrollOrientation.Horizontal || this.Orientation == ScrollOrientation.Both))
                    {
                        _rtlScrollEndRequired = true;
                    }
#endif
                    break;
#endif
            }
        }

        /// <summary>
        /// Method to get the effective flow direction.
        /// </summary>
        /// <param name="element">The element from which the search begins.</param>
        /// <returns>The effective flow direction.</returns>
        FlowDirection GetEffectiveFlowDirection(Element element)
        {
            Element currentElement = element;
            while (currentElement != null)
            {
                if (currentElement is VisualElement visualElement &&
                    visualElement.FlowDirection != FlowDirection.MatchParent)
                {
                    return visualElement.FlowDirection;
                }

                currentElement = currentElement.Parent;
            }

            return FlowDirection.LeftToRight;
        }

        #endregion

        #region Coerce value callbacks

        /// <summary>
        /// Coerces the minimum zoom factor to ensure that it does not exceed the current <see cref="MaxZoomFactor"/>.
        /// </summary>
        /// <param name="bindable">The bindable object. </param>
        /// <param name="value">The proposed value.</param>
        /// <returns>The coerced minimum zoom factor.</returns>
        static object CoerceMinZoom(BindableObject bindable, object value)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                if (double.TryParse(value.ToString(), out double minZoomFactor))
                {
                    if (minZoomFactor > scrollView.MaxZoomFactor)
                        value = scrollView.MaxZoomFactor;
                }
            }

            return value;
        }

        /// <summary>
        /// Coerces the maximum zoom factor to ensure that it is not less than the current <see cref="MinZoomFactor"/>.
        /// </summary>
        /// <param name="bindable"> The bindable object.</param>
        /// <param name="value">The proposed value.</param>
        /// <returns>The coerced maximum zoom factor.</returns>
        static object CoerceMaxZoom(BindableObject bindable, object value)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                if (double.TryParse(value.ToString(), out double maxZoomFactor))
                {
                    if (maxZoomFactor < scrollView.MinZoomFactor)
                        value = scrollView.MinZoomFactor;
                }
            }

            return value;
        }

        #endregion

        #region Property change handlers

        /// <summary>
        /// Called when the <see cref="MinZoomFactor"/> property changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnMinZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                double minZoomFactor = (double)newValue;
                if (scrollView._panZoomListener != null)
                {
                    scrollView._panZoomListener.MinZoomFactor = minZoomFactor;
                }

                if (scrollView.ZoomFactor < minZoomFactor)
                    scrollView.ZoomTo(minZoomFactor, new Point(scrollView.ScrollX, scrollView.ScrollY));
            }
        }

        /// <summary>
        /// Called when the <see cref="MaxZoomFactor"/> property changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnMaxZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                double maxZoomFactor = (double)newValue;
                if (scrollView._panZoomListener != null)
                {
                    scrollView._panZoomListener.MaxZoomFactor = maxZoomFactor;
                }

                if (scrollView.ZoomFactor > maxZoomFactor)
                    scrollView.ZoomTo(maxZoomFactor, new Point(scrollView.ScrollX, scrollView.ScrollY));
            }
        }

        /// <summary>
        /// Called when the <see cref="AllowZoom"/> property changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnAllowZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                bool allowZoom = (bool)newValue;
                scrollView.AddOrRemoveZoomGestures(allowZoom);
            }
        }

        /// <summary>
        /// Called when the horizontal scroll offset changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnHorizontalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdateHorizontalProportion((double)newValue);
            }
        }

        /// <summary>
        /// Called when the vertical scroll offset changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnVerticalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdateVerticalProportion((double)newValue);
            }
        }

        /// <summary>
        /// Called when the scroll orientation changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                ScrollOrientation orientation = (ScrollOrientation)newValue;
                if (scrollView._panZoomListener != null)
                {
                    scrollView._panZoomListener.PanMode = scrollView.ConvertOrientationToPanMode(orientation);
                }
            }
        }

        /// <summary>
        /// Called when the vertical proportional offset changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnVerticalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                if (scrollView._currentVerticalProportion != (double)newValue)
                    scrollView.ScrollToVerticalOffsetProportionately((double)newValue);
            }
        }

        /// <summary>
        /// Called when the horizontal proportional offset changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnHorizontalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                if (scrollView._currentHorizontalProportion != (double)newValue)
                    scrollView.ScrollToHorizontalOffsetProportionately((double)newValue);
            }
        }

        /// <summary>
        /// Called when the content of the scroll viewer changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdatePresentedContent((View)newValue);
            }
        }

        /// <summary>
        /// Called when the content extent size changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnExtentSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                Size extentSize = (Size)newValue;
                var presentedContent = scrollView.PresentedContent;
                if (presentedContent != null)
                {
                    presentedContent.RequestSize(extentSize.Width, extentSize.Height);
#if !WINDOWS
                    scrollView.IsContentLayoutRequested = true;
#endif
                }
            }
        }

        /// <summary>
        /// Called when the requested extent size changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnExtentSizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                Size? extentSizeRequest = (Size?)newValue;
                if (extentSizeRequest != null)
                {
                    scrollView.InvalidateExtentSize(extentSizeRequest.Value);
                }
            }
        }

        /// <summary>
        /// Handles content size changes and updates the scrollable extent.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event arguments.</param>
        void OnContentSizeChanged(object? sender, EventArgs e)
        {
            if (Content != null)
                _contentSize = Content.ComputeDesiredSize(double.PositiveInfinity, double.PositiveInfinity);

            if (ExtentSize == null)
            {
                if (!_contentSize.IsZero)
                {
                    RequestLayout(this.Bounds.Width, this.Bounds.Height);
                    InvalidateExtentSize(_contentSize);
                }
            }

            UpdateVerticalProportion(ScrollY);
            UpdateHorizontalProportion(ScrollX);
#if IOS && !MACCATALYST
            if (_rtlScrollEndRequired && this.ContentSize.Width > 0 && this.ViewportWidth > 0 && this.Bounds.Width > 0)
            {
                double endX = Math.Max(this.Bounds.Width, 0);
                ScrollToX(endX, animated: false);
                _rtlScrollEndRequired = false;
            }
#endif
        }

        #endregion
    }
}