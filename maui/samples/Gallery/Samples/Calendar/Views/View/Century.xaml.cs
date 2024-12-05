namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class Century : SampleView
    {
        /// <summary>
        /// Background color for border control. Border control will not shown on mobile platforms.
        /// </summary>
        public Color BGColor { get; set; }

        /// <summary>
        /// Stroke color for border control. Border control will not shown on mobile platforms.
        /// </summary>
        public Color StrokeColor { get; set; }

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Initializes a new instance of the <see cref="Century" /> class.
        /// </summary>
        public Century()
        {
            InitializeComponent();
#if MACCATALYST || (!ANDROID && !IOS)
            Background = _isLightTheme ? Brush.White : (Brush)Color.FromRgba("#1C1B1F");
            Margin = new Thickness(-4, -4, -6, -6);
            BGColor = _isLightTheme ? Color.FromArgb("#F7F2FB") : Color.FromArgb("#25232A");
            StrokeColor = Colors.Transparent;
#else
            BGColor = Colors.Transparent;
            StrokeColor = Colors.Transparent;
#endif

            BindingContext = this;
            century.SelectedDate = DateTime.Now;
        }
    }
}