using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// This class is used to add an ellipse annotation in the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection. 
    /// </summary>
    /// <remarks>
    /// EllipseAnnotation is used to draw a circle or an ellipse over the chart area.
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart.Annotations>
    ///   <chart:EllipseAnnotation X1="3" Y1="10" X2="4" Y2="15" Text="Ellipse">
    ///   </chart:EllipseAnnotation>
    /// </chart:SfCartesianChart.Annotations>  
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfCartesianChart chart = new SfCartesianChart();
    ///  var ellipse = new EllipseAnnotation()
    ///  {
    ///    X1 = 3,
    ///    Y1 = 10,
    ///    X2 = 4,
    ///    Y2 = 15,
    ///    Text = "Ellipse",
    ///  };
    ///  
    /// chart.Annotations.Add(ellipse);
    /// ]]>
    /// </code>
    /// </example>
    public class EllipseAnnotation : ShapeAnnotation
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Width"/> property sets the width of the <see cref="EllipseAnnotation"/> in the chart.
        /// </remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(EllipseAnnotation),
            10.0,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Height"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Height"/> property sets the height of the <see cref="EllipseAnnotation"/> in the chart.
        /// </remarks>
        public static readonly BindableProperty HeightProperty = BindableProperty.Create(
            nameof(Height),
            typeof(double),
            typeof(EllipseAnnotation),
            10.0,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalAlignment"/> property controls the horizontal position alignment of the <see cref="EllipseAnnotation"/> within the chart area.
        /// </remarks>
        public static readonly BindableProperty HorizontalAlignmentProperty = BindableProperty.Create(
            nameof(HorizontalAlignment),
            typeof(ChartAlignment),
            typeof(EllipseAnnotation),
            ChartAlignment.Center,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalAlignment"/> property controls the vertical position alignment of the <see cref="EllipseAnnotation"/> within the chart area.
        /// </remarks>
        public static readonly BindableProperty VerticalAlignmentProperty = BindableProperty.Create(
            nameof(VerticalAlignment),
            typeof(ChartAlignment),
            typeof(EllipseAnnotation),
            ChartAlignment.Center,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the width value for the ellipse annotation.
        /// </summary>
        /// <value>This property takes the <see cref="double"/> as its value and its default value is 10.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:EllipseAnnotation X1="3" Y1="10" Height="20" Width="20"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///  // Eliminated for simplicity
        ///  var ellipse = new EllipseAnnotation()
        ///  {
        ///    X1 = 3,
        ///    Y1 = 10,
        ///    Width = 20,
        ///    Height = 20,
        ///  };
        ///  
        /// chart.Annotations.Add(ellipse);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height value for the ellipse annotation.
        /// </summary>
        /// <value>This property takes the <see cref="double"/> as its value and its default value is 10.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:EllipseAnnotation X1="3" Y1="10" Width="20" Height="20"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///  // Eliminated for simplicity
        ///  var ellipse = new EllipseAnnotation()
        ///  {
        ///    X1 = 3,
        ///    Y1 = 10,
        ///    Height = 20,
        ///    Width = 20,
        ///  };
        ///  
        /// chart.Annotations.Add(ellipse);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the ellipse annotation.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> as its value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:EllipseAnnotation X1="3" Y1="10" Width="20" Height="20" HorizontalAlignment="Start"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///  // Eliminated for simplicity
        ///  var ellipse = new EllipseAnnotation()
        ///  {
        ///    X1 = 3,
        ///    Y1 = 10,
        ///    Height = 20,
        ///    Width = 20,
        ///    HorizontalAlignment = ChartAlignment.Start,
        ///  };
        ///  
        /// chart.Annotations.Add(ellipse);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment HorizontalAlignment
        {
            get { return (ChartAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the ellipse annotation.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> as its value. Its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:EllipseAnnotation X1="3" Y1="10" Width="20" Height="20" VerticalAlignment="Start"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///  // Eliminated for simplicity
        ///  var ellipse = new EllipseAnnotation()
        ///  {
        ///    X1 = 3,
        ///    Y1 = 10,
        ///    Height = 20,
        ///    Width = 20,
        ///    VerticalAlignment = ChartAlignment.Start,
        ///  };
        ///  
        /// chart.Annotations.Add(ellipse);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment VerticalAlignment
        {
            get { return (ChartAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Chart != null)
            {
                if (CoordinateUnit == ChartCoordinateUnit.Axis)
                {
                    var clipRect = Chart.ChartArea.ActualSeriesClipRect;
                    canvas.ClipRectangle(clipRect);
                }

                canvas.SetFillPaint(Fill, RenderRect);
                canvas.FillEllipse(RenderRect);
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;

                if (StrokeDashArray != null && StrokeDashArray.Count > 0)
                {
                    canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
                }

                canvas.DrawEllipse(RenderRect);
                base.Draw(canvas, dirtyRect);
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition();

            if (double.IsNaN(Y1) || X1 == null)
                return;

            var x2 = ChartUtils.ConvertToDouble(X2);
            var y2 = Y2;

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);

                if (X2 != null && !double.IsNaN(Y2))
                {
                    (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);

                    double x = x2 > x1 ? x1 : x2;
                    double y = y2 > y1 ? y1 : y2;
                    double width = x2 > x1 ? x2 - x1 : x1 - x2;
                    double height = y2 > y1 ? y2 - y1 : y1 - y2;
                    ApplyAlignment(x, y, width, height);
                }
                else
                {
                    var width = double.IsNaN(Width) ? 0 : Width;
                    var height = double.IsNaN(Height) ? 0 : Height;
                    ApplyAlignment(x1, y1, width, height);
                }
            }
            else
            {
                var width = double.IsNaN(Width) ? 0 : Width;
                var height = double.IsNaN(Height) ? 0 : Height;
                if (X2 != null && !double.IsNaN(Y2))
                {
                    width = x2 > x1 ? x2 - x1 : x1 - x2;
                    height = y2 > y1 ? y2 - y1 : y1 - y2;
                }

                ApplyAlignment(x1, y1, width, height);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                SetTextAlignment(x1, y1);
            }
        }

        internal override void ResetPosition()
        {
            RenderRect = Rect.Zero;
            LabelRect = Rect.Zero;
        }

        #endregion

        #region Private Methods

        void ApplyAlignment(double x, double y, double width, double height)
        {
            switch (VerticalAlignment)
            {
                case ChartAlignment.Start:
                    y -= height;
                    break;
                case ChartAlignment.Center:
                    y -= height / 2;
                    break;
            }

            switch (HorizontalAlignment)
            {
                case ChartAlignment.Start:
                    x -= width;
                    break;
                case ChartAlignment.Center:
                    x -= width / 2;
                    break;
            }

            RenderRect = new RectF((float)x, (float)y, (float)width, (float)height);
        }

        #endregion

        #endregion
    }
}