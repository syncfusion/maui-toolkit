using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Enables a crosshair overlay for a Cartesian chart. Draws vertical and horizontal guide lines
    /// at the user interaction position and shows axis labels (trackball-style) for precise value inspection.
    /// </summary>
    /// <remarks>
    /// <para>The crosshair overlay displays information while long press or mouse hovering on the chart plot area.</para>
    /// <para>To display the crosshair lines on the chart, you need to create instance the <see cref="ChartCrosshairBehavior"/> property and set it to the charts <see cref="SfCartesianChart.CrosshairBehavior"/> property.</para>
    /// <para>To display the crosshair labels on the chart, you need to set the <see cref="ChartAxis.ShowTrackballLabel"/> property as <b>true</b> in chart axis.</para>
    /// <para>Crosshair behavior provides the following options to customize the appearance of the crosshair line and label:</para>
    /// <para> <b>Label Customization - </b> To customize the appearance of the crosshair label, refer to the <see cref="ChartLabelStyle.Background"/>, <see cref="ChartLabelStyle.FontAttributes"/>, <see cref="ChartLabelStyle.FontFamily"/>, <see cref="ChartLabelStyle.FontSize"/>, <see cref="ChartLabelStyle.LabelFormat"/>, <see cref="ChartLabelStyle.Margin"/>, <see cref="ChartLabelStyle.StrokeWidth"/>, <see cref="ChartLabelStyle.Stroke"/> and <see cref="ChartLabelStyle.TextColor"/> properties.</para>
    /// <para> <b>Vertical Line Customization - </b> To customize the appearance of the vertical crosshair line, refer to the <see cref="ChartLineStyle.StrokeWidth"/>, <see cref="ChartLineStyle.Stroke"/> and <see cref="ChartLineStyle.StrokeDashArray"/> properties.</para>
    /// <para> <b>Horizontal Line Customization - </b> To customize the appearance of the horizontal crosshair line, refer to the <see cref="ChartLineStyle.StrokeWidth"/>, <see cref="ChartLineStyle.Stroke"/> and <see cref="ChartLineStyle.StrokeDashArray"/> properties.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[ 
    /// <chart:SfCartesianChart>
    ///
    ///     <chart:SfCartesianChart.XAxes>
    ///         <chart:CategoryAxis ShowTrackballLabel = "True" >
    ///            <chart:CategoryAxis.TrackballLabelStyle>
    ///               <chart:ChartLabelStyle Background = "Black" TextColor="White"/>
    ///            </chart:CategoryAxis.TrackballLabelStyle>
    ///    	    </chart:CategoryAxis>
    ///     </chart:SfCartesianChart.XAxes>
    ///   
    ///     <chart:SfCartesianChart.CrosshairBehavior>
    ///       	 <chart:ChartCrosshairBehavior>
    ///            <chart:ChartCrosshairBehavior.VerticalLineStyle>
    ///               <chart:ChartLineStyle Stroke = "Red" StrokeWidth="2"/>
    ///            </chart:ChartCrosshairBehavior.VerticalLineStyle>
    ///           
    ///            <chart:ChartCrosshairBehavior.HorizontalLineStyle>
    ///               <chart:ChartLineStyle Stroke = "Green" StrokeWidth="2"/>
    ///            </chart:ChartCrosshairBehavior.HorizontalLineStyle>
    ///          </chart:ChartCrosshairBehavior>
    ///      </chart:SfCartesianChart.CrosshairBehavior>
    ///   
    ///     <!--omitted for brevity-->
    ///   
    ///	</chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// // omitted for brevity 
    /// 
    /// var trackballLabelStyle = new ChartLabelStyle()
    /// { 
    ///     Background = new SolidColorBrush(Colors.Black),
    ///     TextColor = Colors.White,
    /// };
    /// 
    /// var verticalLineStyle = new ChartLineStyle()
    /// {
    ///     Stroke = new SolidColorBrush(Colors.Red), StrokeWidth = 2,
    /// };
    ///    
    /// var horizontalLineStyle = new ChartLineStyle()
    /// {
    ///     Stroke = new SolidColorBrush(Colors.Green), StrokeWidth = 2,
    /// };
    /// 
    /// CategoryAxis xaxis = new CategoryAxis()
    /// {
    ///    	ShowTrackballLabel = true,
    ///    	TrackballLabelStyle = trackballLabelStyle,
    /// };
    /// 
    /// chart.XAxes.Add(xaxis);
    /// 
    /// var crosshairBehavior = new ChartCrosshairBehavior() 
    /// {
    ///     VerticalLineStyle = verticalLineStyle,
    ///     HorizontalLineStyle = horizontalLineStyle
    /// };
    /// 
    /// chart.CrosshairBehavior = crosshairBehavior;
    ///
    /// ]]></code>
    /// ***
    /// </example>
    public class ChartCrosshairBehavior : ChartBehavior
    {
        #region Fields

        float _verticalCrosshairLineX = float.NaN;
        float _horizontalCrosshairLineY = float.NaN;
        double _yValue = double.NaN, _xValue = double.NaN;
        const int dimension = 2;
        ChartLineStyle verticalStyle;
        ChartLineStyle horizontalStyle;
        readonly List<TrackballAxisInfo> crosshairAxisTrackballInfo = new List<TrackballAxisInfo>();
        TooltipPosition tooltipPosition = TooltipPosition.Auto;

        #endregion

        #region Properties

        internal SfCartesianChart? CartesianChart { get; set; }
        internal TooltipHelper TooltipHelper { get; set; }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="VerticalLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalLineStyleProperty = BindableProperty.Create(
            nameof(VerticalLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartCrosshairBehavior),
            propertyChanged: OnVerticalLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalLineStyleProperty = BindableProperty.Create(
            nameof(HorizontalLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartCrosshairBehavior),
            propertyChanged: OnHorizontalLineStyleChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the style used to draw the vertical crosshair line.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///     
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis ShowTrackballLabel = "True"/>
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.CrosshairBehavior>
        ///        
        ///         <chart:ChartCrosshairBehavior>         
        ///            <chart:ChartCrosshairBehavior.VerticalLineStyle>
        ///               <chart:ChartLineStyle Stroke = "Red" StrokeWidth="2"/>
        ///            </chart:ChartCrosshairBehavior.VerticalLineStyle>           
        ///         </chart:ChartCrosshairBehavior>
        ///           
        ///      </chart:SfCartesianChart.CrosshairBehavior>
        ///        
        ///    <!--omitted for brevity-->
        ///           
        /// </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// // omitted for brevity 
        /// 
        /// var verticalLineStyle = new ChartLineStyle()
        /// {
        ///     Stroke = new SolidColorBrush(Colors.Red), StrokeWidth = 2,
        /// };
        ///    
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowTrackballLabel = true,
        /// };
        /// 
        /// chart.XAxes.Add(xaxis);
        /// 
        /// var crosshairBehavior = new ChartCrosshairBehavior()
        /// {
        ///     VerticalLineStyle = verticalLineStyle,
        /// };
        /// 
        /// chart.CrosshairBehavior = crosshairBehavior;
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle VerticalLineStyle
        {
            get { return (ChartLineStyle)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style used to draw the horizontal crosshair line.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///     
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis ShowTrackballLabel = "True"/>
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.CrosshairBehavior>
        ///        
        ///        <chart:ChartCrosshairBehavior>
        ///           <chart:ChartCrosshairBehavior.HorizontalLineStyle>
        ///              <chart:ChartLineStyle Stroke = "Green" StrokeWidth="2"/>
        ///           </chart:ChartCrosshairBehavior.HorizontalLineStyle>       
        ///        </chart:ChartCrosshairBehavior>
        ///           
        ///      </chart:SfCartesianChart.CrosshairBehavior>
        ///        
        ///  <!--omitted for brevity-->
        ///           
        /// </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// // omitted for brevity
        /// 
        /// var horizontalLineStyle = new ChartLineStyle()
        /// {
        ///     Stroke = new SolidColorBrush(Colors.Green), StrokeWidth = 2,
        /// };
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///     ShowTrackballLabel = true,
        /// };
        /// 
        /// chart.XAxes.Add(xaxis);
        /// 
        /// var crosshairBehavior = new ChartCrosshairBehavior()
        /// {
        ///     HorizontalLineStyle = horizontalLineStyle
        /// };
        /// 
        /// chart.CrosshairBehavior = crosshairBehavior;
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle HorizontalLineStyle
        {
            get { return (ChartLineStyle)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCrosshairBehavior"/> class.
        /// </summary>
        public ChartCrosshairBehavior()
        {
            // use distinct style instances to avoid accidental shared-state changes
            verticalStyle = new ChartLineStyle() { Stroke = Color.FromArgb("#49454F"), StrokeWidth = 1 };
            horizontalStyle = new ChartLineStyle() { Stroke = Color.FromArgb("#49454F"), StrokeWidth = 1 };
            VerticalLineStyle = verticalStyle;
            HorizontalLineStyle = horizontalStyle;
            TooltipHelper = new TooltipHelper(Drawable) { Duration = int.MaxValue };
        }

        #endregion

        #region Methods

        #region Call back Methods

        static void OnVerticalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartCrosshairBehavior behavior)
            {
                if (oldValue is ChartLineStyle oldStyle)
                {
                    SetInheritedBindingContext(oldStyle, null);
                }

                if (newValue is ChartLineStyle newStyle)
                {
                    behavior.verticalStyle = newStyle;
                }
                else
                {
                    behavior.verticalStyle = new ChartLineStyle() { Stroke = Color.FromArgb("#49454F"), StrokeWidth = 1 };
                }

                var effectiveStyle = newValue as ChartLineStyle ?? behavior.verticalStyle;
                SetInheritedBindingContext(effectiveStyle, behavior.BindingContext);
                ChartBase.SetParent((Element?)oldValue, (Element?)effectiveStyle, behavior.Parent);
            }
        }

        static void OnHorizontalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartCrosshairBehavior behavior)
            {
                if (oldValue is ChartLineStyle oldStyle)
                {
                    SetInheritedBindingContext(oldStyle, null);
                }

                if (newValue is ChartLineStyle newStyle)
                {
                    behavior.horizontalStyle = newStyle;
                }
                else
                {
                    behavior.horizontalStyle = new ChartLineStyle() { Stroke = Color.FromArgb("#49454F"), StrokeWidth = 1 };
                }

                var effectiveStyle = newValue as ChartLineStyle ?? behavior.horizontalStyle;
                SetInheritedBindingContext(effectiveStyle, behavior.BindingContext);
                ChartBase.SetParent((Element?)oldValue, (Element?)effectiveStyle, behavior.Parent);
            }
        }

        #endregion

        #region Override Methods

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            SetInheritedBindingContext(VerticalLineStyle ?? verticalStyle, BindingContext);
            SetInheritedBindingContext(HorizontalLineStyle ?? horizontalStyle, BindingContext);
        }

        /// <inheritdoc/>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (VerticalLineStyle != null)
            {
                VerticalLineStyle.Parent = Parent;
            }

            if (HorizontalLineStyle != null)
            {
                HorizontalLineStyle.Parent = Parent;
            }
        }

        /// <inheritdoc/>
        protected internal override void OnTouchMove(ChartBase chart, float pointX, float pointY)
        {
            if (chart is SfCartesianChart cartesianChart && cartesianChart.SeriesBounds.Contains(pointX, pointY))
            {
                Show(pointX, pointY);
            }
            else
            {
                Hide();
            }
        }

        /// <inheritdoc/>
        protected internal override void OnTouchUp(ChartBase chart, float pointX, float pointY)
        {
            Hide();
        }

        /// <inheritdoc/>
        internal override void OnTouchExit()
        {
            Hide();
        }

        /// <inheritdoc/>
        internal override void OnTouchCancel(float pointX, float pointY)
        {
            Hide();
        }


        internal override void OnLongPressActivation(IChart chart, float x, float y, GestureStatus status)
        {
            if (chart is SfCartesianChart cartesianChart && cartesianChart.SeriesBounds.Contains(x, y) && status == GestureStatus.Started)
            {
                Show(x, y);
            }
        }

        #endregion

        #region Internal Methods

        internal void Show(float pointX, float pointY)
        {
            if (CartesianChart == null || CartesianChart is not IChart chart || CartesianChart._chartArea is not CartesianChartArea area)
            {
                return;
            }

            if (CartesianChart.SeriesBounds.Contains(pointX, pointY))
            {
                _verticalCrosshairLineX = pointX;
                _horizontalCrosshairLineY = pointY;

                var axisLayout = area._axisLayout;
                var xAxes = axisLayout.HorizontalAxes;
                var yAxes = axisLayout.VerticalAxes;
                crosshairAxisTrackballInfo.Clear();
                GenerateAxisTrackballInfos(new Point(pointX, pointY), ref tooltipPosition, xAxes);
                GenerateAxisTrackballInfos(new Point(pointX, pointY), ref tooltipPosition, yAxes);
                Invalidate();
            }
        }

        internal void Hide()
        {
            _verticalCrosshairLineX = float.NaN;
            _horizontalCrosshairLineY = float.NaN;
            crosshairAxisTrackballInfo.Clear();
            Invalidate();
        }

        internal void DrawElements(ICanvas canvas, Rect dirtyRect)
        {
            if (CartesianChart == null)
            {
                return;
            }

            DrawCrosshairLines(canvas);
            DrawCrosshairLabels(canvas);
        }

        #endregion

        #region Private Methods

        void DrawCrosshairLines(ICanvas canvas)
        {
            if (CartesianChart == null) return;
            if (float.IsNaN(_verticalCrosshairLineX) && float.IsNaN(_horizontalCrosshairLineY)) return;

            var chartArea = CartesianChart._chartArea;
            var plotBounds = chartArea.PlotArea.PlotAreaBounds;
            var seriesBounds = chartArea.ActualSeriesClipRect;
            var titleHeight = (CartesianChart as IChart).TitleHeight;

            var verticalLineStyle = verticalStyle;
            if (verticalLineStyle != null && !float.IsNaN(_verticalCrosshairLineX))
            {
                canvas.SaveState();
                if (verticalLineStyle.StrokeDashArray != null)
                {
                    canvas.StrokeDashPattern = verticalLineStyle.StrokeDashArray.ToFloatArray();
                }

                canvas.StrokeColor = verticalLineStyle.Stroke.ToColor();
                canvas.StrokeSize = (float)verticalLineStyle.StrokeWidth;

                var x = _verticalCrosshairLineX - (float)plotBounds.Left;
                canvas.DrawLine(x, seriesBounds.Top, x, seriesBounds.Bottom);
                canvas.RestoreState();
            }

            var horizontalLineStyle = horizontalStyle;
            if (horizontalLineStyle != null && !float.IsNaN(_horizontalCrosshairLineY))
            {
                canvas.SaveState();
                if (horizontalLineStyle.StrokeDashArray != null)
                {
                    canvas.StrokeDashPattern = horizontalLineStyle.StrokeDashArray.ToFloatArray();
                }

                canvas.StrokeColor = horizontalLineStyle.Stroke.ToColor();
                canvas.StrokeSize = (float)horizontalLineStyle.StrokeWidth;

                var y = _horizontalCrosshairLineY - (float)plotBounds.Top - (float)titleHeight;
                canvas.DrawLine(seriesBounds.Left, y, seriesBounds.Left + seriesBounds.Width, y);
                canvas.RestoreState();
            }
        }

        void GenerateAxisTrackballInfos(PointF startPoint, ref TooltipPosition tooltipPosition, ObservableCollection<ChartAxis> axes)
        {
            if (CartesianChart == null) return;

            var clipRect = CartesianChart._chartArea.ActualSeriesClipRect;

            foreach (ChartAxis axis in axes)
            {
                if (!axis.ShowTrackballLabel)
                    continue;

                var actualArrangeRect = axis.ArrangeRect;
                var isOpposed = axis.IsOpposed();

                PointF sp = startPoint;

                if (axis.IsVertical)
                {
                    sp = new Point(isOpposed ? actualArrangeRect.X : actualArrangeRect.X + actualArrangeRect.Width, startPoint.Y - (float)CartesianChart._chartArea.PlotArea.PlotAreaBounds.Top);
                    tooltipPosition = isOpposed ? TooltipPosition.Right : TooltipPosition.Left;
                }
                else
                {
                    sp = new Point(startPoint.X, isOpposed ? actualArrangeRect.Y + actualArrangeRect.Height : actualArrangeRect.Y);
                    tooltipPosition = isOpposed ? TooltipPosition.Top : TooltipPosition.Bottom;
                }

                string labelFormat = "##.##";
                if (axis.TrackballLabelStyle != null && !string.IsNullOrEmpty(axis.TrackballLabelStyle.LabelFormat))
                {
                    labelFormat = axis.TrackballLabelStyle.LabelFormat;
                }
                else if (axis is DateTimeAxis || axis is DateTimeCategoryAxis)
                {
                    labelFormat = "MM-dd-yyyy";
                }

                if (!clipRect.IsEmpty)
                {
                    if (axis.IsVertical)
                    {
                        _yValue = (float)clipRect.Top - actualArrangeRect.Top + (float)CartesianChart._chartArea.PlotArea.PlotAreaBounds.Top;
                    }
                    else
                    {
                        _xValue = (float)clipRect.Left - actualArrangeRect.Left;
                    }
                }

                var value = CartesianChart.PointToValue(axis, sp.X + _xValue, sp.Y + _yValue);
                var label = TrackballAxisLabelHelper.GetAxisLabel(axis, value, labelFormat);

                var helper = new TooltipHelper(Drawable) { Duration = int.MaxValue };
                var axisPointInfo = new TrackballAxisInfo(axis, helper, label, sp.X, sp.Y);
                axisPointInfo.Helper.Position = tooltipPosition;

                var arrangedRect = new Rect(axis.ArrangeRect.X, axis.ArrangeRect.Y, axis.ArrangeRect.X + axis.ArrangeRect.Width, axis.ArrangeRect.Y + axis.ArrangeRect.Height);
                axisPointInfo.Helper.Show(arrangedRect, new Rect(axis.IsVertical ? sp.X : sp.X - (CartesianChart.SeriesBounds.X - clipRect.X) - 1, axis.IsVertical ? sp.Y - (CartesianChart as IChart).TitleHeight - 1 : sp.Y, dimension, dimension), false);

                crosshairAxisTrackballInfo.Add(axisPointInfo);
            }
        }

        void DrawCrosshairLabels(ICanvas canvas)
        {
            if (crosshairAxisTrackballInfo.Count == 0 || CartesianChart == null) return;

            foreach (var labelInfo in crosshairAxisTrackballInfo)
            {
                TrackballAxisLabelHelper.MapChartLabelStyle(CartesianChart, labelInfo.Helper, labelInfo.Axis.TrackballLabelStyle);
                labelInfo.Helper.Draw(canvas);
            }
        }

        void Invalidate()
        {
            CartesianChart?._crosshairView.InvalidateDrawable();
        }

        void Drawable()
        {
        }

        #endregion

        #endregion
    }
}
