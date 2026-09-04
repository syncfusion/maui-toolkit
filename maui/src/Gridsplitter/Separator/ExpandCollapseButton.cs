using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Floating expand/collapse button used by <see cref="SfGridSplitter"/>.
    /// The button is a real <see cref="View"/> hosted in the splitter's overlay
    /// grid (not inside a <see cref="SeparatorView"/>), so the separator strip
    /// never blocks touches on the surrounding pane content. The button draws
    /// its own fill, border, and directional chevron on an internal canvas.
    /// </summary>
    internal sealed partial class ExpandCollapseButton : SfView, ITouchListener
    {
        #region Fields

        /// <summary>
        /// Diameter of the circular button face in DIU.
        /// Matches the value previously used inside <see cref="SeparatorView"/>.
        /// </summary>
        internal const float ButtonDiameter = 25f;

        /// <summary>
        /// Extra padding (in DIU) added on every side of the
        /// button's view bounds to keep the stroked circle inside
        /// the drawable area. Without this padding, the
        /// 1.5-DIU border ring of the circle is clipped by the
        /// view's bounds on every platform (Windows panel,
        /// Android canvas, iOS UIView) and the chevron's
        /// antialiased endpoints are cropped by the same amount.
        /// The padding is reserved space — the visual circle still
        /// has exactly <see cref="ButtonDiameter"/> DIU diameter;
        /// the canvas is simply translated to keep the stroke
        /// inside the view.
        /// </summary>
        internal const float ButtonStrokePadding = 2f;

        /// <summary>
        /// Total edge-to-edge size of the button view
        /// (<see cref="ButtonDiameter"/> + 2 * <see cref="ButtonStrokePadding"/>).
        /// The splitter positions the button using this size so the
        /// view box is large enough to hold the stroked circle.
        /// </summary>
        internal const float ButtonTotalSize = ButtonDiameter + (ButtonStrokePadding * 2f);

        /// <summary>
        /// Chevron arm length in DIU. Mirrors the previous <c>ExpandCollapseArrowSize</c>.
        /// </summary>
        internal const float ArrowSize = 6f;

        bool _isPressed;

        /// <summary>
        /// Re-entrancy guard for <see cref="RaiseClicked"/>. Both
        /// the <see cref="Syncfusion.Maui.Toolkit.Internals.TouchDetector"/>
        /// and the <see cref="TapGestureRecognizer"/> may raise the
        /// click for a single tap on Windows. This flag ensures
        /// <see cref="RaiseClicked"/> fires only once per press /
        /// release cycle.
        /// </summary>
        bool _clickRaised;

        bool _isLeadingSide;

        bool _showLeftChevron;

        bool _showRightChevron;

        bool _showUpChevron;

        bool _showDownChevron;

        Color _borderColor = Color.FromArgb("#6750A4");

        Color _fillColor = Color.FromArgb("#FFFFFF");

        Color _arrowColor = Color.FromArgb("#6750A4");

        string _semanticName = "Expand collapse button";

        string _semanticHelp = "Activates the pane expand or collapse action.";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the border / accent color of the button.
        /// </summary>
        internal Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value)
                {
                    return;
                }
                _borderColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the fill color of the button's inner circle.
        /// </summary>
        internal Color FillColor
        {
            get => _fillColor;
            set
            {
                if (_fillColor == value)
                {
                    return;
                }
                _fillColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the chevron stroke color.
        /// </summary>
        internal Color ArrowColor
        {
            get => _arrowColor;
            set
            {
                if (_arrowColor == value)
                {
                    return;
                }
                _arrowColor = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button represents the
        /// leading (left/top) action of its parent separator.
        /// </summary>
        internal bool IsLeadingSide
        {
            get => _isLeadingSide;
            set => _isLeadingSide = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a left-pointing chevron.
        /// </summary>
        internal bool ShowLeftChevron
        {
            get => _showLeftChevron;
            set
            {
                if (_showLeftChevron == value)
                {
                    return;
                }
                _showLeftChevron = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a right-pointing chevron.
        /// </summary>
        internal bool ShowRightChevron
        {
            get => _showRightChevron;
            set
            {
                if (_showRightChevron == value)
                {
                    return;
                }
                _showRightChevron = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw an upward chevron.
        /// </summary>
        internal bool ShowUpChevron
        {
            get => _showUpChevron;
            set
            {
                if (_showUpChevron == value)
                {
                    return;
                }
                _showUpChevron = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a downward chevron.
        /// </summary>
        internal bool ShowDownChevron
        {
            get => _showDownChevron;
            set
            {
                if (_showDownChevron == value)
                {
                    return;
                }
                _showDownChevron = value;
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the accessibility name reported to assistive technologies.
        /// </summary>
        internal string SemanticName
        {
            get => _semanticName;
            set => _semanticName = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the accessibility help text reported to assistive technologies.
        /// </summary>
        internal string SemanticHelp
        {
            get => _semanticHelp;
            set => _semanticHelp = value ?? string.Empty;
        }

        /// <summary>
        /// Gets a value indicating whether the pointer is currently over
        /// the button. The splitter uses this to keep the buttons visible
        /// while the user moves the pointer from the strip onto the
        /// button.
        /// </summary>
        internal bool IsPointerOver { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the button is currently being
        /// pressed. The splitter uses this to keep the button visible
        /// while the user is pressing it (e.g. during a press that
        /// started on the strip and moved onto the button).
        /// </summary>
        internal bool IsPressed => _isPressed;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the user taps the button.
        /// </summary>
        internal event EventHandler? Clicked;

        /// <summary>
        /// Raised when <see cref="IsPointerOver"/> changes. The splitter
        /// listens to this to re-evaluate button visibility — when the
        /// pointer moves from the strip onto a button the separator's
        /// hover state is released, but the button's own pointer-over
        /// state keeps the buttons visible.
        /// </summary>
        internal event EventHandler? PointerOverChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandCollapseButton"/> class.
        /// </summary>
        public ExpandCollapseButton()
        {
            DrawingOrder = DrawingOrder.AboveContent;

            // Explicitly set a transparent background so the WinUI
            // platform view (a LayoutPanelExt / Panel) is reliably
            // hit-testable. Without a background, some WinUI
            // configurations do not raise pointer events on the
            // panel, which would prevent both the touch listener and
            // the TapGestureRecognizer from firing.
            Background = new SolidColorBrush(Colors.Transparent);

            // Disable clipping so the circular affordance is never
            // cropped at its bounds. The circle is exactly
            // ButtonDiameter DIU wide, but without ClipToBounds=false
            // some platform configurations (notably the W2D
            // CanvasControl inside the SfView's NativeGraphicsView)
            // truncate the circle by a fraction of a pixel at all
            // four edges.
            ClipToBounds = false;

            // The button does not capture touches on its background by
            // default; only the circular hit area should receive input.
            // We still register a touch listener so we can drive the
            // click event from the platform input system.
            this.AddTouchListener(this);

            // The platform touch listener is the primary click path.
            // As a backup, also register a TapGestureRecognizer so the
            // click fires even if the touch listener is not wired up
            // correctly on a given platform (e.g. when the button is
            // hosted inside an AbsoluteLayout child of the splitter).
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => RaiseClicked();
            GestureRecognizers.Add(tap);

            // Accessibility metadata is propagated through AutomationProperties
            // so the name/help text is announced by screen readers.
            AutomationProperties.SetName(this, _semanticName);
            AutomationProperties.SetHelpText(this, _semanticHelp);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Updates accessibility metadata in one call to keep the
        /// <see cref="AutomationProperties"/> in sync with the latest
        /// chevron direction and state.
        /// </summary>
        internal void RefreshSemantics()
        {
            AutomationProperties.SetName(this, _semanticName);
            AutomationProperties.SetHelpText(this, _semanticHelp);
        }

        internal void ResetInteractionState()
        {
            _isPressed = false;
            IsPointerOver = false;
            InvalidateDrawable();
        }

        /// <summary>
        /// Raises the <see cref="Clicked"/> event.
        /// </summary>
        void RaiseClicked()
        {
            // Guard against double-fire: on Windows both the
            // TouchDetector and the TapGestureRecognizer raise the
            // click for a single tap. Only the first raise counts;
            // the flag is cleared again on the next press so the
            // user can tap again normally.
            if (_clickRaised)
            {
                return;
            }
            _clickRaised = true;
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Measures the button to a fixed size of <see cref="ButtonTotalSize"/> on
        /// both axes. The button is a circular affordance and always uses the
        /// same size regardless of layout constraints. The total size is the
        /// visible circle diameter plus symmetric stroke padding so the
        /// antialiased edge of the border ring is never clipped by the view
        /// bounds.
        /// </summary>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            return new Size(ButtonTotalSize, ButtonTotalSize);
        }

        /// <summary>
        /// Arranges the button into a square of <see cref="ButtonTotalSize"/> ×
        /// <see cref="ButtonTotalSize"/>. The visual circle occupies the
        /// central <see cref="ButtonDiameter"/> × <see cref="ButtonDiameter"/>
        /// region; the surrounding <see cref="ButtonStrokePadding"/> on every
        /// side is reserved for the stroked border ring so the button is never
        /// clipped on any platform.
        /// </summary>
        protected override Size ArrangeContent(Rect bounds)
        {
            return new Size(ButtonTotalSize, ButtonTotalSize);
        }

        /// <summary>
        /// Draws the button's circular fill, border, and directional chevron.
        /// The circle is centered inside the view's safe area so the
        /// <see cref="ButtonStrokePadding"/> ring around it prevents the
        /// stroked edge from being clipped by the view bounds.
        /// </summary>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);

            float diameter = ButtonDiameter;
            float radius = diameter / 2f;
            // The circle is centered in the view, NOT at dirtyRect.Center
            // (which already accounts for the safe-area padding because
            // the view is now ButtonTotalSize × ButtonTotalSize).
            float cx = dirtyRect.Center.X;
            float cy = dirtyRect.Center.Y;

            // Filled inner circle.
            canvas.FillColor = _fillColor;
            canvas.FillCircle(cx, cy, radius);

            // Border ring.
            float borderThickness = _isPressed ? 2.0f : 1.0f;
            canvas.StrokeColor = _borderColor;
            canvas.StrokeSize = borderThickness;
            canvas.DrawCircle(cx, cy, radius);

            // Directional chevron. Only one of the four flags should ever be
            // set at a time; the splitter computes the direction based on the
            // adjacent pane state.
            float arrowSize = ArrowSize;
            float halfArm = arrowSize / 2f;
            canvas.StrokeColor = _arrowColor;
            canvas.StrokeSize = 1.5f;
			float width = halfArm;
			float height = halfArm * 1.5f;   // Increase this to open the chevron more

			if (_showLeftChevron)
			{
				var path = new PathF();
				path.MoveTo(new PointF(cx + width, cy - height));
				path.LineTo(new PointF(cx - width, cy));
				path.LineTo(new PointF(cx + width, cy + height));
				canvas.DrawPath(path);
			}
			else if (_showRightChevron)
			{
				var path = new PathF();
				path.MoveTo(new PointF(cx - width, cy - height));
				path.LineTo(new PointF(cx + width, cy));
				path.LineTo(new PointF(cx - width, cy + height));
				canvas.DrawPath(path);
			}
			else if (_showUpChevron)
			{
				var path = new PathF();
				path.MoveTo(new PointF(cx - height, cy + width));
				path.LineTo(new PointF(cx, cy - width));
				path.LineTo(new PointF(cx + height, cy + width));
				canvas.DrawPath(path);
			}
			else if (_showDownChevron)
			{
				var path = new PathF();
				path.MoveTo(new PointF(cx - height, cy - width));
				path.LineTo(new PointF(cx, cy + width));
				path.LineTo(new PointF(cx + height, cy - width));
				canvas.DrawPath(path);
			}
		}

        #endregion
    }
}
