using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartEventUnitTest : BaseUnitTest
    {
        [Fact]
        public void GetActualRange_ShouldReturnCorrectRange_WhenActualValuesAreSet()
        {
            var axis = new NumericalAxis();
            axis.ActualMinimum = 10;
            axis.ActualMaximum = 100;
            var eventElement = new ActualRangeChangedEventArgs(axis.ActualMinimum, axis.ActualMaximum); 

            DoubleRange actualRange = eventElement.GetActualRange();

            Assert.Equal(10, actualRange.Start);
            Assert.Equal(100, actualRange.End);
        }

        [Fact]
        public void GetVisibleRange_ShouldReturnVisibleRange_WhenBothVisibleMinimumAndMaximumAreSet()
        {
            var axis = new NumericalAxis();
            axis.ActualMinimum = 0;
            axis.ActualMaximum = 100;
            var eventElement = new ActualRangeChangedEventArgs(axis.ActualMinimum, axis.ActualMaximum);
            eventElement.VisibleMinimum = 20;
            eventElement.VisibleMaximum = 80;

            DoubleRange visibleRange = eventElement.GetVisibleRange();

            Assert.Equal(20, visibleRange.Start);
            Assert.Equal(80, visibleRange.End);
        }

        [Theory]
        [InlineData("123.45", 123.45)]               
        [InlineData(null, double.NaN)]          
        [InlineData("", double.NaN)]            
        public void ToDouble_ShouldHandleStringInput(string input, double expected)
        {
            var axis = new NumericalAxis();
            var eventElement = new ActualRangeChangedEventArgs(axis.ActualMinimum, axis.ActualMaximum);

            var result = InvokePrivateMethod(eventElement, "ToDouble", input);

            Assert.NotNull(result);
            Assert.Equal(expected, (double)result);
        }

        [Theory]
        [InlineData(2020, 1, 1)]
        [InlineData(1999, 12, 31)]
        public void ToDouble_ShouldConvertDateTimeToTicks(int year, int month, int day)
        {
            var axis = new NumericalAxis();
            var eventElement = new ActualRangeChangedEventArgs(axis.ActualMinimum, axis.ActualMaximum);
            eventElement.Axis = new DateTimeAxis();
            DateTime date = new DateTime(year, month, day);
            double expected = date.Ticks;

            var result = InvokePrivateMethod(eventElement, "ToDouble", date);

            Assert.NotNull(result);
            Assert.Equal(expected, (double)result);
        }
    }
}
