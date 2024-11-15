namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.ZoomEnd"/> event.
    /// </summary>
    public class ChartZoomEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets the values of the X and Y axes.
        /// </summary>
        public ChartAxis Axis { get; internal set; }

        /// <summary>
        /// Gets the current zoom factor value for the chart axis.
        /// </summary>
        public double CurrentZoomFactor { get; internal set; }

        /// <summary>
        /// Gets the current zoom position value for the chart axis.
        /// </summary>
        public double CurrentZoomPosition { get; internal set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartZoomEventArgs"/> class.
        /// </summary>
        public ChartZoomEventArgs(ChartAxis axis, double currentZoomFactor, double currentZoomPosition)
        {
            Axis = axis;
            CurrentZoomFactor = currentZoomFactor;
            CurrentZoomPosition = currentZoomPosition;
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.ZoomStart"/> event.
    /// </summary>
    public class ChartZoomStartEventArgs : ChartZoomEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Get or set indicates whether to continue the zooming action.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartZoomStartEventArgs"/> class.
        /// </summary>
        public ChartZoomStartEventArgs(ChartAxis axis, double currentZoomFactor, double currentZoomPosition) : base(axis, currentZoomFactor, currentZoomPosition)
        {
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.ZoomDelta"/> event.
    /// </summary>
    public class ChartZoomDeltaEventArgs : ChartZoomEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets the previous zoom factor values when zooming the chart.
        /// </summary>
        public double PreviousZoomFactor { get; internal set; }

        /// <summary>
        /// Gets the previous zoom position values when zooming the chart.
        /// </summary>
        public double PreviousZoomPosition { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the ongoing zooming action in the chart.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartZoomDeltaEventArgs"/> class.
        /// </summary>
        public ChartZoomDeltaEventArgs(ChartAxis axis, double currentZoomFactor, double currentZoomPosition, double previousZoomFactor, double previousZoomPosition) : base(axis, currentZoomFactor, currentZoomPosition)
        {
            PreviousZoomFactor = previousZoomFactor;
            PreviousZoomPosition = previousZoomPosition;
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.SelectionZoomStart"/> and <see cref="SfCartesianChart.SelectionZoomEnd"/> events.
    /// </summary>
    public class ChartSelectionZoomEventArgs : EventArgs
    {
        #region Public Property

        /// <summary>
        /// Gets the values of the zoom rectangle during selection zooming.
        /// </summary>
        public Rect ZoomRect { get; internal set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionZoomEventArgs"/> class.
        /// </summary>
        public ChartSelectionZoomEventArgs(Rect zoomRect)
        {
            ZoomRect = zoomRect;
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.SelectionZoomDelta"/> event.
    /// </summary>
    public class ChartSelectionZoomDeltaEventArgs : ChartSelectionZoomEventArgs
    {
        #region Public Property

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the ongoing selection zooming action in the chart.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionZoomDeltaEventArgs"/> class.
        /// </summary>
        public ChartSelectionZoomDeltaEventArgs(Rect zoomRect) : base(zoomRect)
        {
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.Scroll"/> event.
    /// </summary>
    public class ChartScrollEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the values of the X and Y axes when panning the chart.
        /// </summary>
        public ChartAxis Axis { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the ongoing panning action in the chart.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the zoom position values when panning the chart.
        /// </summary>
        public double ZoomPosition { get; internal set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartScrollEventArgs"/> class.
        /// </summary>
        public ChartScrollEventArgs(ChartAxis axis, double zoomPosition)
        {
            Axis = axis;
            ZoomPosition = zoomPosition;
        }

        #endregion
    }

    /// <summary>
    /// This class provides data for the <see cref="SfCartesianChart.ResetZoom"/> event.
    /// </summary>
    public class ChartResetZoomEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets the values of the X and Y axes when the chart is reset.
        /// </summary>
        public ChartAxis Axis { get; internal set; }

        /// <summary>
        /// Gets the previous zoom factor values when the chart is reset.
        /// </summary>
        public double PreviousZoomFactor { get; internal set; }

        /// <summary>
        /// Gets the previous zoom position values when the chart is reset.
        /// </summary>
        public double PreviousZoomPosition { get; internal set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartResetZoomEventArgs"/> class.
        /// </summary>
        public ChartResetZoomEventArgs(ChartAxis axis, double previousZoomFactor, double previousZoomPosition)
        {
            Axis = axis;
            PreviousZoomFactor = previousZoomFactor;
            PreviousZoomPosition = previousZoomPosition;
        }

        #endregion
    }
}