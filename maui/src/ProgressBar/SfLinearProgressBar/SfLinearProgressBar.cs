using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Represents <see cref="SfLinearProgressBar"/> class, which is a UI control that displays a linear progress indicator.
    /// This control allows for visualizing the progress of an operation through a customizable bar that fills
    /// from left to right. It supports features such as determinate and indeterminate progress modes,
    /// secondary progress indication, customizable appearance including track and progress colors,
    /// segment divisions, corner radius customization, and animation effects.
    /// </summary>
    /// <example>
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    /// <progressBar:SfLinearProgressBar Minimum="0"
    ///                                  Maximum="100"
    ///                                  TrackFill="Blue"
    ///                                  Progress="70"
    ///                                  ProgressFill="Yellow"
    ///                                  SegmentCount="1"
    ///                                  SegmentGapWidth="10"
    ///                                  IsIndeterminate="False"
    ///                                  IndeterminateIndicatorWidthFactor="0.2"
    ///                                  IndeterminateAnimationEasing="{x:Static Easing.SpringIn}"
    ///                                  IndeterminateAnimationDuration="1500"
    ///                                  AnimationEasing="{x:Static Easing.SpringOut}"
    ///                                  AnimationDuration="4000"
    ///                                  SecondaryProgress="100"
    ///                                  ProgressHeight="25"
    ///                                  SecondaryProgressHeight="25"
    ///                                  ProgressCornerRadius="0,13,0,13"
    ///                                  SecondaryProgressCornerRadius="0,13,0,13"
    ///                                  TrackHeight="25"
    ///                                  TrackCornerRadius="0,13,0,13"
    ///                                  ProgressPadding = "10"
    ///                                  ProgressCompleted="SfLinearProgressBar_ProgressCompleted"
    ///                                  ProgressChanged="SfLinearProgressBar_ProgressChanged">
    ///      <progressBar:SfLinearProgressBar.GradientStops>
    ///          <progressBar:ProgressGradientStop Value="50"
    ///                                            Color="Yellow" />
    ///          <progressBar:ProgressGradientStop Value="100"
    ///                                            Color="Green" />
    ///      </progressBar:SfLinearProgressBar.GradientStops>
    /// </progressBar:SfLinearProgressBar>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// private void SfLinearProgressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
    /// {
    ///     DisplayAlert("ProgressCompleted", "Progress: " + e.Progress, "Ok");
    /// }
    /// private void SfLinearProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    /// {
    ///     DisplayAlert("ValueChanged", "Progress: " + e.Progress, "Ok");
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    public class SfLinearProgressBar : ProgressBarBase, IParentThemeElement
    {
        #region Fields
        /// <summary>
        /// Backing field to store a secondary progress path instance.
        /// </summary>
        PathF? _secondProgressPath;

        /// <summary>
        /// Backing field to store the indeterminate animation value.
        /// </summary>
        double _indeterminateAnimationValue;

        /// <summary>
        /// Backing field to store the secondary progress.
        /// </summary>
        double _actualSecondaryProgress;

        /// <summary>
        /// Backing field to store linear gradient brush value.
        /// </summary>
        LinearGradientBrush? _linearGradientBrush;

        /// <summary>
        /// Backing field to store value to identify whether to animate secondary progress.
        /// </summary>
        bool _canAnimateSecondaryProgress;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TrackHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackHeightProperty =
            BindableProperty.Create(
                nameof(TrackHeight),
                typeof(double),
                typeof(SfLinearProgressBar),
                5d,
                propertyChanged: OnHeightPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TrackCornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackCornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackCornerRadiusProperty =
            BindableProperty.Create(
                nameof(TrackCornerRadius),
                typeof(CornerRadius),
                typeof(SfLinearProgressBar),
                null,
                defaultValueCreator: bindable => new CornerRadius(0d),
                propertyChanged: OnTrackCornerRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressHeightProperty =
            BindableProperty.Create(
                nameof(ProgressHeight),
                typeof(double),
                typeof(SfLinearProgressBar),
                5d,
                propertyChanged: OnHeightPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressCornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressCornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressCornerRadiusProperty =
            BindableProperty.Create(
                nameof(ProgressCornerRadius),
                typeof(CornerRadius),
                typeof(SfLinearProgressBar),
                null,
                defaultValueCreator: bindable => new CornerRadius(0d),
                propertyChanged: OnProgressCornerRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="SecondaryProgress"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryProgress"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryProgressProperty =
            BindableProperty.Create(
                nameof(SecondaryProgress),
                typeof(double),
                typeof(SfLinearProgressBar),
                0d,
                propertyChanged: OnSecondaryProgressPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SecondaryProgressFill"/> bindable property. 
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryProgressFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryProgressFillProperty =
            BindableProperty.Create(
                nameof(SecondaryProgressFill),
                typeof(Brush),
                typeof(SfLinearProgressBar),
                null,
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#806750a4")),
                propertyChanged: OnSecondaryProgressFillChanged);

        /// <summary>
        /// Identifies the <see cref="SecondaryAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryAnimationDurationProperty =
            BindableProperty.Create(
                nameof(SecondaryAnimationDuration),
                typeof(double),
                typeof(SfLinearProgressBar),
                1500d,
                BindingMode.Default,
                null);

        /// <summary>
        /// Identifies the <see cref="SecondaryProgressHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryProgressHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryProgressHeightProperty =
            BindableProperty.Create(
                nameof(SecondaryProgressHeight),
                typeof(double),
                typeof(SfLinearProgressBar),
                5d,
                propertyChanged: OnHeightPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SecondaryProgressCornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryProgressCornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryProgressCornerRadiusProperty =
            BindableProperty.Create(
                nameof(SecondaryProgressCornerRadius),
                typeof(CornerRadius),
                typeof(SfLinearProgressBar),
                null,
                defaultValueCreator: bindable => new CornerRadius(0d),
                propertyChanged: OnSecondaryProgressCornerRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressPadding"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressPadding"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressPaddingProperty =
            BindableProperty.Create(
                nameof(ProgressPadding),
                typeof(double),
                typeof(SfLinearProgressBar),
                0d,
                propertyChanged: OnProgressPaddingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LinearProgressBarBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LinearProgressBarBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty LinearProgressBarBackgroundProperty =
            BindableProperty.Create(
                nameof(LinearProgressBarBackground),
                typeof(Color),
                typeof(SfLinearProgressBar),
                defaultValueCreator: bindable => Color.FromArgb("#FFFBFE"),
                propertyChanged: OnLinearProgressBarBackgroundChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLinearProgressBar"/> class.
        /// </summary>
        public SfLinearProgressBar()
        {
            ThemeElement.InitializeThemeResources(this, "SfLinearProgressBarTheme");
            BackgroundColor = LinearProgressBarBackground;
            // TASK-886910: SfLinearProgressBar gets frozen when switching between tabs in TabBar
            Loaded += OnProgressBarLoaded;
            Unloaded += OnProgressBarUnloaded;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value to determine the corner radius of the track.
        /// </summary>
        /// <value>
        /// The corner radius of the track.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar TrackCornerRadius="5,5,5,5" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.TrackCornerRadius = new CornerRadius(5, 5, 5, 5);
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius TrackCornerRadius
        {
            get { return (CornerRadius)GetValue(TrackCornerRadiusProperty); }
            set { SetValue(TrackCornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to determine the track height.
        /// </summary>
        /// <value>
        /// The height of the track. The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar TrackHeight="20" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.TrackHeight = 20;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double TrackHeight
        {
            get { return (double)GetValue(TrackHeightProperty); }
            set { SetValue(TrackHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to determine the corner radius of the progress.
        /// </summary>
        /// <value>
        /// The corner radius of the progress.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressCornerRadius="5,5,5,5"
        ///                                  Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.ProgressCornerRadius = new CornerRadius(5, 5, 5, 5);
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius ProgressCornerRadius
        {
            get { return (CornerRadius)GetValue(ProgressCornerRadiusProperty); }
            set { SetValue(ProgressCornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to determine the progress height.
        /// </summary>
        /// <value>
        /// The height of the progress. The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressHeight="20"
        ///                                  Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.ProgressHeight = 20;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double ProgressHeight
        {
            get { return (double)GetValue(ProgressHeightProperty); }
            set { SetValue(ProgressHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the secondary progress value for the <see cref="SfLinearProgressBar"/>.
        /// </summary>
        /// <value>
        /// The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SecondaryProgress="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SecondaryProgress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double SecondaryProgress
        {
            get { return (double)GetValue(SecondaryProgressProperty); }
            set { SetValue(SecondaryProgressProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush that paints the interior area of the secondary progress.
        /// </summary>
        /// <value>
        /// <c>SecondaryProgressFill</c> specifies how the secondary progress is painted.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SecondaryProgress="75"
        ///                                  SecondaryProgressFill="Violet" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SecondaryProgress = 75;
        /// progressBar.SecondaryProgressFill = Colors.Violet;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush SecondaryProgressFill
        {
            get { return (Brush)GetValue(SecondaryProgressFillProperty); }
            set { SetValue(SecondaryProgressFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the secondary progress animation duration in milliseconds.
        /// </summary>
        /// <value>
        /// The default value is <c>1500</c> milliseconds.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SecondaryProgress="75"
        ///                                  SecondaryAnimationDuration="3000" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SecondaryProgress = 75;
        /// progressBar.SecondaryAnimationDuration = 3000;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double SecondaryAnimationDuration
        {
            get { return (double)GetValue(SecondaryAnimationDurationProperty); }
            set { SetValue(SecondaryAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to determine the secondary progress height.
        /// </summary>
        /// <value>
        /// The height of the secondary progress. The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SecondaryProgressHeight="20"
        ///                                  SecondaryProgress="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SecondaryProgressHeight = 20;
        /// progressBar.SecondaryProgress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double SecondaryProgressHeight
        {
            get { return (double)GetValue(SecondaryProgressHeightProperty); }
            set { SetValue(SecondaryProgressHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to determine the corner radius of the secondary progress.
        /// </summary>
        /// <value>
        /// The corner radius of the secondary progress.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SecondaryProgressCornerRadius="5,5,5,5"
        ///                                  SecondaryProgress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SecondaryProgressCornerRadius = new CornerRadius(5, 5, 5, 5);
        /// progressBar.SecondaryProgress = 50;
        /// Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius SecondaryProgressCornerRadius
        {
            get { return (CornerRadius)GetValue(SecondaryProgressCornerRadiusProperty); }
            set { SetValue(SecondaryProgressCornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the padding to be applied to the progress or the secondary progress indicator.
        /// </summary>
        /// <value>
        /// The padding of the progress or the secondary progress. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// The padding will be applied only at the start and end of the <c>Progress</c> or <c>SecondaryProgress</c> indicator.
        /// To adjust the top and bottom of the indicator <c>ProgressHeight</c> or <c>SecondaryProgressHeight</c> is considered.
        /// </remarks>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressPadding="10"
        ///                                  Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.ProgressPadding = 10;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double ProgressPadding
        {
            get { return (double)GetValue(ProgressPaddingProperty); }
            set { SetValue(ProgressPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the linear progress bar.
        /// </summary>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar LinearProgressBarBackground="Red"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.LinearProgressBarBackground = Colors.Red;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        internal Color LinearProgressBarBackground
        {
            get { return (Color)GetValue(LinearProgressBarBackgroundProperty); }
            set { SetValue(LinearProgressBarBackgroundProperty, value); }
        }

        #endregion

        #region Private Methods

        #region Segment Methods

        /// <summary>
        /// To create a secondary progress.
        /// </summary>
        void CreateSecondaryProgressPath()
        {
            if (!AvailableSize.IsZero)
            {
                if (_canAnimateSecondaryProgress && !IsIndeterminate && SecondaryAnimationDuration > 0)
                {
                    _canAnimateSecondaryProgress = false;
                    CreateSecondaryProgressAnimation();
                }
                else
                {
                    CreatePath("SecondaryProgress");
                }
            }
        }

        /// <summary>
        /// To create the path.
        /// </summary>
        /// <param name="element">The corresponding element.</param>
        /// <param name="animationDuration">The animation duration.</param>
        void CreatePath(string element, double? animationDuration = null)
        {
            animationDuration ??= AnimationDuration;
            PathF path = new PathF();
            double height = 0, width = 0,
                actualGapWidth = SegmentGapWidth > 0 ? SegmentGapWidth : 0, y = 0, x = 0;
            CornerRadius actualCornerRadius = 0;
            int count = 0,
                actualSegmentCount = SegmentCount > 0 ? SegmentCount : 0;
            double topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius,
                segmentWidth = CalculateSegmentWidth();
            double padding = 0;
            if (ProgressPadding > 0)
            {
                if (ProgressPadding < ((segmentWidth == 0 ? AvailableSize.Width : segmentWidth) / 2))
                {
                    padding = ProgressPadding;
                }
                else
                {
                    padding = (segmentWidth == 0 ? AvailableSize.Width : segmentWidth) / 2;
                }

                //Padding has to be applied at both the ends (left and right) of progress or secondary progress indicator.
                padding *= 2;
            }

            switch (element)
            {
                case "Track":
                    path = TrackPath = new PathF();
                    height = TrackHeight > 0 ? TrackHeight : 0d;
                    width = (float)AvailableSize.Width;
                    y = (GetControlHeight() - height) / 2;
                    actualCornerRadius = TrackCornerRadius;
                    count = IsIndeterminate ? 1 : actualSegmentCount;
                    break;
                case "Progress":
                    SetupProgressPathParameters(ref path, ref height, ref width, ref y, ref x, ref actualCornerRadius, ref count, padding, segmentWidth, animationDuration);
                    if (IsIndeterminate)
                    {
                        CalculateIndeterminateProgressWidth(IndeterminateIndicatorWidthFactor, AvailableSize.Width, _indeterminateAnimationValue, ref x, ref width);
                    }

                    break;
                case "SecondaryProgress":
                    path = _secondProgressPath = new PathF();
                    CreateSecondaryProgressPath(path, ref height, ref width, ref y, ref x, ref actualCornerRadius, ref count, padding, segmentWidth);
                    break;
            }

            topLeftRadius = actualCornerRadius.TopLeft > 0 ? actualCornerRadius.TopLeft : 0;
            topRightRadius = actualCornerRadius.TopRight > 0 ? actualCornerRadius.TopRight : 0;
            bottomLeftRadius = actualCornerRadius.BottomLeft > 0 ? actualCornerRadius.BottomLeft : 0;
            bottomRightRadius = actualCornerRadius.BottomRight > 0 ? actualCornerRadius.BottomRight : 0;
            actualCornerRadius = new CornerRadius(topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
            if (actualSegmentCount > 1 && count > 1 && actualGapWidth != 0 && !IsIndeterminate)
            {
                CreateSegmentedPath(count, width, path, (float)y, (float)height, (float)segmentWidth, actualCornerRadius);
            }
            else
            {
                path.AppendRoundedRectangle((float)x, (float)y, (float)width, (float)height, (float)topLeftRadius, (float)topRightRadius, (float)bottomLeftRadius, (float)bottomRightRadius);
            }
        }

        /// <summary>
        /// To create the segmented path.
        /// </summary>
        /// <param name="count">The segment count.</param>
        /// <param name="width">The segment width.</param>
        /// <param name="path">The path.</param>
        /// <param name="y">The y position.</param>
        /// <param name="height">The height.</param>
        /// <param name="segmentWidth">The height.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        void CreateSegmentedPath(int count, double width, PathF path, float y, float height, float segmentWidth, CornerRadius cornerRadius)
        {
            float init = 0, x, pathWidth, indicatorWidth, segmentHeight = height;
            float actualGapWidth = SegmentGapWidth > 0 ? (float)SegmentGapWidth : 0;
            float padding = (float)(ProgressPadding > 0 ? (ProgressPadding < (segmentWidth / 2) ? ProgressPadding : (segmentWidth / 2)) : 0);
            segmentWidth = path != TrackPath ? segmentWidth - (2 * padding) : segmentWidth;
            int t = 0;
            for (int i = 0; i < count; i++)
            {
                x = init + (i > 0 ? actualGapWidth : 0f);
                init = init + segmentWidth + (i > 0 ? actualGapWidth : 0f);
                indicatorWidth = (float)(width + (i * actualGapWidth));
                if (path != TrackPath)
                {
                    t += 2;
                    x += (i == 0 ? padding : (2 * padding));
                    init += (i == 0 ? padding : (2 * padding));
                    indicatorWidth += ((t * padding) - padding);
                }

                pathWidth = init > indicatorWidth ? indicatorWidth - x : segmentWidth;
                path.AppendRoundedRectangle(x, y, pathWidth, segmentHeight, (float)cornerRadius.TopLeft, (float)cornerRadius.TopRight, (float)cornerRadius.BottomLeft, (float)cornerRadius.BottomRight);
            }
        }

        /// <summary>
        /// Creates the path for the secondary progress indicator. This method calculates the dimensions and position based on the specified parameters and updates the path object.
        /// </summary>
        /// <param name="path">The PathF object to which the secondary progress path will be applied.</param>
        /// <param name="height">Reference to the height of the secondary progress path.</param>
        /// <param name="width">Reference to the width of the secondary progress path.</param>
        /// <param name="y">Reference to the y-coordinate for positioning the secondary progress path.</param>
        /// <param name="x">Reference to the x-coordinate for positioning the secondary progress path.</param>
        /// <param name="actualCornerRadius">Reference to the corner radius for the secondary progress path.</param>
        /// <param name="count">Reference to the number of segments in the secondary progress path.</param>
        /// <param name="padding">The padding to be applied to the secondary progress path.</param>
        /// <param name="segmentWidth">The segment width for the secondary progress path.</param>
        void CreateSecondaryProgressPath(PathF path, ref double height, ref double width, ref double y, ref double x, ref CornerRadius actualCornerRadius, ref int count, double padding, double segmentWidth)
        {
            if (!IsIndeterminate)
            {
                height = SecondaryProgressHeight > 0 ? SecondaryProgressHeight : 0d;
                width = (float)CalculateIndicatorWidth(Math.Clamp(SecondaryAnimationDuration > 0 ? _actualSecondaryProgress : _actualSecondaryProgress = SecondaryProgress, ActualMinimum, ActualMaximum), padding);
                y = (GetControlHeight() - height) / 2;
                //"x" is the starting point of indicator, hence we need to consider padding value to be applied only at the start.
                x = padding / 2;
                actualCornerRadius = IsIndeterminate ? 1 : SecondaryProgressCornerRadius;
                count = (int)Math.Ceiling(width / (segmentWidth - padding));
            }
        }

        /// <summary>
        /// Sets up the basic parameters for the progress path.
        /// </summary>
        /// <param name="path">The path to be assigned.</param>
        /// <param name="height">The height of the progress.</param>
        /// <param name="width">The width of the progress.</param>
        /// <param name="y">The y position of the progress.</param>
        /// <param name="x">The x position of the progress.</param>
        /// <param name="actualCornerRadius">The corner radius of the progress.</param>
        /// <param name="count">The count of segments.</param>
        /// <param name="padding">The padding value.</param>
        /// <param name="segmentWidth">The width of each segment.</param>
        /// <param name="animationDuration">The animation duration.</param>
        void SetupProgressPathParameters(ref PathF path, ref double height, ref double width, ref double y,
                                                 ref double x, ref CornerRadius actualCornerRadius, ref int count,
                                                 double padding, double segmentWidth, double? animationDuration)
        {
            path = ProgressPath = new PathF();
            height = ProgressHeight > 0 ? ProgressHeight : 0d;
            width = (float)CalculateIndicatorWidth(Math.Clamp(animationDuration > 0 ? ActualProgress : ActualProgress = Progress, ActualMinimum, ActualMaximum), padding);
            y = (GetControlHeight() - height) / 2;
            //"x" is the starting point of indicator, hence we need to consider padding value to be applied only at the start.
            x = padding / 2;
            actualCornerRadius = ProgressCornerRadius;
            count = (int)Math.Ceiling(width / (segmentWidth - padding));
        }

        // Second new method - for calculating indeterminate progress width
        /// <summary>
        /// Calculates the width for indeterminate progress mode.
        /// </summary>
        /// <param name="indeterminateIndicatorWidthFactor">The indeterminate indicator width factor.</param>
        /// <param name="availableWidth">The available width.</param>
        /// <param name="indeterminateAnimationValue">The current indeterminate animation value.</param>
        /// <param name="x">The x position (will be updated).</param>
        /// <param name="width">The width (will be updated).</param>
        void CalculateIndeterminateProgressWidth(double indeterminateIndicatorWidthFactor, double availableWidth,
                                                       double indeterminateAnimationValue, ref double x, ref double width)
        {
            if (indeterminateIndicatorWidthFactor <= 0)
            {
                width = 0;
            }
            else
            {
                double indeterminateWidth = availableWidth * indeterminateIndicatorWidthFactor;
                if (indeterminateWidth > indeterminateAnimationValue)
                {
                    width = indeterminateAnimationValue;
                }
                else
                {
                    if (indeterminateAnimationValue > availableWidth)
                    {
                        indeterminateWidth = availableWidth - (indeterminateAnimationValue - indeterminateWidth);
                        _indeterminateAnimationValue = availableWidth;
                    }

                    x = indeterminateAnimationValue - indeterminateWidth;
                    width = indeterminateWidth;
                }

                width = width > 0 ? width : 0;
            }
        }

        #endregion

        #region Animation Methods

        /// <summary>
        /// To update inderterminate animation value. 
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        void OnIndeterminateAnimationUpdate(double value)
        {
            _indeterminateAnimationValue = value;
            CreateProgressPath();
            InvalidateDrawable();
        }

        /// <summary>
        /// Animation of the secondary progress.
        /// </summary>
        void CreateSecondaryProgressAnimation()
        {
            var secondaryProgress = Math.Clamp(SecondaryProgress, ActualMinimum, ActualMaximum);
            if (_actualSecondaryProgress != secondaryProgress)
            {
                AnimationExtensions.Animate(this, "SecondaryProgressAnimation", OnSecondaryAnimationUpdate,
                    _actualSecondaryProgress, secondaryProgress, 16, (uint)SecondaryAnimationDuration,
                    AnimationEasing, OnSecondaryAnimationFinished);
            }
        }

        /// <summary>
        /// To update secondary animation value. 
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        void OnSecondaryAnimationUpdate(double value)
        {
            _actualSecondaryProgress = value;
            CreateSecondaryProgressPath();
            InvalidateDrawable();
        }

        /// <summary>
        /// Called when secondary animation finished. 
        /// </summary>
        void OnSecondaryAnimationFinished(double value, bool isCompleted)
        {
            AnimationExtensions.AbortAnimation(this, "SecondaryProgressAnimation");
            _canAnimateSecondaryProgress = false;
        }

        #endregion

        #region Drawing Methods

        /// <summary>
        /// To draw a secondary progress bar.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        void DrawSecondaryProgress(ICanvas canvas)
        {
            if (_secondProgressPath != null)
            {
                canvas.SaveState();
                canvas.SetFillPaint(SecondaryProgressFill, _secondProgressPath.Bounds);
                canvas.FillPath(_secondProgressPath);
                canvas.RestoreState();
            }
        }

        #endregion

        /// <summary>
        /// To get the width of the progress bar..
        /// </summary>
        /// <param name="value">The value to be convert as width.</param>
        /// <param name="padding">The paddng value.</param>
        /// <returns>Width for the given value.</returns>
        private double GetWidthFromValue(double value, double padding)
        {
            return ValueToFactor(value) * (AvailableSize.Width - padding);
        }

        /// <summary>
        /// Refresh the width of the progress.
        /// </summary>
        /// <returns>Return segment width.</returns>
        private double CalculateSegmentWidth()
        {
            double segmentWidth = 0;
            double actualGapWidth = SegmentGapWidth > 0 ? SegmentGapWidth : 0;
            int actualSegmentCount = SegmentCount > 0 ? SegmentCount : 0;
            if (actualSegmentCount >= 1)
            {
                segmentWidth = (AvailableSize.Width - (actualGapWidth * (actualSegmentCount - 1))) / actualSegmentCount;
            }

            return segmentWidth < 0 ? 0 : segmentWidth;
        }

        /// <summary>
        /// Calculates the width of the indicator.
        /// </summary>
        /// <returns>The indicator width.</returns>
        /// <param name="value">The value.</param>
        /// <param name="padding">The padding value.</param>
        private double CalculateIndicatorWidth(double value, double padding)
        {
            double width = AvailableSize.Width - (SegmentCount * padding);
            double actualGapWidth = SegmentGapWidth > 0 ? SegmentGapWidth : 0;
            int actualSegmentCount = SegmentCount > 0 ? SegmentCount : 0;
            if (actualSegmentCount >= 1)
            {
                width -= actualGapWidth * (actualSegmentCount - 1);
                return width * ValueToFactor(value);
            }

            return GetWidthFromValue(value, padding);
        }

        /// <summary>
        /// To get the control height.
        /// </summary>
        /// <returns>The control height.</returns>
        private double GetControlHeight()
        {
            double actualTrackHeight = TrackHeight > 0 ? TrackHeight : 0;
            double actualProgressHeight = ProgressHeight > 0 ? ProgressHeight : 0;
            double actualSecondaryProgressHeight = SecondaryProgressHeight > 0 ? SecondaryProgressHeight : 0;
            return Math.Max(actualTrackHeight, Math.Max(actualProgressHeight, actualSecondaryProgressHeight));
        }

        /// <summary>
        /// Gets the theme dictionary for the <see cref="SfLinearProgressBar"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="ResourceDictionary"/> containing the theme resources for the <see cref="SfLinearProgressBar"/>.
        /// </returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfLinearProgressBarStyles();
        }

        /// <summary>
        /// Invoked when the control theme changes.
        /// </summary>
        /// <param name="oldTheme">The name of the old theme.</param>
        /// <param name="newTheme">The name of the new theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
            SetDynamicResource(LinearProgressBarBackgroundProperty, "SfLinearProgressBarBackground");
        }

        /// <summary>
        /// Invoked when the common theme changes.
        /// </summary>
        /// <param name="oldTheme">The name of the old theme.</param>
        /// <param name="newTheme">The name of the new theme.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
            // No implementation required for common theme changes.
        }

        /// <summary>
        /// Triggered when the view is loaded.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        void OnProgressBarLoaded(object? sender, EventArgs e)
        {
            if (IsIndeterminate
                && IndeterminateAnimationDuration > 0
                && IndeterminateIndicatorWidthFactor > 0
                && !AnimationExtensions.AnimationIsRunning(this, "IndeterminateAnimation")
                && IsIndeterminateAnimationAborted)
            {
                CreateIndeterminateAnimation();
                IsIndeterminateAnimationAborted = false;
                Loaded -= OnProgressBarLoaded;
            }
        }

        /// <summary>
        /// Triggered while the view removed from main visual tree.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        void OnProgressBarUnloaded(object? sender, EventArgs e)
        {
            AnimationExtensions.AbortAnimation(this, "IndeterminateAnimation");
            // TASK-886910: SfLinearProgressBar gets frozen when switching between tabs in TabBar
            IsIndeterminateAnimationAborted = true;
            Unloaded -= OnProgressBarUnloaded;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Measures the size in layout required for child elements.
        /// </summary>
        /// <param name="widthConstraint">The widthConstraint.</param>
        /// <param name="heightConstraint">The heightConstraint.</param>
        /// <returns>Return child element size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (double.IsInfinity(widthConstraint))
            {
                widthConstraint = 350d;
            }

            if (double.IsInfinity(heightConstraint))
            {
                heightConstraint = GetControlHeight();
            }

            AvailableSize = new Size(widthConstraint, heightConstraint);
            UpdateProgressBar();
            return AvailableSize;
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// To create a track path.
        /// </summary>
        internal override void CreateTrackPath()
        {
            CreatePath("Track");
        }

        /// <summary>
        /// To create a progress path.
        /// </summary>
        /// <param name="animationDuration">The animation duration.</param>
        /// <param name="easing">The easing effect.</param>
        internal override void CreateProgressPath(double? animationDuration = null, Easing? easing = null)
        {
            double duration = animationDuration ?? AnimationDuration;
            var ease = easing ?? AnimationEasing;
            if (CanAnimate && !IsIndeterminate && duration > 0)
            {
                CanAnimate = false;
                CreateProgressAnimation(duration, ease);
            }
            else
            {
                CreatePath("Progress", duration);
                CreateGradient();
            }
        }

        /// <summary>
        /// To update the linear progress bar elements such as track, secondary progress, progress, etc.
        /// </summary>
        internal override void UpdateProgressBar()
        {
            base.UpdateProgressBar();
            CreateSecondaryProgressPath();
        }

        /// <summary>
        /// To animate progress for indeterminate state.
        /// </summary>
        internal override void CreateIndeterminateAnimation()
        {
            double indicatorwidth = AvailableSize.Width * IndeterminateIndicatorWidthFactor;
            AnimationExtensions.Animate(
                this,
                "IndeterminateAnimation",
                OnIndeterminateAnimationUpdate,
                0,
                AvailableSize.Width + indicatorwidth,
                16,
                (uint)IndeterminateAnimationDuration,
                IndeterminateAnimationEasing,
                null,
                () => IsIndeterminate);
        }

        /// <summary>
        /// To draw a linear progress bar elements such as track, secondary progress, progress, etc.
        /// </summary>
        /// <param name="canvas">The drawing canvas.</param>
        internal override void DrawProgressBar(ICanvas canvas)
        {
            base.DrawProgressBar(canvas);
            DrawSecondaryProgress(canvas);
            DrawProgress(canvas);
        }

        /// <summary>
        /// To draw a progress.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal override void DrawProgress(ICanvas canvas)
        {
            if (ProgressPath != null)
            {
                canvas.SaveState();
                if (_linearGradientBrush != null && !IsIndeterminate)
                {
                    canvas.SetFillPaint(_linearGradientBrush, ProgressPath.Bounds);
                }
                else
                {
                    canvas.SetFillPaint(ProgressFill, ProgressPath.Bounds);
                }

                canvas.FillPath(ProgressPath);
                canvas.RestoreState();
            }
        }

        /// <summary>
        /// To create linear gradient for progress.
        /// </summary>
        internal override void CreateGradient()
        {

            if (GradientStops == null || GradientStops.Count == 0 || IsIndeterminate)
            {
                _linearGradientBrush = null;
                return;
            }

            double endValue = Math.Clamp(ActualProgress, ActualMinimum, ActualMaximum);
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0, 0.5);
            gradient.EndPoint = new Point(1, 0.5);
            if (GradientStops.Count == 1)
            {
#if ANDROID
                gradient.GradientStops.Add(new GradientStop()
                {
                    Color = GradientStops[0].Color,
                    Offset = 0
                }); 
#endif
                gradient.GradientStops.Add(new GradientStop()
                {
                    Color = GradientStops[0].Color,
                    Offset = (float)ValueToFactor(ActualMaximum, ActualMinimum, endValue)
                });
            }
            else
            {
                List<ProgressGradientStop> gradientStopsList = GradientStops.OrderBy(x => x.ActualValue).ToList();
                if (gradientStopsList[0].Value != ActualMinimum)
                {
                    gradientStopsList.Insert(0, new ProgressGradientStop
                    {
                        Color = gradientStopsList[0].Color,
                        Value = ActualMinimum
                    });
                }

                if (gradientStopsList[^1].Value != endValue) //Equivalent code: if (gradientStopsList[gradientStopsList.Count -1].Value != endValue)
                {
                    gradientStopsList.Add(new ProgressGradientStop
                    {
                        Color = gradientStopsList[^1].Color,
                        Value = ActualMaximum
                    });
                }

                for (int i = 0; i < gradientStopsList.Count; i++)
                {
                    if (gradientStopsList[i].Value >= ActualMinimum && gradientStopsList[i].ActualValue <= endValue)
                    {
                        gradient.GradientStops.Add(new GradientStop()
                        {
                            Color = gradientStopsList[i].Color,
                            Offset = (float)ValueToFactor(gradientStopsList[i].ActualValue, ActualMinimum, endValue)
                        });
                    }
                }

                if (gradient.GradientStops.Count == 0)
                {
                    for (int i = 0; i < gradientStopsList.Count; i++)
                    {
                        if (gradientStopsList[i].ActualValue >= ActualMinimum)
                        {
                            gradient.GradientStops.Add(new GradientStop()
                            {
                                Color = gradientStopsList[i].Color,
                                Offset = (float)ValueToFactor(ActualMaximum, ActualMinimum, endValue)
                            });
                            break;
                        }
                    }
                }

#if ANDROID
                if (gradient.GradientStops.Count == 1)
                {
                    gradient.GradientStops.Add(new GradientStop()
                    {
                        Color = gradientStopsList[0].Color,
                        Offset = 1
                    });
                } 
#endif
            }

            _linearGradientBrush = gradient;
        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Invoked whenever <see cref="TrackHeight"/> or <see cref="ProgressHeight"/> is set for the progress bar.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.InvalidateMeasureOverride();
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever <see cref="TrackCornerRadius"/> is set for the progress bar.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnTrackCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.CreateTrackPath();
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever <see cref="ProgressCornerRadius"/> is set for the progress bar.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnProgressCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.CreateProgressPath();
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="SecondaryProgress"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSecondaryProgressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar)
            {
                linearProgressBar._canAnimateSecondaryProgress = linearProgressBar.SecondaryAnimationDuration > 0 ? true : false;
                if (!linearProgressBar.AvailableSize.IsZero)
                {
                    linearProgressBar._actualSecondaryProgress = (double)oldValue;
                    linearProgressBar.CreateSecondaryProgressPath();
                    linearProgressBar.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Invoked whenever <see cref="SecondaryProgressFill"/> is set for the progress bar.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSecondaryProgressFillChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever <see cref="SecondaryProgressCornerRadius"/> is set for the progress bar.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSecondaryProgressCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.CreateSecondaryProgressPath();
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever <see cref="ProgressPadding"/> property of progress is changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnProgressPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar && !linearProgressBar.AvailableSize.IsZero)
            {
                linearProgressBar.CreateProgressPath();
                linearProgressBar.CreateSecondaryProgressPath();
                linearProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// called when <see cref="LinearProgressBarBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnLinearProgressBarBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearProgressBar linearProgressBar)
            {
                linearProgressBar.BackgroundColor = linearProgressBar.LinearProgressBarBackground;
            }
        }

        #endregion
    }
}