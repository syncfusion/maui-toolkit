namespace Syncfusion.Maui.Toolkit.Charts
{
    internal partial class DataLabelItemView : SfTemplatedView, ICustomAbsoluteView
    {
        #region Properties

        public bool IsRequiredLayoutChange { get; set; } = true;

        public static readonly BindableProperty XPositionProperty =
            BindableProperty.Create(nameof(XPosition), typeof(double), typeof(DataLabelItemView), double.NaN, BindingMode.Default, null, OnPositionChanged);

        public double XPosition
        {
            get { return (double)GetValue(XPositionProperty); }

            set { SetValue(XPositionProperty, value); }
        }

        public static readonly BindableProperty YPositionProperty =
           BindableProperty.Create(nameof(YPosition), typeof(double), typeof(DataLabelItemView), double.NaN, BindingMode.Default, null, OnPositionChanged);

        public double YPosition
        {
            get { return (double)GetValue(YPositionProperty); }
            set { SetValue(YPositionProperty, value); }
        }

        #endregion

        #region Methods

        static void OnPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DataLabelItemView view && view.Parent is Microsoft.Maui.ILayout layout)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    view.IsRequiredLayoutChange = true;
                    layout.InvalidateMeasure();
                });
            }
        }

        #endregion
    }

    internal interface ICustomAbsoluteView
    {
        public double XPosition { get; set; }

        public double YPosition { get; set; }

        public bool IsRequiredLayoutChange { get; set; }
    }
}