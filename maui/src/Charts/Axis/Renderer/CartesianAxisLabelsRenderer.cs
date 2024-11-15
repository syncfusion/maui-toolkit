namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class CartesianAxisLabelsRenderer : ILayoutCalculator
    {
        #region Fields
        SizeF _desiredSize;

        double _left, _top;

        ChartAxis _chartAxis;

        internal AxisLabelLayout? LabelLayout
        {
            get;
            set;
        }

        bool ILayoutCalculator.IsVisible { get; set; } = true;

        #endregion

        #region Constructor
        public CartesianAxisLabelsRenderer(ChartAxis axis)
        {
            _chartAxis = axis;
        }
        #endregion

        #region public Methods
        public void OnDraw(ICanvas drawing, Size finalSize)
        {
            LabelLayout?.OnDraw(drawing, finalSize);
        }

        public double GetLeft()
        {
            return _left;
        }

        public void SetLeft(double left)
        {
            _left = left;
        }

        public double GetTop()
        {
            return _top;
        }

        public void SetTop(double top)
        {
            _top = top;
        }

        public Size GetDesiredSize()
        {
            return _desiredSize;
        }

        public Size Measure(Size availableSize)
        {
            LabelLayout = AxisLabelLayout.CreateAxisLayout(_chartAxis);
            _desiredSize = LabelLayout.Measure(availableSize);
            return _desiredSize;
        }
        #endregion
    }
}
