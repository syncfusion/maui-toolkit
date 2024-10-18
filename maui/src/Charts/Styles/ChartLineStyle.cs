using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents the chart line style class that can be used to customize the axis line and grid lines.
    /// </summary>
    /// <remarks>
    /// <para>It provides more options to customize the chart lines.</para>
    /// 
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="StrokeWidth"/> property. </para>
    /// <para> <b>StrokeDashArray - </b> To customize the line with dashes and gaps, refer to this <see cref="StrokeDashArray"/> property. </para>
    /// 
    /// </remarks>
    public class ChartLineStyle : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Stroke"/> bindable property determines the stroke color of the chart lines.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke), typeof(Brush),
            typeof(ChartLineStyle),
            null,
            BindingMode.Default,
            null,
            OnStrokeColorChanged,
            defaultValueCreator: StrokeDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width of the chart lines.
        /// </remarks>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(double),
            typeof(ChartLineStyle),
            1d,
            BindingMode.Default,
            null,
            OnStrokeWidthChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="StrokeDashArray"/> bindable property determines the dash pattern of the chart lines.
        /// </remarks>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
            nameof(StrokeDashArray),
            typeof(DoubleCollection),
            typeof(ChartLineStyle),
            null,
            BindingMode.Default,
            null,
            OnStrokeDashArrayChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLineStyle"/>.
        /// </summary>
        public ChartLineStyle()
        {
            ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value to customize the stroke color of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the width of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the default value is 1.</value>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="DoubleCollection"/> values.</value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Interface Implementation

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal bool CanDraw()
        {
            return StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor());
        }

        internal virtual Brush GetDefaultStrokeColor()
        {
            return new SolidColorBrush(Color.FromArgb("#CAC4D0"));
        }

        #endregion

        #region Private Methods

        static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        static void OnStrokeDashArrayChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        static object StrokeDefaultValueCreator(BindableObject bindable)
        {
            return ((ChartLineStyle)bindable).UpdateStrokeValue();
        }

        internal virtual object UpdateStrokeValue()
        {
            return new SolidColorBrush(Color.FromArgb("#CAC4D0"));
        }

        #endregion

        #endregion
    }
}
