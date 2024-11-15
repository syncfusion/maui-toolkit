namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface ILayoutCalculator
    {
        #region Properties

        bool IsVisible { get; set; }

        #endregion

        #region Methods

        void OnDraw(ICanvas canvas, Size finalSize);

        double GetLeft();

        void SetLeft(double left);

        double GetTop();

        void SetTop(double top);

        Size GetDesiredSize();

        Size Measure(Size availableSize);

        #endregion
    }

    internal interface IAxisLayout
    {
        #region Methods

        Size Measure(Size availableSize);

        void OnDraw(ICanvas canvas);

        #endregion
    }
}
