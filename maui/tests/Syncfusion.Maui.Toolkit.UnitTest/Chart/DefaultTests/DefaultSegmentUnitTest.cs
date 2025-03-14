using Syncfusion.Maui.Toolkit.Charts; 

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class DefaultSegmentUnitTest : BaseUnitTest
	{
		[Fact]
		public void FastScatterSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new FastScatterSegment();
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);			
			Assert.False(segment.HasStroke);
		}

		[Fact]
		public void FastScatterSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new FastScatterSegment();
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsEmpty);
		}
	}
}
