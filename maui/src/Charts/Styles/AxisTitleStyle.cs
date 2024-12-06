using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Rect = Microsoft.Maui.Graphics.RectF;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the chart axis's title class.
	/// </summary>
	/// <remarks>
	/// <para>To customize the chart axis's title, add the <see cref="ChartAxisTitle"/> instance to the <see cref="ChartAxis.Title"/> property as shown in the following code sample.</para>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	/// 
	///     <chart:SfCartesianChart.XAxes>
	///         <chart:CategoryAxis>
	///            <chart:CategoryAxis.Title>
	///                <chart:ChartAxisTitle Text="AxisTitle" 
	///                                      TextColor="Red"
	///                                      Background="Yellow/>
	///            </chart:CategoryAxis.Title>
	///        </chart:CategoryAxis>
	///     </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// CategoryAxis xaxis = new CategoryAxis();
	/// xaxis.Title = new ChartAxisTitle()
	/// {
	///     Text = "AxisTitle",
	///     TextColor = Colors.Red,
	///     Background = new SolidColorBrush(Colors.Yellow)
	/// };
	/// chart.XAxes.Add(xaxis);
	///
	/// ]]>
	/// </code>
	/// *** 
	/// 
	/// <para>It provides more options to customize the chart axis title.</para>
	/// 
	/// <para> <b>Text - </b> To sets the title for axis, refer to this <see cref="Text"/> property.</para>
	/// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
	/// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
	/// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
	/// 
	/// </remarks>
	public partial class ChartAxisTitle : ChartLabelStyle
	{
		#region Properties

		/// <summary>
		/// Identifies the <see cref="Text"/> bindable property.
		/// </summary>        
		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			nameof(Text),
			typeof(string),
			typeof(ChartAxisTitle),
			string.Empty,
			BindingMode.Default,
			null,
			OnTextPropertyChanged);

		/// <summary>
		/// Gets or sets a value that displays the content for the axis title.
		/// </summary>
		/// <value>It accepts string values.</value>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		SizeF _textSize;

		Rect _measuringRect;

		internal float Left { get; set; }

		internal float Top { get; set; }

		internal ChartAxis? Axis { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisTitle"/>.
		/// </summary>
		public ChartAxisTitle() : base()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
		}

		#endregion

		#region Methods

		internal override Color GetDefaultTextColor()
		{
			return Color.FromArgb("#49454F");
		}

		internal override double GetDefaultFontSize()
		{
			return 14;
		}

		internal override Thickness GetDefaultMargin()
		{
			return new Thickness(5, 12, 5, 2);
		}

		internal void Measure()
		{
			if (Axis == null)
			{
				return;
			}

			SizeF measuredSize = Text.Measure(this);
			_measuringRect = new Rect(new PointF(0, 0), measuredSize);

			if (Axis.IsVertical)
			{
				_textSize = new SizeF(measuredSize.Height, measuredSize.Width);
			}
			else
			{
				_textSize = new SizeF(measuredSize.Width, measuredSize.Height);
			}
		}

		internal Size GetDesiredSize()
		{
			if (_textSize.Width == 0 && string.IsNullOrEmpty(Text))
			{
				return new SizeF(_textSize.Width, _textSize.Height);
			}

			if (Axis != null && Axis.IsVertical)
			{
				return new Size(_textSize.Width + Margin.Top + Margin.Bottom, _textSize.Height + Margin.Left + Margin.Right);
			}

			return new Size(_textSize.Width + Margin.Left + Margin.Right, _textSize.Height + Margin.Top + Margin.Bottom);
		}

		internal void Draw(ICanvas canvas)
		{
			if (_measuringRect == RectF.Zero)
			{
				_measuringRect = new Rect();
			}

			canvas.CanvasSaveState();

			if (Axis != null && Axis.IsVertical)
			{
				float x, y;
				bool opposedPosition = Axis.IsOpposed();

				x = Left + (float)Margin.Bottom -
						   (opposedPosition ? _measuringRect.Bottom : -_measuringRect.Bottom) - (_textSize.Height / 2);
				//Added platform specific code for position the label.
#if ANDROID
                x += _textSize.Width / 2;
#endif
				y = Top + (_textSize.Width / 2) - _measuringRect.Bottom;

				canvas.Rotate(opposedPosition ? 90 : -90, x + _measuringRect.Center.X, y + _measuringRect.Center.Y);

				if (CanDraw())
				{
					DrawTitleBackground(canvas, _measuringRect, x, y);
				}

#if ANDROID
                if (opposedPosition)
                {
                    y -= (float)Margin.Top;
                }
                canvas.DrawText(Text, x, y , this);
#else
				canvas.DrawText(Text, x, y - (float)Margin.Bottom, this);
#endif
			}
			else
			{
				float x = Left - _measuringRect.Width / 2;
				float y = Top + (float)Margin.Top;
				float yPos = Top + (float)Margin.Top / 4;
#if ANDROID
                //Addded platform specific code for position the label.
                y += _measuringRect.Height / 2;
#endif
				if (CanDraw())
				{
					DrawTitleBackground(canvas, _measuringRect, x, y);
				}

#if ANDROID
                canvas.DrawText(Text, x, y - (float)Margin.Left, this);
#else
				canvas.DrawText(Text, x, yPos, this);
#endif
			}

			canvas.CanvasRestoreState();
		}

		internal void DrawTitleBackground(ICanvas canvas, Rect bounds, float x, float y)
		{
			canvas.StrokeSize = (float)StrokeWidth;
			canvas.StrokeColor = Stroke.ToColor();
			canvas.FillColor = (Background as SolidColorBrush)?.Color;
			CornerRadius cornerRadius = CornerRadius;
			Rect rect;

			if (Axis != null && Axis.IsVertical)
			{
				bool isOpposed = Axis.IsOpposed();

				var top = isOpposed ? Margin.Bottom : Margin.Top;
				var bottom = isOpposed ? Margin.Top : Margin.Bottom;
				var left = isOpposed ? Margin.Right : Margin.Left;
				var right = isOpposed ? Margin.Left : Margin.Right;
				;
#if ANDROID
                    rect = new Rect( x - (float)left,
                    y - bounds.Height - (float)bottom,
                    bounds.Width + (float)left + (float)right,
                    bounds.Height + (float)bottom + (float)top);
#else
				rect = new Rect(x - (float)left,
				y - (float)right,
				bounds.Width + (float)left + (float)right,
				bounds.Height + (float)bottom / 2 + (float)top / 2);
#endif
			}
			else
			{
#if ANDROID
                    rect = new Rect(x - (float)Margin.Left,
                    y - (float)Margin.Top + (float)Margin.Left - bounds.Height,
                    bounds.Width + (float)Margin.Right + (float)Margin.Left,
                    (float)Margin.Bottom  + (float)Margin.Top  + bounds.Height);
#else
				rect = new Rect(x - (float)Margin.Left,
				y - (float)Margin.Top,
				bounds.Width + (float)Margin.Right + (float)Margin.Left,
				(float)Margin.Bottom / 2 + (float)Margin.Top / 2 + bounds.Height);
#endif
			}

			if (cornerRadius.TopLeft > 0)
			{
				canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
			}
			else
			{
				canvas.FillRectangle(rect);
			}

			if (StrokeWidth > 0)
			{
				if (cornerRadius.TopLeft > 0)
				{
					canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
				}
				else
				{
					canvas.DrawRectangle(rect);
				}
			}
		}

		static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		#endregion
	}
}
