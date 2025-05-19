using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Base class of <see cref="SfLinearProgressBar"/> and <see cref="SfCircularProgressBar"/>.
    /// It contains common logic of track, progress, etc that help to visualize the data.
    /// </summary>
    public abstract class ProgressBarBase : SfView
    {
        #region Fields

        /// <summary>
        /// Field used to skip the property changed when the <see cref="Progress"/> is changed from <see cref="SetProgress(double, double?, Easing?)"/>.
        /// </summary>
        bool _skipProgressUpdate = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Minimum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Minimum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinimumProperty =
          BindableProperty.Create(
              nameof(Minimum),
              typeof(double),
              typeof(ProgressBarBase),
              0d,
              propertyChanged: OnMinimumPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Maximum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Maximum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumProperty =
           BindableProperty.Create(
               nameof(Maximum),
               typeof(double),
               typeof(ProgressBarBase),
               100d,
               propertyChanged: OnMaximumPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TrackFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackFillProperty =
            BindableProperty.Create(
                nameof(TrackFill),
                typeof(Brush),
                typeof(ProgressBarBase),
                null,
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#E7E0EC")),
                propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Progress"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Progress"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressProperty =
          BindableProperty.Create(
              nameof(Progress),
              typeof(double),
              typeof(ProgressBarBase),
              0d,
              propertyChanged: OnProgressPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressFillProperty =
            BindableProperty.Create(
                nameof(ProgressFill),
                typeof(Brush),
                typeof(ProgressBarBase),
                null,
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#6750A4")),
                propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SegmentCount"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SegmentCount"/> bindable property.
        /// </value>
        public static readonly BindableProperty SegmentCountProperty =
            BindableProperty.Create(
                nameof(SegmentCount),
                typeof(int),
                typeof(ProgressBarBase),
                1,
                propertyChanged: OnSegmentPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="SegmentGapWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SegmentGapWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty SegmentGapWidthProperty =
            BindableProperty.Create(
                nameof(SegmentGapWidth),
                typeof(double),
                typeof(ProgressBarBase),
                3d,
                propertyChanged: OnSegmentPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="IsIndeterminate"/> bindable property. 
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsIndeterminate"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsIndeterminateProperty =
            BindableProperty.Create(
                nameof(IsIndeterminate),
                typeof(bool),
                typeof(ProgressBarBase),
                false,
                propertyChanged: OnIndeterminatePropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="IndeterminateIndicatorWidthFactor"/> bindable property. 
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndeterminateIndicatorWidthFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty IndeterminateIndicatorWidthFactorProperty =
            BindableProperty.Create(
                nameof(IndeterminateIndicatorWidthFactor),
                typeof(double), typeof(ProgressBarBase),
                0.25d,
                 coerceValue: (bindable, value) =>
                 {
                     return Math.Clamp((double)value, 0, 1);
                 },
                 propertyChanged: OnIndeterminatePropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="IndeterminateAnimationEasing"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndeterminateAnimationEasing"/> bindable property.
        /// </value>
        public static readonly BindableProperty IndeterminateAnimationEasingProperty =
            BindableProperty.Create(
                nameof(IndeterminateAnimationEasing),
                typeof(Easing),
                typeof(ProgressBarBase),
                Easing.Linear,
                propertyChanged: OnIndeterminatePropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="IndeterminateAnimationDuration"/> bindable property. 
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndeterminateAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty IndeterminateAnimationDurationProperty =
            BindableProperty.Create(
                nameof(IndeterminateAnimationDuration),
                typeof(double),
                typeof(ProgressBarBase),
                3000d,
                propertyChanged: OnIndeterminatePropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(
                nameof(AnimationDuration),
                typeof(double),
                typeof(ProgressBarBase),
                1000d,
                BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="AnimationEasing"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationEasing"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationEasingProperty =
            BindableProperty.Create(
                nameof(AnimationEasing),
                typeof(Easing),
                typeof(ProgressBarBase),
                Easing.Linear,
                BindingMode.Default,
                null);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
            BindableProperty.Create(
                nameof(GradientStops),
                typeof(ObservableCollection<ProgressGradientStop>),
                typeof(ProgressBarBase),
                null,
                propertyChanged: OnGradientPropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarBase"/> class.
        /// </summary>
        public ProgressBarBase()
        {
            GradientStops = new ObservableCollection<ProgressGradientStop>();
            DrawingOrder = DrawingOrder.BelowContent;
            ValidateMinimumMaximum();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum possible value of the progress bar. The progress bar range starts from this value.
        /// </summary>
        /// <value>
        /// The minimum possible value of the progress bar. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// 
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Minimum="50" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Minimum="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Minimum = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Minimum = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum possible value of the progress bar. The progress bar ends at this value.
        /// </summary>
        /// <value>
        /// The highest possible value of the progress bar. The default value is <c>100</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Maximum="50" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Maximum="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Maximum = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Maximum = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush that paints the interior area of the track.
        /// </summary>
        /// <value>
        /// <c>TrackFill</c> specifies how the track is painted.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar TrackFill="Red" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar TrackFill="Red" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.TrackFill = Colors.Red;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.TrackFill = Colors.Red;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush TrackFill
        {
            get { return (Brush)GetValue(TrackFillProperty); }
            set { SetValue(TrackFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the current value for the progress. 
        /// </summary>
        /// <value>
        /// The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// 
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Progress="75" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Progress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush that paints the interior area of the progress.
        /// </summary>
        /// <value>
        /// <c>ProgressFill</c> specifies how the progress is painted.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Progress="50"
        ///                                  ProgressFill="Violet" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="50"
        ///                                    ProgressFill="Violet" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Progress = 50;
        /// progressBar.ProgressFill = Colors.Violet;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref = "SfCircularProgressBar" />
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 50;
        /// progressBar.ProgressFill = Colors.Violet;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush ProgressFill
        {
            get { return (Brush)GetValue(ProgressFillProperty); }
            set { SetValue(ProgressFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that determine the segments count of progress bar.
        /// </summary>
        /// <value>
        /// The default value is <c>1</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SegmentCount="4"
        ///                                  Progress="75" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar SegmentCount="4"
        ///                                    Progress="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SegmentCount = 4;
        /// progressBar.Progress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.SegmentCount = 4;
        /// progressBar.Progress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public int SegmentCount
        {
            get { return (int)GetValue(SegmentCountProperty); }
            set { SetValue(SegmentCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that determine the gap between the segments.
        /// </summary>
        /// <value>
        /// The default value is <c>3</c>.
        /// </value> 
        /// <example>
        /// 
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar SegmentCount="4"
        ///                                  SegmentGapWidth="10" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar SegmentCount="4"
        ///                                    SegmentGapWidth="10" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.SegmentCount = 4;
        /// progressBar.SegmentGapWidth = 10;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.SegmentCount = 4;
        /// progressBar.SegmentGapWidth = 10;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double SegmentGapWidth
        {
            get { return (double)GetValue(SegmentGapWidthProperty); }
            set { SetValue(SegmentGapWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar is in indeterminate state or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if it is indeterminate; otherwise, <c>false</c>.The default value is <c>false</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar IsIndeterminate="True" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar IsIndeterminate="True" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies width of the indeterminate indicator.
        /// </summary>
        /// <value>
        /// It ranges from 0 to 1. The default value is <c>0.25d</c>.
        /// </value> 
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar IsIndeterminate="True"
        ///                                  IndeterminateIndicatorWidthFactor="0.7" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar IsIndeterminate="True"
        ///                                    IndeterminateIndicatorWidthFactor="0.7" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateIndicatorWidthFactor = 0.7;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateIndicatorWidthFactor = 0.7;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double IndeterminateIndicatorWidthFactor
        {
            get { return (double)GetValue(IndeterminateIndicatorWidthFactorProperty); }
            set { SetValue(IndeterminateIndicatorWidthFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the easing effect for indeterminate animation.
        /// </summary>
        /// <value>
        /// The default value is <see cref="Easing.Linear"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar IsIndeterminate="True"
        ///                                  IndeterminateAnimationEasing="{x:Static Easing.BounceIn}" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar IsIndeterminate="True"
        ///                                    IndeterminateAnimationEasing="{x:Static Easing.BounceIn}" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateAnimationEasing = Easing.BounceIn;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateAnimationEasing = Easing.BounceIn;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Easing IndeterminateAnimationEasing
        {
            get { return (Easing)GetValue(IndeterminateAnimationEasingProperty); }
            set { SetValue(IndeterminateAnimationEasingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the indeterminate animation duration in milliseconds.
        /// </summary>
        /// <value>
        /// The default value is <c>3000</c> milliseconds.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar IsIndeterminate="True"
        ///                                  IndeterminateAnimationDuration="5000" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar IsIndeterminate="True"
        ///                                    IndeterminateAnimationDuration="5000" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateAnimationDuration = 5000;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.IsIndeterminate = true;
        /// progressBar.IndeterminateAnimationDuration = 5000;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double IndeterminateAnimationDuration
        {
            get { return (double)GetValue(IndeterminateAnimationDurationProperty); }
            set { SetValue(IndeterminateAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the progress animation duration in milliseconds.
        /// </summary>
        /// <value>
        /// The default value is <c>1000</c> milliseconds.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar AnimationDuration="3000"
        ///                                  Progress="50" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar AnimationDuration="3000"
        ///                                    Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.AnimationDuration = 3000;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.AnimationDuration = 3000;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the easing effect for progress animation.
        /// </summary>
        /// <value>
        /// The default value is <see cref="Easing.Linear"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar AnimationEasing="{x:Static Easing.BounceIn}"
        ///                                  Progress="50" />
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar AnimationEasing="{x:Static Easing.BounceIn}"
        ///                                    Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.AnimationEasing = Easing.BounceIn;
        /// progressBar.Progress = 50;
        /// Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.AnimationEasing = Easing.BounceIn;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Easing AnimationEasing
        {
            get { return (Easing)GetValue(AnimationEasingProperty); }
            set { SetValue(AnimationEasingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="ProgressGradientStop"/> to fill the gradient brush to the progress.
        /// </summary>
        /// <remarks>
        /// Gradient effect is not supported for segmented circular progress bar.
        /// </remarks>
        /// <value>
        /// A collection of the <see cref="ProgressGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the axis.
        /// The default is an empty collection.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Progress="75">
        ///     <progressBar:SfLinearProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfLinearProgressBar.GradientStops>
        /// </progressBar:SfLinearProgressBar>
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="75">
        ///     <progressBar:SfCircularProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfCircularProgressBar.GradientStops>                              
        /// </progressBar:SfCircularProgressBar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public ObservableCollection<ProgressGradientStop> GradientStops
        {
            get { return (ObservableCollection<ProgressGradientStop>)GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Backing field to store track and progress path.
        /// </summary>
        internal PathF? TrackPath, ProgressPath;

        /// <summary>
        /// Gets or sets the value to store actual minimum value.
        /// </summary>
        internal double ActualMinimum { get; set; }

        /// <summary>
        /// Gets or sets the value to store actual maximum value
        /// </summary>
        internal double ActualMaximum { get; set; }

        /// <summary>
        /// Gets or sets the value to store the available size of the progress bar.
        /// </summary>
        internal Size AvailableSize { get; set; }

        /// <summary>
        /// Gets or sets the value to identify whether to animate progress.
        /// </summary>
        internal bool CanAnimate { get; set; }

        /// <summary>
        /// Gets or sets the actual progress.
        /// </summary>
        internal double ActualProgress { get; set; }

        /// <summary>
        /// Gets or sets the value to identify whether Indeterminate Animation is aborted.
        /// </summary>
        internal bool IsIndeterminateAnimationAborted { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets <see cref="Progress"/> with corresponding animation duration and easing effects.
        /// </summary>
        /// <param name="progress"><see cref="Progress"/> value.</param>
        /// <param name="animationDuration">Duration to animate the progress. If didn't passed the duration, then it takes <see cref="AnimationDuration"/>.</param>
        /// <param name="easing">Easing effect for the animation. If didn't passed the Easing, then it takes <see cref="AnimationEasing"/>.</param>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar x:Name="linearProgress"/>
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar x:Name="circularProgress"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// linearProgress.SetProgress(50, 10000, Easing.BounceIn);
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// circularProgress.SetProgress(50, 10000, Easing.BounceIn);
        /// ]]></code>
        /// ***
        /// </example>
        public void SetProgress(double progress, double? animationDuration = null, Easing? easing = null)
        {
            _skipProgressUpdate = true;
            Progress = progress;
            _skipProgressUpdate = false;
            var duration = animationDuration ?? AnimationDuration;
            CanAnimate = duration > 0;
            CreateProgressPath(duration, easing);
            if (duration <= 0)
            {
                RaiseProgressChanged();
                RaiseProgressCompleted();
                InvalidateDrawable();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  To create a track path.
        /// </summary>
        internal abstract void CreateTrackPath();

        /// <summary>
        ///  To create a progress path.
        /// </summary>
        /// <param name="animationDuration">The animation duration.</param>
        /// <param name="easing">The easing effect.</param>
        internal abstract void CreateProgressPath(double? animationDuration = null, Easing? easing = null);

        /// <summary>
        /// To draw a progress.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal abstract void DrawProgress(ICanvas canvas);

        /// <summary>
        /// To create indeterminate animation.
        /// </summary>
        internal abstract void CreateIndeterminateAnimation();

        /// <summary>
        /// To create gradient effect.
        /// </summary>
        internal abstract void CreateGradient();

        /// <summary>
        /// To draw a track.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal void DrawTrack(ICanvas canvas)
        {
            if (TrackPath != null)
            {
                canvas.SaveState();
                canvas.SetFillPaint(TrackFill, TrackPath.Bounds);
                canvas.FillPath(TrackPath);
                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Converts progress value to factor value.
        /// </summary>
        /// <param name="value">The value to convert as factor.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <returns>The factor of the provided value.</returns>
        internal double ValueToFactor(double value, double? minimum = null, double? maximum = null)
        {
            double min = minimum ?? ActualMinimum;
            double max = maximum ?? ActualMaximum;
            double factor = (value - min) / (max - min);
            return Math.Clamp(factor, 0, 1);
        }

        /// <summary>
        /// To create progress animation.
        /// </summary>
        /// <param name="animationDuration">The animation duration.</param>
        /// <param name="easingEffect">The easing effect.</param>
        internal void CreateProgressAnimation(double animationDuration, Easing easingEffect)
        {
            var progress = Math.Clamp(Progress, ActualMinimum, ActualMaximum);
            if (ActualProgress != progress)
            {
                AnimationExtensions.Animate(
                    this,
                    "ProgressAnimation",
                    OnProgressAnimationUpdate,
                    ActualProgress,
                    progress,
                    16,
                    (uint)animationDuration,
                    easingEffect,
                    OnProgressAnimationFinished);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To validate the minimum and maximum.
        /// </summary>
        void ValidateMinimumMaximum()
        {
            (double, double) actualValues = Utility.ValidateMinimumMaximumValue(Minimum, Maximum);
            ActualMinimum = actualValues.Item1;
            ActualMaximum = actualValues.Item2;
        }

        /// <summary>
        /// Common logic to perform when <see cref="Minimum"/> or <see cref="Maximum"/> changed.
        /// </summary>
        void OnMinMaxChanged()
        {
            ValidateMinimumMaximum();
            UpdateProgressBar();
            InvalidateDrawable();
        }

        /// <summary>
        /// To raise progress changed event. 
        /// </summary>
        void RaiseProgressChanged()
        {
            if (!IsIndeterminate)
            {
                ProgressChanged?.Invoke(this, new ProgressValueEventArgs
                {
                    CurrentProgress = Progress
                });
            }
        }

        /// <summary>
        /// To raise progress completed event.
        /// </summary>
        void RaiseProgressCompleted()
        {
            if (!IsIndeterminate && Progress >= ActualMaximum)
            {
                ProgressCompleted?.Invoke(this, new ProgressValueEventArgs
                {
                    CurrentProgress = Progress,
                });
            }
        }

        /// <summary>
        /// Called when animation progress finished. 
        /// </summary>
        void OnProgressAnimationFinished(double value, bool isCompleted)
        {
            AnimationExtensions.AbortAnimation(this, "ProgressAnimation");
            CanAnimate = false;
            RaiseProgressChanged();
            RaiseProgressCompleted();
        }

        /// <summary>
        /// To update progress animation value. 
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        void OnProgressAnimationUpdate(double value)
        {
            ActualProgress = value;
            CreateProgressPath();
            InvalidateDrawable();
        }

        /// <summary>
        /// Called when <see cref="GradientStops"/> collection changes.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        void GradientStops_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((obj, index, _) => { AddGradientStop(obj, index); if (obj is ProgressGradientStop) { ((ProgressGradientStop)obj).Parent = this; } }, (obj, index) => { RemoveGradientStop(obj, index); if (obj is ProgressGradientStop) { ((ProgressGradientStop)obj).Parent = null; } }, () => { ResetGradientStop(e); });
            if (!AvailableSize.IsZero)
            {
                CreateGradient();
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Handle's the gradient stop property changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument.</param>
        void GradientStops_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ProgressGradientStop.ColorProperty.PropertyName ||
                e.PropertyName == ProgressGradientStop.ValueProperty.PropertyName)
            {
                CreateGradient();
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// To add gradient stop.
        /// </summary>
        /// <param name="gradientStop">The gradient stop object.</param>
        /// <param name="index">The index value.</param>
        void AddGradientStop(object gradientStop, int index)
        {
            if (gradientStop is ProgressGradientStop newGradientStop)
            {
                newGradientStop.PropertyChanged -= GradientStops_PropertyChanged;
                newGradientStop.PropertyChanged += GradientStops_PropertyChanged;
            }
        }

        /// <summary>
        /// To remove gradient stop.
        /// </summary>
        /// <param name="gradientStop">The gradient stop object</param>
        /// <param name="index">The index value</param>
        void RemoveGradientStop(object gradientStop, int index)
        {
            if (gradientStop is ProgressGradientStop oldGradientStop)
            {
                oldGradientStop.PropertyChanged -= GradientStops_PropertyChanged;
            }
        }

        /// <summary>
        /// To clear gradient stops collection.
        /// </summary>
        /// <param name="e">The collection changed event arguments.</param>
        void ResetGradientStop(NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is ProgressGradientStop gradientStop)
                    {
                        gradientStop.PropertyChanged -= GradientStops_PropertyChanged;
                        gradientStop.Parent = null;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is ProgressGradientStop gradientStop)
                    {
                        gradientStop.PropertyChanged += GradientStops_PropertyChanged;
                        gradientStop.Parent = this;
                    }
                }
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Draws the progress bar.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The dirty rect.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            canvas.SaveState();
            DrawProgressBar(canvas);
            canvas.RestoreState();
        }


        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (GradientStops != null)
            {
                foreach (var gradientStop in GradientStops)
                {
                    SetInheritedBindingContext(gradientStop, BindingContext);
                }
            }
        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Called when <see cref="Minimum"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar)
            {
                progressBar.OnMinMaxChanged();
            }
        }

        /// <summary>
        /// Called when <see cref="Maximum"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar)
            {
                progressBar.OnMinMaxChanged();
                progressBar.RaiseProgressCompleted();
            }
        }

        /// <summary>
        /// Called when <see cref="TrackFill"/> or <see cref="ProgressFill"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar && !progressBar.AvailableSize.IsZero)
            {
                progressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="Progress"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnProgressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar)
            {
                progressBar.CanAnimate = progressBar.AnimationDuration > 0;
                if (!progressBar.AvailableSize.IsZero)
                {
                    progressBar.ActualProgress = (double)oldValue;
                    if (progressBar._skipProgressUpdate)
                    {
                        return;
                    }

                    progressBar.CreateProgressPath();
                    if (progressBar.AnimationDuration <= 0)
                    {
                        progressBar.RaiseProgressChanged();
                        progressBar.RaiseProgressCompleted();
                        progressBar.InvalidateDrawable();
                    }
                }
            }
        }

        /// <summary>
        /// Called when <see cref="SegmentCount"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSegmentPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar && !progressBar.AvailableSize.IsZero)
            {
                progressBar.UpdateProgressBar();
                progressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when the properties associated with indeterminate effect is changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnIndeterminatePropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar && !progressBar.AvailableSize.IsZero)
            {
                AnimationExtensions.AbortAnimation(progressBar, "IndeterminateAnimation");
                progressBar.UpdateProgressBar();
                progressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="GradientStops"/> property changes.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnGradientPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressBarBase progressBar)
            {
                if (oldValue != null && oldValue is ObservableCollection<ProgressGradientStop> oldGradientStops)
                {
                    oldGradientStops.CollectionChanged -= progressBar.GradientStops_CollectionChanged;
                    foreach (ProgressGradientStop gradientStop in oldGradientStops)
                    {
                        gradientStop.PropertyChanged -= progressBar.GradientStops_PropertyChanged;
                        gradientStop.Parent = null;
                    }
                }

                if (newValue != null && newValue is ObservableCollection<ProgressGradientStop> newGradientStops)
                {
                    newGradientStops.CollectionChanged += progressBar.GradientStops_CollectionChanged;
                    foreach (ProgressGradientStop gradientStop in newGradientStops)
                    {
                        gradientStop.Parent = progressBar;
                        SetInheritedBindingContext(gradientStop, progressBar.BindingContext);
                        gradientStop.PropertyChanged += progressBar.GradientStops_PropertyChanged;
                    }
                }

                if (!progressBar.AvailableSize.IsZero)
                {
                    progressBar.CreateGradient();
                    progressBar.InvalidateDrawable();
                }
            }
        }

        #endregion

        #region  Virtual Methods

        /// <summary>
        /// To draw a progress bar elements.
        /// </summary>
        /// <param name="canvas">The drawing canvas.</param>
        internal virtual void DrawProgressBar(ICanvas canvas)
        {
            DrawTrack(canvas);
        }

        /// <summary>
        /// To update the progress bar.
        /// </summary>
        internal virtual void UpdateProgressBar()
        {
            if (!AvailableSize.IsZero)
            {
                CreateTrackPath();
                if (IsIndeterminate
                    && IndeterminateAnimationDuration > 0
                    && IndeterminateIndicatorWidthFactor > 0
                    && !AnimationExtensions.AnimationIsRunning(this, "IndeterminateAnimation"))
                {
                    CreateIndeterminateAnimation();
                }
                else
                {
                    CreateProgressPath();
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// The value change event occurs when <see cref="Progress"/> is changed.
        /// </summary>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// This snippet shows how to hook ProgressChanged event for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressChanged="progressBar_ProgressChanged" 
        ///                                  Progress = "50" />
        /// ]]></code>
        /// This snippet shows how to hook ProgressChanged event for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ProgressChanged="progressBar_ProgressChanged" 
        ///                                    Progress = "50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public event EventHandler<ProgressValueEventArgs>? ProgressChanged;

        /// <summary>
        /// The progress completed event occurs when <see cref="Progress"/> attains <see cref="Maximum"/>.
        /// </summary>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// This snippet shows how to hook ProgressCompleted event for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressCompleted="progressBar_ProgressCompleted" 
        ///                                  Progress = "100"/>
        /// ]]></code>
        /// This snippet shows how to hook ProgressCompleted event for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ProgressCompleted="progressBar_ProgressCompleted" 
        ///                                    Progress = "100"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public event EventHandler<ProgressValueEventArgs>? ProgressCompleted;

        #endregion
    }
}