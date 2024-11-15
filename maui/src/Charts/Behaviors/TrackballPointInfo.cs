using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class contains information about the trackball labels.
	/// </summary>
	public class TrackballPointInfo : INotifyPropertyChanged
    {
        #region Fields

        string _text = string.Empty;
        object _dataItem = string.Empty;
        Rect _rect = default(Rect);

        #endregion

        #region Internal Properties

        internal Rect TargetRect { get; set; } = default(Rect);
        internal float X { get; set; }
        internal float Y { get; set; }
        internal double XValue { get; set; }
        internal List<double> YValues { get; set; }
        internal TooltipHelper TooltipHelper { get; set; }
        internal Size GroupLabelSize { get; set; }
        internal SfTooltip? ContentTemplateView { get; set; }
        internal bool HaveTemplateView
        {
            get
            {
                return Series.TrackballLabelTemplate != null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the series associated with the trackball.
        /// </summary>
        public CartesianSeries Series
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the content to be displayed on trackball tooltip
        /// </summary>
        public string Label
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TrackballPointInfo"/> associated business model. 
        /// </summary>
        public object DataItem
        {
            get
            {
                return _dataItem;
            }
            internal set
            {
                if (_dataItem != value)
                {
                    _dataItem = value;
                    OnPropertyChanged(nameof(DataItem));
                }
            }
        }

        /// <summary>
        /// Gets or sets the style for the trackball label.
        /// </summary>
        public ChartLabelStyle? LabelStyle { get; set; }

        /// <summary>
        /// Gets or sets the style for the trackball markers.
        /// </summary>
        public ChartMarkerSettings? MarkerSettings { get; set; }

		#endregion

		#region Event

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		/// <exclude/>
		public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackballPointInfo"/>.
        /// </summary>
        public TrackballPointInfo(CartesianSeries series)
        {
            Series = series;
            YValues = new List<double>();
            TooltipHelper = new TooltipHelper(Drawable) { Duration = int.MaxValue };
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal void ShowTrackballLabel(SfCartesianChart cartesianChart, Rect bounds)
        {
            TooltipHelper.Text = Label;
            TooltipHelper.GroupedLabelSize = GroupLabelSize;
            TooltipHelper.IsGroupedLabel = !GroupLabelSize.IsZero;

            if (!cartesianChart.IsTransposed)
            {
                TooltipHelper.PriorityPosition = TooltipPosition.Right;
                TooltipHelper.PriorityPositionList = new List<TooltipPosition>() { TooltipPosition.Left, TooltipPosition.Bottom, TooltipPosition.Top };
            }
            else
            {
                TooltipHelper.PriorityPosition = TooltipPosition.Top;
                TooltipHelper.PriorityPositionList = new List<TooltipPosition>() { TooltipPosition.Bottom, TooltipPosition.Right, TooltipPosition.Left };

            }

            var plotAreaMargin = cartesianChart.ChartArea.PlotAreaMargin;
            _rect = new Rect(plotAreaMargin.Left, plotAreaMargin.Top, bounds.Width, bounds.Height);
            TooltipHelper.Show(_rect, TargetRect, false);
        }

        internal void SetTargetRect(IMarkerDependent markerDependent)
        {
            if (markerDependent.ShowMarkers)
            {
                var settings = MarkerSettings ?? markerDependent.MarkerSettings;
                var width = settings.Width; var height = settings.Height;
                TargetRect = new Rect(X - width / 2, Y - height / 2, width, height);
            }
            else
            {
                TargetRect = new Rect(X - 1, Y - 1, 2, 2);
            }
        }

        #endregion

        #region Private Methods

        void Drawable()
        {
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #endregion
    }
}