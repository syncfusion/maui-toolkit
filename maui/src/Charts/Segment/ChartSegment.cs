using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of a chart, providing properties and methods 
    /// to customize its appearance within the chart.
    /// </summary>
    public abstract class ChartSegment : INotifyPropertyChanged
    {
        #region Fields

        Brush? _fill;
        Brush? _stroke;
        double _strokeWidth = 1;
        bool _isVisible = true;
        DoubleCollection? _strokeDashArray;
        //Todo: Need to change this pixel value for mouse move state.
        const int _touchPixel = 10;
        object? _item;
        float _opacity = 1;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <exclude/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets a brush value to customize the appearance of the segment.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        public Brush? Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                //Todo: Invalidate render if necessary
                // Series?.InvalidateRender();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the series associated with the segment.
        /// </summary>
        public ChartSeries? Series { get; internal set; }

        /// <summary>
        /// Gets or sets a brush value to customize the border appearance of the segment.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        public Brush? Stroke
        {
            get { return _stroke; }

            set
            {
                _stroke = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value to change the thickness of the segment's border.
        /// </summary>
        /// <value>It accepts a double value and its default value is 1.</value>
        /// <remarks>Value must be 1 or greater.</remarks>
        public double StrokeWidth
        {
            get { return _strokeWidth; }

            set
            {
                _strokeWidth = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of the stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        public DoubleCollection? StrokeDashArray
        {
            get { return _strokeDashArray; }

            set
            {
                _strokeDashArray = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value to change the opacity of the segment.
        /// </summary>
        /// <value>It accepts a float value ranging from 0 to 1 and its default value is 1.</value>
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (_opacity == value || value < 0 || value > 1)
                {
                    return;
                }

                _opacity = value;
            }
        }

        /// <summary>
        /// Gets the animation value of the associated series for the segment.
        /// </summary>
        /// <exclude/>
        public float AnimatedValue
        {
            get
            {
                if (Series != null)
                {
                    return Series.AnimationValue;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the index of the specified chart segment within the collection.
        /// </summary>
        /// <value>
        /// The index of the chart segment, or -1 if not found.
        /// </value>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the segment data associated with the business model.
        /// </summary>
        public object? Item
        {
            get { return _item; }

            internal set
            {
                _item = value;
            }
        }

        #endregion

        #region Internal Properties

        internal bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible)
                {
                    return;
                }

                _isVisible = value;
            }
        }

        internal bool Empty { get; set; } = false;

        internal RectF SegmentBounds { get; set; }

        internal bool HasStroke
        {
            get { return StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()); }
        }

        internal bool IsSelected { get; set; } = false;

        internal double DataLabelXPosition { get; set; }

        internal double DataLabelYPosition { get; set; }

        internal List<ChartDataLabel> DataLabels { get; set; }

        internal string? LabelContent { get; set; }

        internal List<float> XPoints { get; set; }

        internal List<float> YPoints { get; set; }

        internal bool InVisibleRange { get; set; } = true;

        internal PointF LabelPositionPoint { get; set; }

        internal bool IsEmpty { get; set; }

        internal SeriesView? SeriesView { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSegment"/> class.
        /// </summary>
        public ChartSegment()
        {
            XPoints = new List<float>();
            YPoints = new List<float>();
            DataLabels = new List<ChartDataLabel>();
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Calculates the required values for rendering the chart segment.
        /// </summary>
        protected internal virtual void OnLayout()
        {
        }

        /// <summary>
        /// Draws a segment for the specified chart series or charts.
        /// </summary>
        protected internal virtual void Draw(ICanvas canvas)
        {

        }

        internal virtual void SetData(double[] values)
        {
        }

        internal virtual void SetData(IList xValues, IList yValues)
        {
        }

        internal virtual void SetData(double[] values, bool isTrue, bool isFalse)
        {
        }

        internal virtual void SetData(IList xValues, IList yValues, IList startControlPointsX, IList startControlPointsY, IList endControlPointsX, IList endControlPointsY)
        {
        }

        internal virtual bool HitTest(float valueX, float valueY)
        {
            return GetDataPointIndex(valueX, valueY) >= 0;
        }

        internal virtual int GetDataPointIndex(float valueX, float valueY)
        {
            return -1;
        }

        internal static bool IsRectContains(float xPoint, float yPoint, float valueX, float valueY, float strokeWidth)
        {
			float depth = (strokeWidth < _touchPixel) ? _touchPixel : strokeWidth;
            float x1 = xPoint - depth;
            float y1 = yPoint - depth;
            float x2 = xPoint + depth;
            float y2 = yPoint + depth;
            return x1 < valueX && valueX < x2 && y1 < valueY && valueY < y2;
        }

        internal static bool IsRectContains(float x1Point, float y1Point, float x2Point, float y2Point, float valueX, float valueY, float StrokeWidth)
        {
            float depth = (StrokeWidth < _touchPixel) ? _touchPixel : StrokeWidth;
            float x1 = x1Point - depth;
            float y1 = y1Point - depth;
            float x2 = x2Point + depth;
            float y2 = y2Point + depth;
            return x1 < valueX && valueX < x2 && y1 < valueY && valueY < y2;
        }

        internal virtual void OnDataLabelLayout()
        {

        }

        internal virtual void UpdateDataLabels()
        {

        }

        #endregion

        #region Private Methods

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #endregion
    }

}
