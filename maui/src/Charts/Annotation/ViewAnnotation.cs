using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class is used to add an view annotation in <see cref="SfCartesianChart"/>. An instance of this class need to be added to <see cref="SfCartesianChart.Annotations"/> collection.
	/// </summary>
	/// <remarks>
	/// ViewAnnotation is used to add any custom shape or custom view over the chart area.
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart.Annotations>
	///   <chart:ViewAnnotation X1="3" Y1="10">
	///     <chart:ViewAnnotation.View>
	///         <Label Text = "ViewAnnotation"/>
	///     </chart:ViewAnnotation.View>
	///   </chart:ViewAnnotation>
	/// </chart:SfCartesianChart.Annotations>  
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// var view = new ViewAnnotation()
	/// {
	///   X1 = 3,
	///   Y1 = 10,  
	/// };
	/// var label = new Label()
	/// {
	///    Text = "ViewAnnotation",
	/// };
	/// 
	/// view.View = label;
	/// 
	/// chart.Annotations.Add(view);
	/// ]]>
	/// </code>
	/// </example>
	public class ViewAnnotation : ChartAnnotation
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="View"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="View"/> property defines the custom view to be displayed within the <see cref="ViewAnnotation"/>.
        /// </remarks>
        public static readonly BindableProperty ViewProperty = BindableProperty.Create(
            nameof(View),
            typeof(View),
            typeof(ViewAnnotation),
            null,
            BindingMode.Default,
            null,
            OnViewPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalAlignment"/> property specifies how the custom view is horizontally aligned within the chart.
        /// </remarks>
        public static readonly BindableProperty HorizontalAlignmentProperty = BindableProperty.Create(
            nameof(HorizontalAlignment),
            typeof(ChartAlignment),
            typeof(ViewAnnotation),
            ChartAlignment.Center,
            BindingMode.Default, 
            null,
            OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="VerticalAlignment"/> property specifies how the custom view is vertically aligned within the chart.
        /// </remarks>
        public static readonly BindableProperty VerticalAlignmentProperty = BindableProperty.Create(
            nameof(VerticalAlignment),
            typeof(ChartAlignment),
            typeof(ViewAnnotation),
            ChartAlignment.Center,
            BindingMode.Default,
            null,
            OnAnnotationPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the View that represents custom view for annotation
        /// </summary>
        /// <value>This property takes the <see cref="Microsoft.Maui.Controls.View"/> as its value and its default value is null</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart.Annotations>
        ///   <chart:ViewAnnotation X1="3" Y1="10">
        ///     <chart:ViewAnnotation.View>
        ///         <Label Text = "ViewAnnotation"/>
        ///     </chart:ViewAnnotation.View>
        ///   </chart:ViewAnnotation>
        /// </chart:SfCartesianChart.Annotations>  
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// var view = new ViewAnnotation()
        /// {
        ///   X1 = 3,
        ///   Y1 = 10,  
        /// };
        /// var label = new Label()
        /// {
        ///    Text = "ViewAnnotation",
        /// };
        /// 
        /// view.View = label;
        /// 
        /// chart.Annotations.Add(view);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public View View
        {
            get { return (View)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the view.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> as its value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart.Annotations>
        ///   <chart:ViewAnnotation X1="3" Y1="10" HorizontalAlignment="Start">
        ///     <chart:ViewAnnotation.View>
        ///         <Label Text = "ViewAnnotation"/>
        ///     </chart:ViewAnnotation.View>
        ///   </chart:ViewAnnotation>
        /// </chart:SfCartesianChart.Annotations>  
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// var view = new ViewAnnotation()
        /// {
        ///   X1 = 3,
        ///   Y1 = 10,  
        ///   HorizontalAlignment = ChartAlignment.Start,
        /// };
        /// var label = new Label()
        /// {
        ///    Text = "ViewAnnotation",
        /// };
        /// 
        /// view.View = label;
        /// 
        /// chart.Annotations.Add(view);
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
        /// Gets or sets the vertical alignment of the view. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> as its value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart.Annotations>
        ///   <chart:ViewAnnotation X1="3" Y1="10" VerticalAlignment="Start">
        ///     <chart:ViewAnnotation.View>
        ///         <Label Text = "ViewAnnotation"/>
        ///     </chart:ViewAnnotation.View>
        ///   </chart:ViewAnnotation>
        /// </chart:SfCartesianChart.Annotations>  
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// var view = new ViewAnnotation()
        /// {
        ///   X1 = 3,
        ///   Y1 = 10,  
        ///   VerticalAlignment = ChartAlignment.Start,
        /// };
        /// var label = new Label()
        /// {
        ///    Text = "ViewAnnotation",
        /// };
        /// 
        /// view.View = label;
        /// 
        /// chart.Annotations.Add(view);
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
		/// <exclude/>
		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (View != null)
            {
                SetInheritedBindingContext(View, BindingContext);
            }
        }

        internal override void Dispose()
        {
            if (View != null)
            {
                SetInheritedBindingContext(View, null);
            }
        }

        #endregion

        #region Internal Methods

        internal override void OnLayout(SfCartesianChart chart, ChartAxis xAxis, ChartAxis yAxis, double x1, double y1)
        {
            ResetPosition(x1, y1);

            if (View == null || X1 == null || double.IsNaN(Y1))
                return;

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                if (xAxis == null || yAxis == null)
                {
                    return;
                }

                if (!xAxis.IsVertical)
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                }
                else
                {
                    (x1, y1) = TransformCoordinates(chart, xAxis, yAxis, x1, y1);
                }
            }

            LayoutView(chart, x1, y1);
        }

        #endregion

        #region Property Callback Methods

        static void OnViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartAnnotation annotation)
            {
                if (oldValue != null)
                {
                    annotation.Chart?.ViewAnnotationDetachedToChart(annotation);
                }

                if (newValue != null)
                {
                    annotation.Chart?.ViewAnnotationAttachedToChart(annotation);
                }

                annotation.UpdateLayout();
            }
        }

        #endregion

        #region Private Methods

        void LayoutView(SfCartesianChart chart, double x1, double y1)
        {
            SetInheritedBindingContext(View, BindingContext);

			double measuredWidth;
			double measuredHeight;

			if (View.Bounds.IsEmpty)
			{
				var sizeRequest = View.Measure(double.PositiveInfinity, double.PositiveInfinity);
				measuredWidth = sizeRequest.Request.Width;
				measuredHeight = sizeRequest.Request.Height;
			}
			else
			{
				measuredWidth = View.Bounds.Width;
				measuredHeight = View.Bounds.Height;
			}

			SetViewAlignment(ref x1, ref y1, measuredWidth, measuredHeight);               

            AbsoluteLayout.SetLayoutBounds(View, new Rect(x1, y1, measuredWidth, measuredHeight));
            AbsoluteLayout.SetLayoutFlags(View, AbsoluteLayoutFlags.None);

            if (CoordinateUnit == ChartCoordinateUnit.Axis)
            {
                var bounds = chart.ChartArea.ActualSeriesClipRect;
                ClipViewToSeriesBounds(View, bounds);
            }
        }

        void ClipViewToSeriesBounds(View childView, RectF parentLayoutBounds)
        {
            var childViewBounds = AbsoluteLayout.GetLayoutBounds(childView);

            double clipX = (childViewBounds.X < parentLayoutBounds.X) && (childViewBounds.Right > parentLayoutBounds.Left) ? childViewBounds.Width - (childViewBounds.Right - parentLayoutBounds.Left) : 0;
            double clipY = (childViewBounds.Y < parentLayoutBounds.Y) && (childViewBounds.Bottom > parentLayoutBounds.Top) ? childViewBounds.Height - (childViewBounds.Bottom - parentLayoutBounds.Top) : 0;
            double clipWidth = 0;

            //calculated the width of the clip need to visible in the chart.
            if (childViewBounds.X < parentLayoutBounds.X)
            {
                if (childViewBounds.Right < parentLayoutBounds.Left)
                    clipWidth = 0;
                else
                {
                    if (childViewBounds.Right > parentLayoutBounds.Right)
                        clipWidth = parentLayoutBounds.Width;
                    else
                        clipWidth = childViewBounds.Right - parentLayoutBounds.Left;
                }
            }
            else
            {
                if (childViewBounds.Left > parentLayoutBounds.Right)
                    clipWidth = 0;
                else
                {
                    if (childViewBounds.Right > parentLayoutBounds.Right)
                        clipWidth = parentLayoutBounds.Right - childViewBounds.Left;
                    else
                        clipWidth = childViewBounds.Width;
                }
            }

            double clipHeight = 0;

            //Calculated the height of the view need to visible in the chart.
            if (childViewBounds.Y < parentLayoutBounds.Y)
            {
                if (childViewBounds.Bottom < parentLayoutBounds.Top)
                    clipHeight = 0;
                else
                {
                    if (childViewBounds.Bottom > parentLayoutBounds.Bottom)
                        clipHeight = parentLayoutBounds.Height;
                    else
                        clipHeight = childViewBounds.Bottom - parentLayoutBounds.Top;
                }
            }
            else
            {
                if (childViewBounds.Top > parentLayoutBounds.Bottom)
                    clipHeight = 0;
                else
                {
                    if (childViewBounds.Bottom > parentLayoutBounds.Bottom)
                        clipHeight = parentLayoutBounds.Bottom - childViewBounds.Top;
                    else
                        clipHeight = childViewBounds.Height;
                }
            }

            var clipGeometry = new RectangleGeometry
            {
                Rect = new Rect(clipX, clipY, clipWidth, clipHeight)
            };

            childView.Clip = clipGeometry;
        }

        void SetViewAlignment(ref double x, ref double y, double width, double height)
        {
            switch (VerticalAlignment)
            {
                case ChartAlignment.Start:
                    y -= height;
                    break;
                case ChartAlignment.Center:
                    y -= (height / 2);
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
        }

        void ResetPosition(double x, double y)
        {
            x = y = double.NaN;
        }

        #endregion

        #endregion
    }
}