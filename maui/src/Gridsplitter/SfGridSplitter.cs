using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Themes;
namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// A layout control that arranges content in resizable panes separated by draggable splitters.
    /// Supports pane resizing, collapse/expand operations, size constraints, RTL layouts,
    /// and keyboard and accessibility interactions.
    /// </summary>

    [ContentProperty("SplitterPanes")]
    public partial class SfGridSplitter : SfView, IParentThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="SplitterPanes"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SplitterPanesProperty = BindableProperty.Create( nameof(SplitterPanes), typeof(ObservableCollection<SplitterPane>), typeof(SfGridSplitter),
                null, BindingMode.OneWay, propertyChanged: OnSplitterPanesChanged);

        /// <summary>
        /// Identifies the <see cref="Orientation"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create( nameof(Orientation), typeof(GridSplitterOrientation), typeof(SfGridSplitter),
                GridSplitterOrientation.Horizontal, BindingMode.OneWay, propertyChanged: OnOrientationChanged);

        /// <summary>
        /// /// Identifies the <see cref="SeparatorSize"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SeparatorSizeProperty = BindableProperty.Create( nameof(SeparatorSize),typeof(double), typeof(SfGridSplitter),
                8.0, BindingMode.OneWay, propertyChanged: OnSeparatorSizeChanged);

        /// <summary>
        /// Identifies the <see cref="SeparatorBackground"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SeparatorBackgroundProperty = BindableProperty.Create( nameof(SeparatorBackground), typeof(Brush), typeof(SfGridSplitter),
                new SolidColorBrush(Color.FromArgb("#CAC4D0")), BindingMode.OneWay, propertyChanged: OnSeparatorBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="ResizeIconColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ResizeIconColorProperty = BindableProperty.Create( nameof(ResizeIconColor), typeof(Color), typeof(SfGridSplitter),
                Color.FromArgb("#49454F"), BindingMode.OneWay, propertyChanged: OnResizeIconColorChanged);

        /// <summary>
        /// Identifies the <see cref="ExpandCollapseIconColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ExpandCollapseIconColorProperty = BindableProperty.Create( nameof(ExpandCollapseIconColor), typeof(Color), typeof(SfGridSplitter),
                Color.FromArgb("#6750A4"), BindingMode.OneWay, propertyChanged: OnExpandCollapseIconColorChanged);

        /// <summary>
        /// Identifies the <see cref="ResizeIconTemplate"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ResizeIconTemplateProperty = BindableProperty.Create( nameof(ResizeIconTemplate), typeof(DataTemplate), typeof(SfGridSplitter),
                null, BindingMode.OneWay, propertyChanged: OnResizeIconTemplateChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Internal bindable property that holds the theme-aware hover background
        /// for the separator strip. Populated from
        /// <c>SfGridSplitterSeparatorHoverBackground</c> in the constructor.
        /// </summary>
        internal static readonly BindableProperty _hoverSeparatorBackgroundProperty = BindableProperty.Create(
                nameof(HoverSeparatorBackground), typeof(Brush), typeof(SfGridSplitter),
                new SolidColorBrush(Color.FromArgb("#6750A4")), BindingMode.OneWay);

        /// <summary>
        /// Internal bindable property that holds the theme-aware hover color for
        /// the resize handle. Populated from <c>SfGridSplitterResizeIconHoverColor</c>.
        /// </summary>
        internal static readonly BindableProperty _hoverResizeIconColorProperty = BindableProperty.Create(
                nameof(HoverResizeIconColor), typeof(Color), typeof(SfGridSplitter),
                Color.FromArgb("#FFFFFF"), BindingMode.OneWay);

        /// <summary>
        /// Internal bindable property that holds the theme-aware fill color for
        /// the inner circle of the floating expand/collapse buttons.
        /// Populated from <c>SfGridSplitterExpandCollapseButtonFill</c>.
        /// </summary>
        internal static readonly BindableProperty _expandCollapseButtonFillProperty = BindableProperty.Create(
                nameof(ExpandCollapseButtonFill), typeof(Color), typeof(SfGridSplitter),
                Color.FromArgb("#FFFFFF"), BindingMode.OneWay);

        #endregion

        #region Fields

        /// <summary>
        /// Internal Grid layout that manages pane positioning.
        /// </summary>
        Grid? _internalGrid;

        /// <summary>
        /// Collection of separator views (N-1 for N panes).
        /// </summary>
        List<SeparatorView> _separators = new();

        /// <summary>
        /// Per-separator pair of (leading, trailing) expand/collapse
        /// buttons. Index aligns with <see cref="_separators"/>. The
        /// buttons are added as direct children of the
        /// <see cref="SfGridSplitter"/> (which is an
        /// <see cref="IAbsoluteLayout"/>) and positioned via
        /// AbsoluteLayout.SetLayoutBounds. They are
        /// intentionally not nested in an overlay layout because
        /// <c>InputTransparent</c> on a parent layout also disables
        /// hit-testing for its children on Windows, which would
        /// prevent the buttons from receiving taps.
        /// </summary>
        List<(ExpandCollapseButton Leading, ExpandCollapseButton Trailing)> _separatorButtons = new();

        /// <summary>
        /// Returns a read-only collection of separators used to resolve taps when separators overlap.
        /// Exposed as a method to prevent modification of the internal separator list.
        /// </summary>
        internal IReadOnlyList<SeparatorView> GetSeparators() => _separators;

        /// <summary>
        /// Dictionary storing the original Size of each collapsed pane.
        /// Used for recovery when expanding a pane .
        /// </summary>
        Dictionary<SplitterPane, string> _collapsedPaneSizes = new();

        /// <summary>
        /// Tracks the previous orientation for layout rebuild optimization.
        /// </summary>
        GridSplitterOrientation _previousOrientation = GridSplitterOrientation.Horizontal;

        /// <summary>
        /// The currently active trailing pane index during an active drag.
        /// </summary>
        int _activeDraggingTrailingPaneIndex = -1;

        /// <summary>
        /// Cached leading pane size used during an active drag operation.
        /// </summary>
        double _activeLeadingPaneSize;

        /// <summary>
        /// Cached trailing pane size used during an active drag operation.
        /// </summary>
        double _activeTrailingPaneSize;

        /// <summary>
        /// Indicates whether the splitter is performing its initial load.
        /// </summary>
        bool _isInitialLoad = true;

        /// <summary>
        /// Default value of <see cref="SeparatorSize"/>. 
        /// </summary>
        const double SeparatorSizeDefault = 8.0;

        /// <summary>
        /// Minimum allowed value for <see cref="SeparatorSize"/>.
        /// </summary>
        const double SeparatorSizeMin = double.Epsilon;

        /// <summary>
        /// Default minimum size applied to middle panes when <see cref="SplitterPane.MinimumSize"/> is not set.
        /// </summary>
        internal const double DefaultNonEdgePaneMinimumSize = 58.0;

        /// <summary>
        /// Indicates that a drag has converted the pane definitions to pixel sizes.
        /// </summary>
        bool _hasUserResizedPanes;

        /// <summary>
        /// Available pane-axis length from the last nonzero arrange pass.
        /// </summary>
        double _lastArrangedPaneSpace;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfGridSplitter"/> class.
        /// </summary>
        public SfGridSplitter()
        {
            ThemeElement.InitializeThemeResources(this, "SfGridSplitterTheme");
			SplitterPanes = new ObservableCollection<SplitterPane>();
            // The floating expand/collapse buttons are real <see cref="ExpandCollapseButton"/>
            // views hosted in the overlay grid; they intentionally extend
            // outside the strip (and possibly outside the splitter's own
            // bounds) so they can be visible at the strip edges. Disable
            // clipping on the splitter itself so the buttons are not
            // truncated by the splitter's bounds.
            ClipToBounds = false;
            WireDynamicThemeResources();
        }

        /// <summary>
        /// Registers the theme-aware dynamic resources on the bindable color
        /// properties. Called from the constructor and again whenever the
        /// merged theme dictionary changes so the split-button accent and
        /// hover colors always reflect the active theme.
        /// </summary>
        void WireDynamicThemeResources()
        {
            SetDynamicResource(SeparatorBackgroundProperty, "SfGridSplitterSeparatorBackground");
            SetDynamicResource(ResizeIconColorProperty, "SfGridSplitterResizeIconColor");
            SetDynamicResource(ExpandCollapseIconColorProperty, "SfGridSplitterExpandCollapseIconColor");

            // Cache the hover-variant theme colors so the separator can pick the right one
            // at draw time based on the current interaction state.
            SetDynamicResource(_hoverSeparatorBackgroundProperty, "SfGridSplitterSeparatorHoverBackground");
            SetDynamicResource(_hoverResizeIconColorProperty, "SfGridSplitterResizeIconHoverColor");
            SetDynamicResource(_expandCollapseButtonFillProperty, "SfGridSplitterExpandCollapseButtonFill");
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the panes of the splitter.
        /// </summary>
		public ObservableCollection<SplitterPane> SplitterPanes
        {
            get => (ObservableCollection<SplitterPane>)GetValue(SplitterPanesProperty);
            set => SetValue(SplitterPanesProperty, value);
        }

        /// <summary>
        /// Gets or sets the orientation of the splitter layout.
        /// </summary>
        /// <remarks>
        /// <see cref="GridSplitterOrientation.Horizontal"/> (default): Panes arranged side-by-side (columns), separators are vertical
        /// <see cref="GridSplitterOrientation.Vertical"/>: Panes stacked vertically (rows), separators are horizontal
        /// 
        /// Changing this property rebuilds the layout.
        /// </remarks>
        public GridSplitterOrientation Orientation
        {
            get => (GridSplitterOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets the size (width or height) of each separator .
        /// </summary>
        public double SeparatorSize
        {
            get => (double)GetValue(SeparatorSizeProperty);
            set
            {
                double clamped = value;
                if (double.IsNaN(clamped) || double.IsInfinity(clamped) || clamped < SeparatorSizeMin)
                    clamped = SeparatorSizeDefault;
                SetValue(SeparatorSizeProperty, clamped);
            }
        }

        /// <summary>
        /// Gets or sets the background brush for separators in the default (non-interacting) state.
        /// </summary>
        public Brush SeparatorBackground
        {
            get => (Brush)GetValue(SeparatorBackgroundProperty);
            set => SetValue(SeparatorBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets an optional data template for custom resize icons on separators.
        /// </summary>
        public DataTemplate? ResizeIconTemplate
        {
            get => (DataTemplate?)GetValue(ResizeIconTemplateProperty);
            set => SetValue(ResizeIconTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the color used to draw the resize handle (the small bar icon
        /// drawn in the middle of the separator) and the accent line drawn down/across
        /// the separator strip in the default (non-interactive) state.
        /// </summary>
        public Color ResizeIconColor
        {
            get => (Color)GetValue(ResizeIconColorProperty);
            set => SetValue(ResizeIconColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the color used to draw the floating expand/collapse buttons
        /// and the accent line drawn down/across the separator strip in the
        /// hovered (or sticky-hovered) state.
        /// </summary>
        public Color ExpandCollapseIconColor
        {
            get => (Color)GetValue(ExpandCollapseIconColorProperty);
            set => SetValue(ExpandCollapseIconColorProperty, value);
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the theme-aware hover background for the separator strip.
        /// </summary>
        internal Brush HoverSeparatorBackground => (Brush)GetValue(_hoverSeparatorBackgroundProperty);

        /// <summary>
        /// Gets the theme-aware hover color for the resize handle.
        /// </summary>
        internal Color HoverResizeIconColor => (Color)GetValue(_hoverResizeIconColorProperty);

        /// <summary>
        /// Gets the theme-aware fill color for the inner circle of the
        /// floating expand/collapse buttons.
        /// </summary>
        internal Color ExpandCollapseButtonFill => (Color)GetValue(_expandCollapseButtonFillProperty);

        #endregion

        #region Events

        /// <summary>
        /// Raised when a resize operation is starting (pointer pressed on separator).
        /// </summary>
        public event EventHandler<GridSplitterResizeStartedEventArgs>? ResizeStarted;

        /// <summary>
        /// Raised continuously during a resize operation (pointer move).
        /// </summary>
        public event EventHandler<GridSplitterResizingEventArgs>? Resizing;

        /// <summary>
        /// Raised when a resize operation completes (pointer released).
        /// </summary>
        public event EventHandler<GridSplitterResizeStoppedEventArgs>? ResizeStopped;

        /// <summary>
        /// Raised when a pane collapse operation is starting.
        /// </summary>
        public event EventHandler<GridSplitterPaneCollapsingEventArgs>? Collapsing;

        /// <summary>
        /// Raised after a pane has been successfully collapsed.
        /// </summary>
        public event EventHandler<GridSplitterPaneCollapsedEventArgs>? Collapsed;

        /// <summary>
        /// Raised when a pane expand operation is starting.
        /// </summary>
        public event EventHandler<GridSplitterPaneExpandingEventArgs>? Expanding;

        /// <summary>
        /// Raised after a pane has been successfully expanded.
        /// </summary>
        public event EventHandler<GridSplitterPaneExpandedEventArgs>? Expanded;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new pane to the splitter.
        /// </summary>
        /// <remarks>
        /// The pane is added to the end of the <see cref="SplitterPanes"/> collection
        /// and the layout is automatically rebuilt.
        /// </remarks>
        /// <param name="pane">The pane to add. Must not be <c>null</c>.</param>
        /// <returns><c>true</c> if the pane was added; <c>false</c> if the pane is <c>null</c>.</returns>
        public void AddPane(SplitterPane pane)
        {
            if (pane != null)
            {              
                SplitterPanes.Add(pane);
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Removes a pane from the splitter.
        /// </summary>
        /// <remarks>
        /// The pane at the specified index is removed, and the layout is automatically rebuilt.
        /// </remarks>
        /// <param name="index">The zero-based index of the pane to remove.</param>
        /// <returns><c>true</c> if the pane was removed; <c>false</c> if the index was out of range.</returns>
        public void RemovePane(int index)
        {
            if (index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            SplitterPanes.RemoveAt(index);
            InvalidateMeasure();
        }

        /// <summary>
        /// Collapses the pane at the specified index.
        /// Triggers the <see cref="Collapsing"/> and <see cref="Collapsed"/> events.
        /// </summary>
        /// <param name="index">The zero-based index of the pane to collapse.</param>
        /// <returns>
        /// <c>true</c> if the collapse operation was started; otherwise, <c>false</c>.
        /// </returns>
        public async Task CollapsePane(int index)
        {
            if (index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            var pane = SplitterPanes[index];
            if (!pane.IsCollapsible || pane.IsCollapsed)
            {
                return;
            }

            await CollapsePaneAsync(index);
        }

        /// <summary>
        /// Expands the pane at the specified index.
        /// Triggers the <see cref="Expanding"/> and <see cref="Expanded"/> events.
        /// </summary>
        /// <param name="index">The zero-based index of the pane to expand.</param>
        /// <returns>
        /// <c>true</c> if the expand operation was started; otherwise, <c>false</c>.
        /// </returns>
        public async Task ExpandPane(int index)
        {
            if (index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            var pane = SplitterPanes[index];
            if (!pane.IsCollapsed)
            {
                return;
            }

            await ExpandPaneAsync(index);
        }

        #endregion

        #region internal methods

        /// <summary>
        /// Returns the theme-aware colors that the separator should use for its
        /// strip background, resize handle, and expand/collapse icons based on the
        /// current interaction state. Mirrors the pattern used by
        /// <c>OTPEntry.GetVisualState</c> to pick between normal and hover colors
        /// without relying on the framework's <c>VisualStateManager</c>.
        /// </summary>
        /// <param name="isInteractive">
        /// <c>true</c> when the separator is hovered, dragging, or sticky-hovered;
        /// <c>false</c> for the resting state.
        /// </param>
        /// <param name="separatorBackground">
        /// The brush to use for the separator strip in the resolved state.
        /// </param>
        /// <param name="resizeIconColor">
        /// The color to use for the resize handle in the resolved state.
        /// </param>
        /// <param name="expandCollapseIconColor">
        /// The color to use for the expand/collapse buttons in the resolved state.
        /// </param>
        internal void GetCurrentStateColors(
            bool isInteractive,
            out Brush separatorBackground,
            out Color resizeIconColor,
            out Color expandCollapseIconColor)
        {
            if (!IsEnabled)
            {
                separatorBackground = SeparatorBackground;
                resizeIconColor = ResizeIconColor;
                expandCollapseIconColor = ExpandCollapseIconColor;
                return;
            }

            if (isInteractive)
            {
                separatorBackground = HoverSeparatorBackground;
                resizeIconColor = HoverResizeIconColor;
                // Expand/collapse icons are only visible while hovering, so the
                // same ExpandCollapseIconColor resource applies in both states.
                expandCollapseIconColor = ExpandCollapseIconColor;
                return;
            }

            separatorBackground = SeparatorBackground;
            resizeIconColor = ResizeIconColor;
            expandCollapseIconColor = ExpandCollapseIconColor;
        }

        /// <summary>
        /// Measures the required size for the splitter and its panes.
        /// </summary>
        internal Size MeasureLayout(double widthConstraint, double heightConstraint)
        {
            if (_internalGrid == null)
            {
                return new Size(widthConstraint, heightConstraint);
            }

            return new Size(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Arranges panes and separators within the splitter bounds.
        /// </summary>
        internal void ArrangeLayout(Rect bounds)
        {
            if (_internalGrid != null)
            {
#if IOS || MACCATALYST
                SyncButtonGridPositions();
#endif
				_internalGrid?.Arrange(bounds);
            }

            ArrangeOverlay(bounds);
        }

        /// <summary>
        /// CrossPlatformArrange entry point.
        /// Arranges the splitter's child views manually so the
        /// separator strips are arranged BEFORE the floating
        /// expand/collapse buttons are positioned (the button positions
        /// depend on each separator's arranged bounds). The base
        /// <see cref="SfView.ArrangeContent"/> is then called to let
        /// the layout manager arrange the children at their
        /// <c>AbsoluteLayout.LayoutBounds</c>.
        /// </summary>
        protected override Size ArrangeContent(Rect bounds)
        {
            UpdateAbsolutePaneSizesForAvailableSpace(bounds);
            NormalizeAbsolutePaneLayout(bounds);

            if (_internalGrid != null)
            {
#if IOS || MACCATALYST
                SyncButtonGridPositions();
#endif
				_internalGrid.Arrange(bounds);
            }

            ArrangeOverlay(bounds);

            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// Redistributes the available pane-axis space across absolute-sized
        /// panes. This is required after a Windows maximize/restore arrange
        /// pass because absolute grid definitions do not automatically consume
        /// newly available area.
        /// </summary>
        void NormalizeAbsolutePaneLayout(Rect bounds)
        {
            if (_internalGrid == null)
            {
                return;
            }

            double availableSpace = (Orientation == GridSplitterOrientation.Horizontal ? bounds.Width : bounds.Height)
                - Math.Max(0, SplitterPanes.Count - 1) * SeparatorSize;
            if (availableSpace <= 0 || double.IsNaN(availableSpace))
            {
                return;
            }

            double collapsedSpace = 0;
            double visibleSpace = 0;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var definition = Orientation == GridSplitterOrientation.Horizontal
                    ? _internalGrid.ColumnDefinitions[i * 2].Width
                    : _internalGrid.RowDefinitions[i * 2].Height;

                if (SplitterPanes[i].IsCollapsed)
                {
                    collapsedSpace += definition.IsAbsolute ? definition.Value : 0;
                    continue;
                }

                // Star definitions are already remeasured by Grid. Only the
                // all-absolute layout produced by a user resize needs manual
                // normalization after a minimize/restore cycle.
                if (!definition.IsAbsolute)
                {
                    return;
                }

                visibleSpace += definition.Value;
            }

            double targetVisibleSpace = availableSpace - collapsedSpace;
            if (visibleSpace <= 0 || targetVisibleSpace <= 0 || Math.Abs(visibleSpace - targetVisibleSpace) < 0.5)
            {
                return;
            }

            double scale = targetVisibleSpace / visibleSpace;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var pane = SplitterPanes[i];
                if (pane.IsCollapsed)
                {
                    continue;
                }

                var definition = Orientation == GridSplitterOrientation.Horizontal
                    ? _internalGrid.ColumnDefinitions[i * 2].Width
                    : _internalGrid.RowDefinitions[i * 2].Height;
                double resized = Math.Max(GetEffectiveMinSize(pane, i), definition.Value * scale);
                var absolute = new GridLength(resized, GridUnitType.Absolute);

                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    _internalGrid.ColumnDefinitions[i * 2].Width = absolute;
                }
                else
                {
                    _internalGrid.RowDefinitions[i * 2].Height = absolute;
                }
            }
        }

        /// <summary>
        /// Keeps pane proportions after a user resize when the splitter is
        /// arranged at a new window size. Minimized windows can report zero
        /// space, so they do not replace the last usable measurement.
        /// </summary>
        void UpdateAbsolutePaneSizesForAvailableSpace(Rect bounds)
        {
            double availableSpace = Orientation == GridSplitterOrientation.Horizontal
                ? bounds.Width
                : bounds.Height;
            availableSpace -= Math.Max(0, SplitterPanes.Count - 1) * SeparatorSize;

            if (!_hasUserResizedPanes || availableSpace <= 0 || double.IsNaN(availableSpace))
            {
                if (availableSpace > 0)
                {
                    _lastArrangedPaneSpace = availableSpace;
                }

                return;
            }

            if (_lastArrangedPaneSpace <= 0 || Math.Abs(availableSpace - _lastArrangedPaneSpace) < 0.5)
            {
                _lastArrangedPaneSpace = availableSpace;
                return;
            }

            double scale = availableSpace / _lastArrangedPaneSpace;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var pane = SplitterPanes[i];
                if (pane.IsCollapsed || !double.TryParse(pane.Size, NumberStyles.Float, CultureInfo.InvariantCulture, out var size))
                {
                    continue;
                }

                double resized = size * scale;
                double minimum = GetEffectiveMinSize(pane, i);
                double maximum = double.IsNaN(pane.MaximumSize) || pane.MaximumSize < 0
                    ? double.PositiveInfinity
                    : pane.MaximumSize;
                resized = Math.Max(minimum, Math.Min(maximum, resized));

                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    _internalGrid!.ColumnDefinitions[i * 2].Width = new GridLength(resized, GridUnitType.Absolute);
                }
                else
                {
                    _internalGrid!.RowDefinitions[i * 2].Height = new GridLength(resized, GridUnitType.Absolute);
                }

                pane.SetValue(SplitterPane.SizeProperty, resized.ToString("0.##", CultureInfo.InvariantCulture));
            }

            _lastArrangedPaneSpace = availableSpace;
        }

        /// <summary>
        /// Positions the per-separator expand/collapse buttons in the
        /// overlay grid. The position is sourced from each separator
        /// </summary>
        void ArrangeOverlay(Rect bounds)
        {
            int count = Math.Min(_separators.Count, _separatorButtons.Count);
            for (int i = 0; i < count; i++)
            {
                var sep = _separators[i];
                var (leading, trailing) = _separatorButtons[i];
                if (sep == null)
                {
                    leading.IsVisible = false;
                    trailing.IsVisible = false;
                    continue;
                }

                bool separatorInteractive = sep.IsHovered || sep.IsDragging || sep._isStickyHovered;
                bool anyButtonInteractive = leading.IsPointerOver || leading.IsPressed
                    || trailing.IsPointerOver || trailing.IsPressed;
                bool interactive = separatorInteractive || anyButtonInteractive;

                UpdateButton(leading, sep, isLeading: true, bounds, interactive);
                UpdateButton(trailing, sep, isLeading: false, bounds, interactive);
            }
        }

#if IOS || MACCATALYST
        void SyncButtonGridPositions()
        {
            int count = Math.Min(_separators.Count, _separatorButtons.Count);
            for (int i = 0; i < count; i++)
            {
                var separator = _separators[i];
                var buttons = _separatorButtons[i];
                Grid.SetRow(buttons.Leading, Grid.GetRow(separator));
                Grid.SetColumn(buttons.Leading, Grid.GetColumn(separator));
                Grid.SetRow(buttons.Trailing, Grid.GetRow(separator));
                Grid.SetColumn(buttons.Trailing, Grid.GetColumn(separator));
            }
        }
#endif

        /// <summary>
        /// Updates a single expand/collapse button: its bounds, chevron
        /// direction, theme colors, semantics, and visibility.
        /// </summary>
        /// <param name="interactive">
        /// </param>
        /// <param name="bounds"/>
        /// <param name="button"/>
        /// <param name="isLeading"/>
        /// <param name="sep"/>
        void UpdateButton(ExpandCollapseButton button, SeparatorView sep, bool isLeading, Rect bounds, bool interactive)
        {
            if (!interactive)
            {
                if (button.IsVisible)
                {
                    button.IsVisible = false;
                }
                return;
            }

            // Determine visibility / position from the separator.
            Point center;
            bool visible;
            if (isLeading)
            {
                sep.GetLeadingButtonLayout(out center, out visible);
            }
            else
            {
                sep.GetTrailingButtonLayout(out center, out visible);
            }

            if (!visible || double.IsNaN(center.X) || double.IsNaN(center.Y))
            {
                if (button.IsVisible)
                {
                    button.IsVisible = false;
                }
                return;
            }

            // Chevron direction.
            bool isHorizontal = Orientation == GridSplitterOrientation.Horizontal;
            bool showLeft, showRight, showUp, showDown;
            if (isLeading)
            {
                sep.GetLeadingButtonChevron(isHorizontal, out showLeft, out showRight, out showUp, out showDown);
            }
            else
            {
                sep.GetTrailingButtonChevron(isHorizontal, out showLeft, out showRight, out showUp, out showDown);
            }
            button.ShowLeftChevron = showLeft;
            button.ShowRightChevron = showRight;
            button.ShowUpChevron = showUp;
            button.ShowDownChevron = showDown;

            // Theme colors: keep the button colors in sync with the
            // splitter's accent / fill resources.
            button.BorderColor = ExpandCollapseIconColor;
            button.FillColor = ExpandCollapseButtonFill;
            button.ArrowColor = ExpandCollapseIconColor;

            // Accessibility metadata.
            string name, help;
            if (isLeading)
            {
                sep.GetLeadingButtonSemantics(out name, out help);
            }
            else
            {
                sep.GetTrailingButtonSemantics(out name, out help);
            }
            if (button.SemanticName != name)
            {
                button.SemanticName = name;
            }
            if (button.SemanticHelp != help)
            {
                button.SemanticHelp = help;
            }
            button.RefreshSemantics();

            // Position the button so its center matches the separator's
            // reported center. The button is a fixed size
            // (ButtonTotalSize x ButtonTotalSize) which is
            // ButtonDiameter + 2 * ButtonStrokePadding so the stroked
            // circle is never clipped by the view's bounds on any
            // platform. The overlay positions the button via the
            // bounds of an AbsoluteLayout.
            float size = ExpandCollapseButton.ButtonTotalSize;
            float x = (float)(center.X - size / 2.0);
            float y = (float)(center.Y - size / 2.0);
#if IOS || MACCATALYST
            if (button.Parent == _internalGrid)
            {
                // Keep the button in the separator's grid cell. Translation
                // is relative to that cell, so the separator and button are
                // committed by the same native layout pass.
                double separatorCenterX = sep.X + (sep.Bounds.Width / 2.0);
                double separatorCenterY = sep.Y + (sep.Bounds.Height / 2.0);
                button.TranslationX = center.X - separatorCenterX;
                button.TranslationY = center.Y - separatorCenterY;
            }
            else
            {
                AbsoluteLayout.SetLayoutBounds(button, new Rect(x, y, size, size));
            }
#else
            var buttonBounds = new Rect(x, y, size, size);
            AbsoluteLayout.SetLayoutBounds(button, buttonBounds);
#endif

            if (!button.IsVisible)
            {
                button.IsVisible = true;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the internal Grid in place to reflect the current <see cref="SplitterPanes"/> layout.
        /// Avoids recreating the Grid on platforms where doing so can cause initialization issues,
        /// and falls back to <see cref="RebuildLayout"/> when the Grid has not yet been created.
        /// </summary>
        void UpdateLayoutForPaneChange()
        {
            if (_internalGrid == null)
            {
                RebuildLayout();
                return;
            }

            if (SplitterPanes.Count == 0)
            {
                // No panes left — clear the layout in place.
                _internalGrid.Children.Clear();
                _internalGrid.RowDefinitions.Clear();
                _internalGrid.ColumnDefinitions.Clear();
                _separators.Clear();
                _separatorButtons.Clear();
                return;
            }

            var paneSizes = new GridLength[SplitterPanes.Count];
            for (int i = 0; i < SplitterPanes.Count; i++)
                paneSizes[i] = ResolveInitialPaneGridLength(SplitterPanes[i]);
            double sepSize = SeparatorSize;

            _internalGrid.Children.Clear();
            _internalGrid.RowDefinitions.Clear();
            _internalGrid.ColumnDefinitions.Clear();
            _separators.Clear();
            _separatorButtons.Clear();

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                // Build horizontal: 1 row + alternating pane/separator columns.
                _internalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = paneSizes[i] });

                    SplitterPanes[i].ZIndex = 0;
                    Grid.SetRow(SplitterPanes[i], 0);
                    Grid.SetColumn(SplitterPanes[i], i * 2);
					_internalGrid.Children.Add(SplitterPanes[i]);

                    ApplyInitialCollapseState(SplitterPanes[i]);

                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.ColumnDefinitions.Add(new ColumnDefinition
                        {
                            Width = new GridLength(sepSize, GridUnitType.Absolute)
                        });

                        var separator = CreateSeparator(i + 1);
                        _separators.Add(separator);

                        Grid.SetRow(separator, 0);
                        Grid.SetColumn(separator, i * 2 + 1);
                        _internalGrid.Children.Add(separator);
                    }
                }
            }
            else
            {
                _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    _internalGrid.RowDefinitions.Add(new RowDefinition { Height = paneSizes[i] });

                    SplitterPanes[i].ZIndex = 0;
                    Grid.SetRow(SplitterPanes[i], i * 2);
                    Grid.SetColumn(SplitterPanes[i], 0);
                    _internalGrid.Children.Add(SplitterPanes[i]);


                    ApplyInitialCollapseState(SplitterPanes[i]);

                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.RowDefinitions.Add(new RowDefinition
                        {
                            Height = new GridLength(sepSize, GridUnitType.Absolute)
                        });

                        var separator = CreateSeparator(i + 1);
                        _separators.Add(separator);

                        Grid.SetRow(separator, i * 2 + 1);
                        Grid.SetColumn(separator, 0);
                        _internalGrid.Children.Add(separator);
                    }
                }
            }

            InvalidateMeasure();
        }

        /// <summary>
        /// Creates and configures a <see cref="SeparatorView"/> with resize handlers and the current
        /// <see cref="ResizeIconTemplate"/>, ready to be added to the layout.
        /// </summary>
        /// <param name="trailingPaneIndex">
        /// The index of the pane immediately following the separator.
        /// </param>
        /// <returns>The configured separator.</returns>
        SeparatorView CreateSeparator(int trailingPaneIndex)
        {
            var separator = new SeparatorView
            {
                TrailingPaneIndex = trailingPaneIndex,
                ZIndex = 10,
                AutomationId = string.IsNullOrEmpty(AutomationId) ? $"GridSplitterSeparator{trailingPaneIndex}" : $"{AutomationId}_Separator{trailingPaneIndex}"
			};
			SemanticProperties.SetDescription(separator, string.IsNullOrEmpty(AutomationId) ? $"Grid splitter separator between pane {trailingPaneIndex - 1} and pane {trailingPaneIndex}"
							: $"{AutomationId}_Separator{trailingPaneIndex}");
			separator.SetDragCallbacks( dragStart: (paneIndex) => OnDragStart(paneIndex), dragDelta: (paneIndex, delta) => OnDragDelta(paneIndex, delta),
                dragStop: (paneIndex) => OnDragStop(paneIndex));

            ApplyResizeIconTemplateToSeparator(separator);

            // The expand/collapse buttons are no longer drawn inside the
            // separator. They are real <see cref="ExpandCollapseButton"/>
            // views hosted in the splitter's overlay grid; we create them
            // here so they share the lifetime of the separator and can
            // reuse the separator's per-action helpers.
            var leadingButton = CreateExpandCollapseButton(isLeading: true, owner: separator);
            var trailingButton = CreateExpandCollapseButton(isLeading: false, owner: separator);

            // Buttons are hidden until the user hovers / drags the strip.
            // Show them only during the interactive state; otherwise the
            // overlay is empty and the splitter passthrough works
            // normally.
            leadingButton.IsVisible = false;
            trailingButton.IsVisible = false;

            // Add the buttons as direct children of the splitter. The
            // splitter is an IAbsoluteLayout so each button is
            // positioned via AbsoluteLayout.SetLayoutBounds. They are
            // added AFTER the internal grid in Children, which makes
            // them render on top in the platform layout (Panel
            // hit-tests topmost first, so taps on a button reach the
            // button, not the underlying pane). The bounds use
            // ButtonTotalSize (ButtonDiameter + 2*ButtonStrokePadding)
            // so the stroked circle is never clipped by the view's
            // bounds on any platform.
            float buttonSize = ExpandCollapseButton.ButtonTotalSize;
            AbsoluteLayout.SetLayoutBounds(leadingButton, new Rect(0, 0, buttonSize, buttonSize));
            AbsoluteLayout.SetLayoutFlags(leadingButton, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(trailingButton, new Rect(0, 0, buttonSize, buttonSize));
            AbsoluteLayout.SetLayoutFlags(trailingButton, AbsoluteLayoutFlags.None);

#if IOS || MACCATALYST
            _internalGrid?.Children.Add(leadingButton);
            _internalGrid?.Children.Add(trailingButton);
#else
            Children.Add(leadingButton);
            Children.Add(trailingButton);
#endif

            _separatorButtons.Add((leadingButton, trailingButton));

            return separator;
        }

        /// <summary>
        /// Creates an <see cref="ExpandCollapseButton"/> for the given
        /// separator, wires its <c>Clicked</c> handler to the separator's
        /// action, and applies the current theme colors.
        /// </summary>
        /// <param name="isLeading">
        /// <c>true</c> to build the leading (left/top) button;
        /// <c>false</c> for the trailing (right/bottom) button.
        /// </param>
        /// <param name="owner">
        /// The owning <see cref="SeparatorView"/> whose state drives the
        /// button's visibility, position, and click handler.
        /// </param>
        ExpandCollapseButton CreateExpandCollapseButton(bool isLeading, SeparatorView owner)
        {
            var button = new ExpandCollapseButton
            {
                IsLeadingSide = isLeading,
                WidthRequest = ExpandCollapseButton.ButtonTotalSize,
                HeightRequest = ExpandCollapseButton.ButtonTotalSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BorderColor = ExpandCollapseIconColor,
                FillColor = ExpandCollapseButtonFill,
                ArrowColor = ExpandCollapseIconColor,
				AutomationId = string.IsNullOrEmpty(AutomationId) ? $"GridSplitterSeparator{owner.TrailingPaneIndex}{(isLeading ? "Leading" : "Trailing")}Button"
										: $"{AutomationId}_Separator{owner.TrailingPaneIndex}_{(isLeading ? "Leading" : "Trailing")}Button",
				// Buttons sit above the strip and the resize handle.
				ZIndex = 20
            };
			SemanticProperties.SetDescription( button,string.IsNullOrEmpty(AutomationId)
								? $"{(isLeading ? "Leading" : "Trailing")} expand collapse button for separator {owner.TrailingPaneIndex}"
								: $"{AutomationId} {(isLeading ? "Leading" : "Trailing")} expand collapse button associated with separator {owner.TrailingPaneIndex}");

            // Default bounds; the splitter updates the bounds during
            // arrange using the separator's reported center. The
            // CreateSeparator caller is responsible for adding the
            // button to the splitter's Children collection.

            if (isLeading)
            {
                button.Clicked += (s, e) => owner.OnLeadingButtonClicked();
            }
            else
            {
                button.Clicked += (s, e) => owner.OnTrailingButtonClicked();
            }

            // Re-arrange the overlay when the button's pointer-over
            // state changes, so the buttons stay visible while the
            // user moves the pointer from the strip onto the button
            // and hide when the user moves away from both. Invalidate
            // BOTH measure and arrange to ensure the layout pass runs
            // on every pointer transition (some platforms only re-
            // measure but not re-arrange on InvalidateMeasure alone).
            // Also cancel any pending hover-hide timer on the owner
            // separator so the buttons stay visible while the user
            // moves the pointer from the strip onto the button.
            //
            // On EXIT (IsPointerOver == false) schedule a delayed
            // hide so the user has a brief window to move the cursor
            // back onto the strip or the other button. The grace
            // window matches the strip's ScheduleHoverHide grace so
            // the two paths are consistent and the buttons stay
            // visible as long as the pointer is over either the
            // strip, the bridge, or any visible button.
            button.PointerOverChanged += (s, e) =>
            {
                if (button.IsPointerOver)
                {
                    owner.CancelScheduledHoverHide();
                }
                else
                {
                    owner.ScheduleHoverHideFromButtonExit();
                }
                InvalidateMeasure();
                InvalidateDrawable();
            };

            return button;
        }

        /// <summary>
        /// Instantiates the current <see cref="ResizeIconTemplate"/> and adds it to the internal grid at the separator's position.
        /// If no valid template is available, the separator displays its built-in resize handle.
        /// </summary>
        /// <param name="separator">
        /// The separator for which the template view is created.
        /// </param>
        void ApplyResizeIconTemplateToSeparator(SeparatorView separator)
        {
            if (_internalGrid == null)
            {
                return;
            }

            var oldTemplateView = separator.GetAndClearTemplateView();
            if (oldTemplateView != null && _internalGrid.Children.Contains(oldTemplateView))
            {
                _internalGrid.Children.Remove(oldTemplateView);
            }

            var templateView = separator.SetResizeIconTemplate(ResizeIconTemplate);
            if (templateView == null)
                return;

            int row = Grid.GetRow(separator);
            int col = Grid.GetColumn(separator);
            Grid.SetRow(templateView, row);
            Grid.SetColumn(templateView, col);
            templateView.ZIndex = separator.ZIndex;
            _internalGrid.Children.Add(templateView);
            separator.UpdateResizeIconTemplateVisibility();
        }

        /// <summary>
        /// Updates the internal Grid to match the current <see cref="Orientation"/> without recreating the visual tree.
        /// Repositions panes and separators while preserving the existing Grid instance.
        /// </summary>
        void UpdateLayoutForOrientation()
        {
            if (_internalGrid == null || SplitterPanes.Count == 0)
            {
                RebuildLayout();
                return;
            }

            if (Orientation == _previousOrientation)
            {
                return;
            }

            bool wasHorizontal = _previousOrientation == GridSplitterOrientation.Horizontal;
            bool nowHorizontal = Orientation == GridSplitterOrientation.Horizontal;

            if (wasHorizontal == nowHorizontal)
                return;

            var paneBounds = new double[SplitterPanes.Count];
            double totalPaneBounds = 0;
            bool anyPaneHasBounds = false;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                double bound = wasHorizontal ? SplitterPanes[i].Bounds.Width : SplitterPanes[i].Bounds.Height;
                if (bound > 0)
                {
                    paneBounds[i] = bound;
                    totalPaneBounds += bound;
                    anyPaneHasBounds = true;
                }
                else
                {
                    paneBounds[i] = 0;
                }
            }

            var paneSizes = new GridLength[SplitterPanes.Count];
            if (anyPaneHasBounds && totalPaneBounds > 0)
            {
                double accumulated = 0;
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    if(SplitterPanes[i].IsCollapsed)
                    {
                        paneSizes[i] = new GridLength(0, 0);
                    }
                    else if (i == SplitterPanes.Count - 1 )
                    {
                        // Last pane: snap so weights sum to exactly 1.0.
                        paneSizes[i] = new GridLength(Math.Max(0, 1.0 - accumulated), GridUnitType.Star);
                    }
                    else
                    {
                        double weight = paneBounds[i] / totalPaneBounds;
                        paneSizes[i] = new GridLength(weight, GridUnitType.Star);
                        accumulated += weight;
                    }
                }
            }
            else
            {

                double absoluteSum = 0;
                int starCount = 0;
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    var gl = ParseGridLength(SplitterPanes[i].Size);
                    if (gl.IsAbsolute)
                    {
                        absoluteSum += gl.Value;
                    }
                    else if (gl.IsStar)
                    {
                        starCount += (int)Math.Max(1, gl.Value);
                    }
                }
                double totalWeight = absoluteSum + starCount;
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    var gl = ParseGridLength(SplitterPanes[i].Size);
                    double weight;
                    if (gl.IsAbsolute)
                    {
                        weight = totalWeight > 0 ? gl.Value / totalWeight : 0;
                    }
                    else if (gl.IsStar)
                    {
                        weight = totalWeight > 0 ? Math.Max(1, gl.Value) / totalWeight : 0;
                    }
                    else
                    {
                        weight = 0;
                    }
                    paneSizes[i] = new GridLength(weight, GridUnitType.Star);
                }
            }

            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var pane = SplitterPanes[i];
                if (pane == null)
                {
                    continue;
                }
                if (double.TryParse(pane.Size, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                {
                    pane.SetValue(SplitterPane.SizeProperty, "*");
                }
            }

            double sepSize = SeparatorSize;

            // Clear and rebuild the definitions on the correct axis.
            _internalGrid.RowDefinitions.Clear();
            _internalGrid.ColumnDefinitions.Clear();

            if (nowHorizontal)
            {
                // Build horizontal: 1 row + alternating pane/separator columns.
                _internalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = paneSizes[i] });
                    Grid.SetRow(SplitterPanes[i], 0);
                    Grid.SetColumn(SplitterPanes[i], i * 2);
                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.ColumnDefinitions.Add(new ColumnDefinition
                        {
                            Width = new GridLength(sepSize, GridUnitType.Absolute)
                        });
                        Grid.SetRow(_separators[i], 0);
                        Grid.SetColumn(_separators[i], i * 2 + 1);
                    }
                }
            }
            else
            {
                // Build vertical: 1 column + alternating pane/separator rows.
                _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    _internalGrid.RowDefinitions.Add(new RowDefinition { Height = paneSizes[i] });
                    Grid.SetRow(SplitterPanes[i], i * 2);
                    Grid.SetColumn(SplitterPanes[i], 0);
                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.RowDefinitions.Add(new RowDefinition
                        {
                            Height = new GridLength(sepSize, GridUnitType.Absolute)
                        });
                        Grid.SetRow(_separators[i], i * 2 + 1);
                        Grid.SetColumn(_separators[i], 0);
                    }
                }
            }

            _previousOrientation = Orientation;
        }

        /// <summary>
        /// Rebuilds the internal Grid layout based on current panes and orientation.
        /// Generates internal Grid with correct row/column definitions.
        /// </summary>
        void RebuildLayout()
        {
            if (SplitterPanes.Count == 0)
            {
                // Clear layout if no panes
                Children.Clear();
                _internalGrid = null;
                _separators.Clear();
                _separatorButtons.Clear();
                return;
            }

            // Clear all children first (panes were added as direct children by ContentProperty)
            Children.Clear();

            _internalGrid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = 0
            };

            _separators.Clear();
            _separatorButtons.Clear();

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                BuildHorizontalLayout();
            }
            else
            {
                BuildVerticalLayout();
            }

            _previousOrientation = Orientation;

            // Add the internal grid first (pane / separator columns).
            // The floating expand / collapse buttons are added as
            // direct children of the splitter by CreateSeparator,
            // AFTER the internal grid, so they render on top.
            Children.Add(_internalGrid);
		}

        /// <summary>
        /// Builds horizontal layout: 1 row, alternating columns for panes and separators.
        /// </summary>
        void BuildHorizontalLayout()
        {
            if (_internalGrid == null || SplitterPanes.Count == 0)
            {
                return;
            }

            // Horizontal: 1 row, N panes + (N-1) separators in columns
            _internalGrid.RowDefinitions.Clear();
            _internalGrid.ColumnDefinitions.Clear();
            _internalGrid.Children.Clear();

            var rowDef = new RowDefinition { Height = GridLength.Star };
            _internalGrid.RowDefinitions.Add(rowDef);

            // Add column definitions: pane, separator, pane, separator, ..., pane
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var paneSize = ResolveInitialPaneGridLength(SplitterPanes[i]);
                _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = paneSize });

                // Add pane to grid
                Grid.SetRow(SplitterPanes[i], 0);
                Grid.SetColumn(SplitterPanes[i], i * 2); // Panes at even columns
                SplitterPanes[i].ZIndex = 0;
				var pane = SplitterPanes[i];
				_internalGrid.Children.Add(SplitterPanes[i]);

                ApplyInitialCollapseState(SplitterPanes[i]);

                // Separator column (except after last pane)
                if (i < SplitterPanes.Count - 1)
                {
                    _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = SeparatorSize });

                    var separator = CreateSeparator(i + 1);  // Separator between pane i and i+1
                    _separators.Add(separator);

                    Grid.SetRow(separator, 0);
                    Grid.SetColumn(separator, i * 2 + 1); // Separators at odd columns

                    separator.ZIndex = 10;
                    _internalGrid.Children.Add(separator);
				}
            }
        }

        /// <summary>
        /// Builds vertical layout: 1 column, alternating rows for panes and separators.
        /// </summary>
        void BuildVerticalLayout()
        {
            if (_internalGrid == null || SplitterPanes.Count == 0)
            {
                return;
            }

            // Vertical: 1 column, N panes + (N-1) separators in rows
            _internalGrid.RowDefinitions.Clear();
            _internalGrid.ColumnDefinitions.Clear();
            _internalGrid.Children.Clear();

            var colDef = new ColumnDefinition { Width = GridLength.Star };
            _internalGrid.ColumnDefinitions.Add(colDef);

            // Add row definitions: pane, separator, pane, separator, ..., pane
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var paneSize = ResolveInitialPaneGridLength(SplitterPanes[i]);
                _internalGrid.RowDefinitions.Add(new RowDefinition { Height = paneSize });

                // Add pane to grid
                Grid.SetRow(SplitterPanes[i], i * 2); // Panes at even rows
                Grid.SetColumn(SplitterPanes[i], 0);
                SplitterPanes[i].ZIndex = 0;
                _internalGrid.Children.Add(SplitterPanes[i]);

                ApplyInitialCollapseState(SplitterPanes[i]);

                // Separator row (except after last pane)
                if (i < SplitterPanes.Count - 1)
                {
                    _internalGrid.RowDefinitions.Add(new RowDefinition { Height = SeparatorSize });

                    var separator = CreateSeparator(i + 1);  // Separator between pane i and i+1
                    _separators.Add(separator);

                    Grid.SetRow(separator, i * 2 + 1); // Separators at odd rows
                    Grid.SetColumn(separator, 0);
                    _internalGrid.Children.Add(separator);
                }
            }
        }

        /// <summary>
        /// Parses a <see cref="GridLength"/> from <see cref="SplitterPane.Size"/>, supporting star and absolute sizes.
        /// Invalid values fall back to a default star size.
        /// </summary>
        static GridLength ParseGridLength(string? sizeStr)
        {
            if (string.IsNullOrWhiteSpace(sizeStr))
            {
                return new GridLength(1, GridUnitType.Star);
            }

            // Parse star weights ("*", "1*", "2*", etc.) and absolute values
            sizeStr = sizeStr.Trim();

            if (sizeStr.EndsWith("*"))
            {
                // Star weight
                var weightStr = sizeStr.Substring(0, sizeStr.Length - 1);
                if (double.TryParse(weightStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
                {
                    return new GridLength(Math.Max(0, weight), GridUnitType.Star);
                }
                return new GridLength(1, GridUnitType.Star);
            }

            if (double.TryParse(sizeStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) && value >= 0)
            {
                return new GridLength(value, GridUnitType.Absolute);
            }

            // Default to 1 star
            return new GridLength(1, GridUnitType.Star);
        }

        /// <summary>
        /// Converts a <see cref="GridLength"/> into an absolute pixel size.
        /// </summary>
        /// <param name="gridLength">The grid length to convert.</param>
        /// <param name="availableStarSpace">The available space for star-sized columns/rows.</param>
        /// <param name="totalStarWeight">The total star weight across all star-sized columns/rows.</param>
        /// <returns>The approximate absolute size for the grid length.</returns>
        static double ConvertGridLengthToAbsolute(GridLength gridLength, double availableStarSpace, double totalStarWeight)
        {
            if (gridLength.IsAbsolute)
            {
                return gridLength.Value;
            }

            if (gridLength.IsAuto)
            {
                return 0;
            }

            if (gridLength.IsStar)
            {
                if (double.IsNaN(availableStarSpace) || availableStarSpace <= 0 || totalStarWeight <= 0)
                {
                    return 0;
                }

                return Math.Max(0, availableStarSpace * gridLength.Value / totalStarWeight);
            }

            return 0;
        }


        /// <summary>
        /// Collapses a pane with animation, updates its state, and raises the
        /// <see cref="Collapsing"/> and <see cref="Collapsed"/> events.
        /// </summary>
        async Task CollapsePaneAsync(int index)
        {
            if (index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            if (Orientation == GridSplitterOrientation.Vertical)
            {
                ConvertAllPanesToAbsolute(GridSplitterOrientation.Vertical);
            }
            
            var pane = SplitterPanes[index];

            int adjacentPaneIndex = index < SplitterPanes.Count - 1 ? index + 1 : index - 1;
            SplitterPane? adjacentPane = adjacentPaneIndex >= 0 && adjacentPaneIndex < SplitterPanes.Count ? SplitterPanes[adjacentPaneIndex] : null;

            string currentSize = pane.Size;

            var collapsingIndices = new int[] { index, adjacentPaneIndex };
            var collapsingPanes = adjacentPane != null ? new SplitterPane[] { pane, adjacentPane } : new SplitterPane[] { pane };

            var args = new GridSplitterPaneCollapsingEventArgs(collapsingIndices, collapsingPanes);
            Collapsing?.Invoke(this, args);

            if (args.Cancel)
            {
                return;
            }

            _collapsedPaneSizes[pane] = currentSize;
            pane.IsCollapsed = true;
            pane.Size = "0";
            UpdatePaneVisibility(pane, isCollapsed: true);
            ApplyPaneSizeToGrid(pane, index);
            ApplyAllPaneSizesToGrid();

            // After a manual drag, all panes are absolute-sized. Collapsing
            // a pane to Absolute(0) then leaves a gap in the layout because
            // the natural star redistribution no longer applies. Give the
            // freed size to a visible absolute neighbor so no blank space
            // remains. No-op when the siblings are star-sized, preserving
            // the pre-drag collapse/expand behavior.
            RedistributeCollapsedPaneSize(index);

            // Fire Collapsed event with both affected panes and the previous size.
            var collapsedIndices = new int[] { index, adjacentPaneIndex };
            var collapsedPanes = adjacentPane != null ? new SplitterPane[] { pane, adjacentPane } : new SplitterPane[] { pane };

            Collapsed?.Invoke(this, new GridSplitterPaneCollapsedEventArgs(collapsedIndices, collapsedPanes));

            // Just refresh the separators and the measure pass; do not rebuild the layout.
            InvalidateSeparators();
            ResetInteractionVisualState();
            InvalidateMeasure();
        }

        /// <summary>
        /// Expands a pane by restoring its size, updating its state,
        /// and raising the <see cref="Expanding"/> and <see cref="Expanded"/> events.
        /// </summary>
        async Task ExpandPaneAsync(int index)
        {
            if (index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            var pane = SplitterPanes[index];

            int adjacentPaneIndex = index < SplitterPanes.Count - 1 ? index + 1 : index - 1;
            SplitterPane? adjacentPane = adjacentPaneIndex >= 0 && adjacentPaneIndex < SplitterPanes.Count ? SplitterPanes[adjacentPaneIndex] : null;

            // Retrieve the previous size BEFORE any state mutation.
            var previousSize = _collapsedPaneSizes.TryGetValue(pane, out var size) ? size : "*";

            // Fire Expanding event with both affected panes and the size we are restoring to.
            var expandingIndices = new int[] { index, adjacentPaneIndex };
            var expandingPanes = adjacentPane != null ? new SplitterPane[] { pane, adjacentPane } : new SplitterPane[] { pane };

            var args = new GridSplitterPaneExpandingEventArgs(expandingIndices, expandingPanes);
            Expanding?.Invoke(this, args);

            if (args.Cancel)
            {
                return;
            }

            pane.IsCollapsed = false;
            pane.Size = previousSize;
            UpdatePaneVisibility(pane, isCollapsed: false);
            ApplyPaneSizeToGrid(pane, index);
            ApplyAllPaneSizesToGrid();

            // After a manual drag, all panes are absolute-sized. Restoring
            // an absolute size here (e.g. "300") next to absolute siblings
            // (e.g. "200" + "200") would push the total over the
            // splitter's width, causing the visible "empty pane + unused
            // space" symptom. Scale the restored column down to the
            // remaining absolute budget so the layout stays valid. No-op
            // when the siblings are star-sized (pre-drag path), preserving
            // the existing behavior.
            BalanceExpandedPaneSize(index);

            _collapsedPaneSizes.Remove(pane);

            // Fire Expanded event with both affected panes and the restored size.
            var expandedIndices = new int[] { index, adjacentPaneIndex };
            var expandedPanes = adjacentPane != null ? new SplitterPane[] { pane, adjacentPane } : new SplitterPane[] { pane };

            Expanded?.Invoke(this, new GridSplitterPaneExpandedEventArgs(expandedIndices, expandedPanes));

            // Just refresh the separators and the measure pass; do not rebuild the layout.
            InvalidateSeparators();
            ResetInteractionVisualState();
            InvalidateMeasure();
        }

        void ResetInteractionVisualState()
        {
            ForceClearAllSeparatorsHoverState();
            foreach (var buttonPair in _separatorButtons)
            {
                buttonPair.Leading.ResetInteractionState();
                buttonPair.Trailing.ResetInteractionState();
                buttonPair.Leading.IsVisible = false;
                buttonPair.Trailing.IsVisible = false;
            }
        }

        /// <summary>
        /// Updates the pane's visibility so collapsed panes are hidden without being
        /// removed from the splitter tree, while expanded panes are restored.
        /// </summary>
        void UpdatePaneVisibility(SplitterPane pane, bool isCollapsed)
        {
            pane.IsVisible = !isCollapsed;

            if (pane.Content != null)
            {
                pane.Content.IsVisible = !isCollapsed;
                pane.Content.Opacity = isCollapsed ? 0 : 1;
            }
        }

        /// <summary>
        /// Applies the size string of the given pane (at the given index) to the
        /// matching column/row definition of the internal grid.
        /// </summary>
        void ApplyPaneSizeToGrid(SplitterPane pane, int index)
        {
            if (_internalGrid == null)
            {
                return;
            }

            var gridLength = ParseGridLength(pane.Size);

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                int colIndex = index * 2;
                if (colIndex >= 0 && colIndex < _internalGrid.ColumnDefinitions.Count)
                {
                    _internalGrid.ColumnDefinitions[colIndex].Width = gridLength;
                }
            }
            else
            {
                int rowIndex = index * 2;
                if (rowIndex >= 0 && rowIndex < _internalGrid.RowDefinitions.Count)
                {
                    _internalGrid.RowDefinitions[rowIndex].Height = gridLength;
                }
            }
        }

        /// <summary>
        /// Applies an explicit <see cref="GridLength"/> to the pane's column or row
        /// definition, ignoring the pane's current <see cref="SplitterPane.Size"/>
        /// property. Used by the <see cref="SplitterPane.IsCollapsed"/> handler to
        /// force a collapsed pane's width to 0 (so the adjacent pane automatically
        /// takes the freed space) without depending on the user having changed the
        /// pane's <c>Size</c> property.
        /// </summary>
        void ApplyPaneSizeToGridWithOverride(SplitterPane pane, int index, GridLength gridLength)
        {
            if (_internalGrid == null)
            {
                return;
            }

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                int colIndex = index * 2;
                if (colIndex >= 0 && colIndex < _internalGrid.ColumnDefinitions.Count)
                {
                    _internalGrid.ColumnDefinitions[colIndex].Width = gridLength;
                }
            }
            else
            {
                int rowIndex = index * 2;
                if (rowIndex >= 0 && rowIndex < _internalGrid.RowDefinitions.Count)
                {
                    _internalGrid.RowDefinitions[rowIndex].Height = gridLength;
                }
            }
		}

        GridLength GetPaneLayoutLength(SplitterPane pane, int index)
        {
            if (!pane.IsCollapsed)
            {
                return ParseGridLength(pane.Size);
            }

            if (index > 0 && index < SplitterPanes.Count - 1)
            {
                return new GridLength(SeparatorView.AdjacentSeparatorGapDiu, GridUnitType.Absolute);
            }

            return new GridLength(0, GridUnitType.Absolute);
        }

        /// <summary>
        /// Reapplies all pane sizes to the internal grid to refresh layout after size or collapse state changes.
        /// Ensures collapsed panes remain hidden and visible panes correctly use the available space.
        /// </summary>
		void ApplyAllPaneSizesToGrid()
        {
            if (_internalGrid == null)
            {
                return;
            }

            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                var pane = SplitterPanes[i];
                var gridLength = GetPaneLayoutLength(pane, i);

                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    int colIndex = i * 2;
                    if (colIndex >= 0 && colIndex < _internalGrid.ColumnDefinitions.Count)
                    {
                        _internalGrid.ColumnDefinitions[colIndex].Width = gridLength;
                    }
                }
                else
                {
                    int rowIndex = i * 2;
                    if (rowIndex >= 0 && rowIndex < _internalGrid.RowDefinitions.Count)
                    {
                        _internalGrid.RowDefinitions[rowIndex].Height = gridLength;
                    }
                }
            }
        }

        /// <summary>
        /// Gives the freed absolute size of a collapsed pane to a visible
        /// (non-collapsed) neighbor so the layout does not leave a gap when
        /// the surrounding panes are absolute-sized (e.g. after a manual drag
        /// converted all panes to absolute pixel values).
        ///
        /// This is a no-op when the neighbors are star-sized because the
        /// natural star math already redistributes the freed space.
        /// Does not mutate <see cref="SplitterPane.Size"/>; only the internal
        /// grid column/row width is adjusted, preserving the public surface.
        /// </summary>
        /// <param name="collapsedIndex">
        /// Index of the collapsed pane whose freed size should be redistributed.
        /// </param>
        void RedistributeCollapsedPaneSize(int collapsedIndex)
        {
            if (_internalGrid == null || collapsedIndex < 0 || collapsedIndex >= SplitterPanes.Count)
            {
                return;
            }

            if (!SplitterPanes[collapsedIndex].IsCollapsed)
            {
                return;
            }

            // The collapsed pane must have a remembered absolute size to
            // redistribute. Star-sized collapsed panes are handled by the
            // natural star math and need no compensation.
            if (!_collapsedPaneSizes.TryGetValue(SplitterPanes[collapsedIndex], out var savedSize))
            {
                return;
            }

            if (!double.TryParse(savedSize, NumberStyles.Float, CultureInfo.InvariantCulture, out var freedSize) || freedSize <= 0)
            {
                return;
            }

            // Find the closest visible (non-collapsed) neighbor and only act
            // when that neighbor's column/row is absolute-sized. This
            // restricts the compensation to the post-drag layout where
            // siblings are pinned to pixel values; the pre-drag star-sized
            // case is left to Grid's natural star redistribution.
            int targetIndex = -1;
            for (int offset = 1; offset < SplitterPanes.Count; offset++)
            {
                int candidateLeft = collapsedIndex - offset;
                int candidateRight = collapsedIndex + offset;

                bool leftInRange = candidateLeft >= 0;
                bool rightInRange = candidateRight < SplitterPanes.Count;

                // Skip collapsed panes on either side so we land on the
                // nearest VISIBLE neighbor.
                if (leftInRange && SplitterPanes[candidateLeft].IsCollapsed)
                {
                    leftInRange = false;
                }

                if (rightInRange && SplitterPanes[candidateRight].IsCollapsed)
                {
                    rightInRange = false;
                }

                if (!leftInRange && !rightInRange)
                {
                    continue;
                }

                // Prefer the absolute-sized side; if neither is absolute
                // (e.g. mixed star/absolute layout), pick the first
                // available neighbor so we still absorb the freed space
                // when at least one side is absolute.
                if (rightInRange && IsPaneColumnAbsolute(candidateRight))
                {
                    targetIndex = candidateRight;
                }
                else if (leftInRange && IsPaneColumnAbsolute(candidateLeft))
                {
                    targetIndex = candidateLeft;
                }
                else if (rightInRange)
                {
                    targetIndex = candidateRight;
                }
                else
                {
                    targetIndex = candidateLeft;
                }

                break;
            }

            if (targetIndex < 0)
            {
                return;
            }

            int targetColIndex = targetIndex * 2;
            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                if (targetColIndex < 0 || targetColIndex >= _internalGrid.ColumnDefinitions.Count)
                {
                    return;
                }

                double current = _internalGrid.ColumnDefinitions[targetColIndex].Width.Value;
                _internalGrid.ColumnDefinitions[targetColIndex].Width = new GridLength(current + freedSize, GridUnitType.Absolute);
            }
            else
            {
                if (targetColIndex < 0 || targetColIndex >= _internalGrid.RowDefinitions.Count)
                {
                    return;
                }

                double current = _internalGrid.RowDefinitions[targetColIndex].Height.Value;
                _internalGrid.RowDefinitions[targetColIndex].Height = new GridLength(current + freedSize, GridUnitType.Absolute);
            }
        }

        /// <summary>
        /// Returns <c>true</c> when the pane at <paramref name="index"/> maps
        /// to an absolute-sized column/row in the internal grid. Used by
        /// <see cref="RedistributeCollapsedPaneSize"/> to detect the
        /// post-drag (all-absolute) layout where freed absolute space
        /// would otherwise leave a gap.
        /// </summary>
        bool IsPaneColumnAbsolute(int index)
        {
            if (_internalGrid == null || index < 0 || index >= SplitterPanes.Count)
            {
                return false;
            }

            int defIndex = index * 2;
            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                if (defIndex < 0 || defIndex >= _internalGrid.ColumnDefinitions.Count)
                {
                    return false;
                }

                return _internalGrid.ColumnDefinitions[defIndex].Width.IsAbsolute;
            }

            if (defIndex < 0 || defIndex >= _internalGrid.RowDefinitions.Count)
            {
                return false;
            }

            return _internalGrid.RowDefinitions[defIndex].Height.IsAbsolute;
        }

        /// <summary>
        /// Adjusts the just-expanded pane's column/row width so the
        /// total of all visible absolute panes does not exceed the
        /// current internal-grid size. Mirrors
        /// <see cref="RedistributeCollapsedPaneSize"/> for the expand path
        /// after a manual drag.
        ///
        /// Without this, restoring an absolute size (e.g. "300") next
        /// to absolute siblings (e.g. "200" + "200") pushes the total
        /// over the splitter's actual width and causes the visible
        /// "empty pane + unused space" symptom the user reported.
        ///
        /// No-op when the restored size is star-sized (the natural star
        /// math handles the layout) or when no visible sibling is
        /// absolute-sized (the post-drag precondition is not met).
        /// Does not mutate <see cref="SplitterPane.Size"/>; only the
        /// internal grid column/row width is adjusted.
        /// </summary>
        /// <param name="expandedIndex">
        /// Index of the pane that was just expanded.
        /// </param>
        void BalanceExpandedPaneSize(int expandedIndex)
        {
            if (_internalGrid == null || expandedIndex < 0 || expandedIndex >= SplitterPanes.Count)
            {
                return;
            }

            if (SplitterPanes[expandedIndex].IsCollapsed)
            {
                return;
            }

            // Only balance when the post-drag precondition holds: at least
            // one visible (non-collapsed) sibling is absolute-sized. Star
            // siblings redivide naturally, so we must not change the
            // restored star size.
            bool hasAbsoluteSibling = false;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (i == expandedIndex)
                {
                    continue;
                }

                if (!SplitterPanes[i].IsCollapsed && IsPaneColumnAbsolute(i))
                {
                    hasAbsoluteSibling = true;
                    break;
                }
            }

            if (!hasAbsoluteSibling)
            {
                return;
            }

            int expDefIndex = expandedIndex * 2;
            double available;
            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                if (expDefIndex < 0 || expDefIndex >= _internalGrid.ColumnDefinitions.Count)
                {
                    return;
                }

                if (!_internalGrid.ColumnDefinitions[expDefIndex].Width.IsAbsolute)
                {
                    return;
                }

                available = _internalGrid.Width > 0
                    ? _internalGrid.Width
                    : _internalGrid.ColumnDefinitions[expDefIndex].Width.Value;
            }
            else
            {
                if (expDefIndex < 0 || expDefIndex >= _internalGrid.RowDefinitions.Count)
                {
                    return;
                }

                if (!_internalGrid.RowDefinitions[expDefIndex].Height.IsAbsolute)
                {
                    return;
                }

                available = _internalGrid.Height > 0
                    ? _internalGrid.Height
                    : _internalGrid.RowDefinitions[expDefIndex].Height.Value;
            }

            // Sum the widths of every OTHER visible pane plus the
            // separators that sit between visible panes. The remaining
            // budget is what the expanded pane should occupy; if its
            // restored value exceeds that, scale it down to fit.
            double otherUsed = 0;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (i == expandedIndex)
                {
                    continue;
                }

                int defIndex = i * 2;
                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    if (defIndex < 0 || defIndex >= _internalGrid.ColumnDefinitions.Count)
                    {
                        continue;
                    }

                    if (!SplitterPanes[i].IsCollapsed)
                    {
                        otherUsed += _internalGrid.ColumnDefinitions[defIndex].Width.Value;
                    }
                }
                else
                {
                    if (defIndex < 0 || defIndex >= _internalGrid.RowDefinitions.Count)
                    {
                        continue;
                    }

                    if (!SplitterPanes[i].IsCollapsed)
                    {
                        otherUsed += _internalGrid.RowDefinitions[defIndex].Height.Value;
                    }
                }
            }

            // Separators live at odd indices. The expanded pane has a
            // leading separator (expDefIndex - 1) and a trailing
            // separator (expDefIndex + 1). Add only the ones that
            // exist in the internal grid (i.e. between two real panes).
            int leadingSep = expDefIndex - 1;
            int trailingSep = expDefIndex + 1;
            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                if (leadingSep >= 0 && leadingSep < _internalGrid.ColumnDefinitions.Count)
                {
                    otherUsed += _internalGrid.ColumnDefinitions[leadingSep].Width.Value;
                }

                if (trailingSep >= 0 && trailingSep < _internalGrid.ColumnDefinitions.Count)
                {
                    otherUsed += _internalGrid.ColumnDefinitions[trailingSep].Width.Value;
                }
            }
            else
            {
                if (leadingSep >= 0 && leadingSep < _internalGrid.RowDefinitions.Count)
                {
                    otherUsed += _internalGrid.RowDefinitions[leadingSep].Height.Value;
                }

                if (trailingSep >= 0 && trailingSep < _internalGrid.RowDefinitions.Count)
                {
                    otherUsed += _internalGrid.RowDefinitions[trailingSep].Height.Value;
                }
            }

            double remaining = available - otherUsed;
            if (remaining <= 0)
            {
                return;
            }

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                double current = _internalGrid.ColumnDefinitions[expDefIndex].Width.Value;
                if (Math.Abs(current - remaining) >= 0.5)
                {
                    _internalGrid.ColumnDefinitions[expDefIndex].Width = new GridLength(remaining, GridUnitType.Absolute);
                }
            }
            else
            {
                double current = _internalGrid.RowDefinitions[expDefIndex].Height.Value;
                if (Math.Abs(current - remaining) >= 0.5)
                {
                    _internalGrid.RowDefinitions[expDefIndex].Height = new GridLength(remaining, GridUnitType.Absolute);
                }
            }
        }

        /// <summary>
        /// Determines whether a pane can be collapsed based on the current collapse rules.
        /// Prevents collapsing panes when it would leave no visible pane or create an invalid layout.
        /// </summary>
        bool IsCollapseAllowed(SplitterPane pane, int index)
        {
            if (SplitterPanes.Count == 0)
            {
                return true;
            }

            if (index == 0 && _isInitialLoad)
            {
                int otherVisibleCount = 0;
                for (int i = 1; i < SplitterPanes.Count; i++)
                {
                    if (!SplitterPanes[i].IsCollapsed)
                    {
                        otherVisibleCount++;
                    }
                }

                if (otherVisibleCount == 0)
                {
                    return false;
                }
            }

            int visibleCount = 0;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (!SplitterPanes[i].IsCollapsed)
                {
                    visibleCount++;
                }
            }
            if (visibleCount <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensures collapsed panes are positioned at the end of the layout so visible panes remain grouped together.
        /// Preserves pane order while maintaining the expected layout for collapsed panes.
        /// </summary>
        void EnforceSeparatorRightAlignment()
        {
            if (_internalGrid == null || SplitterPanes.Count < 3)
            {
                return;
            }

            int collapsedCount = 0;
            for (int i = 1; i < SplitterPanes.Count; i++)
            {
                if (SplitterPanes[i].IsCollapsed)
                {
                    collapsedCount++;
                }
            }

            if (collapsedCount < 2)
            {
                return;
            }

            bool collapsedAtEnd = true;
            for (int i = SplitterPanes.Count - collapsedCount; i < SplitterPanes.Count; i++)
            {
                if (!SplitterPanes[i].IsCollapsed)
                {
                    collapsedAtEnd = false;
                    break;
                }
            }

            if (collapsedAtEnd)
            {
                return;
            }

            var visiblePanes = new List<SplitterPane>();
            var collapsedPanes = new List<SplitterPane>();
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (SplitterPanes[i].IsCollapsed)
                {
                    collapsedPanes.Add(SplitterPanes[i]);
                }
                else
                {
                    visiblePanes.Add(SplitterPanes[i]);
                }
            }

            double sepSize = SeparatorSize;

            var paneSizes = new GridLength[SplitterPanes.Count];
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (SplitterPanes[i].IsCollapsed && _collapsedPaneSizes.TryGetValue(SplitterPanes[i], out var prevSize))
                {
                   paneSizes[i] = ParseGridLength(prevSize);
                }
                else
                {
                    paneSizes[i] = ParseGridLength(SplitterPanes[i].Size);
                }
            }

            _internalGrid.Children.Clear();
            _internalGrid.RowDefinitions.Clear();
            _internalGrid.ColumnDefinitions.Clear();
            _separators.Clear();
            _separatorButtons.Clear();

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                _internalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    SplitterPane pane = i < visiblePanes.Count ? visiblePanes[i] : collapsedPanes[i - visiblePanes.Count];
                    int oldIndex = SplitterPanes.IndexOf(pane);
                    var paneSize = pane.IsCollapsed && oldIndex > 0 && oldIndex < SplitterPanes.Count - 1
                        ? new GridLength(SeparatorView.AdjacentSeparatorGapDiu, GridUnitType.Absolute)
                        : pane.IsCollapsed ? new GridLength(0, GridUnitType.Absolute) : paneSizes[oldIndex];

                    _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = paneSize });
                    pane.ZIndex = 0;
                    Grid.SetRow(pane, 0);
                    Grid.SetColumn(pane, i * 2);
                    _internalGrid.Children.Add(pane);
                    ApplyInitialCollapseState(pane);

                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.ColumnDefinitions.Add(new ColumnDefinition
                        {
                            Width = new GridLength(sepSize, GridUnitType.Absolute)
                        });

                        var separator = CreateSeparator(i + 1);
                        _separators.Add(separator);

                        Grid.SetRow(separator, 0);
                        Grid.SetColumn(separator, i * 2 + 1);
                        separator.ZIndex = 10;
                        _internalGrid.Children.Add(separator);
                    }
                }
            }
            else
            {
                _internalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    SplitterPane pane = i < visiblePanes.Count ? visiblePanes[i] : collapsedPanes[i - visiblePanes.Count];
                    int oldIndex = SplitterPanes.IndexOf(pane);
                    var paneSize = pane.IsCollapsed && oldIndex > 0 && oldIndex < SplitterPanes.Count - 1
                        ? new GridLength(SeparatorView.AdjacentSeparatorGapDiu, GridUnitType.Absolute)
                        : pane.IsCollapsed ? new GridLength(0, GridUnitType.Absolute) : paneSizes[oldIndex];

                    _internalGrid.RowDefinitions.Add(new RowDefinition { Height = paneSize });
                    pane.ZIndex = 0;
                    Grid.SetRow(pane, i * 2);
                    Grid.SetColumn(pane, 0);
                    _internalGrid.Children.Add(pane);
                    ApplyInitialCollapseState(pane);

                    if (i < SplitterPanes.Count - 1)
                    {
                        _internalGrid.RowDefinitions.Add(new RowDefinition
                        {
                            Height = new GridLength(sepSize, GridUnitType.Absolute)
                        });

                        var separator = CreateSeparator(i + 1);
                        _separators.Add(separator);

                        Grid.SetRow(separator, i * 2 + 1);
                        Grid.SetColumn(separator, 0);
                        _internalGrid.Children.Add(separator);
                    }
                }
            }
        }

        /// <summary>
        /// Applies collapse validation rules during the initial layout creation.
        /// Ensures the initial pane collapse states comply with the splitter's visibility rules.
        /// </summary>
        void EnforceLoadTimeCollapseValidation()
        {
            // Wait until at least two panes exist so the first pane is not
            // restored merely because later XAML collection items have not
            // been added yet.
            if (SplitterPanes.Count < 2)
            {
                return;
            }

            int visibleCount = 0;
            int firstCollapsedIndex = -1;
            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                if (!SplitterPanes[i].IsCollapsed)
                {
                    visibleCount++;
                }
                else if (firstCollapsedIndex < 0)
                {
                    firstCollapsedIndex = i;
                }
            }

            if (visibleCount == 0 && firstCollapsedIndex >= 0)
            {
                SplitterPanes[firstCollapsedIndex].SetValue(SplitterPane.IsCollapsedProperty, false);
            }
        }

        /// <summary>
        /// Resolves the initial <see cref="GridLength"/> for a pane during layout creation.
        /// Returns a zero-sized length for collapsed panes; otherwise, uses the pane's configured size.
        /// </summary>
        /// <param name="pane">The pane whose size is being resolved.</param>
        /// <returns>
        /// A zero-sized <see cref="GridLength"/> if the pane is collapsed; otherwise,
        /// the <see cref="GridLength"/> parsed from <see cref="SplitterPane.Size"/>.
        /// </returns>
        static GridLength ResolveInitialPaneGridLength(SplitterPane pane)
        {
            if (pane == null)
            {
                return new GridLength(1, GridUnitType.Star);
            }

            if (pane.IsCollapsed)
            {
                return new GridLength(0, GridUnitType.Absolute);
            }

            return ParseGridLength(pane.Size);
        }

        /// <summary>
        /// Applies the current <see cref="SplitterPane.IsCollapsed"/> state to
        /// the pane's visibility (and its content's visibility/opacity) and
        /// records the previous size in <see cref="_collapsedPaneSizes"/> so
        /// a future <see cref="ExpandPaneAsync"/> can restore it. Mirrors
        /// the per-pane work that <see cref="CollapsePaneAsync"/> /
        /// <see cref="ExpandPaneAsync"/> do, but for the load-time path
        /// where <see cref="OnPanePropertyChanged"/> was never invoked.
        /// </summary>
        /// <param name="pane">The pane whose visibility should be synced.</param>
        void ApplyInitialCollapseState(SplitterPane pane)
        {
            if (pane == null)
            {
                return;
            }

            UpdatePaneVisibility(pane, pane.IsCollapsed);

            if (pane.IsCollapsed)
            {
                _collapsedPaneSizes[pane] = pane.Size;
            }
        }

        /// <summary>
        /// Clamps a pane's size to its minimum and maximum constraints and updates the layout.
        /// Used when pane size constraints change at runtime.
        /// </summary>
        /// <param name="pane">The pane whose size should be clamped.</param>
        /// <param name="index">The zero-based index of the pane in <see cref="SplitterPanes"/>.</param>
        void ClampPaneToSizeLimits(SplitterPane pane, int index)
        {
            if (_internalGrid == null || pane == null || index < 0 || index >= SplitterPanes.Count)
            {
                return;
            }

            if (_activeDraggingTrailingPaneIndex >= 0 && (index == _activeDraggingTrailingPaneIndex || index == _activeDraggingTrailingPaneIndex - 1))
            {
                return;
            }

            double currentSize;
            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                currentSize = pane.Bounds.Width;
            }
            else
            {
                currentSize = pane.Bounds.Height;
            }

            if (currentSize <= 0)
            {
                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    int colIndex = index * 2;
                    if (colIndex < 0 || colIndex >= _internalGrid.ColumnDefinitions.Count)
                    {
                        return;
                    }
                    var def = _internalGrid.ColumnDefinitions[colIndex].Width;
                    currentSize = def.IsAbsolute ? def.Value : 0;
                }
                else
                {
                    int rowIndex = index * 2;
                    if (rowIndex < 0 || rowIndex >= _internalGrid.RowDefinitions.Count)
                    {
                        return;
                    }
                    var def = _internalGrid.RowDefinitions[rowIndex].Height;
                    currentSize = def.IsAbsolute ? def.Value : 0;
                }

                if (currentSize <= 0)
                {
                    return;
                }
            }

            double min = double.IsNaN(pane.MinimumSize) || pane.MinimumSize < 0 ? 0 : pane.MinimumSize;
            double max = double.IsNaN(pane.MaximumSize) || pane.MaximumSize < 0 ? double.PositiveInfinity : pane.MaximumSize;

            bool isNonEdgePane = index > 0 && index < SplitterPanes.Count - 1;
            bool hasUserSetMin = !double.IsNaN(pane.MinimumSize) && pane.MinimumSize > 0;
            if (isNonEdgePane && !hasUserSetMin)
            {
                if (min < DefaultNonEdgePaneMinimumSize)
                {
                    min = DefaultNonEdgePaneMinimumSize;
                }
            }

            if (max < min)
            {
                max = min;
            }

            double clamped = Math.Max(min, Math.Min(max, currentSize));
            if (Math.Abs(clamped - currentSize) < 0.5)
            {
                return;
            }

            string newSize = clamped.ToString("0.##", CultureInfo.InvariantCulture);
            pane.Size = newSize;

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                int colIndex = index * 2;
                if (colIndex >= 0 && colIndex < _internalGrid.ColumnDefinitions.Count)
                {
                    _internalGrid.ColumnDefinitions[colIndex].Width = new GridLength(clamped, GridUnitType.Absolute);
                }
            }
            else
            {
                int rowIndex = index * 2;
                if (rowIndex >= 0 && rowIndex < _internalGrid.RowDefinitions.Count)
                {
                    _internalGrid.RowDefinitions[rowIndex].Height = new GridLength(clamped, GridUnitType.Absolute);
                }
            }
        }

        /// <summary>
        /// Returns the effective minimum size for a pane, applying a default minimum to non-edge panes when necessary.
        /// </summary>
        /// <param name="pane">The pane to query.</param>
        /// <param name="index">The zero-based index of the pane in <see cref="SplitterPanes"/>.</param>
        /// <returns>The effective minimum size for the pane.</returns>
        double GetEffectiveMinSize(SplitterPane pane, int index)
        {
            if (pane == null || index < 0 || index >= SplitterPanes.Count)
            {
               return 0;
            }

            double userMin = pane.MinimumSize;
            if (!double.IsNaN(userMin) && userMin > 0)
            {
                return userMin;
            }

            bool isNonEdgePane = index > 0 && index < SplitterPanes.Count - 1;
            if (isNonEdgePane)
            {
                return DefaultNonEdgePaneMinimumSize;
            }

            return 0;
        }

        /// <summary>
        /// Invalidates every separator so the hit zones, hover/disabled state, and
        /// floating-button visibility are re-evaluated against the current pane
        /// properties.
        /// </summary>
        void InvalidateSeparators()
        {
            foreach (var separator in _separators)
            {
                separator.InvalidateMeasure();
                separator.InvalidateDrawable();
                separator.UpdateResizeIconTemplateVisibility();
            }
        }

        /// <summary>
        /// Clears the hover state on all separators. Used as a safety net
        /// after collapse/expand operations on Android (where the touch
        /// path may not deliver Exited events) and as the global
        /// "pointer left the splitter" handler on every platform.
        /// Prevents separators from remaining in a hovered state when
        /// the user has moved the cursor completely away.
        /// </summary>
        void ForceClearAllSeparatorsHoverState()
        {
            foreach (var separator in _separators)
            {
                separator.ForceClearHoverState();
            }
        }

        /// <summary>
        /// Clears stale pointer and press state from the buttons owned by a separator.
        /// Button views live outside the separator, so separator cleanup cannot reset
        /// their interaction flags without this bridge.
        /// </summary>
        internal void ResetSeparatorButtonInteraction(SeparatorView separator)
        {
            int index = _separators.IndexOf(separator);
            if (index < 0 || index >= _separatorButtons.Count)
            {
                return;
            }

            var buttons = _separatorButtons[index];
            buttons.Leading.ResetInteractionState();
            buttons.Trailing.ResetInteractionState();
            buttons.Leading.IsVisible = false;
            buttons.Trailing.IsVisible = false;
        }
	
        /// <summary>
        /// Handles the start of a separator drag operation and raises the <see cref="ResizeStarted"/> event.
        /// Initializes resize state if the operation is not cancelled.
        /// </summary>
        void OnDragStart(int trailingPaneIndex)
        {
            if (trailingPaneIndex < 0 || trailingPaneIndex >= SplitterPanes.Count)
            {
                return;
            }

            // The separator transitioned to the dragging state, which
            // makes the floating buttons visible. Re-arrange the overlay
            // so the buttons appear at their updated positions.
            InvalidateMeasure();

            int leadingPaneIndex = trailingPaneIndex - 1;
            var leadingPane = leadingPaneIndex >= 0 ? SplitterPanes[leadingPaneIndex] : null;
            var trailingPane = SplitterPanes[trailingPaneIndex];

            var args = new GridSplitterResizeStartedEventArgs( new int[] { leadingPaneIndex, trailingPaneIndex },
                leadingPane != null ? new SplitterPane[] { leadingPane, trailingPane } : new SplitterPane[] { trailingPane });
            ResizeStarted?.Invoke(this, args);

            if (args.Cancel)
            {
                _activeDraggingTrailingPaneIndex = int.MinValue;
                AbortSeparatorDrag(trailingPaneIndex);
                return;
            }

            if (_internalGrid != null && leadingPane != null)
            {
                var leadingColumnIndex = leadingPaneIndex * 2;
                var trailingColumnIndex = trailingPaneIndex * 2;

                if (Orientation == GridSplitterOrientation.Horizontal && leadingColumnIndex >= 0 && leadingColumnIndex < _internalGrid.ColumnDefinitions.Count &&
                    trailingColumnIndex >= 0 && trailingColumnIndex < _internalGrid.ColumnDefinitions.Count)
                {
                    _activeDraggingTrailingPaneIndex = trailingPaneIndex;

                    ConvertAllPanesToAbsolute(orientation: GridSplitterOrientation.Horizontal);
                    _hasUserResizedPanes = true;

                    _activeLeadingPaneSize = leadingPane.Bounds.Width > 0 ? leadingPane.Bounds.Width : _internalGrid.ColumnDefinitions[leadingColumnIndex].Width.Value;
                    _activeTrailingPaneSize = trailingPane.Bounds.Width > 0 ? trailingPane.Bounds.Width : _internalGrid.ColumnDefinitions[trailingColumnIndex].Width.Value;
                }
                else if (Orientation == GridSplitterOrientation.Vertical && leadingColumnIndex >= 0 && leadingColumnIndex < _internalGrid.RowDefinitions.Count &&
                         trailingColumnIndex >= 0 && trailingColumnIndex < _internalGrid.RowDefinitions.Count)
                {
                    _activeDraggingTrailingPaneIndex = trailingPaneIndex;

                    // Same pinning for the vertical orientation.
                    ConvertAllPanesToAbsolute(orientation: GridSplitterOrientation.Vertical);
                    _hasUserResizedPanes = true;

                    _activeLeadingPaneSize = leadingPane.Bounds.Height > 0 ? leadingPane.Bounds.Height : _internalGrid.RowDefinitions[leadingColumnIndex].Height.Value;
                    _activeTrailingPaneSize = trailingPane.Bounds.Height > 0 ? trailingPane.Bounds.Height : _internalGrid.RowDefinitions[trailingColumnIndex].Height.Value;
                }
            }
        }

        /// <summary>
        /// Resets the visual state of a separator when a resize operation is cancelled.
        /// </summary>
        /// <param name="trailingPaneIndex">
        /// The index of the pane following the separator.
        /// </param>
        void AbortSeparatorDrag(int trailingPaneIndex)
        {
            if (trailingPaneIndex <= 0 || trailingPaneIndex > _separators.Count)
            {
                return;
            }

            var separator = _separators[trailingPaneIndex - 1];
            if (separator == null)
            {
               return;
            }

            if (separator.UsePlatformTouchPath)
            {
                separator.AbortInternalDrag();
            }
            else
            {
                separator.AbortPanDrag();
            }
        }

        /// <summary>
        /// Converts all pane sizes to absolute values before resizing to prevent layout redistribution during drag operations.
        /// </summary>
        /// <param name="orientation">The orientation to pin panes for.</param>
        void ConvertAllPanesToAbsolute(GridSplitterOrientation orientation)
        {
            if (_internalGrid == null)
            {
                return;
            }

            if (orientation == GridSplitterOrientation.Horizontal)
            {
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    int colIndex = i * 2;
                    if (colIndex < 0 || colIndex >= _internalGrid.ColumnDefinitions.Count)
                    {
                        continue;
                    }

                    var pane = SplitterPanes[i];


                    if (pane.IsCollapsed)
                    {
                        _internalGrid.ColumnDefinitions[colIndex].Width = new GridLength(0, GridUnitType.Absolute);
                        continue;
                    }

                    double width = pane.Bounds.Width;
                    if (width <= 0)
                    {
                        continue;
                    }

                    var absoluteLength = new GridLength(width, GridUnitType.Absolute);
                    _internalGrid.ColumnDefinitions[colIndex].Width = absoluteLength;
                    pane.Size = width.ToString(CultureInfo.InvariantCulture);
                }
            }
            else
            {
                for (int i = 0; i < SplitterPanes.Count; i++)
                {
                    int rowIndex = i * 2;
                    if (rowIndex < 0 || rowIndex >= _internalGrid.RowDefinitions.Count)
                    {
                        continue;
                    }

                    var pane = SplitterPanes[i];

                    // See horizontal branch for rationale. The IsCollapsed
                    // check is the authoritative guard against disturbing
                    // a collapsed pane's row during a sibling drag.
                    if (pane.IsCollapsed)
                    {
                        _internalGrid.RowDefinitions[rowIndex].Height = new GridLength(0, GridUnitType.Absolute);
                        continue;
                    }

                    double height = pane.Bounds.Height;
                    if (height <= 0)
                    {
                       continue;
                    }

                    var absoluteLength = new GridLength(height, GridUnitType.Absolute);
                    _internalGrid.RowDefinitions[rowIndex].Height = absoluteLength;
                    pane.Size = height.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Handles pane resizing during a drag operation and raises the <see cref="Resizing"/> event.
        /// Ignores resize updates when the resize operation has been cancelled.
        /// </summary>
        void OnDragDelta(int trailingPaneIndex, double delta)
        {
            if (trailingPaneIndex <= 0 || trailingPaneIndex >= SplitterPanes.Count || _activeDraggingTrailingPaneIndex == int.MinValue || _activeDraggingTrailingPaneIndex != trailingPaneIndex)
            {
                return;
            }

            // The drag is changing the column / row widths. The
            // separator's strip position moves with the drag, so the
            // floating expand / collapse buttons must move with it.
            // Invalidate the layout so ArrangeOverlay re-positions
            // the buttons on every drag tick.
            InvalidateMeasure();

            int leadingPaneIndex = trailingPaneIndex - 1;
            var leadingPane = SplitterPanes[leadingPaneIndex];
            var trailingPane = SplitterPanes[trailingPaneIndex];

            if (!leadingPane.IsResizable || !trailingPane.IsResizable || _internalGrid == null)
            {
                return;
            }

            if (Orientation == GridSplitterOrientation.Horizontal)
            {
                var leadingColumnIndex = leadingPaneIndex * 2;
                var trailingColumnIndex = trailingPaneIndex * 2;

                if (leadingColumnIndex >= 0 && leadingColumnIndex < _internalGrid.ColumnDefinitions.Count &&
                    trailingColumnIndex >= 0 && trailingColumnIndex < _internalGrid.ColumnDefinitions.Count)
                {
                    var leadingDef = _internalGrid.ColumnDefinitions[leadingColumnIndex];
                    var trailingDef = _internalGrid.ColumnDefinitions[trailingColumnIndex];

                    double originalLeading = _activeLeadingPaneSize;
                    double originalTrailing = _activeTrailingPaneSize;

                    double effectiveLeadingMin = GetEffectiveMinSize(leadingPane, leadingPaneIndex);
                    double effectiveTrailingMin = GetEffectiveMinSize(trailingPane, trailingPaneIndex);

                    double minDelta = Math.Max( effectiveLeadingMin - originalLeading, originalTrailing - trailingPane.MaximumSize);
                    double maxDelta = Math.Min( leadingPane.MaximumSize - originalLeading, originalTrailing - effectiveTrailingMin);

                    if (minDelta > maxDelta)
                    {
                        return;
                    }

                    double effectiveDelta = Math.Max(minDelta, Math.Min(delta, maxDelta));

                    double leadingAbsolute = originalLeading + effectiveDelta;
                    double trailingAbsolute = originalTrailing - effectiveDelta;

                    leadingDef.Width = new GridLength(leadingAbsolute, GridUnitType.Absolute);
                    trailingDef.Width = new GridLength(trailingAbsolute, GridUnitType.Absolute);
                    _activeLeadingPaneSize = leadingAbsolute;
                    _activeTrailingPaneSize = trailingAbsolute;

                    _activeLeadingPaneSize = leadingAbsolute;
                    _activeTrailingPaneSize = trailingAbsolute;
                }
            }
            else
            {
                var leadingRowIndex = leadingPaneIndex * 2;
                var trailingRowIndex = trailingPaneIndex * 2;

                if (leadingRowIndex >= 0 && leadingRowIndex < _internalGrid.RowDefinitions.Count &&
                    trailingRowIndex >= 0 && trailingRowIndex < _internalGrid.RowDefinitions.Count)
                {
                    var leadingRowDef = _internalGrid.RowDefinitions[leadingRowIndex];
                    var trailingRowDef = _internalGrid.RowDefinitions[trailingRowIndex];

                    double originalLeading = _activeLeadingPaneSize;
                    double originalTrailing = _activeTrailingPaneSize;

                    double effectiveLeadingMin = GetEffectiveMinSize(leadingPane, leadingPaneIndex);
                    double effectiveTrailingMin = GetEffectiveMinSize(trailingPane, trailingPaneIndex);

                    double minDelta = Math.Max( effectiveLeadingMin - originalLeading, originalTrailing - trailingPane.MaximumSize);
                    double maxDelta = Math.Min( leadingPane.MaximumSize - originalLeading, originalTrailing - effectiveTrailingMin);

                    if (minDelta > maxDelta)
                    {
                        return;
                    }

                    double effectiveDelta = Math.Max(minDelta, Math.Min(delta, maxDelta));

                    double leadingAbsolute = originalLeading + effectiveDelta;
                    double trailingAbsolute = originalTrailing - effectiveDelta;

                    leadingRowDef.Height = new GridLength(leadingAbsolute, GridUnitType.Absolute);
                    trailingRowDef.Height = new GridLength(trailingAbsolute, GridUnitType.Absolute);

                    _activeLeadingPaneSize = leadingAbsolute;
                    _activeTrailingPaneSize = trailingAbsolute;
                }
            }

            var args = new GridSplitterResizingEventArgs( new int[] { leadingPaneIndex, trailingPaneIndex },
                new SplitterPane[] { leadingPane, trailingPane });
            Resizing?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the end of a resize operation and raises the <see cref="ResizeStopped"/> event.
        /// Ignored when the resize operation was cancelled.
        /// </summary>
        void OnDragStop(int trailingPaneIndex)
        {
            // Re-arrange the overlay so the floating buttons update
            // after the separator leaves the dragging state.
            InvalidateMeasure();

            if (_activeDraggingTrailingPaneIndex == int.MinValue)
            {
                _activeDraggingTrailingPaneIndex = -1;
                _activeLeadingPaneSize = 0;
                _activeTrailingPaneSize = 0;
                return;
            }

            if (_internalGrid != null &&
                trailingPaneIndex > 0 &&
                trailingPaneIndex < SplitterPanes.Count)
            {
                int leadingPaneIndex = trailingPaneIndex - 1;

                var leadingPane = SplitterPanes[leadingPaneIndex];
                var trailingPane = SplitterPanes[trailingPaneIndex];

                //
                // Persist the final dragged sizes.
                //
                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    int leadingCol = leadingPaneIndex * 2;
                    int trailingCol = trailingPaneIndex * 2;

                    leadingPane.Size =
                        _internalGrid.ColumnDefinitions[leadingCol]
                        .Width.Value
                        .ToString(CultureInfo.InvariantCulture);

                    trailingPane.Size =
                        _internalGrid.ColumnDefinitions[trailingCol]
                        .Width.Value
                        .ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    int leadingRow = leadingPaneIndex * 2;
                    int trailingRow = trailingPaneIndex * 2;

                    leadingPane.Size =
                        _internalGrid.RowDefinitions[leadingRow]
                        .Height.Value
                        .ToString(CultureInfo.InvariantCulture);

                    trailingPane.Size =
                        _internalGrid.RowDefinitions[trailingRow]
                        .Height.Value
                        .ToString(CultureInfo.InvariantCulture);
                }
            }

            if (_activeDraggingTrailingPaneIndex == trailingPaneIndex)
            {
                _activeDraggingTrailingPaneIndex = -1;
                _activeLeadingPaneSize = 0;
                _activeTrailingPaneSize = 0;
            }

            if (trailingPaneIndex >= 0 && trailingPaneIndex < SplitterPanes.Count)
            {
                int leadingPaneIndex = trailingPaneIndex - 1;

                SplitterPane? leadingPane =
                    leadingPaneIndex >= 0
                        ? SplitterPanes[leadingPaneIndex]
                        : null;

                SplitterPane? trailingPane =
                    SplitterPanes[trailingPaneIndex];

                ResizeStopped?.Invoke(
                    this,
                    new GridSplitterResizeStoppedEventArgs(
                        new[] { leadingPaneIndex, trailingPaneIndex },
                        leadingPane != null
                            ? new[] { leadingPane, trailingPane }
                            : new[] { trailingPane }));
            }
        }

#endregion

		#region Property Changed methods

        /// <summary>
        /// Handles property-changed events for the <see cref="SplitterPanes"/> property.
        /// </summary>
        static void OnSplitterPanesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateSplitterPanesCollection(oldValue as ObservableCollection<SplitterPane>, newValue as ObservableCollection<SplitterPane>);
            }
        }

        /// <summary>
        /// Subscribes or unsubscribes to the new and old pane collections and
        /// rebuilds the internal layout to reflect the current pane set.
        /// </summary>
        void UpdateSplitterPanesCollection(ObservableCollection<SplitterPane>? oldCollection, ObservableCollection<SplitterPane>? newCollection)
        {
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= OnSplitterPanesCollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += OnSplitterPanesCollectionChanged;
            }

            UpdateLayoutForPaneChange();
        }

        /// <summary>
        /// Handles changes to the <see cref="Orientation"/> property and updates the layout accordingly.
        /// Reapplies collapsed-pane alignment, pane visibility, and separator templates after the orientation change.
        /// </summary>
        static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateOrientationLayout();
            }
        }

        /// <summary>
        /// Rebuilds the layout for the new orientation, re-applies pane
        /// visibility, refreshes separator templates and invalidates the
        /// separators and the splitter measurement.
        /// </summary>
        void UpdateOrientationLayout()
        {
            UpdateLayoutForOrientation();

            EnforceSeparatorRightAlignment();

            for (int i = 0; i < SplitterPanes.Count; i++)
            {
                UpdatePaneVisibility(SplitterPanes[i], SplitterPanes[i].IsCollapsed);
            }

            foreach (var separator in _separators)
            {
                ApplyResizeIconTemplateToSeparator(separator);
            }

            InvalidateSeparators();
            InvalidateMeasure();
        }

        /// <summary>
        /// Handles property-changed events for the <see cref="SeparatorSize"/> property.
        /// Updates the column/row width/height of every separator in the internal grid and
        /// invalidates measurement so the new size is reflected on the next layout pass.
        /// </summary>
        static void OnSeparatorSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateSeparatorSize((double)newValue);
            }
        }

        /// <summary>
        /// Applies the new separator size to the internal grid definitions and
        /// invalidates the separators and the splitter measurement.
        /// </summary>
        void UpdateSeparatorSize(double newSize)
        {
            if (newSize <= 0 || double.IsNaN(newSize) || double.IsInfinity(newSize))
            {
                return;
            }

            if (_internalGrid != null)
            {
                if (Orientation == GridSplitterOrientation.Horizontal)
                {
                    for (int i = 0; i < _internalGrid.ColumnDefinitions.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            _internalGrid.ColumnDefinitions[i].Width = new GridLength(newSize, GridUnitType.Absolute);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _internalGrid.RowDefinitions.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            _internalGrid.RowDefinitions[i].Height = new GridLength(newSize, GridUnitType.Absolute);
                        }
                    }
                }
            }

            foreach (var separator in _separators)
            {
                separator.InvalidateMeasure();
                separator.InvalidateDrawable();
            }
            InvalidateMeasure();
        }

        /// <summary>
        /// Handles property-changed events for the <see cref="SeparatorBackground"/> property.
        /// </summary>
        static void OnSeparatorBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateSeparatorBackground();
            }
        }

        /// <summary>
        /// Invalidates the drawable on every separator so the new background is
        /// picked up on the next frame.
        /// </summary>
        void UpdateSeparatorBackground()
        {
            foreach (var separator in _separators)
            {
                separator.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="ResizeIconTemplate"/> property.
        /// Updates all separators to apply the new resize icon template.
        /// </summary>
        static void OnResizeIconTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateResizeIconTemplate();
            }
        }

        /// <summary>
        /// Reapplies the current <see cref="ResizeIconTemplate"/> to every separator
        /// so the new template is reflected in the visual tree.
        /// </summary>
        void UpdateResizeIconTemplate()
        {
            foreach (var separator in _separators)
            {
                ApplyResizeIconTemplateToSeparator(separator);
            }
        }

        /// <summary>
        /// Handles property-changed events for the <see cref="ResizeIconColor"/> property.
        /// Invalidates the drawable on every separator so the new color is picked up
        /// on the next frame.
        /// </summary>
        static void OnResizeIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateResizeIconColor();
            }
        }

        /// <summary>
        /// Invalidates the drawable on every separator so the new resize icon
        /// color is picked up on the next frame.
        /// </summary>
        void UpdateResizeIconColor()
        {
            foreach (var separator in _separators)
            {
                separator.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handles property-changed events for the <see cref="ExpandCollapseIconColor"/> property.
        /// Invalidates the drawable on every separator so the new color is picked up
        /// on the next frame. This color is used for the floating expand/collapse
        /// button borders and arrows and for the accent line on the hovered state.
        /// </summary>
        static void OnExpandCollapseIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfGridSplitter control)
            {
                control.UpdateExpandCollapseIconColor();
            }
        }

        /// <summary>
        /// Invalidates the drawable on every separator so the new expand/collapse
        /// icon color is picked up on the next frame.
        /// </summary>
        void UpdateExpandCollapseIconColor()
        {
            foreach (var separator in _separators)
            {
                separator.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handles collection-changed events from the <see cref="SplitterPanes"/> collection.
        /// Subscribes to per-pane property-changed notifications and rebuilds the layout.
        /// </summary>
        void OnSplitterPanesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is SplitterPane oldPane)
                    {
                        oldPane.PropertyChanged -= OnPanePropertyChanged;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is SplitterPane newPane)
                    {
                        newPane.PropertyChanged += OnPanePropertyChanged;
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var pane in SplitterPanes)
                {
                    pane.PropertyChanged += OnPanePropertyChanged;
                }
            }

			UpdateLayoutForPaneChange();
			EnforceSeparatorRightAlignment();
			EnforceLoadTimeCollapseValidation();

			_isInitialLoad = false;

			}

        /// <summary>
        /// Handles per-pane property changes. Propagates Size, IsCollapsed,
        /// IsCollapsible, IsResizable, MinimumSize, MaximumSize changes to the layout
        /// and re-invalidates the relevant separators so their hit/draw state stays
        /// in sync.
        /// </summary>
        void OnPanePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not SplitterPane pane)
            {
                return;
            }

            int index = SplitterPanes.IndexOf(pane);
            if (index < 0)
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(SplitterPane.Size):
                    if (pane.IsCollapsed)
                    {
                        ApplyPaneSizeToGridWithOverride(pane, index, new GridLength(0, GridUnitType.Absolute));
                        InvalidateMeasure();
                        break;
                    }

                    ApplyPaneSizeToGrid(pane, index);
                    InvalidateSeparators();
                    if (_activeDraggingTrailingPaneIndex < 0)
                    {
                        ResetInteractionVisualState();
                    }
                    InvalidateMeasure();
                    break;

                case nameof(SplitterPane.IsCollapsed):
                    if (pane.IsCollapsed && !IsCollapseAllowed(pane, index))
                    {
                        pane.SetValue(SplitterPane.IsCollapsedProperty, false);
                        return;
                    }

                    if (pane.IsCollapsed)
                    {
                        _collapsedPaneSizes[pane] = pane.Size;
                    }
                    else
                    {
                        _collapsedPaneSizes.Remove(pane);
                    }

                    ApplyAllPaneSizesToGrid();

                    if (pane.IsCollapsed)
                    {
                        ApplyPaneSizeToGridWithOverride(pane, index, new GridLength(0, GridUnitType.Absolute));
                        // Mirror CollapsePaneAsync: redistribute the freed
                        // absolute size to a visible absolute neighbor so a
                        // post-drag layout does not leave a gap. No-op
                        // when the saved size is star-sized or invalid.
                        RedistributeCollapsedPaneSize(index);
                    }
                    else
                    {
                        // Mirror ExpandPaneAsync: balance the restored
                        // absolute size against the visible absolute
                        // siblings so the total fits the splitter. No-op
                        // when the siblings are star-sized.
                        BalanceExpandedPaneSize(index);
                    }

                    for (int i = 0; i < SplitterPanes.Count; i++)
                    {
                        UpdatePaneVisibility(SplitterPanes[i], SplitterPanes[i].IsCollapsed);
                    }

                    EnforceSeparatorRightAlignment();
                    InvalidateSeparators();
                    InvalidateMeasure();
                    break;

                case nameof(SplitterPane.IsCollapsible):
                case nameof(SplitterPane.IsResizable):
                    if (e.PropertyName == nameof(SplitterPane.IsResizable))
                    {
                        ResetInteractionVisualState();
                    }

                    InvalidateSeparators();
                    InvalidateMeasure();
                    break;

                case nameof(SplitterPane.MinimumSize):
                case nameof(SplitterPane.MaximumSize):
                    ClampPaneToSizeLimits(pane, index);
                    InvalidateSeparators();
                    InvalidateMeasure();
                    break;
            }
        }

		#endregion

        #region IParentThemeElement Implementation
        
        /// <summary>
        /// Returns the default resource dictionary for <see cref="SfGridSplitter"/>.
        /// Provides theme-based styles and resources for the control.
        /// </summary>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfGridSplitterStyles();
        }

		/// <inheritdoc/>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
			// Re-bind the dynamic theme resources so the bindable properties pick
			// up the values from the newly merged theme dictionary, then refresh
			// every separator to redraw with the new colors.
			WireDynamicThemeResources();
			InvalidateSeparators();
		}

		/// <inheritdoc/>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
			WireDynamicThemeResources();
			InvalidateSeparators();
		}

        #endregion
    }
}

