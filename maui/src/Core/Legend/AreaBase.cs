using Microsoft.Maui.Layouts;
#if ANDROID
using Android.Content;
using Android.Provider;
using Android.OS;
#endif

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Represents a base class for areas, inheriting from <see cref="AbsoluteLayout"/> and implementing the <see cref="IArea"/> interface.
	/// </summary>
	internal abstract class AreaBase : AbsoluteLayout, IArea
    {
        #region Fields

        const double _maxSize = 8388607.5;
        readonly CoreScheduler _coreScheduler;
        Rect _areaBounds;
        bool _powerMode;
        bool _isAnimationEnabled;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaBase"/> class.
        /// </summary>
        public AreaBase()
        {
            _powerMode = IsPowerSaverMode();
            _isAnimationEnabled = IsAnimationEnabled();

            if (Application.Current != null)
            {
                _coreScheduler = CoreScheduler.CreateScheduler((!_powerMode && _isAnimationEnabled) ? CoreSchedulerType.Frame : CoreSchedulerType.Main);
            }
            else
            {
                _coreScheduler = CoreScheduler.CreateScheduler(CoreSchedulerType.Main);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the area bounds
        /// </summary>
        public Rect AreaBounds
        {
            get
            {
                return _areaBounds;
            }
            set
            {
                if (_areaBounds != value)
                {
                    if (PlotArea != null)
                    {
                        PlotArea.PlotAreaBounds = value;
                    }

                    _areaBounds = value;
                    NeedsRelayout = true;
                }
            }
        }

		/// <summary>
		/// Gets the plot area
		/// </summary>
		public abstract IPlotArea PlotArea { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the layout needs to be updated.
		/// </summary>
		public bool NeedsRelayout { get; set; } = false;

        IPlotArea IArea.PlotArea => PlotArea;

		#endregion

		#region Methods 

		#region Protected Methods 
		/// <summary>
		/// Updates the core area of the layout. This method can be overridden to provide custom update logic.
		/// </summary>
		protected virtual void UpdateAreaCore()
        {

        }

		/// <summary>
		/// Measures the size required for the layout, given the width and height constraints.
		/// </summary>
		/// <param name="widthConstraint">The available width for the layout.</param>
		/// <param name="heightConstraint">The available height for the layout.</param>
		/// <returns>The size required for the layout.</returns>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            var size = this.ComputeDesiredSize(widthConstraint, heightConstraint);
            // Size.Height is 1.33 for windows platform, so checked condition with less than 1.
            bool isHeightNotContains = double.IsPositiveInfinity(heightConstraint) && Math.Round(size.Height) <= 1;
            bool isWidthNotContains = double.IsPositiveInfinity(widthConstraint) && Math.Round(size.Width) <= 1;

            if (isHeightNotContains || isWidthNotContains)
                DesiredSize = new Size(isWidthNotContains ? 300 : size.Width, isHeightNotContains ? 300 : size.Height);
            else
                DesiredSize = size;

            return DesiredSize;
        }

		/// <summary>
		/// Arranges the content of the layout within the specified bounds.
		/// </summary>
		/// <param name="bounds">The rectangle that defines the bounds of the layout.</param>
		/// <returns>The final size of the arranged content.</returns>
		protected override Size ArrangeOverride(Rect bounds)
        {
            if (!AreaBounds.Equals(bounds) && bounds.Width != _maxSize && bounds.Height != _maxSize)
            {
                AreaBounds = bounds;

                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    ScheduleUpdateArea();
                }
            }

            return base.ArrangeOverride(bounds);
        }

		#endregion

		#region Internal Methods

		/// <summary>
		/// Schedules an update for the area, ensuring that any necessary changes are applied.
		/// </summary>
		public void ScheduleUpdateArea()
        {
            if (NeedsRelayout && !_areaBounds.IsEmpty)
                _coreScheduler.ScheduleCallback(UpdateArea);
        }

        #endregion

        #region Private Method

        void UpdateArea()
        {
            PlotArea.UpdateLegendItems();
            UpdateAreaCore();
            NeedsRelayout = false;
        }

        bool IsPowerSaverMode()
        {
            bool powerSaveOn = false;
#if ANDROID
            var handler = Application.Current?.Handler;
            var powerManager = handler?.MauiContext?.Context?.ApplicationContext?.GetSystemService(Context.PowerService) as PowerManager;
            powerSaveOn = powerManager?.IsPowerSaveMode ?? false;
#endif
            return powerSaveOn;
        }

        bool IsAnimationEnabled()
        {
            bool isAnimationOn = true;
#if ANDROID
            try
            {
                var handler = Application.Current?.Handler;
                float scale = Settings.Global.GetFloat(handler?.MauiContext?.Context?.ApplicationContext?.ContentResolver, Settings.Global.AnimatorDurationScale);
                isAnimationOn = scale != 0.0f;
            }
            catch
            {
                isAnimationOn = false;
            }
#endif
            return isAnimationOn;
        }

        #endregion

        #endregion
    }
}
