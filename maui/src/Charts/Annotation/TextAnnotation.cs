using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class is used to add a text annotation to the <see cref="SfCartesianChart"/>. An instance of this class needs to be added to the <see cref="SfCartesianChart.Annotations"/> collection.
	/// </summary>
	/// <remarks>
	/// Text annotations are used to add simple text at specific points over the chart area.
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart.Annotations>
	///   <chart:TextAnnotation X1="3" Y1="10" Text="TextAnnotation">
	///   </chart:TextAnnotation>
	/// </chart:SfCartesianChart.Annotations>  
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	///  SfCartesianChart chart = new SfCartesianChart();
	///  var text = new TextAnnotation()
	///  {
	///    X1 = 3,
	///    Y1 = 10,
	///    Text = "TextAnnotation",
	///  };
	///  
	/// chart.Annotations.Add(text);
	/// ]]>
	/// </code>
	/// </example>
	public class TextAnnotation : ChartAnnotation, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Text"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Text"/> property defines the text content displayed by the <see cref="TextAnnotation"/> in the chart.
        /// </remarks>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(TextAnnotation),
            string.Empty,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="LabelStyle"/> property allows to customize the appearance of the label associated with the <see cref="TextAnnotation"/>.
        /// </remarks>
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            nameof(LabelStyle),
            typeof(ChartAnnotationLabelStyle),
            typeof(TextAnnotation),
            null,
            BindingMode.Default,
            null,
            OnLabelStylePropertyChanged,
            defaultValueCreator: LabelStyleDefaultValueCreator);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text to be displayed on the annotation. 
        /// </summary>
        /// <value>This property takes the <c>string</c> as its value and its default value is string.Empty.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:TextAnnotation X1="3" Y1="30" Text="TextAnnotation">         
        ///          </chart:HorizontalLineAnnotation>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///     // Eliminated for simplicity
        ///     var text = new TextAnnotation()
        ///     {
        ///         X1 = 3,
        ///         Y1 = 30,       
        ///         Text = "TextAnnotation",
        ///     };
        ///  
        /// chart.Annotations.Add(text);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Get or set the value to customize the appearance of annotation text.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAnnotationLabelStyle"/> as its value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:TextAnnotation X1="3" Y1="30" Text="TextAnnotation">
        ///           <chart:TextAnnotation.LabelStyle>
        ///             <chart:ChartAnnotationLabelStyle HorizontalTextAlignment="Start" VerticalTextAlignment="Start"/>
        ///           </chart:TextAnnotation.LabelStyle>
        ///          </chart:TextAnnotation>
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
        ///     var textAnnotation = new TextAnnotation()
        ///     {
        ///         X1 = 3,
        ///         Y1 = 30,    
        ///         Text = "TextAnnotation",
        ///     };
        ///  
        ///  textAnnotation.LabelStyle = new ChartAnnotationLabelStyle()
        ///  {
        ///     HorizontalTextAlignment = ChartLabelAlignment.Start,
        ///     VerticalTextAlignment = ChartLabelAlignment.Start,
        ///  };
        ///  
        /// chart.Annotations.Add(textAnnotation);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAnnotationLabelStyle LabelStyle
        {
            get { return (ChartAnnotationLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal Rect LabelRect { get; set; }
        internal float XPosition1 { get; set; }
        internal float YPosition1 { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TextAnnotation()
        {
            ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
        }

        #endregion

        #region Methods

        #region Theme Interface Implementation

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                if (Chart != null)
                {
                    if (CoordinateUnit == ChartCoordinateUnit.Axis)
                    {
                        var clipRect = Chart.ChartArea.ActualSeriesClipRect;
                        canvas.ClipRectangle(clipRect);
                    }

                    canvas.StrokeSize = (float)annotationLabelStyle.StrokeWidth;
                    canvas.StrokeColor = annotationLabelStyle.Stroke.ToColor();
                    annotationLabelStyle.DrawTextBackground(canvas, Text, annotationLabelStyle.Background, new Point(LabelRect.X, LabelRect.Y));
                    annotationLabelStyle.DrawLabel(canvas, Text, new Point(LabelRect.X, LabelRect.Y));
                }
            }
        }

        internal override void Dispose()
        {
            UnHookStylePropertyChanged(annotationLabelStyle);
            base.Dispose();
        }

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (annotationLabelStyle != null)
            {
                SetInheritedBindingContext(annotationLabelStyle, BindingContext);
            }
        }

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnParentSet()
        {
            base.OnParentSet();

            if (annotationLabelStyle != null)
            {
                annotationLabelStyle.Parent = Parent;
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition();

            if (X1 == null || double.IsNaN(Y1))
                return;

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                if (!xAxis.IsVertical)
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                }
                else
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                }
            }
            else
            {
                x1 += chart.SeriesBounds.Left;
            }

            XPosition1 = (float)x1;
            YPosition1 = (float)y1;

            if (!string.IsNullOrEmpty(Text))
            {
                SetTextAlignment(XPosition1, YPosition1);
            }
        }

        internal override void ResetPosition()
        {
            XPosition1 = YPosition1 = float.NaN;
            LabelRect = Rect.Zero;
        }

        #endregion

        #region Private Methods

        void SetTextAlignment(double xPosition1, double yPosition1)
        {
            var labelSize = annotationLabelStyle.MeasureLabel(Text);

            var halfBorderWidth = (annotationLabelStyle.StrokeWidth / 2);
            double labelHeight = labelSize.Height, labelWidth = labelSize.Width;
            double labelRectX = xPosition1, labelRectY = yPosition1;
            double labelTop = 0, labelLeft = 0;
#if Android
			double leftMargin = annotationLabelStyle.MarginLeft;
			double rightMargin = annotationLabelStyle.MarginRight;
			double topMargin = annotationLabelStyle.MarginTop;
			double bottomMargin = annotationLabelStyle.MarginBottom ;
			labelTop = (topMargin / 2) - (bottomMargin / 2);
            labelLeft = (leftMargin / 2) - (rightMargin / 2);
#endif
            switch (annotationLabelStyle.VerticalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectY = labelRectY - halfBorderWidth - (labelHeight / 2) + labelTop;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectY = labelRectY + labelTop;
                    break;
                case ChartLabelAlignment.End:
                    labelRectY = labelRectY + halfBorderWidth + (labelHeight / 2) + labelTop;
                    break;
            }

            switch (annotationLabelStyle.HorizontalTextAlignment)
            {
                case ChartLabelAlignment.Start:
                    labelRectX = labelRectX - (labelWidth / 2) - halfBorderWidth + labelLeft;
                    break;
                case ChartLabelAlignment.Center:
                    labelRectX = labelRectX + labelLeft;
                    break;
                case ChartLabelAlignment.End:
                    labelRectX = labelRectX + halfBorderWidth + (labelWidth / 2) + labelLeft;
                    break;
            }

            LabelRect = new Rect(labelRectX, labelRectY, labelWidth, labelHeight);
        }

        static object LabelStyleDefaultValueCreator(BindableObject bindable)
        {
            return new ChartAnnotationLabelStyle() { FontSize = 12 };
        }

        #endregion

        #endregion
    }
}