using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// This class is used to add a horizontal line annotation to the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection. 
    /// </summary>
    /// <remarks>
    /// HorizontalLineAnnotation is used to draw a horizontal line across the chart area.
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart.Annotations>
    ///   <chart:HorizontalLineAnnotation Y1="10" Text="Horizontal" ShowAxisLabel="True">
    ///   </chart:HorizontalLineAnnotation>
    /// </chart:SfCartesianChart.Annotations>  
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfCartesianChart chart = new SfCartesianChart();
    ///  var horizontal = new HorizontalLineAnnotation()
    ///  {
    ///    Y1 = 10,
    ///    Text = "Horizontal",
    ///    ShowAxisLabel= true,
    ///  };
    ///  
    /// chart.Annotations.Add(horizontal);
    /// ]]>
    /// </code>
    /// </example>
    public class HorizontalLineAnnotation : LineAnnotation
    {
        #region Private Fields

        bool _isVertical = false;
        string _labelText = string.Empty;
        RectF _axisLabelRect;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ShowAxisLabel"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowAxisLabel"/> property determines whether the axis label is visible for the <see cref="HorizontalLineAnnotation"/>.
        /// </remarks>
        public static readonly BindableProperty ShowAxisLabelProperty = BindableProperty.Create(
            nameof(ShowAxisLabel),
            typeof(bool),
            typeof(HorizontalLineAnnotation),
            false,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLabelStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="AxisLabelStyle"/> property allows customization of the axis label in the <see cref="HorizontalLineAnnotation"/>.
        /// </remarks>
        public static readonly BindableProperty AxisLabelStyleProperty = BindableProperty.Create(
            nameof(AxisLabelStyle),
            typeof(ChartLabelStyle),
            typeof(HorizontalLineAnnotation),
            null,
            BindingMode.Default,
            null,
            OnAxisLabelStylePropertyChanged,
            defaultValueCreator: AxisLabelStyleDefaultValueCreator);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to enable or disable the display of the annotation label on the axis.
        /// </summary>
        /// <value>This property takes the <c>bool</c> as its value and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:HorizontalLineAnnotation Y1="10" ShowAxisLabel="True"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///    SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///    // Eliminated for simplicity
        ///    var horizontal = new HorizontalLineAnnotation()
        ///    {
        ///        Y1 = 10,
        ///        ShowAxisLabel = true,
        ///    };
        /// 
        ///    chart.Annotations.Add(horizontal);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowAxisLabel
        {
            get { return (bool)GetValue(ShowAxisLabelProperty); }
            set { SetValue(ShowAxisLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the customized style for the annotation axis label.
        /// </summary>
        /// <value>This property takes the <see cref="ChartLabelStyle"/> as its value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:HorizontalLineAnnotation Y1="10" >
        ///           <chart:HorizontalLineAnnotation.AxisLabelStyle>
        ///             <chart:ChartLabelStyle Background="Yellow"/>
        ///           </chart:HorizontalLineAnnotation.AxisLabelStyle>
        ///          </chart:HorizontalLineAnnotation>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///     // Eliminated for simplicity
        ///     var horizontal = new HorizontalLineAnnotation()
        ///     {
        ///         Y1 = 10,
        ///     };
        ///  
        ///  horizontal.AxisLabelStyle = new ChartLabelStyle()
        ///  {
        ///     Background = Colors.Yellow,
        ///  };
        ///  
        /// chart.Annotations.Add(horizontal);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLabelStyle AxisLabelStyle
        {
            get { return (ChartLabelStyle)GetValue(AxisLabelStyleProperty); }
            set { SetValue(AxisLabelStyleProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalLineAnnotation"/>.
        /// </summary>
        public HorizontalLineAnnotation()
        {
            ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
            annotationLabelStyle.HorizontalTextAlignment = ChartLabelAlignment.End;
            annotationLabelStyle.VerticalTextAlignment = ChartLabelAlignment.Start;
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            if (ShowAxisLabel == true)
            {
                canvas.StrokeSize = double.IsNaN(axisLabelStyle.StrokeWidth) ? 0 : (float)axisLabelStyle.StrokeWidth;
                canvas.StrokeColor = axisLabelStyle.Stroke == null ? Colors.Transparent : axisLabelStyle.Stroke.ToColor();
                axisLabelStyle.DrawTextBackground(canvas, _labelText, axisLabelStyle.Background, new PointF(_axisLabelRect.X, _axisLabelRect.Y));
                axisLabelStyle.DrawLabel(canvas, _labelText, new PointF(_axisLabelRect.X, _axisLabelRect.Y));
            }
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (AxisLabelStyle != null)
            {
                SetInheritedBindingContext(AxisLabelStyle, BindingContext);
            }
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (AxisLabelStyle != null)
            {
                AxisLabelStyle.Parent = Parent;
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition();

            if (double.IsNaN(Y1))
                return;

            var actualSeriesClipRect = chart.ChartArea.ActualSeriesClipRect;
            var x2 = X2 != null ? ChartUtils.ConvertToDouble(X2) : double.NaN;
            var y2 = Y1;
            bool isOpposed = yAxis.IsOpposed();

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                if (!xAxis.IsVertical)
                {
                    _isVertical = false;
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                    (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
                    x1 = X1 == null ? isOpposed ? actualSeriesClipRect.Right : actualSeriesClipRect.Left : x1;
                    x2 = X2 == null ? isOpposed ? actualSeriesClipRect.Left : actualSeriesClipRect.Right : x2;
                }
                else
                {
                    _isVertical = true;
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                    (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
                    y1 = X1 == null ? isOpposed ? actualSeriesClipRect.Top : actualSeriesClipRect.Bottom : x1;
                    y2 = X2 == null ? isOpposed ? actualSeriesClipRect.Bottom : actualSeriesClipRect.Top : x2;
                }
            }
            else
            {
                if (!xAxis.IsVertical)
                {
                    x1 = X1 == null ? isOpposed ? actualSeriesClipRect.Right : actualSeriesClipRect.Left : Convert.ToDouble(X1);
                    x2 = X2 == null ? isOpposed ? actualSeriesClipRect.Left : actualSeriesClipRect.Right : Convert.ToDouble(X2);
                }
            }

            XPosition1 = (float)x1;
            XPosition2 = (float)x2;
            YPosition1 = (float)y1;
            YPosition2 = (float)y2;
            Angle = (float)(Math.Atan2(YPosition2 - YPosition1, XPosition2 - XPosition1) * (180 / Math.PI));

            CalculatePosition(_isVertical, xAxis);

            if (ShowAxisLabel)
            {
                CalculateHorizontalAxisLabelPosition(yAxis, actualSeriesClipRect);
            }

            if (Text != null && !string.IsNullOrEmpty(Text))
            {
                SetTextAlignment(x1, y1);
            }
        }

        internal override void SetTextAlignment(double x, double y)
        {
            if (_isVertical)
            {
                SetVerticalTextAlignment(x, y);
            }
            else
            {
                SetHorizontalTextAlignment(x, y);
            }
        }

        internal override void Dispose()
        {
            UnHookStylePropertyChanged(AxisLabelStyle);
            base.Dispose();
        }

        internal override void ResetPosition()
        {
            XPosition1 = XPosition2 = YPosition1 = YPosition2 = float.NaN;
            LineCapPoints.Clear();
            LabelRect = Rect.Zero;
            _axisLabelRect = Rect.Zero;
        }

        #endregion

        #region Private Method

        void CalculateHorizontalAxisLabelPosition(ChartAxis yAxis, Rect actualSeriesClipRect)
        {
            float marginTop = (float)axisLabelStyle.Margin.Top;
            float marginBottom = (float)axisLabelStyle.Margin.Bottom;
            float marginLeft = (float)axisLabelStyle.Margin.Left;
            float marginRight = (float)axisLabelStyle.Margin.Right;
            string labelFormat = !string.IsNullOrEmpty(axisLabelStyle.LabelFormat) ? axisLabelStyle.LabelFormat : "##.##";
            _labelText = Y1.ToString(labelFormat);
            var labelSize = axisLabelStyle.MeasureLabel(_labelText);
            float x, y;

            if (!yAxis.IsVertical)
            {
                x = XPosition1;
                y = (float)(yAxis.IsOpposed() ? (actualSeriesClipRect.Top - labelSize.Height / 2 - marginTop) : (float)actualSeriesClipRect.Bottom + labelSize.Height / 2 + marginBottom);
            }
            else
            {

                x = (float)(yAxis.IsOpposed() ? (yAxis.ArrangeRect.Left + labelSize.Width / 2 + marginRight) : (float)actualSeriesClipRect.Left - labelSize.Width / 2 - marginLeft);
                y = YPosition1;
            }

            _axisLabelRect = new RectF(new Point(x, y), labelSize);
        }

        static object AxisLabelStyleDefaultValueCreator(BindableObject bindable)
        {
            return new ChartLabelStyle() { FontSize = 14 };
        }

        #endregion

        #endregion
    }
}