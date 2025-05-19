using Microsoft.Maui.Animations;
using Syncfusion.Maui.Toolkit.Themes;
using GradientStop = Microsoft.Maui.Graphics.PaintGradientStop;

namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Represents <see cref="SfCircularProgressBar"/> class, which is a UI control that displays a circular progress indicator.
    /// This control allows for visualizing the progress of an operation through a customizable bar
    /// It supports features such as determinate and indeterminate progress modes,
    /// secondary progress indication, customizable appearance including track and progress colors,
    /// segment divisions, corner radius customization, and animation effects.
    /// </summary>
    /// <example>
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    /// <progressBar:SfCircularProgressBar Minimum="0"
    ///                                    Maximum="100"
    ///                                    TrackFill="Blue"
    ///                                    Progress="70"
    ///                                    ProgressFill="Yellow"
    ///                                    SegmentCount="1"
    ///                                    SegmentGapWidth="10"
    ///                                    IsIndeterminate="False"
    ///                                    IndeterminateIndicatorWidthFactor="0.2"
    ///                                    IndeterminateAnimationEasing="{x:Static Easing.SpringIn}"
    ///                                    IndeterminateAnimationDuration="1500"
    ///                                    AnimationEasing="{x:Static Easing.SpringOut}"
    ///                                    AnimationDuration="4000"
    ///                                    StartAngle="0"
    ///                                    EndAngle="0"
    ///                                    TrackRadiusFactor="0.9"
    ///                                    TrackThickness="1"
    ///                                    ThicknessUnit="Factor"
    ///                                    ProgressRadiusFactor="0.7"
    ///                                    ProgressThickness="0.7"
    ///                                    ProgressCompleted="SfCircularProgressBar_ProgressCompleted"
    ///                                    ProgressChanged="SfCircularProgressBar_ProgressChanged">
    ///
    ///     <progressBar:SfCircularProgressBar.Content>
    ///         <Label Text = "{Binding Source={x:Reference circularProgressBar}, Path=Progress}" />
    ///     </progressBar:SfCircularProgressBar.Content>
    ///
    ///     <progressBar:SfCircularProgressBar.GradientStops>
    ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30" />
    ///            <progressBar:ProgressGradientStop Color="Green" Value="60" />
    ///     </progressBar:SfCircularProgressBar.GradientStops>
    /// </progressBar:SfCircularProgressBar>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// private void SfCircularProgressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
    /// {
    ///    DisplayAlert("ProgressCompleted", "Progress: " + e.Progress, "Ok");
    /// }
    /// private void SfCircularProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    /// {
    ///     DisplayAlert("ValueChanged", "Progress: " + e.Progress, "Ok");
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    [ContentProperty(nameof(Content))]
    public class SfCircularProgressBar : ProgressBarBase, IParentThemeElement
    {
        #region Fields

        /// <summary>
        /// Backing field to store actual sweep angle of progress bar.
        /// ie., Calculated actual sweep angle after validating start and end angle.
        /// </summary>
        double _actualSweepAngle;

        /// <summary>
        /// Backing field to store actual start angle of progress bar.
        /// ie., Validated the customer provided value and stored in it and this value should be used in all calculations.
        /// </summary>
        double _actualStartAngle;

        /// <summary>
        /// Backing field to store actual end angle of progress bar.
        /// ie., Validated the customer provided value and stored in it and this value should be used in all calculations.
        /// </summary>
        double _actualEndAngle;

        /// <summary>
        /// Backing field to store progress bar center.
        /// </summary>
        PointF _center;

        /// <summary>
        /// Backing field to store progress bar arrange size.
        /// </summary>
        Size _arrangeSize = Size.Zero;

        /// <summary>
        /// Backing field to store radius of progress bar.
        /// </summary>
        double _radius;

        /// <summary>
        /// Backing field to store the animation start value.
        /// </summary>
        double _animationStart;

        /// <summary>
        /// Backing field to store the animation end value.
        /// </summary>
        double _animationEnd;

        /// <summary>
        /// Backing field to store gradient arc path list.
        /// </summary>
        List<CircularProgressBarArcInfo>? _gradientArcPaths;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartAngle"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartAngleProperty =
           BindableProperty.Create(
               nameof(StartAngle),
               typeof(double),
               typeof(SfCircularProgressBar),
               270d,
               propertyChanged: OnAnglePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndAngle"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndAngleProperty =
            BindableProperty.Create(
                nameof(EndAngle),
                typeof(double),
                typeof(SfCircularProgressBar),
                630d,
                propertyChanged: OnAnglePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TrackRadiusFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackRadiusFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackRadiusFactorProperty =
            BindableProperty.Create(
                nameof(TrackRadiusFactor),
                typeof(double),
                typeof(SfCircularProgressBar), 0.9d,
                coerceValue: (bindable, value) =>
                {
                    return Math.Clamp((double)value, 0, 1);
                },
                propertyChanged: OnTrackPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="TrackThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackThicknessProperty =
            BindableProperty.Create(
                nameof(TrackThickness),
                typeof(double),
                typeof(SfCircularProgressBar),
                5d,
                propertyChanged: OnTrackPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="ThicknessUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ThicknessUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty ThicknessUnitProperty =
            BindableProperty.Create(
                nameof(ThicknessUnit),
                typeof(SizeUnit),
                typeof(SfCircularProgressBar),
                SizeUnit.Pixel,
                propertyChanged: OnThicknessUnitChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressRadiusFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressRadiusFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressRadiusFactorProperty =
            BindableProperty.Create(
                nameof(ProgressRadiusFactor),
                typeof(double),
                typeof(SfCircularProgressBar),
                0.9d,
                coerceValue: (bindable, value) =>
                {
                    return Math.Clamp((double)value, 0, 1);
                },
                propertyChanged: OnProgressPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressThicknessProperty =
            BindableProperty.Create(
                nameof(ProgressThickness),
                typeof(double),
                typeof(SfCircularProgressBar),
                5d,
                propertyChanged: OnProgressPropertiesChanged);

        /// <summary>
        /// Identifies the <see cref="TrackCornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrackCornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty TrackCornerStyleProperty =
            BindableProperty.Create(
                nameof(TrackCornerStyle),
                typeof(CornerStyle),
                typeof(SfCircularProgressBar),
                CornerStyle.BothFlat,
                propertyChanged: OnTrackCornerStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ProgressCornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ProgressCornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty ProgressCornerStyleProperty =
            BindableProperty.Create(
                nameof(ProgressCornerStyle),
                typeof(CornerStyle),
                typeof(SfCircularProgressBar),
                CornerStyle.BothFlat, propertyChanged: OnProgressCornerStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(
                nameof(Content),
                typeof(View),
                typeof(SfCircularProgressBar),
                null,
                propertyChanged: OnContentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CircularProgressBarBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CircularProgressBarBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty CircularProgressBarBackgroundProperty =
          BindableProperty.Create(
              nameof(CircularProgressBarBackground),
              typeof(Color),
              typeof(SfCircularProgressBar),
              defaultValueCreator: bindable => Color.FromArgb("#FFFBFE"),
              propertyChanged: OnCircularProgressBarBackgroundChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCircularProgressBar"/> class.
        /// </summary>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Minimum="0"
        ///                                    Maximum="100"
        ///                                    TrackFill="Blue"
        ///                                    Progress="70"
        ///                                    ProgressFill="Yellow"
        ///                                    SegmentCount="1"
        ///                                    SegmentGapWidth="10"
        ///                                    IsIndeterminate="False"
        ///                                    IndeterminateIndicatorWidthFactor="0.2"
        ///                                    IndeterminateAnimationEasing="{x:Static Easing.SpringIn}"
        ///                                    IndeterminateAnimationDuration="1500"
        ///                                    AnimationEasing="{x:Static Easing.SpringOut}"
        ///                                    AnimationDuration="4000"
        ///                                    StartAngle="0"
        ///                                    EndAngle="0"
        ///                                    TrackRadiusFactor="0.9"
        ///                                    TrackThickness="1"
        ///                                    ThicknessUnit="Factor"
        ///                                    ProgressRadiusFactor="0.7"
        ///                                    ProgressThickness="0.7"
        ///                                    ProgressCompleted="SfCircularProgressBar_ProgressCompleted"
        ///                                    ProgressChanged="SfCircularProgressBar_ProgressChanged">
        ///
        ///     <progressBar:SfCircularProgressBar.Content>
        ///         <Label Text = "{Binding Source={x:Reference circularProgressBar}, Path=Progress}" />
        ///     </progressBar:SfCircularProgressBar.Content>
        ///
        ///     <progressBar:SfCircularProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfCircularProgressBar.GradientStops>
        /// </progressBar:SfCircularProgressBar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// private void SfCircularProgressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
        /// {
        ///    DisplayAlert("ProgressCompleted", "Progress: " + e.Progress, "Ok");
        /// }
        /// private void SfCircularProgressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
        /// {
        ///     DisplayAlert("ValueChanged", "Progress: " + e.Progress, "Ok");
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public SfCircularProgressBar()
        {
            ValidateStartEndAngle();
            ThemeElement.InitializeThemeResources(this, "SfCircularProgressBarTheme");
            BackgroundColor = CircularProgressBarBackground;
            // TASK-886910: SfLinearProgressBar gets frozen when switching between tabs in TabBar
            Loaded += OnProgressBarLoaded;
            Unloaded += OnProgressBarUnloaded;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the <see cref="StartAngle"/> of the progress bar.
        /// </summary>
        /// <value>
        /// It defines the start angle of the progress bar. The default value is <c>270</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar StartAngle="90" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.StartAngle = 90;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the <see cref="EndAngle"/> of the progress bar.
        /// </summary>
        /// <value>
        /// It defines the end angle of the progress bar. The default value is <c>630</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar EndAngle="90" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.EndAngle = 90;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the outer radius factor of the track.
        /// </summary>
        /// <value>
        /// This value ranges from 0 to 1. It defines the radius factor of the track. The default value is <c>0.9</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar TrackRadiusFactor="0.5" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.TrackRadiusFactor = 0.5;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double TrackRadiusFactor
        {
            get { return (double)GetValue(TrackRadiusFactorProperty); }
            set { SetValue(TrackRadiusFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the thickness of track in circular progress bar.
        /// You can specify value either in logical pixel or radius factor using the <see cref="ThicknessUnit"/> property.
        /// </summary>
        /// <value>
        /// The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar TrackThickness="20" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.TrackThickness = 20;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double TrackThickness
        {
            get { return (double)GetValue(TrackThicknessProperty); }
            set { SetValue(TrackThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets enum value that indicates to calculate the track and progress thickness either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the thickness unit value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ThicknessUnit="Factor"
        ///                                    Progress="100"
        ///                                    TrackThickness="0.05"
        ///                                    ProgressRadiusFactor="0.85"
        ///                                    ProgressThickness="1" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.ThicknessUnit = SizeUnit.Factor;
        /// progressBar.Progress = 100;
        /// progressBar.TrackThickness = 0.05;
        /// progressBar.ProgressRadiusFactor = 0.85;
        /// progressBar.ProgressThickness = 1;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public SizeUnit ThicknessUnit
        {
            get { return (SizeUnit)GetValue(ThicknessUnitProperty); }
            set { SetValue(ThicknessUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the outer radius factor of the progress.
        /// </summary>
        /// <value>
        /// It defines the radius factor of the progress. The default value is <c>0.9</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ProgressRadiusFactor="0.5"
        ///                                    Progress="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.ProgressRadiusFactor = 0.5;
        /// progressBar.Progress = 75;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double ProgressRadiusFactor
        {
            get { return (double)GetValue(ProgressRadiusFactorProperty); }
            set { SetValue(ProgressRadiusFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the thickness for the progress.
        /// You can specify value either in logical pixel or radius factor using the <see cref="ThicknessUnit"/> property.
        /// </summary>
        /// <value>
        /// The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="75"
        ///                                    ProgressThickness="20" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.ProgressThickness = 20;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double ProgressThickness
        {
            get { return (double)GetValue(ProgressThicknessProperty); }
            set { SetValue(ProgressThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the corner style of the track.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the corner style of the track in progress bar.
        /// The default is <see cref="CornerStyle.BothFlat"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar StartAngle="180"
        ///                                    EndAngle="360"
        ///                                    TrackCornerStyle="BothCurve" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.StartAngle = 180;
        /// progressBar.EndAngle = 360;
        /// progressBar.TrackCornerStyle = CornerStyle.BothCurve;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerStyle TrackCornerStyle
        {
            get { return (CornerStyle)GetValue(TrackCornerStyleProperty); }
            set { SetValue(TrackCornerStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the corner style of the progress.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the corner style of the progress in progress bar.
        /// The default is <see cref="CornerStyle.BothFlat"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ProgressCornerStyle="BothCurve"
        ///                                    Progress="50" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.ProgressCornerStyle = CornerStyle.BothCurve;
        /// progressBar.Progress = 50;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerStyle ProgressCornerStyle
        {
            get { return (CornerStyle)GetValue(ProgressCornerStyleProperty); }
            set { SetValue(ProgressCornerStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a any view to display in the center of circular progress bar.
        /// </summary>
        /// <value>
        /// An object that contains the progress bar visual content. The default value is <c>null</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar x:Name="progressBar"
        ///                                    Progress="50">
        ///      <progressBar:SfCircularProgressBar.Content>
        ///          <Label Text="{Binding Source={x:Reference progressBar}, Path=Progress, Mode=TwoWay}" />
        ///      </progressBar:SfCircularProgressBar.Content>
        /// </progressBar:SfCircularProgressBar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// Label label = new Label();
        /// label.SetBinding(Label.TextProperty, new Binding("Progress", source: progressBar));
        /// progressBar.Progress = 50;
        /// progressBar.Content = label;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the circular progress bar.
        /// </summary>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar CircularProgressBarBackground="Red"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.CircularProgressBarBackground = Colors.Red;
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        internal Color CircularProgressBarBackground
        {
            get { return (Color)GetValue(CircularProgressBarBackgroundProperty); }
            set { SetValue(CircularProgressBarBackgroundProperty, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To create gradient arc segments.
        /// </summary>
        /// <param name="gradientStops">Thre gradient stop list.</param>
        /// <param name="innerStartRadius">The inner start radius.</param>
        /// <param name="innerEndRadius">The inner end radius.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns>The progress bar arc info.</returns>
        List<CircularProgressBarArcInfo>? CreateGradientArcSegments(
            List<ProgressGradientStop> gradientStops,
            double innerStartRadius,
            double innerEndRadius,
            double rangeStart,
            double rangeEnd)
        {
            gradientStops = Utility.UpdateGradientStopCollection(gradientStops, rangeStart, rangeEnd);
            var gradientRange = Utility.GetGradientRange(gradientStops, rangeStart, rangeEnd);
            double tempInnerStart = innerStartRadius;
            double tempInnerEnd = innerEndRadius;
            List<CircularProgressBarArcInfo> arcInfoCollection = new List<CircularProgressBarArcInfo>();
            for (int i = 0; i < gradientRange.Count - 1; i++)
            {
                double startAngle = ValueToAngle(gradientRange[i]);
                double endAngle = ValueToAngle(gradientRange[i + 1]);
                double offset = 0.5; // Added .5 degree at the end angle to avoid line difference.
                endAngle += i < gradientRange.Count - 2 ? offset : 0;
                double rangeMidAngle = Math.Abs(endAngle - startAngle) / 2;
                Color color1 = gradientStops[i].Color;
                Color color2 = gradientStops[i + 1].Color;
                for (int j = 0; j < gradientStops.Count - 1; j++)
                {
                    if (gradientStops[j].ActualValue <= gradientRange[i] && gradientStops[j + 1].ActualValue > gradientRange[i])
                    {
                        double offset1 = (gradientRange[i] - gradientStops[j].ActualValue) / (gradientStops[j + 1].ActualValue - gradientStops[j].ActualValue);
                        color1 = gradientStops[j].Color.Lerp(gradientStops[j + 1].Color, offset1);
                        double offset2 = (gradientRange[i + 1] - gradientStops[j].ActualValue) / (gradientStops[j + 1].ActualValue - gradientStops[j].ActualValue);
                        color2 = gradientStops[j].Color.Lerp(gradientStops[j + 1].Color, offset2);
                    }
                }

                if (innerStartRadius != innerEndRadius)
                {
                    double innerOffsetFraction = (tempInnerEnd - tempInnerStart) / (rangeEnd - rangeStart);
                    double rangeStartOffset = rangeStart * innerOffsetFraction;
                    innerEndRadius = tempInnerStart + (gradientRange[i + 1] * innerOffsetFraction) - rangeStartOffset;
                    innerStartRadius = tempInnerStart + (gradientRange[i] * innerOffsetFraction) - rangeStartOffset;
                }

                if (rangeMidAngle <= 90)
                {
                    CircularProgressBarArcInfo arcInfo = CreateGradientArcSegment(startAngle, endAngle, color1, color2, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);
                }
                else
                {
                    Color midColor = color1.Lerp(color2, 0.5);
                    double midValue = gradientRange[i] + ((gradientRange[i + 1] - gradientRange[i]) / 2);
                    var midAngle = ValueToAngle(midValue);
                    CircularProgressBarArcInfo arcInfo = CreateGradientArcSegment(startAngle, midAngle + 0.25, color1, midColor, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);
                    arcInfo = CreateGradientArcSegment(midAngle, endAngle, midColor, color2, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);
                }
            }

            return arcInfoCollection;
        }

        /// <summary>
        /// To create gradient arc segment.
        /// </summary>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        /// <param name="color1">The start color.</param>
        /// <param name="color2">The end color.</param>
        /// <param name="innerStartRadius">The inner start radius.</param>
        /// <param name="innerEndRadius">The inner end radius.</param>
        /// <returns>The circular progress bar arc info.</returns>
        CircularProgressBarArcInfo CreateGradientArcSegment(double startAngle, double endAngle, Color color1, Color color2, double innerStartRadius, double innerEndRadius)
        {
            Point startPoint, endPoint;
            CalculateGradientArcOffset(startAngle, endAngle, out startPoint, out endPoint);
            LinearGradientPaint gradient = new LinearGradientPaint()
            {
                StartPoint = startPoint,
                EndPoint = endPoint
            };

            gradient.GradientStops = new GradientStop[]
            {
                new GradientStop( 0.1f ,color1),
                new GradientStop(0.9f, color2)
            };

            CircularProgressBarArcInfo gaugeArcInfo = new CircularProgressBarArcInfo()
            {
                FillPaint = gradient,
                ArcPath = new PathF(),
                StartAngle = (float)startAngle,
                EndAngle = (float)endAngle,
                InnerStartRadius = innerStartRadius,
                InnerEndRadius = innerEndRadius
            };

            return gaugeArcInfo;
        }

        /// <summary>
        /// Calculate gradient arc offset.
        /// </summary>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        void CalculateGradientArcOffset(double startAngle, double endAngle, out Point startPoint, out Point endPoint)
        {
            var start = startAngle > 360 ? startAngle % 360 : startAngle;
            start += 0.5;// Added .5 degree at the end angle to avoid line difference.
            var end = endAngle > 360 ? endAngle % 360 : endAngle;
            if (start >= 0 && start < 90)
            {
                startPoint = new Point(1, 0.5);
                endPoint = new Point(0, 0.5);
            }
            else if (start >= 90 && start < 180)
            {
                startPoint = new Point(0.5, 1);
                endPoint = new Point(0.5, 0);
            }
            else if (start >= 180 && start < 270)
            {
                if (end >= 180 && end < 270)
                {
                    startPoint = new Point(0.5, 1);
                    endPoint = new Point(0.5, 0);
                }
                else
                {
                    startPoint = new Point(0, 0.5);
                    endPoint = new Point(1, 0.5);
                }
            }
            else
            {
                startPoint = new Point(0.5, 0);
                endPoint = new Point(0.5, 1);
            }
        }

        /// <summary>
        /// To create gradient arc.
        /// </summary>
        /// <param name="outerArcTopLeft">The outer arc top left.</param>
        /// <param name="outerArcBottomRight">The outer arc bottom right.</param>
        /// <param name="innerArcTopLeft">The inner arc top left.</param>
        /// <param name="innerArcBottomRight">The inner arc bottom right.</param>
        /// <param name="outerRadius">The outer radius.</param>
        /// <param name="innerRadius">The inner radius.</param>
        /// <param name="startCurveCapCenter">The inner radius.</param>
        /// <param name="endCurveCapCenter">The inner radius.</param>
        void CreateGradientArc(
            PointF outerArcTopLeft,
            PointF outerArcBottomRight,
            PointF innerArcTopLeft, PointF innerArcBottomRight,
            float outerRadius,
            float innerRadius,
            Point? startCurveCapCenter = null,
            Point? endCurveCapCenter = null)
        {
            double halfWidth = (outerRadius - innerRadius) / 2;
            double cornerRadiusAngle = Utility.CornerRadiusAngle(_radius, halfWidth);
            if (_gradientArcPaths != null)
            {
                for (int i = 0; i < _gradientArcPaths.Count; i++)
                {
                    CircularProgressBarArcInfo arcInfo = _gradientArcPaths[i];

                    // Create gradient arc path.
                    if (i == 0 && startCurveCapCenter != null)
                    {
                        arcInfo.StartAngle += (float)cornerRadiusAngle;
                    }

                    if (endCurveCapCenter != null)
                    {
                        float endCornerAngle = (float)cornerRadiusAngle;
                        if (i == _gradientArcPaths.Count - 1)
                        {
                            arcInfo.EndAngle -= endCornerAngle;
                            if (i > 0 && arcInfo.EndAngle < arcInfo.StartAngle)
                                arcInfo.StartAngle = arcInfo.EndAngle;
                        }

                        if (i == _gradientArcPaths.Count - 2 && (_gradientArcPaths[i + 1].EndAngle - endCornerAngle) < arcInfo.EndAngle)
                        {
                            arcInfo.EndAngle = _gradientArcPaths[i + 1].EndAngle - endCornerAngle;
                        }
                    }

                    CreateFilledArc(
                        arcInfo.ArcPath,
                        outerArcTopLeft,
                        outerArcBottomRight,
                        innerArcTopLeft,
                        innerArcBottomRight,
                        arcInfo.StartAngle,
                        arcInfo.EndAngle,
                        outerRadius,
                        innerRadius);

                    // Append circle in edge for corner style.
                    if (i == 0 && startCurveCapCenter != null)
                    {
                        arcInfo.ArcPath.AppendCircle(
                            (float)startCurveCapCenter.Value.X,
                            (float)startCurveCapCenter.Value.Y,
                            (float)halfWidth);
                    }

                    if (i == _gradientArcPaths.Count - 1 && endCurveCapCenter != null)
                    {
                        arcInfo.ArcPath.AppendCircle(
                            (float)endCurveCapCenter.Value.X,
                            (float)endCurveCapCenter.Value.Y,
                            (float)halfWidth);
                    }
                }
            }
        }

        /// <summary>
        /// To update indeterminate animation value.
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        void OnIndeterminateAnimationUpdate(double value)
        {
            _animationStart = value;
            _animationEnd = value + 360 * IndeterminateIndicatorWidthFactor;
            _animationStart %= 360;
            _animationEnd %= 360;
            _animationEnd = (_animationEnd - _animationStart) % 360 == 0
                ? _animationEnd - 0.01
                : _animationEnd;
            while (_animationEnd < _animationStart)
            {
                _animationEnd += 360;
            }

            CreateProgressPath();
            InvalidateDrawable();
        }

        /// <summary>
        /// Calculate the progress percentage.
        /// </summary>
        /// <returns>The progress percentage.</returns>
        /// <param name="indicatorValue">The current progress value.</param>
        /// <param name="min">The segment minimum value.</param>
        /// <param name="max">The segment maximum value.</param>
        double CalculateProgressPercentage(double indicatorValue, double min, double max)
        {
            if (max > min)
            {
                double delta = max - min;
                indicatorValue -= min;
                if (delta > 0 && indicatorValue > 0)
                {
                    double percent = indicatorValue / delta * 100;
                    return percent < 100 ? percent : 100;
                }
            }

            return 0;
        }

        /// <summary>
        /// To create range or progress path.
        /// </summary>
        /// <param name="isTrack"><c>True</c> to create track path; otherwise progress path.</param>
        /// <param name="animationDuration">The animation duration.</param>
        void CreatePath(bool isTrack, double? animationDuration = null)
        {
            animationDuration ??= AnimationDuration;
            float outerRadius, innerRadius;
            double actualEndValue = 0, actualStartAngle, actualEndAngle, midRadius, halfWidth, actualSweepAngle;
            PointF outerArcTopLeft, outerArcBottomRight, innerArcTopLeft, innerArcBottomRight;
            CornerStyle cornerStyle;
            if (isTrack)
            {
                TrackPath = new PathF();
                InitializeTrackPath(out actualStartAngle, out actualEndAngle, out actualSweepAngle, out cornerStyle);
            }
            else
            {
                ProgressPath = new PathF();
                actualEndValue = Math.Clamp(animationDuration > 0 ? ActualProgress : ActualProgress = Progress, ActualMinimum, ActualMaximum);
                cornerStyle = ProgressCornerStyle;

                if (ActualMinimum == actualEndValue && !IsIndeterminate)
                {
                    ProgressPath = null;
                    _gradientArcPaths = null;
                    return;
                }

                InitializeProgressPath(out actualStartAngle, out actualEndAngle, out actualSweepAngle, actualEndValue);
            }

            outerRadius = GetRadiusFromFactor(isTrack ? TrackRadiusFactor : ProgressRadiusFactor, SizeUnit.Factor);
            innerRadius = outerRadius - GetRadiusFromFactor(isTrack ? TrackThickness : ProgressThickness, ThicknessUnit);
            innerRadius = innerRadius < 0 ? 0 : innerRadius;
            halfWidth = (outerRadius - innerRadius) / 2;
            midRadius = outerRadius == 0 ? 0 : outerRadius - halfWidth;

            // Calculating outer arc bounds.
            outerArcTopLeft = GetArcTopLeft(outerRadius);
            outerArcBottomRight = GetArcBottomRight(outerRadius);

            // Calculating inner arc bounds.
            innerArcTopLeft = GetArcTopLeft(innerRadius);
            innerArcBottomRight = GetArcBottomRight(innerRadius);
            Point? endCurveCapCenter = null;
            Point? startCurveCapCenter = null;
            double cornerRadiusAngle = Utility.CornerRadiusAngle(_radius, halfWidth);
            if (cornerStyle == CornerStyle.StartCurve || cornerStyle == CornerStyle.BothCurve && actualSweepAngle != 359.99)
            {
                if (actualStartAngle + cornerRadiusAngle < actualEndAngle)
                {
                    actualStartAngle += cornerRadiusAngle;
                    startCurveCapCenter = CalculateCapCenter(actualStartAngle, midRadius);
                }
            }

            if (cornerStyle == CornerStyle.EndCurve || cornerStyle == CornerStyle.BothCurve && actualSweepAngle != 359.99)
            {
                if (actualEndAngle - cornerRadiusAngle > actualStartAngle)
                {
                    actualEndAngle -= cornerRadiusAngle;
                    endCurveCapCenter = CalculateCapCenter(actualEndAngle, midRadius);
                }
            }

            if (SegmentCount > 1 && !IsIndeterminate)
            {
                CreateSegmentedPath(
                    isTrack, outerArcTopLeft,
                    outerArcBottomRight,
                    innerArcTopLeft,
                    innerArcBottomRight,
                    outerRadius,
                    innerRadius);
            }
            else
            {
                if (!isTrack && !IsIndeterminate && GradientStops != null && GradientStops.Count > 0)
                {
                    ProgressPath = null;
                    _gradientArcPaths = CreateGradientArcSegments(GradientStops.ToList(), innerRadius,
                    innerRadius, ActualMinimum, actualEndValue);
                    CreateGradientArc(
                        outerArcTopLeft,
                        outerArcBottomRight,
                        innerArcTopLeft,
                        innerArcBottomRight,
                        outerRadius,
                        innerRadius,
                        startCurveCapCenter,
                        endCurveCapCenter);
                }
                else
                {
                    if (!isTrack)
                    {
                        _gradientArcPaths = null;
                    }

                    CreateFilledArc(isTrack ? TrackPath : ProgressPath,
                        outerArcTopLeft,
                        outerArcBottomRight,
                        innerArcTopLeft,
                        innerArcBottomRight,
                        actualStartAngle,
                        actualEndAngle,
                        outerRadius,
                        innerRadius);

                    // For 360 angle, fill arc not displaying. To resolve the issue drawn 359.99 angle arc and small gap filled by circle.
                    if (actualSweepAngle == 359.99)
                    {
                        Point vector = Utility.AngleToVector(actualStartAngle);
                        PointF point = new PointF((float)(_center.X + (midRadius * vector.X)),
                           (float)(_center.Y + (midRadius * vector.Y)));
                        (isTrack ? TrackPath : ProgressPath)?.AppendCircle(point, (float)halfWidth);
                    }
                }

                if (startCurveCapCenter != null)
                {
                    (isTrack ? TrackPath : ProgressPath)?.AppendCircle(
                        (float)startCurveCapCenter.Value.X,
                        (float)startCurveCapCenter.Value.Y,
                        (float)halfWidth);
                }

                if (endCurveCapCenter != null)
                {
                    (isTrack ? TrackPath : ProgressPath)?.AppendCircle(
                        (float)endCurveCapCenter.Value.X,
                        (float)endCurveCapCenter.Value.Y,
                        (float)halfWidth);
                }
            }
        }

        /// <summary>
        /// Initializes the track path properties of the progress bar.
        /// </summary>
        /// <param name="actualStartAngle">The actual start angle for rendering the track path.</param>
        /// <param name="actualEndAngle">The actual end angle for rendering the track path.</param>
        /// <param name="actualSweepAngle">The actual sweep angle for the track path.</param>
        /// <param name="cornerStyle">Specifies the corner style of the track.</param>
        /// <remarks>
        /// If the progress bar is in an indeterminate state, this method resets the angles to cover the entire circle and sets the corner style to flat.
        /// </remarks>
        void InitializeTrackPath(out double actualStartAngle, out double actualEndAngle, out double actualSweepAngle, out CornerStyle cornerStyle)
        {
            actualStartAngle = _actualStartAngle;
            actualEndAngle = _actualEndAngle;
            actualSweepAngle = _actualSweepAngle;
            cornerStyle = TrackCornerStyle;
            if (IsIndeterminate)
            {
                actualStartAngle = 0;
                actualEndAngle = 359.99;
                actualSweepAngle = 359.99;
                cornerStyle = CornerStyle.BothFlat;
            }
        }

        /// <summary>
        /// Initializes the progress path properties of the progress bar.
        /// </summary>
        /// <param name="actualStartAngle">The actual start angle for rendering the progress path.</param>
        /// <param name="actualEndAngle">The actual end angle for rendering the progress path.</param>
        /// <param name="actualSweepAngle">The actual sweep angle for the progress path.</param>
        /// <param name="actualEndValue">The value used to calculate the actual end angle of the progress.</param>
        /// <remarks>
        /// If the progress bar is in an indeterminate state, this method calculates the angles and sweep factor based on the animation settings.
        /// </remarks>
        void InitializeProgressPath(out double actualStartAngle, out double actualEndAngle, out double actualSweepAngle, double actualEndValue)
        {
            actualStartAngle = ValueToAngle(ActualMinimum);
            actualEndAngle = ValueToAngle(actualEndValue);
            if (IsIndeterminate)
            {
                actualStartAngle = 0;
                if (IndeterminateIndicatorWidthFactor <= 0)
                {
                    actualEndAngle = 0;
                }
                else if (IndeterminateIndicatorWidthFactor >= 1)
                {
                    actualEndAngle = 359.99;
                }
                else
                {
                    actualStartAngle = _animationStart;
                    actualEndAngle = _animationEnd;
                }
            }

            actualSweepAngle = Utility.CalculateSweepAngle(actualStartAngle, actualEndAngle);
        }

        /// <summary>
        /// To calculate cap center.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="midRadius">The mid radius.</param>
        /// <returns></returns>
        Point CalculateCapCenter(double angle, double midRadius)
        {
            Point vector = Utility.AngleToVector(angle);
            return new Point(_center.X + (midRadius * vector.X),
                        _center.Y + (midRadius * vector.Y));
        }

        /// <summary>
        /// To create segmented path for range and progress.
        /// </summary>
        /// <param name="isTrack"><c>True</c> to create range path; otherwise progress path.</param>
        /// <param name="outerArcTopLeft">The top left point of outer arc.</param>
        /// <param name="outerArcBottomRight">The bottom right point of outer arc.</param>
        /// <param name="innerArcTopLeft">The top left point of inner arc.</param>
        /// <param name="innerArcBottomRight">the bottom right point of inner arc.</param>
        /// <param name="outerRadius">The outer radius of arc.</param>
        /// <param name="innerRadius">The inner radius of arc.</param>
        void CreateSegmentedPath(bool isTrack, Point outerArcTopLeft, Point outerArcBottomRight, Point innerArcTopLeft, Point innerArcBottomRight, double outerRadius, double innerRadius)
        {
            double halfWidth = (outerRadius - innerRadius) / 2;
            double midRadius = outerRadius == 0 ? 0 : outerRadius - halfWidth;
            int actualSegmentCount = SegmentCount > 1 ? SegmentCount : 1;
            double actualGapWidth = SegmentGapWidth > 0 ? SegmentGapWidth : 0;
            float angleOfSlice = (float)(Math.Abs(_actualSweepAngle) > (actualGapWidth * actualSegmentCount) ? (_actualSweepAngle - (actualGapWidth * actualSegmentCount)) / actualSegmentCount : 0);
            float sliceAngle = _actualSweepAngle == 359.99 ? 360 / actualSegmentCount : (float)(_actualSweepAngle / actualSegmentCount);
            double progressAngle = _actualStartAngle + (_actualSweepAngle / 100 * CalculateProgressPercentage(AnimationDuration > 0 ? ActualProgress : ActualProgress = Progress, ActualMinimum, ActualMaximum));
            int segment = isTrack ? actualSegmentCount : (int)Math.Ceiling(Math.Round((progressAngle - _actualStartAngle) / sliceAngle, 2));
            double actualStartAngle = _actualStartAngle + actualGapWidth / 2;
            double range = (ActualMaximum - ActualMinimum) / actualSegmentCount;
            double min, max = 0;
            CornerStyle cornerStyle;
            for (int i = 0; i < segment; i++)
            {
                double segmentStartAngle = actualStartAngle;
                double segmentEndAngle = segmentStartAngle + angleOfSlice;
                actualStartAngle = segmentEndAngle + actualGapWidth;
                cornerStyle = TrackCornerStyle;
                if (!isTrack)
                {
                    double sweepAngle = segmentEndAngle - segmentStartAngle;
                    min = i == 0 ? ActualMinimum : max;
                    max = min + range;
                    segmentEndAngle = segmentStartAngle + (sweepAngle / 100 * CalculateProgressPercentage(AnimationDuration > 0 ? ActualProgress : ActualProgress = Progress, min, max));
                    cornerStyle = ProgressCornerStyle;
                }

                Point? endCurveCapCenter = null;
                Point? startCurveCapCenter = null;
                double cornerRadiusAngle = Utility.CornerRadiusAngle(_radius, halfWidth);

                if (i == 0)
                {
                    if (cornerStyle == CornerStyle.StartCurve || cornerStyle == CornerStyle.BothCurve)
                    {
                        if (segmentStartAngle + cornerRadiusAngle < segmentEndAngle)
                        {
                            segmentStartAngle += cornerRadiusAngle;
                            startCurveCapCenter = CalculateCapCenter(segmentStartAngle, midRadius);
                        }
                    }

                    if (startCurveCapCenter != null)
                    {
                        (isTrack ? TrackPath : ProgressPath)?.AppendCircle((float)startCurveCapCenter.Value.X, (float)startCurveCapCenter.Value.Y, (float)halfWidth);
                    }
                }

                if (i == segment - 1)
                {
                    if (cornerStyle == CornerStyle.EndCurve || cornerStyle == CornerStyle.BothCurve)
                    {
                        if (segmentEndAngle - cornerRadiusAngle > segmentStartAngle)
                        {
                            segmentEndAngle -= cornerRadiusAngle;
                            endCurveCapCenter = CalculateCapCenter(segmentEndAngle, midRadius);
                        }
                    }

                    if (endCurveCapCenter != null)
                    {
                        (isTrack ? TrackPath : ProgressPath)?.AppendCircle((float)endCurveCapCenter.Value.X, (float)endCurveCapCenter.Value.Y, (float)halfWidth);
                    }
                }

                CreateFilledArc(isTrack ? TrackPath : ProgressPath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft, innerArcBottomRight,
                    segmentStartAngle, segmentEndAngle, outerRadius, innerRadius);

            }
        }

        /// <summary>
        /// Provides top left point of arc.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <returns>Top left point of arc.</returns>
        Point GetArcTopLeft(float radius)
        {
            return new PointF(_center.X - radius, _center.Y - radius);
        }

        /// <summary>
        /// Provides bottom right point of arc.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <returns>The bottom right point of arc.</returns>
        Point GetArcBottomRight(float radius)
        {
            return new PointF(_center.X + radius, _center.Y + radius);
        }

        /// <summary>
        /// Get radius from factor value.
        /// </summary>
        /// <param name="thickness">The thickness.</param>
        /// <param name="sizeUnit">The size unit.</param>
        /// <returns>The radius.</returns>
        float GetRadiusFromFactor(double thickness, SizeUnit sizeUnit)
        {
            if (sizeUnit == SizeUnit.Factor)
            {
                thickness = Math.Clamp(thickness, 0, 1) * _radius;
            }

            thickness = thickness < 0 ? 0 : thickness;
            return (float)thickness;
        }

        /// <summary>
        /// To create closed arc.
        /// </summary>
        /// <param name="pathF">The rendering path.</param>
        /// <param name="outerArcTopLeft">The outer arc top left.</param>
        /// <param name="outerArcBottomRight">The outer arc bottom right.</param>
        /// <param name="innerArcTopLeft">The inner arc top left.</param>
        /// <param name="innerArcBottomRight">The inner arc bottom right.</param>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        /// <param name="outerRadius">The outer radius of arc.</param>
        /// <param name="innerRadius">The inner radius of arc.</param>
        void CreateFilledArc(PathF? pathF, PointF outerArcTopLeft, PointF outerArcBottomRight,
          PointF innerArcTopLeft, PointF innerArcBottomRight,
          double startAngle, double endAngle, double outerRadius, double innerRadius)
        {
            if (pathF == null)
            {
                return;
            }

            if (startAngle > endAngle)
            {
                double temp = endAngle;
                endAngle = startAngle;
                startAngle = temp;
            }
#if WINDOWS
            //TODO : Here we moved the path before adding shapes to already closed path.
            //We don't need this move for other platforms. We reported this problem in below link.
            //https://github.com/dotnet/maui/issues/3507
            //Once the problem resolved, we need to remove this part.

            var startAngleVector = Utility.AngleToVector(startAngle);
            var startPoint = new PointF((float)(_center.X + (outerRadius * startAngleVector.X)),
               (float)(_center.Y + (outerRadius * startAngleVector.Y)));
            pathF.MoveTo(startPoint);
#endif
            pathF.AddArc(outerArcTopLeft, outerArcBottomRight, -(float)startAngle, -(float)endAngle, true);
            Point vector = Utility.AngleToVector(endAngle);
            PointF point = new PointF((float)(_center.X + (innerRadius * vector.X)),
               (float)(_center.Y + (innerRadius * vector.Y)));

            // Draw line to inner arc end angle.
            pathF.LineTo(point);

            // Draw inner arc.
            pathF.AddArc(innerArcTopLeft, innerArcBottomRight, -(float)endAngle, -(float)startAngle, false);

            vector = Utility.AngleToVector(startAngle);
            point = new PointF((float)(_center.X + (outerRadius * vector.X)),
               (float)(_center.Y + (outerRadius * vector.Y)));

            // Draw line to outer arc start angle.
            pathF.LineTo(point);

            // Close the path.
            pathF.Close();
        }

        /// <summary>
        /// To validate start and end angle.
        /// </summary>
        void ValidateStartEndAngle()
        {
            var start = double.IsNaN(StartAngle) ? 0 : StartAngle > 360 ? StartAngle % 360 : StartAngle;
            var end = double.IsNaN(EndAngle) ? 0 : EndAngle > 360 ? EndAngle % 360 : EndAngle;
            _actualStartAngle = start;
            end = (end - start) % 360 == 0 ? end - 0.01 : end;
            while (end < start)
            {
                end += 360;
            }

            _actualEndAngle = end;
            _actualSweepAngle = Utility.CalculateSweepAngle(_actualStartAngle, _actualEndAngle);
        }

        /// <summary>
        /// To calculate the radius.
        /// </summary>
        void CalculateRadius()
        {
            if (AvailableSize.IsZero)
            {
                return;
            }

            _radius = Math.Min(AvailableSize.Width, AvailableSize.Height) * 0.5;
            _center = GetCenter(_radius);
            double diff;
            double centerYDiff = Math.Abs((AvailableSize.Height / 2) - _center.Y);
            double centerXDiff = Math.Abs((AvailableSize.Width / 2) - _center.X);
            if (AvailableSize.Width > AvailableSize.Height)
            {
                diff = centerYDiff / 2;
                double radius = (AvailableSize.Height / 2) + diff;

                if ((AvailableSize.Width / 2) < radius)
                {
                    double actualDiff = (AvailableSize.Width / 2) - (AvailableSize.Height / 2);
                    diff = actualDiff * 0.7f;
                }
            }
            else
            {
                diff = centerXDiff / 2;
                double radius = (AvailableSize.Width / 2) + diff;

                if (AvailableSize.Height / 2 < radius)
                {
                    double actualDiff = (AvailableSize.Height / 2) - (AvailableSize.Width / 2);
                    diff = actualDiff * 0.7f;
                }
            }

            _radius += diff;
        }

        /// <summary>
        /// To measure and align content.
        /// </summary>
        void AlignCustomContent()
        {
            if (Content != null)
            {
                Size minSize = Content.Measure(_radius * 2, _radius * 2);
                AbsoluteLayout.SetLayoutBounds(Content, new Rect(_center.X - minSize.Width / 2, _center.Y - minSize.Height / 2, minSize.Width, minSize.Height));
            }
        }

        /// <summary>
        /// To calculate the center point.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <returns>The center point.</returns>
        PointF GetCenter(double radius)
        {
            Point centerPoint;
            centerPoint = new Point(AvailableSize.Width * 0.5d, AvailableSize.Height * 0.5d);
            if (_actualSweepAngle == 359.99 || IsIndeterminate)
            {
                return centerPoint;
            }

            double startAngle = _actualStartAngle;
            double endAngle = _actualEndAngle;
            var arraySize = ((Math.Max(Math.Abs((int)startAngle / 90), Math.Abs((int)endAngle / 90)) + 1) * 2) + 1;
            double[] regions = new double[arraySize];
            int arrayIndex = 0;
            for (int i = -(arraySize / 2); i < (arraySize / 2) + 1; i++)
            {
                regions[arrayIndex] = i * 90;
                arrayIndex++;
            }

            List<int> region = new List<int>();
            if (startAngle < endAngle)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] > startAngle && regions[i] < endAngle)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }
            else
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] < startAngle && regions[i] > endAngle)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }

            return CalculateActualCenter(centerPoint, region, radius);
        }

        /// <summary>
        /// Calculate actual center for progress bar.
        /// </summary>
        /// <param name="centerPoint">The center point.</param>
        /// <param name="region">List of regions.</param>
        /// <param name="radius">The Radius.</param>
        /// <returns>Returns actual center point.</returns>
        PointF CalculateActualCenter(Point centerPoint, List<int> region, double radius)
        {
            var startRadian = 2 * Math.PI * _actualStartAngle / 360;
            var endRadian = 2 * Math.PI * _actualEndAngle / 360;
            Point startPoint = new Point(centerPoint.X + (radius * Math.Cos(startRadian)), centerPoint.Y + (radius * Math.Sin(startRadian)));
            Point endPoint = new Point(centerPoint.X + (radius * Math.Cos(endRadian)), centerPoint.Y + (radius * Math.Sin(endRadian)));
            Point actualCenter = centerPoint;
            switch (region.Count)
            {
                case 0:
                    var longX = Math.Abs(centerPoint.X - startPoint.X) > Math.Abs(centerPoint.X - endPoint.X) ? startPoint.X : endPoint.X;
                    var longY = Math.Abs(centerPoint.Y - startPoint.Y) > Math.Abs(centerPoint.Y - endPoint.Y) ? startPoint.Y : endPoint.Y;
                    var midPoint = new Point(Math.Abs(centerPoint.X + longX) / 2, Math.Abs(centerPoint.Y + longY) / 2);
                    actualCenter.X = centerPoint.X + (centerPoint.X - midPoint.X);
                    actualCenter.Y = centerPoint.Y + (centerPoint.Y - midPoint.Y);
                    break;

                case 1:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0]);
                    actualCenter.X = centerPoint.X + ((centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + ((centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;

                case 2:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0], region[1]);
                    actualCenter.X = centerPoint.X + (midPoint.X == 0 ? 0 : (centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + (midPoint.Y == 0 ? 0 : (centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;

                case 3:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0], region[1], region[2]);
                    actualCenter.X = centerPoint.X + (midPoint.X == 0 ? 0 : (centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + (midPoint.Y == 0 ? 0 : (centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;
            }

            return actualCenter;
        }

        /// <summary>
        /// Calculate region mid point for center calculation.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <param name="region">The region.</param>
        /// <returns>The mid point for center calculation.</returns>
        Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region)
        {
            Point point1 = new Point(), point2 = new Point();
            var maxRadian = 2 * Math.PI * region / 360;
            var maxPoint = new Point(centerPoint.X + (_radius * Math.Cos(maxRadian)), centerPoint.Y + (_radius * Math.Sin(maxRadian)));
            switch (region)
            {
                case 270:
                    point1 = new Point(startPoint.X, maxPoint.Y);
                    point2 = new Point(endPoint.X, centerPoint.Y);
                    break;
                case 0:
                case 360:
                    point1 = new Point(centerPoint.X, endPoint.Y);
                    point2 = new Point(maxPoint.X, startPoint.Y);
                    break;
                case 90:
                    point1 = new Point(endPoint.X, centerPoint.Y);
                    point2 = new Point(startPoint.X, maxPoint.Y);
                    break;
                case 180:
                    point1 = new Point(maxPoint.X, startPoint.Y);
                    point2 = new Point(centerPoint.X, endPoint.Y);
                    break;
            }

            return new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }

        /// <summary>
        /// Calculate region mid point for center calculation.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <param name="region1">The region 1.</param>
        /// <param name="region2">The region 2.</param>
        /// <returns>The mid point for center calculation.</returns>
        Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region1, int region2)
        {
            Point point1, point2;
            var minRadian = 2 * Math.PI * region1 / 360;
            var maxRadian = 2 * Math.PI * region2 / 360;
            var maxPoint = new Point(centerPoint.X + (_radius * Math.Cos(maxRadian)), centerPoint.Y + (_radius * Math.Sin(maxRadian)));
            Point minPoint = new Point(centerPoint.X + (_radius * Math.Cos(minRadian)), centerPoint.Y + (_radius * Math.Sin(minRadian)));
            if ((region1 == 0 && region2 == 90) || (region1 == 180 && region2 == 270))
            {
                point1 = new Point(minPoint.X, maxPoint.Y);
            }
            else
            {
                point1 = new Point(maxPoint.X, minPoint.Y);
            }

            if (region1 == 0 || region1 == 180)
            {
                point2 = new Point(Utility.GetMinMaxValue(startPoint, endPoint, region1), Utility.GetMinMaxValue(startPoint, endPoint, region2));
            }
            else
            {
                point2 = new Point(Utility.GetMinMaxValue(startPoint, endPoint, region2), Utility.GetMinMaxValue(startPoint, endPoint, region1));
            }

            return new Point(Math.Abs(point1.X - point2.X) / 2 >= _radius ? 0 : (point1.X + point2.X) / 2, y: Math.Abs(point1.Y - point2.Y) / 2 >= _radius ? 0 : (point1.Y + point2.Y) / 2);
        }

        /// <summary>
        /// Calculate region mid point for center calculation.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <param name="region1">The region 1.</param>
        /// <param name="region2">The region 2.</param>
        /// <param name="region3">The region 3.</param>
        /// <returns>The mid point for center calculation.</returns>
        Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region1, int region2, int region3)
        {
            float region0Radian = (float)(2 * Math.PI * region1 / 360);
            float region1Radian = (float)(2 * Math.PI * region2 / 360);
            float region2Radian = (float)(2 * Math.PI * region3 / 360);
            Point region0Point = new Point((float)(centerPoint.X + (_radius * Math.Cos(region0Radian))), (float)(centerPoint.Y + (_radius * Math.Sin(region0Radian))));
            Point region1Point = new Point((float)(centerPoint.X + (_radius * Math.Cos(region1Radian))), (float)(centerPoint.Y + (_radius * Math.Sin(region1Radian))));
            Point region2Point = new Point((float)(centerPoint.X + (_radius * Math.Cos(region2Radian))), (float)(centerPoint.Y + (_radius * Math.Sin(region2Radian))));
            Point regionPoint1 = new Point(), regionPoint2 = new Point();
            switch (region3)
            {
                case 0:
                case 360:
                    regionPoint1 = new Point(region0Point.X, region1Point.Y);
                    regionPoint2 = new Point(region2Point.X, Math.Max(startPoint.Y, endPoint.Y));
                    break;
                case 90:
                    regionPoint1 = new Point(Math.Min(startPoint.X, endPoint.X), region0Point.Y);
                    regionPoint2 = new Point(region1Point.X, region2Point.Y);
                    break;
                case 180:
                    regionPoint1 = new Point(region2Point.X, Math.Min(startPoint.Y, endPoint.Y));
                    regionPoint2 = new Point(region0Point.X, region1Point.Y);
                    break;
                case 270:
                    regionPoint1 = new Point(region1Point.X, region2Point.Y);
                    regionPoint2 = new Point(Math.Max(startPoint.X, endPoint.X), region0Point.Y);
                    break;
            }

            return new Point(Math.Abs(regionPoint1.X - regionPoint2.X) / 2 >= _radius ? 0 : (regionPoint1.X + regionPoint2.X) / 2,
                Math.Abs(regionPoint1.Y - regionPoint2.Y) / 2 >= _radius ? 0 : (regionPoint1.Y + regionPoint2.Y) / 2);
        }

        /// <summary>
        /// Converts progress value to its respective angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Angle of the given value.</returns>
        double ValueToAngle(double value)
        {
            return FactorToAngle(ValueToFactor(value));
        }

        /// <summary>
        /// Converts factor value to angle.
        /// </summary>
        /// <param name="factor">Input factor value.</param>
        /// <returns>Returns the angle value.</returns>
        double FactorToAngle(double factor)
        {
            return _actualStartAngle + (factor * _actualSweepAngle);
        }

        /// <summary>
        /// Gets the theme dictionary for the circular progress bar.
        /// </summary>
        /// <returns>
        /// A <see cref="ResourceDictionary"/> containing the theme resources for the circular progress bar.
        /// </returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfCircularProgressBarStyles();
        }

        /// <summary>
        /// Invoked when the control theme changes.
        /// </summary>
        /// <param name="oldTheme">The old theme name.</param>
        /// <param name="newTheme">The new theme name.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
            SetDynamicResource(CircularProgressBarBackgroundProperty, "SfCircularProgressBarBackground");
        }

        /// <summary>
        /// Invoked when the common theme changes.
        /// </summary>
        /// <param name="oldTheme">The old theme name.</param>
        /// <param name="newTheme">The new theme name.</param>
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
        /// Arrange the child elements.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns>Return child element size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (AvailableSize.Width > 0 && AvailableSize.Height > 0)
            {
                if (AvailableSize.Width != _arrangeSize.Width ||
                    AvailableSize.Height != _arrangeSize.Height)
                {
                    _arrangeSize = AvailableSize;
                    UpdateProgressBar();
                }
            }

            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// Measures the size of layout required for child elements.
        /// </summary>
        /// <param name="widthConstraint">The widthConstraint.</param>
        /// <param name="heightConstraint">The heightConstraint.</param>
        /// <returns>Return child element size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            base.MeasureContent(widthConstraint, heightConstraint);
            var width = double.IsPositiveInfinity(widthConstraint) ? 100d : widthConstraint;
            var height = double.IsPositiveInfinity(heightConstraint) ? 100d : heightConstraint;
            if (width > 0 && height > 0)
            {
                GetAvailableSize(width, height);
            }

            return AvailableSize;
        }

        void GetAvailableSize(double width, double height)
        {
            if (HeightRequest > 0)
            {
                height = HeightRequest;
            }
            else
            {
                if (MaximumHeightRequest > 0 && height > MaximumHeightRequest)
                {
                    height = MaximumHeightRequest;
                }

                if (MinimumHeightRequest > 0 && height < MinimumHeightRequest)
                {
                    height = MinimumHeightRequest;
                }
            }

            if (WidthRequest > 0)
            {
                width = WidthRequest;
            }
            else
            {
                if (MaximumWidthRequest > 0 && width > MaximumWidthRequest)
                {
                    width = MaximumWidthRequest;
                }

                if (MinimumWidthRequest > 0 && width < MinimumWidthRequest)
                {
                    width = MinimumWidthRequest;
                }
            }

            AvailableSize = new Size(width, height);
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// To update progress bar and its visual elements.
        /// </summary>
        internal override void UpdateProgressBar()
        {
            CalculateRadius();
            AlignCustomContent();
            base.UpdateProgressBar();
        }

        /// <summary>
        /// To create a track path.
        /// </summary>
        internal override void CreateTrackPath()
        {
            CreatePath(true);
        }

        /// <summary>
        /// To create a progress path.
        /// </summary>
        /// <param name="animationDuration">The animation duration.</param>
        /// <param name="easing">The easing.</param>
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
                CreatePath(false, duration);
            }
        }

        /// <summary>
        /// To animate progress for indeterminate state.
        /// </summary>
        internal override void CreateIndeterminateAnimation()
        {
            if (IndeterminateIndicatorWidthFactor < 1)
            {
                AnimationExtensions.Animate(this,
                    "IndeterminateAnimation",
                    OnIndeterminateAnimationUpdate,
                    0,
                    360,
                    16,
                    (uint)IndeterminateAnimationDuration,
                    IndeterminateAnimationEasing,
                    null,
                    () => IsIndeterminate);
            }
            else
            {
                CreateProgressPath();
            }
        }

        /// <summary>
        /// To draw a circular progress bar elements such as track, progress, etc.
        /// </summary>
        /// <param name="canvas">The drawing canvas.</param>
        internal override void DrawProgressBar(ICanvas canvas)
        {
            base.DrawProgressBar(canvas);
            DrawProgress(canvas);
        }

        /// <summary>
        /// To draw a progress.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal override void DrawProgress(ICanvas canvas)
        {
            canvas.SaveState();
            if (_gradientArcPaths != null && _gradientArcPaths.Count > 0 && !IsIndeterminate && SegmentCount <= 1)
            {
                foreach (var path in _gradientArcPaths)
                {
                    canvas.SetFillPaint(path.FillPaint, path.ArcPath.Bounds);
                    canvas.FillPath(path.ArcPath);
                }
            }
            else if (ProgressPath != null)
            {
                canvas.SetFillPaint(ProgressFill, ProgressPath.Bounds);
                canvas.FillPath(ProgressPath);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// To create gradient fill for circular progress path.
        /// </summary>
        internal override void CreateGradient()
        {
            CreateProgressPath();
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Called when <see cref="StartAngle"/> or <see cref="EndAngle"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnAnglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar)
            {
                circularProgressBar.ValidateStartEndAngle();
                if (!circularProgressBar.AvailableSize.IsZero)
                {
                    circularProgressBar.UpdateProgressBar();
                    circularProgressBar.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="ThicknessUnit"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnThicknessUnitChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar && !circularProgressBar.AvailableSize.IsZero)
            {
                circularProgressBar.UpdateProgressBar();
                circularProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="TrackThickness"/> or <see cref="TrackRadiusFactor"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnTrackPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar && !circularProgressBar.AvailableSize.IsZero)
            {
                circularProgressBar.CreateTrackPath();
                circularProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="ProgressThickness"/> or <see cref="ProgressRadiusFactor"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnProgressPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar && !circularProgressBar.AvailableSize.IsZero)
            {
                circularProgressBar.CreateProgressPath();
                circularProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="TrackCornerStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnTrackCornerStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar && !circularProgressBar.AvailableSize.IsZero)
            {
                circularProgressBar.CreatePath(true);
                circularProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="ProgressCornerStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnProgressCornerStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar && !circularProgressBar.AvailableSize.IsZero)
            {
                circularProgressBar.CreatePath(false);
                circularProgressBar.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="Content"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar)
            {
                if (oldValue is View oldView && circularProgressBar.Children.Contains(oldView))
                {
                    circularProgressBar.Remove(oldView);
                }

                if (newValue is View newView && !circularProgressBar.Children.Contains(newView))
                {
                    circularProgressBar.Add(newView);
                    if (!circularProgressBar.AvailableSize.IsZero)
                    {
                        circularProgressBar.AlignCustomContent();
                    }
                }
            }
        }

        /// <summary>
        /// called when <see cref="CircularProgressBarBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnCircularProgressBarBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCircularProgressBar circularProgressBar)
            {
                circularProgressBar.BackgroundColor = circularProgressBar.CircularProgressBarBackground;
            }
        }

        #endregion
    }
}
