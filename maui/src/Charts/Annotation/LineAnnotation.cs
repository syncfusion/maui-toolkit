using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class is used to add a line annotation to the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection.
	/// </summary>
	/// <remarks>
	/// LineAnnotation is used to draw a line across the chart area.
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart.Annotations>
	///   <chart:LineAnnotation X1="1" Y1="10" X2="4" Y2="20" Text="Line" CoordinateUnit="Axis">
	///   </chart:LineAnnotation>
	/// </chart:SfCartesianChart.Annotations>  
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	///  SfCartesianChart chart = new SfCartesianChart();
	///  var line = new LineAnnotation()
	///  {
	///    X1 = 1,
	///    Y1 = 10,
	///    X2 = 4,
	///    Y2 = 20,
	///    Text = "Line",
	///    CoordinateUnit= ChartCoordinateUnit.Axis,
	///  };
	///  
	/// chart.Annotations.Add(line);
	/// ]]>
	/// </code>
	/// </example>
	public class LineAnnotation : ShapeAnnotation
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="LineCap"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="LineCap"/> bindable property determines the style of the line cap for the <see cref="ChartAnnotation"/>.
        /// </remarks>
        public static readonly BindableProperty LineCapProperty = BindableProperty.Create(
           nameof(LineCap),
           typeof(ChartLineCap),
           typeof(LineAnnotation),
           ChartLineCap.None,
           BindingMode.Default,
           null,
           OnAnnotationPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Represents the type of cap for line annotation.
        /// Gets or sets the line cap value for the line annotation.
        /// </summary>
        /// <value>This property takes the <see cref="ChartLineCap"/> as its value and its default value is <see cref="ChartLineCap.None"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:LineAnnotation X1="0" Y1="10" X2="4" Y2="50" LineCap="Arrow"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///   SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///   // Eliminated for simplicity
        ///   var line = new LineAnnotation()
        ///   {
        ///       X1 = 0,
        ///       Y1 = 10,
        ///       X2 = 4,
        ///       Y2 = 50,
        ///       LineCap = ChartLineCap.Arrow,
        ///   };
        ///  
        ///   chart.Annotations.Add(line);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLineCap LineCap
        {
            get { return (ChartLineCap)GetValue(LineCapProperty); }
            set { SetValue(LineCapProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal float XPosition1 { get; set; }

        internal float XPosition2 { get; set; }

        internal float YPosition1 { get; set; }

        internal float YPosition2 { get; set; }

        internal float Angle { get; set; }

        internal List<Point> LineCapPoints { get; set; }

        internal ChartLabelStyle axisLabelStyle;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LineAnnotation"/>.
        /// </summary>
        public LineAnnotation()
        {
            LineCapPoints = new List<Point>();
            axisLabelStyle = new ChartLabelStyle();
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Chart != null)
            {
                canvas.CanvasSaveState();

                if (CoordinateUnit == ChartCoordinateUnit.Axis)
                {
                    var clipRect = Chart.ChartArea.ActualSeriesClipRect;
                    canvas.ClipRectangle(clipRect);
                }

                if (StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()))
                {
                    canvas.StrokeSize = (float)StrokeWidth;
                    canvas.StrokeColor = Stroke.ToColor();

                    if (StrokeDashArray != null && StrokeDashArray.Count > 0)
                    {
                        canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
                    }
                }

                if (LineCap == ChartLineCap.Arrow)
                {
                    canvas.DrawLine(XPosition1, YPosition1, XPosition2, YPosition2);
                    if (LineCapPoints.Count == 3)
                    {
                        var path = new PathF();
                        path.MoveTo((float)LineCapPoints[0].X, (float)LineCapPoints[0].Y);
                        path.LineTo((float)LineCapPoints[1].X, (float)LineCapPoints[1].Y);
                        path.LineTo((float)LineCapPoints[2].X, (float)LineCapPoints[2].Y);
                        path.Close();
                        canvas.StrokeColor = Stroke.ToColor();
                        canvas.StrokeSize = (float)StrokeWidth;
                        canvas.FillColor = Stroke.ToColor();
                        canvas.FillPath(path);
                    }
                }
                else
                {
                    canvas.DrawLine(XPosition1, YPosition1, XPosition2, YPosition2);
                }

                base.Draw(canvas, dirtyRect);
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition();

            if (X1 == null || X2 == null || double.IsNaN(Y1) || double.IsNaN(Y2))
                return;

            var x2 = ChartUtils.ConvertToDouble(X2);
            var y2 = Y2;

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                (x2, y2) = TransformCoordinates(chart, xAxis, yAxis, x2, y2);
            }

            XPosition1 = (float)x1;
            XPosition2 = (float)x2;
            YPosition1 = (float)y1;
            YPosition2 = (float)y2;

            Angle = (float)(Math.Atan2(YPosition2 - YPosition1, XPosition2 - XPosition1) * (180 / Math.PI));

            if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
            {
                LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
                double x = (LineCapPoints[1].X - LineCapPoints[2].X) / 2;
                double y = (LineCapPoints[1].Y - LineCapPoints[2].Y) / 2;

                XPosition2 = (float)(LineCapPoints[2].X + x);
                YPosition2 = (float)(LineCapPoints[2].Y + y);
            }

            RenderRect = new RectF(XPosition1, YPosition1, XPosition2 - XPosition1, YPosition2 - YPosition1);

            if (!string.IsNullOrEmpty(Text))
            {
                SetTextAlignment(XPosition1, YPosition1);
            }
        }

        internal override void SetTextAlignment(double x, double y)
        {
            if (annotationLabelStyle == null)
                return;

            var labelSize = annotationLabelStyle.MeasureLabel(Text);
            double halfBorderWidth = (annotationLabelStyle.StrokeWidth / 2);
            double labelHeight = labelSize.Height;
            double labelWidth = labelSize.Width;
            double labelRectX = x;
            double labelRectY = y;
            double annotationX = RenderRect.X;
            double annotationY = RenderRect.Y;
            double annotationHeight = RenderRect.Height;
            double annotationWidth = RenderRect.Width;

            switch (annotationLabelStyle.VerticalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectY = annotationY + (annotationHeight / 2) - (labelHeight / 2) - halfBorderWidth - StrokeWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectY = annotationY + (annotationHeight / 2) - (labelHeight / 2) - annotationLabelStyle.Margin.Bottom - StrokeWidth;
#if ANDROID
                    labelRectY = labelRectY - annotationLabelStyle.Margin.Top - StrokeWidth;
#endif
                    break;
                case ChartLabelAlignment.End:
                    labelRectY = annotationY + (annotationHeight / 2) + (labelHeight / 2) + halfBorderWidth - StrokeWidth;
                    break;
            }

            switch (annotationLabelStyle.HorizontalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectX = annotationX + halfBorderWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectX = annotationX + (annotationWidth / 2);
                    break;
                case ChartLabelAlignment.End:
                    labelRectX = annotationX + annotationWidth - halfBorderWidth - (labelWidth / 2);
                    break;
            }

            LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
        }

        internal void SetHorizontalTextAlignment(double x, double y)
        {
            var labelSize = annotationLabelStyle.MeasureLabel(Text);
            double halfBorderWidth = (float)annotationLabelStyle.StrokeWidth / 2;
            double labelHeight = labelSize.Height, labelWidth = labelSize.Width;
            double labelRectX = x, labelRectY = y;
            double annotationX = RenderRect.X, annotationY = RenderRect.Y;
            double annotationHeight = RenderRect.Height, annotationWidth = RenderRect.Width;

            switch (annotationLabelStyle.VerticalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectY = annotationY - (labelHeight / 2) - (annotationHeight / 2) - halfBorderWidth - StrokeWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectY = annotationY - StrokeWidth;
                    break;
                case ChartLabelAlignment.End:
                    labelRectY = annotationY + (labelHeight / 2) + (annotationHeight / 2) + halfBorderWidth - StrokeWidth;
                    break;
            }

            switch (annotationLabelStyle.HorizontalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectX = annotationX + (labelWidth / 2) + halfBorderWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectX = annotationX + (annotationWidth / 2);
                    break;
                case ChartLabelAlignment.End:
                    labelRectX = annotationX + annotationWidth - halfBorderWidth - (labelWidth / 2);
                    break;
            }

            LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
        }

        internal void SetVerticalTextAlignment(double x, double y)
        {
            var labelSize = annotationLabelStyle.MeasureLabel(Text);
            double halfBorderWidth = (float)(annotationLabelStyle.StrokeWidth / 2);
            double labelHeight = labelSize.Height, labelWidth = labelSize.Width;
            double labelRectX = x, labelRectY = y;
            double annotationX = RenderRect.X, annotationY = RenderRect.Y;
            double annotationHeight = RenderRect.Height, annotationWidth = RenderRect.Width;

            switch (annotationLabelStyle.VerticalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectY = annotationY - annotationHeight + (labelWidth / 2) + halfBorderWidth - StrokeWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectY = annotationY - (annotationHeight / 2) - StrokeWidth;
                    break;
                case ChartLabelAlignment.End:
                    labelRectY = annotationY - halfBorderWidth - (labelWidth / 2) - StrokeWidth;
                    break;
            }

            switch (annotationLabelStyle.HorizontalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectX = annotationX - (annotationWidth / 2) - (labelHeight / 2) - halfBorderWidth;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectX = annotationX;
                    break;
                case ChartLabelAlignment.End:
                    labelRectX = annotationX + (annotationWidth / 2) + (labelHeight / 2) + halfBorderWidth;
                    break;
            }

            LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
        }

        internal void CalculatePosition(bool isVertical, ChartAxis xAxis)
        {
            if (xAxis != null)
            {
                if (!isVertical)
                {
                    float x = XPosition2 > XPosition1 ? XPosition1 : XPosition2;
                    float y = YPosition2 > YPosition1 ? YPosition1 : YPosition2;
                    float actualWidth = XPosition2 > XPosition1 ? XPosition2 - XPosition1 : XPosition1 - XPosition2;

                    if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
                    {
                        LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
                        float arrowDifference = (float)(LineCapPoints[0].X - LineCapPoints[1].X);

                        if (xAxis.IsOpposed())
                        {
                            if (annotationLabelStyle.HorizontalTextAlignment == ChartLabelAlignment.Start)
                            {
                                x = x + Math.Abs(arrowDifference);
                            }
                        }
                        else
                        {
                            actualWidth = actualWidth - arrowDifference;
                        }

                        RenderRect = new Rect(x, y, actualWidth, (float)StrokeWidth);
                        XPosition2 -= arrowDifference;
                    }
                    else
                    {
                        RenderRect = new Rect(x, y, actualWidth, (float)StrokeWidth);
                    }
                }
                else
                {
                    if (LineCap == ChartLineCap.Arrow && StrokeWidth > 0)
                    {
                        LineCapPoints = CalculateArrowPoints(XPosition1, YPosition1, XPosition2, YPosition2);
                        float arrowDifference = (float)(LineCapPoints[0].Y - LineCapPoints[1].Y);

                        if (xAxis.IsOpposed())
                        {
                            if (annotationLabelStyle.VerticalTextAlignment == ChartLabelAlignment.End)
                            {
                                YPosition2 = YPosition2 - Math.Abs(arrowDifference);
                            }

                            RenderRect = new Rect(XPosition1, YPosition2, (float)StrokeWidth, YPosition2 - YPosition1);
                        }
                        else
                        {
                            RenderRect = new Rect(XPosition1, YPosition1, (float)StrokeWidth, YPosition1 - YPosition2 + arrowDifference);
                            YPosition2 -= arrowDifference;
                        }
                    }
                    else
                    {
                        if (xAxis.IsOpposed())
                        {
                            RenderRect = new Rect(XPosition1, YPosition2, (float)StrokeWidth, YPosition2 - YPosition1);
                        }
                        else
                        {
                            RenderRect = new Rect(XPosition1, YPosition1, (float)StrokeWidth, YPosition1 - YPosition2);
                        }
                    }
                }
            }
        }

        internal override void ResetPosition()
        {
            XPosition1 = XPosition2 = YPosition1 = YPosition2 = float.NaN;
            LineCapPoints.Clear();
            LabelRect = Rect.Zero;
        }

        internal static void OnAxisLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LineAnnotation annotation)
            {
                ChartBase.SetParent((Element?)oldValue, (Element?)newValue, annotation.Parent);

                if (oldValue is ChartLabelStyle style)
                {
                    annotation.UnHookStylePropertyChanged(style);
                }

                if (newValue is ChartLabelStyle newStyle)
                {
                    annotation.axisLabelStyle = newStyle;
                    newStyle.Parent = annotation.Parent;
                    SetInheritedBindingContext(newStyle, annotation.BindingContext);
                    newStyle.PropertyChanged += annotation.Style_PropertyChanged;
                }
                else
                {
                    ChartLabelStyle defaultStyle = new ChartLabelStyle() { FontSize = 14 };
                    annotation.axisLabelStyle = defaultStyle;
                    defaultStyle.Parent = annotation.Parent;
                    SetInheritedBindingContext(defaultStyle, annotation.BindingContext);
                    defaultStyle.PropertyChanged += annotation.Style_PropertyChanged;
                }

                annotation.UpdateLayout();
                annotation.Invalidate();
            }
        }

        internal override Brush GetDefaultFillColor()
        {
            return new SolidColorBrush(Color.FromArgb("#49454F"));
        }

        internal override Brush GetDefaultStrokeColor()
        {
            return new SolidColorBrush(Color.FromArgb("#49454F"));
        }

        #endregion

        #region Private Methods

        List<Point> CalculateArrowPoints(float xPosition1, float yPosition1, float xPosition2, float yPosition2)
        {
            var height = 10 + StrokeWidth;
            var width = 20 + StrokeWidth;
            var angle = Math.Atan2(yPosition2 - yPosition1, xPosition2 - xPosition1);
            var halfRadius = ((Math.PI * width / 180) / 2) * 3;

            var ax = xPosition2 - (height * Math.Cos(angle - halfRadius));
            var ay = yPosition2 - (height * Math.Sin(angle - halfRadius));
            var bx = xPosition2 - (height * Math.Cos(angle + halfRadius));
            var by = yPosition2 - (height * Math.Sin(angle + halfRadius));

            var points = new List<Point>();
            points.Add(new Point(xPosition2, yPosition2));
            points.Add(new Point(ax, ay));
            points.Add(new Point(bx, by));

            return points;
        }

        void Style_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is ChartLabelStyle)
            {
                if (e.PropertyName == ChartLabelStyle.MarginProperty.PropertyName || e.PropertyName == ChartLabelStyle.FontSizeProperty.PropertyName || e.PropertyName == ChartLabelStyle.LabelFormatProperty.PropertyName)
                {
                    UpdateLayout();
                }

                Invalidate();
            }
        }

        #endregion

        #endregion
    }
}
