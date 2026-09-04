namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Represents a pane within an <see cref="SfGridSplitter"/> control.
    /// </summary>
    [ContentProperty("Content")]
    public class SplitterPane : SfView
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SplitterPane),
            null, BindingMode.OneWay, propertyChanged: OnContentChanged);

        /// <summary>
        /// Identifies the <see cref="Size"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The size value can be:
        /// - A star-weighted value (e.g., "1*", "2*") for proportional sizing
        /// - An absolute value (e.g., "100") for fixed sizing
        /// - "*" for auto-fill remaining space
        /// 
        /// Examples: "1*", "100", "*"
        /// </remarks>
        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(string), typeof(SplitterPane),
                "*", BindingMode.TwoWay, propertyChanged: OnSizeChanged);

        /// <summary>
        /// Identifies the <see cref="MinimumSize"/> bindable property.
        /// </summary>
        public static readonly BindableProperty MinimumSizeProperty = BindableProperty.Create( nameof(MinimumSize), typeof(double), typeof(SplitterPane),
                0.0, BindingMode.OneWay, propertyChanged: OnMinimumSizeChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumSize"/> bindable property.
        /// </summary>
        public static readonly BindableProperty MaximumSizeProperty = BindableProperty.Create( nameof(MaximumSize), typeof(double),typeof(SplitterPane),
                double.PositiveInfinity, BindingMode.OneWay, propertyChanged: OnMaximumSizeChanged);

        /// <summary>
        /// Identifies the <see cref="IsCollapsible"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsCollapsibleProperty = BindableProperty.Create( nameof(IsCollapsible), typeof(bool), typeof(SplitterPane),
                true, BindingMode.OneWay, propertyChanged: OnIsCollapsibleChanged);

        /// <summary>
        /// Identifies the <see cref="IsResizable"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsResizableProperty = BindableProperty.Create( nameof(IsResizable), typeof(bool), typeof(SplitterPane),
                true, BindingMode.OneWay, propertyChanged: OnIsResizableChanged);

        /// <summary>
        /// Identifies the <see cref="IsCollapsed"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsCollapsedProperty = BindableProperty.Create( nameof(IsCollapsed), typeof(bool),typeof(SplitterPane),
                false, BindingMode.TwoWay, propertyChanged: OnIsCollapsedChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        public new static readonly BindableProperty BackgroundProperty = BindableProperty.Create( nameof(Background), typeof(Brush), typeof(SplitterPane),
                null, BindingMode.OneWay, propertyChanged: OnBackgroundChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitterPane"/> class.
        /// </summary>
        public SplitterPane()
        {
            // Draw behind the pane content so the border/background appear as pane chrome.
            DrawingOrder = DrawingOrder.BelowContent;
            SetDynamicResource(BorderColorProperty, "SfGridSplitterPaneBorderColor");
            SetDynamicResource(BorderStrokeSizeProperty, "SfGridSplitterPaneBorderStrokeSize");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content view displayed in this pane.
        /// </summary>
        /// <remarks>
        /// This is the main visual content of the pane.
        /// Setting this property updates the layout to display the new content.
        /// </remarks>
        public View? Content
        {
            get => (View?)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the size of this pane in the grid layout.
        /// </summary>
        public string Size
        {
            get => (string)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum size (in DIU) for this pane during resize operations.
        /// </summary>
        public double MinimumSize
        {
            get => (double)GetValue(MinimumSizeProperty);
            set => SetValue(MinimumSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum size (in DIU) for this pane during resize operations.
        /// </summary>
        public double MaximumSize
        {
            get => (double)GetValue(MaximumSizeProperty);
            set => SetValue(MaximumSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this pane can be collapsed.
        /// </summary>
        public bool IsCollapsible
        {
            get => (bool)GetValue(IsCollapsibleProperty);
            set => SetValue(IsCollapsibleProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this pane can be resized.
        /// </summary>
        public bool IsResizable
        {
            get => (bool)GetValue(IsResizableProperty);
            set => SetValue(IsResizableProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this pane is currently collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush for this pane.
        /// </summary>
        public new Brush? Background
        {
            get => (Brush?)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Default color used to draw the pane border in <see cref="OnDraw"/>.
        /// </summary>
        internal static readonly Color DefaultBorderColor = Colors.LightGray;

        /// <summary>
        /// Default stroke size (in DIU) used to draw the pane border in <see cref="OnDraw"/>.
        /// </summary>
        internal const double DefaultBorderStrokeSize = 1.0;

        /// <summary>
        /// Gets or sets the color used to draw the pane border in <see cref="OnDraw"/>.
        /// </summary>
        internal Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BorderColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(SplitterPane),
            DefaultBorderColor,
            BindingMode.OneWay,
            propertyChanged: OnBorderColorChanged);

        /// <summary>
        /// Gets or sets the stroke size (in DIU) used to draw the pane border in <see cref="OnDraw"/>.
        /// </summary>
        internal double BorderStrokeSize
        {
            get => (double)GetValue(BorderStrokeSizeProperty);
            set => SetValue(BorderStrokeSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BorderStrokeSize"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty BorderStrokeSizeProperty = BindableProperty.Create(
            nameof(BorderStrokeSize),
            typeof(double),
            typeof(SplitterPane),
            DefaultBorderStrokeSize,
            BindingMode.OneWay,
            propertyChanged: OnBorderStrokeSizeChanged);

        #endregion

        #region Property Changed Handlers

        /// <summary>
        /// Handles changes to the <see cref="Content"/> property.
        /// </summary>
        static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not SplitterPane pane)
            {
                return;
            }

            // Remove old content from children if it exists
            if (oldValue is View oldView)
            {
                pane.Children.Remove(oldView);
            }

            // Add new content to children if it exists
            if (newValue is View newView)
            {
                pane.Children.Add(newView);
            }

            // Invalidate layout to trigger re-measure/arrange
            pane.InvalidateMeasure();
        }

        /// <summary>
        /// Handles changes to the <see cref="Size"/> property.
        /// Notifies the parent layout by raising <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> for
        /// <c>Size</c>. The owning <c>SfGridSplitter</c> listens to this and updates the
        /// internal grid column/row definition accordingly.
        /// </summary>
        static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                // The owning SfGridSplitter subscribes to INotifyPropertyChanged and
                // applies the new size to the internal grid.
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.Size));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="MinimumSize"/> property.
        /// </summary>
        static void OnMinimumSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.MinimumSize));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="MaximumSize"/> property.
        /// </summary>
        static void OnMaximumSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.MaximumSize));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="IsCollapsible"/> property.
        /// </summary>
        static void OnIsCollapsibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.IsCollapsible));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="IsResizable"/> property.
        /// </summary>
        static void OnIsResizableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.IsResizable));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="IsCollapsed"/> property.
        /// </summary>
        static void OnIsCollapsedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.IsCollapsed));
                pane.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="Background"/> property.
        /// </summary>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.NotifyPanePropertyChanged(nameof(SplitterPane.Background));
                pane.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="BorderColor"/> property.
        /// </summary>
        static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handles changes to the <see cref="BorderStrokeSize"/> property.
        /// </summary>
        static void OnBorderStrokeSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplitterPane pane)
            {
                pane.InvalidateDrawable();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Raises <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> for the
        /// specified pane property. This is the common entry point used by every per-property
        /// changed handler so the owning <c>SfGridSplitter</c> can react to a single,
        /// consistent notification regardless of which property was updated.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed on the <see cref="SplitterPane"/>.
        /// </param>
        internal void NotifyPanePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the pane background and a subtle border.
        /// </summary>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);

            if (canvas == null)
                return;

            if (Background != null)
            {
                canvas.SetFillPaint(Background, dirtyRect);
            }
            else
            {
                canvas.FillColor = Colors.Transparent;
            }

            canvas.FillRectangle(dirtyRect);

            canvas.StrokeColor = BorderColor;
            canvas.StrokeSize = (float)BorderStrokeSize;
            canvas.DrawRectangle(dirtyRect);
        }

        #endregion
    }
}
