
using Syncfusion.Maui.ControlsGallery.CustomView;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class SfNeumorphismColumnSeries : ColumnSeries
    {
        public SfNeumorphismColumnSeries()
        {
            Drawable = new SfNeumorphismDrawer();
        }

        public SfNeumorphismDrawer Drawable
        {
            get { return (SfNeumorphismDrawer)GetValue(DrawableProperty); }
            set { SetValue(DrawableProperty, value); }
        }

        public static readonly BindableProperty DrawableProperty =
            BindableProperty.Create(nameof(Drawable), typeof(SfNeumorphismDrawer), typeof(SfNeumorphismColumnSeries), defaultValue: null, propertyChanged: OnDrawablePropertyChanged);


        protected override ChartSegment CreateSegment()
        {
            return new SfNeumorphismColumnSegment(Drawable);
        }

        protected static void OnDrawablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

    }

    public class SfNeumorphismColumnSegment : ColumnSegment
    {

        SfNeumorphismDrawer Drawable;

        public SfNeumorphismColumnSegment(SfNeumorphismDrawer drawable)
        {
            Drawable = drawable;
        }

        protected override void Draw(ICanvas canvas)
        {
            if (Series is ColumnSeries series && series.ActualYAxis is NumericalAxis yAxis)
            {
                var top = yAxis.ValueToPoint(Convert.ToDouble(yAxis.Maximum ?? double.NaN));

                var trackRect = new RectF() { Left = Left, Top = top, Right = Right, Bottom = Bottom };

                Drawable.Draw(canvas, trackRect);
            }
        }

    }

}
