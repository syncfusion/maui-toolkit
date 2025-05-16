using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class BehaviorDefaultTests
	{
		[Fact]
		public void DataPointSelectionBehaviorDefaultTests()
		{
			var behavior = new DataPointSelectionBehavior();

			// DataPointSelectionBehavior
			Assert.Null(behavior.Source);

			// ChartSelectionBehavior
			Assert.NotNull(behavior.ActualSelectedIndexes);
			Assert.Empty(behavior.ActualSelectedIndexes);
			Assert.Equal(ChartSelectionType.Single, behavior.Type);
			Assert.Null(behavior.SelectionBrush);
			Assert.Equal(-1, behavior.SelectedIndex);
			Assert.NotNull(behavior.SelectedIndexes);
			Assert.Empty(behavior.SelectedIndexes);

			// ChartBehavior
			Assert.Equal(PointerDeviceType.Touch, behavior.DeviceType);
		}

		[Fact]
		public void SeriesSelectionBehaviorDefaultTests()
		{
			var behavior = new SeriesSelectionBehavior();

			// SeriesSelectionBehavior specific properties
			Assert.Null(behavior.Chart);
			Assert.Null(behavior.ChartArea);

			// ChartSelectionBehavior properties
			Assert.NotNull(behavior.ActualSelectedIndexes);
			Assert.Empty(behavior.ActualSelectedIndexes);
			Assert.Equal(ChartSelectionType.Single, behavior.Type);
			Assert.Null(behavior.SelectionBrush);
			Assert.Equal(-1, behavior.SelectedIndex);
			Assert.NotNull(behavior.SelectedIndexes);
			Assert.Empty(behavior.SelectedIndexes);

			// ChartBehavior properties
			Assert.Equal(PointerDeviceType.Touch, behavior.DeviceType);
		}

		[Fact]
		public void ChartTooltipBehaviorDefaultTests()
		{
			var behavior = new ChartTooltipBehavior();

			// ChartTooltipBehavior specific properties
			Assert.Null(behavior.Background);
			Assert.Equal(2, behavior.Duration);
			Assert.Null(behavior.TextColor);
			Assert.Equal(new Thickness(0), behavior.Margin);
			Assert.Equal(float.NaN, behavior.FontSize);
			Assert.Null(behavior.FontFamily);
			Assert.Equal(FontAttributes.None, behavior.FontAttributes);

			// Internal properties
			Assert.Null(behavior.Chart);

			// ChartBehavior properties
			Assert.Equal(PointerDeviceType.Touch, behavior.DeviceType);
		}

		[Fact]
		public void ChartTrackballBehaviorDefaultTests_Part1()
		{
			var behavior = new ChartTrackballBehavior();

			// ChartTrackballBehavior specific properties
			Assert.Equal(LabelDisplayMode.FloatAllPoints, behavior.DisplayMode);
			Assert.NotNull(behavior.LineStyle);
			Assert.IsType<ChartLineStyle>(behavior.LineStyle);
			Assert.NotNull(behavior.LabelStyle);
			Assert.IsType<ChartLabelStyle>(behavior.LabelStyle);
			Assert.NotNull(behavior.MarkerSettings);
			Assert.IsType<ChartMarkerSettings>(behavior.MarkerSettings);
			Assert.True(behavior.ShowMarkers);
		}

		[Fact]
		public void ChartTrackballBehaviorDefaultTests_Part2()
		{
			var behavior = new ChartTrackballBehavior();

			Assert.True(behavior.ShowLabel);
			Assert.True(behavior.ShowLine);
#if WINDOWS || MACCATALYST
            Assert.Equal(ChartTrackballActivationMode.TouchMove, behavior.ActivationMode);
#elif ANDROID || IOS
            Assert.Equal(ChartTrackballActivationMode.LongPress, behavior.ActivationMode);
#else
			Assert.Equal(ChartTrackballActivationMode.TouchMove, behavior.ActivationMode);
#endif
			// Internal properties
			Assert.False(behavior.IsPressed);
			Assert.False(behavior.LongPressActive);
			Assert.Null(behavior.CartesianChart);
		}

		[Fact]
		public void ChartTrackballBehaviorDefaultTests_Part3()
		{
			var behavior = new ChartTrackballBehavior();

			Assert.Equal(RectF.Zero, behavior.SeriesBounds);
			Assert.NotNull(behavior.PointInfos);
			Assert.Empty(behavior.PointInfos);
			Assert.NotNull(behavior.PreviousPointInfos);
			Assert.Empty(behavior.PreviousPointInfos);
			Assert.NotNull(behavior.ContentList);
			Assert.Empty(behavior.ContentList);
			Assert.Null(behavior.GroupedLabelView);
		}

		[Fact]
		public void ChartTrackballBehaviorDefaultTests_Part4()
		{
			var behavior = new ChartTrackballBehavior();

			Assert.NotNull(behavior.TrackballBackground);
			//Assert.IsType<SolidColorBrush>(behavior.TrackballBackground);
			Assert.Equal(SolidColorBrush.Black, behavior.TrackballBackground);
			Assert.Equal(14.0, behavior.TrackballFontSize);
			Assert.NotNull(behavior.LineStroke);
			//Assert.IsType<SolidColorBrush>(behavior.LineStroke);
			Assert.Equal(Colors.Black, ((SolidColorBrush)behavior.LineStroke).Color);
			Assert.Equal(1d, behavior.LineStrokeWidth);

			// ChartBehavior properties
			Assert.Equal(PointerDeviceType.Touch, behavior.DeviceType);
		}

		[Fact]
        public void ChartZoomPanBehaviorDefaultTests_Part1()
        {
            var behavior = new ChartZoomPanBehavior();

            // ChartZoomPanBehavior specific properties
            Assert.True(behavior.EnablePinchZooming);
            Assert.True(behavior.EnablePanning);
            Assert.True(behavior.EnableDoubleTap);
            Assert.Equal(ZoomMode.XY, behavior.ZoomMode);
            Assert.False(behavior.EnableSelectionZooming);
			Assert.NotNull(behavior.SelectionRectStroke);
			Assert.IsType<SolidColorBrush>(behavior.SelectionRectStroke);
			Assert.Equal(Color.FromArgb("#6200EE"), ((SolidColorBrush)behavior.SelectionRectStroke).Color);
		}

        [Fact]
        public void ChartZoomPanBehaviorDefaultTests_Part2()
        {
            var behavior = new ChartZoomPanBehavior();

            Assert.Equal(1d, behavior.SelectionRectStrokeWidth);
            Assert.NotNull(behavior.SelectionRectFill);
            Assert.IsType<SolidColorBrush>(behavior.SelectionRectFill);
            Assert.Equal(Color.FromArgb("#146200EE"), ((SolidColorBrush)behavior.SelectionRectFill).Color);
			Assert.NotNull(behavior.SelectionRectStrokeDashArray);
			Assert.Equal(2, behavior.SelectionRectStrokeDashArray.Count);
			Assert.Equal(5, behavior.SelectionRectStrokeDashArray[0]);
			Assert.Equal(5, behavior.SelectionRectStrokeDashArray[1]);
		}

        [Fact]
        public void ChartZoomPanBehaviorDefaultTests_Part3()
        {
            var behavior = new ChartZoomPanBehavior();
            
            Assert.False(behavior.EnableDirectionalZooming);
            Assert.Equal(double.NaN, behavior.MaximumZoomLevel);
			// Internal properties
			Assert.Null(behavior.Chart);
			Assert.False(behavior.IsSelectionZoomingActivated);
			Assert.Equal(Rect.Zero, behavior.SelectionRect);
			// ChartBehavior properties
			Assert.Equal(PointerDeviceType.Touch, behavior.DeviceType);
		}
	}
}
