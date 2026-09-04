namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    using Syncfusion.Maui.Toolkit.Internals;
    using View = Microsoft.Maui.Controls.View;

    /// <summary>
    /// Represents a control that enables users to interact with content through zooming and panning operations.
    /// The <see cref="SfInteractiveViewer"/> control can host any .NET MAUI view and provides an intuitive way to view, navigate, and explore content such as images, graphics, diagrams, and custom controls.
    /// </summary>
    public partial class SfInteractiveViewer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
             BindableProperty.Create(
                nameof(Content),
                typeof(View),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => null,
                propertyChanged: OnContentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsZoomEnabled"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="IsZoomEnabled"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsZoomEnabledProperty =
             BindableProperty.Create(
                nameof(IsZoomEnabled),
                typeof(bool),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => true,
                propertyChanged: OnPanAndZoomSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="IsPanEnabled"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="IsPanEnabled"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsPanEnabledProperty =
             BindableProperty.Create(
                nameof(IsPanEnabled),
                typeof(bool),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => true,
                propertyChanged: OnPanAndZoomSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="PanAxis"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="PanAxis"/> bindable property.
        /// </value>
        public static readonly BindableProperty PanAxisProperty =
             BindableProperty.Create(
                nameof(PanAxis),
                typeof(PanAxis),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => PanAxis.Both,
                propertyChanged: OnPanAndZoomSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="ZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="ZoomFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty ZoomFactorProperty =
             BindableProperty.Create(
                nameof(ZoomFactor),
                typeof(double),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => 1d,
                propertyChanged: OnZoomFactorChanged);

        /// <summary>
        /// Identifies the <see cref="MinimumZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="MinimumZoomFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinimumZoomFactorProperty =
             BindableProperty.Create(
                nameof(MinimumZoomFactor),
                typeof(double),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => 1.0d,
                propertyChanged: OnPanAndZoomSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumZoomFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="MaximumZoomFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumZoomFactorProperty =
             BindableProperty.Create(
                nameof(MaximumZoomFactor),
                typeof(double),
                typeof(SfInteractiveViewer),
                defaultValueCreator: bindable => 10.0d,
                propertyChanged: OnPanAndZoomSettingsChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content displayed in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="Content"/> is <see langword="null"/>.
        /// </value>
        /// <seealso cref="IsZoomEnabled"/>
        /// <seealso cref="IsPanEnabled"/>
        public View? Content
        {
            get { return (View?)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zooming is enabled in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="IsZoomEnabled"/> is <see langword="true"/>.
        /// </value>
        /// <seealso cref="MinimumZoomFactor"/>
        /// <seealso cref="MaximumZoomFactor"/>
        public bool IsZoomEnabled
        {
            get { return (bool)GetValue(IsZoomEnabledProperty); }
            set { SetValue(IsZoomEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="IsPanEnabled"/> is <see langword="true"/>.
        /// </value>
        /// <seealso cref="IsZoomEnabled"/>
        /// <seealso cref="PanAxis"/>
        public bool IsPanEnabled
        {
            get { return (bool)GetValue(IsPanEnabledProperty); }
            set { SetValue(IsPanEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the axis in which panning is allowed in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="PanAxis"/> is <see cref="PanAxis.Both"/>.
        /// </value>
        /// <remarks>
        /// This property is applicable only when <see cref="IsPanEnabled"/> is set to <see langword="true"/>.
        /// </remarks>
        public PanAxis PanAxis
        {
            get { return (PanAxis)GetValue(PanAxisProperty); }
            set { SetValue(PanAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current zoom factor of the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="ZoomFactor"/> is 1d.
        /// </value>
        /// <remarks>
        /// This property is applicable only when <see cref="IsZoomEnabled"/> is set to <see langword="true"/>.
        /// </remarks>
        /// <seealso cref="MinimumZoomFactor"/>
        /// <seealso cref="MaximumZoomFactor"/>
        /// <seealso cref="IsZoomEnabled"/>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum zoom factor allowed in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="MinimumZoomFactor"/> is 1d.
        /// </value>
        /// <remarks>
        /// This property is applicable only when <see cref="IsZoomEnabled"/> is set to <see langword="true"/>.
        /// </remarks>
        /// <seealso cref="MaximumZoomFactor"/>
        /// <seealso cref="IsZoomEnabled"/>
        public double MinimumZoomFactor
        {
            get { return (double)GetValue(MinimumZoomFactorProperty); }
            set { SetValue(MinimumZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum zoom factor allowed in the <see cref="SfInteractiveViewer"/>.
        /// </summary>
        /// <value>
        /// The default value of <see cref="MaximumZoomFactor"/> is 10d.
        /// </value>
        /// <remarks>
        /// This property is applicable only when <see cref="IsZoomEnabled"/> is set to <see langword="true"/>.
        /// </remarks>
        /// <seealso cref="MinimumZoomFactor"/>
        /// <seealso cref="IsZoomEnabled"/>
        public double MaximumZoomFactor
        {
            get { return (double)GetValue(MaximumZoomFactorProperty); }
            set { SetValue(MaximumZoomFactorProperty, value); }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Occurs when <see cref="Content"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfInteractiveViewer? interactiveViewer = bindable as SfInteractiveViewer; 
            if (interactiveViewer == null || !interactiveViewer.IsLoaded)
            {
                return;
            }

            if (newValue == null)
            {
                interactiveViewer.RemoveInteractiveViewHandler();
                return;
            }

            if (newValue is View view)
            {
                if (interactiveViewer._interactiveLayout == null || interactiveViewer._scrollView == null)
                {
                    interactiveViewer.InitializeInteractiveViewer();
                }

                interactiveViewer._interactiveLayout?.UpdateContentView(view);
            }
        }

        /// <summary>
        /// Occurs when the zoom settings property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnPanAndZoomSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfInteractiveViewer? interactiveViewer = bindable as SfInteractiveViewer;
            if (interactiveViewer == null || interactiveViewer._interactiveLayout == null || !interactiveViewer.IsLoaded)
            {
                return;
            }

            interactiveViewer.ApplyZoomAndPanSettings();
        }

        /// <summary>
        /// Occurs when <see cref="ZoomFactor"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnZoomFactorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfInteractiveViewer? interactiveViewer = bindable as SfInteractiveViewer;
            if (interactiveViewer == null || interactiveViewer._skipZoomUpdate)
            {
                return;
            }

            interactiveViewer.ZoomTo((double)newValue);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the pan position or zoom factor of the interactive viewer changes.
        /// </summary>
        public event EventHandler<InteractiveScrollChangedEventArgs>? ScrollChanged;

        /// <summary>
        /// Occurs when the zoom factor of the interactive viewer changes.
        /// </summary>
        public event EventHandler<ZoomFactorChangedEventArgs>? ZoomFactorChanged;

        #endregion
    }
}