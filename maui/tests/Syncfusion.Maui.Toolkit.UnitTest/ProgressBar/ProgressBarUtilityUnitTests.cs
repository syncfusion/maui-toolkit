using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.ProgressBar;
using Xunit;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class ProgressBarUtilityUnitTests
	{
		#region UpdateGradientStopCollection Tests

		[Fact]
		public void UpdateGradientStopCollection_SortsStopsByActualValue()
		{
			var stops = new List<ProgressGradientStop>
			{
				new ProgressGradientStop { Color = Colors.Red, ActualValue = 70 },
				new ProgressGradientStop { Color = Colors.Green, ActualValue = 30 },
				new ProgressGradientStop { Color = Colors.Blue, ActualValue = 50 },
			};

			var result = Utility.UpdateGradientStopCollection(stops, 0, 100);

			Assert.True(result[0].ActualValue <= result[1].ActualValue);
			Assert.True(result[1].ActualValue <= result[2].ActualValue);
		}

		[Fact]
		public void UpdateGradientStopCollection_InsertsStopAtStart_WhenFirstExceedsRangeStart()
		{
			var stops = new List<ProgressGradientStop>
			{
				new ProgressGradientStop { Color = Colors.Red, ActualValue = 30 },
				new ProgressGradientStop { Color = Colors.Blue, ActualValue = 70 },
			};

			var result = Utility.UpdateGradientStopCollection(stops, 0, 100);

			// Both start and end are inserted since 30 > 0 and 70 < 100
			Assert.Equal(4, result.Count);
			Assert.Equal(0, result[0].ActualValue);
			Assert.Equal(Colors.Red, result[0].Color);
		}

		[Fact]
		public void UpdateGradientStopCollection_AppendsStopAtEnd_WhenLastBelowRangeEnd()
		{
			var stops = new List<ProgressGradientStop>
			{
				new ProgressGradientStop { Color = Colors.Red, ActualValue = 30 },
				new ProgressGradientStop { Color = Colors.Blue, ActualValue = 70 },
			};

			var result = Utility.UpdateGradientStopCollection(stops, 0, 100);

			// Both start and end are inserted since 30 > 0 and 70 < 100
			Assert.Equal(4, result.Count);
			Assert.Equal(100, result[result.Count - 1].ActualValue);
			Assert.Equal(Colors.Blue, result[result.Count - 1].Color);
		}

		[Fact]
		public void UpdateGradientStopCollection_DoesNotInsert_WhenStopsAlreadyCoverRange()
		{
			var stops = new List<ProgressGradientStop>
			{
				new ProgressGradientStop { Color = Colors.Red, ActualValue = 0 },
				new ProgressGradientStop { Color = Colors.Blue, ActualValue = 100 },
			};

			var result = Utility.UpdateGradientStopCollection(stops, 0, 100);

			Assert.Equal(2, result.Count);
		}

		[Fact]
		public void UpdateGradientStopCollection_InsertsAtBothEnds_WhenStopsDoNotCoverRange()
		{
			var stops = new List<ProgressGradientStop>
			{
				new ProgressGradientStop { Color = Colors.Green, ActualValue = 50 },
			};

			var result = Utility.UpdateGradientStopCollection(stops, 0, 100);

			Assert.Equal(3, result.Count);
			Assert.Equal(0, result[0].ActualValue);
			Assert.Equal(100, result[result.Count - 1].ActualValue);
		}

		#endregion
	}
}
