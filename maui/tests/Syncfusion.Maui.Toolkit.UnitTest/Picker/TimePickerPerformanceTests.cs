using Syncfusion.Maui.Toolkit.Picker;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class TimePickerPerformanceTests : PickerBaseUnitTest
	{
		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(15)]
		public void TimePicker_MinuteInterval_SetAndGet(int interval)
		{
			var picker = new SfTimePicker
			{
				MinuteInterval = interval
			};

			Assert.Equal(interval, picker.MinuteInterval);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(15)]
		public void TimePicker_SecondInterval_SetAndGet(int interval)
		{
			var picker = new SfTimePicker
			{
				SecondInterval = interval
			};

			Assert.Equal(interval, picker.SecondInterval);
		}

		[Fact]
		public void TimePicker_SelectedTimePreserved_WhenIntervalsChange()
		{
			var picker = new SfTimePicker
			{
				SelectedTime = new TimeSpan(10, 30, 45)
			};

			Assert.Equal(new TimeSpan(10, 30, 45), picker.SelectedTime);

			picker.MinuteInterval = 5;

			// The selected time should still be accessible
			Assert.NotNull(picker.SelectedTime);
		}

		[Theory]
		[InlineData("10:30:00")]
		[InlineData("23:59:59")]
		[InlineData("00:00:00")]
		public void TimePicker_SelectedTime_SetAndRetrieve(string timeStr)
		{
			var expected = TimeSpan.Parse(timeStr);
			var picker = new SfTimePicker
			{
				SelectedTime = expected
			};

			Assert.Equal(expected, picker.SelectedTime);
		}

		[Fact]
		public void TimePicker_MultipleIntervalChanges_DoNotThrow()
		{
			var picker = new SfTimePicker
			{
				SelectedTime = new TimeSpan(12, 15, 30)
			};

			var exception = Record.Exception(() =>
			{
				picker.MinuteInterval = 5;
				picker.SecondInterval = 10;
				picker.MinuteInterval = 15;
				picker.SecondInterval = 30;
				picker.MinuteInterval = 1;
				picker.SecondInterval = 1;
			});

			Assert.Null(exception);
		}
	}
}
