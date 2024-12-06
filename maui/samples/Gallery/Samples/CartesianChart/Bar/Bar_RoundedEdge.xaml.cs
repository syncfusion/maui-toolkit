using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class BarChart_RoundedEdge : SampleView
	{
		public BarChart_RoundedEdge()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart2.Handler?.DisconnectHandler();
		}
	}

	public partial class CustomBarChart : ColumnSeries
	{
		protected override ChartSegment CreateSegment()
		{
			return new BarSegmentExt();
		}

		public static readonly BindableProperty TrackColorProperty =
BindableProperty.Create(nameof(TrackColor), typeof(SolidColorBrush), typeof(ColumnSeriesExt), new SolidColorBrush(Color.FromArgb("#E7E0EC")));

		public SolidColorBrush TrackColor
		{
			get { return (SolidColorBrush)GetValue(TrackColorProperty); }
			set { SetValue(TrackColorProperty, value); }
		}
	}

	public partial class BarSegmentExt : ColumnSegment
	{
		RectF _trackRect = RectF.Zero;

		protected override void OnLayout()
		{
			base.OnLayout();
			if (Series is CartesianSeries series && series.ActualYAxis is NumericalAxis yAxis)
			{
				var top = yAxis.ValueToPoint(Convert.ToDouble(yAxis.Maximum ?? double.NaN));
				_trackRect = new RectF() { Left = Left, Top = Top, Right = top, Bottom = Bottom };
			}
		}

		protected override void Draw(ICanvas canvas)
		{
			if (Series is not CustomBarChart series)
			{
				return;
			}

			canvas.SetFillPaint(series.TrackColor, _trackRect);
			canvas.FillRoundedRectangle(_trackRect, 25);

			base.Draw(canvas);
		}
	}

}
